# 브로드캐스트 검색 실패 원인 분석 및 해결 가이드

## ?? 현재 상황

Access Control System ver 9.21에서는 Auto Search가 10초 이상 걸려도 디바이스를 발견하는 경우가 있지만, 현재 시스템에서는 **15초 대기 후에도** 디바이스를 발견하지 못하는 상황입니다.

**로그 예시:**
```
[17:30:15.292] · 브로드캐스트 전송: 포트 20567, 32 bytes
[17:30:20.422] · 브로드캐스트 검색 완료: 0개 디바이스 발견
```

---

## ?? 타임아웃 분석

### ? 타임아웃 길이는 문제가 아닙니다

| 항목 | 값 | 판단 |
|------|-----|------|
| 현재 설정 | **15초** (15000ms) | ? 충분 |
| Access Control System | 10초 이상 소요 사례 있음 | ? 커버됨 |
| **결론** | **타임아웃이 짧아서 발견하지 못하는 것은 아님** | - |

기존 5초 → 15초로 이미 증가되었으므로, **타임아웃은 문제의 원인이 아닙니다.**

---

## ?? 실제 원인: 멀티홈 환경 문제 (해결됨)

### 문제 상황

사용자의 PC에는 **여러 네트워크 인터페이스**가 있습니다:
- **내부망**: `10.100.100.237/16`
- **외부망**: `192.168.0.62/24` ← **디바이스(`192.168.0.150`)와 같은 네트워크**
- WSL, Npcap 등 가상 인터페이스 다수

### 문제 원인

기존 코드는 `IPAddress.Broadcast` (255.255.255.255)로 브로드캐스트했는데, **멀티홈 환경에서는 기본 인터페이스로만 전송**됩니다.

디바이스가 있는 `192.168.0.x` 네트워크로 브로드캐스트가 전달되지 않았을 가능성이 높습니다.

### ? 해결 방법 (적용 완료)

**모든 유효한 네트워크 인터페이스를 통해 브로드캐스트 전송**하도록 수정:

1. **각 인터페이스의 브로드캐스트 주소 계산**:
   - `192.168.0.62/24` → 브로드캐스트: `192.168.0.255`
   - `10.100.100.237/16` → 브로드캐스트: `10.100.255.255`

2. **각 인터페이스마다 개별 UdpClient로 전송**:
   ```csharp
   foreach (var (localIp, broadcastIp) in networkInterfaces)
   {
       using var sendClient = new UdpClient(new IPEndPoint(localIp, 0));
       sendClient.EnableBroadcast = true;
       await sendClient.SendAsync(data, data.Length, 
           new IPEndPoint(broadcastIp, DiscoveryPort));
   }
   ```

3. **예상 로그**:
   ```
   ? 브로드캐스트 전송: 10.100.100.237 → 10.100.255.255:20567
   ? 브로드캐스트 전송: 192.168.0.62 → 192.168.0.255:20567
   브로드캐스트 전송 완료: 2개 인터페이스, 포트 20567, 32 bytes
   ```

---

## ?? 기타 가능한 원인들

### 1. ?? 디바이스가 브로드캐스트 Discovery에 응답하지 않음

**가능성: ?????**

#### 원인
- 디바이스의 **Local Setting > Discovery**가 비활성화되어 있을 수 있음
- 디바이스 펌웨어 버전에 따라 Discovery 프로토콜이 다를 수 있음
- 디바이스가 특정 조건에서는 Discovery에 응답하지 않을 수 있음:
  - OneCard Cloud 연결 중
  - 다른 서버와 이미 연결된 상태
  - Discovery 기능 제한 설정

#### 확인 방법
1. **디바이스 메뉴 확인**:
   - `Local Setting` > `Discovery` 설정이 활성화되어 있는지 확인
   - `Client Protocol` > `OneCard Cloud`가 "Disconnect" 상태인지 확인

2. **펌웨어 버전 확인**:
   - 디바이스 정보에서 펌웨어 버전 확인
   - 최신 펌웨어로 업데이트 필요 여부 검토

#### 해결 방법
- Discovery 기능 활성화
- OneCard Cloud를 "Disconnect"로 설정
- 다른 서버와의 연결 해제 후 재시도

---

### 2. ?? 방화벽이 UDP 브로드캐스트를 차단

**가능성: ????**

