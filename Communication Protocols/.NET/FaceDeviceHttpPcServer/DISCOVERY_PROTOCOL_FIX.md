# Discovery 프로토콜 수정 - 핵심 변경 사항

## ?? 문제 원인

Access Control System에서는 디바이스가 정상 감지되지만, 우리 프로그램에서는 감지되지 않았던 이유:

### ? 이전 방식의 문제
```csharp
// 송신용 클라이언트 (임시 포트)
using var sendClient = new UdpClient(new IPEndPoint(localIp, 0));
await sendClient.SendAsync(data, ...);

// 수신용 클라이언트 (다른 임시 포트)
var receiveClient = new UdpClient(0);
```

**문제점**:
- 송신과 수신에 **서로 다른 포트** 사용
- 디바이스는 **요청을 보낸 포트로 응답**
- 수신 클라이언트는 다른 포트에서 대기 → **응답을 받을 수 없음**

---

## ? 해결 방법

### 송수신을 동일한 UdpClient로 처리

```csharp
// 동일한 클라이언트로 송수신
using var udpClient = new UdpClient(new IPEndPoint(localIp, 0));
udpClient.EnableBroadcast = true;

// 송신
await udpClient.SendAsync(data, data.Length, broadcastEndpoint);

// 수신 (같은 udpClient 사용!)
var result = await udpClient.ReceiveAsync();
```

**작동 원리**:
1. `UdpClient(new IPEndPoint(localIp, 0))`
   - OS가 임의의 포트 할당 (예: `54590`)

2. 브로드캐스트 전송
   - 출발지: `192.168.0.62:54590`
   - 목적지: `192.168.0.255:20567`

3. 디바이스 응답
   - 출발지: `192.168.0.150:20567`
   - 목적지: `192.168.0.62:54590` ← **요청을 보낸 포트로 응답**

4. 같은 클라이언트로 수신
   - `192.168.0.62:54590`에서 대기 중이므로 응답 수신 ?

---

## ?? 주요 변경 사항

### 1. 단일 UdpClient 사용
```csharp
// 이전: 송신용 + 수신용 따로
using var sendClient = new UdpClient(...);
var receiveClient = new UdpClient(0);

// 현재: 하나로 통합
using var udpClient = new UdpClient(new IPEndPoint(localIp, 0));
```

### 2. 로컬 바인딩 정보 로깅
```csharp
var localEndPoint = (IPEndPoint)udpClient.Client.LocalEndPoint!;
LogHub.Instance.Info($"로컬 바인딩: {localEndPoint}");
```

**예시 로그**:
```
로컬 바인딩: 192.168.0.62:54590
? 브로드캐스트 전송: 192.168.0.62 → 192.168.0.255:20567
```

### 3. 응답 패킷 전체 내용 로깅 (64 bytes)
```csharp
var responseHex = BitConverter.ToString(result.Buffer.Take(Math.Min(64, result.Buffer.Length)).ToArray());
LogHub.Instance.Info($"응답 패킷 내용 (처음 64 bytes): {responseHex}");
```

### 4. 자기 패킷 필터링 제거
- 송수신이 같은 소켓이므로 자기가 보낸 패킷을 받지 않음
- 별도의 필터링 로직 불필요

---

## ?? 예상 결과

### 정상 작동 시 로그

```
=== 네트워크 인터페이스 정보 ===
인터페이스: 외부망 (Realtek PCIe GbE Family Controller)
  - IP: 192.168.0.62 / 255.255.255.0
================================

브로드캐스트 패킷 내용 (처음 32 bytes): 0D 38 58 0C B2 42 8B EA 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00

로컬 바인딩: 192.168.0.62:54590
? 브로드캐스트 전송: 192.168.0.62 → 192.168.0.255:20567
브로드캐스트 전송 완료: 포트 20567, 32 bytes, 대기 시간 30000ms

응답 대기 시작: 30000ms 동안 수신 대기...

UDP 응답 수신: 192.168.0.150:20567, 64 bytes (검색 시작 후 0.5초)
응답 패킷 내용 (처음 64 bytes): 84 CB 8F AA 87 CE FE 05 46 43 2D 38 31 39 30 48 ...
? 디바이스 발견: 192.168.0.150 - 12345678 (FC-8190H)

브로드캐스트 검색 완료: 1개 디바이스 발견 (소요 시간: 3.2초)
```

---

## ?? 디버깅 정보

### 응답 패킷이 여전히 없다면

**1단계: 응답 수신 여부 확인**
- 로그에 `UDP 응답 수신:` 메시지가 있는가?
  - 있으면: 패킷 구조 또는 매직 넘버 문제
  - 없으면: 디바이스가 응답하지 않음

**2단계: 디바이스 설정 확인**
```
디바이스 메뉴 → Local Setting
- Discovery: [?] Enable

디바이스 메뉴 → Client Protocol → OneCard Cloud
- Connect Mode: [?] Disconnect
```

**3단계: Wireshark 패킷 캡처**
- 인터페이스: "외부망 (Realtek PCIe GbE Family Controller)"
- 필터: `udp port 20567`

**확인 사항**:
- PC 브로드캐스트: `192.168.0.62:xxxxx → 192.168.0.255:20567`
- 디바이스 응답: `192.168.0.150:20567 → 192.168.0.62:xxxxx`

**4단계: Access Control System과 비교**
- Access Control System에서 Auto Search 실행
- Wireshark로 패킷 캡처
- 요청/응답 패킷 구조 비교

---

## ?? 네트워크 필터링

현재 **192.168.0.x 네트워크만** 사용하도록 필터링:

```csharp
if (!ipString.StartsWith("192.168.0."))
{
    continue;
}
```

다른 네트워크를 사용하려면 이 조건을 수정하세요.

---

## ?? 설정 값

| 항목 | 값 | 설명 |
|------|-----|------|
| Discovery Port | `20567` | UDP 브로드캐스트 포트 |
| Timeout | `30000ms` (30초) | 응답 대기 시간 |
| 네트워크 필터 | `192.168.0.x` | 사용할 네트워크 |
| 진행 로깅 간격 | 5초 | 대기 중 상태 로깅 |

---

## ?? 요약

**핵심 수정**: 송수신을 **동일한 UdpClient**로 처리
- 이전: 송신용 클라이언트 + 수신용 클라이언트 (다른 포트) → ?
- 현재: 하나의 UdpClient로 송수신 (같은 포트) → ?

**예상 결과**: Access Control System처럼 정상적으로 디바이스 감지

**다음 단계**:
1. 서버 재시작
2. Auto Search 실행
3. 로그에서 `UDP 응답 수신:` 확인
4. 여전히 실패하면 Wireshark로 패킷 분석
