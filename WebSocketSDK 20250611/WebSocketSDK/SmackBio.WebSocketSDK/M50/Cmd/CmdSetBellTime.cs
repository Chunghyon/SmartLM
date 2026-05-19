using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SmackBio.WebSocketSDK.Cmd;
using SmackBio.WebSocketSDK.DB;

namespace SmackBio.WebSocketSDK.M50.Cmd
{
    public class CmdSetBellTime : CmdBase
    {
        public const string MSG_KEY = "SetBellTime";

        BellSetting setting;

        public CmdSetBellTime(BellSetting setting)
            : base()
        {
            this.setting = setting;
        }

        public override string Build()
        {
            string result = StartBuild();
            AppendTag(ref result, TAG_REQUEST, MSG_KEY);

            AppendTag(ref result, "BellRingTimes", setting.RingCount);
            AppendTag(ref result, "BellPeriod", 0);
            AppendTag(ref result, "BellCount", setting.BellCount);

            for (int i = 0; i < setting.BellCount; ++i)
            {
                string value = "" + (setting.bells[i].valid ? 1 : 0) + ","
                                + (byte)setting.bells[i].type + ","
                                + setting.bells[i].hour + ","
                                + setting.bells[i].minute;
                AppendTag(ref result, "Bell_" + i, value);
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
