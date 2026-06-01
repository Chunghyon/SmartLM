using System.Text.Json;
using System.Text.Json.Nodes;
using FaceDeviceHttpPcServer.Models;

namespace FaceDeviceHttpPcServer.Services;

public sealed class StateStore
{
    private readonly object _sync = new();
    private readonly string _stateFilePath;
    private readonly string _recordsPath;
    private readonly string _photosPath;
    private readonly JsonSerializerOptions _serializerOptions = new()
    {
        WriteIndented = true,
        PropertyNamingPolicy = null
    };

    private PersistedState _state;

    public StateStore(string rootPath)
    {
        Directory.CreateDirectory(rootPath);
        _recordsPath = Path.Combine(rootPath, "records");
        _photosPath = Path.Combine(rootPath, "photos");
        Directory.CreateDirectory(_recordsPath);
        Directory.CreateDirectory(_photosPath);

        _stateFilePath = Path.Combine(rootPath, "state.json");
        _state = LoadState();
    }

    public KeepaliveResponse UpsertKeepalive(KeepaliveRequest request)
    {
        lock (_sync)
        {
            var device = GetOrCreateDevice(request.SN);
            device.LastKeepalive = request;
            device.LastKeepaliveAtUtc = DateTimeOffset.UtcNow;
            SaveState();

            return new KeepaliveResponse
            {
                SyncParameter = device.PendingSyncParameter ? 1 : null,
                UploadWorkParameter = device.PendingUploadWorkParameter ? 1 : null
            };
        }
    }

    public void SaveUploadedWorkSetting(string deviceSn, JsonObject setting)
    {
        lock (_sync)
        {
            var device = GetOrCreateDevice(deviceSn);
            device.LastUploadedWorkSetting = (JsonObject?)setting.DeepClone();
            device.LastWorkSettingUploadAtUtc = DateTimeOffset.UtcNow;
            device.PendingUploadWorkParameter = false;
            SaveState();
        }
    }

    public JsonObject? GetWorkSettingForDownload(string deviceSn)
    {
        lock (_sync)
        {
            if (!_state.Devices.TryGetValue(deviceSn, out var device))
            {
                return null;
            }

            var source = device.DesiredWorkSetting ?? device.LastUploadedWorkSetting;
            if (source is null)
            {
                return null;
            }

            device.PendingSyncParameter = false;
            var copy = (JsonObject)source.DeepClone();
            copy["Success"] = 0;
            copy["DeviceSN"] = deviceSn;
            SaveState();
            return copy;
        }
    }

    public void SaveIdentifyRecord(string deviceSn, JsonNode? recordNode, IFormFile? photo)
    {
        lock (_sync)
        {
            var device = GetOrCreateDevice(deviceSn);
            var recordId = SanitizeForFileName(recordNode?["RecordID"]?.ToString())
                           ?? DateTimeOffset.UtcNow.ToUnixTimeMilliseconds().ToString();
            var uniqueId = $"{DateTimeOffset.UtcNow:yyyyMMddHHmmssfff}_{recordId}";

            var recordFile = Path.Combine(_recordsPath, $"{SanitizeForFileName(deviceSn)}_{uniqueId}.json");
            File.WriteAllText(recordFile, recordNode?.ToJsonString(_serializerOptions) ?? "{} ");

            string? photoPath = null;
            if (photo is not null && photo.Length > 0)
            {
                var ext = Path.GetExtension(photo.FileName);
                if (string.IsNullOrWhiteSpace(ext))
                {
                    ext = ".bin";
                }

                photoPath = Path.Combine(_photosPath, $"{SanitizeForFileName(deviceSn)}_{uniqueId}{ext}");
                using var fileStream = File.Create(photoPath);
                photo.CopyTo(fileStream);
            }

            device.Records.Add(new RecordSnapshot
            {
                Id = uniqueId,
                ReceivedAtUtc = DateTimeOffset.UtcNow,
                RecordJsonPath = recordFile,
                PhotoPath = photoPath,
                RecordDetail = recordNode?.DeepClone()
            });

            SaveState();
        }
    }

    public IReadOnlyCollection<DeviceSummary> GetDeviceSummaries()
    {
        lock (_sync)
        {
            return _state.Devices.Values
                .OrderByDescending(device => device.LastKeepaliveAtUtc ?? DateTimeOffset.MinValue)
                .Select(device => new DeviceSummary
                {
                    SN = device.SN,
                    LastKeepaliveAtUtc = device.LastKeepaliveAtUtc,
                    LastWorkSettingUploadAtUtc = device.LastWorkSettingUploadAtUtc,
                    PendingSyncParameter = device.PendingSyncParameter,
                    PendingUploadWorkParameter = device.PendingUploadWorkParameter,
                    RecordCount = device.Records.Count
                })
                .ToArray();
        }
    }

    public DeviceSnapshot? GetDevice(string deviceSn)
    {
        lock (_sync)
        {
            return _state.Devices.TryGetValue(deviceSn, out var device)
                ? Clone(device)
                : null;
        }
    }

    public void MarkSyncRequested(string deviceSn)
    {
        lock (_sync)
        {
            var device = GetOrCreateDevice(deviceSn);
            device.PendingSyncParameter = true;
            SaveState();
        }
    }

    public void MarkUploadWorkSettingRequested(string deviceSn)
    {
        lock (_sync)
        {
            var device = GetOrCreateDevice(deviceSn);
            device.PendingUploadWorkParameter = true;
            SaveState();
        }
    }

    public void SetDesiredWorkSetting(string deviceSn, JsonObject workSetting)
    {
        lock (_sync)
        {
            var device = GetOrCreateDevice(deviceSn);
            device.DesiredWorkSetting = (JsonObject)workSetting.DeepClone();
            device.PendingSyncParameter = true;
            SaveState();
        }
    }

    private DeviceSnapshot GetOrCreateDevice(string deviceSn)
    {
        if (!_state.Devices.TryGetValue(deviceSn, out var device))
        {
            device = new DeviceSnapshot { SN = deviceSn };
            _state.Devices[deviceSn] = device;
        }

        return device;
    }

    private PersistedState LoadState()
    {
        if (!File.Exists(_stateFilePath))
        {
            return new PersistedState();
        }

        try
        {
            var json = File.ReadAllText(_stateFilePath);
            return JsonSerializer.Deserialize<PersistedState>(json, _serializerOptions) ?? new PersistedState();
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"[WARN] Failed to load persisted state from '{_stateFilePath}': {ex.Message}");
            return new PersistedState();
        }
    }

    private void SaveState()
    {
        var json = JsonSerializer.Serialize(_state, _serializerOptions);
        File.WriteAllText(_stateFilePath, json);
    }

    private DeviceSnapshot Clone(DeviceSnapshot snapshot)
    {
        var json = JsonSerializer.Serialize(snapshot, _serializerOptions);
        return JsonSerializer.Deserialize<DeviceSnapshot>(json, _serializerOptions)!;
    }

    private static string SanitizeForFileName(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return string.Empty;
        }

        var invalidChars = Path.GetInvalidFileNameChars();
        return string.Concat(value.Trim().Select(ch => invalidChars.Contains(ch) ? '_' : ch));
    }
}
