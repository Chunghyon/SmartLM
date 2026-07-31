using FaceWebServer.Utility.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace FaceWebServer.Utility.FilterAttribute
{
    /// <summary>
    /// 自定义的错误处理工具
    /// </summary>
    public class CustomExceptionActionFilterAttribute : ExceptionFilterAttribute
    {
        private readonly ILogger<CustomExceptionActionFilterAttribute> _logger;

        public CustomExceptionActionFilterAttribute(
            ILogger<CustomExceptionActionFilterAttribute> logger)
        {
            _logger = logger;
        }


        /// <summary>
        /// 没有处理的异常，就会进来
        /// </summary>
        /// <param name="filterContext"></param>
        public override void OnException(ExceptionContext filterContext)
        {
            if (!filterContext.ExceptionHandled)//异常有没有被处理过
            {
                string controllerName = (string)filterContext.RouteData.Values["controller"];
                string actionName = (string)filterContext.RouteData.Values["action"];
                string msgTemplate = $"在执行 controller[{controllerName}] 的 action[{actionName}] 时产生异常 {filterContext.Exception}";
                _logger.LogError(msgTemplate);


                var result = new JsonResultModel(10101, filterContext.Exception.Message);
                ContentResult cResult = new ContentResult
                {
                    StatusCode = 200,
                    Content = JsonConvert.SerializeObject(result),
                    ContentType = "application/json;charset=utf-8;"
                };

                filterContext.Result = cResult;
                filterContext.ExceptionHandled = true;
            }
        }


    }
}
