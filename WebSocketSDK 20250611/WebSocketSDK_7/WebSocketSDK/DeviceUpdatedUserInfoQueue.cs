using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SmackBio.WebSocketSDK.Sample
{
    public class DeviceUpdatedUserInfo
    {
        public string device_uid { get; set; }
        public Int64 user_id { get; set; }
        public string action { get; set; }
    }
    public class DeviceUpdatedUserQueue
    {
        private static object _monitor = new object();
        private static List<DeviceUpdatedUserInfo> _pendings = new List<DeviceUpdatedUserInfo>();

        public static void Enqueue(DeviceUpdatedUserInfo info)
        {
            lock (_monitor)
                _pendings.Add(info);
        }
        public static DeviceUpdatedUserInfo[] GetQueue()
        {
            lock (_monitor)
                return _pendings.ToArray();
        }

        public static void Clear()
        {
            lock (_monitor)
                _pendings.Clear();
        }

        public static bool find(string device_uid, out Int64 user_id)
        {
            lock (_monitor)
            {
                foreach (DeviceUpdatedUserInfo info in _pendings)
                    if (info.device_uid == device_uid)
                    {
                        user_id = info.user_id;
                        return true;
                    }
            }
            user_id = 0;
            return false;
        }
        public static void remove(string device_uid, Int64 user_id)
        {
            while (true)
            {
                var index = _pendings.FindIndex(r => r.device_uid == device_uid && r.user_id == user_id);
                if (index >= 0)
                {   // ensure item found
                    _pendings.RemoveAt(index);
                }
                else
                    return;
            }
        }
    }
}