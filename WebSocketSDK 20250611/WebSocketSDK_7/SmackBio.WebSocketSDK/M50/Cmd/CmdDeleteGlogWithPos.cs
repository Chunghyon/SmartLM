using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using SmackBio.WebSocketSDK.Cmd;

namespace SmackBio.WebSocketSDK.M50.Cmd
{
    public class CmdDeleteGlogWithPos : CmdBase
    {
        public const string MSG_KEY = "DeleteGlogWithPos";

        UInt32 EndPos;

        public CmdDeleteGlogWithPos(UInt32 end_pos)
            : base()
        {
            EndPos = end_pos;
        }

        public override string Build()
        {
            string result = StartBuild();
            AppendTag(ref result, TAG_REQUEST, MSG_KEY);
            AppendTag(ref result, "EndPos", EndPos);
            AppendEndup(ref result);

            return result;
        }

        public override Type GetResponseType()
        {
            return typeof(GeneralResponse);
        }
    }
}
