using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaceWebServer.DTO.HTTPv1_Protocol
{
    /// <summary>
    /// HTTPv1 协议保活包数据结构  /device/updateStateDevice
    /// </summary>
    public class HTTPv1DeviceKeepaliveRequest: HTTPDeviceRequestV1
    {
        /// <summary>
        /// 请求服务器时间间隔
        /// </summary>
        public int OnlineTimeRequest { get; set; }

        /// <summary>
        /// 设备报警状态   
        /// 0--无报警；1--消防报警；2--门磁报警；3--开门超时报警；4--黑名单报警
        /// 5--防拆报警;6--非法验证;7--胁迫报警密码
        /// </summary>
        public int Alarm { get; set; }

        /// <summary>
        ///  0--无门磁；1--门开；2--门关
        /// </summary>
        public int Door { get; set; }
    }
}
