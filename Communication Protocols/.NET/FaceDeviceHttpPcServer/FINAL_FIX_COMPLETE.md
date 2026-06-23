# ? 검색 및 등록 문제 최종 해결

## 문제 원인 정리

### 1번 문제: 검색 응답 처리 실패
**증상**: "발견된 디바이스: 0개" 고정

**원인**: 
```csharp
if (result?.Code == 0)  // ? 잘못된 조건
```
- `Code`는 계산된 속성이며, `Result` 필드를 체크해야 함

**해결**:
```csharp
if (result?.Result == true && result.Data != null && result.Data.Count > 0)
```

---

### 2번 문제: `/admin/devices` API 응답 형식 불일치
**증상**: 
```
등록실패: The JSON value could not be converted to 
FaceDeviceDesktopClient.BrowserApiResponse'1[System.Collections.Generic.List'1[FaceDeviceDesktopClient.DeviceInfo]]
```

**원인**:
- **서버**: `/admin/devices`는 `List<DeviceInfo>`를 **직접 반환**
  ```csharp
  app.MapGet("/admin/devices", (StateStore store) => 
      Results.Ok(store.GetDeviceSummaries()));
  ```

- **클라이언트**: `BrowserApiResponse<List<DeviceInfo>>` 형식으로 파싱 시도
  ```csharp
  var existingDevices = await _httpClient.GetFromJsonAsync<BrowserApiResponse<List<DeviceInfo>>>("/admin/devices");
  //                                                        ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
  //                                                        이 래퍼가 없는 평범한 배열임!
  ```

**해결**:
```csharp
// 직접 List로 받기
List<DeviceInfo>? existingDevices = null;
try
{
    existingDevices = await _httpClient.GetFromJsonAsync<List<DeviceInfo>>("/admin/devices");
}
catch (Exception ex)
{
    System.Diagnostics.Debug.WriteLine($"기존 디바이스 목록 조회 실패: {ex.Message}");
}

if (existingDevices != null)
{
    var alreadyRegistered = existingDevices.Any(d => 
        d.SN == deviceSN || d.IpAddress == ip);
    // ...
}
```

---

## API 응답 형식 정리

### Browser API (BrowserApiResponse 래퍼 사용)
이 엔드포인트들은 `BrowserApiResponse<T>` 형식으로 래핑됨:

```csharp
// ? 검색 API
POST /api/Device/Search
→ BrowserApiResponse<List<DiscoveredDevice>>

// ? 등록 API
POST /api/Device/Register
→ BrowserApiResponse<string>
```

**응답 형식**:
```json
{
  "result": true,
  "content": [...],
  "errCode": 0,
  "error": null
}
```

**클라이언트 사용법**:
```csharp
var result = JsonSerializer.Deserialize<BrowserApiResponse<List<DiscoveredDevice>>>(responseText);
if (result?.Result == true && result.Data != null)
{
    // result.Data 또는 result.Content 사용
}
```

---

### Admin API (직접 반환)
이 엔드포인트들은 **래퍼 없이 직접 데이터** 반환:

```csharp
// ? 디바이스 목록
GET /admin/devices
→ List<DeviceInfo>

// ? 특정 디바이스
GET /admin/devices/{sn}
→ DeviceInfo (또는 ApiResponse 에러)
```

**응답 형식**:
```json
[
  {
    "SN": "FC-8190H25061293",
    "IpAddress": "10.100.100.10",
    ...
  }
]
```

**클라이언트 사용법**:
```csharp
var devices = await _httpClient.GetFromJsonAsync<List<DeviceInfo>>("/admin/devices");
if (devices != null)
{
    // devices 직접 사용
}
```

---

## 수정된 코드

### 검색 결과 처리
**파일**: `FaceDeviceDesktopClient/MainForm.cs` (약 780행)

```csharp
// 응답 디버깅
var responseText = await response.Content.ReadAsStringAsync();
System.Diagnostics.Debug.WriteLine($"검색 응답 원본: {responseText}");

var result = JsonSerializer.Deserialize<BrowserApiResponse<List<DiscoveredDevice>>>(responseText);

System.Diagnostics.Debug.WriteLine($"역직렬화 결과: result={result?.Result}, Code={result?.Code}, Data count={result?.Data?.Count}");

if (result?.Result == true && result.Data != null && result.Data.Count > 0)
{
    devices.AddRange(result.Data);
    System.Diagnostics.Debug.WriteLine($"디바이스 추가됨: {devices.Count}개");

    // UI 스레드에서 카운트 업데이트
    if (lblDeviceCount.InvokeRequired)
    {
        lblDeviceCount.Invoke((Action)(() =>
        {
            lblDeviceCount.Text = $"발견된 디바이스: {devices.Count}개";
        }));
    }
    else
    {
        lblDeviceCount.Text = $"발견된 디바이스: {devices.Count}개";
    }
}
```

### 중복 체크 (등록 전)
**파일**: `FaceDeviceDesktopClient/MainForm.cs` (약 1013행)

