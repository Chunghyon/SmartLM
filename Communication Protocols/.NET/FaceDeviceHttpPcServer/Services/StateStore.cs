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

    public KeepaliveResponse UpsertKeepalive(KeepaliveRequest request, string? deviceIp = null)
    {
        lock (_sync)
        {
            var device = GetOrCreateDevice(request.SN);
            device.LastKeepalive = request;
            device.LastKeepaliveAtUtc = DateTimeOffset.UtcNow;

            // Keepalive를 통해 IP 주소 자동 업데이트 (처음 연결 시 또는 IP 변경 시)
            if (!string.IsNullOrWhiteSpace(deviceIp))
            {
                if (string.IsNullOrWhiteSpace(device.IpAddress))
                {
                    // 처음 연결된 디바이스: IP 주소 저장
                    device.IpAddress = deviceIp;
                    device.ConnectedAtUtc = DateTimeOffset.UtcNow;
                }
                else if (device.IpAddress != deviceIp)
                {
                    // IP 주소 변경됨: 업데이트
                    device.IpAddress = deviceIp;
                    device.ConnectedAtUtc = DateTimeOffset.UtcNow;
                }
            }

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

    public Dictionary<string, int> GetDeviceAssignments()
    {
        lock (_sync)
        {
            var assignments = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);

            // Initialize all people with 0
            foreach (var userId in _state.People.Keys)
            {
                assignments[userId] = 0;
            }

            // Count devices that have each user
            foreach (var device in _state.Devices.Values)
            {
                foreach (var userId in device.DownloadedUserIds)
                {
                    if (_state.People.ContainsKey(userId))
                    {
                        if (!assignments.ContainsKey(userId))
                        {
                            assignments[userId] = 0;
                        }
                        assignments[userId]++;
                    }
                }
            }

            return assignments;
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

    public bool UpdatePerson(PersonInfo person)
    {
        lock (_sync)
        {
            if (!_state.People.ContainsKey(person.UserID))
            {
                return false;
            }

            _state.People[person.UserID] = Clone(person);
            SaveState();
            return true;
        }
    }

    public int FixTimegroupForAllPeople()
    {
        lock (_sync)
        {
            int count = 0;
            foreach (var person in _state.People.Values)
            {
                if (person.Timegroup == 0)
                {
                    person.Timegroup = 1;
                    count++;
                }
            }

            if (count > 0)
            {
                SaveState();
            }

            return count;
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

            // Queue deletion only on devices that have this user assigned
            int devicesAffected = 0;
            foreach (var device in _state.Devices.Values)
            {
                // Check if this device has the user in its downloaded list
                bool hasUser = device.DownloadedUserIds.Contains(userId, StringComparer.OrdinalIgnoreCase);

                if (hasUser)
                {
                    // Remove from device's downloaded list
                    device.DownloadedUserIds.RemoveAll(id => string.Equals(id, userId, StringComparison.OrdinalIgnoreCase));

                    // Add to pending delete queue for this device
                    if (!device.PendingDeleteUserIds.Contains(userId, StringComparer.OrdinalIgnoreCase))
                    {
                        device.PendingDeleteUserIds.Add(userId);
                        devicesAffected++;
                    }
                }
            }

            LogHub.Instance.Info($"[DeletePerson] 사용자 {userId} 삭제: {devicesAffected}개 단말기에 삭제 명령 전송 예정");

            SaveState();
            return true;
        }
    }

    public int DeleteAllPeople(string? deviceSn = null)
    {
        lock (_sync)
        {
            var allUserIds = _state.People.Keys.ToList();
            var deletedCount = allUserIds.Count;

            foreach (var userId in allUserIds)
            {
                _state.People.Remove(userId);

                if (!_state.DeletedUserIds.Contains(userId, StringComparer.OrdinalIgnoreCase))
                {
                    _state.DeletedUserIds.Add(userId);
                }

                if (deviceSn != null)
                {
                    // Mark for deletion on specific device only
                    if (_state.Devices.TryGetValue(deviceSn, out var device))
                    {
                        if (!device.PendingDeleteUserIds.Contains(userId, StringComparer.OrdinalIgnoreCase))
                        {
                            device.PendingDeleteUserIds.Add(userId);
                        }
                    }
                }
                else
                {
                    // Mark for deletion on all devices
                    foreach (var device in _state.Devices.Values)
                    {
                        if (!device.PendingDeleteUserIds.Contains(userId, StringComparer.OrdinalIgnoreCase))
                        {
                            device.PendingDeleteUserIds.Add(userId);
                        }
                    }
                }
            }

            SaveState();
            return deletedCount;
        }
    }

    public IReadOnlyCollection<PersonInfo> GetPeopleForDownload(string deviceSn, int limit)
    {
        lock (_sync)
        {
            var device = GetOrCreateDevice(deviceSn);

            // Only return people if there are pending adds
            if (device.PendingAddPeopleCount <= 0)
            {
                return Array.Empty<PersonInfo>();
            }

            // Protocol allows up to 1000 people per request
            // Use device-requested limit or default to 1000
            var batchSize = limit > 0 ? Math.Min(limit, 1000) : 1000;

            // Get people not yet downloaded by this device
            var people = _state.People.Values
                .Where(p => !device.DownloadedUserIds.Contains(p.UserID, StringComparer.OrdinalIgnoreCase))
                .OrderBy(person => person.UserID, StringComparer.OrdinalIgnoreCase)
                .Take(batchSize)
                .Select(Clone)
                .ToArray();

            // Mark these users as sent (but not confirmed yet)
            foreach (var person in people)
            {
                if (!device.DownloadedUserIds.Contains(person.UserID, StringComparer.OrdinalIgnoreCase))
                {
                    device.DownloadedUserIds.Add(person.UserID);
                }
            }

            // If all people have been sent, clear pending count
            var remainingPeople = _state.People.Values
                .Count(p => !device.DownloadedUserIds.Contains(p.UserID, StringComparer.OrdinalIgnoreCase));

            if (remainingPeople == 0)
            {
                device.PendingAddPeopleCount = 0;
            }

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

    public void ConfirmPeopleDownloaded(string deviceSn, int successCount)
    {
        lock (_sync)
        {
            var device = GetOrCreateDevice(deviceSn);

            // Check if all people have been downloaded
            var remainingPeople = _state.People.Values
                .Count(p => !device.DownloadedUserIds.Contains(p.UserID, StringComparer.OrdinalIgnoreCase));

            if (remainingPeople == 0)
            {
                // All people sent successfully - clear pending count only
                device.PendingAddPeopleCount = 0;
                // DON'T clear DownloadedUserIds - it's a permanent record of users on this device!
                SaveState();
            }
            // If there are more people to send, PendingAddPeopleCount stays > 0
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
            // 단말기가 새 설정 적용 후 업로드했으므로 DesiredWorkSetting은 더 이상 필요 없음
            device.DesiredWorkSetting = null;
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
            // Success 필드는 외부 wrapper에서 설정하므로 WorkSetting 내부에서는 제거
            copy.Remove("Success");
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

            // Enrich the record with server-side fields missing from device payload
            if (recordNode is JsonObject obj)
            {
                // Inject DeviceSN so search can filter by device
                if (obj["DeviceSN"] is null)
                    obj["DeviceSN"] = deviceSn;

                // Convert RecordDate (Unix seconds) to RecordTime (ISO string) if not present
                if (obj["RecordTime"] is null)
                {
                    if (obj["RecordDate"] is JsonNode rdNode &&
                        long.TryParse(rdNode.ToJsonString().Trim('"'), out long unixSec))
                    {
                        obj["RecordTime"] = DateTimeOffset.FromUnixTimeSeconds(unixSec)
                                                .LocalDateTime
                                                .ToString("yyyy-MM-dd HH:mm:ss");
                    }
                    else
                    {
                        obj["RecordTime"] = DateTimeOffset.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    }
                }

                // Map Name -> UserName for search compatibility
                if (obj["UserName"] is null && obj["Name"] is JsonNode nameNode)
                    obj["UserName"] = nameNode.ToJsonString().Trim('"');
            }

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

    public void SaveSystemRecord(string deviceSn, JsonNode? recordNode)
    {
        lock (_sync)
        {
            var device = GetOrCreateDevice(deviceSn);
            var recordId = SanitizeForFileName(recordNode?["RecordID"]?.ToString())
                           ?? DateTimeOffset.UtcNow.ToUnixTimeMilliseconds().ToString();
            var uniqueId = $"{DateTimeOffset.UtcNow:yyyyMMddHHmmssfff}_SYS_{recordId}";

            var recordFile = Path.Combine(_recordsPath, $"{SanitizeForFileName(deviceSn)}_{uniqueId}.json");
            File.WriteAllText(recordFile, recordNode?.ToJsonString(_serializerOptions) ?? "{} ");

            device.Records.Add(new RecordSnapshot
            {
                Id = uniqueId,
                ReceivedAtUtc = DateTimeOffset.UtcNow,
                RecordJsonPath = recordFile,
                PhotoPath = null,
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
                    IpAddress = device.IpAddress,
                    HttpPort = device.HttpPort,
                    DeviceName = device.DeviceName,
                    TagName = device.TagName,
                    Model = device.Model,
                    FirmwareVersion = device.FirmwareVersion,
                    UnitNo = device.UnitNo,
                    ConnectedAtUtc = device.ConnectedAtUtc,
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

    public void ResetPendingState(string deviceSn)
    {
        lock (_sync)
        {
            var device = GetOrCreateDevice(deviceSn);
            device.PendingAddPeopleCount = 0;
            device.PendingDeleteUserIds.Clear();
            device.PendingSyncParameter = false;
            device.PendingUploadWorkParameter = false;
            device.PendingRemote = null;
            device.DownloadedUserIds.Clear();  // Clear download tracking
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
            device.DownloadedUserIds.Clear(); // Reset tracking for new download session
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

    public void SetDesiredWorkSetting(string deviceSn, JsonObject patch)
    {
        lock (_sync)
        {
            var device = GetOrCreateDevice(deviceSn);

            // LastUploadedWorkSetting을 베이스로 삼아 변경 필드만 덮어씀
            // → DesiredWorkSetting은 항상 전체 WorkSetting을 유지해야 단말기에 안전하게 전달됨
            JsonObject merged;
            if (device.LastUploadedWorkSetting is not null)
            {
                merged = (JsonObject)device.LastUploadedWorkSetting.DeepClone();
                foreach (var kv in patch)
                    merged[kv.Key] = kv.Value?.DeepClone();
            }
            else
            {
                merged = (JsonObject)patch.DeepClone();
            }

            device.DesiredWorkSetting = merged;
            device.PendingSyncParameter = true;
            SaveState();
        }
    }

    // ── Remote command ──────────────────────────────────────────────────────

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

    // ── Department management ───────────────────────────────────────────────

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

    // ── System records ──────────────────────────────────────────────────────

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

    // ── People push from device ─────────────────────────────────────────────

    public (int success, int fail) SavePushedPeople(string deviceSn, List<PersonInfo> people, bool addOnly = false)
    {
        lock (_sync)
        {
            int success = 0, fail = 0;
            var device = GetOrCreateDevice(deviceSn);

            foreach (var p in people)
            {
                if (string.IsNullOrWhiteSpace(p.UserID)) { fail++; continue; }

                // If addOnly=true (PushType=1), only add if not exists
                if (addOnly && _state.People.ContainsKey(p.UserID))
                {
                    fail++;
                    continue;
                }

                // If Photo field contains a device file path (e.g., /data/attend_data/photo/frame...),
                // keep it as-is for now - it indicates the person has a photo on the device
                // Later we can implement photo download from device if needed
                // The UI will show "O" if Photo field is not empty (length > 50 check)

                _state.People[p.UserID] = Clone(p);
                _state.DeletedUserIds.RemoveAll(id => string.Equals(id, p.UserID, StringComparison.OrdinalIgnoreCase));

                // Mark this user as being on this device for device assignment tracking
                if (!device.DownloadedUserIds.Contains(p.UserID, StringComparer.OrdinalIgnoreCase))
                {
                    device.DownloadedUserIds.Add(p.UserID);
                }

                success++;
            }

            SaveState();
            return (success, fail);
        }
    }

    public (int success, int fail) DeletePushedPeople(string deviceSn, List<PersonInfo> people)
    {
        lock (_sync)
        {
            int success = 0, fail = 0;
            var device = GetOrCreateDevice(deviceSn);

            foreach (var p in people)
            {
                if (string.IsNullOrWhiteSpace(p.UserID)) { fail++; continue; }

                // Remove user from this device's assignment list
                device.DownloadedUserIds.RemoveAll(id => string.Equals(id, p.UserID, StringComparison.OrdinalIgnoreCase));

                // Count how many devices still have this user assigned
                int assignedDeviceCount = 0;
                foreach (var d in _state.Devices.Values)
                {
                    if (d.DownloadedUserIds.Contains(p.UserID, StringComparer.OrdinalIgnoreCase))
                    {
                        assignedDeviceCount++;
                    }
                }

                // If no devices have this user anymore, delete from server
                if (assignedDeviceCount == 0)
                {
                    if (_state.People.Remove(p.UserID))
                    {
                        // Add to deleted list so other devices will also remove it
                        if (!_state.DeletedUserIds.Contains(p.UserID, StringComparer.OrdinalIgnoreCase))
                        {
                            _state.DeletedUserIds.Add(p.UserID);
                        }

                        // Mark as pending delete for all other devices
                        foreach (var otherDevice in _state.Devices.Values)
                        {
                            if (otherDevice.SN != deviceSn && 
                                !otherDevice.PendingDeleteUserIds.Contains(p.UserID, StringComparer.OrdinalIgnoreCase))
                            {
                                otherDevice.PendingDeleteUserIds.Add(p.UserID);
                            }
                        }
                    }
                }

                success++;
            }

            SaveState();
            return (success, fail);
        }
    }

    // ── Delete people list result ──────────────────────────────────────────

    public void ConfirmDeletePeopleResult(string deviceSn, List<string> confirmedIds)
    {
        // Deletions are already cleared on GetDeletePeople; nothing extra to do.
    }

    // ── Record management ────────────────────────────────────────────────────

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

    public bool ConnectDevice(string deviceSn, string ipAddress, int httpPort, string? deviceName = null, string? tagName = null, string? model = null, string? firmwareVersion = null, int unitNo = 0)
    {
        lock (_sync)
        {
            var device = GetOrCreateDevice(deviceSn);
            device.IpAddress = ipAddress;
            device.HttpPort = httpPort;
            device.DeviceName = deviceName;
            device.TagName = tagName;
            device.Model = model;
            device.FirmwareVersion = firmwareVersion;
            device.UnitNo = unitNo;

            if (!device.ConnectedAtUtc.HasValue)
            {
                device.ConnectedAtUtc = DateTimeOffset.UtcNow;
            }

            SaveState();
            return true;
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

                // 마이그레이션: DesiredWorkSetting이 partial patch(소수 필드)로 저장된 경우
                // LastUploadedWorkSetting과 병합하여 완전한 스냅샷으로 복원
                if (device.DesiredWorkSetting is not null && device.LastUploadedWorkSetting is not null)
                {
                    var desiredKeyCount  = device.DesiredWorkSetting.Count;
                    var uploadedKeyCount = device.LastUploadedWorkSetting.Count;
                    // DesiredWorkSetting의 키 수가 LastUploaded의 절반 미만이면 불완전한 patch로 판단
                    if (desiredKeyCount < uploadedKeyCount / 2)
                    {
                        var merged = (JsonObject)device.LastUploadedWorkSetting.DeepClone();
                        foreach (var kv in device.DesiredWorkSetting)
                            merged[kv.Key] = kv.Value?.DeepClone();
                        device.DesiredWorkSetting = merged;
                    }
                }
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

    public void UpdateDeviceInfo(string deviceSn, string? deviceName, string? tagName)
    {
        lock (_sync)
        {
            var device = GetOrCreateDevice(deviceSn);
            if (deviceName is not null) device.DeviceName = deviceName;
            if (tagName    is not null) device.TagName    = tagName;
            SaveState();
        }
    }

    public bool RemoveDevice(string deviceSn)
    {
        lock (_sync)
        {
            if (_state.Devices.Remove(deviceSn))
            {
                SaveState();
                return true;
            }
            return false;
        }
    }

    public void QueueRemoteCommand(string deviceSn, bool restart = false, bool opendoor = false, 
        bool closealarm = false, bool clearRecord = false, bool repostRecord = false)
    {
        lock (_sync)
        {
            var device = GetOrCreateDevice(deviceSn);
            device.PendingRemote = new PendingRemoteCommand
            {
                Restart = restart ? 1 : null,
                Opendoor = opendoor ? 1 : null,
                Closealarm = closealarm ? 1 : null,
                ClearRecord = clearRecord ? 1 : null,
                RepostRecord = repostRecord ? 1 : null
            };
            SaveState();
        }
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
