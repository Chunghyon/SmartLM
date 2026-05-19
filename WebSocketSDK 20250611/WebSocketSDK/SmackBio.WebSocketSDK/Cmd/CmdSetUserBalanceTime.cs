using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using SmackBio.WebSocketSDK.DB;

namespace SmackBio.WebSocketSDK.Cmd
{
    public class CmdSetUserBalanceTime : CmdBase
    {

        public const string MSG_KEY = "SetUserBalanceTime";
        Int64 user_id;
        int balance_time_in_minutes;

        public CmdSetUserBalanceTime(Int64 user_id, int user_balance_time_in_minutes)
            : base()
        {
            this.user_id = user_id;
            this.balance_time_in_minutes = user_balance_time_in_minutes;
        }

        public override string Build()
        {
            string result = StartBuild();
            AppendTag(ref result, TAG_REQUEST, MSG_KEY);
            AppendTag(ref result, "UserID", user_id);
            AppendTag(ref result, "BalanceTimeInMinues", balance_time_in_minutes);

            AppendEndup(ref result);

            return result;
        }
        public override Type GetResponseType()
        {
            return typeof(GeneralResponse);
        }
 
    }
}
