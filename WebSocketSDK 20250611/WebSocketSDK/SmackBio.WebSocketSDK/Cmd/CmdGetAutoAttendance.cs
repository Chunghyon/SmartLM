using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using SmackBio.WebSocketSDK.M50;

namespace SmackBio.WebSocketSDK.Cmd
{
    /// <summary>
    /// Attendance status
    /// </summary>
    public enum AttendStatus
    {
        DutyOn        = 0,
        DutyOff       = 1,
        OvertimeOn    = 2,
        OvertimeOff   = 3,
        In            = 4,
        Out           = 5,
    }
    public class AutoAttendance
    {
        private int no;
        private int start;
        private int end;

        public AutoAttendance() { }

        public AutoAttendance(int no, int start, int end, AttendStatus status)
        {
            this.no = no;
            this.start = start;
            this.end = end;
            Status = status;
        }

        public int GetStart() { return start; }
        public void SetStart(int start) { this.start = start; }
        public int GetEnd() { return end; }
        public void SetEnd(int end) { this.end = end; }

        public int No { get { return no; } }

        public int StartHour
        {
            get { return start / 60; }
            set
            {
                start = value * 60 + start % 60;
            }
        }

        public int StartMinute
        {
            get { return start % 60; }
            set
            {
                start = start / 60 * 60 + value;
            }
        }

        public int EndHour
        {
            get { return end / 60; }
            set
            {
                end = value * 60 + end % 60;
            }
        }

        public int EndMinute
        {
            get { return end % 60; }
            set
            {
                end = end / 60 * 60 + value;
            }
        }

        public AttendStatus Status { get; set; }
    }
    public class CmdGetAutoAttendance : CmdBase
    {
        public const string MSG_KEY = "GetAutoAttendance";

        public CmdGetAutoAttendance()
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
            return typeof(CmdGetAutoAttendanceResponse);
        }
    }

    public class CmdGetAutoAttendanceResponse : Response
    {
        AutoAttendance[] sections;
        public AutoAttendance[] Sections { get { return sections; } }

        public override bool Parse(XmlDocument doc)
        {
            sections = new AutoAttendance[M50Device.TrTimezoneCount];
            string tag;
            string value;
            for (int i = 0; i < M50Device.TrTimezoneCount; ++i)
            {
                tag = "TimeSection_" + i;
                value = ParseTag(doc, tag);

                sections[i] = new AutoAttendance(i, 0, 0, AttendStatus.DutyOn);
                if (value != null)
                {
                    string[] items = value.Split(new char[] { ',' });
                    if (items.Length != 3)
                        return false;

                    try
                    {
                        sections[i].SetStart(Convert.ToInt32(items[0]));
                        sections[i].SetEnd(Convert.ToInt32(items[1]));
                        sections[i].Status = (AttendStatus)Convert.ToInt32(items[2]);
                    }
                    catch (Exception)
                    {
                    	
                    }
                }
            }
            
            return true;
        }
    }
}
