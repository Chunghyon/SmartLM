using System.IO.Compression;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using FaceDeviceHttpPcServer.Forms;
using FaceDeviceHttpPcServer.Middleware;
using FaceDeviceHttpPcServer.Models;
using FaceDeviceHttpPcServer.Services;
using FaceDeviceHttpPcServer.Data;
using Microsoft.EntityFrameworkCore;

const byte GzipMagicByte1 = 0x1F;
const byte GzipMagicByte2 = 0x8B;

// Windows Forms 초기화
Application.EnableVisualStyles();
Application.SetCompatibleTextRenderingDefault(false);
Application.SetHighDpiMode(HighDpiMode.SystemAware);

var builder = WebApplication.CreateBuilder(args);

// ContentRootPath를 현재 실행 디렉토리로 설정
var contentRoot = AppDomain.CurrentDomain.BaseDirectory;
builder.Environment.ContentRootPath = contentRoot;
builder.Configuration.SetBasePath(contentRoot);

// WebRootPath 설정
var webRoot = Path.Combine(contentRoot, "wwwroot");
if (Directory.Exists(webRoot))
{
    builder.Environment.WebRootPath = webRoot;
}
else
{
    // 개발 환경에서는 프로젝트 루트의 wwwroot 사용
    var projectRoot = Directory.GetCurrentDirectory();
    webRoot = Path.Combine(projectRoot, "wwwroot");
    if (Directory.Exists(webRoot))
    {
        builder.Environment.WebRootPath = webRoot;
    }
}

LogHub.Instance.Info($"ContentRootPath: {builder.Environment.ContentRootPath}");
LogHub.Instance.Info($"WebRootPath: {builder.Environment.WebRootPath}");
LogHub.Instance.Info($"wwwroot exists: {Directory.Exists(builder.Environment.WebRootPath)}");

builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.WriteIndented = true;
    options.SerializerOptions.PropertyNamingPolicy = null;
    options.SerializerOptions.DictionaryKeyPolicy = null;
    // Ignore null values and empty strings to reduce payload size
    options.SerializerOptions.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull;
});

var connectionString = builder.Configuration.GetConnectionString("Default")
    ?? throw new InvalidOperationException("Connection string 'Default' is not configured.");

builder.Services.AddDbContextFactory<FaceDeviceDbContext>(options =>
{
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
});

builder.Services.AddSingleton(sp =>
{
    var env = sp.GetRequiredService<IHostEnvironment>();
    var storagePath = ResolveConfiguredPath(builder.Configuration["StoragePath"], env.ContentRootPath,
        Path.Combine(env.ContentRootPath, "App_Data"));
    Directory.CreateDirectory(storagePath);
    var factory = sp.GetRequiredService<IDbContextFactory<FaceDeviceDbContext>>();
    return new MySqlStateStore(factory, storagePath);
});

builder.Services.AddSingleton(sp =>
{
    var env = sp.GetRequiredService<IHostEnvironment>();
    var storagePath = ResolveConfiguredPath(builder.Configuration["StoragePath"], env.ContentRootPath,
        Path.Combine(env.ContentRootPath, "App_Data"));
    var settingsPath = ResolveConfiguredPath(builder.Configuration["SettingsPath"], env.ContentRootPath,
        Path.Combine(storagePath, "FaceDeviceSettings.xml"));
    Directory.CreateDirectory(Path.GetDirectoryName(settingsPath)!);
    return new SystemSettingsStore(settingsPath, builder.Configuration);
});
builder.Services.AddHostedService<RecordRetentionCleanupService>();
builder.Services.AddSingleton<DeviceCommandTracker>();
builder.Services.AddSingleton<DeviceDiscoveryService>();
builder.Services.AddHttpClient();

// 요청 압축 해제 지원 추가 (GZIP, Deflate, Brotli)
builder.Services.AddRequestDecompression();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var factory = scope.ServiceProvider.GetRequiredService<IDbContextFactory<FaceDeviceDbContext>>();
    using var db = factory.CreateDbContext();
    db.Database.EnsureCreated();
}


// 요청 압축 해제 미들웨어 (HttpLoggingMiddleware보다 먼저 실행되어야 함)
app.UseRequestDecompression();

// HTTP 요청 로깅 미들웨어 추가
app.UseMiddleware<HttpLoggingMiddleware>();

// admin UI 파일을 런타임에 자동 생성 (wwwroot/admin 폴더가 비어있거나 없을 때)
AdminUiWriter.EnsureFiles(app.Environment.WebRootPath);

// charset=utf-8 포함 Content-Type 설정 (JS/HTML 한글 깨짐 방지)
var utf8ContentTypes = new Microsoft.AspNetCore.StaticFiles.FileExtensionContentTypeProvider();
utf8ContentTypes.Mappings[".js"]   = "application/javascript; charset=utf-8";
utf8ContentTypes.Mappings[".mjs"]  = "application/javascript; charset=utf-8";
utf8ContentTypes.Mappings[".html"] = "text/html; charset=utf-8";
utf8ContentTypes.Mappings[".css"]  = "text/css; charset=utf-8";
utf8ContentTypes.Mappings[".json"] = "application/json; charset=utf-8";

// 정적 파일 서비스 구성
app.UseDefaultFiles();
app.UseStaticFiles(new StaticFileOptions { ContentTypeProvider = utf8ContentTypes });

// 소스 wwwroot/admin 폴더를 추가 경로로 등록 (빌드 출력에 없는 경우 대비)
var extraAdminPaths = new[]
{
    Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "wwwroot", "admin"),
    Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "admin"),
};
foreach (var adminPath in extraAdminPaths)
{
    if (Directory.Exists(adminPath))
    {
        app.UseStaticFiles(new StaticFileOptions
        {
            FileProvider = new Microsoft.Extensions.FileProviders.PhysicalFileProvider(adminPath),
            RequestPath = "/admin",
            ContentTypeProvider = utf8ContentTypes
        });
        LogHub.Instance.Info($"Admin static files served from: {adminPath}");
        break;
    }
}

// Favicon 404 방지
app.MapGet("/favicon.ico", () => Results.File(Array.Empty<byte>(), "image/x-icon"));

app.MapGet("/api-info", () => Results.Ok(new
{
    name = "FaceDeviceHttpPcServer",
    purpose = "Phase-2 HTTP integration server for face-recognition terminals",
    endpoints = new[]
    {
        "/Device/Keepalive",
        "/Device/UploadWorkSetting",
        "/Device/DownloadWorkSetting",
        "/People/DownloadPeopleList",
        "/DevicePass/SelectPassInfo",
        "/DevicePass/SelectDeleteInfo",
        "/People/SelectDeleteInfo",
        "/Record/UploadIdentifyRecord",
        "/Record/UploadSystemRecord",
        "/admin",
        "/admin/devices",
        "/admin/people",
        "/admin/devices/{sn}",
        "/admin/devices/{sn}/request-add-people",
        "/admin/devices/{sn}/request-delete-people",
        "/admin/devices/{sn}/request-sync",
        "/admin/devices/{sn}/request-upload-work-setting",
        "/admin/devices/{sn}/work-setting"
    }
}));

app.MapPost("/Device/Keepalive", (KeepaliveRequest request, HttpContext httpContext, MySqlStateStore store) =>
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

    LogHub.Instance.Info($"[Keepalive] 수신: 단말기 {request.SN} (IP: {deviceIp})");

    var response = store.UpsertKeepalive(request, deviceIp);

    if (response.AddPeople.HasValue && response.AddPeople.Value > 0)
    {
        LogHub.Instance.Info($"[Keepalive] 응답: 단말기 {request.SN}에 AddPeople={response.AddPeople} 전송 -> 사용자 다운로드 대기 중");
    }

    return Results.Ok(response);
});

app.MapPost("/Device/UploadWorkSetting", async (HttpRequest request, MySqlStateStore store) =>
{
    var payload = await JsonNode.ParseAsync(request.Body);
    if (payload is not JsonObject setting)
    {
        return Results.BadRequest(new ApiResponse(400, "JSON object is required."));
    }

    var deviceSn = setting["DeviceSN"]?.GetValue<string>()
        ?? setting["SN"]?.GetValue<string>();

    if (string.IsNullOrWhiteSpace(deviceSn))
    {
        return Results.BadRequest(new ApiResponse(400, "DeviceSN or SN is required."));
    }

    store.SaveUploadedWorkSetting(deviceSn, setting);
    return Results.Ok(ApiResponse.Ok());
});

app.MapPost("/Device/DownloadWorkSetting", async (HttpRequest request, MySqlStateStore store) =>
{
    var payload = await JsonNode.ParseAsync(request.Body);
    var sn = payload?["SN"]?.GetValue<string>();
    if (string.IsNullOrWhiteSpace(sn))
    {
        return Results.BadRequest(new ApiResponse(400, "SN is required."));
    }

    var workSetting = store.GetWorkSettingForDownload(sn);
    if (workSetting is null)
    {
        return Results.Ok(new ApiResponse(404, "No work-setting snapshot available"));
    }

    // 단말기 프로토콜: WorkSetting 필드가 Success와 함께 최상위에 위치해야 함
    // { "Success": 1, "ReleaseTime": ..., "FreeOpen": ..., ... }
    workSetting["Success"] = 1;
    return Results.Ok(workSetting);
});

app.MapPost("/People/DownloadPeopleList", DownloadPeopleList);
app.MapPost("/DevicePass/SelectPassInfo", DownloadPeopleList);

app.MapPost("/DevicePass/SelectDeleteInfo", SelectDeleteInfo);
app.MapPost("/People/SelectDeleteInfo", SelectDeleteInfo);

app.MapPost("/Record/UploadIdentifyRecord", async (HttpRequest request, MySqlStateStore store) =>
{
    if (!request.HasFormContentType)
    {
        return Results.BadRequest(new ApiResponse(400, "multipart/form-data is required."));
    }

    var form = await request.ReadFormAsync();
    var sn = FirstNonEmpty(form["SN"].ToString(), form["DeviceSN"].ToString());
    if (string.IsNullOrWhiteSpace(sn))
    {
        return Results.BadRequest(new ApiResponse(400, "SN is required."));
    }

    var recordJson = FirstNonEmpty(
        form["RecordDetail"].ToString(),
        form["recordJson"].ToString());

    if (string.IsNullOrWhiteSpace(recordJson))
    {
        recordJson = await ReadMultipartValueAsync(form, "RecordDetail")
                     ?? await ReadMultipartValueAsync(form, "recordJson");
    }

    if (string.IsNullOrWhiteSpace(recordJson))
    {
        return Results.BadRequest(new ApiResponse(400, "RecordDetail or recordJson is required."));
    }

    JsonNode? recordNode;
    try
    {
        recordNode = JsonNode.Parse(recordJson);
    }
    catch (JsonException ex)
    {
        return Results.BadRequest(new ApiResponse(400, $"Invalid record JSON: {ex.Message}"));
    }

    var photo = form.Files.GetFile("Photo") ?? form.Files.GetFile("pic");
    store.SaveIdentifyRecord(sn, recordNode, photo);
    return Results.Ok(ApiResponse.Ok());
});

app.MapGet("/admin/people", (MySqlStateStore store) => Results.Ok(store.GetPeople()));

app.MapGet("/admin/people/device-assignments", (MySqlStateStore store) =>
{
    var assignments = store.GetDeviceAssignments();
    return Results.Ok(assignments);
});

// POST /admin/people/reload-from-files  →  people 폴더 JSON 파일에서 사용자 목록 재로드

app.MapPost("/admin/people/save-to-files", (JsonNode? body, MySqlStateStore store) =>
{
    var ids = body?["UserIds"]?.AsArray()
        ?.Select(n => n?.GetValue<string>() ?? "")
        .Where(id => id.Length > 0)
        .ToList() ?? new List<string>();
    var (saved, skipped, errors) = store.SavePeopleToFiles(ids);
    LogHub.Instance.Info($"[SaveToFiles] {saved}명 저장, {skipped}건 건너뜀, {errors}건 오류");
    return Results.Ok(new { saved, skipped, errors });
});

app.MapPost("/admin/people/reload-from-files", (MySqlStateStore store) =>
{
    var (loaded, skipped, errors) = store.ReloadPeopleFromFiles();
    LogHub.Instance.Info($"[ReloadFromFiles] people 폴더 로드 완료: {loaded}명 로드, {skipped}건 건너뜀, {errors}건 오류");
    return Results.Ok(new { loaded, skipped, errors });
});

