using System.Security.Cryptography;
using System.Text.Json;
using System.Text.Json.Nodes;
using FaceDeviceHttpPcServer.Models;

namespace FaceDeviceHttpPcServer.Services;

public sealed class StateStore
{
    public const string DefaultDeviceSn = "SIM-DEVICE";

    private static readonly HashSet<char> InvalidFileNameChars = Path.GetInvalidFileNameChars().ToHashSet();
    private readonly object _sync = new();
    private readonly string _stateFilePath;
    private readonly string _recordsPath;
    private readonly string _photosPath;
    private readonly string _firmwarePath;
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
        _firmwarePath = Path.Combine(rootPath, "firmware");
        Directory.CreateDirectory(_recordsPath);
        Directory.CreateDirectory(_photosPath);
        Directory.CreateDirectory(_firmwarePath);

        _stateFilePath = Path.Combine(rootPath, "state.json");
        _state = LoadState();
    }

    public string GetPrimaryDeviceSn()
    {
        lock (_sync)
        {
            return GetPrimaryDeviceUnlocked().SN;
        }
    }

    public KeepaliveResponse UpsertKeepalive(KeepaliveRequest request)
    {
        lock (_sync)
        {
            var device = GetOrCreateDevice(request.SN);
            device.LastKeepalive = Clone(request);
            device.LastKeepaliveAtUtc = DateTimeOffset.UtcNow;
            SaveState();

            return new KeepaliveResponse
            {
                AddPeople = device.PendingAddPeopleCount > 0 ? device.PendingAddPeopleCount : null,
                DeletePeople = device.PendingDeleteAllPeople || device.PendingDeleteUserIds.Count > 0
                    ? Math.Max(device.PendingDeleteUserIds.Count, device.PendingDeleteAllPeople ? 1 : 0)
                    : null,
                SyncParameter = device.PendingSyncParameter ? 1 : null,
                Remote = device.PendingRemoteCommand is not null ? 1 : null,
                UploadWorkParameter = device.PendingUploadWorkParameter ? 1 : null
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

    public PersonInfo? GetPerson(string userId)
    {
        lock (_sync)
        {
            return _state.People.TryGetValue(userId, out var person) ? Clone(person) : null;
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

            UpsertPersonUnlocked(person);
            SaveState();
            return true;
        }
    }

    public void UpsertPerson(PersonInfo person)
    {
        lock (_sync)
        {
            UpsertPersonUnlocked(person);
            SaveState();
        }
    }

    public bool DeletePerson(string userId)
    {
        lock (_sync)
        {
            var deleted = DeletePersonUnlocked(userId);
            if (deleted)
            {
                SaveState();
            }

            return deleted;
        }
    }

    public int DeletePeople(IEnumerable<string> userIds)
    {
        lock (_sync)
        {
            var deletedCount = 0;
            foreach (var userId in userIds.Where(id => !string.IsNullOrWhiteSpace(id)).Select(id => id.Trim()).Distinct(StringComparer.OrdinalIgnoreCase))
            {
                if (DeletePersonUnlocked(userId))
                {
                    deletedCount++;
                }
            }

            if (deletedCount > 0)
            {
                SaveState();
            }

            return deletedCount;
        }
    }

    public int DeleteAllPeople()
    {
        lock (_sync)
        {
            var deletedUserIds = _state.People.Keys.ToArray();
            _state.People.Clear();
            foreach (var userId in deletedUserIds)
            {
                if (!_state.DeletedUserIds.Contains(userId, StringComparer.OrdinalIgnoreCase))
                {
                    _state.DeletedUserIds.Add(userId);
                }
            }

            foreach (var device in _state.Devices.Values)
            {
                device.PendingDeleteAllPeople = true;
                device.PendingDeleteUserIds = deletedUserIds.ToList();
                device.PendingAddPeopleCount = 0;
            }

            SaveState();
            return deletedUserIds.Length;
        }
    }

    public int GetNextUserId()
    {
        lock (_sync)
        {
            var max = 0L;
            foreach (var userId in _state.People.Keys)
            {
                if (long.TryParse(userId, out var parsed))
                {
                    max = Math.Max(max, parsed);
                }
            }

            return (int)Math.Min(max + 1, int.MaxValue);
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

    public SelectDeleteInfoResponse GetDeletePeople(string deviceSn, int limit)
    {
        lock (_sync)
        {
            var device = GetOrCreateDevice(deviceSn);
            var effectiveLimit = limit <= 0 ? 50 : Math.Min(limit, 1000);
            var deleteList = device.PendingDeleteUserIds.Take(effectiveLimit).ToList();
            if (deleteList.Count > 0)
            {
                device.PendingDeleteUserIds = device.PendingDeleteUserIds.Skip(deleteList.Count).ToList();
            }

            var response = new SelectDeleteInfoResponse
            {
                DeleteAll = device.PendingDeleteAllPeople ? 1 : 0,
                DeleteCount = device.PendingDeleteAllPeople ? Math.Max(deleteList.Count, 1) : deleteList.Count,
                DeleteList = deleteList
            };

            if (device.PendingDeleteAllPeople && deleteList.Count == 0)
            {
                response.DeleteCount = 1;
            }

            device.PendingDeleteAllPeople = false;
            SaveState();
            return response;
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
            UpdateManagementPasswordFromSettingUnlocked(device, setting);
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

    public JsonObject GetMergedWorkSetting(string? deviceSn = null)
    {
        lock (_sync)
        {
            var device = GetOrCreateDevice(deviceSn ?? GetPrimaryDeviceUnlocked().SN);
            var merged = new JsonObject
            {
                ["DeviceSN"] = device.SN,
                ["deviceId"] = device.SN
            };

            if (device.LastUploadedWorkSetting is not null)
            {
                MergeInto(merged, device.LastUploadedWorkSetting);
            }

            if (device.DesiredWorkSetting is not null)
            {
                MergeInto(merged, device.DesiredWorkSetting);
            }

            if (device.LastKeepalive is not null)
            {
                merged["RelayStatus"] = device.LastKeepalive.RelayStatus;
                merged["KeepOpenStatus"] = device.LastKeepalive.KeepOpenStatus;
                merged["DoorSensorStatus"] = device.LastKeepalive.DoorSensorStatus;
                merged["LockDoorStatus"] = device.LastKeepalive.LockDoorStatus;
                merged["AlarmStatus"] = device.LastKeepalive.AlarmStatus;
            }

            if (merged["MenuPassword"] is null)
            {
                merged["MenuPassword"] = device.ManagementPassword;
            }

            return merged;
        }
    }

    public void SetDesiredWorkSetting(string deviceSn, JsonObject workSetting)
    {
        lock (_sync)
        {
            var device = GetOrCreateDevice(deviceSn);
            device.DesiredWorkSetting = (JsonObject)workSetting.DeepClone();
            device.PendingSyncParameter = true;
            UpdateManagementPasswordFromSettingUnlocked(device, workSetting);
            SaveState();
        }
    }

    public void UpdateWorkSetting(string deviceSn, JsonObject partialWorkSetting)
    {
        lock (_sync)
        {
            var device = GetOrCreateDevice(deviceSn);
            var target = device.DesiredWorkSetting is null
                ? device.LastUploadedWorkSetting is null
                    ? new JsonObject()
                    : (JsonObject)device.LastUploadedWorkSetting.DeepClone()
                : (JsonObject)device.DesiredWorkSetting.DeepClone();

            MergeInto(target, partialWorkSetting);
            target["DeviceSN"] = deviceSn;
            device.DesiredWorkSetting = target;
            device.PendingSyncParameter = true;
            UpdateManagementPasswordFromSettingUnlocked(device, partialWorkSetting);
            SaveState();
        }
    }

    public void QueueRemoteCommand(string deviceSn, JsonObject remoteCommand)
    {
        lock (_sync)
        {
            var device = GetOrCreateDevice(deviceSn);
            device.PendingRemoteCommand = (JsonObject)remoteCommand.DeepClone();
            device.LastRemoteCommandQueuedAtUtc = DateTimeOffset.UtcNow;
            SaveState();
        }
    }

    public JsonObject? ConsumeRemoteCommand(string deviceSn)
    {
        lock (_sync)
        {
            if (!_state.Devices.TryGetValue(deviceSn, out var device) || device.PendingRemoteCommand is null)
            {
                return null;
            }

            var command = (JsonObject)device.PendingRemoteCommand.DeepClone();
            command["Success"] = 0;
            device.PendingRemoteCommand = null;
            SaveState();
            return command;
        }
    }

    public void SavePeopleDownloadResult(string deviceSn, JsonObject result)
    {
        lock (_sync)
        {
            var device = GetOrCreateDevice(deviceSn);
            device.LastPeopleDownloadResult = (JsonObject)result.DeepClone();
            SaveState();
        }
    }

    public void SaveDeletePeopleResult(string deviceSn, JsonObject result)
    {
        lock (_sync)
        {
            var device = GetOrCreateDevice(deviceSn);
            device.LastDeletePeopleResult = (JsonObject)result.DeepClone();
            SaveState();
        }
    }

    public PersonInfo SavePushedPerson(string deviceSn, int pushType, string userId, JsonObject? detail, IFormFile? photo)
    {
        lock (_sync)
        {
            PersonInfo person;
            if (pushType == 3)
            {
                DeletePersonUnlocked(userId);
                person = new PersonInfo { UserID = userId };
            }
            else
            {
                person = detail is not null ? ToPersonInfo(detail) : new PersonInfo { UserID = userId };
                person.UserID = string.IsNullOrWhiteSpace(person.UserID) ? userId : person.UserID;
                if (photo is not null && photo.Length > 0)
                {
                    person.Photo = SavePhotoUnlocked(deviceSn, person.UserID, photo);
                    person.PhotoLen = (int)photo.Length;
                    person.PhotoMD5 = ComputeMd5Hex(person.Photo);
                }

                UpsertPersonUnlocked(person);
            }

            var payload = detail is not null ? (JsonObject)detail.DeepClone() : new JsonObject();
            payload["PushType"] = pushType;
            payload["UserID"] = person.UserID;
            SaveRecordUnlocked(deviceSn, "PushedPeople", payload, photo);
            SaveState();
            return Clone(person);
        }
    }

    public void SaveIdentifyRecord(string deviceSn, JsonNode? recordNode, IFormFile? photo)
    {
        lock (_sync)
        {
            SaveRecordUnlocked(deviceSn, "Identify", recordNode, photo);
            SaveState();
        }
    }

    public int SaveSystemRecords(string deviceSn, string category, IEnumerable<JsonNode?> records)
    {
        lock (_sync)
        {
            var count = 0;
            foreach (var record in records)
            {
                SaveRecordUnlocked(deviceSn, category, record, null);
                count++;
            }

            SaveState();
            return count;
        }
    }

    public IReadOnlyCollection<RecordSnapshot> GetRecords(string? category = null)
    {
        lock (_sync)
        {
            return _state.Devices.Values
                .SelectMany(device => device.Records)
                .Where(record => string.IsNullOrWhiteSpace(category) || string.Equals(record.Category, category, StringComparison.OrdinalIgnoreCase))
                .OrderByDescending(record => record.ReceivedAtUtc)
                .Select(Clone)
                .ToArray();
        }
    }

    public int DeleteRecords(string? category = null, IEnumerable<int>? recordTypes = null)
    {
        lock (_sync)
        {
            var typeSet = recordTypes?.ToHashSet() ?? new HashSet<int>();
            var deleted = 0;
            foreach (var device in _state.Devices.Values)
            {
                var remaining = new List<RecordSnapshot>();
                foreach (var record in device.Records)
                {
                    var matchesCategory = string.IsNullOrWhiteSpace(category)
                        || string.Equals(record.Category, category, StringComparison.OrdinalIgnoreCase);
                    var matchesType = typeSet.Count == 0 || typeSet.Contains(GetInt(record.RecordDetail, "RecordType") ?? 0);
                    if (matchesCategory && matchesType)
                    {
                        deleted++;
                        DeleteFileIfExists(record.RecordJsonPath);
                        DeleteFileIfExists(record.PhotoPath);
                    }
                    else
                    {
                        remaining.Add(record);
                    }
                }

                device.Records = remaining;
            }

            if (deleted > 0)
            {
                SaveState();
            }

            return deleted;
        }
    }

    public IReadOnlyCollection<DepartmentInfo> GetDepartments()
    {
        lock (_sync)
        {
            return _state.Departments.Values
                .OrderBy(dept => dept.DeptID)
                .Select(Clone)
                .ToArray();
        }
    }

    public int GetNextDepartmentId()
    {
        lock (_sync)
        {
            return _state.Departments.Count == 0 ? 1 : _state.Departments.Keys.Max() + 1;
        }
    }

    public void UpsertDepartment(DepartmentInfo department)
    {
        lock (_sync)
        {
            _state.Departments[department.DeptID] = Clone(department);
            SaveState();
        }
    }

    public int DeleteDepartments(IEnumerable<int> departmentIds)
    {
        lock (_sync)
        {
            var deleted = 0;
            foreach (var departmentId in departmentIds.Distinct())
            {
                if (_state.Departments.Remove(departmentId))
                {
                    deleted++;
                }
            }

            if (deleted > 0)
            {
                SaveState();
            }

            return deleted;
        }
    }

    public int DeleteAllDepartments()
    {
        lock (_sync)
        {
            var count = _state.Departments.Count;
            _state.Departments.Clear();
            SaveState();
            return count;
        }
    }

    public BrowserSession CreateSession(string deviceSn, TimeSpan lifetime)
    {
        lock (_sync)
        {
            PruneExpiredSessionsUnlocked();
            var token = Convert.ToHexString(RandomNumberGenerator.GetBytes(24));
            var session = new BrowserSession
            {
                Token = token,
                DeviceSN = deviceSn,
                ExpiresAtUtc = DateTimeOffset.UtcNow.Add(lifetime)
            };
            _state.Sessions[token] = session;
            SaveState();
            return Clone(session);
        }
    }

    public BrowserSession? GetValidSession(string token)
    {
        lock (_sync)
        {
            if (!_state.Sessions.TryGetValue(token, out var session))
            {
                return null;
            }

            if (session.ExpiresAtUtc <= DateTimeOffset.UtcNow)
            {
                _state.Sessions.Remove(token);
                SaveState();
                return null;
            }

            return Clone(session);
        }
    }

    public BrowserSession? ExtendSession(string token, TimeSpan lifetime)
    {
        lock (_sync)
        {
            if (!_state.Sessions.TryGetValue(token, out var session))
            {
                return null;
            }

            if (session.ExpiresAtUtc <= DateTimeOffset.UtcNow)
            {
                _state.Sessions.Remove(token);
                SaveState();
                return null;
            }

            session.ExpiresAtUtc = DateTimeOffset.UtcNow.Add(lifetime);
            SaveState();
            return Clone(session);
        }
    }

    public bool RemoveSession(string token)
    {
        lock (_sync)
        {
            var removed = _state.Sessions.Remove(token);
            if (removed)
            {
                SaveState();
            }

            return removed;
        }
    }

    public string GetManagementPassword(string? deviceSn = null)
    {
        lock (_sync)
        {
            return GetOrCreateDevice(deviceSn ?? GetPrimaryDeviceUnlocked().SN).ManagementPassword;
        }
    }

    public bool VerifyManagementPassword(string? deviceSn, string password)
    {
        lock (_sync)
        {
            var device = GetOrCreateDevice(deviceSn ?? GetPrimaryDeviceUnlocked().SN);
            return string.Equals(device.ManagementPassword, password, StringComparison.Ordinal);
        }
    }

    public void UpdateManagementPassword(string? deviceSn, string newPassword)
    {
        lock (_sync)
        {
            var device = GetOrCreateDevice(deviceSn ?? GetPrimaryDeviceUnlocked().SN);
            device.ManagementPassword = newPassword;

            if (device.DesiredWorkSetting is null)
            {
                device.DesiredWorkSetting = new JsonObject();
            }

            device.DesiredWorkSetting["MenuPassword"] = newPassword;
            SaveState();
        }
    }

    public FirmwareSnapshot SaveFirmware(string deviceSn, string softwareMd5, IFormFile file)
    {
        lock (_sync)
        {
            var safeDeviceSn = SanitizeForFileName(deviceSn);
            var fileName = string.IsNullOrWhiteSpace(file.FileName) ? "firmware.bin" : file.FileName;
            var safeName = SanitizeForFileName(fileName);
            var path = Path.Combine(_firmwarePath, $"{safeDeviceSn}_{DateTimeOffset.UtcNow:yyyyMMddHHmmssfff}_{safeName}");
            using (var stream = File.Create(path))
            {
                file.CopyTo(stream);
            }

            var snapshot = new FirmwareSnapshot
            {
                FileName = fileName,
                FilePath = path,
                SoftwareMD5 = softwareMd5,
                UploadedAtUtc = DateTimeOffset.UtcNow
            };

            var device = GetOrCreateDevice(deviceSn);
            device.LastFirmware = snapshot;
            SaveState();
            return Clone(snapshot);
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
                    PendingDeletePeopleCount = device.PendingDeleteUserIds.Count + (device.PendingDeleteAllPeople ? 1 : 0),
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
            return device.PendingDeleteUserIds.Count + (device.PendingDeleteAllPeople ? 1 : 0);
        }
    }

    private void UpsertPersonUnlocked(PersonInfo person)
    {
        _state.People[person.UserID] = Clone(person);
        _state.DeletedUserIds.RemoveAll(userId => string.Equals(userId, person.UserID, StringComparison.OrdinalIgnoreCase));
        foreach (var device in _state.Devices.Values)
        {
            device.PendingAddPeopleCount = _state.People.Count;
        }
    }

    private bool DeletePersonUnlocked(string userId)
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

        return true;
    }

    private DeviceSnapshot GetPrimaryDeviceUnlocked()
    {
        return _state.Devices.Values.OrderByDescending(device => device.LastKeepaliveAtUtc ?? DateTimeOffset.MinValue).FirstOrDefault()
               ?? GetOrCreateDevice(DefaultDeviceSn);
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

    private RecordSnapshot SaveRecordUnlocked(string deviceSn, string category, JsonNode? recordNode, IFormFile? photo)
    {
        var device = GetOrCreateDevice(deviceSn);
        var recordId = SanitizeForFileName(recordNode?["RecordID"]?.ToString())
                       ?? DateTimeOffset.UtcNow.ToUnixTimeMilliseconds().ToString();
        var uniqueId = $"{DateTimeOffset.UtcNow:yyyyMMddHHmmssfff}_{recordId}";

        var recordFile = Path.Combine(_recordsPath, $"{SanitizeForFileName(deviceSn)}_{SanitizeForFileName(category)}_{uniqueId}.json");
        File.WriteAllText(recordFile, recordNode?.ToJsonString(_serializerOptions) ?? "{}");

        string? photoPath = null;
        if (photo is not null && photo.Length > 0)
        {
            photoPath = SavePhotoUnlocked(deviceSn, uniqueId, photo);
        }

        var snapshot = new RecordSnapshot
        {
            Id = uniqueId,
            DeviceSN = deviceSn,
            Category = category,
            ReceivedAtUtc = DateTimeOffset.UtcNow,
            RecordJsonPath = recordFile,
            PhotoPath = photoPath,
            RecordDetail = recordNode?.DeepClone()
        };

        device.Records.Add(snapshot);
        return snapshot;
    }

    private string SavePhotoUnlocked(string deviceSn, string key, IFormFile photo)
    {
        var ext = Path.GetExtension(photo.FileName);
        if (string.IsNullOrWhiteSpace(ext))
        {
            ext = ".bin";
        }

        var photoPath = Path.Combine(_photosPath, $"{SanitizeForFileName(deviceSn)}_{SanitizeForFileName(key)}{ext}");
        using var fileStream = File.Create(photoPath);
        photo.CopyTo(fileStream);
        return photoPath;
    }

    private void UpdateManagementPasswordFromSettingUnlocked(DeviceSnapshot device, JsonObject setting)
    {
        var menuPassword = setting["MenuPassword"]?.ToString();
        if (!string.IsNullOrWhiteSpace(menuPassword))
        {
            device.ManagementPassword = menuPassword;
        }
    }

    private void PruneExpiredSessionsUnlocked()
    {
        var now = DateTimeOffset.UtcNow;
        var expiredTokens = _state.Sessions
            .Where(pair => pair.Value.ExpiresAtUtc <= now)
            .Select(pair => pair.Key)
            .ToArray();

        foreach (var expiredToken in expiredTokens)
        {
            _state.Sessions.Remove(expiredToken);
        }
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
            state.Devices = new Dictionary<string, DeviceSnapshot>(state.Devices ?? new(), StringComparer.OrdinalIgnoreCase);
            state.People = new Dictionary<string, PersonInfo>(state.People ?? new(), StringComparer.OrdinalIgnoreCase);
            state.Departments ??= new();
            state.Sessions = new Dictionary<string, BrowserSession>(state.Sessions ?? new(), StringComparer.OrdinalIgnoreCase);
            state.DeletedUserIds ??= new();

            foreach (var device in state.Devices.Values)
            {
                device.PendingDeleteUserIds ??= new();
                device.Records ??= new();
                device.ManagementPassword = string.IsNullOrWhiteSpace(device.ManagementPassword) ? "admin" : device.ManagementPassword;
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
        PruneExpiredSessionsUnlocked();
        var json = JsonSerializer.Serialize(_state, _serializerOptions);
        File.WriteAllText(_stateFilePath, json);
    }

    private T Clone<T>(T value)
    {
        var json = JsonSerializer.Serialize(value, _serializerOptions);
        return JsonSerializer.Deserialize<T>(json, _serializerOptions)!;
    }

    private static PersonInfo ToPersonInfo(JsonObject detail)
    {
        var person = detail.Deserialize<PersonInfo>(new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        }) ?? new PersonInfo();

        if (string.IsNullOrWhiteSpace(person.Name))
        {
            person.Name = detail["UserName"]?.ToString() ?? string.Empty;
        }

        return person;
    }

    private static int? GetInt(JsonNode? node, string name)
    {
        var value = node?[name];
        if (value is null)
        {
            return null;
        }

        if (value is JsonValue jsonValue && jsonValue.TryGetValue<int>(out var intValue))
        {
            return intValue;
        }

        return int.TryParse(value.ToString(), out var parsed) ? parsed : null;
    }

    private static void MergeInto(JsonObject target, JsonObject source)
    {
        foreach (var pair in source)
        {
            if (pair.Value is JsonObject sourceChild)
            {
                if (target[pair.Key] is not JsonObject targetChild)
                {
                    targetChild = new JsonObject();
                    target[pair.Key] = targetChild;
                }

                MergeInto(targetChild, sourceChild);
            }
            else
            {
                target[pair.Key] = pair.Value?.DeepClone();
            }
        }
    }

    private static void DeleteFileIfExists(string? path)
    {
        if (!string.IsNullOrWhiteSpace(path) && File.Exists(path))
        {
            File.Delete(path);
        }
    }

    private static string ComputeMd5Hex(string value)
    {
        using var md5 = MD5.Create();
        var hash = md5.ComputeHash(System.Text.Encoding.UTF8.GetBytes(value));
        return Convert.ToHexString(hash);
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
