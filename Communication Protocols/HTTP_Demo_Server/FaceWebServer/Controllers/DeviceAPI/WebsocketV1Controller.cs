using Azure;
using FaceWebServer.DTO.Config;
using FaceWebServer.DTO.HTTPv1_Protocol;
using FaceWebServer.Interface;
using FaceWebServer.Language;
using FaceWebServer.Utility;
using FaceWebServer.Utility.Model;
using MathNet.Numerics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NPOI.HPSF;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using static DeviceProtocolServer.WebSocket.WebsocketMiddlewareBase;

namespace DeviceProtocolServer.Controllers.DeviceAPI
{
    /// <summary>
    /// Websocket V1 协议测试
    /// </summary>

    [ApiController]
    public class WebsocketV1Controller : WebsocketControllerBase
    {
        private IServiceProvider _ServiceProvider;

        private ICacheService _Cache;
        private LanguageHandler _LanguageHandler;

        private IOptionsMonitor<HTTPProtocolOption> httpOptionMonitor = null;


        public WebsocketV1Controller(ILogger<WebsocketV1Controller> logger,
           ICacheService cache,
           IServiceProvider serviceProvider,
           IOptionsSnapshot<LanguageOption> lngopt,
           IOptionsMonitor<HTTPProtocolOption> httpProtocolOption) : base(logger)
        {
            _Cache = cache;
            _ServiceProvider = serviceProvider;
            _LanguageHandler = lngopt.Value.GetCurrentLanguageHandler();


            httpOptionMonitor = httpProtocolOption;
        }


        [Route("/WebsocketV1")]
        [HttpGet]
        public async Task<IActionResult> Connect()
        {
            var context = HttpContext;
            // 检查是否为WebSocket请求
            if (!context.WebSockets.IsWebSocketRequest)
            {
                //返回异常处理
                return new JsonResult(new JsonResultModel(1, "仅接受Websocket请求"));
            }

            // 连接客户端
            var mySocket = await ConnectClient(context);

            // 运行WebSocket处理逻辑，将接收到的消息原样返回
            await Run(mySocket, OnWebsocketMessageReceive);

            //返回异常处理
            return new JsonResult(new JsonResultModel("Websocket连接处理完毕"));
        }


        private async Task OnWebsocketMessageReceive(SocketContext socket, WebsocketMessage msg)
        {
            if (msg.IsBinary)
            {
                //接收到二进制消息
                var ms = msg.Body;
                ms.Seek(0, System.IO.SeekOrigin.Begin);
                StringBuilder hexBuf = new StringBuilder((int)ms.Length * 2);
                do
                {
                    hexBuf.Append(ms.ReadByte().ToString("X2"));

                } while (ms.Position != ms.Length);
                //接收到文本消息
                _logger.LogInformation($"接收到二进制消息：{hexBuf.ToString()}");
                hexBuf.Clear();
            }
            else
            {
                var response = await msg.GetText();
                //接收到文本消息
                _logger.LogInformation($"接收到消息：{response}");
                if (response == "PING")
                {
                    await SendStringAsync(socket.CurrentSocket, "PONG", socket.SocketCancellationToken);
                }
                else
                {
                    //检查是否为json字符串
                    if (response.StartsWith("{") && response.EndsWith("}"))
                    {
                        await ReceivedJsonProcessor(socket, response);
                    }
                    else
                    {
                        //直接原样返回
                        await SendStringAsync(socket.CurrentSocket, response, socket.SocketCancellationToken);
                    }
                }

            }

        }

        //接收到的命令处理
        private async Task ReceivedJsonProcessor(SocketContext socket, string jsonMessage)
        {
            try
            {
                // 解析JSON消息
                using var document = JsonDocument.Parse(jsonMessage);
                var root = document.RootElement;

                // 提取cmd字段
                string cmd = string.Empty;
                if (root.TryGetProperty("cmd", out var cmdProperty))
                {
                    cmd = cmdProperty.GetString();
                }

                // 提取ret字段
                string ret = string.Empty;
                if (root.TryGetProperty("ret", out var retProperty))
                {
                    ret = retProperty.GetString();
                }

                // 记录解析结果
                _logger.LogInformation($"解析命令: cmd={cmd}, ret={ret}");

                if (!string.IsNullOrEmpty(cmd))
                {
                    //处理设备发来的指令
                    await ReceivedCommandProcessor(cmd, socket, document);

                }
                if (!string.IsNullOrEmpty(ret))
                {
                    //处理设备发来的响应
                    await ReceivedCommandRetProcessor(cmd, socket, document);
                }

            }
            catch (JsonException ex)
            {
                // 记录JSON解析错误
                _logger.LogError($"JSON解析错误: {ex.Message}");
            }
            catch (Exception ex)
            {
                // 记录其他错误
                _logger.LogError($"处理命令时发生错误: {ex.Message}");
            }
        }

