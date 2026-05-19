using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using SmackBio.WebSocketSDK.DB;
using SmackBio.WebSocketSDK.Cmd;
using SmackBio.WebSocketSDK.Util;

namespace SmackBio.WebSocketSDK.M50.Event
{
    public class EvtKeepAlive : EvtBase
    {
        public const string EVT_KEY = "KeepAlive";
        public DateTime dev_time;
        public override bool Parse(XmlDocument doc)
        {
            string str_time = ParseTag(doc, "DevTime");
            if (str_time == null)
                return false;

            dev_time = Utils.ParseDateTime(str_time);
            return true;
        }
    }
}
