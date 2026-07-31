using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaceWebServer.DTO.MQTT_Protocol.Command.Server
{
    /// <summary>
    /// MQTT命令 服务器反馈接收到设备推送的打卡记录  由服务器发送
    /// </summary>
    public class MQTT_Command_UploadIdentifyRecordACK : MQTTCommandPacket
    {
        public MQTT_Command_UploadIdentifyRecordACK(string cmdID)
        {
            SetToken(cmdID);
            Cmd = MQTT_Command_Define.UploadIdentifyRecordACK;
        }

    }
}
