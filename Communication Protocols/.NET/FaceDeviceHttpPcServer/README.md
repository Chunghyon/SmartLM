# FaceDeviceHttpPcServer

PC에서 실행할 수 있는 1단계 최소 HTTP 연동 서버 샘플입니다.

## 지원 범위
- `POST /Device/Keepalive`
- `POST /Device/UploadWorkSetting`
- `POST /Device/DownloadWorkSetting`
- `POST /Record/UploadIdentifyRecord`
- 운영 확인용 `/admin/*` 엔드포인트

## 실행
```bash
dotnet run --project "/tmp/workspace/Chunghyon/SmartLM/Communication Protocols/.NET/FaceDeviceHttpPcServer/FaceDeviceHttpPcServer.csproj"
```

기본 저장 위치는 프로젝트 내부 `App_Data` 폴더입니다.

## 최소 연동 절차
1. 단말이 `UploadWorkSetting`으로 현재 설정을 업로드합니다.
2. 단말이 `Keepalive`를 보내면 서버는 현재 대기 중인 플래그를 반환합니다.
3. PC 운영자가 `/admin/devices/{sn}/request-sync` 또는 `/admin/devices/{sn}/work-setting`으로 동기화를 요청합니다.
4. 다음 Keepalive 응답에서 `SyncParameter=1`이 내려가면 단말이 `DownloadWorkSetting`을 호출합니다.
5. 출입 이벤트가 발생하면 단말이 `UploadIdentifyRecord`로 기록과 사진을 업로드합니다.

## 운영 확인용 엔드포인트
- `GET /admin/devices`
- `GET /admin/devices/{sn}`
- `POST /admin/devices/{sn}/request-sync`
- `POST /admin/devices/{sn}/request-upload-work-setting`
- `PUT /admin/devices/{sn}/work-setting`
