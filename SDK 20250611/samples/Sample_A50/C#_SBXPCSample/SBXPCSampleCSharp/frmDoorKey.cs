using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.VisualBasic.Compatibility.VB6;
using System.Runtime.InteropServices;

namespace SBXPCSampleCSharp
{
    public partial class frmDoorKey : Form
    {
        

        public frmDoorKey()
        {
            InitializeComponent();
        }


        AxSBXPCLib.AxSBXPC bpc;

        Byte[] DbOpenTimeArray = new Byte[4 * 8];
        Byte[] DbUserGroupArray = new Byte[1 * 10];
        Byte[] DbUnlockGroupArray = new Byte[3 * 5];
        Boolean bShowProcessing = false;

        private void DbDoorOpenTimeDraw()
        {
            int r, c;
            bShowProcessing = true;
            timePicker.Visible = false;
            gridDoorOpenTime.Enabled = false;
            for (r = 0; r < 8; r ++)
            {
                gridDoorOpenTime.Row = r + 1;
                for (c = 0; c < 2; c ++)
                {
                    gridDoorOpenTime.Col = c + 1;
                    if (c % 2 == 0)
                        timePicker.Value = new System.DateTime(2010, 1, 1, DbOpenTimeArray[r * 4 + 0], DbOpenTimeArray[r * 4 + 1], 0);
				    else
                        timePicker.Value = new System.DateTime(2010, 1, 1, DbOpenTimeArray[r * 4 + 2], DbOpenTimeArray[r * 4 + 3], 0);
				    
                    gridDoorOpenTime.Text = timePicker.Value.ToString("HH:mm");
                }
            }
            bShowProcessing = false;
            gridDoorOpenTime.Enabled = true;
        }

        private void DbUserGroupDraw()
        {
            int r;
            bShowProcessing = true;
            cmbGroup.Visible = false;
            gridUserGroupSet.Enabled = false;
            for (r = 0; r < 10; r ++)
            {
                gridUserGroupSet.Row = r + 1;
                gridUserGroupSet.Col = 1;
                cmbGroup.SelectedIndex = DbUserGroupArray[r];
                gridUserGroupSet.Text = cmbGroup.Text;
            }
            bShowProcessing = false;
            gridUserGroupSet.Enabled = true;
        }

        private void DbUnlockGroupDraw()
        {
            int r, c;
            bShowProcessing = true;
            cmbUnlockGroup.Visible = false;
            gridUnlockGroupSet.Enabled = false;
            for (r = 0; r < 5; r ++)
            {
                for (c = 0; c < 3; c ++)
                {
                    gridUnlockGroupSet.Row = r + 1;
                    gridUnlockGroupSet.Col = c + 1;
                    cmbUnlockGroup.SelectedIndex = DbUnlockGroupArray[r * 3 + c];
                    gridUnlockGroupSet.Text = cmbUnlockGroup.Text;
                }
            }

            bShowProcessing = false;
            gridUnlockGroupSet.Enabled = true;
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void frmDoorKey_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.OpenForms["frmMain"].Visible = true;
        }

        private void cmdAccessTimezoneRead_Click(object sender, EventArgs e)
        {
            Boolean bRet;
            int vErrorCode = 0;

            int lDoorNumber;
            int lInfo;
            String strXML = "";

            lblMessage.Text = "Waiting...";
            Application.DoEvents();

            if (!bpc.EnableDevice(Program.gMachineNumber, 0))
            {
                lblMessage.Text = util.gstrNoDevice;
                return;
            }

            lDoorNumber = Convert.ToInt32(txtDoorNumber.Text);
            lInfo = 2;

            // make xml
            util.MakeXMLRequestHeader(bpc, ref strXML, "GetDoorParam");

            bpc.XML_AddInt(ref strXML, "DoorNo", lDoorNumber);
            bpc.XML_AddInt(ref strXML, "SubType", lInfo);

            bRet = bpc.GeneralOperationXML(ref strXML);

            if (bRet)
            {
                try
                {
                    txtTimezone1.Text = Convert.ToString(bpc.XML_ParseInt(ref strXML, "Timezone1"));
                    txtTimezone2.Text = Convert.ToString(bpc.XML_ParseInt(ref strXML, "Timezone2"));
                    txtTimezone3.Text = Convert.ToString(bpc.XML_ParseInt(ref strXML, "Timezone3"));
                    lblMessage.Text = "GetDoorParam(2-Timezone) = Success!";
                }
                catch (Exception)
                {
                    lblMessage.Text = "GetDoorParam(2-Timezone), XML Parse Error!";
                }
            }
            else
            {
                bpc.GetLastError(ref vErrorCode);
                lblMessage.Text = util.ErrorPrint(vErrorCode);
            }
            bpc.EnableDevice(Program.gMachineNumber, 1);
        }

