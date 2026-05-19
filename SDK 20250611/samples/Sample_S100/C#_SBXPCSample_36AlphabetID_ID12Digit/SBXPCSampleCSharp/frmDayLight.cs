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
    public partial class frmDayLight : Form
    {
        public frmDayLight()
        {
            InitializeComponent();
        }

        AxSBXPCLib.AxSBXPC bpc;

        private void frmDayLight_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.OpenForms["frmMain"].Visible = true;
        }

        private void cmdExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void cmdRead_Click(object sender, EventArgs e)
        {
            bool bRet;
            int vErrorCode = 0;
            int[] timeInfo = new int[3];

            lblMessage.Text = "Working...";
            Application.DoEvents();

            bRet = bpc.EnableDevice(Program.gMachineNumber, 0); // 0 : disable

            if (!bRet)
            {
                lblMessage.Text = util.gstrNoDevice;
                return;
            }

            GCHandle gh = GCHandle.Alloc(timeInfo, GCHandleType.Pinned);
            IntPtr AddrOftimeInfo = gh.AddrOfPinnedObject();
            int nAddr = AddrOftimeInfo.ToInt32();

            bRet = bpc.GetDeviceLongInfo(Program.gMachineNumber, 1, ref nAddr);

            if (bRet)
            {
                lblMessage.Text = "Success!";
            }
            else
            {
                bpc.GetLastError(ref vErrorCode);
                lblMessage.Text = util.ErrorPrint(vErrorCode);
            }

            bRet = bpc.EnableDevice(Program.gMachineNumber, 1); // 1 : enable
        }

        private void frmDayLight_Load(object sender, EventArgs e)
        {
            bpc = (AxSBXPCLib.AxSBXPC)Application.OpenForms["frmMain"].Controls["SBXPC1"];
        }

        private void cmdWrite_Click(object sender, EventArgs e)
        {
            bool bRet;
            int vErrorCode = 0;
            int[] timeInfo = new int[3];

            lblMessage.Text = "Working...";
            Application.DoEvents();

            bRet = bpc.EnableDevice(Program.gMachineNumber, 0); // 0 : disable

            if (!bRet)
            {
                lblMessage.Text = util.gstrNoDevice;
                return;
            }

            GCHandle gh = GCHandle.Alloc(timeInfo, GCHandleType.Pinned);
            IntPtr AddrOftimeInfo = gh.AddrOfPinnedObject();
            int nAddr = AddrOftimeInfo.ToInt32();

            bRet = bpc.SetDeviceLongInfo(Program.gMachineNumber, 1, ref nAddr);

            if (bRet)
            {
                lblMessage.Text = "Success!";
            }
            else
            {
                bpc.GetLastError(ref vErrorCode);
                lblMessage.Text = util.ErrorPrint(vErrorCode);
            }

            bRet = bpc.EnableDevice(Program.gMachineNumber, 1); // 1 : enable
        }
    }
}
