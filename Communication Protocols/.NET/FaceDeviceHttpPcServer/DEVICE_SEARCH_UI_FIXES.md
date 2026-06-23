# 디바이스 검색 및 등록 UI 개선 완료

## 수정 사항

### 1. 포트 칼럼 제거 ?
**문제**: 검색된 디바이스 목록에 불필요한 "포트" 칼럼이 표시되고, 잘못된 값(31 또는 0)이 나타남

**해결**:
- `SetupDiscoveredDevicesGrid()` 메서드에서 `HttpPort` 칼럼 완전 제거
- 대신 더 유용한 정보를 표시하도록 칼럼 구성:
  - **IP 주소**: 디바이스 IP
  - **시리얼넘버**: 디바이스 SN
  - **디바이스명**: 디바이스 이름

### 2. "0개 발견" 문제 해결 ?
**문제**: 서버에서 디바이스를 발견했음에도 검색 진행 대화상자에 "0개 발견"으로 고정되어 표시됨

**해결**:
- 검색 진행 대화상자에 **발견된 디바이스 카운트 라벨** 추가
- 서버에서 검색 결과를 받으면 실시간으로 UI 업데이트:
  ```csharp
  lblDeviceCount.Text = $"발견된 디바이스: {devices.Count}개";
  ```
- UI 스레드 호출 안전성 확보 (`InvokeRequired` 체크)

### 3. 중지 버튼 추가 ?
**문제**: 검색 진행 중 사용자가 중단할 방법이 없었음

**해결**:
- 검색 대화상자에 **"중지" 버튼** 추가
- `CancellationTokenSource`를 통해 검색 작업을 안전하게 취소
- 폼 닫기 이벤트와 연동하여 예외 처리

### 4. 등록 실패 문제 해결 ?
**문제**: "등록 실패: The JSON value could not be converted..." 오류 발생

**해결** (이전 커밋에서 이미 완료):
- 수동 JSON 파싱으로 변경:
  ```csharp
  string responseContent = await registerResponse.Content.ReadAsStringAsync();
  var registerResult = JsonSerializer.Deserialize<BrowserApiResponse<string>>(responseContent);
  ```
- 상세한 오류 메시지와 함께 디버깅 정보 출력
- `System.Text.Json.Serialization` 네임스페이스 import 추가

## 개선된 UI 흐름

### 검색 프로세스
1. 사용자가 "브로드캐스트 검색" 실행
2. 진행 대화상자 표시:
   - "단말기 검색 중... 잠시만 기다려주세요."
   - **"발견된 디바이스: X개"** (실시간 업데이트)
   - **[중지]** 버튼
3. 검색 완료 후 자동으로 대화상자 닫힘
4. 메인 화면의 DataGridView에 결과 표시:
   - IP 주소
   - 시리얼넘버
   - 디바이스명

### 등록 프로세스
1. 검색된 디바이스를 더블클릭 또는 "등록" 버튼 클릭
2. 중복 체크 (IP 및 시리얼넘버)
3. 확인 대화상자:
   ```
   디바이스를 등록하시겠습니까?

   IP 주소: 10.100.100.123
   디바이스 SN: ABC123456

   디바이스는 HTTPv2 프로토콜을 통해 이 서버(포트 80)와 통신합니다.
   ```
4. **IP 주소와 시리얼넘버만 저장** (포트 정보 제외)
5. 성공 메시지와 함께 디바이스 목록 자동 새로고침

## 기술적 세부사항

### DiscoveredDevice 모델 (간소화)
```csharp
public record DiscoveredDevice(
    string IpAddress,
    string DeviceSN,
    string DeviceName,
    string Model,
    string FirmwareVersion
);
```
- `HttpPort` 필드 제거됨
- HTTPv2 프로토콜에서는 디바이스가 서버의 **포트 80**으로 연결하므로 포트 정보 불필요

### 등록 API 엔드포인트
- **POST** `/api/Device/Register`
- 요청 본문:
  ```json
  {
    "IpAddress": "10.100.100.123",
    "DeviceSN": "ABC123456"
  }
  ```
- 응답: `BrowserApiResponse<string>` 형식
- 중복 체크: 서버에서 SN 및 IP로 기존 디바이스 확인 후 409 Conflict 반환

## 빌드 상태
? **빌드 성공** - 모든 컴파일 오류 해결됨

## 다음 테스트 항목
1. 브로드캐스트 검색 실행 → 디바이스 카운트가 실시간으로 업데이트되는지 확인
2. 검색 결과 그리드에 IP, 시리얼넘버, 디바이스명만 표시되는지 확인
3. 중지 버튼이 정상 작동하는지 확인
4. 디바이스 등록 시 JSON 파싱 오류 없이 성공하는지 확인
5. 중복 디바이스 등록 시도 시 경고 메시지가 표시되는지 확인
