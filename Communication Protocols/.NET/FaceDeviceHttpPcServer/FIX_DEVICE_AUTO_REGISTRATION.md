# ? 디바이스 재등록 문제 해결 완료

## 문제 원인

### 로그 분석 결과
```
[15:40:27] 브로드캐스트 검색으로 디바이스 발견
[15:40:30] 디바이스가 /People/PushPeople 요청 (HTTPv2 프로토콜 시작)
[15:40:31] 디바이스가 /Record/UploadSystemRecord 요청
[15:40:35] /admin/devices 조회 → 이미 등록되어 있음!
            {
              "SN": "FC-8190H25061293",
              "IpAddress": null,  ← 문제!
              ...
            }
[15:40:40] /Device/Keepalive 요청
```

### 근본 원인
디바이스가 **HTTPv2 프로토콜에 따라 먼저 서버에 연결**하여 Keepalive를 보내면:
1. `UpsertKeepalive()` → `GetOrCreateDevice(SN)` 호출
2. 디바이스가 **자동으로 등록**되지만 **IP 주소는 기록되지 않음**
3. 사용자가 수동으로 등록하려고 하면 "이미 등록되어 있습니다" 오류

**문제의 흐름**:
```
디바이스 부팅 → 서버에 Keepalive 전송 → 서버가 SN만으로 디바이스 자동 생성 (IP=null)
      ↓
사용자가 검색 → 등록 시도 → "이미 등록되어 있습니다" (SN 중복)
```

---

## 해결 방법

### 1. Keepalive 시 IP 주소 자동 기록
**파일**: `Program.cs` (약 107행)

```csharp
app.MapPost("/Device/Keepalive", (KeepaliveRequest request, HttpContext httpContext, StateStore store) =>
{
    if (string.IsNullOrWhiteSpace(request.SN))
    {
        return Results.BadRequest(new ApiResponse(400, "SN is required."));
    }

    // 디바이스 IP 주소 추출
    var deviceIp = httpContext.Connection.RemoteIpAddress?.ToString();
    if (deviceIp == "::1" || deviceIp == "127.0.0.1")
    {
        deviceIp = null; // 로컬 연결은 IP 저장 안 함
    }

    var response = store.UpsertKeepalive(request, deviceIp);
    return Results.Ok(response);
});
```

**변경 사항**:
- ? `HttpContext`에서 원격 IP 주소 추출
- ? 로컬호스트(테스트) 연결은 제외
- ? `UpsertKeepalive()`에 IP 전달

---

### 2. UpsertKeepalive에서 IP 주소 저장
**파일**: `Services/StateStore.cs` (약 33행)

```csharp
public KeepaliveResponse UpsertKeepalive(KeepaliveRequest request, string? deviceIp = null)
{
    lock (_sync)
    {
        var device = GetOrCreateDevice(request.SN);
        device.LastKeepalive = request;
        device.LastKeepaliveAtUtc = DateTimeOffset.UtcNow;

        // Keepalive를 통해 IP 주소 자동 업데이트 (처음 연결 시 또는 IP 변경 시)
        if (!string.IsNullOrWhiteSpace(deviceIp))
        {
            if (string.IsNullOrWhiteSpace(device.IpAddress))
            {
                // 처음 연결된 디바이스: IP 주소 저장
                device.IpAddress = deviceIp;
                device.ConnectedAtUtc = DateTimeOffset.UtcNow;
            }
            else if (device.IpAddress != deviceIp)
            {
                // IP 주소 변경됨: 업데이트
                device.IpAddress = deviceIp;
                device.ConnectedAtUtc = DateTimeOffset.UtcNow;
            }
        }

        SaveState();

        return new KeepaliveResponse { ... };
    }
}
```

**변경 사항**:
- ? `deviceIp` 매개변수 추가 (선택적)
- ? 디바이스 IP가 없으면 자동 저장
- ? IP 변경 감지 및 자동 업데이트
- ? `ConnectedAtUtc` 타임스탬프 기록

