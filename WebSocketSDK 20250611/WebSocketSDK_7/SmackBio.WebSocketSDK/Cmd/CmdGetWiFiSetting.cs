using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using SmackBio.WebSocketSDK.DB;

namespace SmackBio.WebSocketSDK.Cmd
{
    public class CmdGetWiFiSetting : CmdBase
    {
        public const string MSG_KEY = "GetWiFiSetting";

        public CmdGetWiFiSetting()
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
            return typeof(CmdGetWiFiSettingResponse);
        }
    }

    public class CmdGetWiFiSettingResponse : Response
    {
        public CmdGetWiFiSettingResponse() { }

        public WiFiSetting setting = new WiFiSetting();

        public override bool Parse(XmlDocument doc)
        {
            if (TagIsBooleanTrue(doc, "Use"))
                setting.use_wifi = true;
            else
                setting.use_wifi = false;

            setting.SSID = ParseTag(doc, "SSID");
            setting.key = ParseTag(doc, "Key");

            if (TagIsBooleanTrue(doc, "DHCP"))
                setting.use_dhcp = true;
            else
                setting.use_dhcp = false;

            setting.ip = ParseTag(doc, "IP");
            setting.subnet = ParseTag(doc, "Subnet");
            setting.gateway = ParseTag(doc, "DefaultGateway");
            string str_port = ParseTag(doc, "Port");
            if (str_port != null)
                setting.port = Convert.ToInt32(str_port);

            setting.ip_from_dhcp = ParseTag(doc, "IP_from_dhcp");
            setting.subnet_from_dhcp = ParseTag(doc, "Subnet_from_dhcp");
            setting.gateway_from_dhcp = ParseTag(doc, "DefaultGateway_from_dhcp");

            str_result = ParseTag(doc, TAG_RESULT);

            return true;
        }
    }
}
