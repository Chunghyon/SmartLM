using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using System.IO;
using SmackBio.WebSocketSDK.Cmd;
using SmackBio.WebSocketSDK.DB;
using SmackBio.WebSocketSDK.M50;
using SmackBio.WebSocketSDK.M50.Cmd;
using SmackBio.WebSocketSDK.Util;

namespace SmackBio.WebSocketSDK.Sample.Pages
{
    public partial class TimeLogReadPane : System.Web.UI.Page
    {
        static List<TimeLogRowData> timelog_datasource = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            var sid = Context.Request.Params["session_id"];
            var dev_uid = Context.Request.Params["device_uid"];
            if (!string.IsNullOrEmpty(sid))
            {
                session_id.Text = sid;
                device_uid.Text = dev_uid;
            }
            else
                Context.Response.Redirect("~/ViewOnlineDevices.aspx");

            timelog_datasource = new List<TimeLogRowData>();
        }


        ////////////////////////////////////////////////////////////////////////////////
        private void prepare_ReadTimeLog(ref Int64? user_id, ref DateTime? start_time, ref DateTime? end_time)
        {
            user_id = null;
            start_time = null;
            end_time = null;

            timelog_datasource.Clear();
            GridView1.DataSource = timelog_datasource;
            GridView1.DataBind();

            ///////////
            try
            {
                user_id = Convert.ToInt64(TextUserID.Text);
            }
            catch (Exception)
            {
            }
            if (user_id == null)
                TextUserID.Text = "";

            //////////
            try
            {
                start_time = new DateTime(
                    Convert.ToInt32(TextStartTime_y.Text),
                    Convert.ToInt32(TextStartTime_m.Text),
                    Convert.ToInt32(TextStartTime_d.Text),
                    Convert.ToInt32(TextStartTime_hh.Text),
                    Convert.ToInt32(TextStartTime_mm.Text), 0);
            }
            catch (Exception)
            {
            }
            if (start_time == null)
            {
                TextStartTime_y.Text = "";
                TextStartTime_m.Text = "";
                TextStartTime_d.Text = "";
                TextStartTime_hh.Text = "";
                TextStartTime_mm.Text = "";
            }

            /////////////
            try
            {
                end_time = new DateTime(
                    Convert.ToInt32(TextEndTime_y.Text),
                    Convert.ToInt32(TextEndTime_m.Text),
                    Convert.ToInt32(TextEndTime_d.Text),
                    Convert.ToInt32(TextEndTime_hh.Text),
                    Convert.ToInt32(TextEndTime_mm.Text), 59);
            }
            catch (Exception)
            {
            }
            if (end_time == null)
            {
                TextEndTime_y.Text = "";
                TextEndTime_m.Text = "";
                TextEndTime_d.Text = "";
                TextEndTime_hh.Text = "";
                TextEndTime_mm.Text = "";
            }

            txtSearchCondition.Text = "[Search Result] ";
            txtSearchCondition.Text += "UserId: ";
            if (user_id == null)
                txtSearchCondition.Text += "(Any)";
            else
                txtSearchCondition.Text += Convert.ToString(user_id);

            txtSearchCondition.Text += ",  StartTime: ";
            if (start_time == null)
                txtSearchCondition.Text += "(from First log)";
            else
                txtSearchCondition.Text += start_time.GetValueOrDefault(DateTime.Now).ToString("yyyy-MM-dd HH:mm:ss");

            txtSearchCondition.Text += ",  EndTime: ";
            if (end_time == null)
                txtSearchCondition.Text += "(to Last log)";
            else
                txtSearchCondition.Text += end_time.GetValueOrDefault(DateTime.Now).ToString("yyyy-MM-dd HH:mm:ss");

            txtSearchCondition2.Text = txtSearchCondition.Text;
        }