// 사용자 Photo 필드가 Base64이면 직접 반환, 단말기 경로이면 온라인 단말기에서 프록시 다운로드
app.MapGet("/admin/people/{userId}/photo", (string userId, MySqlStateStore store) =>
{
    var person = store.GetPeople().FirstOrDefault(p =>
        string.Equals(p.UserID, userId, StringComparison.OrdinalIgnoreCase));
    if (person is null)
        return Results.NotFound(new ApiResponse(404, "Person not found."));

    var photo = person.Photo;
    if (string.IsNullOrWhiteSpace(photo))
        return Results.NotFound(new ApiResponse(404, "No photo."));

    // Base64 사진인 경우 바로 반환
    if (!photo.StartsWith("/") && !photo.Contains("\\"))
    {
        try
        {
            var bytes = Convert.FromBase64String(photo);
            return Results.File(bytes, "image/jpeg");
        }
        catch
        {
            return Results.NotFound(new ApiResponse(404, "Invalid photo data."));
        }
    }

    // 단말기 내부 경로(/data/...)는 단말기가 outbound HTTP 클라이언트 전용이므로
    // 서버에서 역방향 접속이 불가능함
    return Results.NotFound(new ApiResponse(404, "Device photo path is not accessible from server (device is outbound-only)."));
});

app.MapPost("/admin/people", (PersonInfo person, MySqlStateStore store) =>
{
    var normalized = NormalizePerson(person);
    if (string.IsNullOrWhiteSpace(normalized.UserID))
    {
        return Results.BadRequest(new ApiResponse(400, "UserID is required."));
    }

    return store.TryAddPerson(normalized)
        ? Results.Ok(ApiResponse.Ok($"Person {normalized.UserID} added and queued for device download."))
        : Results.Conflict(new ApiResponse(409, $"Person {normalized.UserID} already exists."));
});

app.MapDelete("/admin/people/{userId}", (string userId, MySqlStateStore store) =>
{
    var normalizedUserId = userId.Trim();
    if (string.IsNullOrWhiteSpace(normalizedUserId))
    {
        return Results.BadRequest(new ApiResponse(400, "UserID is required."));
    }

    return store.DeletePerson(normalizedUserId)
        ? Results.Ok(ApiResponse.Ok($"Person {normalizedUserId} deleted and queued for device removal."))
        : Results.NotFound(new ApiResponse(404, "Person not found."));
});

app.MapGet("/admin/devices", (MySqlStateStore store) => Results.Ok(store.GetDeviceSummaries()));

app.MapGet("/admin/devices/{sn}", (string sn, MySqlStateStore store) =>
{
    var device = store.GetDevice(sn);
    return device is null
        ? Results.NotFound(new ApiResponse(404, "Device not found."))
        : Results.Ok(device);
});

app.MapGet("/admin/devices/{sn}/work-setting", (string sn, MySqlStateStore store) =>
{
    var device = store.GetDevice(sn);
    if (device is null)
        return Results.NotFound(new ApiResponse(404, "Device not found."));
    var ws = device.DesiredWorkSetting ?? device.LastUploadedWorkSetting;
    return ws is null
        ? Results.NotFound(new ApiResponse(404, "No work-setting available."))
        : Results.Ok(ws);
});

// 단말기 내부 경로의 사진을 서버가 프록시로 다운로드하여 반환
// GET /admin/devices/{sn}/photo?path=/data/user_pic/xxx.jpg
app.MapGet("/admin/devices/{sn}/photo", async (string sn, string path, MySqlStateStore store, IHttpClientFactory httpClientFactory) =>
{
    var device = store.GetDevice(sn);
    if (device is null)
        return Results.NotFound(new ApiResponse(404, "Device not found."));
    if (string.IsNullOrWhiteSpace(path))
        return Results.BadRequest(new ApiResponse(400, "path is required."));

    // 단말기 IP:Port로 HTTP GET 요청
    var ip   = device.IpAddress;
    var port = device.HttpPort > 0 ? device.HttpPort : 80;
    if (string.IsNullOrWhiteSpace(ip))
        return Results.NotFound(new ApiResponse(404, "Device IP not available."));

    try
    {
        var client = httpClientFactory.CreateClient();
        client.Timeout = TimeSpan.FromSeconds(5);
        var url = $"http://{ip}:{port}{path}";
        var bytes = await client.GetByteArrayAsync(url);
        // 확장자로 Content-Type 결정
        var ext = System.IO.Path.GetExtension(path).ToLowerInvariant();
        var mime = ext switch
        {
            ".jpg" or ".jpeg" => "image/jpeg",
            ".png"            => "image/png",
            ".bmp"            => "image/bmp",
            _                 => "application/octet-stream"
        };
        return Results.File(bytes, mime);
    }
    catch (Exception ex)
    {
        return Results.NotFound(new ApiResponse(404, $"Failed to fetch photo from device: {ex.Message}"));
    }
});

app.MapPost("/admin/devices/{sn}/request-add-people", (string sn, MySqlStateStore store) =>
{
    var count = store.MarkAddPeopleRequested(sn);
    LogHub.Instance.Info($"[클라이언트 요청] 단말기 {sn}에 사용자 전송 요청 -> 대기 중인 사용자: {count}명");
    return Results.Ok(ApiResponse.Ok($"AddPeople={count} will be returned on the next keepalive for {sn}."));
});

app.MapPost("/admin/people/fix-timegroup", (MySqlStateStore store) =>
{
    var count = store.FixTimegroupForAllPeople();
    LogHub.Instance.Info($"[관리자] {count}명의 사용자 Timegroup을 1로 수정 완료");
    return Results.Ok(ApiResponse.Ok($"Fixed Timegroup for {count} people."));
});

app.MapPost("/admin/devices/{sn}/request-delete-people", (string sn, MySqlStateStore store) =>
{
    var count = store.MarkDeletePeopleRequested(sn);
    return Results.Ok(ApiResponse.Ok(
        count > 0
            ? $"DeletePeople={count} will be returned on the next keepalive for {sn}."
            : $"There are no pending deletions for {sn}."));
});

app.MapPost("/admin/devices/{sn}/request-sync", (string sn, MySqlStateStore store) =>
{
    store.MarkSyncRequested(sn);
    return Results.Ok(ApiResponse.Ok($"SyncParameter will be returned on the next keepalive for {sn}."));
});

app.MapPost("/admin/devices/{sn}/reset-pending", (string sn, MySqlStateStore store) =>
{
    var device = store.GetDevice(sn);
    if (device == null)
        return Results.NotFound(new ApiResponse(404, "Device not found"));

    store.ResetPendingState(sn);
    LogHub.Instance.Info($"[관리자] 단말기 {sn}의 pending 상태 초기화 완료");
    return Results.Ok(ApiResponse.Ok($"Reset pending state for device {sn}."));
});

app.MapPost("/admin/devices/{sn}/remote-command", (string sn, JsonNode? body, MySqlStateStore store, DeviceCommandTracker tracker) =>
{
    try
    {
        var commandType = body?["CommandType"]?.GetValue<string>()?.ToLower();

        if (string.IsNullOrWhiteSpace(commandType))
            return Results.BadRequest(new ApiResponse(400, "CommandType is required"));

        var job = tracker.Start(sn, "Remote", $"원격 명령 대기 중: {commandType}");

        switch (commandType)
        {
            case "restart":
                store.QueueRemoteCommand(sn, restart: true);
                LogHub.Instance.Info($"Remote command queued: Restart device {sn}");
                return Results.Ok(ApiResponseWithContent.Ok(new { JobId = job.Id, Message = "Restart command queued" }));

            case "opendoor":
                store.QueueRemoteCommand(sn, opendoor: true);
                LogHub.Instance.Info($"Remote command queued: Open door on {sn}");
                return Results.Ok(ApiResponseWithContent.Ok(new { JobId = job.Id, Message = "Open door command queued" }));

            case "closealarm":
                store.QueueRemoteCommand(sn, closealarm: true);
                LogHub.Instance.Info($"Remote command queued: Close alarm on {sn}");
                return Results.Ok(ApiResponseWithContent.Ok(new { JobId = job.Id, Message = "Close alarm command queued" }));

            case "pushallpeople":
                var peopleCount = store.MarkAddPeopleRequested(sn);
                var addJob = tracker.Start(sn, "AddPeople", $"{peopleCount}명 배포 대기 중");
                LogHub.Instance.Info($"Remote command queued: Push all people to {sn} ({peopleCount} people)");
                return Results.Ok(ApiResponseWithContent.Ok(new { JobId = addJob.Id, Message = $"Push all people command queued ({peopleCount} people)" }));

            case "deleteallpeople":
                var deletedCount = store.DeleteAllPeople(sn);
                var delJob = tracker.Start(sn, "DeletePeople", $"{deletedCount}명 삭제 대기 중");
                LogHub.Instance.Info($"Remote command: Delete all people from {sn} ({deletedCount} people marked for deletion)");
                return Results.Ok(ApiResponseWithContent.Ok(new { JobId = delJob.Id, Message = $"Delete all people command queued ({deletedCount} people)" }));

            case "clearrecords":
                store.QueueRemoteCommand(sn, clearRecord: true);
                LogHub.Instance.Info($"Remote command queued: Clear records on {sn}");
                return Results.Ok(ApiResponseWithContent.Ok(new { JobId = job.Id, Message = "Clear records command queued" }));

            case "repostrecord":
                store.QueueRemoteCommand(sn, repostRecord: true);
                LogHub.Instance.Info($"Remote command queued: Repost records from {sn}");
                return Results.Ok(ApiResponseWithContent.Ok(new { JobId = job.Id, Message = "Repost records command queued" }));

            case "synctime":
                var nowTs = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
                store.QueueSyncTime(sn, nowTs);
                LogHub.Instance.Info($"[시간동기화] 단말기 {sn}에 시간 동기화 명령 예약 (Unix={nowTs})");
                return Results.Ok(ApiResponse.Ok($"SetTime command queued (ts={nowTs})"));

            default:
                return Results.BadRequest(new ApiResponse(400, $"Unknown command type: {commandType}"));
        }
    }
    catch (Exception ex)
    {
        LogHub.Instance.Error($"Failed to queue remote command for {sn}: {ex.Message}");
        return Results.Ok(new ApiResponse(500, $"Failed to queue command: {ex.Message}"));
    }
});

app.MapPost("/admin/devices/{sn}/update-info", (string sn, JsonNode? body, MySqlStateStore store) =>
{
    var deviceName = body?["DeviceName"]?.GetValue<string>();
    var tagName    = body?["TagName"]?.GetValue<string>();
    var device = store.GetDevice(sn);
    if (device is null)
        return Results.NotFound(new ApiResponse(404, $"Device {sn} not found"));
    store.UpdateDeviceInfo(sn, deviceName, tagName);
    return Results.Ok(new ApiResponse(200, "OK"));
});

app.MapDelete("/admin/devices/{sn}", (string sn, MySqlStateStore store) =>
{
    try
    {
        LogHub.Instance.Info($"디바이스 제거 요청: {sn}");

        if (store.RemoveDevice(sn))
        {
            LogHub.Instance.Info($"디바이스 제거 완료: {sn}");

            return Results.Ok(ApiResponse.Ok($"Device {sn} removed successfully"));
        }
        else
        {
            LogHub.Instance.Warn($"디바이스 제거 실패: {sn} (찾을 수 없음)");
            return Results.NotFound(new ApiResponse(404, "Device not found"));
        }
    }
    catch (Exception ex)
    {
        LogHub.Instance.Error($"Failed to remove device {sn}: {ex.Message}");
        return Results.Ok(new ApiResponse(500, $"Failed to remove device: {ex.Message}"));
    }
});

app.MapPost("/admin/devices/{sn}/request-upload-work-setting", (string sn, MySqlStateStore store) =>
{
    store.MarkUploadWorkSettingRequested(sn);
    return Results.Ok(ApiResponse.Ok($"UploadWorkParameter will be returned on the next keepalive for {sn}."));
});

app.MapPut("/admin/devices/{sn}/work-setting", async (string sn, HttpRequest request, MySqlStateStore store) =>
{
    var payload = await JsonNode.ParseAsync(request.Body);
    if (payload is not JsonObject workSetting)
    {
        return Results.BadRequest(new ApiResponse(400, "JSON object is required."));
    }

    workSetting["DeviceSN"] = sn;
    store.SetDesiredWorkSetting(sn, workSetting);
    return Results.Ok(ApiResponse.Ok("Desired work-setting snapshot saved."));
});

// ── Admin: remote command dispatch ────────────────────────────────────────────
app.MapPost("/admin/devices/{sn}/remote", (string sn, DeviceRemoteRequest cmd, MySqlStateStore store) =>
{
    store.SetPendingRemoteCommand(sn, new PendingRemoteCommand
    {
        Opendoor = cmd.Opendoor,
        Restart = cmd.Restart,
        Recover = cmd.Recover,
        Closealarm = cmd.Closealarm
    });
    return Results.Ok(ApiResponse.Ok($"Remote command queued for {sn}."));
});

// ── Admin: system-info (dashboard summary) ───────────────────────────────────
app.MapGet("/admin/system-info", (MySqlStateStore store) =>
{
    var devs    = store.GetDeviceSummaries();
    var people  = store.GetPeople();
    var totalRecords = devs.Sum(d => d.RecordCount);
    var cutoff = DateTimeOffset.UtcNow.AddMinutes(-5);
    var onlineCount  = devs.Count(d => d.LastKeepaliveAtUtc.HasValue && d.LastKeepaliveAtUtc.Value >= cutoff);
    var months = app.Services.GetRequiredService<SystemSettingsStore>().Get().RecordRetentionMonths;
    return Results.Ok(new
    {
        TotalDevices   = devs.Count,
        OnlineDevices  = onlineCount,
        TotalPeople    = people.Count,
        TotalRecords   = totalRecords,
        RecordRetentionMonths = months
    });
});

