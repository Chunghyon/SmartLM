# 얼굴인식 디바이스 통신 설정 가이드

## 현재 상황 분석

### 발견된 디바이스
- **FC-8190H25061293** (192.168.0.150:8080)

### 서버 정보
- **서버 IP**: 192.168.0.62
- **서버 HTTP 포트**: 8100 (기본 설정)
- **서버 역할**: FaceDeviceHttpPcServer (HTTP 프로토콜 Phase-2)

---

## ? 권장 디바이스 설정

### 디바이스: FC-8190H25061293 (192.168.0.150)

#### 1?? **Local Setting → IPv4**

```
? IP Address: 192.168.0.150 (현재 설정 유지)
? Subnet Mask: 255.255.255.0
? Gateway: 192.168.0.1

? Use UDP: ON
? UDP Port: 20567 (Discovery용 - 변경 불필요)
```

#### 2?? **Web Manage**

```
? Use: ON
? HTTP Port: 8080 (현재 설정 유지 - 정상)
   - 디바이스 웹 관리 인터페이스용 포트
```

#### 3?? **Client Protocol → HTTP** ? **중요**

```
? Use: ON (필수!)
? Protocol Type: HTTP V2 (권장)
? Server Address: http://192.168.0.62:8100
   - 포트 8100을 명시적으로 포함

? Keepalive Time: 30초 (권장)
   - 서버와의 연결 유지 주기

? Use GZIP: ON (선택사항)
   - 데이터 압축으로 네트워크 효율성 향상
```

#### 4?? **Client Protocol → OneCard Cloud** ??

```
? Connect Mode: 사용 안 함 권장
   - 현재 서버는 HTTP 프로토콜만 지원
   - UDP 모드는 다른 용도 (클라우드 서비스)

만약 사용해야 한다면:
?? Connect Mode: HTTP (UDP 아님)
?? Server IP: 192.168.0.62
?? Server Port: 8100
```

---

## ?? 포트 사용 정리

### 서버 (192.168.0.62)
| 포트 | 프로토콜 | 용도 | 상태 |
|------|---------|------|------|
| 8100 | HTTP | Phase-2 API 서버 | ? 실행 중 |
| 20567 | UDP | Device Discovery (수신) | ? 수신 대기 |

### 디바이스 (FC-8190H25061293)
| 포트 | 프로토콜 | 용도 | 권장 |
|------|---------|------|------|
| 8080 | HTTP | 웹 관리 인터페이스 | ? 정상 |
| 20567 | UDP | Discovery 응답 | ? 정상 |

---

## ?? 통신 프로토콜 설명

### 1. HTTP Client Protocol (권장) ?

**용도**: 디바이스 → 서버 통신 (Phase-2 프로토콜)

**통신 흐름**:
```
디바이스 (192.168.0.150)          서버 (192.168.0.62:8100)
   |                                    |
   |------- POST /Device/Keepalive --->|  (30초마다)
   |<----- addPeople=N, deletePeople=M-|
   |                                    |
   |-- POST /People/DownloadPeopleList-|  (인원 추가 요청)
   |<---------- 인원 리스트 ------------|
   |                                    |
   |-- POST /People/SelectDeleteInfo --|  (삭제 요청)
   |<---------- 삭제 리스트 ------------|
   |                                    |
   |-- POST /Record/UploadIdentifyRecord| (출입 기록)
   |<------------- OK ------------------|
```

**필수 엔드포인트**:
- `/Device/Keepalive` - 연결 유지 및 명령 수신
- `/People/DownloadPeopleList` - 인원 데이터 다운로드
- `/People/SelectDeleteInfo` - 삭제할 인원 조회
- `/Record/UploadIdentifyRecord` - 출입 기록 업로드
- `/Device/UploadWorkSetting` - 작업 설정 업로드
- `/Device/DownloadWorkSetting` - 작업 설정 다운로드

### 2. OneCard Cloud Protocol (선택사항)

**용도**: 클라우드 서비스 연동

**현재 서버**: HTTP 프로토콜만 지원하므로 사용 안 함 권장

만약 사용한다면:
- `Connect Mode: HTTP` 선택 (UDP 아님)
- `Server: http://192.168.0.62:8100`

### 3. UDP Discovery (자동)

**용도**: 네트워크에서 디바이스 자동 검색

**포트**: 20567 (변경 불필요)

