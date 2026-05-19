using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using SmackBio.WebSocketSDK.Util;

namespace SmackBio.WebSocketSDK.Cmd
{
    public class CmdGetTime : CmdBase
    {
        public const string MSG_KEY = "GetTime";

        public CmdGetTime() : base() { }

        public override string Build()
        {
            string result = StartBuild();
            AppendTag(ref result, TAG_REQUEST, MSG_KEY);
            AppendEndup(ref result);

            return result;
        }

        public override Type GetResponseType()
        {
            return typeof(CmdGetTimeResponse);
        }
    }

    public class CmdGetTimeResponse : Response
    {
        public CmdGetTimeResponse() { }

        public DateTime time;

        public override bool Parse(XmlDocument doc)
        {
            string str_time = ParseTag(doc, "Time");
            if (str_time == null)
                return false;

            time = Utils.ParseDateTime(str_time);

            str_result = ParseTag(doc, TAG_RESULT);

            return true;
        }
    }
}