        private void frmDoorKey_Load(object sender, EventArgs e)
        {
            bpc = (AxSBXPCLib.AxSBXPC)Application.OpenForms["frmMain"].Controls["SBXPC1"];
            
            int i;
            bShowProcessing = false;
            String[] OpenTimeColText = {"No.", "Start", "End"};
            String[] UserGroupColText = {"No.", "Group"};
            String[] UnlockGroupColText = {"No.", "Group1", "Group2", "Group3"};
            
        //======================================================================
            gridDoorOpenTime.Clear();
            gridDoorOpenTime.Row = 0;
            for (i = 0; i < 3; i ++)
            {
                gridDoorOpenTime.Col = i;
                gridDoorOpenTime.Text = OpenTimeColText[i];
                gridDoorOpenTime.set_ColWidth(i, (int)Support.PixelsToTwipsX(timePicker.Width));
                gridDoorOpenTime.set_ColAlignment(i, (short)MSFlexGridLib.AlignmentSettings.flexAlignRightCenter);
            }
            gridDoorOpenTime.set_ColWidth(0, (int)Support.PixelsToTwipsX(timePicker.Width));
            gridDoorOpenTime.set_RowHeight(0, (int)Support.PixelsToTwipsY(timePicker.Height));
            for (i = 1; i < 8; i ++)
            {
                gridDoorOpenTime.Row = i;
                gridDoorOpenTime.Col = 0;
                gridDoorOpenTime.Text = Convert.ToString(i);
                gridDoorOpenTime.set_RowHeight(i, (int)Support.PixelsToTwipsY(timePicker.Height));
            }
            DbDoorOpenTimeDraw();
        //======================================================================

            gridUserGroupSet.Clear();
            gridUserGroupSet.Row = 0;
            cmbGroup.SelectedIndex = 0;
            cmbUnlockGroup.SelectedIndex = 0;
            for (i = 0; i < 2; i ++)
            {
                gridUserGroupSet.Col = i;
                gridUserGroupSet.Text = UserGroupColText[i];
                gridUserGroupSet.set_ColWidth(i, (int)Support.PixelsToTwipsX(cmbGroup.Width));
                gridUserGroupSet.set_ColAlignment(i, (short)MSFlexGridLib.AlignmentSettings.flexAlignRightCenter);
            }
            gridUserGroupSet.set_ColWidth(0, 500);
            gridUserGroupSet.set_RowHeight(0, 300);

            for (i = 1; i < 11; i ++)
            {
                gridUserGroupSet.Row = i;
                gridUserGroupSet.Col = 0;
                gridUserGroupSet.Text = Convert.ToString(i);
                gridUserGroupSet.set_RowHeight(i, (int)Support.PixelsToTwipsY(cmbGroup.Height));
            }
            
            DbUserGroupDraw();
        //======================================================================
            gridUnlockGroupSet.Clear();
            gridUnlockGroupSet.Row = 0;
            
            for (i = 0; i < 4; i ++)
            {
                gridUnlockGroupSet.Col = i;
                gridUnlockGroupSet.Text = UnlockGroupColText[i];
                gridUnlockGroupSet.set_ColWidth(i, (int)Support.PixelsToTwipsX(cmbUnlockGroup.Width));
                gridUnlockGroupSet.set_ColAlignment(i, (short)MSFlexGridLib.AlignmentSettings.flexAlignRightCenter);
            }
            gridUnlockGroupSet.set_ColWidth(0, 500);
            gridUnlockGroupSet.set_RowHeight(0, 300);
            for (i = 1; i < 6; i ++)
            {
                gridUnlockGroupSet.Row = i;
                gridUnlockGroupSet.Col = 0;
                gridUnlockGroupSet.Text = Convert.ToString(i);
                gridUnlockGroupSet.set_RowHeight(i, (int)Support.PixelsToTwipsY(cmbUnlockGroup.Height));
            }
            
            DbUnlockGroupDraw();

            cmbSensorType.SelectedIndex = 1;
            cmbUseAntipass.SelectedIndex = 0;
            cmbAntipassNo.SelectedIndex = 0;
            cmbLocation.SelectedIndex = 0;
        }

