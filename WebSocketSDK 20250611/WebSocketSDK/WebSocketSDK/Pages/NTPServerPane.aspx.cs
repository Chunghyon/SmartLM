using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using SmackBio.WebSocketSDK.Cmd;
using SmackBio.WebSocketSDK.M50.Cmd;

namespace SmackBio.WebSocketSDK.Sample.Pages
{
    public partial class NTPServerPane : System.Web.UI.Page
    {
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
        protected void btnGet_Click(object sender, EventArgs e)
        {
            CmdGetDeviceInfoExt cmd = new CmdGetDeviceInfoExt(DevInfoExtParamType.NTPServer);
            try
            {
                var session = SessionRegistry.GetSession(Guid.Parse(session_id.Text));

                XmlDocument doc = new XmlDocument();
                doc.LoadXml(cmd.Build());

                session.ExecuteCommand(this, doc, (response) =>
                {
                    CmdGetDeviceInfoExtResponse cmd_resp = new CmdGetDeviceInfoExtResponse();
                    if (cmd_resp.Parse(response.Xml))
                    {
                        txtMessage.Text = "Get Success!";
                        txtNTPServer.Text = cmd_resp.Value1;
                        Int32 m = Convert.ToInt32(cmd_resp.Value2);
                        txtTimezoneHour.Text = Convert.ToString(m / 60);
                        txtTimezoneMinute.Text = Convert.ToString(Math.Abs(m % 60));
                        txtAutoSyncInterval.Text = cmd_resp.Value3;
                    }
                    else
                        txtMessage.Text = "Get Failed";
                }, (ex) => { error_message.Text = ex.Message; });
            }
            catch (Exception ex)
            {
                error_message.Text = ex.Message;
            }
        }
        protected void btnSet_Click(object sender, EventArgs e)
        {
            try
            {
                int tz = Convert.ToInt32(txtTimezoneHour.Text) * 60;
                if (tz > 0) tz += Convert.ToInt32(txtTimezoneMinute.Text);
                else tz -= Convert.ToInt32(txtTimezoneMinute.Text);

                CmdSetDeviceInfoExt cmd = new CmdSetDeviceInfoExt(DevInfoExtParamType.NTPServer,
                                                    txtNTPServer.Text,
                                                    Convert.ToString(tz),
                                                    txtAutoSyncInterval.Text);

                var session = SessionRegistry.GetSession(Guid.Parse(session_id.Text));

                XmlDocument doc = new XmlDocument();
                doc.LoadXml(cmd.Build());

                session.ExecuteCommand(this, doc, (response) =>
                {
                    txtMessage.Text = "Set Failed.";
                    if (BaseMessage.IsResponseKey(response.Xml, CmdSetDeviceInfoExt.MSG_KEY))
                    {
                        GeneralResponse re = new GeneralResponse();
                        if (re.ParseResult(response.Xml) == CommandExeResult.OK)
                            txtMessage.Text = "Set Success!";
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