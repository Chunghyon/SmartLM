using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using SmackBio.WebSocketSDK.Util;

namespace SmackBio.WebSocketSDK.Cmd
{
    public class CmdLoginResponse : CmdBase
    {
        public const string MSG_KEY = "Login";

        string sn;

        public CmdLoginResponse() : base() { }

        public CmdLoginResponse(string sn)
            : base()
        {
            this.sn = sn;
        }

        public override string Build()
        {
            string result = StartBuild();
            AppendTag(ref result, TAG_RESPONSE, MSG_KEY);
            AppendTag(ref result, TAG_DEV_SN, sn);
            AppendTag(ref result, TAG_RESULT, STR_OK);
            AppendEndup(ref result);

            return result;
        }

        public override Type GetResponseType()
        {
            return typeof(CmdLogin);
        }
    }

    class CmdLogin : Response
    {
        public CmdLogin() { }

        public string deviceSerialNo;
        public string token;

        public override bool Parse(XmlDocument doc)
        {
            deviceSerialNo = ParseTag(doc, TAG_DEV_SN);
            if (deviceSerialNo == null)
                return false;
            token = ParseTag(doc, "Token");
            if (token == null)
                return false;

            return true;
        }
    }
}
