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
    public partial class UserManagePane : System.Web.UI.Page
    {
        private const string ENROLLDB_DIR = "D:\\ENROLLDB_DIR\\";
        private const string ENROLLDB_DAT = ENROLLDB_DIR + "ENROLLDB.DAT";

	    private const Int32 EnrollMagic = 0x454E524F; //ENRO

        static readonly List<UserPrivilege> user_privileges = new List<UserPrivilege> {
                UserPrivilege.NormalUser,
		        UserPrivilege.Manager,
		        UserPrivilege.Administrator,
        };
        public static List<UserPrivilege> GetPrivilegeList()
        {
            return user_privileges;
        }

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


            //ChkEnabled.Checked = true;
            //
            //TextTimeSet1.Text = "0";
            //TextTimeSet2.Text = "0";
            //TextTimeSet3.Text = "0";
            //TextTimeSet4.Text = "0";
            //TextTimeSet5.Text = "0";
            //
            //TextCard.Text = "0";
            //TextDepart.Text = "0";
        }

        protected void btnGetUserData_Click(object sender, EventArgs e)
        {
            try
            {
                CmdGetUserData cmd = new CmdGetUserData(Convert.ToUInt32(TextUserID.Text));
                try
                {
                    var session = SessionRegistry.GetSession(Guid.Parse(session_id.Text));

                    XmlDocument doc = new XmlDocument();
                    doc.LoadXml(cmd.Build());

                    session.ExecuteCommand(this, doc, (response) =>
                    {
                        CmdGetUserDataResponse cmd_resp = new CmdGetUserDataResponse();
                        if (cmd_resp.Parse(response.Xml))
                        {
                            TextMessage.Text = "GetUserData Success!";
                            TextName.Text = cmd_resp.user.name;
                            TextDepart.Text = cmd_resp.user.depart.ToString();
                            TextPWD.Text = cmd_resp.user.password;
                            TextCard.Text = cmd_resp.user.card.ToString();
                            comboUserPrivileges.SelectedValue = cmd_resp.user.privilege.ToString();
                            ChkEnabled.Checked = cmd_resp.user.enabled;

                            TextTimeSet1.Text = cmd_resp.user.timeset1.ToString();
                            TextTimeSet2.Text = cmd_resp.user.timeset2.ToString();
                            TextTimeSet3.Text = cmd_resp.user.timeset3.ToString();
                            TextTimeSet4.Text = cmd_resp.user.timeset4.ToString();
                            TextTimeSet5.Text = cmd_resp.user.timeset5.ToString();

                            ChkFpEnrolled0.Checked = cmd_resp.user.fingerprints[0].enrolled;
                            ChkFpEnrolled1.Checked = cmd_resp.user.fingerprints[1].enrolled;
                            ChkFpEnrolled2.Checked = cmd_resp.user.fingerprints[2].enrolled;
                            ChkFpEnrolled3.Checked = cmd_resp.user.fingerprints[3].enrolled;
                            ChkFpEnrolled4.Checked = cmd_resp.user.fingerprints[4].enrolled;
                            ChkFpEnrolled5.Checked = cmd_resp.user.fingerprints[5].enrolled;
                            ChkFpEnrolled6.Checked = cmd_resp.user.fingerprints[6].enrolled;
                            ChkFpEnrolled7.Checked = cmd_resp.user.fingerprints[7].enrolled;
                            ChkFpEnrolled8.Checked = cmd_resp.user.fingerprints[8].enrolled;
                            ChkFpEnrolled9.Checked = cmd_resp.user.fingerprints[9].enrolled;

                            ChkFpDuress0.Checked = cmd_resp.user.fingerprints[0].duress;
                            ChkFpDuress1.Checked = cmd_resp.user.fingerprints[1].duress;
                            ChkFpDuress2.Checked = cmd_resp.user.fingerprints[2].duress;
                            ChkFpDuress3.Checked = cmd_resp.user.fingerprints[3].duress;
                            ChkFpDuress4.Checked = cmd_resp.user.fingerprints[4].duress;
                            ChkFpDuress5.Checked = cmd_resp.user.fingerprints[5].duress;
                            ChkFpDuress6.Checked = cmd_resp.user.fingerprints[6].duress;
                            ChkFpDuress7.Checked = cmd_resp.user.fingerprints[7].duress;
                            ChkFpDuress8.Checked = cmd_resp.user.fingerprints[8].duress;
                            ChkFpDuress9.Checked = cmd_resp.user.fingerprints[9].duress;

                            ChkFaceEnrolled.Checked = cmd_resp.user.face.enrolled;
                        }
                        else
                            TextMessage.Text = "GetUserData Failed!";
                    });
                }
                catch (Exception ex)
                {
                    TextMessage.Text = ex.Message;
                }
            }
            catch (Exception)
            {
                TextMessage.Text = "Please Input UserID Correctly!";
                TextUserID.Focus();
            }
        }

        protected void btnSetUserData_Click(object sender, EventArgs e)
        {
            try
            {
                UserInfo user = new UserInfo();
                user.user_id = Convert.ToInt64(TextUserID.Text);
                user.name = TextName.Text;
                user.enabled = ChkEnabled.Checked;
                user.privilege = user_privileges[comboUserPrivileges.SelectedIndex];
                user.timeset1 = Convert.ToInt32(TextTimeSet1.Text);
                user.timeset2 = Convert.ToInt32(TextTimeSet2.Text);
                user.timeset3 = Convert.ToInt32(TextTimeSet3.Text);
                user.timeset4 = Convert.ToInt32(TextTimeSet4.Text);
                user.timeset5 = Convert.ToInt32(TextTimeSet5.Text);
                user.card = Convert.ToUInt32(TextCard.Text);
                user.password = TextPWD.Text;
                user.depart = Convert.ToByte(TextDepart.Text);
                        
                CmdSetUserData cmd = new CmdSetUserData(user, SetUserMode.Set); // for delete, please select SetUserMode.Delete
                try
                {
                    var session = SessionRegistry.GetSession(Guid.Parse(session_id.Text));

                    XmlDocument doc = new XmlDocument();
                    doc.LoadXml(cmd.Build());

                    session.ExecuteCommand(this, doc, (response) =>
                    {
                        CmdSetUserDataResponse cmd_resp = new CmdSetUserDataResponse();
                        if (cmd_resp.Parse(response.Xml))
                        {
                            TextMessage.Text = "SetUserData Success!";
                        }
                        else
                            TextMessage.Text = "SetUserData Failed!";
                    });
                }
                catch (Exception ex)
                {
                    TextMessage.Text = ex.Message;
                }
            }
            catch (Exception)
            {
                TextMessage.Text = "Please Input User Information Correctly!";
                TextUserID.Focus();
            }
        }
        protected void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                UserInfo user = new UserInfo();
                user.user_id = Convert.ToInt64(TextUserID.Text);

                CmdSetUserData cmd = new CmdSetUserData(user, SetUserMode.Delete); 
                try
                {
                    var session = SessionRegistry.GetSession(Guid.Parse(session_id.Text));

                    XmlDocument doc = new XmlDocument();
                    doc.LoadXml(cmd.Build());

                    session.ExecuteCommand(this, doc, (response) =>
                    {
                        CmdSetUserDataResponse cmd_resp = new CmdSetUserDataResponse();
                        if (cmd_resp.Parse(response.Xml))
                        {
                            TextMessage.Text = "DeleteUser Success!";
                        }
                        else
                            TextMessage.Text = "DeleteUser Failed!";
                    });
                }
                catch (Exception ex)
                {
                    TextMessage.Text = ex.Message;
                }
            }
            catch (Exception)
            {
                TextMessage.Text = "Please Input User Information Correctly!";
                TextUserID.Focus();
            }
        }
        protected void btnTakeOffManager_Click(object sender, EventArgs e)
        {
            CmdTakeOffManager cmd = new CmdTakeOffManager();
            try
            {
                var session = SessionRegistry.GetSession(Guid.Parse(session_id.Text));

                XmlDocument doc = new XmlDocument();
                doc.LoadXml(cmd.Build());

                session.ExecuteCommand(this, doc, (response) =>
                {
                    GeneralResponse cmd_resp = new GeneralResponse();
                    if (cmd_resp.ParseResult(response.Xml) == CommandExeResult.OK)
                    {
                        TextMessage.Text = "TakeOffManager Success!";
                    }
                    else
                        TextMessage.Text = "TakeOffManager Failed!";
                });
            }
            catch (Exception ex)
            {
                TextMessage.Text = ex.Message;
            }
        }

        protected void btnGetFirstUserData_Click(object sender, EventArgs e)
        {
            CmdGetFirstUserDataExt cmd = new CmdGetFirstUserDataExt();
            try
            {
                var session = SessionRegistry.GetSession(Guid.Parse(session_id.Text));

                XmlDocument doc = new XmlDocument();
                doc.LoadXml(cmd.Build());

                session.ExecuteCommand(this, doc, (response) =>
                {
                    CmdGetNextUserDataResponse cmd_resp = new CmdGetNextUserDataResponse();
                    if (cmd_resp.Parse(response.Xml))
                    {
                        TextMessage.Text = "GetFirstUserData Success!";
                        if (cmd_resp.is_last)
                            TextMessage.Text += " [Last User!!!]";
                        TextUserID.Text = cmd_resp.user.user_id.ToString();
                        TextName.Text = cmd_resp.user.name;
                        TextDepart.Text = cmd_resp.user.depart.ToString();
                        TextPWD.Text = cmd_resp.user.password;
                        TextCard.Text = cmd_resp.user.card.ToString();
                        comboUserPrivileges.SelectedValue = cmd_resp.user.privilege.ToString();
                        ChkEnabled.Checked = cmd_resp.user.enabled;

                        TextTimeSet1.Text = cmd_resp.user.timeset1.ToString();
                        TextTimeSet2.Text = cmd_resp.user.timeset2.ToString();
                        TextTimeSet3.Text = cmd_resp.user.timeset3.ToString();
                        TextTimeSet4.Text = cmd_resp.user.timeset4.ToString();
                        TextTimeSet5.Text = cmd_resp.user.timeset5.ToString();

                        ChkFpEnrolled0.Checked = cmd_resp.user.fingerprints[0].enrolled;
                        ChkFpEnrolled1.Checked = cmd_resp.user.fingerprints[1].enrolled;
                        ChkFpEnrolled2.Checked = cmd_resp.user.fingerprints[2].enrolled;
                        ChkFpEnrolled3.Checked = cmd_resp.user.fingerprints[3].enrolled;
                        ChkFpEnrolled4.Checked = cmd_resp.user.fingerprints[4].enrolled;
                        ChkFpEnrolled5.Checked = cmd_resp.user.fingerprints[5].enrolled;
                        ChkFpEnrolled6.Checked = cmd_resp.user.fingerprints[6].enrolled;
                        ChkFpEnrolled7.Checked = cmd_resp.user.fingerprints[7].enrolled;
                        ChkFpEnrolled8.Checked = cmd_resp.user.fingerprints[8].enrolled;
                        ChkFpEnrolled9.Checked = cmd_resp.user.fingerprints[9].enrolled;

                        ChkFpDuress0.Checked = cmd_resp.user.fingerprints[0].duress;
                        ChkFpDuress1.Checked = cmd_resp.user.fingerprints[1].duress;
                        ChkFpDuress2.Checked = cmd_resp.user.fingerprints[2].duress;
                        ChkFpDuress3.Checked = cmd_resp.user.fingerprints[3].duress;
                        ChkFpDuress4.Checked = cmd_resp.user.fingerprints[4].duress;
                        ChkFpDuress5.Checked = cmd_resp.user.fingerprints[5].duress;
                        ChkFpDuress6.Checked = cmd_resp.user.fingerprints[6].duress;
                        ChkFpDuress7.Checked = cmd_resp.user.fingerprints[7].duress;
                        ChkFpDuress8.Checked = cmd_resp.user.fingerprints[8].duress;
                        ChkFpDuress9.Checked = cmd_resp.user.fingerprints[9].duress;

                        ChkFaceEnrolled.Checked = cmd_resp.user.face.enrolled;
                    }
                    else
                        TextMessage.Text = "GetFirstUserData Failed!";
                });
            }
            catch (Exception ex)
            {
                TextMessage.Text = ex.Message;
            }
        }
  
        protected void btnGetNextUserData_Click(object sender, EventArgs e)
        {
            try
            {
                CmdGetNextUserDataExt cmd = new CmdGetNextUserDataExt(Convert.ToUInt32(TextUserID.Text));
                try
                {
                    var session = SessionRegistry.GetSession(Guid.Parse(session_id.Text));

                    XmlDocument doc = new XmlDocument();
                    doc.LoadXml(cmd.Build());

                    session.ExecuteCommand(this, doc, (response) =>
                    {
                        CmdGetNextUserDataResponse cmd_resp = new CmdGetNextUserDataResponse();
                        if (cmd_resp.Parse(response.Xml))
                        {
                            TextMessage.Text = "GetNextUserData Success!";
                            if (cmd_resp.is_last)
                                TextMessage.Text += " [Last User!!!]";
                            TextUserID.Text = cmd_resp.user.user_id.ToString();
                            TextName.Text = cmd_resp.user.name;
                            TextDepart.Text = cmd_resp.user.depart.ToString();
                            TextPWD.Text = cmd_resp.user.password;
                            TextCard.Text = cmd_resp.user.card.ToString();
                            comboUserPrivileges.SelectedValue = cmd_resp.user.privilege.ToString();
                            ChkEnabled.Checked = cmd_resp.user.enabled;

                            TextTimeSet1.Text = cmd_resp.user.timeset1.ToString();
                            TextTimeSet2.Text = cmd_resp.user.timeset2.ToString();
                            TextTimeSet3.Text = cmd_resp.user.timeset3.ToString();
                            TextTimeSet4.Text = cmd_resp.user.timeset4.ToString();
                            TextTimeSet5.Text = cmd_resp.user.timeset5.ToString();

                            ChkFpEnrolled0.Checked = cmd_resp.user.fingerprints[0].enrolled;
                            ChkFpEnrolled1.Checked = cmd_resp.user.fingerprints[1].enrolled;
                            ChkFpEnrolled2.Checked = cmd_resp.user.fingerprints[2].enrolled;
                            ChkFpEnrolled3.Checked = cmd_resp.user.fingerprints[3].enrolled;
                            ChkFpEnrolled4.Checked = cmd_resp.user.fingerprints[4].enrolled;
                            ChkFpEnrolled5.Checked = cmd_resp.user.fingerprints[5].enrolled;
                            ChkFpEnrolled6.Checked = cmd_resp.user.fingerprints[6].enrolled;
                            ChkFpEnrolled7.Checked = cmd_resp.user.fingerprints[7].enrolled;
                            ChkFpEnrolled8.Checked = cmd_resp.user.fingerprints[8].enrolled;
                            ChkFpEnrolled9.Checked = cmd_resp.user.fingerprints[9].enrolled;

                            ChkFpDuress0.Checked = cmd_resp.user.fingerprints[0].duress;
                            ChkFpDuress1.Checked = cmd_resp.user.fingerprints[1].duress;
                            ChkFpDuress2.Checked = cmd_resp.user.fingerprints[2].duress;
                            ChkFpDuress3.Checked = cmd_resp.user.fingerprints[3].duress;
                            ChkFpDuress4.Checked = cmd_resp.user.fingerprints[4].duress;
                            ChkFpDuress5.Checked = cmd_resp.user.fingerprints[5].duress;
                            ChkFpDuress6.Checked = cmd_resp.user.fingerprints[6].duress;
                            ChkFpDuress7.Checked = cmd_resp.user.fingerprints[7].duress;
                            ChkFpDuress8.Checked = cmd_resp.user.fingerprints[8].duress;
                            ChkFpDuress9.Checked = cmd_resp.user.fingerprints[9].duress;

                            ChkFaceEnrolled.Checked = cmd_resp.user.face.enrolled;
                        }
                        else
                            TextMessage.Text = "GetNextUserData Failed! (No Next User)";
                    });
                }
                catch (Exception ex)
                {
                    TextMessage.Text = ex.Message;
                }
            }
            catch (Exception /*ex*/)
            {
                TextMessage.Text = "Please Input UserID Correctly!";
                TextUserID.Focus();
            }
        }

        protected void btnGetUserPassword_Click(object sender, EventArgs e)
        {
            try
            {
                CmdGetUserPassword cmd = new CmdGetUserPassword(Convert.ToInt64(TextUserID.Text));
                try
                {
                    var session = SessionRegistry.GetSession(Guid.Parse(session_id.Text));

                    XmlDocument doc = new XmlDocument();
                    doc.LoadXml(cmd.Build());

                    session.ExecuteCommand(this, doc, (response) =>
                    {
                        CmdGetUserPasswordResponse cmd_resp = new CmdGetUserPasswordResponse();
                        if (cmd_resp.Parse(response.Xml))
                        {
                            TextMessage.Text = "GetUserPassword Success!";
                            TextPWD.Text = cmd_resp.Password;
                            if (TextPWD.Text == null)
                                TextMessage.Text += " [No Password Enrolled For This User!]";
                        }
                        else
                            TextMessage.Text = "GetUserPassword Failed!";
                    });
                }
                catch (Exception ex)
                {
                    TextMessage.Text = ex.Message;
                }
            }
            catch (Exception)
            {
                TextMessage.Text = "Please Input UserID Correctly!";
                TextUserID.Focus();
            }
        }

        protected void btnGetUserCardNo_Click(object sender, EventArgs e)
        {
            try
            {
                CmdGetUserCardNo cmd = new CmdGetUserCardNo(Convert.ToInt64(TextUserID.Text));
                try
                {
                    var session = SessionRegistry.GetSession(Guid.Parse(session_id.Text));

                    XmlDocument doc = new XmlDocument();
                    doc.LoadXml(cmd.Build());

                    session.ExecuteCommand(this, doc, (response) =>
                    {
                        CmdGetUserCardNoResponse cmd_resp = new CmdGetUserCardNoResponse();
                        if (cmd_resp.Parse(response.Xml))
                        {
                            TextMessage.Text = "GetUserCardNo Success!";
                            TextCard.Text = cmd_resp.CardNo.ToString();
                            if (cmd_resp.CardNo == 0)
                                TextMessage.Text += " [No Card Enrolled For This User!]";
                        }
                        else
                            TextMessage.Text = "GetUserCardNo Failed!";
                    });
                }
                catch (Exception ex)
                {
                    TextMessage.Text = ex.Message;
                }
            }
            catch (Exception)
            {
                TextMessage.Text = "Please Input UserID Correctly!";
                TextUserID.Focus();
            }
        }

        protected void btnGetFingerData_Click(object sender, EventArgs e)
        {
            try
            {
                CmdGetFingerData cmd = new CmdGetFingerData(Convert.ToInt64(TextUserID.Text), Convert.ToInt32(TextFingerNo.Text), true);
                try
                {
                    var session = SessionRegistry.GetSession(Guid.Parse(session_id.Text));

                    XmlDocument doc = new XmlDocument();
                    doc.LoadXml(cmd.Build());

                    session.ExecuteCommand(this, doc, (response) =>
                    {
                        CmdGetFingerDataResponse cmd_resp = new CmdGetFingerDataResponse();
                        if (cmd_resp.Parse(response.Xml))
                        {
                            TextMessage.Text = "GetFingerData Success!";

                            FileStream fs = null;
                            BinaryWriter bw = null;
                            try
                            {
                                fs = new FileStream(TextFilePath.Text, FileMode.OpenOrCreate);
                                bw = new BinaryWriter(fs);
                                bw.Write(cmd_resp.FingerData);
                                bw.Flush();
                                bw.Close();
                                fs.Close();
                                TextMessage.Text += " Saved To ";
                                TextMessage.Text += TextFilePath.Text;
                            }
                            catch (Exception)
                            {
                                TextMessage.Text += " Save Failed! Please Input File Path Correctly!";
                                //TextFilePath.Focus();
                            }
                        }
                        else
                            TextMessage.Text = "GetFingerData Failed!";
                    });
                }
                catch (Exception ex)
                {
                    TextMessage.Text = ex.Message;
                }
            }
            catch (Exception)
            {
                TextMessage.Text = "Please Input UserID and FingerNo Correctly!";
                TextUserID.Focus();
            }
        }

        protected void btnSetFingerData_Click(object sender, EventArgs e)
        {
            FileStream fs = null;
            BinaryReader bw = null;
            byte[] fp_data = null;
            try
            {
                fs = new FileStream(TextFilePath.Text, FileMode.Open);
                bw = new BinaryReader(fs);
                fp_data = bw.ReadBytes(Fingerprint.FP_SIZE);
                bw.Close();
                fs.Close();
            }
            catch (Exception)
            {
                TextMessage.Text = "Read File Failed! Please Input File Path Correctly!";
                TextFilePath.Focus();
                return;
            }

            try
            {
                CmdSetFingerData cmd = new CmdSetFingerData(Convert.ToInt64(TextUserID.Text), 
                    Convert.ToInt32(TextFingerNo.Text),
                    ChkDuress.Checked,
                    fp_data,
                    user_privileges[comboUserPrivileges.SelectedIndex],
                    ChkDuplicationCheck.Checked);
                try
                {
                    var session = SessionRegistry.GetSession(Guid.Parse(session_id.Text));

                    XmlDocument doc = new XmlDocument();
                    doc.LoadXml(cmd.Build());

                    session.ExecuteCommand(this, doc, (response) =>
                    {
                        GeneralResponse cmd_resp = new GeneralResponse();
                        if (cmd_resp.Parse(response.Xml))
                            TextMessage.Text = "SetFingerData Success!";
                        else
                            TextMessage.Text = "SetFingerData Failed!";
                    });
                }
                catch (Exception ex)
                {
                    TextMessage.Text = ex.Message;
                }
            }
            catch (Exception)
            {
                TextMessage.Text = "Please Input UserID and FingerNo Correctly!";
                TextUserID.Focus();
            }
        }

        protected void btnGetFaceData_Click(object sender, EventArgs e)
        {
            try
            {
                CmdGetFaceData cmd = new CmdGetFaceData(Convert.ToInt64(TextUserID.Text));
                try
                {
                    var session = SessionRegistry.GetSession(Guid.Parse(session_id.Text));

                    XmlDocument doc = new XmlDocument();
                    doc.LoadXml(cmd.Build());

                    session.ExecuteCommand(this, doc, (response) =>
                    {
                        CmdGetFaceDataResponse cmd_resp = new CmdGetFaceDataResponse();
                        if (cmd_resp.Parse(response.Xml))
                        {
                            TextMessage.Text = "GetFaceData Success!";
                            FileStream fs = null;
                            BinaryWriter bw = null;
                            try
                            {
                                fs = new FileStream(TextFilePath.Text, FileMode.OpenOrCreate);
                                bw = new BinaryWriter(fs);
                                bw.Write(cmd_resp.FaceData.face_data);
                                bw.Flush();
                                bw.Close();
                                fs.Close();
                                TextMessage.Text += " Saved To ";
                                TextMessage.Text += TextFilePath.Text;
                            }
                            catch (Exception)
                            {
                                TextMessage.Text += " Save Failed! Please Input File Path Correctly!";
                                //TextFilePath.Focus();
                            }
                        }
                        else
                            TextMessage.Text = "GetFaceData Failed!";
                    });
                }
                catch (Exception ex)
                {
                    TextMessage.Text = ex.Message;
                }
            }
            catch (Exception)
            {
                TextMessage.Text = "Please Input UserID Correctly!";
                TextUserID.Focus();
            }
        }

        protected void btnSetFaceData_Click(object sender, EventArgs e)
        {
            FileStream fs = null;
            BinaryReader bw = null;
            byte[] face_data = null;
            try
            {
                fs = new FileStream(TextFilePath.Text, FileMode.Open);
                bw = new BinaryReader(fs);
                face_data = bw.ReadBytes(Face.FACE_SIZE);
                bw.Close();
                fs.Close();
            }
            catch (Exception)
            {
                TextMessage.Text = "Read File Failed! Please Input File Path Correctly!";
                TextFilePath.Focus();
                return;
            }

            try
            {
                CmdSetFaceData cmd = new CmdSetFaceData(Convert.ToInt64(TextUserID.Text),
                    face_data,
                    user_privileges[comboUserPrivileges.SelectedIndex],
                    ChkDuplicationCheck.Checked);
                try
                {
                    var session = SessionRegistry.GetSession(Guid.Parse(session_id.Text));

                    XmlDocument doc = new XmlDocument();
                    doc.LoadXml(cmd.Build());

                    session.ExecuteCommand(this, doc, (response) =>
                    {
                        GeneralResponse cmd_resp = new GeneralResponse();
                        if (cmd_resp.Parse(response.Xml))
                            TextMessage.Text = "SetFaceData Success!";
                        else
                            TextMessage.Text = "SetFaceData Failed!";
                    });
                }
                catch (Exception ex)
                {
                    TextMessage.Text = ex.Message;
                }
            }
            catch (Exception)
            {
                TextMessage.Text = "Please Input UserID Correctly!";
                TextUserID.Focus();
            }
        }

        protected void btnGetUserPhoto_Click(object sender, EventArgs e)
        {
            try
            {
                CmdGetUserPhoto cmd = new CmdGetUserPhoto(Convert.ToInt64(TextUserID.Text));
                try
                {
                    var session = SessionRegistry.GetSession(Guid.Parse(session_id.Text));

                    XmlDocument doc = new XmlDocument();
                    doc.LoadXml(cmd.Build());

                    session.ExecuteCommand(this, doc, (response) =>
                    {
                        CmdGetUserPhotoResponse cmd_resp = new CmdGetUserPhotoResponse();
                        if (cmd_resp.Parse(response.Xml))
                        {
                            TextMessage.Text = "GetUserPhoto Success!";
                            FileStream fs = null;
                            BinaryWriter bw = null;
                            try
                            {
                                fs = new FileStream(TextFilePath.Text, FileMode.OpenOrCreate);
                                bw = new BinaryWriter(fs);
                                bw.Write(cmd_resp.PhotoData);
                                bw.Flush();
                                bw.Close();
                                fs.Close();
                                TextMessage.Text += " Saved To ";
                                TextMessage.Text += TextFilePath.Text;
                            }
                            catch (Exception)
                            {
                                TextMessage.Text += " Save Failed! Please Input File Path Correctly!";
                                //TextFilePath.Focus();
                            }
                        }
                        else
                            TextMessage.Text = "GetUserPhoto Failed!";
                    });
                }
                catch (Exception ex)
                {
                    TextMessage.Text = ex.Message;
                }
            }
            catch (Exception)
            {
                TextMessage.Text = "Please Input UserID Correctly!";
                TextUserID.Focus();
            }
        }

        protected void btnSetUserPhoto_Click(object sender, EventArgs e)
        {
            FileStream fs = null;
            BinaryReader bw = null;
            byte[] photo_data = null;
            try
            {
                fs = new FileStream(TextFilePath.Text, FileMode.Open);
                if (fs.Length <= 0 || fs.Length > UserInfo.MAX_PHOTO_SIZE_32K)
				{
					fs.Close();
					TextMessage.Text = "Photo file size is invalid.";
					TextFilePath.Focus();
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
                TextMessage.Text = "Read File Failed! Please Input File Path Correctly!";
                TextFilePath.Focus();
                return;
            }

            try
            {
                CmdSetUserPhoto cmd = new CmdSetUserPhoto(Convert.ToInt64(TextUserID.Text),
                    photo_data);
                try
                {
                    var session = SessionRegistry.GetSession(Guid.Parse(session_id.Text));

                    XmlDocument doc = new XmlDocument();
                    doc.LoadXml(cmd.Build());

                    session.ExecuteCommand(this, doc, (response) =>
                    {
                        GeneralResponse cmd_resp = new GeneralResponse();
                        if (cmd_resp.Parse(response.Xml))
                            TextMessage.Text = "SetPhotoData Success!";
                        else
                            TextMessage.Text = "SetPhotoData Failed!";
                    });
                }
                catch (Exception ex)
                {
                    TextMessage.Text = ex.Message;
                }
            }
            catch (Exception)
            {
                TextMessage.Text = "Please Input UserID Correctly!";
                TextUserID.Focus();
            }
        }
        protected void btnRemoteEnrollFace_Click(object sender, EventArgs e)
        {
            try
            {
                CmdRemoteEnroll cmd = new CmdRemoteEnroll(Convert.ToInt64(TextUserID.Text), RemoteEnrollBackupType.RemoteEnrollFace);
                try
                {
                    var session = SessionRegistry.GetSession(Guid.Parse(session_id.Text));

                    XmlDocument doc = new XmlDocument();
                    doc.LoadXml(cmd.Build());

                    session.ExecuteCommand(this, doc, (response) =>
                    {
                        CmdRemoteEnrollResponse cmd_resp = new CmdRemoteEnrollResponse();
                        if (cmd_resp.ParseRemoteEnrollResult(response.Xml) == RemoteEnrollResult.Success)
                            TextMessage.Text = "RemoteEnrollFace OK.";
                        else                    
                            TextMessage.Text = "RemoteEnrollFace Failed! " + cmd_resp.remote_enroll_result.ToString();
                    });
                }
                catch (Exception ex)
                {
                    TextMessage.Text = ex.Message;
                }
            }
            catch (Exception)
            {
                TextMessage.Text = "Please Input UserID Correctly!";
                TextUserID.Focus();
            }
        }
        protected void btnRemoteEnrollFP_Click(object sender, EventArgs e)
        {
            try
            {
                CmdRemoteEnroll cmd = new CmdRemoteEnroll(Convert.ToInt64(TextUserID.Text), RemoteEnrollBackupType.RemoteEnrollFP, TextFingerNo.Text);
                try
                {
                    var session = SessionRegistry.GetSession(Guid.Parse(session_id.Text));

                    XmlDocument doc = new XmlDocument();
                    doc.LoadXml(cmd.Build());

                    session.ExecuteCommand(this, doc, (response) =>
                    {
                        CmdRemoteEnrollResponse cmd_resp = new CmdRemoteEnrollResponse();
                        if (cmd_resp.ParseRemoteEnrollResult(response.Xml) == RemoteEnrollResult.Success)
                            TextMessage.Text = "RemoteEnrollFP OK.";
                        else
                            TextMessage.Text = "RemoteEnrollFP Failed! " + cmd_resp.remote_enroll_result.ToString();
                    });
                }
                catch (Exception ex)
                {
                    TextMessage.Text = ex.Message;
                }
            }
            catch (Exception)
            {
                TextMessage.Text = "Please Input UserID Correctly!";
                TextUserID.Focus();
            }
        }
        protected void btnRemoteEnrollCard_Click(object sender, EventArgs e)
        {
            try
            {
                CmdRemoteEnroll cmd = new CmdRemoteEnroll(Convert.ToInt64(TextUserID.Text), RemoteEnrollBackupType.RemoteEnrollCard);
                try
                {
                    var session = SessionRegistry.GetSession(Guid.Parse(session_id.Text));

                    XmlDocument doc = new XmlDocument();
                    doc.LoadXml(cmd.Build());

                    session.ExecuteCommand(this, doc, (response) =>
                    {
                        CmdRemoteEnrollResponse cmd_resp = new CmdRemoteEnrollResponse();
                        if (cmd_resp.ParseRemoteEnrollResult(response.Xml) == RemoteEnrollResult.Success)
                            TextMessage.Text = "RemoteEnrollCard OK.";
                        else
                            TextMessage.Text = "RemoteEnrollCard Failed! " + cmd_resp.remote_enroll_result.ToString();
                    });
                }
                catch (Exception ex)
                {
                    TextMessage.Text = ex.Message;
                }
            }
            catch (Exception)
            {
                TextMessage.Text = "Please Input UserID Correctly!";
                TextUserID.Focus();
            }
        }

        protected void btnExitRemoteEnroll_Click(object sender, EventArgs e)
        {
            try
            {
                CmdExitRemoteEnroll cmd = new CmdExitRemoteEnroll();
                try
                {
                    var session = SessionRegistry.GetSession(Guid.Parse(session_id.Text));

                    XmlDocument doc = new XmlDocument();
                    doc.LoadXml(cmd.Build());

                    session.ExecuteCommand(this, doc, (response) =>
                    {
                        CmdExitRemoteEnrollResponse cmd_resp = new CmdExitRemoteEnrollResponse();
                        if (cmd_resp.ParseResult(response.Xml) == ExitRemoteEnrollResult.SuccessExitRemoteEnroll)
                            TextMessage.Text = "Exit RemoteEnroll Success.";
                        else
                            TextMessage.Text = "Exit RemoteEnroll Failed! " + cmd_resp.result.ToString();
                    });
                }
                catch (Exception ex)
                {
                    TextMessage.Text = ex.Message;
                }
            }
            catch (Exception)
            {
                TextMessage.Text = "Please Input UserID Correctly!";
                TextUserID.Focus();
            }
        }

        ////////////////////////////////////////////////////////////////////////////////
        void start_from_first_user()
        {
            CmdGetFirstUserDataExt cmd = new CmdGetFirstUserDataExt();
            try
            {
                var session = SessionRegistry.GetSession(Guid.Parse(session_id.Text));

                XmlDocument doc = new XmlDocument();
                doc.LoadXml(cmd.Build());

                session.ExecuteCommand(this, doc, (response) =>
                {
                    CmdGetNextUserDataResponse cmd_resp = new CmdGetNextUserDataResponse();
                    if (cmd_resp.Parse(response.Xml))
                    {
                        // save user_data except FPs and Face

                        FileStream fs = new FileStream(ENROLLDB_DAT, FileMode.Append);
                        BinaryWriter bw = new BinaryWriter(fs);
                        Utils.WriteUserInfo(bw, cmd_resp.user);
                        bw.Flush();
                        bw.Close();
                        fs.Close();

                        continue_next_info(cmd_resp.is_last, cmd_resp.user, 0);
                    }
                    else
                    {
                        TextMessage.Text = "No User Enrolled!";
                    }
                });
            }
            catch (Exception ex)
            {
                TextMessage.Text = ex.Message;
            }
        }
        void continue_next_user(Int64 user_id)
        {
            CmdGetNextUserDataExt cmd = new CmdGetNextUserDataExt(user_id);
            try
            {
                var session = SessionRegistry.GetSession(Guid.Parse(session_id.Text));

                XmlDocument doc = new XmlDocument();
                doc.LoadXml(cmd.Build());

                session.ExecuteCommand(this, doc, (response) =>
                {
                    CmdGetNextUserDataResponse cmd_resp = new CmdGetNextUserDataResponse();
                    if (cmd_resp.Parse(response.Xml))
                    {
                        // save user_data except FPs and Face

                        FileStream fs = new FileStream(ENROLLDB_DAT, FileMode.Append);
                        BinaryWriter bw = new BinaryWriter(fs);
                        Utils.WriteUserInfo(bw, cmd_resp.user);
                        bw.Flush();
                        bw.Close();
                        fs.Close();

                        continue_next_info(cmd_resp.is_last, cmd_resp.user, 0);
                    }
                    else
                        TextMessage.Text = "GetNextUserData Failed!";
                });
            }
            catch (Exception ex)
            {
                TextMessage.Text = ex.Message;
            }
        }
        void continue_next_info(bool is_last, UserInfo user, int info_index)
        {
            switch (info_index)
            {
                case 0:
                case 1:
                case 2:
                case 3:
                case 4:
                case 5:
                case 6:
                case 7:
                case 8:
                case 9:
                    if (user.fingerprints[info_index].enrolled)
                    {
                        CmdGetFingerData cmd = new CmdGetFingerData(user.user_id, info_index, true);
                        try
                        {
                            var session = SessionRegistry.GetSession(Guid.Parse(session_id.Text));

                            XmlDocument doc = new XmlDocument();
                            doc.LoadXml(cmd.Build());

                            session.ExecuteCommand(this, doc, (response) =>
                            {
                                CmdGetFingerDataResponse cmd_resp = new CmdGetFingerDataResponse();
                                if (cmd_resp.Parse(response.Xml))
                                {
                                    // save FP[info_index - 1]
                                    
                                    FileStream fs = new FileStream(ENROLLDB_DAT, FileMode.Append);
                                    BinaryWriter bw = new BinaryWriter(fs);
                                    bw.Write(cmd_resp.FingerData, 0, cmd_resp.FingerData.Length);
                                    bw.Flush();
                                    bw.Close();
                                    fs.Close();

                                    continue_next_info(is_last, user, info_index + 1);
                                }
                                else
                                    TextMessage.Text = "GetFingerData Failed!";

                            });
                        }
                        catch (Exception ex)
                        {
                            TextMessage.Text = ex.Message;
                        }
                    }
                    else
                        continue_next_info(is_last, user, info_index + 1);
                    break;
                case 10:
                    if (user.face.enrolled)
                    {
                        CmdGetFaceData cmd = new CmdGetFaceData(user.user_id);

                        try
                        {
                            var session = SessionRegistry.GetSession(Guid.Parse(session_id.Text));

                            XmlDocument doc = new XmlDocument();
                            doc.LoadXml(cmd.Build());

                            session.ExecuteCommand(this, doc, (response) =>
                            {
                                CmdGetFaceDataResponse cmd_resp = new CmdGetFaceDataResponse();
                                if (cmd_resp.Parse(response.Xml))
                                {
                                    // save Face

                                    FileStream fs = new FileStream(ENROLLDB_DAT, FileMode.Append);
                                    BinaryWriter bw = new BinaryWriter(fs);
                                    bw.Write(cmd_resp.FaceData.face_data, 0, cmd_resp.FaceData.face_data.Length);
                                    bw.Flush();
                                    bw.Close();
                                    fs.Close();

                                    continue_next_info(is_last, user, info_index + 1);
                                }
                                else
                                    TextMessage.Text = "GetFaceData Failed!";
                            });
                        }
                        catch (Exception ex)
                        {
                            TextMessage.Text = ex.Message;
                        }
                    }
                    else
                        continue_next_info(is_last, user, info_index + 1);
                    break;
                case 11:
                    {
                        CmdGetUserPhoto cmd = new CmdGetUserPhoto(user.user_id);

                        try
                        {
                            var session = SessionRegistry.GetSession(Guid.Parse(session_id.Text));

                            XmlDocument doc = new XmlDocument();
                            doc.LoadXml(cmd.Build());

                            session.ExecuteCommand(this, doc, (response) =>
                            {
                                CmdGetUserPhotoResponse cmd_resp = new CmdGetUserPhotoResponse();
                                if (cmd_resp.Parse(response.Xml))
                                {
                                    // Save Photo

                                    FileStream fs = new FileStream(ENROLLDB_DIR + user.user_id.ToString() + ".jpg", FileMode.Create);
                                    BinaryWriter bw = new BinaryWriter(fs);
                                    bw.Write(cmd_resp.PhotoData);
                                    bw.Flush();
                                    bw.Close();
                                    fs.Close();
                                }
                                else
                                {
                                    // No photo
                                }

                                if (is_last)
                                    TextMessage.Text = "GetAllUserData Success! Saved to " + ENROLLDB_DIR; 
                                else
                                    continue_next_user(user.user_id);
                            });
                        }
                        catch (Exception ex)
                        {
                            TextMessage.Text = ex.Message;
                        }
                    }
                    break;
                default:
                    break;
            }
        }
        protected void btnGetAllUserData_Click(object sender, EventArgs e)
        {
            try
            {
                Directory.CreateDirectory(ENROLLDB_DIR);
            }
            catch (Exception) { }

            try
            {
                CmdGetDeviceStatus cmd = new CmdGetDeviceStatus(DevStatusParamType.UserCount);
                var session = SessionRegistry.GetSession(Guid.Parse(session_id.Text));
               
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(cmd.Build());

                session.ExecuteCommand(this, doc, (response) =>
                {
                    CmdGetDeviceStatusResponse cmd_resp = new CmdGetDeviceStatusResponse();
                    if (cmd_resp.Parse(response.Xml))
                    {
                        UInt32 user_count = cmd_resp.param_val;

                        if (user_count == 0)
                            TextMessage.Text = "No User Enrolled!";
                        else
                        {
                            try
                            {
                                FileStream fs = new FileStream(ENROLLDB_DAT, FileMode.Create);
                                BinaryWriter bw = new BinaryWriter(fs);
                                bw.Write(EnrollMagic);
                                bw.Write(user_count);
                                bw.Flush();
                                bw.Close();
                                fs.Close();

                                start_from_first_user();
                            }
                            catch (Exception)
                            {
                                TextMessage.Text += "FileOpen Failed! " + ENROLLDB_DAT;
                            }
                        }
                    }
                    else
                        TextMessage.Text = "Get User Count Failed";
                });
            }
            catch (Exception ex)
            {
                TextMessage.Text = ex.Message;
            }
        }

        UserInfo[] users;
        void continue_set_user(int user_index)
        {
            try
            {
                var session = SessionRegistry.GetSession(Guid.Parse(session_id.Text));
                CmdSetUserData cmd = new CmdSetUserData(users[user_index], SetUserMode.Set);
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(cmd.Build());
                session.ExecuteCommand(this, doc, (response) =>
                {
                    CmdSetUserDataResponse cmd_resp = new CmdSetUserDataResponse();
                    if (cmd_resp.Parse(response.Xml))
                        continue_set_info(user_index, 11);      // continue from current user, photo
                    else
                        TextMessage.Text = "SetUserData Failed!";
                });
            }
            catch (Exception ex)
            {
                TextMessage.Text = "Set Failed! " + ex.Message;
            }
        }
        void continue_set_info(int user_index, int info_index)
        {
            switch (info_index)
            {
                case 0:
                case 1:
                case 2:
                case 3:
                case 4:
                case 5:
                case 6:
                case 7:
                case 8:
                case 9:
                    if (users[user_index].fingerprints[info_index].enrolled)
                    {
                        CmdSetFingerData cmd = new CmdSetFingerData(
                            users[user_index].user_id,
                            info_index,
                            false,
                            users[user_index].fingerprints[info_index].fp_info,
                            users[user_index].privilege,
                            true);
                        try
                        {
                            var session = SessionRegistry.GetSession(Guid.Parse(session_id.Text));
                            XmlDocument doc = new XmlDocument();
                            doc.LoadXml(cmd.Build());

                            session.ExecuteCommand(this, doc, (response) =>
                            {
                                GeneralResponse cmd_resp = new GeneralResponse();
                                if (cmd_resp.Parse(response.Xml))
                                    continue_set_info(user_index, info_index + 1);
                                else
                                   TextMessage.Text = "SetFingerData Failed!";
                            });
                        }
                        catch (Exception ex)
                        {
                            TextMessage.Text = ex.Message;
                        }
                    }
                    else
                        continue_set_info(user_index, info_index + 1);
                    break;
                case 10:
                    /* skip CmdSetFaceData, because FaceData will be set in CmdSetUserData later.]*/
                    continue_set_user(user_index);          // set current user data. for password, card, face
                    break;
                case 11:
                    {
                        byte[] photo_data = null;
                        try
                        {
                            FileStream fs = new FileStream(ENROLLDB_DIR + users[user_index].user_id.ToString() + ".jpg", FileMode.Open);
                            if (fs.Length <= 0 || fs.Length > UserInfo.MAX_PHOTO_SIZE_32K)
							{
								// Invalid Photo
							}
                            else
							{
                                int nPhotoSize = (int)fs.Length;
								BinaryReader br = new BinaryReader(fs);
								photo_data = br.ReadBytes(nPhotoSize);
								br.Close();
							}
							fs.Close();
                        }
                        catch (Exception)
                        {
                            // No Photo
                        }

                        if (photo_data != null)
                        {
                            CmdSetUserPhoto cmd = new CmdSetUserPhoto(users[user_index].user_id, photo_data);
                            try
                            {
                                var session = SessionRegistry.GetSession(Guid.Parse(session_id.Text));

                                XmlDocument doc = new XmlDocument();
                                doc.LoadXml(cmd.Build());

                                session.ExecuteCommand(this, doc, (response) =>
                                {
                                    GeneralResponse cmd_resp = new GeneralResponse();
                                    if (cmd_resp.Parse(response.Xml))
                                    {
                                        if (user_index + 1 < users.Length)
                                            continue_set_info(user_index + 1, 0);   // continue from next user, fp0 
                                        else
                                            TextMessage.Text = "SetAllUserData Success!";
                                    }
                                    else
                                        TextMessage.Text = "SetPhotoData Failed!";
                                });
                            }
                            catch (Exception ex)
                            {
                                TextMessage.Text = ex.Message;
                            }
                        }
                        else
                        {
                            if (user_index + 1 < users.Length)
                                continue_set_info(user_index + 1, 0);   // continue from next user, fp0 
                            else
                                TextMessage.Text = "SetAllUserData Success!";
                        }
                    }
                    break;
                default:
                    break;
            }
        }
        protected void btnSetAllUserData_Click(object sender, EventArgs e)
        {
            try
            {
                FileStream fs = new FileStream(ENROLLDB_DAT, FileMode.Open);
                BinaryReader br = new BinaryReader(fs);
                Int32 magic = br.ReadInt32();
                Int32 user_count = br.ReadInt32();
                bool read_success = false;

                if (magic != EnrollMagic)
                    TextMessage.Text = "Invalid Enroll Database File! " + ENROLLDB_DAT;
                else if (user_count == 0)
                    TextMessage.Text = "Enroll Database Empty! " + ENROLLDB_DAT;
                else
                {
                    users = new UserInfo[user_count];
                    for (int k = 0; k < user_count; k++)
                    {
                        try
                        {
                            users[k] = Utils.ReadUserInfo(br);
                            for (int i = Convert.ToInt32(UserBackup.BackupFingerStart); i <= Convert.ToInt32(UserBackup.BackupFingerEnd); i++)
                            {
                                if ((users[k].enroll_mask & (1 << Convert.ToInt32(i))) > 0)
                                {
                                    users[k].fingerprints[i - Convert.ToInt32(UserBackup.BackupFingerStart)].fp_info = new byte[Fingerprint.FP_SIZE];
                                    br.Read(users[k].fingerprints[i - Convert.ToInt32(UserBackup.BackupFingerStart)].fp_info, 0, Fingerprint.FP_SIZE);
                                }
                            }
                            if ((users[k].enroll_mask & (1 << Convert.ToInt32(UserBackup.BackupFace))) > 0)
                            {
                                users[k].face.face_data = new byte[Face.FACE_SIZE];
                                br.Read(users[k].face.face_data, 0, Face.FACE_SIZE);
                            }
                            read_success = true;
                        }
                        catch (Exception ex)
                        {
                            TextMessage.Text = "Read Failed!. Enroll Database File " + ENROLLDB_DAT;
                            TextMessage.Text += "\r\n" + ex.Message;
                            break;
                        }
                    }
                }
                br.Close();
                fs.Close();

                if (read_success)
                    continue_set_info(0, 0);
            }
            catch (Exception ex)
            {
                TextMessage.Text = "FileOpen Failed! " + ENROLLDB_DAT;
                TextMessage.Text += "\r\n" + ex.Message;
                TextFilePath.Focus();
            }
        }
        /////////////////////////////////////////////////////////////////////////////////////
        void continue_get_user(Int64 user_id)
        {
            CmdGetUserData cmd = new CmdGetUserData(user_id);
            try
            {
                var session = SessionRegistry.GetSession(Guid.Parse(session_id.Text));

                XmlDocument doc = new XmlDocument();
                doc.LoadXml(cmd.Build());

                session.ExecuteCommand(this, doc, (response) =>
                {
                    CmdGetUserDataResponse cmd_resp = new CmdGetUserDataResponse();
                    if (cmd_resp.Parse(response.Xml))
                    {
                        // save user_data except FPs and Face
                        continue_get_next_info(cmd_resp.user, 0);
                    }
                    else
                    {
                        // delete user  and continue

                        DeviceUpdatedUserQueue.remove(device_uid.Text, user_id);
                        
                        if (!DeviceUpdatedUserQueue.find(device_uid.Text, out user_id))
                            TextMessage.Text = "GetAllUpdatedUser Success.";
                        else
                            continue_get_user(user_id);
                    }
                });
            }
            catch (Exception ex)
            {
                TextMessage.Text = ex.Message;
            }
        }
        void continue_get_next_info(UserInfo user, int info_index)
        {
            switch (info_index)
            {
                case 0:
                case 1:
                case 2:
                case 3:
                case 4:
                case 5:
                case 6:
                case 7:
                case 8:
                case 9:
                    if (user.fingerprints[info_index].enrolled)
                    {
                        CmdGetFingerData cmd = new CmdGetFingerData(user.user_id, info_index, true);
                        try
                        {
                            var session = SessionRegistry.GetSession(Guid.Parse(session_id.Text));

                            XmlDocument doc = new XmlDocument();
                            doc.LoadXml(cmd.Build());

                            session.ExecuteCommand(this, doc, (response) =>
                            {
                                CmdGetFingerDataResponse cmd_resp = new CmdGetFingerDataResponse();
                                if (cmd_resp.Parse(response.Xml))
                                {
                                    // save FP[info_index - 1]
                                    continue_get_next_info(user, info_index + 1);
                                }
                                else
                                    TextMessage.Text = "GetFingerData Failed!";

                            });
                        }
                        catch (Exception ex)
                        {
                            TextMessage.Text = ex.Message;
                        }
                    }
                    else
                        continue_get_next_info(user, info_index + 1);
                    break;
                case 10:
                    if (user.face.enrolled)
                    {
                        CmdGetFaceData cmd = new CmdGetFaceData(user.user_id);

                        try
                        {
                            var session = SessionRegistry.GetSession(Guid.Parse(session_id.Text));

                            XmlDocument doc = new XmlDocument();
                            doc.LoadXml(cmd.Build());

                            session.ExecuteCommand(this, doc, (response) =>
                            {
                                CmdGetFaceDataResponse cmd_resp = new CmdGetFaceDataResponse();
                                if (cmd_resp.Parse(response.Xml))
                                {
                                    // save Face

                                    Int64 user_id = user.user_id;
                                    DeviceUpdatedUserQueue.remove(device_uid.Text, user_id);

                                    if (!DeviceUpdatedUserQueue.find(device_uid.Text, out user_id))
                                        TextMessage.Text = "GetAllUpdatedUser Success.";
                                    else
                                        continue_get_user(user_id);
                                }
                                else
                                    TextMessage.Text = "GetFaceData Failed!";
                            });
                        }
                        catch (Exception ex)
                        {
                            TextMessage.Text = ex.Message;
                        }
                    }
                    else
                    {
                        Int64 user_id = user.user_id;
                        DeviceUpdatedUserQueue.remove(device_uid.Text, user_id);

                        if (!DeviceUpdatedUserQueue.find(device_uid.Text, out user_id))
                            TextMessage.Text = "GetAllUpdatedUser Success.";
                        else
                            continue_get_user(user_id);
                    }
                    break;
                default:
                    break;
            }
        }

        protected void btnGetAllUpdatedUserData_Click(object sender, EventArgs e)
        {
            Int64 user_id;
            if (!DeviceUpdatedUserQueue.find(device_uid.Text, out user_id))
                TextMessage.Text = "No Updated User.";
            else
                continue_get_user(user_id);
        }


        //////////////////////////////////////////////////////////////////////////////////////////
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            lock (Session.SyncRoot)
            {
                Session["cancelled"] = true;
            }
        }
        protected void btnGetAllUserData2_Click(object sender, EventArgs e)
        {
            Session["cancelled"] = false;
            Session["ready"] = true;

            msg.Text = "Getting user count...";
            Session["next_cmd"] = "get_user_count";
           
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

            if (cur_cmd == "get_user_count")
            {
                CmdGetDeviceStatus cmd = new CmdGetDeviceStatus(DevStatusParamType.UserCount);

                var session = SessionRegistry.GetSession(Guid.Parse(session_id.Text));
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(cmd.Build());

                session.ExecuteCommand(this, doc, (response) =>
                {
                    CmdGetDeviceStatusResponse cmd_resp = new CmdGetDeviceStatusResponse();
                    if (cmd_resp.Parse(response.Xml))
                    {
                        UInt32 user_count = cmd_resp.param_val;

                        if (user_count == 0)
                            msg.Text = "No User Enrolled!";
                        else
                        {
                            msg.Text = "User Count : " + user_count.ToString();
                            Session["total_count"] = user_count;
                            Session["cur_count"] = 0;

                            try
                            {
                                FileStream fs = new FileStream(ENROLLDB_DAT, FileMode.Create);
                                BinaryWriter bw = new BinaryWriter(fs);
                                bw.Write(EnrollMagic);
                                bw.Write(user_count);
                                bw.Flush();
                                bw.Close();
                                fs.Close();

                                Session["next_cmd"] = "disable_device";
                            }
                            catch (Exception)
                            {
                                msg.Text = "FileOpen Failed! " + ENROLLDB_DAT;
                            }
                        }
                    }
                });
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
                                msg.Text = "Disable Device Success!";
                                Session["next_cmd"] = "start_from_first_user";
                            }
                        }
                    });
                }
                catch (Exception)
                {
                    msg.Text = "Disable Device Failed!";
                }
            }
            else if (cur_cmd == "start_from_first_user")
            {
                CmdGetFirstUserDataExt cmd = new CmdGetFirstUserDataExt();
                try
                {
                    var session = SessionRegistry.GetSession(Guid.Parse(session_id.Text));
                    XmlDocument doc = new XmlDocument();
                    doc.LoadXml(cmd.Build());

                    session.ExecuteCommand(this, doc, (response) =>
                    {
                        CmdGetNextUserDataResponse cmd_resp = new CmdGetNextUserDataResponse();
                        if (cmd_resp.Parse(response.Xml))
                        {
                            Session["cur_count"] = Convert.ToInt32(Session["cur_count"]) + 1;

                            // save user_data except FPs and Face

                            FileStream fs = new FileStream(ENROLLDB_DAT, FileMode.Append);
                            BinaryWriter bw = new BinaryWriter(fs);
                            Utils.WriteUserInfo(bw, cmd_resp.user);
                            bw.Flush();
                            bw.Close();
                            fs.Close();

                            msg.Text = "GetUserData(userid:" + cmd_resp.user.user_id.ToString() + ") ";
                            msg.Text += Session["cur_count"].ToString() + "/" + Session["total_count"].ToString();
                            
                            Session["next_cmd"] = "continue_next_info";
                            {
                                Session["is_last"] = cmd_resp.is_last;
                                Session["user"] = cmd_resp.user;
                                Session["info_index"] = 0;
                            }
                        }
                        else
                            msg.Text = "No User Enrolled!";
                    });
                }
                catch (Exception ex)
                {
                    msg.Text = ex.Message;
                }
            }
            else if (cur_cmd == "continue_next_user")
            {
                Int64 user_id = Convert.ToInt64(Session["user_id"]);

                CmdGetNextUserDataExt cmd = new CmdGetNextUserDataExt(user_id);
                try
                {
                    var session = SessionRegistry.GetSession(Guid.Parse(session_id.Text));

                    XmlDocument doc = new XmlDocument();
                    doc.LoadXml(cmd.Build());

                    session.ExecuteCommand(this, doc, (response) =>
                    {
                        CmdGetNextUserDataResponse cmd_resp = new CmdGetNextUserDataResponse();
                        if (cmd_resp.Parse(response.Xml))
                        {
                            Session["cur_count"] = Convert.ToInt32(Session["cur_count"]) + 1;
                            
                            // save user_data except FPs and Face

                            FileStream fs = new FileStream(ENROLLDB_DAT, FileMode.Append);
                            BinaryWriter bw = new BinaryWriter(fs);
                            Utils.WriteUserInfo(bw, cmd_resp.user);
                            bw.Flush();
                            bw.Close();
                            fs.Close();

                            msg.Text = "GetUserData(userid:" + cmd_resp.user.user_id.ToString() + ") ";
                            msg.Text += Session["cur_count"].ToString() + "/" + Session["total_count"].ToString();
                            
                            Session["next_cmd"] = "continue_next_info";
                            {
                                Session["is_last"] = cmd_resp.is_last;
                                Session["user"] = cmd_resp.user;
                                Session["info_index"] = 0;
                            }
                        }
                        else
                            msg.Text = "GetNextUserData Failed!";
                    });
                }
                catch (Exception ex)
                {
                    msg.Text = ex.Message;
                }
            }
            else if (cur_cmd == "continue_next_info")
            {
                bool is_last = Convert.ToBoolean(Session["is_last"]);
                UserInfo user = Session["user"] as UserInfo;
                int info_index = Convert.ToInt32(Session["info_index"]);

                msg.Text = "GetUserInfo(userid:" + user.user_id.ToString() + ", info:" + info_index + ") ";
                msg.Text += Session["cur_count"].ToString() + "/" + Session["total_count"].ToString();

                switch (info_index)
                {
                    case 0:
                    case 1:
                    case 2:
                    case 3:
                    case 4:
                    case 5:
                    case 6:
                    case 7:
                    case 8:
                    case 9:
                        if (user.fingerprints[info_index].enrolled)
                        {
                            CmdGetFingerData cmd = new CmdGetFingerData(user.user_id, info_index, true);
                            try
                            {
                                var session = SessionRegistry.GetSession(Guid.Parse(session_id.Text));

                                XmlDocument doc = new XmlDocument();
                                doc.LoadXml(cmd.Build());

                                session.ExecuteCommand(this, doc, (response) =>
                                {
                                    CmdGetFingerDataResponse cmd_resp = new CmdGetFingerDataResponse();
                                    if (cmd_resp.Parse(response.Xml))
                                    {
                                        // save FP[info_index - 1]

                                        FileStream fs = new FileStream(ENROLLDB_DAT, FileMode.Append);
                                        BinaryWriter bw = new BinaryWriter(fs);
                                        bw.Write(cmd_resp.FingerData, 0, cmd_resp.FingerData.Length);
                                        bw.Flush();
                                        bw.Close();
                                        fs.Close();

                                        Session["next_cmd"] = "continue_next_info";
                                        {
                                            Session["is_last"] = is_last;
                                            Session["user"] = user;
                                            Session["info_index"] = info_index + 1;
                                        }
                                    }
                                    else
                                        msg.Text = "GetFingerData Failed!";

                                });
                            }
                            catch (Exception ex)
                            {
                                msg.Text = ex.Message;
                            }
                        }
                        else
                        {
                            Session["next_cmd"] = "continue_next_info";
                            {
                                Session["is_last"] = is_last;
                                Session["user"] = user;
                                Session["info_index"] = info_index + 1;
                            } 
                        }
                        break;
                    case 10:
                        if (user.face.enrolled)
                        {
                            CmdGetFaceData cmd = new CmdGetFaceData(user.user_id);

                            try
                            {
                                var session = SessionRegistry.GetSession(Guid.Parse(session_id.Text));

                                XmlDocument doc = new XmlDocument();
                                doc.LoadXml(cmd.Build());

                                session.ExecuteCommand(this, doc, (response) =>
                                {
                                    CmdGetFaceDataResponse cmd_resp = new CmdGetFaceDataResponse();
                                    if (cmd_resp.Parse(response.Xml))
                                    {
                                        // save Face

                                        FileStream fs = new FileStream(ENROLLDB_DAT, FileMode.Append);
                                        BinaryWriter bw = new BinaryWriter(fs);
                                        bw.Write(cmd_resp.FaceData.face_data, 0, cmd_resp.FaceData.face_data.Length);
                                        bw.Flush();
                                        bw.Close();
                                        fs.Close();

                                        Session["next_cmd"] = "continue_next_info";
                                        {
                                            Session["is_last"] = is_last;
                                            Session["user"] = user;
                                            Session["info_index"] = info_index + 1;
                                        }
                                    }
                                    else
                                        msg.Text = "GetFaceData Failed!";
                                });
                            }
                            catch (Exception ex)
                            {
                                msg.Text = ex.Message;
                            }
                        }
                        else
                        {
                            Session["next_cmd"] = "continue_next_info";
                            {
                                Session["is_last"] = is_last;
                                Session["user"] = user;
                                Session["info_index"] = info_index + 1;
                            }
                        }
                        break;
                    case 11:
                        {
                            CmdGetUserPhoto cmd = new CmdGetUserPhoto(user.user_id);

                            try
                            {
                                var session = SessionRegistry.GetSession(Guid.Parse(session_id.Text));

                                XmlDocument doc = new XmlDocument();
                                doc.LoadXml(cmd.Build());

                                session.ExecuteCommand(this, doc, (response) =>
                                {
                                    CmdGetUserPhotoResponse cmd_resp = new CmdGetUserPhotoResponse();
                                    if (cmd_resp.Parse(response.Xml))
                                    {
                                        // Save Photo

                                        FileStream fs = new FileStream(ENROLLDB_DIR + user.user_id.ToString() + ".jpg", FileMode.Create);
                                        BinaryWriter bw = new BinaryWriter(fs);
                                        bw.Write(cmd_resp.PhotoData);
                                        bw.Flush();
                                        bw.Close();
                                        fs.Close();
                                    }
                                    else
                                    {
                                        // No photo
                                    }

                                    if (is_last)
                                    {
                                        msg.Text = "GetAllUserData Success! Saved to " + ENROLLDB_DIR;
                                        Session["next_cmd"] = "enable_device";
                                    }
                                    else
                                    {
                                        Session["next_cmd"] = "continue_next_user";
                                        {
                                            Session["user_id"] = user.user_id;
                                        }
                                    }
                                });
                            }
                            catch (Exception ex)
                            {
                                msg.Text = ex.Message;
                            }
                        }
                        break;
                    default:
                        break;
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
                    });
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