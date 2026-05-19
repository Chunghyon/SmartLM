using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SmackBio.WebSocketSDK.Util;

namespace SmackBio.WebSocketSDK.Cmd
{
    public class CmdSetTime : CmdBase
    {
        public CmdSetTime() : base() { }

        public const string MSG_KEY = "SetTime";

        public override string Build()
        {
            string result = StartBuild();
            AppendTag(ref result, TAG_REQUEST, MSG_KEY);
            AppendTag(ref result, "Time", Utils.DateTime2string(DateTime.Now));
            AppendEndup(ref result);

            return result;
        }

        public override Type GetResponseType()
        {
            return typeof(GeneralResponse);
        }
    }
}
