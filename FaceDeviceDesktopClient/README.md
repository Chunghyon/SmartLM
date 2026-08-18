# Face Device Management System

이 솔루션은 얼굴 인식 단말기를 관리하기 위한 두 가지 애플리케이션을 포함합니다.

## 프로젝트 구성

### 1. FaceDeviceHttpPcServer
HTTP 서버 기반 관리 시스템 (웹 UI + Windows Forms 로그 뷰어)

**기능:**
- HTTP 서버로 동작하여 얼굴 인식 단말기와 통신
- 웹 브라우저 기반 관리 UI (`http://localhost:8100/admin/`)
- Windows Forms 로그 뷰어 (디버깅 및 모니터링)
- 디바이스 자동 검색 (UDP 브로드캐스트 및 네트워크 스캔)
- 디바이스 연결 관리
- 부서 관리
- 직원 관리
- 출퇴근 기록 조회 및 통계
- 출퇴근 데이터 CSV 내보내기

**실행 방법:**
```bash
dotnet run --project FaceDeviceHttpPcServer.csproj
```

또는 Visual Studio에서 `FaceDeviceHttpPcServer` 프로젝트를 시작 프로젝트로 설정하고 F5 키를 누릅니다.

**사용법:**
1. 프로그램이 시작되면 Windows Forms 로그 창과 함께 웹 서버가 시작됩니다
2. 로그 창에서 "웹 관리자" 버튼을 클릭하거나 브라우저에서 `http://localhost:8100/admin/` 접속
3. 웹 UI에서 모든 관리 기능 사용 가능

**포트:** 8100 (설정 변경 가능)

---

### 2. FaceDeviceDesktopClient
Windows Forms 데스크톱 클라이언트 애플리케이션

**기능:**
- 완전한 Windows Forms UI
- 서버 (`FaceDeviceHttpPcServer`)와 REST API로 통신
- 모든 웹 UI 기능과 동일:
  - Dashboard (시스템 정보 요약)
  - Device Install (디바이스 검색 및 연결)
  - Departments (부서 관리)
  - Personnel (직원 관리)
  - Attendance (출퇴근 관리 및 통계)

**실행 방법:**
```bash
dotnet run --project FaceDeviceDesktopClient/FaceDeviceDesktopClient.csproj
```

또는 Visual Studio에서 `FaceDeviceDesktopClient` 프로젝트를 시작 프로젝트로 설정하고 F5 키를 누릅니다.

**사용법:**
1. **먼저 `FaceDeviceHttpPcServer`를 실행**해야 합니다 (백엔드 서버)
2. `FaceDeviceDesktopClient`를 실행합니다
3. 탭을 전환하며 각 기능 사용

**필수 조건:** `FaceDeviceHttpPcServer`가 `http://localhost:8100`에서 실행 중이어야 합니다

---

## 시스템 아키텍처

```
┌─────────────────────────────────────┐
│  FaceDeviceHttpPcServer (백엔드)    │
│  - HTTP Server (포트 8100)           │
│  - REST API 엔드포인트               │
│  - 디바이스 상태 관리                │
│  - 데이터 저장소 (StateStore)        │
│  - Windows Forms 로그 뷰어           │
└─────────────────────────────────────┘
          ▲                    ▲
          │                    │
    HTTP/REST API        HTTP/REST API
          │                    │
┌─────────┴────────┐    ┌──────┴─────────────┐
│  웹 브라우저     │    │ FaceDeviceDesktop  │
│  (admin UI)      │    │ Client (WinForms)  │
└──────────────────┘    └────────────────────┘
```

---

## 주요 기능

### 1. 디바이스 관리
- **Auto Search**: UDP 브로드캐스트로 LAN 내 디바이스 자동 검색
- **Network Scan**: 지정된 서브넷 IP 범위 스캔
- **Connect**: 검색된 디바이스에 연결하여 관리 시스템에 등록
- **디바이스 상태 모니터링**: 마지막 Keepalive 시간, 대기 중인 작업 확인

### 2. 부서 관리
- 부서 추가, 조회, 삭제
- 부서 ID 및 이름 관리

### 3. 직원 관리
- 직원 정보 등록 (ID, 이름, 부서, 직책, 카드번호, 비밀번호)
- 접근 권한 설정 (일반 사용자, 관리자, 블랙리스트)
- 직원 검색 및 조회

### 4. 출퇴근 관리
- 출퇴근 기록 조회
- 필터링: 사용자 ID, 이름, 부서, 날짜 범위
- 출퇴근 통계: 총 기록 수, 고유 사용자 수, 부서 수
- CSV 내보내기 (Excel에서 열기 가능)

---

## API 엔드포인트

### 디바이스
- `POST /api/Device/Search` - 디바이스 검색
- `POST /api/Device/ProbeDevice` - 디바이스 정보 조회 (CORS 우회)
- `POST /api/Device/Connect` - 디바이스 연결
- `GET /admin/devices` - 연결된 디바이스 목록

### 부서
- `POST /api/Department/GetList` - 부서 목록 조회
- `POST /api/Department/New` - 부서 추가
- `POST /api/Department/Delete` - 부서 삭제

### 직원
- `POST /api/People/GetList` - 직원 목록 조회
- `POST /api/People/New` - 직원 추가
- `POST /api/People/Delete` - 직원 삭제

### 출퇴근
- `POST /api/Attendance/Search` - 출퇴근 기록 검색
- `POST /api/Attendance/Statistics` - 출퇴근 통계

---

## 기술 스택

- **.NET 9** (net9.0-windows)
- **ASP.NET Core Minimal APIs** (서버)
- **Windows Forms** (UI)
- **System.Text.Json** (JSON 처리)
- **HttpClient** (REST API 통신)

---

## 개발 환경

- **Visual Studio 2022** 또는 **Visual Studio Code**
- **.NET 9 SDK**
- **Windows OS** (Windows Forms 요구)

---

## 실행 순서

1. **서버 시작:**
   ```bash
   dotnet run --project FaceDeviceHttpPcServer.csproj
   ```

2. **웹 UI 접속 (옵션 1):**
   - 브라우저에서 `http://localhost:8100/admin/` 접속

3. **데스크톱 클라이언트 실행 (옵션 2):**
   ```bash
   dotnet run --project FaceDeviceDesktopClient/FaceDeviceDesktopClient.csproj
   ```

---

## 데이터 저장

- 모든 데이터는 `App_Data` 폴더에 JSON 형식으로 저장됩니다
- `App_Data/state.json`: 디바이스, 부서, 직원 정보
- `App_Data/records/`: 출퇴근 기록
- `App_Data/photos/`: 얼굴 사진 (있는 경우)

---

## 문제 해결

### "Connection failed: Failed to fetch" 오류
- CORS 문제로 인해 브라우저가 직접 디바이스에 접근할 수 없습니다
- 해결됨: `/api/Device/ProbeDevice` 프록시 엔드포인트 사용

### 데스크톱 클라이언트가 서버에 연결되지 않음
- `FaceDeviceHttpPcServer`가 실행 중인지 확인
- 서버 URL이 `http://localhost:8100`인지 확인
- 방화벽 설정 확인

### 디바이스가 검색되지 않음
- 디바이스와 PC가 동일한 네트워크에 있는지 확인
- 디바이스의 HTTP 포트가 열려 있는지 확인 (기본 80 또는 8100)
- UDP 포트 60000이 차단되지 않았는지 확인

---

## 라이선스

이 프로젝트는 Smart LM China 프로젝트의 일부입니다.

---

## 기여

버그 리포트 및 기능 요청은 GitHub Issues를 통해 제출해 주세요.
