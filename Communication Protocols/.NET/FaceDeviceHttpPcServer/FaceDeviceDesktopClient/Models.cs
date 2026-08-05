using System.Text.Json.Serialization;
using System.Text.Json.Serialization;

namespace FaceDeviceDesktopClient;

// API Response wrappers - matching server's BrowserApiResponse format
public class BrowserApiResponse<T>
{
    [JsonPropertyName("result")]
    public bool Result { get; set; }

    [JsonPropertyName("content")]
    public T? Content { get; set; }

    [JsonPropertyName("errCode")]
    public int ErrCode { get; set; }

    [JsonPropertyName("error")]
    public string? Error { get; set; }

    // Compatibility properties for easier access
    public int Code => Result ? 0 : ErrCode;
    public string? Msg => Error;
    public T? Data => Content;
}

// Device models
public class DeviceInfo
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

    // Display properties for DataGridView
    public string Status => LastKeepaliveAtUtc.HasValue 
        ? (DateTimeOffset.UtcNow - LastKeepaliveAtUtc.Value).TotalSeconds <= 45 ? "정상" : "통신불량"
        : "통신불량";
}

public class DiscoveredDevice
{
    public string IpAddress { get; set; } = string.Empty;
    public string DeviceSN { get; set; } = string.Empty;
    public string? DeviceName { get; set; }
    public string? Model { get; set; }
    public string? FirmwareVersion { get; set; }
}

public class DeviceProbeInfo
{
    public string DeviceSN { get; set; } = string.Empty;
    public string? DeviceName { get; set; }
    public string? Model { get; set; }
    public string? FirmwareVersion { get; set; }
}

// Department model
public class DepartmentInfo
{
    public string DepartmentID { get; set; } = string.Empty;
    public string DepartmentName { get; set; } = string.Empty;
}

// Person model
public class PersonInfo
{
    public string UserID { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string? DepartmentID { get; set; }
    public string? DepartmentName { get; set; }
    public string? Job { get; set; }
    public string? CardNum { get; set; }
    public string? Password { get; set; }
    public int AccessType { get; set; }

    // 유효기간 (Unix timestamp, 0=무제한)
    public uint ExpirationDate { get; set; }

    // Backend uses Photo field, client uses PhotoUrl for display
    public string? Photo { get; set; }

    public string? PhotoUrl
    {
        get => Photo; // Map Photo to PhotoUrl for backward compatibility
        set => Photo = value;
    }

    // Photo binary data (Base64 encoded for JSON transmission)
    [System.ComponentModel.Browsable(false)]
    public byte[]? PhotoData { get; set; }

    // 지문 / 정맥 (서버 PersonInfo와 동일 구조)
    public List<FingerprintItem> Fingerprints { get; set; } = new();
    public List<PalmveinItem>    Palmveins     { get; set; } = new();
    public string? FaceFeature { get; set; }
}

public class FingerprintItem
{
    public int    Num  { get; set; }
    public string Data { get; set; } = string.Empty;
    public string MD5  { get; set; } = string.Empty;
}

public class PalmveinItem
{
    public int    Num  { get; set; }
    public string Data { get; set; } = string.Empty;
    public string MD5  { get; set; } = string.Empty;
}

// Attendance models
public class AttendanceSearchRequest
{
    public int PageIndex { get; set; } = 1;
    public int PageSize { get; set; } = 1000;
    public DateTime? StartTime { get; set; }
    public DateTime? EndTime { get; set; }
    public string? UserID { get; set; }
    public long? UserIDMin { get; set; }
    public long? UserIDMax { get; set; }
    public string? UserName { get; set; }
    public string? DepartmentID { get; set; }
    public string? DeviceSN { get; set; }
}

public class AttendanceRecord
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

public class AttendanceSearchResult
{
    public int TotalCount { get; set; }
    public int PageIndex { get; set; }
    public int PageSize { get; set; }
    public List<AttendanceRecord> DataList { get; set; } = new();
}

public class AttendanceStatistics
{
    public int TotalRecords { get; set; }
    public int UniqueUsers { get; set; }
    public int UniqueDepartments { get; set; }
    public DateTime? StartTime { get; set; }
    public DateTime? EndTime { get; set; }
}

// Network Interface model
public class NetworkInterfaceInfo
{
    public string LocalIp { get; set; } = string.Empty;
    public string BroadcastIp { get; set; } = string.Empty;
}
