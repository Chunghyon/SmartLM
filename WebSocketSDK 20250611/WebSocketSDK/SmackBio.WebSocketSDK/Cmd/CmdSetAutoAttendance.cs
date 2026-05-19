using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SmackBio.WebSocketSDK.M50;

namespace SmackBio.WebSocketSDK.Cmd
{
    public class CmdSetAutoAttendance : CmdBase
    {
        public const string MSG_KEY = "SetAutoAttendance";
        AutoAttendance[] sections;

        public CmdSetAutoAttendance(AutoAttendance[] sections)
            : base()
        {
            if (sections == null || sections.Length != M50Device.TrTimezoneCount)
                throw new ArgumentException("Invalid Command");
            this.sections = sections;
        }

        public override string Build()
        {
            string result = StartBuild();
            AppendTag(ref result, TAG_REQUEST, MSG_KEY);

            for (int i = 0; i < sections.Length; ++i)
            {
                if (sections[i] == null)
                    throw new ArgumentException("Invalid Command");

                string tag = "TimeSection_" + sections[i].No;
                string value = "" + sections[i].GetStart() + ","
                                + sections[i].GetEnd() + ","
                                + (int)sections[i].Status;
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