        private void cmdAccessTimezoneWrite_Click(object sender, EventArgs e)
        {
            Boolean bRet;
            int vErrorCode = 0;

            int lDoorNumber;
            int lInfo;
            String strXML = "";

            lblMessage.Text = "Waiting...";
            Application.DoEvents();

            if (!bpc.EnableDevice(Program.gMachineNumber, 0))
            {
                lblMessage.Text = util.gstrNoDevice;
                return;
            }

            lDoorNumber = Convert.ToInt32(txtDoorNumber.Text);
            lInfo = 2;

            // make xml
            util.MakeXMLRequestHeader(bpc, ref strXML, "SetDoorParam");

            bpc.XML_AddInt(ref strXML, "DoorNo", lDoorNumber);
            bpc.XML_AddInt(ref strXML, "SubType", lInfo);
            
            bpc.XML_AddInt(ref strXML, "Timezone1", Convert.ToInt32(txtTimezone1.Text));
            bpc.XML_AddInt(ref strXML, "Timezone2", Convert.ToInt32(txtTimezone2.Text));
            bpc.XML_AddInt(ref strXML, "Timezone3", Convert.ToInt32(txtTimezone3.Text));

            bRet = bpc.GeneralOperationXML(ref strXML);

            if (bRet)
            {
                lblMessage.Text = "SetDoorParam(2-Timezone) = Success!";
            }
            else
            {
                bpc.GetLastError(ref vErrorCode);
                lblMessage.Text = util.ErrorPrint(vErrorCode);
            }
            bpc.EnableDevice(Program.gMachineNumber, 1);
        }

        private void gridDoorOpenTime_ClickEvent(object sender, EventArgs e)
        {
            gridDoorOpenTime_EnterCell(sender, new System.EventArgs());
        }

        private void gridDoorOpenTime_Scroll(object sender, EventArgs e)
        {
            gridDoorOpenTime_LeaveCell(sender, new System.EventArgs());
        }

        private void cmdUserAcceptModeRead_Click(object sender, EventArgs e)
        {
            Boolean bRet;
            int vErrorCode = 0;

            int lDoorNumber;
            int lInfo;
            String strXML = "";

            lblMessage.Text = "Waiting...";
            Application.DoEvents();

            if (!bpc.EnableDevice(Program.gMachineNumber, 0))
            {
                lblMessage.Text = util.gstrNoDevice;
                return;
            }

            lDoorNumber = Convert.ToInt32(txtDoorNumber.Text);
            lInfo = 4;

            // make xml
            util.MakeXMLRequestHeader(bpc, ref strXML, "GetDoorParam");

            bpc.XML_AddInt(ref strXML, "DoorNo", lDoorNumber);
            bpc.XML_AddInt(ref strXML, "SubType", lInfo);

            bRet = bpc.GeneralOperationXML(ref strXML);

            if (bRet)
            {
                try
                {
                    txtUserMode.Text = Convert.ToString(bpc.XML_ParseInt(ref strXML, "UserMode"));
                    lblMessage.Text = "GetDoorParam(4-UserMode) = Success!";
                }
                catch (Exception)
                {
                    lblMessage.Text = "GetDoorParam(4-UserMode), XML Parse Error!";
                }
            }
            else
            {
                bpc.GetLastError(ref vErrorCode);
                lblMessage.Text = util.ErrorPrint(vErrorCode);
            }
            bpc.EnableDevice(Program.gMachineNumber, 1);
        }

