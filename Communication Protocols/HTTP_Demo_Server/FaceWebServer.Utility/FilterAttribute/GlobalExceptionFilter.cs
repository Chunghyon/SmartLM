using FaceWebServer.Utility.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;

namespace FaceWebServer.Utility.FilterAttribute
{
    public class GlobalExceptionFilter : Attribute, IExceptionFilter
    {
        private readonly ILogger<GlobalExceptionFilter> _logger;

        public GlobalExceptionFilter(
            ILogger<GlobalExceptionFilter> logger)
        {
            _logger = logger;
        }
        /// <summary>
        /// 发生异常进入
        /// </summary>
        /// <param name="context"></param>
        public void OnException(ExceptionContext context)
        {
            if (!context.ExceptionHandled)//异常有没有被处理过
            {

                string controllerName = (string)context.RouteData.Values["controller"];
                string actionName = (string)context.RouteData.Values["action"];
                string msgTemplate = $"在执行 controller[{controllerName}] 的 action[{actionName}] 时产生异常 {context.Exception}";
                _logger.LogError(msgTemplate);


                var result = new JsonResultModel(10101, context.Exception.Message);
                ContentResult cResult = new ContentResult
                {
                    StatusCode = 200,
                    Content = JsonConvert.SerializeObject(result),
                    ContentType = "application/json;charset=utf-8;"
                };

                context.Result = cResult;
                context.ExceptionHandled = true;
            }
        }
    }
}
