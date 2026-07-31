using FaceWebServer.DTO.Access;
using FaceWebServer.DTO.Config;
using FaceWebServer.Interface;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using System;
using MQTTnet.AspNetCore;
using MQTTnet.Server;
using System.Text;
using MQTTnet.Client;
using System.Security.Cryptography.X509Certificates;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using MQTTnet;
using FaceWebServer.DTO.MQTT;
using System.Net.Security;
using FaceWebServer.DTO.MQTT_Protocol;
using MQTTnet.Packets;
using System.Linq;
using Org.BouncyCastle.Bcpg;
using System.Collections.Concurrent;

namespace DeviceProtocolServer.MQTTServer
{
    public static class MQTTHostServiceExtensions
    {
        public static IServiceCollection AddMQTTHostService(this IServiceCollection services,
            IConfiguration config)
        {
            services.Configure<MQTTOptions>(config);

            services.AddSingleton(s =>
            {
                var optionMonitor = s.GetRequiredService<IOptionsMonitor<MQTTOptions>>();
                var options = optionMonitor.CurrentValue;
                var mqttFactory = new MqttFactory();
                var mqttOptionBuilder = new MqttServerOptionsBuilder();

                if (options.UseTCP)
                {
                    mqttOptionBuilder
                    .WithDefaultEndpoint()
                    .WithDefaultEndpointPort(options.TCPPort); //设置TCP端口号
                }

                if (options.UseTLS)
                {
                    X509Certificate2 pfx_x509 = new X509Certificate2(options.PfxCerfFile, options.PfxCerfPassword);

                    mqttOptionBuilder
                    .WithEncryptedEndpoint()
                    .WithEncryptedEndpointPort(options.TLSPort) //设置TLS加密端口号
                    .WithEncryptionSslProtocol(System.Security.Authentication.SslProtocols.Tls13)
                    .WithEncryptionCertificate(pfx_x509);

                    if (options.UseClientCert)
                    {
                        mqttOptionBuilder.WithClientCertificate(MQTTHostService.MQTT_CertificateValidation, false); //启用客户端证书验证
                    }
                    else
                    {
                        mqttOptionBuilder.WithRemoteCertificateValidationCallback(MQTTHostService.MQTT_CertificateValidation);
                    }
                }

                var mqttServerOptions = mqttOptionBuilder.Build();

                return mqttFactory.CreateMqttServer(mqttServerOptions);
            });


            services.AddHostedService<MQTTHostService>();
            services.AddScoped<MQTTCommandHandler>();
            return services;
        }
    }

    public class MQTTHostService : BackgroundService
    {
        private static MQTTOptions MQTT_Options;
        private static ILogger<MQTTHostService> _logger;

        private MqttServer _Mqtt;
        private IServiceProvider _ServiceProvider;
        private readonly object _lock = new object();

        /// <summary>
        /// MQTT客户端上下文，用来保存客户端命令队列。
        /// key = 客户端ID clientID，
        /// value = 客户端上下文
        /// </summary>
        private readonly ConcurrentDictionary<string, MQTT_Client_Context> _ClientContextMap = new();

        public MQTTHostService(
            ILogger<MQTTHostService> log,
            IOptionsMonitor<MQTTOptions> mqttOpt,
            IServiceProvider serviceProvider,
            MqttServer mqtt
        )
        {
            MQTT_Options = mqttOpt.CurrentValue;
            _logger = log;
            _Mqtt = mqtt;
            _ServiceProvider = serviceProvider;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            return Task.Run(() => Run(stoppingToken));
        }

        private async Task Run(CancellationToken stoppingToken)
        {
            await Task.Delay(3000);

            // 在这里添加MQTT消息处理逻辑
            _Mqtt.InterceptingPublishAsync += Mqtt_InterceptingPublishAsync;//当客户端向服务器发布消息时触发。
            _Mqtt.ValidatingConnectionAsync += MqttServer_ValidatingConnectionAsync; //1、验证连接请求
            _Mqtt.ClientConnectedAsync += MqttServer_ClientConnectedAsync;//2、客户端连接成功


            //拦截客户端的订阅请求，可以验证订阅的合法性或拒绝订阅。例如，限制某些主题的订阅权限。
            _Mqtt.InterceptingSubscriptionAsync += _Mqtt_InterceptingSubscriptionAsync;//当客户端尝试订阅某个主题时触发。

            _Mqtt.ClientSubscribedTopicAsync += MqttServer_ClientSubscribedTopicAsync;//3、客户端订阅
            _Mqtt.ClientUnsubscribedTopicAsync += MqttServer_ClientUnsubscribedTopicAsync;//4、客户端取消订阅 
            _Mqtt.ClientDisconnectedAsync += MqttServer_ClientDisconnectedAsync;//5、客户端断开连接
            await _Mqtt.StartAsync();

            StringBuilder sLog = new StringBuilder();
            sLog.Append("MQTT服务已启:");
            if (MQTT_Options.UseTCP)
                sLog.Append($" TCP:{MQTT_Options.TCPPort}");
            if (MQTT_Options.UseTLS)
                sLog.Append($" TLS:{MQTT_Options.TLSPort}");

            _logger.LogInformation(sLog.ToString());



            do
            {
                // 在这里添加其他后台任务逻辑
                var clientIDs = _ClientContextMap.Keys.ToArray();
                if (clientIDs.Length > 0)
                {
                    using var serviceScope = _ServiceProvider.CreateScope();
                    foreach (var clientid in clientIDs)
                    {
                        if (_ClientContextMap.TryGetValue(clientid, out var context))
                        {
                            try
                            {
                                if (!string.IsNullOrEmpty(context.DeviceSN))
                                {
                                    var handler = serviceScope.ServiceProvider.GetService<MQTTCommandHandler>();
                                    await handler.CheckDeviceCommandQueue(context, _Mqtt);
                                }

                            }
                            catch (Exception ex)
                            {

                                _logger.LogError(ex.ToString());
                            }

                        }

                    }
                }


                // 暂停一段时间，以避免过于频繁的循环
                await Task.Delay(TimeSpan.FromMilliseconds(10), stoppingToken);
            } while (!stoppingToken.IsCancellationRequested);
        }

