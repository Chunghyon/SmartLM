using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SmackBio.WebSocketSDK.Cmd;
using System.Xml;

namespace SmackBio.WebSocketSDK.M50.Cmd
{
    public class CmdGetFirmwareVersion: CmdBase
    {
        public const string MSG_KEY = "GetFirmwareVersion";

        public CmdGetFirmwareVersion()
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
            return typeof(CmdGetFirmwareVersionResponse);
        }
    }

    public class CmdGetFirmwareVersionResponse : Response
    {
        string version;
        UInt32 build_number;
        public string Version { get { return version; } }
        public string BuildNumber { get { return "0x" + build_number.ToString("X8"); } }

        public override bool Parse(XmlDocument doc)
        {
            
            try
            {
                version = ParseTag(doc, "Version");
                build_number = Convert.ToUInt32(ParseTag(doc, "BuildNumber"));
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }
    }
}
