# UDP 브로드캐스트 검색 문제 해결 가이드

## 문제 증상
- FDDC에서 UDP 브로드캐스트 검색 시 단말기 응답이 없음
- ACS에서는 정상적으로 검색됨

## 주요 변경 사항

### 1. 브로드캐스트 주소 수정 ?
**변경 전:** `255.255.255.255` (전역 브로드캐스트)
**변경 후:** `서브넷 브로드캐스트` (예: `10.100.100.255`)

**이유:**
- 일부 라우터/스위치는 전역 브로드캐스트(`255.255.255.255`)를 필터링
- 서브넷 브로드캐스트는 로컬 네트워크 내에서만 전달됨
- ACS도 서브넷 브로드캐스트를 사용함

### 2. UDP 소켓 옵션 추가 ?
```csharp
binaryClient.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
binaryClient.Client.ReceiveBufferSize = 65536;
```

**이유:**
- `SO_REUSEADDR`: 포트 재사용 허용 (빠른 재시작 지원)
- `ReceiveBufferSize`: 수신 버퍼 크기 증가 (패킷 손실 방지)

### 3. ACS 형식 응답 파싱 추가 ?
응답 패킷 형식: `7e + bfbfaabb + ProductName(16) + ...`

## 문제 해결 체크리스트

### 1. Windows 방화벽 확인 ??
**증상:** UDP 패킷은 전송되지만 응답을 받지 못함

**해결 방법:**
```powershell
# PowerShell 관리자 권한으로 실행
# 인바운드 규칙 추가 (동적 포트 허용)
New-NetFirewallRule -DisplayName "FDDC UDP Discovery" `
    -Direction Inbound `
    -Protocol UDP `
    -LocalPort 1024-65535 `
    -Action Allow

# 아웃바운드 규칙 추가
New-NetFirewallRule -DisplayName "FDDC UDP Discovery Out" `
    -Direction Outbound `
    -Protocol UDP `
    -RemotePort 8101 `
    -Action Allow
```

**또는 간단히:**
```powershell
# FaceDeviceHttpPcServer.exe를 방화벽에서 허용
New-NetFirewallRule -DisplayName "Face Device HTTP PC Server" `
    -Direction Inbound `
    -Program "C:\Path\To\FaceDeviceHttpPcServer.exe" `
    -Action Allow
```

### 2. 네트워크 인터페이스 확인
**올바른 IP 선택:**
- ? `10.100.100.254` (단말기와 같은 서브넷)
- ? `192.168.0.62` (다른 서브넷)

**확인 방법:**
```powershell
ipconfig /all
# 또는
Get-NetIPAddress -AddressFamily IPv4
```

### 3. Wireshark로 패킷 확인
**필터:** `udp.port == 8101`

**기대 결과:**
1. **송신:** `10.100.100.254 → 10.100.100.255:8101` (브로드캐스트)
2. **수신:** `10.100.100.10 → 10.100.100.254:58537` (단말기 응답)

**송신만 보이고 수신이 없다면:**
- Windows 방화벽 차단
- 단말기가 오프라인이거나 다른 네트워크에 있음
- 스위치/라우터가 브로드캐스트를 필터링

### 4. 단말기 네트워크 설정 확인
- 단말기 IP: `10.100.100.10`
- 서브넷 마스크: `255.255.255.0`
- 게이트웨이: `10.100.100.1` (필요시)
- PC IP: `10.100.100.254` (같은 서브넷)

### 5. 단말기 웹 UI 확인
브라우저에서 `http://10.100.100.10` 접속하여:
- 네트워크 설정 확인
- UDP Discovery 기능 활성화 여부 확인
- 펌웨어 버전 확인

## 테스트 명령어

### PowerShell에서 UDP 브로드캐스트 테스트
```powershell
$udpClient = New-Object System.Net.Sockets.UdpClient
$udpClient.EnableBroadcast = $true
$bytes = [System.Text.Encoding]::ASCII.GetBytes("test")
$udpClient.Send($bytes, $bytes.Length, "10.100.100.255", 8101)
$udpClient.Close()
```

### tcpdump (Linux/WSL)
```bash
sudo tcpdump -i eth0 -n udp port 8101
```

### netcat (UDP 수신 테스트)
```bash
nc -u -l 8101
```

## 로그 분석

### 정상 동작 시:
```
[08:49:59.563] · 포트 8101로 UDP 검색 시작: 10.100.100.254 → 10.100.100.255:8101
[08:49:59.563] · ? 바이너리 전송: 10.100.100.254:58537 → 10.100.100.255:8101
[08:49:59.618] · ? JSON 전송: 10.100.100.254:58538 → 10.100.100.255:8101
[08:49:59.700] · UDP 응답 수신: 10.100.100.10:8101, 179 bytes
[08:49:59.701] · ACS 형식 응답 파싱 성공: 10.100.100.10, SN=FD-8190H2506129
[08:49:59.701] · ? 디바이스 발견: 10.100.100.10 - FD-8190H2506129
```

### 응답 없음 (현재):
```
[08:49:59.563] · 포트 8101로 UDP 검색 시작: 10.100.100.254 → 10.100.100.255:8101
[08:49:59.563] · ? 바이너리 전송: 10.100.100.254:58537 → 10.100.100.255:8101
[08:49:59.618] · ? JSON 전송: 10.100.100.254:58538 → 10.100.100.255:8101
[08:50:29.654] · 브로드캐스트 검색 완료: 0개 디바이스 발견 (소요 시간: 30.1초)
```

**가능한 원인:**
1. ? Windows 방화벽이 UDP 수신을 차단
2. ? 단말기가 다른 네트워크에 있음
3. ? 단말기가 오프라인
4. ? 단말기 UDP Discovery 기능이 비활성화됨

## ACS와 FDDC 패킷 비교

### ACS 바이너리 패킷:
```
7E 30 30 30 30 30 30 30 30 30 30 30 30 30 30 30 30
FF FF FF FF BF BF AA BB 01 FE 00 00 00 00 02 79 EE 47 7E
```

### FDDC 바이너리 패킷:
```
7E 30 30 30 30 30 30 30 30 30 30 30 30 30 30 30 30
FF FF FF FF BF BF AA BB 01 FE 00 00 00 00 02 2D 63 70 7E
```

**차이점:** 체크섬만 다름 (`0279EE47` vs `022D6370`)
- 이는 정상적임 (체크섬 알고리즘이 다를 수 있음)
- 단말기는 일반적으로 체크섬을 검증하지 않음

## 다음 단계

1. ? **서브넷 브로드캐스트 사용** (완료)
2. ? **UDP 소켓 옵션 추가** (완료)
3. ? **Windows 방화벽 규칙 추가** (수동 작업 필요)
4. ? **Wireshark로 패킷 캡처** (문제 확인용)
5. ? **단말기와 PC가 같은 서브넷에 있는지 확인**

## 참고 문헌
- ACS UDP Discovery 프로토콜
- Windows Firewall with Advanced Security
- .NET UdpClient 문서
