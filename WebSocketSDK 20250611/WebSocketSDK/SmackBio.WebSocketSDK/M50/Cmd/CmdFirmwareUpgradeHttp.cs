using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using SmackBio.WebSocketSDK.Cmd;
using System.Text;

namespace SmackBio.WebSocketSDK.M50.Cmd
{
    public class CmdFirmwareUpgradeHttp : CmdBase
    {
        public const string MSG_KEY = "FirmwareUpgradeHttp";
        string url;

        public CmdFirmwareUpgradeHttp(string url)
            : base()
        {
            this.url = url;
        }

        public override string Build()
        {
            string result = StartBuild();
            AppendTag(ref result, TAG_REQUEST, MSG_KEY);

            byte[] url_binary = Encoding.ASCII.GetBytes(url);
            AppendTag(ref result, "Size", url.Length);
            AppendTag(ref result, "Data", Convert.ToBase64String(url_binary));

            AppendEndup(ref result);
            return result;
        }

        public override Type GetResponseType()
        {
            return typeof(GeneralResponse);
        }
    }
}