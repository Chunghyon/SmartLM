using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace SmackBio.WebSocketSDK
{
    public static class SessionRegistry
    {
        internal static object _monitor = new object();
        internal static Dictionary<Guid, SBWebSocketHandler> _sessions = new Dictionary<Guid, SBWebSocketHandler>();
        private static RandomNumberGenerator _random = RandomNumberGenerator.Create();

        /// <summary>
        /// Retrieves an open session from its ID.
        /// </summary>
        /// <param name="guid">Session ID</param>
        /// <returns>A session object if matching ID was found; otherwise null</returns>
        public static SBWebSocketHandler GetSession(Guid guid)
        {
            lock (_monitor)
            {
                SBWebSocketHandler result;
                if (_sessions.TryGetValue(guid, out result))
                    return result;
                else
                {
                    // return null;
                    throw new WebDeviceException("Invalid session ID. The session may have been already closed.");
                }
            }
        }

        internal static Guid AddSession(SBWebSocketHandler handler)
        {
            lock (_monitor)
            {
                byte[] guidBytes = new byte[16];
                Guid guid;

                do
                {
                    _random.GetBytes(guidBytes);
                    guid = new Guid(guidBytes);
                } while (_sessions.ContainsKey(guid));

                _sessions.Add(guid, handler);

                return guid;
            }
        }

        public static void RemoveSession(Guid guid)
        {
            lock (_monitor)
            {
                _sessions.Remove(guid);
            }
        }

        public static void RemoveSessionsByDeviceId(string device_id, Guid guidNotToRemove)
        {
            lock (_monitor)
            {
                var guidsToDelete = _sessions.Where(x => x.Value.deviceId == device_id).Select(x => x.Key).ToArray();
                foreach (var guid in guidsToDelete)
                {
                    if (guid != guidNotToRemove)
                        _sessions.Remove(guid);
                }
            }
        }

        public static Dictionary<Guid, SBWebSocketHandler>.KeyCollection GetKeys()
        {
            lock (_monitor)
            {
                return _sessions.Keys;
            }
        }

    }
}
