# 브로드캐스트 검색 최종 분석

## ?? 핵심 발견 사항

### ? 멀티홈 환경 수정 작동 확인
```
? 브로드캐스트 전송: 10.100.100.237 → 10.100.255.255:20567
? 브로드캐스트 전송: 192.168.0.62 → 192.168.0.255:20567     ← 디바이스 네트워크
? 브로드캐스트 전송: 172.21.96.1 → 172.21.111.255:20567
브로드캐스트 전송 완료: 3개 인터페이스
```

### ?? 자기 자신이 보낸 패킷 수신 문제 (해결됨)
```
UDP 응답 수신: 192.168.0.62:54590  ← 자기 자신!
응답 패킷 내용: 0D 38 58 0C B2 42 8B EA ... ← 요청 매직 넘버
잘못된 매직 넘버: 0x0C58380D, 0xEA8B42B2 ← 응답이 아님
```

**원인**: 포트 20567로 바인딩하여 자신이 보낸 브로드캐스트도 수신
**해결**: 
- 수신 클라이언트를 임의의 포트(`UdpClient(0)`)로 바인딩
- 자기 IP에서 온 패킷을 필터링하여 무시

---

## ?? 디바이스가 응답하지 않는 이유

현재 로그에서 **디바이스(`192.168.0.150`)로부터 응답이 없음**

### 가능한 원인

#### 1. 디바이스 Discovery 설정 문제 ?????
- **Local Setting > Discovery**가 비활성화되어 있을 가능성
- **Client Protocol > OneCard Cloud**가 "Connect" 상태일 가능성
  - Access Control System과 연결 중이면 Discovery에 응답하지 않을 수 있음

**확인 방법**:
1. 디바이스 메뉴 접속 (IP 버튼 누르기)
2. `Local Setting` → `Discovery` 확인
3. `Client Protocol` → `OneCard Cloud` 상태 확인

**해결**:
- Discovery 활성화
- OneCard Cloud를 **"Disconnect"**로 변경
- 디바이스 재부팅

---

#### 2. 방화벽 차단 ????
- Windows 방화벽이 UDP 20567 포트 차단
- 백신/보안 프로그램이 브로드캐스트 차단

**확인 방법**:
```powershell
# 현재 방화벽 규칙 확인
Get-NetFirewallRule | Where-Object {$_.DisplayName -like "*20567*" -or $_.DisplayName -like "*Face*"}

# 방화벽 상태 확인
Get-NetFirewallProfile | Select-Object Name, Enabled
```

**해결 방법** (PowerShell 관리자 권한):
```powershell
# UDP 20567 포트 허용
New-NetFirewallRule -DisplayName "Face Device Discovery (UDP 20567)" `
    -Direction Inbound -Protocol UDP -LocalPort 20567 -Action Allow

New-NetFirewallRule -DisplayName "Face Device Discovery Out (UDP 20567)" `
    -Direction Outbound -Protocol UDP -LocalPort 20567 -Action Allow
```

**임시 테스트** (관리자 권한):
```powershell
# 방화벽 임시 비활성화
Set-NetFirewallProfile -Profile Domain,Public,Private -Enabled False

# Auto Search 재실행

# 반드시 재활성화!
Set-NetFirewallProfile -Profile Domain,Public,Private -Enabled True
```

---

#### 3. 네트워크 장비 설정 ???
- 스위치가 브로드캐스트 패킷을 필터링
- VLAN 분리
- 무선 네트워크의 AP Isolation

**확인 방법**:
- PC와 디바이스를 직접 연결하여 테스트 (크로스 케이블 또는 직접 스위치)
- 같은 스위치 포트에 연결 확인

---

#### 4. Discovery 프로토콜 차이 ?
- 디바이스 펌웨어 버전에 따라 Discovery 프로토콜이 다를 수 있음
- 매직 넘버나 패킷 구조 차이

**확인 방법**:
- Wireshark로 패킷 캡처
- Access Control System과 비교

---

## ?? Wireshark 패킷 분석

### 캡처 설정
1. **인터페이스 선택**: "외부망 (Realtek PCIe GbE Family Controller)"
2. **캡처 필터**: `udp port 20567`
3. Auto Search 실행

### 확인 사항