        protected void btnReadTimeLog_Click(object sender, EventArgs e)
        {
            Int64? user_id = null;
            DateTime? start_time = null;
            DateTime? end_time = null;

            prepare_ReadTimeLog(ref user_id, ref start_time, ref end_time);

            CmdGetFirstGlog cmd = new CmdGetFirstGlog(user_id, start_time, end_time);
            try
            {
                var session = SessionRegistry.GetSession(Guid.Parse(session_id.Text));

                XmlDocument doc = new XmlDocument();
                doc.LoadXml(cmd.Build());

                session.ExecuteCommand(this, doc, (response) =>
                {
                    CmdGetNextGlogResponse cmd_resp = new CmdGetNextGlogResponse();
                    switch (cmd_resp.ParseResult(response.Xml))
                    {
                        case CommandExeResult.OK:
                            cmd_resp.Parse(response.Xml);
                            Add_to_GridView(cmd_resp.log);
                            continue_next_glog(cmd_resp.log.log_id + 1);
                            break;
                        case CommandExeResult.Fail:
                            TextMessage.Text = "Not Found Glog!";
                            break;
                        case CommandExeResult.InvalidParam:
                            TextMessage.Text = "Invalid Param!";
                            break;
                        default:
                            TextMessage.Text = "Failed!";
                            break;
                    }
                }, (ex) => { TextMessage.Text = ex.Message; });
            }
            catch (Exception ex)
            {
                TextMessage.Text = ex.Message;
            }
        }
        void continue_next_glog(Int32 log_id)
        {
            CmdGetNextGlog cmd = new CmdGetNextGlog(log_id);
            try
            {
                var session = SessionRegistry.GetSession(Guid.Parse(session_id.Text));

                XmlDocument doc = new XmlDocument();
                doc.LoadXml(cmd.Build());

                session.ExecuteCommand(this, doc, (response) =>
                {
                    CmdGetNextGlogResponse cmd_resp = new CmdGetNextGlogResponse();
                    switch (cmd_resp.ParseResult(response.Xml))
                    {
                        case CommandExeResult.OK:
                            cmd_resp.Parse(response.Xml);
                            Add_to_GridView(cmd_resp.log);
                            continue_next_glog(cmd_resp.log.log_id + 1);
                            break;
                        case CommandExeResult.Fail:
                            TextMessage.Text = "Read Glog Finished. Total Count: ";
                            TextMessage.Text += Convert.ToString(timelog_datasource.Count);
                            break;
                        default:
                            TextMessage.Text = "Read Glog Failed!";
                            break;
                    }
                }, (ex) => { TextMessage.Text = ex.Message; });
            }
            catch (Exception ex)
            {
                TextMessage.Text = ex.Message;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private class TimeLogRowData
        {
            public TimeLog log { get; set; }
            public int row_index { get; set; }
            public string comprehensive_string
            {
                get
                {
                    return "LogID(" + log.log_id.ToString() + "), "
                             + "UTC(" + Utils.Utc2Str(log.UtcTimezoneMinutes) + "), "
                             + "Time(" + log.time.ToString() + "), "
                             + "UserID(" + log.emp_id.ToString() + "), "
                             + "AttendStat(" + log.attend_status + "), "
                             + "Action(" + log.action + "), "
                             // + "JobCode(" + log.jobcode + "), "
                             + "Photo(" + log.photo + ")"
                             + ((log.body_temp_100 <= 0) ? "" :
                                (", BodyTemp(" + (log.body_temp_100 / 100).ToString() + "." + ((log.body_temp_100 % 100) / 10).ToString() + "'C)"))
                             + (log.attend_only ? ", AttendOnly(Yes)" : "")
                             + (log.expired ? ", Expired(Yes)" : "")
                             + ((log.latitude == null) ? "" : ", Latitude(" + log.latitude + ")")
                             + ((log.longitude == null) ? "" : ", Longitude(" + log.longitude + ")");
                }
			}
            public string no
            {
                get { return Convert.ToString(row_index); }
            }
        }
        public void Add_to_GridView(TimeLog log)
        {
            timelog_datasource.Add(new TimeLogRowData { row_index = timelog_datasource.Count + 1, log = log });

            GridView1.DataSource = timelog_datasource;
            GridView1.DataBind();
        }
        public void Add_to_GridView2(TimeLog log)
        {
            // timelog_datasource.Add(new TimeLogRowData { row_index = timelog_datasource.Count + 1, log = log });
            // 
            // GridView1.DataSource = timelog_datasource;
            // GridView1.DataBind();

            TimeLogRowData rowdata = new TimeLogRowData { row_index = timelog_datasource.Count + 1, log = log };
            comprehensive_string2.Text = "CurentLogData:   " + rowdata.comprehensive_string;
        }

        protected void btnGetTimeLogPosInfo_Click(object sender, EventArgs e)
        {
            CmdGetGlogPosInfo cmd = new CmdGetGlogPosInfo();
            try
            {
                var session = SessionRegistry.GetSession(Guid.Parse(session_id.Text));

                XmlDocument doc = new XmlDocument();
                doc.LoadXml(cmd.Build());

                session.ExecuteCommand(this, doc, (response) =>
                {
                    CmdGetGlogPosInfoResponse cmd_resp = new CmdGetGlogPosInfoResponse();
                    if (cmd_resp.Parse(response.Xml))
                        TextMessage.Text = "Success." +
                            " LogCount: " + cmd_resp.LogCount.ToString() +
                            " MaxCount: " + cmd_resp.MaxCount.ToString();
                    else
                        TextMessage.Text = "Get GlogPosInfo Failed";
                }, (ex) => { TextMessage.Text = ex.Message; });
            }
            catch (Exception ex)
            {
                TextMessage.Text = ex.Message;
            }
        }

        protected void btnDeleteTimelogWithPos_Click(object sender, EventArgs e)
        {
            CmdDeleteGlogWithPos cmd;
            try
            {
                cmd = new CmdDeleteGlogWithPos(Convert.ToUInt32(txtEndPos.Text));
            }
            catch (Exception)
            {
                TextMessage.Text = "Please input EndPos correctly";
                return;
            }

            try
            {
                var session = SessionRegistry.GetSession(Guid.Parse(session_id.Text));

                XmlDocument doc = new XmlDocument();
                doc.LoadXml(cmd.Build());

                session.ExecuteCommand(this, doc, (response) =>
                {
                    GeneralResponse cmd_resp = new GeneralResponse();
                    if (cmd_resp.ParseResult(response.Xml) == CommandExeResult.OK)
                        TextMessage.Text = "Delete TimeLog Success!";
                    else
                        TextMessage.Text = "Delete TimeLog Failed";
                }, (ex) => { TextMessage.Text = ex.Message; });
            }
            catch (Exception ex)
            {
                TextMessage.Text = ex.Message;
            }
        }

        //////////////////////////////////////////////////////////////////////////////////////////
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            lock (Session.SyncRoot)
            {
                Session["cancelled"] = true;
            }
        }
        protected void btnReadTimeLogTimer_Click(object sender, EventArgs e)
        {
            Int64? user_id = null;
            DateTime? start_time = null;
            DateTime? end_time = null;

            prepare_ReadTimeLog(ref user_id, ref start_time, ref end_time);
            Session["user_id"] = user_id;
            Session["start_time"] = start_time;
            Session["end_time"] = end_time;

            Session["cancelled"] = false;
            Session["ready"] = true;

            msg.Text = "Read Glog Info...";
            Session["next_cmd"] = "get_glog_count";

            mvvProcess.SetActiveView(vProgress);
            Timer1.Enabled = true;
        }

        protected void Timer1_Tick(object sender, EventArgs e)
        {
            if (Session["ready"] as bool? != true)
                return;

            Timer1.Enabled = false;

            if (Session["cancelled"] as bool? == true)
            {
                Session["next_cmd"] = "enable_device";
                Session["cancelled"] = false;

                msg.Text = "Cancelled.";
            }

            String cur_cmd = Session["next_cmd"].ToString();
            Session["ready"] = false;
            Session["next_cmd"] = "";

            Int64? user_id = Session["user_id"] as Int64?;
            DateTime? start_time = Session["start_time"] as DateTime?;
            DateTime? end_time = Session["end_time"] as DateTime?;


            if (cur_cmd == "get_glog_count")
            {
                CmdGetGlogPosInfo cmd = new CmdGetGlogPosInfo();
                try
                {
                    var session = SessionRegistry.GetSession(Guid.Parse(session_id.Text));

                    XmlDocument doc = new XmlDocument();
                    doc.LoadXml(cmd.Build());

                    session.ExecuteCommand(this, doc, (response) =>
                    {
                        CmdGetGlogPosInfoResponse cmd_resp = new CmdGetGlogPosInfoResponse();
                        if (cmd_resp.Parse(response.Xml))
                        {
                            msg.Text = "LogCount: " + cmd_resp.LogCount.ToString();

                            Session["total_count"] = cmd_resp.LogCount;
                            Session["cur_count"] = 0;

                            Session["next_cmd"] = "disable_device";
                        }
                        else
                            msg.Text = "Get GlogPosInfo Failed";
                    }, (ex) => { msg.Text = ex.Message; });
                }
                catch (Exception ex)
                {
                    msg.Text = ex.Message;
                }
            }
            else if (cur_cmd == "disable_device")
            {
                CmdEnableDevice cmd = new CmdEnableDevice(false);
                try
                {
                    var session = SessionRegistry.GetSession(Guid.Parse(session_id.Text));
                    XmlDocument doc = new XmlDocument();
                    doc.LoadXml(cmd.Build());

                    session.ExecuteCommand(this, doc, (response) =>
                    {
                        msg.Text = "Disable Device Failed.";
                        if (BaseMessage.IsResponseKey(response.Xml, CmdEnableDevice.MSG_KEY))
                        {
                            GeneralResponse re = new GeneralResponse();
                            if (re.ParseResult(response.Xml) == CommandExeResult.OK)
                            {
                                msg.Text = "Disabled Device!";
                                Session["next_cmd"] = "start_first_glog";
                            }
                        }
                    }, (ex) => { msg.Text = ex.Message; });
                }
                catch (Exception)
                {
                    msg.Text = "Disable Device Failed!";
                }
            }
            else if (cur_cmd == "start_first_glog")
            {
                CmdGetFirstGlog cmd = new CmdGetFirstGlog(user_id, start_time, end_time);
                try
                {
                    var session = SessionRegistry.GetSession(Guid.Parse(session_id.Text));
                    XmlDocument doc = new XmlDocument();
                    doc.LoadXml(cmd.Build());

                    session.ExecuteCommand(this, doc, (response) =>
                    {
                        CmdGetNextGlogResponse cmd_resp = new CmdGetNextGlogResponse();
                        switch (cmd_resp.ParseResult(response.Xml))
                        {
                            case CommandExeResult.OK:
                                cmd_resp.Parse(response.Xml);
                                Add_to_GridView2(cmd_resp.log);
                                //continue_next_glog(cmd_resp.log.log_id + 1);
                                Session["cur_count"] = Convert.ToInt32(Session["cur_count"]) + 1;
                                msg.Text = "Read Count: " + Session["cur_count"].ToString() + " / " + Session["total_count"].ToString();

                                Session["next_cmd"] = "continue_next_glog";
                                {
                                    Session["log_id"] = cmd_resp.log.log_id + 1;
                                }
                                break;
                            case CommandExeResult.Fail:
                                msg.Text = "Not Found Glog!";
                                Session["next_cmd"] = "enable_device";
                                break;
                            case CommandExeResult.InvalidParam:
                                msg.Text = "Invalid Param!";
                                break;
                            default:
                                msg.Text = "Failed!";
                                break;
                        }
                    }, (ex) => { msg.Text = ex.Message; });
                }
                catch (Exception ex)
                {
                    msg.Text = ex.Message;
                }
            }
            else if (cur_cmd == "continue_next_glog")
            {
                Int32 log_id = Convert.ToInt32(Session["log_id"]);

                CmdGetNextGlog cmd = new CmdGetNextGlog(log_id);
                try
                {
                    var session = SessionRegistry.GetSession(Guid.Parse(session_id.Text));

                    XmlDocument doc = new XmlDocument();
                    doc.LoadXml(cmd.Build());

                    session.ExecuteCommand(this, doc, (response) =>
                    {
                        CmdGetNextGlogResponse cmd_resp = new CmdGetNextGlogResponse();
                        switch (cmd_resp.ParseResult(response.Xml))
                        {
                            case CommandExeResult.OK:
                                cmd_resp.Parse(response.Xml);
                                Add_to_GridView2(cmd_resp.log);
                                //continue_next_glog(cmd_resp.log.log_id + 1);
                                Session["cur_count"] = Convert.ToInt32(Session["cur_count"]) + 1;
                                msg.Text = "Read Count: " + Session["cur_count"].ToString() + " / " + Session["total_count"].ToString();

                                Session["next_cmd"] = "continue_next_glog";
                                {
                                    Session["log_id"] = cmd_resp.log.log_id + 1;
                                }
                                break;
                            case CommandExeResult.Fail:
                                msg.Text = "Read Glog Finished. Total Count: ";
                                msg.Text += Convert.ToString(Convert.ToInt32(Session["cur_count"]));

                                Session["next_cmd"] = "enable_device";

                                break;
                            default:
                                msg.Text = "Read Glog Failed!";
                                break;
                        }
                    }, (ex) => { msg.Text = ex.Message; });
                }
                catch (Exception ex)
                {
                    msg.Text = ex.Message;
                }
            }
            else if (cur_cmd == "enable_device")
            {
                CmdEnableDevice cmd = new CmdEnableDevice(true);
                try
                {
                    var session = SessionRegistry.GetSession(Guid.Parse(session_id.Text));
                    XmlDocument doc = new XmlDocument();
                    doc.LoadXml(cmd.Build());

                    session.ExecuteCommand(this, doc, (response) =>
                    {
                        bool bsuccess = false;
                        if (BaseMessage.IsResponseKey(response.Xml, CmdEnableDevice.MSG_KEY))
                        {
                            GeneralResponse re = new GeneralResponse();
                            if (re.ParseResult(response.Xml) == CommandExeResult.OK)
                            {
                                //msg.Text = "Enable Device Success!";
                                bsuccess = true;
                                Session["next_cmd"] = "";
                            }
                        }

                        if (!bsuccess)
                            msg.Text = "Enable Device Failed.";
                    }, (ex) => { msg.Text = ex.Message; });
                }
                catch (Exception ex)
                {
                    msg.Text = ex.Message;
                }
            }

            if (cur_cmd == "")
            {
                msg.Text += " Finished.";
                Timer1.Enabled = false;
                mvvProcess.SetActiveView(vLaunch);
            }
            else
                Timer1.Enabled = true;

            Session["ready"] = true;
        }
    }
}