        private void cmdUserAcceptModeWrite_Click(object sender, EventArgs e)
        {
            Boolean bRet;
            int vErrorCode = 0;

            int lDoorNumber;
            int lInfo;
            String strXML = "";

            lblMessage.Text = "Waiting...";
            Application.DoEvents();

            if (!bpc.EnableDevice(Program.gMachineNumber, 0))
            {
                lblMessage.Text = util.gstrNoDevice;
                return;
            }

            lDoorNumber = Convert.ToInt32(txtDoorNumber.Text);
            lInfo = 4;

            // make xml
            util.MakeXMLRequestHeader(bpc, ref strXML, "SetDoorParam");

            bpc.XML_AddInt(ref strXML, "DoorNo", lDoorNumber);
            bpc.XML_AddInt(ref strXML, "SubType", lInfo);
            bpc.XML_AddInt(ref strXML, "UserMode", Convert.ToInt32(txtUserMode.Text));

            bRet = bpc.GeneralOperationXML(ref strXML);

            if (bRet)
            {
                lblMessage.Text = "SetDoorParam(4-UserMode) = Success!";
            }
            else
            {
                bpc.GetLastError(ref vErrorCode);
                lblMessage.Text = util.ErrorPrint(vErrorCode);
            }
            bpc.EnableDevice(Program.gMachineNumber, 1);
        }

        private void cmdDoorOpenTZWrite_Click(object sender, EventArgs e)
        {
            Boolean bRet;
            int vErrorCode = 0;

            int lDoorNumber;
            int lInfo;
            String strXML = "";

            lblMessage.Text = "Waiting...";
            Application.DoEvents();

            if (!bpc.EnableDevice(Program.gMachineNumber, 0))
            {
                lblMessage.Text = util.gstrNoDevice;
                return;
            }

            lDoorNumber = Convert.ToInt32(txtDoorNumber.Text);
            lInfo = 3;

            // make xml
            util.MakeXMLRequestHeader(bpc, ref strXML, "SetDoorParam");

            bpc.XML_AddInt(ref strXML, "DoorNo", lDoorNumber);
            bpc.XML_AddInt(ref strXML, "SubType", lInfo);

            GCHandle gh = GCHandle.Alloc(DbOpenTimeArray, GCHandleType.Pinned);
            IntPtr AddrOfDbOpenTimeArray = gh.AddrOfPinnedObject();
            int nAddr = AddrOfDbOpenTimeArray.ToInt32();
            bRet = bpc.XML_AddBinaryLong(ref strXML, "DoorOpenTimeBinary", ref nAddr, 4 * 8);

            bRet = bpc.GeneralOperationXML(ref strXML);

            if (bRet)
            {
                lblMessage.Text = "SetDoorParam(3-DoorOpenTime) = Success!";
            }
            else
            {
                bpc.GetLastError(ref vErrorCode);
                lblMessage.Text = util.ErrorPrint(vErrorCode);
            }
            bpc.EnableDevice(Program.gMachineNumber, 1);
        }

        private void cmdDoorOpenTZRead_Click(object sender, EventArgs e)
        {
            Boolean bRet;
            int vErrorCode = 0;

            int lDoorNumber;
            int lInfo;
            String strXML = "";

            lblMessage.Text = "Waiting...";
            Application.DoEvents();

            if (!bpc.EnableDevice(Program.gMachineNumber, 0))
            {
                lblMessage.Text = util.gstrNoDevice;
                return;
            }

            lDoorNumber = Convert.ToInt32(txtDoorNumber.Text);
            lInfo = 3;

            // make xml
            util.MakeXMLRequestHeader(bpc, ref strXML, "GetDoorParam");

            bpc.XML_AddInt(ref strXML, "DoorNo", lDoorNumber);
            bpc.XML_AddInt(ref strXML, "SubType", lInfo);

            bRet = bpc.GeneralOperationXML(ref strXML);

            if (bRet)
            {
                GCHandle gh = GCHandle.Alloc(DbOpenTimeArray, GCHandleType.Pinned);
                IntPtr AddrOfDbOpenTimeArray = gh.AddrOfPinnedObject();
                int nAddr = AddrOfDbOpenTimeArray.ToInt32();
                bRet = bpc.XML_ParseBinaryLong(ref strXML, "DoorOpenTimeBinary", ref nAddr, 4 * 8);
                if (bRet)
                {
                    DbDoorOpenTimeDraw();
                    lblMessage.Text = "GetDoorParam(3-DoorOpenTime) = Success!";
                }
                else
                    lblMessage.Text = "GetDoorParam(3-DoorOpenTime), XML Parse Error!";
            }
            else
            {
                bpc.GetLastError(ref vErrorCode);
                lblMessage.Text = util.ErrorPrint(vErrorCode);
            }
            bpc.EnableDevice(Program.gMachineNumber, 1);
        }

