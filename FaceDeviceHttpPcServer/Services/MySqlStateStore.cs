using System.Text.Json;
using System.Text.Json.Nodes;
using FaceDeviceHttpPcServer.Data;
using FaceDeviceHttpPcServer.Data.Entities;
using FaceDeviceHttpPcServer.Models;
using Microsoft.EntityFrameworkCore;

namespace FaceDeviceHttpPcServer.Services;

public sealed class MySqlStateStore
{
    private static readonly HashSet<char> InvalidFileNameChars = Path.GetInvalidFileNameChars().ToHashSet();
    private readonly IDbContextFactory<FaceDeviceDbContext> _dbFactory;
    private readonly string _photosPath;
    private readonly string _peoplePath;
    private readonly JsonSerializerOptions _json = new()
    {
        WriteIndented = false,
        PropertyNamingPolicy = null
    };

    public MySqlStateStore(IDbContextFactory<FaceDeviceDbContext> dbFactory, string storagePath)
    {
        _dbFactory = dbFactory;
        Directory.CreateDirectory(storagePath);
        _photosPath = Path.Combine(storagePath, "photos");
        Directory.CreateDirectory(_photosPath);
        _peoplePath = Path.Combine(storagePath, "people");
        Directory.CreateDirectory(_peoplePath);
    }

    private FaceDeviceDbContext CreateDb() => _dbFactory.CreateDbContext();

    public KeepaliveResponse UpsertKeepalive(KeepaliveRequest request, string? deviceIp = null)
    {
        using var db = CreateDb();
        var device = GetOrCreateDevice(db, request.SN);

        device.LastKeepaliveJson = JsonSerializer.Serialize(request, _json);
        device.LastKeepaliveAt = DateTime.UtcNow;
        if (!string.IsNullOrWhiteSpace(deviceIp))
        {
            if (string.IsNullOrWhiteSpace(device.IpAddress) || device.IpAddress != deviceIp)
            {
                device.IpAddress = deviceIp;
                device.ConnectedAt = DateTime.UtcNow;
            }
        }

        db.SaveChanges();

        var pendingDelete = db.PendingDeletes.Count(x => x.DeviceSn == request.SN);
        return new KeepaliveResponse
        {
            AddPeople = device.PendingAddPeopleCount > 0 ? device.PendingAddPeopleCount : null,
            DeletePeople = pendingDelete > 0 ? pendingDelete : null,
            SyncParameter = device.PendingSyncParameter ? 1 : null,
            UploadWorkParameter = device.PendingUploadWorkParameter ? 1 : null,
            Remote = string.IsNullOrWhiteSpace(device.PendingRemoteJson) ? null : 1
        };
    }

    public IReadOnlyCollection<PersonInfo> GetPeople()
    {
        using var db = CreateDb();
        return db.People.AsNoTracking()
            .OrderBy(p => p.UserId)
            .Select(ToPersonInfo)
            .ToArray();
    }

    public Dictionary<string, int> GetDeviceAssignments()
    {
        using var db = CreateDb();
        var assignments = db.People.AsNoTracking().Select(p => p.UserId)
            .ToDictionary(id => id, _ => 0, StringComparer.OrdinalIgnoreCase);

        var owned = db.DevicePeople.AsNoTracking()
            .Where(x => x.Downloaded || x.Owned)
            .ToList();

        foreach (var row in owned)
        {
            if (assignments.ContainsKey(row.UserId))
                assignments[row.UserId]++;
        }
        return assignments;
    }

    public bool TryAddPerson(PersonInfo person)
    {
        using var db = CreateDb();
        if (db.People.Any(p => p.UserId == person.UserID))
            return false;

        db.People.Add(ToEntity(person));
        var deleted = db.DeletedUserIds.Find(person.UserID);
        if (deleted != null)
            db.DeletedUserIds.Remove(deleted);
        db.SaveChanges();
        return true;
    }

    public bool UpdatePerson(PersonInfo person)
    {
        using var db = CreateDb();
        var entity = db.People.Find(person.UserID);
        if (entity == null)
            return false;
        CopyToEntity(person, entity);
        entity.UpdatedAt = DateTime.UtcNow;
        db.SaveChanges();
        return true;
    }

    public int FixTimegroupForAllPeople()
    {
        using var db = CreateDb();
        var list = db.People.Where(p => p.Timegroup == 0).ToList();
        foreach (var p in list)
            p.Timegroup = 1;
        db.SaveChanges();
        return list.Count;
    }

    public bool DeletePerson(string userId)
    {
        using var db = CreateDb();
        var entity = db.People.Find(userId);
        if (entity == null)
            return false;

        db.DevicePeople.RemoveRange(db.DevicePeople.Where(x => x.UserId == userId));
        db.People.Remove(entity);
        if (!db.DeletedUserIds.Any(x => x.UserId == userId))
            db.DeletedUserIds.Add(new DeletedUserIdEntity { UserId = userId });
        db.SaveChanges();
        LogHub.Instance.Info($"[DeletePerson] 사용자 {userId} 삭제 완료");
        return true;
    }

