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
    public partial class frmLockCtrl : Form
    {
        public frmLockCtrl()
        {
            InitializeComponent();
        }

        

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
            
            vRet = sbxpc.SBXPCDLL.EnableDevice(Program.gMachineNumber, 0);
            if (!vRet)
            {
                lblMessage.Text = util.gstrNoDevice;
                return;
            }

            sbxpc.SBXPCDLL.XML_AddString(ref strXML, "REQUEST", "GetDoorStatusMulti");
            sbxpc.SBXPCDLL.XML_AddString(ref strXML, "MSGTYPE", "request");
            sbxpc.SBXPCDLL.XML_AddInt(ref strXML, "MachineID", Program.gMachineNumber);
            sbxpc.SBXPCDLL.XML_AddInt(ref strXML, "DoorNo", lDoorNumber);

            vRet = sbxpc.SBXPCDLL.GeneralOperationXML(Program.gMachineNumber, ref strXML);
            if (vRet)
            {
                vStatus = sbxpc.SBXPCDLL.XML_ParseLong(ref strXML, "DoorStatus");
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
                sbxpc.SBXPCDLL.GetLastError(Program.gMachineNumber, out vErrorCode);
                lblMessage.Text = util.ErrorPrint(vErrorCode);
            }
            
            sbxpc.SBXPCDLL.EnableDevice(Program.gMachineNumber, 1);
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
            
            vRet = sbxpc.SBXPCDLL.EnableDevice(Program.gMachineNumber, 0);
            if (!vRet)
            {
                lblMessage.Text = util.gstrNoDevice;
                return;
            }
            
            sbxpc.SBXPCDLL.XML_AddString(ref strXML, "REQUEST", "SetDoorStatusMulti");
            sbxpc.SBXPCDLL.XML_AddString(ref strXML, "MSGTYPE", "request");
            sbxpc.SBXPCDLL.XML_AddInt(ref strXML, "MachineID", Program.gMachineNumber);
            sbxpc.SBXPCDLL.XML_AddInt(ref strXML, "DoorNo", lDoorNumber);
            sbxpc.SBXPCDLL.XML_AddInt(ref strXML, "DoorStatus", 3);

            vRet = sbxpc.SBXPCDLL.GeneralOperationXML(Program.gMachineNumber, ref strXML);
            
            if (vRet)
            {
                lblMessage.Text = "Door Open Success!";
            }
            else
            {
                sbxpc.SBXPCDLL.GetLastError(Program.gMachineNumber, out vErrorCode);
                lblMessage.Text = util.ErrorPrint(vErrorCode);
            }
            sbxpc.SBXPCDLL.EnableDevice(Program.gMachineNumber, 1);
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
            
            vRet = sbxpc.SBXPCDLL.EnableDevice(Program.gMachineNumber, 0);
            if (!vRet)
            {
                lblMessage.Text = util.gstrNoDevice;
                return;
            }
            
            sbxpc.SBXPCDLL.XML_AddString(ref strXML, "REQUEST", "SetDoorStatusMulti");
            sbxpc.SBXPCDLL.XML_AddString(ref strXML, "MSGTYPE", "request");
            sbxpc.SBXPCDLL.XML_AddInt(ref strXML, "MachineID", Program.gMachineNumber);
            sbxpc.SBXPCDLL.XML_AddInt(ref strXML, "DoorNo", lDoorNumber);
            sbxpc.SBXPCDLL.XML_AddInt(ref strXML, "DoorStatus", 4);

            vRet = sbxpc.SBXPCDLL.GeneralOperationXML(Program.gMachineNumber, ref strXML);
            if (vRet)
            {
                lblMessage.Text = "Auto Recover Success!";
            }
            else
            {
                sbxpc.SBXPCDLL.GetLastError(Program.gMachineNumber, out vErrorCode);
                lblMessage.Text = util.ErrorPrint(vErrorCode);
            }
            sbxpc.SBXPCDLL.EnableDevice(Program.gMachineNumber, 1);
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
            
            sbxpc.SBXPCDLL.XML_AddString(ref strXML, "REQUEST", "SetDoorStatusMulti");
            sbxpc.SBXPCDLL.XML_AddString(ref strXML, "MSGTYPE", "request");
            sbxpc.SBXPCDLL.XML_AddInt(ref strXML, "MachineID", Program.gMachineNumber);
            sbxpc.SBXPCDLL.XML_AddInt(ref strXML, "DoorNo", lDoorNumber);
            sbxpc.SBXPCDLL.XML_AddInt(ref strXML, "DoorStatus", 5);

            vRet = sbxpc.SBXPCDLL.GeneralOperationXML(Program.gMachineNumber, ref strXML);

            if (vRet)
            {
                lblMessage.Text = "Reboot Success!";
            }
            else
            {
                sbxpc.SBXPCDLL.GetLastError(Program.gMachineNumber, out vErrorCode);
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
            
            vRet = sbxpc.SBXPCDLL.EnableDevice(Program.gMachineNumber, 0);
            if (!vRet)
            {
                lblMessage.Text = util.gstrNoDevice;
                return;
            }
            
            sbxpc.SBXPCDLL.XML_AddString(ref strXML, "REQUEST", "SetDoorStatusMulti");
            sbxpc.SBXPCDLL.XML_AddString(ref strXML, "MSGTYPE", "request");
            sbxpc.SBXPCDLL.XML_AddInt(ref strXML, "MachineID", Program.gMachineNumber);
            sbxpc.SBXPCDLL.XML_AddInt(ref strXML, "DoorNo", lDoorNumber);
            sbxpc.SBXPCDLL.XML_AddInt(ref strXML, "DoorStatus", 1);
            vRet = sbxpc.SBXPCDLL.GeneralOperationXML(Program.gMachineNumber, ref strXML);
            if (vRet)
            {
                lblMessage.Text = "Uncond Door Open Success!";
            }
            else
            {
                sbxpc.SBXPCDLL.GetLastError(Program.gMachineNumber, out vErrorCode);
                lblMessage.Text = util.ErrorPrint(vErrorCode);
            }
            sbxpc.SBXPCDLL.EnableDevice(Program.gMachineNumber, 1);
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
            
            vRet = sbxpc.SBXPCDLL.EnableDevice(Program.gMachineNumber, 0);
            if (!vRet)
            {
                lblMessage.Text = util.gstrNoDevice;
                return;
            }
            
            sbxpc.SBXPCDLL.XML_AddString(ref strXML, "REQUEST", "SetDoorStatusMulti");
            sbxpc.SBXPCDLL.XML_AddString(ref strXML, "MSGTYPE", "request");
            sbxpc.SBXPCDLL.XML_AddInt(ref  strXML, "MachineID", Program.gMachineNumber);
            sbxpc.SBXPCDLL.XML_AddInt(ref strXML, "DoorNo", lDoorNumber);
            sbxpc.SBXPCDLL.XML_AddInt(ref strXML, "DoorStatus", 2);

            vRet = sbxpc.SBXPCDLL.GeneralOperationXML(Program.gMachineNumber, ref strXML);
            if (vRet)
            {
                lblMessage.Text = "Uncond Door Close Success!";
            }
            else
            {
                sbxpc.SBXPCDLL.GetLastError(Program.gMachineNumber, out vErrorCode);
                lblMessage.Text = util.ErrorPrint(vErrorCode);
            }
            sbxpc.SBXPCDLL.EnableDevice(Program.gMachineNumber, 1);
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
            
            vRet = sbxpc.SBXPCDLL.EnableDevice(Program.gMachineNumber, 0);
            if (!vRet)
            {
                lblMessage.Text = util.gstrNoDevice;
                return;
            }
            
            sbxpc.SBXPCDLL.XML_AddString(ref strXML, "REQUEST", "SetDoorStatusMulti");
            sbxpc.SBXPCDLL.XML_AddString(ref strXML, "MSGTYPE", "request");
            sbxpc.SBXPCDLL.XML_AddInt(ref strXML, "MachineID", Program.gMachineNumber);
            sbxpc.SBXPCDLL.XML_AddInt(ref strXML, "DoorNo", lDoorNumber);
            sbxpc.SBXPCDLL.XML_AddInt(ref strXML, "DoorStatus", 6);
            vRet = sbxpc.SBXPCDLL.GeneralOperationXML(Program.gMachineNumber, ref strXML);
            
            if (vRet)
            {
                lblMessage.Text = "Warning cancel Success!";
            }
            else
            {
                sbxpc.SBXPCDLL.GetLastError(Program.gMachineNumber, out vErrorCode);
                lblMessage.Text = util.ErrorPrint(vErrorCode);
            }
            sbxpc.SBXPCDLL.EnableDevice(Program.gMachineNumber, 1);
            Application.DoEvents();
        }

        private void frmLockCrl_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.OpenForms["frmMain"].Visible = true;
        }

        private void frmLockCrl_Load(object sender, EventArgs e)
        {
            cmbSensorType.SelectedIndex = 1;
            cmbUseAntipass.SelectedIndex = 0;
            cmbAntipassNo.SelectedIndex = 0;
            cmbLocation.SelectedIndex = 0;
        }

        private void cmdGet_Click(object sender, EventArgs e)
        {
            Boolean bRet;
            int vErrorCode = 0;

            int lDoorNumber;
            String strXML = "";
            
            lblMessage.Text = "Waiting...";
            Application.DoEvents();
            
            bRet = sbxpc.SBXPCDLL.EnableDevice(Program.gMachineNumber, 0);
            if (!bRet)
            {
                lblMessage.Text = util.gstrNoDevice;
                return;
            }
            
            lDoorNumber = Convert.ToInt32(txtDoorNumber.Text);
            
            sbxpc.SBXPCDLL.XML_AddString(ref strXML, "REQUEST", "GetDoorParam");
            sbxpc.SBXPCDLL.XML_AddString(ref strXML, "MSGTYPE", "request");
            sbxpc.SBXPCDLL.XML_AddInt(ref strXML, "MachineID", Program.gMachineNumber);
            sbxpc.SBXPCDLL.XML_AddInt(ref strXML, "DoorNo", lDoorNumber);
            
            bRet = sbxpc.SBXPCDLL.GeneralOperationXML(Program.gMachineNumber, ref strXML);

            if (bRet)
            {
                cmbSensorType.SelectedIndex = sbxpc.SBXPCDLL.XML_ParseInt(ref strXML, "DoorSensorType");
                txtLockReleaseTime.Text = Convert.ToString(sbxpc.SBXPCDLL.XML_ParseInt(ref strXML, "LockReleaseTime"));
                txtOpenTimeout.Text = Convert.ToString(sbxpc.SBXPCDLL.XML_ParseInt(ref strXML, "DoorOpenTimeout"));
                cmbUseAntipass.SelectedIndex = 1 - sbxpc.SBXPCDLL.XML_ParseInt(ref strXML, "UseAntipass");
                cmbAntipassNo.SelectedIndex = sbxpc.SBXPCDLL.XML_ParseInt(ref strXML, "AntipassNo");
                cmbLocation.SelectedIndex = sbxpc.SBXPCDLL.XML_ParseInt(ref strXML, "Location");
                lblMessage.Text = "Success!";
            }
            else
            {
                sbxpc.SBXPCDLL.GetLastError(Program.gMachineNumber, out vErrorCode);
                lblMessage.Text = util.ErrorPrint(vErrorCode);
            }
            sbxpc.SBXPCDLL.EnableDevice(Program.gMachineNumber, 1);
        }

        private void cmdSet_Click(object sender, EventArgs e)
        {
            Boolean bRet;
            int vErrorCode = 0;

            int lDoorNumber;
            String strXML = "";
            
            lblMessage.Text = "Waiting...";
            Application.DoEvents();
            
            bRet = sbxpc.SBXPCDLL.EnableDevice(Program.gMachineNumber, 0);
            if (!bRet)
            {
                lblMessage.Text = util.gstrNoDevice;
                return;
            }
            
            lDoorNumber = Convert.ToInt32(txtDoorNumber.Text);
            
            sbxpc.SBXPCDLL.XML_AddString(ref strXML, "REQUEST", "SetDoorParam");
            sbxpc.SBXPCDLL.XML_AddString(ref strXML, "MSGTYPE", "request");
            sbxpc.SBXPCDLL.XML_AddInt(ref strXML, "MachineID", Program.gMachineNumber);
            sbxpc.SBXPCDLL.XML_AddInt(ref strXML, "DoorNo", lDoorNumber);
            
            sbxpc.SBXPCDLL.XML_AddInt(ref strXML, "DoorSensorType", cmbSensorType.SelectedIndex);
            sbxpc.SBXPCDLL.XML_AddInt(ref strXML, "LockReleaseTime", Convert.ToInt32(txtLockReleaseTime.Text));
            sbxpc.SBXPCDLL.XML_AddInt(ref strXML, "DoorOpenTimeout", Convert.ToInt32(txtOpenTimeout.Text));
            sbxpc.SBXPCDLL.XML_AddInt(ref strXML, "UseAntipass", 1 - cmbUseAntipass.SelectedIndex);
            sbxpc.SBXPCDLL.XML_AddInt(ref strXML, "AntipassNo", cmbAntipassNo.SelectedIndex);
            sbxpc.SBXPCDLL.XML_AddInt(ref strXML, "Location", cmbLocation.SelectedIndex);

            bRet = sbxpc.SBXPCDLL.GeneralOperationXML(Program.gMachineNumber, ref strXML);

            if (bRet)
            {
                lblMessage.Text = "Success!";
            }
            else
            {
                sbxpc.SBXPCDLL.GetLastError(Program.gMachineNumber, out vErrorCode);
                lblMessage.Text = util.ErrorPrint(vErrorCode);
            }
            sbxpc.SBXPCDLL.EnableDevice(Program.gMachineNumber, 1);
        }

		private void cmdReadUnlockGroup_Click(object sender, EventArgs e)
		{
			Boolean bRet;
			int vErrorCode = 0;
			String strXML = "";
			byte[] unlockGroup = new byte[2*10];

			lblMessage.Text = "Waiting...";
			Application.DoEvents();

			bRet = sbxpc.SBXPCDLL.EnableDevice(Program.gMachineNumber, 0);
			if (!bRet)
			{
				lblMessage.Text = util.gstrNoDevice;
				return;
			}

			sbxpc.SBXPCDLL.XML_AddString(ref strXML, "REQUEST", "GetUnlockgroup");
			sbxpc.SBXPCDLL.XML_AddString(ref strXML, "MSGTYPE", "request");
			sbxpc.SBXPCDLL.XML_AddInt(ref strXML, "MachineID", Program.gMachineNumber);

			bRet = sbxpc.SBXPCDLL.GeneralOperationXML(Program.gMachineNumber, ref strXML);

			if (bRet)
			{
				GCHandle gh = GCHandle.Alloc(unlockGroup, GCHandleType.Pinned);
				IntPtr pinnedAddr = gh.AddrOfPinnedObject();
                sbxpc.SBXPCDLL.XML_ParseBinaryLong(ref strXML, "UnlockGroupBinary", pinnedAddr, 2 * 10);
				txtUnlockGroup1.Text = Convert.ToString(unlockGroup[0]);
				txtUnlockGroup2.Text = Convert.ToString(unlockGroup[1]);
				txtUnlockGroup3.Text = Convert.ToString(unlockGroup[2]);
				txtUnlockGroup4.Text = Convert.ToString(unlockGroup[3]);
				txtUnlockGroup5.Text = Convert.ToString(unlockGroup[4]);
				txtUnlockGroup6.Text = Convert.ToString(unlockGroup[5]);
				txtUnlockGroup7.Text = Convert.ToString(unlockGroup[6]);
				txtUnlockGroup8.Text = Convert.ToString(unlockGroup[7]);
				txtUnlockGroup9.Text = Convert.ToString(unlockGroup[8]);
				txtUnlockGroup10.Text = Convert.ToString(unlockGroup[9]);
				txtUnlockGroup11.Text = Convert.ToString(unlockGroup[10]);
				txtUnlockGroup12.Text = Convert.ToString(unlockGroup[11]);
				txtUnlockGroup13.Text = Convert.ToString(unlockGroup[12]);
				txtUnlockGroup14.Text = Convert.ToString(unlockGroup[13]);
				txtUnlockGroup15.Text = Convert.ToString(unlockGroup[14]);
				txtUnlockGroup16.Text = Convert.ToString(unlockGroup[15]);
				txtUnlockGroup17.Text = Convert.ToString(unlockGroup[16]);
				txtUnlockGroup18.Text = Convert.ToString(unlockGroup[17]);
				txtUnlockGroup19.Text = Convert.ToString(unlockGroup[18]);
				txtUnlockGroup20.Text = Convert.ToString(unlockGroup[19]);

				lblMessage.Text = "Success!";
			}
			else
			{
				sbxpc.SBXPCDLL.GetLastError(Program.gMachineNumber, out vErrorCode);
				lblMessage.Text = util.ErrorPrint(vErrorCode);
			}
			sbxpc.SBXPCDLL.EnableDevice(Program.gMachineNumber, 1);
		}

		private void cmdWriteUnlockGroup_Click(object sender, EventArgs e)
		{
			Boolean bRet;
			int vErrorCode = 0;
			String strXML = "";
			byte[] unlockGroup = new byte[2 * 10];

			lblMessage.Text = "Waiting...";
			Application.DoEvents();

			bRet = sbxpc.SBXPCDLL.EnableDevice(Program.gMachineNumber, 0);
			if (!bRet)
			{
				lblMessage.Text = util.gstrNoDevice;
				return;
			}

			if (txtUnlockGroup1.Text == "") unlockGroup[0] = 0; else unlockGroup[0] = Convert.ToByte(txtUnlockGroup1.Text);
			if (txtUnlockGroup2.Text == "") unlockGroup[1] = 0; else unlockGroup[1] = Convert.ToByte(txtUnlockGroup2.Text);
			if (txtUnlockGroup3.Text == "") unlockGroup[2] = 0; else unlockGroup[2] = Convert.ToByte(txtUnlockGroup3.Text);
			if (txtUnlockGroup4.Text == "") unlockGroup[3] = 0; else unlockGroup[3] = Convert.ToByte(txtUnlockGroup4.Text);
			if (txtUnlockGroup5.Text == "") unlockGroup[4] = 0; else unlockGroup[4] = Convert.ToByte(txtUnlockGroup5.Text);
			if (txtUnlockGroup6.Text == "") unlockGroup[5] = 0; else unlockGroup[5] = Convert.ToByte(txtUnlockGroup6.Text);
			if (txtUnlockGroup7.Text == "") unlockGroup[6] = 0; else unlockGroup[6] = Convert.ToByte(txtUnlockGroup7.Text);
			if (txtUnlockGroup8.Text == "") unlockGroup[7] = 0; else unlockGroup[7] = Convert.ToByte(txtUnlockGroup8.Text);
			if (txtUnlockGroup9.Text == "") unlockGroup[8] = 0; else unlockGroup[8] = Convert.ToByte(txtUnlockGroup9.Text);
			if (txtUnlockGroup10.Text == "") unlockGroup[9] = 0; else unlockGroup[9] = Convert.ToByte(txtUnlockGroup10.Text);
			if (txtUnlockGroup11.Text == "") unlockGroup[10] = 0; else unlockGroup[10] = Convert.ToByte(txtUnlockGroup11.Text);
			if (txtUnlockGroup12.Text == "") unlockGroup[11] = 0; else unlockGroup[11] = Convert.ToByte(txtUnlockGroup12.Text);
			if (txtUnlockGroup13.Text == "") unlockGroup[12] = 0; else unlockGroup[12] = Convert.ToByte(txtUnlockGroup13.Text);
			if (txtUnlockGroup14.Text == "") unlockGroup[13] = 0; else unlockGroup[13] = Convert.ToByte(txtUnlockGroup14.Text);
			if (txtUnlockGroup15.Text == "") unlockGroup[14] = 0; else unlockGroup[14] = Convert.ToByte(txtUnlockGroup15.Text);
			if (txtUnlockGroup16.Text == "") unlockGroup[15] = 0; else unlockGroup[15] = Convert.ToByte(txtUnlockGroup16.Text);
			if (txtUnlockGroup17.Text == "") unlockGroup[16] = 0; else unlockGroup[16] = Convert.ToByte(txtUnlockGroup17.Text);
			if (txtUnlockGroup18.Text == "") unlockGroup[17] = 0; else unlockGroup[17] = Convert.ToByte(txtUnlockGroup18.Text);
			if (txtUnlockGroup19.Text == "") unlockGroup[18] = 0; else unlockGroup[18] = Convert.ToByte(txtUnlockGroup19.Text);
			if (txtUnlockGroup20.Text == "") unlockGroup[19] = 0; else unlockGroup[19] = Convert.ToByte(txtUnlockGroup20.Text);

			sbxpc.SBXPCDLL.XML_AddString(ref strXML, "REQUEST", "SetUnlockgroup");
			sbxpc.SBXPCDLL.XML_AddString(ref strXML, "MSGTYPE", "request");
			sbxpc.SBXPCDLL.XML_AddInt(ref strXML, "MachineID", Program.gMachineNumber);

			GCHandle gh = GCHandle.Alloc(unlockGroup, GCHandleType.Pinned);
			IntPtr pinnedAddr = gh.AddrOfPinnedObject();
            sbxpc.SBXPCDLL.XML_AddBinaryLong(ref strXML, "UnlockGroupBinary", pinnedAddr, 2 * 10);

			bRet = sbxpc.SBXPCDLL.GeneralOperationXML(Program.gMachineNumber, ref strXML);

			if (bRet)
			{
				lblMessage.Text = "Success!";
			}
			else
			{
				sbxpc.SBXPCDLL.GetLastError(Program.gMachineNumber, out vErrorCode);
				lblMessage.Text = util.ErrorPrint(vErrorCode);
			}
			sbxpc.SBXPCDLL.EnableDevice(Program.gMachineNumber, 1);
		}
    }
}