        private void gridUserGroupSet_ClickEvent(object sender, EventArgs e)
        {
            gridUserGroupSet_EnterCell(sender, new System.EventArgs());
        }

        private void gridUserGroupSet_Scroll(object sender, EventArgs e)
        {
            gridUserGroupSet_LeaveCell(sender, new System.EventArgs());
        }

        private void cmdUserAccessGroupRead_Click(object sender, EventArgs e)
        {
            Boolean bRet;
            int vErrorCode = 0;

            int lDoorNumber;
            int lInfo;
            String strXML = "";

            lblMessage.Text = "Waiting...";
            Application.DoEvents();

            if (!bpc.EnableDevice(Program.gMachineNumber, 0))
            {
                lblMessage.Text = util.gstrNoDevice;
                return;
            }

            lDoorNumber = Convert.ToInt32(txtDoorNumber.Text);
            lInfo = 5;

            // make xml
            util.MakeXMLRequestHeader(bpc, ref strXML, "GetDoorParam");

            bpc.XML_AddInt(ref strXML, "DoorNo", lDoorNumber);
            bpc.XML_AddInt(ref strXML, "SubType", lInfo);

            bRet = bpc.GeneralOperationXML(ref strXML);

            if (bRet)
            {
                GCHandle gh = GCHandle.Alloc(DbUserGroupArray, GCHandleType.Pinned);
                IntPtr AddrOfDbUserGroupArray = gh.AddrOfPinnedObject();
                int nAddr = AddrOfDbUserGroupArray.ToInt32();
                bRet = bpc.XML_ParseBinaryLong(ref strXML, "GroupBinary", ref nAddr, 10);
                if (bRet)
                {
                    DbUserGroupDraw();
                    lblMessage.Text = "GetDoorParam(5-UserGroup) = Success!";
                }
                else
                    lblMessage.Text = "GetDoorParam(5-UserGroup), XML Parse Error!";
            }
            else
            {
                bpc.GetLastError(ref vErrorCode);
                lblMessage.Text = util.ErrorPrint(vErrorCode);
            }
            bpc.EnableDevice(Program.gMachineNumber, 1);
        }

        private void cmdUserAccessGroupWrite_Click(object sender, EventArgs e)
        {
            Boolean bRet;
            int vErrorCode = 0;

            int lDoorNumber;
            int lInfo;
            String strXML = "";

            lblMessage.Text = "Waiting...";
            Application.DoEvents();

            if (!bpc.EnableDevice(Program.gMachineNumber, 0))
            {
                lblMessage.Text = util.gstrNoDevice;
                return;
            }

            lDoorNumber = Convert.ToInt32(txtDoorNumber.Text);
            lInfo = 5;

            // make xml
            util.MakeXMLRequestHeader(bpc, ref strXML, "SetDoorParam");

            bpc.XML_AddInt(ref strXML, "DoorNo", lDoorNumber);
            bpc.XML_AddInt(ref strXML, "SubType", lInfo);

            GCHandle gh = GCHandle.Alloc(DbUserGroupArray, GCHandleType.Pinned);
            IntPtr AddrOfDbUserGroupArray = gh.AddrOfPinnedObject();
            int nAddr = AddrOfDbUserGroupArray.ToInt32();
            bRet = bpc.XML_AddBinaryLong(ref strXML, "GroupBinary", ref nAddr, 10);

            bRet = bpc.GeneralOperationXML(ref strXML);

            if (bRet)
            {
                lblMessage.Text = "SetDoorParam(5-UserGroup) = Success!";
            }
            else
            {
                bpc.GetLastError(ref vErrorCode);
                lblMessage.Text = util.ErrorPrint(vErrorCode);
            }
            bpc.EnableDevice(Program.gMachineNumber, 1);
        }

