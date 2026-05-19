using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmackBio.WebSocketSDK.Cmd
{
    public class CmdEnableDevice : CmdBase
    {
        public const string MSG_KEY = "EnableDevice";
        bool enable;
        public CmdEnableDevice(bool enable = true) : base()
        {
            this.enable = enable;
        }

        public override string Build()
        {
            string result = StartBuild();
            AppendTag(ref result, TAG_REQUEST, MSG_KEY);
            if (enable)
                AppendTag(ref result, "Enable", "Yes");
            else
                AppendTag(ref result, "Enable", "No");
            AppendEndup(ref result);

            return result;
        }

        public override Type GetResponseType()
        {
            return typeof(GeneralResponse);
        }
    }
}
