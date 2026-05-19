using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using SmackBio.WebSocketSDK.DB;

namespace SmackBio.WebSocketSDK.Cmd
{
    public class CmdGetEthernetSetting : CmdBase
    {
        public const string MSG_KEY = "GetEthernetSetting";

        public CmdGetEthernetSetting() : base() { }

        public override string Build()
        {
            string result = StartBuild();
            AppendTag(ref result, TAG_REQUEST, MSG_KEY);
            AppendEndup(ref result);

            return result;
        }

        public override Type GetResponseType()
        {
            return typeof(CmdGetEthernetSettingResponse);
        }
    }

    public class CmdGetEthernetSettingResponse : Response 
    {
        public EthernetSetting setting = new EthernetSetting();

        public CmdGetEthernetSettingResponse() { }

        public override bool Parse(XmlDocument doc)
        {
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

            setting.macaddr = ParseTag(doc, "MacAddress");

            setting.ip_from_dhcp = ParseTag(doc, "IP_from_dhcp");
            setting.subnet_from_dhcp = ParseTag(doc, "Subnet_from_dhcp");
            setting.gateway_from_dhcp = ParseTag(doc, "DefaultGateway_from_dhcp");

            str_result = ParseTag(doc, TAG_RESULT);

            return true;
        }
    }
}
