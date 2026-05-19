using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.Runtime.InteropServices;

namespace SBXPCSampleCSharp
{
    public partial class frmScreenSaver : Form
    {
        public frmScreenSaver()
        {
            InitializeComponent();
        }

        AxSBXPCLib.AxSBXPC bpc;
        const int SLEEP_MSG_LEN = 128 * 2; //Sleep Message can be 128 characters of UNICODE
        const int COMPANY_NAME_LEN = 64 * 2; //Company Name can be 64 characters of UNICODE
        const int CUSTOMER_NAME_LEN = 64 * 2; //Customer Name can be 64 characters of UNICODE

        private void cmdGetCustomerInfo_Click(object sender, EventArgs e)
        {
            bool bRet;
            int vErrorCode = 0;
            string strXML  = "";
            string strCompanyName = "", strCustomerName = "";

            lblMessage.Text = "Working...";
            Application.DoEvents();

            bRet = bpc.EnableDevice(Program.gMachineNumber, 0);

            if(!bRet)
            {
                lblMessage.Text = util.gstrNoDevice;
                return;                 
            }

            util.MakeXMLRequestHeader(bpc, ref strXML, "GetCustomerInfo");

            bRet = bpc.GeneralOperationXML(ref strXML);

            if(!bRet)
            {
                bpc.GetLastError(ref vErrorCode);
                lblMessage.Text = util.ErrorPrint(vErrorCode);
                goto _lexit;                
            }

            lblMessage.Text = "GetCustomerInfo OK";

            bpc.XML_ParseBinaryUnicode(ref strXML, "CompanyName", ref strCompanyName, COMPANY_NAME_LEN);
            bpc.XML_ParseBinaryUnicode(ref strXML, "CustomerName", ref strCustomerName, CUSTOMER_NAME_LEN);

            txtCompanyName.Text = strCompanyName;
            txtCustomerName.Text = strCustomerName;

        _lexit:
            bRet = bpc.EnableDevice(Program.gMachineNumber, 1);
        }

        private void frmScreenSaver_Load(object sender, EventArgs e)
        {
            bpc = (AxSBXPCLib.AxSBXPC)Application.OpenForms["frmMain"].Controls["SBXPC1"];
        }

        private void cmdSetCustomerInfo_Click(object sender, EventArgs e)
        {
            bool bRet;
            int vErrorCode = 0;
            string strXML = "", strCompanyName = "", strCustomerName = "";

            lblMessage.Text = "Working...";
            Application.DoEvents();

            bRet = bpc.EnableDevice(Program.gMachineNumber, 0);

            if(!bRet)
            {
                lblMessage.Text = util.gstrNoDevice;
                return;
            }

            strCompanyName = txtCompanyName.Text;
            strCustomerName = txtCustomerName.Text;

            util.MakeXMLRequestHeader(bpc, ref strXML, "SetCustomerInfo");
            bpc.XML_AddBinaryUnicode(ref strXML, "CompanyName", ref strCompanyName);
            bpc.XML_AddBinaryUnicode(ref strXML, "CustomerName", ref strCustomerName);

            bRet = bpc.GeneralOperationXML(ref strXML);

            if (bRet)
                lblMessage.Text = "SetCustomerInfo OK";
            else
            {
                bpc.GetLastError(ref vErrorCode);
                lblMessage.Text = util.ErrorPrint(vErrorCode);
            }

            bRet = bpc.EnableDevice(Program.gMachineNumber, 1);
        }

        private void frmScreenSaver_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.OpenForms["frmMain"].Visible = true;
        }

        private void cmdExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void cmdGetGlyphSize_Click(object sender, EventArgs e)
        {
            bool bRet;
            int vErrorCode = 0;
            string strXML = "";
            int glyphWidth, glyphHeight;

            lblMessage.Text = "Working...";
            Application.DoEvents();

            bRet = bpc.EnableDevice(Program.gMachineNumber, 0);

            if(!bRet)
            {
                lblMessage.Text = util.gstrNoDevice;
                return;
            }

            util.MakeXMLRequestHeader(bpc, ref strXML, "GetSleepMsgGlyphSize");

            bRet = bpc.GeneralOperationXML(ref strXML);

            if(bRet)
            {
                lblMessage.Text = "GetSleepMsgGlyphSize OK"; 
                glyphWidth = bpc.XML_ParseInt(ref strXML, "Width");
                glyphHeight = bpc.XML_ParseInt(ref strXML, "Height");
                txtGlyphWidth.Text = Convert.ToString(glyphWidth);
                txtGlyphHeight.Text = Convert.ToString(glyphHeight);
            }
            else
            {
                bpc.GetLastError(ref vErrorCode);
                lblMessage.Text = util.ErrorPrint(vErrorCode);
            }

            bRet = bpc.EnableDevice(Program.gMachineNumber, 1);
        }

