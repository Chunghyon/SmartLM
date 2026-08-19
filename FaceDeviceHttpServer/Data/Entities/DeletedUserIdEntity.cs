using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FaceDeviceHttpPcServer.Data.Entities;

[Table("deleted_user_ids")]
public class DeletedUserIdEntity
{
    [Key]
    [Column("user_id")]
    [MaxLength(64)]
    public string UserId { get; set; } = string.Empty;

    [Column("deleted_at")]
    public DateTime DeletedAt { get; set; } = DateTime.UtcNow;
}
