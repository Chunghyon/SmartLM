using System.IO.Compression;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using FaceDeviceHttpPcServer.Forms;
using FaceDeviceHttpPcServer.Middleware;
using FaceDeviceHttpPcServer.Models;
using FaceDeviceHttpPcServer.Services;

const byte GzipMagicByte1 = 0x1F;
const byte GzipMagicByte2 = 0x8B;

// Windows Forms ÃÊ±âÈ­
Application.EnableVisualStyles();
Application.SetCompatibleTextRenderingDefault(false);
Application.SetHighDpiMode(HighDpiMode.SystemAware);

var builder = WebApplication.CreateBuilder(args);

// ContentRootPath¸¦ ÇöÀç ½ÇÇà µð·ºÅä¸®·Î ¼³Á¤
var contentRoot = AppDomain.CurrentDomain.BaseDirectory;
builder.Environment.ContentRootPath = contentRoot;
builder.Configuration.SetBasePath(contentRoot);

// WebRootPath ¼³Á¤
var webRoot = Path.Combine(contentRoot, "wwwroot");
if (Directory.Exists(webRoot))
{
    builder.Environment.WebRootPath = webRoot;
}
else
{
    // °³¹ß È¯°æ¿¡¼­´Â ÇÁ·ÎÁ§Æ® ·çÆ®ÀÇ wwwroot »ç¿ë
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
});

builder.Services.AddSingleton(sp =>
{
    var env = sp.GetRequiredService<IHostEnvironment>();
    var configuredPath = builder.Configuration["StoragePath"];
    var storagePath = string.IsNullOrWhiteSpace(configuredPath)
        ? Path.Combine(env.ContentRootPath, "App_Data")
        : Path.GetFullPath(Path.IsPathRooted(configuredPath)
            ? configuredPath
            : Path.Combine(env.ContentRootPath, configuredPath));

    return new StateStore(storagePath);
});

builder.Services.AddSingleton<DeviceDiscoveryService>();
builder.Services.AddHttpClient();

// ¿äÃ» ¾ÐÃà ÇØÁ¦ Áö¿ø Ãß°¡ (GZIP, Deflate, Brotli)
builder.Services.AddRequestDecompression();

var app = builder.Build();

// ¿äÃ» ¾ÐÃà ÇØÁ¦ ¹Ìµé¿þ¾î (HttpLoggingMiddlewareº¸´Ù ¸ÕÀú ½ÇÇàµÇ¾î¾ß ÇÔ)
app.UseRequestDecompression();

// HTTP ¿äÃ» ·Î±ë ¹Ìµé¿þ¾î Ãß°¡
app.UseMiddleware<HttpLoggingMiddleware>();

// Á¤Àû ÆÄÀÏ ¼­ºñ½º ±¸¼º
app.UseDefaultFiles();
app.UseStaticFiles();

// Favicon 404 ¹æÁö
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

app.MapPost("/Device/Keepalive", (KeepaliveRequest request, HttpContext httpContext, StateStore store) =>
{
    if (string.IsNullOrWhiteSpace(request.SN))
    {
        return Results.BadRequest(new ApiResponse(400, "SN is required."));
    }

    // µð¹ÙÀÌ½º IP ÁÖ¼Ò ÃßÃâ
    var deviceIp = httpContext.Connection.RemoteIpAddress?.ToString();
    if (deviceIp == "::1" || deviceIp == "127.0.0.1")
    {
        deviceIp = null; // ·ÎÄÃ ¿¬°áÀº IP ÀúÀå ¾È ÇÔ
    }

    var response = store.UpsertKeepalive(request, deviceIp);
    return Results.Ok(response);
});

app.MapPost("/Device/UploadWorkSetting", async (HttpRequest request, StateStore store) =>
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

app.MapPost("/Device/DownloadWorkSetting", async (HttpRequest request, StateStore store) =>
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
        return Results.Ok(new ApiResponseWithContent { Success = 404, Content = "No work-setting snapshot available" });
    }

    // Return with Content field as per protocol
    return Results.Ok(new ApiResponseWithContent { Success = 0, Content = workSetting });
});

app.MapPost("/People/DownloadPeopleList", DownloadPeopleList);
app.MapPost("/DevicePass/SelectPassInfo", DownloadPeopleList);

app.MapPost("/DevicePass/SelectDeleteInfo", SelectDeleteInfo);
app.MapPost("/People/SelectDeleteInfo", SelectDeleteInfo);

app.MapPost("/Record/UploadIdentifyRecord", async (HttpRequest request, StateStore store) =>
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

app.MapPost("/Record/UploadSystemRecord", async (HttpRequest request, StateStore store) =>
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

    // System records typically don't have photos
    store.SaveSystemRecord(sn, recordNode);
    LogHub.Instance.Info($"System record uploaded from device {sn}");
    return Results.Ok(ApiResponse.Ok());
});

