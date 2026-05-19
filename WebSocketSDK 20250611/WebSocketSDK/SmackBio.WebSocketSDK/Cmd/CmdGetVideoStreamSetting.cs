using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using SmackBio.WebSocketSDK.DB;

namespace SmackBio.WebSocketSDK.Cmd
{
    public class CmdGetVideoStreamSetting : CmdBase
    {
        public const string MSG_KEY = "GetVideoStreamSetting";

        public CmdGetVideoStreamSetting() : base() { }

        public override string Build()
        {
            string result = StartBuild();
            AppendTag(ref result, TAG_REQUEST, MSG_KEY);
            AppendEndup(ref result);

            return result;
        }

        public override Type GetResponseType()
        {
            return typeof(CmdGetVideoStreamSettingResponse);
        }
    }

    public class CmdGetVideoStreamSettingResponse : Response 
    {
        public RtspSetting setting = new RtspSetting();

        public CmdGetVideoStreamSettingResponse() { }

        public override bool Parse(XmlDocument doc)
        {
            var rtsp_enable_str = ParseTag(doc, "rtsp_enable");
            if (rtsp_enable_str != null)
                setting.rtsp_enable = Convert.ToInt32(rtsp_enable_str);

            var rtsp_resolution_str = ParseTag(doc, "rtsp_resolution");
            if (rtsp_resolution_str != null)
                setting.rtsp_resolution = Convert.ToInt32(rtsp_resolution_str);

            var rtsp_bitrate_mbps_str = ParseTag(doc, "rtsp_bitrate_mbps");
            if (rtsp_bitrate_mbps_str != null)
                setting.rtsp_bitrate_mbps = Convert.ToInt32(rtsp_bitrate_mbps_str);

            return true;
        }
    }
}
