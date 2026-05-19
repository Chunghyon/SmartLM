using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.Runtime.InteropServices;
using System.IO;

namespace SBXPCSampleCSharp
{
    public partial class frmLog : Form
    {
        public frmLog()
        {
            InitializeComponent();
        }

        AxSBXPCLib.AxSBXPC bpc;
        Object[] gstrLogItem;
        const int gMaxLow = 30000;

        private void cmdSLogData_Click(object sender, EventArgs e)
        {
            int vTMachineNumber = 0;
            int vSMachineNumber = 0;
            int vSEnrollNumber = 0;
            int vGEnrollNumber = 0;
            int vGMachineNumber = 0;
            int vManipulation = 0;
            int vFingerNumber = 0;
            int vYear = 0;
            int vMonth = 0;
            int vDay = 0;
            int vHour = 0;
            int vMinute = 0;
            int vSecond = 0;
            Boolean vRet;
            int vErrorCode = 0;
            int i;
            int n;

            lblMessage.Text = "Waiting...";
            LabelTotal.Text = "Total : ";
            Application.DoEvents();

            gridSLogData.Redraw = false;
            gridSLogData.Height = 298;
            gridSLogData.Clear();

            gstrLogItem = new Object[] { "", "TMNo", "SEnlNo", "SMNo", "GEnlNo", "GMNo", "Manipulation", "FpNo", "DateTime" };

            // gridSLogData
            gridSLogData.Cols = 9;
            gridSLogData.Row = 0;
            gridSLogData.set_ColWidth(0, 600);
            for (i = 1; i < 9; i++)
            {
                gridSLogData.Col = i;
                gridSLogData.Text = (string)gstrLogItem[i];
                gridSLogData.set_ColAlignment(i, 3);
                gridSLogData.set_ColWidth(i, 900);
            }
            gridSLogData.Col = 6;
            gridSLogData.set_ColWidth(6, 2000);
            gridSLogData.set_ColAlignment(6, 2);
            gridSLogData.set_ColWidth(7, 800);
            gridSLogData.Col = 8;
            gridSLogData.Text = (string)gstrLogItem[8];
            gridSLogData.set_ColWidth(8, 2000);
            n = gridSLogData.Rows;
            if (n > 2)
            {
                while (n != 2)
                {
                    gridSLogData.RemoveItem(n);
                    n--;
                }
            }
            gridSLogData.Redraw = true;

            Cursor = System.Windows.Forms.Cursors.WaitCursor;
            vRet = bpc.EnableDevice(Program.gMachineNumber, 0); // 0 : false
            if (!vRet)
            {
                lblMessage.Text = util.gstrNoDevice;
                Cursor = System.Windows.Forms.Cursors.Default;
                return;
            }

            vRet = bpc.ReadSuperLogData(Program.gMachineNumber);
            if (!vRet)
            {
                bpc.GetLastError(ref vErrorCode);
                lblMessage.Text = util.ErrorPrint(vErrorCode);
            }
            else
            {
                if (chkAndDelete.Checked)
                    bpc.EmptySuperLogData(Program.gMachineNumber);
            }

            if (vRet)
            {
                Cursor = System.Windows.Forms.Cursors.WaitCursor;
                lblMessage.Text = "Getting ...";
                Application.DoEvents();

                gridSLogData.Redraw = false;
                i = 1;
                while (true)
                {
                    vRet = bpc.GetSuperLogData( Program.gMachineNumber, 
                                                ref vTMachineNumber, 
                                                ref vSEnrollNumber, 
                                                ref vSMachineNumber, 
                                                ref vGEnrollNumber, 
                                                ref vGMachineNumber, 
                                                ref vManipulation, 
                                                ref vFingerNumber, 
                                                ref vYear, 
                                                ref vMonth, 
                                                ref vDay, 
                                                ref vHour, 
                                                ref vMinute,
                                                ref vSecond);
                    if (!vRet) break;
                    if (vRet && i != 1) gridSLogData.AddItem(Convert.ToString(1));

                    gridSLogData.Row = i;
                    gridSLogData.Col = 0;
                    gridSLogData.Text = Convert.ToString(i);
                    gridSLogData.Col = 1;
                    gridSLogData.Text = Convert.ToString(vTMachineNumber);
                    gridSLogData.Col = 2;
                    gridSLogData.Text = Convert.ToString(vSEnrollNumber);
                    gridSLogData.Col = 3;
                    gridSLogData.Text = Convert.ToString(vSMachineNumber);
                    gridSLogData.Col = 4;
                    gridSLogData.Text = Convert.ToString(vGEnrollNumber);
                    gridSLogData.Col = 5;
                    gridSLogData.Text = Convert.ToString(vGMachineNumber);
                    gridSLogData.Col = 6;
                    switch (vManipulation)
                    {
                        case 1:
                        case 2:
                        case 3:
                            gridSLogData.Text = Convert.ToString(vManipulation) + "--" + "Enroll user";
                            break;
                        case 4:
                            gridSLogData.Text = Convert.ToString(vManipulation) + "--" + "Enroll Manager";
                            break;
                        case 5:
                            gridSLogData.Text = Convert.ToString(vManipulation) + "--" + "Delete Fp Data";
                            break;
                        case 6:
                            gridSLogData.Text = Convert.ToString(vManipulation) + "--" + "Delete Password";
                            break;
                        case 7:
                            gridSLogData.Text = Convert.ToString(vManipulation) + "--" + "Delete Card Data";
                            break;
                        case 8:
                            gridSLogData.Text = Convert.ToString(vManipulation) + "--" + "Delete All LogData";
                            break;
                        case 9:
                            gridSLogData.Text = Convert.ToString(vManipulation) + "--" + "Modify System Info";
                            break;
                        case 10:
                            gridSLogData.Text = Convert.ToString(vManipulation) + "--" + "Modify System Time";
                            break;
                        case 11:
                            gridSLogData.Text = Convert.ToString(vManipulation) + "--" + "Modify Log Setting";
                            break;
                        case 12:
                            gridSLogData.Text = Convert.ToString(vManipulation) + "--" + "Modify Comm Setting";
                            break;
                        case 13:
                            gridSLogData.Text = Convert.ToString(vManipulation) + "--" + "Modify Timezone Setting";
                            break;
                        case 14:
                            gridSLogData.Text = Convert.ToString(vManipulation) + "--" + "Delete User";
                            break;
                        case 15:
                            gridSLogData.Text = Convert.ToString(vManipulation) + "--" + "Delete All User";
                            break;
                        case 16:
                            gridSLogData.Text = Convert.ToString(vManipulation) + "--" + "Restore";
                            break;
                        case 17:
                            gridSLogData.Text = Convert.ToString(vManipulation) + "--" + "Modify User Period";
                            break;
                        default:
                            gridSLogData.Text = Convert.ToString(vManipulation) + "--" + "Unknown";
                            break;
                    }
                    gridSLogData.Col = 7;
                    if (vFingerNumber < 10)
                        gridSLogData.Text = Convert.ToString(vFingerNumber);
                    else if (vFingerNumber == 10)
                        gridSLogData.Text = "Password";
                    else
                        gridSLogData.Text = "Card";
                    gridSLogData.Col = 8;
                    gridSLogData.Text = Convert.ToString(vYear) + "/" + String.Format("{0:D2}", vMonth) + "/" + String.Format("{0:D2}", vDay) + " " + String.Format("{0:D2}", vHour) + ":" + String.Format("{0:D2}", vMinute);
                    LabelTotal.Text = "Total : " + Convert.ToString(i);
                    Application.DoEvents();
                    i = i + 1;
                }
                gridSLogData.Redraw = true;
                lblMessage.Text = "ReadSuperLogData OK";
            }

            Cursor = System.Windows.Forms.Cursors.Default;
            bpc.EnableDevice(Program.gMachineNumber, 1); // 1 : true
        }

