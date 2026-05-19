using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using SmackBio.WebSocketSDK.Cmd;
using SmackBio.WebSocketSDK.DB;
using SmackBio.WebSocketSDK.Util;

namespace SmackBio.WebSocketSDK.M50.Cmd
{
    public class CmdGetNextGlog : CmdBase
    {
        public const string MSG_KEY = "GetNextGlog";
        public Int32 pos_begin { get; set; }

        public CmdGetNextGlog(Int32 pos_begin)
            : base()
        {
            this.pos_begin = pos_begin;
        }

        public override string Build()
        {
            string result = StartBuild();
            AppendTag(ref result, TAG_REQUEST, MSG_KEY);
            AppendTag(ref result, "BeginLogPos", pos_begin);
            AppendEndup(ref result);

            return result;
        }

        public override Type GetResponseType()
        {
            return typeof(CmdGetNextGlogResponse);
        }
    }

    public class CmdGetNextGlogResponse : GeneralResponse
    {
        public TimeLog log = new TimeLog();
        public Byte[] photo_data;
        public override bool Parse(XmlDocument doc)
        {
            bool ret = base.Parse(doc);
            if (!ret)
                return false;

            log.log_id = Convert.ToInt32(ParseTag(doc, "LogID"));
            string str = ParseTag(doc, "UtcTimezoneMinutes");
            if (str != null)
                log.UtcTimezoneMinutes = Convert.ToInt32(str);
            else
                log.UtcTimezoneMinutes = Int32.MaxValue;
            log.time = Utils.ParseDateTime(ParseTag(doc, "Time"));
            try
            {
                log.emp_id = Convert.ToInt64(ParseTag(doc, "UserID"));
            }
            catch (System.Exception)
            {
                log.emp_id = 0;
            }
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
}
