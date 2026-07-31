using FaceWebServer.Utility.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Threading.Tasks;

namespace FaceWebServer.Utility.Middleware
{
    /// <summary>
    /// 拦截控制器返回非200 OK请求，转换为json 格式返回
    /// </summary>
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate next;
        private readonly ILogger<ExceptionMiddleware> _logger;


        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> log)
        {
            this.next = next;
            _logger = log;
        }

        public async Task Invoke(HttpContext context)
        {

            var ret = context.Request;
            var rep = context.Response;
            string sURL = ret.Path.ToString();
            if (sURL.StartsWith("/api/") || sURL.StartsWith("/FaceAPI/") 
                || sURL.StartsWith("/note/") || sURL.StartsWith("/device/")
                || sURL.StartsWith("/devicePass/") || sURL.StartsWith("/parameter/"))
            {
                var responseOriginalBody = rep.Body;
                try
                {
                    using (var memStream = new MemoryStream())
                    {
                        rep.Body = memStream;
                        await next.Invoke(context);
                        if (context.Response.StatusCode != 200 && context.Response.StatusCode >= 400)
                        {
                            //处理执行其他中间件后的ResponseBody
                            memStream.Position = 0;
                            var responseReader = new StreamReader(memStream);
                            var responseBody = await responseReader.ReadToEndAsync();

                            _logger.LogError($"{ret.Method} URL:{ret.Path} : {context.Response.StatusCode} {responseBody}");


                            var result = new JsonResultModel(10000 + context.Response.StatusCode, responseBody);

                            rep.Body = responseOriginalBody;
                            rep.Clear();
                            rep.StatusCode = 200;
                            rep.ContentType = "application/json; charset=utf-8";
                            var sJson = JsonConvert.SerializeObject(result);
                            //rep.ContentLength = System.Text.Encoding.UTF8.GetByteCount(sJson);
                            await context.Response.WriteAsync(sJson);
                        }
                        else
                        {
                            memStream.Position = 0;
                            await memStream.CopyToAsync(responseOriginalBody);
                            rep.Body = responseOriginalBody;
                            //rep.ContentLength = memStream.Length;
                        }
                    }
                }
                catch (Exception e)
                {
                    rep.Body = responseOriginalBody;
                    await HandleException(context, e);
                }
            }
            else
            {
                await next.Invoke(context);
            }

        }

        private async Task HandleException(HttpContext context, Exception e)
        {
            var req = context.Request;
            var rst = context.Response;
            _logger.LogError($"{req.Method} URL:{req.Path} : {e.ToString()}");
            var result = new JsonResultModel(10500, e.Message);
            rst.Clear();
            rst.StatusCode = 200;
            rst.ContentType = "application/json; charset=utf-8";
            string sJson = JsonConvert.SerializeObject(result);
            //rst.ContentLength = System.Text.Encoding.UTF8.GetByteCount(sJson);
            await context.Response.WriteAsync(sJson);
        }
    }
}