        private void cmdAllSLogData_Click(object sender, EventArgs e)
        {
            int vTMachineNumber = 0;
            int vSMachineNumber = 0;
            int vSEnrollNumber = 0;
            int vGEnrollNumber = 0;
            int vGMachineNumber = 0;
            int vManipulation = 0;
            int vFingerNumber = 0;
            int vYear = 0;
            int vMonth = 0;
            int vDay = 0;
            int vHour = 0;
            int vMinute = 0;
            int vSecond = 0;
            Boolean vRet;
            int vErrorCode = 0;
            int i;
            int n;

            lblMessage.Text = "Waiting...";
            LabelTotal.Text = "Total : ";
            Application.DoEvents();

            gridSLogData.Redraw = false;
            gridSLogData.Height = 298;
            gridSLogData.Clear();

            gstrLogItem = new Object[] { "", "TMNo", "SEnlNo", "SMNo", "GEnlNo", "GMNo", "Manipulation", "FpNo", "DateTime" };

            // gridSLogData
            gridSLogData.Cols = 9;
            gridSLogData.Row = 0;
            gridSLogData.set_ColWidth(0, 600);
            for (i = 1; i < 9; i++)
            {
                gridSLogData.Col = i;
                gridSLogData.Text = (string)gstrLogItem[i];
                gridSLogData.set_ColAlignment(i, 3);
                gridSLogData.set_ColWidth(i, 900);
            }
            gridSLogData.Col = 6;
            gridSLogData.set_ColWidth(6, 2000);
            gridSLogData.set_ColAlignment(6, 2);
            gridSLogData.set_ColWidth(7, 800);
            gridSLogData.Col = 8;
            gridSLogData.Text = (string)gstrLogItem[8];
            gridSLogData.set_ColWidth(8, 2000);
            n = gridSLogData.Rows;
            if (n > 2)
            {
                while (n != 2)
                {
                    gridSLogData.RemoveItem(n);
                    n--;
                }
            }
            gridSLogData.Redraw = true;

            Cursor = System.Windows.Forms.Cursors.WaitCursor;
            vRet = bpc.EnableDevice(Program.gMachineNumber, 0); // 0 : false
            if (!vRet)
            {
                lblMessage.Text = util.gstrNoDevice;
                Cursor = System.Windows.Forms.Cursors.Default;
                return;
            }

            vRet = bpc.ReadAllSLogData(Program.gMachineNumber);
            if (!vRet)
            {
                bpc.GetLastError(ref vErrorCode);
                lblMessage.Text = util.ErrorPrint(vErrorCode);
            }
            else
            {
                if (chkAndDelete.Checked)
                    bpc.EmptySuperLogData(Program.gMachineNumber);
            }

            if (vRet)
            {
                Cursor = System.Windows.Forms.Cursors.WaitCursor;
                lblMessage.Text = "Getting ...";
                Application.DoEvents();

                gridSLogData.Redraw = false;
                i = 1;
                while (true)
                {
                    vRet = bpc.GetAllSLogData(  Program.gMachineNumber, 
                                                ref vTMachineNumber, 
                                                ref vSEnrollNumber, 
                                                ref vSMachineNumber, 
                                                ref vGEnrollNumber, 
                                                ref vGMachineNumber, 
                                                ref vManipulation, 
                                                ref vFingerNumber, 
                                                ref vYear, 
                                                ref vMonth, 
                                                ref vDay, 
                                                ref vHour, 
                                                ref vMinute,
                                                ref vSecond);
                    if (!vRet) break;
                    if (vRet && i != 1) gridSLogData.AddItem(Convert.ToString(1));

                    gridSLogData.Row = i;
                    gridSLogData.Col = 0;
                    gridSLogData.Text = Convert.ToString(i);
                    gridSLogData.Col = 1;
                    gridSLogData.Text = Convert.ToString(vTMachineNumber);
                    gridSLogData.Col = 2;
                    gridSLogData.Text = Convert.ToString(vSEnrollNumber);
                    gridSLogData.Col = 3;
                    gridSLogData.Text = Convert.ToString(vSMachineNumber);
                    gridSLogData.Col = 4;
                    gridSLogData.Text = Convert.ToString(vGEnrollNumber);
                    gridSLogData.Col = 5;
                    gridSLogData.Text = Convert.ToString(vGMachineNumber);
                    gridSLogData.Col = 6;
                    switch (vManipulation)
                    {
                        case 1:
                        case 2:
                        case 3:
                            gridSLogData.Text = Convert.ToString(vManipulation) + "--" + "Enroll user";
                            break;
                        case 4:
                            gridSLogData.Text = Convert.ToString(vManipulation) + "--" + "Enroll Manager";
                            break;
                        case 5:
                            gridSLogData.Text = Convert.ToString(vManipulation) + "--" + "Delete Fp Data";
                            break;
                        case 6:
                            gridSLogData.Text = Convert.ToString(vManipulation) + "--" + "Delete Password";
                            break;
                        case 7:
                            gridSLogData.Text = Convert.ToString(vManipulation) + "--" + "Delete Card Data";
                            break;
                        case 8:
                            gridSLogData.Text = Convert.ToString(vManipulation) + "--" + "Delete All LogData";
                            break;
                        case 9:
                            gridSLogData.Text = Convert.ToString(vManipulation) + "--" + "Modify System Info";
                            break;
                        case 10:
                            gridSLogData.Text = Convert.ToString(vManipulation) + "--" + "Modify System Time";
                            break;
                        case 11:
                            gridSLogData.Text = Convert.ToString(vManipulation) + "--" + "Modify Log Setting";
                            break;
                        case 12:
                            gridSLogData.Text = Convert.ToString(vManipulation) + "--" + "Modify Comm Setting";
                            break;
                        case 13:
                            gridSLogData.Text = Convert.ToString(vManipulation) + "--" + "Modify Timezone Setting";
                            break;
                        case 14:
                            gridSLogData.Text = Convert.ToString(vManipulation) + "--" + "Delete User";
                            break;
                        case 15:
                            gridSLogData.Text = Convert.ToString(vManipulation) + "--" + "Delete All User";
                            break;
                        case 16:
                            gridSLogData.Text = Convert.ToString(vManipulation) + "--" + "Restore";
                            break;
                        case 17:
                            gridSLogData.Text = Convert.ToString(vManipulation) + "--" + "Modify User Period";
                            break;
                        default:
                            gridSLogData.Text = Convert.ToString(vManipulation) + "--" + "Unknown";
                            break;
                    }
                    gridSLogData.Col = 7;
                    if (vFingerNumber < 10)
                        gridSLogData.Text = Convert.ToString(vFingerNumber);
                    else if (vFingerNumber == 10)
                        gridSLogData.Text = "Password";
                    else
                        gridSLogData.Text = "Card";
                    gridSLogData.Col = 8;
                    gridSLogData.Text = Convert.ToString(vYear) + "/" + String.Format("{0:D2}", vMonth) + "/" + String.Format("{0:D2}", vDay) + " " + String.Format("{0:D2}", vHour) + ":" + String.Format("{0:D2}", vMinute);
                    LabelTotal.Text = "Total : " + Convert.ToString(i);
                    Application.DoEvents();
                    i = i + 1;
                }
                gridSLogData.Redraw = true;
                lblMessage.Text = "ReadAllSLogData OK";
            }

            Cursor = System.Windows.Forms.Cursors.Default;
            bpc.EnableDevice(Program.gMachineNumber, 1); // 1 : true
        }

