using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SmackBio.WebSocketSDK.Sample
{
    public partial class ViewUserSyncPending : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected void Clear_Click(object sender, EventArgs e)
        {
            DeviceUpdatedUserQueue.Clear();
            updated_users.DataBind();
        }

        protected void Refresh_Click(object sender, EventArgs e)
        {
            updated_users.DataBind();
        }
    }
}