    public int DeleteAllPeople(string? deviceSn = null)
    {
        using var db = CreateDb();
        var allIds = db.People.Select(p => p.UserId).ToList();
        foreach (var userId in allIds)
        {
            if (!db.DeletedUserIds.Any(x => x.UserId == userId))
                db.DeletedUserIds.Add(new DeletedUserIdEntity { UserId = userId });
        }

        if (deviceSn != null)
        {
            var device = GetOrCreateDevice(db, deviceSn);
            foreach (var userId in allIds)
            {
                if (!db.PendingDeletes.Any(x => x.DeviceSn == deviceSn && x.UserId == userId))
                    db.PendingDeletes.Add(new PendingDeleteEntity { DeviceSn = deviceSn, UserId = userId });
            }
            db.DevicePeople.RemoveRange(db.DevicePeople.Where(x => x.DeviceSn == deviceSn));
        }
        else
        {
            foreach (var device in db.Devices.ToList())
            {
                foreach (var userId in allIds)
                {
                    if (!db.PendingDeletes.Any(x => x.DeviceSn == device.Sn && x.UserId == userId))
                        db.PendingDeletes.Add(new PendingDeleteEntity { DeviceSn = device.Sn, UserId = userId });
                }
            }
            db.DevicePeople.RemoveRange(db.DevicePeople);
        }

        db.People.RemoveRange(db.People);
        db.SaveChanges();
        return allIds.Count;
    }

    public IReadOnlyCollection<PersonInfo> GetPeopleForDownload(string deviceSn, int limit)
    {
        using var db = CreateDb();
        var device = GetOrCreateDevice(db, deviceSn);
        if (device.PendingAddPeopleCount <= 0)
            return Array.Empty<PersonInfo>();

        var batchSize = limit > 0 ? Math.Min(limit, 1000) : 1000;
        var staged = db.DevicePeople.Where(x => x.DeviceSn == deviceSn && x.Staged).Select(x => x.UserId).ToList();
        var downloaded = db.DevicePeople.Where(x => x.DeviceSn == deviceSn && x.Downloaded).Select(x => x.UserId).ToHashSet(StringComparer.OrdinalIgnoreCase);

        var query = db.People.AsQueryable();
        if (staged.Count > 0)
            query = query.Where(p => staged.Contains(p.UserId));

        return query.AsEnumerable()
            .Where(p => !downloaded.Contains(p.UserId))
            .Take(batchSize)
            .Select(ToPersonInfo)
            .ToArray();
    }

    public IReadOnlyCollection<string> GetDeletePeople(string deviceSn)
    {
        using var db = CreateDb();
        return db.PendingDeletes.Where(x => x.DeviceSn == deviceSn).Select(x => x.UserId).ToArray();
    }

    public void ConfirmPeopleDownloaded(string deviceSn, int successCount)
    {
        using var db = CreateDb();
        var device = GetOrCreateDevice(db, deviceSn);
        var pending = db.People.Select(p => p.UserId).ToList();
        var downloaded = db.DevicePeople.Where(x => x.DeviceSn == deviceSn && x.Downloaded).Select(x => x.UserId).ToHashSet(StringComparer.OrdinalIgnoreCase);
        var remaining = pending.Count(id => !downloaded.Contains(id));
        if (remaining == 0)
            device.PendingAddPeopleCount = 0;
        db.SaveChanges();
    }

    public void SaveUploadedWorkSetting(string deviceSn, JsonObject setting)
    {
        using var db = CreateDb();
        var device = GetOrCreateDevice(db, deviceSn);
        device.LastUploadedWorkSetting = setting.ToJsonString();
        device.LastWorkSettingUploadAt = DateTime.UtcNow;
        device.PendingUploadWorkParameter = false;
        device.DesiredWorkSetting = null;
        db.SaveChanges();
    }

    public JsonObject? GetWorkSettingForDownload(string deviceSn)
    {
        using var db = CreateDb();
        var device = db.Devices.Find(deviceSn);
        if (device == null)
            return null;

        var source = device.DesiredWorkSetting ?? device.LastUploadedWorkSetting;
        if (string.IsNullOrWhiteSpace(source))
            return null;

        device.PendingSyncParameter = false;
        var copy = JsonNode.Parse(source) as JsonObject;
        if (copy == null)
            return null;
        copy.Remove("Success");
        copy["DeviceSN"] = deviceSn;
        db.SaveChanges();
        return copy;
    }

