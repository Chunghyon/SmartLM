using System.IO.Compression;
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

// Windows Forms 蟾晦��
Application.EnableVisualStyles();
Application.SetCompatibleTextRenderingDefault(false);
Application.SetHighDpiMode(HighDpiMode.SystemAware);

var builder = WebApplication.CreateBuilder(args);

// ContentRootPath蒂 ⑷營 褒ч 蛤滓饜葬煎 撲薑
var contentRoot = AppDomain.CurrentDomain.BaseDirectory;
builder.Environment.ContentRootPath = contentRoot;
builder.Configuration.SetBasePath(contentRoot);

// WebRootPath 撲薑
var webRoot = Path.Combine(contentRoot, "wwwroot");
if (Directory.Exists(webRoot))
{
    builder.Environment.WebRootPath = webRoot;
}
else
{
    // 偃嫦 �秣瞈□韭� Щ煎薛お 瑞お曖 wwwroot 餌辨
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

var app = builder.Build();

// HTTP 蹂羶 煎梵 嘐菟錚橫 蹺陛
app.UseMiddleware<HttpLoggingMiddleware>();

// 薑瞳 だ橾 憮綠蝶 掘撩
app.UseDefaultFiles();
app.UseStaticFiles();

// Favicon 404 寞雖
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

app.MapPost("/Device/Keepalive", (KeepaliveRequest request, StateStore store) =>
{
    if (string.IsNullOrWhiteSpace(request.SN))
    {
        return Results.BadRequest(new ApiResponse(400, "SN is required."));
    }

    var response = store.UpsertKeepalive(request);
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
    return workSetting is null
        ? Results.Ok(new ApiResponse(404, "No work-setting snapshot is available for this device yet."))
        : Results.Ok(workSetting);
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

// 式式 Admin: remote command dispatch 式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式
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

// 式式 Admin: departments 式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式
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
// HTTP-Docking Protocol  (Device ⊥ Server)
// ???????????????????????????????????????????????????????????????????????????????

// 式式 /Device/RemoteCommand 式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式
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

// 式式 /People/DownloadPeopleListResult 式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式
app.MapPost("/People/DownloadPeopleListResult", (DownloadPeopleListResultRequest request, StateStore store) =>
{
    if (string.IsNullOrWhiteSpace(request.SN))
        return Results.BadRequest(new ApiResponse(400, "SN is required."));
    return Results.Ok(ApiResponse.Ok());
});

// 式式 /People/DeletePeopleList 式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式
app.MapPost("/People/DeletePeopleList", (DeletePeopleListRequest request, StateStore store) =>
{
    if (string.IsNullOrWhiteSpace(request.SN))
        return Results.BadRequest(new ApiResponse(400, "SN is required."));

    var effectiveLimit = request.Limit <= 0 ? 50 : Math.Min(request.Limit, 1000);
    var list = store.GetDeletePeople(request.SN).Take(effectiveLimit).ToList();
    return Results.Ok(new DeletePeopleListResponse { DeleteList = list });
});

// 式式 /People/DeletePeopleListResult 式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式
app.MapPost("/People/DeletePeopleListResult", (DeletePeopleListResultRequest request, StateStore store) =>
{
    if (string.IsNullOrWhiteSpace(request.SN))
        return Results.BadRequest(new ApiResponse(400, "SN is required."));
    return Results.Ok(ApiResponse.Ok());
});

// 式式 /People/PushPeople  (device uploads its stored people to server) 式式式式式式式式式式
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

// 式式 /Record/UploadSystemRecord 式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式
app.MapPost("/Record/UploadSystemRecord", (UploadSystemRecordRequest request, StateStore store) =>
{
    if (string.IsNullOrWhiteSpace(request.SN))
        return Results.BadRequest(new ApiResponse(400, "SN is required."));

    store.SaveSystemRecords(request.SN, request.RecordType, request.Records ?? new());
    return Results.Ok(ApiResponse.Ok());
});

// ???????????????????????????????????????????????????????????????????????????????
// Browser UI Protocol  (Browser ⊥ Server acting as device proxy)
// ???????????????????????????????????????????????????????????????????????????????

// 式式 /api/heartBeat 式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式
app.MapGet("/api/heartBeat", () => Results.Ok(BrowserApiResponse.Ok("OK")));

// 式式 /api/GetDeviceSN 式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式
app.MapGet("/api/GetDeviceSN", (StateStore store) =>
{
    var devices = store.GetDeviceSummaries();
    var sn = devices.FirstOrDefault()?.SN ?? "UNKNOWN";
    return Results.Ok(BrowserApiResponse.Ok(sn));
});

// 式式 /api/User/Login 式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式
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

// 式式 /api/Device/FunctionList 式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式
app.MapGet("/api/Device/FunctionList", () => Results.Ok(BrowserApiResponse.Ok(new
{
    FaceIR = true, BodyTemperature = false, Elevator = true,
    FaceMask = true, AlarmClock = true, ExcelFile = true, ZipFile = true,
    TimeGreoup = true, WIFI = true,
    HTTPClient_V1 = true, HTTPClient_V2 = true, MQTT = true,
    Websocket_V1 = true, Websocket_V2 = true
})));

// 式式 /api/Device/Search 式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式
app.MapPost("/api/Device/Search", async (DeviceDiscoveryService discoveryService, JsonNode? body) =>
{
    try
    {
        var searchType = body?["SearchType"]?.GetValue<string>() ?? "broadcast";
        var subnet = body?["Subnet"]?.GetValue<string>();

        List<DeviceDiscoveryService.DiscoveredDevice> devices;

        if (searchType.Equals("scan", StringComparison.OrdinalIgnoreCase) && !string.IsNullOrWhiteSpace(subnet))
        {
            // IP 彰嬪 蝶警
            devices = await discoveryService.ScanNetworkAsync(subnet);
        }
        else
        {
            // UDP 粽煎萄議蝶お
            devices = await discoveryService.SearchDevicesAsync();
        }

        return Results.Ok(BrowserApiResponse.Ok(devices));
    }
    catch (Exception ex)
    {
        return Results.Ok(BrowserApiResponse.Fail(500, $"Search failed: {ex.Message}"));
    }
});

// 式式 /api/Device/ProbeDevice 式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式
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

// 式式 /api/Device/Connect 式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式
app.MapPost("/api/Device/Connect", (JsonNode? body, StateStore store) =>
{
    try
    {
        var sn = body?["DeviceSN"]?.GetValue<string>();
        var ip = body?["IpAddress"]?.GetValue<string>();
        var port = body?["HttpPort"]?.GetValue<int>() ?? 80;
        var name = body?["DeviceName"]?.GetValue<string>();
        var model = body?["Model"]?.GetValue<string>();
        var firmware = body?["FirmwareVersion"]?.GetValue<string>();

        if (string.IsNullOrWhiteSpace(sn) || string.IsNullOrWhiteSpace(ip))
        {
            return Results.Ok(BrowserApiResponse.Fail(3, "DeviceSN and IpAddress are required."));
        }

        store.ConnectDevice(sn, ip, port, name, model, firmware);
        return Results.Ok(BrowserApiResponse.Ok($"Device {sn} connected successfully"));
    }
    catch (Exception ex)
    {
        return Results.Ok(BrowserApiResponse.Fail(500, $"Connection failed: {ex.Message}"));
    }
});

// 式式 /api/Device/GetDetail 式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式
app.MapGet("/api/Device/GetDetail", (StateStore store) =>
{
    var device = store.GetDeviceSummaries().FirstOrDefault();
    return Results.Ok(BrowserApiResponse.Ok(device));
});

// 式式 /api/Device/UpdateParameter 式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式
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

// 式式 /api/Device/Remote 式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式
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

// 式式 /api/Device/UploadSoftware 式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式
app.MapPost("/api/Device/UploadSoftware", () =>
    Results.Ok(BrowserApiResponse.Ok()));

// 式式 /api/People/Search 式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式
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

// 式式 /api/People/GetDetail 式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式
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

// 式式 /api/People/GetNewID 式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式
app.MapGet("/api/People/GetNewID", (StateStore store) =>
{
    var all = store.GetPeople();
    var maxId = all.Select(p => long.TryParse(p.UserID, out var n) ? n : 0L).DefaultIfEmpty(0).Max();
    return Results.Ok(BrowserApiResponse.Ok((maxId + 1).ToString()));
});

// 式式 /api/People/New 式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式
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

// 式式 /api/People/Delete 式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式
app.MapPost("/api/People/Delete", (JsonNode? body, StateStore store) =>
{
    var userId = body?["UserID"]?.GetValue<string>();
    if (string.IsNullOrWhiteSpace(userId))
        return Results.Ok(BrowserApiResponse.Fail(3, "UserID is required."));
    return store.DeletePerson(userId)
        ? Results.Ok(BrowserApiResponse.Ok())
        : Results.Ok(BrowserApiResponse.Fail(3, "Person not found."));
});

// 式式 /api/Department/Search 式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式
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

// 式式 /api/Attendance/* 式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式
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

// 式式 /api/Record/* 式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式
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

// HTTP 憮幗蒂 寥斜塭遴萄縑憮 褒ч
var cts = new CancellationTokenSource();
var serverTask = app.RunAsync(cts.Token);

// 憮幗 URL 陛螳螃晦 塽 粽塭辦盪辨戲煎 滲��
var urls = app.Urls.ToArray();
var rawUrl = urls.Length > 0 ? urls[0] : "http://localhost:8100";
// 0.0.0.0擊 localhost煎 滲�� (粽塭辦盪縑憮 餌辨 陛棟ж紫煙)
var serverUrl = rawUrl.Replace("0.0.0.0", "localhost");

// MainForm 儅撩 塽 憮幗 URL 撲薑
var mainForm = new MainForm();
mainForm.SetServerUrl(serverUrl);
mainForm.FormClosed += (_, _) =>
{
    cts.Cancel();
};

// Windows Forms 褒ч
Application.Run(mainForm);

// 憮幗 謙猿 渠晦
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
