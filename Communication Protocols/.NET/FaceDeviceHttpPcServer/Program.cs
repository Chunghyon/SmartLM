using System.IO.Compression;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using FaceDeviceHttpPcServer.Models;
using FaceDeviceHttpPcServer.Services;

const byte GzipMagicByte1 = 0x1F;
const byte GzipMagicByte2 = 0x8B;
var browserSessionLifetime = TimeSpan.FromHours(24);
const long UnixMillisecondsThreshold = 10_000_000_000;

var builder = WebApplication.CreateBuilder(args);

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

var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();

app.MapGet("/", (StateStore store) => Results.Ok(new
{
    name = "FaceDeviceHttpPcServer",
    purpose = "HTTP integration server and browser-UI emulator for face-recognition terminals",
    primaryDeviceSn = store.GetPrimaryDeviceSn(),
    endpoints = new[]
    {
        "/Device/Keepalive",
        "/Device/UploadWorkSetting",
        "/Device/DownloadWorkSetting",
        "/Device/RemoteCommand",
        "/People/DownloadPeopleList",
        "/People/DownloadPeopleListResult",
        "/People/DeletePeopleList",
        "/People/DeletePeopleListResult",
        "/People/PushPeople",
        "/Record/UploadIdentifyRecord",
        "/Record/UploadSystemRecord",
        "/api/heartBeat",
        "/api/GetDeviceSN",
        "/api/User/Login",
        "/api/Device/FunctionList",
        "/api/Device/GetDetail",
        "/api/Device/UpdateParameter",
        "/api/Device/Remote",
        "/api/People/Search",
        "/api/People/GetDetail",
        "/api/People/New",
        "/api/Department/Search",
        "/api/Record/Identify/Search",
        "/admin"
    }
}));

app.MapGet("/admin", () => Results.Redirect("/admin/"));

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
    var payload = await ReadJsonObjectAsync(request);
    if (payload is null)
    {
        return Results.BadRequest(new ApiResponse(400, "JSON object is required."));
    }

    var deviceSn = payload["DeviceSN"]?.GetValue<string>()
        ?? payload["SN"]?.GetValue<string>()
        ?? store.GetPrimaryDeviceSn();

    store.SaveUploadedWorkSetting(deviceSn, payload);
    return Results.Ok(ApiResponse.Ok());
});

app.MapPost("/Device/DownloadWorkSetting", async (HttpRequest request, StateStore store) =>
{
    var payload = await ReadJsonObjectAsync(request);
    var sn = payload?["SN"]?.GetValue<string>() ?? store.GetPrimaryDeviceSn();
    if (string.IsNullOrWhiteSpace(sn))
    {
        return Results.BadRequest(new ApiResponse(400, "SN is required."));
    }

    var workSetting = store.GetWorkSettingForDownload(sn);
    return workSetting is null
        ? Results.Ok(new ApiResponse(404, "No work-setting snapshot is available for this device yet."))
        : Results.Ok(workSetting);
});

app.MapPost("/Device/RemoteCommand", async (HttpRequest request, StateStore store) =>
{
    var payload = await ReadJsonObjectAsync(request);
    var sn = payload?["SN"]?.ToString() ?? store.GetPrimaryDeviceSn();
    if (string.IsNullOrWhiteSpace(sn))
    {
        return Results.BadRequest(new ApiResponse(400, "SN is required."));
    }

    return Results.Ok(store.ConsumeRemoteCommand(sn) ?? new JsonObject { ["Success"] = 0 });
});

app.MapPost("/People/DownloadPeopleList", DownloadPeopleList);
app.MapPost("/DevicePass/SelectPassInfo", DownloadPeopleList);

app.MapPost("/People/DownloadPeopleListResult", async (HttpRequest request, StateStore store) =>
{
    var payload = await ReadJsonObjectAsync(request);
    if (payload is null)
    {
        return Results.BadRequest(new ApiResponse(400, "JSON object is required."));
    }

    var sn = payload["SN"]?.ToString();
    if (string.IsNullOrWhiteSpace(sn))
    {
        return Results.BadRequest(new ApiResponse(400, "SN is required."));
    }

    store.SavePeopleDownloadResult(sn, payload);
    return Results.Ok(ApiResponse.Ok());
});

