using Autofac;
using DeviceProtocolServer.HostedService;
using DeviceProtocolServer.Middleware;
using DeviceProtocolServer.MQTTServer;
using DeviceProtocolServer.Utilities;
using DeviceProtocolServer.WebSocket;
using FaceWebServer.DB;
using FaceWebServer.DTO.Config;
using FaceWebServer.Interface;
using FaceWebServer.Language;
using FaceWebServer.Utility.FilterAttribute;
using FaceWebServer.Utility.JWT;
using FaceWebServer.Utility.Middleware;
using FaceWebServer.Utility.Model;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MQTTnet;
using MQTTnet.AspNetCore;
using MQTTnet.Server;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using System;
using System.IO;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace DeviceProtocolServer
{
    public class Startup
    {
        ILogger<Startup> _ilogger;
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        public static bool MQTT_CertificateValidation(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            Console.WriteLine("开始验证客户端证书");

            if (certificate == null)
            {

                Console.WriteLine("客户端证书为空");
                return false;
            }

            // 加载 CA 证书
            var caCertPath = "E:\\OpenSSL\\pem\\ca.pem";
            var caCert = new X509Certificate2(caCertPath);
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
                Console.WriteLine("证书链验证失败");
                foreach (var chainStatus in chain.ChainStatus)
                {
                    Console.WriteLine($"链状态: {chainStatus.StatusInformation}");
                }
                return false;
            }

            // 验证证书的有效期
            if (DateTime.Now < clientCertificate.NotBefore || DateTime.Now > clientCertificate.NotAfter)
            {
                Console.WriteLine("证书不在有效期内");
                return false;
            }

            // 验证证书的主题
            string expectedIssuer = "CN=WebCA"; // 替换为预期的颁发者名称
            if (!clientCertificate.Issuer.Contains(expectedIssuer))
            {
                Console.WriteLine($"证书颁发者不匹配: {clientCertificate.Issuer}");
                return false;
            }
            Console.WriteLine($"客户端证书颁发者: {clientCertificate.Issuer}");
            Console.WriteLine($"客户端证书身份: {clientCertificate.Subject}");
            Console.WriteLine("证书验证成功");
            return true;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            #region Sqlite
            string sFile = Path.Combine(Directory.GetCurrentDirectory(), "DB", "HTTPTestServer.DB");
            string connecttext = $"Filename={sFile}";

            services.AddDbContext<DbContext, FaceDBContext>(options => options.UseSqlite(connecttext,
                (sqliteOptionsAction) =>
                {
                    //sqliteOptionsAction.MaxBatchSize(50000);

                }));
            #endregion

            #region SQLServer
            //string connecttext = @"Data Source=127.0.0.1,1433;Initial Catalog=HTTPTestServerDB;;User ID=sa;Pwd=123";
            //services.AddDbContext<DbContext, FaceDBContext>(options => 
            //    options.UseSqlServer(connecttext, op => 
            //        op.MigrationsAssembly("FaceWebServer.DB")));
            #endregion

            //启用内存缓存
            services.AddMemoryCache();

            services.AddHttpClient();

            services.AddLanguage(Configuration.GetSection("LanguageHandler"));
            services.AddScoped<GlobalExceptionFilter>();

            services.AddMvcCore(options =>
            {
                options.Filters.Add<GlobalExceptionFilter>();
            });

            //#region 压缩
            //services.AddResponseCompression(options =>
            //{
            //    options.Providers.Add<BrotliCompressionProvider>();
            //    options.Providers.Add<GzipCompressionProvider>();
            //    options.EnableForHttps = true;
            //});
            //#endregion

            #region NewtonsoftJson

            services.AddControllers().AddNewtonsoftJson(options =>
            {
                //修改属性名称的序列化方式，
                options.SerializerSettings.ContractResolver = new DefaultContractResolver(); //不改变字母大小写，以字段定义的格式为准
                //options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();//首字母小写

                //修改时间的序列化方式
                options.SerializerSettings.Converters.Add(new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                //空字段忽略
                options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
            });
            #endregion

            #region Swagger
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "设备协议调试器 v1.0", Version = "v1" });
            });
            #endregion

            #region JWT

            #region 读取证书
            string path = Path.Combine(Directory.GetCurrentDirectory(), "client-rsa.pfx");
            X509Certificate2 x509 = new X509Certificate2(path, "YunPC61006535");

            var publicRSA = x509.PublicKey.GetRSAPublicKey();
            var rsaKey = new RsaSecurityKey(publicRSA);
            #endregion

            #region JWT Server RS256
            JWTTokenOptions.X509 = x509;
            services.AddScoped<IJWTService, JWTRSService>();
            services.Configure<JWTTokenOptions>(Configuration.GetSection("JWTTokenOptions"));
            #endregion


            #region JWT校验  RS

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,//是否验证Issuer
                    ValidateAudience = true,//是否验证Audience
                    ValidateLifetime = false,//是否验证失效时间
                    ValidateIssuerSigningKey = true,//是否验证SecurityKey
                    ValidAudience = Configuration["Audience"],//Audience
                    ValidIssuer = Configuration["Issuer"],//Issuer，这两项和前面签发jwt的设置一致
                    IssuerSigningKey = rsaKey,
                    //IssuerSigningKeyValidator = (m, n, z) =>
                    // {
                    //     Console.WriteLine("This is IssuerValidator");
                    //     return true;
                    // },
                    //IssuerValidator = (m, n, z) =>
                    // {
                    //     Console.WriteLine("This is IssuerValidator");
                    //     return "http://localhost:3698";
                    // },
                    //AudienceValidator = (m, n, z) =>
                    //{
                    //    Console.WriteLine("This is AudienceValidator");
                    //    return true;
                    //    //return m != null && m.FirstOrDefault().Equals(this.Configuration["Audience"]);
                    //},//自定义校验规则，可以新登录后将之前的无效
                };
                options.Events = new JwtBearerEvents();
                options.Events.OnChallenge += jwt_OnChallenge;
            });

            #endregion
            #endregion

            services.Configure<FaceDBOption>(Configuration.GetSection("DBOptions"));

            services.Configure<HTTPProtocolOption>(Configuration.GetSection("HTTPProtocolOptions"));
            services.AddAutoAccessTestService(Configuration.GetSection("AutoAccessTestOptions"));


            #region MQTT
            services.AddMQTTHostService(Configuration.GetSection("MQTTOptions"));

            #endregion
        }

        private async Task jwt_OnChallenge(JwtBearerChallengeContext arg)
        {
            arg.HandleResponse();

            _ilogger.LogInformation($" {arg.Request.Path} JWT 验证失败 {arg.AuthenticateFailure}");

            var result = new JsonResultModel(401, $"Unauthorized,{arg.Error},{arg.ErrorDescription}");
            var rst = arg.Response;
            rst.StatusCode = StatusCodes.Status200OK;
            rst.ContentType = "application/json; charset=utf-8";
            await rst.WriteAsync(JsonConvert.SerializeObject(result));
        }



        public void ConfigureContainer(ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterModule<CustomAutofacModule>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory factory, IServiceProvider provider)
        {
            _ilogger = factory.CreateLogger<Startup>();
            _ilogger.LogInformation($"设备HTTP服务器已启动");

            _ilogger.LogInformation($"服务器根目录：{Directory.GetCurrentDirectory()}");

            //app.ApplicationServices.GetService<Startup>();


            #region "初始化缓存--缓存预热"
            _ilogger.LogInformation($"开始初始化缓存");
            using (var iDBCache = provider.GetService<ICacheService>())
            {
                iDBCache.IniSystemCache();
            }
            _ilogger.LogInformation($"缓存预热完毕");
            #endregion

            #region 压缩
            app.UseMiddleware<RequestCompressMiddleware>();
            //app.UseResponseCompression();
            #endregion


            #region 接口日志记录

            app.Use(next =>//接口日志记录
            {
                return new RequestDelegate(async context =>
                {
                    var req = context.Request;

                    if(req.Path != "/api/Device/GetDeviceOnlineStatus")
                        _ilogger.LogInformation($"{req.Method} URL:{req.Path}  ");
                    await next.Invoke(context);
                });
            });
            #endregion


            //设备接口请求日志
            app.UseMiddleware<FaceAPISoftwareDownloadLogMiddleware>();
 


            var staticOption = new StaticFileOptions();
            var contentTypeProvider = new FileExtensionContentTypeProvider();
            contentTypeProvider.Mappings.Add(".pkg", "application/octet-stream");
            //contentTypeProvider.Mappings.Add(".bin", "application/octet-stream");
            staticOption.OnPrepareResponse += HttpImageFileUtil.AddImageMD5Head;
            staticOption.ContentTypeProvider = contentTypeProvider;
            app.UseStaticFiles(staticOption);
            //app.UseStaticFiles(new StaticFileOptions() { 
            //    FileProvider = new PhysicalFileProvider(Directory.GetCurrentDirectory()),
            //    RequestPath=new PathString("/wwwroot")
            //});

            app.UseWebSockets();
            //app.UseMiddleware<EchoWebSocketMiddleware>();
            app.UseMiddleware<ExceptionMiddleware>();

            ////固件下载日志


            //设备接口请求日志
            app.UseMiddleware<FaceHTTPv1APILogResourceMiddleware>();
            app.UseMiddleware<FaceHTTPv2APILogResourceMiddleware>();
            

            app.UseRouting();


            //// Setup MQTT stuff.
            //app.UseEndpoints(endpoints =>
            //{
            //    endpoints.MapMqtt("/mqtt");
            //});


            #region  JWT
            app.UseAuthentication();//鉴权：解析信息--就是读取token，解密token
            app.UseAuthorization();
            app.UseUserRegister();
            #endregion



            app.UseLanguage();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });


            #region 网站默认页
            //DefaultFilesOptions options = new DefaultFilesOptions();
            //options.DefaultFileNames.Add("index.html");    //将index.html改为需要默认起始页的文件名.
            //app.UseDefaultFiles(options);

            app.Run(ctx =>
            {
                if (ctx.Request.Path == "/")
                {
                    ctx.Response.Redirect("/index.html");
                }
                else
                {
                    ctx.Response.StatusCode = 404;
                }


                return Task.FromResult(0);
            });
            #endregion



        }
    }
}
