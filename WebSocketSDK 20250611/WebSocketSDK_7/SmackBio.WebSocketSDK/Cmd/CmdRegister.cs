using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using SmackBio.WebSocketSDK.Util;

namespace SmackBio.WebSocketSDK.Cmd
{
    public class CmdRegisterResponse : CmdBase
    {
        public const string MSG_KEY = "Register";
        string sn;
        string token;

        public CmdRegisterResponse(string sn, string token) : base()
        {
            this.sn = sn;
            this.token = token;
        }

        public override string Build()
        {
            string result = StartBuild();
            AppendTag(ref result, TAG_RESPONSE, MSG_KEY);
            AppendTag(ref result, TAG_DEV_SN, sn);
            AppendTag(ref result, "Token", token);
            AppendTag(ref result, TAG_RESULT, STR_OK);
            AppendEndup(ref result);

            return result;
        }

        public override Type GetResponseType()
        {
            return typeof(CmdRegister);
        }
    }

    class CmdRegister : Response
    {
        public const string MSG_KEY = "Register";

        public CmdRegister() { }

        public string deviceSerialNo;
        public string terminalType;
        
        public string CloudId;

        public override bool Parse(XmlDocument doc)
        {
            string cmd = ParseTag(doc, TAG_REQUEST);
            if (cmd == null || cmd != MSG_KEY)
                return false;
            deviceSerialNo = ParseTag(doc, TAG_DEV_SN);
            if (deviceSerialNo == null)
                return false;
            terminalType = ParseTag(doc, TAG_MODEL);
            if (terminalType == null)
                return false;

            CloudId = ParseTag(doc, TAG_CLOUD_ID);
            if (CloudId == null)
                CloudId = "";

            return true;
        }
    }
}
