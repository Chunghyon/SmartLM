using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using System.Web.Script.Serialization;
using A3_8190_HTTPServer.Models;

namespace A3_8190_HTTPServer.HttpServer
{
    // ─── Event Delegates ────────────────────────────────────────────────────
    public delegate void LogEventHandler(string message);
    public delegate void KeepaliveEventHandler(string sn, KeepaliveRequest req);
    public delegate void RecordEventHandler(string sn, RecordDetail record);

    /// <summary>
    /// A3-8190 HTTP 프로토콜 서버 (디바이스가 이 서버에 접속)
    ///
    /// 통신 흐름:
    ///   1. 디바이스가 주기적으로 POST /Device/Keepalive 전송
    ///   2. 서버 응답에 AddPeople/DeletePeople/Remote 플래그 포함 시 디바이스가 추가 요청
    ///   3. 인원 추가: /People/DownloadPeopleList 요청 → 서버가 PersonnelList 반환
    ///   4. 인원 삭제: /DevicePass/SelectDeleteInfo 요청 → 서버가 DeleteList 반환
    ///   5. 원격 제어: /Device/RemoteCommand 요청 → 서버가 명령 반환
    ///   6. 출입 기록: /Record/UploadIdentifyRecord (multipart/form-data) 수신
    /// </summary>
    public class A3HttpListener
    {
        private HttpListener     _listener;
        private Thread           _listenerThread;
        private volatile bool    _running;
        private readonly JavaScriptSerializer _json = new JavaScriptSerializer { MaxJsonLength = int.MaxValue };

        // ─── Shared State ────────────────────────────────────────────────────
        private readonly Dictionary<string, int>                 _pendingAdd    = new Dictionary<string, int>();
        private readonly Dictionary<string, int>                 _pendingDelete = new Dictionary<string, int>();
        private readonly Dictionary<string, PendingRemoteCommand> _pendingRemote = new Dictionary<string, PendingRemoteCommand>();

        /// <summary>전송 대기 중인 인원 목록 (스레드 세이프: lock(PersonnelList))</summary>
        public List<PersonInfo>   PersonnelList { get; } = new List<PersonInfo>();
        /// <summary>삭제 대기 중인 UserID 목록 (스레드 세이프: lock(DeleteList))</summary>
        public List<string>       DeleteList    { get; } = new List<string>();
        /// <summary>수신된 출입 기록 목록 (스레드 세이프: lock(Records))</summary>
        public List<(string SN, RecordDetail Record)> Records { get; } = new List<(string, RecordDetail)>();
        /// <summary>연결된 디바이스 SN 목록</summary>
        public HashSet<string>    KnownDevices  { get; } = new HashSet<string>();

        // ─── Events ──────────────────────────────────────────────────────────
        public event LogEventHandler       OnLog;
        public event KeepaliveEventHandler OnKeepalive;
        public event RecordEventHandler    OnRecord;

        public bool IsRunning => _running;

        // ─── Start / Stop ────────────────────────────────────────────────────
        public void Start(int port)
        {
            _listener = new HttpListener();
            _listener.Prefixes.Add($"http://+:{port}/");
            _listener.Start();
            _running = true;
            _listenerThread = new Thread(ListenLoop) { IsBackground = true, Name = "A3HttpListener" };
            _listenerThread.Start();
            Log($"HTTP 서버 시작됨  포트 {port}  URL: http://+:{port}/");
            Log("디바이스에서 이 PC의 IP:{port} 로 서버 주소를 설정하세요.");
        }

        public void Stop()
        {
            _running = false;
            try { _listener?.Stop(); } catch { }
            Log("HTTP 서버 중지됨");
        }

        // ─── Listener Loop ───────────────────────────────────────────────────
        private void ListenLoop()
        {
            while (_running)
            {
                try
                {
                    var ctx = _listener.GetContext();
                    ThreadPool.QueueUserWorkItem(HandleRequest, ctx);
                }
                catch (HttpListenerException) when (!_running) { /* 정상 종료 */ }
                catch (Exception ex) { Log($"[오류] 요청 수신 실패: {ex.Message}"); }
            }
        }

