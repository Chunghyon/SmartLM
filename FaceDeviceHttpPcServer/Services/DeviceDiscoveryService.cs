using System.Net;
using System.Net.Sockets;
using System.Net.NetworkInformation;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;

namespace FaceDeviceHttpPcServer.Services;

/// <summary>
/// 로컬 네트워크에서 Face Recognition 디바이스를 자동으로 검색하는 서비스
/// </summary>
public sealed class DeviceDiscoveryService
{
    private const int DiscoveryPort = 8101; // Face Device Discovery 포트 (ACS 및 단말 기본값)
    private const int TimeoutMs = 30000; // 30초 - 디바이스 응답 대기 시간

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
        string FirmwareVersion);

    /// <summary>
    /// 로컬 네트워크에서 디바이스를 검색합니다 (UDP 브로드캐스트) - ACS 방식
    /// </summary>
    /// <param name="localIpAddress">사용할 로컬 IP 주소. null이면 첫 번째 유효한 인터페이스 사용</param>
    /// <param name="discoveryPort">UDP 검색 포트 (기본값: 8101)</param>
    public async Task<List<DiscoveredDevice>> SearchDevicesAsync(string? localIpAddress = null, int discoveryPort = 8101, CancellationToken cancellationToken = default)
    {
        var devices = new List<DiscoveredDevice>();
        var startTime = DateTime.Now;

        UdpClient? binaryClient = null;
        UdpClient? jsonClient = null;

        try
        {
            // 로컬 네트워크 인터페이스 가져오기
            var networkInterfaces = GetValidNetworkInterfaces();

            if (networkInterfaces.Count == 0)
            {
                LogHub.Instance.Error("브로드캐스트 전송 실패: 유효한 네트워크 인터페이스가 없습니다.");
                return devices;
            }

            // 사용할 인터페이스 선택
            (IPAddress localIp, IPAddress broadcastIp) selectedInterface;

            if (!string.IsNullOrWhiteSpace(localIpAddress))
            {
                // 지정된 IP 찾기
                var matchedInterface = networkInterfaces.FirstOrDefault(ni => ni.localIp.ToString() == localIpAddress);
                if (matchedInterface.localIp == null)
                {
                    LogHub.Instance.Error($"지정된 IP를 찾을 수 없습니다: {localIpAddress}");
                    LogHub.Instance.Info($"사용 가능한 IP: {string.Join(", ", networkInterfaces.Select(ni => ni.localIp.ToString()))}");
                    return devices;
                }
                selectedInterface = matchedInterface;
            }
            else
            {
                // 첫 번째 인터페이스 사용
                selectedInterface = networkInterfaces[0];
            }

            var (localIp, broadcastIp) = selectedInterface;

            // ACS와 동일하게 255.255.255.255 전체 브로드캐스트 사용
            broadcastIp = IPAddress.Broadcast; // 255.255.255.255
            LogHub.Instance.Info($"UDP 검색 시작: {localIp} → 브로드캐스트 {broadcastIp}:{discoveryPort}");

            // ACS 방식 프로토콜 준비
            var binaryRequest = CreateDiscoveryRequest_ACS(); // 36 bytes
            var jsonRequest = Encoding.UTF8.GetBytes(@"{""cmd"":""UDPSerach"",""Ver"":1}" + "\0"); // 28 bytes

            // 브로드캐스트 주소 - ACS와 동일하게 255.255.255.255 사용
            var broadcastEndpoint = new IPEndPoint(broadcastIp, discoveryPort);

            // ?? ACS 방식: 서로 다른 포트 2개 사용 (자동 할당)

            // 1?? 바이너리 프로토콜 전송 (자동 포트 할당)
            binaryClient = new UdpClient(new IPEndPoint(localIp, 0));
            binaryClient.EnableBroadcast = true;
            binaryClient.Client.Blocking = false;
            // SO_REUSEADDR 설정
            binaryClient.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
            // SO_BROADCAST 명시적 설정
            binaryClient.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.Broadcast, true);
            // 수신 버퍼 크기 증가
            binaryClient.Client.ReceiveBufferSize = 65536;
            // 송신 버퍼 크기도 증가
            binaryClient.Client.SendBufferSize = 65536;

            var binaryPort = ((IPEndPoint)binaryClient.Client.LocalEndPoint!).Port;

            // 전송 결과 확인
            try
            {
                int bytesSent = await binaryClient.SendAsync(binaryRequest, binaryRequest.Length, broadcastEndpoint);
                if (bytesSent != binaryRequest.Length)
                {
                    LogHub.Instance.Warn($"바이너리 패킷 부분 전송: {bytesSent}/{binaryRequest.Length} bytes");
                }
            }
            catch (Exception ex)
            {
                LogHub.Instance.Error($"바이너리 전송 실패: {ex.Message}");
                throw;
            }

            // 약간의 지연 (디바이스 처리 시간)
            await Task.Delay(50);

            // 2?? JSON 프로토콜 전송 (자동 포트 할당)
            jsonClient = new UdpClient(new IPEndPoint(localIp, 0));
            jsonClient.EnableBroadcast = true;
            jsonClient.Client.Blocking = false;
            // SO_REUSEADDR 설정
            jsonClient.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
            // 수신 버퍼 크기 증가
            jsonClient.Client.ReceiveBufferSize = 65536;

            var jsonPort = ((IPEndPoint)jsonClient.Client.LocalEndPoint!).Port;
            await jsonClient.SendAsync(jsonRequest, jsonRequest.Length, broadcastEndpoint);

            LogHub.Instance.Info($"브로드캐스트 전송 완료 (포트: {binaryPort}, {jsonPort}), 대기 시간: {TimeoutMs}ms");

            // ?? UDP 패킷 스니퍼 (필요 시에만 활성화)
            UdpPacketSniffer? sniffer = null;
            bool enableSniffer = false; // 디버깅이 필요할 때만 true로 변경

            if (enableSniffer)
            {
                try
                {
                    sniffer = new UdpPacketSniffer(localIp);
                    sniffer.PacketReceived += (src, dst, data) =>
                    {
                        // 포트 8101 또는 우리의 수신 포트와 관련된 패킷만 로깅
                        if (src.Port == 8101 || dst.Port == 8101 || 
                            dst.Port == binaryPort || dst.Port == jsonPort)
                        {
                            LogHub.Instance.Info($"UDP 캡처: {src} → {dst}, {data.Length} bytes");
                        }
                    };
                    sniffer.Start();
                    LogHub.Instance.Info("?? UDP 패킷 스니퍼 활성화됨 (진단 모드)");
                }
                catch
                {
                    LogHub.Instance.Info("?? UDP 패킷 스니퍼 비활성화됨 (관리자 권한 필요)");
                }
            }

            // 3?? 응답 수신 및 주기적 재전송 (ACS 방식: 5초마다 재전송)
            var endTime = DateTime.Now.AddMilliseconds(TimeoutMs);
            var lastLogTime = DateTime.Now;
            var lastResponseTime = DateTime.Now;
            var lastSendTime = DateTime.Now; // 마지막 전송 시간 추적

            LogHub.Instance.Info($"응답 대기 시작: {TimeoutMs}ms 동안 수신 대기 + 5초마다 재전송...");

            int sendCount = 1; // 이미 1회 전송했음

            while (DateTime.Now < endTime && !cancellationToken.IsCancellationRequested)
            {
                // ?? ACS 방식: 5초마다 재전송
                if ((DateTime.Now - lastSendTime).TotalSeconds >= 5)
                {
                    sendCount++;
                    // 재전송 로그는 3회까지만 출력
                    if (sendCount <= 3)
                    {
                        LogHub.Instance.Info($"재전송 #{sendCount}");
                    }

                    try
                    {
                        // 바이너리 재전송
                        await binaryClient.SendAsync(binaryRequest, binaryRequest.Length, broadcastEndpoint);
                        await Task.Delay(50); // 약간의 지연

                        // JSON 재전송
                        await jsonClient.SendAsync(jsonRequest, jsonRequest.Length, broadcastEndpoint);

                        lastSendTime = DateTime.Now;
                    }
                    catch (Exception ex)
                    {
                        LogHub.Instance.Error($"재전송 실패: {ex.Message}");
                    }
                }

                // 5초마다 진행 상황 로깅
                if ((DateTime.Now - lastLogTime).TotalSeconds >= 5)
                {
                    var elapsed = (DateTime.Now - startTime).TotalSeconds;
                    var remaining = (endTime - DateTime.Now).TotalSeconds;
                    LogHub.Instance.Info($"대기 중... 경과: {elapsed:F1}초, 남은 시간: {remaining:F1}초, 발견된 디바이스: {devices.Count}개, 전송 횟수: {sendCount}회");
                    lastLogTime = DateTime.Now;
                }

                bool receivedData = false;

                try
                {
                    // 바이너리 클라이언트에서 수신 확인
                    if (binaryClient.Available > 0)
                    {
                        var result = await binaryClient.ReceiveAsync();
                        ProcessDiscoveryResponse(result, devices, startTime);
                        lastResponseTime = DateTime.Now;
                        receivedData = true;
                    }

                    // JSON 클라이언트에서 수신 확인
                    if (jsonClient.Available > 0)
                    {
                        var result = await jsonClient.ReceiveAsync();
                        ProcessDiscoveryResponse(result, devices, startTime);
                        lastResponseTime = DateTime.Now;
                        receivedData = true;
                    }

                    // 데이터가 없으면 짧은 대기
                    if (!receivedData)
                    {
                        await Task.Delay(100, cancellationToken);
                    }

                    // 디바이스 발견 시에도 검색 계속 (복수의 디바이스가 있을 수 있음)
                    // 조기 종료 로직 제거 - 타임아웃까지 계속 검색
                }
                catch (SocketException ex) when (ex.SocketErrorCode == SocketError.WouldBlock)
                {
                    await Task.Delay(100, cancellationToken);
                }
                catch (Exception ex)
                {
                    LogHub.Instance.Warn($"수신 오류: {ex.Message}");
                    await Task.Delay(100, cancellationToken);
                }
            }

            var elapsedSeconds = (DateTime.Now - startTime).TotalSeconds;
            LogHub.Instance.Info($"브로드캐스트 검색 완료: {devices.Count}개 디바이스 발견 (소요 시간: {elapsedSeconds:F1}초, 총 전송: {sendCount}회)");

            // 스니퍼 정리
            sniffer?.Dispose();
        }
        catch (Exception ex)
        {
            LogHub.Instance.Error($"디바이스 검색 오류: {ex.Message}");
            LogHub.Instance.Error($"스택 트레이스: {ex.StackTrace}");
        }
        finally
        {
            binaryClient?.Close();
            jsonClient?.Close();
        }

        return devices;
    }

    /// <summary>
    /// HTTP를 통한 네트워크 스캔 (IP 범위 스캔) - 실시간 결과 반환
    /// Progress와 Device 정보를 모두 yield
    /// </summary>
    public async IAsyncEnumerable<object> ScanNetworkStreamAsync(string subnet, [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        // 예: "192.168.0" -> 192.168.0.1-254 스캔
        var ipParts = subnet.Split('.');
        if (ipParts.Length != 3)
        {
            LogHub.Instance.Warn("잘못된 서브넷 형식. 예: 192.168.0");
            yield break;
        }

        LogHub.Instance.Info($"네트워크 스캔 시작: {subnet}.1-254");

        var tasks = new List<Task<(int ipIndex, DiscoveredDevice? device)>>();
        var deviceCount = 0;
        var scannedCount = 0;
        const int maxConcurrent = 50;

        for (int i = 1; i <= 254; i++)
        {
            if (cancellationToken.IsCancellationRequested) break;

            var ip = $"{subnet}.{i}";
            var index = i;
            tasks.Add(Task.Run(async () => (index, await ProbeDeviceAsync(ip, cancellationToken)), cancellationToken));

            // 동시 연결 수 제한 (50개씩) - 배치가 가득 차거나 마지막 IP일 때만 대기
            if (tasks.Count >= maxConcurrent || i == 254)
            {
                // 한 번에 하나씩만 처리하지 않고, 완료된 것들을 먼저 수집
                var completed = await Task.WhenAny(tasks);
                tasks.Remove(completed);

                var (ipIndex, device) = await completed;
                scannedCount++;

                // 진행 상황 전송
                yield return new { type = "progress", scanned = scannedCount, total = 254 };

                if (device != null)
                {
                    deviceCount++;
                    LogHub.Instance.Info($"디바이스 발견 #{deviceCount}: {device.IpAddress} - {device.DeviceSN}");
                    yield return new { type = "device", device };
                }
            }
        }

        // 남은 작업 완료
        while (tasks.Any())
        {
            var completed = await Task.WhenAny(tasks);
            tasks.Remove(completed);

            var (ipIndex, device) = await completed;
            scannedCount++;

            // 진행 상황 전송
            yield return new { type = "progress", scanned = scannedCount, total = 254 };

            if (device != null)
            {
                deviceCount++;
                LogHub.Instance.Info($"디바이스 발견 #{deviceCount}: {device.IpAddress} - {device.DeviceSN}");
                yield return new { type = "device", device };
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
        await foreach (var item in ScanNetworkStreamAsync(subnet, cancellationToken))
        {
            // 동적 객체에서 type 속성 확인
            var itemType = item.GetType();
            var typeProperty = itemType.GetProperty("type");
            if (typeProperty != null)
            {
                var type = typeProperty.GetValue(item) as string;
                if (type == "device")
                {
                    var deviceProperty = itemType.GetProperty("device");
                    if (deviceProperty != null)
                    {
                        var device = deviceProperty.GetValue(item) as DiscoveredDevice;
                        if (device != null)
                        {
                            devices.Add(device);
                        }
                    }
                }
            }
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
            using var httpClient = new HttpClient { Timeout = TimeSpan.FromMilliseconds(800) };

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

                            return new DiscoveredDevice(ip, sn, deviceName, model, firmware);
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

            return new DiscoveredDevice(ipAddress, sn, name, model, firmware);
        }
        catch
        {
            return null;
        }
    }

    /// <summary>
    /// UDP 브로드캐스트 디스커버리 요청 패킷 생성
    /// </summary>
    /// <summary>
    /// Access Control System 방식 바이너리 Discovery 요청 생성 (36 bytes)
    /// ACS 실제 패킷과 100% 동일한 바이트 시퀀스 사용
    /// </summary>
    private byte[] CreateDiscoveryRequest_ACS()
    {
        // ACS 실제 캡처 패킷의 UDP 페이로드를 그대로 복사
        // 7e30303030303030303030303030303030ffffffffbfbfaabb01fe0000000002b88b237e
        return new byte[]
        {
            0x7e,                                           // Start
            0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, // "00000000"
            0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, // "00000000"
            0xff, 0xff, 0xff, 0xff,                         // Fixed
            0xbf, 0xbf, 0xaa, 0xbb,                         // Magic
            0x01,                                           // Command
            0xfe,                                           // Sub-command
            0x00, 0x00, 0x00, 0x00,                         // Reserved
            0x02, 0xb8, 0x8b, 0x23,                         // Checksum (ACS 실제 값)
            0x7e                                            // End
        };
    }

    /// <summary>
    /// Discovery 응답 처리 헬퍼
    /// </summary>
    private void ProcessDiscoveryResponse(UdpReceiveResult result, List<DiscoveredDevice> devices, DateTime startTime)
    {
        var remoteIp = result.RemoteEndPoint.Address.ToString();

        LogHub.Instance.Info($"UDP 응답 수신: {result.RemoteEndPoint}, {result.Buffer.Length} bytes");

        try
        {
            var device = ParseUdpDiscoveryResponse(remoteIp, result.Buffer);
            if (device != null)
            {
                if (!devices.Any(d => d.DeviceSN == device.DeviceSN))
                {
                    devices.Add(device);
                    LogHub.Instance.Info($"? 디바이스 발견: {device.IpAddress} - {device.DeviceSN} ({device.DeviceName})");
                }
                else
                {
                    LogHub.Instance.Info($"중복 응답 무시: {device.DeviceSN}");
                }
            }
            else
            {
                LogHub.Instance.Warn($"응답 파싱 실패");
            }
        }
        catch (Exception ex)
        {
            LogHub.Instance.Warn($"응답 파싱 오류: {ex.Message}");
        }
    }

    /// <summary>
    /// UDP 브로드캐스트 디스커버리 요청 패킷 생성 (구버전 - 사용 안 함)
    /// </summary>
    private byte[] CreateDiscoveryRequest(string productNamePrefix)
    {
        // Access Control System 방식의 바이너리 프로토콜
        var packet = new List<byte>();

        // 매직 바이트
        packet.Add(0x7e); // '~'

        // ProductNamePrefix: "000000000000000" (16 bytes)
        packet.AddRange(Encoding.ASCII.GetBytes("000000000000000"));

        // 고정 필드들
        packet.AddRange(new byte[] { 0xff, 0xff, 0xff, 0xff });
        packet.AddRange(new byte[] { 0xbf, 0xbf, 0xaa, 0xbb });
        packet.Add(0x01);
        packet.Add(0xfe);
        packet.AddRange(new byte[] { 0x00, 0x00, 0x00, 0x00 });
        packet.AddRange(new byte[] { 0x02, 0xd4, 0xe1, 0x95 });
        packet.Add(0x7e); // 종료 바이트

        return packet.ToArray();
    }

    /// <summary>
    /// UDP 브로드캐스트 디스커버리 응답 파싱
    /// ACS 형식: 7e + bfbfaabb + ProductName(16) + ... + 7e
    /// </summary>
    private DiscoveredDevice? ParseUdpDiscoveryResponse(string ipAddress, byte[] data)
    {
        try
        {
            if (data.Length < 32)
            {
                LogHub.Instance.Warn($"응답 패킷 너무 짧음: {data.Length} bytes");
                return null;
            }

            // ACS 형식 파싱: 7e + bfbfaabb + ...
            if (data[0] == 0x7e && data.Length >= 36)
            {
                // 매직 헤더 확인
                if (data[1] == 0xbf && data[2] == 0xbf && data[3] == 0xaa && data[4] == 0xbb)
                {
                    // ProductName (16 bytes) - offset 5
                    var productNameBytes = new byte[16];
                    Array.Copy(data, 5, productNameBytes, 0, 16);
                    var productName = Encoding.ASCII.GetString(productNameBytes).TrimEnd('\0');

                    // IP 주소 (4 bytes) - offset 38 (실제 응답 패킷 분석 결과)
                    if (data.Length >= 42)
                    {
                        var ipBytes = new byte[4];
                        Array.Copy(data, 38, ipBytes, 0, 4);
                        var detectedIp = $"{ipBytes[0]}.{ipBytes[1]}.{ipBytes[2]}.{ipBytes[3]}";

                        LogHub.Instance.Info($"ACS 형식 응답 파싱 성공: {detectedIp}, SN={productName}");

                        return new DiscoveredDevice(
                            detectedIp,
                            productName,
                            productName,
                            "Face Device",
                            "Unknown");
                    }
                }
            }

            // 기존 형식 시도 (magic1 + magic2)
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
            var productNameBytes2 = reader.ReadBytes(ProductNameLen_Max);
            var productName2 = Encoding.UTF8.GetString(productNameBytes2).TrimEnd('\0');

            var deviceId = reader.ReadUInt32();
            var dwIp = reader.ReadUInt32();
            var dwSubnetMask = reader.ReadUInt32();
            var dwDefaultGateway = reader.ReadUInt32();
            var port2 = reader.ReadUInt16();
            var useDhcp = reader.ReadUInt16();

            // IP 주소 변환 (빅 엔디안)
            var detectedIp2 = $"{(dwIp >> 24) & 0xFF}.{(dwIp >> 16) & 0xFF}.{(dwIp >> 8) & 0xFF}.{dwIp & 0xFF}";

            LogHub.Instance.Info($"기존 형식 응답 파싱 성공: {detectedIp2}, SN={productName2}");

            return new DiscoveredDevice(
                detectedIp2,
                deviceId.ToString(),
                productName2,
                productName2,
                "Unknown");
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

    /// <summary>
    /// 유효한 네트워크 인터페이스 목록 가져오기 (로컬 IP, 브로드캐스트 IP)
    /// </summary>
    public List<(IPAddress localIp, IPAddress broadcastIp)> GetValidNetworkInterfaces()
    {
        var result = new List<(IPAddress, IPAddress)>();

        try
        {
            var interfaces = NetworkInterface.GetAllNetworkInterfaces()
                .Where(ni => ni.OperationalStatus == OperationalStatus.Up &&
                            ni.NetworkInterfaceType != NetworkInterfaceType.Loopback)
                .ToList();

            foreach (var ni in interfaces)
            {
                var props = ni.GetIPProperties();
                var ipv4Addresses = props.UnicastAddresses
                    .Where(addr => addr.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                    .ToList();

                foreach (var addr in ipv4Addresses)
                {
                    var ipString = addr.Address.ToString();

                    // 루프백 제외 (이미 위에서 처리됨)
                    if (ipString.StartsWith("127."))
                    {
                        continue;
                    }

                    // 브로드캐스트 주소 계산
                    var ip = addr.Address.GetAddressBytes();
                    var mask = addr.IPv4Mask.GetAddressBytes();
                    var broadcast = new byte[4];

                    for (int i = 0; i < 4; i++)
                    {
                        broadcast[i] = (byte)(ip[i] | ~mask[i]);
                    }

                    var broadcastIp = new IPAddress(broadcast);
                    result.Add((addr.Address, broadcastIp));
                }
            }
        }
        catch (Exception ex)
        {
            LogHub.Instance.Warn($"네트워크 인터페이스 정보 조회 실패: {ex.Message}");
        }

        return result;
    }

    /// <summary>
    /// 로컬 네트워크 인터페이스 정보 로깅
    /// </summary>
    private void LogNetworkInterfaces()
    {
        try
        {
            LogHub.Instance.Info("=== 네트워크 인터페이스 정보 ===");

            var interfaces = NetworkInterface.GetAllNetworkInterfaces()
                .Where(ni => ni.OperationalStatus == OperationalStatus.Up && 
                            ni.NetworkInterfaceType != NetworkInterfaceType.Loopback)
                .ToList();

            if (!interfaces.Any())
            {
                LogHub.Instance.Warn("활성화된 네트워크 인터페이스를 찾을 수 없습니다.");
                return;
            }

            foreach (var ni in interfaces)
            {
                var props = ni.GetIPProperties();
                var ipv4Addresses = props.UnicastAddresses
                    .Where(addr => addr.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                    .ToList();

                LogHub.Instance.Info($"인터페이스: {ni.Name} ({ni.Description})");
                LogHub.Instance.Info($"  - 상태: {ni.OperationalStatus}");
                LogHub.Instance.Info($"  - 타입: {ni.NetworkInterfaceType}");

                foreach (var addr in ipv4Addresses)
                {
                    LogHub.Instance.Info($"  - IP: {addr.Address} / {addr.IPv4Mask}");
                }

                if (!ipv4Addresses.Any())
                {
                    LogHub.Instance.Warn($"  - IPv4 주소가 없습니다.");
                }
            }

            LogHub.Instance.Info("================================");
        }
        catch (Exception ex)
        {
            LogHub.Instance.Warn($"네트워크 인터페이스 정보 조회 실패: {ex.Message}");
        }
    }
}