    public void SaveIdentifyRecord(string deviceSn, JsonNode? recordNode, IFormFile? photo)
    {
        using var db = CreateDb();
        GetOrCreateDevice(db, deviceSn);

        var recordId = SanitizeForFileName(recordNode?["RecordID"]?.ToString())
                       ?? DateTimeOffset.UtcNow.ToUnixTimeMilliseconds().ToString();

        string? photoPath = null;
        if (photo != null && photo.Length > 0)
        {
            var fileName = $"{DateTime.UtcNow:yyyyMMddHHmmssfff}_{SanitizeForFileName(recordId)}.jpg";
            photoPath = Path.Combine(_photosPath, fileName);
            using var fs = File.Create(photoPath);
            photo.CopyTo(fs);
        }

        if (recordNode is JsonObject obj && obj["DeviceSN"] is null)
            obj["DeviceSN"] = deviceSn;

        DateTime? recordTime = null;
        if (long.TryParse(recordNode?["RecordDate"]?.ToString() ?? recordNode?["RecordTime"]?.ToString(), out var unix))
        {
            recordTime = DateTimeOffset.FromUnixTimeSeconds(unix).UtcDateTime;
        }

        db.IdentifyRecords.Add(new IdentifyRecordEntity
        {
            DeviceSn = deviceSn,
            RecordId = recordId,
            UserId = recordNode?["UserID"]?.ToString(),
            UserName = recordNode?["Name"]?.ToString(),
            RecordType = recordNode?["RecordType"]?.GetValue<int?>(),
            RecordTime = recordTime,
            Temperature = recordNode?["Temperature"]?.ToString(),
            PhotoPath = photoPath,
            RawJson = recordNode?.ToJsonString(),
            ReceivedAt = DateTime.UtcNow
        });
        db.SaveChanges();
    }

    public void SaveSystemRecord(string deviceSn, JsonNode? recordNode)
    {
        SaveIdentifyRecord(deviceSn, recordNode, null);
    }

    public IReadOnlyCollection<DeviceSummary> GetDeviceSummaries()
    {
        using var db = CreateDb();
        var deleteCounts = db.PendingDeletes.GroupBy(x => x.DeviceSn)
            .ToDictionary(g => g.Key, g => g.Count(), StringComparer.OrdinalIgnoreCase);
        var recordCounts = db.IdentifyRecords.GroupBy(x => x.DeviceSn)
            .ToDictionary(g => g.Key, g => g.Count(), StringComparer.OrdinalIgnoreCase);

        return db.Devices.AsNoTracking().Select(d => new DeviceSummary
        {
            SN = d.Sn,
            IpAddress = d.IpAddress,
            HttpPort = d.HttpPort,
            DeviceName = d.DeviceName,
            TagName = d.TagName,
            Model = d.Model,
            FirmwareVersion = d.FirmwareVersion,
            UnitNo = d.UnitNo,
            ConnectedAtUtc = ToOffset(d.ConnectedAt),
            LastKeepaliveAtUtc = ToOffset(d.LastKeepaliveAt),
            LastWorkSettingUploadAtUtc = ToOffset(d.LastWorkSettingUploadAt),
            PendingSyncParameter = d.PendingSyncParameter,
            PendingUploadWorkParameter = d.PendingUploadWorkParameter,
            PendingAddPeopleCount = d.PendingAddPeopleCount,
            PendingDeletePeopleCount = deleteCounts.GetValueOrDefault(d.Sn),
            RecordCount = recordCounts.GetValueOrDefault(d.Sn)
        }).ToArray();
    }

    public DeviceSnapshot? GetDevice(string deviceSn)
    {
        using var db = CreateDb();
        var d = db.Devices.AsNoTracking().FirstOrDefault(x => x.Sn == deviceSn);
        return d == null ? null : ToSnapshot(db, d);
    }

    public void MarkSyncRequested(string deviceSn)
    {
        using var db = CreateDb();
        GetOrCreateDevice(db, deviceSn).PendingSyncParameter = true;
        db.SaveChanges();
    }

    public void ResetPendingState(string deviceSn)
    {
        using var db = CreateDb();
        var device = GetOrCreateDevice(db, deviceSn);
        device.PendingSyncParameter = false;
        device.PendingUploadWorkParameter = false;
        device.PendingAddPeopleCount = 0;
        device.PendingRemoteJson = null;
        db.PendingDeletes.RemoveRange(db.PendingDeletes.Where(x => x.DeviceSn == deviceSn));
        db.SaveChanges();
    }

    public void MarkUploadWorkSettingRequested(string deviceSn)
    {
        using var db = CreateDb();
        GetOrCreateDevice(db, deviceSn).PendingUploadWorkParameter = true;
        db.SaveChanges();
    }

    public int MarkAddPeopleRequested(string deviceSn)
    {
        using var db = CreateDb();
        var device = GetOrCreateDevice(db, deviceSn);
        var stagedCount = db.DevicePeople.Count(x => x.DeviceSn == deviceSn && x.Staged);
        var count = stagedCount > 0 ? stagedCount : db.People.Count();
        device.PendingAddPeopleCount = count;
        db.SaveChanges();
        return count;
    }

