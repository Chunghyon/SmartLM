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
    /// 设备表
    /// </summary>
    [Table("DeviceDetail")]
    public class DeviceDetail
    {

        public const string HTTPv1 = "HTTPv1";
        public const string HTTPv2 = "HTTPv2";
        public const string MQTT = "MQTT";
        public const string Websocket = "Websocket";

        /// <summary>
        /// 主键
        /// </summary>
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        /// <summary>
        /// 设备 SN
        /// </summary>
        public string SN { get; set; }

        /// <summary>
        /// 协议类型  HTTPv1   HTTPv2  MQTT  Websocket
        /// </summary>
        public string Protocol { get; set; } 

        /// <summary>
        /// 设备名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 设备版本号
        /// </summary>
        public string DeviceVer { get; set; }

        /// <summary>
        /// 是否为进门设备 1--是；0--否
        /// </summary>
        public int IsEntry { get; set; }

        /// <summary>
        /// 设备属性详情  Json字符串
        /// </summary>
        public string Detail { get; set; }


        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; } 

        /// <summary>
        /// 最后更新时间
        /// </summary>
        public DateTime LastUpdatetime { get; set; }  // 是


        /// <summary>
        /// 同步状态
        /// 0--未同步；1--已同步
        /// </summary>
        public int UploadStatus { get; set; }

        /// <summary>
        /// 同步时间
        /// </summary>
        public DateTime UploadStatusTime { get; set; }


        public DeviceDetail()
        {
            CreateTime = DateTime.Now;
            LastUpdatetime = DateTime.Now;
            UploadStatusTime = DateTime.Now;
        }
    }
}
