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
    public partial class frmMiscSettings : Form
    {
        public frmMiscSettings()
        {
            InitializeComponent();
        }

        AxSBXPCLib.AxSBXPC bpc;

        private void cmdGetDoorStatus_Click(object sender, EventArgs e)
        {
            int vStatus;
            int vErrorCode = 0;
            Boolean vRet;
            int lDoorNumber;
            String strXML = "";
            
            lDoorNumber = Convert.ToInt32(txtDoorNumber.Text);
            
            lblMessage.Text = "Working...";
            Application.DoEvents();
            
            vRet = bpc.EnableDevice(Program.gMachineNumber, 0);
            if (!vRet)
            {
                lblMessage.Text = util.gstrNoDevice;
                return;
            }

            bpc.XML_AddString(ref strXML, "REQUEST", "GetDoorStatusMulti");
            bpc.XML_AddString(ref strXML, "MSGTYPE", "request");
            bpc.XML_AddInt(ref strXML, "MachineID", Program.gMachineNumber);
            bpc.XML_AddInt(ref strXML, "DoorNo", lDoorNumber);

            vRet = bpc.GeneralOperationXML(ref strXML);
            if (vRet)
            {
                vStatus = bpc.XML_ParseLong(ref strXML, "DoorStatus");
                switch (vStatus)
                {
                    case 1: lblMessage.Text = "Uncond Door Open State!"; break;
                    case 2: lblMessage.Text = "Uncond Door Close State!"; break;
                    case 3: lblMessage.Text = "Door Open State!"; break;
                    case 4: lblMessage.Text = "Auto Recover State!"; break;
                    case 5: lblMessage.Text = "Door Close State!"; break;
                    case 6: lblMessage.Text = "Watching for Close!"; break;
                    case 7: lblMessage.Text = "Illegal open!"; break;
                    default: lblMessage.Text = "User State !"; break;
                }
            }
            else
            {
                bpc.GetLastError(ref vErrorCode);
                lblMessage.Text = util.ErrorPrint(vErrorCode);
            }
            
            bpc.EnableDevice(Program.gMachineNumber, 1);
            Application.DoEvents();
        }

        private void cmdDoorOpen_Click(object sender, EventArgs e)
        {
            int vErrorCode = 0;
            Boolean vRet;
            String strXML = "";
            int lDoorNumber;
            
            lDoorNumber = Convert.ToInt32(txtDoorNumber.Text);
            
            lblMessage.Text = "Working...";
            Application.DoEvents();
            
            vRet = bpc.EnableDevice(Program.gMachineNumber, 0);
            if (!vRet)
            {
                lblMessage.Text = util.gstrNoDevice;
                return;
            }
            
            bpc.XML_AddString(ref strXML, "REQUEST", "SetDoorStatusMulti");
            bpc.XML_AddString(ref strXML, "MSGTYPE", "request");
            bpc.XML_AddInt(ref strXML, "MachineID", Program.gMachineNumber);
            bpc.XML_AddInt(ref strXML, "DoorNo", lDoorNumber);
            bpc.XML_AddInt(ref strXML, "DoorStatus", 3);

            vRet = bpc.GeneralOperationXML(ref strXML);
            
            if (vRet)
            {
                lblMessage.Text = "Door Open Success!";
            }
            else
            {
                bpc.GetLastError(ref vErrorCode);
                lblMessage.Text = util.ErrorPrint(vErrorCode);
            }
            bpc.EnableDevice(Program.gMachineNumber, 1);
            Application.DoEvents();
        }

        private void cmdAutoRecover_Click(object sender, EventArgs e)
        {
            int vErrorCode = 0;
            Boolean vRet;
            int lDoorNumber;
            String strXML = "";
            
            lDoorNumber = Convert.ToInt32(txtDoorNumber.Text);
            
            lblMessage.Text = "Working...";
            Application.DoEvents();
            
            vRet = bpc.EnableDevice(Program.gMachineNumber, 0);
            if (!vRet)
            {
                lblMessage.Text = util.gstrNoDevice;
                return;
            }
            
            bpc.XML_AddString(ref strXML, "REQUEST", "SetDoorStatusMulti");
            bpc.XML_AddString(ref strXML, "MSGTYPE", "request");
            bpc.XML_AddInt(ref strXML, "MachineID", Program.gMachineNumber);
            bpc.XML_AddInt(ref strXML, "DoorNo", lDoorNumber);
            bpc.XML_AddInt(ref strXML, "DoorStatus", 4);

            vRet = bpc.GeneralOperationXML(ref strXML);
            if (vRet)
            {
                lblMessage.Text = "Auto Recover Success!";
            }
            else
            {
                bpc.GetLastError(ref vErrorCode);
                lblMessage.Text = util.ErrorPrint(vErrorCode);
            }
            bpc.EnableDevice(Program.gMachineNumber, 1);
            Application.DoEvents();
        }

        private void cmdRestart_Click(object sender, EventArgs e)
        {
            int vErrorCode = 0;
            Boolean vRet;
            String strXML = "";
            int lDoorNumber;

            lDoorNumber = Convert.ToInt32(txtDoorNumber.Text);
            
            lblMessage.Text = "Working...";
            Application.DoEvents();
            
            bpc.XML_AddString(ref strXML, "REQUEST", "SetDoorStatusMulti");
            bpc.XML_AddString(ref strXML, "MSGTYPE", "request");
            bpc.XML_AddInt(ref strXML, "MachineID", Program.gMachineNumber);
            bpc.XML_AddInt(ref strXML, "DoorNo", lDoorNumber);
            bpc.XML_AddInt(ref strXML, "DoorStatus", 5);

            vRet = bpc.GeneralOperationXML(ref strXML);

            if (vRet)
            {
                lblMessage.Text = "Reboot Success!";
            }
            else
            {
                bpc.GetLastError(ref vErrorCode);
                lblMessage.Text = util.ErrorPrint(vErrorCode);
            }
            Application.DoEvents();
        }

        private void cmdUncondOpen_Click(object sender, EventArgs e)
        {
            int vErrorCode = 0;
            Boolean vRet;
            int lDoorNumber;
            String strXML = "";
            
            lDoorNumber = Convert.ToInt32(txtDoorNumber.Text);
            
            lblMessage.Text = "Working...";
            Application.DoEvents();
            
            vRet = bpc.EnableDevice(Program.gMachineNumber, 0);
            if (!vRet)
            {
                lblMessage.Text = util.gstrNoDevice;
                return;
            }
            
            bpc.XML_AddString(ref strXML, "REQUEST", "SetDoorStatusMulti");
            bpc.XML_AddString(ref strXML, "MSGTYPE", "request");
            bpc.XML_AddInt(ref strXML, "MachineID", Program.gMachineNumber);
            bpc.XML_AddInt(ref strXML, "DoorNo", lDoorNumber);
            bpc.XML_AddInt(ref strXML, "DoorStatus", 1);
            vRet = bpc.GeneralOperationXML(ref strXML);
            if (vRet)
            {
                lblMessage.Text = "Uncond Door Open Success!";
            }
            else
            {
                bpc.GetLastError(ref vErrorCode);
                lblMessage.Text = util.ErrorPrint(vErrorCode);
            }
            bpc.EnableDevice(Program.gMachineNumber, 1);
            Application.DoEvents();
        }

        private void cmdUncondClose_Click(object sender, EventArgs e)
        {
            int vErrorCode = 0;
            Boolean vRet;
            int lDoorNumber;
            String strXML = "";
            
            lDoorNumber = Convert.ToInt32(txtDoorNumber.Text);
            
            lblMessage.Text = "Working...";
            Application.DoEvents();
            
            vRet = bpc.EnableDevice(Program.gMachineNumber, 0);
            if (!vRet)
            {
                lblMessage.Text = util.gstrNoDevice;
                return;
            }
            
            bpc.XML_AddString(ref strXML, "REQUEST", "SetDoorStatusMulti");
            bpc.XML_AddString(ref strXML, "MSGTYPE", "request");
            bpc.XML_AddInt(ref  strXML, "MachineID", Program.gMachineNumber);
            bpc.XML_AddInt(ref strXML, "DoorNo", lDoorNumber);
            bpc.XML_AddInt(ref strXML, "DoorStatus", 2);

            vRet = bpc.GeneralOperationXML(ref strXML);
            if (vRet)
            {
                lblMessage.Text = "Uncond Door Close Success!";
            }
            else
            {
                bpc.GetLastError(ref vErrorCode);
                lblMessage.Text = util.ErrorPrint(vErrorCode);
            }
            bpc.EnableDevice(Program.gMachineNumber, 1);
            Application.DoEvents();
        }

        private void cmdWarnCancel_Click(object sender, EventArgs e)
        {
            int vErrorCode = 0;
            Boolean vRet;
            int lDoorNumber;
            String strXML = "";
            
            lDoorNumber = 0;
            
            lblMessage.Text = "Working...";
            Application.DoEvents();
            
            vRet = bpc.EnableDevice(Program.gMachineNumber, 0);
            if (!vRet)
            {
                lblMessage.Text = util.gstrNoDevice;
                return;
            }
            
            bpc.XML_AddString(ref strXML, "REQUEST", "SetDoorStatusMulti");
            bpc.XML_AddString(ref strXML, "MSGTYPE", "request");
            bpc.XML_AddInt(ref strXML, "MachineID", Program.gMachineNumber);
            bpc.XML_AddInt(ref strXML, "DoorNo", lDoorNumber);
            bpc.XML_AddInt(ref strXML, "DoorStatus", 6);
            vRet = bpc.GeneralOperationXML(ref strXML);
            
            if (vRet)
            {
                lblMessage.Text = "Warning cancel Success!";
            }
            else
            {
                bpc.GetLastError(ref vErrorCode);
                lblMessage.Text = util.ErrorPrint(vErrorCode);
            }
            bpc.EnableDevice(Program.gMachineNumber, 1);
            Application.DoEvents();
        }

        private void frmLockCrl_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.OpenForms["frmMain"].Visible = true;
        }

        private void frmLockCrl_Load(object sender, EventArgs e)
        {
            bpc = (AxSBXPCLib.AxSBXPC)Application.OpenForms["frmMain"].Controls["SBXPC1"];
            cmbAccessMode.SelectedIndex = 0;
            cmbAlarmOutMode.SelectedIndex = 0;
            cmbDualFpMode.SelectedIndex = 0;
            cmbUseM1.SelectedIndex = 0;
        }

        private void cmdRead_Click(object sender, EventArgs e)
        {
            Boolean bRet;
            int vErrorCode = 0;

            String strXML = "";

            lblMessage.Text = "Waiting...";
            Application.DoEvents();

            bRet = bpc.EnableDevice(Program.gMachineNumber, 0);
            if (!bRet)
            {
                lblMessage.Text = util.gstrNoDevice;
                return;
            }

            util.MakeXMLRequestHeader(bpc, ref strXML, "ReadAccessSetting");

            bRet = bpc.GeneralOperationXML(ref strXML);

            if (bRet)
            {
                cmbAccessMode.SelectedIndex = bpc.XML_ParseInt(ref strXML, "AccessMode");
                txtIllegVerifTimes.Text = Convert.ToString(bpc.XML_ParseInt(ref strXML, "IllegVerifTimes"));
                cmbAlarmOutMode.SelectedIndex = bpc.XML_ParseInt(ref strXML, "AlarmOutMode");
                txtDuressDelay.Text = Convert.ToString(bpc.XML_ParseInt(ref strXML, "DuressDelay"));
                txtSynchOpenCount.Text = Convert.ToString(bpc.XML_ParseInt(ref strXML, "SynchOpenCount"));
                cmbDualFpMode.SelectedIndex = bpc.XML_ParseInt(ref strXML, "DualFpMode");
                txtDualFpTimeout.Text = Convert.ToString(bpc.XML_ParseInt(ref strXML, "DualFpTimeout"));
                cmbUseM1.SelectedIndex = bpc.XML_ParseInt(ref strXML, "UseM1Card");
                lblMessage.Text = "Success!";
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
            Boolean bRet;
            int vErrorCode = 0;

            String strXML = "";

            lblMessage.Text = "Waiting...";
            Application.DoEvents();

            bRet = bpc.EnableDevice(Program.gMachineNumber, 0);
            if (!bRet)
            {
                lblMessage.Text = util.gstrNoDevice;
                return;
            }

            util.MakeXMLRequestHeader(bpc, ref strXML, "WriteAccessSetting");
            bpc.XML_AddInt(ref strXML, "AccessMode", cmbAccessMode.SelectedIndex);
            bpc.XML_AddInt(ref strXML, "IllegVerifTimes", Convert.ToInt32(txtIllegVerifTimes.Text));
            bpc.XML_AddInt(ref strXML, "AlarmOutMode", cmbAlarmOutMode.SelectedIndex);
            bpc.XML_AddInt(ref strXML, "DuressDelay", Convert.ToInt32(txtDuressDelay.Text));
            bpc.XML_AddInt(ref strXML, "SynchOpenCount", Convert.ToInt32(txtSynchOpenCount.Text));
            bpc.XML_AddInt(ref strXML, "DualFpMode", cmbDualFpMode.SelectedIndex);
            bpc.XML_AddInt(ref strXML, "DualFpTimeout", Convert.ToInt32(txtDualFpTimeout.Text));
            bpc.XML_AddInt(ref strXML, "UseM1Card", Convert.ToInt32(cmbUseM1.SelectedIndex));

            bRet = bpc.GeneralOperationXML(ref strXML);

            if (bRet)
            {
                lblMessage.Text = "Success!";
            }
            else
            {
                bpc.GetLastError(ref vErrorCode);
                lblMessage.Text = util.ErrorPrint(vErrorCode);
            }
            bpc.EnableDevice(Program.gMachineNumber, 1);
        }
    }
}
