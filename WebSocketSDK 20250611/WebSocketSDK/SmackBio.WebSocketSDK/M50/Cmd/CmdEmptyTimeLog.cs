using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SmackBio.WebSocketSDK.Cmd;

namespace SmackBio.WebSocketSDK.M50.Cmd
{
    public class CmdEmptyTimeLog : CmdBase
    {
        public const string MSG_KEY = "EmptyTimeLog";

        public CmdEmptyTimeLog() : base() { }

        public override string Build()
        {
            string result = StartBuild();
            AppendTag(ref result, TAG_REQUEST, MSG_KEY);
            AppendEndup(ref result);

            return result;
        }

        public override Type GetResponseType()
        {
            return typeof(GeneralResponse);
        }
    }
}