app.MapGet("/admin/settings", (SystemSettingsStore settings) =>
{
    var s = settings.Get();
    return Results.Ok(new
    {
        s.ServerUrl,
        s.RecordRetentionMonths,
        SettingsPath = settings.SettingsFilePath,
        LocalUrls = settings.GetLocalServerUrls()
    });
});

app.MapPost("/admin/settings", (JsonNode? body, SystemSettingsStore settings) =>
{
    int? months = body?["RecordRetentionMonths"]?.GetValue<int>();
    var url = body?["ServerUrl"]?.GetValue<string>();
    var saved = settings.Save(months, url);
    LogHub.Instance.Info($"[Settings] URL={saved.ServerUrl}, 보관기간={saved.RecordRetentionMonths}개월, file={settings.SettingsFilePath}");
    return Results.Ok(new
    {
        saved.ServerUrl,
        saved.RecordRetentionMonths,
        SettingsPath = settings.SettingsFilePath,
        LocalUrls = settings.GetLocalServerUrls()
    });
});

// ── Admin: departments ────────────────────────────────────────────────────────
app.MapGet("/admin/departments", (MySqlStateStore store) => Results.Ok(store.GetDepartments()));

app.MapPost("/admin/departments", (DepartmentInfo dept, MySqlStateStore store) =>
{
    if (string.IsNullOrWhiteSpace(dept.DepartmentID))
        return Results.BadRequest(new ApiResponse(400, "DepartmentID is required."));
    return store.TryAddDepartment(dept)
        ? Results.Ok(ApiResponse.Ok($"Department {dept.DepartmentID} added."))
        : Results.Conflict(new ApiResponse(409, $"Department {dept.DepartmentID} already exists."));
});

app.MapDelete("/admin/departments/{id}", (string id, MySqlStateStore store) =>
    store.DeleteDepartment(id)
        ? Results.Ok(ApiResponse.Ok($"Department {id} deleted."))
        : Results.NotFound(new ApiResponse(404, "Department not found.")));

// ── Admin: 단말기→서버 전체 사용자 Pull 요청 ─────────────────────────────────────
// 단말기에게 PushAllPeople 원격 명령 전송 → 단말기가 /People/PushPeople 로 모든 사용자 Push
app.MapPost("/admin/devices/{sn}/pull-all-people", (string sn, MySqlStateStore store, DeviceCommandTracker tracker) =>
{
    var device = store.GetDevice(sn);
    if (device is null)
        return Results.NotFound(new ApiResponse(404, "Device not found."));

    store.QueueRemoteCommand(sn, pushAllPeople: true);
    tracker.MarkOwnedQueryReset(sn);
    tracker.MarkImportToServer(sn);
    var job = tracker.Start(sn, "Remote", "단말기 사용자 가져오기 대기 중");
    LogHub.Instance.Info($"[Pull People] 단말기 {sn}에게 PushAllPeople 명령 전송 -> 서버 사용자로 가져오기");
    return Results.Ok(ApiResponseWithContent.Ok(new { JobId = job.Id }));
});

app.MapPost("/admin/devices/{sn}/query-owned-people", (string sn, MySqlStateStore store, DeviceCommandTracker tracker) =>
{
    var device = store.GetDevice(sn);
    if (device is null)
        return Results.NotFound(new ApiResponse(404, "Device not found."));
    store.QueueRemoteCommand(sn, pushAllPeople: true);
    tracker.MarkOwnedQueryReset(sn);
    var job = tracker.Start(sn, "Remote", "단말기 사용자 목록 조회 대기 중");
    LogHub.Instance.Info($"[Query Owned] 단말기 {sn}에게 PushAllPeople 명령 전송 -> 단말기 목록만 조회 (서버 사용자 덮어쓰지 않음)");
    return Results.Ok(ApiResponseWithContent.Ok(new { JobId = job.Id }));
});

// ── Admin: 단말기→서버 Photo 가져오기 (단말기 경로 → Base64 변환 저장) ──────────────
app.MapPost("/admin/devices/{sn}/fetch-photo", async (string sn, JsonNode? body, MySqlStateStore store, IHttpClientFactory httpClientFactory) =>
{
    var userId = body?["UserID"]?.GetValue<string>();
    var photoPath = body?["PhotoPath"]?.GetValue<string>();

    if (string.IsNullOrWhiteSpace(userId))
        return Results.BadRequest(new ApiResponse(400, "UserID is required."));

    var device = store.GetDevice(sn);
    if (device is null)
        return Results.NotFound(new ApiResponse(404, "Device not found."));

    if (string.IsNullOrWhiteSpace(photoPath))
    {
        var people = store.GetPeople();
        var person = people.FirstOrDefault(p => string.Equals(p.UserID, userId, StringComparison.OrdinalIgnoreCase));
        if (person is null)
            return Results.NotFound(new ApiResponse(404, "Person not found."));
        photoPath = person.Photo;
    }

    if (string.IsNullOrWhiteSpace(photoPath) || !photoPath.StartsWith("/"))
        return Results.BadRequest(new ApiResponse(400, "No device photo path to fetch."));

    var ip   = device.IpAddress;
    var port = device.HttpPort > 0 ? device.HttpPort : 80;
    if (string.IsNullOrWhiteSpace(ip))
        return Results.NotFound(new ApiResponse(404, "Device IP not available."));

    try
    {
        var client = httpClientFactory.CreateClient();
        client.Timeout = TimeSpan.FromSeconds(10);
        var url = $"http://{ip}:{port}{photoPath}";
        var bytes = await client.GetByteArrayAsync(url);
        var base64 = Convert.ToBase64String(bytes);
        store.UpdatePersonPhoto(userId, base64);
        LogHub.Instance.Info($"[FetchPhoto] 단말기 {sn}에서 사용자 {userId} 사진 다운로드 완료 ({bytes.Length} bytes)");
        return Results.Ok(ApiResponse.Ok($"Photo fetched and saved for user {userId} ({bytes.Length} bytes)."));
    }
    catch (Exception ex)
    {
        LogHub.Instance.Error($"[FetchPhoto] 단말기 {sn}에서 사용자 {userId} 사진 다운로드 실패: {ex.Message}");
        return Results.Ok(new ApiResponse(500, $"Failed to fetch photo: {ex.Message}"));
    }
});

// ── Admin: 사용자 데이터 내보내기 (개별) ─────────────────────────────────────────
app.MapGet("/admin/people/{userId}/export", (string userId, MySqlStateStore store) =>
{
    var json = store.ExportPersonJson(userId);
    if (json is null)
        return Results.NotFound(new ApiResponse(404, "Person not found."));

    var bytes = Encoding.UTF8.GetBytes(json);
    return Results.File(bytes, "application/json", $"person_{userId}.json");
});

// ── Admin: 전체 사용자 데이터 내보내기 (JSON 배열) ────────────────────────────────
app.MapGet("/admin/people/export-all", (MySqlStateStore store) =>
{
    var people = store.GetPeople();
    var options = new JsonSerializerOptions { WriteIndented = true, PropertyNamingPolicy = null };
    var json = JsonSerializer.Serialize(people, options);
    var bytes = Encoding.UTF8.GetBytes(json);
    var timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
    return Results.File(bytes, "application/json", $"people_export_{timestamp}.json");
});

// ── Admin: 사용자 데이터를 특정 단말기로 배포 ────────────────────────────────────
// POST /admin/people/{userId}/distribute  body: { "TargetSN": "SN001" }
app.MapPost("/admin/people/{userId}/distribute", (string userId, JsonNode? body, MySqlStateStore store) =>
{
    var targetSn = body?["TargetSN"]?.GetValue<string>();
    if (string.IsNullOrWhiteSpace(targetSn))
        return Results.BadRequest(new ApiResponse(400, "TargetSN is required."));

    var people = store.GetPeople();
    var person = people.FirstOrDefault(p => string.Equals(p.UserID, userId, StringComparison.OrdinalIgnoreCase));
    if (person is null)
        return Results.NotFound(new ApiResponse(404, "Person not found."));

    var device = store.GetDevice(targetSn);
    if (device is null)
        return Results.NotFound(new ApiResponse(404, $"Target device {targetSn} not found."));

    var count = store.MarkAddPeopleRequested(targetSn);
    LogHub.Instance.Info($"[Distribute] 사용자 {userId}를 단말기 {targetSn}으로 전달 예약 (총 {count}명 전송 예정)");
    return Results.Ok(ApiResponse.Ok($"User {userId} will be sent to device {targetSn} on next keepalive (total {count} pending)."));
});

// ── Admin: 전체 사용자를 특정 단말기로 배포 ──────────────────────────────────────
// POST /admin/people/distribute-all  body: { "TargetSN": "SN001" }
app.MapPost("/admin/people/distribute-all", (JsonNode? body, MySqlStateStore store) =>
{
    var targetSn = body?["TargetSN"]?.GetValue<string>();
    if (string.IsNullOrWhiteSpace(targetSn))
        return Results.BadRequest(new ApiResponse(400, "TargetSN is required."));

    var device = store.GetDevice(targetSn);
    if (device is null)
        return Results.NotFound(new ApiResponse(404, $"Target device {targetSn} not found."));

    var count = store.MarkAddPeopleRequested(targetSn);
    LogHub.Instance.Info($"[Distribute-All] 전체 {count}명을 단말기 {targetSn}으로 전달 예약");
    return Results.Ok(ApiResponse.Ok($"All {count} users queued for delivery to device {targetSn}."));
});

// ── Admin: 복수 단말기에 전체 사용자 배포 ────────────────────────────────────────
// POST /admin/people/distribute-to-devices  body: { "TargetSNs": ["SN001","SN002"] }
app.MapPost("/admin/people/distribute-to-devices", (JsonNode? body, MySqlStateStore store, DeviceCommandTracker tracker) =>
{
    var snArray = body?["TargetSNs"]?.AsArray();
    if (snArray is null || snArray.Count == 0)
        return Results.BadRequest(new ApiResponse(400, "TargetSNs array is required."));

    // PersonIds가 전달된 경우 해당 사용자만, 없으면 전체
    var personIdsNode = body?["PersonIds"]?.AsArray();
    var allPeople = store.GetPeople();
    HashSet<string>? personIdFilter = null;
    if (personIdsNode != null && personIdsNode.Count > 0)
        personIdFilter = personIdsNode.Select(n => n?.GetValue<string>() ?? "").Where(s => s.Length > 0).ToHashSet();
    var serverPeople = personIdFilter != null
        ? allPeople.Where(p => personIdFilter.Contains(p.UserID)).ToList()
        : allPeople;

    var results = new List<object>();
    foreach (var snNode in snArray)
    {
        var sn = snNode?.GetValue<string>();
        if (string.IsNullOrWhiteSpace(sn)) continue;

        var device = store.GetDevice(sn);
        if (device is null)
        {
            results.Add(new { SN = sn, Success = false, Message = "Device not found." });
            continue;
        }

        store.StageServerPeopleForDevice(sn, serverPeople);
        var count = store.MarkAddPeopleRequested(sn);
        var job = tracker.Start(sn, "AddPeople", $"{count}명 배포 대기 중");
        LogHub.Instance.Info($"[Distribute-Multi] {count}명을 단말기 {sn}으로 전달 예약 (stage={serverPeople.Count()}명)");
        results.Add(new { SN = sn, Success = true, PendingCount = count, JobId = job.Id });
    }

    return Results.Ok(ApiResponseWithContent.Ok(results));
});

// ── Admin: 단말기별 사용자 목록 조회 (단말기에 실제 등록된 사용자) ─────────────────
app.MapGet("/admin/devices/{sn}/people", (string sn, MySqlStateStore store) =>
{
    var device = store.GetDevice(sn);
    if (device is null)
        return Results.NotFound(new ApiResponse(404, "Device not found."));
    return Results.Ok(store.GetDeviceOwnedPeople(sn));
});

// ── Admin: 단말기별 사용자 추가/수정 ─────────────────────────────────────────────
app.MapPost("/admin/devices/{sn}/people", (string sn, PersonInfo person, MySqlStateStore store) =>
{
    if (string.IsNullOrWhiteSpace(person.UserID))
        return Results.BadRequest(new ApiResponse(400, "UserID is required."));
    var device = store.GetDevice(sn);
    if (device is null)
        return Results.NotFound(new ApiResponse(404, "Device not found."));
    store.UpsertDeviceOwnedPerson(sn, person);
    LogHub.Instance.Info($"[DevicePeople] 단말기 {sn}: 사용자 {person.UserID} 추가/수정 (단말기 전용)");
    return Results.Ok(ApiResponse.Ok($"User {person.UserID} staged for device {sn}."));
});

