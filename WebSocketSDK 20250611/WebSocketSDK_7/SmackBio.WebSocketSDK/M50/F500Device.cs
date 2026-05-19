using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Xml;
//using SmackBio.WebSocketSDK.DB;
using System.Data;
//using SmackBio.WebSocketSDK.Cmd;
using System.IO;

namespace SmackBio.WebSocketSDK.M50
{
    public class M50Device
    {
        public const string  MODEL_NAME = "F500";
        public const int     MAX_FINGERS_PER_USER = 10;
        public const int     TIMESECTION_COUNT_PER_TIMEZONE = 7;
        
		public const uint   MaxDeptCount        = 100;
        public const uint	UserDeptCount		= 20;
		public const uint	ProxyDeptCount		= 20;
        public const uint	MaxProxyDeptCount	= 32;
        public const uint	DeptNameLength		= 12;
        public const uint   AcTimezoneCount     = 50;
        public const uint   TrTimezoneCount		= 10;
        public const uint   BellCount           = 24;
        public const uint   MaxAutoRestartDateTimes = 12;

        public const uint   UserNameLength      = 24;
        public const uint   MaxUserPasswordLength = 6;
    }
}