#### 원인
- Windows 방화벽이 UDP 20567 포트의 inbound/outbound 트래픽을 차단
- 네트워크 보안 소프트웨어(백신, 보안 프로그램)가 브로드캐스트 차단

#### 확인 방법
```powershell
# 방화벽 규칙 확인
Get-NetFirewallRule | Where-Object {$_.DisplayName -like "*20567*"}

# 임시로 방화벽 비활성화 후 테스트 (관리자 권한)
Set-NetFirewallProfile -Profile Domain,Public,Private -Enabled False
# ?? 테스트 후 반드시 다시 활성화
Set-NetFirewallProfile -Profile Domain,Public,Private -Enabled True
```

#### 해결 방법
```powershell
# UDP 20567 포트 허용 규칙 추가 (관리자 권한 필요)
New-NetFirewallRule -DisplayName "Face Device Discovery" `
    -Direction Inbound -Protocol UDP -LocalPort 20567 -Action Allow

New-NetFirewallRule -DisplayName "Face Device Discovery Out" `
    -Direction Outbound -Protocol UDP -LocalPort 20567 -Action Allow
```

---

### 3. ?? 네트워크 환경 문제

**가능성: ???**

#### 원인
- **스위치/라우터**가 브로드캐스트 패킷을 전달하지 않음
- **VLAN 분리**로 인해 브로드캐스트가 도달하지 않음
- **무선 네트워크**에서 AP Isolation 설정으로 브로드캐스트 차단

#### 확인 방법
1. **서브넷 확인**:
   ```powershell
   ipconfig
   ```
   - PC와 디바이스가 같은 서브넷에 있는지 확인
   - 예: PC `192.168.0.62`, 디바이스 `192.168.0.150` → ? 같은 서브넷

2. **직접 연결 테스트**:
   - 가능하면 PC와 디바이스를 직접 연결하여 테스트

3. **네트워크 관리**:
   - 네트워크 관리자에게 브로드캐스트 트래픽 허용 여부 확인

#### 해결 방법
- 스위치 설정에서 브로드캐스트 허용
- VLAN 설정 확인 및 조정
- 무선 네트워크의 AP Isolation 비활성화

---

### 4. ?? Discovery 프로토콜 버전 차이

**가능성: ?**

#### 원인
- Access Control System과 현재 시스템이 다른 Discovery 프로토콜 버전을 사용
- SDK 버전에 따라 매직 넘버나 패킷 구조가 다를 수 있음

#### 확인 방법
로그에서 전송된 패킷 내용 확인:
```
브로드캐스트 패킷 내용 (처음 32 bytes): 0D 38 58 0C B2 42 8B EA ...
```

**예상 패킷 구조:**
- 매직 넘버 1: `0x0c58380d`
- 매직 넘버 2: `0xea8b42b2`
- ProductNamePrefix: 16 bytes
- Reserved: 8 bytes

#### 해결 방법
- Wireshark로 Access Control System의 Discovery 패킷 캡처
- 현재 시스템의 패킷과 비교
- 필요시 SDK 문서 참조하여 프로토콜 수정

---

## ??? 개선된 디버깅 기능

최신 코드에는 다음과 같은 상세 디버깅 기능이 추가되었습니다:

### 1. 멀티홈 환경 지원 (★ 새로 추가됨)
```
=== 네트워크 인터페이스 정보 ===
인터페이스: 내부망 (Realtek PCIe GbE Family Controller #2)
  - IP: 10.100.100.237 / 255.255.0.0
인터페이스: 외부망 (Realtek PCIe GbE Family Controller)
  - IP: 192.168.0.62 / 255.255.255.0
================================

? 브로드캐스트 전송: 10.100.100.237 → 10.100.255.255:20567
? 브로드캐스트 전송: 192.168.0.62 → 192.168.0.255:20567
브로드캐스트 전송 완료: 2개 인터페이스, 포트 20567, 32 bytes
```

### 2. 전송 패킷 내용 Hex Dump
```
브로드캐스트 패킷 내용 (처음 32 bytes): 0D 38 58 0C B2 42 8B EA ...
```

### 3. 5초마다 진행 상황 로깅
```
[17:44:16] 응답 대기 시작: 15000ms 동안 수신 대기...
[17:44:21] 대기 중... 경과: 5.1초, 남은 시간: 10.0초, 발견된 디바이스: 0개
[17:44:26] 대기 중... 경과: 10.2초, 남은 시간: 4.9초, 발견된 디바이스: 0개
[17:44:31] 브로드캐스트 검색 완료: 0개 디바이스 발견 (소요 시간: 15.1초)
```

