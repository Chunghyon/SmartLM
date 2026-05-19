using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmackBio.WebSocketSDK.Cmd
{
    public class CmdRestart : CmdBase
    {
        public const string MSG_KEY = "Restart";

        public CmdRestart() : base() { }

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
