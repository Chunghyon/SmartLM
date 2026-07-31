using FaceWebServer.DTO.HTTPv2_Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaceWebServer.DTO.MQTT_Protocol.Command.Server
{
    /// <summary>
    ///  MQTT  服务器发送告知设备鉴权结果  由服务器发送
    /// </summary>
    public class MQTT_Command_DeviceAuthentication : MQTTCommandPacket<MQTT_DeviceAuthentication>
    {

        public MQTT_Command_DeviceAuthentication(MQTT_DeviceAuthentication data)
        {
            Cmd = MQTT_Command_Define.DeviceAuthentication;
            Body = data;
            CreateToken();
        }
    }

    /// <summary>
    /// 设备鉴权结果
    /// </summary>
    public class MQTT_DeviceAuthentication
    {
        /// <summary>
        /// 鉴权结果 
        /// 0 表示鉴权失败，设备不允许发送人员、照片、打卡记录到服务器
        /// 1 表示鉴权成功，设备可以正常上传人员、照片，打卡记录到的服务器、
        /// </summary>
        public int? Authentication { get; set; }

        /// <summary>
        /// 服务器端的错误代码，0表示无错误，非0表示有错误，具体错误代码由服务器定义
        /// </summary>
        public int? code { get; set; }


        /// <summary>
        /// 鉴权失败的描述
        /// </summary>
        public string? ErrorMessage { get; set; }
    }

}
