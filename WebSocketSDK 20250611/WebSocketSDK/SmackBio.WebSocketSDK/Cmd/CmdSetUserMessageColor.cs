using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using SmackBio.WebSocketSDK.DB;

namespace SmackBio.WebSocketSDK.Cmd
{
    public class CmdSetUserMessageColor : CmdBase
    {

        public const string MSG_KEY = "SetUserMessageColor";

        public uint message_color;
        public uint message_bk_color;

        public CmdSetUserMessageColor(uint c, uint bk_color) 
            : base()
        {
            this.message_color = c;
            this.message_bk_color = bk_color;
        }

        public override string Build()
        {
            string result = StartBuild();
            AppendTag(ref result, TAG_REQUEST, MSG_KEY);

            AppendTag(ref result, "MessageColor", "FF" + message_color.ToString("X6"));
            AppendTag(ref result, "MessageBkColor", "FF" + message_bk_color.ToString("X6"));

            AppendEndup(ref result);

            return result;
        }
        public override Type GetResponseType()
        {
            return typeof(GeneralResponse);
        }
 
    }
}
