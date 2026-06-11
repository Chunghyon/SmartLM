using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;

namespace FaceDeviceHttpPcServer.Services;

/// <summary>
/// 로컬 네트워크에서 Face Recognition 디바이스를 자동으로 검색하는 서비스
/// </summary>
public sealed class DeviceDiscoveryService
{
    private const int DiscoveryPort = 20567; // 표준 디스커버리 포트 (UDP 브로드캐스트)
    private const int TimeoutMs = 5000; // 5초로 증가 - 디바이스 응답 대기 시간

    // UDP 브로드캐스트 프로토콜 매직 넘버
    private const uint DEVDISCOVER_REQUEST_MAGIC1 = 0x0c58380d;
    private const uint DEVDISCOVER_REQUEST_MAGIC2 = 0xea8b42b2;
    private const uint DEVDISCOVER_RESPONSE_MAGIC1 = 0xaa8fcb84;
    private const uint DEVDISCOVER_RESPONSE_MAGIC2 = 0x05fece87;
    private const int ProductNameLen_Max = 16;

    public record DiscoveredDevice(
        string IpAddress,
        string DeviceSN,
        string DeviceName,
        string Model,
        string FirmwareVersion,
        int HttpPort);

    /// <summary>
    /// 로컬 네트워크에서 디바이스를 검색합니다 (UDP 브로드캐스트)
    /// </summary>
    public async Task<List<DiscoveredDevice>> SearchDevicesAsync(CancellationToken cancellationToken = default)
    {
        var devices = new List<DiscoveredDevice>();

        try
        {
            using var udpClient = new UdpClient();
            udpClient.EnableBroadcast = true;
            udpClient.Client.ReceiveTimeout = TimeoutMs;

            // 브로드캐스트 메시지 준비 (바이너리 프로토콜)
            var data = CreateDiscoveryRequest(""); // 빈 prefix는 모든 디바이스 검색

            // 브로드캐스트 전송
            var broadcastEndpoint = new IPEndPoint(IPAddress.Broadcast, DiscoveryPort);
            await udpClient.SendAsync(data, data.Length, broadcastEndpoint);

            LogHub.Instance.Info($"브로드캐스트 전송: 포트 {DiscoveryPort}, {data.Length} bytes");

            // 응답 수신 (3초 동안)
            var endTime = DateTime.Now.AddMilliseconds(TimeoutMs);

            while (DateTime.Now < endTime && !cancellationToken.IsCancellationRequested)
            {
                try
                {
                    var receiveTask = udpClient.ReceiveAsync();
                    var completedTask = await Task.WhenAny(receiveTask, Task.Delay(500, cancellationToken));

                    if (completedTask == receiveTask)
                    {
                        var result = await receiveTask;

                        LogHub.Instance.Info($"UDP 응답 수신: {result.RemoteEndPoint}, {result.Buffer.Length} bytes");

                        try
                        {
                            var device = ParseUdpDiscoveryResponse(result.RemoteEndPoint.Address.ToString(), result.Buffer);
                            if (device != null && !devices.Any(d => d.DeviceSN == device.DeviceSN))
                            {
                                devices.Add(device);
                                LogHub.Instance.Info($"디바이스 발견: {device.IpAddress} - {device.DeviceSN}");
                            }
                        }
                        catch (Exception ex)
                        {
                            LogHub.Instance.Warn($"응답 파싱 실패: {ex.Message}");
                        }
                    }
                }
                catch (SocketException)
                {
                    // 타임아웃 또는 기타 소켓 오류 - 계속 진행
                }
            }
        }
        catch (Exception ex)
        {
            LogHub.Instance.Error($"디바이스 검색 오류: {ex.Message}");
        }

        LogHub.Instance.Info($"브로드캐스트 검색 완료: {devices.Count}개 디바이스 발견");
        return devices;
    }

