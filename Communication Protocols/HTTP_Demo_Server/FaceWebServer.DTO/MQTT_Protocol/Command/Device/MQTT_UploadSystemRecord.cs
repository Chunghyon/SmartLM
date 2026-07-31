using FaceWebServer.DTO.HTTPv2_Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaceWebServer.DTO.MQTT_Protocol.Command.Device
{

    /// <summary>
    /// MQTT 设备上传系统记录 ，由设备发送
    /// </summary>
    public class MQTT_Command_UploadSystemRecord : MQTTCommandPacket<MQTT_UploadSystemRecord>
    {
        public MQTT_Command_UploadSystemRecord()
        {
            Cmd = MQTT_Command_Define.UploadSystemRecord;

        }

        public MQTT_Command_UploadSystemRecord(MQTT_UploadSystemRecord data)
        {
            Cmd = MQTT_Command_Define.UploadSystemRecord;
            Body = data;

        }
    }

    /// <summary>
    /// 设备主动推送的系统记录
    /// </summary>
    public class MQTT_UploadSystemRecord : HTTPRecordUploadSystemRecordRequest
    {
    }
}
