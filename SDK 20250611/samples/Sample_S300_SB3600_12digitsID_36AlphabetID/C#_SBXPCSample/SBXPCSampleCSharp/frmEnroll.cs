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
    public partial class frmEnroll : Form
    {
        public frmEnroll()
        {
            InitializeComponent();
        }

        AxSBXPCLib.AxSBXPC bpc;

        int DATASIZE = (1404 + 12) / 4;
        int NAMESIZE = 54;
        int[] gTemplngEnrollData;
        Byte[] gbytEnrollData;
        Byte[] gbytEnrollData1;
        int[] gTempEnrollName;
        int glngEnrollPData;
        //        Boolean gGetState;
        ASCIIEncoding ascii;

        DataSet dsEnrolls;

        private void frmEnroll_Load(object sender, EventArgs e)
        {
            bpc = (AxSBXPCLib.AxSBXPC)Application.OpenForms["frmMain"].Controls["SBXPC1"];
            picUserPhoto.Image = null;
            gbytEnrollData = new Byte[DATASIZE * 5];
            gbytEnrollData1 = new Byte[DATASIZE * 5];
            gTemplngEnrollData = new int[DATASIZE];
            gTempEnrollName = new int[NAMESIZE];
            ascii = new ASCIIEncoding();

            EnrollData ed = new EnrollData();
            ed.New("./");
            dsEnrolls = EnrollData.DataModule.GetEnrollDatas();
        }

        private void cmdGetEnrollData_Click(object sender, EventArgs e)
        {
            string vEnrollNumber;
            int vEMachineNumber;
            int vFingerNumber;
            int vPrivilege = 0;
            Boolean vRet;
            int vErrorCode = 0;
            int i;
            string strXML;
            int vConv;

            GCHandle gh;
            IntPtr AddrOfTemplngEnrollData;

            lblEnrollData.Text = "Enrolled Data";
            lstEnrollData.Items.Clear();
            Label2.Text = "";
            lstEnrollData.Items.Clear();
            lblMessage.Text = "Working...";
            Application.DoEvents();

            vRet = bpc.EnableDevice(Program.gMachineNumber, 0); // 0 : false
            if (!vRet)
            {
                lblMessage.Text = util.gstrNoDevice;
                return;
            }

            vEnrollNumber = txtEnrollNumber.Text;
            vFingerNumber = Convert.ToInt32(cmbBackupNumber.Text);
            if (vFingerNumber == 10) vFingerNumber = 15;
            vEMachineNumber = Program.gMachineNumber;

            gh = GCHandle.Alloc(gTemplngEnrollData, GCHandleType.Pinned);
            AddrOfTemplngEnrollData = gh.AddrOfPinnedObject();

            glngEnrollPData = 0;
            int nAddr = AddrOfTemplngEnrollData.ToInt32();

            strXML = "";
            if (vFingerNumber >= 0 && vFingerNumber <= 9)
            {
                util.MakeXMLRequestHeader(bpc, ref strXML, "GetEnrollDataFP");
                bpc.XML_AddString(ref strXML, "UserID", vEnrollNumber);
                bpc.XML_AddLong(ref strXML, "FPNumber", vFingerNumber);
                vRet = bpc.GeneralOperationXML(ref strXML);
                if (vRet)
                {
                    vPrivilege = bpc.XML_ParseLong(ref strXML, "Privilege");
                    vRet = bpc.XML_ParseBinaryLong(ref strXML, "Template", ref nAddr, DATASIZE * 4);
                }
            }
            else if (vFingerNumber == 10 || vFingerNumber == 15)
            {
                if (vFingerNumber == 10) vConv = 0; else vConv = 1;
                util.MakeXMLRequestHeader(bpc, ref strXML, "GetEnrollDataPWD");
                bpc.XML_AddString(ref strXML, "UserID", vEnrollNumber);
                bpc.XML_AddLong(ref strXML, "Conv", vConv);
                vRet = bpc.GeneralOperationXML(ref strXML);
                if (vRet)
                {
                    vPrivilege = bpc.XML_ParseLong(ref strXML, "Privilege");
                    glngEnrollPData = bpc.XML_ParseLong(ref strXML, "Password");
                }
            }
            else if (vFingerNumber == 11)
            {
                util.MakeXMLRequestHeader(bpc, ref strXML, "GetEnrollDataCARD");
                bpc.XML_AddString(ref strXML, "UserID", vEnrollNumber);
                vRet = bpc.GeneralOperationXML(ref strXML);
                if (vRet)
                {
                    vPrivilege = bpc.XML_ParseLong(ref strXML, "Privilege");
                    glngEnrollPData = bpc.XML_ParseLong(ref strXML, "CardNum");
                }
            }
            else if (vFingerNumber == 14)
            {
                util.MakeXMLRequestHeader(bpc, ref strXML, "GetEnrollDataUserTZ");
                bpc.XML_AddString(ref strXML, "UserID", vEnrollNumber);
                vRet = bpc.GeneralOperationXML(ref strXML);
                if (vRet)
                {
                    vPrivilege = bpc.XML_ParseLong(ref strXML, "Privilege");
                    glngEnrollPData = bpc.XML_ParseLong(ref strXML, "UserTZ");
                }
            }
            else if (vFingerNumber == 16)
            {
                util.MakeXMLRequestHeader(bpc, ref strXML, "GetEnrollDataDepart");
                bpc.XML_AddString(ref strXML, "UserID", vEnrollNumber);
                vRet = bpc.GeneralOperationXML(ref strXML);
                if (vRet)
                {
                    vPrivilege = bpc.XML_ParseLong(ref strXML, "Privilege");
                    glngEnrollPData = bpc.XML_ParseLong(ref strXML, "Depart");
                }
            }
            gh.Free();
            if (vRet)
            {
                cmbPrivilege.SelectedIndex = vPrivilege;
                lblMessage.Text = "GetEnrollData OK";
                if (vFingerNumber == 11) // Card Number
                {
                    txtCardNumber.Text = Convert.ToString(glngEnrollPData, 16).ToUpper();
                }
                else if (vFingerNumber == 14) // User timezone
                {
                    txtUserTz1.Text = Convert.ToString(glngEnrollPData % 64); glngEnrollPData = glngEnrollPData / 64;
                    txtUserTz2.Text = Convert.ToString(glngEnrollPData % 64); glngEnrollPData = glngEnrollPData / 64;
                    txtUserTz3.Text = Convert.ToString(glngEnrollPData % 64); glngEnrollPData = glngEnrollPData / 64;
                    txtUserTz4.Text = Convert.ToString(glngEnrollPData % 64); glngEnrollPData = glngEnrollPData / 64;
                    txtUserTz5.Text = Convert.ToString(glngEnrollPData % 64); glngEnrollPData = glngEnrollPData / 64;
                }
                else if (vFingerNumber == 15) // Password
                {
                    txtCardNumber.Text = "";
                    while (glngEnrollPData > 0)
                    {
                        i = glngEnrollPData % 16 - 1;
                        txtCardNumber.Text = txtCardNumber.Text + Convert.ToString(i);
                        glngEnrollPData = glngEnrollPData / 16;
                    }
                }
                else if (vFingerNumber == 16) // User department
                {
                    txtDepart.Text = Convert.ToString(glngEnrollPData);
                }
                else // other
                {
                    for (i = 0; i < DATASIZE; i++)
                        lstEnrollData.Items.Add(Convert.ToString(gTemplngEnrollData[i]));
                }
            }
            else
            {
                bpc.GetLastError(ref vErrorCode);
                lblMessage.Text = util.ErrorPrint(vErrorCode);
            }

            bpc.EnableDevice(Program.gMachineNumber, 1); // 1 : true
        }

        private void cmdSetEnrollData_Click(object sender, EventArgs e)
        {
            string vEnrollNumber;
            int vEMachineNumber;
            int vFingerNumber;
            int vPrivilege;
            Boolean vRet;
            int vErrorCode = 0;
            GCHandle gh;
            IntPtr AddrOfTemplngEnrollData;
            string strXML;
            int vNoDup;
            int vConv;

            lblMessage.Text = "Working...";
            Application.DoEvents();

            if (txtEnrollNumber.Text == "") txtEnrollNumber.Text = "0";
            if (txtCardNumber.Text == "") txtCardNumber.Text = "0";
            
            vEnrollNumber = txtEnrollNumber.Text;
            vFingerNumber = Convert.ToInt32(cmbBackupNumber.Text);
            if (vFingerNumber == 10) vFingerNumber = 15;
            vPrivilege = Convert.ToInt32(cmbPrivilege.Text);
            vEMachineNumber = Program.gMachineNumber;

            if (vFingerNumber == 11) // Card 
            {
                glngEnrollPData = Convert.ToInt32(txtCardNumber.Text, 16);
            }
            else if (vFingerNumber == 14) // User timezone
            {
                glngEnrollPData = Convert.ToInt32(txtUserTz5.Text);
                glngEnrollPData = glngEnrollPData * 64 + Convert.ToInt32(txtUserTz4.Text);
                glngEnrollPData = glngEnrollPData * 64 + Convert.ToInt32(txtUserTz3.Text);
                glngEnrollPData = glngEnrollPData * 64 + Convert.ToInt32(txtUserTz2.Text);
                glngEnrollPData = glngEnrollPData * 64 + Convert.ToInt32(txtUserTz1.Text);
            }
            else if (vFingerNumber == 15) // Password
            {
                int i = txtCardNumber.Text.Length;
                if (i > 6) i = 6;
                glngEnrollPData = 0;
                while (i > 0)
                {
                    glngEnrollPData = glngEnrollPData * 16 + Convert.ToInt16(txtCardNumber.Text.Substring(i - 1, 1)) + 1;
                    i--;
                }
            }
            else if (vFingerNumber == 16) // User department
            {
                glngEnrollPData = Convert.ToInt32(txtDepart.Text);
            }
            
            vRet = bpc.EnableDevice(Program.gMachineNumber, 0); // 0 : false
            
            if (!vRet)
            {
                lblMessage.Text = util.gstrNoDevice;
                return;
            }

            gh = GCHandle.Alloc(gTemplngEnrollData, GCHandleType.Pinned);
            AddrOfTemplngEnrollData = gh.AddrOfPinnedObject();
            int nAddr = AddrOfTemplngEnrollData.ToInt32();

            strXML = "";
            if (vFingerNumber < 10)
            {
                util.MakeXMLRequestHeader(bpc, ref strXML, "SetEnrollDataFP");
                bpc.XML_AddString(ref strXML, "UserID", vEnrollNumber);
                bpc.XML_AddLong(ref strXML, "FPNumber", vFingerNumber);
                bpc.XML_AddLong(ref strXML, "Privilege", vPrivilege);
                vNoDup = chkDupCheck.Checked ? 0 : 1;
                bpc.XML_AddLong(ref strXML, "NoDupCheck", vNoDup);
                bpc.XML_AddBinaryLong(ref strXML, "Template", ref nAddr, DATASIZE * 4);
            }
            else if (vFingerNumber == 10 || vFingerNumber == 15)
            {
                vConv = (vFingerNumber == 10) ? 0 : 1;
                util.MakeXMLRequestHeader(bpc, ref strXML, "SetEnrollDataPWD");
                bpc.XML_AddString(ref strXML, "UserID", vEnrollNumber);
                bpc.XML_AddLong(ref strXML, "Privilege", vPrivilege);
                bpc.XML_AddLong(ref strXML, "Password", glngEnrollPData);
                bpc.XML_AddLong(ref strXML, "Conv", vConv);
            }
            else if (vFingerNumber == 11)
            {
                util.MakeXMLRequestHeader(bpc, ref strXML, "SetEnrollDataCARD");
                bpc.XML_AddString(ref strXML, "UserID", vEnrollNumber);
                bpc.XML_AddLong(ref strXML, "Privilege", vPrivilege);
                bpc.XML_AddLong(ref strXML, "CardNum", glngEnrollPData);
            }
            vRet = bpc.GeneralOperationXML(ref strXML);

            gh.Free();

            if (vRet)
            {
                lblMessage.Text = "SetEnrollData OK";
            }
            else
            {
                bpc.GetLastError(ref vErrorCode);
                lblMessage.Text = util.ErrorPrint(vErrorCode);
            }

            bpc.EnableDevice(Program.gMachineNumber, 1); // 1 : true
        }

        private void cmdDeleteEnrollData_Click(object sender, EventArgs e)
        {
            string vEnrollNumber;
            int vEMachineNumber;
            int vFingerNumber;
            Boolean vRet;
            int vErrorCode = 0;
            string strXML;

            lblMessage.Text = "Working...";
            Application.DoEvents();

            vRet = bpc.EnableDevice(Program.gMachineNumber, 0); // 0 : false
            if (!vRet)
            {
                lblMessage.Text = util.gstrNoDevice;
                return;
            }

            vEnrollNumber = txtEnrollNumber.Text;
            vEMachineNumber = Program.gMachineNumber;
            vFingerNumber = Convert.ToInt32(cmbBackupNumber.Text);

            strXML = "";
            if (vFingerNumber < 10)
            {
                util.MakeXMLRequestHeader(bpc, ref strXML, "DeleteEnrollDataFP");
                bpc.XML_AddString(ref strXML, "UserID", vEnrollNumber);
                bpc.XML_AddLong(ref strXML, "FPNumber", vFingerNumber);
            }
            else if (vFingerNumber == 10 || vFingerNumber == 15)
            {
                util.MakeXMLRequestHeader(bpc, ref strXML, "DeleteEnrollDataPWD");
                bpc.XML_AddString(ref strXML, "UserID", vEnrollNumber);
            }
            else if (vFingerNumber == 11)
            {
                util.MakeXMLRequestHeader(bpc, ref strXML, "DeleteEnrollDataCARD");
                bpc.XML_AddString(ref strXML, "UserID", vEnrollNumber);
            }
            else if (vFingerNumber == 12)
            {
                util.MakeXMLRequestHeader(bpc, ref strXML, "DeleteEnrollDataAll");
                bpc.XML_AddString(ref strXML, "UserID", vEnrollNumber);
            }
            else if (vFingerNumber == 13)
            {
                util.MakeXMLRequestHeader(bpc, ref strXML, "DeleteEnrollDataAllFP");
                bpc.XML_AddString(ref strXML, "UserID", vEnrollNumber);
            }
            vRet = bpc.GeneralOperationXML(ref strXML);
            if (vRet)
            {
                lblMessage.Text = "DeleteEnrollData OK";
            }
            else
            {
                bpc.GetLastError(ref vErrorCode);
                lblMessage.Text = util.ErrorPrint(vErrorCode);
            }

            bpc.EnableDevice(Program.gMachineNumber, 1); // 1 : true
        }

        private void cmdGetAllEnrollData_Click(object sender, EventArgs e)
        {
            int vEnrollNumber_l = 0;
            int vEnrollNumber_h = 0;
            int vFingerNumber = 0;
            int vPrivilege = 0;
            int vEnable = 0;
            int vConv;
            Boolean vFlag;
            Boolean vRet;
            int vMsgRet;
            int vErrorCode = 0;
            string vStr = "";
            int i;
            string vTitle;
            string strXML;
            string qid;

            DataTable dbEnrollTble;
            DataRow dbRow;
            DataSet dsChange;

            GCHandle gh;

            lstEnrollData.Items.Clear();
            vTitle = this.Text;
            Label2.Text = "";
            lblMessage.Text = "Working...";
            Application.DoEvents();

            vRet = bpc.EnableDevice(Program.gMachineNumber, 0); // 0 : false
            if (!vRet)
            {
                lblMessage.Text = util.gstrNoDevice;
                return;
            }

            vRet = bpc.ReadAllUserID(Program.gMachineNumber);
            if (vRet)
            {
                lblMessage.Text = "ReadAllUserID OK";
            }
            else
            {
                bpc.GetLastError(ref vErrorCode);
                lblMessage.Text = util.ErrorPrint(vErrorCode);
                bpc.EnableDevice(Program.gMachineNumber, 1); // 1 : true
                return;
            }

            //*'*/---- Get Enroll data and save into database -------------
            Cursor = System.Windows.Forms.Cursors.WaitCursor;
            vFlag = false;


            dbEnrollTble = dsEnrolls.Tables[0];

            //            gGetState = true;

            while (true)
            {
                vRet = bpc.GetAllUserID(Program.gMachineNumber, ref vEnrollNumber_l, ref vEnrollNumber_h, ref vFingerNumber, ref vPrivilege, ref vEnable);
                if (!vRet) break;
                vFlag = true;
            EEE:
//                if (vFingerNumber == 10) vFingerNumber = 15;

                gh = GCHandle.Alloc(gTemplngEnrollData, GCHandleType.Pinned);
                IntPtr AddrOfTemplngEnrollData = gh.AddrOfPinnedObject();
                int nAddr = AddrOfTemplngEnrollData.ToInt32();

                if (vFingerNumber >= 51)
                    continue;
                if (util.gIsBigUserId)
                {
                    vRet = util.GetLastBigUserId_AsString1(Program.gMachineNumber, out qid);
                }
                else
                {
                    strXML = "";
                    util.MakeXMLRequestHeader(bpc, ref strXML, "GetQIDString");
                    bpc.XML_AddLong(ref strXML, "LowPart", vEnrollNumber_l);
                    bpc.XML_AddLong(ref strXML, "HighPart", vEnrollNumber_h);
                    vRet = bpc.GeneralOperationXML(ref strXML);
                    qid = "";
                    bpc.XML_ParseString(ref strXML, "QID", ref qid);
                }

                if (vFingerNumber == 50) //name only
                {
                    string vName = "";
                    strXML = "";
                    util.MakeXMLRequestHeader(bpc, ref strXML, "GetUserName");
                    bpc.XML_AddString(ref strXML, "UserID", qid);
                    vRet = bpc.GeneralOperationXML(ref strXML);
                    if (vRet)
                    {
                        if (util.gIsS300)
                        {
                            bpc.XML_ParseBinaryAnsi2Unicode(ref strXML, "UserNameA", ref vName, 24);
                        }
                        else
                        {
                            bpc.XML_ParseBinaryUnicode(ref strXML, "UserName", ref vName, 24 * 2);
                        }
                        txtName.Text = vName;
                        lblMessage.Text = "GetUserName OK";
                    }
                    else
                    {
                        bpc.GetLastError(ref vErrorCode);
                        lblMessage.Text = util.ErrorPrint(vErrorCode);
                    }

                    if (vRet)
                    {
                        dbRow = dbEnrollTble.NewRow();
                        dbRow["EMachineNumber"] = Program.gMachineNumber;
                        dbRow["EnrollNumber"] = qid;
                        dbRow["FingerNumber"] = vFingerNumber;
                        dbRow["Privilige"] = vPrivilege;
                        dbRow["UserName"] = vName;
                        dbRow["Password1"] = 0;
                        dbEnrollTble.Rows.Add(dbRow);
                    }
                    goto FFF;
                }

                if (vRet)
                {
                    strXML = "";
                    if (vFingerNumber >= 0 && vFingerNumber <= 9)
                    {
                        util.MakeXMLRequestHeader(bpc, ref strXML, "GetEnrollDataFP");
                        bpc.XML_AddString(ref strXML, "UserID", qid);
                        bpc.XML_AddLong(ref strXML, "FPNumber", vFingerNumber);
                        vRet = bpc.GeneralOperationXML(ref strXML);
                        if (vRet)
                        {
                            vPrivilege = bpc.XML_ParseLong(ref strXML, "Privilege");
                            vRet = bpc.XML_ParseBinaryLong(ref strXML, "Template", ref nAddr, DATASIZE * 4);
                        }
                    }
                    else if (vFingerNumber == 10 || vFingerNumber == 15)
                    {
                        if (vFingerNumber == 10) vConv = 0; else vConv = 1;
                        util.MakeXMLRequestHeader(bpc, ref strXML, "GetEnrollDataPWD");
                        bpc.XML_AddString(ref strXML, "UserID", qid);
                        bpc.XML_AddLong(ref strXML, "Conv", vConv);
                        vRet = bpc.GeneralOperationXML(ref strXML);
                        if (vRet)
                        {
                            vPrivilege = bpc.XML_ParseLong(ref strXML, "Privilege");
                            glngEnrollPData = bpc.XML_ParseLong(ref strXML, "Password");
                        }
                    }
                    else if (vFingerNumber == 11)
                    {
                        util.MakeXMLRequestHeader(bpc, ref strXML, "GetEnrollDataCARD");
                        bpc.XML_AddString(ref strXML, "UserID", qid);
                        vRet = bpc.GeneralOperationXML(ref strXML);
                        if (vRet)
                        {
                            vPrivilege = bpc.XML_ParseLong(ref strXML, "Privilege");
                            glngEnrollPData = bpc.XML_ParseLong(ref strXML, "CardNum");
                        }
                    }
                }
                if (!vRet)
                {
                    vFlag = false;
                    vStr = "GetEnrollData";
                    bpc.GetLastError(ref vErrorCode);
                    vMsgRet = util.MessageBox(new IntPtr(0), util.ErrorPrint(vErrorCode) + ": Continue ?", "GetEnrollData", 4);
                    if (vMsgRet == 6/*MsgBoxResult.Yes*/)
                    {
                        goto EEE;
                    }
                    else if (vMsgRet == 7/*MsgBoxResult.Cancel*/)
                    {
                        Cursor = System.Windows.Forms.Cursors.Default;
                        bpc.EnableDevice(Program.gMachineNumber, 1); // 1 : true
                        //                        gGetState = false;
                        return;
                    }
                }
                foreach (DataRow dbRow1 in dbEnrollTble.Rows)
                {
                    if ((string)dbRow1["EnrollNumber"] == qid)
                    {
                        if ((int)dbRow1["EMachineNumber"] == Program.gMachineNumber)
                        {
                            if ((int)dbRow1["FingerNumber"] == vFingerNumber)
                            {
                                lblMessage.Text = "Double ID";
                                goto FFF;
                            }
                        }
                    }
                }

                dbRow = dbEnrollTble.NewRow();
                dbRow["EMachineNumber"] = Program.gMachineNumber;
                dbRow["EnrollNumber"] = qid;
                dbRow["FingerNumber"] = vFingerNumber;
                dbRow["Privilige"] = vPrivilege;

                if (vFingerNumber == 10)
                {
                    dbRow["Password1"] = glngEnrollPData;
                }
                else if (vFingerNumber == 15)
                {
                    dbRow["Password1"] = glngEnrollPData;
                }
                else if (vFingerNumber == 11)
                {
                    dbRow["Password1"] = glngEnrollPData;
                }
                else
                {
                    dbRow["Password1"] = 0;

                    for (i = 0; i < DATASIZE; i++)
                    {
                        gbytEnrollData[i * 5] = 1;
                        if (gTemplngEnrollData[i] < 0)
                        {
                            gbytEnrollData[i * 5] = 0;
                            gTemplngEnrollData[i] = System.Math.Abs(gTemplngEnrollData[i]);
                        }
                        gbytEnrollData[i * 5 + 1] = (Byte)(gTemplngEnrollData[i] / 256 / 256 / 256);
                        gbytEnrollData[i * 5 + 2] = (Byte)((gTemplngEnrollData[i] / 256 / 256) % 256);
                        gbytEnrollData[i * 5 + 3] = (Byte)((gTemplngEnrollData[i] / 256) % 256);
                        gbytEnrollData[i * 5 + 4] = (Byte)(gTemplngEnrollData[i] % 256);
                    }

                    //dbRow("FPdata") = gbytEnrollData        '<---------- Error

                    Byte[] gbyt = new Byte[DATASIZE * 5];
                    for (i = 0; i < DATASIZE * 5; i++)
                        gbyt[i] = gbytEnrollData[i];
                    dbRow["FPdata"] = gbyt;

                }
                dbEnrollTble.Rows.Add(dbRow);

            FFF:

                lblMessage.Text = String.Format("{0:D2}", Program.gMachineNumber) + "-" + qid + "-" + vFingerNumber;
                this.Text = qid;
                txtEnrollNumber.Text = qid;
                cmbBackupNumber.Text = Convert.ToString(vFingerNumber);
                cmbPrivilege.Text = Convert.ToString(vPrivilege);
                Application.DoEvents();
            }

            Label2.Text = "Total : " + dsEnrolls.Tables["tblEnroll"].Rows.Count;
            dsChange = dsEnrolls.GetChanges();
            EnrollData.DataModule.SaveEnrolls(dsEnrolls);

            //            gh.Free();


            //            gGetState = false; 

            vTitle = this.Text;
            Cursor = System.Windows.Forms.Cursors.Default;

            if (vFlag)
                lblMessage.Text = "GetAllUserID OK";
            else
                lblMessage.Text = vStr + ":" + util.ErrorPrint(vErrorCode);

            Application.DoEvents();
            bpc.EnableDevice(Program.gMachineNumber, 1); // 1 : true
        }

        private void cmdSetAllEnrollData_Click(object sender, EventArgs e)
        {
            string vEnrollNumber;
            int vEMachineNumber;
            int vFingerNumber;
            int vPrivilege;
            Boolean vRet;
            int vErrorCode = 0;
            Byte[] vByte;
            int i;
            string vTitle;
            DataTable dbEnrollTble;
            GCHandle gh;
            int num;
            int vConv;
            int vNoDup;
            string strXML;

            lstEnrollData.Items.Clear();
            vTitle = this.Text;
            lblMessage.Text = "Working...";
            Application.DoEvents();

            vRet = bpc.EnableDevice(Program.gMachineNumber, 0); // 0 : false
            if (!vRet)
            {
                lblMessage.Text = util.gstrNoDevice;
                return;
            }

            Cursor = System.Windows.Forms.Cursors.WaitCursor;


            dbEnrollTble = dsEnrolls.Tables[0];

            num = 0;


            if (dbEnrollTble.Rows.Count == 0) goto EEE;

            foreach (DataRow dbRow in dbEnrollTble.Rows)
            {
                vEMachineNumber = (int)dbRow["EMachineNumber"];
                vEnrollNumber = (string)dbRow["EnrollNumber"];
                vFingerNumber = (int)dbRow["FingerNumber"];
                vPrivilege = (int)dbRow["Privilige"];
                glngEnrollPData = (int)dbRow["Password1"];

                num = num + 1;
                if (vFingerNumber == 50) //name only
                {
                    string vName = dbRow["UserName"] as string;
                    strXML = "";
                    util.MakeXMLRequestHeader(bpc, ref strXML, "SetUserName");
                    bpc.XML_AddString(ref strXML, "UserID", vEnrollNumber);
                    bpc.XML_AddBinaryUnicode(ref strXML, "UserName", ref vName);
                    if (util.gIsS300)
                        bpc.XML_AddBinaryNameGlyph(Program.gMachineNumber, ref strXML, ref vName);
                    vRet = bpc.GeneralOperationXML(ref strXML);
                    goto LLL;
                }
                if (vFingerNumber < 10)
                {
                    vByte = (Byte[])dbRow["FPData"];

                    for (i = 0; i < DATASIZE; i++)
                    {
                        gTemplngEnrollData[i] = vByte[i * 5 + 1];
                        gTemplngEnrollData[i] = gTemplngEnrollData[i] * 256 + vByte[i * 5 + 2];
                        gTemplngEnrollData[i] = gTemplngEnrollData[i] * 256 + vByte[i * 5 + 3];
                        gTemplngEnrollData[i] = gTemplngEnrollData[i] * 256 + vByte[i * 5 + 4];
                        if (vByte[i * 5] == 0)
                            gTemplngEnrollData[i] = 0 - gTemplngEnrollData[i];
                    }
                }
            FFF:

                gh = GCHandle.Alloc(gTemplngEnrollData, GCHandleType.Pinned);
                IntPtr AddrOfTemplngEnrollData = gh.AddrOfPinnedObject();
                int nAddr = AddrOfTemplngEnrollData.ToInt32();

                int vFingerNumber2 = vFingerNumber;
                if (vFingerNumber2 >= 0 && vFingerNumber2 <= 9 && !chkDupCheck.Checked)
                    vFingerNumber2 += 20;

                strXML = "";
                if (vFingerNumber < 10)
                {
                    util.MakeXMLRequestHeader(bpc, ref strXML, "SetEnrollDataFP");
                    bpc.XML_AddString(ref strXML, "UserID", vEnrollNumber);
                    bpc.XML_AddLong(ref strXML, "FPNumber", vFingerNumber);
                    bpc.XML_AddLong(ref strXML, "Privilege", vPrivilege);
                    vNoDup = chkDupCheck.Checked ? 0 : 1;
                    bpc.XML_AddLong(ref strXML, "NoDupCheck", vNoDup);
                    bpc.XML_AddBinaryLong(ref strXML, "Template", ref nAddr, DATASIZE * 4);
                }
                else if (vFingerNumber == 10 || vFingerNumber == 15)
                {
                    vConv = (vFingerNumber == 10) ? 0 : 1;
                    util.MakeXMLRequestHeader(bpc, ref strXML, "SetEnrollDataPWD");
                    bpc.XML_AddString(ref strXML, "UserID", vEnrollNumber);
                    bpc.XML_AddLong(ref strXML, "Privilege", vPrivilege);
                    bpc.XML_AddLong(ref strXML, "Password", glngEnrollPData);
                    bpc.XML_AddLong(ref strXML, "Conv", vConv);
                }
                else if (vFingerNumber == 11)
                {
                    util.MakeXMLRequestHeader(bpc, ref strXML, "SetEnrollDataCARD");
                    bpc.XML_AddString(ref strXML, "UserID", vEnrollNumber);
                    bpc.XML_AddLong(ref strXML, "Privilege", vPrivilege);
                    bpc.XML_AddLong(ref strXML, "CardNum", glngEnrollPData);
                }
                vRet = bpc.GeneralOperationXML(ref strXML);

                gh.Free();

                if (!vRet)
                {
                    bpc.GetLastError(ref vErrorCode);
                    int vMsgRet = util.MessageBox(new IntPtr(0), util.ErrorPrint(vErrorCode) + ": Continue ?", "SetEnrollData", 4);
                    if (vMsgRet == 6/*Yes Button*/) goto FFF;
                    if (vMsgRet == 7/*No Button*/) goto EEE;
                }

        LLL:
                lblMessage.Text = "EMachine = " + Convert.ToString(vEMachineNumber) + ", ID = " + vEnrollNumber + ", FpNo = " + vFingerNumber + ", Count = " + num;

                this.Text = Convert.ToString(num);
                Application.DoEvents();
            }
        EEE:
            vTitle = this.Text;
            Cursor = System.Windows.Forms.Cursors.Default;
            //            gGetState = false;

            lblMessage.Text = "SetAllUserData OK";
            Application.DoEvents();

            bpc.EnableDevice(Program.gMachineNumber, 1); // 1 : true
        }

        private void cmdGetName_Click(object sender, EventArgs e)
        {
            string vEnrollNumber;
            int vEMachineNumber;
            Boolean vRet;
            int vErrorCode = 0;
            string vName = "";
            string strXML;

            lblMessage.Text = "Working...";
            Application.DoEvents();

            vRet = bpc.EnableDevice(Program.gMachineNumber, 0); // 0 : false
            if (!vRet)
            {
                lblMessage.Text = util.gstrNoDevice;
                return;
            }

            vEnrollNumber = txtEnrollNumber.Text;
            vEMachineNumber = Program.gMachineNumber;

            strXML = "";
            util.MakeXMLRequestHeader(bpc, ref strXML, "GetUserName");
            bpc.XML_AddString(ref strXML, "UserID", vEnrollNumber);
            vRet = bpc.GeneralOperationXML(ref strXML);
            if (vRet)
            {
                if (util.gIsS300)
                {
                    bpc.XML_ParseBinaryAnsi2Unicode(ref strXML, "UserNameA", ref vName, 24);
                }
                else
                {
                    bpc.XML_ParseBinaryUnicode(ref strXML, "UserName", ref vName, 24 * 2);
                }
                txtName.Text = vName;
                lblMessage.Text = "GetUserName OK";
            }
            else
            {
                bpc.GetLastError(ref vErrorCode);
                lblMessage.Text = util.ErrorPrint(vErrorCode);
            }

            bpc.EnableDevice(Program.gMachineNumber, 1); // 1 : true
        }

        private void cmdSetName_Click(object sender, EventArgs e)
        {
            string vEnrollNumber;
            int vEMachineNumber;
            Boolean vRet;
            int vErrorCode = 0;
            string vName = "";
            string strXML;

            lblMessage.Text = "Working...";
            Application.DoEvents();

            vRet = bpc.EnableDevice(Program.gMachineNumber, 0); // 0 : false
            if (!vRet)
            {
                lblMessage.Text = util.gstrNoDevice;
                return;
            }

            vEnrollNumber = txtEnrollNumber.Text;
            vEMachineNumber = Program.gMachineNumber;
            vName = txtName.Text;
            strXML = "";
            util.MakeXMLRequestHeader(bpc, ref strXML, "SetUserName");
            bpc.XML_AddString(ref strXML, "UserID", vEnrollNumber);
            bpc.XML_AddBinaryUnicode(ref strXML, "UserName", ref vName);
            if (util.gIsS300)
                bpc.XML_AddBinaryNameGlyph(Program.gMachineNumber, ref strXML, ref vName);
            vRet = bpc.GeneralOperationXML(ref strXML);
            if (vRet)
            {
                lblMessage.Text = "SetUserName OK";
            }
            else
            {
                bpc.GetLastError(ref vErrorCode);
                lblMessage.Text = util.ErrorPrint(vErrorCode);
            }

            bpc.EnableDevice(Program.gMachineNumber, 1); // 1 : true
        }
        private void cmdGetCompany_Click(object sender, EventArgs e)
        {
            int vEMachineNumber;
            Boolean vRet;
            int vErrorCode = 0;
            string vName = "";


            lblMessage.Text = "Working...";
            Application.DoEvents();

            vRet = bpc.EnableDevice(Program.gMachineNumber, 0); // 0 : false
            if (!vRet)
            {
                lblMessage.Text = util.gstrNoDevice;
                return;
            }

            vEMachineNumber = Program.gMachineNumber;

            vRet = bpc.GetCompanyName1(Program.gMachineNumber, ref vName);
            if (vRet)
            {
                txtName.Text = vName;
                lblMessage.Text = "Get Company Name OK";
            }
            else
            {
                bpc.GetLastError(ref vErrorCode);
                lblMessage.Text = util.ErrorPrint(vErrorCode);
            }

            bpc.EnableDevice(Program.gMachineNumber, 1); // 1 : true
        }
        private void cmdSetCompany_Click(object sender, EventArgs e)
        {
            int vEMachineNumber;
            Boolean vRet;
            int vErrorCode = 0;
            string vName = "";


            lblMessage.Text = "Working...";
            Application.DoEvents();

            vRet = bpc.EnableDevice(Program.gMachineNumber, 0); // 0 : false
            if (!vRet)
            {
                lblMessage.Text = util.gstrNoDevice;
                return;
            }

            vEMachineNumber = Program.gMachineNumber;

            vName = txtName.Text;
            vRet = bpc.SetCompanyName1(Program.gMachineNumber, 1, ref vName);
            if (vRet)
            {
                lblMessage.Text = "Set Company Name OK";
            }
            else
            {
                bpc.GetLastError(ref vErrorCode);
                lblMessage.Text = util.ErrorPrint(vErrorCode);
            }

            bpc.EnableDevice(Program.gMachineNumber, 1); // 1 : true
        }

        private void cmdDeleteCompany_Click(object sender, EventArgs e)
        {
            int vEMachineNumber;
            Boolean vRet;
            int vErrorCode = 0;
            string vName = "";


            lblMessage.Text = "Working...";
            Application.DoEvents();

            vRet = bpc.EnableDevice(Program.gMachineNumber, 0); // 0 : false
            if (!vRet)
            {
                lblMessage.Text = util.gstrNoDevice;
                return;
            }

            vEMachineNumber = Program.gMachineNumber;

            vRet = bpc.SetCompanyName1(Program.gMachineNumber, 0, ref vName);
            if (vRet)
            {
                lblMessage.Text = "Delete Company Name OK";
            }
            else
            {
                bpc.GetLastError(ref vErrorCode);
                lblMessage.Text = util.ErrorPrint(vErrorCode);
            }

            bpc.EnableDevice(Program.gMachineNumber, 1); // 1 : true
        }

        private void cmdGetEnrollInfo_Click(object sender, EventArgs e)
        {
            int vEnrollNumber_l = 0;
            int vEnrollNumber_h = 0;
            int vFingerNumber = 0;
            int vPrivilege = 0;
            int vEnable = 0;
            Boolean vRet;
            Boolean vFlag;
            int vErrorCode = 0;
            int i;
            string strXML;
            string qid = "";

            lblEnrollData.Text = "User IDs";
            lstEnrollData.Items.Clear();
            lblMessage.Text = "Working...";
            Application.DoEvents();

            vRet = bpc.EnableDevice(Program.gMachineNumber, 0); // 0 : false
            if (!vRet)
            {
                lblMessage.Text = util.gstrNoDevice;
                return;
            }

            vRet = bpc.ReadAllUserID(Program.gMachineNumber);
            if (vRet)
            {
                lblMessage.Text = "ReadAllUserID OK";
            }
            else
            {
                bpc.GetLastError(ref vErrorCode);
                lblMessage.Text = util.ErrorPrint(vErrorCode);
                bpc.EnableDevice(Program.gMachineNumber, 1); // 1 : true
                return;
            }

            //------ Show all enroll information ----------
            vFlag = false;
            i = 0;
            lstEnrollData.Items.Add(("No.  EnNo   EMNo   Fp   Priv  Enable Duress"));
            while (true)
            {
                vRet = bpc.GetAllUserID(Program.gMachineNumber, ref vEnrollNumber_l, ref vEnrollNumber_h, ref vFingerNumber, ref vPrivilege, ref vEnable);
                if (!vRet) break;
                if (util.gIsBigUserId)
                {
                    if (!util.GetLastBigUserId_AsString1(Program.gMachineNumber, out qid))
                        break;
                }
                else
                {
                    strXML = "";
                    util.MakeXMLRequestHeader(bpc, ref strXML, "GetQIDString");
                    bpc.XML_AddLong(ref strXML, "LowPart", vEnrollNumber_l);
                    bpc.XML_AddLong(ref strXML, "HighPart", vEnrollNumber_h);
                    vRet = bpc.GeneralOperationXML(ref strXML);
                    if (!vRet) break;
                    qid = "";
                    bpc.XML_ParseString(ref strXML, "QID", ref qid);
                }
                vFlag = true;
                lstEnrollData.Items.Add((String.Format("{0:D5}", i) + "    " + qid + "    " + String.Format("{0:D3}", Program.gMachineNumber) + "    " + String.Format("{0:D2}", vFingerNumber) + "    " + Convert.ToString(vPrivilege) + "    " + Convert.ToString(vEnable % 256) + "     " + Convert.ToString(vEnable / 256)));

                i = i + 1;
                Label2.Text = "Total : " + i;
            }

            if (vFlag)
                lblMessage.Text = "GetAllUserID OK";
            else
                lblMessage.Text = util.ErrorPrint(vErrorCode);

            bpc.EnableDevice(Program.gMachineNumber, 1); // 1 : true
        }

        private void cmdEnableUser_Click(object sender, EventArgs e)
        {
            string vEnrollNumber;
            int vEMachineNumber;
            int vFingerNumber;
            Boolean vRet;
            int vFlag;
            int vErrorCode = 0;
            string strXML;

            lblMessage.Text = "Working...";
            Application.DoEvents();

            vEMachineNumber = Program.gMachineNumber;
            vEnrollNumber = txtEnrollNumber.Text;
            vFingerNumber = Convert.ToInt32(cmbBackupNumber.Text);
            vFlag = chkDisable.Checked ? 0 : 1;

            vRet = bpc.EnableDevice(Program.gMachineNumber, 0);
            if (!vRet)
            {
                lblMessage.Text = util.gstrNoDevice;
                return;
            }
			strXML = "";
            util.MakeXMLRequestHeader(bpc, ref strXML, "EnableUser");
            bpc.XML_AddString(ref strXML, "UserID", vEnrollNumber);
            bpc.XML_AddInt(ref strXML, "Enable", vFlag);
            vRet = bpc.GeneralOperationXML(ref strXML);
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
        }

        private void cmdModifyPrivilege_Click(object sender, EventArgs e)
        {
            string vEnrollNumber;
            int vEMachineNumber;
            int vFingerNumber;
            int vMachinePrivilege;
            Boolean vRet;
            int vErrorCode = 0;
            string strXML;

            lblMessage.Text = "Working...";
            Application.DoEvents();

            vEMachineNumber = Program.gMachineNumber;
            vEnrollNumber = txtEnrollNumber.Text;
            vFingerNumber = Convert.ToInt32(cmbBackupNumber.Text);
            vMachinePrivilege = Convert.ToInt32(cmbPrivilege.Text);

            vRet = bpc.EnableDevice(Program.gMachineNumber, 0); // 0 : false
            if (!vRet)
            {
                lblMessage.Text = util.gstrNoDevice;
                return;
            }

            strXML = "";
            util.MakeXMLRequestHeader(bpc, ref strXML, "ModifyPrivilege");
            bpc.XML_AddString(ref strXML, "UserID", vEnrollNumber);
            bpc.XML_AddInt(ref strXML, "Privilege", vMachinePrivilege);
            vRet = bpc.GeneralOperationXML(ref strXML);
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
        }

        private void cmdEmptyEnrollData_Click(object sender, EventArgs e)
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

            vRet = bpc.EmptyEnrollData(Program.gMachineNumber);
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
        }

        private void cmdClearData_Click(object sender, EventArgs e)
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

            vRet = bpc.ClearKeeperData(Program.gMachineNumber);
            if (vRet)
            {
                lblMessage.Text = "ClearKeeperData OK!";
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

        private void frmEnroll_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.OpenForms["frmMain"].Visible = true;
            ClearUserPhoto();
        }

        private void cmdDel_Click(object sender, EventArgs e)
        {
            EnrollData.DataModule.DeleteDB();

            Label2.Text = "Total : 0";
            lblMessage.Text = "Deleted PC Database";
        }

        private void cmdModifyDuressFP_Click(object sender, EventArgs e)
        {
            string vEnrollNumber;
            int vFingerNumber;
            int vDuressSetting;
            bool bRet;
            int vErrorCode = 0;
            string strXML;

            lblMessage.Text = "Working...";
            Application.DoEvents();

            bRet = bpc.EnableDevice(Program.gMachineNumber, 0); // 0 : disable
            if(!bRet)
            {
                lblMessage.Text = util.gstrNoDevice;
                return;
            }

            vEnrollNumber = txtEnrollNumber.Text;
            vFingerNumber = Convert.ToInt32(cmbBackupNumber.Text);
            vDuressSetting = Convert.ToInt32(cmbDuressSetting.Text);

            strXML = "";
            util.MakeXMLRequestHeader(bpc, ref strXML, "ModifyDuressFP");
            bpc.XML_AddString(ref strXML, "UserID", vEnrollNumber);
            bpc.XML_AddInt(ref strXML, "FPNumber", vFingerNumber);
            bpc.XML_AddInt(ref strXML, "Status", vDuressSetting);
            bRet = bpc.GeneralOperationXML(ref strXML);
            if (bRet)
                lblMessage.Text = "ModifyDuressFP OK";
            else
            {
                bpc.GetLastError(ref vErrorCode);
                lblMessage.Text = util.ErrorPrint(vErrorCode);
            }

            bRet = bpc.EnableDevice(Program.gMachineNumber, 1); // 1 : enable
        }

        private void cmdGetUserPhoto_Click(object sender, EventArgs e)
        {
            string vEnrollNumber;
            bool bRet;
            string strXML = "";
            int vErrorCode = 0;

            txtUserPhotoFile.Text = "";
            
            lblMessage.Text = "Working...";
            Application.DoEvents();

            bRet = bpc.EnableDevice(Program.gMachineNumber, 0); // 0 : disable

            if(!bRet)
            {
                lblMessage.Text = util.gstrNoDevice;
                return;
            }

            vEnrollNumber = txtEnrollNumber.Text;
            util.MakeXMLRequestHeader(bpc, ref strXML, "GetUserPhotoData");
            bpc.XML_AddString(ref strXML, "UserID", vEnrollNumber);

            bRet = bpc.GeneralOperationXML(ref strXML);

            if(!bRet)
            {
                bpc.GetLastError(ref vErrorCode);
                lblMessage.Text = util.ErrorPrint(vErrorCode);
                ClearUserPhoto();
                goto _lexit;
            }

            lblMessage.Text = "GetUserPhotoData OK";
            Byte[] photoData = new Byte[util.gCompressPhotoSize];
            GCHandle gh = GCHandle.Alloc(photoData, GCHandleType.Pinned);
            IntPtr AddrOfPhotoData = gh.AddrOfPinnedObject();
            int nAddr = AddrOfPhotoData.ToInt32();

            bRet = bpc.XML_ParseBinaryLong(ref strXML, "PhotoData", ref nAddr, util.gCompressPhotoSize);

            ClearUserPhoto();

            if (!bRet)
            {
                lblMessage.Text = "GetGlogPhotoData - XML Parse Error";
                goto _lexit;
            }
            
            if (File.Exists(util.gTempPhotoFile))
                File.Delete(util.gTempPhotoFile);
            FileStream FS = File.Create(util.gTempPhotoFile);
            FS.Write(photoData, 0, util.gCompressPhotoSize);
            FS.Close();
            FS.Dispose();
            FS = null;

            picUserPhoto.Image = Image.FromFile(util.gTempPhotoFile);
            txtUserPhotoFile.Text = util.gTempPhotoFile;

        _lexit:
            bRet = bpc.EnableDevice(Program.gMachineNumber, 1); // 1 : enable
        }

        private void cmdSetUserPhoto_Click(object sender, EventArgs e)
        {
            bool bRet;
            int vErrorCode = 0;
            string strXML = "";
            string vEnrollNumber;

            string photoFileName = txtUserPhotoFile.Text;
            if (!File.Exists(photoFileName))
            {
                lblMessage.Text = "Can not find the photo file.";
                return;
            }

            lblMessage.Text = "Working...";
            Application.DoEvents();

            bRet = bpc.EnableDevice(Program.gMachineNumber, 0);

            if(!bRet)
            {
                lblMessage.Text = util.gstrNoDevice;
                return;
            }

            ClearUserPhoto();

            FileStream FS = File.Open(photoFileName, FileMode.Open, FileAccess.Read, FileShare.None);
            if(FS.Length != util.gCompressPhotoSize)
            {
                lblMessage.Text = "Photo file size is not" + Convert.ToString(util.gCompressPhotoSize) + "Byte";
                return;
            }

            Byte[] photoData = new Byte[util.gCompressPhotoSize];
            FS.Read(photoData, 0, util.gCompressPhotoSize);
            FS.Close();
            FS.Dispose();
            FS = null;

            picUserPhoto.Image = Image.FromFile(photoFileName);

            GCHandle gh = GCHandle.Alloc(photoData, GCHandleType.Pinned);
            IntPtr AddrOfPhotoData = gh.AddrOfPinnedObject();
            int nAddr = AddrOfPhotoData.ToInt32();

            vEnrollNumber = txtEnrollNumber.Text;

            util.MakeXMLRequestHeader(bpc, ref strXML, "SetUserPhotoData");
            bpc.XML_AddString(ref strXML, "UserID", vEnrollNumber);
            bpc.XML_AddBinaryLong(ref strXML, "PhotoData", ref nAddr, util.gCompressPhotoSize);

            bRet = bpc.GeneralOperationXML(ref strXML);

            if (bRet)
                lblMessage.Text = "SetUserPhotoData OK";
            else
            {
                bpc.GetLastError(ref vErrorCode);
                lblMessage.Text = util.ErrorPrint(vErrorCode);
            }

            bRet = bpc.EnableDevice(Program.gMachineNumber, 1); // 1 : enable
        }

        private void cmdUserPhotoBrowse_Click(object sender, EventArgs e)
        {
            OpenFileDlg.ShowDialog();
            txtUserPhotoFile.Text = OpenFileDlg.FileName;
            ClearUserPhoto();
            if (!File.Exists(txtUserPhotoFile.Text))
                return;

            picUserPhoto.Image = Image.FromFile(txtUserPhotoFile.Text);
        }

        private void ClearUserPhoto()
        {
            if (picUserPhoto.Image != null) picUserPhoto.Image.Dispose();
            picUserPhoto.Image = null;
        }

        private void cmdDeleteUserPhoto_Click(object sender, EventArgs e)
        {
            bool bRet;
            int vErrorCode = 0;
            string strXML = "";
            string vEnrollNumber;

            bRet = bpc.EnableDevice(Program.gMachineNumber, 0);

            if (!bRet)
            {
                lblMessage.Text = util.gstrNoDevice;
                return;
            }
            
            string photoFileName = txtUserPhotoFile.Text;
            if (!File.Exists(photoFileName))
            {
                lblMessage.Text = "Can not find the photo file.";
                return;
            }

            vEnrollNumber = txtEnrollNumber.Text;

            util.MakeXMLRequestHeader(bpc, ref strXML, "SetUserPhotoData");
            bpc.XML_AddString(ref strXML, "UserID", vEnrollNumber);
            // Don't make "PhotoData" tag to delete user photo

            bRet = bpc.GeneralOperationXML(ref strXML);

            if (bRet)
                lblMessage.Text = "DeleteUserPhotoData OK";
            else
            {
                bpc.GetLastError(ref vErrorCode);
                lblMessage.Text = util.ErrorPrint(vErrorCode);
            }

            bRet = bpc.EnableDevice(Program.gMachineNumber, 1); // 1 : enable
        }


		private void btnGetUserPeriod_Click(object sender, EventArgs e)
		{
			Boolean bRet;
			int vErrorCode = 0;
			int lUserID, lStartDate, lEndDate;
			int yy, mm, dd;
			String strXML = "";

			lblMessage.Text = "Working...";
			Application.DoEvents();

			bRet = bpc.EnableDevice(Program.gMachineNumber, 0);
			if (!bRet)
			{
				lblMessage.Text = util.gstrNoDevice;
				return;
			}

			lUserID = Convert.ToInt32(txtEnrollNumber.Text);

			bpc.XML_AddString(ref strXML, "REQUEST", "GetUserPeriod");
			bpc.XML_AddString(ref strXML, "MSGTYPE", "request");
			bpc.XML_AddInt(ref strXML, "MachineID", Program.gMachineNumber);
			bpc.XML_AddInt(ref strXML, "UserID", lUserID);

			bRet = bpc.GeneralOperationXML(ref strXML);

			if (bRet)
			{
				chkUsePeriod.Checked = (bpc.XML_ParseInt(ref strXML, "Used") != 0);
				lStartDate = bpc.XML_ParseInt(ref strXML, "Start");
				lEndDate = bpc.XML_ParseInt(ref strXML, "End");
				if (chkUsePeriod.Checked)
				{
					yy = lStartDate / 256 / 256;
					mm = (lStartDate - yy * 256 * 256) / 256;
					dd = lStartDate & 0xFF;
					dtPeriodFrom.Value = new DateTime(yy + 2000, mm, dd);

					yy = lEndDate / 256 / 256;
					mm = (lEndDate - yy * 256 * 256) / 256;
					dd = lEndDate & 0xFF;
					dtPeriodTo.Value = new DateTime(yy + 2000, mm, dd);
				}
				else
				{
					dtPeriodFrom.Enabled = false;
					dtPeriodTo.Enabled = false;
				}
				lblMessage.Text = "Success!";
			}
			else
			{
				bpc.GetLastError(ref vErrorCode);
				lblMessage.Text = util.ErrorPrint(vErrorCode);
			}
			bpc.EnableDevice(Program.gMachineNumber, 1);
		}

		private void btnSetUserPeriod_Click(object sender, EventArgs e)
		{
			Boolean bRet;
			int vErrorCode = 0;
			int lUserID;
			int lStartPeriod, lEndPeriod;
			String strXML = "";

			lblMessage.Text = "Waiting...";
			Application.DoEvents();

			bRet = bpc.EnableDevice(Program.gMachineNumber, 0);
			if (!bRet)
			{
				lblMessage.Text = util.gstrNoDevice;
				return;
			}

			lUserID = Convert.ToInt32(txtEnrollNumber.Text);

			bpc.XML_AddString(ref strXML, "REQUEST", "SetUserPeriod");
			bpc.XML_AddString(ref strXML, "MSGTYPE", "request");
			bpc.XML_AddInt(ref strXML, "MachineID", Program.gMachineNumber);
			bpc.XML_AddInt(ref strXML, "UserID", lUserID);

			bpc.XML_AddInt(ref strXML, "Used", chkUsePeriod.Checked ? 1 : 0);
			if (chkUsePeriod.Checked)
			{
				lStartPeriod = (dtPeriodFrom.Value.Year - 2000) * 256 * 256 + dtPeriodFrom.Value.Month * 256 + dtPeriodFrom.Value.Day;
				lEndPeriod = (dtPeriodTo.Value.Year - 2000) * 256 * 256 + dtPeriodTo.Value.Month * 256 + dtPeriodTo.Value.Day;
			}
			else
			{
				lStartPeriod = 1 * 256 + 1;
				lEndPeriod = 1 * 256 + 1;
			}
			bpc.XML_AddInt(ref strXML, "Start", lStartPeriod);
			bpc.XML_AddInt(ref strXML, "End", lEndPeriod);

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

		private void chkUsePeriod_CheckedChanged(object sender, EventArgs e)
		{
			if (chkUsePeriod.Checked)
			{
				dtPeriodFrom.Enabled = true;
				dtPeriodTo.Enabled = true;
			}
			else
			{
				dtPeriodFrom.Enabled = false;
				dtPeriodTo.Enabled = false;
			}
		}
    }
}
