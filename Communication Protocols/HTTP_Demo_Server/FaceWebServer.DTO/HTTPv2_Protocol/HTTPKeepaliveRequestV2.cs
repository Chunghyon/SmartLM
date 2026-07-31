using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaceWebServer.DTO.HTTPv2_Protocol
{
    /// <summary>
    /// HTTPv2 协议 设备保活包请求 /Device/Keepalive
    /// </summary>
    public class HTTPKeepaliveRequestV2
    {
        /// <summary>
        /// 设备SN
        /// </summary>
        public string SN { get; set; }


        /// <summary>
        /// 继电器物状态  0--表示COM和NC常闭  1--表示COM和NO常闭
        /// </summary>
        public int RelayStatus { get; set; }


        /// <summary>
        /// 常开状态  0--表示常闭   1--表示常开
        /// </summary>
        public int KeepOpenStatus { get; set; }

        /// <summary>
        /// 门磁状态  0--表示关  1--表示开
        /// </summary>
        public int DoorSensorStatus { get; set; }

        /// <summary>
        /// 门锁定状态  0--表示未锁定  1--表示已锁定
        /// </summary>
        public int LockDoorStatus { get; set; }

        /// <summary>
        /// 门报警状态  空字符串为无报警，否则会有具体报警名称
        /// <para>fire--消防报警      blacklist--黑名单报警    anti--防拆报警</para>
        /// <para>illegal--非法验证   password--胁迫报警密码   openTimeout--开门超时报警</para>
        /// <para>doorSensor--门磁报警</para>
        /// <para>有多个报警时，使用逗号分隔 fire,blacklist</para>
        /// </summary>
        public string? AlarmStatus { get; set; }
    }
}
