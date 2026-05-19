using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SmackBio.WebSocketSDK.Cmd;

namespace SmackBio.WebSocketSDK.M50.Cmd
{
    public class CmdSetAccessTimeZone : CmdBase
    {
        public const string MSG_KEY = "SetAccessTimeZone";
        int timezone_no;
        AccessTimeSection[] sections;

        public CmdSetAccessTimeZone(int timezone_no, AccessTimeSection[] sections)
            : base()
        {
            this.timezone_no = timezone_no;
            if (sections == null || sections.Length != M50Device.TIMESECTION_COUNT_PER_TIMEZONE)
                throw new ArgumentException("Invalid Command");
            this.sections = sections;
        }

        public override string Build()
        {
            string result = StartBuild();
            AppendTag(ref result, TAG_REQUEST, MSG_KEY);
            AppendTag(ref result, "TimeZoneNo", timezone_no);

            for (int i = 0; i < sections.Length; ++i)
            {
                if (sections[i] == null)
                    throw new ArgumentException("Invalid Command");

                string tag = "TimeSection_" + sections[i].GetNo();
                string value = "" + sections[i].GetStart() + ","
                                + sections[i].GetEnd();
                AppendTag(ref result, tag, value);
            }

            AppendEndup(ref result);

            return result;
        }

        public override Type GetResponseType()
        {
            return typeof(GeneralResponse);
        }
    }
}
