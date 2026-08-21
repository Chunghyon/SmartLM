using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FaceDeviceHttpPcServer.Data.Entities;

[Table("people")]
public class PersonEntity
{
    [Key]
    [Column("user_id")]
    [MaxLength(64)]
    public string UserId { get; set; } = string.Empty;

    [Column("code")]
    [MaxLength(64)]
    public string? Code { get; set; }

    [Column("name")]
    [MaxLength(100)]
    public string? Name { get; set; }

    [Column("photo")]
    public string? Photo { get; set; }

    [Column("photo_md5")]
    [MaxLength(64)]
    public string? PhotoMd5 { get; set; }

    [Column("photo_len")]
    public int PhotoLen { get; set; }

    [Column("password")]
    [MaxLength(100)]
    public string? Password { get; set; }

    [Column("card_num")]
    [MaxLength(50)]
    public string CardNum { get; set; } = "0";

    [Column("qr_code")]
    [MaxLength(255)]
    public string? QrCode { get; set; }

    [Column("access_type")]
    public int AccessType { get; set; }

    [Column("expiration_date")]
    public uint ExpirationDate { get; set; }

    [Column("open_times")]
    public int OpenTimes { get; set; } = 65535;

    [Column("face_feature")]
    public string? FaceFeature { get; set; }

    [Column("face_feature_md5")]
    [MaxLength(64)]
    public string? FaceFeatureMd5 { get; set; }

    /// <summary>JSON array of FingerprintItem</summary>
    [Column("fingerprints_json")]
    public string? FingerprintsJson { get; set; }

    /// <summary>JSON array of PalmveinItem</summary>
    [Column("palmveins_json")]
    public string? PalmveinsJson { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [Column("updated_at")]
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
