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

namespace SBXPCDLLSampleCSharp
{
    public partial class frmLog : Form
    {
        public frmLog()
        {
            InitializeComponent();

            grdSlog.DataSource = slogs_;
            grdSlog.AllowUserToAddRows = false;
            grdSlog.AllowUserToDeleteRows = false;
            grdSlog.ReadOnly = true;
            grdSlog.Visible = true;
            grdSlog.RowsAdded += (sender, e) =>
            {
                this.grdSlog.Rows[e.RowIndex].HeaderCell.Value = string.Format("{0}", e.RowIndex + 1);
                this.grdSlog.AutoResizeRowHeadersWidth(e.RowIndex, DataGridViewRowHeadersWidthSizeMode.AutoSizeToFirstHeader);
            };

            grdGlog.DataSource = glogs_;
            grdGlog.AllowUserToAddRows = false;
            grdGlog.AllowUserToDeleteRows = false;
            grdGlog.ReadOnly = true;
            grdGlog.Visible = false;
            grdGlog.RowsAdded += (sender, e) =>
            {
                this.grdGlog.Rows[e.RowIndex].HeaderCell.Value = string.Format("{0}", e.RowIndex + 1);
                this.grdGlog.AutoResizeRowHeadersWidth(e.RowIndex, DataGridViewRowHeadersWidthSizeMode.AutoSizeToFirstHeader);
            };
        }

        BindingList<SLogInfo> slogs_ = new BindingList<SLogInfo>();
        BindingList<GLogInfo> glogs_ = new BindingList<GLogInfo>();

        private void prepareSLog()
        {
            slogs_.Clear();
            grdSlog.Visible = true;
            grdGlog.Visible = false;
        }

        private void cmdSLogData_Click(object sender, EventArgs e)
        {
            Boolean vRet;
            int vErrorCode = 0;

            lblMessage.Text = "Waiting...";
            LabelTotal.Text = "Total : ";
            Application.DoEvents();

            prepareSLog();

            Cursor = System.Windows.Forms.Cursors.WaitCursor;
            vRet = sbxpc.SBXPCDLL.EnableDevice(Program.gMachineNumber, 0); // 0 : false
            if (!vRet)
            {
                lblMessage.Text = util.gstrNoDevice;
                Cursor = System.Windows.Forms.Cursors.Default;
                return;
            }

            vRet = sbxpc.SBXPCDLL.ReadSuperLogData(Program.gMachineNumber, (bool)chkReadMark.Checked ? (byte)1 : (byte)0);
            if (!vRet)
            {
                sbxpc.SBXPCDLL.GetLastError(Program.gMachineNumber, out vErrorCode);
                lblMessage.Text = util.ErrorPrint(vErrorCode);
            }
            else
            {
                if (chkAndDelete.Checked)
                    sbxpc.SBXPCDLL.EmptySuperLogData(Program.gMachineNumber);
            }

            if (vRet)
            {
                Cursor = System.Windows.Forms.Cursors.WaitCursor;
                lblMessage.Text = "Getting ...";
                Application.DoEvents();

                while (true)
                {
                    SLogInfo si = new SLogInfo();
                    vRet = sbxpc.SBXPCDLL.GetSuperLogData(Program.gMachineNumber,
                                                out si.tmno,
                                                out si.seno,
                                                out si.smno,
                                                out si.geno,
                                                out si.gmno,
                                                out si.mnpl,
                                                out si.fpno,
                                                out si.yr,
                                                out si.mon,
                                                out si.day,
                                                out si.hr,
                                                out si.min,
                                                out si.sec);
                    if (!vRet) break;
                    slogs_.Add(si);

                    LabelTotal.Text = "Total : " + slogs_.Count;
                    Application.DoEvents();
                }

                lblMessage.Text = "ReadSuperLogData OK";
            }

            Cursor = System.Windows.Forms.Cursors.Default;
            sbxpc.SBXPCDLL.EnableDevice(Program.gMachineNumber, 1); // 1 : true
        }

