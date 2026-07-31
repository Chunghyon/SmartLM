using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaceWebServer.DTO.MQTT_Protocol.Command.Server
{
    /// <summary>
    /// MQTT命令 服务器要求设备上传工作参数  由服务器发送
    /// </summary>
    public class MQTT_Command_ReadWorkSetting : MQTTCommandPacket
    {
        public MQTT_Command_ReadWorkSetting()
        {

            Cmd = MQTT_Command_Define.ReadWorkSetting;
            CreateToken();
        }

    }
}