app.MapGet("/admin/people", (StateStore store) => Results.Ok(store.GetPeople()));

app.MapPost("/admin/people", (PersonInfo person, StateStore store) =>
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

app.MapDelete("/admin/people/{userId}", (string userId, StateStore store) =>
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

app.MapGet("/admin/devices", (StateStore store) => Results.Ok(store.GetDeviceSummaries()));

app.MapGet("/admin/devices/{sn}", (string sn, StateStore store) =>
{
    var device = store.GetDevice(sn);
    return device is null
        ? Results.NotFound(new ApiResponse(404, "Device not found."))
        : Results.Ok(device);
});

app.MapPost("/admin/devices/{sn}/request-add-people", (string sn, StateStore store) =>
{
    var count = store.MarkAddPeopleRequested(sn);
    return Results.Ok(ApiResponse.Ok($"AddPeople={count} will be returned on the next keepalive for {sn}."));
});

app.MapPost("/admin/devices/{sn}/request-delete-people", (string sn, StateStore store) =>
{
    var count = store.MarkDeletePeopleRequested(sn);
    return Results.Ok(ApiResponse.Ok(
        count > 0
            ? $"DeletePeople={count} will be returned on the next keepalive for {sn}."
            : $"There are no pending deletions for {sn}."));
});

app.MapPost("/admin/devices/{sn}/request-sync", (string sn, StateStore store) =>
{
    store.MarkSyncRequested(sn);
    return Results.Ok(ApiResponse.Ok($"SyncParameter will be returned on the next keepalive for {sn}."));
});

app.MapPost("/admin/devices/{sn}/remote-command", (string sn, JsonNode? body, StateStore store) =>
{
    try
    {
        var commandType = body?["CommandType"]?.GetValue<string>()?.ToLower();

        if (string.IsNullOrWhiteSpace(commandType))
            return Results.BadRequest(new ApiResponse(400, "CommandType is required"));

        switch (commandType)
        {
            case "restart":
                store.QueueRemoteCommand(sn, restart: true);
                LogHub.Instance.Info($"Remote command queued: Restart device {sn}");
                return Results.Ok(ApiResponse.Ok("Restart command queued"));

            case "opendoor":
                store.QueueRemoteCommand(sn, opendoor: true);
                LogHub.Instance.Info($"Remote command queued: Open door on {sn}");
                return Results.Ok(ApiResponse.Ok("Open door command queued"));

            case "closealarm":
                store.QueueRemoteCommand(sn, closealarm: true);
                LogHub.Instance.Info($"Remote command queued: Close alarm on {sn}");
                return Results.Ok(ApiResponse.Ok("Close alarm command queued"));

            case "pushallpeople":
                var peopleCount = store.MarkAddPeopleRequested(sn);
                LogHub.Instance.Info($"Remote command queued: Push all people to {sn} ({peopleCount} people)");
                return Results.Ok(ApiResponse.Ok($"Push all people command queued ({peopleCount} people)"));

            case "deleteallpeople":
                var deletedCount = store.DeleteAllPeople(sn);
                LogHub.Instance.Info($"Remote command: Delete all people from {sn} ({deletedCount} people marked for deletion)");
                return Results.Ok(ApiResponse.Ok($"Delete all people command queued ({deletedCount} people)"));

            case "clearrecords":
                store.QueueRemoteCommand(sn, clearRecord: true);
                LogHub.Instance.Info($"Remote command queued: Clear records on {sn}");
                return Results.Ok(ApiResponse.Ok("Clear records command queued"));

            case "repostrecord":
                store.QueueRemoteCommand(sn, repostRecord: true);
                LogHub.Instance.Info($"Remote command queued: Repost records from {sn}");
                return Results.Ok(ApiResponse.Ok("Repost records command queued"));

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

app.MapDelete("/admin/devices/{sn}", (string sn, StateStore store) =>
{
    try
    {
        LogHub.Instance.Info($"µð¹ÙÀÌ½º Á¦°Å ¿äÃ»: {sn}");

        if (store.RemoveDevice(sn))
        {
            LogHub.Instance.Info($"µð¹ÙÀÌ½º Á¦°Å ¿Ï·á: {sn}");

            return Results.Ok(ApiResponse.Ok($"Device {sn} removed successfully"));
        }
        else
        {
            LogHub.Instance.Warn($"µð¹ÙÀÌ½º Á¦°Å ½ÇÆÐ: {sn} (Ã£À» ¼ö ¾øÀ½)");
            return Results.NotFound(new ApiResponse(404, "Device not found"));
        }
    }
    catch (Exception ex)
    {
        LogHub.Instance.Error($"Failed to remove device {sn}: {ex.Message}");
        return Results.Ok(new ApiResponse(500, $"Failed to remove device: {ex.Message}"));
    }
});

app.MapPost("/admin/devices/{sn}/request-upload-work-setting", (string sn, StateStore store) =>
{
    store.MarkUploadWorkSettingRequested(sn);
    return Results.Ok(ApiResponse.Ok($"UploadWorkParameter will be returned on the next keepalive for {sn}."));
});

app.MapPut("/admin/devices/{sn}/work-setting", async (string sn, HttpRequest request, StateStore store) =>
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

// ¦¡¦¡ Admin: remote command dispatch ¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡
app.MapPost("/admin/devices/{sn}/remote", (string sn, DeviceRemoteRequest cmd, StateStore store) =>
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

// ¦¡¦¡ Admin: departments ¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡
app.MapGet("/admin/departments", (StateStore store) => Results.Ok(store.GetDepartments()));

app.MapPost("/admin/departments", (DepartmentInfo dept, StateStore store) =>
{
    if (string.IsNullOrWhiteSpace(dept.DepartmentID))
        return Results.BadRequest(new ApiResponse(400, "DepartmentID is required."));
    return store.TryAddDepartment(dept)
        ? Results.Ok(ApiResponse.Ok($"Department {dept.DepartmentID} added."))
        : Results.Conflict(new ApiResponse(409, $"Department {dept.DepartmentID} already exists."));
});

app.MapDelete("/admin/departments/{id}", (string id, StateStore store) =>
    store.DeleteDepartment(id)
        ? Results.Ok(ApiResponse.Ok($"Department {id} deleted."))
        : Results.NotFound(new ApiResponse(404, "Department not found.")));

// ???????????????????????????????????????????????????????????????????????????????
// HTTP-Docking Protocol  (Device ¡æ Server)
// ???????????????????????????????????????????????????????????????????????????????

// ¦¡¦¡ /Device/RemoteCommand ¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡
app.MapPost("/Device/RemoteCommand", (RemoteCommandRequest request, StateStore store) =>
{
    if (string.IsNullOrWhiteSpace(request.SN))
        return Results.BadRequest(new ApiResponse(400, "SN is required."));

    var cmd = store.ConsumeRemoteCommand(request.SN);
    if (cmd is null)
        return Results.Ok(new RemoteCommandResponse());

    return Results.Ok(new RemoteCommandResponse
    {
        Restart = cmd.Restart,
        Recover = cmd.Recover,
        Opendoor = cmd.Opendoor,
        Closealarm = cmd.Closealarm,
        RepostRecord = cmd.RepostRecord,
        PushAllPeople = cmd.PushAllPeople,
        QueryPeople = cmd.QueryPeople,
        ClearRecord = cmd.ClearRecord
    });
});

// ¦¡¦¡ /People/DownloadPeopleListResult ¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡
app.MapPost("/People/DownloadPeopleListResult", (DownloadPeopleListResultRequest request, StateStore store) =>
{
    if (string.IsNullOrWhiteSpace(request.SN))
        return Results.BadRequest(new ApiResponse(400, "SN is required."));
    return Results.Ok(ApiResponse.Ok());
});

// ¦¡¦¡ /People/DeletePeopleList ¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡
app.MapPost("/People/DeletePeopleList", (DeletePeopleListRequest request, StateStore store) =>
{
    if (string.IsNullOrWhiteSpace(request.SN))
        return Results.BadRequest(new ApiResponse(400, "SN is required."));

    var effectiveLimit = request.Limit <= 0 ? 50 : Math.Min(request.Limit, 1000);
    var list = store.GetDeletePeople(request.SN).Take(effectiveLimit).ToList();
    return Results.Ok(new DeletePeopleListResponse { DeleteList = list });
});

// ¦¡¦¡ /People/DeletePeopleListResult ¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡
app.MapPost("/People/DeletePeopleListResult", (DeletePeopleListResultRequest request, StateStore store) =>
{
    if (string.IsNullOrWhiteSpace(request.SN))
        return Results.BadRequest(new ApiResponse(400, "SN is required."));
    return Results.Ok(ApiResponse.Ok());
});

// ¦¡¦¡ /People/PushPeople  (device uploads its stored people to server) ¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡
app.MapPost("/People/PushPeople", async (HttpRequest httpRequest, StateStore store) =>
{
    List<PersonInfo>? people = null;
    string? sn = null;

    if (httpRequest.HasFormContentType)
    {
        var form = await httpRequest.ReadFormAsync();
        sn = FirstNonEmpty(form["SN"].ToString(), form["DeviceSN"].ToString());
        var json = form["PeopleJson"].ToString();
        if (!string.IsNullOrWhiteSpace(json))
            people = System.Text.Json.JsonSerializer.Deserialize<List<PersonInfo>>(json);
    }
    else
    {
        var payload = await JsonNode.ParseAsync(httpRequest.Body);
        sn = payload?["SN"]?.GetValue<string>();
        var listNode = payload?["PeopleList"];
        if (listNode is not null)
            people = System.Text.Json.JsonSerializer.Deserialize<List<PersonInfo>>(listNode.ToJsonString());
    }

    if (string.IsNullOrWhiteSpace(sn))
        return Results.BadRequest(new ApiResponse(400, "SN is required."));

    var (success, fail) = store.SavePushedPeople(people ?? new());
    return Results.Ok(ApiResponse.Ok($"Received {success} people, {fail} failed."));
});

// ¦¡¦¡ /Record/UploadSystemRecord ¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡
app.MapPost("/Record/UploadSystemRecord", (UploadSystemRecordRequest request, StateStore store) =>
{
    if (string.IsNullOrWhiteSpace(request.SN))
        return Results.BadRequest(new ApiResponse(400, "SN is required."));

    store.SaveSystemRecords(request.SN, request.RecordType, request.Records ?? new());
    return Results.Ok(ApiResponse.Ok());
});

// ???????????????????????????????????????????????????????????????????????????????
// Browser UI Protocol  (Browser ¡æ Server acting as device proxy)
// ???????????????????????????????????????????????????????????????????????????????

// ¦¡¦¡ /api/heartBeat ¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡
app.MapGet("/api/heartBeat", () => Results.Ok(BrowserApiResponse.Ok("OK")));

// ¦¡¦¡ /api/GetDeviceSN ¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡
app.MapGet("/api/GetDeviceSN", (StateStore store) =>
{
    var devices = store.GetDeviceSummaries();
    var sn = devices.FirstOrDefault()?.SN ?? "UNKNOWN";
    return Results.Ok(BrowserApiResponse.Ok(sn));
});

// ¦¡¦¡ /api/User/Login ¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡
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

// ¦¡¦¡ /api/Device/FunctionList ¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡
app.MapGet("/api/Device/FunctionList", () => Results.Ok(BrowserApiResponse.Ok(new
{
    FaceIR = true, BodyTemperature = false, Elevator = true,
    FaceMask = true, AlarmClock = true, ExcelFile = true, ZipFile = true,
    TimeGreoup = true, WIFI = true,
    HTTPClient_V1 = true, HTTPClient_V2 = true, MQTT = true,
    Websocket_V1 = true, Websocket_V2 = true
})));

// ¦¡¦¡ /api/Device/GetNetworkInterfaces ¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡
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

// ¦¡¦¡ /api/Device/Search ¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡
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
            // IP ¹üÀ§ ½ºÄµ
            devices = await discoveryService.ScanNetworkAsync(subnet);
        }
        else
        {
            // UDP ºê·ÎµåÄ³½ºÆ® (¼±ÅÃµÈ ·ÎÄÃ IP »ç¿ë)
            devices = await discoveryService.SearchDevicesAsync(localIpAddress, discoveryPort);
        }

        return Results.Ok(BrowserApiResponse.Ok(devices));
    }
    catch (Exception ex)
    {
        return Results.Ok(BrowserApiResponse.Fail(500, $"Search failed: {ex.Message}"));
    }
});

// ¦¡¦¡ /api/Device/SearchStream ¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡
app.MapPost("/api/Device/SearchStream", async (DeviceDiscoveryService discoveryService, JsonNode? body, HttpContext context) =>
{
    try
    {
        var searchType = body?["SearchType"]?.GetValue<string>() ?? "broadcast";
        var subnet = body?["Subnet"]?.GetValue<string>();

        context.Response.ContentType = "text/event-stream";
        context.Response.Headers.Add("Cache-Control", "no-cache");
        context.Response.Headers.Add("Connection", "keep-alive");

        await context.Response.Body.FlushAsync();

        if (searchType.Equals("scan", StringComparison.OrdinalIgnoreCase) && !string.IsNullOrWhiteSpace(subnet))
        {
            // IP ¹üÀ§ ½ºÄµ - ½Ç½Ã°£ ½ºÆ®¸®¹Ö (progress + device)
            await foreach (var item in discoveryService.ScanNetworkStreamAsync(subnet, context.RequestAborted))
            {
                // µ¿Àû °´Ã¼¿¡¼­ type ¼Ó¼º È®ÀÎ
                var itemType = item.GetType();
                var typeProperty = itemType.GetProperty("type");
                if (typeProperty != null)
                {
                    var type = typeProperty.GetValue(item) as string;

                    if (type == "progress")
                    {
                        var scannedProperty = itemType.GetProperty("scanned");
                        var scanned = scannedProperty?.GetValue(item);

                        // ÁøÇà »óÈ² Àü¼Û
                        var message = $"progress: {scanned}\n\n";
                        await context.Response.WriteAsync(message);
                        await context.Response.Body.FlushAsync();
                    }
                    else if (type == "device")
                    {
                        var deviceProperty = itemType.GetProperty("device");
                        var device = deviceProperty?.GetValue(item);

                        // µð¹ÙÀÌ½º µ¥ÀÌÅÍ Àü¼Û
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
            // UDP ºê·ÎµåÄ³½ºÆ®
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

        // ¿Ï·á ½Ã±×³Î
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

// ¦¡¦¡ /api/Device/ProbeDevice ¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡
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

// ¦¡¦¡ /api/Device/Register ¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡
// ´Ü¼øÈ÷ IP ÁÖ¼Ò·Î µð¹ÙÀÌ½º µî·Ï (HTTPv2 ÇÁ·ÎÅäÄÝ¿¡ µû¶ó µð¹ÙÀÌ½º°¡ ¼­¹ö·Î Á¢¼ÓÇÔ)
app.MapPost("/api/Device/Register", (JsonNode? body, StateStore store) =>
{
    try
    {
        var ip = body?["IpAddress"]?.GetValue<string>();
        var sn = body?["DeviceSN"]?.GetValue<string>();

        if (string.IsNullOrWhiteSpace(ip))
        {
            return Results.Ok(BrowserApiResponse.Fail(3, "IpAddress is required."));
        }

        // SNÀÌ ¾øÀ¸¸é IP¸¦ SNÀ¸·Î »ç¿ë
        if (string.IsNullOrWhiteSpace(sn))
        {
            sn = $"Device-{ip.Replace(".", "-")}";
        }

        // ÀÌ¹Ì µî·ÏµÈ µð¹ÙÀÌ½ºÀÎÁö È®ÀÎ
        var existingDevices = store.GetDeviceSummaries();

        LogHub.Instance.Info($"µî·Ï ¿äÃ»: {sn} at {ip} (ÇöÀç {existingDevices.Count}°³ µð¹ÙÀÌ½º µî·ÏµÊ)");

        var existingBySN = existingDevices.FirstOrDefault(d => d.SN == sn);
        var existingByIP = existingDevices.FirstOrDefault(d => d.IpAddress == ip);

        // ¿ÏÀüÈ÷ µ¿ÀÏÇÑ µð¹ÙÀÌ½º (SN°ú IP ¸ðµÎ ÀÏÄ¡)
        if (existingBySN != null && existingBySN.IpAddress == ip)
        {
            LogHub.Instance.Warn($"µð¹ÙÀÌ½º Áßº¹ µî·Ï ½Ãµµ: {sn} at {ip} (ÀÌ¹Ì µî·ÏµÊ)");
            return Results.Ok(BrowserApiResponse.Fail(409, $"µð¹ÙÀÌ½º°¡ ÀÌ¹Ì µî·ÏµÇ¾î ÀÖ½À´Ï´Ù. (SN: {sn}, IP: {ip})"));
        }

        // SNÀº °°Áö¸¸ IP°¡ ´Ù¸§ (Keepalive·Î ÀÚµ¿ µî·ÏµÈ °æ¿ì) ¡æ IP ¾÷µ¥ÀÌÆ®
        if (existingBySN != null && existingBySN.IpAddress != ip)
        {
            LogHub.Instance.Info($"µð¹ÙÀÌ½º IP ¾÷µ¥ÀÌÆ®: {sn} ({existingBySN.IpAddress ?? "(¾øÀ½)"} ¡æ {ip})");
            store.ConnectDevice(sn, ip, 80, sn, "", null, null, 0);
            return Results.Ok(BrowserApiResponse.Ok($"µð¹ÙÀÌ½º {sn}ÀÇ IP ÁÖ¼Ò°¡ {ip}·Î ¾÷µ¥ÀÌÆ®µÇ¾ú½À´Ï´Ù."));
        }

        // IP´Â °°Áö¸¸ SNÀÌ ´Ù¸§ ¡æ ±âÁ¸ µð¹ÙÀÌ½º Á¦°Å ÈÄ »õ·Î µî·Ï
        if (existingByIP != null && existingByIP.SN != sn)
        {
            LogHub.Instance.Info($"IP Áßº¹ ¹ß°ß: {ip}¿¡ ±âÁ¸ µð¹ÙÀÌ½º {existingByIP.SN} ÀÖÀ½ ¡æ Á¦°Å ÈÄ {sn} µî·Ï");
            store.RemoveDevice(existingByIP.SN);
        }

        // IP ÁÖ¼Ò¸¸À¸·Î µð¹ÙÀÌ½º µî·Ï (Æ÷Æ® 80 ±âº»°ª)
        // µð¹ÙÀÌ½º´Â HTTPv2 ÇÁ·ÎÅäÄÝ¿¡ µû¶ó ÀÌ ¼­¹öÀÇ 80¹ø Æ÷Æ®·Î Keepalive¸¦ º¸³¿
        store.ConnectDevice(sn, ip, 80, sn, "", null, null, 0);

        LogHub.Instance.Info($"µð¹ÙÀÌ½º µî·Ï: {sn} at {ip} (HTTPv2 ÇÁ·ÎÅäÄÝ ´ë±â)");

        return Results.Ok(BrowserApiResponse.Ok($"µð¹ÙÀÌ½º {sn}ÀÌ(°¡) ¼º°øÀûÀ¸·Î µî·ÏµÇ¾ú½À´Ï´Ù."));
    }
    catch (Exception ex)
    {
        return Results.Ok(BrowserApiResponse.Fail(500, $"µî·Ï ½ÇÆÐ: {ex.Message}"));
    }
});

// ¦¡¦¡ /api/Device/Connect ¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡
app.MapPost("/api/Device/Connect", (JsonNode? body, StateStore store) =>
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

// ¦¡¦¡ /api/Device/GetDetail ¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡
app.MapGet("/api/Device/GetDetail", (StateStore store) =>
{
    var device = store.GetDeviceSummaries().FirstOrDefault();
    return Results.Ok(BrowserApiResponse.Ok(device));
});

// ¦¡¦¡ /api/Device/UpdateParameter ¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡
app.MapPost("/api/Device/UpdateParameter", async (HttpRequest request, StateStore store) =>
{
    var payload = await JsonNode.ParseAsync(request.Body);
    if (payload is not JsonObject setting)
        return Results.Ok(BrowserApiResponse.Fail(10004, "json parse error"));
    var sn = setting["DeviceSN"]?.GetValue<string>() ?? setting["SN"]?.GetValue<string>() ?? "";
    if (!string.IsNullOrWhiteSpace(sn))
        store.SetDesiredWorkSetting(sn, setting);
    return Results.Ok(BrowserApiResponse.Ok());
});

// ¦¡¦¡ /api/Device/Remote ¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡
app.MapPost("/api/Device/Remote", (DeviceRemoteRequest cmd, StateStore store) =>
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

// ¦¡¦¡ /api/Device/UploadSoftware ¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡
app.MapPost("/api/Device/UploadSoftware", () =>
    Results.Ok(BrowserApiResponse.Ok()));

// ¦¡¦¡ /api/People/Search ¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡
app.MapPost("/api/People/Search", (PeopleSearchRequest req, StateStore store) =>
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

// ¦¡¦¡ /api/People/GetDetail ¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡
app.MapPost("/api/People/GetDetail", (JsonNode? body, StateStore store) =>
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

// ¦¡¦¡ /api/People/GetNewID ¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡
app.MapGet("/api/People/GetNewID", (StateStore store) =>
{
    var all = store.GetPeople();
    var maxId = all.Select(p => long.TryParse(p.UserID, out var n) ? n : 0L).DefaultIfEmpty(0).Max();
    return Results.Ok(BrowserApiResponse.Ok((maxId + 1).ToString()));
});

// ¦¡¦¡ /api/People/New ¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡
app.MapPost("/api/People/New", async (HttpRequest request, StateStore store) =>
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
    return store.TryAddPerson(normalized)
        ? Results.Ok(BrowserApiResponse.Ok())
        : Results.Ok(BrowserApiResponse.Fail(23, "UserID or card number duplicated."));
});

// ¦¡¦¡ /api/People/Update ¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡
app.MapPost("/api/People/Update", async (HttpRequest request, StateStore store) =>
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

// ¦¡¦¡ /api/People/Delete ¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡
app.MapPost("/api/People/Delete", (JsonNode? body, StateStore store) =>
{
    var userId = body?["UserID"]?.GetValue<string>();
    if (string.IsNullOrWhiteSpace(userId))
        return Results.Ok(BrowserApiResponse.Fail(3, "UserID is required."));
    return store.DeletePerson(userId)
        ? Results.Ok(BrowserApiResponse.Ok())
        : Results.Ok(BrowserApiResponse.Fail(3, "Person not found."));
});

// ¦¡¦¡ /api/Department/Search ¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡
app.MapPost("/api/Department/Search", (DepartmentSearchRequest req, StateStore store) =>
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

app.MapGet("/api/Department/GetNewID", (StateStore store) =>
{
    var all = store.GetDepartments();
    var maxId = all.Select(d => long.TryParse(d.DepartmentID, out var n) ? n : 0L).DefaultIfEmpty(0).Max();
    return Results.Ok(BrowserApiResponse.Ok((maxId + 1).ToString()));
});

app.MapPost("/api/Department/New", (DepartmentInfo dept, StateStore store) =>
{
    if (string.IsNullOrWhiteSpace(dept.DepartmentID))
        return Results.Ok(BrowserApiResponse.Fail(3, "DepartmentID is required."));
    return store.TryAddDepartment(dept)
        ? Results.Ok(BrowserApiResponse.Ok())
        : Results.Ok(BrowserApiResponse.Fail(23, "Department already exists."));
});

app.MapPost("/api/Department/Delete", (JsonNode? body, StateStore store) =>
{
    var id = body?["DepartmentID"]?.GetValue<string>();
    if (string.IsNullOrWhiteSpace(id))
        return Results.Ok(BrowserApiResponse.Fail(3, "DepartmentID is required."));
    return store.DeleteDepartment(id)
        ? Results.Ok(BrowserApiResponse.Ok())
        : Results.Ok(BrowserApiResponse.Fail(3, "Department not found."));
});

// ¦¡¦¡ /api/Attendance/* ¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡
app.MapPost("/api/Attendance/Search", (AttendanceSearchRequest req, StateStore store) =>
{
    var records = store.GetDeviceSummaries()
        .SelectMany(d => store.GetDevice(d.SN)?.Records ?? new())
        .Where(r => r.RecordDetail?["RecordType"] is not null)
        .AsEnumerable();

    if (!string.IsNullOrWhiteSpace(req.UserID))
        records = records.Where(r => 
            string.Equals(r.RecordDetail?["UserID"]?.GetValue<string>(), req.UserID, StringComparison.OrdinalIgnoreCase));

    if (!string.IsNullOrWhiteSpace(req.UserName))
        records = records.Where(r => 
            (r.RecordDetail?["UserName"]?.GetValue<string>() ?? "").Contains(req.UserName, StringComparison.OrdinalIgnoreCase));

    if (!string.IsNullOrWhiteSpace(req.DepartmentID))
        records = records.Where(r => 
            string.Equals(r.RecordDetail?["DepartmentID"]?.GetValue<string>(), req.DepartmentID, StringComparison.OrdinalIgnoreCase));

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

    var list = records.Select(r => new AttendanceRecord
    {
        UserID = r.RecordDetail?["UserID"]?.GetValue<string>() ?? "",
        UserName = r.RecordDetail?["UserName"]?.GetValue<string>() ?? "",
        DepartmentID = r.RecordDetail?["DepartmentID"]?.GetValue<string>() ?? "",
        DepartmentName = r.RecordDetail?["DepartmentName"]?.GetValue<string>() ?? "",
        RecordTime = r.RecordDetail?["RecordTime"]?.GetValue<string>() ?? "",
        DeviceSN = r.RecordDetail?["DeviceSN"]?.GetValue<string>() ?? "",
        RecordType = r.RecordDetail?["RecordType"]?.GetValue<int>() ?? 0,
        Temperature = r.RecordDetail?["Temperature"]?.GetValue<string>(),
        PhotoUrl = r.RecordDetail?["PhotoUrl"]?.GetValue<string>()
    }).ToList();

    var page = Math.Max(1, req.PageIndex);
    var size = Math.Max(1, req.PageSize);

    return Results.Ok(BrowserApiResponse.Ok(new AttendanceSearchResult
    {
        TotalCount = list.Count,
        PageIndex = page,
        PageSize = size,
        DataList = list.Skip((page - 1) * size).Take(size).ToList()
    }));
});

app.MapPost("/api/Attendance/Statistics", (AttendanceSearchRequest req, StateStore store) =>
{
    var records = store.GetDeviceSummaries()
        .SelectMany(d => store.GetDevice(d.SN)?.Records ?? new())
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

// ¦¡¦¡ /api/Record/* ¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡
app.MapPost("/api/Record/Identify/Search", (RecordSearchRequest req, StateStore store) =>
{
    var all = store.GetDeviceSummaries()
        .SelectMany(d => store.GetDevice(d.SN)?.Records ?? new())
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

app.MapPost("/api/Record/DoorSensor/Search", (RecordSearchRequest req, StateStore store) =>
    Results.Ok(BrowserApiResponse.Ok(new { TotalCount = 0, PageIndex = req.PageIndex, PageSize = req.PageSize, DataList = Array.Empty<object>() })));

app.MapPost("/api/Record/System/Search", (RecordSearchRequest req, StateStore store) =>
    Results.Ok(BrowserApiResponse.Ok(new { TotalCount = 0, PageIndex = req.PageIndex, PageSize = req.PageSize, DataList = Array.Empty<object>() })));

app.MapPost("/api/Record/Delete/All", (StateStore store) =>
{
    store.ClearAllRecords();
    return Results.Ok(BrowserApiResponse.Ok());
});

app.MapPost("/api/Record/Delete/ByType", (DeleteRecordsByTypeRequest req, StateStore store) =>
{
    store.ClearRecordsByType(req.RecordType);
    return Results.Ok(BrowserApiResponse.Ok());
});

// HTTP ¼­¹ö¸¦ ¹é±×¶ó¿îµå¿¡¼­ ½ÇÇà
var cts = new CancellationTokenSource();

// Windows ¹æÈ­º® Ã¼Å©
try
{
    var exePath = System.Diagnostics.Process.GetCurrentProcess().MainModule?.FileName ?? "";
    if (!string.IsNullOrEmpty(exePath))
    {
        if (!FirewallHelper.CheckFirewallRule("FDDC UDP Discovery"))
        {
            LogHub.Instance.Warn("?? Windows ¹æÈ­º® ±ÔÄ¢ÀÌ ¾ø½À´Ï´Ù. UDP ºê·ÎµåÄ³½ºÆ® °Ë»öÀÌ Â÷´ÜµÉ ¼ö ÀÖ½À´Ï´Ù.");
            LogHub.Instance.Info("¹æÈ­º® ±ÔÄ¢ Ãß°¡¸¦ ½ÃµµÇÕ´Ï´Ù...");

            if (FirewallHelper.AddUdpFirewallRule(exePath))
            {
                LogHub.Instance.Info("? ¹æÈ­º® ±ÔÄ¢ÀÌ Ãß°¡µÇ¾ú½À´Ï´Ù.");
            }
            else
            {
                LogHub.Instance.Warn("?? ¹æÈ­º® ±ÔÄ¢ Ãß°¡ ½ÇÆÐ. ¼öµ¿À¸·Î Ãß°¡ÇØ¾ß ÇÕ´Ï´Ù:");
                LogHub.Instance.Warn(FirewallHelper.GetManualFirewallInstructions());
            }
        }
        else
        {
            LogHub.Instance.Info("? Windows ¹æÈ­º® ±ÔÄ¢ÀÌ ¼³Á¤µÇ¾î ÀÖ½À´Ï´Ù.");
        }
    }
}
catch (Exception ex)
{
    LogHub.Instance.Warn($"¹æÈ­º® Ã¼Å© ½ÇÆÐ: {ex.Message}");
}

var serverTask = app.RunAsync(cts.Token);

// ¼­¹ö URL °¡Á®¿À±â ¹× ºê¶ó¿ìÀú¿ëÀ¸·Î º¯È¯
var urls = app.Urls.ToArray();
var rawUrl = urls.Length > 0 ? urls[0] : "http://localhost:8100";
// 0.0.0.0À» localhost·Î º¯È¯ (ºê¶ó¿ìÀú¿¡¼­ »ç¿ë °¡´ÉÇÏµµ·Ï)
var serverUrl = rawUrl.Replace("0.0.0.0", "localhost");

// MainForm »ý¼º ¹× ¼­¹ö URL ¼³Á¤
var mainForm = new MainForm();
mainForm.SetServerUrl(serverUrl);
mainForm.FormClosed += (_, _) =>
{
    cts.Cancel();
};

// Windows Forms ½ÇÇà
Application.Run(mainForm);

// ¼­¹ö Á¾·á ´ë±â
await serverTask;

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

static IResult DownloadPeopleList(DownloadPeopleListRequest request, StateStore store)
{
    if (string.IsNullOrWhiteSpace(request.SN))
    {
        return Results.BadRequest(new ApiResponse(400, "SN is required."));
    }

    var people = store.GetPeopleForDownload(request.SN, request.Limit).ToList();
    return Results.Ok(new DownloadPeopleListResponse
    {
        PeopleCount = people.Count,
        PeopleList = people
    });
}

static IResult SelectDeleteInfo(SelectDeleteInfoRequest request, StateStore store)
{
    if (string.IsNullOrWhiteSpace(request.SN))
    {
        return Results.BadRequest(new ApiResponse(400, "SN is required."));
    }

    return Results.Ok(new SelectDeleteInfoResponse
    {
        DeleteList = store.GetDeletePeople(request.SN).ToList()
    });
}

static PersonInfo NormalizePerson(PersonInfo person) => new()
{
    UserID = person.UserID?.Trim() ?? string.Empty,
    Name = person.Name?.Trim() ?? string.Empty,
    Job = person.Job?.Trim() ?? string.Empty,
    Department = person.Department?.Trim() ?? string.Empty,
    IdentityCard = person.IdentityCard?.Trim() ?? string.Empty,
    Attachment = person.Attachment?.Trim() ?? string.Empty,
    Photo = person.Photo?.Trim() ?? string.Empty,
    PhotoMD5 = person.PhotoMD5?.Trim() ?? string.Empty,
    PhotoLen = person.PhotoLen,
    Password = person.Password?.Trim() ?? string.Empty,
    CardNum = person.CardNum?.Trim() ?? string.Empty,
    QRCode = person.QRCode?.Trim() ?? string.Empty,
    AccessType = Math.Clamp(person.AccessType, 0, 2),
    ExpirationDate = person.ExpirationDate,
    OpenTimes = person.OpenTimes <= 0 ? 65535 : person.OpenTimes,
    KeepOpen = person.KeepOpen,
    Timegroup = person.Timegroup,
    Holidays = person.Holidays?.Trim() ?? string.Empty,
    Elevators = person.Elevators?.Trim() ?? string.Empty,
    FaceFeature = person.FaceFeature?.Trim() ?? string.Empty,
    FaceFeatureMD5 = person.FaceFeatureMD5?.Trim() ?? string.Empty,
    Fingerprints = person.Fingerprints ?? new(),
    Palmveins = person.Palmveins ?? new()
};

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