        private Task _Mqtt_InterceptingSubscriptionAsync(InterceptingSubscriptionEventArgs arg)
        {
            //if (!arg.TopicFilter.Topic.StartsWith("/iot_hub/publish"))
            //{
            //    arg.Response.ReasonCode = MQTTnet.Protocol.MqttSubscribeReasonCode.UnspecifiedError;
            //    arg.ProcessSubscription = false;
            //    _logger.LogInformation($"拦截客户端订阅消息 ClientId:{arg.ClientId}  {arg.TopicFilter.Topic} 拒绝订阅");
            //}

            return Task.CompletedTask;
        }

        public static bool MQTT_CertificateValidation(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            if (MQTT_Options == null)
            {
                return true;
            }

            _logger.LogInformation("SSL开始验证客户端证书");

            if (certificate == null)
            {
                if (MQTT_Options.UseClientCert)
                {
                    _logger.LogWarning("SSL客户端证书为空");
                    return false;
                }
                else
                    return true;

            }

            if (string.IsNullOrEmpty(MQTT_Options.CaPfxCerf))
            {
                return true;
            }

            // 加载 CA 证书
            var caCert = new X509Certificate2(MQTT_Options.CaPfxCerf);
            // 将 X509Certificate 转换为 X509Certificate2
            var clientCertificate = new X509Certificate2(certificate);
            // 配置证书链

            chain.ChainPolicy.ExtraStore.Add(caCert);
            chain.ChainPolicy.RevocationMode = X509RevocationMode.NoCheck; // 1.	禁用吊销检查：将 RevocationMode 设置为 NoCheck，因为自签名 CA 证书通常没有吊销列表
            chain.ChainPolicy.VerificationFlags = X509VerificationFlags.AllowUnknownCertificateAuthority;//2.	允许未知的证书颁发机构：将 VerificationFlags 设置为 AllowUnknownCertificateAuthority，以允许自签名 CA 证书。
            chain.ChainPolicy.VerificationTime = DateTime.Now;

            // 验证证书链
            bool isChainValid = chain.Build(clientCertificate);
            if (!isChainValid)
            {
                _logger.LogWarning("SSL证书链验证失败");
                foreach (var chainStatus in chain.ChainStatus)
                {
                    _logger.LogWarning($"    链状态: {chainStatus.StatusInformation}");
                }
                return false;
            }

            // 验证证书的有效期
            if (DateTime.Now < clientCertificate.NotBefore || DateTime.Now > clientCertificate.NotAfter)
            {
                _logger.LogWarning("SSL证书不在有效期内");
                return false;
            }

            // 验证证书的主题
            string expectedIssuer = "CN=WebCA"; // 替换为预期的颁发者名称
            if (!clientCertificate.Issuer.Contains(expectedIssuer))
            {
                Console.WriteLine($"证书颁发者不匹配: {clientCertificate.Issuer}");
                return false;
            }

            _logger.LogInformation($"客户端证书颁发者: {clientCertificate.Issuer}");
            _logger.LogInformation($"客户端证书身份: {clientCertificate.Subject}");
            _logger.LogInformation("SSL证书验证成功");
            return true;
        }