// ── Admin: 단말기별 사용자 삭제 ───────────────────────────────────────────────────
app.MapDelete("/admin/devices/{sn}/people/{userId}", (string sn, string userId, MySqlStateStore store) =>
{
    var device = store.GetDevice(sn);
    if (device is null)
        return Results.NotFound(new ApiResponse(404, "Device not found."));
    store.DeleteDeviceOwnedPerson(sn, userId);
    LogHub.Instance.Info($"[DevicePeople] 단말기 {sn}: 사용자 {userId} 삭제 명령 예약");
    return Results.Ok(ApiResponse.Ok($"User {userId} queued for deletion from device {sn}."));
});

// ???????????????????????????????????????????????????????????????????????????????
// HTTP-Docking Protocol  (Device → Server)
// ???????????????????????????????????????????????????????????????????????????????

// ── /Device/RemoteCommand ─────────────────────────────────────────────────────
app.MapPost("/Device/RemoteCommand", (RemoteCommandRequest request, MySqlStateStore store) =>
{
    if (string.IsNullOrWhiteSpace(request.SN))
        return Results.BadRequest(new ApiResponse(400, "SN is required."));

    var cmd = store.ConsumeRemoteCommand(request.SN);
    if (cmd is null)
        return Results.Ok(new RemoteCommandResponse());

    if (cmd.PushAllPeople == 1)
        app.Services.GetRequiredService<DeviceCommandTracker>().MarkOwnedQueryReset(request.SN);

    app.Services.GetRequiredService<DeviceCommandTracker>()
        .CompleteLatest(request.SN, "Remote", true, "원격 명령이 단말기에 전달되었습니다.");

    return Results.Ok(new RemoteCommandResponse
    {
        Restart = cmd.Restart,
        Recover = cmd.Recover,
        Opendoor = cmd.Opendoor,
        Closealarm = cmd.Closealarm,
        RepostRecord = cmd.RepostRecord,
        PushAllPeople = cmd.PushAllPeople,
        QueryPeople = cmd.QueryPeople,
        ClearRecord = cmd.ClearRecord,
        SetTime = cmd.SetTime
    });
});


app.MapGet("/admin/command-jobs/{id}", (string id, DeviceCommandTracker tracker) =>
{
    var job = tracker.Get(id);
    return job is null ? Results.NotFound(new ApiResponse(404, "Job not found.")) : Results.Ok(job);
});

// ── /People/DownloadPeopleListResult ─────────────────────────────────────────
app.MapPost("/People/DownloadPeopleListResult", (DownloadPeopleListResultRequest request, MySqlStateStore store) =>
{
    if (string.IsNullOrWhiteSpace(request.SN))
    {
        return Results.BadRequest(new ApiResponse(400, "SN is required."));
    }

    if (request.FailCount > 0)
    {
        LogHub.Instance.Warn($"[결과] 다운로드 실패: 단말기 {request.SN} - 성공 {request.SuccessCount}명, 실패 {request.FailCount}명");

        if (request.FailList != null && request.FailList.Count > 0)
        {
            foreach (var fail in request.FailList.Take(3))
            {
                LogHub.Instance.Warn($"  실패: UserID={fail.UserID}, 에러={fail.ErrMsg}");
            }
        }
    }
    else if (request.SuccessCount > 0)
    {
        LogHub.Instance.Info($"[결과] 다운로드 성공: 단말기 {request.SN}에 {request.SuccessCount}명 정상 저장 완료!");
    }

    var tracker = app.Services.GetRequiredService<DeviceCommandTracker>();
    if (request.FailCount > 0)
    {
        var msg = request.FailList != null && request.FailList.Count > 0
            ? string.Join("\n", request.FailList.Select(f => $"사용자 {f.UserID}: {f.ErrMsg} (ErrorCode {f.ErrorCode})"))
            : $"다운로드 실패 {request.FailCount}명";
        tracker.CompleteLatest(request.SN, "AddPeople", false, msg);
    }
    else
    {
        tracker.CompleteLatest(request.SN, "AddPeople", true,
            $"단말기 {request.SN}에 {request.SuccessCount}명 저장 완료");
    }

    // Clear pending count only after device confirms successful save
    store.ConfirmPeopleDownloaded(request.SN, request.SuccessCount);

    return Results.Ok(ApiResponse.Ok());
});

// ── /People/DeletePeopleList ──────────────────────────────────────────────────
app.MapPost("/People/DeletePeopleList", (DeletePeopleListRequest request, MySqlStateStore store) =>
{
    if (string.IsNullOrWhiteSpace(request.SN))
        return Results.BadRequest(new ApiResponse(400, "SN is required."));

    var effectiveLimit = request.Limit <= 0 ? 50 : Math.Min(request.Limit, 1000);
    var list = store.GetDeletePeople(request.SN).Take(effectiveLimit).ToList();

    // Protocol: Stop cycle when Success=0 OR (Success=1 AND DeleteList is empty)
    // Use Success=0 when list is empty to ensure device stops polling
    var response = new DeletePeopleListResponse 
    { 
        Success = list.Count > 0 ? 1 : 0,  // Stop cycle with Success=0 when empty
        DeleteList = list 
    };

    return Results.Ok(response);
});

// ── /People/DeletePeopleListResult ───────────────────────────────────────────
app.MapPost("/People/DeletePeopleListResult", (DeletePeopleListResultRequest request, MySqlStateStore store, DeviceCommandTracker tracker) =>
{
    if (string.IsNullOrWhiteSpace(request.SN))
        return Results.BadRequest(new ApiResponse(400, "SN is required."));
    var ok = request.FailCount <= 0;
    tracker.CompleteLatest(request.SN, "DeletePeople", ok,
        ok ? $"단말기 {request.SN}에서 {request.SuccessCount}명 삭제 완료"
           : $"삭제 실패 {request.FailCount}명 (성공 {request.SuccessCount}명)");
    return Results.Ok(ApiResponse.Ok());
});

// ── /People/PushPeople  (device uploads its stored people to server) ──────────
app.MapPost("/People/PushPeople", async (HttpRequest httpRequest, MySqlStateStore store, IHttpClientFactory httpClientFactory) =>
{
    List<PersonInfo>? people = null;
    string? sn = null;
    int pushType = 0;

    if (httpRequest.HasFormContentType)
    {
        var form = await httpRequest.ReadFormAsync();
        sn = FirstNonEmpty(form["SN"].ToString(), form["DeviceSN"].ToString());

        // Try to get PushType
        if (int.TryParse(form["PushType"].ToString(), out var pt))
            pushType = pt;

        // The protocol specifies "Detail" field contains the PersonInfo JSON
        var detailJson = await ReadMultipartValueAsync(form, "Detail");
        if (string.IsNullOrWhiteSpace(detailJson))
            detailJson = form["Detail"].ToString();

        // Also try legacy field names
        if (string.IsNullOrWhiteSpace(detailJson))
            detailJson = form["PeopleJson"].ToString();

        if (!string.IsNullOrWhiteSpace(detailJson))
        {
            // Log the raw Detail JSON to see what the device sends
            LogHub.Instance.Info($"[PushPeople] Detail JSON 받음: {detailJson}");

            try
            {
                // Try to parse as single PersonInfo
                var person = System.Text.Json.JsonSerializer.Deserialize<PersonInfo>(detailJson);
                if (person != null)
                    people = new List<PersonInfo> { person };
            }
            catch
            {
                // Try to parse as array
                people = System.Text.Json.JsonSerializer.Deserialize<List<PersonInfo>>(detailJson);
            }
        }

        // Photo 파일이 multipart로 함께 전송된 경우 base64로 변환하여 사용자 Photo에 설정
        // (단말기 내부 경로 /data/... 를 실제 바이트로 대체)
        var photoFile = form.Files["Photo"];
        if (photoFile != null && photoFile.Length > 0 && people != null && people.Count > 0)
        {
            using var ms = new MemoryStream();
            await photoFile.CopyToAsync(ms);
            var photoBytes = ms.ToArray();
            var photoBase64 = Convert.ToBase64String(photoBytes);
            var photoMd5 = Convert.ToHexString(System.Security.Cryptography.MD5.HashData(photoBytes));
            foreach (var p in people)
            {
                p.Photo = photoBase64;
                p.PhotoLen = photoBytes.Length;
                p.PhotoMD5 = photoMd5;
            }
        }

        // If Detail is not provided but UserID is (common for Delete operations)
        if ((people == null || people.Count == 0) && !string.IsNullOrWhiteSpace(form["UserID"].ToString()))
        {
            var userId = form["UserID"].ToString();
            people = new List<PersonInfo> { new PersonInfo { UserID = userId } };
            LogHub.Instance.Info($"[PushPeople] Detail 없음, UserID 필드 사용: {userId}");
        }
    }
    else
    {
        var payload = await JsonNode.ParseAsync(httpRequest.Body);
        sn = payload?["SN"]?.GetValue<string>();
        pushType = payload?["PushType"]?.GetValue<int>() ?? 0;
        var listNode = payload?["PeopleList"];
        if (listNode is not null)
            people = System.Text.Json.JsonSerializer.Deserialize<List<PersonInfo>>(listNode.ToJsonString());

        // If PeopleList is not provided but UserID is (common for Delete operations)
        if ((people == null || people.Count == 0) && payload?["UserID"] != null)
        {
            var userId = payload["UserID"]?.GetValue<string>();
            if (!string.IsNullOrWhiteSpace(userId))
            {
                people = new List<PersonInfo> { new PersonInfo { UserID = userId } };
                LogHub.Instance.Info($"[PushPeople] PeopleList 없음, UserID 필드 사용: {userId}");
            }
        }
    }

    if (string.IsNullOrWhiteSpace(sn))
    {
        return Results.BadRequest(new ApiResponse(400, "SN is required."));
    }

    // Handle different PushType operations
    // PushType: 1=Add New, 2=Update, 3=Delete, 4=Query
    int success = 0, fail = 0;
    string operation = pushType switch
    {
        1 => "Add New",
        2 => "Update",
        3 => "Delete",
        4 => "Query",
        _ => "Unknown"
    };

    // Only log if device is pushing non-zero people (suppress routine empty keepalive-driven pushes)
    if (people != null && people.Count > 0)
    {
        LogHub.Instance.Info($"[PushPeople] 단말기 {sn}에서 {people.Count}명 업로드 (PushType={pushType} - {operation})");
        foreach (var person in people.Take(3))
        {
            LogHub.Instance.Info($"  - UserID={person.UserID}, Name={person.Name}, AccessType={person.AccessType}");

            // Log detailed field analysis to compare with download format
            LogHub.Instance.Info($"  [비교용] Password={person.Password ?? "null"}, " +
                $"CardNum={person.CardNum ?? "null"}, QRCode={person.QRCode ?? "null"}, " +
                $"OpenTimes={person.OpenTimes}, Timegroup={person.Timegroup}, " +
                $"ExpirationDate={person.ExpirationDate}, PhotoLen={person.PhotoLen}");
        }
    }

    switch (pushType)
    {
        case 1: // Add New
            (success, fail) = store.SavePushedPeople(sn, people ?? new(), addOnly: true);
            break;

        case 2: // Update
            (success, fail) = store.SavePushedPeople(sn, people ?? new(), addOnly: false);
            break;

        case 3: // Delete
            (success, fail) = store.DeletePushedPeople(sn, people ?? new());
            LogHub.Instance.Info($"[PushPeople-Delete] 단말기 {sn}에서 {success}명 삭제 처리 완료");
            break;

        case 4: // Query - 단말기 현재 사용자 전체 목록으로 OwnedPeople 전면 교체
        {
            var tracker = app.Services.GetRequiredService<DeviceCommandTracker>();
            if (tracker.ConsumeOwnedQueryReset(sn))
                store.BeginOwnedPeopleQuery(sn);
            var import = tracker.ConsumeImportToServer(sn);
            var (s, f, photoPathsToFetch) = store.ReplaceDeviceOwnedPeople(sn, people ?? new(), updateServerPeople: import);
            success = s; fail = f;
            LogHub.Instance.Info($"[PushPeople-Query] 단말기 {sn}에서 {success}명 조회 → OwnedPeople 동기화 완료");

            // 사진이 단말기 내부 경로인 경우 백그라운드에서 자동 다운로드
            if (import && photoPathsToFetch.Count > 0)
            {
                var device = store.GetDevice(sn);
                var ip = device?.IpAddress;
                var port = device?.HttpPort > 0 ? device.HttpPort : 80;
                if (!string.IsNullOrWhiteSpace(ip))
                {
                    _ = Task.Run(async () =>
                    {
                        var client = httpClientFactory.CreateClient();
                        client.Timeout = TimeSpan.FromSeconds(15);
                        int photoOk = 0, photoFail = 0;
                        foreach (var (userId, photoPath) in photoPathsToFetch)
                        {
                            try
                            {
                                var url = $"http://{ip}:{port}{photoPath}";
                                var bytes = await client.GetByteArrayAsync(url);
                                if (bytes.Length > 0)
                                {
                                    var base64 = Convert.ToBase64String(bytes);
                                    store.UpdatePersonPhoto(userId, base64);
                                    photoOk++;
                                }
                            }
                            catch (Exception ex)
                            {
                                LogHub.Instance.Warn($"[PushPeople-Query] 사용자 {userId} 사진 다운로드 실패: {ex.Message}");
                                photoFail++;
                            }
                        }
                        LogHub.Instance.Info($"[PushPeople-Query] 사진 자동 다운로드 완료: 성공={photoOk}, 실패={photoFail}");
                        if (photoOk > 0)
                            LogHub.Instance.NotifyPeopleListChanged();
                    });
                }
            }
            break;
        }

        default: // Unknown or 0 - treat as Update for backward compatibility
            (success, fail) = store.SavePushedPeople(sn, people ?? new(), addOnly: false);
            break;
    }

    // Notify UI to refresh the personnel list
    if (success > 0 && pushType != 4)
    {
        LogHub.Instance.NotifyPeopleListChanged();
    }

    return Results.Ok(ApiResponse.Ok($"PushType={operation}: {success} succeeded, {fail} failed."));
});

