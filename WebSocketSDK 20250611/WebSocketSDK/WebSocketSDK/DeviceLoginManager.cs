using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Xml;
using System.Security.Cryptography;

using SmackBio.WebSocketSDK;
using SmackBio.WebSocketSDK.M50.Event;
using SmackBio.WebSocketSDK.Cmd;

namespace SmackBio.WebSocketSDK.Sample
{
    public class OnlineDevice
    {
        public string device_uid { get { return session.deviceId; } }
        public SBWebSocketHandler session { get; set; }
        public Guid session_id { get { return session.SessionId; } }
    }

    public class DeviceLoginManager : IDeviceLoginManager
    {
        private static object _monitor = new object();
        public static Dictionary<string, string> _registeredDevices = new Dictionary<string, string>();

        public bool Verify(string deviceId, string token)
        {
            return true;
            /*
            lock (_monitor)
            {
                string regToken;
                if (_registeredDevices.TryGetValue(deviceId, out regToken))
                {
                    return (token == regToken);
                }
                else
                {
                    return false;
                }
            }
             */
        }

        public string GetRegisterInfo(string model, string deviceId)
        {
            byte[] guidBytes = new byte[16];
            Guid guid;
            RandomNumberGenerator _random = RandomNumberGenerator.Create();

            _random.GetBytes(guidBytes);
            guid = new Guid(guidBytes);

            lock (_monitor)
            {
                string oldToken;
                if (!_registeredDevices.TryGetValue(deviceId, out oldToken))
                {
                    _registeredDevices.Add(deviceId, guid.ToString());
                    return guid.ToString();
                }
                else
                    return oldToken;
            }
            /*
            lock (_monitor)
            {
                string token;
                if (_registeredDevices.TryGetValue(deviceId, out token))
                {
                    return token;
                }
                else
                {
                    return null;
                }
            }
            */
        }

        public static OnlineDevice[] GetOnlineDevices()
        {
            List<OnlineDevice> online = new List<OnlineDevice>();

            foreach(Guid guid in SessionRegistry.GetKeys())
            {
                if (SessionRegistry.GetSession(guid).loggedIn)
                {
                    online.Add(new OnlineDevice { session = SessionRegistry.GetSession(guid) });
                }

                const int KeepAliveTimeout = 10 * 60 * 1000;
                SBWebSocketHandler aSession = SessionRegistry.GetSession(guid);
                if (aSession.keepAliveMsgSupported &&
                    aSession.keepAliveRecvedTime < Environment.TickCount - KeepAliveTimeout)
                {
                    aSession.Close();
                }
            }

            return online.ToArray();
        }

        public class RegisterDevice
        {
            public string device_uid { get { return session.deviceId; } }
            public SBWebSocketHandler session { get; set; }
            public Guid session_id { get { return session.SessionId; } }
            public string device_name { get { return session.deviceModel; } }
        }

        public static RegisterDevice[] GetRegisterDevices()
        {
            lock (_monitor)
            {
                List<RegisterDevice> online = new List<RegisterDevice>();

                foreach (Guid guid in SessionRegistry.GetKeys())
                {
                    SBWebSocketHandler aSession = SessionRegistry.GetSession(guid);
                    if (!aSession.loggedIn && aSession.registerMsgRecved
                        && aSession.registerMsgTime > Environment.TickCount - 30000)
                    {
                        string oldToken;

                        if (!_registeredDevices.TryGetValue(aSession.deviceId, out oldToken))
                            online.Add(new RegisterDevice { session = aSession });
                    }
                }

                return online.ToArray();
            }
        }

        public void OnDeviceEvent(XmlDocument xml)
        {
            string device_sn = EvtBase.ParseDeviceSerialNo(xml);
            DeviceEvent evt = new DeviceEvent { device_uid = device_sn, content = xml };
            DeviceEventQueue.Enqueue(evt);

            Int64 user_id;
            string action;
            if (evt.is_userinfo_updated_event(out user_id, out action))
                DeviceUpdatedUserQueue.Enqueue(new DeviceUpdatedUserInfo { device_uid = device_sn, user_id = user_id, action = action });

        }
    }
}