        private void gridUnlockGroupSet_ClickEvent(object sender, EventArgs e)
        {
            gridUnlockGroupSet_EnterCell(sender, new System.EventArgs());
        }

        private void gridUnlockGroupSet_Scroll(object sender, EventArgs e)
        {
            gridUnlockGroupSet_LeaveCell(sender, new System.EventArgs());
        }

        private void cmdUnlockGroupRead_Click(object sender, EventArgs e)
        {
            Boolean bRet;
            int vErrorCode = 0;

            int lDoorNumber;
            int lInfo;
            String strXML = "";

            lblMessage.Text = "Waiting...";
            Application.DoEvents();

            if (!bpc.EnableDevice(Program.gMachineNumber, 0))
            {
                lblMessage.Text = util.gstrNoDevice;
                return;
            }

            lDoorNumber = Convert.ToInt32(txtDoorNumber.Text);
            lInfo = 6;

            // make xml
            util.MakeXMLRequestHeader(bpc, ref strXML, "GetDoorParam");

            bpc.XML_AddInt(ref strXML, "DoorNo", lDoorNumber);
            bpc.XML_AddInt(ref strXML, "SubType", lInfo);

            bRet = bpc.GeneralOperationXML(ref strXML);

            if (bRet)
            {
                GCHandle gh = GCHandle.Alloc(DbUnlockGroupArray, GCHandleType.Pinned);
                IntPtr AddrOfDbUnlockGroupArray = gh.AddrOfPinnedObject();
                int nAddr = AddrOfDbUnlockGroupArray.ToInt32();
                bRet = bpc.XML_ParseBinaryLong(ref strXML, "UnlockGroupBinary", ref nAddr, 3 * 5);
                if (bRet)
                {
                    DbUnlockGroupDraw();
                    lblMessage.Text = "GetDoorParam(6-UnlockGroup) = Success!";
                }
                else
                    lblMessage.Text = "GetDoorParam(6-UnlockGroup), XML Parse Error!";
            }
            else
            {
                bpc.GetLastError(ref vErrorCode);
                lblMessage.Text = util.ErrorPrint(vErrorCode);
            }
            bpc.EnableDevice(Program.gMachineNumber, 1);
        }

        private void cmdUnlockGroupWrite_Click(object sender, EventArgs e)
        {
            Boolean bRet;
            int vErrorCode = 0;

            int lDoorNumber;
            int lInfo;
            String strXML = "";

            lblMessage.Text = "Waiting...";
            Application.DoEvents();

            if (!bpc.EnableDevice(Program.gMachineNumber, 0))
            {
                lblMessage.Text = util.gstrNoDevice;
                return;
            }

            lDoorNumber = Convert.ToInt32(txtDoorNumber.Text);
            lInfo = 6;

            // make xml
            util.MakeXMLRequestHeader(bpc, ref strXML, "SetDoorParam");

            bpc.XML_AddInt(ref strXML, "DoorNo", lDoorNumber);
            bpc.XML_AddInt(ref strXML, "SubType", lInfo);

            GCHandle gh = GCHandle.Alloc(DbUnlockGroupArray, GCHandleType.Pinned);
            IntPtr AddrOfDbUnlockGroupArray = gh.AddrOfPinnedObject();
            int nAddr = AddrOfDbUnlockGroupArray.ToInt32();
            bRet = bpc.XML_AddBinaryLong(ref strXML, "UnlockGroupBinary", ref nAddr, 3 * 5);

            bRet = bpc.GeneralOperationXML(ref strXML);

            if (bRet)
            {
                lblMessage.Text = "SetDoorParam(4-UserMode) = Success!";
            }
            else
            {
                bpc.GetLastError(ref vErrorCode);
                lblMessage.Text = util.ErrorPrint(vErrorCode);
            }
            bpc.EnableDevice(Program.gMachineNumber, 1);
        }

