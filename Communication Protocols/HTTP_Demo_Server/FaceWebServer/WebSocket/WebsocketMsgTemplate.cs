using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DeviceProtocolServer.WebSocket
{
    /// <summary>
    /// 消息模板
    /// </summary>
    public class WebsocketMsgTemplate
    {
        /// <summary>
        /// 发送者ID
        /// </summary>
        public string SenderID { get; set; }
        /// <summary>
        /// 接收者ID
        /// </summary>
        public string ReceiverID { get; set; }
        /// <summary>
        /// 消息类型
        /// </summary>
        public string MessageType { get; set; }
        /// <summary>
        /// 消息内容
        /// </summary>
        public string Content { get; set; }
    }
}