app.MapPost("/People/DeletePeopleList", SelectDeleteInfo);
app.MapPost("/DevicePass/SelectDeleteInfo", SelectDeleteInfo);
app.MapPost("/People/SelectDeleteInfo", SelectDeleteInfo);

app.MapPost("/People/DeletePeopleListResult", async (HttpRequest request, StateStore store) =>
{
    var payload = await ReadJsonObjectAsync(request);
    if (payload is null)
    {
        return Results.BadRequest(new ApiResponse(400, "JSON object is required."));
    }

    var sn = payload["SN"]?.ToString();
    if (string.IsNullOrWhiteSpace(sn))
    {
        return Results.BadRequest(new ApiResponse(400, "SN is required."));
    }

    store.SaveDeletePeopleResult(sn, payload);
    return Results.Ok(ApiResponse.Ok());
});

app.MapPost("/People/PushPeople", async (HttpRequest request, StateStore store) =>
{
    if (!request.HasFormContentType)
    {
        return Results.BadRequest(new ApiResponse(400, "multipart/form-data is required."));
    }

    var form = await request.ReadFormAsync();
    var sn = FirstNonEmpty(form["SN"].ToString(), form["DeviceSN"].ToString(), store.GetPrimaryDeviceSn());
    if (string.IsNullOrWhiteSpace(sn))
    {
        return Results.BadRequest(new ApiResponse(400, "SN is required."));
    }

    var pushType = ParseInt(FirstNonEmpty(form["PushType"].ToString(), "0"));
    var userId = FirstNonEmpty(form["UserID"].ToString(), form["userid"].ToString()) ?? string.Empty;
    var detailJson = FirstNonEmpty(form["Detail"].ToString(), await ReadMultipartValueAsync(form, "Detail"));
    JsonObject? detail = null;
    if (!string.IsNullOrWhiteSpace(detailJson))
    {
        detail = JsonNode.Parse(detailJson) as JsonObject;
        userId = string.IsNullOrWhiteSpace(userId) ? detail?["UserID"]?.ToString() ?? string.Empty : userId;
    }

    if (string.IsNullOrWhiteSpace(userId))
    {
        return Results.BadRequest(new ApiResponse(400, "UserID is required."));
    }

    var photo = form.Files.GetFile("Photo");
    store.SavePushedPerson(sn, pushType, userId, detail, photo);
    return Results.Ok(ApiResponse.Ok());
});

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
    var payload = await ReadJsonObjectAsync(request);
    if (payload is null)
    {
        return Results.BadRequest(new ApiResponse(400, "JSON object is required."));
    }

    var sn = payload["SN"]?.ToString() ?? payload["DeviceSN"]?.ToString() ?? store.GetPrimaryDeviceSn();
    if (string.IsNullOrWhiteSpace(sn))
    {
        return Results.BadRequest(new ApiResponse(400, "SN is required."));
    }

    var category = payload["Category"]?.ToString() ?? payload["RecordCategory"]?.ToString() ?? "System";
    var records = new List<JsonNode?>();
    if (payload["RecordList"] is JsonArray recordArray)
    {
        records.AddRange(recordArray.Select(item => item?.DeepClone()));
    }
    else if (payload["content"] is JsonArray contentArray)
    {
        records.AddRange(contentArray.Select(item => item?.DeepClone()));
    }
    else
    {
        records.Add(payload);
    }

    store.SaveSystemRecords(sn, category, records);
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
    var payload = await ReadJsonObjectAsync(request);
    if (payload is null)
    {
        return Results.BadRequest(new ApiResponse(400, "JSON object is required."));
    }

    payload["DeviceSN"] = sn;
    store.SetDesiredWorkSetting(sn, payload);
    return Results.Ok(ApiResponse.Ok("Desired work-setting snapshot saved."));
});

app.MapGet("/api/heartBeat", () => BrowserOk("OK"));

app.MapGet("/api/GetDeviceSN", (StateStore store) => BrowserOk(store.GetPrimaryDeviceSn()));

