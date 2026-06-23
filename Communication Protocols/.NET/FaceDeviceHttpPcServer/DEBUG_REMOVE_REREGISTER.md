# 디바이스 제거 후 재등록 문제 디버깅

## 문제 증상
디바이스를 "제거"한 후 다시 검색하여 "등록"하려고 하면:
```
"이 디바이스는 이미 등록되어 있습니다"
```
오류가 발생합니다.

## 가능한 원인
1. **서버 측 제거 미완료**: `StateStore.RemoveDevice()`가 실제로 디바이스를 제거하지 못함
2. **클라이언트 측 캐싱**: 클라이언트가 오래된 디바이스 목록을 캐시하고 있음
3. **타이밍 이슈**: 제거 요청과 등록 요청 사이에 상태 동기화 지연
4. **중복 체크 로직 오류**: 서버 또는 클라이언트의 중복 체크 로직 버그

## 코드 검증 결과

### 서버 측 코드
#### RemoveDevice (StateStore.cs:630)
```csharp
public bool RemoveDevice(string deviceSn)
{
    lock (_sync)
    {
        if (_state.Devices.Remove(deviceSn))
        {
            SaveState();
            return true;
        }
        return false;
    }
}
```
? **올바름**: 디바이스를 `_state.Devices` 딕셔너리에서 제거하고 즉시 `SaveState()` 호출

#### GetDeviceSummaries (StateStore.cs:303)
```csharp
public IReadOnlyCollection<DeviceSummary> GetDeviceSummaries()
{
    lock (_sync)
    {
        return _state.Devices.Values
            .OrderByDescending(device => device.LastKeepaliveAtUtc ?? DateTimeOffset.MinValue)
            .Select(device => new DeviceSummary { ... })
            .ToArray();
    }
}
```
? **올바름**: 현재 `_state.Devices`의 스냅샷을 반환

#### 등록 API 중복 체크 (Program.cs:822-830)
```csharp
var existingDevices = store.GetDeviceSummaries();
var alreadyRegistered = existingDevices.FirstOrDefault(d => d.SN == sn || d.IpAddress == ip);

if (alreadyRegistered != null)
{
    LogHub.Instance.Warn($"디바이스 중복 등록 시도: {sn} at {ip}...");
    return Results.Ok(BrowserApiResponse.Fail(409, "..."));
}
```
? **올바름**: SN 또는 IP로 중복 체크

### 클라이언트 측 코드
#### 중복 체크 (MainForm.cs:1013-1042)
```csharp
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

    if (alreadyRegistered)
    {
        MessageBox.Show("이 디바이스는 이미 등록되어 있습니다...");
        return;
    }
}
```
? **올바름**: 매번 서버에서 최신 목록 조회

## 추가된 디버그 로깅

### 서버 측 로깅

#### 디바이스 제거 시
```csharp
app.MapDelete("/admin/devices/{sn}", (string sn, StateStore store) =>
{
    LogHub.Instance.Info($"디바이스 제거 요청: {sn}");

    if (store.RemoveDevice(sn))
    {
        LogHub.Instance.Info($"디바이스 제거 완료: {sn}");

        // 제거 후 현재 상태 로그
        var remainingDevices = store.GetDeviceSummaries();
        LogHub.Instance.Info($"제거 후 남은 디바이스 수: {remainingDevices.Count}");
        foreach (var dev in remainingDevices)
        {
            LogHub.Instance.Info($"  - {dev.SN} at {dev.IpAddress}");
        }

        return Results.Ok(ApiResponse.Ok($"Device {sn} removed successfully"));
    }
    ...
});
```

#### 디바이스 등록 시
```csharp
app.MapPost("/api/Device/Register", (JsonNode? body, StateStore store) =>
{
    var existingDevices = store.GetDeviceSummaries();

    LogHub.Instance.Info($"등록 요청: {sn} at {ip}");
    LogHub.Instance.Info($"현재 등록된 디바이스 수: {existingDevices.Count}");
    foreach (var dev in existingDevices)
    {
        LogHub.Instance.Info($"  - {dev.SN} at {dev.IpAddress}");
    }

    var alreadyRegistered = existingDevices.FirstOrDefault(d => d.SN == sn || d.IpAddress == ip);
    ...
});
```

### 클라이언트 측 로깅

```csharp
List<DeviceInfo>? existingDevices = null;
try
{
    existingDevices = await _httpClient.GetFromJsonAsync<List<DeviceInfo>>("/admin/devices");
    System.Diagnostics.Debug.WriteLine($"클라이언트: 기존 디바이스 목록 조회 성공 ({existingDevices?.Count ?? 0}개)");
    if (existingDevices != null)
    {
        foreach (var dev in existingDevices)
        {
            System.Diagnostics.Debug.WriteLine($"  - {dev.SN} at {dev.IpAddress}");
        }
    }
}
catch (Exception ex)
{
    System.Diagnostics.Debug.WriteLine($"기존 디바이스 목록 조회 실패: {ex.Message}");
}

if (existingDevices != null)
{
    System.Diagnostics.Debug.WriteLine($"클라이언트: 중복 체크 - 등록 시도 중인 디바이스: {deviceSN} at {ip}");

    var duplicateBySN = existingDevices.FirstOrDefault(d => d.SN == deviceSN);
    var duplicateByIP = existingDevices.FirstOrDefault(d => d.IpAddress == ip);

    if (duplicateBySN != null)
    {
        System.Diagnostics.Debug.WriteLine($"클라이언트: SN 중복 발견: {duplicateBySN.SN} at {duplicateBySN.IpAddress}");
    }
    if (duplicateByIP != null)
    {
        System.Diagnostics.Debug.WriteLine($"클라이언트: IP 중복 발견: {duplicateByIP.SN} at {duplicateByIP.IpAddress}");
    }

    var alreadyRegistered = existingDevices.Any(d => 
        d.SN == deviceSN || d.IpAddress == ip);

    if (alreadyRegistered)
    {
        MessageBox.Show("이 디바이스는 이미 등록되어 있습니다...");
        lblStatus.Text = "이미 등록된 디바이스";
        return;
    }
    else
    {
        System.Diagnostics.Debug.WriteLine($"클라이언트: 중복 없음, 등록 진행");
    }
}
```

