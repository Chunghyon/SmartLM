using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaceWebServer.DTO.HTTPv1_Protocol
{
    /// <summary>
    /// HTTPv1 协议设备请求包结构基类
    /// </summary>
    public class HTTPDeviceRequestV1
    {
        /// <summary>
        /// 设备ID
        /// </summary>
        public string DeviceID { get; set; }
    }
}