    public int MarkDeletePeopleRequested(string deviceSn)
    {
        using var db = CreateDb();
        GetOrCreateDevice(db, deviceSn);
        var ids = db.People.Select(p => p.UserId).ToList();
        foreach (var id in ids)
        {
            if (!db.PendingDeletes.Any(x => x.DeviceSn == deviceSn && x.UserId == id))
                db.PendingDeletes.Add(new PendingDeleteEntity { DeviceSn = deviceSn, UserId = id });
        }
        db.SaveChanges();
        return ids.Count;
    }

    public void SetDesiredWorkSetting(string deviceSn, JsonObject patch)
    {
        using var db = CreateDb();
        var device = GetOrCreateDevice(db, deviceSn);
        JsonObject target;
        if (!string.IsNullOrWhiteSpace(device.DesiredWorkSetting))
            target = JsonNode.Parse(device.DesiredWorkSetting) as JsonObject ?? new JsonObject();
        else if (!string.IsNullOrWhiteSpace(device.LastUploadedWorkSetting))
            target = JsonNode.Parse(device.LastUploadedWorkSetting) as JsonObject ?? new JsonObject();
        else
            target = new JsonObject();

        foreach (var kv in patch)
            target[kv.Key] = kv.Value?.DeepClone();

        device.DesiredWorkSetting = target.ToJsonString();
        device.PendingSyncParameter = true;
        db.SaveChanges();
    }

    public void SetPendingRemoteCommand(string deviceSn, PendingRemoteCommand cmd)
    {
        using var db = CreateDb();
        GetOrCreateDevice(db, deviceSn).PendingRemoteJson = JsonSerializer.Serialize(cmd, _json);
        db.SaveChanges();
    }

    public PendingRemoteCommand? ConsumeRemoteCommand(string deviceSn)
    {
        using var db = CreateDb();
        var device = db.Devices.Find(deviceSn);
        if (device == null || string.IsNullOrWhiteSpace(device.PendingRemoteJson))
            return null;
        var cmd = JsonSerializer.Deserialize<PendingRemoteCommand>(device.PendingRemoteJson, _json);
        device.PendingRemoteJson = null;
        db.SaveChanges();
        return cmd;
    }

    public IReadOnlyCollection<DepartmentInfo> GetDepartments()
    {
        using var db = CreateDb();
        return db.Departments.AsNoTracking()
            .Select(d => new DepartmentInfo { DepartmentID = d.DepartmentId, Name = d.Name })
            .ToArray();
    }

    public bool TryAddDepartment(DepartmentInfo dept)
    {
        using var db = CreateDb();
        if (db.Departments.Any(d => d.DepartmentId == dept.DepartmentID))
            return false;
        db.Departments.Add(new DepartmentEntity { DepartmentId = dept.DepartmentID, Name = dept.Name });
        db.SaveChanges();
        return true;
    }

    public bool DeleteDepartment(string deptId)
    {
        using var db = CreateDb();
        var entity = db.Departments.Find(deptId);
        if (entity == null)
            return false;
        db.Departments.Remove(entity);
        db.SaveChanges();
        return true;
    }

    public void SaveSystemRecords(string deviceSn, int recordType, List<SystemRecordItem> items)
    {
        foreach (var item in items)
        {
            var node = new JsonObject
            {
                ["RecordID"] = item.RecordID,
                ["RecordType"] = item.RecordType != 0 ? item.RecordType : recordType,
                ["RecordDate"] = item.RecordDate
            };
            SaveSystemRecord(deviceSn, node);
        }
    }

    public (int success, int fail) SavePushedPeople(string deviceSn, List<PersonInfo> people, bool addOnly = false)
    {
        int success = 0, fail = 0;
        using var db = CreateDb();
        GetOrCreateDevice(db, deviceSn);
        foreach (var person in people)
        {
            try
            {
                // 서버 마스터 사용자는 '사용자 가져오기'(Query)에서만 변경한다.
                // 단말기의 개별 추가/수정 Push는 단말기 소유 목록만 갱신한다.
                UpsertDevicePerson(db, deviceSn, person.UserID, downloaded: true, owned: true, person: person);
                success++;
            }
            catch
            {
                fail++;
            }
        }
        db.SaveChanges();
        return (success, fail);
    }

    public void BeginOwnedPeopleQuery(string deviceSn)
    {
        using var db = CreateDb();
        GetOrCreateDevice(db, deviceSn);
        var rows = db.DevicePeople.Where(x => x.DeviceSn == deviceSn && x.Owned).ToList();
        foreach (var row in rows)
            row.Owned = false;
        db.SaveChanges();
    }

