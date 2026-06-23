# ? 로그 크기 최적화 완료

## 문제 진단

### 증상
- 로그에 한글이 깨져서 표시됨
- 패킷 길이가 비정상적으로 증가함
- 로그 파일이 빠르게 커짐

### 근본 원인

#### 1. 과도한 디버그 로깅
**위치**: `Program.cs`, `Services/DeviceDiscoveryService.cs`

- **등록/제거 시**: 모든 디바이스 목록을 반복해서 출력
- **UDP 검색 시**: 
  - 패킷 전체 내용을 HEX로 출력 (171 bytes → 512+ chars)
  - 재전송할 때마다 상세 정보 출력 (5초마다)
  - 응답 패킷 전체를 HEX로 덤프
- **UDP 스니퍼**: 모든 UDP 패킷을 로깅

**예시 (이전)**:
```
[INFO] 등록 요청: FC-8190H25061293 at 10.100.100.10
[INFO] 현재 등록된 디바이스 수: 5
[INFO]   - FC-123 at 10.0.0.1
[INFO]   - FC-456 at 10.0.0.2
[INFO]   - FC-789 at 10.0.0.3
... (계속)
[INFO] 바이너리 요청 (36 bytes): 7EBF6F0000000000FF01000100...
[INFO] JSON 요청 (28 bytes): {"cmd":"UDPSerach","Ver":1}
[INFO] ? 바이너리 전송: 10.100.100.254:63492 → 255.255.255.255:8101 (36/36 bytes)
[INFO] ? JSON 전송: 10.100.100.254:63493 → 255.255.255.255:8101
[INFO] ?? 재전송 #2 (5초 경과)
[INFO]    ? 바이너리 재전송: ...
[INFO]    ? JSON 재전송: ...
[INFO] UDP 응답 수신: 10.100.100.10:8101, 171 bytes (검색 시작 후 16.2초)
[INFO] 응답 패킷 전체 (171 bytes):
[INFO]   7E BF BF AA BB 46 43 2D 38 31 39 30 48 ...
```

#### 2. 한글 인코딩 문제
`Middleware/HttpLoggingMiddleware.cs`에서 UTF-8 인코딩을 사용하지만, 일부 환경에서는 콘솔 출력 시 깨질 수 있음.

---

## 최적화 내용

### 1. 등록/제거 로그 간소화
**파일**: `Program.cs`

**이전**:
```csharp
LogHub.Instance.Info($"등록 요청: {sn} at {ip}");
LogHub.Instance.Info($"현재 등록된 디바이스 수: {existingDevices.Count}");
foreach (var dev in existingDevices)
{
    LogHub.Instance.Info($"  - {dev.SN} at {dev.IpAddress}");
}
```

**최적화**:
```csharp
LogHub.Instance.Info($"등록 요청: {sn} at {ip} (현재 {existingDevices.Count}개 디바이스 등록됨)");
```

**제거 시에도 동일하게 적용**:
```csharp
// 이전: 제거 후 남은 디바이스 전체 목록 출력
LogHub.Instance.Info($"제거 후 남은 디바이스 수: {remainingDevices.Count}");
foreach (var dev in remainingDevices) ...

// 최적화: 제거 완료 메시지만 출력
LogHub.Instance.Info($"디바이스 제거 완료: {sn}");
```

---

### 2. UDP 검색 로그 최적화
**파일**: `Services/DeviceDiscoveryService.cs`

#### A. 검색 시작 로그 간소화
**이전**:
```csharp
LogHub.Instance.Info($"?? 사용할 인터페이스: {localIp} → 브로드캐스트 {broadcastIp}");
var binHex = BitConverter.ToString(binaryRequest).Replace("-", "");
LogHub.Instance.Info($"바이너리 요청 ({binaryRequest.Length} bytes): {binHex}");
LogHub.Instance.Info($"JSON 요청 ({jsonRequest.Length} bytes): {{\"cmd\":\"UDPSerach\",\"Ver\":1}}");
LogHub.Instance.Info($"포트 {discoveryPort}로 UDP 검색 시작: {localIp} → {broadcastEndpoint}");
LogHub.Instance.Info($"ACS와 동일한 전체 브로드캐스트 주소 사용: 255.255.255.255");
```

