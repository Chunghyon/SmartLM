using FaceWebServer.DTO.HTTPv2_Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaceWebServer.DTO.MQTT_Protocol.Command.Device
{
    /// <summary>
    /// MQTT 设备反馈注册用户凭证结果  由设备发送
    /// </summary>
    public class MQTT_Command_RegisterIdentifyTicketACK : MQTTCommandPacket<HTTPDeviceRegisterIdentifyTicketResult>
    {
        /// <summary>
        /// 人员照片
        /// </summary>
        private ArraySegment<byte> PeopleImage = null;
        public MQTT_Command_RegisterIdentifyTicketACK()
        {
            Cmd = MQTT_Command_Define.RegisterIdentifyTicketACK;

        }

        /// <summary>
        /// 设置附加数据到命令中
        /// </summary>
        /// <param name="dataBuf"></param>
        public override void SetDataBuf(ArraySegment<byte> dataBuf)
        {
            //有需要附加数据的命令，需要重写此方法
            PeopleImage = dataBuf;
        }

        /// <summary>
        /// 从命令中获取附加数据，如果没有附加数据，返回null
        /// </summary>
        /// <returns></returns>
        public override ArraySegment<byte> GetDataBuf()
        {
            return PeopleImage;
        }
    }
}
