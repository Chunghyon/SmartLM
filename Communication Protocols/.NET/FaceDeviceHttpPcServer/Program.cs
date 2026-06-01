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

app.MapGet("/", () => Results.Ok(new
{
    name = "FaceDeviceHttpPcServer",
    purpose = "Phase-1 HTTP integration server for face-recognition terminals",
    endpoints = new[]
    {
        "/Device/Keepalive",
        "/Device/UploadWorkSetting",
        "/Device/DownloadWorkSetting",
        "/Record/UploadIdentifyRecord",
        "/admin/devices",
        "/admin/devices/{sn}",
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

app.MapGet("/admin/devices", (StateStore store) => Results.Ok(store.GetDeviceSummaries()));

app.MapGet("/admin/devices/{sn}", (string sn, StateStore store) =>
{
    var device = store.GetDevice(sn);
    return device is null
        ? Results.NotFound(new ApiResponse(404, "Device not found."))
        : Results.Ok(device);
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
