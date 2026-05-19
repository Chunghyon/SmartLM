using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoNetDrive.Protocol.Fingerprint.Test.Model
{
    public class DeviceInfo
    {
        public string SN { get; set; }
        public string IP { get; set; }
        public string MAC { get; set; }

        public DeviceInfo()
        {

        }
        public DeviceInfo(string SN, string IP, string MAC)
        {
            this.SN = SN;
            this.IP = IP;
            this.MAC = MAC;
        }
    }
}
