using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using SmackBio.WebSocketSDK.Cmd;
using SmackBio.WebSocketSDK.DB;
using SmackBio.WebSocketSDK.M50.Cmd;

namespace SmackBio.WebSocketSDK.Sample.Pages
{
    public partial class DeviceInfoPane : System.Web.UI.Page
    {
        static readonly List<DevInfoParamType> device_info_params = new List<DevInfoParamType> {
                DevInfoParamType.ManagersNumber,
                DevInfoParamType.MachineID,
                DevInfoParamType.Language,
                DevInfoParamType.LockReleaseTime,    // in seconds
                DevInfoParamType.SLogWarning,
                DevInfoParamType.GLogWarning,
                DevInfoParamType.ReverifyTime,       // in minutes
                DevInfoParamType.Baudrate,
                DevInfoParamType.IdentifyMode,
                DevInfoParamType.LockMode,
                DevInfoParamType.DoorSensorType,
                DevInfoParamType.DoorOpenTimeout,    // in seconds
                DevInfoParamType.AutoSleepTime,      // in minutes
                DevInfoParamType.EventSendType,
		        DevInfoParamType.WiegandFormat,
		        DevInfoParamType.CommPassword,
                DevInfoParamType.UseProxyInput,
                DevInfoParamType.ProxyDlgTimeout,

                DevInfoParamType.SoundVolume,
                DevInfoParamType.ShowRealtimeCamera,
                DevInfoParamType.UseFailLog,

                DevInfoParamType.FaceEngineThreshold,
                DevInfoParamType.FaceEngineUseAntispoofing,

                DevInfoParamType.NeedWearingMask,
                DevInfoParamType.SuggestWearingMask,

                DevInfoParamType.UseMeasureTemperature,
                DevInfoParamType.UseVisitorMode,
                DevInfoParamType.ShowRealtimeTemperature,
                DevInfoParamType.AbnormalTempDisableDoorOpen,
                DevInfoParamType.MeasuringDurationType,
                DevInfoParamType.MeasuringDistanceType,
                DevInfoParamType.TemperatureUnit,
                DevInfoParamType.AbnormalTempThreshold_Celsius,
                DevInfoParamType.AbnormalTempThreshold_Fahrenheit,
                DevInfoParamType.UtcTimezoneMinutes,
                DevInfoParamType.BackgroundColor,
			};

        static readonly List<DevStatusParamType> device_status_params =  new List<DevStatusParamType> {
                DevStatusParamType.ManagerCount,
		        DevStatusParamType.UserCount,
		        DevStatusParamType.FaceCount,
                DevStatusParamType.FpCount,
		        DevStatusParamType.CardCount,
                DevStatusParamType.PwdCount,
                DevStatusParamType.QRCount,
                DevStatusParamType.DoorStatus,
		        DevStatusParamType.AlarmStatus,
            };

        static readonly List<LockControlMode> lock_control_params = new List<LockControlMode>  {
            LockControlMode.ForceOpen,
            LockControlMode.ForceClose,
            LockControlMode.NormalOpen,
            LockControlMode.AutoRecover,
            LockControlMode.Restart,
            LockControlMode.CancelWarning,
//             LockControlMode.IllegalOpen,
        };


        public static List<DevInfoParamType> GetDeviceInfoItemList()
        {
            return device_info_params;
        }
        public static List<DevStatusParamType> GetDeviceStatusItemList()
        {
            return device_status_params;
        }
        public static List<LockControlMode> GetLockControlItemList()
        {
            return lock_control_params;
        }
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
        protected void btnGetTime_Click(object sender, EventArgs e)
        {
            CmdGetTime cmd = new CmdGetTime();
            try
            {
                var session = SessionRegistry.GetSession(Guid.Parse(session_id.Text));

                XmlDocument doc = new XmlDocument();
                doc.LoadXml(cmd.Build());
                session.ExecuteCommand(this, doc, (response) =>
                    {
                        CmdGetTimeResponse cmd_resp = new CmdGetTimeResponse();
                        if (cmd_resp.Parse(response.Xml))
                            txtMessage.Text = cmd_resp.time.ToString();
                        else
                            txtMessage.Text = "Get Time Failed";
                    }, (ex) => { error_message.Text = ex.Message; });
            }
            catch (Exception ex)
            {
                error_message.Text = ex.Message;
            }
        }

