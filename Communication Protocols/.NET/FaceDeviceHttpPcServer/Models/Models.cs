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
    public int RecordCount { get; set; }
}

public sealed class PersistedState
{
    public Dictionary<string, DeviceSnapshot> Devices { get; set; } = new(StringComparer.OrdinalIgnoreCase);
}
