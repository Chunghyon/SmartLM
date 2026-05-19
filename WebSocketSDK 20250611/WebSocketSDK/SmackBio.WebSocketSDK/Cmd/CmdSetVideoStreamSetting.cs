using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using SmackBio.WebSocketSDK.DB;

namespace SmackBio.WebSocketSDK.Cmd
{
    public class CmdSetVideoStreamSetting : CmdBase
    {
        public const string MSG_KEY = "SetVideoStreamSetting";
        RtspSetting setting;
        
        public CmdSetVideoStreamSetting(RtspSetting setting) 
            : base()
        {
            this.setting = setting;
        }

        public override string Build()
        {
            string result = StartBuild();
            AppendTag(ref result, TAG_REQUEST, MSG_KEY);
            AppendTag(ref result, "rtsp_enable", setting.rtsp_enable);
            AppendTag(ref result, "rtsp_resolution", setting.rtsp_resolution);
            AppendTag(ref result, "rtsp_bitrate_mbps", setting.rtsp_bitrate_mbps);

            AppendEndup(ref result);

            return result;
        }
        public override Type GetResponseType()
        {
            return typeof(GeneralResponse);
        }
 
    }
}
