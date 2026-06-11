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
    private const int DiscoveryPort = 60000; // 표준 디스커버리 포트
    private const int TimeoutMs = 3000;

    public record DiscoveredDevice(
        string IpAddress,
        string DeviceSN,
        string DeviceName,
        string Model,
        string FirmwareVersion,
        int HttpPort);

    /// <summary>
    /// 로컬 네트워크에서 디바이스를 검색합니다
    /// </summary>
    public async Task<List<DiscoveredDevice>> SearchDevicesAsync(CancellationToken cancellationToken = default)
    {
        var devices = new List<DiscoveredDevice>();

        try
        {
            using var udpClient = new UdpClient();
            udpClient.EnableBroadcast = true;
            udpClient.Client.ReceiveTimeout = TimeoutMs;

            // 브로드캐스트 메시지 준비
            var discoveryMessage = new
            {
                Type = "Discovery",
                Message = "FaceDevice_Search",
                Timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds()
            };

            var jsonMessage = JsonSerializer.Serialize(discoveryMessage);
            var data = Encoding.UTF8.GetBytes(jsonMessage);

            // 브로드캐스트 전송
            var broadcastEndpoint = new IPEndPoint(IPAddress.Broadcast, DiscoveryPort);
            await udpClient.SendAsync(data, data.Length, broadcastEndpoint);

            LogHub.Instance.Info($"브로드캐스트 전송: {jsonMessage}");

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
                        var responseJson = Encoding.UTF8.GetString(result.Buffer);

                        LogHub.Instance.Info($"응답 수신: {result.RemoteEndPoint} - {responseJson}");

                        try
                        {
                            var response = JsonSerializer.Deserialize<JsonDocument>(responseJson);
                            if (response != null)
                            {
                                var device = ParseDeviceResponse(result.RemoteEndPoint.Address.ToString(), response);
                                if (device != null && !devices.Any(d => d.DeviceSN == device.DeviceSN))
                                {
                                    devices.Add(device);
                                }
                            }
                        }
                        catch (JsonException ex)
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

        return devices;
    }

    /// <summary>
    /// HTTP를 통한 네트워크 스캔 (IP 범위 스캔)
    /// </summary>
    public async Task<List<DiscoveredDevice>> ScanNetworkAsync(string subnet, CancellationToken cancellationToken = default)
    {
        var devices = new List<DiscoveredDevice>();

        // 예: "192.168.0" -> 192.168.0.1-254 스캔
        var ipParts = subnet.Split('.');
        if (ipParts.Length != 3)
        {
            LogHub.Instance.Warn("잘못된 서브넷 형식. 예: 192.168.0");
            return devices;
        }

        LogHub.Instance.Info($"네트워크 스캔 시작: {subnet}.1-254");

        var tasks = new List<Task<DiscoveredDevice?>>();

        for (int i = 1; i <= 254; i++)
        {
            if (cancellationToken.IsCancellationRequested) break;

            var ip = $"{subnet}.{i}";
            tasks.Add(ProbeDeviceAsync(ip, cancellationToken));

            // 동시 연결 수 제한 (20개씩)
            if (tasks.Count >= 20)
            {
                var completed = await Task.WhenAll(tasks);
                foreach (var device in completed.Where(d => d != null))
                {
                    devices.Add(device!);
                }
                tasks.Clear();
            }
        }

        // 남은 작업 완료
        if (tasks.Any())
        {
            var completed = await Task.WhenAll(tasks);
            foreach (var device in completed.Where(d => d != null))
            {
                devices.Add(device!);
            }
        }

        LogHub.Instance.Info($"네트워크 스캔 완료: {devices.Count}개 디바이스 발견");

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
}
