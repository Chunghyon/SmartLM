using FaceWebServer.DB.Table;
using FaceWebServer.Interface;
using FaceWebServer.Utility;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using NPOI.SS.Formula.Functions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace DeviceProtocolServer.Utilities
{
    public class HttpImageFileUtil
    {
        private static HashSet<string> Headerfilter = new HashSet<string>();
        private static Random random = new Random();
        static HttpImageFileUtil()
        {
            Headerfilter.Add("Cache-Control");
            Headerfilter.Add("Connection");
            Headerfilter.Add("Pragma");
            Headerfilter.Add("Accept");
            Headerfilter.Add("Accept-Language");
            //Headerfilter.Add("User-Agent");
            //Headerfilter.Add("Accept-Encoding");
            Headerfilter.Add("Host");
            //Headerfilter.Add("ClientID");
        }

        /// <summary>
        /// 为图片类型的数据返回时增加 Content-MD5 head头
        /// </summary>
        /// <param name="FileResponseContext"></param>
        public static void AddImageMD5Head(StaticFileResponseContext FileResponseContext)
        {
            var file = FileResponseContext.File;

            if (!file.Exists)
            {
                FileResponseContext.Context.Response.StatusCode = 404;

                return;
            }

            if (file.Exists &&
            file.IsDirectory == false &&
            file.Length > 0)
            {
                string md5;
                var fileExtension = Path.GetExtension(file.PhysicalPath);
                if (fileExtension == ".jpg")
                {
                    var Context = FileResponseContext.Context;
                    var cache = Context.RequestServices.GetService<IMemoryCache>();
                    var key = MD5Helper.GetStringMD5ByBase64($"{file.PhysicalPath}_{file.LastModified}_{file.Length}");
                    var md5Cache = cache.Get<string>(key);
                    if (md5Cache != null)
                    {
                        FileResponseContext.Context.Response.Headers.Add("Content-MD5", md5Cache);
                        md5 = md5Cache;
                    }
                    else
                    {
                        md5 = MD5Helper.GetFileMD5ByBase64(FileResponseContext.File.PhysicalPath);
                        FileResponseContext.Context.Response.Headers.Add("Content-MD5", md5);
                        cache.Set(key, md5, TimeSpan.FromMinutes(30));
                    }
                    //增加日志记录
                    {

                        var context = FileResponseContext.Context;

                        var http = context;
                        var ret = http.Request;
                        var sHost = string.Empty;
                        var SN = string.Empty;
                        if (ret.Headers.ContainsKey("ClientID"))
                            SN = sHost = ret.Headers["ClientID"].ToString();
                        if (ret.Headers.ContainsKey("SN"))
                            SN = sHost = ret.Headers["SN"].ToString();
                        //if(!string.IsNullOrEmpty(SN))
                        //{
                        //    if(random.Next(1,100)>50)
                        //    {
                        //        Thread.Sleep(60_000);
                        //    }

                        //}

                        if (ret.Headers.ContainsKey("Host"))
                            sHost = ret.Headers["Host"].ToString();

                        string URL = ret.Path;
                        if (!URL.Contains("People"))
                        {
                            return;
                        }

                        string APIName = "下载图片";

                        string IPAddr;
                        if (ret.Headers.ContainsKey("X-Forwarded-For"))
                        {
                            IPAddr = ret.Headers["X-Forwarded-For"].ToString();
                        }
                        else
                            IPAddr = $"{http.Connection.RemoteIpAddress}:{http.Connection.RemotePort}";



                        //获取url参数

                        Dictionary<string, string> Querys = new Dictionary<string, string>();
                        foreach (var item in ret.Query.Keys)
                        {
                            Querys.Add(item, ret.Query[item].ToString());
                        }
                        //获取请求头

                        Dictionary<string, string> Headers = new Dictionary<string, string>();

                        //Headerfilter.Add("Host");
                        foreach (var item in ret.Headers.Keys)
                        {
                            if (!Headerfilter.Contains(item))
                                Headers.Add(item, ret.Headers[item].ToString());
                        }


                        ConnectIOLog RequestLog = new ConnectIOLog()
                        {
                            Protocol = DeviceDetail.HTTPv1,
                            HttpType = "Request",
                            IPAddr = IPAddr,
                            LogTime = DateTime.Now,
                            APIName = APIName,
                            URL = $"{sHost}{URL}",
                            Method = ret.Method,
                            ContentLength = 0,
                            ContentType = string.Empty,
                            //Body =$"照片存在，大小：{file.Length} md5:{md5} {JsonConvert.SerializeObject( ret.Query)}" ,
                            Body = JsonConvert.SerializeObject(new
                            {
                                FileSize = file.Length,
                                FileMD5 = md5,
                                Querys,
                                Headers

                            }, Formatting.Indented),
                            SN = SN,
                            RequestID = string.Empty,
                        };
                        IConnectIOLogService log = (IConnectIOLogService)context.RequestServices.GetService(typeof(IConnectIOLogService));
                        log.AddConnectLog(RequestLog);
                    }
                }
            }
        }
    }
}
