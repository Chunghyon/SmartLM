using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using SmackBio.WebSocketSDK.DB;

namespace SmackBio.WebSocketSDK.Cmd
{
    public class CmdSetUserHolidays : CmdBase
    {

        public const string MSG_KEY = "SetUserHolidays";
        Int64 user_id;
        int holidays_in_10;

        public CmdSetUserHolidays(Int64 user_id, int user_holidays_in_10)
            : base()
        {
            this.user_id = user_id;
            this.holidays_in_10 = user_holidays_in_10;
        }

        public override string Build()
        {
            string result = StartBuild();
            AppendTag(ref result, TAG_REQUEST, MSG_KEY);
            AppendTag(ref result, "UserID", user_id);
            AppendTag(ref result, "HolidaysInDays10", holidays_in_10);

            AppendEndup(ref result);

            return result;
        }
        public override Type GetResponseType()
        {
            return typeof(GeneralResponse);
        }
 
    }
}