        protected void btnSetTime_Click(object sender, EventArgs e)
        {
             CmdSetTime cmd = new CmdSetTime();
             try
             {
                 var session = SessionRegistry.GetSession(Guid.Parse(session_id.Text));

                 XmlDocument doc = new XmlDocument();
                 doc.LoadXml(cmd.Build());
                 
                 session.ExecuteCommand(this, doc, (response) =>
                 {
                     txtMessage.Text = "Set Time Failed.";
                     if (BaseMessage.IsResponseKey(response.Xml, CmdSetTime.MSG_KEY))
                     {
                         GeneralResponse re = new GeneralResponse();
                         if (re.ParseResult(response.Xml) == CommandExeResult.OK)
                             txtMessage.Text = "Set Time Success!";
                     }
                 }, (ex) => { error_message.Text = ex.Message; });
            }
            catch (Exception ex)
             {
                 error_message.Text = ex.Message;
             }
        }

        protected void btnEnableDevice_Click(object sender, EventArgs e)
        {
            CmdEnableDevice cmd = new CmdEnableDevice();
            try
            {
                var session = SessionRegistry.GetSession(Guid.Parse(session_id.Text));

                XmlDocument doc = new XmlDocument();
                doc.LoadXml(cmd.Build());
                
                session.ExecuteCommand(this, doc, (response) =>
                {
                    txtMessage.Text = "Enable Device Failed.";
                    if (BaseMessage.IsResponseKey(response.Xml, CmdEnableDevice.MSG_KEY))
                    {
                        GeneralResponse re = new GeneralResponse();
                        if (re.ParseResult(response.Xml) == CommandExeResult.OK)
                            txtMessage.Text = "Enable Device Success!";
                    }
                }, (ex) => { error_message.Text = ex.Message; });
            }
            catch (Exception ex)
            {
                error_message.Text = ex.Message;
            }
        }
        protected void btnDisableDevice_Click(object sender, EventArgs e)
        {
            CmdEnableDevice cmd = new CmdEnableDevice(false);
            try
            {
                var session = SessionRegistry.GetSession(Guid.Parse(session_id.Text));

                XmlDocument doc = new XmlDocument();
                doc.LoadXml(cmd.Build());
                
                session.ExecuteCommand(this, doc, (response) =>
                {
                    txtMessage.Text = "Disable Device Failed.";
                    if (BaseMessage.IsResponseKey(response.Xml, CmdEnableDevice.MSG_KEY))
                    {
                        GeneralResponse re = new GeneralResponse();
                        if (re.ParseResult(response.Xml) == CommandExeResult.OK)
                            txtMessage.Text = "Disable Device Success!";
                    }
                }, (ex) => { error_message.Text = ex.Message; });
            }
            catch (Exception ex)
            {
                error_message.Text = ex.Message;
            }
        }

