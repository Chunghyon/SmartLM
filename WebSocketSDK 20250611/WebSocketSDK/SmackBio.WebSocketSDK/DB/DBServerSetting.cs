using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace SmackBio.WebSocketSDK.DB
{
    public class DBServerSetting
    {
        public const string TABLE_NAME = "ServerSetting";
        const int SERVER_PORT_PARAM_ID = 1;
        const string SERVER_PORT_PARAM_NAME = "TCP Server Port";

        const int KEEP_ALIVE_TIMEOUT_PARAM_ID = 2;
        const string KEEP_ALIVE_TIMEOUT_PARAM_NAME = "Device Connection Timeout";

        const int ENCRYPTION_PARAM_ID = 3;
        const string ENCRYPTION_PARAM_NAME = "Use encryption while communicating";

        public int id;
        public string name;
        public Int64 numeric_val;
        public string str_val;
    }
}