        // ─── Request Dispatcher ──────────────────────────────────────────────
        private void HandleRequest(object state)
        {
            var ctx = (HttpListenerContext)state;
            var req = ctx.Request;
            var res = ctx.Response;
            string responseBody = "{\"Success\":0}";

            try
            {
                string path = req.Url.AbsolutePath.ToLower().TrimEnd('/');
                Log($"← [{req.HttpMethod}] {req.Url.AbsolutePath}  ({req.RemoteEndPoint})");

                switch (path)
                {
                    case "/device/keepalive":
                        responseBody = HandleKeepalive(ReadBody(req));
                        break;
                    case "/people/downloadpeoplelist":
                    case "/devicepass/selectpassinfo":
                        responseBody = HandleDownloadPeopleList(ReadBody(req));
                        break;
                    case "/devicepass/selectdeleteinfo":
                    case "/people/selectdeleteinfo":
                        responseBody = HandleSelectDeleteInfo(ReadBody(req));
                        break;
                    case "/device/remotecommand":
                    case "/device/setrestart":
                        responseBody = HandleRemoteCommand(ReadBody(req));
                        break;
                    case "/record/uploadidentifyrecord":
                        responseBody = HandleUploadRecord(req);
                        break;
                    case "/people/downloadpeoplelistresult":
                    case "/people/pushpeople":
                    case "/record/uploadsystemrecord":
                    case "/device/uploadworkparameter":
                    case "/device/downloadworksetting":
                        // 응답만 확인, 내용 저장 불필요
                        responseBody = "{\"Success\":0}";
                        break;
                    default:
                        Log($"  [미처리] {req.Url.AbsolutePath}");
                        responseBody = "{\"Success\":0}";
                        break;
                }

                WriteResponse(res, responseBody);
                Log($"→ {responseBody.Length > 120 ? responseBody.Substring(0, 120) + "…" : responseBody}");
            }
            catch (Exception ex)
            {
                Log($"[오류] 요청 처리 중 예외: {ex.Message}");
                try { WriteResponse(res, "{\"Success\":500,\"Message\":\"Internal Server Error\"}"); } catch { }
            }
        }

        // ─── API Handlers ────────────────────────────────────────────────────

        private string HandleKeepalive(string body)
        {
            KeepaliveRequest req;
            try { req = _json.Deserialize<KeepaliveRequest>(body); }
            catch { return "{\"Success\":400,\"Message\":\"JSON parse error\"}"; }

            if (req == null || string.IsNullOrWhiteSpace(req.SN))
                return "{\"Success\":400,\"Message\":\"SN required\"}";

            lock (KnownDevices) { KnownDevices.Add(req.SN); }
            OnKeepalive?.Invoke(req.SN, req);

            var resp = new KeepaliveResponse { Success = 0 };
            lock (_pendingAdd)
            {
                if (_pendingAdd.TryGetValue(req.SN, out int cnt) && cnt > 0)
                    resp.AddPeople = cnt;
            }
            lock (_pendingDelete)
            {
                if (_pendingDelete.TryGetValue(req.SN, out int cnt) && cnt > 0)
                    resp.DeletePeople = cnt;
            }
            lock (_pendingRemote)
            {
                if (_pendingRemote.ContainsKey(req.SN))
                    resp.Remote = 1;
            }
            return _json.Serialize(resp);
        }

        private string HandleDownloadPeopleList(string body)
        {
            DownloadPeopleListRequest req;
            try { req = _json.Deserialize<DownloadPeopleListRequest>(body); }
            catch { return "{\"Success\":400}"; }

            if (req == null) return "{\"Success\":400}";

            List<PersonInfo> list;
            lock (PersonnelList) { list = new List<PersonInfo>(PersonnelList); }

            // 전송 완료 후 플래그 클리어
            lock (_pendingAdd)
            {
                if (_pendingAdd.ContainsKey(req.SN ?? "")) _pendingAdd[req.SN] = 0;
            }

            var resp = new DownloadPeopleListResponse
            {
                Success     = 0,
                PeopleCount = list.Count,
                PeopleList  = list
            };
            Log($"  ↑ 인원 {list.Count}명 전송 → {req.SN}");
            return _json.Serialize(resp);
        }

        private string HandleSelectDeleteInfo(string body)
        {
            SelectDeleteInfoRequest req;
            try { req = _json.Deserialize<SelectDeleteInfoRequest>(body); }
            catch { return "{\"Success\":400}"; }

            List<string> ids;
            lock (DeleteList) { ids = new List<string>(DeleteList); DeleteList.Clear(); }
            lock (_pendingDelete)
            {
                if (_pendingDelete.ContainsKey(req?.SN ?? "")) _pendingDelete[req.SN] = 0;
            }

            var resp = new SelectDeleteInfoResponse { Success = 0, DeleteList = ids };
            Log($"  ↑ 삭제 UserID {ids.Count}건 전송 → {req?.SN}");
            return _json.Serialize(resp);
        }

        private string HandleRemoteCommand(string body)
        {
            RemoteCommandRequest req;
            try { req = _json.Deserialize<RemoteCommandRequest>(body); }
            catch { return "{\"Success\":400}"; }

            PendingRemoteCommand cmd = null;
            lock (_pendingRemote)
            {
                if (req != null && _pendingRemote.TryGetValue(req.SN ?? "", out cmd))
                    _pendingRemote.Remove(req.SN);
            }

            if (cmd == null) return "{\"Success\":0}";

            var resp = new RemoteCommandResponse
            {
                Success     = 0,
                Opendoor    = cmd.Opendoor,
                Restart     = cmd.Restart,
                Recover     = cmd.Recover,
                Closealarm  = cmd.Closealarm,
                ClearRecord = cmd.ClearRecord
            };
            return _json.Serialize(resp);
        }

