using System.IO.Compression;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using FaceDeviceHttpPcServer.Models;
using FaceDeviceHttpPcServer.Services;

const byte GzipMagicByte1 = 0x1F;
const byte GzipMagicByte2 = 0x8B;

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

app.MapGet("/", () => Results.Ok(new
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
