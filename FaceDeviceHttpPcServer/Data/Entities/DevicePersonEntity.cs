using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FaceDeviceHttpPcServer.Data.Entities;

[Table("device_people")]
public class DevicePersonEntity
{
    [Column("device_sn")]
    [MaxLength(64)]
    public string DeviceSn { get; set; } = string.Empty;

    [Column("user_id")]
    [MaxLength(64)]
    public string UserId { get; set; } = string.Empty;

    /// <summary>Already sent to this device</summary>
    [Column("downloaded")]
    public bool Downloaded { get; set; }

    /// <summary>Staged for this device only (StagedPeople)</summary>
    [Column("staged")]
    public bool Staged { get; set; }

    /// <summary>Currently registered on the device (OwnedPeople)</summary>
    [Column("owned")]
    public bool Owned { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [Column("updated_at")]
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    // Navigation
    public DeviceEntity? Device { get; set; }
    public PersonEntity? Person { get; set; }
}
