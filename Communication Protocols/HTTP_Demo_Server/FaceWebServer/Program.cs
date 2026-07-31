using Autofac.Extensions.DependencyInjection;
using FaceWebServer.DTO.MQTT_Protocol;
using FaceWebServer.Utility.Extend;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.Connections;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NPOI.SS.Formula.Functions;
using System;
using System.IO;
using System.Net.Security;
using System.Text;
using System.Threading.Tasks;

namespace DeviceProtocolServer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //MQTTCommandPacketExtend.TestPacket();

            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);//使得应用程序能够使用更多的字符编码，特别是一些不在 .NET Core 默认支持范围内的编码

            IConfiguration config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .AddEnvironmentVariables().Build();

            //  Console.Title = "人脸机管理系统 ";
            CreateHostBuilder(args, config).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args, IConfiguration config) =>
            Host.CreateDefaultBuilder(args)
            .ConfigureLogging((context, loggingBuilder) =>
            {

                loggingBuilder.ClearProviders();

                loggingBuilder.AddConsole();

                loggingBuilder.AddFilter("System", LogLevel.Warning);
                loggingBuilder.AddFilter("Microsoft", LogLevel.Error);//过滤掉系统默认的一些日志
                string sfile = Path.Combine(Directory.GetCurrentDirectory(), "log4net.Config");
                if (File.Exists(sfile))
                    loggingBuilder.AddLog4Net(sfile);//文件路径
            })
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.ConfigureKestrel((context, options) =>
                {
                    // Handle requests up to 50 MB
                    options.Limits.MaxRequestBodySize = 400 * 1024 * 1024;
                });

                webBuilder.UseKestrel((context, options) =>
                {

                    string sHttpPort = config["HttpPort"];
                    if (!string.IsNullOrEmpty(sHttpPort) && int.TryParse(sHttpPort, out int iHttpPort))
                    {
                        options.ListenAnyIP(iHttpPort);
                    }

                    sHttpPort = config["https:port"];
                    if (!string.IsNullOrEmpty(sHttpPort) && int.TryParse(sHttpPort, out iHttpPort))
                    {
                        var sCertFile = Path.Combine(Directory.GetCurrentDirectory(), config["https:cert"]);
                        if (File.Exists(sCertFile))
                        {
                            var sCertPassword = config["https:password"];
                            options.ListenAnyIP(iHttpPort, lop =>
                            {
                                lop.UseHttps(sCertFile, sCertPassword, connectionOptions =>
                                {

                                    Console.WriteLine($"端口:{iHttpPort}\n 证书主题：{connectionOptions.ServerCertificate.Subject}\n 签发机构：{connectionOptions.ServerCertificate.Issuer}");

                                    connectionOptions.ClientCertificateValidation = (cert, chain, errors) =>
                                    {
                                        if (cert != null)
                                        {
                                            Console.WriteLine($"客户端证书验证 \n 证书主题:{cert.Subject}\n 签发机构: {cert.Issuer}");

                                        }

                                        // 自定义客户端证书验证逻辑
                                        return true;
                                    };
                                    //connectionOptions.AllowAnyClientCertificate();
                                    connectionOptions.CheckCertificateRevocation = false;
                                    //RequireCertificate 强制客户端必须要配置证书
                                    connectionOptions.ClientCertificateMode = Microsoft.AspNetCore.Server.Kestrel.Https.ClientCertificateMode.AllowCertificate;
                                });
                                lop.Protocols = Microsoft.AspNetCore.Server.Kestrel.Core.HttpProtocols.Http1;

                            });
                        }
                        else
                        {
                            Console.WriteLine($"Load SSL Cert Error:{sCertFile}");
                        }

                    }

                });
                webBuilder.UseStartup<Startup>();
            })
            .UseServiceProviderFactory(new AutofacServiceProviderFactory());

    }
}