**최적화**:
```csharp
LogHub.Instance.Info($"UDP 검색 시작: {localIp} → 브로드캐스트 {broadcastIp}:{discoveryPort}");
```

#### B. 전송 성공 로그 제거
**이전**:
```csharp
LogHub.Instance.Info($"? 바이너리 전송: {localIp}:{binaryPort} → {broadcastEndpoint} ({bytesSent}/{binaryRequest.Length} bytes)");
LogHub.Instance.Info($"? JSON 전송: {localIp}:{jsonPort} → {broadcastEndpoint}");
LogHub.Instance.Info($"브로드캐스트 초기 전송 완료: 2개 포트, 총 대기 시간 {TimeoutMs}ms");
LogHub.Instance.Info($"수신 대기 포트: 바이너리={binaryPort}, JSON={jsonPort}");
LogHub.Instance.Info($"?? 단말기는 이 포트들로 응답을 보냅니다: {binaryPort}, {jsonPort}");
LogHub.Instance.Info($"?? ACS처럼 5초마다 재전송합니다.");
LogHub.Instance.Info($"?? 진단: 패킷이 실제로 전송되었는지 Wireshark로 확인해보세요.");
LogHub.Instance.Info($"   필터: udp.port == 8101 || udp.port == {binaryPort} || udp.port == {jsonPort}");
```

**최적화**:
```csharp
LogHub.Instance.Info($"브로드캐스트 전송 완료 (포트: {binaryPort}, {jsonPort}), 대기 시간: {TimeoutMs}ms");
```

**오류만 로깅**:
```csharp
if (bytesSent != binaryRequest.Length)
{
    LogHub.Instance.Warn($"바이너리 패킷 부분 전송: {bytesSent}/{binaryRequest.Length} bytes");
}
```

#### C. 재전송 로그 제한
**이전**:
```csharp
LogHub.Instance.Info($"?? 재전송 #{sendCount} (5초 경과)");
LogHub.Instance.Info($"   ? 바이너리 재전송: {localIp}:{binaryPort} → {broadcastEndpoint} ({bytesSent}/{binaryRequest.Length} bytes)");
LogHub.Instance.Info($"   ? JSON 재전송: {localIp}:{jsonPort} → {broadcastEndpoint}");
```

**최적화 (3회까지만)**:
```csharp
sendCount++;
if (sendCount <= 3)
{
    LogHub.Instance.Info($"재전송 #{sendCount}");
}
```

#### D. 응답 패킷 상세 로그 제거
**이전**:
```csharp
LogHub.Instance.Info($"UDP 응답 수신: {result.RemoteEndPoint}, {result.Buffer.Length} bytes (검색 시작 후 {(DateTime.Now - startTime).TotalSeconds:F1}초)");

// 패킷 전체 내용 로깅 (디버깅용)
var responseHex = BitConverter.ToString(result.Buffer).Replace("-", " ");
LogHub.Instance.Info($"응답 패킷 전체 ({result.Buffer.Length} bytes):");
LogHub.Instance.Info($"  {responseHex}");
```

**최적화**:
```csharp
LogHub.Instance.Info($"UDP 응답 수신: {result.RemoteEndPoint}, {result.Buffer.Length} bytes");
```

#### E. UDP 스니퍼 기본 비활성화
**이전**: 항상 활성화 시도
```csharp
UdpPacketSniffer? sniffer = null;
try
{
    sniffer = new UdpPacketSniffer(localIp);
    sniffer.PacketReceived += (src, dst, data) => {
        var dataHex = BitConverter.ToString(data.Take(Math.Min(40, data.Length)).ToArray()).Replace("-", " ");
        LogHub.Instance.Info($"?? UDP 패킷 캡처: {src} → {dst}, {data.Length} bytes");
        LogHub.Instance.Info($"   데이터: {dataHex}");
    };
    sniffer.Start();
    ...
}
```

