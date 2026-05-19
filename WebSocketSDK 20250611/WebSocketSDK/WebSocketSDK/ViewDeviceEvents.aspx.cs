using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SmackBio.WebSocketSDK;

namespace SmackBio.WebSocketSDK.Sample
{
    public partial class ViewDeviceEvents : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Clear_Click(object sender, EventArgs e)
        {
            DeviceEventQueue.Clear();
            device_events.DataBind();
        }

        protected void Refresh_Click(object sender, EventArgs e)
        {
            device_events.DataBind();
        }

        protected void Timer_Watch(object sender, EventArgs e)
        {
            device_events.DataBind();
        }
    }
}