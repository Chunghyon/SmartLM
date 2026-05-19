using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Configuration;

// using Microsoft.Web.WebSockets;
using SuperWebSocket;

using SmackBio.WebSocketSDK.Cmd;
using System.Threading;
using System.IO;
using SmackBio.WebSocketSDK.M50.Event;

namespace SmackBio.WebSocketSDK
{
    public class SBWebSocketHandler //: WebSocketHandler
    {
        private Guid m_guid;

        public bool loggedIn;
        public int registerMsgTime;
        public bool registerMsgRecved;

        public string deviceId;
        public string deviceModel;
        public int state;
        internal object _state = new object();
        public string msg;

        private uint _nextTransactionId = 0;
        private Dictionary<uint, CommandRequest> _commandsWaitingForResponses = new Dictionary<uint, CommandRequest>();
//        private DeviceEventHandler _eventHandler = null;

        private IDeviceLoginManager _loginManager;
        public Guid SessionId { get { return m_guid; } }

        WebSocketSession m_session;

        internal static object _monitor = new object();
        internal static Dictionary<string, Guid> _Guids = new Dictionary<string, Guid>();

        public static SBWebSocketHandler getHandler(WebSocketSession session)
        {
            lock (_monitor)
            {
                Guid guid;
                if (_Guids.TryGetValue(session.SessionID, out guid))
                    return SessionRegistry.GetSession(guid);
                else
                    return null;
            }
        }

        public /*override*/ void OnOpen(WebSocketSession session)
        {
            _loginManager = (IDeviceLoginManager)
                Type.GetType(ConfigurationManager.AppSettings["smackbio.websocketsdk.deviceLoginManager"])
                .GetConstructor(new Type[0])
                .Invoke(new object[0]);

            m_guid = SessionRegistry.AddSession(this);

            registerMsgRecved = false;
            loggedIn = false;

            m_session = session;
            lock (_monitor)
            {
                _Guids.Add(session.SessionID, m_guid);
            }
        }

        public /*override*/ void OnMessage(string message)
        {
            if (!loggedIn)
            {
                CmdRegister cmdr = new CmdRegister();
                XmlDocument docr = new XmlDocument();
                docr.LoadXml(message);
                if (cmdr.Parse(docr))
                {
                    if (Convert.ToBoolean(ConfigurationManager.AppSettings["check_cloud_id"]) &&
                        cmdr.CloudId != ConfigurationManager.AppSettings["valid_cloud_id"])
                    {
                        OnClose();    // close session
                        return;
                    }

                    // register success
                    string token = _loginManager.GetRegisterInfo(cmdr.terminalType, cmdr.deviceSerialNo);
                    if (token != null)
                    {
                        CmdRegisterResponse cmd_resp = new CmdRegisterResponse(cmdr.deviceSerialNo, token);
                        m_session.Send(cmd_resp.Build());
                    }

                    registerMsgRecved = true;
                    registerMsgTime = Environment.TickCount;

                    deviceModel = cmdr.terminalType;
                    deviceId = cmdr.deviceSerialNo;
                }
                else
                {
                    CmdLogin cmd = new CmdLogin();
                    XmlDocument doc = new XmlDocument();
                    doc.LoadXml(message);

                    if (cmd.Parse(doc))
                    {
                        if (_loginManager.Verify(cmd.deviceSerialNo, cmd.token))
                        {
                            m_session.Send(new CmdLoginResponse(cmd.deviceSerialNo).Build());

                            loggedIn = true;
                            deviceId = cmd.deviceSerialNo;
                        }
                    }
                }
            }
            else
            {
                CommandRequest? request = popCommandRequest(1);
                if (request == null)
                {
                    //                   throw new WebDeviceException("Invalid message ID. The command may have been already canceled.");
                    XmlDocument recvXml = new XmlDocument();
                    recvXml.LoadXml(message);

                    if (EvtTimeLog.ParseTag(recvXml, "Event") == "TimeLog" || EvtAdminLog.ParseTag(recvXml, "Event") == "AdminLog")
                    {
                        _loginManager.OnDeviceEvent(recvXml);

                        XmlDocument result = new XmlDocument();

                        if (EvtTimeLog.ParseTag(recvXml, "Event") == "TimeLog")
                            result.LoadXml(@"<?xml version=""1.0""?><Message><Response>TimeLog</Response><Result>OK</Result></Message>");
                        else if (EvtAdminLog.ParseTag(recvXml, "Event") == "AdminLog")
                            result.LoadXml(@"<?xml version=""1.0""?><Message><Response>AdminLog</Response><Result>OK</Result></Message>");
                      
                        m_session.Send(result.OuterXml);
                    }
                    else if (EvtTimeLog_v2.ParseTag(recvXml, "Event") == "TimeLog_v2" || EvtAdminLog_v2.ParseTag(recvXml, "Event") == "AdminLog_v2")
                    {
                        _loginManager.OnDeviceEvent(recvXml);

                        String TransID;
                        String EventType;
                        if (EvtTimeLog_v2.ParseTag(recvXml, "Event") == "TimeLog_v2")
                        {
                            TransID = EvtTimeLog_v2.ParseTag(recvXml, "TransID");
                            EventType = EvtTimeLog_v2.ParseTag(recvXml, "Event");
                        }
                        else
                        {
                            TransID = EvtAdminLog_v2.ParseTag(recvXml, "TransID");
                            EventType = EvtAdminLog_v2.ParseTag(recvXml, "Event");
                        }

                        String xml_str = @"<?xml version=""1.0""?><Message>";
                        xml_str = xml_str + "<Response>" + EventType + "</Response>";
                        xml_str = xml_str + "<Result>OK</Result>";
                        xml_str = xml_str + "<TransID>" + TransID + "</TransID>";
                        xml_str = xml_str + "</Message>";

                        XmlDocument result = new XmlDocument();
                        result.LoadXml(xml_str);
                       
						m_session.Send(result.OuterXml);
                    }
                }
                else
                {
                    XmlDocument response;
                    try
                    {
                        lock (_state)
                        {
                            response = new XmlDocument();
                            response.LoadXml(message);
                        }
                    }
                    catch (Exception ex)
                    {
                        request.Value.ar.setException(new WebDeviceException("Error while parsing response.", ex));
                        request.Value.ar.complete(false);
                        return;
                    }

                    request.Value.ar.setValue(response);
                    request.Value.ar.complete(false);
                }
            }
        }