app.MapPost("/api/User/Login", async (HttpRequest request, StateStore store) =>
{
    var payload = await ReadJsonObjectAsync(request);
    var password = payload?["password"]?.ToString() ?? string.Empty;
    var deviceSn = store.GetPrimaryDeviceSn();
    if (!store.VerifyManagementPassword(deviceSn, password))
    {
        return BrowserError(1, "Wrong Password!");
    }

    var session = store.CreateSession(deviceSn, browserSessionLifetime);
    return BrowserOk(new
    {
        token = session.Token,
        expiration = session.ExpiresAtUtc.ToUnixTimeSeconds()
    });
});

app.MapGet("/api/User/Logout", (HttpRequest request, StateStore store) =>
{
    var token = ReadBearerToken(request);
    if (string.IsNullOrWhiteSpace(token) || store.RemoveSession(token) is false)
    {
        return BrowserError(10000, "Token is invalid");
    }

    return BrowserOk("ok");
});

app.MapGet("/api/User/CheckLoginToken", (HttpRequest request, StateStore store) =>
{
    var session = RequireSession(request, store, out var error);
    return session is null ? error! : BrowserOk(session.ExpiresAtUtc.ToUnixTimeSeconds());
});

app.MapGet("/api/User/TokenExtension", (HttpRequest request, StateStore store) =>
{
    var token = ReadBearerToken(request);
    if (string.IsNullOrWhiteSpace(token))
    {
        return BrowserError(10000, "Token is invalid");
    }

    var session = store.ExtendSession(token, browserSessionLifetime);
    return session is null ? BrowserError(10000, "Token is invalid") : BrowserOk(session.ExpiresAtUtc.ToUnixTimeSeconds());
});

app.MapPost("/api/User/EditPassword", async (HttpRequest request, StateStore store) =>
{
    var session = RequireSession(request, store, out var error);
    if (session is null)
    {
        return error!;
    }

    var payload = await ReadJsonObjectAsync(request);
    if (payload is null)
    {
        return BrowserError(10004, "The Json format in the request body is incorrect");
    }

    var oldPassword = payload["OldPassword"]?.ToString() ?? string.Empty;
    var newPassword = payload["NewPassword"]?.ToString() ?? string.Empty;
    if (!store.VerifyManagementPassword(session.DeviceSN, oldPassword))
    {
        return BrowserError(1, "old password error");
    }

    if (!string.IsNullOrWhiteSpace(newPassword) && newPassword.Length >= 4 && newPassword.Length <= 8 && newPassword.All(char.IsDigit))
    {
        store.UpdateManagementPassword(session.DeviceSN, newPassword);
        return BrowserOk();
    }

    return BrowserError(2, "The new password format is incorrect");
});

app.MapPost("/api/Device/FunctionList", (HttpRequest request, StateStore store) =>
{
    var session = RequireSession(request, store, out var error);
    if (session is null)
    {
        return error!;
    }

    return BrowserOk(new
    {
        BodyTemperature = true,
        Fingerprint = true,
        Palmvein = true,
        Face = true,
        QRCode = true,
        FaceMask = true,
        SafetyHelmet = false,
        Lift = true,
        AlarmClock = true,
        ExcelFile = false,
        ZipFile = false,
        TimeGreoup = true,
        WIFI = true,
        HTTPClient_V1 = true,
        HTTPClient_V2 = true,
        MQTT = true,
        YZW = false,
        Websocket_V1 = false,
        Websocket_V2 = false
    });
});

app.MapPost("/api/Device/GetDetail", async (HttpRequest request, StateStore store) =>
{
    var session = RequireSession(request, store, out var error);
    if (session is null)
    {
        return error!;
    }

    _ = await ReadJsonObjectAsync(request);
    return BrowserOk(store.GetMergedWorkSetting(session.DeviceSN));
});

app.MapPost("/api/Device/UpdateParameter", async (HttpRequest request, StateStore store) =>
{
    var session = RequireSession(request, store, out var error);
    if (session is null)
    {
        return error!;
    }

    var payload = await ReadJsonObjectAsync(request);
    if (payload is null)
    {
        return BrowserError(10004, "The Json format in the request body is incorrect");
    }

    store.UpdateWorkSetting(session.DeviceSN, payload);
    return BrowserOk();
});