        private void cmdAllSLogData_Click(object sender, EventArgs e)
        {
            Boolean vRet;
            int vErrorCode = 0;

            lblMessage.Text = "Waiting...";
            LabelTotal.Text = "Total : ";
            Application.DoEvents();

            prepareSLog();

            Cursor = System.Windows.Forms.Cursors.WaitCursor;
            vRet = sbxpc.SBXPCDLL.EnableDevice(Program.gMachineNumber, 0); // 0 : false
            if (!vRet)
            {
                lblMessage.Text = util.gstrNoDevice;
                Cursor = System.Windows.Forms.Cursors.Default;
                return;
            }

            vRet = sbxpc.SBXPCDLL.ReadAllSLogData(Program.gMachineNumber);
            if (!vRet)
            {
                sbxpc.SBXPCDLL.GetLastError(Program.gMachineNumber, out vErrorCode);
                lblMessage.Text = util.ErrorPrint(vErrorCode);
            }
            else
            {
                if (chkAndDelete.Checked)
                    sbxpc.SBXPCDLL.EmptySuperLogData(Program.gMachineNumber);
            }

            if (vRet)
            {
                Cursor = System.Windows.Forms.Cursors.WaitCursor;
                lblMessage.Text = "Getting ...";
                Application.DoEvents();

                while (true)
                {
                    SLogInfo si = new SLogInfo();
                    vRet = sbxpc.SBXPCDLL.GetAllSLogData(Program.gMachineNumber,
                                                out si.tmno,
                                                out si.seno,
                                                out si.smno,
                                                out si.geno,
                                                out si.gmno,
                                                out si.mnpl,
                                                out si.fpno,
                                                out si.yr,
                                                out si.mon,
                                                out si.day,
                                                out si.hr,
                                                out si.min,
                                                out si.sec);
                    if (!vRet) break;
                    slogs_.Add(si);

                    LabelTotal.Text = "Total : " + slogs_.Count;
                    Application.DoEvents();
                }

                lblMessage.Text = "ReadAllSLogData OK";
            }

            Cursor = System.Windows.Forms.Cursors.Default;
            sbxpc.SBXPCDLL.EnableDevice(Program.gMachineNumber, 1); // 1 : true
        }

        private void cmdEmptySLog_Click(object sender, EventArgs e)
        {
            Boolean vRet;
            int vErrorCode = 0;

            lblMessage.Text = "Working...";
            Application.DoEvents();

            prepareSLog();

            vRet = sbxpc.SBXPCDLL.EnableDevice(Program.gMachineNumber, 0); // 0 : false
            if (!vRet)
            {
                lblMessage.Text = util.gstrNoDevice;
                return;
            }

            vRet = sbxpc.SBXPCDLL.EmptySuperLogData(Program.gMachineNumber);
            if (vRet)
            {
                lblMessage.Text = "EmptySuperLogData OK";
            }
            else
            {
                sbxpc.SBXPCDLL.GetLastError(Program.gMachineNumber, out vErrorCode);
                lblMessage.Text = util.ErrorPrint(vErrorCode);
            }

            sbxpc.SBXPCDLL.EnableDevice(Program.gMachineNumber, 1); // 1 : true
        }

        private void prepareGLog()
        {
            glogs_.Clear();
            grdSlog.Visible = false;
            grdGlog.Visible = true;
        }

