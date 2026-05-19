using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Security.Cryptography;

namespace SmackBio.WebSocketSDK.Sample
{
    public partial class ViewRegisterDevices : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void register_devices_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Register")
            {
                int index = Convert.ToInt32(e.CommandArgument);
                GridViewRow row = register_devices.Rows[index];

                byte[] guidBytes = new byte[16];
                Guid guid;
                RandomNumberGenerator _random = RandomNumberGenerator.Create();

                _random.GetBytes(guidBytes);
                guid = new Guid(guidBytes);

                string oldToken;
                if (!DeviceLoginManager._registeredDevices.TryGetValue(row.Cells[1].Text, out oldToken))
                    DeviceLoginManager._registeredDevices.Add(row.Cells[1].Text, guid.ToString());
                Context.Response.Redirect("~/ViewRegisterDevices.aspx");
            }
        }

        protected void Refresh_Click(object sender, EventArgs e)
        {
            register_devices.DataBind();
        }
    }
}