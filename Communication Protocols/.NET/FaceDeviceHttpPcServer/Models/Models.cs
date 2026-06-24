using System.Text.Json.Nodes;

namespace FaceDeviceHttpPcServer.Models;

// 式式式 Common response formats 式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式

/// <summary>HTTP-Docking protocol response (Success 1 = OK, others = error code)</summary>
public record ApiResponse(int Success, string? Message = null)
{
    public static ApiResponse Ok(string? message = null) => new(1, message);
}

/// <summary>HTTP-Docking protocol response with Content field</summary>
public sealed class ApiResponseWithContent
{
    public int Success { get; set; }
    public object? Content { get; set; }

    public static ApiResponseWithContent Ok(object? content = null) => 
        new() { Success = 1, Content = content };

    public static ApiResponseWithContent Error(int code, string? message = null) => 
        new() { Success = code, Content = message };
}

/// <summary>Browser-UI protocol unified response</summary>
public sealed class BrowserApiResponse
{
    public bool result { get; set; }
    public object? content { get; set; }
    public int errCode { get; set; }
    public string? error { get; set; }

    public static BrowserApiResponse Ok(object? content = null) =>
        new() { result = true, content = content };

    public static BrowserApiResponse Fail(int code, string err) =>
        new() { result = false, errCode = code, error = err };
}

// 式式式 Browser-UI: User / Login 式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式

public sealed class LoginRequest
{
    public string password { get; set; } = string.Empty;
}

// 式式式 Browser-UI: Department 式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式

