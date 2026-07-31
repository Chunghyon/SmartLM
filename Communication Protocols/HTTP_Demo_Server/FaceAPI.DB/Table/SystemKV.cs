using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaceWebServer.DB.Table
{
    [Table("SystemKV")]
    public class SystemKV
    {
        /// <summary>
        /// 主键
        /// </summary>
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Key { get; set; }

        /// <summary>
        /// 值
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime LastUpdateTime { get; set; }


        public SystemKV()
        {
            CreateTime = DateTime.Now;
            LastUpdateTime = DateTime.Now;
        }
    }
}
