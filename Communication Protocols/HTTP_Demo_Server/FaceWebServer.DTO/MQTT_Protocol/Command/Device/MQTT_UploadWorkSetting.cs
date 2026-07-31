using FaceWebServer.DTO.HTTPv2_Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaceWebServer.DTO.MQTT_Protocol.Command.Device
{
    /// <summary>
    /// MQTT 设备主动上传工作参数 ，由设备发送
    /// </summary>
    public class MQTT_Command_UploadWorkSetting : MQTTCommandPacket<MQTT_UploadWorkSetting>
    {
        public MQTT_Command_UploadWorkSetting()
        {
            Cmd = MQTT_Command_Define.UploadWorkSetting;

        }

        public MQTT_Command_UploadWorkSetting(MQTT_UploadWorkSetting data)
        {
            Cmd = MQTT_Command_Define.UploadWorkSetting;
            Body = data;

        }
    }

    /// <summary>
    /// 设备主动工作参数
    /// </summary>
    public class MQTT_UploadWorkSetting: HTTPDeviceUploadWorkSettingRequest
    {
    }
}
