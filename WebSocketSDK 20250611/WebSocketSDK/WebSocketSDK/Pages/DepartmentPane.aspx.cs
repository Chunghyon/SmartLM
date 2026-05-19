using System;
using System.Collections.Generic;
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
    public partial class DepartmentPane : System.Web.UI.Page
    {
        public partial class UiDepartment
        {
            public string Name { get; set; }
        }

        static readonly List<int> department_no_params = new List<int> { };
        static readonly List<int> proxy_department_no_params = new List<int> { };
        static DepartmentPane()
        {
            for (int i = 0; i < M50Device.MaxDeptCount; i++)
                department_no_params.Add(i);
            for (int i = 0; i < M50Device.MaxProxyDeptCount; i++)
                proxy_department_no_params.Add(i);
        }
        public static List<int> GetDepartmentNoList()
        {
            return department_no_params;
        }
        public static List<int> GetProxyDepartmentNoList()
        {
            return proxy_department_no_params;
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
        protected void btnGetDepartment_Click(object sender, EventArgs e)
        {
            CmdGetDepartment cmd = new CmdGetDepartment(cmbDepartmentNo.SelectedIndex);
            try
            {
                var session = SessionRegistry.GetSession(Guid.Parse(session_id.Text));

                XmlDocument doc = new XmlDocument();
                doc.LoadXml(cmd.Build());

                session.ExecuteCommand(this, doc, (response) =>
                {
                    CmdGetDepartmentResponse cmd_resp = new CmdGetDepartmentResponse();
                    if (cmd_resp.ParseResult(response.Xml) == CommandExeResult.InvalidParam)
					{
						txtMessage.Text = "GetDepartment Failed. (InvalidParam)";
					}
					else
					{
						if (cmd_resp.Parse(response.Xml))
						{
							txtMessage.Text = "GetDepartment OK. Name of Department(" + cmbDepartmentNo.SelectedIndex.ToString() + ") = \"" + cmd_resp.Name + "\"";
							txtMessage.DataBind();
						}
						else
						{
							txtMessage.Text = "GetDepartment Failed";
						}
					}
                }, (ex) => { error_message.Text = ex.Message; });
            }
            catch (Exception ex)
            {
                error_message.Text = ex.Message;
            }
        }
        protected void btnSetDepartment_Click(object sender, EventArgs e)
        {
            CmdSetDepartment cmd = new CmdSetDepartment(cmbDepartmentNo.SelectedIndex, txtDepartmentName.Text);
            try
            {
                var session = SessionRegistry.GetSession(Guid.Parse(session_id.Text));

                XmlDocument doc = new XmlDocument();
                doc.LoadXml(cmd.Build());

                session.ExecuteCommand(this, doc, (response) =>
                {
                    txtMessage.Text = "Set Department Failed.";
                    if (BaseMessage.IsResponseKey(response.Xml, CmdSetDepartment.MSG_KEY))
                    {
                        GeneralResponse re = new GeneralResponse();
                        if (re.ParseResult(response.Xml) == CommandExeResult.OK)
                            txtMessage.Text = "SetDepartment OK. (Department" + cmbDepartmentNo.SelectedIndex.ToString() + ")";
                    }
                }, (ex) => { error_message.Text = ex.Message; });
            }
            catch (Exception ex)
            {
                error_message.Text = ex.Message;
            }
        }
        protected void btnGetProxyDept_Click(object sender, EventArgs e)
        {
            CmdGetProxyDept cmd = new CmdGetProxyDept(cmbProxyDepartmentNo.SelectedIndex);
            try
            {
                var session = SessionRegistry.GetSession(Guid.Parse(session_id.Text));

                XmlDocument doc = new XmlDocument();
                doc.LoadXml(cmd.Build());

                session.ExecuteCommand(this, doc, (response) =>
                {
                    CmdGetProxyDeptResponse cmd_resp = new CmdGetProxyDeptResponse();
					if (cmd_resp.ParseResult(response.Xml) == CommandExeResult.InvalidParam)
					{
						txtMessage.Text = "GetProxyDept Failed. (InvalidParam)";
					}
					else
					{
						if (cmd_resp.Parse(response.Xml))
						{
							txtMessage.Text = "GetProxyDept OK. Name of Proxy Department(" + cmbProxyDepartmentNo.SelectedIndex.ToString() + ") = \"" + cmd_resp.Name + "\"";
							txtMessage.DataBind();
						}
						else
						{
							txtMessage.Text = "GetProxyDept Failed";
						}
					}
                }, (ex) => { error_message.Text = ex.Message; });
            }
            catch (Exception ex)
            {
                error_message.Text = ex.Message;
            }
        }
        protected void btnSetProxyDept_Click(object sender, EventArgs e)
        {
            CmdSetProxyDept cmd = new CmdSetProxyDept(cmbProxyDepartmentNo.SelectedIndex, txtProxyDepartmentName.Text);
            try
            {
                var session = SessionRegistry.GetSession(Guid.Parse(session_id.Text));

                XmlDocument doc = new XmlDocument();
                doc.LoadXml(cmd.Build());

                session.ExecuteCommand(this, doc, (response) =>
                {
                    txtMessage.Text = "Set Department Failed.";
                    if (BaseMessage.IsResponseKey(response.Xml, CmdSetProxyDept.MSG_KEY))
                    {
                        GeneralResponse re = new GeneralResponse();
                        if (re.ParseResult(response.Xml) == CommandExeResult.OK)
                            txtMessage.Text = "SetProxyDept OK. (ProxyDepartment" + cmbProxyDepartmentNo.SelectedIndex.ToString() + ")";
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