        private void cmdEmptySLog_Click(object sender, EventArgs e)
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

            vRet = bpc.EmptySuperLogData(Program.gMachineNumber);
            if (vRet)
            {
                lblMessage.Text = "EmptySuperLogData OK";
            }
            else
            {
                bpc.GetLastError(ref vErrorCode);
                lblMessage.Text = util.ErrorPrint(vErrorCode);
            }

            bpc.EnableDevice(Program.gMachineNumber, 1); // 1 : true
        }

        private void cmdGlogData_Click(object sender, EventArgs e)
        {
            int vTMachineNumber = 0;
            int vSMachineNumber = 0;
            int vSEnrollNumber = 0;
            //            int vInOutMode = 0;
            int vVerifyMode = 0;
            int vYear = 0;
            int vMonth = 0;
            int vDay = 0;
            int vHour = 0;
            int vMinute = 0;
            int vSecond = 0;
            Boolean vRet;
            int vErrorCode = 0;
            int i;
            int n;
            int vMaxLogCnt;
            int vAntipass;
            int vDoor;
            string stAntipass, stDoor;
            int vDiv;

            vMaxLogCnt = gMaxLow;
            vDiv = 65536;

            lblMessage.Text = "Waiting...";
            LabelTotal.Text = "Total : ";
            Application.DoEvents();

            gridSLogData.Redraw = false;
            gridSLogData.Height = 298;
            gridSLogData.Clear();
            gridSLogData1.Top = gridSLogData.Top + gridSLogData.Height;
            gridSLogData1.Height = 0;
            gridSLogData1.Redraw = false;
            gridSLogData1.Clear();
            gridSLogData2.Top = gridSLogData.Top + gridSLogData.Height;
            gridSLogData2.Height = 0;
            gridSLogData2.Redraw = false;
            gridSLogData2.Clear();

            gstrLogItem = new Object[] { "", "TMachineNo", "EnrollNo", "EMachineNo", "Door", "VeriMode", "DateTime" };

            // gridSLogData
            gridSLogData.Row = 0;
            gridSLogData.Cols = 9;
            gridSLogData.set_ColWidth(0, 600);
            for (i = 1; i < 7; i++)
            {
                gridSLogData.Col = i;
                gridSLogData.Text = (string)gstrLogItem[i];
                gridSLogData.set_ColAlignment(i, 3);
                gridSLogData.set_ColWidth(i, 1200);
            }
            gridSLogData.set_ColWidth(5, 2000);
            gridSLogData.Col = 6;
            gridSLogData.set_ColWidth(6, 2000);
            gridSLogData.set_ColWidth(7, 700);
            gridSLogData.set_ColWidth(8, 700);
            n = gridSLogData.Rows;
            if (n > 2)
            {
                while (n != 2)
                {
                    gridSLogData.RemoveItem((n));
                    n = n - 1;
                }
            }
            gridSLogData.Redraw = true;

            // gridSLogData1
            gridSLogData1.Row = 0;
            gridSLogData1.Cols = 9;
            gridSLogData1.set_ColWidth(0, 600);
            for (i = 1; i < 7; i++)
            {
                gridSLogData1.Col = i;
                gridSLogData1.Text = (string)gstrLogItem[i];
                gridSLogData1.set_ColAlignment(i, 3);
                gridSLogData1.set_ColWidth(i, 1200);
            }
            gridSLogData1.set_ColWidth(5, 2000);
            gridSLogData1.Col = 6;
            gridSLogData1.set_ColWidth(6, 2000);
            gridSLogData1.set_ColWidth(7, 700);
            gridSLogData1.set_ColWidth(8, 700);
            n = gridSLogData1.Rows;
            if (n > 2)
            {
                while (n != 2)
                {
                    gridSLogData1.RemoveItem((n));
                    n = n - 1;
                }
            }
            gridSLogData1.Redraw = true;

            // gridSLogData2
            gridSLogData2.Row = 0;
            gridSLogData2.Cols = 9;
            gridSLogData2.set_ColWidth(0, 600);
            for (i = 1; i < 7; i++)
            {
                gridSLogData2.Col = i;
                gridSLogData2.Text = (string)gstrLogItem[i];
                gridSLogData2.set_ColAlignment(i, 3);
                gridSLogData2.set_ColWidth(i, 1200);
            }
            gridSLogData2.set_ColWidth(5, 2000);
            gridSLogData2.Col = 6;
            gridSLogData2.set_ColWidth(6, 2000);
            gridSLogData2.set_ColWidth(7, 700);
            gridSLogData2.set_ColWidth(8, 700);
            n = gridSLogData2.Rows;
            if (n > 2)
            {
                while (n != 2)
                {
                    gridSLogData2.RemoveItem((n));
                    n = n - 1;
                }
            }
            gridSLogData2.Redraw = true;

            Cursor = System.Windows.Forms.Cursors.WaitCursor;
            vRet = bpc.EnableDevice(Program.gMachineNumber, 0); // 0 : false
            if (!vRet)
            {
                lblMessage.Text = util.gstrNoDevice;
                Cursor = System.Windows.Forms.Cursors.Default;
                return;
            }

            vRet = bpc.ReadGeneralLogData(Program.gMachineNumber);
            if (!vRet)
            {
                bpc.GetLastError(ref vErrorCode);
                lblMessage.Text = util.ErrorPrint(vErrorCode);
            }
            else
            {
                if (chkAndDelete.Checked)
                    bpc.EmptyGeneralLogData(Program.gMachineNumber);
            }
            if (vRet)
            {
                Cursor = System.Windows.Forms.Cursors.WaitCursor;
                lblMessage.Text = "Getting ...";
                Application.DoEvents();
                gridSLogData.Redraw = false;
                gridSLogData1.Redraw = false;
                gridSLogData2.Redraw = false;

                i = 1;
                while (true)
                {
                    vRet = bpc.GetGeneralLogData(   Program.gMachineNumber, 
                                                    ref vTMachineNumber, 
                                                    ref vSEnrollNumber, 
                                                    ref vSMachineNumber, 
                                                    ref vVerifyMode, 
                                                    ref vYear, 
                                                    ref vMonth, 
                                                    ref vDay, 
                                                    ref vHour, 
                                                    ref vMinute,
                                                    ref vSecond);
                    if (!vRet) break;
                    if (vRet && i != 1) gridSLogData.AddItem(Convert.ToString(1));

                    vAntipass = vVerifyMode / vDiv;
                    vVerifyMode = vVerifyMode % vDiv;
                    vDoor = vVerifyMode / 256;
                    vVerifyMode = vVerifyMode % 256;
                    stAntipass = "";
                    stDoor = Convert.ToString(vDoor);

                    if (vAntipass == 1)
                        stAntipass = "(AP_In)";
                    else if (vAntipass == 3)
                        stAntipass = "(AP_Out)";

                    gridSLogData.Row = i;
                    gridSLogData.Col = 0;
                    gridSLogData.Text = Convert.ToString(i);
                    gridSLogData.Col = 1;
                    gridSLogData.Text = Convert.ToString(vTMachineNumber);
                    gridSLogData.Col = 2;
                    gridSLogData.Text = Convert.ToString(vSEnrollNumber);
                    gridSLogData.Col = 3;
                    gridSLogData.Text = Convert.ToString(vSMachineNumber);
                    gridSLogData.Col = 4;
                    gridSLogData.Text = stDoor;
                    gridSLogData.Col = 5;
                    if (vVerifyMode == 1)
                        gridSLogData.Text = "Fp";
                    else if (vVerifyMode == 2)
                        gridSLogData.Text = "Password";
                    else if (vVerifyMode == 3)
                        gridSLogData.Text = "Card";
                    else if (vVerifyMode == 4)
                        gridSLogData.Text = "FP+Card";
                    else if (vVerifyMode == 5)
                        gridSLogData.Text = "FP+Pwd";
                    else if (vVerifyMode == 6)
                        gridSLogData.Text = "Card+Pwd";
                    else if (vVerifyMode == 7)
                        gridSLogData.Text = "FP+Card+Pwd";
                    else if (vVerifyMode == 10)
                        gridSLogData.Text = "Hand Lock";
                    else if (vVerifyMode == 11)
                        gridSLogData.Text = "Prog Lock";
                    else if (vVerifyMode == 12)
                        gridSLogData.Text = "Prog Open";
                    else if (vVerifyMode == 13)
                        gridSLogData.Text = "Prog Close";
                    else if (vVerifyMode == 14)
                        gridSLogData.Text = "Auto Recover";
                    else if (vVerifyMode == 20)
                        gridSLogData.Text = "Lock Over";
                    else if (vVerifyMode == 21)
                        gridSLogData.Text = "Illegal Open";
                    else if (vVerifyMode == 22)
                        gridSLogData.Text = "Duress alarm";
                    else if (vVerifyMode == 23)
                        gridSLogData.Text = "Tamper detect";
                    else
                        gridSLogData.Text = "--";

                    gridSLogData.Text = gridSLogData.Text + stAntipass;
                    gridSLogData.Col = 6;
                    gridSLogData.Text = Convert.ToString(vYear) + "/" + String.Format("{0:D2}", vMonth) + "/" + String.Format("{0:D2}", vDay) + " " + String.Format("{0:D2}", vHour) + ":" + String.Format("{0:D2}", vMinute);

                    LabelTotal.Text = "Total : " + Convert.ToString(i);
                    Application.DoEvents();
                    i = i + 1;
                    if (i > vMaxLogCnt) break;

                }

                // gridSLogData1
                if (i > vMaxLogCnt)
                {
                    gridSLogData.Height = gridSLogData.Height / 2;
                    gridSLogData1.Top = gridSLogData.Top + gridSLogData.Height;
                    gridSLogData1.Height = gridSLogData.Height;

                    while (true)
                    {
                        vRet = bpc.GetGeneralLogData(   Program.gMachineNumber, 
                                                        ref vTMachineNumber, 
                                                        ref vSEnrollNumber, 
                                                        ref vSMachineNumber, 
                                                        ref vVerifyMode, 
                                                        ref vYear, 
                                                        ref vMonth, 
                                                        ref vDay, 
                                                        ref vHour, 
                                                        ref vMinute,
                                                        ref vSecond);
                        if (!vRet) break;
                        if (vRet && i != 1)
                            if (i - vMaxLogCnt > 1)
                                gridSLogData1.AddItem(Convert.ToString(1));

                        vAntipass = vVerifyMode / vDiv;
                        vVerifyMode = vVerifyMode % vDiv;
                        vDoor = vVerifyMode / 256;
                        vVerifyMode = vVerifyMode % 256;
                        stAntipass = "";
                        stDoor = Convert.ToString(vDoor);

                        if (vAntipass == 1)
                            stAntipass = "(AP_In)";
                        else if (vAntipass == 3)
                            stAntipass = "(AP_Out)";

                        gridSLogData1.Row = i - vMaxLogCnt;
                        gridSLogData1.Col = 0;
                        gridSLogData1.Text = Convert.ToString(i);
                        gridSLogData1.Col = 1;
                        gridSLogData1.Text = Convert.ToString(vTMachineNumber);
                        gridSLogData1.Col = 2;
                        gridSLogData1.Text = Convert.ToString(vSEnrollNumber);
                        gridSLogData1.Col = 3;
                        gridSLogData1.Text = Convert.ToString(vSMachineNumber);
                        gridSLogData1.Col = 4;
                        gridSLogData1.Text = stDoor;
                        gridSLogData1.Col = 5;
                        if (vVerifyMode == 1)
                            gridSLogData1.Text = "Fp";
                        else if (vVerifyMode == 2)
                            gridSLogData1.Text = "Password";
                        else if (vVerifyMode == 3)
                            gridSLogData1.Text = "Card";
                        else if (vVerifyMode == 4)
                            gridSLogData1.Text = "FP+Card";
                        else if (vVerifyMode == 5)
                            gridSLogData1.Text = "FP+Pwd";
                        else if (vVerifyMode == 6)
                            gridSLogData1.Text = "Card+Pwd";
                        else if (vVerifyMode == 7)
                            gridSLogData1.Text = "FP+Card+Pwd";
                        else if (vVerifyMode == 10)
                            gridSLogData1.Text = "Hand Lock";
                        else if (vVerifyMode == 11)
                            gridSLogData1.Text = "Prog Lock";
                        else if (vVerifyMode == 12)
                            gridSLogData1.Text = "Prog Open";
                        else if (vVerifyMode == 13)
                            gridSLogData1.Text = "Prog Close";
                        else if (vVerifyMode == 14)
                            gridSLogData1.Text = "Auto Recover";
                        else if (vVerifyMode == 20)
                            gridSLogData1.Text = "Lock Over";
                        else if (vVerifyMode == 21)
                            gridSLogData1.Text = "Illegal Open";
                        else if (vVerifyMode == 22)
                            gridSLogData1.Text = "Duress alarm";
                        else if (vVerifyMode == 23)
                            gridSLogData1.Text = "Tamper detect";
                        else
                            gridSLogData1.Text = "--";

                        gridSLogData1.Text = gridSLogData1.Text + stAntipass;
                        gridSLogData1.Col = 6;
                        gridSLogData1.Text = Convert.ToString(vYear) + "/" + String.Format("{0:D2}", vMonth) + "/" + String.Format("{0:D2}", vDay) + " " + String.Format("{0:D2}", vHour) + ":" + String.Format("{0:D2}", vMinute);

                        LabelTotal.Text = "Total : " + Convert.ToString(i);
                        Application.DoEvents();
                        i = i + 1;
                        if (i > vMaxLogCnt * 2) break;

                    }
                }

                // gridSLogData2
                vMaxLogCnt = vMaxLogCnt * 2;
                if (i > vMaxLogCnt)
                {
                    gridSLogData.Height = gridSLogData.Height * 2 / 3;
                    gridSLogData1.Top = gridSLogData.Top + gridSLogData.Height;
                    gridSLogData1.Height = gridSLogData.Height;
                    gridSLogData2.Top = gridSLogData.Top + gridSLogData.Height * 2;
                    gridSLogData2.Height = gridSLogData.Height;

                    while (true)
                    {
                        vRet = bpc.GetGeneralLogData(Program.gMachineNumber, 
                                                        ref vTMachineNumber, 
                                                        ref vSEnrollNumber, 
                                                        ref vSMachineNumber, 
                                                        ref vVerifyMode, 
                                                        ref vYear, 
                                                        ref vMonth, 
                                                        ref vDay, 
                                                        ref vHour, 
                                                        ref vMinute,
                                                        ref vSecond);
                        if (!vRet) break;
                        if (vRet && i != 1)
                            if (i - vMaxLogCnt > 1)
                                gridSLogData2.AddItem(Convert.ToString(1));

                        vAntipass = vVerifyMode / vDiv;
                        vVerifyMode = vVerifyMode % vDiv;
                        vDoor = vVerifyMode / 256;
                        vVerifyMode = vVerifyMode % 256;
                        stAntipass = "";
                        stDoor = Convert.ToString(vDoor);

                        if (vAntipass == 1)
                            stAntipass = "(AP_In)";
                        else if (vAntipass == 3)
                            stAntipass = "(AP_Out)";

                        gridSLogData2.Row = i - vMaxLogCnt;
                        gridSLogData2.Col = 0;
                        gridSLogData2.Text = Convert.ToString(i);
                        gridSLogData2.Col = 1;
                        gridSLogData2.Text = Convert.ToString(vTMachineNumber);
                        gridSLogData2.Col = 2;
                        gridSLogData2.Text = Convert.ToString(vSEnrollNumber);
                        gridSLogData2.Col = 3;
                        gridSLogData2.Text = Convert.ToString(vSMachineNumber);
                        gridSLogData2.Col = 4;
                        gridSLogData2.Text = stDoor;
                        gridSLogData2.Col = 5;
                        if (vVerifyMode == 1)
                            gridSLogData2.Text = "Fp";
                        else if (vVerifyMode == 2)
                            gridSLogData2.Text = "Password";
                        else if (vVerifyMode == 3)
                            gridSLogData2.Text = "Card";
                        else if (vVerifyMode == 4)
                            gridSLogData2.Text = "FP+Card";
                        else if (vVerifyMode == 5)
                            gridSLogData2.Text = "FP+Pwd";
                        else if (vVerifyMode == 6)
                            gridSLogData2.Text = "Card+Pwd";
                        else if (vVerifyMode == 7)
                            gridSLogData2.Text = "FP+Card+Pwd";
                        else if (vVerifyMode == 10)
                            gridSLogData2.Text = "Hand Lock";
                        else if (vVerifyMode == 11)
                            gridSLogData2.Text = "Prog Lock";
                        else if (vVerifyMode == 12)
                            gridSLogData2.Text = "Prog Open";
                        else if (vVerifyMode == 13)
                            gridSLogData2.Text = "Prog Close";
                        else if (vVerifyMode == 14)
                            gridSLogData2.Text = "Auto Recover";
                        else if (vVerifyMode == 20)
                            gridSLogData2.Text = "Lock Over";
                        else if (vVerifyMode == 21)
                            gridSLogData2.Text = "Illegal Open";
                        else if (vVerifyMode == 22)
                            gridSLogData2.Text = "Duress alarm";
                        else if (vVerifyMode == 23)
                            gridSLogData2.Text = "Tamper detect";
                        else
                            gridSLogData2.Text = "--";

                        gridSLogData2.Text = gridSLogData2.Text + stAntipass;
                        gridSLogData2.Col = 6;
                        gridSLogData2.Text = Convert.ToString(vYear) + "/" + String.Format("{0:D2}", vMonth) + "/" + String.Format("{0:D2}", vDay) + " " + String.Format("{0:D2}", vHour) + ":" + String.Format("{0:D2}", vMinute);

                        LabelTotal.Text = "Total : " + Convert.ToString(i);
                        Application.DoEvents();
                        i = i + 1;
                        if (i > gMaxLow * 3) break;
                    }
                }
                gridSLogData.Redraw = true;
                gridSLogData1.Redraw = true;
                gridSLogData2.Redraw = true;

                lblMessage.Text = "ReadGeneralLogData OK";
            }

            Cursor = System.Windows.Forms.Cursors.Default;
            bpc.EnableDevice(Program.gMachineNumber, 1); // 1 : true
        }