app.MapPost("/api/Device/Remote", async (HttpRequest request, StateStore store) =>
{
    var session = RequireSession(request, store, out var error);
    if (session is null)
    {
        return error!;
    }

    var payload = await ReadJsonObjectAsync(request);
    if (payload is null)
    {
        return BrowserError(10004, "The Json format in the request body is incorrect");
    }

    var command = new JsonObject();
    if (payload["OpenDoor"]?.GetValue<bool>() == true) command["Opendoor"] = 1;
    if (payload["KeepOpen"]?.GetValue<bool>() == true) command["Opendoor"] = 2;
    if (payload["CloseDoor"]?.GetValue<bool>() == true) command["Opendoor"] = 3;
    if (payload["LockDoor"]?.GetValue<bool>() == true) command["Opendoor"] = 4;
    if (payload["UnlockDoor"]?.GetValue<bool>() == true) command["Opendoor"] = 5;
    if (payload["CloseAlarm"]?.GetValue<bool>() == true) command["Closealarm"] = 1;
    if (payload["Restart"]?.GetValue<bool>() == true) command["Restart"] = 1;
    if (payload["Recover"]?.GetValue<bool>() == true) command["Recover"] = 1;
    if (payload["FireAlarm"]?.GetValue<bool>() == true) command["FireAlarm"] = 1;

    if (!command.Any())
    {
        return BrowserError(1, "no action to be performed");
    }

    store.QueueRemoteCommand(session.DeviceSN, command);
    return BrowserOk();
});

app.MapPost("/api/Device/UploadSoftware", async (HttpRequest request, StateStore store) =>
{
    var session = RequireSession(request, store, out var error);
    if (session is null)
    {
        return error!;
    }

    if (!request.HasFormContentType)
    {
        return BrowserError(10002, "Request Content-Type error");
    }

    var form = await request.ReadFormAsync();
    var softwareMd5 = FirstNonEmpty(form["SoftwareMD5"].ToString(), form["softwareMD5"].ToString()) ?? string.Empty;
    var file = form.Files.GetFile("SoftwareFile") ?? form.Files.GetFile("softwareFile");
    if (string.IsNullOrWhiteSpace(softwareMd5) || file is null)
    {
        return BrowserError(3, "Request parameter error");
    }

    store.SaveFirmware(session.DeviceSN, softwareMd5, file);
    return BrowserOk();
});

app.MapPost("/api/People/Search", async (HttpRequest request, StateStore store) =>
{
    var session = RequireSession(request, store, out var error);
    if (session is null)
    {
        return error!;
    }

    var payload = await ReadJsonObjectAsync(request) ?? new JsonObject();
    var pageIndex = Math.Max(ParseInt(payload["PageIndex"]?.ToString(), 1), 1);
    var pageSize = Math.Clamp(ParseInt(payload["PageSize"]?.ToString(), 20), 1, 1000);
    var people = store.GetPeople().AsEnumerable();

    people = FilterText(people, payload, "UserID", person => person.UserID);
    people = FilterText(people, payload, "Name", person => person.Name);
    people = FilterText(people, payload, "UserName", person => person.Name);
    people = FilterText(people, payload, "Job", person => person.Job);
    people = FilterText(people, payload, "Department", person => person.Department);
    people = FilterText(people, payload, "CardNum", person => person.CardNum);
    people = FilterText(people, payload, "IdentityCard", person => person.IdentityCard);

    if (payload["AccessType"] is not null)
    {
        var accessType = ParseInt(payload["AccessType"]?.ToString());
        people = people.Where(person => person.AccessType == accessType);
    }

    var totalCount = people.Count();
    var dataList = people
        .Skip((pageIndex - 1) * pageSize)
        .Take(pageSize)
        .Select(person => new
        {
            person.UserID,
            person.Name,
            person.Job,
            person.Department,
            person.Password,
            person.CardNum,
            person.AccessType,
            person.ExpirationDate,
            person.OpenTimes,
            person.KeepOpen,
            person.Timegroup,
            FaceFeature = string.IsNullOrWhiteSpace(person.FaceFeature) ? 0 : 1,
            photo = person.Photo,
            Fingerprint = person.Fingerprints.Count > 0 ? 1 : 0,
            Palmprint = person.Palmveins.Count > 0 ? 1 : 0
        })
        .ToArray();

    return BrowserOk(new
    {
        TotalCount = totalCount,
        PageIndex = pageIndex,
        PageSize = pageSize,
        DataList = dataList
    });
});