        protected void btnDeviceStatusGetAll_Click(object sender, EventArgs e)
        {
            spanResult.InnerText = "";
            CmdGetDeviceStatusAll cmd = new CmdGetDeviceStatusAll();
            try
            {
                var session = SessionRegistry.GetSession(Guid.Parse(session_id.Text));

                XmlDocument doc = new XmlDocument();
                doc.LoadXml(cmd.Build());

                session.ExecuteCommand(this, doc, (response) =>
                {
                    CmdGetDeviceStatusAllResponse cmd_resp = new CmdGetDeviceStatusAllResponse();
                    if (cmd_resp.Parse(response.Xml))
                    {
                        txtMessage.Text = "GetDeviceStatusAll Success.";
                        spanResult.InnerText = cmd_resp.result_str;
                    }
                    else
                        txtMessage.Text = "GetDeviceStatusAll Failed.";
                }, (ex) => { error_message.Text = ex.Message; });
            }
            catch (Exception ex)
            {
                error_message.Text = ex.Message;
            }
        }
        protected void btnDeviceInfoGetAll_Click(object sender, EventArgs e)
        {
            spanResult.InnerText = "";
            CmdGetDeviceInfoAll cmd = new CmdGetDeviceInfoAll();
            try
            {
                var session = SessionRegistry.GetSession(Guid.Parse(session_id.Text));

                XmlDocument doc = new XmlDocument();
                doc.LoadXml(cmd.Build());

                session.ExecuteCommand(this, doc, (response) =>
                {
                    CmdGetDeviceInfoAllResponse cmd_resp = new CmdGetDeviceInfoAllResponse();
                    if (cmd_resp.Parse(response.Xml))
                    {
                        txtMessage.Text = "Get Device Info All Success.";
                        spanResult.InnerText = cmd_resp.result_str;
                    }
                    else
                        txtMessage.Text = "Get Device Info All Failed.";
                }, (ex) => { error_message.Text = ex.Message; });
            }
            catch (Exception ex)
            {
                error_message.Text = ex.Message;
            }
        }
        protected void btnDeviceInfoGet_Click(object sender, EventArgs e)
        {
            CmdGetDeviceInfo cmd = new CmdGetDeviceInfo(device_info_params[comboDeviceInfo.SelectedIndex]);
            try
            {
                var session = SessionRegistry.GetSession(Guid.Parse(session_id.Text));

                XmlDocument doc = new XmlDocument();
                doc.LoadXml(cmd.Build());
                
                session.ExecuteCommand(this, doc, (response) =>
                {
                    CmdGetDeviceInfoResponse cmd_resp = new CmdGetDeviceInfoResponse();
                    if (cmd_resp.Parse(response.Xml))
                    {
                        if (device_info_params[comboDeviceInfo.SelectedIndex] == DevInfoParamType.UtcTimezoneMinutes)
                            txtMessage.Text = device_info_params[comboDeviceInfo.SelectedIndex].ToString() + " = " + ((int)cmd_resp.param_val).ToString();
                        else
                            txtMessage.Text = device_info_params[comboDeviceInfo.SelectedIndex].ToString() + " = " + cmd_resp.param_val.ToString();
                    }
                    else
                        txtMessage.Text = "Get Device Info Failed";
                }, (ex) => { error_message.Text = ex.Message; });
            }
            catch (Exception ex)
            {
                error_message.Text = ex.Message;
            }
        }

        protected void btnDeviceInfoSet_Click(object sender, EventArgs e)
        {
            try
            {
                CmdSetDeviceInfo cmd = new CmdSetDeviceInfo(device_info_params[comboDeviceInfo.SelectedIndex], Convert.ToUInt32(txtSetDeviceInfoParam.Text));
                var session = SessionRegistry.GetSession(Guid.Parse(session_id.Text));

                XmlDocument doc = new XmlDocument();
                doc.LoadXml(cmd.Build());
                
                session.ExecuteCommand(this, doc, (response) =>
                {
                    txtMessage.Text = "Set DeviceInfo Failed.";
                    if (BaseMessage.IsResponseKey(response.Xml, CmdSetDeviceInfo.MSG_KEY))
                    {
                        GeneralResponse re = new GeneralResponse();
                        if (re.ParseResult(response.Xml) == CommandExeResult.OK)
                            txtMessage.Text = "Set Success! (" + device_info_params[comboDeviceInfo.SelectedIndex].ToString() + ")";
                    }
                }, (ex) => { error_message.Text = ex.Message; });
            }
            catch (Exception ex)
            {
                error_message.Text = ex.Message;
            }
        }
        protected void btnDeviceStatusGet_Click(object sender, EventArgs e)
        {
            CmdGetDeviceStatus cmd = new CmdGetDeviceStatus(device_status_params[comboDeviceStatus.SelectedIndex]);
            try
            {
                var session = SessionRegistry.GetSession(Guid.Parse(session_id.Text));

                XmlDocument doc = new XmlDocument();
                doc.LoadXml(cmd.Build());
                
                session.ExecuteCommand(this, doc, (response) =>
                {
                    CmdGetDeviceStatusResponse cmd_resp = new CmdGetDeviceStatusResponse();
                    if (cmd_resp.Parse(response.Xml))
                        txtMessage.Text = device_status_params[comboDeviceStatus.SelectedIndex].ToString() + " = " + cmd_resp.param_val.ToString();
                    else
                        txtMessage.Text = "Get Device Status Failed";
                }, (ex) => { error_message.Text = ex.Message; });
            }
            catch (Exception ex)
            {
                error_message.Text = ex.Message;
            }
        }

