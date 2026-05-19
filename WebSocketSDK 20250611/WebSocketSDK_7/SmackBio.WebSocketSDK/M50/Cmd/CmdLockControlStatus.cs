using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SmackBio.WebSocketSDK.Cmd;
using System.Xml;

namespace SmackBio.WebSocketSDK.M50.Cmd
{
    public class CmdLockControlStatus : CmdBase
    {
        public const string MSG_KEY = "LockControlStatus";

        public CmdLockControlStatus()
            : base()
        {

        }

        public override string Build()
        {
            string result = StartBuild();
            AppendTag(ref result, TAG_REQUEST, MSG_KEY);
            AppendEndup(ref result);
            return result;
        }

        public override Type GetResponseType()
        {
            return typeof(CmdLockControlStatusResponse);
        }
    }

    public class CmdLockControlStatusResponse : Response
    {
        LockControlMode mode;

        public LockControlMode Mode { get { return mode; } }

        public override bool Parse(XmlDocument doc)
        {
            string strMode = ParseTag(doc, "Mode");
            if (strMode == null)
                return false;

            try
            {
                mode = (LockControlMode)Convert.ToInt32(strMode);
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }
    }
}
