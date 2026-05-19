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
    public partial class AccessTimeZonePane : System.Web.UI.Page
    {
        static AccessTimeSection[] time_sections = null;

        static readonly List<string> timezone_params = new List<string> { };
        static AccessTimeZonePane()
        {
            for (int i = 0; i < M50Device.AcTimezoneCount; i++)
                timezone_params.Add("Timezone" + Convert.ToString(i + 1));
        }
        public static List<string> GetTimezoneList()
        {
            return timezone_params;
        }

        static int edit_index;
        public static void UpdateTimesection(int StartHour, int StartMinute, int EndHour, int EndMinute)
        {
            if (StartHour > 23) StartHour = 23;
            if (StartMinute > 59) StartMinute = 59;
            if (EndHour > 23) EndHour = 23;
            if (EndMinute > 59) EndMinute = 59;

            time_sections[edit_index].SetStart(StartHour * 60 + StartMinute);
            time_sections[edit_index].SetEnd(EndHour * 60 + EndMinute);
        }
        public static void UpdateTimesection()
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

            if (time_sections == null)
            {
                time_sections = new AccessTimeSection[M50Device.TIMESECTION_COUNT_PER_TIMEZONE];
                for (int i = 0; i < M50Device.TIMESECTION_COUNT_PER_TIMEZONE; i++)
                    time_sections[i] = new AccessTimeSection();
            }
        }
        public static AccessTimeSection[] GetTimeSections()
        {
            return time_sections;
        }

        protected void btnGetAccessTimeZone_Click(object sender, EventArgs e)
        {
            CmdGetAccessTimeZone cmd = new CmdGetAccessTimeZone(cmbTimezoneNo.SelectedIndex);
            try
            {
                var session = SessionRegistry.GetSession(Guid.Parse(session_id.Text));

                XmlDocument doc = new XmlDocument();
                doc.LoadXml(cmd.Build());

                session.ExecuteCommand(this, doc, (response) =>
                {
                    CmdGetAccessTimeZoneResponse cmd_resp = new CmdGetAccessTimeZoneResponse();
                    if (cmd_resp.Parse(response.Xml))
                    {
                        time_sections = cmd_resp.Sections;
                        gridview_timesections.DataBind();

                        txtMessage.Text = "GetAccessTimeZone OK. (" + timezone_params[cmbTimezoneNo.SelectedIndex] + ")";
                    }
                    else
                        txtMessage.Text = "Get AccessTimeZone Failed";
                }, (ex) => { error_message.Text = ex.Message; });
            }
            catch (Exception ex)
            {
                error_message.Text = ex.Message;
            }
        }
        protected void btnSetAccessTimeZone_Click(object sender, EventArgs e)
        {
            CmdSetAccessTimeZone cmd = new CmdSetAccessTimeZone(cmbTimezoneNo.SelectedIndex, time_sections);
            try
            {
                var session = SessionRegistry.GetSession(Guid.Parse(session_id.Text));

                XmlDocument doc = new XmlDocument();
                doc.LoadXml(cmd.Build());

                session.ExecuteCommand(this, doc, (response) =>
                {
                    txtMessage.Text = "Set AccessTimezone Failed.";
                    if (BaseMessage.IsResponseKey(response.Xml, CmdSetAccessTimeZone.MSG_KEY))
                    {
                        GeneralResponse re = new GeneralResponse();
                        if (re.ParseResult(response.Xml) == CommandExeResult.OK)
                            txtMessage.Text = "SetAccessTimeZone OK. (" + timezone_params[cmbTimezoneNo.SelectedIndex] + ")";
                    }
                }, (ex) => { error_message.Text = ex.Message; });
            }
            catch (Exception ex)
            {
                error_message.Text = ex.Message;
            }
        }

        protected void timezone_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Edit")
            {
                int index = Convert.ToInt32(e.CommandArgument);
                GridViewRow row = gridview_timesections.Rows[index];
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