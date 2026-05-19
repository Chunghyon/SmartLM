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
        public enum LoginResultType {
            Ok,
            Fail,
            FailUnknownToken,
        };

        public const string MSG_KEY = "Login";

        string sn;
        LoginResultType login_result;

        public CmdLoginResponse() : base() { }

        public CmdLoginResponse(string sn, LoginResultType login_result)
            : base()
        {
            this.sn = sn;
            this.login_result = login_result;
        }

        public override string Build()
        {
            string result = StartBuild();
            AppendTag(ref result, TAG_RESPONSE, MSG_KEY);
            AppendTag(ref result, TAG_DEV_SN, sn);
            if (login_result == LoginResultType.Ok)
                AppendTag(ref result, TAG_RESULT, STR_OK);
            else if (login_result == LoginResultType.FailUnknownToken)
                AppendTag(ref result, TAG_RESULT, STR_FAIL_UNKNOWN_TOKEN);
            else
                AppendTag(ref result, TAG_RESULT, STR_FAIL);
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
