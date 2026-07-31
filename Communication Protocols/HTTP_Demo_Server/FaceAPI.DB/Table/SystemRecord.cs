using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaceWebServer.DB.Table
{
    /// <summary>
    /// 系统记录表
    /// </summary>
    [Table("SystemRecord")]
    public class SystemRecord
    {
        /// <summary>
        /// 主键
        /// </summary>
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        /// <summary>
        /// 记录序号
        /// </summary>
        public int RecordID { get; set; }

        /// <summary>
        /// 记录的MD5 签名，
        /// 签名算法是  SN+RecordDate+RecordID 目的是为了防止重复
        /// </summary>
        public string RecordMD5 { get; set; }

        /// <summary>
        /// 事件类型   1-1000 表示门磁记录 ；  1001 - 2000 表示系统记录；  
        /// </summary>
        public int RecordType { get; set; }

        /// <summary>
        /// 记录时间
        /// </summary>
        public DateTime RecordDate { get; set; }

        /// <summary>
        /// 设备SN
        /// </summary>
        public string SN { get; set; }

        /// <summary>
        /// 插入时间（本地字段）
        /// </summary>
        public DateTime InsertTime { get; set; }


        public SystemRecord()
        {
            InsertTime = DateTime.Now;
        }
    }
}
