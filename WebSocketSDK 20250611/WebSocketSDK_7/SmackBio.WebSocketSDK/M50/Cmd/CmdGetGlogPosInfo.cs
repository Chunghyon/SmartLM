using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using SmackBio.WebSocketSDK.Cmd;

namespace SmackBio.WebSocketSDK.M50.Cmd
{
    public class CmdGetGlogPosInfo : CmdBase
    {
        public const string MSG_KEY = "GetGlogPosInfo";

        public CmdGetGlogPosInfo()
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
            return typeof(CmdGetGlogPosInfoResponse);
        }
    }

    public class CmdGetGlogPosInfoResponse : Response
    {
        public UInt32 LogCount;
        public UInt32 MaxCount;
        public UInt32 StartPos;

        public override bool Parse(XmlDocument doc)
        {
            string str;
            str = ParseTag(doc, "LogCount");
            try
            {
                LogCount = Convert.ToUInt32(str);
            }
            catch (Exception)
            {
                return false;
            }

            str = ParseTag(doc, "MaxCount");
            try
            {
                MaxCount = Convert.ToUInt32(str);
            }
            catch (Exception)
            {
                return false;
            }

            str = ParseTag(doc, "StartPos");
            try
            {
                StartPos = Convert.ToUInt32(str);
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }
    }
}
