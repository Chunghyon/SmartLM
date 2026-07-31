using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaceWebServer.DTO.MQTT_Protocol.Command.Server
{
    /// <summary>
    /// 服务器反馈接收到设备推送的人员  由服务器发送
    /// </summary>
    public class MQTT_Command_UploadPeopleACK : MQTTCommandPacket
    {
        public MQTT_Command_UploadPeopleACK(string cmdID)
        {

            Cmd = MQTT_Command_Define.UploadPeopleACK;
            SetToken(cmdID);
        }

    }
}
