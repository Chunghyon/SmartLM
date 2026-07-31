using FaceWebServer.DTO.MQTT_Protocol.Command.Device;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace FaceWebServer.DTO.MQTT_Protocol
{
    /// <summary>
    /// MQTT命令工厂 ，用来根据命令名称创建对应的命令对象
    /// </summary>
    public static class MQTTCommandFactory
    {

        public static Dictionary<string,Type> MqttCommandMap ;

        static MQTTCommandFactory()
        {
            MqttCommandMap = new Dictionary<string, Type>();
            MqttCommandMap[MQTT_Command_Define.KeepAlive] = typeof(MQTT_Command_KeepAlive);
            MqttCommandMap[MQTT_Command_Define.Offline] = typeof(MQTT_Command_Offline);
            MqttCommandMap[MQTT_Command_Define.UploadWorkSetting] = typeof(MQTT_Command_UploadWorkSetting);
            MqttCommandMap[MQTT_Command_Define.PushWorkSettingACK] = typeof(MQTT_Command_PushWorkSettingACK);
            MqttCommandMap[MQTT_Command_Define.RemoteCommandACK] = typeof(MQTT_Command_RemoteCommandACK);
            MqttCommandMap[MQTT_Command_Define.PushPeopleACK] = typeof(MQTT_Command_PushPeopleACK);
            MqttCommandMap[MQTT_Command_Define.PushDeletePeopleACK] = typeof(MQTT_Command_PushDeletePeopleACK);
            MqttCommandMap[MQTT_Command_Define.UploadPeople] = typeof(MQTT_Command_UploadPeople);
            MqttCommandMap[MQTT_Command_Define.UploadIdentifyRecord] = typeof(MQTT_Command_UploadIdentifyRecord);
            MqttCommandMap[MQTT_Command_Define.UploadSystemRecord] = typeof(MQTT_Command_UploadSystemRecord);
            MqttCommandMap[MQTT_Command_Define.PushSoftwareACK] = typeof(MQTT_Command_PushSoftwareACK);
            MqttCommandMap[MQTT_Command_Define.PushSystemFileACK] = typeof(MQTT_Command_PushSystemFileACK);
            MqttCommandMap[MQTT_Command_Define.RegisterIdentifyTicketACK] = typeof(MQTT_Command_RegisterIdentifyTicketACK);
            MqttCommandMap[MQTT_Command_Define.RequestAuthorization] = typeof(MQTT_Command_RequestAuthorization);
            MqttCommandMap[MQTT_Command_Define.RequestSnapshootACK] = typeof(MQTT_Command_RequestSnapshootACK);
        }

        public static MQTTCommandPacket CreateCommand(string cmd, JObject cmdObj, ArraySegment<byte> fileDataBuf)
        {
            if(MqttCommandMap.ContainsKey(cmd))
            {
                var obj = (MQTTCommandPacket)cmdObj.ToObject(MqttCommandMap[cmd]);
                if(fileDataBuf != null)
                {
                    obj.SetDataBuf(fileDataBuf);
                }

                return obj;
            }
            return null;
        }
    }
}