**동작**:
- 서버가 UDP 브로드캐스트를 20567 포트로 전송
- 디바이스가 자신의 정보를 응답
- "Broadcast Search" 기능에서 사용

---

## ?? 설정 단계별 가이드

### Step 1: HTTP Client 설정 확인 ? 가장 중요!

1. 디바이스 메뉴 → `Comm` → `Client Protocol` → `HTTP`
2. 다음 설정 확인/변경:
   ```
   Use: ON (필수!)
   Protocol Type: HTTP V2
   Server Address: http://192.168.0.62:8100
   Keepalive Time: 30
   Use GZIP: ON (선택)
   ```
3. 설정 저장

### Step 2: OneCard Cloud 비활성화 (권장)

1. 디바이스 메뉴 → `Comm` → `Client Protocol` → `OneCard Cloud`
2. `Connect Mode`를 OFF로 설정
3. 설정 저장

### Step 3: 디바이스 재부팅

1. 설정 변경 후 디바이스 재부팅
2. 재부팅 후 1분 정도 대기 (초기화 시간)

### Step 4: 서버에서 연결 확인

1. 브라우저에서 `http://192.168.0.62:8100/admin` 접속
2. Device 탭 이동
3. Connected Devices에서 **FC-8190H25061293** 확인
4. Last Keepalive 시간이 업데이트되는지 확인 (30초마다)

---

## ?? 테스트 방법

### 1. Keepalive 테스트

**예상 로그** (서버):
```
[HH:MM:SS] ? POST /Device/Keepalive → 200
    ▼ Request Body
    {
      "SN": "FC-8190H25061293",
      "RelayStatus": 0,
      "KeepOpenStatus": 0,
      ...
    }
    ▼ Response Body
    {
      "result": 1,
      "AddPeople": 0,
      "DeletePeople": 0,
      ...
    }
```

### 2. 인원 추가 테스트

1. Admin 페이지 → Door Access 탭
2. "Add New Personnel" 클릭
3. 정보 입력:
   - User ID: TEST001
   - Name: 홍길동
   - Department: 개발팀
4. 저장
5. Device 탭에서 "Sync People" 클릭
6. 디바이스가 다음 Keepalive에서 인원 다운로드

**예상 로그**:
```
[HH:MM:SS] ? POST /Device/Keepalive → 200
    Response: { "AddPeople": 1 }
[HH:MM:SS] ? POST /People/DownloadPeopleList → 200
    Request: { "SN": "FC-8190H25061293", "Limit": 50 }
    Response: { "PeopleList": [{"UserID":"TEST001",...}] }
```

### 3. 출입 기록 테스트

1. 디바이스에서 얼굴 인식 수행
2. 서버 로그에서 기록 업로드 확인

**예상 로그**:
```
[HH:MM:SS] ? POST /Record/UploadIdentifyRecord → 200
    ▼ Form Data
    SN: FC-8190H25061293
    RecordDetail: {"UserID":"TEST001","Name":"홍길동",...}
    Photo: [binary]
```

3. Admin 페이지 → Access Record 탭에서 기록 확인

---

## ?? 문제 해결

### 문제 1: 디바이스가 서버에 연결되지 않음

**증상**: Connected Devices에 디바이스가 나타나지 않음

**해결 순서**:

1. ? **네트워크 연결 확인**
   ```powershell
   ping 192.168.0.62
   ```
   - 핑이 응답하지 않으면 네트워크 문제

2. ? **서버 실행 확인**
   - 브라우저에서 `http://192.168.0.62:8100/api-info` 접속
   - JSON 응답이 나오면 서버 정상

3. ? **디바이스 HTTP Client 설정 확인**
   - Use: ON 확인
   - Server: http://192.168.0.62:8100 확인 (포트 포함!)
   - 설정 저장 후 디바이스 재부팅