    /// <summary>
    /// HTTP를 통한 네트워크 스캔 (IP 범위 스캔) - 실시간 결과 반환
    /// </summary>
    public async IAsyncEnumerable<DiscoveredDevice> ScanNetworkStreamAsync(string subnet, CancellationToken cancellationToken = default)
    {
        // 예: "192.168.0" -> 192.168.0.1-254 스캔
        var ipParts = subnet.Split('.');
        if (ipParts.Length != 3)
        {
            LogHub.Instance.Warn("잘못된 서브넷 형식. 예: 192.168.0");
            yield break;
        }

        LogHub.Instance.Info($"네트워크 스캔 시작: {subnet}.1-254");

        var tasks = new List<Task<DiscoveredDevice?>>();
        var deviceCount = 0;

        for (int i = 1; i <= 254; i++)
        {
            if (cancellationToken.IsCancellationRequested) break;

            var ip = $"{subnet}.{i}";
            tasks.Add(ProbeDeviceAsync(ip, cancellationToken));

            // 동시 연결 수 제한 (20개씩) 및 완료된 작업 확인
            if (tasks.Count >= 20)
            {
                while (tasks.Any())
                {
                    var completed = await Task.WhenAny(tasks);
                    tasks.Remove(completed);

                    var device = await completed;
                    if (device != null)
                    {
                        deviceCount++;
                        LogHub.Instance.Info($"디바이스 발견 #{deviceCount}: {device.IpAddress} - {device.DeviceSN}");
                        yield return device;
                    }
                }
            }
        }

        // 남은 작업 완료
        while (tasks.Any())
        {
            var completed = await Task.WhenAny(tasks);
            tasks.Remove(completed);

            var device = await completed;
            if (device != null)
            {
                deviceCount++;
                LogHub.Instance.Info($"디바이스 발견 #{deviceCount}: {device.IpAddress} - {device.DeviceSN}");
                yield return device;
            }
        }

        LogHub.Instance.Info($"네트워크 스캔 완료: {deviceCount}개 디바이스 발견");
    }

    /// <summary>
    /// HTTP를 통한 네트워크 스캔 (IP 범위 스캔) - 전체 결과 반환 (하위 호환성)
    /// </summary>
    public async Task<List<DiscoveredDevice>> ScanNetworkAsync(string subnet, CancellationToken cancellationToken = default)
    {
        var devices = new List<DiscoveredDevice>();
        await foreach (var device in ScanNetworkStreamAsync(subnet, cancellationToken))
        {
            devices.Add(device);
        }
        return devices;
    }

    /// <summary>
    /// 특정 IP의 디바이스 정보 조회 시도
    /// </summary>
    private async Task<DiscoveredDevice?> ProbeDeviceAsync(string ip, CancellationToken cancellationToken)
    {
        try
        {
            using var httpClient = new HttpClient { Timeout = TimeSpan.FromSeconds(2) };

            // 일반적인 포트들 시도
            foreach (var port in new[] { 80, 8080, 8100 })
            {
                try
                {
                    var url = $"http://{ip}:{port}/api/GetDeviceSN";
                    var response = await httpClient.GetAsync(url, cancellationToken);

                    if (response.IsSuccessStatusCode)
                    {
                        var json = await response.Content.ReadAsStringAsync(cancellationToken);
                        var doc = JsonDocument.Parse(json);

                        if (doc.RootElement.TryGetProperty("result", out var result) && result.GetBoolean())
                        {
                            var sn = doc.RootElement.GetProperty("content").GetString() ?? "UNKNOWN";

                            // 추가 정보 조회 시도
                            var detailUrl = $"http://{ip}:{port}/api/Device/GetDetail";
                            var detailResponse = await httpClient.GetAsync(detailUrl, cancellationToken);

                            string deviceName = "Face Device";
                            string model = "Unknown";
                            string firmware = "Unknown";

                            if (detailResponse.IsSuccessStatusCode)
                            {
                                var detailJson = await detailResponse.Content.ReadAsStringAsync(cancellationToken);
                                var detailDoc = JsonDocument.Parse(detailJson);

                                if (detailDoc.RootElement.TryGetProperty("content", out var content))
                                {
                                    if (content.TryGetProperty("DeviceName", out var nameEl))
                                        deviceName = nameEl.GetString() ?? deviceName;
                                    if (content.TryGetProperty("Model", out var modelEl))
                                        model = modelEl.GetString() ?? model;
                                    if (content.TryGetProperty("FirmwareVersion", out var fwEl))
                                        firmware = fwEl.GetString() ?? firmware;
                                }
                            }

                            LogHub.Instance.Info($"디바이스 발견: {ip}:{port} - {sn}");

                            return new DiscoveredDevice(ip, sn, deviceName, model, firmware, port);
                        }
                    }
                }
                catch
                {
                    // 해당 포트에서 응답 없음 - 다음 포트 시도
                }
            }
        }
        catch
        {
            // IP에 디바이스 없음
        }

        return null;
    }