    public (int success, int fail, List<(string userId, string photoPath)> photoPathsToFetch) ReplaceDeviceOwnedPeople(string deviceSn, List<PersonInfo> people)
    {
        int success = 0, fail = 0;
        var photoPaths = new List<(string, string)>();
        using var db = CreateDb();
        GetOrCreateDevice(db, deviceSn);
        foreach (var person in people)
        {
            try
            {
                var existing = db.People.Find(person.UserID);
                if (existing == null)
                    db.People.Add(ToEntity(person));
                else
                    CopyToEntity(person, existing);
                UpsertDevicePerson(db, deviceSn, person.UserID, downloaded: true, owned: true);
                db.SaveChanges();
                success++;
            }
            catch (Exception ex)
            {
                LogHub.Instance.Error($"[PushPeople-Query] 저장 실패 UserID={person.UserID}: {ex.Message}");
                fail++;
            }
        }
        return (success, fail, photoPaths);
    }

    public (int success, int fail) DeletePushedPeople(string deviceSn, List<PersonInfo> people)
    {
        int success = 0, fail = 0;
        using var db = CreateDb();
        foreach (var person in people)
        {
            try
            {
                var rows = db.DevicePeople.Where(x => x.DeviceSn == deviceSn && x.UserId == person.UserID);
                db.DevicePeople.RemoveRange(rows);
                success++;
            }
            catch
            {
                fail++;
            }
        }
        db.SaveChanges();
        return (success, fail);
    }

    public void ConfirmDeletePeopleResult(string deviceSn, List<string> confirmedIds)
    {
        using var db = CreateDb();
        var rows = db.PendingDeletes.Where(x => x.DeviceSn == deviceSn && confirmedIds.Contains(x.UserId));
        db.PendingDeletes.RemoveRange(rows);
        db.SaveChanges();
    }

    public void StageServerPeopleForDevice(string deviceSn, IReadOnlyCollection<PersonInfo> serverPeople)
    {
        using var db = CreateDb();
        GetOrCreateDevice(db, deviceSn);

        // 이번 배포 대상만 staged로 남긴다
        foreach (var row in db.DevicePeople.Where(x => x.DeviceSn == deviceSn && x.Staged))
            row.Staged = false;

        var targetIds = serverPeople.Select(p => p.UserID).ToHashSet(StringComparer.OrdinalIgnoreCase);
        foreach (var person in serverPeople)
            UpsertDevicePerson(db, deviceSn, person.UserID, staged: true, downloaded: false);

        db.SaveChanges();
    }

    public void ClearAllRecords()
    {
        using var db = CreateDb();
        db.IdentifyRecords.RemoveRange(db.IdentifyRecords);
        db.SaveChanges();
    }

    public void ClearRecordsByType(int recordType)
    {
        using var db = CreateDb();
        db.IdentifyRecords.RemoveRange(db.IdentifyRecords.Where(x => x.RecordType == recordType));
        db.SaveChanges();
    }

    public bool ConnectDevice(string deviceSn, string ipAddress, int httpPort, string? deviceName = null, string? tagName = null, string? model = null, string? firmwareVersion = null, int unitNo = 0)
    {
        using var db = CreateDb();
        var device = GetOrCreateDevice(db, deviceSn);
        device.IpAddress = ipAddress;
        device.HttpPort = httpPort;
        if (deviceName != null) device.DeviceName = deviceName;
        if (tagName != null) device.TagName = tagName;
        if (model != null) device.Model = model;
        if (firmwareVersion != null) device.FirmwareVersion = firmwareVersion;
        device.UnitNo = unitNo;
        device.ConnectedAt = DateTime.UtcNow;
        db.SaveChanges();
        return true;
    }


    public (int saved, int skipped, int errors) SavePeopleToFiles(IEnumerable<string>? userIds)
    {
        int saved = 0, skipped = 0, errors = 0;
        Directory.CreateDirectory(_peoplePath);

        using var db = CreateDb();
        IEnumerable<PersonEntity> query;
        var ids = userIds?.Where(id => !string.IsNullOrWhiteSpace(id)).Distinct().ToList() ?? new List<string>();
        if (ids.Count == 0)
        {
            skipped = 0;
            return (0, 0, 0);
        }

        var people = db.People.AsNoTracking().Where(p => ids.Contains(p.UserId)).ToList();
        var found = people.Select(p => p.UserId).ToHashSet(StringComparer.OrdinalIgnoreCase);
        skipped = ids.Count(id => !found.Contains(id));

        var opts = new JsonSerializerOptions(_json) { WriteIndented = true };
        foreach (var entity in people)
        {
            try
            {
                var fileName = SanitizeForFileName(entity.UserId);
                if (string.IsNullOrEmpty(fileName))
                {
                    skipped++;
                    continue;
                }
                var path = Path.Combine(_peoplePath, fileName + ".json");
                var json = JsonSerializer.Serialize(ToPersonInfo(entity), opts);
                File.WriteAllText(path, json);
                saved++;
            }
            catch
            {
                errors++;
            }
        }

        return (saved, skipped, errors);
    }

