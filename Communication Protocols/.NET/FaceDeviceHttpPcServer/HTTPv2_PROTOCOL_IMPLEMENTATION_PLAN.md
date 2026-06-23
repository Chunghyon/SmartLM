# HTTPv2 프로토콜 구현 계획

## 현재 문제점

### ? 잘못된 구현 (웹 브라우저 UI 프로토콜)
```
Desktop Client → 장치 (8080) → /personnel/new
Desktop Client → 장치 (8080) → /personnel/listRecord
Desktop Client → 장치 (8080) → /personnel/deleteAll
```

이는 **"Face recognition device browser UI interface document.md"** 프로토콜이며,  
웹 브라우저에서 직접 장치를 관리하는 방식입니다.

---

## ? 올바른 구현 (HTTPv2 Backend Integration Protocol)

### 흐름:
```
1. 장치 → 서버 (80) → POST /Device/Keepalive
   ← 서버: { "Success": 0, "AddPeople": 5, "DeletePeople": 2 }

2. 장치 → 서버 (80) → POST /People/DownloadPeopleList
   ← 서버: { "PeopleCount": 5, "PeopleList": [...] }

3. Desktop Client → 서버 (8100) → POST /api/People/New
   서버는 AddPeople 카운트 증가

4. 다음 Keepalive 시:
   장치 → 서버: POST /Device/Keepalive
   ← 서버: { "Success": 0, "AddPeople": 6 }

5. 장치가 AddPeople > 0 확인 → DownloadPeopleList 호출
```

---

## 수정 필요 항목

### 1. DeviceDetailForm.cs
- ? 제거: 직접 장치 HTTP 호출 (`/personnel/new`, `/personnel/listRecord`)
- ? 추가: 서버 API 호출 (`/admin/devices/{sn}/request-add-people`)

### 2. Program.cs 서버 API
- ? 이미 구현됨: `/Device/Keepalive` → `AddPeople`, `DeletePeople` 플래그
- ? 이미 구현됨: `/People/DownloadPeopleList`
- ? 누락: 명시적인 "사용자 추가/삭제 요청" Admin API

### 3. StateStore.cs
- ? 이미 구현됨: `PendingAddPeopleCount`
- ? 이미 구현됨: `PendingDeleteUserIds`
- ? 개선 필요: 장치별 pending 사용자 목록 관리

---

## 구현 단계

### Phase 1: 서버 Admin API 추가
```csharp
POST /admin/devices/{sn}/request-sync-people
→ 서버가 PendingAddPeopleCount 설정
→ 다음 Keepalive 시 장치가 DownloadPeopleList 호출
```

### Phase 2: DeviceDetailForm 수정
```csharp
BtnUpload_Click:
  - 사용자를 서버 DB에 저장
  - POST /admin/devices/{sn}/request-sync-people
  - "서버에 업로드 요청 완료. 장치가 다음 Keepalive 시 동기화됩니다."

BtnDownload_Click:
  - 서버에서 장치의 현재 사용자 목록 조회
  - 또는 장치에게 강제 업로드 요청
```

### Phase 3: 초기화 명령
```csharp
BtnInitialize_Click:
  - POST /admin/devices/{sn}/remote-command
  - { "DeleteAllPeople": 1 }
```

---

## HTTPv2 프로토콜 핵심

### Keepalive 응답 우선순위:
1. UploadWorkParameter (설정 업로드)
2. Remote (원격 제어)
3. SyncParameter (설정 동기화)
4. DeletePeople (사용자 삭제)
5. AddPeople (사용자 추가)

### 장치 → 서버 Pull 방식:
- 장치가 Keepalive로 "할 일" 확인
- 장치가 서버 API 호출하여 데이터 가져감
- 클라이언트는 서버에만 요청