    private DiscoveredDevice? ParseDeviceResponse(string ipAddress, JsonDocument response)
    {
        try
        {
            var root = response.RootElement;

            var sn = root.GetProperty("DeviceSN").GetString() ?? "UNKNOWN";
            var name = root.TryGetProperty("DeviceName", out var nameEl) ? nameEl.GetString() ?? "Face Device" : "Face Device";
            var model = root.TryGetProperty("Model", out var modelEl) ? modelEl.GetString() ?? "Unknown" : "Unknown";
            var firmware = root.TryGetProperty("FirmwareVersion", out var fwEl) ? fwEl.GetString() ?? "Unknown" : "Unknown";
            var port = root.TryGetProperty("HttpPort", out var portEl) ? portEl.GetInt32() : 80;

            return new DiscoveredDevice(ipAddress, sn, name, model, firmware, port);
        }
        catch
        {
            return null;
        }
    }

    /// <summary>
    /// UDP 브로드캐스트 디스커버리 요청 패킷 생성
    /// </summary>
    private byte[] CreateDiscoveryRequest(string productNamePrefix)
    {
        using var stream = new MemoryStream();
        using var writer = new BinaryWriter(stream);

        writer.Write(DEVDISCOVER_REQUEST_MAGIC1);
        writer.Write(DEVDISCOVER_REQUEST_MAGIC2);

        // ProductNamePrefix (16 bytes, null-terminated UTF-8)
        var prefixBytes = Encoding.UTF8.GetBytes(productNamePrefix);
        var prefixBuffer = new byte[ProductNameLen_Max];
        Array.Copy(prefixBytes, prefixBuffer, Math.Min(prefixBytes.Length, ProductNameLen_Max - 1));
        writer.Write(prefixBuffer);

        // Reserved (2 * 4 bytes)
        writer.Write((uint)0);
        writer.Write((uint)0);

        return stream.ToArray();
    }

    /// <summary>
    /// UDP 브로드캐스트 디스커버리 응답 파싱
    /// </summary>
    private DiscoveredDevice? ParseUdpDiscoveryResponse(string ipAddress, byte[] data)
    {
        try
        {
            if (data.Length < 32)
                return null;

            using var stream = new MemoryStream(data);
            using var reader = new BinaryReader(stream);

            var magic1 = reader.ReadUInt32();
            var magic2 = reader.ReadUInt32();

            // 매직 넘버 확인
            if (magic1 != DEVDISCOVER_RESPONSE_MAGIC1 || magic2 != DEVDISCOVER_RESPONSE_MAGIC2)
            {
                LogHub.Instance.Warn($"잘못된 매직 넘버: 0x{magic1:X8}, 0x{magic2:X8}");
                return null;
            }

            // ProductName (16 bytes)
            var productNameBytes = reader.ReadBytes(ProductNameLen_Max);
            var productName = Encoding.UTF8.GetString(productNameBytes).TrimEnd('\0');

            var deviceId = reader.ReadUInt32();
            var dwIp = reader.ReadUInt32();
            var dwSubnetMask = reader.ReadUInt32();
            var dwDefaultGateway = reader.ReadUInt32();
            var port = reader.ReadUInt16();
            var useDhcp = reader.ReadUInt16();

            // IP 주소 변환 (빅 엔디안)
            var detectedIp = $"{(dwIp >> 24) & 0xFF}.{(dwIp >> 16) & 0xFF}.{(dwIp >> 8) & 0xFF}.{dwIp & 0xFF}";

            // HTTP 포트는 기본값 80 사용 (나중에 HTTP로 세부 정보 확인)
            return new DiscoveredDevice(
                detectedIp,
                deviceId.ToString(),
                productName,
                productName,
                "Unknown",
                (int)port);
        }
        catch (Exception ex)
        {
            LogHub.Instance.Warn($"UDP 응답 파싱 실패: {ex.Message}");
            return null;
        }
    }

    /// <summary>
    /// HTTP 포트 확인 (80, 8080, 8100 등)
    /// </summary>
    private async Task<int> ProbeHttpPortAsync(string ip)
    {
        foreach (var port in new[] { 80, 8080, 8100 })
        {
            try
            {
                using var httpClient = new HttpClient { Timeout = TimeSpan.FromSeconds(1) };
                var response = await httpClient.GetAsync($"http://{ip}:{port}/api/heartBeat");
                if (response.IsSuccessStatusCode)
                    return port;
            }
            catch
            {
                // 포트 확인 실패 - 다음 포트 시도
            }
        }
        return 80; // 기본값
    }
}
