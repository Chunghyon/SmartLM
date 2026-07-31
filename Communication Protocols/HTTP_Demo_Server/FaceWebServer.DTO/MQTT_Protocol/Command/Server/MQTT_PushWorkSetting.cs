using FaceWebServer.DTO.HTTPv2_Protocol;
using FaceWebServer.DTO.MQTT_Protocol.Command.Device;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaceWebServer.DTO.MQTT_Protocol.Command.Server
{
    /// <summary>
    /// MQTT 服务器主动推送工作参数 ，由服务器发送
    /// </summary>
    public class MQTT_Command_PushWorkSetting : MQTTCommandPacket<MQTT_PushWorkSetting>
    {

        public MQTT_Command_PushWorkSetting(MQTT_PushWorkSetting data)
        {
            Cmd = MQTT_Command_Define.PushWorkSetting;
            Body = data;
            CreateToken();
        }
    }

    /// <summary>
    /// 服务器推送的工作参数
    /// </summary>
    public class MQTT_PushWorkSetting: HTTPDeviceParameterCoreV2
    {

    }
}
