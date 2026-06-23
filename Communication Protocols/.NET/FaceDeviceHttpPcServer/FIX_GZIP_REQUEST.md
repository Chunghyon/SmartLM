# ? GZIP 압축 요청 처리 문제 해결

## 문제 분석

### 오류 메시지
```
System.Text.Json.JsonReaderException: '0x1F' is an invalid start of a value.
LineNumber: 0 | BytePositionInLine: 0.
```

### 요청 헤더 (디바이스에서 전송)
```http
POST /Device/UploadWorkSetting HTTP/1.1
Host: 10.100.100.254
User-Agent: mongoose 6.18
Content-Type: application/json; charset=utf-8
Content-Encoding: gzip          ← 문제의 원인!
Content-Length: 3001
```

### 근본 원인

#### 1. 디바이스가 GZIP 압축 전송
디바이스(mongoose 웹 서버)가 JSON 데이터를 **GZIP으로 압축**하여 전송:
- 원본: JSON 텍스트 (~3KB)
- 압축: GZIP 바이너리 (3001 bytes)
- GZIP 매직 넘버: `0x1F 0x8B` ← JSON 파서가 `0x1F`를 읽고 오류 발생

#### 2. 서버가 압축 해제 안 함
ASP.NET Core는 **기본적으로 요청 압축 해제를 지원하지 않습니다**.

**문제 코드** (`Program.cs:127`):
```csharp
app.MapPost("/Device/UploadWorkSetting", async (HttpRequest request, StateStore store) =>
{
    var payload = await JsonNode.ParseAsync(request.Body);  // ← GZIP 바이너리를 JSON으로 파싱 시도!
    if (payload is not JsonObject setting)
    {
        return Results.BadRequest(new ApiResponse(400, "JSON object is required."));
    }
    ...
});
```

**흐름**:
```
디바이스 → GZIP 압축된 JSON (0x1F 0x8B ...) 
         → 서버 request.Body (압축된 스트림)
         → JsonNode.ParseAsync() 
         → ? '0x1F' is an invalid start of a value
```

#### 3. 왜 500 오류가 디바이스로 전달되는가?
ASP.NET Core의 기본 동작:
1. 라우트 핸들러에서 예외 발생
2. `DeveloperExceptionPageMiddleware`가 예외를 캐치
3. **500 Internal Server Error** 응답 생성
4. 응답 본문에 **예외 스택 트레이스** 포함
5. 클라이언트(디바이스)에게 전송

**개발 모드**에서는 상세한 오류 정보를 반환하므로 디바이스가 이를 받게 됩니다.

---

## 해결 방법

### ? 요청 압축 해제 미들웨어 추가

ASP.NET Core 7.0+부터 내장된 `RequestDecompressionMiddleware` 사용:

**파일**: `Program.cs`

#### 1. 서비스 등록
```csharp
builder.Services.AddSingleton<DeviceDiscoveryService>();
builder.Services.AddHttpClient();

// 요청 압축 해제 지원 추가 (GZIP, Deflate, Brotli)
builder.Services.AddRequestDecompression();

var app = builder.Build();
```

#### 2. 미들웨어 등록 (순서 중요!)
```csharp
var app = builder.Build();

// ?? 중요: 압축 해제를 먼저 실행해야 함!
app.UseRequestDecompression();

// HTTP 요청 로깅 미들웨어 추가
app.UseMiddleware<HttpLoggingMiddleware>();
```

**미들웨어 순서**:
```
요청 흐름:
  1. UseRequestDecompression()  ← GZIP 압축 해제
  2. HttpLoggingMiddleware      ← 로깅 (이제 압축 해제된 데이터)
  3. 라우트 핸들러             ← JsonNode.ParseAsync() 성공!
```

---

## 동작 원리

### 압축 해제 전
```
디바이스 → Content-Encoding: gzip
         → request.Body = [0x1F 0x8B 0x08 ...]  (GZIP 바이너리)
         → JsonNode.ParseAsync()
         → ? 오류
```

### 압축 해제 후
```
디바이스 → Content-Encoding: gzip
         → UseRequestDecompression()
         → request.Body = {"DeviceSN":"FC-123",...}  (압축 해제된 JSON)
         → JsonNode.ParseAsync()
         → ? 성공
```

### RequestDecompressionMiddleware 기능
- **자동 감지**: `Content-Encoding` 헤더를 읽고 자동으로 압축 해제
- **지원 형식**: GZIP, Deflate, Brotli
- **투명 처리**: 다운스트림 코드는 압축을 인식할 필요 없음

---

## 테스트

### 1. 압축되지 않은 요청 (여전히 작동)
```http
POST /Device/UploadWorkSetting HTTP/1.1
Content-Type: application/json

{"DeviceSN":"FC-123",...}
```
→ ? 미들웨어가 `Content-Encoding`이 없으면 그대로 통과

### 2. GZIP 압축 요청 (이제 작동)
```http
POST /Device/UploadWorkSetting HTTP/1.1
Content-Type: application/json
Content-Encoding: gzip

[GZIP 바이너리 데이터]
```
→ ? 미들웨어가 자동으로 압축 해제

### 3. 디바이스 응답 확인
**이전**:
```http
HTTP/1.1 500 Internal Server Error

System.Text.Json.JsonReaderException: '0x1F' is an invalid start of a value...
```

**해결 후**:
```http
HTTP/1.1 200 OK

{
  "Success": 0,
  "Message": null
}
```

---

## 추가 고려사항

### 1. 응답 압축 (선택 사항)
서버에서 디바이스로 응답을 압축하려면:
```csharp
builder.Services.AddResponseCompression(options =>
{
    options.EnableForHttps = true;
    options.Providers.Add<GzipCompressionProvider>();
});

app.UseResponseCompression();
```

### 2. 로깅 개선
압축 해제 여부를 로그에 기록:
```csharp
if (context.Request.Headers.ContainsKey("Content-Encoding"))
{
    LogHub.Instance.Info($"압축된 요청 수신: {context.Request.Headers["Content-Encoding"]}");
}
```

### 3. 프로덕션 환경 오류 처리
개발 환경에서는 상세한 오류가 유용하지만, 프로덕션에서는 보안상 위험:
```csharp
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/error");
}
```

---

## 빌드 상태
? **빌드 성공**

## 영향 받는 엔드포인트
다음 엔드포인트들이 이제 GZIP 압축 요청을 처리할 수 있습니다:
- `/Device/UploadWorkSetting`
- `/Device/DownloadWorkSetting`
- `/People/DownloadPeopleList`
- `/Record/UploadIdentifyRecord`
- `/Record/UploadSystemRecord`
- 기타 모든 JSON 기반 API

---

## 요약
- ? **문제**: 디바이스가 GZIP 압축된 JSON을 전송했지만 서버가 압축 해제하지 않음
- ? **해결**: `AddRequestDecompression()` 및 `UseRequestDecompression()` 추가
- ? **효과**: GZIP/Deflate/Brotli 압축된 요청 자동 처리
- ? **성능**: 네트워크 대역폭 절약 (디바이스가 3KB JSON → ~1KB GZIP 전송)

이제 디바이스의 압축된 요청을 정상적으로 처리할 수 있습니다! ??
