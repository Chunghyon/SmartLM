using System.Text.Json;
using System.Text.Json.Nodes;
using FaceDeviceHttpPcServer.Models;

namespace FaceDeviceHttpPcServer.Services;

public sealed class StateStore
{
    private static readonly HashSet<char> InvalidFileNameChars = Path.GetInvalidFileNameChars().ToHashSet();
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
                AddPeople = device.PendingAddPeopleCount > 0 ? device.PendingAddPeopleCount : null,
                DeletePeople = device.PendingDeleteUserIds.Count > 0 ? device.PendingDeleteUserIds.Count : null,
                SyncParameter = device.PendingSyncParameter ? 1 : null,
                UploadWorkParameter = device.PendingUploadWorkParameter ? 1 : null,
                Remote = device.PendingRemote is not null ? 1 : null
            };
        }
    }

    public IReadOnlyCollection<PersonInfo> GetPeople()
    {
        lock (_sync)
        {
            return _state.People.Values
                .OrderBy(person => person.UserID, StringComparer.OrdinalIgnoreCase)
                .Select(Clone)
                .ToArray();
        }
    }

    public bool TryAddPerson(PersonInfo person)
    {
        lock (_sync)
        {
            if (_state.People.ContainsKey(person.UserID))
            {
                return false;
            }

            _state.People[person.UserID] = Clone(person);
            _state.DeletedUserIds.RemoveAll(userId => string.Equals(userId, person.UserID, StringComparison.OrdinalIgnoreCase));
            foreach (var device in _state.Devices.Values)
            {
                device.PendingAddPeopleCount = _state.People.Count;
            }

            SaveState();
            return true;
        }
    }

    public bool DeletePerson(string userId)
    {
        lock (_sync)
        {
            if (!_state.People.Remove(userId))
            {
                return false;
            }

            if (!_state.DeletedUserIds.Contains(userId, StringComparer.OrdinalIgnoreCase))
            {
                _state.DeletedUserIds.Add(userId);
            }

            foreach (var device in _state.Devices.Values)
            {
                if (!device.PendingDeleteUserIds.Contains(userId, StringComparer.OrdinalIgnoreCase))
                {
                    device.PendingDeleteUserIds.Add(userId);
                }
            }

            SaveState();
            return true;
        }
    }

    public IReadOnlyCollection<PersonInfo> GetPeopleForDownload(string deviceSn, int limit)
    {
        lock (_sync)
        {
            var device = GetOrCreateDevice(deviceSn);
            var effectiveLimit = limit <= 0 ? 1000 : Math.Min(limit, 1000);
            var people = _state.People.Values
                .OrderBy(person => person.UserID, StringComparer.OrdinalIgnoreCase)
                .Take(effectiveLimit)
                .Select(Clone)
                .ToArray();

            device.PendingAddPeopleCount = 0;
            SaveState();
            return people;
        }
    }

    public IReadOnlyCollection<string> GetDeletePeople(string deviceSn)
    {
        lock (_sync)
        {
            var device = GetOrCreateDevice(deviceSn);
            var deleteList = device.PendingDeleteUserIds.ToArray();
            device.PendingDeleteUserIds.Clear();
            SaveState();
            return deleteList;
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
                    PendingAddPeopleCount = device.PendingAddPeopleCount,
                    PendingDeletePeopleCount = device.PendingDeleteUserIds.Count,
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

    public int MarkAddPeopleRequested(string deviceSn)
    {
        lock (_sync)
        {
            var device = GetOrCreateDevice(deviceSn);
            device.PendingAddPeopleCount = _state.People.Count;
            SaveState();
            return device.PendingAddPeopleCount;
        }
    }

    public int MarkDeletePeopleRequested(string deviceSn)
    {
        lock (_sync)
        {
            var device = GetOrCreateDevice(deviceSn);
            foreach (var userId in _state.DeletedUserIds)
            {
                if (!device.PendingDeleteUserIds.Contains(userId, StringComparer.OrdinalIgnoreCase))
                {
                    device.PendingDeleteUserIds.Add(userId);
                }
            }

            SaveState();
            return device.PendingDeleteUserIds.Count;
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

    // 式式 Remote command 式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式

    public void SetPendingRemoteCommand(string deviceSn, PendingRemoteCommand cmd)
    {
        lock (_sync)
        {
            var device = GetOrCreateDevice(deviceSn);
            device.PendingRemote = cmd;
            SaveState();
        }
    }

    public PendingRemoteCommand? ConsumeRemoteCommand(string deviceSn)
    {
        lock (_sync)
        {
            if (!_state.Devices.TryGetValue(deviceSn, out var device))
                return null;
            var cmd = device.PendingRemote;
            device.PendingRemote = null;
            SaveState();
            return cmd;
        }
    }

    // 式式 Department management 式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式

    public IReadOnlyCollection<DepartmentInfo> GetDepartments() =>
        _state.Departments.Values.OrderBy(d => d.DepartmentID, StringComparer.OrdinalIgnoreCase).ToArray();

    public bool TryAddDepartment(DepartmentInfo dept)
    {
        lock (_sync)
        {
            if (_state.Departments.ContainsKey(dept.DepartmentID))
                return false;
            _state.Departments[dept.DepartmentID] = Clone(dept);
            SaveState();
            return true;
        }
    }

    public bool DeleteDepartment(string deptId)
    {
        lock (_sync)
        {
            if (!_state.Departments.Remove(deptId))
                return false;
            SaveState();
            return true;
        }
    }

    // 式式 System records 式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式

    public void SaveSystemRecords(string deviceSn, int recordType, List<SystemRecordItem> items)
    {
        lock (_sync)
        {
            var device = GetOrCreateDevice(deviceSn);
            foreach (var item in items)
            {
                var uniqueId = $"{DateTimeOffset.UtcNow:yyyyMMddHHmmssfff}_{item.RecordID}";
                var recordFile = Path.Combine(_recordsPath, $"sys_{SanitizeForFileName(deviceSn)}_{uniqueId}.json");
                var json = System.Text.Json.JsonSerializer.Serialize(
                    new { DeviceSN = deviceSn, RecordType = recordType, item.RecordID, item.RecordDate },
                    _serializerOptions);
                File.WriteAllText(recordFile, json);

                device.Records.Add(new RecordSnapshot
                {
                    Id = uniqueId,
                    ReceivedAtUtc = DateTimeOffset.UtcNow,
                    RecordJsonPath = recordFile,
                    PhotoPath = null,
                    RecordDetail = System.Text.Json.Nodes.JsonNode.Parse(json)
                });
            }

            SaveState();
        }
    }

    // 式式 People push from device 式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式

    public (int success, int fail) SavePushedPeople(List<PersonInfo> people)
    {
        lock (_sync)
        {
            int success = 0, fail = 0;
            foreach (var p in people)
            {
                if (string.IsNullOrWhiteSpace(p.UserID)) { fail++; continue; }
                _state.People[p.UserID] = Clone(p);
                _state.DeletedUserIds.RemoveAll(id => string.Equals(id, p.UserID, StringComparison.OrdinalIgnoreCase));
                success++;
            }

            SaveState();
            return (success, fail);
        }
    }

    // 式式 Delete people list result 式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式

    public void ConfirmDeletePeopleResult(string deviceSn, List<string> confirmedIds)
    {
        // Deletions are already cleared on GetDeletePeople; nothing extra to do.
    }

    // 式式 Record management 式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式

    public void ClearAllRecords()
    {
        lock (_sync)
        {
            foreach (var device in _state.Devices.Values)
                device.Records.Clear();
            SaveState();
        }
    }

    public void ClearRecordsByType(int recordType)
    {
        lock (_sync)
        {
            // recordType: 1=identify, 2=door-sensor, 3=system
            // System records are saved with a "sys_" prefix file name; identify records without.
            foreach (var device in _state.Devices.Values)
            {
                device.Records.RemoveAll(r =>
                    recordType == 1 ? !r.RecordJsonPath.Contains("sys_") :
                    recordType >= 2 && r.RecordJsonPath.Contains("sys_"));
            }

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
            var state = JsonSerializer.Deserialize<PersistedState>(json, _serializerOptions) ?? new PersistedState();
            state.Devices = new Dictionary<string, DeviceSnapshot>(
                state.Devices ?? new Dictionary<string, DeviceSnapshot>(),
                StringComparer.OrdinalIgnoreCase);
            state.People = new Dictionary<string, PersonInfo>(
                state.People ?? new Dictionary<string, PersonInfo>(),
                StringComparer.OrdinalIgnoreCase);
            state.DeletedUserIds ??= new();
            state.Departments = new Dictionary<string, DepartmentInfo>(
                state.Departments ?? new Dictionary<string, DepartmentInfo>(),
                StringComparer.OrdinalIgnoreCase);

            foreach (var device in state.Devices.Values)
            {
                device.PendingDeleteUserIds ??= new();
                device.Records ??= new();
            }

            foreach (var person in state.People.Values)
            {
                person.Fingerprints ??= new();
                person.Palmveins ??= new();
            }

            return state;
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

    private T Clone<T>(T value)
    {
        var json = JsonSerializer.Serialize(value, _serializerOptions);
        return JsonSerializer.Deserialize<T>(json, _serializerOptions)!;
    }

    private static string SanitizeForFileName(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return string.Empty;
        }

        return string.Concat(value.Trim().Select(ch => InvalidFileNameChars.Contains(ch) ? '_' : ch));
    }
}