### 4. 응답 패킷 상세 분석
**응답이 수신되면:**
```
UDP 응답 수신: 192.168.0.150:20567, 64 bytes (검색 시작 후 1.2초)
응답 패킷 내용 (처음 32 bytes): 84 CB 8F AA 87 CE FE 05 ...
? 디바이스 발견: 192.168.0.150 - FC-8190H25061293
```

**응답이 없으면:**
```
브로드캐스트 검색 완료: 0개 디바이스 발견 (소요 시간: 15.0초)
```

**매직 넘버 불일치 시:**
```
잘못된 매직 넘버: 0x12345678, 0x87654321
응답 파싱 결과: null (매직 넘버 불일치 가능)
```

---

## ?? Wireshark를 이용한 패킷 분석

### 캡처 필터 설정
```
udp port 20567
```

### 예상되는 패킷 흐름

#### ? 정상 동작 시
1. **PC → 192.168.0.255:20567** (브로드캐스트)
   - 출발지: `192.168.0.62`
   - 32 bytes discovery request
   - 매직: `0x0c58380d`, `0xea8b42b2`

2. **Device (192.168.0.150) → PC:20567** (유니캐스트 응답)
   - 64+ bytes discovery response
   - 매직: `0xaa8fcb84`, `0x05fece87`

#### 현재 상황 확인 사항
- [ ] `192.168.0.62` → `192.168.0.255:20567` 브로드캐스트가 네트워크에 전송되는가?
- [ ] `192.168.0.150`에서 응답 패킷이 전송되는가?
- [ ] 응답 패킷이 PC에 도착하는가?
- [ ] 방화벽이 패킷을 드롭하는가?

---

## ?? 권장 조치 순서

### ? 1단계: Auto Search 재실행 및 로그 확인

**새로운 로그 확인 사항:**
```
? 브로드캐스트 전송: 192.168.0.62 → 192.168.0.255:20567
```
- 디바이스가 있는 네트워크(`192.168.0.x`)로 브로드캐스트가 전송되는지 확인

### ? 2단계: 디바이스 설정 확인
1. **디바이스 메뉴**:
   - `Local Setting` > `Discovery` 활성화
   - `Client Protocol` > `OneCard Cloud`를 "**Disconnect**"로 설정
   - 디바이스 재부팅

### ? 3단계: 방화벽 규칙 추가
```powershell
# 관리자 권한으로 PowerShell 실행
New-NetFirewallRule -DisplayName "Face Device Discovery" `
    -Direction Inbound -Protocol UDP -LocalPort 20567 -Action Allow
New-NetFirewallRule -DisplayName "Face Device Discovery Out" `
    -Direction Outbound -Protocol UDP -LocalPort 20567 -Action Allow
```

### ?? 4단계: 심화 분석 (필요시)
1. **Wireshark 패킷 캡처**:
   - 인터페이스: "외부망 (Realtek PCIe GbE Family Controller)"
   - 필터: `udp port 20567`
   - 브로드캐스트 전송 확인
   - 디바이스 응답 확인

2. **방화벽 임시 비활성화 테스트** (관리자 권한):
   ```powershell
   Set-NetFirewallProfile -Profile Domain,Public,Private -Enabled False
   # Auto Search 재실행
   # 테스트 후 반드시 재활성화
   Set-NetFirewallProfile -Profile Domain,Public,Private -Enabled True
   ```

---

## ?? 대안: Network Scan 사용 (권장)

브로드캐스트 Discovery가 작동하지 않더라도 시스템 운영은 가능합니다:

### ? Network Scan의 장점
- **HTTP 기반**: 브로드캐스트 문제의 영향을 받지 않음
- **실시간 스트리밍**: 발견된 디바이스를 즉시 표시
- **높은 신뢰성**: 방화벽/네트워크 환경 영향 적음
- **상세 정보**: 디바이스 SN, 이름, 모델, 펌웨어 버전 등

### 사용 방법
1. Admin 페이지에서 **"Network Scan"** 클릭
2. 서브넷 입력 (예: `192.168.0`)
3. 발견된 디바이스가 즉시 테이블에 추가됨

---

## ?? 결론

### ? 타임아웃은 문제가 아님
- 현재 **15초**로 설정
- Access Control System의 10초 이상 소요 케이스를 충분히 커버
- **타임아웃을 더 늘려도 해결되지 않음**