        private void cmdGetSleepMsg_Click(object sender, EventArgs e)
        {
            bool bRet;
            int vErrorCode = 0;
            string strXML = "";

            lblMessage.Text = "Working...";
            Application.DoEvents();

            bRet = bpc.EnableDevice(Program.gMachineNumber, 0);
            
            if(!bRet)
            {
                lblMessage.Text = util.gstrNoDevice;
                return;
            }

            util.MakeXMLRequestHeader(bpc, ref strXML, "GetSleepMessage");

            bRet = bpc.GeneralOperationXML(ref strXML);

            if(bRet)
            {
                lblMessage.Text = "GetSleepMessage OK";
                string strSleepMessage = "";
                bpc.XML_ParseBinaryUnicode(ref strXML, "SleepMessage", ref strSleepMessage, SLEEP_MSG_LEN);
                txtSleepMessage.Text = strSleepMessage;
            }
            else
            {
                bpc.GetLastError(ref vErrorCode);
                lblMessage.Text = util.ErrorPrint(vErrorCode);
            }

            bRet = bpc.EnableDevice(Program.gMachineNumber, 1);
        }

        private void cmdSetSleepMsg_Click(object sender, EventArgs e)
        {
            bool bRet;
            int vErrorCode = 0;
            string strXML = "", strFontXML = "", strSleepMsg;
            int glyphWidth, glyphHeight;
            int fontHeight, fontWidth, fontWeight;


            lblMessage.Text = "Working...";
            Application.DoEvents();

            bRet = bpc.EnableDevice(Program.gMachineNumber, 0);

            if(!bRet)
            {
                lblMessage.Text = util.gstrNoDevice;
                return;
            }

            util.MakeXMLRequestHeader(bpc, ref strXML, "GetSleepMsgGlyphSize");

            bRet = bpc.GeneralOperationXML(ref strXML);

            if(!bRet)
            {
                bpc.GetLastError(ref vErrorCode);
                lblMessage.Text = util.ErrorPrint(vErrorCode);
                goto _lexit;
            }

            glyphWidth = bpc.XML_ParseInt(ref strXML, "Width");
            glyphHeight = bpc.XML_ParseInt(ref strXML, "Height");

            fontHeight = Convert.ToInt32(txtFontHeight.Text == "" ? "0" : txtFontHeight.Text);
            fontWidth = Convert.ToInt32(txtFontWidth.Text == "" ? "0" : txtFontWidth.Text);
            fontWeight = Convert.ToInt32(txtFontWeight.Text == "" ? "0" : txtFontWeight.Text);
            strSleepMsg = txtSleepMessage.Text;

            bpc.XML_AddString(ref strFontXML, "FaceName", "Arial");
            bpc.XML_AddInt(ref strFontXML, "Height", fontHeight);
            bpc.XML_AddInt(ref strFontXML, "Width", fontWidth);
            bpc.XML_AddInt(ref strFontXML, "Weight", fontWeight);
            bpc.XML_AddBoolean(ref strFontXML, "Italic", chkItalic.Checked);
            bpc.XML_AddBoolean(ref strFontXML, "Underline", chkUnderline.Checked);
            bpc.XML_AddBoolean(ref strFontXML, "StrikeOut", chkStrikeOut.Checked);

            if(chkDebugOut.Checked && txtDebugOutFile.Text != "")
            {
                bpc.XML_AddString(ref strFontXML, "DebugOut", txtDebugOutFile.Text);
            }

            strXML = "";
            util.MakeXMLRequestHeader(bpc, ref strXML, "SetSleepMessage");
            bpc.XML_AddBinaryGlyph(ref strXML, strSleepMsg, glyphWidth, glyphHeight, strFontXML);

            bRet = bpc.GeneralOperationXML(ref strXML);

            if (bRet)
                lblMessage.Text = "SetSleepMessage OK";
            else
            {
                bpc.GetLastError(ref vErrorCode);
                lblMessage.Text = util.ErrorPrint(vErrorCode);
            }

        _lexit:
            bRet = bpc.EnableDevice(Program.gMachineNumber, 1);
        }

        private void cmdDebugOutFileBrowse_Click(object sender, EventArgs e)
        {
            OpenSaveDlg.ShowDialog();
            txtDebugOutFile.Text = OpenSaveDlg.FileName;
        }

    }
}
