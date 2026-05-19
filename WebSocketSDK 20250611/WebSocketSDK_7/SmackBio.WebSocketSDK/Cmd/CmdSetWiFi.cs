using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using SmackBio.WebSocketSDK.DB;

namespace SmackBio.WebSocketSDK.Cmd
{
    public class CmdSetWiFi : CmdBase
    {
        public const string MSG_KEY = "SetWiFi";

        public WiFiSetting setting;

        public CmdSetWiFi(WiFiSetting setting) : base()
        {
            this.setting = setting;
        }

        public override string Build()
        {
            string result = StartBuild();
            AppendTag(ref result, TAG_REQUEST, MSG_KEY);

            if (setting.use_wifi)
                AppendTag(ref result, "Use", "Yes");
            else
                AppendTag(ref result, "Use", "No");

            AppendTag(ref result, "SSID", setting.SSID);
            AppendTag(ref result, "Key", setting.key);

            if (setting.use_dhcp)
                AppendTag(ref result, "DHCP", "Yes");
            else
                AppendTag(ref result, "DHCP", "No");

            AppendTag(ref result, "IP", setting.ip);
            AppendTag(ref result, "Subnet", setting.subnet);
            AppendTag(ref result, "DefaultGateway", setting.gateway);
            AppendTag(ref result, "Port", setting.port);

            AppendEndup(ref result);

            return result;
        }

        public override Type GetResponseType()
        {
            return typeof(GeneralResponse);
        }
          
    }
}
