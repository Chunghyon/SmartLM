using FaceWebServer.DTO.HTTPv2_Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaceWebServer.DTO.MQTT_Protocol.Command.Server
{
    /// <summary>
    /// MQTT 服务器发送删除人员消息 ，由服务器发送
    /// </summary>
    public class MQTT_Command_PushDeletePeople : MQTTCommandPacket<MQTT_PushDeletePeople>
    {

        public MQTT_Command_PushDeletePeople(MQTT_PushDeletePeople data)
        {
            Cmd = MQTT_Command_Define.PushDeletePeople;
            Body = data;
            CreateToken();

        }
    }

    /// <summary>
    /// 服务器推送的工作参数
    /// </summary>
    public class MQTT_PushDeletePeople 
    {
        /// <summary>
        /// 1：清空所有人员信息  0：按指定用户号删除
        /// </summary>
        public int DeleteAll { get; set; }



        /// <summary>
        /// 待删除人员数量
        /// </summary>
        public int DeleteCount { get; set; }

        /// <summary>
        /// 需要删除的用户号列表
        /// </summary>
        public List<long> DeleteList { get; set; }
    }

}
