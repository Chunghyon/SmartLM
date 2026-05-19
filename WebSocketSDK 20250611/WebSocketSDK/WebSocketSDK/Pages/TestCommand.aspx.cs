using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using SmackBio.WebSocketSDK.Cmd;

namespace SmackBio.WebSocketSDK.Sample.Pages
{
    public partial class TestCommand : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var sid = Context.Request.Params["session_id"];
            if (!string.IsNullOrEmpty(sid))
                session_id.Text = sid;
            else
                Context.Response.Redirect("~/ViewOnlineDevices.aspx");
        }

        protected void Execute_Click(object sender, EventArgs e)
        {
            try
            {
                var session = SessionRegistry.GetSession(Guid.Parse(session_id.Text));

                XmlDocument doc = new XmlDocument();
                doc.LoadXml(request_xml.Text);

                session.ExecuteCommand(this, doc, (response) =>
                {
                    response_xml.Text = response.Xml.OuterXml;
                    error_message.Text = "";
                }, (ex) => { error_message.Text = ex.Message; });
            }
            catch (Exception ex)
            {
                error_message.Text = ex.Message;
            }
        }

        protected void btnGetTime_Click(object sender, EventArgs e)
        {
            CmdGetTime cmd = new CmdGetTime();
            request_xml.Text = cmd.Build();
            response_xml.Text = "";
        }
    }
}