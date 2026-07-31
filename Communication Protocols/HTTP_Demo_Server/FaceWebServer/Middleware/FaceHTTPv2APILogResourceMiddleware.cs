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
using DoNetDrive.Common.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.IO.Compression;
using FaceWebServer.DB.Table;
using NPOI.XWPF.UserModel;
using System.Diagnostics.Eventing.Reader;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using FaceWebServer.DTO.Config;
using Microsoft.Extensions.Options;

namespace DeviceProtocolServer.Middleware
{
    public class FaceHTTPv2APILogResourceMiddleware
    {
        private Snowflake.Core.IdWorker RequestWorker;
        private readonly Dictionary<string, string> PathNames;
        private HashSet<string> Headerfilter = new HashSet<string>();
        private readonly RequestDelegate next;

        private static JsonSerializerSettings JsonSettings = new JsonSerializerSettings()
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver(),
            DateFormatString = "yyyy-MM-dd HH:mm:ss",
            NullValueHandling = NullValueHandling.Ignore

        };



        private class APIContext
        {
            public IConnectIOLogService LogDB;
            public string APIName = string.Empty;
            public string IPAddr = string.Empty;
            public string URL = string.Empty;
            public string DeviceSN = string.Empty;
            public string RequestID = string.Empty;
        }


        public FaceHTTPv2APILogResourceMiddleware(RequestDelegate next)
        {
            RequestWorker = new Snowflake.Core.IdWorker(1, 1);
            PathNames = new Dictionary<string, string>();
            PathNames.Add("/Device/Keepalive", "心跳保活包");

            PathNames.Add("/Device/UploadWorkSetting", "上传工作参数");
            PathNames.Add("/Device/DownloadWorkSetting", "拉取工作参数");

            PathNames.Add("/Device/RemoteCommand", "获取远程操作指令");
            PathNames.Add("/Device/RegisterIdentifyTicketResult", "反馈人员注册凭证的结果");
            PathNames.Add("/Device/UploadSnapshoot", "上传设备摄像头快照");

            PathNames.Add("/People/DownloadPeopleList", "拉取待下载的人员");
            PathNames.Add("/People/DownloadPeopleListResult", "反馈下载的人员存储结果");

            PathNames.Add("/People/DeletePeopleList", "拉取待删除人员名单");
            PathNames.Add("/People/DeletePeopleListResult", "反馈删除人员结果");

            PathNames.Add("/People/PushPeople", "推送人员信息");
            PathNames.Add("/People/RequestAuthorization", "请求服务器鉴权");

            PathNames.Add("/Record/UploadIdentifyRecord", "推送打卡记录");
            PathNames.Add("/Record/UploadSystemRecord", "推送系统记录");


            Headerfilter.Add("Cache-Control");
            Headerfilter.Add("Connection");
            Headerfilter.Add("Pragma");
            Headerfilter.Add("Accept");
            Headerfilter.Add("Accept-Language");
            Headerfilter.Add("User-Agent");

            this.next = next;
        }


        public async Task Invoke(HttpContext context)
        {
            var httpConfig = context.RequestServices.GetService<IOptionsMonitor<HTTPProtocolOption>>();
            if (httpConfig.CurrentValue.SaveIOLog == false)
            {
                await next.Invoke(context);
                return;
            }

            var Request = context.Request;
            bool bExecuting = false;
            APIContext api = null;
            var sAPIPath = Request.Path.Value;
            if (PathNames.ContainsKey(sAPIPath))
            {
                bExecuting = true;
            }
            if (bExecuting)
            {

                IConfiguration conf = context.RequestServices.GetService<IConfiguration>();
                if (!httpConfig.CurrentValue.SaveKeepaliveLog)
                {
                    if (sAPIPath.Contains("Keepalive"))
                    {
                        bExecuting = false;
                    }
                }
            }

            Stream ResponseBody = context.Response.Body;
            if (bExecuting)
            {
                api = new APIContext()
                {
                    LogDB = (IConnectIOLogService)context.RequestServices.GetService(typeof(IConnectIOLogService))
                };

                OnResourceExecuting(context, api);

                context.Response.Body =new MemoryStream();
            }
            try
            {
                await next.Invoke(context);

                if (bExecuting)
                {
                    await OnResourceExecuted(context, api);
                    var memStream =(MemoryStream) context.Response.Body;
                    var bdata = memStream.GetBuffer();
                    await ResponseBody.WriteAsync(bdata, 0, (int)memStream.Length);
                    context.Response.Body = ResponseBody;
                    memStream.Dispose();
                }
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync(ex.Message);
                if (bExecuting)
                {
                    var memStream = (MemoryStream)context.Response.Body;
                    var bdata = memStream.GetBuffer();
                    await ResponseBody.WriteAsync(bdata, 0, (int)memStream.Length);
                    context.Response.Body = ResponseBody;
                    memStream.Dispose();
                }

                throw;
            }
            

        }

