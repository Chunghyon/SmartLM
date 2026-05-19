using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using SmackBio.WebSocketSDK.DB;

namespace SmackBio.WebSocketSDK.Cmd
{
    public class CmdGetUserMessageColor : CmdBase
    {
        public const string MSG_KEY = "GetUserMessageColor";

        public CmdGetUserMessageColor() : base() { }

        public override string Build()
        {
            string result = StartBuild();
            AppendTag(ref result, TAG_REQUEST, MSG_KEY);
            AppendEndup(ref result);

            return result;
        }

        public override Type GetResponseType()
        {
            return typeof(CmdGetUserMessageColorResponse);
        }
    }

    public class CmdGetUserMessageColorResponse : Response 
    {
        public uint message_color = 0;
        public uint message_bk_color = 0;

        public CmdGetUserMessageColorResponse() { }

        public override bool Parse(XmlDocument doc)
        {
            bool ret = base.Parse(doc);
            base.ParseResult(doc);
            if (!ret || result != CommandExeResult.OK)
                return false;

            var str = ParseTag(doc, "MessageColor");
            if (str != null)
                message_color = Convert.ToUInt32(str, 16) & 0xFFFFF;

            str = ParseTag(doc, "MessageBkColor");
            if (str != null)
                message_bk_color = Convert.ToUInt32(str, 16) & 0xFFFFF;

            return true;
        }
    }
}
