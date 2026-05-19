using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using SmackBio.WebSocketSDK.DB;

namespace SmackBio.WebSocketSDK.Cmd
{
    public class CmdGetUserHolidays : CmdBase
    {
        public const string MSG_KEY = "GetUserHolidays";

        Int64 user_id;

        public CmdGetUserHolidays(Int64 user_id) : base()
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
            return typeof(CmdGetUserHolidaysResponse);
        }
    }

    public class CmdGetUserHolidaysResponse : Response 
    {
        Int64 user_id;
        public int holidays_in_10;

        public CmdGetUserHolidaysResponse() { }

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

                var holidays_str = ParseTag(doc, "HolidaysInDays10");
                if (holidays_str != null)
                    holidays_in_10 = Convert.ToInt32(holidays_str);
                else
                    holidays_in_10 = 0;
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }
    }
}
