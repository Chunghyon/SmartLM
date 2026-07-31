
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace DeviceProtocolServer.Controllers.DeviceAPI
{
    /// <summary>
    /// Websocket 协议处理器基类
    /// </summary>
    public abstract class WebsocketControllerBase : ControllerBase
    {
        /// <summary>
        /// 日志记录器
        /// </summary>
        public ILogger _logger;

        public WebsocketControllerBase(ILogger log)
        {
            _logger = log;
        }

        /// <summary>
        /// Socket连接上下文类，用于存储WebSocket连接相关信息
        /// </summary>
        protected class SocketContext
        {
            /// <summary>
            /// 当前的WebSocket连接
            /// </summary>
            public System.Net.WebSockets.WebSocket CurrentSocket;
            /// <summary>
            /// Socket取消令牌，用于检测连接是否被取消
            /// </summary>
            public CancellationToken SocketCancellationToken;
            /// <summary>
            /// WebSocket连接的唯一标识符
            /// </summary>
            public string WebsocketID;
        }


        /// <summary>
        /// 表示一个Websocket消息
        /// </summary>
        protected class WebsocketMessage : IDisposable
        {
            /// <summary>
            /// 表示是否为二进制消息
            /// </summary>
            public bool IsBinary { get; private set; }

            /// <summary>
            /// 包含消息内容的内存流
            /// </summary>
            public MemoryStream Body { get; private set; }


            public WebsocketMessage(bool isbin, MemoryStream ms)
            {
                IsBinary = isbin;
                Body = ms;
            }

            //获取消息文本
            public async Task<string> GetText()
            {
                // 重置内存流位置
                Body.Seek(0, SeekOrigin.Begin);
                // 检查消息类型是否为文本
                if (IsBinary == true)
                {
                    return string.Empty;
                }

                // 读取消息内容
                using (var reader = new StreamReader(Body, Encoding.UTF8))
                {
                    return await reader.ReadToEndAsync();
                }
            }

            /// <summary>
            /// 释放消息
            /// </summary>
            public void Dispose()
            {
                if (Body != null)
                {
                    Body.Dispose();
                    Body = null;
                }
            }
        }

        /// <summary>
        /// 连接客户端，创建并返回Socket上下文
        /// </summary>
        /// <param name="context">HTTP上下文</param>
        /// <returns>Socket上下文</returns>
        protected async Task<SocketContext> ConnectClient(HttpContext context)
        {
            SocketContext socket = new SocketContext();
            // 生成唯一的WebSocket ID
            socket.WebsocketID = Guid.NewGuid().ToString();

            // 获取请求取消令牌
            socket.SocketCancellationToken = context.RequestAborted;
            // 接受WebSocket连接
            socket.CurrentSocket = await context.WebSockets.AcceptWebSocketAsync();
            return socket;
        }

        /// <summary>
        /// 运行WebSocket处理逻辑
        /// </summary>
        /// <param name="socket">Socket上下文</param>
        /// <param name="actionReceive">消息接收处理委托</param>
        /// <returns>任务</returns>
        protected async Task Run(SocketContext socket, Func<SocketContext, WebsocketMessage, Task> actionReceive)
        {
            while (true)
            {
                // 检查是否请求取消
                if (socket.SocketCancellationToken.IsCancellationRequested)
                {
                    break;
                }
                try
                {
                    // 检查Socket是否为空
                    if (socket.CurrentSocket == null) break;
                    // 接收消息
                    WebsocketMessage response = await ReceiveAsync(socket);
                    // 检查Socket是否为空
                    if (socket.CurrentSocket == null) break;
                    // 检查Socket状态是否为打开
                    if (socket.CurrentSocket.State != WebSocketState.Open)
                    {
                        break;
                    }
                    // 检查消息是否为空
                    if (response == null)
                    {
                        continue;
                    }
                    else
                    {
                        // 处理消息
                        await actionReceive(socket, response);
                        response.Dispose();
                    }
                }
                catch (Exception ex)
                {
                    // 记录错误并继续
                    _logger.LogError("WebsocketMiddlewareBase.Run -- " + ex.Message);
                    continue;
                }

            }
            // 关闭连接
            await CloseConnect(socket);
        }

        /// <summary>
        /// 发送字符串消息
        /// </summary>
        /// <param name="socket">WebSocket连接</param>
        /// <param name="data">要发送的数据</param>
        /// <param name="ct">取消令牌</param>
        /// <returns>任务</returns>
        protected Task SendStringAsync(System.Net.WebSockets.WebSocket socket, string data, CancellationToken ct = default)
        {
            // 将字符串转换为字节数组
            var buffer = Encoding.UTF8.GetBytes(data);
            var segment = new ArraySegment<byte>(buffer);
            // 发送消息
            return socket.SendAsync(segment, WebSocketMessageType.Text, true, ct);
        }

        /// <summary>
        /// 发送Json对象，将对象序列化后发送json文本
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="socket"></param>
        /// <param name="data"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        protected Task SendJsonObjectAsync<T>(System.Net.WebSockets.WebSocket socket, T data, CancellationToken ct = default)
            where T:class 
        {
            // 序列化响应JSON
            string responseJson = JsonSerializer.Serialize<T>(data);
            // 将字符串转换为字节数组
            var buffer = Encoding.UTF8.GetBytes(responseJson);
            var segment = new ArraySegment<byte>(buffer);
            // 发送消息
            return socket.SendAsync(segment, WebSocketMessageType.Text, true, ct);
        }


        /// <summary>
        /// 发送二进制消息
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="data"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        protected Task SendBytesAsync(System.Net.WebSockets.WebSocket socket, MemoryStream ms, CancellationToken ct = default)
        {
            if (ms == null)
            {
                throw new ArgumentNullException(nameof(ms), "MemoryStream 实例不能为 null");
            }

            // 增加溢出检查，避免long转int出错
            if (ms.Position > int.MaxValue || (ms.Length - ms.Position) > int.MaxValue)
            {
                throw new InvalidOperationException("数据量过大，超出int类型的最大范围");
            }

            if ((ms.Length - ms.Position) == 0)
            {
                throw new InvalidOperationException("没有需要发送的数据");
            }

            byte[] buffer;
            // 先判断是否能安全获取内部缓冲区，不能则抛出明确异常
            if (!ms.TryGetBuffer(out ArraySegment<byte> internalBuffer))
            {
                throw new InvalidOperationException("当前 MemoryStream 不允许访问内部缓冲区，无法执行无拷贝发送");
            }
            buffer = internalBuffer.Array;

            // 将内存流转换为字节数组
            var segment = new ArraySegment<byte>(buffer, (int)ms.Position, (int)(ms.Length - ms.Position));
            // 发送消息
            return socket.SendAsync(segment, WebSocketMessageType.Binary, true, ct);
        }


        /// <summary>
        /// 接收消息
        /// </summary>
        /// <param name="socket">Socket上下文</param>
        protected async Task<WebsocketMessage> ReceiveAsync(SocketContext socket)
        {
            // 创建缓冲区
            var buffer = new ArraySegment<byte>(new byte[8192]);
            var ms = new MemoryStream(8192);


            WebSocketReceiveResult result = null;
            try
            {
                do
                {
                    // 检查是否请求取消
                    socket.SocketCancellationToken.ThrowIfCancellationRequested();
                    // 检查Socket是否为空
                    if (socket.CurrentSocket == null) break;
                    // 接收消息
                    result = await socket.CurrentSocket.ReceiveAsync(buffer, socket.SocketCancellationToken);
                    // 写入内存流
                    ms.Write(buffer.Array, buffer.Offset, result.Count);
                }
                while (!result.EndOfMessage);
            }
            catch (Exception ex)
            {
                // 记录错误并返回空字符串
                _logger.LogError("WebsocketMiddlewareBase.ReceiveStringAsync -- " + ex.Message);
                ms.Dispose();
                return null;
            }

            // 检查结果是否为空
            if (result == null)
            {
                ms.Dispose();
                return null;
            }
            return new WebsocketMessage(result.MessageType == WebSocketMessageType.Binary, ms);

        }

        /// <summary>
        /// 关闭WebSocket连接
        /// </summary>
        /// <param name="socket">Socket上下文</param>
        /// <returns>任务</returns>
        protected async Task CloseConnect(SocketContext socket)
        {
            // 检查Socket是否为空
            if (socket.CurrentSocket == null) return;

            try
            {
                // 检查Socket状态是否为已中止
                if (socket.CurrentSocket.State != WebSocketState.Aborted)
                {
                    // 关闭连接
                    await socket.CurrentSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closing", socket.SocketCancellationToken);
                    // 释放资源
                    socket.CurrentSocket.Dispose();
                }
            }
            catch (Exception)
            {
                // 记录错误
                _logger.LogError("WebsocketMiddlewareBase.CloseConnect -- " + "关闭 websocket 连接时发生错误"); ;
            }
            // 设置Socket为null
            socket.CurrentSocket = null;
        }
    }
}