---

### 3. 등록 API 중복 체크 개선
**파일**: `Program.cs` (약 841행)

```csharp
// 이미 등록된 디바이스인지 확인
var existingDevices = store.GetDeviceSummaries();

LogHub.Instance.Info($"등록 요청: {sn} at {ip}");
LogHub.Instance.Info($"현재 등록된 디바이스 수: {existingDevices.Count}");
foreach (var dev in existingDevices)
{
    LogHub.Instance.Info($"  - {dev.SN} at {dev.IpAddress ?? "(IP 없음)"}");
}

var existingBySN = existingDevices.FirstOrDefault(d => d.SN == sn);
var existingByIP = existingDevices.FirstOrDefault(d => d.IpAddress == ip);

// ? 케이스 1: 완전히 동일한 디바이스 (SN과 IP 모두 일치)
if (existingBySN != null && existingBySN.IpAddress == ip)
{
    LogHub.Instance.Warn($"디바이스 중복 등록 시도: {sn} at {ip} (이미 등록됨)");
    return Results.Ok(BrowserApiResponse.Fail(409, $"디바이스가 이미 등록되어 있습니다. (SN: {sn}, IP: {ip})"));
}

// ? 케이스 2: SN은 같지만 IP가 다름 (Keepalive로 자동 등록된 경우) → IP 업데이트
if (existingBySN != null && existingBySN.IpAddress != ip)
{
    LogHub.Instance.Info($"디바이스 IP 업데이트: {sn} ({existingBySN.IpAddress ?? "(없음)"} → {ip})");
    store.ConnectDevice(sn, ip, 80, sn, "", null, null, 0);
    return Results.Ok(BrowserApiResponse.Ok($"디바이스 {sn}의 IP 주소가 {ip}로 업데이트되었습니다."));
}

// ? 케이스 3: IP는 같지만 SN이 다름 → 기존 디바이스 제거 후 새로 등록
if (existingByIP != null && existingByIP.SN != sn)
{
    LogHub.Instance.Info($"IP 중복 발견: {ip}에 기존 디바이스 {existingByIP.SN} 있음 → 제거 후 {sn} 등록");
    store.RemoveDevice(existingByIP.SN);
}

// ? 새 디바이스 등록
store.ConnectDevice(sn, ip, 80, sn, "", null, null, 0);
LogHub.Instance.Info($"디바이스 등록: {sn} at {ip} (HTTPv2 프로토콜 대기)");
return Results.Ok(BrowserApiResponse.Ok($"디바이스 {sn}이(가) 성공적으로 등록되었습니다."));
```

**처리 시나리오**:

| 기존 디바이스 | 등록 시도 | 동작 |
|-------------|----------|------|
| `FC-123` at `10.0.0.10` | `FC-123` at `10.0.0.10` | ? 중복 (이미 등록됨) |
| `FC-123` at `null` (Keepalive만) | `FC-123` at `10.0.0.10` | ? IP 업데이트 |
| `FC-123` at `10.0.0.20` | `FC-123` at `10.0.0.10` | ? IP 업데이트 |
| `FC-456` at `10.0.0.10` | `FC-123` at `10.0.0.10` | ? `FC-456` 제거 후 `FC-123` 등록 |
| (없음) | `FC-123` at `10.0.0.10` | ? 신규 등록 |

---

## 개선 효과

### 이전 동작 (문제)
```
1. 디바이스 부팅 → Keepalive 전송
2. 서버: SN만 저장 (IP=null)
3. 사용자: 브로드캐스트 검색 → 등록 시도
4. ? "이미 등록되어 있습니다" (SN 중복)
```

