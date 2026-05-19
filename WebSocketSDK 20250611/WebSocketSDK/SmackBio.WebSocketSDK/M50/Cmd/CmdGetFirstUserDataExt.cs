using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using SmackBio.WebSocketSDK.Cmd;

namespace SmackBio.WebSocketSDK.M50.Cmd
{
    public class CmdGetFirstUserDataExt : CmdGetNextUserDataExt
    {
        public CmdGetFirstUserDataExt() 
            : base(0)
        { 
        }
    }
}
