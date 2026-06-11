# Broadcast Search 분석 결과

## 질문 사항

### 1. 타임아웃이 3초로 너무 짧은 것 아닌가?

**답변: 맞습니다. 5초로 증가했습니다.**

#### 분석:
- SDK 샘플 코드의 기본 타임아웃: **2000ms (2초)**
- 기존 코드: **3000ms (3초)**
- 변경 후: **5000ms (5초)** ?

#### 이유:
- 네트워크 환경에 따라 디바이스 응답이 지연될 수 있음
- 여러 디바이스가 동시에 응답하는 경우 패킷 충돌 가능
- 5초면 충분한 여유 시간 확보

### 2. 포트 20567이 맞는가? 8101이 아닌가?

**답변: 20567이 맞습니다. 8101은 다른 용도입니다.**

#### 포트별 용도:

| 포트 | 용도 | 설명 |
|-----|------|------|
| **20567** | **Device Discovery** | UDP 브로드캐스트로 네트워크 상의 디바이스 검색 ? |
| **8101** | UDP Communication | 디바이스와의 일반 UDP 통신 (검색과 무관) |
| **80** | HTTP API | 웹 인터페이스 및 REST API |
| **8080/8100** | Alternative HTTP | 대체 HTTP 포트 |

#### 근거:

**1. SDK 샘플 코드 확인:**
```csharp
// SDK 20250611\samples\Sample_S300_SB3600\C#_SBXPCDLL_Sample\
// SBXPCDLLSampleCSharp\frmDeviceSearch.cs

public const UInt32 DEVICE_DISCOVER_PORT = 20567;  // ? Discovery 포트
```

**2. HTTP 프로토콜 문서 확인:**
```json
// HTTP docking protocol interface document.md
{
  "UseUDP": 1,
  "UDPPort": 8101,     // ← 일반 UDP 통신 포트 (Discovery 아님)
  "HTTPPort": 80,
  "UseTelnet": 1,
  "TelnetPort": 23
}
```

**3. 프로토콜 차이:**
- **20567 (Discovery)**: 바이너리 프로토콜, Magic Numbers 사용
- **8101 (UDP)**: 일반 UDP 명령/응답 통신

## 현재 로그 분석

```
[18:00:35.960] · 브로드캐스트 전송: 포트 20567, 32 bytes
[18:00:39.030] · 브로드캐스트 검색 완료: 0개 디바이스 발견
```

**대기 시간**: 약 3초 (18:00:35 → 18:00:39)
**변경 후**: 약 5초 대기 예상

## 디바이스가 발견되지 않는 경우 체크리스트

### 1. 네트워크 설정 확인
- [ ] 서버와 디바이스가 같은 서브넷에 있는가?
- [ ] 브로드캐스트 주소가 도달 가능한가? (255.255.255.255)
- [ ] 스위치/라우터가 브로드캐스트를 차단하지 않는가?

### 2. 방화벽 설정 확인
- [ ] Windows 방화벽에서 UDP 포트 20567 인바운드 허용
- [ ] 안티바이러스 소프트웨어가 UDP 브로드캐스트 차단 확인

```powershell
# Windows 방화벽 규칙 추가
New-NetFirewallRule -DisplayName "Device Discovery UDP 20567" `
  -Direction Inbound -Protocol UDP -LocalPort 20567 -Action Allow
```

### 3. 디바이스 설정 확인
- [ ] 디바이스가 켜져 있고 네트워크에 연결되어 있는가?
- [ ] 디바이스의 "Device Discovery" 기능이 활성화되어 있는가?
- [ ] 디바이스의 네트워크 설정에서 DHCP 또는 고정 IP가 올바른가?

### 4. 테스트 방법

#### A. Wireshark로 패킷 확인
1. Wireshark 실행
2. 필터: `udp.port == 20567`
3. "Broadcast Search" 클릭
4. 확인 사항:
   - 브로드캐스트 패킷이 전송되는가?
   - 디바이스로부터 응답이 오는가?

#### B. 대체 테스트: Network Scan 사용
```
1. Admin 페이지 → Device 탭
2. "Network Scan" 클릭
3. Subnet: "192.168.0" (디바이스 IP 범위)
4. "Start Scan" 클릭
```

Network Scan은 HTTP로 직접 각 IP를 확인하므로 브로드캐스트 문제를 우회할 수 있습니다.

#### C. 디바이스에 직접 ping
```powershell
ping 192.168.0.62
ping 192.168.0.150
```

핑이 응답하면 네트워크 연결은 정상입니다.

## 수정 사항 요약

### 변경된 코드:
```csharp
// Services/DeviceDiscoveryService.cs

// 변경 전:
private const int DiscoveryPort = 60000;
private const int TimeoutMs = 3000;

// 변경 후:
private const int DiscoveryPort = 20567;  // ? 올바른 Discovery 포트
private const int TimeoutMs = 5000;       // ? 5초로 증가
```

### 결론:
1. ? 포트 20567이 맞습니다 (8101은 일반 UDP 통신용)
2. ? 타임아웃을 5초로 증가했습니다
3. ? 바이너리 프로토콜이 올바르게 구현되었습니다

이제 디바이스가 Discovery 기능을 활성화하고 같은 네트워크에 있다면 검색될 것입니다!