// ── /Record/UploadSystemRecord ────────────────────────────────────────────────
app.MapPost("/Record/UploadSystemRecord", (UploadSystemRecordRequest request, MySqlStateStore store) =>
{
    if (string.IsNullOrWhiteSpace(request.SN))
        return Results.BadRequest(new ApiResponse(400, "SN is required."));

    store.SaveSystemRecords(request.SN, request.RecordType, request.Records ?? new());
    return Results.Ok(ApiResponse.Ok());
});

// ???????????????????????????????????????????????????????????????????????????????
// Browser UI Protocol  (Browser → Server acting as device proxy)
// ???????????????????????????????????????????????????????????????????????????????

// ── /api/heartBeat ─────────────────────────────────────────────────────────────
app.MapGet("/api/heartBeat", () => Results.Ok(BrowserApiResponse.Ok("OK")));

// ── /api/GetDeviceSN ──────────────────────────────────────────────────────────
app.MapGet("/api/GetDeviceSN", (MySqlStateStore store) =>
{
    var devices = store.GetDeviceSummaries();
    var sn = devices.FirstOrDefault()?.SN ?? "UNKNOWN";
    return Results.Ok(BrowserApiResponse.Ok(sn));
});

// ── /api/User/Login ───────────────────────────────────────────────────────────
{
    const string DefaultPassword = "123456";
    var tokens = new System.Collections.Concurrent.ConcurrentDictionary<string, DateTimeOffset>(StringComparer.Ordinal);

    app.MapPost("/api/User/Login", (LoginRequest req) =>
    {
        if (req.password != DefaultPassword)
            return Results.Ok(BrowserApiResponse.Fail(1, "password incorrect"));
        var token = Guid.NewGuid().ToString("N");
        tokens[token] = DateTimeOffset.UtcNow.AddHours(24);
        return Results.Ok(BrowserApiResponse.Ok(token));
    });

    app.MapGet("/api/User/Logout", (HttpContext ctx) =>
    {
        var token = ExtractBearer(ctx);
        if (token is not null) tokens.TryRemove(token, out _);
        return Results.Ok(BrowserApiResponse.Ok("ok"));
    });

    app.MapGet("/api/User/CheckLoginToken", (HttpContext ctx) =>
    {
        var token = ExtractBearer(ctx);
        if (token is null || !tokens.TryGetValue(token, out var exp) || exp < DateTimeOffset.UtcNow)
            return Results.Ok(BrowserApiResponse.Fail(10000, "Token is invalid"));
        return Results.Ok(BrowserApiResponse.Ok(exp.ToUnixTimeSeconds()));
    });

    app.MapGet("/api/User/TokenExtension", (HttpContext ctx) =>
    {
        var token = ExtractBearer(ctx);
        if (token is null || !tokens.TryGetValue(token, out var exp) || exp < DateTimeOffset.UtcNow)
            return Results.Ok(BrowserApiResponse.Fail(10000, "Token is invalid"));
        var newExp = DateTimeOffset.UtcNow.AddHours(24);
        tokens[token] = newExp;
        return Results.Ok(BrowserApiResponse.Ok(newExp.ToUnixTimeSeconds()));
    });

    app.MapPost("/api/User/EditPassword", (JsonNode? body) =>
        Results.Ok(BrowserApiResponse.Ok()));
}

// ── /api/Device/FunctionList ──────────────────────────────────────────────────
app.MapGet("/api/Device/FunctionList", () => Results.Ok(BrowserApiResponse.Ok(new
{
    FaceIR = true, BodyTemperature = false, Elevator = true,
    FaceMask = true, AlarmClock = true, ExcelFile = true, ZipFile = true,
    TimeGreoup = true, WIFI = true,
    HTTPClient_V1 = true, HTTPClient_V2 = true, MQTT = true,
    Websocket_V1 = true, Websocket_V2 = true
})));

// ── /api/Device/GetNetworkInterfaces ──────────────────────────────────────────
app.MapGet("/api/Device/GetNetworkInterfaces", (DeviceDiscoveryService discoveryService) =>
{
    try
    {
        var interfaces = discoveryService.GetValidNetworkInterfaces();
        var result = interfaces.Select(i => new NetworkInterfaceInfo
        {
            LocalIp = i.localIp.ToString(),
            BroadcastIp = i.broadcastIp.ToString()
        }).ToList();

        return Results.Ok(BrowserApiResponse.Ok(result));
    }
    catch (Exception ex)
    {
        return Results.Ok(BrowserApiResponse.Fail(500, $"Failed to get network interfaces: {ex.Message}"));
    }
});

// ── /api/Device/Search ────────────────────────────────────────────────────────
app.MapPost("/api/Device/Search", async (DeviceDiscoveryService discoveryService, JsonNode? body) =>
{
    try
    {
        var searchType = body?["SearchType"]?.GetValue<string>() ?? "broadcast";
        var subnet = body?["Subnet"]?.GetValue<string>();
        var localIpAddress = body?["LocalIpAddress"]?.GetValue<string>();
        var discoveryPort = body?["DiscoveryPort"]?.GetValue<int>() ?? 8101;

        List<DeviceDiscoveryService.DiscoveredDevice> devices;

        if (searchType.Equals("scan", StringComparison.OrdinalIgnoreCase) && !string.IsNullOrWhiteSpace(subnet))
        {
            // IP 범위 스캔
            devices = await discoveryService.ScanNetworkAsync(subnet);
        }
        else
        {
            // UDP 브로드캐스트 (선택된 로컬 IP 사용)
            devices = await discoveryService.SearchDevicesAsync(localIpAddress, discoveryPort);
        }

        return Results.Ok(BrowserApiResponse.Ok(devices));
    }
    catch (Exception ex)
    {
        return Results.Ok(BrowserApiResponse.Fail(500, $"Search failed: {ex.Message}"));
    }
});

// ── /api/Device/SearchStream ──────────────────────────────────────────────────
app.MapPost("/api/Device/SearchStream", async (DeviceDiscoveryService discoveryService, JsonNode? body, HttpContext context) =>
{
    try
    {
        var searchType = body?["SearchType"]?.GetValue<string>() ?? "broadcast";
        var subnet = body?["Subnet"]?.GetValue<string>();

        context.Response.ContentType = "text/event-stream";
        context.Response.Headers.CacheControl = "no-cache";
        context.Response.Headers.Connection = "keep-alive";

        await context.Response.Body.FlushAsync();

        if (searchType.Equals("scan", StringComparison.OrdinalIgnoreCase) && !string.IsNullOrWhiteSpace(subnet))
        {
            // IP 범위 스캔 - 실시간 스트리밍 (progress + device)
            await foreach (var item in discoveryService.ScanNetworkStreamAsync(subnet, context.RequestAborted))
            {
                // 동적 객체에서 type 속성 확인
                var itemType = item.GetType();
                var typeProperty = itemType.GetProperty("type");
                if (typeProperty != null)
                {
                    var type = typeProperty.GetValue(item) as string;

                    if (type == "progress")
                    {
                        var scannedProperty = itemType.GetProperty("scanned");
                        var scanned = scannedProperty?.GetValue(item);

                        // 진행 상황 전송
                        var message = $"progress: {scanned}\n\n";
                        await context.Response.WriteAsync(message);
                        await context.Response.Body.FlushAsync();
                    }
                    else if (type == "device")
                    {
                        var deviceProperty = itemType.GetProperty("device");
                        var device = deviceProperty?.GetValue(item);

                        // 디바이스 데이터 전송
                        var json = System.Text.Json.JsonSerializer.Serialize(device);
                        var message = $"data: {json}\n\n";
                        await context.Response.WriteAsync(message);
                        await context.Response.Body.FlushAsync();
                    }
                }
            }
        }
        else
        {
            // UDP 브로드캐스트
            var localIpAddress = body?["LocalIpAddress"]?.GetValue<string>();
            var discoveryPort = body?["DiscoveryPort"]?.GetValue<int>() ?? 8101;
            var devices = await discoveryService.SearchDevicesAsync(localIpAddress, discoveryPort, context.RequestAborted);
            foreach (var device in devices)
            {
                var json = System.Text.Json.JsonSerializer.Serialize(device);
                var message = $"data: {json}\n\n";
                await context.Response.WriteAsync(message);
                await context.Response.Body.FlushAsync();
            }
        }

        // 완료 시그널
        await context.Response.WriteAsync("data: [DONE]\n\n");
        await context.Response.Body.FlushAsync();
    }
    catch (Exception ex)
    {
        var errorJson = System.Text.Json.JsonSerializer.Serialize(new
        {
            result = false,
            error = ex.Message
        });
        await context.Response.WriteAsync($"data: {errorJson}\n\n");
        await context.Response.Body.FlushAsync();
    }
});

// ── /api/Device/ProbeDevice ──────────────────────────────────────────────────
app.MapPost("/api/Device/ProbeDevice", async (JsonNode? body, IHttpClientFactory httpFactory) =>
{
    try
    {
        var ip = body?["IpAddress"]?.GetValue<string>();
        var port = body?["HttpPort"]?.GetValue<int>() ?? 80;

        if (string.IsNullOrWhiteSpace(ip))
        {
            return Results.Ok(BrowserApiResponse.Fail(3, "IpAddress is required."));
        }

        var client = httpFactory.CreateClient();
        client.Timeout = TimeSpan.FromSeconds(5);

        // Get device SN
        var snUrl = $"http://{ip}:{port}/api/GetDeviceSN";
        var snResponse = await client.GetAsync(snUrl);

        if (!snResponse.IsSuccessStatusCode)
        {
            return Results.Ok(BrowserApiResponse.Fail(500, $"Failed to connect to device: HTTP {snResponse.StatusCode}"));
        }

        var snJson = await snResponse.Content.ReadFromJsonAsync<JsonNode>();
        var deviceSN = snJson?["content"]?.GetValue<string>();

        if (string.IsNullOrWhiteSpace(deviceSN))
        {
            return Results.Ok(BrowserApiResponse.Fail(500, "Invalid device response"));
        }

        // Try to get additional device info
        string? deviceName = null;
        string? model = null;
        string? firmware = null;

        try
        {
            var detailUrl = $"http://{ip}:{port}/api/Device/GetDetail";
            var detailResponse = await client.GetAsync(detailUrl);
            if (detailResponse.IsSuccessStatusCode)
            {
                var detailJson = await detailResponse.Content.ReadFromJsonAsync<JsonNode>();
                var content = detailJson?["content"];
                deviceName = content?["DeviceName"]?.GetValue<string>();
                model = content?["Model"]?.GetValue<string>();
                firmware = content?["FirmwareVersion"]?.GetValue<string>();
            }
        }
        catch
        {
            // Optional info, continue without it
        }

        return Results.Ok(BrowserApiResponse.Ok(new
        {
            DeviceSN = deviceSN,
            DeviceName = deviceName,
            Model = model,
            FirmwareVersion = firmware
        }));
    }
    catch (Exception ex)
    {
        return Results.Ok(BrowserApiResponse.Fail(500, $"Probe failed: {ex.Message}"));
    }
});

