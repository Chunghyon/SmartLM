using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaceWebServer.DTO.MQTT_Protocol.Command.Device
{
    /// <summary>
    /// MQTT 设备离线的遗嘱 ，由设备定时发送
    /// </summary>
    public class MQTT_Command_Offline : MQTTCommandPacket
    {
        public MQTT_Command_Offline()
        {
            Cmd = MQTT_Command_Define.Offline;

        }

    }
}