        private async Task OnResourceExecuted(HttpContext context, APIContext api)
        {
            //处理执行其他中间件后的ResponseBody
            var http = context;
            var Response = http.Response;
            var ret = http.Request;


            ConnectIOLog ResponseLog = new ConnectIOLog()
            {
                Protocol = DeviceDetail.HTTPv2,
                APIName = api.APIName,
                HttpType = "Response",
                IPAddr = api.IPAddr,
                LogTime = DateTime.Now,
                URL =  api.URL,
                Method = ret.Method,
                ContentLength = 0,
                ContentType = "application/json; charset=utf-8",
                Body = string.Empty,
                SN = api.DeviceSN,
                RequestID = api.RequestID
            };

            //var obj = JsonConvert.SerializeObject(Result, JsonSettings);

            if (!Response.ContentLength.HasValue)
            {
                try
                {
                    Response.ContentLength = Response.Body.Length;
                }
                catch (Exception ex)
                {
                    //Console.WriteLine(ex.Message);
                    ;
                }
                
            }
            Dictionary<string, string> headers = new Dictionary<string, string>();

            //Headerfilter.Add("Host");
            foreach (var item in Response.Headers.Keys)
            {
                if (!Headerfilter.Contains(item))
                    headers.Add(item, Response.Headers[item].ToString());
            }

            Dictionary<string, object> body = new Dictionary<string, object>();
            body.Add("Headers", headers);

            string responseBody = string.Empty;
            Response.Body.Position = 0;
            using (var responseReader = new StreamReader(Response.Body, System.Text.Encoding.UTF8, true, -1, true))
            {
                responseBody = await responseReader.ReadToEndAsync();
            }
            Response.Body.Position = 0;
            body.Add("Body", JsonConvert.DeserializeObject(responseBody));


            ResponseLog.Body = JsonConvert.SerializeObject(body, JsonSettings);
            if (Response.ContentLength.HasValue)
                ResponseLog.ContentLength = (int)Response.ContentLength.Value;

            api.LogDB.AddConnectLog(ResponseLog);
        }

