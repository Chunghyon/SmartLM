using FaceWebServer.DB.Table;
using FaceWebServer.Utility.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace FaceWebServer.Utility.Middleware
{
    public class UserRegisterMiddleware
    {
        private readonly RequestDelegate next;

        public UserRegisterMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            if (context.User != null && context.User.Identity != null)
            {
                var ident = context.User.Identities.First();
                if(ident.Claims.Count() >0)
                {
                    UserDetail user = context.RequestServices.GetService<UserDetail>();
                    user.UserID = int.Parse(ident.Claims.Where(x => x.Type.Equals("ID")).First().Value);
                    user.UserName = ident.Claims.Where(x => x.Type.Equals("name")).First().Value;
                    user.LogTime = TimestampUtility.ToLocalTimeDateBySeconds(long.Parse(ident.Claims.Where(x => x.Type.Equals("iat")).First().Value));
                    user.OnlineTime = DateTime.Now;//TimestampUtility.ToLocalTimeDateBySeconds(long.Parse(ident.Claims.Where(x => x.Type.Equals("exp")).First().Value)).AddMinutes(-60);
                }
                
            }

            await next.Invoke(context);




        }

    }
}