// ── /api/Device/Register ─────────────────────────────────────────────────────
// 단순히 IP 주소로 디바이스 등록 (HTTPv2 프로토콜에 따라 디바이스가 서버로 접속함)
app.MapPost("/api/Device/Register", (JsonNode? body, MySqlStateStore store) =>
{
    try
    {
        var ip = body?["IpAddress"]?.GetValue<string>();
        var sn = body?["DeviceSN"]?.GetValue<string>();

        if (string.IsNullOrWhiteSpace(ip))
        {
            return Results.Ok(BrowserApiResponse.Fail(3, "IpAddress is required."));
        }

        // SN이 없으면 IP를 SN으로 사용
        if (string.IsNullOrWhiteSpace(sn))
        {
            sn = $"Device-{ip.Replace(".", "-")}";
        }

        // 이미 등록된 디바이스인지 확인
        var existingDevices = store.GetDeviceSummaries();

        LogHub.Instance.Info($"등록 요청: {sn} at {ip} (현재 {existingDevices.Count}개 디바이스 등록됨)");

        var existingBySN = existingDevices.FirstOrDefault(d => d.SN == sn);
        var existingByIP = existingDevices.FirstOrDefault(d => d.IpAddress == ip);

        // 완전히 동일한 디바이스 (SN과 IP 모두 일치)
        if (existingBySN != null && existingBySN.IpAddress == ip)
        {
            LogHub.Instance.Warn($"디바이스 중복 등록 시도: {sn} at {ip} (이미 등록됨)");
            return Results.Ok(BrowserApiResponse.Fail(409, $"디바이스가 이미 등록되어 있습니다. (SN: {sn}, IP: {ip})"));
        }

        // SN은 같지만 IP가 다름 (Keepalive로 자동 등록된 경우) → IP 업데이트
        if (existingBySN != null && existingBySN.IpAddress != ip)
        {
            LogHub.Instance.Info($"디바이스 IP 업데이트: {sn} ({existingBySN.IpAddress ?? "(없음)"} → {ip})");
            store.ConnectDevice(sn, ip, 80, sn, "", null, null, 0);
            return Results.Ok(BrowserApiResponse.Ok($"디바이스 {sn}의 IP 주소가 {ip}로 업데이트되었습니다."));
        }

        // IP는 같지만 SN이 다름 → 기존 디바이스 제거 후 새로 등록
        if (existingByIP != null && existingByIP.SN != sn)
        {
            LogHub.Instance.Info($"IP 중복 발견: {ip}에 기존 디바이스 {existingByIP.SN} 있음 → 제거 후 {sn} 등록");
            store.RemoveDevice(existingByIP.SN);
        }

        // IP 주소만으로 디바이스 등록 (포트 80 기본값)
        // 디바이스는 HTTPv2 프로토콜에 따라 이 서버의 80번 포트로 Keepalive를 보냄
        store.ConnectDevice(sn, ip, 80, sn, "", null, null, 0);

        LogHub.Instance.Info($"디바이스 등록: {sn} at {ip} (HTTPv2 프로토콜 대기)");

        return Results.Ok(BrowserApiResponse.Ok($"디바이스 {sn}이(가) 성공적으로 등록되었습니다."));
    }
    catch (Exception ex)
    {
        return Results.Ok(BrowserApiResponse.Fail(500, $"등록 실패: {ex.Message}"));
    }
});

// ── /api/Device/Connect ──────────────────────────────────────────────────────
app.MapPost("/api/Device/Connect", (JsonNode? body, MySqlStateStore store) =>
{
    try
    {
        var sn = body?["DeviceSN"]?.GetValue<string>();
        var ip = body?["IpAddress"]?.GetValue<string>();
        var port = body?["HttpPort"]?.GetValue<int>() ?? 80;
        var name = body?["DeviceName"]?.GetValue<string>() ?? "Unknown Device";
        var tagName = body?["TagName"]?.GetValue<string>() ?? "";
        var menuPassword = body?["MenuPassword"]?.GetValue<string>() ?? "888888";
        var language = body?["Language"]?.GetValue<string>() ?? "English";
        var model = body?["Model"]?.GetValue<string>();
        var firmware = body?["FirmwareVersion"]?.GetValue<string>();
        var unitNo = body?["UnitNo"]?.GetValue<int>() ?? 0;

        if (string.IsNullOrWhiteSpace(sn) || string.IsNullOrWhiteSpace(ip))
        {
            return Results.Ok(BrowserApiResponse.Fail(3, "DeviceSN and IpAddress are required."));
        }

        // Connect device with installation settings
        store.ConnectDevice(sn, ip, port, name, tagName, model, firmware, unitNo);

        LogHub.Instance.Info($"Device installed: {sn} ({name}) at {ip}:{port}, Tag: {tagName}, Language: {language}");

        return Results.Ok(BrowserApiResponse.Ok($"Device {sn} installed successfully with name '{name}'"));
    }
    catch (Exception ex)
    {
        return Results.Ok(BrowserApiResponse.Fail(500, $"Installation failed: {ex.Message}"));
    }
});

// ── /api/Device/GetDetail ─────────────────────────────────────────────────────
app.MapGet("/api/Device/GetDetail", (MySqlStateStore store) =>
{
    var device = store.GetDeviceSummaries().FirstOrDefault();
    return Results.Ok(BrowserApiResponse.Ok(device));
});

// ── /api/Device/UpdateParameter ──────────────────────────────────────────────
app.MapPost("/api/Device/UpdateParameter", async (HttpRequest request, MySqlStateStore store) =>
{
    var payload = await JsonNode.ParseAsync(request.Body);
    if (payload is not JsonObject setting)
        return Results.Ok(BrowserApiResponse.Fail(10004, "json parse error"));
    var sn = setting["DeviceSN"]?.GetValue<string>() ?? setting["SN"]?.GetValue<string>() ?? "";
    if (!string.IsNullOrWhiteSpace(sn))
        store.SetDesiredWorkSetting(sn, setting);
    return Results.Ok(BrowserApiResponse.Ok());
});

// ── /api/Device/Remote ───────────────────────────────────────────────────────
app.MapPost("/api/Device/Remote", (DeviceRemoteRequest cmd, MySqlStateStore store) =>
{
    var devices = store.GetDeviceSummaries();
    foreach (var d in devices)
    {
        store.SetPendingRemoteCommand(d.SN, new PendingRemoteCommand
        {
            Opendoor = cmd.Opendoor,
            Restart = cmd.Restart,
            Recover = cmd.Recover,
            Closealarm = cmd.Closealarm
        });
    }
    return Results.Ok(BrowserApiResponse.Ok());
});

// ── /api/Device/UploadSoftware ────────────────────────────────────────────────
app.MapPost("/api/Device/UploadSoftware", () =>
    Results.Ok(BrowserApiResponse.Ok()));

// ── /api/People/Search ────────────────────────────────────────────────────────
app.MapPost("/api/People/Search", (PeopleSearchRequest req, MySqlStateStore store) =>
{
    var all = store.GetPeople().AsEnumerable();

    if (!string.IsNullOrWhiteSpace(req.UserID))
        all = all.Where(p => p.UserID.Contains(req.UserID, StringComparison.OrdinalIgnoreCase));
    if (!string.IsNullOrWhiteSpace(req.Name))
        all = all.Where(p => p.Name.Contains(req.Name, StringComparison.OrdinalIgnoreCase));
    if (!string.IsNullOrWhiteSpace(req.Department))
        all = all.Where(p => p.Department.Contains(req.Department, StringComparison.OrdinalIgnoreCase));
    if (!string.IsNullOrWhiteSpace(req.Job))
        all = all.Where(p => p.Job.Contains(req.Job, StringComparison.OrdinalIgnoreCase));
    if (req.AccessType.HasValue)
        all = all.Where(p => p.AccessType == req.AccessType.Value);
    if (req.Timegroup.HasValue)
        all = all.Where(p => p.Timegroup == req.Timegroup.Value);
    if (!string.IsNullOrWhiteSpace(req.CardNum))
        all = all.Where(p => p.CardNum.Contains(req.CardNum, StringComparison.OrdinalIgnoreCase));
    if (!string.IsNullOrWhiteSpace(req.IdentityCard))
        all = all.Where(p => p.IdentityCard.Contains(req.IdentityCard, StringComparison.OrdinalIgnoreCase));
    if (req.Photo.HasValue)
        all = all.Where(p => req.Photo.Value == 1 ? !string.IsNullOrEmpty(p.Photo) : string.IsNullOrEmpty(p.Photo));
    if (req.Fingerprint.HasValue)
        all = all.Where(p => req.Fingerprint.Value == 1 ? p.Fingerprints.Count > 0 : p.Fingerprints.Count == 0);
    if (req.Palmprint.HasValue)
        all = all.Where(p => req.Palmprint.Value == 1 ? p.Palmveins.Count > 0 : p.Palmveins.Count == 0);

    // Sort
    var sortCol = req.OrderByColumn ?? "UserID";
    var desc = string.Equals(req.OrderByType, "DESC", StringComparison.OrdinalIgnoreCase);
    all = sortCol switch
    {
        "Name" => desc ? all.OrderByDescending(p => p.Name) : all.OrderBy(p => p.Name),
        "Department" => desc ? all.OrderByDescending(p => p.Department) : all.OrderBy(p => p.Department),
        "CardNum" => desc ? all.OrderByDescending(p => p.CardNum) : all.OrderBy(p => p.CardNum),
        _ => desc ? all.OrderByDescending(p => p.UserID) : all.OrderBy(p => p.UserID)
    };

    var list = all.ToList();
    var total = list.Count;
    var page = Math.Max(1, req.PageIndex);
    var size = Math.Max(1, req.PageSize);
    var data = list.Skip((page - 1) * size).Take(size).ToList();

    return Results.Ok(BrowserApiResponse.Ok(new PeopleSearchResult
    {
        TotalCount = total,
        PageIndex = page,
        PageSize = size,
        DataList = data
    }));
});

// ── /api/People/GetDetail ─────────────────────────────────────────────────────
app.MapPost("/api/People/GetDetail", (JsonNode? body, MySqlStateStore store) =>
{
    var userId = body?["UserID"]?.GetValue<string>();
    if (string.IsNullOrWhiteSpace(userId))
        return Results.Ok(BrowserApiResponse.Fail(3, "UserID is required."));
    var person = store.GetPeople().FirstOrDefault(p =>
        string.Equals(p.UserID, userId, StringComparison.OrdinalIgnoreCase));
    return person is null
        ? Results.Ok(BrowserApiResponse.Fail(3, "Person not found."))
        : Results.Ok(BrowserApiResponse.Ok(person));
});

// ── /api/People/GetNewID ──────────────────────────────────────────────────────
app.MapGet("/api/People/GetNewID", (MySqlStateStore store) =>
{
    var all = store.GetPeople();
    var maxId = all.Select(p => long.TryParse(p.UserID, out var n) ? n : 0L).DefaultIfEmpty(0).Max();
    return Results.Ok(BrowserApiResponse.Ok((maxId + 1).ToString()));
});

// ── /api/People/New ───────────────────────────────────────────────────────────
app.MapPost("/api/People/New", async (HttpRequest request, MySqlStateStore store) =>
{
    PersonInfo? person = null;

    if (request.HasFormContentType)
    {
        var form = await request.ReadFormAsync();
        var json = form["PeopleJson"].ToString();
        if (!string.IsNullOrWhiteSpace(json))
            person = System.Text.Json.JsonSerializer.Deserialize<PersonInfo>(json);
    }
    else
    {
        person = await request.ReadFromJsonAsync<PersonInfo>();
    }

    if (person is null || string.IsNullOrWhiteSpace(person.UserID))
        return Results.Ok(BrowserApiResponse.Fail(11, "Personnel parameter verification failed."));

    var normalized = NormalizePerson(person);
    var success = store.TryAddPerson(normalized);

    if (!success)
    {
        var existingPeople = store.GetPeople();
        LogHub.Instance.Warn($"Failed to add person {normalized.UserID}. Current people count: {existingPeople.Count}. " +
            $"Existing UserIDs: [{string.Join(", ", existingPeople.Select(p => p.UserID))}]");
    }

    return success
        ? Results.Ok(BrowserApiResponse.Ok())
        : Results.Ok(BrowserApiResponse.Fail(23, "UserID or card number duplicated."));
});

// ── /api/People/Update ────────────────────────────────────────────────────────
app.MapPost("/api/People/Update", async (HttpRequest request, MySqlStateStore store) =>
{
    PersonInfo? person = null;

    if (request.HasFormContentType)
    {
        var form = await request.ReadFormAsync();
        var json = form["PeopleJson"].ToString();
        if (!string.IsNullOrWhiteSpace(json))
            person = System.Text.Json.JsonSerializer.Deserialize<PersonInfo>(json);
    }
    else
    {
        person = await request.ReadFromJsonAsync<PersonInfo>();
    }

    if (person is null || string.IsNullOrWhiteSpace(person.UserID))
        return Results.Ok(BrowserApiResponse.Fail(11, "Personnel parameter verification failed."));

    var normalized = NormalizePerson(person);
    return store.UpdatePerson(normalized)
        ? Results.Ok(BrowserApiResponse.Ok())
        : Results.Ok(BrowserApiResponse.Fail(3, "Person not found."));
});

// ── /api/People/Delete ────────────────────────────────────────────────────────
app.MapPost("/api/People/Delete", (JsonNode? body, MySqlStateStore store) =>
{
    var userId = body?["UserID"]?.GetValue<string>();
    if (string.IsNullOrWhiteSpace(userId))
        return Results.Ok(BrowserApiResponse.Fail(3, "UserID is required."));
    return store.DeletePerson(userId)
        ? Results.Ok(BrowserApiResponse.Ok())
        : Results.Ok(BrowserApiResponse.Fail(3, "Person not found."));
});

// ── /api/Department/Search ────────────────────────────────────────────────────
app.MapPost("/api/Department/Search", (DepartmentSearchRequest req, MySqlStateStore store) =>
{
    var all = store.GetDepartments().AsEnumerable();
    if (!string.IsNullOrWhiteSpace(req.Name))
        all = all.Where(d => d.Name.Contains(req.Name, StringComparison.OrdinalIgnoreCase));
    var list = all.ToList();
    var page = Math.Max(1, req.PageIndex);
    var size = Math.Max(1, req.PageSize);
    return Results.Ok(BrowserApiResponse.Ok(new
    {
        TotalCount = list.Count,
        PageIndex = page,
        PageSize = size,
        DataList = list.Skip((page - 1) * size).Take(size).ToList()
    }));
});