    public (int loaded, int skipped, int errors) ReloadPeopleFromFiles()
    {
        int loaded = 0, skipped = 0, errors = 0;
        if (!Directory.Exists(_peoplePath))
            return (0, 0, 0);

        var files = Directory.GetFiles(_peoplePath, "*.json", SearchOption.TopDirectoryOnly);
        using var db = CreateDb();
        foreach (var file in files)
        {
            try
            {
                var json = File.ReadAllText(file);
                var person = JsonSerializer.Deserialize<PersonInfo>(json, _json);
                if (person == null || string.IsNullOrWhiteSpace(person.UserID))
                {
                    skipped++;
                    continue;
                }

                person.Fingerprints ??= new();
                person.Palmveins ??= new();

                var existing = db.People.Find(person.UserID);
                if (existing == null)
                    db.People.Add(ToEntity(person));
                else
                    CopyToEntity(person, existing);
                loaded++;
            }
            catch
            {
                errors++;
            }
        }

        db.SaveChanges();
        return (loaded, skipped, errors);
    }

    public string? GetPersonFilePath(string userId) => null;

    public string? ExportPersonJson(string userId)
    {
        using var db = CreateDb();
        var person = db.People.AsNoTracking().FirstOrDefault(p => p.UserId == userId);
        return person == null ? null : JsonSerializer.Serialize(ToPersonInfo(person), _json);
    }

    public void UpdatePersonPhoto(string userId, string base64Photo)
    {
        using var db = CreateDb();
        var person = db.People.Find(userId);
        if (person == null)
            return;
        person.Photo = base64Photo;
        person.PhotoLen = base64Photo?.Length ?? 0;
        person.UpdatedAt = DateTime.UtcNow;
        db.SaveChanges();
    }

    public IReadOnlyCollection<PersonInfo> GetDeviceOwnedPeople(string deviceSn)
    {
        using var db = CreateDb();
        var ids = db.DevicePeople.Where(x => x.DeviceSn == deviceSn && x.Owned).Select(x => x.UserId).ToList();
        return db.People.Where(p => ids.Contains(p.UserId)).AsEnumerable().Select(ToPersonInfo).ToArray();
    }

    public void UpsertDeviceOwnedPerson(string deviceSn, PersonInfo person)
    {
        using var db = CreateDb();
        GetOrCreateDevice(db, deviceSn);
        var existing = db.People.Find(person.UserID);
        if (existing == null)
            db.People.Add(ToEntity(person));
        else
            CopyToEntity(person, existing);
        UpsertDevicePerson(db, deviceSn, person.UserID, owned: true, downloaded: true, person: person);
        db.SaveChanges();
    }

    public void DeleteDeviceOwnedPerson(string deviceSn, string userId)
    {
        using var db = CreateDb();
        var row = db.DevicePeople.FirstOrDefault(x => x.DeviceSn == deviceSn && x.UserId == userId);
        if (row != null)
            db.DevicePeople.Remove(row);
        db.SaveChanges();
    }

    public void UpdateDeviceInfo(string deviceSn, string? deviceName, string? tagName)
    {
        using var db = CreateDb();
        var device = GetOrCreateDevice(db, deviceSn);
        device.DeviceName = deviceName;
        device.TagName = tagName;
        db.SaveChanges();
    }

    public bool RemoveDevice(string deviceSn)
    {
        using var db = CreateDb();
        var device = db.Devices.Find(deviceSn);
        if (device == null)
            return false;
        db.Devices.Remove(device);
        db.SaveChanges();
        return true;
    }

    public void QueueRemoteCommand(string deviceSn, bool restart = false, bool opendoor = false,
        bool closealarm = false, bool clearRecord = false, bool repostRecord = false, bool pushAllPeople = false)
    {
        SetPendingRemoteCommand(deviceSn, new PendingRemoteCommand
        {
            Restart = restart ? 1 : null,
            Opendoor = opendoor ? 1 : null,
            Closealarm = closealarm ? 1 : null,
            ClearRecord = clearRecord ? 1 : null,
            RepostRecord = repostRecord ? 1 : null,
            PushAllPeople = pushAllPeople ? 1 : null
        });
    }

    public void QueueSyncTime(string deviceSn, long unixTimestamp)
    {
        SetPendingRemoteCommand(deviceSn, new PendingRemoteCommand { SetTime = unixTimestamp });
    }

    public List<string> GetAllDeviceSNs()
    {
        using var db = CreateDb();
        return db.Devices.Select(d => d.Sn).ToList();
    }

    private static DeviceEntity GetOrCreateDevice(FaceDeviceDbContext db, string sn)
    {
        var device = db.Devices.Find(sn);
        if (device != null)
            return device;
        device = new DeviceEntity { Sn = sn };
        db.Devices.Add(device);
        db.SaveChanges();
        return device;
    }

