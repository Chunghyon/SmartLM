using FaceWebServer.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Collections.Generic;
using System;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Serialization;
using FaceWebServer.DB.Table;
using FaceWebServer.DTO.Config;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace DeviceProtocolServer.Middleware
{
    public class FaceAPISoftwareDownloadLogMiddleware
    {
        private readonly RequestDelegate next;

        HashSet<string> Headerfilter = new HashSet<string>();


        public FaceAPISoftwareDownloadLogMiddleware(RequestDelegate next)
        {
            Headerfilter.Add("Cache-Control");
            Headerfilter.Add("Connection");
            Headerfilter.Add("Pragma");
            Headerfilter.Add("Accept");
            Headerfilter.Add("Accept-Language");
            //Headerfilter.Add("User-Agent");
            //Headerfilter.Add("ClientID");

            this.next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            var httpConfig = context.RequestServices.GetService<IOptionsMonitor<HTTPProtocolOption>>();
            if(httpConfig.CurrentValue.SaveIOLog == false)
            {
                await next.Invoke(context);
                return;
            }

            var req = context.Request;
            var sHost = string.Empty;
            var SN = string.Empty;
            if (req.Headers.ContainsKey("ClientID"))
                SN = sHost = req.Headers["ClientID"].ToString();
            if (req.Headers.ContainsKey("SN"))
                SN = sHost = req.Headers["SN"].ToString();

            if (req.Headers.ContainsKey("Host"))
                sHost = req.Headers["Host"].ToString();
            var sURL = req.Path.Value;
            if (sURL.StartsWith("/softpkg") && sURL.Contains(".pkg"))
            {
                Dictionary<string, string> headers = new Dictionary<string, string>();

                //Headerfilter.Add("Host");
                foreach (var item in req.Headers.Keys)
                {
                    if (!Headerfilter.Contains(item))
                        headers.Add(item, req.Headers[item].ToString());
                }

                IConnectIOLogService log = (IConnectIOLogService)context.RequestServices.GetService(typeof(IConnectIOLogService));

                string APIName = string.Empty;
                string IPAddr = string.Empty;
                string URL = string.Empty;
                var http = context;
                var ret = http.Request;
                URL = ret.Path;

                APIName = "下载固件";
                var sAPIPath = URL;

                if (ret.Headers.ContainsKey("X-Forwarded-For"))
                {
                    IPAddr = ret.Headers["X-Forwarded-For"].ToString();
                }
                else
                    IPAddr = $"{http.Connection.RemoteIpAddress}:{http.Connection.RemotePort}";
                ConnectIOLog RequestLog = new ConnectIOLog()
                {
                    Protocol = "HTTP",
                    HttpType = "Request",
                    IPAddr = IPAddr,
                    LogTime = DateTime.Now,
                    APIName = APIName,
                    URL = $"{sHost}{URL}",
                    Method = ret.Method,
                    ContentLength = 0,
                    ContentType = string.Empty,
                    Body = string.Empty,
                    SN = SN,
                    RequestID = string.Empty,
                };
                RequestLog.Body = JsonConvert.SerializeObject(headers);
                log.AddConnectLog(RequestLog);
            }

            await next.Invoke(context);

            //context.Response
        }
    }






}