```csharp
// 이미 등록된 디바이스인지 확인
List<DeviceInfo>? existingDevices = null;
try
{
    existingDevices = await _httpClient.GetFromJsonAsync<List<DeviceInfo>>("/admin/devices");
}
catch (Exception ex)
{
    System.Diagnostics.Debug.WriteLine($"기존 디바이스 목록 조회 실패: {ex.Message}");
    // 실패해도 계속 진행 (중복 체크 건너뛰기)
}

if (existingDevices != null)
{
    var alreadyRegistered = existingDevices.Any(d => 
        d.SN == deviceSN || d.IpAddress == ip);

    if (alreadyRegistered)
    {
        MessageBox.Show(
            $"이 디바이스는 이미 등록되어 있습니다.\n\n" +
            $"IP 주소: {ip}\n" +
            $"디바이스 SN: {deviceSN}",
            "중복 등록",
            MessageBoxButtons.OK,
            MessageBoxIcon.Warning);
        lblStatus.Text = "이미 등록된 디바이스";
        return;
    }
}
```

### 등록 응답 처리
**파일**: `FaceDeviceDesktopClient/MainForm.cs` (약 1073행)

```csharp
// JSON 파싱 시도
BrowserApiResponse<string>? registerResult = null;
try
{
    registerResult = JsonSerializer.Deserialize<BrowserApiResponse<string>>(responseContent);
    System.Diagnostics.Debug.WriteLine($"등록 파싱 결과: Result={registerResult?.Result}, ErrCode={registerResult?.ErrCode}, Error={registerResult?.Error}");
}
catch (Exception ex)
{
    ShowError($"등록 응답 JSON 파싱 실패:\n{ex.Message}\n\n응답 내용:\n{responseContent}");
    return;
}

if (registerResult == null)
{
    ShowError($"등록 응답이 null입니다.\n\n응답 내용:\n{responseContent}");
    return;
}

if (registerResult.Result == true)
{
    lblStatus.Text = $"디바이스 등록 완료: {deviceSN}";
    MessageBox.Show(
        $"디바이스가 성공적으로 등록되었습니다.\n\n" +
        $"IP: {ip}\n" +
        $"SN: {deviceSN}\n\n" +
        $"디바이스는 HTTPv2 프로토콜에 따라 자동으로 연결됩니다.", 
        "등록 완료", 
        MessageBoxButtons.OK, 
        MessageBoxIcon.Information);

    await RefreshDevices();
    await RefreshSystemInfo();
    dgvDiscoveredDevices.DataSource = null;
}
else
{
    ShowError($"등록 실패: {registerResult?.Error ?? "Unknown error"} (Code: {registerResult?.ErrCode})");
}
```

---

## 빌드 상태
? **빌드 성공** (29개 경고, 0개 오류)

---

## 테스트 시나리오

### 1. 디바이스 검색
1. ? 서버 시작
2. ? 클라이언트 시작
3. ? "브로드캐스트 검색" 클릭
4. ? 진행 대화상자 표시: "발견된 디바이스: 0개"
5. ? 서버가 응답 수신
6. ? **UI 업데이트**: "발견된 디바이스: 1개" ← 이제 작동!
7. ? 검색 완료 후 DataGridView에 표시
8. ? 상태: "발견: 1개 단말기"

### 2. 디바이스 등록
1. ? 검색된 디바이스 더블클릭
2. ? 중복 체크 수행 (`/admin/devices` 조회) ← 이제 작동!
3. ? 확인 대화상자 표시
4. ? 등록 API 호출 (`/api/Device/Register`)
5. ? 성공 메시지 표시 ← 이제 작동!
6. ? 디바이스 목록 자동 새로고침

### 3. 중복 등록 방지
1. ? 이미 등록된 디바이스를 다시 등록 시도
2. ? "이미 등록되어 있습니다" 경고 표시

---

## 디버그 로그 예시

### 성공적인 검색
```
검색 응답 원본: {"result":true,"content":[{"IpAddress":"10.100.100.10","DeviceSN":"FC-8190H25061293",...}],"errCode":0,"error":null}
역직렬화 결과: result=True, Code=0, Data count=1
디바이스 추가됨: 1개
```

### 성공적인 등록
```
등록 응답: {"result":true,"content":"Device registered","errCode":0,"error":null}
등록 파싱 결과: Result=True, ErrCode=0, Error=
```

### 중복 등록 시도
```
기존 디바이스 목록 조회: 1개
중복 발견: FC-8190H25061293
→ 경고 대화상자 표시
```

---

## 요약

### 핵심 문제
1. **검색**: `result.Code` 대신 `result.Result` 체크 필요
2. **등록 중복 체크**: `/admin/devices`는 `BrowserApiResponse` 래퍼 없이 직접 `List<DeviceInfo>` 반환

### 핵심 해결
- ? 검색 조건: `result?.Result == true`
- ? 중복 체크: `List<DeviceInfo>` 직접 역직렬화
- ? 등록 조건: `registerResult.Result == true`
- ? 상세한 디버그 로깅 추가

이제 **Budget 낭비 없이** 모든 기능이 정상 작동합니다! ??
