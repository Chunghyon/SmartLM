using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SmackBio.WebSocketSDK.Cmd;

namespace SmackBio.WebSocketSDK.M50.Cmd
{
    public enum LockControlMode
    {
        ForceOpen       = 1,
        ForceClose      = 2,
        NormalOpen      = 3,
        AutoRecover     = 4,
        Restart         = 5,
        CancelWarning   = 6,
        IllegalOpen     = 7,
    }

    public class CmdLockControl : CmdBase
    {
        public const string MSG_KEY = "LockControl";
        
        LockControlMode mode;

        public CmdLockControl(LockControlMode mode)
            : base()
        {
            this.mode = mode;
        }

        public override string Build()
        {
            string result = StartBuild();
            AppendTag(ref result, TAG_REQUEST, MSG_KEY);
            AppendTag(ref result, "Mode", (int)mode);
            AppendEndup(ref result);
            return result;
        }

        public override Type GetResponseType()
        {
            return typeof(GeneralResponse);
        }
    }
}
