using FaceWebServer.DTO.HTTPv2_Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaceWebServer.DTO.MQTT_Protocol.Command.Device
{
    /// <summary>
    /// MQTT 设备反馈已收到系统文件更新通知  由设备发送 
    /// </summary>
    public class MQTT_Command_PushSystemFileACK : MQTTCommandPacket
    {
        public MQTT_Command_PushSystemFileACK()
        {
            Cmd = MQTT_Command_Define.PushSystemFileACK;

        }

    }
}
