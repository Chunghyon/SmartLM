# UDP 브로드캐스트 검색 응답 없음 문제 해결

## 현재 상황
- FDDC에서 UDP 브로드캐스트를 전송하지만 단말기 응답이 없음
- ACS에서는 동일한 네트워크에서 정상 작동

## 주요 수정 사항 ?

### 1. 브로드캐스트 주소 수정
```
변경 전: 255.255.255.255 (전역 브로드캐스트)
변경 후: 10.100.100.255 (서브넷 브로드캐스트)
```

**로그 확인:**
```
[08:49:59.563] · 포트 8101로 UDP 검색 시작: 10.100.100.254 → 10.100.100.255:8101
[08:49:59.563] · 서브넷 브로드캐스트 주소 사용: 10.100.100.255 (255.255.255.255가 아님!)
```

### 2. UDP 포트 설정 가능
- 검색 다이얼로그에서 UDP 포트 지정 가능 (기본값: 8101)
- 다른 포트로 변경하여 테스트 가능

### 3. UDP 소켓 옵션 최적화
```csharp
binaryClient.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
binaryClient.Client.ReceiveBufferSize = 65536;
```

### 4. ACS 응답 형식 파싱 추가
- `7e + bfbfaabb + ProductName(16) + ...` 형식 지원
- 기존 형식도 fallback으로 지원

### 5. Windows 방화벽 자동 체크
- 프로그램 시작 시 방화벽 규칙 확인
- 규칙이 없으면 자동 추가 시도 (관리자 권한 필요)
- 실패 시 수동 추가 방법 안내

## 다음 단계: 문제 진단 ??

### 1단계: 로그 확인
프로그램 시작 시 다음 로그가 나타나는지 확인:

```
? Windows 방화벽 규칙이 설정되어 있습니다.
```

또는

```
?? Windows 방화벽 규칙이 없습니다. UDP 브로드캐스트 검색이 차단될 수 있습니다.
```

### 2단계: 수동 방화벽 규칙 추가 (필요시)

**PowerShell (관리자 권한):**
```powershell
New-NetFirewallRule -DisplayName "FDDC UDP Discovery" `
    -Direction Inbound `
    -Protocol UDP `
    -LocalPort 1024-65535 `
    -Action Allow
```

**또는 프로그램 경로로:**
```powershell
$exePath = "D:\Documents\Smart_LM_China\Communication Protocols\.NET\FaceDeviceHttpPcServer\bin\Debug\net9.0-windows\FaceDeviceHttpPcServer.exe"

New-NetFirewallRule -DisplayName "FDDC UDP Discovery" `
    -Direction Inbound `
    -Protocol UDP `
    -Program $exePath `
    -Action Allow
```

**방화벽 규칙 확인:**
```powershell
Get-NetFirewallRule -DisplayName "FDDC UDP Discovery"
```

### 3단계: Wireshark로 패킷 캡처

**필터:** `udp.port == 8101 || udp.port == 58537 || udp.port == 58538`

**기대 결과:**
```
# 송신 (FDDC → 브로드캐스트)
10.100.100.254:58537 → 10.100.100.255:8101 [UDP 36 bytes]
10.100.100.254:58538 → 10.100.100.255:8101 [UDP 28 bytes]

# 수신 (단말기 → FDDC)
10.100.100.10:8101 → 10.100.100.254:58537 [UDP 179 bytes]
```

**송신만 보이는 경우:**
1. ? Windows 방화벽이 차단 중
2. ? 단말기가 오프라인
3. ? 단말기가 다른 네트워크에 있음

**아무것도 안 보이는 경우:**
- Wireshark가 올바른 네트워크 인터페이스를 캡처하지 않음

### 4단계: 단말기 직접 접속 테스트

**브라우저에서:**
```
http://10.100.100.10
```

**응답이 있으면:** 단말기는 온라인이지만 UDP Discovery가 비활성화됨
**응답이 없으면:** 단말기가 오프라인이거나 IP가 다름

### 5단계: 네트워크 경로 확인

```powershell
# PC → 단말기 핑 테스트
ping 10.100.100.10

# 라우팅 테이블 확인
route print

# ARP 캐시 확인
arp -a
```

## 가능한 원인 분석

### 원인 1: Windows 방화벽 차단 (가장 가능성 높음) ??
**증상:**
- 패킷은 전송되지만 응답을 받지 못함
- Wireshark에서 송신은 보이지만 수신이 안 보임

**해결:**
- 방화벽 규칙 추가 (위의 2단계 참고)

### 원인 2: 서브넷 불일치
**증상:**
- PC와 단말기가 다른 서브넷에 있음

**확인:**
```
PC IP:      10.100.100.254/24
단말기 IP:   10.100.100.10/24
서브넷:     같음 ?
```

**해결:**
- 이미 같은 서브넷이므로 문제 없음

