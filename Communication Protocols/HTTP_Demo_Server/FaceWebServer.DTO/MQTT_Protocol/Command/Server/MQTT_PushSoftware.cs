using FaceWebServer.DTO.HTTPv2_Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaceWebServer.DTO.MQTT_Protocol.Command.Server
{
    /// <summary>
    /// MQTT  服务器发送固件升级通知  由服务器发送
    /// </summary>
    public class MQTT_Command_PushSoftware : MQTTCommandPacket<PushSoftwareDTO>
    {

        public MQTT_Command_PushSoftware(PushSoftwareDTO data)
        {
            Cmd = MQTT_Command_Define.PushSoftware;
            Body = data;
            CreateToken();
        }
    }


}
