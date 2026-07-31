using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaceWebServer.DTO.Device
{
    /// <summary>
    /// 设置设备默认值的请求参数
    /// </summary>
    public class SetDefaultValueRequestDTO
    {
        /// <summary>
        /// 协议类型   HTTPv1   HTTPv2  MQTT  Websocket
        /// </summary>
        public string Protocol { get; set; }

        /// <summary>
        /// 默认参数的Json字符串
        /// </summary>
        public string DefaultJson { get; set; }
    }
}
