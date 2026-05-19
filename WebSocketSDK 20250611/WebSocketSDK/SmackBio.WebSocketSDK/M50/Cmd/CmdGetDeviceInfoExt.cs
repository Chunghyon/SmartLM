using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using SmackBio.WebSocketSDK.Cmd;

namespace SmackBio.WebSocketSDK.M50.Cmd
{
    public enum DevInfoExtParamType
    {
        WebServerUrl,
        SendLogUrl,
        DeviceName,
        
        MobileNetwork,
        NTPServer,
        VPNServer,

        GPS,
    }
    public class CmdGetDeviceInfoExt : CmdBase
    {
        public const string MSG_KEY = "GetDeviceInfoExt";

        DevInfoExtParamType type;

        public CmdGetDeviceInfoExt(DevInfoExtParamType type)
            : base()
        {
            this.type = type;
        }

        public override string Build()
        {
            string result = StartBuild();
            AppendTag(ref result, TAG_REQUEST, MSG_KEY);
            AppendTag(ref result, "ParamName", type.ToString());
            AppendEndup(ref result);

            return result;
        }

        public override Type GetResponseType()
        {
            return typeof(CmdGetDeviceInfoExtResponse);
        }
    }

    public class CmdGetDeviceInfoExtResponse : Response
    {
        public string Value1;
        public string Value2;
        public string Value3;
        public string Value4;
        public string Value5;

        public override bool Parse(XmlDocument doc)
        {
            Value1 = ParseTag(doc, "Value1");
            Value2 = ParseTag(doc, "Value2");
            Value3 = ParseTag(doc, "Value3");
            Value4 = ParseTag(doc, "Value4");
            Value5 = ParseTag(doc, "Value5");

            if (Value1 == null)
                str_result = ParseTag(doc, TAG_RESULT);

            return true;
        }
    }
}