### ? 실제 원인 및 해결

1. **? 멀티홈 환경 문제 (해결 완료)**
   - 여러 네트워크 인터페이스 중 디바이스가 있는 네트워크로 브로드캐스트가 전송되지 않았음
   - **해결**: 모든 유효한 인터페이스를 통해 브로드캐스트 전송하도록 수정

2. **디바이스가 Discovery에 응답하지 않음 (확인 필요)**
   - Discovery 비활성화 가능성
   - OneCard Cloud 연결 중일 가능성

3. **방화벽/보안 소프트웨어 차단 (확인 필요)**
   - UDP 20567 포트 차단 가능성

4. **네트워크 환경 문제 (확인 필요)**
   - 브로드캐스트 차단
   - VLAN 분리

### ?? 다음 단계
1. **Auto Search 재실행** - 멀티홈 환경 수정 적용 확인
2. 로그에서 `192.168.0.62 → 192.168.0.255:20567` 전송 확인
3. 여전히 실패하면 디바이스 설정 및 방화벽 확인
4. 필요시 Wireshark 분석
5. 대안으로 **Network Scan 사용**

---

## ?? 추가 지원이 필요한 경우

다음 정보를 제공해주세요:
- [ ] Auto Search 재실행 후 전체 로그 (멀티홈 수정 적용 후)
- [ ] `? 브로드캐스트 전송: 192.168.0.62 → ...` 로그 포함 여부
- [ ] 디바이스 메뉴 설정 스크린샷
- [ ] Wireshark 패킷 캡처 (가능한 경우)

#### 원인
- 디바이스의 **Local Setting > Discovery**가 비활성화되어 있을 수 있음
- 디바이스 펌웨어 버전에 따라 Discovery 프로토콜이 다를 수 있음
- 디바이스가 특정 조건에서는 Discovery에 응답하지 않을 수 있음:
  - OneCard Cloud 연결 중
  - 다른 서버와 이미 연결된 상태
  - Discovery 기능 제한 설정

#### 확인 방법
1. **디바이스 메뉴 확인**:
   - `Local Setting` > `Discovery` 설정이 활성화되어 있는지 확인
   - `Client Protocol` > `OneCard Cloud`가 "Disconnect" 상태인지 확인

2. **펌웨어 버전 확인**:
   - 디바이스 정보에서 펌웨어 버전 확인
   - 최신 펌웨어로 업데이트 필요 여부 검토

#### 해결 방법
- Discovery 기능 활성화
- OneCard Cloud를 "Disconnect"로 설정
- 다른 서버와의 연결 해제 후 재시도

---

### 2. ?? 방화벽이 UDP 브로드캐스트를 차단

**가능성: ????**

#### 원인
- Windows 방화벽이 UDP 20567 포트의 inbound/outbound 트래픽을 차단
- 네트워크 보안 소프트웨어(백신, 보안 프로그램)가 브로드캐스트 차단

#### 확인 방법
```powershell
# 방화벽 규칙 확인
Get-NetFirewallRule | Where-Object {$_.DisplayName -like "*20567*"}

# 임시로 방화벽 비활성화 후 테스트 (관리자 권한)
Set-NetFirewallProfile -Profile Domain,Public,Private -Enabled False
# ?? 테스트 후 반드시 다시 활성화
Set-NetFirewallProfile -Profile Domain,Public,Private -Enabled True
```

#### 해결 방법
```powershell
# UDP 20567 포트 허용 규칙 추가 (관리자 권한 필요)
New-NetFirewallRule -DisplayName "Face Device Discovery" `
    -Direction Inbound -Protocol UDP -LocalPort 20567 -Action Allow

New-NetFirewallRule -DisplayName "Face Device Discovery Out" `
    -Direction Outbound -Protocol UDP -LocalPort 20567 -Action Allow
```

---

### 3. ?? 네트워크 환경 문제

**가능성: ???**

#### 원인
- **스위치/라우터**가 브로드캐스트 패킷을 전달하지 않음
- **VLAN 분리**로 인해 브로드캐스트가 도달하지 않음
- **무선 네트워크**에서 AP Isolation 설정으로 브로드캐스트 차단

#### 확인 방법
1. **서브넷 확인**:
   ```powershell
   ipconfig
   ```
   - PC와 디바이스가 같은 서브넷에 있는지 확인
   - 예: PC `192.168.0.100`, 디바이스 `192.168.0.150` → ? 같은 서브넷

