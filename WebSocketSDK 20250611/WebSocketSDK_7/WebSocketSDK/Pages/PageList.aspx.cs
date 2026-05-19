using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SmackBio.WebSocketSDK.Sample.Pages
{
    public partial class PageList : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var sid = Context.Request.Params["session_id"];
            var dev_id = Context.Request.Params["device_uid"];
            if (!string.IsNullOrEmpty(sid))
            {
                session_id.Text = sid;
                device_uid.Text = dev_id;
                Panel1.Visible = true;
                Panel2.Visible = false;
            }
            else
            {
                session_id.Text = "";
                Panel1.Visible = false;
                Panel2.Visible = true;
            }
        }

        protected void btnViewOnlineDevices_Click(object sender, EventArgs e)
        {
            Context.Response.Redirect("~/ViewOnlineDevices.aspx");
        }
        protected void btnTestCommand_Click(object sender, EventArgs e)
        {
            Context.Response.Redirect("~/Pages/TestCommand.aspx?session_id=" + session_id.Text);
        }
        protected void btnDeviceInfo_Click(object sender, EventArgs e)
        {
            Context.Response.Redirect("~/Pages/DeviceInfoPane.aspx?session_id=" + session_id.Text);
        }
        protected void btnUserManage_Click(object sender, EventArgs e)
        {
            Context.Response.Redirect("~/Pages/UserManagePane.aspx?session_id=" + session_id.Text + "&device_uid=" + device_uid.Text);
        }
        protected void btnDepartment_Click(object sender, EventArgs e)
        {
            Context.Response.Redirect("~/Pages/DepartmentPane.aspx?session_id=" + session_id.Text);
        }
        protected void btnAccessTimeZone_Click(object sender, EventArgs e)
        {
            Context.Response.Redirect("~/Pages/AccessTimeZonePane.aspx?session_id=" + session_id.Text);
        }
        protected void btnAutoAttendance_Click(object sender, EventArgs e)
        {
            Context.Response.Redirect("~/Pages/AutoAttendancePane.aspx?session_id=" + session_id.Text);
        }
        protected void btnBellTime_Click(object sender, EventArgs e)
        {
            Context.Response.Redirect("~/Pages/BellTimePane.aspx?session_id=" + session_id.Text);
        }
        protected void btnNetworkSetting_Click(object sender, EventArgs e)
        {
            Context.Response.Redirect("~/Pages/NetworkSettingPane.aspx?session_id=" + session_id.Text);
        }
        protected void btnWiFiSetting_Click(object sender, EventArgs e)
        {
            Context.Response.Redirect("~/Pages/WiFiSettingPane.aspx?session_id=" + session_id.Text);
        }
        protected void btnDataEmpty_Click(object sender, EventArgs e)
        {
            Context.Response.Redirect("~/Pages/DataEmptyPane.aspx?session_id=" + session_id.Text);
        }
        protected void btnTimeLogRead_Click(object sender, EventArgs e)
        {
            Context.Response.Redirect("~/Pages/TimeLogReadPane.aspx?session_id=" + session_id.Text);
        }
        protected void btnNTPServer_Click(object sender, EventArgs e)
        {
            Context.Response.Redirect("~/Pages/NTPServerPane.aspx?session_id=" + session_id.Text);
        }
        protected void btnVPNServer_Click(object sender, EventArgs e)
        {
            Context.Response.Redirect("~/Pages/VPNServerPane.aspx?session_id=" + session_id.Text);
        }
        protected void btnServerURL_Click(object sender, EventArgs e)
        {
            Context.Response.Redirect("~/Pages/ServerUrlPane.aspx?session_id=" + session_id.Text);
        }
        protected void btnFirmwareUpgrade_Click(object sender, EventArgs e)
        {
            Context.Response.Redirect("~/Pages/FirmwareUpgradePane.aspx?session_id=" + session_id.Text);
        }
    }
}