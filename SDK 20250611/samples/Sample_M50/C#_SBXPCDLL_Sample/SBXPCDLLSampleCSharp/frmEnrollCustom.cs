using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.Runtime.InteropServices;
using System.IO;
using System.Xml.Linq;

namespace SBXPCDLLSampleCSharp
{
	public partial class frmEnrollCustom : Form
	{

		public frmEnrollCustom()
		{
			InitializeComponent();
		}
		private void frmEnrollCustom_Load(object sender, EventArgs e)
		{

		}

        private void frmEnrollCustom_FormClosed(object sender, FormClosedEventArgs e)
		{
			Application.OpenForms["frmEnroll"].Visible = true;
		}

        private void btnGetUserVerifyCount_Click(object sender, EventArgs e)
        {
            Boolean bRet;
            int vErrorCode = 0;
			int lUserID;
            String strXML = "";

            lblMessage.Text = "Working...";
            Application.DoEvents();

            bRet = sbxpc.SBXPCDLL.EnableDevice(Program.gMachineNumber, 0);
            if (!bRet)
            {
                lblMessage.Text = util.gstrNoDevice;
                return;
            }

            lUserID = Convert.ToInt32(txtEnrollNumber.Text);

            sbxpc.SBXPCDLL.XML_AddString(ref strXML, "REQUEST", "GetUserVerifyCount");
            sbxpc.SBXPCDLL.XML_AddString(ref strXML, "MSGTYPE", "request");
            sbxpc.SBXPCDLL.XML_AddInt(ref strXML, "MachineID", Program.gMachineNumber);
            sbxpc.SBXPCDLL.XML_AddInt(ref strXML, "UserID", lUserID);

            bRet = sbxpc.SBXPCDLL.GeneralOperationXML(Program.gMachineNumber, ref strXML);

            if (bRet)
            {
                chkUseVerifyCount.Checked = (sbxpc.SBXPCDLL.XML_ParseInt(ref strXML, "Used") != 0);
                txtVerifyCount.Text = sbxpc.SBXPCDLL.XML_ParseInt(ref strXML, "Count").ToString();
                lblMessage.Text = "Success!";
            }
            else
            {
                sbxpc.SBXPCDLL.GetLastError(Program.gMachineNumber, out vErrorCode);
                lblMessage.Text = util.ErrorPrint(vErrorCode);
            }
            sbxpc.SBXPCDLL.EnableDevice(Program.gMachineNumber, 1);
        }

        private void btnSetUserVerifyCount_Click(object sender, EventArgs e)
        {
            Boolean bRet;
            int vErrorCode = 0;
            int lUserID;
            String strXML = "";

            lblMessage.Text = "Waiting...";
            Application.DoEvents();

            bRet = sbxpc.SBXPCDLL.EnableDevice(Program.gMachineNumber, 0);
            if (!bRet)
            {
                lblMessage.Text = util.gstrNoDevice;
                return;
            }

            lUserID = Convert.ToInt32(txtEnrollNumber.Text);

            sbxpc.SBXPCDLL.XML_AddString(ref strXML, "REQUEST", "SetUserVerifyCount");
            sbxpc.SBXPCDLL.XML_AddString(ref strXML, "MSGTYPE", "request");
            sbxpc.SBXPCDLL.XML_AddInt(ref strXML, "MachineID", Program.gMachineNumber);
            sbxpc.SBXPCDLL.XML_AddInt(ref strXML, "UserID", lUserID);

            sbxpc.SBXPCDLL.XML_AddInt(ref strXML, "Used", chkUseVerifyCount.Checked ? 1 : 0);
            sbxpc.SBXPCDLL.XML_AddInt(ref strXML, "Count", int.Parse(txtVerifyCount.Text));

            bRet = sbxpc.SBXPCDLL.GeneralOperationXML(Program.gMachineNumber, ref strXML);

            if (bRet)
            {
                lblMessage.Text = "Success!";
            }
            else
            {
                sbxpc.SBXPCDLL.GetLastError(Program.gMachineNumber, out vErrorCode);
                lblMessage.Text = util.ErrorPrint(vErrorCode);
            }
            sbxpc.SBXPCDLL.EnableDevice(Program.gMachineNumber, 1);
        }

        private void btnGetUserAttendOnly_Click(object sender, EventArgs e)
        {
            Boolean bRet;
            int vErrorCode = 0;
            int lUserID;
            String strXML = "";

            lblMessage.Text = "Working...";
            Application.DoEvents();

            bRet = sbxpc.SBXPCDLL.EnableDevice(Program.gMachineNumber, 0);
            if (!bRet)
            {
                lblMessage.Text = util.gstrNoDevice;
                return;
            }

            lUserID = Convert.ToInt32(txtEnrollNumber.Text);

            sbxpc.SBXPCDLL.XML_AddString(ref strXML, "REQUEST", "GetUserAttendOnly");
            sbxpc.SBXPCDLL.XML_AddString(ref strXML, "MSGTYPE", "request");
            sbxpc.SBXPCDLL.XML_AddInt(ref strXML, "MachineID", Program.gMachineNumber);
            sbxpc.SBXPCDLL.XML_AddInt(ref strXML, "UserID", lUserID);

            bRet = sbxpc.SBXPCDLL.GeneralOperationXML(Program.gMachineNumber, ref strXML);

            if (bRet)
            {
                chkUserAttendOnly.Checked = sbxpc.SBXPCDLL.XML_ParseBoolean(ref strXML, "Value");
                lblMessage.Text = "Success!";
            }
            else
            {
                sbxpc.SBXPCDLL.GetLastError(Program.gMachineNumber, out vErrorCode);
                lblMessage.Text = util.ErrorPrint(vErrorCode);
            }
            sbxpc.SBXPCDLL.EnableDevice(Program.gMachineNumber, 1);
        }

