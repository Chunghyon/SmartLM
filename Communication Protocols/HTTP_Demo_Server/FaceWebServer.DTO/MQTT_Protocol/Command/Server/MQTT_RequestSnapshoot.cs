using FaceWebServer.DTO.HTTPv2_Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaceWebServer.DTO.MQTT_Protocol.Command.Server
{
    /// <summary>
    ///  MQTT  服务器发送获取设备摄像头快照请求  由服务器发送
    /// </summary>
    public class MQTT_Command_RequestSnapshoot : MQTTCommandPacket
    {

        public MQTT_Command_RequestSnapshoot()
        {
            Cmd = MQTT_Command_Define.RequestSnapshoot;
            CreateToken();
        }
    }


}
