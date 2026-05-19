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
    public partial class AutoAttendancePane : System.Web.UI.Page
    {
        static AutoAttendance[] attendances = null;

        static int edit_index;
        public static void UpdateAttendance(int StartHour, int StartMinute, int EndHour, int EndMinute, AttendStatus Status)
        {
            if (StartHour > 23) StartHour = 23;
            if (StartMinute > 59) StartMinute = 59;
            if (EndHour > 23) EndHour = 23;
            if (EndMinute > 59) EndMinute = 59;

            attendances[edit_index].SetStart(StartHour * 60 + StartMinute);
            attendances[edit_index].SetEnd(EndHour * 60 + EndMinute);
            attendances[edit_index].Status = Status;
        }
        public static void UpdateAttendance()
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

            if (attendances == null)
            {
                attendances = new AutoAttendance[M50Device.TrTimezoneCount];
                for (int i = 0; i < M50Device.TrTimezoneCount; i++)
                    attendances[i] = new AutoAttendance();
            }
        }
        public static AutoAttendance[] GetAttendances()
        {
            return attendances;
        }

        protected void btnGetAutoAttendance_Click(object sender, EventArgs e)
        {
            CmdGetAutoAttendance cmd = new CmdGetAutoAttendance();
            try
            {
                var session = SessionRegistry.GetSession(Guid.Parse(session_id.Text));

                XmlDocument doc = new XmlDocument();
                doc.LoadXml(cmd.Build());

                session.ExecuteCommand(this, doc, (response) =>
                {
                    CmdGetAutoAttendanceResponse cmd_resp = new CmdGetAutoAttendanceResponse();
                    if (cmd_resp.Parse(response.Xml))
                    {
                        attendances = cmd_resp.Sections;
                        gridview_attendances.DataBind();

                        txtMessage.Text = "GetAutoAttendance OK.";
                    }
                    else
                        txtMessage.Text = "Get AutoAttendance Failed";
                }, (ex) => { error_message.Text = ex.Message; });
            }
            catch (Exception ex)
            {
                error_message.Text = ex.Message;
            }
        }
        protected void btnSetAutoAttendance_Click(object sender, EventArgs e)
        {
            CmdSetAutoAttendance cmd = new CmdSetAutoAttendance(attendances);
            try
            {
                var session = SessionRegistry.GetSession(Guid.Parse(session_id.Text));

                XmlDocument doc = new XmlDocument();
                doc.LoadXml(cmd.Build());

                session.ExecuteCommand(this, doc, (response) =>
                {
                    txtMessage.Text = "Set AutoAttendance Failed.";
                    if (BaseMessage.IsResponseKey(response.Xml, CmdSetAutoAttendance.MSG_KEY))
                    {
                        GeneralResponse re = new GeneralResponse();
                        if (re.ParseResult(response.Xml) == CommandExeResult.OK)
                            txtMessage.Text = "SetAutoAttendance OK.";
                    }
                }, (ex) => { error_message.Text = ex.Message; });
            }
            catch (Exception ex)
            {
                error_message.Text = ex.Message;
            }
        }

        protected void attendance_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Edit")
            {
                int index = Convert.ToInt32(e.CommandArgument);
                GridViewRow row = gridview_attendances.Rows[index];
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