        private void gridDoorOpenTime_EnterCell(object sender, EventArgs e)
        {
            int c, r;
            if (this.Visible && !bShowProcessing)
            {
                if (!timePicker.Visible)
                    timePicker.Visible = true;
                r = gridDoorOpenTime.Row - 1;
                c = gridDoorOpenTime.Col - 1;
                timePicker.Left = (int)Support.TwipsToPixelsX(Support.PixelsToTwipsX(gridDoorOpenTime.Left) + gridDoorOpenTime.CellLeft);
                timePicker.Top = (int)Support.TwipsToPixelsY(Support.PixelsToTwipsY(gridDoorOpenTime.Top) + gridDoorOpenTime.CellTop);
                if (c % 2 == 0)
                    timePicker.Value = new System.DateTime(2010, 1, 1, DbOpenTimeArray[r * 4 + 0], DbOpenTimeArray[r * 4 + 1], 0);
                else
                    timePicker.Value = new System.DateTime(2010, 1, 1, DbOpenTimeArray[r * 4 + 2], DbOpenTimeArray[r * 4 + 3], 0);
            }
        }

        private void gridDoorOpenTime_LeaveCell(object sender, EventArgs e)
        {
            int c, r;
            if (this.Visible && !bShowProcessing)
            {
                r = gridDoorOpenTime.Row - 1;
                c = gridDoorOpenTime.Col - 1;
                if (r < 0 || c < 0) return;
                if (c % 2 == 0)
                {
                    DbOpenTimeArray[r * 4 + 0] = (byte)timePicker.Value.Hour;
                    DbOpenTimeArray[r * 4 + 1] = (byte)timePicker.Value.Minute;
                }
                else
                {
                    DbOpenTimeArray[r * 4 + 2] = (byte)timePicker.Value.Hour;
                    DbOpenTimeArray[r * 4 + 3] = (byte)timePicker.Value.Minute;
                }
                gridDoorOpenTime.Text = timePicker.Value.ToString("HH:mm");
            }
        }

        private void gridUserGroupSet_EnterCell(object sender, EventArgs e)
        {
            int c, r;
            if (this.Visible && !bShowProcessing)
            {
                r = gridUserGroupSet.Row - 1;
                c = 1;
                if (!cmbGroup.Visible) cmbGroup.Visible = true;
                cmbGroup.Left = (int)Support.TwipsToPixelsX(Support.PixelsToTwipsX(gridUserGroupSet.Left) + gridUserGroupSet.CellLeft);
                cmbGroup.Top = (int)Support.TwipsToPixelsX(Support.PixelsToTwipsX(gridUserGroupSet.Top) + gridUserGroupSet.CellTop);
                cmbGroup.SelectedIndex = DbUserGroupArray[r];
            }
        }

        private void gridUserGroupSet_LeaveCell(object sender, EventArgs e)
        {
            int c, r;
            if (this.Visible && !bShowProcessing)
            {
                r = gridUserGroupSet.Row - 1;
                c = gridUserGroupSet.Col - 1;
                if (r < 0 || c < 0) return;
                DbUserGroupArray[r] = (byte)cmbGroup.SelectedIndex;
                gridUserGroupSet.Text = cmbGroup.Text;
            }
        }

        private void gridUnlockGroupSet_EnterCell(object sender, EventArgs e)
        {
            int c, r;
            if (this.Visible && !bShowProcessing)
            {
                r = gridUnlockGroupSet.Row - 1;
                c = gridUnlockGroupSet.Col - 1;
                if (!cmbUnlockGroup.Visible)
                    cmbUnlockGroup.Visible = true;
                cmbUnlockGroup.Left = (int)Support.TwipsToPixelsX(Support.PixelsToTwipsX(gridUnlockGroupSet.Left) + gridUnlockGroupSet.CellLeft);
                cmbUnlockGroup.Top = (int)Support.TwipsToPixelsY(Support.PixelsToTwipsY(gridUnlockGroupSet.Top) + gridUnlockGroupSet.CellTop);
                if (DbUnlockGroupArray[r * 3 + c] == 255)
                    cmbUnlockGroup.SelectedIndex = 11;
                else
                    cmbUnlockGroup.SelectedIndex = DbUnlockGroupArray[r * 3 + c];
            }
        }