4. ? **방화벽 확인**
   ```powershell
   # Windows 방화벽 규칙 추가
   New-NetFirewallRule -DisplayName "FaceDevice HTTP Server 8100" `
     -Direction Inbound -Protocol TCP -LocalPort 8100 -Action Allow
   ```

5. ? **서버 로그 확인**
   - 서버 로그 창에서 Keepalive 요청이 오는지 확인
   - 오류 메시지가 있는지 확인

### 문제 2: Keepalive만 되고 인원 동기화 안 됨

**증상**: Last Keepalive는 업데이트되지만 인원이 전송되지 않음

**해결**:

1. ? **인원 추가 확인**
   - Admin → Door Access 탭
   - 인원이 리스트에 있는지 확인

2. ? **동기화 요청**
   - Device 탭 이동
   - 디바이스의 "Sync People" 버튼 클릭

3. ? **로그 확인**
   - 다음 Keepalive에서 `"AddPeople": 1` 응답 확인
   - `/People/DownloadPeopleList` 요청 확인

4. ? **디바이스 저장 공간 확인**
   - 디바이스 메뉴 → System → Storage
   - 저장 공간이 부족하면 인원 추가 실패

### 문제 3: 출입 기록이 업로드되지 않음

**증상**: 디바이스에서 인식은 되지만 서버에 기록이 없음

**해결**:

1. ? **디바이스 설정 확인**
   - 디바이스 메뉴 → Comm → Client Protocol → HTTP
   - "Upload Record" 옵션이 활성화되어 있는지 확인

2. ? **네트워크 상태 확인**
   - Keepalive가 정상적으로 되고 있는지 확인
   - 네트워크 지연이나 끊김이 없는지 확인

3. ? **서버 로그 확인**
   - `/Record/UploadIdentifyRecord` 요청이 오는지 확인
   - 오류 응답이 있는지 확인

4. ? **디바이스 재부팅**
   - 설정 리셋을 위해 재부팅

### 문제 4: Broadcast Search에서 디바이스가 발견되지 않음

**증상**: "Broadcast Search" 버튼을 눌러도 디바이스가 나타나지 않음

**이유**: 
- Broadcast Search는 UDP Discovery 프로토콜 사용
- 네트워크 환경에 따라 브로드캐스트가 차단될 수 있음

**해결**:

1. ? **Network Scan 사용** (권장)
   - "Network Scan" 버튼 클릭
   - Subnet: "192.168.0" 입력
   - "Start Scan" 클릭
   - HTTP로 직접 확인하므로 더 안정적

2. ? **방화벽 확인**
   ```powershell
   # UDP 20567 포트 허용
   New-NetFirewallRule -DisplayName "Device Discovery UDP 20567" `
     -Direction Inbound -Protocol UDP -LocalPort 20567 -Action Allow
   ```

3. ? **디바이스 Discovery 설정 확인**
   - Local Setting → UDP: ON 확인
   - UDP Port: 20567 확인

---

## ?? 최종 설정 체크리스트

### ? 필수 설정

| 항목 | 설정값 | 확인 |
|-----|--------|------|
| 디바이스 IP | 192.168.0.150 | ? |
| 디바이스 HTTP 포트 | 8080 | ? |
| **HTTP Client Use** | **ON** | ? |
| **HTTP Client Server** | **http://192.168.0.62:8100** | ? |
| HTTP Client Keepalive | 30초 | ? |
| 서버 접속 가능 | http://192.168.0.62:8100/admin | ? |

### ? 연결 확인

| 확인 항목 | 예상 결과 | 확인 |
|----------|----------|------|
| Admin 페이지 접속 | 정상 로드 | ? |
| Device 탭에 디바이스 표시 | FC-8190H25061293 표시 | ? |
| Last Keepalive 업데이트 | 30초마다 갱신 | ? |
| 인원 추가 후 동기화 | 디바이스에 인원 추가됨 | ? |
| 얼굴 인식 테스트 | 기록이 서버에 업로드됨 | ? |

---

## ?? 다음 단계

설정 완료 후:

1. ? 디바이스 재부팅
2. ? Admin 페이지에서 디바이스 연결 확인
3. ? 테스트 인원 추가 및 동기화
4. ? 얼굴 인식 테스트 및 기록 확인
5. ? 로그 모니터링으로 정상 동작 확인

---

## ?? 추가 지원

### 로그 파일 위치
- **서버 로그**: 콘솔 창 또는 Visual Studio Output 창
- **디바이스 로그**: 디바이스 메뉴 → System → Log

### 유용한 명령어

```powershell
# 서버 포트 확인
netstat -ano | findstr :8100

# 방화벽 규칙 확인
Get-NetFirewallRule | Where-Object {$_.DisplayName -like "*8100*"}

# 서버 재시작
# Visual Studio에서 F5 또는
dotnet run --project FaceDeviceHttpPcServer.csproj
```

---

축하합니다! 이제 얼굴인식 시스템이 정상적으로 통신할 준비가 되었습니다! ??
