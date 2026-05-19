using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using SmackBio.WebSocketSDK.Cmd;

namespace SmackBio.WebSocketSDK.M50.Cmd
{
    public class CmdGetUserAttendOnly : CmdBase
    {
        public const string MSG_KEY = "GetUserAttendOnly";

        Int64 user_id;

        public CmdGetUserAttendOnly(Int64 user_id)
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

        // did not override the function "check" to check if response is valid or not.
        // You need to implement this logic for your application.
        // ex: public override CommandExeResult check(BaseMessage response)

        public override Type GetResponseType()
        {
            return typeof(CmdGetUserAttendOnlyResponse);
        }
    }

    public class CmdGetUserAttendOnlyResponse : Response
    {
        Int64 user_id;
        bool value;

        public Int64 UserId { get { return user_id; } }
        public bool Value { get { return value; } }

        public override bool Parse(XmlDocument doc)
        {
            bool ret = base.Parse(doc);
            base.ParseResult(doc);
            if (!ret || result != CommandExeResult.OK)
                return false;

            string str_user_id = ParseTag(doc, "UserID");
            try
            {
                user_id = Convert.ToInt32(str_user_id);
                value = TagIsBooleanTrue(doc, "Value");
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }
    }
}
