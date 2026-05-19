using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using SmackBio.WebSocketSDK.Cmd;

namespace SmackBio.WebSocketSDK.M50.Cmd
{
    public class CmdGetNextUserDataExt : CmdBase
    {
        public const string MSG_KEY = "GetNextUserDataExt";
        public Int64 user_id { get; set; }

        public CmdGetNextUserDataExt(Int64 user_id)
            : base()
        {
            this.user_id = user_id;
        }

        public override string Build()
        {
            string result = StartBuild();
            AppendTag(ref result, TAG_REQUEST, MSG_KEY);
            AppendTag(ref result, "UserID", user_id);
            AppendEndup(ref result);

            return result;
        }

        public override Type GetResponseType()
        {
            return typeof(CmdGetNextUserDataResponse);
        }
    }

    public class CmdGetNextUserDataResponse : CmdGetUserDataResponse
    {
        public bool is_last = false;
        public override bool Parse(XmlDocument doc)
        {
            bool ret = base.Parse(doc);
            if (!ret)
                return false;

            is_last = !TagIsBooleanTrue(doc, "More");

            return ret;
        }
    }
}
