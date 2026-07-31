using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaceWebServer.DTO.MQTT_Protocol.Command.Device
{
    /// <summary>
    /// MQTT 设备心跳保活包 ，由设备定时发送
    /// </summary>
    public class MQTT_Command_KeepAlive : MQTTCommandPacket<MQTT_KeepAlive>
    {
        public MQTT_Command_KeepAlive()
        {
            Cmd = MQTT_Command_Define.KeepAlive;

        }

        public MQTT_Command_KeepAlive(MQTT_KeepAlive data)
        {
            Cmd = MQTT_Command_Define.KeepAlive;
            Body = data;

        }
    }

    /// <summary>
    /// MQTT 设备心跳保活包数据
    /// </summary>
    public class MQTT_KeepAlive
    {
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

        /// <summary>
        /// 请求鉴权，服务器需要响应鉴权结果，以方便设备决策是否发送人员、打卡记录等数据
        /// </summary>
        public int? RequestAuthentication { get; set; }
    }


}
