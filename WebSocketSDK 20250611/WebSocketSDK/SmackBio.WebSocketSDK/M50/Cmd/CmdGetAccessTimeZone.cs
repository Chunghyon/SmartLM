using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SmackBio.WebSocketSDK.Cmd;
using System.Xml;

namespace SmackBio.WebSocketSDK.M50.Cmd
{
    public class AccessTimeSection
    {
        private int no;
        private int start;
        private int end;

        public AccessTimeSection() { }

        public AccessTimeSection(int no, int start, int end)
        {
            this.no = no;
            this.start = start;
            this.end = end;
        }

        public int GetStart() { return start; }
        public void SetStart(int start) { this.start = start; }
        public int GetEnd() { return end; }
        public void SetEnd(int end) { this.end = end; }

        public int GetNo() {  return no; }
        public string Day 
        {
            get 
            {
                switch (no)
                {
                    case 0:
                        return "SUN";
                    case 1:
                        return "MON";
                    case 2:
                        return "TUE";
                    case 3:
                        return "WED";
                    case 4:
                        return "THU";
                    case 5:
                        return "FRI";
                    case 6:
                        return "SAT";
                    default:
                        return "";
                }
            }
        }

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
    }
    public class CmdGetAccessTimeZone : CmdBase
    {
        public const string MSG_KEY = "GetAccessTimeZone";
        int timezone_no;

        public CmdGetAccessTimeZone(int timezone_no) : base()
        {
            this.timezone_no = timezone_no;
        }

        public override string Build()
        {
            string result = StartBuild();
            AppendTag(ref result, TAG_REQUEST, MSG_KEY);
            AppendTag(ref result, "TimeZoneNo", timezone_no);
            AppendEndup(ref result);

            return result;
        }

        public override Type GetResponseType()
        {
            return typeof(CmdGetAccessTimeZoneResponse);
        }
    }

    public class CmdGetAccessTimeZoneResponse : Response
    {
        AccessTimeSection[] sections;
        public AccessTimeSection[] Sections { get { return sections; } }

        public override bool Parse(XmlDocument doc)
        {
            sections = new AccessTimeSection[M50Device.TIMESECTION_COUNT_PER_TIMEZONE];
            string tag;
            string value;
            for (int i = 0; i < M50Device.TIMESECTION_COUNT_PER_TIMEZONE; ++i)
            {
                tag = "TimeSection_" + i;
                value = ParseTag(doc, tag);

                sections[i] = new AccessTimeSection(i, 0, 0);
                if (value != null)
                {
                    string[] items = value.Split(new char[] { ',' });
                    if (items.Length != 2)
                        return false;

                    try
                    {
                        sections[i].SetStart(Convert.ToInt32(items[0]));
                        sections[i].SetEnd(Convert.ToInt32(items[1]));
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
