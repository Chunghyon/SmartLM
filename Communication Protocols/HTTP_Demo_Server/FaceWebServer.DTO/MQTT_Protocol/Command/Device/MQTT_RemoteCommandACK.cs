using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaceWebServer.DTO.MQTT_Protocol.Command.Device
{

    /// <summary>
    /// 设备确认收到远程指令  由设备发送
    /// </summary>
    public class MQTT_Command_RemoteCommandACK : MQTTCommandPacket
    {
        public MQTT_Command_RemoteCommandACK()
        {

            Cmd = MQTT_Command_Define.RemoteCommandACK;
        }

    }
}