        private void cmdGlogData_Click(object sender, EventArgs e)
        {
            Boolean vRet;
            int vErrorCode = 0;

            lblMessage.Text = "Waiting...";
            LabelTotal.Text = "Total : ";
            Application.DoEvents();

            prepareGLog();

            Cursor = System.Windows.Forms.Cursors.WaitCursor;
            vRet = sbxpc.SBXPCDLL.EnableDevice(Program.gMachineNumber, 0); // 0 : false
            if (!vRet)
            {
                lblMessage.Text = util.gstrNoDevice;
                Cursor = System.Windows.Forms.Cursors.Default;
                return;
            }

            uint DevInfo_WiegandLogMode;
            vRet = sbxpc.SBXPCDLL.GetDeviceInfo(Program.gMachineNumber, 26, out DevInfo_WiegandLogMode);    // WiegandLogMode. 0: None, 1: IN, 2: OUT
            if (!vRet)
            {
                sbxpc.SBXPCDLL.GetLastError(Program.gMachineNumber, out vErrorCode);
                lblMessage.Text = util.ErrorPrint(vErrorCode);
            }
            else
            {
                vRet = sbxpc.SBXPCDLL.ReadGeneralLogData(Program.gMachineNumber, chkReadMark.Checked ? (byte)1 : (byte)0);
                if (!vRet)
                {
                    sbxpc.SBXPCDLL.GetLastError(Program.gMachineNumber, out vErrorCode);
                    lblMessage.Text = util.ErrorPrint(vErrorCode);
                }
                else
                {
                    if (chkAndDelete.Checked)
                        sbxpc.SBXPCDLL.EmptyGeneralLogData(Program.gMachineNumber);
                }
            }
            if (vRet)
            {
                Cursor = System.Windows.Forms.Cursors.WaitCursor;
                lblMessage.Text = "Getting ...";
                Application.DoEvents();

                while (true)
                {
                    GLogInfo gi = new GLogInfo();
                    vRet = sbxpc.SBXPCDLL.GetGeneralLogData(Program.gMachineNumber,
                                                    out gi.tmno,
                                                    out gi.seno,
                                                    out gi.smno,
                                                    out gi.vmode,
                                                    out gi.yr,
                                                    out gi.mon,
                                                    out gi.day,
                                                    out gi.hr,
                                                    out gi.min,
                                                    out gi.sec);
                    if (!vRet) break;
                    gi.WiegandLogMode = DevInfo_WiegandLogMode;
                    glogs_.Add(gi);

                    LabelTotal.Text = "Total : " + glogs_.Count;
                    Application.DoEvents();
                }

                lblMessage.Text = "ReadGeneralLogData OK";
            }

            Cursor = System.Windows.Forms.Cursors.Default;
            sbxpc.SBXPCDLL.EnableDevice(Program.gMachineNumber, 1); // 1 : true
        }

        private void cmdAllGLogData_Click(object sender, EventArgs e)
        {
            Boolean vRet;
            int vErrorCode = 0;

            lblMessage.Text = "Waiting...";
            LabelTotal.Text = "Total : ";

            prepareGLog();

            Cursor = System.Windows.Forms.Cursors.WaitCursor;
            vRet = sbxpc.SBXPCDLL.EnableDevice(Program.gMachineNumber, 0); // 0 : false
            if (!vRet)
            {
                lblMessage.Text = util.gstrNoDevice;
                Cursor = System.Windows.Forms.Cursors.Default;
                return;
            }

            uint DevInfo_WiegandLogMode;
            vRet = sbxpc.SBXPCDLL.GetDeviceInfo(Program.gMachineNumber, 26, out DevInfo_WiegandLogMode);    // WiegandLogMode. 0: None, 1: IN, 2: OUT
            if (!vRet)
            {
                sbxpc.SBXPCDLL.GetLastError(Program.gMachineNumber, out vErrorCode);
                lblMessage.Text = util.ErrorPrint(vErrorCode);
            }
            else
            {
                vRet = sbxpc.SBXPCDLL.ReadAllGLogData(Program.gMachineNumber);
                if (!vRet)
                {
                    sbxpc.SBXPCDLL.GetLastError(Program.gMachineNumber, out vErrorCode);
                    lblMessage.Text = util.ErrorPrint(vErrorCode);
                }
                else
                {
                    if (chkAndDelete.Checked)
                        sbxpc.SBXPCDLL.EmptyGeneralLogData(Program.gMachineNumber);
                }
            }
            if (vRet)
            {
                Cursor = System.Windows.Forms.Cursors.WaitCursor;
                lblMessage.Text = "Getting ...";
                Application.DoEvents();

                while (true)
                {
                    GLogInfo gi = new GLogInfo();
                    vRet = sbxpc.SBXPCDLL.GetAllGLogData(Program.gMachineNumber,
                                                out gi.tmno,
                                                out gi.seno,
                                                out gi.smno,
                                                out gi.vmode,
                                                out gi.yr,
                                                out gi.mon,
                                                out gi.day,
                                                out gi.hr,
                                                out gi.min,
                                                out gi.sec);
                    if (!vRet) break;
                    gi.WiegandLogMode = DevInfo_WiegandLogMode;
                    glogs_.Add(gi);

                    LabelTotal.Text = "Total : " + glogs_.Count;
                    Application.DoEvents();
                }

                lblMessage.Text = "ReadAllGLogData OK";
            }

            Cursor = System.Windows.Forms.Cursors.Default;
            sbxpc.SBXPCDLL.EnableDevice(Program.gMachineNumber, 1); // 1 : true
        }