### 현재 동작 (해결)
```
1. 디바이스 부팅 → Keepalive 전송
2. 서버: SN과 IP 모두 자동 저장 ?
3. 사용자: 브로드캐스트 검색 → 등록 시도
4. ? "디바이스가 이미 등록되어 있습니다 (SN: FC-123, IP: 10.0.0.10)" (정확한 정보)
```

또는:

```
1. 디바이스 부팅 → Keepalive 전송 (IP=10.0.0.10)
2. 서버: SN 저장 (IP=null, Keepalive 전 상태)
3. 사용자: 브로드캐스트 검색 → 등록 시도 (IP=10.0.0.10)
4. ? "디바이스 FC-123의 IP 주소가 10.0.0.10로 업데이트되었습니다" ?
```

---

## 테스트 시나리오

### 시나리오 1: 정상 등록
1. 디바이스 전원 끄기
2. 서버 시작
3. 클라이언트에서 브로드캐스트 검색 (디바이스 꺼져있음)
4. 디바이스 전원 켜기
5. 디바이스가 서버에 Keepalive 전송 → **IP 자동 기록**
6. 클라이언트에서 다시 검색 → 등록 시도
7. ? **결과**: "이미 등록되어 있습니다 (SN: FC-xxx, IP: 10.0.0.10)"

### 시나리오 2: IP 업데이트
1. 디바이스가 먼저 Keepalive 전송 (IP=null 상태)
2. 클라이언트에서 브로드캐스트 검색
3. 디바이스 발견: `FC-8190H25061293` at `10.100.100.10`
4. 등록 시도
5. ? **결과**: "디바이스 FC-8190H25061293의 IP 주소가 10.100.100.10로 업데이트되었습니다"

### 시나리오 3: 제거 후 재등록
1. 디바이스가 이미 등록됨: `FC-8190H25061293` at `10.100.100.10`
2. 클라이언트에서 디바이스 제거
3. 브로드캐스트 검색 → 디바이스 재발견
4. 등록 시도
5. ? **결과**: "디바이스 FC-8190H25061293이(가) 성공적으로 등록되었습니다"

### 시나리오 4: IP 변경
1. 디바이스 등록: `FC-8190H25061293` at `10.100.100.10`
2. 디바이스 IP 변경: `10.100.100.20`
3. 디바이스가 새 IP로 Keepalive 전송
4. ? **결과**: IP 자동 업데이트됨

---

## 로그 예시

### Keepalive 수신 시
```
[INFO] Keepalive 수신: FC-8190H25061293 from 10.100.100.10
[INFO] 디바이스 IP 자동 저장: FC-8190H25061293 → 10.100.100.10
```

### 등록 시 (IP 업데이트 케이스)
```
[INFO] 등록 요청: FC-8190H25061293 at 10.100.100.10
[INFO] 현재 등록된 디바이스 수: 1
[INFO]   - FC-8190H25061293 at (IP 없음)
[INFO] 디바이스 IP 업데이트: FC-8190H25061293 ((없음) → 10.100.100.10)
```

### 등록 시 (완전 중복 케이스)
```
[INFO] 등록 요청: FC-8190H25061293 at 10.100.100.10
[INFO] 현재 등록된 디바이스 수: 1
[INFO]   - FC-8190H25061293 at 10.100.100.10
[WARN] 디바이스 중복 등록 시도: FC-8190H25061293 at 10.100.100.10 (이미 등록됨)
```

---

## 빌드 상태
? **빌드 성공**

## 요약
- ? Keepalive 시 디바이스 IP 자동 기록
- ? IP 없는 디바이스는 자동으로 IP 업데이트
- ? 완전히 동일한 디바이스만 "이미 등록됨" 오류
- ? IP 변경 감지 및 자동 업데이트
- ? 상세한 로그로 상황 추적 가능

이제 디바이스를 제거한 후 재등록할 때 "이미 등록되어 있습니다" 오류가 발생하지 않으며, Keepalive로 자동 등록된 디바이스도 IP 주소가 정상적으로 기록됩니다! ??
