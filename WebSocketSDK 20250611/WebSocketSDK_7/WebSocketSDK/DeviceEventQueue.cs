using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Xml;
using SmackBio.WebSocketSDK.Cmd;
using SmackBio.WebSocketSDK.M50.Event;

namespace SmackBio.WebSocketSDK.Sample
{
    public class DeviceEvent
    {
        public string device_uid { get; set; }
        public string device_name { get; set; }
        public XmlDocument content { get; set; }

        public string content_string
        {
            get
            {
                StringWriter sw = new StringWriter();
                var xml_w = XmlWriter.Create(sw);
                content.WriteContentTo(xml_w);
                xml_w.Flush();
                return sw.ToString();
            }
        }
        public string comprehensive_string
        {
            get
            {
                if (BaseMessage.IsEventKey(content, EvtTimeLog.EVT_KEY) ||
                    BaseMessage.IsEventKey(content, EvtTimeLog_v2.EVT_KEY))
                {
                    EvtTimeLog evt = new EvtTimeLog();
                    if (evt.Parse(content))
                    {
                        return "[TimeLog] " + "LogID(" + evt.log.log_id.ToString() + "), "
                            + "Time(" + evt.log.time.ToString() + "), "
                            + "UserID(" + evt.log.emp_id.ToString() + "), "
                            + "AttendStat(" + evt.log.attend_status + "), "
                            + "Action(" + evt.log.action + "), "
                            + "JobCode(" + evt.log.jobcode + "), "
                            + "Photo(" + evt.log.photo + ")"
                            + ((evt.log.body_temp_100 <= 0) ? "" :
                                (", BodyTemp(" + (evt.log.body_temp_100 / 100).ToString() + "." + ((evt.log.body_temp_100 % 100) / 10).ToString() + "'C)"))
                            + ((evt.log.latitude == null) ? "" : ", Latitude(" + evt.log.latitude + ")")
                            + ((evt.log.longitude == null) ? "" : ", Longitude(" + evt.log.longitude + ")");
                    }
                }
                else if (BaseMessage.IsEventKey(content, EvtAdminLog.EVT_KEY) ||
                         BaseMessage.IsEventKey(content, EvtAdminLog_v2.EVT_KEY))
                {
                    EvtAdminLog evt = new EvtAdminLog();
                    if (evt.Parse(content))
                    {
                        return "[AdminLog] " + "LogID(" + evt.log.log_id.ToString() + "), "
                            + "Time(" + evt.log.time.ToString() + "), "
                            + "AdminID(" + evt.log.admin_id.ToString() + "), "
                            + "UserID(" + evt.log.emp_id + "), "
                            + "Action(" + evt.log.action + "), "
                            + "Stat(" + evt.log.result + ")";
                    }
                }
                return "";
            }
        }

/*     
const char* slog_action_strings[] = 
{
	"TurnOn",
	"TurnOff",
	"IllegalAlarm",
	"Tamper",
	"EnterMenu",
	"SettingChanged",
	"BackupFP",
	"SetPWD",
	"EnrollCard",
	"DeleteUser",
	"DeleteFP",
	"DeletePWD",
	"DeleteCard",
	"DeleteAll",
	"SetTime",
	"Restore",
	"DeleteAllLog",
	"RemovePrivilege",
	"TGSet",
	"UserTZSet",
	"TZSet",
	"LockGroupSet",
	"DoorOpen",
	"EnrollUser",
	"OpenTimeoutAlarm",
	"IllegalOpenAlarm",
	"DuressAlarm",
	"TimeSetSet",
	"EnrollPhoto",
	"DeletePhoto",
	"EnrollMsg",
	"DeleteMsg",
	"EnrollFace",
	"DeleteFace",
};
*/

        public bool is_userinfo_updated_event(out Int64 user_id, out string action)
        {
            if (BaseMessage.IsEventKey(content, EvtAdminLog.EVT_KEY))
            {
                EvtAdminLog evt = new EvtAdminLog();
                if (evt.Parse(content))
                {
                    if (
			                (evt.log.action == "BackupFP") ||
			                (evt.log.action == "SetPWD") ||
			                (evt.log.action == "EnrollCard") ||
			                (evt.log.action == "DeleteUser") ||
			                (evt.log.action == "DeleteFP") ||
			                (evt.log.action == "DeletePWD") ||
			                (evt.log.action == "DeleteCard") ||
//			                (evt.log.action == "DeleteAll") ||
//			                (evt.log.action == "RemovePrivilege") ||
			                (evt.log.action == "UserTZSet") ||
			                (evt.log.action == "EnrollUser") ||
			                (evt.log.action == "EnrollFace") ||
			                (evt.log.action == "DeleteFace") 
                       )                       
                    {
                        user_id = evt.log.emp_id;
                        action = evt.log.action;
                        return true;
                    }
                }
            }
            user_id = 0;
            action = "";
            return false;
        }
    }

    public class DeviceEventQueue
    {
        private static object _monitor = new object();
        private static List<DeviceEvent> _events = new List<DeviceEvent>();

        public static void Enqueue(DeviceEvent dev_ev)
        {
            lock (_monitor)
                _events.Add(dev_ev);
        }

        public static DeviceEvent[] GetQueuedEvents()
        {
            lock (_monitor)
                return _events.ToArray();
        }

        public static void Clear()
        {
            lock (_monitor)
                _events.Clear();
        }
    }
}