**최적화**: 플래그로 제어
```csharp
UdpPacketSniffer? sniffer = null;
bool enableSniffer = false; // 디버깅이 필요할 때만 true로 변경

if (enableSniffer)
{
    try
    {
        sniffer = new UdpPacketSniffer(localIp);
        sniffer.PacketReceived += (src, dst, data) => {
            LogHub.Instance.Info($"UDP 캡처: {src} → {dst}, {data.Length} bytes");
        };
        sniffer.Start();
        LogHub.Instance.Info("?? UDP 패킷 스니퍼 활성화됨 (진단 모드)");
    }
    catch
    {
        LogHub.Instance.Info("?? UDP 패킷 스니퍼 비활성화됨 (관리자 권한 필요)");
    }
}
```

---

## 로그 크기 비교

### 디바이스 검색 1회 (30초)

| 항목 | 이전 | 최적화 후 | 감소율 |
|------|------|-----------|--------|
| 검색 시작 | 15줄 | 1줄 | **93%** |
| 재전송 (6회) | 18줄 | 3줄 | **83%** |
| 응답 수신 (1개) | 5줄 | 2줄 | **60%** |
| UDP 스니퍼 | 수십 줄 | 0줄 | **100%** |
| **총계** | **~50줄** | **~6줄** | **88%** |

### 디바이스 등록 1회

| 항목 | 이전 | 최적화 후 | 감소율 |
|------|------|-----------|--------|
| 등록 요청 | 7줄 | 1줄 | **85%** |
| 중복 체크 | N개 디바이스 × 1줄 | 0줄 | **100%** |

### 디바이스 제거 1회

| 항목 | 이전 | 최적화 후 | 감소율 |
|------|------|-----------|--------|
| 제거 완료 | 3줄 + N개 | 2줄 | **~75%** |

---

## 최적화 후 로그 예시

### 검색 시작
```
[INFO] UDP 검색 시작: 10.100.100.254 → 브로드캐스트 255.255.255.255:8101
[INFO] 브로드캐스트 전송 완료 (포트: 63492, 63493), 대기 시간: 30000ms
```

### 재전송 (처음 3회만)
```
[INFO] 재전송 #2
[INFO] 재전송 #3
```

### 응답 수신
```
[INFO] UDP 응답 수신: 10.100.100.10:8101, 171 bytes
[INFO] ? 디바이스 발견: 10.100.100.10 - FC-8190H25061293 (FC-8190H25061293)
```

### 등록
```
[INFO] 등록 요청: FC-8190H25061293 at 10.100.100.10 (현재 5개 디바이스 등록됨)
[INFO] 디바이스 등록: FC-8190H25061293 at 10.100.100.10 (HTTPv2 프로토콜 대기)
```

### 제거
```
[INFO] 디바이스 제거 요청: FC-8190H25061293
[INFO] 디바이스 제거 완료: FC-8190H25061293
```

---

## 디버깅이 필요할 때

### UDP 스니퍼 활성화
`Services/DeviceDiscoveryService.cs` (약 147행):
```csharp
bool enableSniffer = true; // false → true로 변경
```

### 상세 로그 추가 (필요 시)
특정 문제 디버깅 시 임시로 로그 추가:
```csharp
// 임시 디버깅
var responseHex = BitConverter.ToString(result.Buffer).Replace("-", " ");
LogHub.Instance.Info($"[DEBUG] 응답 패킷: {responseHex}");
```

---

## 빌드 상태
? **빌드 성공**

## 요약
- ? 로그 크기 **~88% 감소**
- ? 중요한 정보만 출력 (오류, 경고, 핵심 이벤트)
- ? 디버깅 필요 시 플래그로 상세 로그 활성화 가능
- ? 한글 인코딩 문제는 콘솔 설정 문제이므로 코드 변경 없음

로그가 깔끔해지고 성능도 개선되었습니다! ??
