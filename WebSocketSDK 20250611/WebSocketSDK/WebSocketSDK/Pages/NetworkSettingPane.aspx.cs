using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using SmackBio.WebSocketSDK.Cmd;
using SmackBio.WebSocketSDK.DB;

namespace SmackBio.WebSocketSDK.Sample.Pages
{
    public partial class NetworkSettingPane : System.Web.UI.Page
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
        protected void btnGetEthernetSetting_Click(object sender, EventArgs e)
        {
            CmdGetEthernetSetting cmd = new CmdGetEthernetSetting();
            try
            {
                var session = SessionRegistry.GetSession(Guid.Parse(session_id.Text));;

                XmlDocument doc = new XmlDocument();
                doc.LoadXml(cmd.Build());

                session.ExecuteCommand(this, doc, (response) =>
                {
                    CmdGetEthernetSettingResponse cmd_resp = new CmdGetEthernetSettingResponse();
                    if (cmd_resp.Parse(response.Xml))
                    {
                        chkUseDHCP.Checked = cmd_resp.setting.use_dhcp;
                        txtIP.Text = cmd_resp.setting.ip;
                        txtSubnetMask.Text = cmd_resp.setting.subnet;
                        txtGateway.Text = cmd_resp.setting.gateway;
                        txtPort.Text = cmd_resp.setting.port.ToString();
                        txtMacAddress.Text = cmd_resp.setting.macaddr;

                        txtIP_from_dhcp.Text = cmd_resp.setting.ip_from_dhcp;
                        txtSubnetMask_from_dhcp.Text = cmd_resp.setting.subnet_from_dhcp;
                        txtGateway_from_dhcp.Text = cmd_resp.setting.gateway_from_dhcp;

                        txtMessage.Text = "Get EthernetSetting OK";
                    }
                    else
                        txtMessage.Text = "Get EthernetSetting Failed";
                }, (ex) => { error_message.Text = ex.Message; });
            }
            catch (Exception ex)
            {
                error_message.Text = ex.Message;
            }
        }
        protected void btnSetEthernetSetting_Click(object sender, EventArgs e)
        {
            try
            {
                EthernetSetting ether_setting = new EthernetSetting();
                
                ether_setting.use_dhcp = chkUseDHCP.Checked;
                ether_setting.ip = txtIP.Text;
                ether_setting.subnet = txtSubnetMask.Text;
                ether_setting.gateway = txtGateway.Text;
                ether_setting.port = Convert.ToInt32(txtPort.Text);

                CmdSetEthernet cmd = new CmdSetEthernet(ether_setting);
                var session = SessionRegistry.GetSession(Guid.Parse(session_id.Text));;

                XmlDocument doc = new XmlDocument();
                doc.LoadXml(cmd.Build());

                session.ExecuteCommand(this, doc, (response) =>
                {
                    txtMessage.Text = "Set Ethernet Failed.";
                    if (BaseMessage.IsResponseKey(response.Xml, CmdSetEthernet.MSG_KEY))
                    {
                        GeneralResponse re = new GeneralResponse();
                        if (re.ParseResult(response.Xml) == CommandExeResult.OK)
                            txtMessage.Text = "SetEthernet OK!";
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