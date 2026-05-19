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

        private void btnGetUserAttendOnly_Click(object sender, EventArgs e)
        {
            Boolean bRet;
            int vErrorCode = 0;
            String strXML = "";

            lblMessage.Text = "Working...";
            Application.DoEvents();

            bRet = sbxpc.SBXPCDLL.EnableDevice(Program.gMachineNumber, 0);
            if (!bRet)
            {
                lblMessage.Text = util.gstrNoDevice;
                return;
            }

            string strUserID = txtEnrollNumber.Text;

            sbxpc.SBXPCDLL.XML_AddString(ref strXML, "REQUEST", "GetUserAttendOnly");
            sbxpc.SBXPCDLL.XML_AddString(ref strXML, "MSGTYPE", "request");
            sbxpc.SBXPCDLL.XML_AddInt(ref strXML, "MachineID", Program.gMachineNumber);
            sbxpc.SBXPCDLL.XML_AddString(ref strXML, "UserID", strUserID);

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
            String strXML = "";

            lblMessage.Text = "Waiting...";
            Application.DoEvents();

            bRet = sbxpc.SBXPCDLL.EnableDevice(Program.gMachineNumber, 0);
            if (!bRet)
            {
                lblMessage.Text = util.gstrNoDevice;
                return;
            }

            string strUserID = txtEnrollNumber.Text;

            sbxpc.SBXPCDLL.XML_AddString(ref strXML, "REQUEST", "SetUserAttendOnly");
            sbxpc.SBXPCDLL.XML_AddString(ref strXML, "MSGTYPE", "request");
            sbxpc.SBXPCDLL.XML_AddInt(ref strXML, "MachineID", Program.gMachineNumber);
            sbxpc.SBXPCDLL.XML_AddString(ref strXML, "UserID", strUserID);

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
    }
}