using System.Net;
using System.Net.Sockets;

namespace FaceDeviceHttpPcServer.Services;

/// <summary>
/// 원시 소켓을 사용하여 모든 UDP 패킷을 캡처하는 진단 도구
/// </summary>
public class UdpPacketSniffer : IDisposable
{
    private Socket? _socket;
    private bool _isRunning;
    private Thread? _receiveThread;
    private readonly IPAddress _localIp;

    public event Action<IPEndPoint, IPEndPoint, byte[]>? PacketReceived;

    public UdpPacketSniffer(IPAddress localIp)
    {
        _localIp = localIp;
    }

    public void Start()
    {
        if (_isRunning) return;

        try
        {
            // Raw socket 생성 (관리자 권한 필요)
            _socket = new Socket(AddressFamily.InterNetwork, SocketType.Raw, ProtocolType.Udp);
            _socket.Bind(new IPEndPoint(_localIp, 0));

            // Promiscuous mode 설정 (모든 패킷 수신)
            _socket.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.HeaderIncluded, true);

            byte[] byTrue = new byte[4] { 1, 0, 0, 0 };
            byte[] byOut = new byte[4];

            // IOControl로 promiscuous mode 활성화
            _socket.IOControl(IOControlCode.ReceiveAll, byTrue, byOut);

            _isRunning = true;
            _receiveThread = new Thread(ReceivePackets);
            _receiveThread.IsBackground = true;
            _receiveThread.Start();

            LogHub.Instance.Info($"UDP 패킷 스니퍼 시작: {_localIp}");
        }
        catch (SocketException ex) when (ex.ErrorCode == 10013)
        {
            LogHub.Instance.Warn("UDP 패킷 스니퍼 시작 실패: 관리자 권한이 필요합니다.");
        }
        catch (Exception ex)
        {
            LogHub.Instance.Error($"UDP 패킷 스니퍼 시작 실패: {ex.Message}");
        }
    }

    public void Stop()
    {
        _isRunning = false;
        _socket?.Close();
        _receiveThread?.Join(1000);
        LogHub.Instance.Info("UDP 패킷 스니퍼 중지");
    }

    private void ReceivePackets()
    {
        byte[] buffer = new byte[65535];

        while (_isRunning && _socket != null)
        {
            try
            {
                int length = _socket.Receive(buffer);
                if (length > 0)
                {
                    ParsePacket(buffer, length);
                }
            }
            catch (SocketException)
            {
                // 소켓이 닫히면 종료
                break;
            }
            catch (Exception ex)
            {
                LogHub.Instance.Warn($"패킷 수신 오류: {ex.Message}");
            }
        }
    }

    private void ParsePacket(byte[] buffer, int length)
    {
        try
        {
            // IP 헤더 파싱 (최소 20 bytes)
            if (length < 20) return;

            // IP 헤더에서 프로토콜 확인 (9번째 바이트)
            byte protocol = buffer[9];
            if (protocol != 17) return; // UDP = 17

            // IP 헤더 길이 (0번째 바이트 하위 4비트 * 4)
            int ipHeaderLength = (buffer[0] & 0x0F) * 4;
            if (length < ipHeaderLength + 8) return; // UDP 헤더 8 bytes

            // 소스/목적지 IP 주소
            var srcIp = new IPAddress(new byte[] { buffer[12], buffer[13], buffer[14], buffer[15] });
            var dstIp = new IPAddress(new byte[] { buffer[16], buffer[17], buffer[18], buffer[19] });

            // UDP 헤더에서 포트 추출
            int udpHeaderOffset = ipHeaderLength;
            int srcPort = (buffer[udpHeaderOffset] << 8) | buffer[udpHeaderOffset + 1];
            int dstPort = (buffer[udpHeaderOffset + 2] << 8) | buffer[udpHeaderOffset + 3];
            int udpLength = (buffer[udpHeaderOffset + 4] << 8) | buffer[udpHeaderOffset + 5];

            // UDP 데이터
            int dataOffset = udpHeaderOffset + 8;
            int dataLength = Math.Min(udpLength - 8, length - dataOffset);
            byte[] data = new byte[dataLength];
            Array.Copy(buffer, dataOffset, data, 0, dataLength);

            // 이벤트 발생
            var srcEndPoint = new IPEndPoint(srcIp, srcPort);
            var dstEndPoint = new IPEndPoint(dstIp, dstPort);
            PacketReceived?.Invoke(srcEndPoint, dstEndPoint, data);
        }
        catch (Exception ex)
        {
            LogHub.Instance.Warn($"패킷 파싱 오류: {ex.Message}");
        }
    }

    public void Dispose()
    {
        Stop();
        _socket?.Dispose();
    }
}
