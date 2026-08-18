using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FaceDeviceHttpPcServer.Data.Entities;

[Table("departments")]
public class DepartmentEntity
{
    [Key]
    [Column("department_id")]
    [MaxLength(64)]
    public string DepartmentId { get; set; } = string.Empty;

    [Column("name")]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;
}
