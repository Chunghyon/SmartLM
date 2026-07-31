using FaceWebServer.DTO.HTTPv2_Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaceWebServer.DTO.MQTT_Protocol.Command.Device
{
    /// <summary>
    /// 设备反馈人员存储结果  由设备发送
    /// </summary>
    public class MQTT_Command_PushPeopleACK : MQTTCommandPacket<MQTT_PushPeopleACK>
    {
        public MQTT_Command_PushPeopleACK()
        {

            Cmd = MQTT_Command_Define.PushPeopleACK;
        }

    }

    public class MQTT_PushPeopleACK :HTTPPeopleDownloadPeopleListResultRequest
    {

    }
}
