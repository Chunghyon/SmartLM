using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FaceWebServer.Utility.Middleware
{
    public static class UserRegisterExtensions
    {
        public static IApplicationBuilder UseUserRegister(this IApplicationBuilder app)
        {
            app.UseMiddleware<UserRegisterMiddleware>();
            return app;
        }
    }
}
