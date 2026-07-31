using FaceWebServer.DTO.HTTPv2_Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaceWebServer.DTO.MQTT_Protocol.Command.Server
{
    /// <summary>
    /// MQTT  服务器反馈设备鉴权结果  由服务器发送
    /// </summary>
    public class MQTT_Command_RequestAuthorizationACK : MQTTCommandPacket<RequestAuthorizationResultDTO>
    {
        

        public MQTT_Command_RequestAuthorizationACK(RequestAuthorizationResultDTO data,string cmdid)
        {
            Cmd = MQTT_Command_Define.RequestAuthorizationACK;
            Body = data;
            CreateToken();
            CmdID = cmdid;
        }

    }

    /// <summary>
    /// 服务器反馈设备鉴权结果
    /// </summary>
    public class RequestAuthorizationResultDTO
    {
        /// <summary>
        /// 请求ID
        /// </summary>
        public long RecordID { get; set; }
        /// <summary>
        /// 鉴权结果
        /// </summary>
        public int VerifyResult { get; set; }

        /// <summary>
        /// 鉴权消息文本
        /// </summary>
        public string VerifyMessage { get; set; }
    }
}