        private void btnSetUserAttendOnly_Click(object sender, EventArgs e)
        {
            Boolean bRet;
            int vErrorCode = 0;
            int lUserID;
            String strXML = "";

            lblMessage.Text = "Waiting...";
            Application.DoEvents();

            bRet = sbxpc.SBXPCDLL.EnableDevice(Program.gMachineNumber, 0);
            if (!bRet)
            {
                lblMessage.Text = util.gstrNoDevice;
                return;
            }

            lUserID = Convert.ToInt32(txtEnrollNumber.Text);

            sbxpc.SBXPCDLL.XML_AddString(ref strXML, "REQUEST", "SetUserAttendOnly");
            sbxpc.SBXPCDLL.XML_AddString(ref strXML, "MSGTYPE", "request");
            sbxpc.SBXPCDLL.XML_AddInt(ref strXML, "MachineID", Program.gMachineNumber);
            sbxpc.SBXPCDLL.XML_AddInt(ref strXML, "UserID", lUserID);

            sbxpc.SBXPCDLL.XML_AddBoolean(ref strXML, "Value", chkUserAttendOnly.Checked);

            bRet = sbxpc.SBXPCDLL.GeneralOperationXML(Program.gMachineNumber, ref strXML);

            if (bRet)
            {
                lblMessage.Text = "Success!";
            }
            else
            {
                sbxpc.SBXPCDLL.GetLastError(Program.gMachineNumber, out vErrorCode);
                lblMessage.Text = util.ErrorPrint(vErrorCode);
            }
            sbxpc.SBXPCDLL.EnableDevice(Program.gMachineNumber, 1);
        }

        private void btnGetUserMessage_Click(object sender, EventArgs e)
        {
            Boolean bRet;
            int vErrorCode = 0;
            int lUserID;
            String strXML = "";

            lblMessage.Text = "Working...";
            Application.DoEvents();

            bRet = sbxpc.SBXPCDLL.EnableDevice(Program.gMachineNumber, 0);
            if (!bRet)
            {
                lblMessage.Text = util.gstrNoDevice;
                return;
            }

            lUserID = Convert.ToInt32(txtEnrollNumber.Text);

            sbxpc.SBXPCDLL.XML_AddString(ref strXML, "REQUEST", "GetUserMessage");
            sbxpc.SBXPCDLL.XML_AddString(ref strXML, "MSGTYPE", "request");
            sbxpc.SBXPCDLL.XML_AddInt(ref strXML, "MachineID", Program.gMachineNumber);
            sbxpc.SBXPCDLL.XML_AddInt(ref strXML, "UserID", lUserID);

            bRet = sbxpc.SBXPCDLL.GeneralOperationXML(Program.gMachineNumber, ref strXML);

            if (bRet)
            {
                string base64_name;
                if (!sbxpc.SBXPCDLL.XML_ParseString(ref strXML, "Message", out base64_name))
                {
                    lblMessage.Text = "Failed to parse 'Message' string.";
                }
                else
                {
                    if (base64_name != null)
                    {
                        try
                        {
                            byte[] name_binary = Convert.FromBase64String(base64_name);
                            int index = 0;
                            for (int i = 0; i < name_binary.Length - 1; i += 2)
                            {
                                if (name_binary[i] == 0 && name_binary[i + 1] == 0)
                                {
                                    index = i;
                                    break;
                                }
                            }

                            txtUserMessage.Text = Encoding.Unicode.GetString(name_binary, 0, index);
                        }
                        catch (Exception)
                        {
                        }
                    }
                    lblMessage.Text = "Success!";
                }
            }
            else
            {
                sbxpc.SBXPCDLL.GetLastError(Program.gMachineNumber, out vErrorCode);
                lblMessage.Text = util.ErrorPrint(vErrorCode);
            }
            sbxpc.SBXPCDLL.EnableDevice(Program.gMachineNumber, 1);
        }

