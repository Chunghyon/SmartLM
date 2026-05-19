using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SBXPCSampleCSharp
{
    public partial class frmGroup : Form
    {
        AxSBXPCLib.AxSBXPC bpc;

        public frmGroup()
        {
            InitializeComponent();
        }

        private void cmdRead_Click(object sender, EventArgs e)
        {
            int vGroupNumber;
            Boolean vRet;
            int vErrorCode = 0;
            String vName = "", strXML = "";

            lblMessage.Text = "Working...";
            Application.DoEvents();

            vRet = bpc.EnableDevice(Program.gMachineNumber, 0);
            if (!vRet)
            {
                lblMessage.Text = util.gstrNoDevice;
                return;
            }

            vGroupNumber = Convert.ToInt32(txtGroupNumber.Text);

            util.MakeXMLRequestHeader(bpc, ref strXML, "GetGroupName");

            bpc.XML_AddInt(ref strXML, "GroupNo", vGroupNumber);
    
            vRet = bpc.GeneralOperationXML(ref strXML);
    
            if (vRet)
            {
                vRet = bpc.XML_ParseMultiUnicode(ref strXML, "GroupName", ref vName, 5 * 2);
                if (vRet)
                {
                    txtGroupName.Text = vName;
                    lblMessage.Text = "GetGroupName OK";
                }
                else
                    lblMessage.Text = "GetGroupName - XML Parse Error!";
            }
            else
            {
                bpc.GetLastError(ref vErrorCode);
                lblMessage.Text = util.ErrorPrint(vErrorCode);
            }
            
            bpc.EnableDevice(Program.gMachineNumber, 1);
        }

        private void cmdWrite_Click(object sender, EventArgs e)
        {
            int vGroupNumber;
            Boolean vRet;
            int vErrorCode = 0;
            String strXML = "";
            String vName= "";
            
            lblMessage.Text = "Working...";
            Application.DoEvents();
            
            vRet = bpc.EnableDevice(Program.gMachineNumber, 0);
            if (!vRet)
            {
                lblMessage.Text = util.gstrNoDevice;
                return;
            }
            
            vGroupNumber = Convert.ToInt32(txtGroupNumber.Text);
            vName = txtGroupName.Text;

            util.MakeXMLRequestHeader(bpc, ref strXML, "SetGroupName");

            bpc.XML_AddInt(ref strXML, "GroupNo", vGroupNumber);
            bpc.XML_AddBinaryNameGlyph(Program.gMachineNumber, ref strXML, ref vName);
            
            vRet = bpc.GeneralOperationXML(ref strXML);
            if (vRet)
                lblMessage.Text = "SetGroupName OK";
            else
            {
                bpc.GetLastError(ref vErrorCode);
                lblMessage.Text = util.ErrorPrint(vErrorCode);
            }

            bpc.EnableDevice(Program.gMachineNumber, 1);      
        }

        private void frmGroup_Load(object sender, EventArgs e)
        {
            bpc = (AxSBXPCLib.AxSBXPC)Application.OpenForms["frmMain"].Controls["SBXPC1"];
        }

        private void frmGroup_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.OpenForms["frmMain"].Visible = true;
        }

        private void cmdExit_Click(object sender, EventArgs e)
        {
            Close();
        }

    }
}