        private void cmdAllGLogData_Click(object sender, EventArgs e)
        {
            int vTMachineNumber = 0;
            int vSMachineNumber = 0;
            int vSEnrollNumber = 0;
            int vVerifyMode = 0;
            int vYear = 0;
            int vMonth = 0;
            int vDay = 0;
            int vHour = 0;
            int vMinute = 0;
            int vSecond = 0;
            Boolean vRet;
            int vErrorCode = 0;
            int i;
            int n;
            int vMaxLogCnt;

            vMaxLogCnt = gMaxLow;

            lblMessage.Text = "Waiting...";
            LabelTotal.Text = "Total : ";

            gridSLogData.Redraw = false;
            gridSLogData.Height = 298;
            gridSLogData.Clear();
            gridSLogData1.Top = gridSLogData.Top + gridSLogData.Height;
            gridSLogData1.Height = 0;
            gridSLogData1.Redraw = false;
            gridSLogData1.Clear();
            gridSLogData2.Top = gridSLogData.Top + gridSLogData.Height;
            gridSLogData2.Height = 0;
            gridSLogData2.Redraw = false;
            gridSLogData2.Clear();

            gstrLogItem = new Object[] { "", "TMachineNo", "EnrollNo", "EMachineNo", "Door", "VeriMode", "DateTime" };

            // gridSLogData
            gridSLogData.Row = 0;
            gridSLogData.Cols = 9;
            gridSLogData.set_ColWidth(0, 600);
            for (i = 1; i < 7; i++)
            {
                gridSLogData.Col = i;
                gridSLogData.Text = (string)gstrLogItem[i];
                gridSLogData.set_ColAlignment(i, 3);
                gridSLogData.set_ColWidth(i, 1200);
            }
            gridSLogData.set_ColWidth(5, 2000);
            gridSLogData.Col = 6;
            gridSLogData.set_ColWidth(6, 2000);
            gridSLogData.set_ColWidth(7, 700);
            gridSLogData.set_ColWidth(8, 700);
            n = gridSLogData.Rows;
            if (n > 2)
            {
                while (n != 2)
                {
                    gridSLogData.RemoveItem((n));
                    n = n - 1;
                }
            }
            gridSLogData.Redraw = true;

            // gridSLogData1
            gridSLogData1.Row = 0;
            gridSLogData1.Cols = 9;
            gridSLogData1.set_ColWidth(0, 600);
            for (i = 1; i < 7; i++)
            {
                gridSLogData1.Col = i;
                gridSLogData1.Text = (string)gstrLogItem[i];
                gridSLogData1.set_ColAlignment(i, 3);
                gridSLogData1.set_ColWidth(i, 1200);
            }
            gridSLogData1.set_ColWidth(5, 2000);
            gridSLogData1.Col = 6;
            gridSLogData1.set_ColWidth(6, 2000);
            gridSLogData1.set_ColWidth(7, 700);
            gridSLogData1.set_ColWidth(8, 700);
            n = gridSLogData1.Rows;
            if (n > 2)
            {
                while (n != 2)
                {
                    gridSLogData1.RemoveItem((n));
                    n = n - 1;
                }
            }
            gridSLogData1.Redraw = true;

            // gridSLogData2
            gridSLogData2.Row = 0;
            gridSLogData2.Cols = 9;
            gridSLogData2.set_ColWidth(0, 600);
            for (i = 1; i < 7; i++)
            {
                gridSLogData2.Col = i;
                gridSLogData2.Text = (string)gstrLogItem[i];
                gridSLogData2.set_ColAlignment(i, 3);
                gridSLogData2.set_ColWidth(i, 1200);
            }
            gridSLogData2.set_ColWidth(5, 2000);
            gridSLogData2.Col = 6;
            gridSLogData2.set_ColWidth(6, 2000);
            gridSLogData2.set_ColWidth(7, 700);
            gridSLogData2.set_ColWidth(8, 700);
            n = gridSLogData2.Rows;
            if (n > 2)
            {
                while (n != 2)
                {
                    gridSLogData2.RemoveItem((n));
                    n = n - 1;
                }
            }
            gridSLogData2.Redraw = true;

            Cursor = System.Windows.Forms.Cursors.WaitCursor;
            vRet = bpc.EnableDevice(Program.gMachineNumber, 0); // 0 : false
            if (!vRet)
            {
                lblMessage.Text = util.gstrNoDevice;
                Cursor = System.Windows.Forms.Cursors.Default;
                return;
            }

            vRet = bpc.ReadAllGLogData(Program.gMachineNumber);
            if (!vRet)
            {
                bpc.GetLastError(ref vErrorCode);
                lblMessage.Text = util.ErrorPrint(vErrorCode);
            }
            else
            {
                if (chkAndDelete.Checked)
                    bpc.EmptyGeneralLogData(Program.gMachineNumber);
            }
            if (vRet)
            {
                Cursor = System.Windows.Forms.Cursors.WaitCursor;
                lblMessage.Text = "Getting ...";
                Application.DoEvents();
                gridSLogData.Redraw = false;
                gridSLogData1.Redraw = false;
                gridSLogData2.Redraw = false;

                i = 1;
                while (true)
                {
                    vRet = bpc.GetAllGLogData(  Program.gMachineNumber, 
                                                ref vTMachineNumber, 
                                                ref vSEnrollNumber, 
                                                ref vSMachineNumber, 
                                                ref vVerifyMode, 
                                                ref vYear, 
                                                ref vMonth, 
                                                ref vDay, 
                                                ref vHour, 
                                                ref vMinute,
                                                ref vSecond);
                    if (!vRet) break;
                    if (vRet && i != 1) gridSLogData.AddItem(Convert.ToString(1));

                    AddGLogItem(vTMachineNumber, 
                                vSEnrollNumber, 
                                vSMachineNumber, 
                                vVerifyMode, 
                                vYear, 
                                vMonth, 
                                vDay, 
                                vHour, 
                                vMinute, 
                                vSecond, 
                                i,
                                0,
                                gridSLogData
                               );

                    LabelTotal.Text = "Total : " + Convert.ToString(i);
                    Application.DoEvents();
                    i = i + 1;
                    if (i > vMaxLogCnt) break;

                }

                if (i > vMaxLogCnt)
                {
                    gridSLogData.Height = gridSLogData.Height / 2;
                    gridSLogData1.Top = gridSLogData.Top + gridSLogData.Height;
                    gridSLogData1.Height = gridSLogData.Height;

                    while (true)
                    {
                        vRet = bpc.GetAllGLogData(  Program.gMachineNumber, 
                                                    ref vTMachineNumber, 
                                                    ref vSEnrollNumber, 
                                                    ref vSMachineNumber, 
                                                    ref vVerifyMode, 
                                                    ref vYear, 
                                                    ref vMonth, 
                                                    ref vDay, 
                                                    ref vHour, 
                                                    ref vMinute,
                                                    ref vSecond);
                        if (!vRet) break;
                        if (vRet && i != 1)
                            if (i - vMaxLogCnt > 1)
                                gridSLogData1.AddItem(Convert.ToString(1));

                        AddGLogItem(vTMachineNumber,
                                    vSEnrollNumber,
                                    vSMachineNumber,
                                    vVerifyMode,
                                    vYear,
                                    vMonth,
                                    vDay,
                                    vHour,
                                    vMinute,
                                    vSecond,
                                    i,
                                    vMaxLogCnt,
                                    gridSLogData1
                                   );

                        LabelTotal.Text = "Total : " + Convert.ToString(i);
                        Application.DoEvents();
                        i = i + 1;
                        if (i > vMaxLogCnt * 2) break;

                    }
                }

                vMaxLogCnt = vMaxLogCnt * 2;
                if (i > vMaxLogCnt)
                {
                    gridSLogData.Height = gridSLogData.Height * 2 / 3;
                    gridSLogData1.Top = gridSLogData.Top + gridSLogData.Height;
                    gridSLogData1.Height = gridSLogData.Height;
                    gridSLogData2.Top = gridSLogData.Top + gridSLogData.Height * 2;
                    gridSLogData2.Height = gridSLogData.Height;

                    while (true)
                    {
                        vRet = bpc.GetAllGLogData(  Program.gMachineNumber, 
                                                    ref vTMachineNumber, 
                                                    ref vSEnrollNumber, 
                                                    ref vSMachineNumber, 
                                                    ref vVerifyMode, 
                                                    ref vYear, 
                                                    ref vMonth, 
                                                    ref vDay, 
                                                    ref vHour, 
                                                    ref vMinute,
                                                    ref vSecond);
                        if (!vRet) break;
                        if (vRet && i != 1)
                            if (i - vMaxLogCnt > 1)
                                gridSLogData2.AddItem(Convert.ToString(1));

                        AddGLogItem(vTMachineNumber,
                                    vSEnrollNumber,
                                    vSMachineNumber,
                                    vVerifyMode,
                                    vYear,
                                    vMonth,
                                    vDay,
                                    vHour,
                                    vMinute,
                                    vSecond,
                                    i,
                                    vMaxLogCnt,
                                    gridSLogData2
                                   );

                        LabelTotal.Text = "Total : " + Convert.ToString(i);
                        Application.DoEvents();
                        i = i + 1;
                        if (i > gMaxLow * 3) break;
                    }
                }
                gridSLogData.Redraw = true;
                gridSLogData1.Redraw = true;
                gridSLogData2.Redraw = true;

                lblMessage.Text = "ReadAllGLogData OK";
            }

            Cursor = System.Windows.Forms.Cursors.Default;
            bpc.EnableDevice(Program.gMachineNumber, 1); // 1 : true
        }

