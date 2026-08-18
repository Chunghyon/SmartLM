using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FaceDeviceHttpPcServer.Data.Entities;

[Table("devices")]
public class DeviceEntity
{
    [Key]
    [Column("sn")]
    [MaxLength(64)]
    public string Sn { get; set; } = string.Empty;

    [Column("ip_address")]
    [MaxLength(45)]
    public string? IpAddress { get; set; }

    [Column("http_port")]
    public int HttpPort { get; set; } = 80;

    [Column("device_name")]
    [MaxLength(100)]
    public string? DeviceName { get; set; }

    [Column("tag_name")]
    [MaxLength(100)]
    public string? TagName { get; set; }

    [Column("model")]
    [MaxLength(50)]
    public string? Model { get; set; }

    [Column("firmware_version")]
    [MaxLength(50)]
    public string? FirmwareVersion { get; set; }

    [Column("unit_no")]
    public int UnitNo { get; set; }

    [Column("connected_at")]
    public DateTime? ConnectedAt { get; set; }

    [Column("last_keepalive_at")]
    public DateTime? LastKeepaliveAt { get; set; }

    [Column("last_work_setting_upload_at")]
    public DateTime? LastWorkSettingUploadAt { get; set; }

    /// <summary>Last KeepaliveRequest as JSON</summary>
    [Column("last_keepalive_json")]
    public string? LastKeepaliveJson { get; set; }

    [Column("last_uploaded_work_setting")]
    public string? LastUploadedWorkSetting { get; set; }

    [Column("desired_work_setting")]
    public string? DesiredWorkSetting { get; set; }

    [Column("pending_sync_parameter")]
    public bool PendingSyncParameter { get; set; }

    [Column("pending_upload_work_parameter")]
    public bool PendingUploadWorkParameter { get; set; }

    [Column("pending_add_people_count")]
    public int PendingAddPeopleCount { get; set; }

    /// <summary>PendingRemoteCommand as JSON</summary>
    [Column("pending_remote_json")]
    public string? PendingRemoteJson { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [Column("updated_at")]
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    // Navigation
    public ICollection<DevicePersonEntity> DevicePeople { get; set; } = new List<DevicePersonEntity>();
    public ICollection<PendingDeleteEntity> PendingDeletes { get; set; } = new List<PendingDeleteEntity>();
}
