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
    public partial class frmHoliday : Form
    {
        public frmHoliday()
        {
            InitializeComponent();
        }

        

        const int DBHOLIDAYS_MAX = 256;
        const int HOLIDAY_BYTES = 3;
        const int ALLBYTE_COUNT = DBHOLIDAYS_MAX * HOLIDAY_BYTES;
        int[] holidayInfo;

        private void frmHoliday_Load(object sender, EventArgs e)
        {
            holidayInfo = new int[ALLBYTE_COUNT];
            HolidayInit();
            DrawHolidays();
        }

        private void HolidayInit()
        {
            for(int i = 0; i < DBHOLIDAYS_MAX; i ++)
            {
                holidayInfo[i * HOLIDAY_BYTES] = 1;
                holidayInfo[i * HOLIDAY_BYTES + 1] = 1;
                holidayInfo[i * HOLIDAY_BYTES + 2] = 0;
            }
        }

        private void DrawHolidays()
        {
            lstHoliday.Items.Clear();
            string itemString;
            for(int i = 0; i < DBHOLIDAYS_MAX; i ++)
            {
                itemString = "[No.] " + String.Format("{0:D2}", i) + " ";
                itemString += "[Day/Month] ";
                itemString += String.Format("{0:D2}", holidayInfo[i * HOLIDAY_BYTES + 1]) + "/";
                itemString += String.Format("{0:D2}", holidayInfo[i * HOLIDAY_BYTES]) + " ";
                itemString += "[Period] ";
                itemString += String.Format("{0:D2}", holidayInfo[i * HOLIDAY_BYTES + 2]);

                lstHoliday.Items.Add(itemString);
            }
        }

        private void cmdUpdate_Click(object sender, EventArgs e)
        {
            if (lstHoliday.SelectedIndex == -1)
                return;
            int index = lstHoliday.SelectedIndex * HOLIDAY_BYTES;

            holidayInfo[index] = dtHoliday.Value.Month;
            holidayInfo[index + 1] = dtHoliday.Value.Day;
            holidayInfo[index + 2] = Convert.ToInt32(txtDays.Text);

            DrawHolidays();
        }

        private void lstHoliday_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstHoliday.SelectedIndex == -1)
                return;
            int index = lstHoliday.SelectedIndex * HOLIDAY_BYTES;

            if (holidayInfo[index] > 0 && holidayInfo[index] <= 12 &&
                holidayInfo[index + 1] > 0 && holidayInfo[index + 1] <= 31)
            {
                dtHoliday.Value = new DateTime(2000, holidayInfo[index], holidayInfo[index + 1]);
            }

            txtDays.Text = Convert.ToString(holidayInfo[index + 2]);
        }

        private void cmdExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void frmHoliday_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.OpenForms["frmMain"].Visible = true;
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

            GCHandle gh = GCHandle.Alloc(holidayInfo, GCHandleType.Pinned);
            IntPtr AddrOfholidayInfo = gh.AddrOfPinnedObject();
            bRet = sbxpc.SBXPCDLL.GetDeviceLongInfo(Program.gMachineNumber, 6, AddrOfholidayInfo);

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

            DrawHolidays();
        }

        private void cmdWrite_Click(object sender, EventArgs e)
        {
            bool bRet;
            int vErrorCode = 0;
            lblMessage.Text = "Working...";
            Application.DoEvents();

            bRet = sbxpc.SBXPCDLL.EnableDevice(Program.gMachineNumber, 0);

            if (!bRet)
            {
                lblMessage.Text = util.gstrNoDevice;
                return;
            }

            GCHandle gh = GCHandle.Alloc(holidayInfo, GCHandleType.Pinned);
            IntPtr AddrOfholidayInfo = gh.AddrOfPinnedObject();
            bRet = sbxpc.SBXPCDLL.SetDeviceLongInfo(Program.gMachineNumber, 6, AddrOfholidayInfo);

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
    }
}