        private void cmdEmptyGLog_Click(object sender, EventArgs e)
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

            vRet = bpc.EmptyGeneralLogData(Program.gMachineNumber);
            if (vRet)
            {
                lblMessage.Text = "EmptyGeneralLogData OK";
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

        private void frmLog_Load(object sender, EventArgs e)
        {
            bpc = (AxSBXPCLib.AxSBXPC)Application.OpenForms["frmMain"].Controls["SBXPC1"];
            chkReadMark.Checked = true;
        }

        private void chkReadMark_CheckedChanged(object sender, EventArgs e)
        {
            bpc.ReadMark = chkReadMark.Checked;
        }

        private void AddGLogItem(int vTMachineNumber, int vSEnrollNumber, int vSMachineNumber, int vVerifyMode, int vYear, int vMonth, int vDay, int vHour, int vMinute, int vSecond, int index, int vMaxLogCnt, AxMSFlexGridLib.AxMSFlexGrid gridGlogData)
        {
            int vDoor, vAntipass;
            string stDoor, stAntipass;
            int vDiv = 65536;

            vAntipass = vVerifyMode / vDiv;
            vVerifyMode = vVerifyMode % vDiv;
            vDoor = vVerifyMode / 256;
            vVerifyMode = vVerifyMode % 256;
            stDoor = Convert.ToString(vDoor);
            stAntipass = "";
            if (vAntipass == 1)
                stAntipass = "(AP_In)";
            else if (vAntipass == 3)
                stAntipass = "(AP_Out)";

            gridGlogData.Row = index - vMaxLogCnt;
            gridGlogData.Col = 0;
            gridGlogData.Text = Convert.ToString(index);
            gridGlogData.Col = 1;
            gridGlogData.Text = vTMachineNumber == -1 ? "No Photo" : Convert.ToString(vTMachineNumber);
            gridGlogData.Col = 2;
            gridGlogData.Text = Convert.ToString(vSEnrollNumber);
            gridGlogData.Col = 3;
            gridGlogData.Text = Convert.ToString(vSMachineNumber);
            gridGlogData.Col = 4;
            gridGlogData.Text = stDoor;
            gridGlogData.Col = 5;
            if (vVerifyMode == 1)
                gridGlogData.Text = "Fp";
            else if (vVerifyMode == 2)
                gridGlogData.Text = "Password";
            else if (vVerifyMode == 3)
                gridGlogData.Text = "Card";
            else if (vVerifyMode == 4)
                gridGlogData.Text = "FP+Card";
            else if (vVerifyMode == 5)
                gridGlogData.Text = "FP+Pwd";
            else if (vVerifyMode == 6)
                gridGlogData.Text = "Card+Pwd";
            else if (vVerifyMode == 7)
                gridGlogData.Text = "FP+Card+Pwd";
            else if (vVerifyMode == 10)
                gridGlogData.Text = "Hand Lock";
            else if (vVerifyMode == 11)
                gridGlogData.Text = "Prog Lock";
            else if (vVerifyMode == 12)
                gridGlogData.Text = "Prog Open";
            else if (vVerifyMode == 13)
                gridGlogData.Text = "Prog Close";
            else if (vVerifyMode == 14)
                gridGlogData.Text = "Auto Recover";
            else if (vVerifyMode == 20)
                gridGlogData.Text = "Lock Over";
            else if (vVerifyMode == 21)
                gridGlogData.Text = "Illegal Open";
            else if (vVerifyMode == 22)
                gridGlogData.Text = "Duress alarm";
            else if (vVerifyMode == 23)
                gridGlogData.Text = "Tamper detect";
            else
                gridGlogData.Text = "--";

            gridGlogData.Text = gridGlogData.Text + stAntipass;
            gridGlogData.Col = 6;
            gridGlogData.Text = Convert.ToString(vYear) + "/" + String.Format("{0:D2}", vMonth) + "/" + String.Format("{0:D2}", vDay) + " " + String.Format("{0:D2}", vHour) + ":" + String.Format("{0:D2}", vMinute);
        }

        private void frmLog_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.OpenForms["frmMain"].Visible = true;
        }

    }
}
