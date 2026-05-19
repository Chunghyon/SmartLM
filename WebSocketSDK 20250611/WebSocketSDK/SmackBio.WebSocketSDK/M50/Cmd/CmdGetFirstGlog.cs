using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using SmackBio.WebSocketSDK.Cmd;
using SmackBio.WebSocketSDK.Util;

namespace SmackBio.WebSocketSDK.M50.Cmd
{
    public class CmdGetFirstGlog : CmdBase
    {
        public const string MSG_KEY = "GetFirstGlog";
        public Int64? user_id { get; set; }
        public DateTime? start_time { get; set; }
        public DateTime? end_time { get; set; }

        public CmdGetFirstGlog(Int64? user_id, DateTime? start_time, DateTime? end_time)
            : base()
        {
            this.user_id = user_id;
            this.start_time = start_time;
            this.end_time = end_time;
        }

        public override string Build()
        {
            string result = StartBuild();
            AppendTag(ref result, TAG_REQUEST, MSG_KEY);
            AppendTag(ref result, "BeginLogPos", 0);
            if (user_id != null)
                AppendTag(ref result, "UserID", user_id);
            if (start_time != null)
                AppendTag(ref result, "StartTime", Utils.DateTime2string(start_time.GetValueOrDefault(DateTime.Now)));
            if (end_time != null)
                AppendTag(ref result, "EndTime", Utils.DateTime2string(end_time.GetValueOrDefault(DateTime.Now)));
            AppendEndup(ref result);

            return result;
        }

        public override Type GetResponseType()
        {
            return typeof(CmdGetNextGlogResponse);
        }
    }
}