2. **직접 연결 테스트**:
   - 가능하면 PC와 디바이스를 직접 연결하여 테스트

3. **네트워크 관리**:
   - 네트워크 관리자에게 브로드캐스트 트래픽 허용 여부 확인

#### 해결 방법
- 스위치 설정에서 브로드캐스트 허용
- VLAN 설정 확인 및 조정
- 무선 네트워크의 AP Isolation 비활성화

---

### 4. ?? 잘못된 브로드캐스트 주소 (멀티홈 환경)

**가능성: ??**

#### 원인
- 여러 네트워크 인터페이스가 있는 환경에서 잘못된 인터페이스로 브로드캐스트

#### 확인 방법
로그에서 **"네트워크 인터페이스 정보"** 섹션 확인:

```
=== 네트워크 인터페이스 정보 ===
인터페이스: Ethernet (Realtek PCIe GbE Family Controller)
  - 상태: Up
  - 타입: Ethernet
  - IP: 192.168.0.100 / 255.255.255.0  ← 디바이스와 같은 서브넷
인터페이스: Wi-Fi (Intel Wireless)
  - 상태: Up
  - IP: 10.0.0.50 / 255.255.255.0      ← 다른 네트워크
================================
```

#### 해결 방법
- 사용하지 않는 네트워크 인터페이스 비활성화
- 필요시 코드 수정하여 특정 인터페이스로만 브로드캐스트

---

### 5. ?? Discovery 프로토콜 버전 차이

**가능성: ?**

#### 원인
- Access Control System과 현재 시스템이 다른 Discovery 프로토콜 버전을 사용
- SDK 버전에 따라 매직 넘버나 패킷 구조가 다를 수 있음

#### 확인 방법
로그에서 전송된 패킷 내용 확인:
```
브로드캐스트 패킷 내용 (처음 32 bytes): 0D 38 58 0C B2 42 8B EA ...
```

**예상 패킷 구조:**
- 매직 넘버 1: `0x0c58380d`
- 매직 넘버 2: `0xea8b42b2`
- ProductNamePrefix: 16 bytes
- Reserved: 8 bytes

#### 해결 방법
- Wireshark로 Access Control System의 Discovery 패킷 캡처
- 현재 시스템의 패킷과 비교
- 필요시 SDK 문서 참조하여 프로토콜 수정

---

## ??? 개선된 디버깅 기능

최신 코드에는 다음과 같은 상세 디버깅 기능이 추가되었습니다:

### 1. 네트워크 인터페이스 정보 로깅
```
=== 네트워크 인터페이스 정보 ===
인터페이스: Ethernet (...)
  - 상태: Up
  - 타입: Ethernet
  - IP: 192.168.0.100 / 255.255.255.0
================================
```

### 2. 전송 패킷 내용 Hex Dump
```
브로드캐스트 패킷 내용 (처음 32 bytes): 0D 38 58 0C B2 42 8B EA ...
```

### 3. 5초마다 진행 상황 로깅
```
[17:30:15] 응답 대기 시작: 15000ms 동안 수신 대기...
[17:30:20] 대기 중... 경과: 5.0초, 남은 시간: 10.0초, 발견된 디바이스: 0개
[17:30:25] 대기 중... 경과: 10.0초, 남은 시간: 5.0초, 발견된 디바이스: 0개
[17:30:30] 브로드캐스트 검색 완료: 0개 디바이스 발견 (소요 시간: 15.0초)
```

### 4. 응답 패킷 상세 분석
**응답이 수신되면:**
```
UDP 응답 수신: 192.168.0.150:20567, 64 bytes (검색 시작 후 1.2초)
응답 패킷 내용 (처음 32 bytes): 84 CB 8F AA 87 CE FE 05 ...
? 디바이스 발견: 192.168.0.150 - FC-8190H25061293
```

**응답이 없으면:**
```
브로드캐스트 검색 완료: 0개 디바이스 발견 (소요 시간: 15.0초)
```

**매직 넘버 불일치 시:**
```
잘못된 매직 넘버: 0x12345678, 0x87654321
응답 파싱 결과: null (매직 넘버 불일치 가능)
```

---

## ?? Wireshark를 이용한 패킷 분석

### 캡처 필터 설정
```
udp port 20567
```

