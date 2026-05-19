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
using SmackBio.WebSocketSDK.M50.Cmd;

namespace SmackBio.WebSocketSDK.Sample.Pages
{
    public partial class ServerUrlPane : System.Web.UI.Page
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
        protected void btnWebServerUrlGet_Click(object sender, EventArgs e)
        {
            CmdGetDeviceInfoExt cmd = new CmdGetDeviceInfoExt(DevInfoExtParamType.WebServerUrl);
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
                        txtMessage.Text = DevInfoExtParamType.WebServerUrl.ToString() + ":   " + cmd_resp.Value1.ToString();
                        txtWebServerUrl.Text = cmd_resp.Value1.ToString();
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

        protected void btnWebServerUrlSet_Click(object sender, EventArgs e)
        {
            try
            {
                CmdSetDeviceInfoExt cmd = new CmdSetDeviceInfoExt(DevInfoExtParamType.WebServerUrl, txtWebServerUrl.Text);
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