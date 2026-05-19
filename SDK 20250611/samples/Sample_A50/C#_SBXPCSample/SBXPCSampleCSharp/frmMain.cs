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
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
        }

        frmEvent frm_event = new frmEvent();

        Boolean mOpenFlag;
        private void cmdOpen_Click(object sender, EventArgs e)
        {
            String lpszIPAddress;
            //		Dim vRet As Boolean
            Program.gMachineNumber = Convert.ToInt32(cmbMachineNumber.Text);
		    if (optNetworkDevice.Checked)
            {
			    lpszIPAddress = txtIPAddress.Text;
			    if (SBXPC1.ConnectTcpip(Program.gMachineNumber, ref lpszIPAddress, Convert.ToInt32(txtPortNo.Text), Convert.ToInt32(txtPassword.Text))) 
                {
				    mOpenFlag = true;
				    cmdOpen.Enabled = false;

				    cmdClose.Enabled = true;
				    cmdEnrollData.Enabled = true;
				    cmdLogData.Enabled = true;
				    cmdSystemInfo.Enabled = true;
				    cmdProuctCode.Enabled = true;
				    cmdBellInfo.Enabled = true;
				    cmdMiscSettings.Enabled = true;
                    cmdAccessTz.Enabled = true;
                    cmdGroupName.Enabled = true;
                    cmdHoliday.Enabled = true;
                    cmdDoorKey.Enabled = true;
                }
		    }
		    if (optSerialDevice.Checked)
            {
			    if (SBXPC1.ConnectSerial(Program.gMachineNumber, cmbComPort.SelectedIndex + 1, Convert.ToInt32(cmbBaudrate.Text)))
                {
				    mOpenFlag = true;
				    cmdOpen.Enabled = false;
				    cmdClose.Enabled = true;
				    cmdEnrollData.Enabled = true;
				    cmdLogData.Enabled = true;
				    cmdSystemInfo.Enabled = true;
				    cmdProuctCode.Enabled = true;
				    cmdBellInfo.Enabled = true;
				    cmdMiscSettings.Enabled = true;
                    cmdAccessTz.Enabled = true;
                    cmdGroupName.Enabled = true;
                    cmdHoliday.Enabled = true;
                    cmdDoorKey.Enabled = true;
                }
		    }
            if (optUSBDevice.Checked)
            {
                if (SBXPC1.ConnectSerial(Program.gMachineNumber, 0, 0))
                {
                    mOpenFlag = true;
                    cmdOpen.Enabled = false;
                    cmdClose.Enabled = true;
                    cmdEnrollData.Enabled = true;
                    cmdLogData.Enabled = true;
                    cmdSystemInfo.Enabled = true;
                    cmdProuctCode.Enabled = true;
                    cmdBellInfo.Enabled = true;
                    cmdMiscSettings.Enabled = true;
                    cmdAccessTz.Enabled = true;
                    cmdGroupName.Enabled = true;
                    cmdHoliday.Enabled = true;
                    cmdDoorKey.Enabled = true;
                }
            }
        }

        public void Reopen(int nMachineNum)
        {
            Program.gMachineNumber = nMachineNum;
            cmbMachineNumber.SelectedIndex = nMachineNum - 1;
            cmdOpen_Click(null, null);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            optSerialDevice.Checked = false;
            lblComPort.Enabled = false;
            cmbComPort.Enabled = false;
            lblBaudrate.Enabled = false;
            cmbBaudrate.Enabled = false;

            optNetworkDevice.Checked = false;
            lblIPAddress.Enabled = false;
            txtIPAddress.Enabled = false;
            lblPortNo.Enabled = false;
            txtPortNo.Enabled = false;
            lblPassword.Enabled = false;
            txtPassword.Enabled = false;

            cmdOpen.Enabled = true;
            cmdClose.Enabled = false;
            cmdEnrollData.Enabled = false;
            cmdLogData.Enabled = false;
            cmdSystemInfo.Enabled = false;
            cmdProuctCode.Enabled = false;
            cmdBellInfo.Enabled = false;
            cmdMiscSettings.Enabled = false;
            cmdAccessTz.Enabled = false;
            cmdGroupName.Enabled = false;
            cmdHoliday.Enabled = false;
            cmdDoorKey.Enabled = false;

            mOpenFlag = false;
            cmbMachineNumber.Text = Convert.ToString(1);
            cmbComPort.Text = Convert.ToString(1);
            cmbBaudrate.Text = "115200";

            SBXPC1.DotNET();
        }

        private void cmdClose_Click(object sender, EventArgs e)
        {
            if (mOpenFlag == true)
            {
                SBXPC1.Disconnect();
			    mOpenFlag = false;
			    cmdOpen.Enabled = true;
			    cmdClose.Enabled = false;
                cmdEnrollData.Enabled = false;
                cmdLogData.Enabled = false;
                cmdSystemInfo.Enabled = false;
                cmdProuctCode.Enabled = false;
                cmdBellInfo.Enabled = false;
                cmdMiscSettings.Enabled = false;
                cmdAccessTz.Enabled = false;
                cmdGroupName.Enabled = false;
                cmdHoliday.Enabled = false;
                cmdDoorKey.Enabled = false;
            }
        }

        private void optSerialDevice_CheckedChanged(object sender, EventArgs e)
        {
            if (optSerialDevice.Checked)
            {
                String lpszIPAddress;
                optSerialDevice.Checked = true;
                optNetworkDevice.Checked = false;
                optUSBDevice.Checked = false;

                if (optSerialDevice.Checked)
                {
                    lblComPort.Enabled = true;
                    cmbComPort.Enabled = true;
                    lblBaudrate.Enabled = true;
                    cmbBaudrate.Enabled = true;
                    lblIPAddress.Enabled = false;
                    txtIPAddress.Enabled = false;
                    lblPortNo.Enabled = false;
                    txtPortNo.Enabled = false;
                    lblPassword.Enabled = false;
                    txtPassword.Enabled = false;
                }
                else if (optNetworkDevice.Checked)
                {
                    lblComPort.Enabled = false;
                    cmbComPort.Enabled = false;
                    lblBaudrate.Enabled = false;
                    cmbBaudrate.Enabled = false;
                    lblIPAddress.Enabled = true;
                    txtIPAddress.Enabled = true;
                    lblPortNo.Enabled = true;
                    txtPortNo.Enabled = true;
                    lblPassword.Enabled = true;
                    txtPassword.Enabled = true;
                    lpszIPAddress = txtIPAddress.Text;
                }
                else
                {
                    lblComPort.Enabled = false;
                    cmbComPort.Enabled = false;
                    lblBaudrate.Enabled = false;
                    cmbBaudrate.Enabled = false;
                    lblIPAddress.Enabled = false;
                    txtIPAddress.Enabled = false;
                    lblPortNo.Enabled = false;
                    txtPortNo.Enabled = false;
                    lblPassword.Enabled = false;
                    txtPassword.Enabled = false;
                }
            }
        }

        private void optNetworkDevice_CheckedChanged(object sender, EventArgs e)
        {
            if (optNetworkDevice.Checked)
            {
                String lpszIPAddress;
                optSerialDevice.Checked = false;
                optNetworkDevice.Checked = true;
                optUSBDevice.Checked = false;

                if (optSerialDevice.Checked)
                {
                    lblComPort.Enabled = true;
                    cmbComPort.Enabled = true;
                    lblBaudrate.Enabled = true;
                    cmbBaudrate.Enabled = true;
                    lblIPAddress.Enabled = false;
                    txtIPAddress.Enabled = false;
                    lblPortNo.Enabled = false;
                    txtPortNo.Enabled = false;
                    lblPassword.Enabled = false;
                    txtPassword.Enabled = false;
                }
                else if (optNetworkDevice.Checked)
                {
                    lblComPort.Enabled = false;
                    cmbComPort.Enabled = false;
                    lblBaudrate.Enabled = false;
                    cmbBaudrate.Enabled = false;
                    lblIPAddress.Enabled = true;
                    txtIPAddress.Enabled = true;
                    lblPortNo.Enabled = true;
                    txtPortNo.Enabled = true;
                    lblPassword.Enabled = true;
                    txtPassword.Enabled = true;
                    lpszIPAddress = txtIPAddress.Text;
                }
                else
                {
                    lblComPort.Enabled = false;
                    cmbComPort.Enabled = false;
                    lblBaudrate.Enabled = false;
                    cmbBaudrate.Enabled = false;
                    lblIPAddress.Enabled = false;
                    txtIPAddress.Enabled = false;
                    lblPortNo.Enabled = false;
                    txtPortNo.Enabled = false;
                    lblPassword.Enabled = false;
                    txtPassword.Enabled = false;
                }
            }
        }

        private void optUSBDevice_CheckedChanged(object sender, EventArgs e)
        {
            if (optUSBDevice.Checked)
            {
                String lpszIPAddress;
                optSerialDevice.Checked = false;
                optNetworkDevice.Checked = false;
                optUSBDevice.Checked = true;

                if (optSerialDevice.Checked)
                {
                    lblComPort.Enabled = true;
                    cmbComPort.Enabled = true;
                    lblBaudrate.Enabled = true;
                    cmbBaudrate.Enabled = true;
                    lblIPAddress.Enabled = false;
                    txtIPAddress.Enabled = false;
                    lblPortNo.Enabled = false;
                    txtPortNo.Enabled = false;
                    lblPassword.Enabled = false;
                    txtPassword.Enabled = false;
                }
                else if (optNetworkDevice.Checked)
                {
                    lblComPort.Enabled = false;
                    cmbComPort.Enabled = false;
                    lblBaudrate.Enabled = false;
                    cmbBaudrate.Enabled = false;
                    lblIPAddress.Enabled = true;
                    txtIPAddress.Enabled = true;
                    lblPortNo.Enabled = true;
                    txtPortNo.Enabled = true;
                    lblPassword.Enabled = true;
                    txtPassword.Enabled = true;
                    lpszIPAddress = txtIPAddress.Text;
                }
                else
                {
                    lblComPort.Enabled = false;
                    cmbComPort.Enabled = false;
                    lblBaudrate.Enabled = false;
                    cmbBaudrate.Enabled = false;
                    lblIPAddress.Enabled = false;
                    txtIPAddress.Enabled = false;
                    lblPortNo.Enabled = false;
                    txtPortNo.Enabled = false;
                    lblPassword.Enabled = false;
                    txtPassword.Enabled = false;
                }
            }
        }

        private void cmdProuctCode_Click(object sender, EventArgs e)
        {
            frmPrtCode frm_prtcode = new frmPrtCode();
            frm_prtcode.Activate();
            frm_prtcode.Visible = true;
            this.Visible = false;
        }

        private void cmdExit_Click(object sender, EventArgs e)
        {
           if (mOpenFlag) 
               SBXPC1.Disconnect();
            Close();
        }

        private void frmMain_FormClosed(object sender, FormClosedEventArgs e)
        {

        }

        private void cmdSystemInfo_Click(object sender, EventArgs e)
        {
            frmSystemInfo frm_SystemInfo = new frmSystemInfo();
            frm_SystemInfo.Activate();
            frm_SystemInfo.Visible = true;
            this.Visible = false;
        }

        private void cmdMiscSettings_Click(object sender, EventArgs e)
        {
            frmMiscSettings frm_misc = new frmMiscSettings();
            frm_misc.Activate();
            frm_misc.Visible = true;
            this.Visible = false;
        }

        private void cmdBellInfo_Click(object sender, EventArgs e)
        {
            frmBellInfo frm_BellInfo = new frmBellInfo();
            frm_BellInfo.Activate();
            frm_BellInfo.Visible = true;
            this.Visible = false;
        }

        private void cmdLogData_Click(object sender, EventArgs e)
        {
            frmLog frm_log = new frmLog();
            frm_log.Activate();
            frm_log.Visible = true;
            this.Visible = false;
        }

        private void cmdEnrollData_Click(object sender, EventArgs e)
        {
            frmEnroll frm_enroll = new frmEnroll();
            frm_enroll.Activate();
            frm_enroll.Visible = true;
            this.Visible = false;
        }

        private void cmdAccessTz_Click(object sender, EventArgs e)
        {
            frmAccessTz frm_access_tz = new frmAccessTz();
            frm_access_tz.Activate();
            frm_access_tz.Visible = true;
            this.Visible = false;
        }

        private void cmdHoliday_Click(object sender, EventArgs e)
        {
            frmHoliday frm_holiday = new frmHoliday();
            frm_holiday.Activate();
            frm_holiday.Visible = true;
            this.Visible = false;
        }

        private void cmdEventMonitor_Click(object sender, EventArgs e)
        {
            frm_event.Activate();
            frm_event.Visible = true;
            this.Visible = false;
        }

        private void SBXPC1_OnReceiveEventXML(object sender, AxSBXPCLib._DSBXPCEvents_OnReceiveEventXMLEvent e)
        {
            frm_event.ReceiveEvent(e.lpszEventXML);
        }

        private void cmdDoorKey_Click(object sender, EventArgs e)
        {
            frmDoorKey frm_doorKey = new frmDoorKey();
            frm_doorKey.Activate();
            frm_doorKey.Visible = true;
            this.Visible = false;
        }

        private void cmdGroupName_Click(object sender, EventArgs e)
        {
            frmGroup frm_group = new frmGroup();
            frm_group.Activate();
            frm_group.Visible = true;
            this.Visible = false;
        }
    }
}
