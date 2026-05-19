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
    public partial class UserManageGermanPane : System.Web.UI.Page
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

        protected void btnGetUserBalanceTime_Click(object sender, EventArgs e)
        {
            try
            {
                CmdGetUserBalanceTime cmd = new CmdGetUserBalanceTime(Convert.ToInt64(TextUserID.Text));
                try
                {
                    var session = SessionRegistry.GetSession(Guid.Parse(session_id.Text));

                    XmlDocument doc = new XmlDocument();
                    doc.LoadXml(cmd.Build());

                    session.ExecuteCommand(this, doc, (response) =>
                    {
                        CmdGetUserBalanceTimeResponse cmd_resp = new CmdGetUserBalanceTimeResponse();
                        if (cmd_resp.Parse(response.Xml))
                        {
                            txtUserBalanceHour.Text = (cmd_resp.balance_time_in_minutes / 60).ToString();
                            txtUserBalanceMinute.Text = (cmd_resp.balance_time_in_minutes % 60).ToString();

                            TextMessage.Text = "GetUserBalanceTime OK";
                        }
                        else
                            TextMessage.Text = "GetUserBalanceTime Failed";
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

        protected void btnSetUserBalanceTime_Click(object sender, EventArgs e)
        {
            try
            {
                Int64 user_id = Convert.ToInt64(TextUserID.Text);

                try
                {
                    Int32 hour = Convert.ToInt32(txtUserBalanceHour.Text);
                    if (hour < 0 || hour > 1092)
                    {
                        TextMessage.Text = "Balance time should be in range 00:00~1092:35";
                        txtUserBalanceHour.Focus();
                        return;
                    }

                    Int32 minute = Convert.ToInt32(txtUserBalanceMinute.Text);
                    if (minute < 0 || minute > 59)
                    {
                        TextMessage.Text = "Balance time should be in range 00:00~1092:35";
                        txtUserBalanceMinute.Focus();
                        return;
                    }

                    Int32 balance_time = hour * 60 + minute;
                    if (balance_time < 0 || balance_time > 65535)
                    {
                        TextMessage.Text = "Balance time should be in range 00:00~1092:35";
                        txtUserBalanceHour.Focus();
                        return;
                    }

                    CmdSetUserBalanceTime cmd = new CmdSetUserBalanceTime(user_id, balance_time);

                    var session = SessionRegistry.GetSession(Guid.Parse(session_id.Text));

                    XmlDocument doc = new XmlDocument();
                    doc.LoadXml(cmd.Build());

                    session.ExecuteCommand(this, doc, (response) =>
                    {
                        TextMessage.Text = "SetUserBalanceTime Failed.";
                        if (BaseMessage.IsResponseKey(response.Xml, CmdSetUserBalanceTime.MSG_KEY))
                        {
                            GeneralResponse re = new GeneralResponse();

                            if (re.ParseResult(response.Xml) == CommandExeResult.OK)
                                TextMessage.Text = "SetUserBalanceTime OK!";
                        }
                    }, (ex) => { TextMessage.Text = ex.Message; });
                }
                catch (Exception ex)
                {
                    TextMessage.Text = ex.Message;
                    txtUserBalanceHour.Focus();
                }

            }
            catch (Exception)
            {
                TextMessage.Text = "Please Input UserID Correctly!";
                TextUserID.Focus();
            }
        }

        protected void btnGetUserHolidays_Click(object sender, EventArgs e)
        {
            try
            {
                CmdGetUserHolidays cmd = new CmdGetUserHolidays(Convert.ToInt64(TextUserID.Text));
                try
                {
                    var session = SessionRegistry.GetSession(Guid.Parse(session_id.Text));

                    XmlDocument doc = new XmlDocument();
                    doc.LoadXml(cmd.Build());

                    session.ExecuteCommand(this, doc, (response) =>
                    {
                        CmdGetUserHolidaysResponse cmd_resp = new CmdGetUserHolidaysResponse();
                        if (cmd_resp.Parse(response.Xml))
                        {
                            txtUserHolidays.Text = (cmd_resp.holidays_in_10 / 10).ToString() + "." + (cmd_resp.holidays_in_10 % 10).ToString();

                            TextMessage.Text = "GetUserHolidays OK";
                        }
                        else
                            TextMessage.Text = "GetUserHolidays Failed";
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

        protected void btnSetUserHolidays_Click(object sender, EventArgs e)
        {
            try
            {
                Int64 user_id = Convert.ToInt64(TextUserID.Text);
                try
                {
                    Int32 holidays = Convert.ToInt32(Convert.ToDouble(txtUserHolidays.Text) * 10);
                    if (holidays < 0 || holidays > 65535)
                    {
                        TextMessage.Text = "Holidays should be in range 0.0~6553.5!";
                        txtUserHolidays.Focus();
                        return;
                    }

                    CmdSetUserHolidays cmd = new CmdSetUserHolidays(user_id, holidays);

                    var session = SessionRegistry.GetSession(Guid.Parse(session_id.Text));

                    XmlDocument doc = new XmlDocument();
                    doc.LoadXml(cmd.Build());

                    session.ExecuteCommand(this, doc, (response) =>
                    {
                        TextMessage.Text = "SetUserHolidays Failed.";
                        if (BaseMessage.IsResponseKey(response.Xml, CmdSetUserHolidays.MSG_KEY))
                        {
                            GeneralResponse re = new GeneralResponse();
                            if (re.ParseResult(response.Xml) == CommandExeResult.OK)
                                TextMessage.Text = "SetUserHolidays OK!";
                        }
                    }, (ex) => { TextMessage.Text = ex.Message; });
                }
                catch (Exception ex)
                {
                    TextMessage.Text = ex.Message;
                    txtUserHolidays.Focus();
                }

            }
            catch (Exception)
            {
                TextMessage.Text = "Please Input UserID Correctly!";
                TextUserID.Focus();
            }
        }
    }
}