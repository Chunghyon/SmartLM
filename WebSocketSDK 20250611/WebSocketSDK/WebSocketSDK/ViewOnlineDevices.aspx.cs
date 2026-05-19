using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SmackBio.WebSocketSDK.Sample
{
    public partial class ViewOnlineDevices : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void online_devices_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Open")
            {
                int index = Convert.ToInt32(e.CommandArgument);
                GridViewRow row = online_devices.Rows[index];
                Context.Response.Redirect("~/Pages/PageList.aspx?session_id=" + row.Cells[1].Text + "&device_uid=" + row.Cells[0].Text);
            }
        }

        protected void Refresh_Click(object sender, EventArgs e)
        {
            online_devices.DataBind();
        }
    }
}