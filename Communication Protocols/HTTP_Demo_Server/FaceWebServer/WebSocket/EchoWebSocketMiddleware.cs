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
    /// 回声WebSocket中间件，用于将接收到的消息原样返回
    /// </summary>
    public class EchoWebSocketMiddleware : WebsocketMiddlewareBase
    {
        /// <summary>
        /// 下一个中间件委托
        /// </summary>
        private readonly RequestDelegate _next;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="logger">日志记录器</param>
        /// <param name="next">下一个中间件委托</param>
        public EchoWebSocketMiddleware(ILogger<EchoWebSocketMiddleware> logger, RequestDelegate next)
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

            // 连接客户端
            var mySocket = await ConnectClient(context);

            // 运行WebSocket处理逻辑，将接收到的消息原样返回
            await Run(mySocket, async (socket, response) =>
             {
                 _logger.LogInformation($"接收到消息：{response}");
                 await SendStringAsync(socket.CurrentSocket, response, socket.SocketCancellationToken);
             });
        }
    }
}
