using FaceWebServer.Language;
using FaceWebServer.Utility.Model;
using FaceWebServer.Utility.VerifyAttribute;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FaceWebServer.Utility.FilterAttribute
{
    /// <summary>
    /// 参数验证过滤器
    /// </summary>
    public class VerifyActionFilterAttribute : Attribute, IActionFilter
    {
        private ILogger<VerifyActionFilterAttribute> _logger = null;
        private LanguageHandler _LanguageHandler;
        public VerifyActionFilterAttribute(ILogger<VerifyActionFilterAttribute> logger,
            IOptionsSnapshot<LanguageOption> lngopt)
        {
            this._logger = logger;
            _LanguageHandler = lngopt.Value.GetCurrentLanguageHandler();
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            //this?._logger.LogDebug($"{context.HttpContext.Request.Path} ActionFilter 执行后!");
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {

            if (context.ActionArguments.Count > 0)
            {
                var Parameters = context.ActionDescriptor.Parameters;
                var parDic = new Dictionary<string, ParameterDescriptor>();
                foreach (var parDtl in Parameters)
                {
                    parDic.Add(parDtl.Name, parDtl);
                }

                foreach (var ArgumentItem in context.ActionArguments)
                {

                    object parValue = ArgumentItem.Value;
                    var parDtl = parDic[ArgumentItem.Key];
                    Type parType = parDtl.ParameterType;
                    //var parInfo= parDtl.ParameterInfo;

                    if (parType.IsClass)
                    {
                        if (!VerifyEntity(parType, parValue, r => context.Result = r, _LanguageHandler)) return;
                    }
                }

            }
            //this?._logger.LogDebug($"{context.HttpContext.Request.Path} ActionFilter 执行前!");
        }

        public static bool VerifyEntity(Type parType, object parValue,
            Action<IActionResult> VerifyErrorCallblack, LanguageHandler lng)
        {
            foreach (var parFiled in parType.GetProperties())
            {
                if (parFiled.IsDefined(typeof(AbstractVerifyAttribute), true))
                {
                    var attrs = parFiled.GetCustomAttributes(typeof(AbstractVerifyAttribute), true);
                    foreach (var objAttr in attrs)
                    {
                        AbstractVerifyAttribute attr = objAttr as AbstractVerifyAttribute;
                        object pValue = parFiled.GetValue(parValue);
                        if (!attr.Verify(ref pValue))
                        {

                            //给Result 赋值，可以直接阻断Action执行
                            //var result = new JsonResultModel(attr.ErrorCode, $"Filed [{parFiled.Name}] verify error");
                            var sErrMsg = attr.ErrorDescription;
                            var slngMsg = lng.GetCheckParameterErrorMessage(attr.LanguageCode);
                            if (!string.IsNullOrEmpty(slngMsg))
                                sErrMsg = slngMsg;
                            var result = new JsonResultModel(attr.ErrorCode, sErrMsg);

                            VerifyErrorCallblack(new JsonResult(result));
                            return false;
                        }
                        else
                        {
                            object poldValue = parFiled.GetValue(parValue);
                            if(!object.ReferenceEquals(pValue, poldValue))
                            {
                                parFiled.SetValue(parValue, pValue);
                            }

                        }
                    }
                }
            }
            return true;
        }
    }
}
