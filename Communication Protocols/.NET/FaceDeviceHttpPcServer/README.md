# FaceDeviceHttpPcServer

PC에서 실행할 수 있는 2단계 HTTP 연동 서버 샘플입니다.

## 지원 범위
- `POST /Device/Keepalive`
- `POST /Device/UploadWorkSetting`
- `POST /Device/DownloadWorkSetting`
- `POST /People/DownloadPeopleList`
- `POST /DevicePass/SelectPassInfo`
- `POST /DevicePass/SelectDeleteInfo`
- `POST /People/SelectDeleteInfo`
- `POST /Record/UploadIdentifyRecord`
- 운영 확인용 `/admin/*` 엔드포인트
- 간단한 관리자 웹 UI (`/admin`)

## 실행
```bash
cd "Communication Protocols/.NET/FaceDeviceHttpPcServer"
dotnet run
```

기본 저장 위치는 프로젝트 내부 `App_Data` 폴더입니다.

## 최소 연동 절차
1. 단말이 `UploadWorkSetting`으로 현재 설정을 업로드합니다.
2. 단말이 `Keepalive`를 보내면 서버는 현재 대기 중인 플래그를 반환합니다.
3. 관리자가 `/admin` 또는 `/admin/people`로 사람을 추가하면 다음 Keepalive에서 `AddPeople` 플래그가 내려갑니다.
4. 단말이 `People/DownloadPeopleList` 또는 `DevicePass/SelectPassInfo`로 전체 사람 목록을 가져갑니다.
5. 관리자가 사람을 삭제하면 다음 Keepalive에서 `DeletePeople` 플래그가 내려가고 단말이 `DevicePass/SelectDeleteInfo` 또는 `People/SelectDeleteInfo`를 호출합니다.
6. PC 운영자가 `/admin/devices/{sn}/request-sync` 또는 `/admin/devices/{sn}/work-setting`으로 설정 동기화를 요청할 수 있습니다.
7. 다음 Keepalive 응답에서 `SyncParameter=1`이 내려가면 단말이 `DownloadWorkSetting`을 호출합니다.
8. 출입 이벤트가 발생하면 단말이 `UploadIdentifyRecord`로 기록과 사진을 업로드합니다.

## 운영 확인용 엔드포인트
- `GET /admin`
- `GET /admin/devices`
- `GET /admin/people`
- `POST /admin/people`
- `DELETE /admin/people/{userId}`
- `POST /admin/devices/{sn}/request-add-people`
- `POST /admin/devices/{sn}/request-delete-people`
- `GET /admin/devices/{sn}`
- `POST /admin/devices/{sn}/request-sync`
- `POST /admin/devices/{sn}/request-upload-work-setting`
- `PUT /admin/devices/{sn}/work-setting`

## 관리자 웹 UI
- `/admin` 에 접속하면 디바이스 상태, 사람 목록, 사람 추가/삭제 기능을 브라우저에서 바로 확인할 수 있습니다.
- 사람 추가/삭제는 저장과 동시에 현재 알려진 디바이스에 대한 Add/Delete 플래그 예약까지 같이 처리합니다.