app.MapGet("/api/Department/GetNewID", (MySqlStateStore store) =>
{
    var all = store.GetDepartments();
    var maxId = all.Select(d => long.TryParse(d.DepartmentID, out var n) ? n : 0L).DefaultIfEmpty(0).Max();
    return Results.Ok(BrowserApiResponse.Ok((maxId + 1).ToString()));
});

app.MapPost("/api/Department/New", (DepartmentInfo dept, MySqlStateStore store) =>
{
    if (string.IsNullOrWhiteSpace(dept.DepartmentID))
        return Results.Ok(BrowserApiResponse.Fail(3, "DepartmentID is required."));
    return store.TryAddDepartment(dept)
        ? Results.Ok(BrowserApiResponse.Ok())
        : Results.Ok(BrowserApiResponse.Fail(23, "Department already exists."));
});

app.MapPost("/api/Department/Delete", (JsonNode? body, MySqlStateStore store) =>
{
    var id = body?["DepartmentID"]?.GetValue<string>();
    if (string.IsNullOrWhiteSpace(id))
        return Results.Ok(BrowserApiResponse.Fail(3, "DepartmentID is required."));
    return store.DeleteDepartment(id)
        ? Results.Ok(BrowserApiResponse.Ok())
        : Results.Ok(BrowserApiResponse.Fail(3, "Department not found."));
});

// ── /api/Attendance/* ─────────────────────────────────────────────────────────
app.MapPost("/api/Attendance/Search", (AttendanceSearchRequest req, MySqlStateStore store) =>
{
    try
    {
    var records = store.GetAllRecords()
        .Where(r => r.RecordDetail?["RecordType"] is not null)
        .AsEnumerable();

    if (!string.IsNullOrWhiteSpace(req.UserID))
        records = records.Where(r =>
        {
            var uid = r.RecordDetail?["UserID"]?.GetValue<string>() ?? "";
            // Support prefix match: dong-only search (e.g. "101000000" prefix)
            return uid == req.UserID || uid.StartsWith(req.UserID);
        });

    // Range-based UserID filter (dong-only or dong+ho partial search)
    if (req.UserIDMin.HasValue)
        records = records.Where(r =>
            long.TryParse(r.RecordDetail?["UserID"]?.GetValue<string>() ?? "", out long v) && v >= req.UserIDMin.Value);
    if (req.UserIDMax.HasValue)
        records = records.Where(r =>
            long.TryParse(r.RecordDetail?["UserID"]?.GetValue<string>() ?? "", out long v) && v < req.UserIDMax.Value);

    if (!string.IsNullOrWhiteSpace(req.DeviceSN))
        records = records.Where(r =>
        {
            var sn = r.RecordDetail?["DeviceSN"]?.GetValue<string>() ?? "";
            if (string.IsNullOrWhiteSpace(sn) && !string.IsNullOrWhiteSpace(r.RecordJsonPath))
            {
                var fn = Path.GetFileNameWithoutExtension(r.RecordJsonPath);
                var m = System.Text.RegularExpressions.Regex.Match(fn, @"^(.+?)_(\d{17}_)");
                if (m.Success) sn = m.Groups[1].Value;
            }
            return string.Equals(sn, req.DeviceSN, StringComparison.OrdinalIgnoreCase);
        });

    if (!string.IsNullOrWhiteSpace(req.UserName))
        records = records.Where(r => 
            (r.RecordDetail?["UserName"]?.GetValue<string>() ?? r.RecordDetail?["Name"]?.GetValue<string>() ?? "")
                .Contains(req.UserName, StringComparison.OrdinalIgnoreCase));

    if (!string.IsNullOrWhiteSpace(req.DepartmentID))
        records = records.Where(r => 
            string.Equals(r.RecordDetail?["DepartmentID"]?.GetValue<string>(), req.DepartmentID, StringComparison.OrdinalIgnoreCase));

    if (req.StartTime.HasValue)
        records = records.Where(r =>
        {
            var timeStr = r.RecordDetail?["RecordTime"]?.GetValue<string>();
            if (timeStr is null && r.RecordDetail?["RecordDate"] is JsonNode rdF)
            {
                if (long.TryParse(rdF.ToJsonString().Trim('"'), out long us))
                    return DateTimeOffset.FromUnixTimeSeconds(us).LocalDateTime >= req.StartTime.Value;
            }
            return DateTime.TryParse(timeStr, out var dt) && dt >= req.StartTime.Value;
        });

    if (req.EndTime.HasValue)
        records = records.Where(r =>
        {
            var timeStr = r.RecordDetail?["RecordTime"]?.GetValue<string>();
            if (timeStr is null && r.RecordDetail?["RecordDate"] is JsonNode rdF)
            {
                if (long.TryParse(rdF.ToJsonString().Trim('"'), out long us))
                    return DateTimeOffset.FromUnixTimeSeconds(us).LocalDateTime <= req.EndTime.Value;
            }
            return DateTime.TryParse(timeStr, out var dt) && dt <= req.EndTime.Value;
        });

    var list = records.Select(r =>
    {
        var devSn = r.RecordDetail?["DeviceSN"]?.GetValue<string>() ?? "";
        // Fallback: extract deviceSN from record file name if not in payload
        if (string.IsNullOrWhiteSpace(devSn) && !string.IsNullOrWhiteSpace(r.RecordJsonPath))
        {
            var fileName = Path.GetFileNameWithoutExtension(r.RecordJsonPath);
            // File name format: {deviceSn}_{yyyyMMddHHmmssfff}_{recordId}
            // The deviceSn portion ends at the first '_' followed by a digit sequence (date)
            var match = System.Text.RegularExpressions.Regex.Match(fileName, @"^(.+?)_(\d{17}_)");
            if (match.Success)
                devSn = match.Groups[1].Value;
        }
        var devSnapshot = store.GetDevice(devSn);
        var devDisplay = devSnapshot?.DeviceName is string dn && !string.IsNullOrWhiteSpace(dn)
            ? $"{dn} ({devSn})" : devSn;

        // RecordDate -> RecordTime conversion
        string? recordTime = r.RecordDetail?["RecordTime"]?.GetValue<string>();
        if (recordTime is null && r.RecordDetail?["RecordDate"] is JsonNode rdNode)
        {
            var rdStr = rdNode.ToJsonString().Trim('"');
            if (long.TryParse(rdStr, out long unixSec))
                recordTime = DateTimeOffset.FromUnixTimeSeconds(unixSec)
                                .LocalDateTime.ToString("yyyy-MM-dd HH:mm:ss");
        }
        recordTime ??= r.ReceivedAtUtc.LocalDateTime.ToString("yyyy-MM-dd HH:mm:ss");

        // Safe BodyTemp read (device sends int 0..99, not double)
        string? tempStr = r.RecordDetail?["Temperature"]?.GetValue<string>();
        if (tempStr is null && r.RecordDetail?["BodyTemp"] is JsonNode btNode)
        {
            var btStr = btNode.ToJsonString().Trim('"');
            if (double.TryParse(btStr, out double btVal) && btVal > 0)
                tempStr = btVal.ToString("F1");
        }

        return new AttendanceRecord
        {
            UserID         = r.RecordDetail?["UserID"]?.GetValue<string>() ?? "",
            UserName       = r.RecordDetail?["UserName"]?.GetValue<string>()
                          ?? r.RecordDetail?["Name"]?.GetValue<string>() ?? "",
            DepartmentID   = r.RecordDetail?["DepartmentID"]?.GetValue<string>()
                          ?? r.RecordDetail?["Department"]?.GetValue<string>() ?? "",
            DepartmentName = r.RecordDetail?["DepartmentName"]?.GetValue<string>() ?? "",
            RecordTime     = recordTime,
            DeviceSN       = devDisplay,
            RecordType     = r.RecordDetail?["RecordType"]?.GetValue<int>() ?? 0,
            Temperature    = tempStr,
            PhotoUrl       = r.RecordDetail?["PhotoUrl"]?.GetValue<string>()
                          ?? r.RecordDetail?["Photo"]?.GetValue<string>()
        };
    }).ToList();

    var page = Math.Max(1, req.PageIndex);
    var size = Math.Max(1, req.PageSize);

    return Results.Ok(BrowserApiResponse.Ok(new AttendanceSearchResult
    {
        TotalCount = list.Count,
        PageIndex  = page,
        PageSize   = size,
        DataList   = list.Skip((page - 1) * size).Take(size).ToList()
    }));
    }
    catch (Exception ex)
    {
        return Results.Ok(BrowserApiResponse.Fail(500, $"Search error: {ex.Message} | {ex.InnerException?.Message}"));
    }
});

app.MapPost("/api/Attendance/Statistics", (AttendanceSearchRequest req, MySqlStateStore store) =>
{
    var records = store.GetAllRecords()
        .Where(r => r.RecordDetail?["RecordType"] is not null)
        .AsEnumerable();

    if (req.StartTime.HasValue)
        records = records.Where(r =>
        {
            var timeStr = r.RecordDetail?["RecordTime"]?.GetValue<string>();
            return DateTime.TryParse(timeStr, out var dt) && dt >= req.StartTime.Value;
        });

    if (req.EndTime.HasValue)
        records = records.Where(r =>
        {
            var timeStr = r.RecordDetail?["RecordTime"]?.GetValue<string>();
            return DateTime.TryParse(timeStr, out var dt) && dt <= req.EndTime.Value;
        });

    var list = records.ToList();
    var uniqueUsers = list.Select(r => r.RecordDetail?["UserID"]?.GetValue<string>()).Distinct().Count();
    var uniqueDepts = list.Select(r => r.RecordDetail?["DepartmentID"]?.GetValue<string>()).Distinct().Count();

    var stats = new AttendanceStatistics
    {
        TotalRecords = list.Count,
        UniqueUsers = uniqueUsers,
        UniqueDepartments = uniqueDepts,
        StartTime = req.StartTime,
        EndTime = req.EndTime
    };

    return Results.Ok(BrowserApiResponse.Ok(stats));
});

// ── /api/Record/* ─────────────────────────────────────────────────────────────
app.MapPost("/api/Record/Identify/Search", (RecordSearchRequest req, MySqlStateStore store) =>
{
    var all = store.GetAllRecords()
        .Where(r => r.RecordDetail?["RecordType"] is not null)
        .AsEnumerable();

    all = ApplyRecordFilters(all, req);
    var list = all.ToList();
    var page = Math.Max(1, req.PageIndex);
    var size = Math.Max(1, req.PageSize);
    return Results.Ok(BrowserApiResponse.Ok(new
    {
        TotalCount = list.Count,
        PageIndex = page,
        PageSize = size,
        DataList = list.Skip((page - 1) * size).Take(size)
            .Select(r => r.RecordDetail)
            .ToList()
    }));
});

app.MapPost("/api/Record/DoorSensor/Search", (RecordSearchRequest req, MySqlStateStore store) =>
    Results.Ok(BrowserApiResponse.Ok(new { TotalCount = 0, PageIndex = req.PageIndex, PageSize = req.PageSize, DataList = Array.Empty<object>() })));

app.MapPost("/api/Record/System/Search", (RecordSearchRequest req, MySqlStateStore store) =>
    Results.Ok(BrowserApiResponse.Ok(new { TotalCount = 0, PageIndex = req.PageIndex, PageSize = req.PageSize, DataList = Array.Empty<object>() })));

app.MapPost("/api/Record/Delete/All", (MySqlStateStore store) =>
{
    store.ClearAllRecords();
    return Results.Ok(BrowserApiResponse.Ok());
});

app.MapPost("/api/Record/Delete/ByType", (DeleteRecordsByTypeRequest req, MySqlStateStore store) =>
{
    store.ClearRecordsByType(req.RecordType);
    return Results.Ok(BrowserApiResponse.Ok());
});

// HTTP 서버를 백그라운드에서 실행
var cts = new CancellationTokenSource();

// ── 자동 시간동기화 타이머 (하루 4회: 00:00, 06:00, 12:00, 18:00) ──────────────
_ = Task.Run(async () =>
{
    // 다음 정각(0, 6, 12, 18시) 까지 대기 후 반복
    while (!cts.Token.IsCancellationRequested)
    {
        var now = DateTime.Now;
        var nextHours = new[] { 0, 6, 12, 18 };
        var next = nextHours
            .Select(h => new DateTime(now.Year, now.Month, now.Day, h, 0, 0))
            .Select(t => t <= now ? t.AddDays(1) : t)
            .OrderBy(t => t)
            .First();

        var delay = next - now;
        try { await Task.Delay(delay, cts.Token); } catch { break; }

        if (cts.Token.IsCancellationRequested) break;

        var sns = app.Services.GetRequiredService<MySqlStateStore>().GetAllDeviceSNs();
        var ts = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        foreach (var sn in sns)
            app.Services.GetRequiredService<MySqlStateStore>().QueueSyncTime(sn, ts);
        LogHub.Instance.Info($"[자동 시간동기화] {sns.Count}개 단말기에 시간 동기화 명령 예약 (Unix={ts})");
    }
});