        private string HandleUploadRecord(HttpListenerRequest httpReq)
        {
            string sn = "";
            RecordDetail record = null;

            try
            {
                if (httpReq.ContentType != null && httpReq.ContentType.IndexOf("multipart", StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    // multipart/form-data 파싱
                    string rawBody = ReadBody(httpReq);
                    sn = ExtractFormField(rawBody, "SN") ?? "";

                    // 필드명: recordJson 또는 RecordDetail
                    string recJson = ExtractFormField(rawBody, "recordJson")
                                  ?? ExtractFormField(rawBody, "RecordDetail")
                                  ?? ExtractFormField(rawBody, "record");
                    if (!string.IsNullOrEmpty(recJson))
                        record = TryDeserialize<RecordDetail>(recJson);
                }
                else
                {
                    string body = ReadBody(httpReq);
                    var dict = TryDeserialize<Dictionary<string, object>>(body);
                    if (dict != null)
                    {
                        if (dict.TryGetValue("SN", out object snObj)) sn = snObj?.ToString() ?? "";
                        record = TryDeserialize<RecordDetail>(body);
                    }
                }
            }
            catch (Exception ex)
            {
                Log($"  [오류] 기록 파싱 실패: {ex.Message}");
            }

            if (record != null)
            {
                lock (Records) { Records.Add((sn, record)); }
                OnRecord?.Invoke(sn, record);
                Log($"  ★ 출입기록  UserID={record.UserID} Name={record.Name}  " +
                    $"Type={RecordDetail.GetRecordTypeText(record.RecordType)}  " +
                    $"{'입' }{(record.IsEntry == 1 ? "입실" : "퇴실")}  " +
                    $"{record.GetDateTime():HH:mm:ss}");
            }
            return "{\"Success\":0}";
        }

        // ─── Queue Commands (UI → Server → Device) ───────────────────────────

        /// <summary>다음 Keepalive 응답에 AddPeople 플래그를 설정</summary>
        public void QueueAddPeople(string sn, int count = 1)
        {
            lock (_pendingAdd) { _pendingAdd[sn] = count; }
            Log($"[대기] {sn} ← 인원 추가 예약 ({count}명)  다음 Keepalive 수신 시 전송");
        }

        /// <summary>다음 Keepalive 응답에 DeletePeople 플래그를 설정</summary>
        public void QueueDeletePeople(string sn)
        {
            lock (_pendingDelete) { _pendingDelete[sn] = 1; }
            Log($"[대기] {sn} ← 인원 삭제 예약  다음 Keepalive 수신 시 전송");
        }

        /// <summary>다음 Keepalive 응답에 Remote 플래그를 설정, 명령 내용 저장</summary>
        public void QueueRemoteCommand(string sn, PendingRemoteCommand cmd)
        {
            lock (_pendingRemote) { _pendingRemote[sn] = cmd; }
            string detail = "";
            if (cmd.Opendoor > 0)    detail += $"문열기={cmd.Opendoor} ";
            if (cmd.Restart  > 0)    detail += "재시작 ";
            if (cmd.Recover  > 0)    detail += "초기화 ";
            if (cmd.Closealarm > 0)  detail += "알람해제 ";
            if (cmd.ClearRecord > 0) detail += "기록삭제 ";
            Log($"[대기] {sn} ← 원격명령 예약: {detail.Trim()}");
        }

        // ─── Helpers ─────────────────────────────────────────────────────────
        private static void WriteResponse(HttpListenerResponse res, string body)
        {
            byte[] buf = Encoding.UTF8.GetBytes(body);
            res.ContentType     = "application/json; charset=utf-8";
            res.ContentLength64 = buf.Length;
            res.OutputStream.Write(buf, 0, buf.Length);
            res.OutputStream.Close();
        }

        private static string ReadBody(HttpListenerRequest req)
        {
            if (!req.HasEntityBody) return "";
            using (var sr = new StreamReader(req.InputStream, req.ContentEncoding ?? Encoding.UTF8))
                return sr.ReadToEnd();
        }

        private T TryDeserialize<T>(string json) where T : class
        {
            try { return string.IsNullOrWhiteSpace(json) ? null : _json.Deserialize<T>(json); }
            catch { return null; }
        }

        /// <summary>단순 multipart/form-data 파싱 (단일 필드 추출)</summary>
        private static string ExtractFormField(string body, string fieldName)
        {
            string marker = $"name=\"{fieldName}\"";
            int pos = body.IndexOf(marker, StringComparison.OrdinalIgnoreCase);
            if (pos < 0) return null;

            pos += marker.Length;
            // Skip optional header lines until blank line
            int start = body.IndexOf("\r\n\r\n", pos, StringComparison.Ordinal);
            if (start < 0) { start = body.IndexOf("\n\n", pos, StringComparison.Ordinal); if (start < 0) return null; start += 2; } else start += 4;

            // Find next boundary
            int end = body.IndexOf("\r\n--", start, StringComparison.Ordinal);
            if (end < 0) end = body.IndexOf("\n--", start, StringComparison.Ordinal);
            if (end < 0) return null;

            return body.Substring(start, end - start).Trim();
        }

        private void Log(string msg) => OnLog?.Invoke($"[{DateTime.Now:HH:mm:ss}] {msg}");
    }
}
