using FaceWebServer.DTO.HTTPv2_Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaceWebServer.DTO.MQTT_Protocol.Command.Server
{
    /// <summary>
    /// MQTT  服务器发送通知设备注册用户凭证  由服务器发送
    /// </summary>
    public class MQTT_Command_RegisterIdentifyTicket : MQTTCommandPacket<RegisterIdentifyTicketDTO>
    {

        public MQTT_Command_RegisterIdentifyTicket(RegisterIdentifyTicketDTO data)
        {
            Cmd = MQTT_Command_Define.RegisterIdentifyTicket;
            Body = data;
            CreateToken();
        }
    }
}