app.MapPost("/api/People/GetDetail", async (HttpRequest request, StateStore store) =>
{
    var session = RequireSession(request, store, out var error);
    if (session is null)
    {
        return error!;
    }

    var payload = await ReadJsonObjectAsync(request);
    var userId = payload?["UserID"]?.ToString() ?? string.Empty;
    var person = store.GetPerson(userId);
    if (person is null)
    {
        return BrowserError(404, "Person not found");
    }

    if (payload?["PhotoBase64"]?.GetValue<int>() == 1 && !string.IsNullOrWhiteSpace(person.Photo) && File.Exists(person.Photo))
    {
        person.Photo = Convert.ToBase64String(await File.ReadAllBytesAsync(person.Photo));
    }

    return BrowserOk(person);
});

app.MapPost("/api/People/GetNewID", (HttpRequest request, StateStore store) =>
{
    var session = RequireSession(request, store, out var error);
    return session is null ? error! : BrowserOk(new { NewUserID = store.GetNextUserId() });
});

app.MapPost("/api/People/New", async (HttpRequest request, StateStore store) =>
{
    var session = RequireSession(request, store, out var error);
    if (session is null)
    {
        return error!;
    }

    if (!request.HasFormContentType)
    {
        return BrowserError(3, "Request parameter error");
    }

    var form = await request.ReadFormAsync();
    var peopleJson = FirstNonEmpty(form["PeopleJson"].ToString(), await ReadMultipartValueAsync(form, "PeopleJson"));
    if (string.IsNullOrWhiteSpace(peopleJson))
    {
        return BrowserError(3, "Request parameter error");
    }

    JsonObject? payload;
    try
    {
        payload = JsonNode.Parse(peopleJson) as JsonObject;
    }
    catch (JsonException)
    {
        return BrowserError(10, "JSON parsing failed");
    }

    if (payload is null)
    {
        return BrowserError(10, "JSON parsing failed");
    }

    var person = payload.Deserialize<PersonInfo>(new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new PersonInfo();
    if (string.IsNullOrWhiteSpace(person.Name))
    {
        person.Name = payload["UserName"]?.ToString() ?? string.Empty;
    }

    var photo = form.Files.GetFile("Photo");
    if (photo is not null)
    {
        person.Photo = await SavePhotoFromFormAsync(session.DeviceSN, person.UserID, photo);
        person.PhotoLen = (int)photo.Length;
    }

    store.UpsertPerson(NormalizePerson(person));
    return BrowserOk();
});

app.MapPost("/api/People/Delete", async (HttpRequest request, StateStore store) =>
{
    var session = RequireSession(request, store, out var error);
    if (session is null)
    {
        return error!;
    }

    var payload = await ReadJsonObjectAsync(request) ?? new JsonObject();
    if (payload["DeleteALL"]?.GetValue<int>() == 1)
    {
        store.DeleteAllPeople();
        return BrowserOk();
    }

    var ids = payload["UserIDs"] as JsonArray;
    var deleted = store.DeletePeople(ids?.Select(item => item?.ToString() ?? string.Empty) ?? Array.Empty<string>());
    return deleted >= 0 ? BrowserOk() : BrowserError(25, "Error in querying data");
});

app.MapPost("/api/Department/Search", async (HttpRequest request, StateStore store) =>
{
    var session = RequireSession(request, store, out var error);
    if (session is null)
    {
        return error!;
    }

    var payload = await ReadJsonObjectAsync(request) ?? new JsonObject();
    var pageIndex = Math.Max(ParseInt(payload["PageIndex"]?.ToString(), 1), 1);
    var pageSize = Math.Clamp(ParseInt(payload["PageSize"]?.ToString(), 20), 1, 1000);
    var departments = store.GetDepartments().AsEnumerable();

    if (payload["DeptID"] is not null)
    {
        var deptId = ParseInt(payload["DeptID"]?.ToString());
        departments = departments.Where(dept => dept.DeptID == deptId);
    }

    departments = FilterText(departments, payload, "Name", dept => dept.Name);
    var totalCount = departments.Count();
    var dataList = departments.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToArray();
    return BrowserOk(new
    {
        TotalCount = totalCount,
        PageIndex = pageIndex,
        PageSize = pageSize,
        DataList = dataList
    });
});

app.MapPost("/api/Department/GetNewID", (HttpRequest request, StateStore store) =>
{
    var session = RequireSession(request, store, out var error);
    return session is null ? error! : BrowserOk(new { NewDeptID = store.GetNextDepartmentId() });
});

app.MapPost("/api/Department/New", async (HttpRequest request, StateStore store) =>
{
    var session = RequireSession(request, store, out var error);
    if (session is null)
    {
        return error!;
    }

    JsonObject? payload = request.HasFormContentType ? await ReadJsonObjectFromFormAsync(request) : await ReadJsonObjectAsync(request);
    if (payload is null)
    {
        return BrowserError(3, "Request parameter error");
    }

    var department = payload.Deserialize<DepartmentInfo>(new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new DepartmentInfo();
    if (department.DeptID <= 0)
    {
        return BrowserError(3, "Request parameter error");
    }

    store.UpsertDepartment(department);
    return BrowserOk();
});

app.MapPost("/api/Department/Delete", async (HttpRequest request, StateStore store) =>
{
    var session = RequireSession(request, store, out var error);
    if (session is null)
    {
        return error!;
    }

    var payload = await ReadJsonObjectAsync(request) ?? new JsonObject();
    if (payload["DeleteALL"]?.GetValue<int>() == 1)
    {
        store.DeleteAllDepartments();
        return BrowserOk();
    }

    var ids = payload["DeptIDs"] as JsonArray;
    store.DeleteDepartments(ids?.Select(item => ParseInt(item?.ToString())).Where(id => id > 0) ?? Array.Empty<int>());
    return BrowserOk();
});

app.MapPost("/api/Record/Identify/Search", async (HttpRequest request, StateStore store) =>
    await SearchRecordsAsync(request, store, "Identify"));
app.MapPost("/api/Record/DoorSensor/Search", async (HttpRequest request, StateStore store) =>
    await SearchRecordsAsync(request, store, "DoorSensor"));
app.MapPost("/api/Record/System/Search", async (HttpRequest request, StateStore store) =>
    await SearchRecordsAsync(request, store, "System"));

app.MapPost("/api/Record/Delete/All", async (HttpRequest request, StateStore store) =>
{
    var session = RequireSession(request, store, out var error);
    if (session is null)
    {
        return error!;
    }

    _ = await ReadJsonObjectAsync(request);
    store.DeleteRecords();
    return BrowserOk();
});

app.MapPost("/api/Record/Delete/ByType", async (HttpRequest request, StateStore store) =>
{
    var session = RequireSession(request, store, out var error);
    if (session is null)
    {
        return error!;
    }

    var payload = await ReadJsonObjectAsync(request) ?? new JsonObject();
    var recordTypes = SplitCsv(payload["RecordTypes"]?.ToString()).Select(ParseInt).Where(value => value > 0).ToArray();
    var category = payload["Category"]?.ToString();
    store.DeleteRecords(category, recordTypes);
    return BrowserOk();
});

app.Run();

static string? FirstNonEmpty(params string?[] values) =>
    values.FirstOrDefault(value => !string.IsNullOrWhiteSpace(value));

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

    return Results.Ok(store.GetDeletePeople(request.SN, request.Limit));
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

static async Task<JsonObject?> ReadJsonObjectAsync(HttpRequest request)
{
    if (request.Body.CanSeek)
    {
        request.Body.Position = 0;
    }

    try
    {
        var payload = await JsonNode.ParseAsync(request.Body);
        return payload as JsonObject;
    }
    finally
    {
        if (request.Body.CanSeek)
        {
            request.Body.Position = 0;
        }
    }
}

static async Task<JsonObject?> ReadJsonObjectFromFormAsync(HttpRequest request)
{
    var form = await request.ReadFormAsync();
    var json = FirstNonEmpty(form["DepartmentJson"].ToString(), form["json"].ToString(), form["payload"].ToString());
    if (string.IsNullOrWhiteSpace(json))
    {
        json = form.Keys.Count == 0 ? null : JsonSerializer.Serialize(form.ToDictionary(pair => pair.Key, pair => pair.Value.ToString()));
    }

    return string.IsNullOrWhiteSpace(json) ? null : JsonNode.Parse(json) as JsonObject;
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

static string? ReadBearerToken(HttpRequest request)
{
    var auth = request.Headers.Authorization.ToString();
    if (string.IsNullOrWhiteSpace(auth))
    {
        return null;
    }

    const string prefix = "Bearer ";
    return auth.StartsWith(prefix, StringComparison.OrdinalIgnoreCase)
        ? auth[prefix.Length..].Trim()
        : null;
}

static BrowserSession? RequireSession(HttpRequest request, StateStore store, out IResult? error)
{
    var token = ReadBearerToken(request);
    if (string.IsNullOrWhiteSpace(token))
    {
        error = BrowserError(10000, "Token is invalid");
        return null;
    }

    var session = store.GetValidSession(token);
    if (session is null)
    {
        error = BrowserError(10000, "Token is invalid");
        return null;
    }

    error = null;
    return session;
}

static IResult BrowserOk(object? content = null, string? msg = null)
{
    return Results.Ok(new
    {
        result = true,
        content,
        errCode = 0,
        msg
    });
}

static IResult BrowserError(int errCode, string message)
{
    return Results.Ok(new
    {
        result = false,
        content = (object?)null,
        errCode,
        error = message,
        msg = message
    });
}

static int ParseInt(string? value, int fallback = 0) => int.TryParse(value, out var parsed) ? parsed : fallback;

static IEnumerable<string> SplitCsv(string? value) =>
    string.IsNullOrWhiteSpace(value)
        ? Array.Empty<string>()
        : value.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

static IEnumerable<T> FilterText<T>(IEnumerable<T> source, JsonObject payload, string key, Func<T, string> selector)
{
    var text = payload[key]?.ToString();
    if (string.IsNullOrWhiteSpace(text))
    {
        return source;
    }

    return source.Where(item => selector(item).Contains(text, StringComparison.OrdinalIgnoreCase));
}

static async Task<string> SavePhotoFromFormAsync(string deviceSn, string userId, IFormFile photo)
{
    var appData = Path.Combine(AppContext.BaseDirectory, "App_Data", "photos");
    Directory.CreateDirectory(appData);
    var ext = Path.GetExtension(photo.FileName);
    if (string.IsNullOrWhiteSpace(ext))
    {
        ext = ".bin";
    }

    var path = Path.Combine(appData, $"{SanitizeForFileName(deviceSn)}_{SanitizeForFileName(userId)}{ext}");
    await using var stream = File.Create(path);
    await photo.CopyToAsync(stream);
    return path;
}

static string SanitizeForFileName(string? value)
{
    if (string.IsNullOrWhiteSpace(value))
    {
        return "item";
    }

    return string.Concat(value.Trim().Select(ch => Path.GetInvalidFileNameChars().Contains(ch) ? '_' : ch));
}

static async Task<IResult> SearchRecordsAsync(HttpRequest request, StateStore store, string category)
{
    var session = RequireSession(request, store, out var error);
    if (session is null)
    {
        return error!;
    }

    var payload = await ReadJsonObjectAsync(request) ?? new JsonObject();
    var pageIndex = Math.Max(ParseInt(payload["PageIndex"]?.ToString(), 1), 1);
    var pageSize = Math.Clamp(ParseInt(payload["PageSize"]?.ToString(), 20), 1, 1000);
    var beginDate = ParseUnix(payload["BeginDate"]?.ToString(), DateTimeOffset.MinValue);
    var endDate = ParseUnix(payload["EndDate"]?.ToString(), DateTimeOffset.MaxValue);
    var typeFilter = SplitCsv(payload["RecordTypes"]?.ToString()).Select(ParseInt).Where(value => value > 0).ToHashSet();
    var minRecordId = ParseLong(payload["RecordID"]?.ToString(), 0L);

    var personMap = store.GetPeople().ToDictionary(person => person.UserID, StringComparer.OrdinalIgnoreCase);
    var records = store.GetRecords(category)
        .Where(record => record.ReceivedAtUtc >= beginDate && record.ReceivedAtUtc <= endDate)
        .Select(record => ToBrowserRecord(record, personMap))
        .Where(record => record.RecordID >= minRecordId);

    if (typeFilter.Count > 0)
    {
        records = records.Where(record => typeFilter.Contains(record.RecordType));
    }

    records = FilterRecordText(records, payload, "UserID", record => record.UserID)
        .Where(record => ContainsOrEmpty(record.Name, payload["Name"]?.ToString(), payload["UserName"]?.ToString()))
        .Where(record => ContainsOrEmpty(record.Department, payload["Department"]?.ToString()))
        .Where(record => ContainsOrEmpty(record.Job, payload["Job"]?.ToString()))
        .Where(record => ContainsOrEmpty(record.CardNum, payload["CardNum"]?.ToString()))
        .Where(record => ContainsOrEmpty(record.IdentityCard, payload["IdentityCard"]?.ToString()));

    var totalCount = records.Count();
    var page = records.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToArray();
    return BrowserOk(new
    {
        TotalCount = totalCount,
        PageIndex = pageIndex,
        PageSize = pageSize,
        DataList = page
    });
}

static IEnumerable<dynamic> FilterRecordText(IEnumerable<dynamic> records, JsonObject payload, string key, Func<dynamic, string> selector)
{
    var text = payload[key]?.ToString();
    if (string.IsNullOrWhiteSpace(text))
    {
        return records;
    }

    return records.Where(record => selector(record).Contains(text, StringComparison.OrdinalIgnoreCase));
}

static bool ContainsOrEmpty(string source, params string?[] values)
{
    foreach (var value in values)
    {
        if (!string.IsNullOrWhiteSpace(value) && !source.Contains(value, StringComparison.OrdinalIgnoreCase))
        {
            return false;
        }
    }

    return true;
}

static dynamic ToBrowserRecord(RecordSnapshot record, IReadOnlyDictionary<string, PersonInfo> people)
{
    var detail = record.RecordDetail;
    var userId = detail?["UserID"]?.ToString() ?? string.Empty;
    people.TryGetValue(userId, out var person);
    var recordId = ParseLong(detail?["RecordID"]?.ToString(), ParseLong(record.Id, DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()));
    var recordDate = ParseLong(detail?["RecordDate"]?.ToString(), record.ReceivedAtUtc.ToUnixTimeSeconds());
    var recordType = ParseInt(detail?["RecordType"]?.ToString(), 0);
    var bodyTemp = ParseDouble(detail?["BodyTemp"]?.ToString(), 0);
    var isEntry = ParseInt(detail?["IsEntry"]?.ToString(), 0);
    var photo = detail?["Photo"]?.ToString() ?? record.PhotoPath ?? string.Empty;
    var photoLen = ParseInt(detail?["PhotoLen"]?.ToString(), !string.IsNullOrWhiteSpace(record.PhotoPath) && File.Exists(record.PhotoPath) ? (int)new FileInfo(record.PhotoPath).Length : 0);

    return new
    {
        RecordID = recordId,
        UserID = userId,
        Name = detail?["Name"]?.ToString() ?? person?.Name ?? string.Empty,
        IdentityCard = detail?["IdentityCard"]?.ToString() ?? person?.IdentityCard ?? string.Empty,
        Job = detail?["Job"]?.ToString() ?? person?.Job ?? string.Empty,
        Department = detail?["Department"]?.ToString() ?? person?.Department ?? string.Empty,
        CardNum = detail?["CardNum"]?.ToString() ?? person?.CardNum ?? string.Empty,
        QRCode = detail?["QRCode"]?.ToString() ?? person?.QRCode ?? string.Empty,
        RecordType = recordType,
        IsEntry = isEntry,
        RecordDate = recordDate,
        BodyTemp = bodyTemp,
        PhotoLen = photoLen,
        Photo = photo
    };
}

static DateTimeOffset ParseUnix(string? value, DateTimeOffset fallback)
{
    var unix = ParseLong(value, long.MinValue);
    if (unix == long.MinValue)
    {
        return fallback;
    }

    if (unix > UnixMillisecondsThreshold)
    {
        return DateTimeOffset.FromUnixTimeMilliseconds(unix);
    }

    return DateTimeOffset.FromUnixTimeSeconds(unix);
}

static long ParseLong(string? value, long fallback = 0) => long.TryParse(value, out var parsed) ? parsed : fallback;
static double ParseDouble(string? value, double fallback = 0) => double.TryParse(value, out var parsed) ? parsed : fallback;