        public /*override*/ void OnClose()
        {
        //    base.OnClose();
            SessionRegistry.RemoveSession(m_guid);
            lock (_monitor)
            {
                _Guids.Remove(m_session.SessionID);
            }
        }

        public /*override*/ void OnError()
        {
        //    base.OnError();
        }

        public XmlDocument EndCommand(IAsyncResult ar)
        {
            return ((Async)ar).endOperation();
        }

        public void CancelCommand(IAsyncResult ar, Exception error)
        {
            Async async = (Async)ar;
            lock (_state)
            {
                if (!_commandsWaitingForResponses.Remove(async.message_id))
                    async = null;
            }

            if (async != null)
            {
                async.setException(error);
                async.complete(false);
            }
        }

        public void ExecuteCommand(System.Web.UI.Page page, XmlDocument request, DeviceResponseHandler responseHandler)
        {
            page.RegisterAsyncTask(new System.Web.UI.PageAsyncTask(
                (sender, e, cb, extraData) => BeginCommand(request, cb, extraData),
                (ar) => { using (var resp = new DeviceResponse(this, ar)) responseHandler(resp); },
                null,//(ar) => CancelCommand(ar, new TimeoutException()),
                null));
        }

        public IAsyncResult BeginCommand(XmlDocument message, AsyncCallback cb, object extra)
        {
            return BeginCommand(message, cb, extra, 5000);////Timeout.Infinite);
        }
        public IAsyncResult BeginCommand(XmlDocument message, AsyncCallback cb, object extra, int millisecondsTimeout)
        {
            Async ar = new Async(cb, extra);

            if (millisecondsTimeout != Timeout.Infinite)
                ar.timeout_timer = new Timer(ar.on_timeout, this, millisecondsTimeout, Timeout.Infinite);

            CommandRequest request;
            request.message = message;
            request.ar = ar;

            lock (_state)
            {
                ar.message_id = 1;// _nextTransactionId;
                ///_nextTransactionId = (_nextTransactionId + 1) & MsgId_SequenceMask;
                _nextTransactionId = _nextTransactionId + 1;


                // Add command to wait-response queue.
                _commandsWaitingForResponses.Add(request.ar.message_id, request);

            }

            bool succeeded = false;
            try
            {
                m_session.Send(message.OuterXml);

                succeeded = true;
                return ar;
            }
            finally
            {
                if (!succeeded)
                {
                    lock (_state)
                    {
                        // Remove the command from wait-polling queue.
//                        _commandsWaitingForResponses.Remove(request.ar.message_id);
                    }

                    lock (ar.monitor)
                    {
                        if (ar.timeout_timer != null)
                        {
                            ar.timeout_timer.Dispose();
                            ar.timeout_timer = null;
                        }
                    }
                }
            }
        }
        internal CommandRequest? popCommandRequest(uint message_id)
        {
            lock (_state)
            {
                CommandRequest result;
                if (_commandsWaitingForResponses.TryGetValue(message_id, out result))
                {
                    _commandsWaitingForResponses.Remove(message_id);
                    return result;
                }
                else
                    return null;
            }
        }

        internal class Async : AsyncResultWithValue<XmlDocument>
        {
            public Async(AsyncCallback cb, object extra)
                : base(cb, extra)
            { }

//            public LinkedListNode<CommandRequest> queueNode;
            public uint message_id;
            public Timer timeout_timer;

            public void on_timeout(object param)
            {
                ((SBWebSocketHandler)param).CancelCommand(this, new TimeoutException());
            }

            public override void complete(bool isSynchronous)
            {
                base.complete(isSynchronous);

                lock (monitor)
                {
                    if (timeout_timer != null)
                    {
                        timeout_timer.Dispose();
                        timeout_timer = null;
                    }
                }
            }
        }

    }

    public class DeviceResponse : IDisposable
    {
        private SBWebSocketHandler _session;
        private IAsyncResult _ar;
        private XmlDocument _xml;

        internal DeviceResponse(SBWebSocketHandler session, IAsyncResult ar)
        {
            _session = session;
            _ar = ar;
        }

        /// <summary>
        /// Message contents.
        /// </summary>
        public XmlDocument Xml
        {
            get
            {
                if (_ar != null)
                {
                    IAsyncResult ar = _ar;
                    _ar = null;

                    _xml = _session.EndCommand(ar);
                }

                if (_xml == null)
                    throw new NullReferenceException();

                return _xml;
            }
        }

        public void Dispose()
        {
            if (_ar != null)
            {
                try
                {
                    _session.EndCommand(_ar); 
                }
                catch { }
                _ar = null;
            }
        }
    }

    /// <summary>
    /// Device-side triggered event handler.
    /// </summary>
    /// <param name="message">Event contents</param>
    /// <returns>Response to be returned to device</returns>
    public delegate XmlDocument DeviceEventHandler(XmlDocument message);

    /// <summary>
    /// Device response handler.
    /// </summary>
    /// <param name="response">Response from device</param>
    public delegate void DeviceResponseHandler(DeviceResponse response);

}