public sealed class DepartmentInfo
{
    public string DepartmentID { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
}

public sealed class DepartmentSearchRequest
{
    public int PageIndex { get; set; } = 1;
    public int PageSize { get; set; } = 20;
    public string? Name { get; set; }
}

// 式式式 Browser-UI: People search 式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式

public sealed class PeopleSearchRequest
{
    public int PageIndex { get; set; } = 1;
    public int PageSize { get; set; } = 20;
    public string? UserID { get; set; }
    public string? Name { get; set; }
    public string? Job { get; set; }
    public string? Department { get; set; }
    public int? AccessType { get; set; }
    public int? Timegroup { get; set; }
    public int? Photo { get; set; }
    public string? CardNum { get; set; }
    public string? IdentityCard { get; set; }
    public int? Fingerprint { get; set; }
    public int? Palmprint { get; set; }
    public string? OrderByColumn { get; set; }
    public string? OrderByType { get; set; }
}

public sealed class PeopleSearchResult
{
    public int TotalCount { get; set; }
    public int PageIndex { get; set; }
    public int PageSize { get; set; }
    public List<PersonInfo> DataList { get; set; } = new();
}

// 式式式 Browser-UI: Record search 式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式

public sealed class RecordSearchRequest
{
    public int PageIndex { get; set; } = 1;
    public int PageSize { get; set; } = 20;
    public long BeginDate { get; set; }
    public long EndDate { get; set; }
    public string? UserID { get; set; }
    public string? Name { get; set; }
    public string? Department { get; set; }
    public string? Job { get; set; }
    public string? CardNum { get; set; }
    public string? IdentityCard { get; set; }
    public string? RecordTypes { get; set; }
    public int? PhotoBase64 { get; set; }
    public long? RecordID { get; set; }
    public string? OrderByColumn { get; set; }
    public string? OrderByType { get; set; }
}

public sealed class DeleteRecordsByTypeRequest
{
    public int RecordType { get; set; }
}

// 式式式 Browser-UI: Device 式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式

public sealed class DeviceRemoteRequest
{
    public int? Opendoor { get; set; }
    public int? Restart { get; set; }
    public int? Recover { get; set; }
    public int? Closealarm { get; set; }
}

// 式式式 HTTP-Docking Protocol models 式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式

public sealed class KeepaliveRequest
{
    public string SN { get; set; } = string.Empty;
    public int RelayStatus { get; set; }
    public int KeepOpenStatus { get; set; }
    public int DoorSensorStatus { get; set; }
    public int LockDoorStatus { get; set; }
    public string AlarmStatus { get; set; } = string.Empty;
}

public sealed class KeepaliveResponse
{
    // Protocol: Success = 1 indicates success, other values are error codes
    public int Success { get; set; } = 1;
    public string? Message { get; set; }
    public int? AddPeople { get; set; }
    public int? DeletePeople { get; set; }
    public int? SyncParameter { get; set; }
    public int? Remote { get; set; }
    public int? UploadWorkParameter { get; set; }
}

public sealed class PersonInfo
{
    public string UserID { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;  // Device requires this field (usually same as UserID)
    public string Name { get; set; } = string.Empty;
    public string Job { get; set; } = string.Empty;
    public string Department { get; set; } = string.Empty;
    public string IdentityCard { get; set; } = string.Empty;
    public string Attachment { get; set; } = string.Empty;
    public string Photo { get; set; } = string.Empty;
    public string PhotoMD5 { get; set; } = string.Empty;
    public int PhotoLen { get; set; }
    public string Password { get; set; } = string.Empty;
    public string CardNum { get; set; } = "0";  // Default "0" for no card, not empty string
    public string QRCode { get; set; } = string.Empty;
    public int AccessType { get; set; }
    public uint ExpirationDate { get; set; }
    public int OpenTimes { get; set; } = 65535;
    public int KeepOpen { get; set; }
    public int Timegroup { get; set; } = 1;  // Default to time zone 1 (0 means no access)
    public string Holidays { get; set; } = string.Empty;
    public string Elevators { get; set; } = string.Empty;
    public string FaceFeature { get; set; } = string.Empty;
    public string FaceFeatureMD5 { get; set; } = string.Empty;
    public List<FingerprintItem> Fingerprints { get; set; } = new();
    public List<PalmveinItem> Palmveins { get; set; } = new();
}

public sealed class FingerprintItem
{
    public int Num { get; set; }
    public string Data { get; set; } = string.Empty;
    public string MD5 { get; set; } = string.Empty;
}

public sealed class PalmveinItem
{
    public int Num { get; set; }
    public string Data { get; set; } = string.Empty;
    public string MD5 { get; set; } = string.Empty;
}

public sealed class DownloadPeopleListRequest
{
    public string SN { get; set; } = string.Empty;
    public int Limit { get; set; }
}

public sealed class DownloadPeopleListResponse
{
    // Device expects Success=1 for successful response
    public int Success { get; set; } = 1;
    public string? Message { get; set; }
    public int PeopleCount { get; set; }
    public List<PersonInfo> PeopleList { get; set; } = new();
}

public sealed class SelectDeleteInfoRequest
{
    public string SN { get; set; } = string.Empty;
}

public sealed class SelectDeleteInfoResponse
{
    // Device expects Success=1 for successful response
    public int Success { get; set; } = 1;
    public string? Message { get; set; }
    public List<string> DeleteList { get; set; } = new();
}

public sealed class DeviceSnapshot
{
    public string SN { get; set; } = string.Empty;
    public string? IpAddress { get; set; }
    public int HttpPort { get; set; } = 80;
    public string? DeviceName { get; set; }
    public string? TagName { get; set; }
    public string? Model { get; set; }
    public string? FirmwareVersion { get; set; }
    public int UnitNo { get; set; }
    public DateTimeOffset? ConnectedAtUtc { get; set; }
    public KeepaliveRequest? LastKeepalive { get; set; }
    public DateTimeOffset? LastKeepaliveAtUtc { get; set; }
    public DateTimeOffset? LastWorkSettingUploadAtUtc { get; set; }
    public JsonObject? LastUploadedWorkSetting { get; set; }
    public JsonObject? DesiredWorkSetting { get; set; }
    public bool PendingSyncParameter { get; set; }
    public bool PendingUploadWorkParameter { get; set; }
    public int PendingAddPeopleCount { get; set; }
    public List<string> PendingDeleteUserIds { get; set; } = new();
    public List<string> DownloadedUserIds { get; set; } = new(); // Track users already sent to device in current batch
    public List<RecordSnapshot> Records { get; set; } = new();
    public PendingRemoteCommand? PendingRemote { get; set; }
}

public sealed class RecordSnapshot
{
    public string Id { get; set; } = string.Empty;
    public DateTimeOffset ReceivedAtUtc { get; set; }
    public string RecordJsonPath { get; set; } = string.Empty;
    public string? PhotoPath { get; set; }
    public JsonNode? RecordDetail { get; set; }
}

public sealed class DeviceSummary
{
    public string SN { get; set; } = string.Empty;
    public string? IpAddress { get; set; }
    public int HttpPort { get; set; } = 80;
    public string? DeviceName { get; set; }
    public string? TagName { get; set; }
    public string? Model { get; set; }
    public string? FirmwareVersion { get; set; }
    public int UnitNo { get; set; }
    public DateTimeOffset? ConnectedAtUtc { get; set; }
    public DateTimeOffset? LastKeepaliveAtUtc { get; set; }
    public DateTimeOffset? LastWorkSettingUploadAtUtc { get; set; }
    public bool PendingSyncParameter { get; set; }
    public bool PendingUploadWorkParameter { get; set; }
    public int PendingAddPeopleCount { get; set; }
    public int PendingDeletePeopleCount { get; set; }
    public int RecordCount { get; set; }
}

public sealed class PersistedState
{
    public Dictionary<string, DeviceSnapshot> Devices { get; set; } = new(StringComparer.OrdinalIgnoreCase);
    public Dictionary<string, PersonInfo> People { get; set; } = new(StringComparer.OrdinalIgnoreCase);
    public List<string> DeletedUserIds { get; set; } = new();
    public Dictionary<string, DepartmentInfo> Departments { get; set; } = new(StringComparer.OrdinalIgnoreCase);
}

// 式式式 HTTP-Docking: Remote command 式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式

public sealed class RemoteCommandRequest
{
    public string SN { get; set; } = string.Empty;
}

public sealed class RemoteCommandResponse
{
    // Device expects Success=1 for successful response
    public int Success { get; set; } = 1;
    public string? Message { get; set; }
    public int? Restart { get; set; }
    public int? Recover { get; set; }
    public int? Opendoor { get; set; }
    public int? Closealarm { get; set; }
    public int? RepostRecord { get; set; }
    public int? PushAllPeople { get; set; }
    public List<uint>? QueryPeople { get; set; }
    public int? ClearRecord { get; set; }
}

// 式式式 HTTP-Docking: People push from device 式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式

public sealed class PushPeopleRequest
{
    public string SN { get; set; } = string.Empty;
    public List<PersonInfo> PeopleList { get; set; } = new();
}

// 式式式 HTTP-Docking: Download people list result 式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式

public sealed class DownloadPeopleListResultRequest
{
    public string SN { get; set; } = string.Empty;
    public int SuccessCount { get; set; }
    public int FailCount { get; set; }
    public List<PeopleImportFailItem> FailList { get; set; } = new();
}

public sealed class PeopleImportFailItem
{
    public string UserID { get; set; } = string.Empty;
    public int ErrorCode { get; set; }
    public string? RepeatID { get; set; }
    public string? ErrMsg { get; set; }
}

// 式式式 HTTP-Docking: Delete people list 式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式

public sealed class DeletePeopleListRequest
{
    public string SN { get; set; } = string.Empty;
    public int Limit { get; set; }
}

public sealed class DeletePeopleListResponse
{
    // Device expects Success=1 for successful response
    public int Success { get; set; } = 1;
    public string? Message { get; set; }
    public List<string> DeleteList { get; set; } = new();
}

public sealed class DeletePeopleListResultRequest
{
    public string SN { get; set; } = string.Empty;
    public int SuccessCount { get; set; }
    public int FailCount { get; set; }
}

// 式式式 HTTP-Docking: System record upload 式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式

public sealed class UploadSystemRecordRequest
{
    public string SN { get; set; } = string.Empty;
    public int RecordType { get; set; }
    public List<SystemRecordItem> Records { get; set; } = new();
}

public sealed class SystemRecordItem
{
    public long RecordID { get; set; }
    public int RecordType { get; set; }
    public long RecordDate { get; set; }
}

// 式式式 DeviceSnapshot: pending remote command 式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式

public sealed class PendingRemoteCommand
{
    public int? Restart { get; set; }
    public int? Recover { get; set; }
    public int? Opendoor { get; set; }
    public int? Closealarm { get; set; }
    public int? RepostRecord { get; set; }
    public int? PushAllPeople { get; set; }
    public List<uint>? QueryPeople { get; set; }
    public int? ClearRecord { get; set; }
}

// 式式式 Attendance Management 式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式

public sealed class AttendanceSearchRequest
{
    public int PageIndex { get; set; } = 1;
    public int PageSize { get; set; } = 20;
    public DateTime? StartTime { get; set; }
    public DateTime? EndTime { get; set; }
    public string? UserID { get; set; }
    public string? UserName { get; set; }
    public string? DepartmentID { get; set; }
}

public sealed class AttendanceRecord
{
    public string UserID { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
    public string DepartmentID { get; set; } = string.Empty;
    public string DepartmentName { get; set; } = string.Empty;
    public string RecordTime { get; set; } = string.Empty;
    public string DeviceSN { get; set; } = string.Empty;
    public int RecordType { get; set; }
    public string? Temperature { get; set; }
    public string? PhotoUrl { get; set; }
}

public sealed class AttendanceSearchResult
{
    public int TotalCount { get; set; }
    public int PageIndex { get; set; }
    public int PageSize { get; set; }
    public List<AttendanceRecord> DataList { get; set; } = new();
}

public sealed class AttendanceStatistics
{
    public int TotalRecords { get; set; }
    public int UniqueUsers { get; set; }
    public int UniqueDepartments { get; set; }
    public DateTime? StartTime { get; set; }
    public DateTime? EndTime { get; set; }
}

// 式式式 Network Interface 式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式

public sealed class NetworkInterfaceInfo
{
    public string LocalIp { get; set; } = string.Empty;
    public string BroadcastIp { get; set; } = string.Empty;
}
