using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FaceDeviceHttpPcServer.Data.Entities;

[Table("pending_deletes")]
public class PendingDeleteEntity
{
    [Column("device_sn")]
    [MaxLength(64)]
    public string DeviceSn { get; set; } = string.Empty;

    [Column("user_id")]
    [MaxLength(64)]
    public string UserId { get; set; } = string.Empty;

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DeviceEntity? Device { get; set; }
}