### 원인 3: 단말기 UDP Discovery 비활성화
**증상:**
- 단말기 웹 UI는 접속되지만 UDP 응답이 없음

**해결:**
- 단말기 웹 UI → 네트워크 설정 → UDP Discovery 활성화

### 원인 4: 네트워크 장비 필터링
**증상:**
- 스위치/라우터가 브로드캐스트를 차단

**확인:**
- ACS는 작동하므로 이 문제는 아님

### 원인 5: 패킷 내용 불일치
**증상:**
- 단말기가 FDDC 패킷을 인식하지 못함

**확인:**
```
ACS:  7E30303030...0279EE477E
FDDC: 7E30303030...022D63707E
```

**차이:** 체크섬만 다름 (일반적으로 문제 없음)

## 테스트 시나리오

### 시나리오 1: 방화벽 테스트
1. Windows 방화벽 완전 비활성화
2. FDDC에서 UDP 검색 실행
3. 응답이 오면 → 방화벽이 원인
4. 방화벽 다시 활성화 후 규칙 추가

### 시나리오 2: 직접 UDP 테스트
PowerShell에서:
```powershell
$udp = New-Object System.Net.Sockets.UdpClient
$udp.EnableBroadcast = $true
$bytes = [byte[]](0x7e, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0xff, 0xff, 0xff, 0xff, 0xbf, 0xbf, 0xaa, 0xbb, 0x01, 0xfe, 0x00, 0x00, 0x00, 0x00, 0x02, 0x2d, 0x63, 0x70, 0x7e)
$udp.Send($bytes, $bytes.Length, "10.100.100.255", 8101)
# 5초 대기
$udp.Client.ReceiveTimeout = 5000
try {
    $remoteEP = New-Object System.Net.IPEndPoint([System.Net.IPAddress]::Any, 0)
    $received = $udp.Receive([ref]$remoteEP)
    Write-Host "응답 수신: $($remoteEP.Address):$($remoteEP.Port), $($received.Length) bytes"
} catch {
    Write-Host "응답 없음: $_"
}
$udp.Close()
```

### 시나리오 3: 포트 변경 테스트
1. 다른 UDP 포트로 변경 (예: 8102)
2. 단말기 설정도 동일하게 변경
3. 검색 실행

## 최종 체크리스트

- [ ] Windows 방화벽 규칙 추가됨
- [ ] PC IP: `10.100.100.254` (단말기와 같은 서브넷)
- [ ] 단말기 IP: `10.100.100.10` (ping 응답 있음)
- [ ] 브로드캐스트 주소: `10.100.100.255` (서브넷 브로드캐스트)
- [ ] UDP 포트: `8101` (기본값)
- [ ] Wireshark에서 송신 패킷 확인됨
- [ ] Wireshark에서 수신 패킷 확인됨
- [ ] 단말기 웹 UI 접속 가능: `http://10.100.100.10`
- [ ] 단말기 UDP Discovery 기능 활성화됨

## 예상 로그 (정상 작동 시)

```
[08:49:59.560] · ? Windows 방화벽 규칙이 설정되어 있습니다.
[08:49:59.563] · 바이너리 요청 (36 bytes): 7E30303030...
[08:49:59.563] · JSON 요청 (28 bytes): {"cmd":"UDPSerach","Ver":1}
[08:49:59.563] · 포트 8101로 UDP 검색 시작: 10.100.100.254 → 10.100.100.255:8101
[08:49:59.563] · 서브넷 브로드캐스트 주소 사용: 10.100.100.255
[08:49:59.563] · ? 바이너리 전송: 10.100.100.254:58537 → 10.100.100.255:8101
[08:49:59.618] · ? JSON 전송: 10.100.100.254:58538 → 10.100.100.255:8101
[08:49:59.618] · ?? Windows 방화벽에서 UDP 포트 58537, 58538 수신을 허용해야 합니다!
[08:49:59.700] · UDP 응답 수신: 10.100.100.10:8101, 179 bytes
[08:49:59.701] · 응답 패킷 내용 (처음 80 bytes): 7E BF BF AA BB 46 43 2D ...
[08:49:59.701] · ACS 형식 응답 파싱 성공: 10.100.100.10, SN=FD-8190H2506129, Port=8101
[08:49:59.701] · ? 디바이스 발견: 10.100.100.10 - FD-8190H2506129 (Face Device)
[08:49:59.701] · 브로드캐스트 검색 완료: 1개 디바이스 발견 (소요 시간: 0.2초)
```

## 연락처 & 추가 지원

문제가 계속되면 다음 정보를 제공해주세요:
1. FDDC 로그 전체
2. Wireshark 캡처 파일 (.pcapng)
3. `ipconfig /all` 출력
4. `Get-NetFirewallRule | Where-Object {$_.DisplayName -like "*FDDC*"}` 출력
5. 단말기 모델 및 펌웨어 버전