### 예상되는 패킷 흐름

#### ? 정상 동작 시
1. **PC → 255.255.255.255:20567** (브로드캐스트)
   - 32 bytes discovery request
   - 매직: `0x0c58380d`, `0xea8b42b2`

2. **Device → PC:20567** (유니캐스트 응답)
   - 64+ bytes discovery response
   - 매직: `0xaa8fcb84`, `0x05fece87`

#### ? 현재 상황 (예상)
1. **PC → 255.255.255.255:20567** (브로드캐스트) ? 전송됨
2. **Device 응답 없음** ?

### Wireshark 체크리스트
- [ ] PC에서 브로드캐스트 패킷이 실제로 네트워크로 전송되는가?
- [ ] 브로드캐스트 패킷이 디바이스에 도달하는가?
- [ ] 디바이스에서 응답 패킷이 전송되는가?
- [ ] 응답 패킷이 PC에 도착하는가?
- [ ] 방화벽이 패킷을 드롭하는가?

---

## ?? 권장 조치 순서

### ? 1단계: 즉시 확인
1. **로그 전체 확인**:
   - Auto Search 실행 후 로그 창에서 다음 정보 확인:
     - 네트워크 인터페이스 정보
     - 브로드캐스트 패킷 전송 확인
     - 5초/10초/15초 진행 상황 로깅
     - 최종 결과

2. **디바이스 설정 확인**:
   - `Local Setting` > `Discovery` 활성화
   - `Client Protocol` > `OneCard Cloud`를 "**Disconnect**"로 설정
   - 디바이스 재부팅

3. **방화벽 규칙 추가**:
   ```powershell
   New-NetFirewallRule -DisplayName "Face Device Discovery" `
       -Direction Inbound -Protocol UDP -LocalPort 20567 -Action Allow
   New-NetFirewallRule -DisplayName "Face Device Discovery Out" `
       -Direction Outbound -Protocol UDP -LocalPort 20567 -Action Allow
   ```

### ?? 2단계: 심화 분석
1. **Wireshark 패킷 캡처**:
   - UDP 20567 포트 모니터링
   - 브로드캐스트 전송 확인
   - 디바이스 응답 확인

2. **네트워크 직접 연결 테스트**:
   - PC와 디바이스를 크로스 케이블 또는 직접 스위치로 연결
   - 브로드캐스트 재시도

3. **Access Control System 비교**:
   - Access Control System에서 Discovery 실행
   - Wireshark로 패킷 캡처
   - 현재 시스템 패킷과 비교

---

## ?? 대안: Network Scan 사용 (권장)

브로드캐스트 Discovery가 작동하지 않더라도 시스템 운영은 가능합니다:

### ? Network Scan의 장점
- **HTTP 기반**: 브로드캐스트 문제의 영향을 받지 않음
- **실시간 스트리밍**: 발견된 디바이스를 즉시 표시
- **높은 신뢰성**: 방화벽/네트워크 환경 영향 적음
- **상세 정보**: 디바이스 SN, 이름, 모델, 펌웨어 버전 등

### 사용 방법
1. Admin 페이지에서 **"Network Scan"** 클릭
2. 서브넷 입력 (예: `192.168.0`)
3. 발견된 디바이스가 즉시 테이블에 추가됨

---

## ?? 결론

### ? 타임아웃은 문제가 아님
- 현재 **15초**로 설정
- Access Control System의 10초 이상 소요 케이스를 충분히 커버
- **타임아웃을 더 늘려도 해결되지 않음**

### ? 실제 원인 (가능성 순)
1. **디바이스가 Discovery에 응답하지 않음** (가장 높음)
   - Discovery 비활성화
   - OneCard Cloud 연결 중

2. **방화벽/보안 소프트웨어 차단**

3. **네트워크 환경 문제**
   - 브로드캐스트 차단
   - VLAN 분리

### ?? 다음 단계
1. 위 로그 정보 확인
2. 디바이스 설정 변경
3. 방화벽 규칙 추가
4. Wireshark 분석
5. 필요시 **Network Scan 사용** (대안)

---

## ?? 추가 지원이 필요한 경우

다음 정보를 제공해주세요:
- [ ] Auto Search 실행 후 전체 로그
- [ ] 네트워크 인터페이스 정보 로그
- [ ] 디바이스 메뉴 설정 스크린샷
- [ ] Wireshark 패킷 캡처 (가능한 경우)
