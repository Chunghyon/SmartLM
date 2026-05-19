using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using SmackBio.WebSocketSDK.Cmd;
using SmackBio.WebSocketSDK.M50;
using SmackBio.WebSocketSDK.M50.Cmd;

namespace SmackBio.WebSocketSDK.Sample.Pages
{
    public partial class FirmwareUpgradePane : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            lblMessage.Text = "";
            error_message.Text = "";

            var sid = Context.Request.Params["session_id"];
            if (!string.IsNullOrEmpty(sid))
                session_id.Text = sid;
            else
                Context.Response.Redirect("~/ViewOnlineDevices.aspx");
        }

        protected void btnGetFirmwareVersion_Click(object sender, EventArgs e)
        {
            CmdGetFirmwareVersion cmd = new CmdGetFirmwareVersion();
            try
            {
                var session = SessionRegistry.GetSession(Guid.Parse(session_id.Text));

                XmlDocument doc = new XmlDocument();
                doc.LoadXml(cmd.Build());

                session.ExecuteCommand(this, doc, (response) =>
                {
                    CmdGetFirmwareVersionResponse cmd_resp = new CmdGetFirmwareVersionResponse();
                    if (cmd_resp.Parse(response.Xml))
                    {
                        lblMessage.Text = "Version: " + cmd_resp.Version + ", BuildNumber: " + cmd_resp.BuildNumber;
                    }
                    else
                        lblMessage.Text = "Get Failed";
                }, (ex) => { error_message.Text = ex.Message; });
            }
            catch (Exception ex)
            {
                error_message.Text = ex.Message;
            }
        }

        protected void btnFirmwareUpgradeHttp_Click(object sender, EventArgs e)
        {
            CmdFirmwareUpgradeHttp cmd = new CmdFirmwareUpgradeHttp(txtUrl.Text);
            try
            {
                var session = SessionRegistry.GetSession(Guid.Parse(session_id.Text));

                XmlDocument doc = new XmlDocument();
                doc.LoadXml(cmd.Build());

                session.ExecuteCommand(this, doc, (response) =>
                {
                    GeneralResponse cmd_resp = new GeneralResponse();
                    if (cmd_resp.Parse(response.Xml))
                    {
                        lblMessage.Text = "Send Firmware Download Url Success. (Starting download and upgrade now...)";
                    }
                    else
                        lblMessage.Text = "Get Failed";
                }, (ex) => { error_message.Text = ex.Message; });
            }
            catch (Exception ex)
            {
                error_message.Text = ex.Message;
            }
        }
	}
}