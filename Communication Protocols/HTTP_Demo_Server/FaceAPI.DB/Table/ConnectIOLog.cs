using FaceWebServer.DB.Log;
using Newtonsoft.Json;
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
    /// 网络连接日志
    /// </summary>
    [Table("ConnectIOLog")]
    public class ConnectIOLog
    {
        /// <summary>
        /// 日志ID
        /// </summary>
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int LogID { get; set; }

        /// <summary>
        /// 日志时间
        /// </summary>
        [JsonConverter(typeof(ChinaDateTimeMillisecondConverter))]
        public DateTime LogTime { get; set; }

        /// <summary>
        /// 请求的唯一ID
        /// </summary>
        public string RequestID { get; set; }

        /// <summary>
        /// 请求类型 Request or Response
        /// </summary>
        public string HttpType { get; set; }

        /// <summary>
        /// 请求的设备SN
        /// </summary>
        public string? SN { get; set; }

        /// <summary>
        /// IP地址信息
        /// </summary>
        public string IPAddr { get; set; }


        /// <summary>
        /// 协议类型  HTTPv1   HTTPv2  MQTT  Websocket 
        /// </summary>
        public string? Protocol { get; set; }


        /// <summary>
        /// API名称
        /// </summary>
        public string? APIName { get; set; }

        /// <summary>
        /// 请求的URL
        /// </summary>
        public string URL { get; set; }


        /// <summary>
        /// HTTP的 Method 方法
        /// </summary>
        public string Method { get; set; }


        /// <summary>
        /// HTTP的Body长度
        /// </summary>
        public int ContentLength { get; set; }

        /// <summary>
        /// HTTP的ContentType 内容类型
        /// </summary>
        public string ContentType { get; set; }


        /// <summary>
        /// HTTP的Body 有效数据内容
        /// </summary>
        public string Body { get; set; }

    }
}
