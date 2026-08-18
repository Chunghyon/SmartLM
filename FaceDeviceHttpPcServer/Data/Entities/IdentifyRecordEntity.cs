using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FaceDeviceHttpPcServer.Data.Entities;

[Table("identify_records")]
public class IdentifyRecordEntity
{
    [Key]
    [Column("id")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id { get; set; }

    [Column("device_sn")]
    [MaxLength(64)]
    public string DeviceSn { get; set; } = string.Empty;

    [Column("record_id")]
    [MaxLength(64)]
    public string? RecordId { get; set; }

    [Column("user_id")]
    [MaxLength(64)]
    public string? UserId { get; set; }

    [Column("user_name")]
    [MaxLength(100)]
    public string? UserName { get; set; }

    [Column("record_type")]
    public int? RecordType { get; set; }

    [Column("record_time")]
    public DateTime? RecordTime { get; set; }

    [Column("temperature")]
    [MaxLength(20)]
    public string? Temperature { get; set; }

    [Column("photo_path")]
    [MaxLength(500)]
    public string? PhotoPath { get; set; }

    [Column("raw_json")]
    public string? RawJson { get; set; }

    [Column("received_at")]
    public DateTime ReceivedAt { get; set; } = DateTime.UtcNow;

    public DeviceEntity? Device { get; set; }
}
