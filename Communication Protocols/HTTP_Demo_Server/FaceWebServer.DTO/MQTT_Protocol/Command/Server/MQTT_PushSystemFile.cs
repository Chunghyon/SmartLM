using FaceWebServer.DTO.HTTPv2_Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaceWebServer.DTO.MQTT_Protocol.Command.Server
{
    /// <summary>
    /// MQTT  服务器发送系统文件更新通知  由服务器发送
    /// </summary>
    public class MQTT_Command_PushSystemFile : MQTTCommandPacket<List<PushSystemFileDTO>>
    {

        public MQTT_Command_PushSystemFile(List<PushSystemFileDTO> data)
        {
            Cmd = MQTT_Command_Define.PushSystemFile;
            Body = data;
            CreateToken();
        }
    }
}