        private async Task Mqtt_InterceptingPublishAsync(InterceptingPublishEventArgs arg)
        {

            var msg = arg.ApplicationMessage;
            //var msgBody = UTF8Encoding.UTF8.GetString(msg.PayloadSegment);

            //_logger.LogInformation($"拦截应用消息 ClientId:{arg.ClientId}  {msg.Topic} -- {msgBody}");

            if (arg.ClientId != "@@Server")
            {
                if (_ClientContextMap.TryGetValue(arg.ClientId, out var oClient))
                {
                    //进行解包
                    var mqttPacket = await MQTTCommandPacketExtend.Parse(msg.PayloadSegment);//解析数据包
                    if (mqttPacket != null)
                    {
                        oClient.ClienPublishTopic = msg.Topic;
                        oClient.PacketUseGZIP = mqttPacket.UseGZIP;
                        if (mqttPacket.Packet != null)
                        {
                            oClient.ReceivedMessage.Enqueue(mqttPacket);//加入消息队列中处理
                            _logger.LogInformation($"收到设备MQTT协议数据包  ClientId:{arg.ClientId}  {msg.Topic} -- cmd:{mqttPacket.Packet.Cmd}");
                        } 
                    }
                    else
                    {
                        var msgBody = msg.ConvertPayloadToString();
                        _logger.LogInformation($"收到MQTT消息 ClientId:{arg.ClientId}  {msg.Topic} -- {msgBody}");
                        //发回去
                        if (!string.IsNullOrEmpty(oClient.ClientSubscribeTopic))
                        {

                            var message = new MqttApplicationMessageBuilder()
                                        .WithTopic(oClient.ClientSubscribeTopic)
                                        .WithPayloadSegment(msg.PayloadSegment)
                                        .Build();
                            await Task.Delay(300);
                            // Now inject the new message at the broker.
                            await _Mqtt.InjectApplicationMessage(
                                new InjectedMqttApplicationMessage(message)
                                {
                                    SenderClientId = "@@Server",
                                });
                        }
                    }
                }
            }
        }


        private async Task MqttServer_ValidatingConnectionAsync(ValidatingConnectionEventArgs arg)
        {
            //在此处验证客户端的账号密码，客户端id等信息
            if (!arg.UserName.StartsWith("IotDevice"))
            {
                _logger.LogInformation($"收到MQTT握手 但是用户名不合法:{arg.UserName}");
                arg.ReasonCode = MQTTnet.Protocol.MqttConnectReasonCode.BadUserNameOrPassword;
                return;
            }
            if (!arg.ClientId.StartsWith("iot_device"))
            {
                _logger.LogInformation($"收到MQTT握手 但是客户端ID不合法:{arg.ClientId}");
                arg.ReasonCode = MQTTnet.Protocol.MqttConnectReasonCode.ClientIdentifierNotValid;
                return;
            }
            //_logger.LogInformation($"客户端身份验证 {arg.Endpoint} -- ProtocolVersion:{arg.ProtocolVersion} \n ClientId:{arg.ClientId}  UserName:{arg.UserName} \n 密码长度：{arg.RawPassword.Length}");

            //可以在此处使用证书进行身份验证
            if (arg.ClientCertificate != null)
                _logger.LogInformation($"客户端证书身份 {arg.ClientCertificate.Subject}");
            //拒绝连接
            //await _Mqtt.DisconnectClientAsync(arg.ClientId);


            await Task.CompletedTask;
        }

        /// <summary>
        /// 客户端订阅主题
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>

        private async Task MqttServer_ClientSubscribedTopicAsync(ClientSubscribedTopicEventArgs arg)
        {
            _logger.LogInformation($"客户端订阅主题 ClientId:{arg.ClientId} -- {arg.TopicFilter.Topic}");


            if (_ClientContextMap.TryGetValue(arg.ClientId, out var client))
            {
                client.ClientSubscribeTopic = arg.TopicFilter.Topic;
                client.DeviceSN = client.ClientSubscribeTopic.Split('/').LastOrDefault();

                await Task.CompletedTask;
            }

        }

        /// <summary>
        /// 客户端取消订阅主题
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        private async Task MqttServer_ClientUnsubscribedTopicAsync(ClientUnsubscribedTopicEventArgs arg)
        {
            //_logger.LogInformation($"客户端取消订阅主题 ClientId:{arg.ClientId} -- {arg.TopicFilter}");
            if (_ClientContextMap.TryGetValue(arg.ClientId, out var client))
            {
                client.ClientSubscribeTopic = string.Empty;
                await Task.CompletedTask;
            }

        }

        /// <summary>
        /// 客户端连接接入
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        private async Task MqttServer_ClientConnectedAsync(ClientConnectedEventArgs arg)
        {

            //创建MQTT客户端上下文
            MQTT_Client_Context context = new MQTT_Client_Context();
            context.ClientID = arg.ClientId;
            context.RemoteAddr = arg.Endpoint.ToString();


            _ClientContextMap.TryAdd(context.ClientID, context);


            _logger.LogInformation($"客户端连接接入 {arg.Endpoint} -- ProtocolVersion:{arg.ProtocolVersion} \n ClientId:{arg.ClientId}  UserName:{arg.UserName}   ");
            await Task.CompletedTask;
        }


        /// <summary>
        /// 客户端断开连接
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        private async Task MqttServer_ClientDisconnectedAsync(ClientDisconnectedEventArgs arg)
        {

            _ClientContextMap.TryRemove(arg.ClientId, out _);
            _logger.LogInformation($"客户端断开连接 {arg.ClientId} {arg.Endpoint}");
            await Task.CompletedTask;
        }

    }
}