        protected void btnLockControlStatus_Click(object sender, EventArgs e)
        {
            CmdLockControlStatus cmd = new CmdLockControlStatus();
            try
            {
                var session = SessionRegistry.GetSession(Guid.Parse(session_id.Text));

                XmlDocument doc = new XmlDocument();
                doc.LoadXml(cmd.Build());
                
                session.ExecuteCommand(this, doc, (response) =>
                {
                    CmdLockControlStatusResponse cmd_resp = new CmdLockControlStatusResponse();
                    if (cmd_resp.Parse(response.Xml))
                        txtMessage.Text = "LockStatus = " + cmd_resp.Mode.ToString();
                    else
                        txtMessage.Text = "Get Door Status Failed";
                }, (ex) => { error_message.Text = ex.Message; });
            }
            catch (Exception ex)
            {
                error_message.Text = ex.Message;
            }
        }

        protected void btnLockControl_Click(object sender, EventArgs e)
        {
            try
            {
                CmdLockControl cmd = new CmdLockControl(lock_control_params[comboLockControl.SelectedIndex]);
                var session = SessionRegistry.GetSession(Guid.Parse(session_id.Text));

                XmlDocument doc = new XmlDocument();
                doc.LoadXml(cmd.Build());
                
                session.ExecuteCommand(this, doc, (response) =>
                {
                    txtMessage.Text = "Lock Control Failed.";
                    if (BaseMessage.IsResponseKey(response.Xml, CmdLockControl.MSG_KEY))
                    {
                        GeneralResponse re = new GeneralResponse();
                        if (re.ParseResult(response.Xml) == CommandExeResult.OK)
                            txtMessage.Text = "Success! (" + lock_control_params[comboLockControl.SelectedIndex].ToString() + ")";
                    }
                }, (ex) => { error_message.Text = ex.Message; });
            }
            catch (Exception ex)
            {
                error_message.Text = ex.Message;
            }
        }

        protected void btnSetTrIcon_Click(object sender, EventArgs e)
        {
            FileStream fs = null;
            BinaryReader bw = null;
            byte[] photo_data = null;
            bool need_delete = chkDelete.Checked;

            if (!need_delete)
            {
                try
                {
                    fs = new FileStream(txtIconPath.Text, FileMode.Open);
                    if (fs.Length <= 0 || fs.Length > UserInfo.MAX_PHOTO_SIZE_64K)
                    {
                        fs.Close();
                        txtMessage.Text = "Image file size is invalid.";
                        txtIconPath.Focus();
                        return;
                    }
                    int nPhotoSize = (int)fs.Length;

                    bw = new BinaryReader(fs);
                    photo_data = bw.ReadBytes(nPhotoSize);
                    bw.Close();
                    fs.Close();
                }
                catch (Exception)
                {
                    txtMessage.Text = "Read File Failed! Please Input File Path Correctly!";
                    txtIconPath.Focus();
                    return;
                }
            }

            try
            {
                CmdSetTrIcon cmd = new CmdSetTrIcon(Convert.ToInt32(txtIconNo.Text), Convert.ToInt32(txtIconStatus.Text), need_delete, photo_data);
                try
                {
                    var session = SessionRegistry.GetSession(Guid.Parse(session_id.Text));

                    XmlDocument doc = new XmlDocument();
                    doc.LoadXml(cmd.Build());

                    session.ExecuteCommand(this, doc, (response) =>
                    {
                        CmdSetTrIconResponse cmd_resp = new CmdSetTrIconResponse();
                        var result = cmd_resp.ParseResult(response.Xml);
                        if (result == CommandExeResult.OK)
                        {
                            txtMessage.Text = "SetTrIcon Success!";
                        }
                        else
                        {
                            txtMessage.Text = "SetTrIcon Failed!";
                            if (result == CommandExeResult.Fail)
                            {
                                cmd_resp.Parse(response.Xml);
                                if (cmd_resp.fail_reason != null)
                                    txtMessage.Text += " Reason: " + cmd_resp.fail_reason;
                            }
                        }
                    }, (ex) => { txtMessage.Text = ex.Message; });
                }
                catch (Exception ex)
                {
                    txtMessage.Text = ex.Message;
                }
            }
            catch (Exception ex)
            {
                txtMessage.Text = ex.Message;
            }
        }
    }
}