    private static void UpsertDevicePerson(FaceDeviceDbContext db, string deviceSn, string userId,
        bool? downloaded = null, bool? staged = null, bool? owned = null, PersonInfo? person = null)
    {
        var row = db.DevicePeople.FirstOrDefault(x => x.DeviceSn == deviceSn && x.UserId == userId);
        if (row == null)
        {
            row = new DevicePersonEntity { DeviceSn = deviceSn, UserId = userId };
            db.DevicePeople.Add(row);
        }
        if (downloaded.HasValue) row.Downloaded = downloaded.Value;
        if (staged.HasValue) row.Staged = staged.Value;
        if (owned.HasValue) row.Owned = owned.Value;
        row.UpdatedAt = DateTime.UtcNow;
    }


    public IReadOnlyList<RecordSnapshot> GetAllRecords()
    {
        using var db = CreateDb();
        return db.IdentifyRecords.AsNoTracking()
            .OrderByDescending(r => r.ReceivedAt)
            .ToList()
            .Select(ToRecordSnapshot)
            .ToList();
    }

    private static RecordSnapshot ToRecordSnapshot(IdentifyRecordEntity e)
    {
        JsonNode? detail = null;
        if (!string.IsNullOrWhiteSpace(e.RawJson))
        {
            try { detail = JsonNode.Parse(e.RawJson); }
            catch { detail = null; }
        }
        if (detail is null)
        {
            detail = new JsonObject
            {
                ["DeviceSN"] = e.DeviceSn,
                ["UserID"] = e.UserId,
                ["Name"] = e.UserName,
                ["RecordType"] = e.RecordType,
                ["RecordID"] = e.RecordId,
                ["Temperature"] = e.Temperature
            };
            if (e.RecordTime.HasValue)
            {
                var local = DateTime.SpecifyKind(e.RecordTime.Value, DateTimeKind.Utc).ToLocalTime();
                detail["RecordTime"] = local.ToString("yyyy-MM-dd HH:mm:ss");
                detail["RecordDate"] = new DateTimeOffset(DateTime.SpecifyKind(e.RecordTime.Value, DateTimeKind.Utc)).ToUnixTimeSeconds();
            }
        }
        else if (detail is JsonObject obj)
        {
            if (obj["DeviceSN"] is null) obj["DeviceSN"] = e.DeviceSn;
            if (obj["RecordType"] is null && e.RecordType.HasValue) obj["RecordType"] = e.RecordType;
        }

        return new RecordSnapshot
        {
            Id = e.Id.ToString(),
            ReceivedAtUtc = new DateTimeOffset(DateTime.SpecifyKind(e.ReceivedAt, DateTimeKind.Utc)),
            RecordJsonPath = e.PhotoPath ?? string.Empty,
            PhotoPath = e.PhotoPath,
            RecordDetail = detail
        };
    }

    private DeviceSnapshot ToSnapshot(FaceDeviceDbContext db, DeviceEntity d)
    {
        KeepaliveRequest? lastKa = null;
        if (!string.IsNullOrWhiteSpace(d.LastKeepaliveJson))
            lastKa = JsonSerializer.Deserialize<KeepaliveRequest>(d.LastKeepaliveJson, _json);

        PendingRemoteCommand? remote = null;
        if (!string.IsNullOrWhiteSpace(d.PendingRemoteJson))
            remote = JsonSerializer.Deserialize<PendingRemoteCommand>(d.PendingRemoteJson, _json);

        var ownedIds = db.DevicePeople.Where(x => x.DeviceSn == d.Sn && x.Owned).Select(x => x.UserId).ToList();
        var stagedIds = db.DevicePeople.Where(x => x.DeviceSn == d.Sn && x.Staged).Select(x => x.UserId).ToList();
        var downloaded = db.DevicePeople.Where(x => x.DeviceSn == d.Sn && x.Downloaded).Select(x => x.UserId).ToList();
        var people = db.People.ToList();

        return new DeviceSnapshot
        {
            SN = d.Sn,
            IpAddress = d.IpAddress,
            HttpPort = d.HttpPort,
            DeviceName = d.DeviceName,
            TagName = d.TagName,
            Model = d.Model,
            FirmwareVersion = d.FirmwareVersion,
            UnitNo = d.UnitNo,
            ConnectedAtUtc = ToOffset(d.ConnectedAt),
            LastKeepalive = lastKa,
            LastKeepaliveAtUtc = ToOffset(d.LastKeepaliveAt),
            LastWorkSettingUploadAtUtc = ToOffset(d.LastWorkSettingUploadAt),
            LastUploadedWorkSetting = ParseObject(d.LastUploadedWorkSetting),
            DesiredWorkSetting = ParseObject(d.DesiredWorkSetting),
            PendingSyncParameter = d.PendingSyncParameter,
            PendingUploadWorkParameter = d.PendingUploadWorkParameter,
            PendingAddPeopleCount = d.PendingAddPeopleCount,
            PendingDeleteUserIds = db.PendingDeletes.Where(x => x.DeviceSn == d.Sn).Select(x => x.UserId).ToList(),
            DownloadedUserIds = downloaded,
            OwnedPeople = people.Where(p => ownedIds.Contains(p.UserId)).ToDictionary(p => p.UserId, ToPersonInfo, StringComparer.OrdinalIgnoreCase),
            StagedPeople = people.Where(p => stagedIds.Contains(p.UserId)).ToDictionary(p => p.UserId, ToPersonInfo, StringComparer.OrdinalIgnoreCase),
            PendingRemote = remote
        };
    }

