using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FaceWebServer.DTO.MQTT_Protocol
{
    /// <summary>
    /// MQTT客户端上下文，用来保存客户端命令队列
    /// </summary>
    public class MQTT_Client_Context
    {
        /// <summary>
        /// 设备SN
        /// </summary>
        public string DeviceSN { get; set; }

        /// <summary>
        /// 远程地址
        /// </summary>
        public string RemoteAddr { get; set; }

        /// <summary>
        /// 客户端ID
        /// </summary>
        public string ClientID { get; set; }

        /// <summary>
        /// 客户端订阅的主题，以便于服务器推送消息
        /// </summary>
        public string ClientSubscribeTopic { get; set; }

        /// <summary>
        /// 客户端发布消息的主题
        /// </summary>
        public string ClienPublishTopic { get; set; }

        /// <summary>
        /// 当前正在执行的命令
        /// </summary>
        public MQTTCommandPacket CurrentCommand { get; set; }

        /// <summary>
        /// 当前命令的执行时间
        /// </summary>
        public DateTime CurrentCommandSendTime { get; set; }

        /// <summary>
        /// 当前命令的超时时间
        /// </summary>
        public DateTime CurrentCommandTimeOutTime { get; set; }


        /// <summary>
        /// 并发控制信号量，防止并发执行 CommandHandler
        /// </summary>
        //public SemaphoreSlim AsyncSemaphore { get; private set; } = new SemaphoreSlim(1, 1);

        /// <summary>
        /// 表示协议包是否需要GZIP压缩
        /// </summary>
        public bool PacketUseGZIP { get; set; }

        /// <summary>
        /// 客户端是否已激活，收到keepalive 表示已激活
        /// </summary>
        public bool ClientKeepliveActivate { get; set; }

        /// <summary>
        /// 接收到的设备消息队列
        /// </summary>
        public ConcurrentQueue<MQTTCommandPacketParseResult> ReceivedMessage { get; set; } = new ConcurrentQueue<MQTTCommandPacketParseResult>();
        ///// <summary>
        ///// 需要执行的命令队列
        ///// key=命令，value=命令数据包
        ///// </summary>
        //public Dictionary<string, MQTTCommandPacket> CommandMap { get; set; } 

        //public MQTT_Client_Context()
        //{
        //    CommandMap = new Dictionary<string, MQTTCommandPacket>();
        //}
    }
}
