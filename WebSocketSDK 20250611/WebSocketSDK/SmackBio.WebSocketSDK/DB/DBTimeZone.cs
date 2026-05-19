using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace SmackBio.WebSocketSDK.DB
{
    public class TimeZone
    {
        public int StartHour = 0;
        public int StartMinute = 0;
        public int EndHour = 23;
        public int EndMinute = 59;
    }

    public class TimeGroup
    {
        public const uint TIME_ZONES_PER_GROUP = 10;
        public uint[] timezones = new uint[TIME_ZONES_PER_GROUP];
    }

    public class TimeSet
    {
        public const uint TIME_GROUPS_PER_TIMESET = 7;
        public uint[] timegroups = new uint[TIME_GROUPS_PER_TIMESET];
    }

    public class TimeZoneSetting
    {
        public const string TABLE_NAME = "TimeZones";
        public const uint TIME_ZONE_COUNT = 32;
        public TimeZone[] timezones = new TimeZone[TIME_ZONE_COUNT];
    }

    public class TimeGroupSetting
    {
        public const string TABLE_NAME = "TimeGroups";
        public const uint TIME_GROUP_COUNT = 32;
        public TimeGroup[] timegroups;

        public TimeGroupSetting()
        {
            timegroups = new TimeGroup[TIME_GROUP_COUNT];
            for (int i = 0; i < TIME_GROUP_COUNT; ++i)
            {
                timegroups[i] = new TimeGroup();
            }
        }
    }

    public class TimeSetSetting
    {
        public const string TABLE_NAME = "TimeSets";
        public const uint TIME_SET_COUNT = 32;
        public TimeSet[] timesets;

        public TimeSetSetting()
        {
            timesets = new TimeSet[TIME_SET_COUNT];
            for (int i = 0; i < TIME_SET_COUNT; ++i)
                timesets[i] = new TimeSet();
        }
    }

    public enum BellType
    {
        Bell1 = 0,
        Bell2 = 1,
        Bell3 = 2,
        Bell4 = 3,
        Bell5 = 4,
    }

    public class Belling
    {
        public bool valid { get; set; }
        public BellType type { get; set; }
        public byte hour { get; set; }
        public byte minute { get; set; }
    }

    public class BellSetting
    {
        public const string TABLE_NAME = "BellSetting";
        public uint BellCount { get; set; }

        public uint RingCount { get; set; }
        public uint RingInterval { get; set; }
        
        public Belling[] bells;
    }

    public class JobCode
    {
        public const int JOB_COUNT = 30;
        uint code;
        string name;

        public uint Code { get { return code; } set { this.code = value; } }
        public string Name { get { return name; } set { this.name = value; } }
    }

    public class RestartInfo
    {
        public bool enable { get; set; }
        public byte weekday_mask { get; set; }
        public byte hour { get; set; }
        public byte minute { get; set; }
    }

    public class PowerSetting
    {
        public uint wakeup_delay { get; set; }
        public uint idle_time_for_sleep { get; set; }

        public uint RestartInfoCount { get; set; }

        public RestartInfo[] restart_infos;
    }
}
