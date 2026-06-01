using System.Text.Json.Nodes;

namespace FaceDeviceHttpPcServer.Models;

public record ApiResponse(int Success, string? Message = null)
{
    public static ApiResponse Ok(string? message = null) => new(0, message);
}

public sealed class KeepaliveRequest
{
    public string SN { get; set; } = string.Empty;
    public int RelayStatus { get; set; }
    public int KeepOpenStatus { get; set; }
    public int DoorSensorStatus { get; set; }
    public int LockDoorStatus { get; set; }
    public string AlarmStatus { get; set; } = string.Empty;
}

public sealed record KeepaliveResponse : ApiResponse
{
    public KeepaliveResponse() : base(0)
    {
    }

    public int? AddPeople { get; set; }
    public int? DeletePeople { get; set; }
    public int? SyncParameter { get; set; }
    public int? Remote { get; set; }
    public int? UploadWorkParameter { get; set; }
}

public sealed class PersonInfo
{
    public string UserID { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Job { get; set; } = string.Empty;
    public string Department { get; set; } = string.Empty;
    public string IdentityCard { get; set; } = string.Empty;
    public string Attachment { get; set; } = string.Empty;
    public string Photo { get; set; } = string.Empty;
    public string PhotoMD5 { get; set; } = string.Empty;
    public int PhotoLen { get; set; }
    public string Password { get; set; } = string.Empty;
    public string CardNum { get; set; } = string.Empty;
    public string QRCode { get; set; } = string.Empty;
    public int AccessType { get; set; }
    public uint ExpirationDate { get; set; }
    public int OpenTimes { get; set; } = 65535;
    public int KeepOpen { get; set; }
    public int Timegroup { get; set; }
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

public sealed record DownloadPeopleListResponse : ApiResponse
{
    public DownloadPeopleListResponse() : base(0)
    {
    }

    public int PeopleCount { get; set; }
    public List<PersonInfo> PeopleList { get; set; } = new();
}

public sealed class SelectDeleteInfoRequest
{
    public string SN { get; set; } = string.Empty;
}

public sealed record SelectDeleteInfoResponse : ApiResponse
{
    public SelectDeleteInfoResponse() : base(0)
    {
    }

    public List<string> DeleteList { get; set; } = new();
}

public sealed class DeviceSnapshot
{
    public string SN { get; set; } = string.Empty;
    public KeepaliveRequest? LastKeepalive { get; set; }
    public DateTimeOffset? LastKeepaliveAtUtc { get; set; }
    public DateTimeOffset? LastWorkSettingUploadAtUtc { get; set; }
    public JsonObject? LastUploadedWorkSetting { get; set; }
    public JsonObject? DesiredWorkSetting { get; set; }
    public bool PendingSyncParameter { get; set; }
    public bool PendingUploadWorkParameter { get; set; }
    public int PendingAddPeopleCount { get; set; }
    public List<string> PendingDeleteUserIds { get; set; } = new();
    public List<RecordSnapshot> Records { get; set; } = new();
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
}
