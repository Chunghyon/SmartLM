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
    public partial class frmTMode : Form
    {
        public frmTMode()
        {
            InitializeComponent();
        }

        AxSBXPCLib.AxSBXPC bpc;
        const int DB_TMODE_COUNT = 10;
        const int DB_TMODE_BYTE_COUNT = 5;
        int[] tModeInfo;

        private void frmTMode_Load(object sender, EventArgs e)
        {
            bpc = (AxSBXPCLib.AxSBXPC)Application.OpenForms["frmMain"].Controls["SBXPC1"];
            tModeInfo = new int[DB_TMODE_COUNT * DB_TMODE_BYTE_COUNT];
            InitTrMode();
            DrawTModeList();
            dtStart.ShowUpDown = true;
            dtEnd.ShowUpDown = true;
        }

        private void InitTrMode()
        {
            for (int i = 0; i < DB_TMODE_COUNT; i++)
            {
                tModeInfo[i * DB_TMODE_BYTE_COUNT] = 0;
                tModeInfo[i * DB_TMODE_BYTE_COUNT + 1] = 0;
                tModeInfo[i * DB_TMODE_BYTE_COUNT + 2] = 0;
                tModeInfo[i * DB_TMODE_BYTE_COUNT + 3] = 0;
                tModeInfo[i * DB_TMODE_BYTE_COUNT + 4] = 0;
            }
        }

        private void DrawTModeList()
        {
            string itemString;
            lstTMode.Items.Clear();
            for (int i = 0; i < DB_TMODE_COUNT; i++)
            {
                itemString = "[No.]" + String.Format("{0:D2}", i) + " ";
                itemString += "[S]";
                itemString += String.Format("{0:D2}", tModeInfo[i * DB_TMODE_BYTE_COUNT + 1]) + ":";
                itemString += String.Format("{0:D2}", tModeInfo[i * DB_TMODE_BYTE_COUNT + 2]) + " ";
                itemString += "[E]";
                itemString += String.Format("{0:D2}", tModeInfo[i * DB_TMODE_BYTE_COUNT + 3]) + ":";
                itemString += String.Format("{0:D2}", tModeInfo[i * DB_TMODE_BYTE_COUNT + 4]) + " ";
                itemString += cmbDoorMode.Items[tModeInfo[i * DB_TMODE_BYTE_COUNT]];
                lstTMode.Items.Add(itemString);
            }
        }

        private void frmTMode_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.OpenForms["frmMain"].Visible = true;
        }

        private void cmdExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void lstTMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstTMode.SelectedIndex == -1)
                return;

            int index = lstTMode.SelectedIndex * DB_TMODE_BYTE_COUNT;
            int startHour = tModeInfo[index + 1];
            int startMinute = tModeInfo[index + 2];
            int endHour = tModeInfo[index + 3];
            int endMinute = tModeInfo[index + 4];
            int tMode = tModeInfo[index];

            dtStart.Value = new DateTime(2000, 1, 1, startHour, startMinute, 0);
            dtEnd.Value = new DateTime(2000, 1, 1, endHour, endMinute, 0);
            cmbDoorMode.SelectedIndex = tMode;
        }

        private void cmdUpdate_Click(object sender, EventArgs e)
        {
            if (lstTMode.SelectedIndex == -1)
                return;

            int index = lstTMode.SelectedIndex * DB_TMODE_BYTE_COUNT;
            tModeInfo[index + 1] = dtStart.Value.Hour;
            tModeInfo[index + 2] = dtStart.Value.Minute;
            tModeInfo[index + 3] = dtEnd.Value.Hour;
            tModeInfo[index + 4] = dtEnd.Value.Minute;
            tModeInfo[index] = cmbDoorMode.SelectedIndex;

            DrawTModeList();
        }

        private void cmdRead_Click(object sender, EventArgs e)
        {
            bool bRet;
            int vErrorCode = 0;

            lblMessage.Text = "Working...";
            Application.DoEvents();

            bRet = bpc.EnableDevice(Program.gMachineNumber, 0); // 0 : disable

            if (!bRet)
            {
                lblMessage.Text = util.gstrNoDevice;
                return;
            }

            GCHandle gh = GCHandle.Alloc(tModeInfo, GCHandleType.Pinned);
            IntPtr AddrOftModeInfo = gh.AddrOfPinnedObject();
            int nAddr = AddrOftModeInfo.ToInt32();

            bRet = bpc.GetDeviceLongInfo(Program.gMachineNumber, 4, ref nAddr);

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

            DrawTModeList();
        }

        private void cmdWrite_Click(object sender, EventArgs e)
        {
            bool bRet;
            int vErrorCode = 0;

            lblMessage.Text = "Working...";
            Application.DoEvents();

            bRet = bpc.EnableDevice(Program.gMachineNumber, 0); // 0 : disable

            if (!bRet)
            {
                lblMessage.Text = util.gstrNoDevice;
                return;
            }

            GCHandle gh = GCHandle.Alloc(tModeInfo, GCHandleType.Pinned);
            IntPtr AddrOftModeInfo = gh.AddrOfPinnedObject();
            int nAddr = AddrOftModeInfo.ToInt32();

            bRet = bpc.SetDeviceLongInfo(Program.gMachineNumber, 4, ref nAddr);

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

            DrawTModeList();
        }
    }
}
