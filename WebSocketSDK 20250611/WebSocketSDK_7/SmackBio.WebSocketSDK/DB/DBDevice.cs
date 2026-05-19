using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace SmackBio.WebSocketSDK.DB
{
    public class WiFiSetting
    {
        public bool use_wifi = false;
        public string SSID = null;
        public string key = null;
        public bool use_dhcp = false;
        public string ip = "192.168.1.224";
        public string subnet = "255.255.255.0";
        public string gateway = "192.168.1.1";
        public int port = 0;    // for OCX

        public string ip_from_dhcp = "0.0.0.0";
        public string subnet_from_dhcp = "0.0.0.0";
        public string gateway_from_dhcp = "0.0.0.0";
    }

    public class EthernetSetting
    {
        public bool use_dhcp = false;
        public string ip = "192.168.1.224";
        public string subnet = "255.255.255.0";
        public string gateway = "192.168.1.1";
        public int port;
        public string macaddr = "";

        public string ip_from_dhcp = "0.0.0.0";
        public string subnet_from_dhcp = "0.0.0.0";
        public string gateway_from_dhcp = "0.0.0.0";
    }

    public enum RelayType
    {
        None,
        NO,
        NC,
    }
}
