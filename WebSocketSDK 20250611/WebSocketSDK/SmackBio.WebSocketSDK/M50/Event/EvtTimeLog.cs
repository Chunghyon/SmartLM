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
    public class EvtTimeLog : EvtBase
    {
        public const string EVT_KEY = "TimeLog";

        public TimeLog log = new TimeLog();
        public Byte[] photo_data;

        public override bool Parse(XmlDocument doc)
        {
            bool ret = base.Parse(doc);
            if (!ret)
                return false;

            log.device_serial = device_sn;
            log.log_id = Convert.ToInt32(ParseTag(doc, "LogID"));
            string str = ParseTag(doc, "UtcTimezoneMinutes");
            if (str != null)
                log.UtcTimezoneMinutes = Convert.ToInt32(str);
            else
                log.UtcTimezoneMinutes = Int32.MaxValue;
            log.time = Utils.ParseDateTime(ParseTag(doc, "Time"));
            log.emp_id = Convert.ToInt64(ParseTag(doc, "UserID"));
            log.attend_status = ParseTag(doc, "AttendStat");
            log.action = ParseTag(doc, "Action");
            log.jobcode = Convert.ToInt32(ParseTag(doc, "JobCode"));
            log.photo = TagIsBooleanTrue(doc, "Photo");

            if (log.photo)
            {
                string str_photo_data = ParseTag(doc, "LogImage");
                if (str_photo_data != null)
                    photo_data = Convert.FromBase64String(str_photo_data);
            }
			
            try
			{
				log.body_temp_100 = Convert.ToInt32(ParseTag(doc, "BodyTemperature100"));
			}
			catch (System.Exception)
			{
				log.body_temp_100 = -1;
			}

            log.attend_only = TagIsBooleanTrue(doc, "AttendOnly");
            log.expired = TagIsBooleanTrue(doc, "Expired");

            try
			{
				log.latitude = ParseTag(doc, "Latitude");
			}
			catch (System.Exception)
			{
				log.latitude = null;
			}

            try
			{
				log.longitude = ParseTag(doc, "Longitude");
			}
			catch (System.Exception)
			{
				log.longitude = null;
			}

            return true;
        }
    }

    public class EvtTimeLog_v2 : EvtTimeLog
    {
        public new const string EVT_KEY = "TimeLog_v2";
    }
}
