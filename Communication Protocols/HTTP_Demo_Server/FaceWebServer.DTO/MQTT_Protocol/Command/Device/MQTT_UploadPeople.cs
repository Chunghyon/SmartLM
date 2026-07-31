using FaceWebServer.DTO.HTTPv2_Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaceWebServer.DTO.MQTT_Protocol.Command.Device
{

    /// <summary>
    /// MQTT 设备主动推送人员信息 ，由设备发送
    /// </summary>
    public class MQTT_Command_UploadPeople : MQTTCommandPacket<MQTT_UploadPeople>
    {
        /// <summary>
        /// 人员照片
        /// </summary>
        private ArraySegment<byte> PeopleImage = null;

        public MQTT_Command_UploadPeople()
        {
            Cmd = MQTT_Command_Define.UploadPeople;

        }

        public MQTT_Command_UploadPeople(MQTT_UploadPeople data)
        {
            Cmd = MQTT_Command_Define.UploadPeople;
            Body = data;

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

    /// <summary>
    /// 设备主动推送的人员信息
    /// </summary>
    public class MQTT_UploadPeople 
    {
        /// <summary>
        /// 人员在设备中的改变类型：
        /// 1--新增；2--更新；3--删除；4--查询；
        /// </summary>
        public int PushType { get; set; }

        /// <summary>
        /// 人员用户号
        /// </summary>
        public long UserID { get; set; }

        /// <summary>
        /// 人员详情
        /// </summary>
        public HTTPPeopleV2? Detail { get; set; }
    }
}