        private void gridUnlockGroupSet_LeaveCell(object sender, EventArgs e)
        {
            int c, r;
            if (this.Visible && !bShowProcessing)
            {
                r = gridUnlockGroupSet.Row - 1;
                c = gridUnlockGroupSet.Col - 1;
                if (r < 0 || c < 0) return;
                if (cmbUnlockGroup.SelectedIndex == 11)
                    DbUnlockGroupArray[r * 3 + c] = 255;
                else
                    DbUnlockGroupArray[r * 3 + c] = (byte)cmbUnlockGroup.SelectedIndex;

                gridUnlockGroupSet.Text = cmbUnlockGroup.Text;
            }
        }

        private void cmdGet_Click(object sender, EventArgs e)
        {
            Boolean bRet;
            int vErrorCode = 0;

            int lDoorNumber;
            int lInfo;
            String strXML = "";

            lblMessage.Text = "Waiting...";
            Application.DoEvents();

            bRet = bpc.EnableDevice(Program.gMachineNumber, 0);
            if (!bRet)
            {
                lblMessage.Text = util.gstrNoDevice;
                return;
            }

            lDoorNumber = Convert.ToInt32(txtDoorNumber.Text);
            lInfo = 1;

            util.MakeXMLRequestHeader(bpc, ref strXML, "GetDoorParam");
            bpc.XML_AddInt(ref strXML, "DoorNo", lDoorNumber);
            bpc.XML_AddInt(ref strXML, "SubType", lInfo);

            bRet = bpc.GeneralOperationXML(ref strXML);

            if (bRet)
            {
                cmbSensorType.SelectedIndex = bpc.XML_ParseInt(ref strXML, "DoorSensorType");
                txtLockReleaseTime.Text = Convert.ToString(bpc.XML_ParseInt(ref strXML, "LockReleaseTime"));
                txtOpenTimeout.Text = Convert.ToString(bpc.XML_ParseInt(ref strXML, "DoorOpenTimeout"));
                cmbUseAntipass.SelectedIndex = 1 - bpc.XML_ParseInt(ref strXML, "UseAntipass");
                cmbAntipassNo.SelectedIndex = bpc.XML_ParseInt(ref strXML, "AntipassNo");
                cmbLocation.SelectedIndex = bpc.XML_ParseInt(ref strXML, "Locale");
                lblMessage.Text = "Success!";
            }
            else
            {
                bpc.GetLastError(ref vErrorCode);
                lblMessage.Text = util.ErrorPrint(vErrorCode);
            }
            bpc.EnableDevice(Program.gMachineNumber, 1);
        }

        private void cmdSet_Click(object sender, EventArgs e)
        {
            Boolean bRet;
            int vErrorCode = 0;

            int lDoorNumber;
            int lInfo;
            String strXML = "";

            lblMessage.Text = "Waiting...";
            Application.DoEvents();

            bRet = bpc.EnableDevice(Program.gMachineNumber, 0);
            if (!bRet)
            {
                lblMessage.Text = util.gstrNoDevice;
                return;
            }

            lDoorNumber = Convert.ToInt32(txtDoorNumber.Text);
            lInfo = 1;

            util.MakeXMLRequestHeader(bpc, ref strXML, "SetDoorParam");
            bpc.XML_AddInt(ref strXML, "DoorNo", lDoorNumber);
            bpc.XML_AddInt(ref strXML, "SubType", lInfo);

            bpc.XML_AddInt(ref strXML, "DoorSensorType", cmbSensorType.SelectedIndex);
            bpc.XML_AddInt(ref strXML, "LockReleaseTime", Convert.ToInt32(txtLockReleaseTime.Text));
            bpc.XML_AddInt(ref strXML, "DoorOpenTimeout", Convert.ToInt32(txtOpenTimeout.Text));
            bpc.XML_AddInt(ref strXML, "UseAntipass", 1 - cmbUseAntipass.SelectedIndex);
            bpc.XML_AddInt(ref strXML, "AntipassNo", cmbAntipassNo.SelectedIndex);
            bpc.XML_AddInt(ref strXML, "Locale", cmbLocation.SelectedIndex);

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
