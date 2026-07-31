using FaceWebServer.DTO.HTTPv2_Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace FaceWebServer.DTO.MQTT_Protocol.Command.Device
{
    /// <summary>
    /// MQTT  设备发送请求服务器鉴权  由设备发送 
    /// </summary>
    public class MQTT_Command_RequestAuthorization : MQTT_Command_UploadIdentifyRecord
    {
        public MQTT_Command_RequestAuthorization() : base()
        {
            Cmd = MQTT_Command_Define.RequestAuthorization;

        }
        public MQTT_Command_RequestAuthorization(MQTT_UploadIdentifyRecord data) : base(data)
        {
            Cmd = MQTT_Command_Define.RequestAuthorization;
            Body = data;

        }

    }
}
