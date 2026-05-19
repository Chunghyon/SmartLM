using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace SBXPCDLLSampleCSharp
{
    public partial class frmUnlockGroup : Form
    {
        public frmUnlockGroup()
        {
            InitializeComponent();
        }

        

        const int UNLOCKGROUP_COUNT = 10;
        const int UNLOCKGROUP_INFO_BYTE = 5;
        const int ALLBYTE_COUNT = UNLOCKGROUP_COUNT * UNLOCKGROUP_INFO_BYTE;
        int[] unlockGroupInfo;

        private void frmUnlockGroup_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.OpenForms["frmMain"].Visible = true;
            unlockGroupInfo = new int[ALLBYTE_COUNT];
        }

        private void cmdExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void frmUnlockGroup_Load(object sender, EventArgs e)
        {
            unlockGroupInfo = new int[ALLBYTE_COUNT];
            UnlockGroupInit();
            DrawUnlockGroupList();
        }

        private void UnlockGroupInit()
        {
            for(int i = 0; i < ALLBYTE_COUNT; i ++)
                unlockGroupInfo[i] = 0;
        }

        private void DrawUnlockGroupList()
        {
            string itemString;
            lstUnlockGroup.Items.Clear();
            for(int i = 0; i < UNLOCKGROUP_COUNT; i ++)
            {
                itemString = "[Unlock Group.]" + String.Format("{0:D2}: - ", i);
                for( int j = 0; j < UNLOCKGROUP_INFO_BYTE; j ++)
                    itemString += unlockGroupInfo[i * UNLOCKGROUP_INFO_BYTE + j] + ":";
                lstUnlockGroup.Items.Add(itemString);
            }
        }

        private void cmdUpdate_Click(object sender, EventArgs e)
        {
            if (lstUnlockGroup.SelectedIndex == -1)
                return;
            int index = lstUnlockGroup.SelectedIndex * UNLOCKGROUP_INFO_BYTE;
            unlockGroupInfo[index] = chkGroup1.Checked ? 1 : 0;
            unlockGroupInfo[index + 1] = chkGroup2.Checked ? 1 : 0;
            unlockGroupInfo[index + 2] = chkGroup3.Checked ? 1 : 0;
            unlockGroupInfo[index + 3] = chkGroup4.Checked ? 1 : 0;
            unlockGroupInfo[index + 4] = chkGroup5.Checked ? 1 : 0;
        }

        private void cmdRead_Click(object sender, EventArgs e)
        {
            bool bRet;
            int vErrorCode = 0;
            lblMessage.Text = "Working...";
            Application.DoEvents();

            bRet = sbxpc.SBXPCDLL.EnableDevice(Program.gMachineNumber, 0); // 0 : disable

            if (!bRet)
            {
                lblMessage.Text = util.gstrNoDevice;
                return;
            }

            GCHandle gh = GCHandle.Alloc(unlockGroupInfo, GCHandleType.Pinned);
            IntPtr AddrOfunlockGroupInfo = gh.AddrOfPinnedObject();
            bRet = sbxpc.SBXPCDLL.GetDeviceLongInfo(Program.gMachineNumber, 7, AddrOfunlockGroupInfo);

            if (bRet)
            {
                lblMessage.Text = "Success!";
            }
            else
            {
                sbxpc.SBXPCDLL.GetLastError(Program.gMachineNumber, out vErrorCode);
                lblMessage.Text = util.ErrorPrint(vErrorCode);
            }

            bRet = sbxpc.SBXPCDLL.EnableDevice(Program.gMachineNumber, 1); // 1 : enable
        }

        private void cmdWrite_Click(object sender, EventArgs e)
        {
            bool bRet;
            int vErrorCode = 0;
            lblMessage.Text = "Working...";
            Application.DoEvents();

            bRet = sbxpc.SBXPCDLL.EnableDevice(Program.gMachineNumber, 0); // 0 : disable

            if (!bRet)
            {
                lblMessage.Text = util.gstrNoDevice;
                return;
            }

            GCHandle gh = GCHandle.Alloc(unlockGroupInfo, GCHandleType.Pinned);
            IntPtr AddrOfunlockGroupInfo = gh.AddrOfPinnedObject();
            bRet = sbxpc.SBXPCDLL.SetDeviceLongInfo(Program.gMachineNumber, 7, AddrOfunlockGroupInfo);

            if (bRet)
            {
                lblMessage.Text = "Success!";
            }
            else
            {
                sbxpc.SBXPCDLL.GetLastError(Program.gMachineNumber, out vErrorCode);
                lblMessage.Text = util.ErrorPrint(vErrorCode);
            }

            bRet = sbxpc.SBXPCDLL.EnableDevice(Program.gMachineNumber, 1); // 1 : enable
        }

        private void lstUnlockGroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstUnlockGroup.SelectedIndex == -1)
                return;
            int index = lstUnlockGroup.SelectedIndex * UNLOCKGROUP_INFO_BYTE;
            chkGroup1.Checked = unlockGroupInfo[index] > 0 ? true : false;
            chkGroup2.Checked = unlockGroupInfo[index + 1] > 0 ? true : false;
            chkGroup3.Checked = unlockGroupInfo[index + 2] > 0 ? true : false;
            chkGroup4.Checked = unlockGroupInfo[index + 3] > 0 ? true : false;
            chkGroup5.Checked = unlockGroupInfo[index + 4] > 0 ? true : false;
        }
    }
}
