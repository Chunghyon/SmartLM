using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Microsoft.Web.WebSockets;

namespace SmackBio.WebSocketSDK
{
    public class GenericHandler : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            if (context.IsWebSocketRequest || context.IsWebSocketRequestUpgrading)
            {
                context.AcceptWebSocketRequest(new SBWebSocketHandler());
            }
            else
            {
                context.Response.ContentType = "text/plain";
                context.Response.Write("Service running");
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}