        private void btnSetUserMessage_Click(object sender, EventArgs e)
        {
            Boolean bRet;
            int vErrorCode = 0;
            int lUserID;
            String strXML = "";

            lblMessage.Text = "Waiting...";
            Application.DoEvents();

            bRet = sbxpc.SBXPCDLL.EnableDevice(Program.gMachineNumber, 0);
            if (!bRet)
            {
                lblMessage.Text = util.gstrNoDevice;
                return;
            }

            lUserID = Convert.ToInt32(txtEnrollNumber.Text);

            sbxpc.SBXPCDLL.XML_AddString(ref strXML, "REQUEST", "SetUserMessage");
            sbxpc.SBXPCDLL.XML_AddString(ref strXML, "MSGTYPE", "request");
            sbxpc.SBXPCDLL.XML_AddInt(ref strXML, "MachineID", Program.gMachineNumber);
            sbxpc.SBXPCDLL.XML_AddInt(ref strXML, "UserID", lUserID);

            {
                byte[] name_binary = Encoding.Unicode.GetBytes(txtUserMessage.Text);
                sbxpc.SBXPCDLL.XML_AddString(ref strXML, "Message", Convert.ToBase64String(name_binary));
            }

            bRet = sbxpc.SBXPCDLL.GeneralOperationXML(Program.gMachineNumber, ref strXML);

            if (bRet)
            {
                lblMessage.Text = "Success!";
            }
            else
            {
                sbxpc.SBXPCDLL.GetLastError(Program.gMachineNumber, out vErrorCode);
                lblMessage.Text = util.ErrorPrint(vErrorCode);
            }
            sbxpc.SBXPCDLL.EnableDevice(Program.gMachineNumber, 1);
        }

        private uint validate_msg_color(TextBox tb, uint defColor)
        {
            uint color;
            try
            {
                color = Convert.ToUInt32(tb.Text, 16);
            }
            catch
            {
                color = defColor;
            }
            color &= 0xFFFFFF;
            tb.Text = color.ToString("X6");
            return color;
        }

        const uint MESSAGE_DEF_COLOR = 0x000000;
        const uint MESSAGE_DEF_BK_COLOR = 0xFFFFFF;

        private void btnMessageColorPiker_Click(object sender, EventArgs e)
        {
            using (ColorDialog colorDialog = new ColorDialog())
            {
                colorDialog.Color = Color.FromArgb((int)(validate_msg_color(txtMessageColor, MESSAGE_DEF_COLOR) + 0xFF000000));
                if (colorDialog.ShowDialog() == DialogResult.OK)
                {
                    txtMessageColor.Text = (colorDialog.Color.ToArgb() & 0xFFFFFF).ToString("X6");
                }
            }
        }

        private void btnMessageBkColorPiker_Click(object sender, EventArgs e)
        {
            using (ColorDialog colorDialog = new ColorDialog())
            {
                colorDialog.Color = Color.FromArgb((int)(validate_msg_color(txtMessageBkColor, MESSAGE_DEF_BK_COLOR) + 0xFF000000));
                if (colorDialog.ShowDialog() == DialogResult.OK)
                {
                    txtMessageBkColor.Text = (colorDialog.Color.ToArgb() & 0xFFFFFF).ToString("X6");
                }
            }
        }

        private void btnGetMessageColor_Click(object sender, EventArgs e)
        {
            string strXML = null;
            string strValue = "";

            sbxpc.SBXPCDLL.XML_AddString(ref strXML, "REQUEST", "GetUserMessageColor");
            sbxpc.SBXPCDLL.XML_AddString(ref strXML, "MSGTYPE", "request");
            sbxpc.SBXPCDLL.XML_AddLong(ref strXML, "MachineID", Program.gMachineNumber);

            if (sbxpc.SBXPCDLL.GeneralOperationXML(Program.gMachineNumber, ref strXML))
            {
                sbxpc.SBXPCDLL.XML_ParseString(ref strXML, "MessageColor", out strValue);
                txtMessageColor.Text = (Convert.ToUInt32(strValue, 16) & 0xFFFFFF).ToString("X6");

                sbxpc.SBXPCDLL.XML_ParseString(ref strXML, "MessageBkColor", out strValue);
                txtMessageBkColor.Text = (Convert.ToUInt32(strValue, 16) & 0xFFFFFF).ToString("X6");

                MessageBox.Show("GetUserMessageColor OK!");
            }
            else
            {
                MessageBox.Show("GetUserMessageColor Failed.");
            }
        }

        private void btnSetMessageColor_Click(object sender, EventArgs e)
        {
            string strXML = null;
            sbxpc.SBXPCDLL.XML_AddString(ref strXML, "REQUEST", "SetUserMessageColor");
            sbxpc.SBXPCDLL.XML_AddString(ref strXML, "MSGTYPE", "request");
            sbxpc.SBXPCDLL.XML_AddLong(ref strXML, "MachineID", Program.gMachineNumber);

            uint color = validate_msg_color(txtMessageColor, MESSAGE_DEF_COLOR);
            uint bk_color = validate_msg_color(txtMessageBkColor, MESSAGE_DEF_BK_COLOR);

            sbxpc.SBXPCDLL.XML_AddString(ref strXML, "MessageColor", "FF" + color.ToString("X6"));
            sbxpc.SBXPCDLL.XML_AddString(ref strXML, "MessageBkColor", "FF" + bk_color.ToString("X6"));

            if (sbxpc.SBXPCDLL.GeneralOperationXML(Program.gMachineNumber, ref strXML))
            {
                MessageBox.Show("SetUserMessageColor OK!");
            }
            else
            {
                MessageBox.Show("SetUserMessageColor Failed.");
            }
        }
    }
}