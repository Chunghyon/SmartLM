using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FaceWebServer.Language
{
    public static class LanguageExtensions
    {
        /// <summary>
        /// 使用多语言扩展
        /// </summary>
        /// <param name="services"></param>
        /// <param name="languageConfig"></param>
        /// <returns></returns>
        public static IServiceCollection AddLanguage(this IServiceCollection services, IConfiguration languageConfig)
        {
            services.AddOptions();
            services.Configure<LanguageOption>(languageConfig);
            return services;
        }
        /// <summary>
        /// 使用多语言扩展
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseLanguage(this IApplicationBuilder app)
        {
            app.UseMiddleware<LanguageMiddleware>();
            return app;
        }
    }
}
