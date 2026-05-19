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
    public class EvtAdminLog : EvtBase
    {
        public const string EVT_KEY = "AdminLog";

        public AdminLog log = new AdminLog();

        public override bool Parse(XmlDocument doc)
        {
            bool ret = base.Parse(doc);
            if (!ret)
                return false;

            log.device_serial = device_sn;
            log.log_id = Convert.ToInt32(ParseTag(doc, "LogID"));
            log.time = Utils.ParseDateTime(ParseTag(doc, "Time"));
            log.admin_id = Convert.ToInt64(ParseTag(doc, "AdminID"));
            log.emp_id = Convert.ToInt64(ParseTag(doc, "UserID"));
            log.action = ParseTag(doc, "Action");
            log.result = Convert.ToInt16(ParseTag(doc, "Stat"));

            return true;
        }
    }

    public class EvtAdminLog_v2 : EvtAdminLog
    {
        public new const string EVT_KEY = "AdminLog_v2";
    }
}
