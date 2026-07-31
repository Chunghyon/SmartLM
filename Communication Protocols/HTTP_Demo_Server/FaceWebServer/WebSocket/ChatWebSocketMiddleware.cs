using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DeviceProtocolServer.WebSocket
{
    /// <summary>
    /// 聊天WebSocket中间件，用于处理消息的发送和接收
    /// </summary>
    public class ChatWebSocketMiddleware : WebsocketMiddlewareBase
    {
        /// <summary>
        /// 存储WebSocket连接的并发字典，键为socket ID，值为Socket上下文
        /// </summary>
        private static ConcurrentDictionary<string, SocketContext> _sockets = new ConcurrentDictionary<string, SocketContext>();

        /// <summary>
        /// 下一个中间件委托
        /// </summary>
        private readonly RequestDelegate _next;

        /// <summary>
        /// Socket ID
        /// </summary>
        private string _socketId;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="logger">日志记录器</param>
        /// <param name="next">下一个中间件委托</param>
        public ChatWebSocketMiddleware(ILogger<ChatWebSocketMiddleware> logger, RequestDelegate next)
        {
            _next = next;
            _logger = logger;
        }

        /// <summary>
        /// 处理HTTP请求
        /// </summary>
        /// <param name="context">HTTP上下文</param>
        /// <returns>任务</returns>
        public async Task Invoke(HttpContext context)
        {
            // 检查是否为WebSocket请求
            if (!context.WebSockets.IsWebSocketRequest)
            {
                // 不是WebSocket请求，传递给下一个中间件
                await _next.Invoke(context);
                return;
            }
            // 检查路径是否为/message
            if (context.Request.Path != "/message")
            {
                // 路径不正确，传递给下一个中间件
                await _next.Invoke(context);
                return;
            }

            // 连接客户端
            var mySocket = await ConnectClient(context);

            // 获取socket ID
            _socketId = context.Request.Query["sid"].ToString();
            // 检查socket ID是否有效且不存在
            if (!string.IsNullOrWhiteSpace(_socketId) && !_sockets.ContainsKey(_socketId))
            {
                // 添加到连接字典
                _sockets.TryAdd(_socketId, mySocket);
            }
            else
            {
                // 关闭连接并返回
                await CloseConnect(mySocket);//终止了
                return;
            }

            // 开始运行基础逻辑
            await Run(mySocket, ResponseMessage);

            // 从连接字典中移除
            _sockets.TryRemove(_socketId, out mySocket);
            mySocket = null;
        }

        /// <summary>
        /// 响应消息处理
        /// </summary>
        /// <param name="sendSocket">发送方Socket上下文</param>
        /// <param name="sMessage">消息内容</param>
        /// <returns>任务</returns>
        public async Task ResponseMessage(SocketContext sendSocket, string sMessage)
        {
            try
            {
                // 反序列化消息模板
                WebsocketMsgTemplate msg = JsonConvert.DeserializeObject<WebsocketMsgTemplate>(sMessage);
                // 遍历所有连接
                foreach (var socket in _sockets)
                {
                    // 检查Socket状态是否为打开
                    if (socket.Value.CurrentSocket.State != WebSocketState.Open)
                    {
                        continue;
                    }
                    // 检查是否为接收方
                    if (socket.Key == msg.ReceiverID)
                    {
                        // 发送消息给接收方
                        await SendStringAsync(socket.Value.CurrentSocket, JsonConvert.SerializeObject(msg), socket.Value.SocketCancellationToken);
                    }
                }
            }
            catch (Exception ex)
            {
                // 记录错误
                _logger.LogError("ChatWebSocketMiddleware.ResponseMessage -- " + ex.Message);
            }
        }

    }
}