        private void OnResourceExecuting(HttpContext context, APIContext api)
        {
            var http = context;
            var ret = http.Request;
            api.URL = ret.Path;


            var sAPIPath = api.URL;

            if (PathNames.ContainsKey(sAPIPath))
            {
                api.APIName = PathNames[sAPIPath];
            }
            Dictionary<string, string> headers = new Dictionary<string, string>();

            //Headerfilter.Add("Host");
            foreach (var item in ret.Headers.Keys)
            {
                if (!Headerfilter.Contains(item))
                    headers.Add(item, ret.Headers[item].ToString());
            }


            ret.EnableBuffering();
            api.RequestID = RequestWorker.NextId().ToString();

            //X-Forwarded-For
            if (ret.Headers.ContainsKey("X-Forwarded-For"))
            {
                api.IPAddr = ret.Headers["X-Forwarded-For"].ToString();
            }
            else
                api.IPAddr = $"{http.Connection.RemoteIpAddress}:{http.Connection.RemotePort}";
            var sHost = string.Empty;
            if (ret.Headers.ContainsKey("Host"))
                sHost = ret.Headers["Host"].ToString();
            api.URL = $"{sHost}{api.URL}";
            ConnectIOLog RequestLog = new ConnectIOLog()
            {
                Protocol = DeviceDetail.HTTPv2,
                APIName = api.APIName,
                HttpType = "Request",
                IPAddr = api.IPAddr,
                LogTime = DateTime.Now,
                URL = api.URL,
                Method = ret.Method,
                ContentLength = 0,
                ContentType = string.Empty,
                Body = string.Empty,
                SN = api.DeviceSN,
                RequestID = api.RequestID
            };

            if (ret.ContentLength.HasValue)
            {
                RequestLog.ContentType = ret.ContentType;
                RequestLog.ContentLength = (int)ret.ContentLength.Value;

                if (RequestLog.ContentType.StartsWith("multipart/form-data"))
                {
                    var sBuf = new System.Text.StringBuilder();

                    try
                    {
                        sBuf.Append("{");

                        foreach (var key in ret.Form.Keys)
                        {
                            string sValue = ret.Form[key];
                            if(key == "SN")
                            {
                                api.DeviceSN = sValue;
                                RequestLog.SN = api.DeviceSN;
                            }
                            if (key == "DeviceSN")
                            {
                                api.DeviceSN = sValue;
                                RequestLog.SN = api.DeviceSN;
                            }

                            if (sValue.StartsWith("{") || sValue.StartsWith("["))
                            {
                                sBuf.Append("\"").Append(key).Append("\":").Append(sValue).Append(",");
                            }
                            else
                                sBuf.Append("\"").Append(key).Append("\":\"").Append(sValue).Append("\",");
                        }

                        foreach (var file in ret.Form.Files)
                        {
                            sBuf.Append("\"").Append(file.Name).Append("\":{")
                                .Append("\"Name\" :\"").Append(file.FileName).Append("\",")
                                .Append("\"Length\" :\"").Append(file.Length).Append("\",")
                                .Append("\"Header\" :").Append(JsonConvert.SerializeObject(file.Headers)).Append(",")
                                .Append("\"FileName\" :\"").Append(file.FileName).Append("\"");


                            if (file.Headers.ContainsKey("Content-Encoding"))
                            {
                                string sJson = GzipDecompressedAsync(file).Result;
                                sBuf.Append(",\"Body\" :").Append(sJson);
                            }
                            sBuf.Append("},");
                        }
                        sBuf.Length -= 1;
                        sBuf.Append("}");

                        RequestLog.Body = sBuf.ToString();
                    }
                    catch (Exception ex)
                    {

                        sBuf.Append(ex.ToString());
                    }


                }
                else
                {
                    try
                    {
                        http.Request.Body.Position = 0;
                        using (var requestReader = new StreamReader(ret.Body, encoding: System.Text.Encoding.UTF8, leaveOpen: true))
                        {
                            var requestContent = requestReader.ReadToEndAsync().Result;
                            RequestLog.Body = requestContent;
                            if (!string.IsNullOrEmpty(requestContent))
                            {
                                try
                                {
                                    //尝试读取
                                    var dic = JsonConvert.DeserializeObject<Dictionary<string, object>>(requestContent);
                                    if (dic.ContainsKey("SN"))
                                    {
                                        api.DeviceSN = dic["SN"].ToString();
                                        RequestLog.SN = api.DeviceSN;
                                    }
                                    if (dic.ContainsKey("DeviceSN"))
                                    {
                                        api.DeviceSN = dic["DeviceSN"].ToString();
                                        RequestLog.SN = api.DeviceSN;
                                    }
                                }
                                catch (Exception)
                                {


                                }
                            }

                        }
                    }
                    catch (Exception ex)
                    {

                        RequestLog.Body = $"{{\"err\":\"读取Body时发生错误！{ex.ToString()}\"}}";
                    }

                    http.Request.Body.Position = 0;
                }
            }

            if (!string.IsNullOrEmpty(RequestLog.Body))
            {
                System.Text.StringBuilder sBuf = new System.Text.StringBuilder();
                sBuf.Append("{\"Header\":").Append(JsonConvert.SerializeObject(headers));
                sBuf.Append(",\"Body\":").Append(RequestLog.Body).Append("}");
                RequestLog.Body = sBuf.ToString();
            }
            api.LogDB.AddConnectLog(RequestLog);

        }

        private async Task<string> GzipDecompressedAsync(IFormFile ifile)
        {
            string sValue = string.Empty;
            if (ifile.Headers.ContainsKey("Content-Encoding"))
            {
                var encoding = ifile.Headers["Content-Encoding"].ToString();
                if ("gzip".Equals(encoding))
                {
                    try
                    {
                        using MemoryStream compressedStream = new MemoryStream();
                        using MemoryStream decompressedStream = new MemoryStream();
                        await ifile.CopyToAsync(compressedStream);
                        compressedStream.Position = 0;
                        using var gzipStream = new GZipStream(compressedStream, CompressionMode.Decompress);

                        await gzipStream.CopyToAsync(decompressedStream);
                        decompressedStream.Position = 0;
                        using var requestReader = new StreamReader(decompressedStream, encoding: System.Text.Encoding.UTF8, leaveOpen: true);
                        sValue = await requestReader.ReadToEndAsync();
                    }
                    catch (Exception)
                    {

                        sValue = string.Empty;
                    }


                }
            }
            return sValue;
        }
    }






}
