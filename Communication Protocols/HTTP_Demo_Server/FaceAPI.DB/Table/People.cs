using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaceWebServer.DB.Table
{
    [Table("People")]
    public class People
    {
        /// <summary>
        /// 主键
        /// </summary>
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        /// <summary>
        /// 用户号 （数字 最大值 4294967295 类型 UINT32）
        /// </summary>
        public long UserID { get; set; } //是
        /// <summary>
        /// 人员姓名（字符<32位）
        /// </summary>
        public string Name { get; set; } //是
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
        /// 其他信息
        /// </summary>
        public string? Attachment { get; set; }
        /// <summary>
        /// 照片
        /// </summary>
        public string? Photo { get; set; }

        /// <summary>
        /// 图片文件的MD5
        /// </summary>
        public string? PhotoMD5 { get; set; }

        /// <summary>
        /// 图片文件大小 字节
        /// </summary>
        public int PhotoLen { get; set; }

        /// <summary>
        /// 密码，纯数字,长度：（0 / 4-8）
        /// </summary>
        public string? Password { get; set; }


        /// <summary>
        /// IC卡号 纯数字
        /// </summary>
        public ulong? CardNum { get; set; }


        /// <summary>
        /// 二维码
        /// </summary>
        public string? QRCode { get; set; }

        /// <summary>
        /// 人脸特征码的 json字符串  FeatureCode 结构
        /// </summary>
        public string? FaceFeature { get; set; }


        /// <summary>
        /// 指纹特征码的Json字符串 FeatureCode数组 结构
        /// </summary>
        public string? Fingerprints { get; set; }

        /// <summary>
        /// 掌静脉特征码的Json字符串 FeatureCode数组 结构
        /// </summary>
        public string? Palmveins { get; set; }


        /// <summary>
        /// 人脸数量
        /// </summary>
        public int FaceNum { get; set; }

        /// <summary>
        /// 掌静脉数量
        /// </summary>
        public int PalmveinsNum { get; set; }


        /// <summary>
        /// 指纹数量
        /// </summary>
        public int FingerprintsNum { get; set; }


        /// <summary>
        /// 创建时间 
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 最后更新时间 
        /// </summary>
        public DateTime LastUpdatetime { get; set; }
        public People()
        {
            CreateTime = DateTime.Now;
            LastUpdatetime = DateTime.Now;
        }

    }


}
