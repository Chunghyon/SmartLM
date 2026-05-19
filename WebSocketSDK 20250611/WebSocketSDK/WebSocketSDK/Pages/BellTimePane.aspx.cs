using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using SmackBio.WebSocketSDK.Cmd;
using SmackBio.WebSocketSDK.DB;
using SmackBio.WebSocketSDK.M50;
using SmackBio.WebSocketSDK.M50.Cmd;

namespace SmackBio.WebSocketSDK.Sample.Pages
{
    public partial class BellTimePane : System.Web.UI.Page
    {
        static BellSetting bellsetting = null;

        static int edit_index;
        public static void UpdateBell(bool valid, BellType type, byte hour, byte minute)
        {
            if (hour > 23) hour = 23;
            if (minute > 59) minute = 59;

            bellsetting.bells[edit_index].valid = valid;
            bellsetting.bells[edit_index].type = type;
            bellsetting.bells[edit_index].hour = hour;
            bellsetting.bells[edit_index].minute = minute;
        }
        public static void UpdateBell()
        { }

        protected void Page_Load(object sender, EventArgs e)
        {
            txtMessage.Text = "";
            error_message.Text = "";

            var sid = Context.Request.Params["session_id"];
            if (!string.IsNullOrEmpty(sid))
                session_id.Text = sid;
            else
                Context.Response.Redirect("~/ViewOnlineDevices.aspx");

            if (bellsetting == null)
            {
                bellsetting = new BellSetting();

                bellsetting.bells = new Belling[M50Device.BellCount];
                for (int i = 0; i < M50Device.BellCount; i++)
                    bellsetting.bells[i] = new Belling();
            }
        }
        public static Belling[] GetBells()
        {
            return bellsetting.bells;
        }

        protected void btnGetBellTime_Click(object sender, EventArgs e)
        {
            CmdGetBellTime cmd = new CmdGetBellTime();
            try
            {
                var session = SessionRegistry.GetSession(Guid.Parse(session_id.Text));

                XmlDocument doc = new XmlDocument();
                doc.LoadXml(cmd.Build());

                session.ExecuteCommand(this, doc, (response) =>
                {
                    CmdGetBellTimeResponse cmd_resp = new CmdGetBellTimeResponse();
                    if (cmd_resp.Parse(response.Xml))
                    {
                        txtBellCount.Text = bellsetting.BellCount.ToString();
                        txtBellRingTimes.Text = bellsetting.RingCount.ToString();

                        bellsetting = cmd_resp.setting;
                        gridview_bells.DataBind();

                        txtMessage.Text = "GetBellTime OK.";
                    }
                    else
                        txtMessage.Text = "Get BellTime Failed";
                }, (ex) => { error_message.Text = ex.Message; });
            }
            catch (Exception ex)
            {
                error_message.Text = ex.Message;
            }
        }
        protected void btnSetBellTime_Click(object sender, EventArgs e)
        {
            try
            {
                bellsetting.BellCount = Convert.ToUInt32(txtBellCount.Text);
                bellsetting.RingCount = Convert.ToUInt32(txtBellRingTimes.Text);

                CmdSetBellTime cmd = new CmdSetBellTime(bellsetting);
                var session = SessionRegistry.GetSession(Guid.Parse(session_id.Text));

                XmlDocument doc = new XmlDocument();
                doc.LoadXml(cmd.Build());

                session.ExecuteCommand(this, doc, (response) =>
                {
                    txtMessage.Text = "Set BellTime Failed.";
                    if (BaseMessage.IsResponseKey(response.Xml, CmdSetBellTime.MSG_KEY))
                    {
                        GeneralResponse re = new GeneralResponse();
                        if (re.ParseResult(response.Xml) == CommandExeResult.OK)
                            txtMessage.Text = "SetBellTime OK.";
                    }
                }, (ex) => { error_message.Text = ex.Message; });
            }
            catch (Exception ex)
            {
                error_message.Text = ex.Message;
            }
        }

        protected void bell_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Edit")
            {
                int index = Convert.ToInt32(e.CommandArgument);
                GridViewRow row = gridview_bells.Rows[index];
            }
            else if (e.CommandName == "Update")
            {
                edit_index = Convert.ToInt32(e.CommandArgument);
            }
            else if (e.CommandName == "Cancel")
            {
                int index = Convert.ToInt32(e.CommandArgument);
            }
        }
    }
}