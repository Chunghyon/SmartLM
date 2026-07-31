using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace FaceWebServer.Language
{
    public class LanguageMiddleware
    {
        private readonly RequestDelegate next;

        public LanguageMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            var reqHeaders = context.Request.Headers;
            LanguageOption option = context.RequestServices.GetService<IOptionsSnapshot<LanguageOption>>().Value;
            if (reqHeaders.ContainsKey("ClientLanguage"))
            {
                var sValue = reqHeaders["ClientLanguage"].FirstOrDefault();

                //检查是否支持此种语言
                var config = option.Languages.Where(x => x.Language == sValue).FirstOrDefault();
                if (config == null)
                {
                    //不支持这种语言
                    option.CurrentLanguage = option.DefaultLanguage;
                }
                else
                {
                    option.CurrentLanguage = sValue;
                }
            }
            else
            {
                option.CurrentLanguage = option.DefaultLanguage;
            }

            await next.Invoke(context);




        }

    }
}