        private void cmdEmptyGLog_Click(object sender, EventArgs e)
        {
            Boolean vRet;
            int vErrorCode = 0;

            lblMessage.Text = "Working...";
            Application.DoEvents();

            prepareGLog();

            vRet = sbxpc.SBXPCDLL.EnableDevice(Program.gMachineNumber, 0); // 0 : false
            if (!vRet)
            {
                lblMessage.Text = util.gstrNoDevice;
                return;
            }

            vRet = sbxpc.SBXPCDLL.EmptyGeneralLogData(Program.gMachineNumber);
            if (vRet)
            {
                lblMessage.Text = "EmptyGeneralLogData OK";
            }
            else
            {
                sbxpc.SBXPCDLL.GetLastError(Program.gMachineNumber, out vErrorCode);
                lblMessage.Text = util.ErrorPrint(vErrorCode);
            }

            sbxpc.SBXPCDLL.EnableDevice(Program.gMachineNumber, 1); // 1 : true
        }

        private void cmdExit_Click(object sender, EventArgs e)
        {
            Close();
            Application.OpenForms["frmMain"].Visible = true;
        }

        private void frmLog_Load(object sender, EventArgs e)
        {
            chkReadMark.Checked = true;
        }

        private void frmLog_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.OpenForms["frmMain"].Visible = true;
        }
    }

	class SLogInfo
	{
		public int tmno = 0;
		public int seno = 0, smno = 0;
		public int geno = 0, gmno = 0;
		public int mnpl = 0;
		public int fpno = 0;
		public int yr = 0, mon = 0, day = 0, hr = 0, min = 0, sec = 0;

		public int tmachine { get { return tmno; } }
		public int senroll { get { return seno; } }
		public int smachine { get { return smno; } }
		public int genroll { get { return geno; } }
		public int gmachine { get { return gmno; } }
		public string manipulation
		{
			get
			{
				switch (mnpl)
				{
					case 1:
					case 2:
					case 3:
						return Convert.ToString(mnpl) + "--" + "Enroll User";

					case 4:
						return Convert.ToString(mnpl) + "--" + "Enroll Manager";

					case 5:
						return Convert.ToString(mnpl) + "--" + "Delete Fp Data";

					case 6:
						return Convert.ToString(mnpl) + "--" + "Delete Password";

					case 7:
						return Convert.ToString(mnpl) + "--" + "Delete Card Data";

					case 8:
						return Convert.ToString(mnpl) + "--" + "Delete All LogData";

					case 9:
						return Convert.ToString(mnpl) + "--" + "Modify System Info";

					case 10:
						return Convert.ToString(mnpl) + "--" + "Modify System Time";

					case 11:
						return Convert.ToString(mnpl) + "--" + "Modify Log Setting";

					case 12:
						return Convert.ToString(mnpl) + "--" + "Modify Comm Setting";

					case 13:
						return Convert.ToString(mnpl) + "--" + "Modify Timezone Setting";

                    case 14:
                        return Convert.ToString(mnpl) + "--" + "Delete Face";

					default:
						return Convert.ToString(mnpl) + "--" + "Unknown";
				}
			}
		}

		public string finger { get { return (fpno < 10) ? Convert.ToString(fpno) : (fpno == 10) ? "Password" : (fpno == 14) ? "Face" : "Card"; } }
		public string logtime { get { return string.Format("{0:D4}-{1:D2}-{2:D2} {3:D2}:{4:D2}:{5:D2}", yr, mon, day, hr, min, sec); } }
	}

	class GLogInfo
	{
		public int tmno;
		public int smno, seno;
		public int vmode;
		public int yr, mon, day, hr, min, sec;
        public uint WiegandLogMode;

		public string photo { get { return (tmno == -1) ? "No Photo" : Convert.ToString(tmno); } }
		public int enroll { get { return seno; } }
		public int machine { get { return smno; } }

		public string verify_mode
		{
			get
			{
				string attend_status = "";
				switch ((vmode >> 8) & 0xFF)
				{
					case 0: attend_status = "_DutyOn"; break;
					case 1: attend_status = "_DutyOff"; break;
					case 2: attend_status = "_OverOn"; break;
					case 3: attend_status = "_OverOff"; break;
					case 4: attend_status = "_GoIn"; break;
					case 5: attend_status = "_GoOut"; break;
				}

				string antipass = "";
				switch ((vmode >> 16) & 0xFF)
				{
					case 1: antipass = "(AP_In)"; break;
					case 3: antipass = "(AP_Out)"; break;
				}

                string strInOut = "";
                switch ((vmode >> 24) & 0xFF)
                {
                    case 0:
                        if (WiegandLogMode == 1)
                            strInOut = "[OUT]";
                        else if (WiegandLogMode == 2)
                            strInOut = "[IN]";
                        break;
                    case 1:
                        if (WiegandLogMode == 1)
                            strInOut = "[IN]";
                        else if (WiegandLogMode == 2)
                            strInOut = "[OUT]";
                        break;
                }

				int vm = vmode & 0xFF; 
                while (vm > 50)
                    vm -= 50;

				string str = "--";
				switch (vm)
				{
					case 1: str = "Fp"; break;
					case 2: str = "Password"; break;
					case 3: str = "Card"; break;
					case 4: str = "FP+Card"; break;
					case 5: str = "FP+Pwd"; break;
					case 6: str = "Card+Pwd"; break;
					case 7: str = "FP+Card+Pwd"; break;
					case 10: str = "Hand Lock"; break;
					case 11: str = "Prog Lock"; break;
					case 12: str = "Prog Open"; break;
					case 13: str = "Prog Close"; break;
					case 14: str = "Auto Recover"; break;
					case 20: str = "Lock Over"; break;
					case 21: str = "Illegal Open"; break;
					case 22: str = "Duress alarm"; break;
					case 23: str = "Tamper detect"; break;
                    case 30: str = "FACE"; break;
                    case 31: str = "FACE+CARD"; break;
                    case 32: str = "FACE+PWD"; break;
                    case 33: str = "FACE+CARD+PWD"; break;
                    case 34: str = "FACE+FP"; break;
				}

				if ((1 <= vm && vm <= 7) ||
                    (30 <= vm && vm <= 34))
				{
					str = str + attend_status + strInOut;
				}

				str += antipass;

				return str;
			}
		}

		public string logtime { get { return string.Format("{0:D4}-{1:D2}-{2:D2} {3:D2}:{4:D2}:{5:D2}", yr, mon, day, hr, min, sec); } }
	}
}
