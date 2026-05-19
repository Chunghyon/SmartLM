using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using SmackBio.WebSocketSDK.Cmd;
using SmackBio.WebSocketSDK.DB;

namespace SmackBio.WebSocketSDK.M50.Cmd
{
    public class CmdSetUserAttendOnly : CmdBase
    {
        public const string MSG_KEY = "SetUserAttendOnly";

        Int64 user_id;
        bool value;

        public CmdSetUserAttendOnly(Int64 user_id, bool value) 
            : base()
        {
            this.user_id = user_id;
            this.value = value;
        }

        public override string Build()
        {
            string result = StartBuild();
            AppendTag(ref result, TAG_REQUEST, MSG_KEY);
            AppendTag(ref result, "UserID", user_id);
            AppendTag(ref result, "Value", value ? "Yes" : "No");

            AppendEndup(ref result);

            return result;
        }

        public override Type GetResponseType()
        {
            return typeof(GeneralResponse);
        }
    }

    public class CmdSetUserAttendOnlyResponse : GeneralResponse
    {
        public override bool Parse(XmlDocument doc)
        {
            bool ret = base.Parse(doc);
            base.ParseResult(doc);
            if (!ret || result != CommandExeResult.OK)
                return false;

            return ret;
        }
    }
}
