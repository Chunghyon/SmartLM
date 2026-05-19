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
    public partial class WiFiSettingPane : System.Web.UI.Page
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
        protected void btnGetWiFiSetting_Click(object sender, EventArgs e)
        {
            CmdGetWiFiSetting cmd = new CmdGetWiFiSetting();
            try
            {
                var session = SessionRegistry.GetSession(Guid.Parse(session_id.Text));

                XmlDocument doc = new XmlDocument();
                doc.LoadXml(cmd.Build());

                session.ExecuteCommand(this, doc, (response) =>
                {
                    CmdGetWiFiSettingResponse cmd_resp = new CmdGetWiFiSettingResponse();
                    if (cmd_resp.Parse(response.Xml))
                    {
                        chkUseWifi.Checked = cmd_resp.setting.use_wifi;
                        txtSSID.Text = cmd_resp.setting.SSID;
                        txtKey.Text = cmd_resp.setting.key;
                        chkUseDHCP.Checked = cmd_resp.setting.use_dhcp;
                        txtIP.Text = cmd_resp.setting.ip;
                        txtSubnetMask.Text = cmd_resp.setting.subnet;
                        txtGateway.Text = cmd_resp.setting.gateway;
                        txtPort.Text = cmd_resp.setting.port.ToString();

                        txtIP_from_dhcp.Text = cmd_resp.setting.ip_from_dhcp;
                        txtSubnetMask_from_dhcp.Text = cmd_resp.setting.subnet_from_dhcp;
                        txtGateway_from_dhcp.Text = cmd_resp.setting.gateway_from_dhcp;

                        txtMessage.Text = "Get WiFiSetting OK";
                    }
                    else
                        txtMessage.Text = "Get WiFiSetting Failed";
                }, (ex) => { error_message.Text = ex.Message; });
            }
            catch (Exception ex)
            {
                error_message.Text = ex.Message;
            }
        }
        protected void btnSetWiFiSetting_Click(object sender, EventArgs e)
        {
            try
            {
                WiFiSetting wifi_setting = new WiFiSetting();

                wifi_setting.use_wifi = chkUseWifi.Checked;
                wifi_setting.SSID = txtSSID.Text;
                wifi_setting.key = txtKey.Text;
                wifi_setting.use_dhcp = chkUseDHCP.Checked;
                wifi_setting.ip = txtIP.Text;
                wifi_setting.subnet = txtSubnetMask.Text;
                wifi_setting.gateway = txtGateway.Text;
                wifi_setting.port = Convert.ToInt32(txtPort.Text);

                CmdSetWiFi cmd = new CmdSetWiFi(wifi_setting);
                var session = SessionRegistry.GetSession(Guid.Parse(session_id.Text));

                XmlDocument doc = new XmlDocument();
                doc.LoadXml(cmd.Build());

                session.ExecuteCommand(this, doc, (response) =>
                {
                    txtMessage.Text = "Set WiFi Failed.";
                    if (BaseMessage.IsResponseKey(response.Xml, CmdSetWiFi.MSG_KEY))
                    {
                        GeneralResponse re = new GeneralResponse();
                        if (re.ParseResult(response.Xml) == CommandExeResult.OK)
                            txtMessage.Text = "SetWiFi OK!";
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