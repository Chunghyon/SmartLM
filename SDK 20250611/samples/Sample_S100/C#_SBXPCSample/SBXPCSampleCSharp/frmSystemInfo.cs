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
    public partial class frmSystemInfo : Form
    {
        public frmSystemInfo()
        {
            InitializeComponent();
        }
        AxSBXPCLib.AxSBXPC bpc;

        private string GetWeekDay(int anDay)
        {
            switch (anDay)
            {
                case 1:
                    return "Sunday";
                case 2:
                    return "Monday";
                case 3:
                    return "Tuesday";
                case 4:
                    return "Wednesday";
                case 5:
                    return "Thursday";
                case 6:
                    return "Friday";
                case 7:
                    return "Saturday";
                default:
                    return "Sunday";
            }
        }

        private void cmdGetDeviceTime_Click(object sender, EventArgs e)
        {
            int vYear = 0;
            int vMonth = 0;
            int vDay = 0;
            int vHour = 0;
            int vMinute = 0;
            int vSecond = 0;
            int vDayOfWeek = 0;
            string strDataTime;
            Boolean vRet;
            int vErrorCode = 0;

            lblMessage.Text = "Working...";
            Application.DoEvents();

            vRet = bpc.EnableDevice(Program.gMachineNumber, 0); // 0 : false
            if (!vRet)
            {
                lblMessage.Text = util.gstrNoDevice;
                return;
            }

            vRet = bpc.GetDeviceTime(Program.gMachineNumber, 
                                     ref vYear, 
                                     ref vMonth, 
                                     ref vDay, 
                                     ref vHour, 
                                     ref vMinute, 
                                     ref vSecond,
                                     ref vDayOfWeek);
            if (vRet)
            {
                if (vDayOfWeek == 0) vDayOfWeek = 7;
                strDataTime = "Date = " + Convert.ToString(vYear) + "/" + Convert.ToString(vMonth) + "/" + Convert.ToString(vDay) + " , " + GetWeekDay(vDayOfWeek) + " , Time = " + Convert.ToString(vHour) + ":" + Convert.ToString(vMinute) + ":" + Convert.ToString(vSecond);
                lblMessage.Text = strDataTime;
            }
            else
            {
                bpc.GetLastError(ref vErrorCode);
                lblMessage.Text = util.ErrorPrint(vErrorCode);
            }
            bpc.EnableDevice(Program.gMachineNumber, 1); // 1 : true
            Application.DoEvents();
        }

        private void cmdSetDeviceTime_Click(object sender, EventArgs e)
        {
            Boolean vRet;
            int vErrorCode = 0;

            lblMessage.Text = "Working...";
            Application.DoEvents();

            vRet = bpc.EnableDevice(Program.gMachineNumber, 0); // 0 : false
            if (!vRet)
            {
                lblMessage.Text = util.gstrNoDevice;
                return;
            }

            vRet = bpc.SetDeviceTime(Program.gMachineNumber);
            if (vRet)
            {
                lblMessage.Text = "Success!";
            }
            else
            {
                bpc.GetLastError(ref vErrorCode);
                lblMessage.Text = util.ErrorPrint(vErrorCode);
            }

            bpc.EnableDevice(Program.gMachineNumber, 1); // 1 : true
            Application.DoEvents();
        }

        private void cmdPowerOn_Click(object sender, EventArgs e)
        {
            bpc.PowerOnAllDevice();
        }

        private void PowerOffDevice_Click(object sender, EventArgs e)
        {
            Boolean vRet;
            int vErrorCode = 0;

            lblMessage.Text = "Working...";
            Application.DoEvents();

            vRet = bpc.PowerOffDevice(Program.gMachineNumber);
            if (vRet)
            {
                lblMessage.Text = "Success!";
            }
            else
            {
                bpc.GetLastError(ref vErrorCode);
                lblMessage.Text = util.ErrorPrint(vErrorCode);
            }
        }

        private void cmdEnableDevice_Click(object sender, EventArgs e)
        {
            int vFlag;
            Boolean vRet;
            int vErrorCode = 0;

            lblMessage.Text = "Working...";
            Application.DoEvents();

            vFlag = chkEnableDevice.Checked ? 1 : 0;

            vRet = bpc.EnableDevice(Program.gMachineNumber, vFlag);
            if (vRet)
            {
                if (vFlag == 1)
                {
                    lblMessage.Text = "Enable Device Success!";
                    cmdEnableDevice.Text = "DisableDevice";
                }
                else
                {
                    lblMessage.Text = "Disable Device Success!";
                    cmdEnableDevice.Text = "EnableDevice";
                }
            }
            else
            {
                bpc.GetLastError(ref vErrorCode);
                lblMessage.Text = util.ErrorPrint(vErrorCode);
                return;
            }

            chkEnableDevice.Checked = !chkEnableDevice.Checked;
            Application.DoEvents();
        }

        private void cmdExit_Click(object sender, EventArgs e)
        {
            Close();
            Application.OpenForms["frmMain"].Visible = true;
        }

        private void cmdGetDeviceInfo_Click(object sender, EventArgs e)
        {
            int vInfo;
            int vValue = 0;
            Boolean vRet;
            int vErrorCode = 0;

            lblMessage.Text = "Working...";
            Application.DoEvents();

            vInfo = cmbSatus.SelectedIndex + 1;

            vRet = bpc.EnableDevice(Program.gMachineNumber, 0); // 0 : false
            if (!vRet)
            {
                lblMessage.Text = util.gstrNoDevice;
                return;
            }

            vRet = bpc.GetDeviceInfo(Program.gMachineNumber, vInfo, ref vValue);
            if (vRet)
            {
                switch (vInfo)
                {
                    case 1:
                        lblMessage.Text = "(1) = ManagerCount = " + vValue;
                        break;
                    case 2:
                        lblMessage.Text = "(2) = Device ID = " + vValue;
                        break;
                    case 3:
                        lblMessage.Text = "(3) = Language = " + vValue;
                        break;
                    case 4:
                        lblMessage.Text = "(4) = PowerOffTime = " + vValue;
                        break;
                    case 5:
                        lblMessage.Text = "(5) = Lock release time = " + vValue;
                        break;
                    case 6:
                        lblMessage.Text = "(6) = GLogWarning = " + vValue;
                        break;
                    case 7:
                        lblMessage.Text = "(7) = SLogWarning = " + vValue;
                        break;
                    case 8:
                        lblMessage.Text = "(8) = ReVerifyTime = " + vValue;
                        break;
                    case 9:
                        lblMessage.Text = "(9) = Baudrate = " + vValue;
                        break;
                    case 10:
                        lblMessage.Text = "(10) = Parity check = " + vValue;
                        break;
                    case 11:
                        lblMessage.Text = "(11) = Stop bit = " + vValue;
                        break;
                    case 12:
                        lblMessage.Text = "(12) = Date Seperator = " + vValue;
                        break;
                    case 13:
                        lblMessage.Text = "(13) = Identificatin mode = " + vValue;
                        break;
                    case 14:
                        lblMessage.Text = "(14) = LockOperate = " + vValue;
                        break;
                    case 15:
                        lblMessage.Text = "(15) = Door sensor type = " + vValue;
                        break;
                    case 16:
                        lblMessage.Text = "(16) = Door open time limit = " + vValue;
                        break;
                    case 17:
                        lblMessage.Text = "(17) = Anti-pass = " + vValue;
                        break;
                    case 18:
                        lblMessage.Text = "(18) = Auto sleep time = " + vValue;
                        break;
                    case 19:
                        lblMessage.Text = "(19) = Daylight offset = " + vValue;
                        break;
                    case 20:
                        lblMessage.Text = "(20) = UDP Server = " + util.pubLongToIPAddr(vValue);
                        break;
                    case 21:
                        lblMessage.Text = "(21) = DHCP Use = " + vValue;
                        break;
                    case 22:
                        lblMessage.Text = "(22) = Main Lock Group = " + vValue;
                        break;
                }
            }
            else
            {
                bpc.GetLastError(ref vErrorCode);
                lblMessage.Text = util.ErrorPrint(vErrorCode);
            }

            bpc.EnableDevice(Program.gMachineNumber, 1); // 1 : true
        }

        private void cmdSetDeviceInfo_Click(object sender, EventArgs e)
        {
            int vInfo;
            int vValue;
            int vErrorCode = 0;
            Boolean vRet;

            lblMessage.Text = "Working...";
            Application.DoEvents();

            vInfo = cmbSatus.SelectedIndex + 1;
            if (vInfo != 20)
                vValue = Convert.ToInt32(txtSetDevInfo.Text == "" ? "0" : txtSetDevInfo.Text);
            else
                vValue = util.pubIPAddrToLong(txtSetDevInfo.Text == "" ? "0" : txtSetDevInfo.Text);

            vRet = bpc.EnableDevice(Program.gMachineNumber, 0); // 0 : false
            if (!vRet)
            {
                lblMessage.Text = util.gstrNoDevice;
                return;
            }

            vRet = bpc.SetDeviceInfo(Program.gMachineNumber, vInfo, vValue);
            if (vRet)
            {
                lblMessage.Text = "Success!";

                //SmackBio
                if (vInfo == 2)
                {
                    Program.gMachineNumber = vValue;
                    util.Sleep(1000);
                }
            }
            else
            {
                bpc.GetLastError(ref vErrorCode);
                lblMessage.Text = util.ErrorPrint(vErrorCode);
            }
            bpc.EnableDevice(Program.gMachineNumber, 1); // 1 : true
            Application.DoEvents();
        }

        private void cmdGetDeviceStaus_Click(object sender, EventArgs e)
        {
            int vStatus;
            int vValue = 0;
            int vErrorCode = 0;
            Boolean vRet;

            lblMessage.Text = "Working...";
            Application.DoEvents();

            vStatus = cmbSatus.SelectedIndex + 1;

            vRet = bpc.EnableDevice(Program.gMachineNumber, 0); // 0 : false
            if (!vRet)
            {
                lblMessage.Text = util.gstrNoDevice;
                return;
            }

            vRet = bpc.GetDeviceStatus(Program.gMachineNumber, vStatus, ref vValue);
            if (vRet)
            {
                switch (vStatus)
                {
                    case 1:
                        lblMessage.Text = "(1) = Manager count = " + vValue;
                        break;
                    case 2:
                        lblMessage.Text = "(2) = User count = " + vValue;
                        break;
                    case 3:
                        lblMessage.Text = "(3) = Fp count = " + vValue;
                        break;
                    case 4:
                        lblMessage.Text = "(4) = Password count = " + vValue;
                        break;
                    case 5:
                        lblMessage.Text = "(5) = SLog count = " + vValue;
                        break;
                    case 6:
                        lblMessage.Text = "(6) = GLog count = " + vValue;
                        break;
                    case 7:
                        lblMessage.Text = "(7) = Card count = " + vValue;
                        break;
                    case 8:
                        lblMessage.Text = "(8) = Alarm status = " + vValue;
                        break;
                }
            }
            else
            {
                bpc.GetLastError(ref vErrorCode);
                lblMessage.Text = util.ErrorPrint(vErrorCode);
            }

            bpc.EnableDevice(Program.gMachineNumber, 1); // 1 : true
        }

        private void frmSystemInfo_Load(object sender, EventArgs e)
        {
            cmbSatus.SelectedIndex = 0;
            bpc = (AxSBXPCLib.AxSBXPC)Application.OpenForms["frmMain"].Controls["SBXPC1"];
        }

        private void frmSystemInfo_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.OpenForms["frmMain"].Visible = true;
        }
    }
}
