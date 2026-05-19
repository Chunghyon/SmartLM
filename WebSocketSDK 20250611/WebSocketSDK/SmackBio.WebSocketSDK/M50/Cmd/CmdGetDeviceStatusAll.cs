using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using SmackBio.WebSocketSDK.Cmd;

namespace SmackBio.WebSocketSDK.M50.Cmd
{
    public class CmdGetDeviceStatusAll : CmdBase
    {
        public const string MSG_KEY = "GetDeviceStatusAll";

        public CmdGetDeviceStatusAll()
            : base()
        {
        }

        public override string Build()
        {
            string result = StartBuild();
            AppendTag(ref result, TAG_REQUEST, MSG_KEY);
            AppendEndup(ref result);

            return result;
        }

        public override Type GetResponseType()
        {
            return typeof(CmdGetDeviceStatusAllResponse);
        }
    }

    public class CmdGetDeviceStatusAllResponse : Response
    {
        public string result_str;

        public override bool Parse(XmlDocument doc)
        {
            result_str = doc.InnerXml;
            return true;
        }
    }
}
