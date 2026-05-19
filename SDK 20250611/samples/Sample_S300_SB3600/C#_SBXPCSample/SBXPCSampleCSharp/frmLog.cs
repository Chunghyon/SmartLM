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
        bool glogSearched = false;
        int prevSelectLogIndex = -1;

        private void cmdSLogData_Click(object sender, EventArgs e)
        {
            glogSearched = false;

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
            glogSearched = false;

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
            glogSearched = true;

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
            int vAttStatus, vAntipass;
            string stAttStatus, stAntipass;
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

            gstrLogItem = new Object[] { "", "PhotoNo", "EnrollNo", "EMachineNo", "VeriMode", "DateTime" };

            // gridSLogData
            gridSLogData.Row = 0;
            gridSLogData.Cols = 9;
            gridSLogData.set_ColWidth(0, 600);
            for (i = 1; i < 6; i++)
            {
                gridSLogData.Col = i;
                gridSLogData.Text = (string)gstrLogItem[i];
                gridSLogData.set_ColAlignment(i, 3);
                gridSLogData.set_ColWidth(i, 1200);
            }
            gridSLogData.set_ColWidth(4, 2000);
            gridSLogData.Col = 5;
            gridSLogData.set_ColWidth(5, 2000);
            gridSLogData.set_ColWidth(6, 700);
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
            for (i = 1; i < 6; i++)
            {
                gridSLogData1.Col = i;
                gridSLogData1.Text = (string)gstrLogItem[i];
                gridSLogData1.set_ColAlignment(i, 3);
                gridSLogData1.set_ColWidth(i, 1200);
            }
            gridSLogData1.set_ColWidth(4, 2000);
            gridSLogData1.Col = 5;
            gridSLogData1.set_ColWidth(5, 2000);
            gridSLogData1.set_ColWidth(6, 700);
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
            for (i = 1; i < 6; i++)
            {
                gridSLogData2.Col = i;
                gridSLogData2.Text = (string)gstrLogItem[i];
                gridSLogData2.set_ColAlignment(i, 3);
                gridSLogData2.set_ColWidth(i, 1200);
            }
            gridSLogData2.set_ColWidth(4, 2000);
            gridSLogData2.Col = 5;
            gridSLogData2.set_ColWidth(5, 2000);
            gridSLogData2.set_ColWidth(6, 700);
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
                               gridSLogData1
                              );

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

                lblMessage.Text = "ReadGeneralLogData OK";
            }

            Cursor = System.Windows.Forms.Cursors.Default;
            bpc.EnableDevice(Program.gMachineNumber, 1); // 1 : true
        }

        private void cmdAllGLogData_Click(object sender, EventArgs e)
        {
            glogSearched = true;

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

            gstrLogItem = new Object[] { "", "PhotoNo", "EnrollNo", "EMachineNo", "VeriMode", "DateTime" };

            // gridSLogData
            gridSLogData.Row = 0;
            gridSLogData.Cols = 9;
            gridSLogData.set_ColWidth(0, 600);
            for (i = 1; i < 6; i++)
            {
                gridSLogData.Col = i;
                gridSLogData.Text = (string)gstrLogItem[i];
                gridSLogData.set_ColAlignment(i, 3);
                gridSLogData.set_ColWidth(i, 1200);
            }
            gridSLogData.set_ColWidth(4, 2000);
            gridSLogData.Col = 5;
            gridSLogData.set_ColWidth(5, 2000);
            gridSLogData.set_ColWidth(6, 700);
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
            for (i = 1; i < 6; i++)
            {
                gridSLogData1.Col = i;
                gridSLogData1.Text = (string)gstrLogItem[i];
                gridSLogData1.set_ColAlignment(i, 3);
                gridSLogData1.set_ColWidth(i, 1200);
            }
            gridSLogData1.set_ColWidth(4, 2000);
            gridSLogData1.Col = 5;
            gridSLogData1.set_ColWidth(5, 2000);
            gridSLogData1.set_ColWidth(6, 700);
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
            for (i = 1; i < 6; i++)
            {
                gridSLogData2.Col = i;
                gridSLogData2.Text = (string)gstrLogItem[i];
                gridSLogData2.set_ColAlignment(i, 3);
                gridSLogData2.set_ColWidth(i, 1200);
            }
            gridSLogData2.set_ColWidth(4, 2000);
            gridSLogData2.Col = 5;
            gridSLogData2.set_ColWidth(5, 2000);
            gridSLogData2.set_ColWidth(6, 700);
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
            picGlogPhoto.Image = null;
            chkReadMark.Checked = true;
        }

        private void chkReadMark_CheckedChanged(object sender, EventArgs e)
        {
            bpc.ReadMark = chkReadMark.Checked;
        }

        private void btnSetRange_Click(object sender, EventArgs e)
        {
            bool bRet = false;
            int vErrorCode = 0;
            String strXML = "";

            lblMessage.Text = "Working...";
            Application.DoEvents();

            bRet = bpc.EnableDevice(Program.gMachineNumber, 0); // 0 : disable
            if(!bRet)
            {
                lblMessage.Text = util.gstrNoDevice;
                return;
            }

            util.MakeXMLRequestHeader(bpc, ref strXML, "SetGLogSearchRange");
            bpc.XML_AddBoolean(ref strXML, "UseSearchRange", chkSearchRangeUse.Checked);

            if(chkSearchRangeUse.Checked)
            {
                bpc.XML_AddLong(ref strXML, "StartYear", dtGlogSearchStart.Value.Year);
                bpc.XML_AddLong(ref strXML, "StartMonth", dtGlogSearchStart.Value.Month);
                bpc.XML_AddLong(ref strXML, "StartDate", dtGlogSearchStart.Value.Day);
                bpc.XML_AddLong(ref strXML, "EndYear", dtGlogSearchEnd.Value.Year);
                bpc.XML_AddLong(ref strXML, "EndMonth", dtGlogSearchEnd.Value.Month);
                bpc.XML_AddLong(ref strXML, "EndDate", dtGlogSearchEnd.Value.Day);
            }

            bRet = bpc.GeneralOperationXML(ref strXML);

            if (bRet)
                lblMessage.Text = "SetGlogSearchRange OK";
            else
            {
                bpc.GetLastError(ref vErrorCode);
                lblMessage.Text = util.ErrorPrint(vErrorCode);
            }

            bRet = bpc.EnableDevice(Program.gMachineNumber, 1); // 1 : enable
        }

        private void AddGLogItem(int vTMachineNumber, int vSEnrollNumber, int vSMachineNumber, int vVerifyMode, int vYear, int vMonth, int vDay, int vHour, int vMinute, int vSecond, int index, int vMaxLogCnt, AxMSFlexGridLib.AxMSFlexGrid gridGlogData)
        {
            int vAttStatus, vAntipass;
            string stAttStatus, stAntipass;
            int vDiv = 65536;

            vAntipass = vVerifyMode / vDiv;
            vVerifyMode = vVerifyMode % vDiv;
            vAttStatus = vVerifyMode / 256;
            vVerifyMode = vVerifyMode % 256;
            stAttStatus = "";
            stAntipass = "";
            if (vAttStatus == 0)
                stAttStatus = "_DutyOn";
            else if (vAttStatus == 1)
                stAttStatus = "_DutyOff";
            else if (vAttStatus == 2)
                stAttStatus = "_OverOn";
            else if (vAttStatus == 3)
                stAttStatus = "_OverOff";
            else if (vAttStatus == 4)
                stAttStatus = "_GoIn";
            else if (vAttStatus == 5)
                stAttStatus = "_GoOut";

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
            else if (vVerifyMode == 51)
                 gridGlogData.Text = "Fp";
            else if (vVerifyMode == 52)
                gridGlogData.Text = "Password";
            else if (vVerifyMode == 53)
                gridGlogData.Text = "Card";
            else if (vVerifyMode == 101)
                gridGlogData.Text = "Fp";
            else if (vVerifyMode == 102)
                gridGlogData.Text = "Password";
            else if (vVerifyMode == 103)
                gridGlogData.Text = "Card";
            else if (vVerifyMode == 151)
                gridGlogData.Text = "Fp";
            else if (vVerifyMode == 152)
                gridGlogData.Text = "Password";
            else if (vVerifyMode == 153)
                gridGlogData.Text = "Card";
            else
                gridGlogData.Text = "--";

            if (1 <= vVerifyMode && vVerifyMode <= 7)
                gridGlogData.Text = gridGlogData.Text + stAttStatus;
            else if (51 <= vVerifyMode && vVerifyMode <= 53)
                gridGlogData.Text = gridGlogData.Text + stAttStatus;
            else if (101 <= vVerifyMode && vVerifyMode <= 103)
                gridGlogData.Text = gridGlogData.Text + stAttStatus;
            else if (151 <= vVerifyMode && vVerifyMode <= 153)
                gridGlogData.Text = gridGlogData.Text + stAttStatus;

            gridGlogData.Text = gridGlogData.Text + stAntipass;
            gridGlogData.Col = 5;
            gridGlogData.Text = Convert.ToString(vYear) + "/" + String.Format("{0:D2}", vMonth) + "/" + String.Format("{0:D2}", vDay) + " " + String.Format("{0:D2}", vHour) + ":" + String.Format("{0:D2}", vMinute);
        }

        private void gridSLogData_ClickEvent(object sender, EventArgs e)
        {
            if (!glogSearched) return;
            if (!chkShowGlogPhoto.Checked) return;
            if (prevSelectLogIndex == gridSLogData.Row) return;
            prevSelectLogIndex = gridSLogData.Row;

            bool bRet;
            int vErrorCode = 0;
            String strXML = "", strPhotoNum;
            int photoNumber;

            strPhotoNum = gridSLogData.get_TextMatrix(gridSLogData.Row, 1);
            if (strPhotoNum == "No Photo")
            {
                ClearGlogPhoto();
                return;
            }
            photoNumber = Convert.ToInt32(strPhotoNum);

            lblMessage.Text = "Working...";
            Application.DoEvents();

            bRet = bpc.EnableDevice(Program.gMachineNumber, 0); // 0 : disable

            if (!bRet)
            {
                lblMessage.Text = util.gstrNoDevice;
                return;
            }

            util.MakeXMLRequestHeader(bpc, ref strXML, "GetGLogPhotoData");
            bpc.XML_AddLong(ref strXML, "PhotoPos", photoNumber);

            bRet = bpc.GeneralOperationXML(ref strXML);

            if (!bRet)
            {
                bpc.GetLastError(ref vErrorCode);
                lblMessage.Text = util.ErrorPrint(vErrorCode);
                ClearGlogPhoto();
                goto _lexit;
            }

            Byte[] photoData = new Byte[util.gCompressPhotoSize];
            GCHandle gh = GCHandle.Alloc(photoData, GCHandleType.Pinned);
            IntPtr AddrOfPhotoData = gh.AddrOfPinnedObject();
            int nAddr = AddrOfPhotoData.ToInt32();
            bRet = bpc.XML_ParseBinaryLong(ref strXML, "PhotoData", ref nAddr, util.gCompressPhotoSize);

            if (!bRet)
            {
                lblMessage.Text = "GetGlogPhotoData - XML Parse Error";
                ClearGlogPhoto();
                goto _lexit;
            }

            lblMessage.Text = "GetGLogPhotoData OK";

            ClearGlogPhoto();
            if (File.Exists(util.gTempPhotoFile))
                File.Delete(util.gTempPhotoFile);
            FileStream FS = File.Create(util.gTempPhotoFile);
            FS.Write(photoData, 0, util.gCompressPhotoSize);
            FS.Close();
            FS.Dispose();
            FS = null;

            picGlogPhoto.Image = Image.FromFile(util.gTempPhotoFile);

        _lexit:
            bRet = bpc.EnableDevice(Program.gMachineNumber, 1); // 1 : enable
            return;
        }

        private void frmLog_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.OpenForms["frmMain"].Visible = true;
            ClearGlogPhoto();
        }

        private void ClearGlogPhoto()
        {
            if (picGlogPhoto.Image != null) picGlogPhoto.Image.Dispose();
            picGlogPhoto.Image = null;
        }
    }
}
