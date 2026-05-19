using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace SmackBio.WebSocketSDK.DB
{
    public class TimeLog
    {
        public const string TABLE_NAME = "TimeLogs";
        public const string PHOTO_LOG_DIR = "photo\\";
        public long id;
        public string device_serial = null;
        public Int32 log_id = 0;
        public DateTime time = new DateTime();
        public Int64 emp_id = 0;
        public string attend_status = null;
        public string action = null;
        public string access_type = null;
        public int jobcode = 0;
        public bool photo;
        public int body_temp_100 = -1;
        public string latitude = null;
        public string longitude = null;
    }

    public class AdminLog
    {
        public const string TABLE_NAME = "AdminLogs";
        public string device_serial = null;
        public Int32 log_id = 0;
        public DateTime time = new DateTime();
        public Int64 admin_id = 0;
        public Int64 emp_id = 0;
        public string action = null;
        public Int16 result = 0;
    }
}
