using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using System.IO;
using SmackBio.WebSocketSDK.Cmd;
using SmackBio.WebSocketSDK.DB;
using SmackBio.WebSocketSDK.M50;
using SmackBio.WebSocketSDK.M50.Cmd;
using SmackBio.WebSocketSDK.Util;

namespace SmackBio.WebSocketSDK.Sample.Pages
{
    public partial class UserManageCustomPane : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var sid = Context.Request.Params["session_id"];
            var dev_uid = Context.Request.Params["device_uid"];
            if (!string.IsNullOrEmpty(sid))
            {
                session_id.Text = sid;
                device_uid.Text = dev_uid;
            }
            else
                Context.Response.Redirect("~/ViewOnlineDevices.aspx");
        }

        ////////////////////////////////////////////////////////////////////////////////
        protected void btnGetUserAttendOnly_Click(object sender, EventArgs e)
        {
            try
            {
                CmdGetUserAttendOnly cmd = new CmdGetUserAttendOnly(Convert.ToInt64(TextUserID.Text));
                try
                {
                    var session = SessionRegistry.GetSession(Guid.Parse(session_id.Text));

                    XmlDocument doc = new XmlDocument();
                    doc.LoadXml(cmd.Build());

                    session.ExecuteCommand(this, doc, (response) =>
                    {
                        CmdGetUserAttendOnlyResponse cmd_resp = new CmdGetUserAttendOnlyResponse();
                        if (cmd_resp.Parse(response.Xml))
                        {
                            TextMessage.Text = "GetUserAttendOnly Success!";
                            ChkAttendOnly.Checked = cmd_resp.Value;
                        }
                        else
                            TextMessage.Text = "GetUserAttendOnly Failed!";
                    }, (ex) => { TextMessage.Text = ex.Message; });
                }
                catch (Exception ex)
                {
                    TextMessage.Text = ex.Message;
                }
            }
            catch (Exception)
            {
                TextMessage.Text = "Please Input UserID Correctly!";
                TextUserID.Focus();
            }
        }

        protected void btnSetUserAttendOnly_Click(object sender, EventArgs e)
        {
            try
            {
                CmdSetUserAttendOnly cmd = new CmdSetUserAttendOnly(Convert.ToInt64(TextUserID.Text),
                    ChkAttendOnly.Checked);
                try
                {
                    var session = SessionRegistry.GetSession(Guid.Parse(session_id.Text));

                    XmlDocument doc = new XmlDocument();
                    doc.LoadXml(cmd.Build());

                    session.ExecuteCommand(this, doc, (response) =>
                    {
                        CmdSetUserAttendOnlyResponse cmd_resp = new CmdSetUserAttendOnlyResponse();
                        if (cmd_resp.Parse(response.Xml))
                            TextMessage.Text = "SetUserAttendOnly Success!";
                        else
                            TextMessage.Text = "SetUserAttendOnly Failed!";
                    }, (ex) => { TextMessage.Text = ex.Message; });
                }
                catch (Exception ex)
                {
                    TextMessage.Text = ex.Message;
                }
            }
            catch (Exception)
            {
                TextMessage.Text = "Please Input UserID Correctly!";
                TextUserID.Focus();
            }
        }

        protected void btnGetUserMessage_Click(object sender, EventArgs e)
        {
            try
            {
                CmdGetUserMessage cmd = new CmdGetUserMessage(Convert.ToInt64(TextUserID.Text));
                try
                {
                    var session = SessionRegistry.GetSession(Guid.Parse(session_id.Text));

                    XmlDocument doc = new XmlDocument();
                    doc.LoadXml(cmd.Build());

                    session.ExecuteCommand(this, doc, (response) =>
                    {
                        CmdGetUserMessageResponse cmd_resp = new CmdGetUserMessageResponse();
                        if (cmd_resp.Parse(response.Xml))
                        {
                            txtUserMessage.Text = cmd_resp.message;

                            TextMessage.Text = "GetUserMessage OK";
                        }
                        else
                            TextMessage.Text = "GetUserMessage Failed";
                    }, (ex) => { TextMessage.Text = ex.Message; });
                }
                catch (Exception ex)
                {
                    TextMessage.Text = ex.Message;
                }
            }
            catch (Exception)
            {
                TextMessage.Text = "Please Input UserID Correctly!";
                TextUserID.Focus();
            }
        }

        protected void btnSetUserMessage_Click(object sender, EventArgs e)
        {
            try
            {
                CmdSetUserMessage cmd = new CmdSetUserMessage(Convert.ToInt64(TextUserID.Text), txtUserMessage.Text);
                try
                {
                    var session = SessionRegistry.GetSession(Guid.Parse(session_id.Text));

                    XmlDocument doc = new XmlDocument();
                    doc.LoadXml(cmd.Build());

                    session.ExecuteCommand(this, doc, (response) =>
                    {
                        TextMessage.Text = "SetUserMessage Failed.";
                        if (BaseMessage.IsResponseKey(response.Xml, CmdSetUserMessage.MSG_KEY))
                        {
                            GeneralResponse re = new GeneralResponse();
                            if (re.ParseResult(response.Xml) == CommandExeResult.OK)
                                TextMessage.Text = "SetUserMessage OK!";
                        }
                    }, (ex) => { TextMessage.Text = ex.Message; });
                }
                catch (Exception ex)
                {
                    TextMessage.Text = ex.Message;
                }

            }
            catch (Exception)
            {
                TextMessage.Text = "Please Input UserID Correctly!";
                TextUserID.Focus();
            }
        }

        protected void btnGetMessageColor_Click(object sender, EventArgs e)
        {
            CmdGetUserMessageColor cmd = new CmdGetUserMessageColor();
            try
            {
                var session = SessionRegistry.GetSession(Guid.Parse(session_id.Text));

                XmlDocument doc = new XmlDocument();
                doc.LoadXml(cmd.Build());

                session.ExecuteCommand(this, doc, (response) =>
                {
                    CmdGetUserMessageColorResponse cmd_resp = new CmdGetUserMessageColorResponse();
                    if (cmd_resp.Parse(response.Xml))
                    {
                        txtMessageColor.Text = cmd_resp.message_color.ToString("X6");
                        txtMessageBkColor.Text = cmd_resp.message_bk_color.ToString("X6");

                        TextMessage.Text = "GetUserMessageColor OK";
                    }
                    else
                        TextMessage.Text = "GetUserMessageColor Failed";
                }, (ex) => { TextMessage.Text = ex.Message; });
            }
            catch (Exception ex)
            {
                TextMessage.Text = ex.Message;
            }
        }

        const uint MESSAGE_DEF_COLOR = 0x000000;
        const uint MESSAGE_DEF_BK_COLOR = 0xFFFFFF;

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

        protected void btnSetMessageColor_Click(object sender, EventArgs e)
        {
            try
            {
                CmdSetUserMessageColor cmd = new CmdSetUserMessageColor(
                    validate_msg_color(txtMessageColor, MESSAGE_DEF_COLOR),
                    validate_msg_color(txtMessageBkColor, MESSAGE_DEF_BK_COLOR));
                var session = SessionRegistry.GetSession(Guid.Parse(session_id.Text));

                XmlDocument doc = new XmlDocument();
                doc.LoadXml(cmd.Build());

                session.ExecuteCommand(this, doc, (response) =>
                {
                    TextMessage.Text = "SetUserMessageColor Failed.";
                    if (BaseMessage.IsResponseKey(response.Xml, CmdSetUserMessageColor.MSG_KEY))
                    {
                        GeneralResponse re = new GeneralResponse();
                        if (re.ParseResult(response.Xml) == CommandExeResult.OK)
                            TextMessage.Text = "SetUserMessageColor OK!";
                    }
                }, (ex) => { TextMessage.Text = ex.Message; });
            }
            catch (Exception ex)
            {
                TextMessage.Text = ex.Message;
            }
        }
    }
}