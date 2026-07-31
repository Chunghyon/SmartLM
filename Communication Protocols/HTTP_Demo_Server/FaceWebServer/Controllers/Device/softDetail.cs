using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DeviceProtocolServer.Controllers.Device
{
    public class softDetailList
    {
        public List<softDetail> list { get; set; }
    }

    public class softDetail
    {
        public string name { get; set; }
        public string url { get; set; }
        public string ver { get; set; }
        public string md5 { get; set; }
    }
}