        /// <summary>
        /// 处理设备发来的命令请求
        /// </summary>
        /// <returns></returns>
        private async Task ReceivedCommandProcessor(string cmd, SocketContext socket, JsonDocument jsonDoc)
        {
            string sFile9_23 = "e:\\\\TWL918_Patch_v9.23.zip";
            string sFile9_22 = "e:\\\\TWL918_Patch_v9.22.zip";
            bool bUseURL = false;
            //string sFile9_23 = "e:\\\\FC-8190H_USB_v9.23.zip";
            //string sFile9_22 = "e:\\\\FC-8190H_USB_v9.22.zip";
            var root = jsonDoc.RootElement;
            try
            {
                object response = null;

                switch (cmd)
                {
                    case "ping":
                        {
                            response = new
                            {
                                ret = "pong", // 命令类型
                            };
                            break;
                        }
                    case "otacheck"://固件ota检查
                        {
                            //await Task.Delay(1 * 50);
                            string sFile = string.Empty;
                            if (root.TryGetProperty("version", out var retVersion))
                            {
                                string sVer = retVersion.GetString();
                                if (sVer.Contains("9.23"))
                                {
                                    sFile = sFile9_22;
                                }
                                else
                                {
                                    sFile = sFile9_23;
                                }
                            }

                            FileInfo sortInfo = new FileInfo(sFile);
                            string sURL = $"{httpOptionMonitor.CurrentValue.PeopleURLPrefix}/{sortInfo.Name}";
                            if (!bUseURL)
                                sURL = null;
                            // 创建响应JSON对象
                            response = new
                            {
                                ret = cmd, // 命令类型
                                result = true, // 执行结果
                                filesize = sortInfo.Length,
                                filename = sortInfo.Name,
                                md5 = MD5Helper.GetFileMD5ByHex(sFile),
                                url= sURL
                            };
                            break;
                        }
                    case "otaget"://获取固件分片
                        {
                            await Task.Delay(1*200);
                            int index = 0;
                            int icount = 0;
                            string sFile = string.Empty;
                            if (root.TryGetProperty("filename", out var retfilename))
                            {
                                string sfilename = retfilename.GetString();
                                if (sfilename.Contains("9.23"))
                                {
                                    sFile = sFile9_23;
                                }
                                else
                                {
                                    sFile = sFile9_22;
                                }
                            }

                            if (root.TryGetProperty("index", out var jsonProperty))
                            {
                                index = jsonProperty.GetInt32();
                            }
                            if (root.TryGetProperty("count", out jsonProperty))
                            {
                                icount = jsonProperty.GetInt32();
                            }

                            using var filestream = System.IO.File.OpenRead(sFile);
                            filestream.Position = index;
                            //if (icount < 1024 * 1024 *3)
                            //    icount = (int)Math.Min(1024 * 1024*3 , filestream.Length - index);
                            byte[] buf = new byte[icount];
                            int iReadCount = await filestream.ReadAsync(buf);
                            filestream.Close();
                            if (iReadCount > 0)
                            {


                                response = new
                                {
                                    ret = cmd, // 命令类型
                                    result = true, // 执行结果
                                    index = index,
                                    count = iReadCount,
                                    record = Convert.ToBase64String(buf, 0, iReadCount)
                                };
                            }
                            else
                            {
                                response = new
                                {
                                    ret = cmd, // 命令类型
                                    result = false, // 执行结果
                                };
                            }


                            break;
                        }
                    default:
                        {
                            // 创建响应JSON对象
                            response = new
                            {
                                ret = cmd, // 命令类型
                                result = true, // 执行结果
                            };
                            break;
                        }

                }


                if (response != null)
                {
                    // 发送响应
                    await SendJsonObjectAsync(socket.CurrentSocket, response, socket.SocketCancellationToken);
                }



                if (cmd == "reg")
                {
                    //await SendOTACommand(socket);
                    //await SendSetUserInfo(socket);
                }
            }
            catch (Exception ex)
            {
                // 记录错误
                _logger.LogError($"发送命令响应时发生错误: {ex.Message}");
            }
        }

        private async Task SendSetUserInfo(SocketContext socket)
        {
            //发送注册人员指令
            var response = new
            {
                cmd = "setuserinfo", // 命令类型
                enrollid = 9000,
                name = "test9000"
            };


            // 发送响应
            await SendJsonObjectAsync(socket.CurrentSocket, response, socket.SocketCancellationToken);
        }

        //发送强制OTA升级请求
        private async Task SendOTACommand(SocketContext socket)
        {
            //发送强制OTA指令
            var response = new
            {
                cmd = "forceota", // 命令类型
            };


            // 发送响应
            await SendJsonObjectAsync(socket.CurrentSocket, response, socket.SocketCancellationToken);
        }

        /// <summary>
        /// 处理设备发来的命令响应
        /// </summary>
        /// <returns></returns>
        private async Task ReceivedCommandRetProcessor(string cmd, SocketContext socket, JsonDocument jsonDoc)
        {
            //不处理响应
            await Task.CompletedTask;
        }
    }




}