#### ? PC에서 브로드캐스트 전송 확인
```
출발지: 192.168.0.62:xxxxx
목적지: 192.168.0.255:20567
프로토콜: UDP
길이: 32 bytes
내용: 0D 38 58 0C B2 42 8B EA ...
```

#### ? 디바이스 응답 확인
**정상 응답 예상**:
```
출발지: 192.168.0.150:20567
목적지: 192.168.0.62:xxxxx
프로토콜: UDP
길이: 64+ bytes
내용: 84 CB 8F AA 87 CE FE 05 ... (응답 매직 넘버)
```

**현재 상황**: 위 응답이 보이지 않음 → 디바이스가 응답하지 않음

---

## ?? 추가 디버깅 단계

### 1단계: 디바이스 핑 테스트
```powershell
ping 192.168.0.150
```
- 응답 있으면: 네트워크 연결 정상
- 응답 없으면: 네트워크 연결 문제

---

### 2단계: HTTP 접속 테스트
```powershell
# 브라우저에서
http://192.168.0.150:8080/

# 또는 PowerShell
Invoke-WebRequest -Uri "http://192.168.0.150:8080/api/GetDeviceSN"
```
- 성공: 디바이스 작동 중, Discovery만 비활성화
- 실패: 디바이스 오프라인 또는 HTTP 포트 다름

---

### 3단계: 디바이스 설정 화면 확인

**설정 확인 항목**:
```
Local Setting
  ├─ Discovery: [ ] Disable / [?] Enable  ← 반드시 Enable
  └─ ...

Client Protocol
  ├─ OneCard Cloud
  │   ├─ Connect Mode: [ ] Connect / [?] Disconnect  ← 반드시 Disconnect
  │   └─ ...
  └─ HTTP Phase-2: ...
```

---

### 4단계: Access Control System과 비교

**Access Control System에서**:
1. Auto Search 실행
2. Wireshark로 패킷 캡처
3. Discovery 패킷 내용 확인
4. 현재 시스템 패킷과 비교

---

## ?? 권장 조치 순서

### ? 즉시 실행
1. **서버 재시작** - 자기 패킷 필터링 적용
2. **Auto Search 재실행**
3. 로그 확인:
   ```
   → 자기 자신이 보낸 패킷 무시: 192.168.0.62:xxxxx  ← 이 로그가 보여야 함
   ```

---

### ?? 고우선순위
1. **디바이스 설정 확인 및 변경**:
   - Discovery: Enable
   - OneCard Cloud: Disconnect
   - 디바이스 재부팅

2. **방화벽 규칙 추가**:
   ```powershell
   New-NetFirewallRule -DisplayName "Face Device Discovery (UDP 20567)" `
       -Direction Inbound -Protocol UDP -LocalPort 20567 -Action Allow
   ```

---

### ?? 심화 분석
1. **Wireshark 패킷 캡처**
   - PC 브로드캐스트 전송 확인
   - 디바이스 응답 여부 확인

2. **방화벽 임시 비활성화 테스트**

3. **직접 연결 테스트**

---

## ?? 대안: Network Scan (권장)

브로드캐스트 Discovery가 계속 실패하더라도:

### Network Scan의 장점
- ? **HTTP 기반**: Discovery 설정 무관
- ? **이미 작동 확인**: `192.168.0.150` 발견 가능
- ? **실시간 결과**: 즉시 표시
- ? **상세 정보**: SN, 이름, 모델, 펌웨어 등

### 사용 방법
1. Admin 페이지 → **"Network Scan"**
2. 서브넷 입력: `192.168.0`
3. 스캔 시작

---

## ?? 요약

| 항목 | 상태 | 비고 |
|------|------|------|
| 멀티홈 브로드캐스트 | ? 작동 | 3개 인터페이스로 전송 확인 |
| 자기 패킷 필터링 | ? 구현됨 | 재시작 후 적용 |
| 디바이스 응답 | ? 없음 | 디바이스 설정/방화벽 확인 필요 |
| Network Scan | ? 대안 | 정상 작동 |

### ?? 다음 단계
1. 서버 재시작 → Auto Search
2. 디바이스 Discovery 설정 확인
3. 방화벽 규칙 추가
4. Wireshark 분석 (필요시)
5. Network Scan 사용 (대안)