// ── 관리자: 전체 단말기 시간동기화 즉시 실행 ─────────────────────────────────────
app.MapPost("/admin/devices/sync-time-all", (MySqlStateStore store) =>
{
    var sns = store.GetAllDeviceSNs();
    var ts = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
    foreach (var sn in sns)
        store.QueueSyncTime(sn, ts);
    LogHub.Instance.Info($"[시간동기화] 전체 {sns.Count}개 단말기에 시간 동기화 명령 예약 (Unix={ts})");
    return Results.Ok(ApiResponse.Ok($"{sns.Count}개 단말기에 시간 동기화 명령 예약 완료"));
});

// Windows 방화벽 체크
try
{
    var exePath = System.Diagnostics.Process.GetCurrentProcess().MainModule?.FileName ?? "";
    if (!string.IsNullOrEmpty(exePath))
    {
        if (!FirewallHelper.CheckFirewallRule("FDDC UDP Discovery"))
        {
            LogHub.Instance.Warn("?? Windows 방화벽 규칙이 없습니다. UDP 브로드캐스트 검색이 차단될 수 있습니다.");
            LogHub.Instance.Info("방화벽 규칙 추가를 시도합니다...");

            if (FirewallHelper.AddUdpFirewallRule(exePath))
            {
                LogHub.Instance.Info("? 방화벽 규칙이 추가되었습니다.");
            }
            else
            {
                LogHub.Instance.Warn("?? 방화벽 규칙 추가 실패. 수동으로 추가해야 합니다:");
                LogHub.Instance.Warn(FirewallHelper.GetManualFirewallInstructions());
            }
        }
        else
        {
            LogHub.Instance.Info("? Windows 방화벽 규칙이 설정되어 있습니다.");
        }
    }
}
catch (Exception ex)
{
    LogHub.Instance.Warn($"방화벽 체크 실패: {ex.Message}");
}

var serverTask = app.RunAsync(cts.Token);

// 서버 URL 가져오기 및 브라우저용으로 변환
var urls = app.Urls.ToArray();
var rawUrl = urls.Length > 0 ? urls[0] : "http://localhost";
// 0.0.0.0을 localhost로 변환 (브라우저에서 사용 가능하도록)
var serverUrl = rawUrl.Replace("0.0.0.0", "localhost");

// MainForm 생성 및 서버 URL 설정
var mainForm = new MainForm();
mainForm.SetServerUrl(serverUrl);
mainForm.FormClosed += (_, _) =>
{
    cts.Cancel();
};

// Windows Forms 실행
Application.Run(mainForm);

// 서버 종료 대기
await serverTask;


static string ResolveConfiguredPath(string? configured, string contentRoot, string? fallback = null)
{
    var path = string.IsNullOrWhiteSpace(configured) ? fallback : configured;
    if (string.IsNullOrWhiteSpace(path))
        return contentRoot;
    path = Environment.ExpandEnvironmentVariables(path);
    var myDocs = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
    path = path.Replace("{MyDocuments}", myDocs, StringComparison.OrdinalIgnoreCase);
    if (!Path.IsPathRooted(path))
        path = Path.Combine(contentRoot, path);
    return Path.GetFullPath(path);
}

static string? FirstNonEmpty(params string?[] values) =>
    values.FirstOrDefault(value => !string.IsNullOrWhiteSpace(value));

static string? ExtractBearer(HttpContext ctx)
{
    var auth = ctx.Request.Headers.Authorization.ToString();
    if (auth.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
        return auth["Bearer ".Length..].Trim();
    return null;
}

static IEnumerable<RecordSnapshot> ApplyRecordFilters(IEnumerable<RecordSnapshot> all, RecordSearchRequest req)
{
    if (req.BeginDate > 0)
        all = all.Where(r => r.RecordDetail?["RecordDate"]?.GetValue<long>() >= req.BeginDate);
    if (req.EndDate > 0)
        all = all.Where(r => r.RecordDetail?["RecordDate"]?.GetValue<long>() <= req.EndDate);
    if (!string.IsNullOrWhiteSpace(req.UserID))
        all = all.Where(r => r.RecordDetail?["UserID"]?.GetValue<string>()?.Contains(req.UserID, StringComparison.OrdinalIgnoreCase) == true);
    if (!string.IsNullOrWhiteSpace(req.Name))
        all = all.Where(r => r.RecordDetail?["Name"]?.GetValue<string>()?.Contains(req.Name, StringComparison.OrdinalIgnoreCase) == true);
    return all;
}

static IResult DownloadPeopleList(DownloadPeopleListRequest request, MySqlStateStore store)
{
    if (string.IsNullOrWhiteSpace(request.SN))
    {
        return Results.BadRequest(new ApiResponse(400, "SN is required."));
    }

    var people = store.GetPeopleForDownload(request.SN, request.Limit).ToList();

    LogHub.Instance.Info($"[다운로드] 요청: 단말기 {request.SN}가 사용자 목록 요청");
    LogHub.Instance.Info($"[다운로드] 응답: 단말기 {request.SN}에 {people.Count}명의 사용자 정보 전송");

    if (people.Count > 0)
    {
        var firstPerson = people.First();
        var hasPhoto = !string.IsNullOrEmpty(firstPerson.Photo);
        var photoSize = hasPhoto ? firstPerson.PhotoLen : 0;
        LogHub.Instance.Info($"  샘플: UserID={firstPerson.UserID}, 이름={firstPerson.Name}, 사진={(hasPhoto ? $"{photoSize}bytes" : "없음")}");

        // Log all fields of first person for debugging
        LogHub.Instance.Info($"  상세: Password={firstPerson.Password ?? "(null)"}, CardNum={firstPerson.CardNum ?? "(null)"}, " +
            $"AccessType={firstPerson.AccessType}, OpenTimes={firstPerson.OpenTimes}, " +
            $"Timegroup={firstPerson.Timegroup}, ExpirationDate={firstPerson.ExpirationDate}");

        // Log statistics
        var withPhoto = people.Count(p => !string.IsNullOrEmpty(p.Photo));
        var totalPhotoSize = people.Where(p => !string.IsNullOrEmpty(p.Photo)).Sum(p => p.PhotoLen);
        LogHub.Instance.Info($"  통계: 사진 있음={withPhoto}명, 총 사진 크기={totalPhotoSize}bytes ({totalPhotoSize/1024}KB)");
    }

    var response = new DownloadPeopleListResponse
    {
        Success = people.Count > 0 ? 1 : 0,  // Stop cycle with Success=0 when empty
        PeopleCount = people.Count,
        PeopleList = people
    };

    // Log actual JSON being sent to device for debugging
    try
    {
        var json = System.Text.Json.JsonSerializer.Serialize(response);

        // Save full JSON to file for debugging
        if (people.Count > 0)
        {
            var debugPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "App_Data", "last_download_payload.json");
            File.WriteAllText(debugPath, json);
            LogHub.Instance.Info($"[디버그] 전체 JSON을 파일로 저장: {debugPath}");
        }

        var preview = json.Length > 800 ? json.Substring(0, 800) + "..." : json;
        LogHub.Instance.Info($"[응답] JSON (처음 800자): {preview}");
    }
    catch (Exception ex)
    {
        LogHub.Instance.Error($"JSON 직렬화 실패: {ex.Message}");
    }

    return Results.Ok(response);
}

static IResult SelectDeleteInfo(SelectDeleteInfoRequest request, MySqlStateStore store)
{
    if (string.IsNullOrWhiteSpace(request.SN))
    {
        return Results.BadRequest(new ApiResponse(400, "SN is required."));
    }

    var deleteList = store.GetDeletePeople(request.SN).ToList();
    return Results.Ok(new SelectDeleteInfoResponse
    {
        Success = deleteList.Count > 0 ? 1 : 0,  // Stop cycle with Success=0 when empty
        DeleteList = deleteList
    });
}

static PersonInfo NormalizePerson(PersonInfo person)
{
    var normalized = new PersonInfo
    {
        UserID = person.UserID?.Trim() ?? string.Empty,
        Code = string.IsNullOrWhiteSpace(person.Code?.Trim()) 
            ? (person.UserID?.Trim() ?? string.Empty)  // Default Code to UserID
            : person.Code.Trim(),
        Name = person.Name?.Trim() ?? string.Empty,
        Job = person.Job?.Trim() ?? string.Empty,
        Department = person.Department?.Trim() ?? string.Empty,
        IdentityCard = person.IdentityCard?.Trim() ?? string.Empty,
        Attachment = person.Attachment?.Trim() ?? string.Empty,
        Photo = person.Photo?.Trim() ?? string.Empty,
        PhotoMD5 = person.PhotoMD5?.Trim() ?? string.Empty,
        PhotoLen = person.PhotoLen,
        Password = string.IsNullOrWhiteSpace(person.Password?.Trim()) ? "0000" : person.Password.Trim(),
        CardNum = string.IsNullOrWhiteSpace(person.CardNum?.Trim()) ? "0" : person.CardNum.Trim(),
        QRCode = person.QRCode?.Trim() ?? string.Empty,
        AccessType = Math.Clamp(person.AccessType, 0, 2),
        // ExpirationDate: 0이거나 int32 범위 초과 값이면 2037-12-31 23:59:59 UTC(=2145916799)로 설정
        // 단말기 펜웨어가 int32로 처리하므로 2^31-1을 쓴음
        ExpirationDate = (person.ExpirationDate == 0 || person.ExpirationDate > 4102412399u)
            ? 4102412399u   // 2099-12-31 23:59:59 KST(UTC+9) ? 단말기 실제 최대값
            : person.ExpirationDate,
        OpenTimes = person.OpenTimes <= 0 ? 65535 : person.OpenTimes,
        KeepOpen = person.KeepOpen,
        Timegroup = person.Timegroup <= 0 ? 1 : person.Timegroup,  // Default to 1 if 0 or negative
        Holidays = person.Holidays?.Trim() ?? string.Empty,
        Elevators = person.Elevators?.Trim() ?? string.Empty,
        FaceFeature = person.FaceFeature?.Trim() ?? string.Empty,
        FaceFeatureMD5 = person.FaceFeatureMD5?.Trim() ?? string.Empty,
        Fingerprints = person.Fingerprints ?? new(),
        Palmveins = person.Palmveins ?? new()
    };

    // Calculate PhotoMD5 and PhotoLen if Photo is Base64 encoded
    if (!string.IsNullOrWhiteSpace(normalized.Photo))
    {
        // 로컬 파일 경로 또는 단말기 내부 경로가 들어온 경우 Photo 초기화
        // JPEG Base64는 "/9j/..."로 시작하므로 확장자 포함 여부로 단말기 경로 판별
        bool looksLikePath = normalized.Photo.Contains(":\\") ||  // C:\...
                             normalized.Photo.Contains(":/") ||   // C:/...
                             (normalized.Photo.StartsWith("/") &&
                              System.IO.Path.HasExtension(normalized.Photo));  // /data/user_pic/xxx.jpg
        if (looksLikePath)
        {
            normalized.Photo    = string.Empty;
            normalized.PhotoMD5 = string.Empty;
            normalized.PhotoLen = 0;
        }
        else
        {
            try
            {
                var photoBytes = Convert.FromBase64String(normalized.Photo);
                normalized.PhotoLen = photoBytes.Length;

                if (string.IsNullOrWhiteSpace(normalized.PhotoMD5))
                {
                    using var md5 = System.Security.Cryptography.MD5.Create();
                    var hash = md5.ComputeHash(photoBytes);
                    normalized.PhotoMD5 = BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
                }
            }
            catch
            {
                // Base64 디코딩 실패 → 유효하지 않은 값이므로 초기화
                normalized.Photo    = string.Empty;
                normalized.PhotoMD5 = string.Empty;
                normalized.PhotoLen = 0;
            }
        }
    }

    return normalized;
}

static async Task<string?> ReadMultipartValueAsync(IFormCollection form, string key)
{
    var file = form.Files.GetFile(key);
    if (file is null)
    {
        return null;
    }

    await using var stream = file.OpenReadStream();
    using var memory = new MemoryStream();
    await stream.CopyToAsync(memory);
    var bytes = memory.ToArray();

    if (file.ContentType.Contains("gzip", StringComparison.OrdinalIgnoreCase) || LooksLikeGzip(bytes))
    {
        bytes = DecompressGzip(bytes);
    }

    return Encoding.UTF8.GetString(bytes);
}

static bool LooksLikeGzip(byte[] bytes) =>
    bytes.Length >= 2 && bytes[0] == GzipMagicByte1 && bytes[1] == GzipMagicByte2;

static byte[] DecompressGzip(byte[] bytes)
{
    using var source = new MemoryStream(bytes);
    using var gzip = new GZipStream(source, CompressionMode.Decompress);
    using var destination = new MemoryStream();
    gzip.CopyTo(destination);
    return destination.ToArray();
}
