using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using SmackBio.WebSocketSDK.Cmd;
using SmackBio.WebSocketSDK.DB;
using SmackBio.WebSocketSDK.M50;

namespace SmackBio.WebSocketSDK.Sample.Pages
{
    public partial class VideoStremingSettingPane : System.Web.UI.Page
    {
        const uint CENTER_SCREEN_MSG_DEF_COLOR = 0x54B248;
        const uint CENTER_SCREEN_MSG_DEF_BORDER_COLOR = 0xFFFFFF;

        private uint validate_msg_color(TextBox tb, uint defColor)
        {
            uint color;
            try
            {
                color = Convert.ToUInt32(tb.Text, 16);
            }
            catch
            {
                color = defColor;
            }
            color &= 0xFFFFFF;
            tb.Text = color.ToString("X6");
            return color;
        }

        static readonly List<string> rtsp_resolution_items = new List<string> { };
        static readonly List<string> rtsp_bitrate_items = new List<string> { };
        static VideoStremingSettingPane()
        {
            rtsp_resolution_items.Add("1920x1080");
            rtsp_resolution_items.Add("1280x720");
            rtsp_resolution_items.Add("960x540");
            rtsp_resolution_items.Add("640x360");

            rtsp_bitrate_items.Add("5 (default)");
            for(int i=1; i<=20; i++)
                rtsp_bitrate_items.Add(i.ToString());
        }
        public static List<string> GetRtspResolutionList()
        {
            return rtsp_resolution_items;
        }
        public static List<string> GetRtspBitrateList()
        {
            return rtsp_bitrate_items;
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            txtMessage.Text = "";
            error_message.Text = "";

            var sid = Context.Request.Params["session_id"];
            if (!string.IsNullOrEmpty(sid))
                session_id.Text = sid;
            else
                Context.Response.Redirect("~/ViewOnlineDevices.aspx");
        }
        protected void btnGetRtspSetting_Click(object sender, EventArgs e)
        {
            CmdGetVideoStreamSetting cmd = new CmdGetVideoStreamSetting();
            try
            {
                var session = SessionRegistry.GetSession(Guid.Parse(session_id.Text)); ;

                XmlDocument doc = new XmlDocument();
                doc.LoadXml(cmd.Build());

                session.ExecuteCommand(this, doc, (response) =>
                {
                    CmdGetVideoStreamSettingResponse cmd_resp = new CmdGetVideoStreamSettingResponse();
                    if (cmd_resp.Parse(response.Xml))
                    {
                        chkRtspEnable.Checked = cmd_resp.setting.rtsp_enable != 0;
                        cmbRtspResolution.SelectedIndex = cmd_resp.setting.rtsp_resolution;
                        cmbRtspBitrate.SelectedIndex = cmd_resp.setting.rtsp_bitrate_mbps;

                        txtMessage.Text = "Get VideoStreamSetting OK";
                    }
                    else
                        txtMessage.Text = "Get VideoStreamSetting Failed";
                }, (ex) => { error_message.Text = ex.Message; });
            }
            catch (Exception ex)
            {
                error_message.Text = ex.Message;
            }
        }
        protected void btnSetRtspSetting_Click(object sender, EventArgs e)
        {
            try
            {
                RtspSetting rtsp_setting = new RtspSetting();

                rtsp_setting.rtsp_enable = chkRtspEnable.Checked ? 1 : 0;
                rtsp_setting.rtsp_resolution = cmbRtspResolution.SelectedIndex;
                rtsp_setting.rtsp_bitrate_mbps = cmbRtspBitrate.SelectedIndex;

                CmdSetVideoStreamSetting cmd = new CmdSetVideoStreamSetting(rtsp_setting);
                var session = SessionRegistry.GetSession(Guid.Parse(session_id.Text)); ;

                XmlDocument doc = new XmlDocument();
                doc.LoadXml(cmd.Build());

                session.ExecuteCommand(this, doc, (response) =>
                {
                    txtMessage.Text = "Set VideoStreamSetting Failed.";
                    if (BaseMessage.IsResponseKey(response.Xml, CmdSetVideoStreamSetting.MSG_KEY))
                    {
                        GeneralResponse re = new GeneralResponse();
                        if (re.ParseResult(response.Xml) == CommandExeResult.OK)
                            txtMessage.Text = "Set VideoStreamSetting OK!";
                    }
                }, (ex) => { error_message.Text = ex.Message; });
            }
            catch (Exception ex)
            {
                error_message.Text = ex.Message;
            }
        }

        protected void btnGetCenterScreenMsg_Click(object sender, EventArgs e)
        {
            CmdGetCenterScreenMessageSetting cmd = new CmdGetCenterScreenMessageSetting();
            try
            {
                var session = SessionRegistry.GetSession(Guid.Parse(session_id.Text));

                XmlDocument doc = new XmlDocument();
                doc.LoadXml(cmd.Build());

                session.ExecuteCommand(this, doc, (response) =>
                {
                    CmdGetCenterScreenMessageSettingResponse cmd_resp = new CmdGetCenterScreenMessageSettingResponse();
                    if (cmd_resp.Parse(response.Xml))
                    {
                        chkVerifyDisable.Checked = cmd_resp.setting.verify_disable != 0;
                        txtCenterScreenMsg.Text = cmd_resp.setting.center_screen_message;
                        txtTextColor.Text = cmd_resp.setting.center_screen_message_color.ToString("X6");
                        txtTextBorderColor.Text = cmd_resp.setting.center_screen_message_border_color.ToString("X6");

                        txtMessage.Text = "Get CenterScreenMessageSetting OK";
                    }
                    else
                        txtMessage.Text = "Get CenterScreenMessageSetting Failed";
                }, (ex) => { error_message.Text = ex.Message; });
            }
            catch (Exception ex)
            {
                error_message.Text = ex.Message;
            }
        }

        protected void btnSetCenterScreenMsg_Click(object sender, EventArgs e)
        {
            try
            {
                CenterScreenMessageSetting msg_setting = new CenterScreenMessageSetting();

                msg_setting.verify_disable = chkVerifyDisable.Checked ? 1 : 0;
                msg_setting.center_screen_message = txtCenterScreenMsg.Text;
                msg_setting.center_screen_message_color = validate_msg_color(txtTextColor, CENTER_SCREEN_MSG_DEF_COLOR);
                msg_setting.center_screen_message_border_color = validate_msg_color(txtTextBorderColor, CENTER_SCREEN_MSG_DEF_BORDER_COLOR);

                CmdSetCenterScreenMessageSetting cmd = new CmdSetCenterScreenMessageSetting(msg_setting);
                var session = SessionRegistry.GetSession(Guid.Parse(session_id.Text));

                XmlDocument doc = new XmlDocument();
                doc.LoadXml(cmd.Build());

                session.ExecuteCommand(this, doc, (response) =>
                {
                    txtMessage.Text = "Set CenterScreenMessageSetting Failed.";
                    if (BaseMessage.IsResponseKey(response.Xml, CmdSetCenterScreenMessageSetting.MSG_KEY))
                    {
                        GeneralResponse re = new GeneralResponse();
                        if (re.ParseResult(response.Xml) == CommandExeResult.OK)
                            txtMessage.Text = "Set CenterScreenMessageSetting OK!";
                    }
                }, (ex) => { error_message.Text = ex.Message; });
            }
            catch (Exception ex)
            {
                error_message.Text = ex.Message;
            }
        }
    }
}