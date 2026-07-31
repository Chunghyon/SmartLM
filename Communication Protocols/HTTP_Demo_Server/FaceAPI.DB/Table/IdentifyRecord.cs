using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FaceWebServer.DB.Table
{
    /// <summary>
    /// 打卡记录
    /// </summary>
    [Table("IdentifyRecord")]
    public class IdentifyRecord
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
        /// 事件类型
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
        /// 用户号
        /// </summary>
        public long UserID { get; set; } //是
        /// <summary>
        /// 人员姓名（字符<32位）
        /// </summary>
        public string? Name { get; set; } //是
        /// <summary>
        /// 职务
        /// </summary>
        public string? Job { get; set; } //否
        /// <summary>
        /// 部门
        /// </summary>
        public string? Department { get; set; }

        /// <summary>
        /// 身份证
        /// </summary>
        public string? IdentityCard { get; set; }

        /// <summary>
        /// IC卡号 纯数字
        /// </summary>
        public ulong CardNum { get; set; }


        /// <summary>
        /// 二维码
        /// </summary>
        public string? QRCode { get; set; }

        /// <summary>
        /// 是否为进入，1表示进入，0表示出门
        /// </summary>
        public int IsEntry { get; set; }

        /// <summary>
        /// 人体测量温度 （摄氏度）
        /// </summary>
        public float BodyTemp { get; set; }


        /// <summary>
        /// 图片长度
        /// </summary>
        public int PhotoLen { get; set; }

        /// <summary>
        /// 图片地址
        /// </summary>
        public string? Photo { get; set; }


        /// <summary>
        /// 插入时间（本地字段）
        /// </summary>
        public DateTime InsertTime { get; set; }

        public IdentifyRecord()
        {
            InsertTime = DateTime.Now;
        }
    }
}
