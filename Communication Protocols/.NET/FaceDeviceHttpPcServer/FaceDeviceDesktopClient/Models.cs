using System.Text.Json.Serialization;

namespace FaceDeviceDesktopClient;

// API Response wrappers
public class BrowserApiResponse<T>
{
    [JsonPropertyName("Code")]
    public int Code { get; set; }

    [JsonPropertyName("Msg")]
    public string? Msg { get; set; }

    [JsonPropertyName("Data")]
    public T? Data { get; set; }
}

// Device models
public class DeviceInfo
{
    public string SN { get; set; } = string.Empty;
    public string? IpAddress { get; set; }
    public int? HttpPort { get; set; }
    public string? DeviceName { get; set; }
    public string? Model { get; set; }
    public string? FirmwareVersion { get; set; }
    public DateTimeOffset? LastKeepaliveAtUtc { get; set; }
    public DateTimeOffset? ConnectedAtUtc { get; set; }
    public int AddPendingCount { get; set; }
    public int DeletePendingCount { get; set; }
    public int RecordCount { get; set; }
}

public class DiscoveredDevice
{
    public string IpAddress { get; set; } = string.Empty;
    public string DeviceSN { get; set; } = string.Empty;
    public string? DeviceName { get; set; }
    public string? Model { get; set; }
    public string? FirmwareVersion { get; set; }
    public int HttpPort { get; set; } = 80;
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
    public string? PhotoUrl { get; set; }
}

// Attendance models
public class AttendanceSearchRequest
{
    public int PageIndex { get; set; } = 1;
    public int PageSize { get; set; } = 1000;
    public DateTime? StartTime { get; set; }
    public DateTime? EndTime { get; set; }
    public string? UserID { get; set; }
    public string? UserName { get; set; }
    public string? DepartmentID { get; set; }
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
