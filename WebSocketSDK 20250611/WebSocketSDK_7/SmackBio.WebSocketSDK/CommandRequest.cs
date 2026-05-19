using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace SmackBio.WebSocketSDK
{
    struct CommandRequest
    {
        public XmlDocument message;
        public SBWebSocketHandler.Async ar;
    }
}
