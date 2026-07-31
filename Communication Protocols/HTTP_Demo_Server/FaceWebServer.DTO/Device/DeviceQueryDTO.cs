using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaceWebServer.DTO.Device
{
    /// <summary>
    /// 查询设备列表的请求参数
    /// </summary>
    public class DeviceQueryDTO : BasePageParameter
    {
        /// <summary>
        /// 主键
        /// </summary>
        public int? ID { get; set; }

        /// <summary>
        /// 设备 SN
        /// </summary>
        public string? SN { get; set; }

        /// <summary>
        /// 协议类型  HTTPv1   HTTPv2  MQTT  Websocket
        /// </summary>
        public string? Protocol { get; set; }

        /// <summary>
        /// 设备名称
        /// </summary>
        public string? DeviceName { get; set; }

        /// <summary>
        /// 是否为进门设备 1--是；0--否
        /// </summary>
        public int? IsEntry { get; set; }

        /// <summary>
        /// 设备是否在线 1--是；0--否
        /// </summary>
        public int? IsOnline { get; set; }
    }
}
