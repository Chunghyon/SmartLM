# A3-8190 HTTP Server 예제  
**BOWE A3-8190 얼굴인식 출입 단말기 ─ Visual Studio C# WinForms 예제**

---

## 개요

이 예제는 **BOWE A3-8190 얼굴인식 단말기**와 통신하는 Windows PC 서버 애플리케이션입니다.  
A3-8190은 HTTP 프로토콜을 사용하며, **디바이스가 서버로 접속**하는 방식(Push 구조)을 채택합니다.

```
A3-8190 디바이스  →  POST /Device/Keepalive     →  이 서버 (PC)
A3-8190 디바이스  ←  응답: {AddPeople, Remote…} ←
A3-8190 디바이스  →  POST /People/DownloadPeopleList  →
A3-8190 디바이스  →  POST /Record/UploadIdentifyRecord →
```

---

## 개발 환경 (Requirements)

| 항목 | 내용 |
|------|------|
| IDE | Visual Studio 2019 / 2022 |
| .NET | .NET Framework 4.8 |
| 참조 DLL | `System.Web.Extensions` (GAC 내장, 별도 설치 불필요) |
| 권한 | **관리자 권한 실행 필요** (`http://+:PORT/` URL ACL) |

---

## 프로젝트 구조

```
C#_A3-8190_HTTPServer/
├── A3-8190_HTTPServer.sln
└── A3-8190_HTTPServer/
    ├── A3-8190_HTTPServer.csproj
    ├── app.manifest              ← 관리자 권한 요청 (requireAdministrator)
    ├── Program.cs                ← 진입점
    ├── frmMain.cs/.Designer.cs   ← 메인 폼 (서버 설정, 디바이스 목록, 원격 제어)
    ├── frmPersonnel.cs/Designer  ← 인원 관리 (추가/삭제/디바이스 전송)
    ├── frmRecords.cs/Designer    ← 출입 기록 조회 및 CSV 내보내기
    ├── Models/
    │   └── Models.cs             ← HTTP 프로토콜 데이터 모델
    └── HttpServer/
        └── A3HttpListener.cs     ← HttpListener 기반 HTTP 서버 핵심 로직
```

---

## 빌드 및 실행 방법

1. `A3-8190_HTTPServer.sln` 파일을 Visual Studio로 열기
2. **빌드 → 솔루션 빌드** (Ctrl+Shift+B)
3. **관리자 권한으로 실행** (app.manifest에 requireAdministrator 설정됨)
4. 포트 번호 확인 (기본 8080) 후 **"서버 시작"** 클릭
5. A3-8190 디바이스 웹 설정에서 **서버 주소 = 이 PC의 IP:포트** 입력

> **참고**: `http://+:PORT/` URL은 관리자 권한이 필요합니다.  
> 또는 아래 명령으로 URL ACL을 미리 등록할 수 있습니다:
> ```
> netsh http add urlacl url=http://+:8080/ user=EVERYONE
> ```

---

## 주요 기능

### 1. 서버 설정 (frmMain)
- 포트 번호 설정 및 HTTP 서버 시작/중지
- 연결된 디바이스 SN, 릴레이 상태, 도어 센서 상태 실시간 표시
- 서버 로그 실시간 출력

### 2. 원격 제어 (frmMain)
- 문 열기 / 상시 개방 / 문 닫기
- 디바이스 원격 재시작
- 명령은 큐에 저장 → 다음 Keepalive 수신 시 디바이스로 전달

### 3. 인원 관리 (frmPersonnel)
- 인원 추가 (UserID, 이름, 부서, 직위, 카드번호, 비밀번호, 권한)
- 인원 삭제 (삭제 목록에 저장 → 디바이스로 삭제 요청)
- "인원 추가 전송" / "인원 삭제 전송" 버튼으로 특정 SN 디바이스에 전송 예약

### 4. 출입 기록 (frmRecords)
- 디바이스에서 전송된 출입 기록 실시간 표시
- 입실/퇴실 색상 구분
- CSV 내보내기 (BOM UTF-8, 엑셀 호환)

---

## HTTP API 엔드포인트 (서버 구현 목록)

| Method | URL | 설명 |
|--------|-----|------|
| POST | `/Device/Keepalive` | 디바이스 하트비트 + 대기 명령 플래그 응답 |
| POST | `/People/DownloadPeopleList` | 인원 목록 다운로드 (디바이스가 요청) |
| POST | `/DevicePass/SelectDeleteInfo` | 삭제할 인원 목록 조회 |
| POST | `/Device/RemoteCommand` | 원격 명령 전달 (문열기, 재시작 등) |
| POST | `/Record/UploadIdentifyRecord` | 출입 기록 수신 (multipart/form-data) |
| POST | `/People/PushPeople` | 디바이스에서 인원 정보 업로드 수신 |
| POST | `/Record/UploadSystemRecord` | 시스템 기록 수신 |

---

## 통신 프로토콜 참고 문서

```
Communication Protocols/
├── HTTP protocol of Sony facial recognition terminal.md  ← 주요 HTTP API 문서
├── HTTPv2_API_Sequence_Diagrams_EN.html                  ← API 시퀀스 다이어그램
├── MQTT protocol interface document …pdf                 ← MQTT 대안 프로토콜
└── .NET/sample/DoNetDrive.Protocol.Fingerprint and Face.rar  ← .NET 샘플 (압축)
```

---

## 디바이스 설정 (A3-8190)

A3-8190 디바이스 웹 관리 페이지에서:

1. **Network → HTTP Server** 설정:
   - Server Address: `[이 PC의 IP]`
   - Server Port: `8080` (또는 설정한 포트)
   - HTTP Version: HTTP 1.1 또는 HTTP 2.0
2. **Keepalive Interval**: 15초 권장

---

*SmartLM Repository / Chunghyon/SmartLM*
