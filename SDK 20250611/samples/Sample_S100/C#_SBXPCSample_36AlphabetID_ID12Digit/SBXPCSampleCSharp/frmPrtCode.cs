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
    public partial class frmPrtCode : Form
    {
        public frmPrtCode()
        {
            InitializeComponent();
        }

        AxSBXPCLib.AxSBXPC bpc;

        private void cmdGetSerialNumber_Click(object sender, EventArgs e)
        {
            string vSerialNumber = "";
            Boolean vRet;
            int vErrorCode = 0;

            txtSerialNo.Text = "";
            lblMessage.Text = "Waiting...";
            Application.DoEvents();

            vRet = bpc.EnableDevice(Program.gMachineNumber, 0); // 0 : false
            if (!vRet)
            {
                lblMessage.Text = util.gstrNoDevice;
                return;
            }

            vRet = bpc.GetSerialNumber(Program.gMachineNumber, ref vSerialNumber);
            if (vRet)
            {
                txtSerialNo.Text = vSerialNumber;
                lblMessage.Text = "Success";
            }
            else
            {
                bpc.GetLastError(ref vErrorCode);
                lblMessage.Text = util.ErrorPrint(vErrorCode);
            }

            bpc.EnableDevice(Program.gMachineNumber, 1); // 1 : true
        }

        private void cmdGetBackupNumber_Click(object sender, EventArgs e)
        {
            int vBackupNumber;
            int vErrorCode = 0;
            Boolean vRet;

            lblMessage.Text = "Waiting...";
            txtBackupNo.Text = "";
            Application.DoEvents();

            vRet = bpc.EnableDevice(Program.gMachineNumber, 0); // 0 : false
            if (!vRet)
            {
                lblMessage.Text = util.gstrNoDevice;
                return;
            }

            vBackupNumber = bpc.GetBackupNumber(Program.gMachineNumber);
            if (vBackupNumber != 0)
            {
                txtBackupNo.Text = Convert.ToString(vBackupNumber);
                lblMessage.Text = "Success";
            }
            else
            {
                bpc.GetLastError(ref vErrorCode);
                lblMessage.Text = util.ErrorPrint(vErrorCode);
            }

            bpc.EnableDevice(Program.gMachineNumber, 1); // 1 : true
        }

        private void cmdGetProductCode_Click(object sender, EventArgs e)
        {
            string vProductCode = "";
            Boolean vRet;
            int vErrorCode = 0;

            txtProductCode.Text = "";
            lblMessage.Text = "Waiting...";
            Application.DoEvents();

            vRet = bpc.EnableDevice(Program.gMachineNumber, 0); // 0 : false
            if (!vRet)
            {
                lblMessage.Text = util.gstrNoDevice;
                return;
            }

            vRet = bpc.GetProductCode(Program.gMachineNumber, ref vProductCode);
            if (vRet)
            {
                txtProductCode.Text = vProductCode;
                lblMessage.Text = "Success";
            }
            else
            {
                bpc.GetLastError(ref vErrorCode);
                lblMessage.Text = util.ErrorPrint(vErrorCode);
            }

            bpc.EnableDevice(Program.gMachineNumber, 1); // 1 : true
        }

        private void cmdExit_Click(object sender, EventArgs e)
        {
            Close();
            Application.OpenForms["frmMain"].Visible = true;
        }

        private void frmPrtCode_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.OpenForms["frmMain"].Visible = true;
        }

        private void frmPrtCode_Load(object sender, EventArgs e)
        {
            bpc = (AxSBXPCLib.AxSBXPC)Application.OpenForms["frmMain"].Controls["SBXPC1"];
        }
    }
}
