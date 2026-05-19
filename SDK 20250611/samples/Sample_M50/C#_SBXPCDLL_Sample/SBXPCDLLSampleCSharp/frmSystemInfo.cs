using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlTypes;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace SBXPCDLLSampleCSharp
{
    public partial class frmSystemInfo : Form
    {
        public frmSystemInfo()
        {
            InitializeComponent();
        }

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

            vRet = sbxpc.SBXPCDLL.EnableDevice(Program.gMachineNumber, 0); // 0 : false
            if (!vRet)
            {
                lblMessage.Text = util.gstrNoDevice;
                return;
            }

            vRet = sbxpc.SBXPCDLL.GetDeviceTime(Program.gMachineNumber,
                                     out vYear,
                                     out vMonth,
                                     out vDay,
                                     out vHour,
                                     out vMinute,
                                     out vSecond,
                                     out vDayOfWeek);
            if (vRet)
            {
                if (vDayOfWeek == 0) vDayOfWeek = 7;
                strDataTime = "Date = " + Convert.ToString(vYear) + "/" + Convert.ToString(vMonth) + "/" + Convert.ToString(vDay) + " , " + GetWeekDay(vDayOfWeek) + " , Time = " + Convert.ToString(vHour) + ":" + Convert.ToString(vMinute) + ":" + Convert.ToString(vSecond);
                lblMessage.Text = strDataTime;
            }
            else
            {
                sbxpc.SBXPCDLL.GetLastError(Program.gMachineNumber, out vErrorCode);
                lblMessage.Text = util.ErrorPrint(vErrorCode);
            }
            sbxpc.SBXPCDLL.EnableDevice(Program.gMachineNumber, 1); // 1 : true
            Application.DoEvents();
        }

        private void cmdSetDeviceTime_Click(object sender, EventArgs e)
        {
            Boolean vRet;
            int vErrorCode = 0;

            lblMessage.Text = "Working...";
            Application.DoEvents();

            vRet = sbxpc.SBXPCDLL.EnableDevice(Program.gMachineNumber, 0); // 0 : false
            if (!vRet)
            {
                lblMessage.Text = util.gstrNoDevice;
                return;
            }

            vRet = sbxpc.SBXPCDLL.SetDeviceTime(Program.gMachineNumber);
            if (vRet)
            {
                lblMessage.Text = "Success!";
            }
            else
            {
                sbxpc.SBXPCDLL.GetLastError(Program.gMachineNumber, out vErrorCode);
                lblMessage.Text = util.ErrorPrint(vErrorCode);
            }

            sbxpc.SBXPCDLL.EnableDevice(Program.gMachineNumber, 1); // 1 : true
            Application.DoEvents();
        }

        private void cmdPowerOn_Click(object sender, EventArgs e)
        {
            sbxpc.SBXPCDLL.PowerOnAllDevice(Program.gMachineNumber);
        }

        private void PowerOffDevice_Click(object sender, EventArgs e)
        {
            Boolean vRet;
            int vErrorCode = 0;

            lblMessage.Text = "Working...";
            Application.DoEvents();

            vRet = sbxpc.SBXPCDLL.PowerOffDevice(Program.gMachineNumber);
            if (vRet)
            {
                lblMessage.Text = "Success!";
            }
            else
            {
                sbxpc.SBXPCDLL.GetLastError(Program.gMachineNumber, out vErrorCode);
                lblMessage.Text = util.ErrorPrint(vErrorCode);
            }
        }

        private void cmdEnableDevice_Click(object sender, EventArgs e)
        {
            byte vFlag;
            Boolean vRet;
            int vErrorCode = 0;

            lblMessage.Text = "Working...";
            Application.DoEvents();

            vFlag = chkEnableDevice.Checked ? (byte)1 : (byte)0;

            vRet = sbxpc.SBXPCDLL.EnableDevice(Program.gMachineNumber, vFlag);
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
                sbxpc.SBXPCDLL.GetLastError(Program.gMachineNumber, out vErrorCode);
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
            uint vValue = 0;
            Boolean vRet;
            int vErrorCode = 0;

            lblMessage.Text = "Working...";
            Application.DoEvents();

            vInfo = cmbSatus.SelectedIndex + 1;

            vRet = sbxpc.SBXPCDLL.EnableDevice(Program.gMachineNumber, 0); // 0 : false
            if (!vRet)
            {
                lblMessage.Text = util.gstrNoDevice;
                return;
            }

            vRet = sbxpc.SBXPCDLL.GetDeviceInfo(Program.gMachineNumber, vInfo, out vValue);
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
                        lblMessage.Text = "(13) = Identification mode = " + vValue;
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
                        lblMessage.Text = "(20) = UDP Server = " + util.pubLongToIPAddr((int)vValue);
                        break;
                    case 21:
                        lblMessage.Text = "(21) = DHCP Use = " + vValue;
                        break;
                    case 22:
                        lblMessage.Text = "(22) = Main Lock Group = " + vValue;
                        break;
                    case 23:
                        lblMessage.Text = "(23) = Show Realtime Camera = " + vValue;
                        lblMessage.Text += " (" + ((vValue == 2) ? "Realtime Photo" : ((vValue == 1) ? "Enrolled Photo" : "None")) + ")";
                        break;
                    case 24:
                        lblMessage.Text = "(24) = Use Fail Log = " + vValue;
                        break;
                    case 28:
                        lblMessage.Text = "(28) = Sound Volume = " + vValue;
                        break;
                    case 29:
                        lblMessage.Text = "(29) = Monitor Tamper = " + vValue;
                        break;
                    case 30:
                        lblMessage.Text = "(30) = Face Engine Threshold = " + vValue;
                        lblMessage.Text += " (" + ((vValue == 2) ? "Low" : ((vValue == 1) ? "High" : "Normal")) + ")";
                        break;
                    case 31:
                        lblMessage.Text = "(31) = Face Anti-Spoofing = " + vValue;
                        break;
                    case 32:
                        lblMessage.Text = "(32) = Use Measure Temperature = " + vValue;
                        break;
                    case 33:
                        lblMessage.Text = "(33) = Show Realtime Temperature = " + vValue;
                        break;
                    case 34:
                        lblMessage.Text = "(34) = Abnormal temperature Disable door open = " + vValue;
                        break;
                    case 35:
                        lblMessage.Text = "(35) = Abnormal temperature threshold 10 = " + vValue +
                                                    "  (" + vValue / 10 + "." + (vValue % 10) + "'C)";
                        break;
                    case 36:
                        {
                            String vStr = "Unknown";
                            switch (vValue)
                            {
                                case 0: vStr = "0.5 second"; break;
                                case 1: vStr = "1.0 second"; break;
                                case 2: vStr = "1.5 second"; break;
                                case 3: vStr = "2.0 second"; break;
                                case 4: vStr = "2.5 second"; break;
                                case 5: vStr = "3.0 second"; break;
                            }
                            lblMessage.Text = "(36) = Measuring Duration Type = " + vValue +
                                                    "  (" + vStr + ")";
                            break;
                        }
                    case 37:
                        {
                            String vStr = "Unknown";
                            switch (vValue)
                            {
                                case 0: vStr = "Near"; break;
                                case 1: vStr = "Middle"; break;
                                case 2: vStr = "Far"; break;
                                case 3: vStr = "No Limit"; break;
                            }
                            lblMessage.Text = "(37) = Measuring Distance Type = " + vValue +
                                                    "  (" + vStr + ")";
                            break;
                        }
                    case 38:
                        {
                            String vStr = "Unknown";
                            switch (vValue)
                            {
                                case 0: vStr = "Celsius"; break;
                                case 1: vStr = "Fahrenheit"; break;
                            }
                            lblMessage.Text = "(38) = Temperature Unit = " + vValue +
                                                    "  (" + vStr + ")";
                            break;
                        }
                    case 39:
                        lblMessage.Text = "(39) = Abnormal temperature threshold 10 = " + vValue +
                                                    "  (" + vValue / 10 + "." + (vValue % 10) + "'F)";
                        break;
                    case 40:
                        lblMessage.Text = "(40) = Use Visitor Mode = " + vValue;
                        break;
                    case 41:
                        lblMessage.Text = "(41) = Need Wearing Mask = " + vValue;
                        break;
                    case 42:
                        lblMessage.Text = "(42) = Suggest Wearing Mask = " + vValue;
                        break;
                    case 43:
                        {
                            String vStr = "Unknown";
                            switch (vValue)
                            {
                                case 0: vStr = "PlayerMode_None"; break;
                                case 1: vStr = "PlayerMode_Picture"; break;
                            }
                            lblMessage.Text = "(43) = PlayerMode = " + vValue +
                                                    "  (" + vStr + ")";
                            break;
                        }
                    case 44:
                        lblMessage.Text = "(44) = PlayerPictureInterval = " + vValue + " (second)";
                        break;
                    case 45:
                        {
                            String vStr = "Unknown";
                            switch (vValue)
                            {
                                case 0: vStr = "ScrMode_Small"; break;
                                case 1: vStr = "ScrMode_Normal"; break;
                            }
                            lblMessage.Text = "(45) = PlayerVerifyScrMode = " + vValue +
                                                    "  (" + vStr + ")";
                            break;
                        }
                    case 46:
                        lblMessage.Text = "(46) = UtcTimezoneMinutes = " + ((int)vValue) + " (minutes)";
                        break;
                    case 47:
                        lblMessage.Text = "(47) = Multi Verify Count = " + vValue;
                        break;
                    case 48:
                        {
                            String vStr = "Unknown";
                            switch (vValue)
                            {
                                case 0: vStr = "Green"; break;
                                case 1: vStr = "Turquoise"; break;
                                case 2: vStr = "Purple"; break;
                                case 3: vStr = "Orange"; break;
                                case 4: vStr = "Blue"; break;
                                case 5: vStr = "Grey"; break;
                                case 6: vStr = "Red"; break;
                                case 7: vStr = "White"; break;
                            }
                            lblMessage.Text = "(48) = Background Color = " + vValue +
                                                    "  (" + vStr + ")";
                            break;
                        }

                    default:
                        lblMessage.Text = "(" + vInfo.ToString() + ") = " + vValue;
                        break;
                }
            }
            else
            {
                sbxpc.SBXPCDLL.GetLastError(Program.gMachineNumber, out vErrorCode);
                lblMessage.Text = util.ErrorPrint(vErrorCode);
            }

            sbxpc.SBXPCDLL.EnableDevice(Program.gMachineNumber, 1); // 1 : true
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

            vRet = sbxpc.SBXPCDLL.EnableDevice(Program.gMachineNumber, 0); // 0 : false
            if (!vRet)
            {
                lblMessage.Text = util.gstrNoDevice;
                return;
            }

            vRet = sbxpc.SBXPCDLL.SetDeviceInfo(Program.gMachineNumber, vInfo, vValue);
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
                sbxpc.SBXPCDLL.GetLastError(Program.gMachineNumber, out vErrorCode);
                lblMessage.Text = util.ErrorPrint(vErrorCode);
            }
            sbxpc.SBXPCDLL.EnableDevice(Program.gMachineNumber, 1); // 1 : true
            Application.DoEvents();
        }

        private void cmdGetDeviceStaus_Click(object sender, EventArgs e)
        {
            int vStatus;
            uint vValue = 0;
            int vErrorCode = 0;
            Boolean vRet;

            lblMessage.Text = "Working...";
            Application.DoEvents();

            vStatus = cmbSatus.SelectedIndex + 1;

            vRet = sbxpc.SBXPCDLL.EnableDevice(Program.gMachineNumber, 0); // 0 : false
            if (!vRet)
            {
                lblMessage.Text = util.gstrNoDevice;
                return;
            }

            vRet = sbxpc.SBXPCDLL.GetDeviceStatus(Program.gMachineNumber, vStatus, out vValue);
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
                    case 9:
                        lblMessage.Text = "(9) = Face Count = " + vValue;
                        break;
                    case 10:
                        lblMessage.Text = "(10) = SLog unread count = " + vValue;
                        break;
                    case 11:
                        lblMessage.Text = "(11) = GLog unread count = " + vValue;
                        break;
                    case 12:
                        lblMessage.Text = "(12) = Max User count = " + vValue;
                        break;
                    case 13:
                        lblMessage.Text = "(13) = Max Face count = " + vValue;
                        break;
                    case 14:
                        lblMessage.Text = "(14) = Max Fp count = " + vValue;
                        break;
                    case 15:
                        lblMessage.Text = "(15) = Max GLog count = " + vValue;
                        break;
                    case 16:
                        lblMessage.Text = "(16) = Max SLog count = " + vValue;
                        break;
                    case 17:
                        lblMessage.Text = "(17) = QR count = " + vValue;
                        break;
                }
            }
            else
            {
                sbxpc.SBXPCDLL.GetLastError(Program.gMachineNumber, out vErrorCode);
                lblMessage.Text = util.ErrorPrint(vErrorCode);
            }

            sbxpc.SBXPCDLL.EnableDevice(Program.gMachineNumber, 1); // 1 : true
        }

        private void frmSystemInfo_Load(object sender, EventArgs e)
        {
            cmbSatus.SelectedIndex = 0;

            //txtPictureFolderDir.Text = @"F:\_Work\New folder";
        }

        private void frmSystemInfo_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.OpenForms["frmMain"].Visible = true;
        }

        private void cmdPictureFolderBrowse_Click(object sender, EventArgs e)
        {
            folderBrowserDialog1.ShowDialog();
            txtPictureFolderDir.Text = folderBrowserDialog1.SelectedPath;
        }

        private void cmdUploadPictureByAdmin_Click(object sender, EventArgs e)
        {
            on_UploadPicture(true);
        }

        private void cmdUploadPicture_Click(object sender, EventArgs e)
        {
            on_UploadPicture(false);
        }
        private void on_UploadPicture(bool admin)
        {
            string strSectionToUpload = "UnProtected";

            System.IO.DirectoryInfo dir;
            IEnumerable<System.IO.FileInfo> fileList = null;

            try
            {
                dir = new System.IO.DirectoryInfo(txtPictureFolderDir.Text);
                fileList = dir.GetFiles("*.jpg", System.IO.SearchOption.TopDirectoryOnly);
            }
            catch
            {
            }

            if (fileList == null ||
                fileList.Count() <= 0)
            {
                lblMessage.Text = "No jpg file found.";
                return;
            }

            bool bRet;
            int vErrorCode = 0;
            string strXML;
            string strResultCode = "";

            bRet = sbxpc.SBXPCDLL.EnableDevice(Program.gMachineNumber, 0);
            if (!bRet)
            {
                lblMessage.Text = util.gstrNoDevice;
                return;
            }

            strXML = "";
            util.MakeXMLRequestHeader(ref strXML, "WritePicFilePrepare");
            bRet = sbxpc.SBXPCDLL.GeneralOperationXML(Program.gMachineNumber, ref strXML);

            if (!bRet)
            {
                sbxpc.SBXPCDLL.GetLastError(Program.gMachineNumber, out vErrorCode);
                lblMessage.Text = util.ErrorPrint(vErrorCode);
                goto _lexit;
            }

            sbxpc.SBXPCDLL.XML_ParseString(ref strXML, "ResultCode", out strResultCode);
            if (strResultCode == "NeedToSelectSection")
            {
                string strSection = "";
                sbxpc.SBXPCDLL.XML_ParseString(ref strXML, "SectionMode", out strSection);

                if (strSection == "Protect_Partial")
                {
                    if (admin)
                    {
                        switch (MessageBox.Show("Please select section to upload.\n\n" +
                                            " [Yes] -> Protected Section (AdminPwd required)\n" +
                                            " [No] -> UnProtected Section",
                                            "Upload picture.", MessageBoxButtons.YesNoCancel))
                        {
                            case DialogResult.Yes:
                                strSectionToUpload = "Protected";
                                break;
                            case DialogResult.No:
                                strSectionToUpload = "UnProtected";
                                break;
                            default:
                                lblMessage.Text = "Operation canceled.";
                                goto _lexit;
                        }
                    }
                    else
                    {
                        strSectionToUpload = "UnProtected";
                    }
                }
                else if (strSection == "Protect_All")
                {
                    if (admin)
                    {
                        strSectionToUpload = "Protected";
                    }
                    else
                    {
                        lblMessage.Text = "Not allowed to upload picture.";
                        goto _lexit;
                    }
                }
                else
                {
                    strSectionToUpload = "UnProtected";
                }

                strXML = "";
                util.MakeXMLRequestHeader(ref strXML, "WritePicFilePrepare");
                sbxpc.SBXPCDLL.XML_AddString(ref strXML, "UploadSection", strSectionToUpload);
                if (admin)
                    sbxpc.SBXPCDLL.XML_AddString(ref strXML, "AdminPwd", txtAdminPwd.Text);
                bRet = sbxpc.SBXPCDLL.GeneralOperationXML(Program.gMachineNumber, ref strXML);

                if (!bRet)
                {
                    sbxpc.SBXPCDLL.GetLastError(Program.gMachineNumber, out vErrorCode);
                    lblMessage.Text = util.ErrorPrint(vErrorCode);
                    goto _lexit;
                }

                sbxpc.SBXPCDLL.XML_ParseString(ref strXML, "ResultCode", out strResultCode);
            }

            if (strResultCode != "Success")
            {
                lblMessage.Text = "WritePicFilePrepare Failed. (" + strResultCode + ")";
                goto _lexit;
            }

            int max_recv_size = sbxpc.SBXPCDLL.XML_ParseInt(ref strXML, "MaxRecvSize");

            int file_index = 0;
            int upload_count = 0;

            foreach (var file in fileList)
            {
                file_index++;
                lblMessage.Text = "WritePicFile: " + file_index.ToString() + "- " + file.Name;

                using (FileStream FS = File.Open(file.FullName, FileMode.Open, FileAccess.Read, FileShare.None))
                {
                    if (FS.Length > 0)
                    {
                        int size;
                        int offset = 0;
                        while (offset < FS.Length)
                        {
                            size = max_recv_size;
                            if (FS.Length - offset < size)
                                size = (int)(FS.Length - offset);

                            Byte[] fileData = new Byte[size];
                            FS.Read(fileData, 0, size);

                            GCHandle gh = GCHandle.Alloc(fileData, GCHandleType.Pinned);
                            IntPtr AddrOfFileData = gh.AddrOfPinnedObject();

                            strXML = "";
                            util.MakeXMLRequestHeader(ref strXML, "WritePicFile");
                            sbxpc.SBXPCDLL.XML_AddString(ref strXML, "UploadSection", strSectionToUpload);
                            if (admin)
                                sbxpc.SBXPCDLL.XML_AddString(ref strXML, "AdminPwd", txtAdminPwd.Text);
                            sbxpc.SBXPCDLL.XML_AddInt(ref strXML, "FileIndex", file_index);
                            sbxpc.SBXPCDLL.XML_AddInt(ref strXML, "FileSize", (int)FS.Length);
                            sbxpc.SBXPCDLL.XML_AddInt(ref strXML, "Offset", offset);
                            sbxpc.SBXPCDLL.XML_AddInt(ref strXML, "Size", size);
                            sbxpc.SBXPCDLL.XML_AddBinaryLong(ref strXML, "Data", AddrOfFileData, size);

                            bRet = sbxpc.SBXPCDLL.GeneralOperationXML(Program.gMachineNumber, ref strXML);

                            if (!bRet)
                                break;

                            sbxpc.SBXPCDLL.XML_ParseString(ref strXML, "ResultCode", out strResultCode);

                            if (strResultCode != "Success")
                                break;

                            offset += size;

                            lblMessage.Text = "WritePicFile: " + file_index.ToString() + "- " + file.Name
                                + " " + offset.ToString() + "/" + FS.Length.ToString();
                            Application.DoEvents();
                        }
                    }
                    FS.Close();
                    FS.Dispose();
                }

                if (!bRet)
                {
                    break;
                }
                else if (strResultCode != "Success")
                {
                    if (MessageBox.Show("FileName: " + file.Name
                                        + "\nResultCode： " + strResultCode
                                        + "\n\nContinue?",
                                        "Upload Failed.", MessageBoxButtons.YesNo) != DialogResult.Yes)
                        break;
                }
                else
                {
                    upload_count++;
                }
            }

            if (bRet)
            {
                if (strResultCode == "Success")
                    lblMessage.Text = "Success.";
                else
                    lblMessage.Text = "Failed. (" + strResultCode + ")";

                lblMessage.Text += " Uploaded Count: " + upload_count;
            }
            else
            {
                sbxpc.SBXPCDLL.GetLastError(Program.gMachineNumber, out vErrorCode);
                lblMessage.Text = util.ErrorPrint(vErrorCode);
            }

        _lexit:
            sbxpc.SBXPCDLL.EnableDevice(Program.gMachineNumber, 1);
        }

        private void CmdGetGPSCoordinate_Click(object sender, EventArgs e)
        {
            string strXML = null;
            string strLatitude = "";
            string strLongitude = "";
            sbxpc.SBXPCDLL.XML_AddString(ref strXML, "REQUEST", "GetDeviceInfoExt");
            sbxpc.SBXPCDLL.XML_AddString(ref strXML, "MSGTYPE", "request");
            sbxpc.SBXPCDLL.XML_AddLong(ref strXML, "MachineID", Program.gMachineNumber);
            sbxpc.SBXPCDLL.XML_AddString(ref strXML, "ParamName", "gps_position");

            if (sbxpc.SBXPCDLL.GeneralOperationXML(Program.gMachineNumber, ref strXML))
            {
                sbxpc.SBXPCDLL.XML_ParseString(ref strXML, "Value1", out strLatitude);
                sbxpc.SBXPCDLL.XML_ParseString(ref strXML, "Value2", out strLongitude);

                lblMessage.Text = "Latitude=" + strLatitude + ", Longitude=" + strLongitude;
            }
            else
            {
                lblMessage.Text = "Get GPS Coordinate Failed.";
            }
        }

        private void ClearIconPhoto()
        {
            if (picIcon.Image != null) picIcon.Image.Dispose();
            picIcon.Image = null;
        }

        private void cmdIconBrowse_Click(object sender, EventArgs e)
        {
            OpenFileDlg.ShowDialog();
            txtIconFile.Text = OpenFileDlg.FileName;
            ClearIconPhoto();
            if (!File.Exists(txtIconFile.Text))
                return;

            picIcon.Image = Image.FromFile(txtIconFile.Text);
        }

        private void cmdSet_Click(object sender, EventArgs e)
        {
            bool bRet;
            int vErrorCode = 0;
            string strXML = "";

            util.MakeXMLRequestHeader(ref strXML, "SetTrIcon");
            sbxpc.SBXPCDLL.XML_AddInt(ref strXML, "IconNo", int.Parse(txtIconNo.Text)); // 1~8
            sbxpc.SBXPCDLL.XML_AddInt(ref strXML, "Status", int.Parse(txtIconStatus.Text)); // 0~2

            if (chkDelete.Checked)
            {
                sbxpc.SBXPCDLL.XML_AddBoolean(ref strXML, "Delete", true);
            }
            else
            {
                string photoFileName = txtIconFile.Text;
                if (!File.Exists(photoFileName))
                {
                    lblMessage.Text = "Can not find the icon file.";
                    return;
                }

                ClearIconPhoto();

                Byte[] photoData;
                int nPhotoSize;
                using (FileStream FS = File.Open(photoFileName, FileMode.Open, FileAccess.Read, FileShare.None))
                {
                    if (FS.Length <= 0 || FS.Length > util.gCompressPhotoSize_64K)
                    {
                        lblMessage.Text = "Photo file size is invalid.";
                        return;
                    }

                    nPhotoSize = (int)FS.Length;
                    photoData = new Byte[nPhotoSize];
                    FS.Read(photoData, 0, nPhotoSize);
                    FS.Close();
                    FS.Dispose();
                }

                picIcon.Image = Image.FromFile(photoFileName);

                GCHandle gh = GCHandle.Alloc(photoData, GCHandleType.Pinned);
                IntPtr AddrOfPhotoData = gh.AddrOfPinnedObject();

                sbxpc.SBXPCDLL.XML_AddBinaryLong(ref strXML, "IconData", AddrOfPhotoData, nPhotoSize);
                sbxpc.SBXPCDLL.XML_AddInt(ref strXML, "IconSize", nPhotoSize);
            }

            lblMessage.Text = "Working...";
            Application.DoEvents();

            bRet = sbxpc.SBXPCDLL.EnableDevice(Program.gMachineNumber, 0);
            if (!bRet)
            {
                lblMessage.Text = util.gstrNoDevice;
                return;
            }

            bRet = sbxpc.SBXPCDLL.GeneralOperationXML(Program.gMachineNumber, ref strXML);

            if (bRet)
            {
                string strResultCode;
                string strErr;
                sbxpc.SBXPCDLL.XML_ParseString(ref strXML, "ResultCode", out strResultCode);
                sbxpc.SBXPCDLL.XML_ParseString(ref strXML, "ErrStr", out strErr);

                if (strResultCode == "Success")
                {
                    lblMessage.Text = "SetTrIcon OK";
                }
                else
                {
                    lblMessage.Text = "SetTrIcon Failed. (" + strResultCode
                        + ((strErr.Length > 0) ? (": " + strErr) : "")
                        + ")";
                }
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