## 재현 시나리오

### 1단계: 디바이스 등록
1. 브로드캐스트 검색
2. 디바이스 발견: `FC-8190H25061293` at `10.100.100.10`
3. 등록 성공

**예상 로그**:
```
[서버] 등록 요청: FC-8190H25061293 at 10.100.100.10
[서버] 현재 등록된 디바이스 수: 0
[서버] 디바이스 등록: FC-8190H25061293 at 10.100.100.10 (HTTPv2 프로토콜 대기)
```

### 2단계: 디바이스 제거
1. 단말기 탭에서 디바이스 선택
2. "제거" 버튼 클릭
3. 확인

**예상 로그**:
```
[서버] 디바이스 제거 요청: FC-8190H25061293
[서버] 디바이스 제거 완료: FC-8190H25061293
[서버] 제거 후 남은 디바이스 수: 0
```

### 3단계: 재등록 시도
1. 브로드캐스트 검색
2. 디바이스 재발견: `FC-8190H25061293` at `10.100.100.10`
3. 등록 시도

**예상 로그 (문제 없을 경우)**:
```
[클라이언트] 기존 디바이스 목록 조회 성공 (0개)
[클라이언트] 중복 체크 - 등록 시도 중인 디바이스: FC-8190H25061293 at 10.100.100.10
[클라이언트] 중복 없음, 등록 진행
[서버] 등록 요청: FC-8190H25061293 at 10.100.100.10
[서버] 현재 등록된 디바이스 수: 0
[서버] 디바이스 등록: FC-8190H25061293 at 10.100.100.10
```

**예상 로그 (문제 있을 경우)**:
```
[클라이언트] 기존 디바이스 목록 조회 성공 (1개)
[클라이언트]   - FC-8190H25061293 at 10.100.100.10
[클라이언트] 중복 체크 - 등록 시도 중인 디바이스: FC-8190H25061293 at 10.100.100.10
[클라이언트] SN 중복 발견: FC-8190H25061293 at 10.100.100.10
[클라이언트] IP 중복 발견: FC-8190H25061293 at 10.100.100.10
→ "이 디바이스는 이미 등록되어 있습니다" 경고
```

또는:

```
[클라이언트] 기존 디바이스 목록 조회 성공 (0개)
[클라이언트] 중복 없음, 등록 진행
[서버] 등록 요청: FC-8190H25061293 at 10.100.100.10
[서버] 현재 등록된 디바이스 수: 1
[서버]   - FC-8190H25061293 at 10.100.100.10
[서버] 디바이스 중복 등록 시도: FC-8190H25061293 at 10.100.100.10
→ 서버에서 409 Conflict 응답
```

## 로그 확인 방법

### 서버 로그
서버 콘솔 출력 또는 `App_Data/logs` 폴더에서 확인:
```
[INFO] 디바이스 제거 요청: FC-8190H25061293
[INFO] 디바이스 제거 완료: FC-8190H25061293
[INFO] 제거 후 남은 디바이스 수: 0
...
[INFO] 등록 요청: FC-8190H25061293 at 10.100.100.10
[INFO] 현재 등록된 디바이스 수: ???
```

### 클라이언트 로그
Visual Studio Output 창 (Debug 모드):
```
클라이언트: 기존 디바이스 목록 조회 성공 (???개)
클라이언트: 중복 체크 - 등록 시도 중인 디바이스: FC-8190H25061293 at 10.100.100.10
...
```

## 빌드 상태
? **빌드 성공** (29개 경고, 0개 오류)

## 다음 단계
1. ? 프로그램 실행
2. ? 디바이스 등록
3. ? 디바이스 제거
4. ? 재등록 시도
5. ? **로그 확인** - 위 시나리오의 어느 단계에서 문제가 발생하는지 파악
6. ? 로그 내용을 바탕으로 정확한 원인 파악 및 수정

## 예상 해결 방법

### 케이스 A: 서버 제거가 실제로 실행되지 않음
**증상**: 제거 후 로그에서 "제거 후 남은 디바이스 수: 1"  
**원인**: `RemoveDevice()` 호출 실패 또는 상태 저장 실패  
**해결**: `StateStore.SaveState()` 검증, 파일 권한 확인

### 케이스 B: 클라이언트가 오래된 목록 캐시
**증상**: 클라이언트 로그에서 "기존 디바이스 목록 조회 성공 (1개)"이지만 서버 로그에서는 "0개"  
**원인**: HTTP 캐싱, 네트워크 지연  
**해결**: HTTP 헤더에 `Cache-Control: no-cache` 추가

### 케이스 C: 서버와 클라이언트 간 상태 불일치
**증상**: 클라이언트는 "0개", 서버는 "1개"  
**원인**: 타이밍 이슈, 제거 후 `SaveState()` 미완료  
**해결**: 제거 API에서 `SaveState()` 완료 대기

## 요약
디버그 로깅을 추가하여 문제의 정확한 위치를 파악할 수 있도록 했습니다. 이제 재현 시나리오를 실행하고 로그를 확인하면 원인을 정확히 찾을 수 있습니다.
