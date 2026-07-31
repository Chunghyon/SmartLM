using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaceWebServer.DTO.MQTT_Protocol.Command.Server
{
    /// <summary>
    /// MQTT命令 服务器反馈接收到设备推送的系统记录  由服务器发送
    /// </summary>
    public class MQTT_Command_UploadSystemRecordACK : MQTTCommandPacket
    {
        public MQTT_Command_UploadSystemRecordACK(string cmdID)
        {

            SetToken(cmdID);
            Cmd = MQTT_Command_Define.UploadSystemRecordACK;
        }

    }
}
