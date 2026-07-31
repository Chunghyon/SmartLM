using FaceWebServer.DTO.HTTPv2_Protocol;
using FaceWebServer.DTO.MQTT_Protocol.Command.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaceWebServer.DTO.MQTT_Protocol.Command.Device
{
    /// <summary>
    /// 设备反馈删除人员结果  由设备发送
    /// </summary>
    public class MQTT_Command_PushDeletePeopleACK : MQTTCommandPacket<MQTT_PushDeletePeopleACK>
    {
        public MQTT_Command_PushDeletePeopleACK()
        {

            Cmd = MQTT_Command_Define.PushDeletePeopleACK;
        }

    }

    public class MQTT_PushDeletePeopleACK : MQTT_PushDeletePeople
    {

    }
}
