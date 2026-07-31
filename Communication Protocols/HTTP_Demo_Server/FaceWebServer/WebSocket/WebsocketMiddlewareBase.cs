using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
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
    /// WebSocket中间件基类，提供WebSocket连接管理的基础功能
    /// </summary>
    public class WebsocketMiddlewareBase
    {
        /// <summary>
        /// 日志记录器
        /// </summary>
        public ILogger _logger;

        /// <summary>
        /// Socket连接上下文类，用于存储WebSocket连接相关信息
        /// </summary>
        public class SocketContext
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
        /// 连接客户端，创建并返回Socket上下文
        /// </summary>
        /// <param name="context">HTTP上下文</param>
        /// <returns>Socket上下文</returns>
        public async Task<SocketContext> ConnectClient(HttpContext context)
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
        public async Task Run(SocketContext socket, Func<SocketContext, string, Task> actionReceive)
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
                    string response = await ReceiveStringAsync(socket);
                    // 检查Socket是否为空
                    if (socket.CurrentSocket == null) break;
                    // 检查Socket状态是否为打开
                    if (socket.CurrentSocket.State != WebSocketState.Open)
                    {
                        break;
                    }
                    // 检查消息是否为空
                    if (string.IsNullOrEmpty(response))
                    {
                        continue;
                    }
                    else
                    {
                        // 处理PING消息，返回PONG
                        if (response.StartsWith("PING"))
                        {
                            await SendStringAsync(socket.CurrentSocket, "PONG", socket.SocketCancellationToken);
                            continue;
                        }
                        else
                        {
                            // 处理其他消息
                            await actionReceive(socket, response);
                        }
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
        public Task SendStringAsync(System.Net.WebSockets.WebSocket socket, string data, CancellationToken ct = default)
        {
            // 将字符串转换为字节数组
            var buffer = Encoding.UTF8.GetBytes(data);
            var segment = new ArraySegment<byte>(buffer);
            // 发送消息
            return socket.SendAsync(segment, WebSocketMessageType.Text, true, ct);
        }

        /// <summary>
        /// 接收字符串消息
        /// </summary>
        /// <param name="socket">Socket上下文</param>
        /// <returns>接收到的字符串消息</returns>
        public async Task<string> ReceiveStringAsync(SocketContext socket)
        {
            // 创建缓冲区
            var buffer = new ArraySegment<byte>(new byte[8192]);
            using (var ms = new MemoryStream())
            {
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
                    return string.Empty;
                }

                // 检查结果是否为空
                if (result == null)
                {
                    return string.Empty;
                }
                // 重置内存流位置
                ms.Seek(0, SeekOrigin.Begin);
                // 检查消息类型是否为文本
                if (result.MessageType != WebSocketMessageType.Text)
                {
                    return null;
                }

                // 读取消息内容
                using (var reader = new StreamReader(ms, Encoding.UTF8))
                {
                    return await reader.ReadToEndAsync();
                }
            }
        }

        /// <summary>
        /// 关闭WebSocket连接
        /// </summary>
        /// <param name="socket">Socket上下文</param>
        /// <returns>任务</returns>
        public async Task CloseConnect(SocketContext socket)
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
