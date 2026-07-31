using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaceWebServer.DTO.Device
{
    /// <summary>
    /// 设备列表查询返回值
    /// </summary>
    public class DeviceQueryResultDTO 
    {
        /// <summary>
        /// 主键
        /// </summary>
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
        public string DeviceName { get; set; }

        /// <summary>
        /// 设备版本号
        /// </summary>
        public string DeviceVer { get; set; }

        /// <summary>
        /// 是否为进门设备 1--是；0--否
        /// </summary>
        public int IsEntry { get; set; }

        /// <summary>
        /// 最近保活包时间
        /// </summary>
        public DateTime LastKeepaliveTime { get; set; }

        /// <summary>
        /// 保活包状态信息 根据不同协议对象值不同
        /// </summary>
        public object KeepaliveStatus { get; set; }
    }
}
