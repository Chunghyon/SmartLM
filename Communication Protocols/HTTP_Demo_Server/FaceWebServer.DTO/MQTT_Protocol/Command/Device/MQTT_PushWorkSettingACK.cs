using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaceWebServer.DTO.MQTT_Protocol.Command.Device
{
    /// <summary>
    /// 设备确认收到工作参数  由设备发送
    /// </summary>
    public class MQTT_Command_PushWorkSettingACK : MQTTCommandPacket
    {
        public MQTT_Command_PushWorkSettingACK()
        {

            Cmd = MQTT_Command_Define.PushWorkSettingACK;
        }

    }
}
