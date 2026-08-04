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

    private readonly string _peoplePath;
    private PersistedState _state;

    public StateStore(string rootPath)
    {
        Directory.CreateDirectory(rootPath);
        _recordsPath = Path.Combine(rootPath, "records");
        _photosPath  = Path.Combine(rootPath, "photos");
        _peoplePath  = Path.Combine(rootPath, "people");
        Directory.CreateDirectory(_recordsPath);
        Directory.CreateDirectory(_photosPath);
        Directory.CreateDirectory(_peoplePath);

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
            // 서버 사용자 추가는 단말기에 자동 반영안함 - distribute를 통해서만 전달됨

            SavePersonFile(person);
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
            SavePersonFile(person);
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

            // 서버 사용자 삭제는 단말기에 자동 반영안함 - distribute를 통해서만 연동됨
            LogHub.Instance.Info($"[DeletePerson] 서버 사용자 {userId} 삭제 완료");

            DeletePersonFile(userId);
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
                            device.PendingDeleteUserIds.Add(userId);
                    }
                }
                else
                {
                    // Mark for deletion on all devices
                    foreach (var device in _state.Devices.Values)
                    {
                        if (!device.PendingDeleteUserIds.Contains(userId, StringComparer.OrdinalIgnoreCase))
                            device.PendingDeleteUserIds.Add(userId);
                    }
                }
            }

            // OwnedPeople / DownloadedUserIds 초기화
            if (deviceSn != null)
            {
                if (_state.Devices.TryGetValue(deviceSn, out var targetDevice))
                {
                    targetDevice.OwnedPeople.Clear();
                    targetDevice.DownloadedUserIds.Clear();
                }
            }
            else
            {
                foreach (var device in _state.Devices.Values)
                {
                    device.OwnedPeople.Clear();
                    device.DownloadedUserIds.Clear();
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

            var batchSize = limit > 0 ? Math.Min(limit, 1000) : 1000;

            // StagedPeople(단말기별 전송 대기)이 있으면 우선 사용, 없으면 서버 전체 사용자 사용
            var sourcePool = device.StagedPeople.Count > 0
                ? device.StagedPeople.Values.Cast<PersonInfo>()
                : _state.People.Values.Cast<PersonInfo>();

            var people = sourcePool
                .Where(p => !device.DownloadedUserIds.Contains(p.UserID, StringComparer.OrdinalIgnoreCase))
                .OrderBy(person => person.UserID, StringComparer.OrdinalIgnoreCase)
                .Take(batchSize)
                .Select(p =>
                {
                    var c = Clone(p);
                    // 단말기 내부 경로는 서버에서 전송 불가 → 빈 값으로 처리
                    if (!string.IsNullOrWhiteSpace(c.Photo) && c.Photo.StartsWith("/") && !c.Photo.StartsWith("/9j/"))
                    {
                        c.Photo = string.Empty;
                        c.PhotoLen = 0;
                        c.PhotoMD5 = string.Empty;
                    }
                    return c;
                })
                .ToArray();

            foreach (var person in people)
            {
                if (!device.DownloadedUserIds.Contains(person.UserID, StringComparer.OrdinalIgnoreCase))
                    device.DownloadedUserIds.Add(person.UserID);

                // OwnedPeople도 갱신하여 단말기 설정 창의 사용자 정보에 반영
                device.OwnedPeople[person.UserID] = Clone(person);
            }

            var remainingPeople = sourcePool
                .Count(p => !device.DownloadedUserIds.Contains(p.UserID, StringComparer.OrdinalIgnoreCase));

            if (remainingPeople == 0)
            {
                device.PendingAddPeopleCount = 0;
                device.StagedPeople.Clear(); // 전송 완료 후 Stage 초기화
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
            // StagedPeople\uc774 \uc788\uc73c\uba74 \uadf8 \uc218\ub97c, \uc5c6\uc73c\uba74 \uc11c\ubc84 \uc804\uccb4 \uc0ac\uc6a9\uc790 \uc218\ub97c \uc0ac\uc6a9
            device.PendingAddPeopleCount = device.StagedPeople.Count > 0
                ? device.StagedPeople.Count
                : _state.People.Count;
            device.DownloadedUserIds.Clear();
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

                // addOnly=true(PushType=1)여도 서버에 없는 사용자(사용자 탭에서 삭제됐거나
                // 아직 서버에 등록되지 않은 사용자)는 재등록 허용.
                // 서버에 이미 있는 사용자만 skip(중복 방지).
                if (addOnly && _state.People.ContainsKey(p.UserID))
                {
                    // OwnedPeople은 항상 최신 단말기 상태로 갱신
                    device.OwnedPeople[p.UserID] = Clone(p);
                    success++;
                    continue;
                }

                // 단말기 내부 경로(예: /data/user_pic/...)는 서버에서 사용 불가 → 기존 서버 Photo 보존
                var toSave = Clone(p);
                if (!string.IsNullOrWhiteSpace(toSave.Photo) && toSave.Photo.StartsWith("/") && !toSave.Photo.StartsWith("/9j/"))
                {
                    if (_state.People.TryGetValue(p.UserID, out var existing) &&
                        !string.IsNullOrWhiteSpace(existing.Photo) &&
                        !existing.Photo.StartsWith("/"))
                    {
                        // 기존 base64 Photo 보존
                        toSave.Photo = existing.Photo;
                        toSave.PhotoLen = existing.PhotoLen;
                        toSave.PhotoMD5 = existing.PhotoMD5;
                    }
                    else
                    {
                        // 가져올 수 없는 경로만 있으면 빈 값으로
                        toSave.Photo = string.Empty;
                        toSave.PhotoLen = 0;
                        toSave.PhotoMD5 = string.Empty;
                    }
                }
                _state.People[p.UserID] = toSave;
                _state.DeletedUserIds.RemoveAll(id => string.Equals(id, p.UserID, StringComparison.OrdinalIgnoreCase));

                // Mark this user as being on this device for device assignment tracking
                if (!device.DownloadedUserIds.Contains(p.UserID, StringComparer.OrdinalIgnoreCase))
                {
                    device.DownloadedUserIds.Add(p.UserID);
                }

                // 단말기 고유 사용자 목록에도 저장 (단말기별 독립 관리)
                device.OwnedPeople[p.UserID] = Clone(toSave);

                SavePersonFile(toSave);
                success++;
            }

            SaveState();
            return (success, fail);
        }
    }

    /// <summary>단말기가 Query(PushType=4)로 전체 목록을 보내왔을 때 OwnedPeople 전면 교체 + _state.People 병합</summary>
    /// <returns>(success, fail, photoPathsToFetch): photoPathsToFetch는 (userId, photoPath) 목록 - 호출자가 비동기로 다운로드</returns>
    public (int success, int fail, List<(string userId, string photoPath)> photoPathsToFetch) ReplaceDeviceOwnedPeople(string deviceSn, List<PersonInfo> people)
    {
        lock (_sync)
        {
            var device = GetOrCreateDevice(deviceSn);
            device.OwnedPeople.Clear();
            device.DownloadedUserIds.Clear();
            int success = 0, fail = 0;
            var photoPathsToFetch = new List<(string, string)>();

            foreach (var p in people)
            {
                if (string.IsNullOrWhiteSpace(p.UserID)) { fail++; continue; }

                var toSave = Clone(p);

                // 단말기 내부 경로인 경우: 기존 base64가 있으면 보존, 없으면 경로를 기억해두고 나중에 다운로드
                if (!string.IsNullOrWhiteSpace(toSave.Photo) && toSave.Photo.StartsWith("/") && !toSave.Photo.StartsWith("/9j/"))
                {
                    var devicePhotoPath = toSave.Photo;
                    if (_state.People.TryGetValue(p.UserID, out var existing) &&
                        !string.IsNullOrWhiteSpace(existing.Photo) &&
                        !existing.Photo.StartsWith("/"))
                    {
                        // 기존 base64 Photo 보존
                        toSave.Photo = existing.Photo;
                        toSave.PhotoLen = existing.PhotoLen;
                        toSave.PhotoMD5 = existing.PhotoMD5;
                    }
                    else
                    {
                        // 아직 사진 없음 → 다운로드 목록에 추가, Photo는 일단 빈 값
                        photoPathsToFetch.Add((p.UserID, devicePhotoPath));
                        toSave.Photo = string.Empty;
                        toSave.PhotoLen = 0;
                        toSave.PhotoMD5 = string.Empty;
                    }
                }

                device.OwnedPeople[p.UserID] = Clone(toSave);
                device.DownloadedUserIds.Add(p.UserID);

                // _state.People에도 병합 (사용자 탭 새로고침에 표시되도록)
                _state.People[p.UserID] = toSave;
                _state.DeletedUserIds.RemoveAll(id => string.Equals(id, p.UserID, StringComparison.OrdinalIgnoreCase));
                SavePersonFile(toSave);

                success++;
            }
            SaveState();
            return (success, fail, photoPathsToFetch);
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

                // Remove user from this device's OwnedPeople and assignment list
                device.OwnedPeople.Remove(p.UserID);
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

    /// <summary>서버 사용자 목록을 특정 단말기의 StagedPeople에 복사 (단말기로 배포 시 사용)</summary>
    public void StageServerPeopleForDevice(string deviceSn, IReadOnlyCollection<PersonInfo> serverPeople)
    {
        lock (_sync)
        {
            var device = GetOrCreateDevice(deviceSn);
            device.StagedPeople.Clear();
            foreach (var p in serverPeople)
                device.StagedPeople[p.UserID] = Clone(p);
            SaveState();
        }
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
        PersistedState state;

        if (!File.Exists(_stateFilePath))
        {
            state = new PersistedState();
        }
        else
        {
            try
            {
                var json = File.ReadAllText(_stateFilePath);
                state = JsonSerializer.Deserialize<PersistedState>(json, _serializerOptions) ?? new PersistedState();
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"[WARN] Failed to load persisted state from '{_stateFilePath}': {ex.Message}");
                state = new PersistedState();
            }
        }

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

        // people 폴더의 JSON 파일을 _state.People로 병합 (파일이 우선순위)
        if (Directory.Exists(_peoplePath))
        {
            foreach (var file in Directory.GetFiles(_peoplePath, "*.json", SearchOption.TopDirectoryOnly))
            {
                try
                {
                    var personJson = File.ReadAllText(file);
                    var person = JsonSerializer.Deserialize<PersonInfo>(personJson, _serializerOptions);
                    if (person == null || string.IsNullOrWhiteSpace(person.UserID)) continue;
                    person.Fingerprints ??= new();
                    person.Palmveins    ??= new();
                    state.People[person.UserID] = person;
                }
                catch { /* 단일 파일 오류는 무시 */ }
            }
        }

        foreach (var device in state.Devices.Values)
        {
            device.PendingDeleteUserIds ??= new();
            device.Records ??= new();
            device.OwnedPeople ??= new(StringComparer.OrdinalIgnoreCase);
            device.StagedPeople ??= new(StringComparer.OrdinalIgnoreCase);

            if (device.DesiredWorkSetting is not null && device.LastUploadedWorkSetting is not null)
            {
                var desiredKeyCount  = device.DesiredWorkSetting.Count;
                var uploadedKeyCount = device.LastUploadedWorkSetting.Count;
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

    private void SaveState()
    {
        var json = JsonSerializer.Serialize(_state, _serializerOptions);
        File.WriteAllText(_stateFilePath, json);
    }

    /// <summary>사용자 데이터를 App_Data/people/{UserID}.json 파일로 저장</summary>
    private void SavePersonFile(PersonInfo person)
    {
        var fileName = SanitizeForFileName(person.UserID);
        if (string.IsNullOrEmpty(fileName)) return;
        var filePath = Path.Combine(_peoplePath, $"{fileName}.json");
        var json = JsonSerializer.Serialize(person, _serializerOptions);
        File.WriteAllText(filePath, json);
    }

    /// <summary>people 폴더의 JSON 파일들을 읽어 _state.People을 재구성합니다.</summary>
    public (int loaded, int skipped, int errors) ReloadPeopleFromFiles()
    {
        lock (_sync)
        {
            int loaded = 0, skipped = 0, errors = 0;
            var files = Directory.GetFiles(_peoplePath, "*.json", SearchOption.TopDirectoryOnly);
            foreach (var file in files)
            {
                try
                {
                    var json = File.ReadAllText(file);
                    var person = JsonSerializer.Deserialize<PersonInfo>(json, _serializerOptions);
                    if (person == null || string.IsNullOrWhiteSpace(person.UserID))
                    {
                        skipped++;
                        continue;
                    }
                    person.Fingerprints ??= new();
                    person.Palmveins    ??= new();
                    _state.People[person.UserID] = person;
                    loaded++;
                }
                catch
                {
                    errors++;
                }
            }
            SaveState();
            return (loaded, skipped, errors);
        }
    }

    /// <summary>사용자 개별 파일 삭제</summary>
    private void DeletePersonFile(string userId)
    {
        var fileName = SanitizeForFileName(userId);
        if (string.IsNullOrEmpty(fileName)) return;
        var filePath = Path.Combine(_peoplePath, $"{fileName}.json");
        if (File.Exists(filePath)) File.Delete(filePath);
    }

    /// <summary>특정 사용자의 JSON 파일 경로 반환 (내보내기용)</summary>
    public string? GetPersonFilePath(string userId)
    {
        var fileName = SanitizeForFileName(userId);
        if (string.IsNullOrEmpty(fileName)) return null;
        var filePath = Path.Combine(_peoplePath, $"{fileName}.json");
        return File.Exists(filePath) ? filePath : null;
    }

    /// <summary>사용자 데이터를 Photo/FaceFeature 등 전체 포함 JSON 문자열로 반환 (내보내기용)</summary>
    public string? ExportPersonJson(string userId)
    {
        lock (_sync)
        {
            if (!_state.People.TryGetValue(userId, out var person))
                return null;
            return JsonSerializer.Serialize(person, _serializerOptions);
        }
    }

    /// <summary>단말기에 Push된 Photo(Base64)를 파일로도 저장하고 PersonInfo를 업데이트</summary>
    public void UpdatePersonPhoto(string userId, string base64Photo)
    {
        lock (_sync)
        {
            if (!_state.People.TryGetValue(userId, out var person))
                return;
            person.Photo = base64Photo;
            // base64 → 실제 바이트 크기 계산하여 PhotoLen 갱신
            var bytes = Convert.FromBase64String(base64Photo);
            person.PhotoLen = bytes.Length;
            person.PhotoMD5 = Convert.ToHexString(System.Security.Cryptography.MD5.HashData(bytes));
            SavePersonFile(person);
            SaveState();
        }
    }

    // ── 단말기별 사용자 관리 (서버와 독립) ──────────────────────────────

    /// <summary>단말기에 등록된 사용자 목록 반환 (PushPeople로 받은 OwnedPeople)</summary>
    public IReadOnlyCollection<PersonInfo> GetDeviceOwnedPeople(string deviceSn)
    {
        lock (_sync)
        {
            if (!_state.Devices.TryGetValue(deviceSn, out var device))
                return Array.Empty<PersonInfo>();
            return device.OwnedPeople.Values.OrderBy(p => p.UserID, StringComparer.OrdinalIgnoreCase).ToArray();
        }
    }

    /// <summary>단말기별 사용자 추가/수정 (단말기에만 영향, 서버 사용자와 독립)</summary>
    public void UpsertDeviceOwnedPerson(string deviceSn, PersonInfo person)
    {
        lock (_sync)
        {
            var device = GetOrCreateDevice(deviceSn);
            device.OwnedPeople[person.UserID] = Clone(person);
            // StagedPeople에도 놓아서 다음 Keepalive 시 단말기로 전송
            device.StagedPeople[person.UserID] = Clone(person);
            device.PendingAddPeopleCount = device.StagedPeople.Count;
            SaveState();
        }
    }

    /// <summary>단말기별 사용자 삭제 (단말기에만 영향, 서버 사용자와 독립)</summary>
    public void DeleteDeviceOwnedPerson(string deviceSn, string userId)
    {
        lock (_sync)
        {
            if (!_state.Devices.TryGetValue(deviceSn, out var device))
                return;
            device.OwnedPeople.Remove(userId);
            device.StagedPeople.Remove(userId);
            // DownloadedUserIds에서도 제거 (재배포 시 다시 전달되도록)
            device.DownloadedUserIds.RemoveAll(id =>
                string.Equals(id, userId, StringComparison.OrdinalIgnoreCase));
            if (!device.PendingDeleteUserIds.Contains(userId, StringComparer.OrdinalIgnoreCase))
                device.PendingDeleteUserIds.Add(userId);
            SaveState();
        }
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
        bool closealarm = false, bool clearRecord = false, bool repostRecord = false, bool pushAllPeople = false)
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
                RepostRecord = repostRecord ? 1 : null,
                PushAllPeople = pushAllPeople ? 1 : null
            };
            SaveState();
        }
    }

    public void QueueSyncTime(string deviceSn, long unixTimestamp)
    {
        lock (_sync)
        {
            var device = GetOrCreateDevice(deviceSn);
            device.PendingRemote = new PendingRemoteCommand { SetTime = unixTimestamp };
            SaveState();
        }
    }

    public List<string> GetAllDeviceSNs()
    {
        lock (_sync)
        {
            return _state.Devices.Keys.ToList();
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