    private static JsonObject? ParseObject(string? json)
        => string.IsNullOrWhiteSpace(json) ? null : JsonNode.Parse(json) as JsonObject;

    private static DateTimeOffset? ToOffset(DateTime? utc)
        => utc.HasValue ? new DateTimeOffset(DateTime.SpecifyKind(utc.Value, DateTimeKind.Utc)) : null;

    private static string? SanitizeForFileName(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return null;
        var chars = value.Select(c => InvalidFileNameChars.Contains(c) ? '_' : c).ToArray();
        return new string(chars);
    }

    private PersonInfo ToPersonInfo(PersonEntity e) => new()
    {
        UserID = e.UserId,
        Code = e.Code ?? e.UserId,
        Name = e.Name ?? string.Empty,
        Job = e.Job ?? string.Empty,
        Department = e.Department ?? string.Empty,
        IdentityCard = e.IdentityCard ?? string.Empty,
        Attachment = e.Attachment ?? string.Empty,
        Photo = e.Photo ?? string.Empty,
        PhotoMD5 = e.PhotoMd5 ?? string.Empty,
        PhotoLen = e.PhotoLen,
        Password = e.Password ?? string.Empty,
        CardNum = string.IsNullOrWhiteSpace(e.CardNum) ? "0" : e.CardNum,
        QRCode = e.QrCode ?? string.Empty,
        AccessType = e.AccessType,
        ExpirationDate = e.ExpirationDate,
        OpenTimes = e.OpenTimes,
        KeepOpen = e.KeepOpen,
        Timegroup = e.Timegroup,
        Holidays = e.Holidays ?? string.Empty,
        Elevators = e.Elevators ?? string.Empty,
        FaceFeature = e.FaceFeature ?? string.Empty,
        FaceFeatureMD5 = e.FaceFeatureMd5 ?? string.Empty,
        Fingerprints = string.IsNullOrWhiteSpace(e.FingerprintsJson)
            ? new List<FingerprintItem>()
            : JsonSerializer.Deserialize<List<FingerprintItem>>(e.FingerprintsJson, _json) ?? new(),
        Palmveins = string.IsNullOrWhiteSpace(e.PalmveinsJson)
            ? new List<PalmveinItem>()
            : JsonSerializer.Deserialize<List<PalmveinItem>>(e.PalmveinsJson, _json) ?? new()
    };

    private PersonEntity ToEntity(PersonInfo p)
    {
        var e = new PersonEntity { UserId = p.UserID };
        CopyToEntity(p, e);
        return e;
    }

    private void CopyToEntity(PersonInfo p, PersonEntity e)
    {
        e.Code = string.IsNullOrWhiteSpace(p.Code) ? p.UserID : p.Code;
        e.Name = p.Name;
        e.Job = p.Job;
        e.Department = p.Department;
        e.IdentityCard = p.IdentityCard;
        e.Attachment = p.Attachment;
        e.Photo = p.Photo;
        e.PhotoMd5 = p.PhotoMD5;
        e.PhotoLen = p.PhotoLen;
        e.Password = p.Password;
        e.CardNum = string.IsNullOrWhiteSpace(p.CardNum) ? "0" : p.CardNum;
        e.QrCode = p.QRCode;
        e.AccessType = p.AccessType;
        e.ExpirationDate = p.ExpirationDate;
        e.OpenTimes = p.OpenTimes;
        e.KeepOpen = p.KeepOpen;
        e.Timegroup = p.Timegroup == 0 ? 1 : p.Timegroup;
        e.Holidays = p.Holidays;
        e.Elevators = p.Elevators;
        e.FaceFeature = p.FaceFeature;
        e.FaceFeatureMd5 = p.FaceFeatureMD5;
        e.FingerprintsJson = JsonSerializer.Serialize(p.Fingerprints ?? new(), _json);
        e.PalmveinsJson = JsonSerializer.Serialize(p.Palmveins ?? new(), _json);
        e.UpdatedAt = DateTime.UtcNow;
    }
}
