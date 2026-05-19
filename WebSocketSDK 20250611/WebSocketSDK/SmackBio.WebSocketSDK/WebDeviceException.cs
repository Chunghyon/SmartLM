using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmackBio.WebSocketSDK
{
    public class WebDeviceException : Exception
    {
        public WebDeviceException(string message)
            : base(message)
        { }
        public WebDeviceException(string message, Exception ex)
            : base(message, ex)
        { }
    }
}
