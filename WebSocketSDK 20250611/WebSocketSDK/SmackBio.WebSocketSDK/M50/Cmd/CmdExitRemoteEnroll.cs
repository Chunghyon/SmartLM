using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;
using SmackBio.WebSocketSDK.Cmd;

namespace SmackBio.WebSocketSDK.M50.Cmd
{
    public class CmdExitRemoteEnroll : CmdBase
    {
        public const string MSG_KEY = "ExitRemoteEnroll";
        public CmdExitRemoteEnroll()
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
            return typeof(CmdExitRemoteEnrollResponse);
        }
    }

    public enum ExitRemoteEnrollResult
    {
        SuccessExitRemoteEnroll,
        NotStartedRemoteEnroll,
        Unknown,
    }

    public class CmdExitRemoteEnrollResponse : CmdBase
    {
        public ExitRemoteEnrollResult result;

        public ExitRemoteEnrollResult ParseResult(XmlDocument doc)
        {
            switch (ParseTag(doc, "ResultCode"))
            {
                case "SuccessExitRemoteEnroll":
                    result = ExitRemoteEnrollResult.SuccessExitRemoteEnroll;
                    break;
                case "NotStartedRemoteEnroll":
                    result = ExitRemoteEnrollResult.NotStartedRemoteEnroll;
                    break;
                default:
                    result = ExitRemoteEnrollResult.Unknown;
                    break;
            }
            return result; 
        }
        public override Type GetResponseType() { return null; }
    }
}