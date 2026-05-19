using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using SmackBio.WebSocketSDK.DB;

namespace SmackBio.WebSocketSDK.Cmd
{
    public class CmdGetUserBalanceTime : CmdBase
    {
        public const string MSG_KEY = "GetUserBalanceTime";

        Int64 user_id;

        public CmdGetUserBalanceTime(Int64 user_id) : base()
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
            return typeof(CmdGetUserBalanceTimeResponse);
        }
    }

    public class CmdGetUserBalanceTimeResponse : Response 
    {
        Int64 user_id;
        public int balance_time_in_minutes;

        public CmdGetUserBalanceTimeResponse() { }

        public override bool Parse(XmlDocument doc)
        {
            bool ret = base.Parse(doc);
            base.ParseResult(doc);
            if (!ret || result != CommandExeResult.OK)
                return false;

            try
            {
                string str_user_id = ParseTag(doc, "UserID");
                user_id = Convert.ToInt32(str_user_id);

                var balance_time_str = ParseTag(doc, "BalanceTimeInMinues");
                if (balance_time_str != null)
                    balance_time_in_minutes = Convert.ToInt32(balance_time_str);
                else
                    balance_time_in_minutes = 0;
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }
    }
}
