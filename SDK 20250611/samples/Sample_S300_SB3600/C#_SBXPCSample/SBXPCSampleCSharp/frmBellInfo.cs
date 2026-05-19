using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.Runtime.InteropServices;

namespace SBXPCSampleCSharp
{
    public partial class frmBellInfo : Form
    {
        public frmBellInfo()
        {
            InitializeComponent();
        }

        AxSBXPCLib.AxSBXPC bpc;

        const int MAX_BELLCOUNT = 42;
		int[] BellInfo;


        private void frmBellInfo_Load(object sender, EventArgs e)
        {
			BellInfo = new int[MAX_BELLCOUNT * 4];
			dtStart.ShowUpDown = true;

			for (int i = 0; i < MAX_BELLCOUNT; i++)
			{
				BellInfo[i * 4] = 0;
				BellInfo[i * 4 + 1] = 0;
				BellInfo[i * 4 + 2] = 0;
				BellInfo[i * 4 + 3] = 0;
			}
			DrawBellInfo();

            bpc = (AxSBXPCLib.AxSBXPC)Application.OpenForms["frmMain"].Controls["SBXPC1"];
        }
		private void DrawBellInfo()
		{
			string itemString = "";
			lstTimeZone.Items.Clear();
			int valid, startHour, startMinute, weekDay;
			for (int i = 0; i < MAX_BELLCOUNT; i++)
			{
				valid = BellInfo[i * 4];
				startHour = BellInfo[i * 4 + 1];
				startMinute = BellInfo[i * 4 + 2];
				weekDay = BellInfo[i * 4 + 3];

				itemString = "[" + String.Format("{0:D2} ] ", i);
				itemString += "[VALID] " + String.Format("{0:D1} ", valid);
				itemString += "[TIME] " + String.Format("{0:D2}:{1:D2} ", startHour, startMinute);
				itemString += "[WEEKDAY] " + String.Format("{0:D1} ", weekDay);
				lstTimeZone.Items.Add(itemString);
			}
		}

        private void cmdRead_Click(object sender, EventArgs e)
        {
            Boolean vRet = true;
            int vErrorCode = 0;

            lblMessage.Text = "Waiting...";
            Application.DoEvents();

            if (!bpc.EnableDevice(Program.gMachineNumber, 0)) // 0 : false
            {
                lblMessage.Text = util.gstrNoDevice;
                return;
            }

			string strXML = null;
			bpc.XML_AddString(ref strXML, "REQUEST", "GetBellTime42");
			bpc.XML_AddString(ref strXML, "MSGTYPE", "request");
			bpc.XML_AddLong(ref strXML, "MachineID", Program.gMachineNumber);

            vRet = bpc.GeneralOperationXML(ref strXML);

            if (vRet)
            {
				txtBellCount.Text = Convert.ToString(bpc.XML_ParseLong(ref strXML, "BellRingTimes"));
				txtBellPeriod.Text = Convert.ToString(bpc.XML_ParseLong(ref strXML, "BellPeriod"));

				for (int i = 0; i < MAX_BELLCOUNT; i++)
				{
					BellInfo[i * 4] = bpc.XML_ParseLong(ref strXML, String.Format("BellValid_{0:D2}", i));
					BellInfo[i * 4 + 1] = bpc.XML_ParseLong(ref strXML, String.Format("BellHour_{0:D2}", i));
					BellInfo[i * 4 + 2] = bpc.XML_ParseLong(ref strXML, String.Format("BellMin_{0:D2}", i));
					BellInfo[i * 4 + 3] = bpc.XML_ParseLong(ref strXML, String.Format("BellDay_{0:D2}", i));
				}
				DrawBellInfo();

				lblMessage.Text = "Success!";
            }
            else
            {
                bpc.GetLastError(ref vErrorCode);
                lblMessage.Text = util.ErrorPrint(vErrorCode);
            }

            bpc.EnableDevice(Program.gMachineNumber, 1); // 1 : true
        }

        private void cmdWrite_Click(object sender, EventArgs e)
        {
            Boolean vRet;
            int vErrorCode = 0;

            lblMessage.Text = "Waiting...";
            Application.DoEvents();

            if (!bpc.EnableDevice(Program.gMachineNumber, 0)) // 0 : false
            {
                lblMessage.Text = util.gstrNoDevice;
                return;
            }

			string strXML = null;
			bpc.XML_AddString(ref strXML, "REQUEST", "SetBellTime42");
			bpc.XML_AddString(ref strXML, "MSGTYPE", "request");
			bpc.XML_AddLong(ref strXML, "MachineID", Program.gMachineNumber);

			bpc.XML_AddLong(ref strXML, "BellRingTimes", Convert.ToInt32(txtBellCount.Text));
			bpc.XML_AddLong(ref strXML, "BellPeriod", Convert.ToInt32(txtBellPeriod.Text));
			for (int i = 0; i < MAX_BELLCOUNT; i++)
			{
				bpc.XML_AddLong(ref strXML, String.Format("BellValid_{0:D2}", i), BellInfo[i * 4]);
				bpc.XML_AddLong(ref strXML, String.Format("BellHour_{0:D2}", i), BellInfo[i * 4 + 1]);
				bpc.XML_AddLong(ref strXML, String.Format("BellMin_{0:D2}", i), BellInfo[i * 4 + 2]);
				bpc.XML_AddLong(ref strXML, String.Format("BellDay_{0:D2}", i), BellInfo[i * 4 + 3]);
			}

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


        private void cmdExit_Click(object sender, EventArgs e)
        {
            Close();
        }

		private void lstTimeZone_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (lstTimeZone.SelectedIndex == -1)
				return;

			int index = lstTimeZone.SelectedIndex;

			chkUsed.Checked = BellInfo[index * 4] != 0;
			dtStart.Value = new DateTime(2000, 1, 1,         // Don't care year/month/date
											BellInfo[index * 4 + 1],
											BellInfo[index * 4 + 2],
											0
										);
			cmbWeekday.SelectedIndex = BellInfo[index * 4 + 3];
		}

		private void cmdUpdate_Click(object sender, EventArgs e)
		{
            if (lstTimeZone.SelectedIndex == -1)
                return;

            int index = lstTimeZone.SelectedIndex;

			BellInfo[index * 4] = chkUsed.Checked ? 1 : 0;
			BellInfo[index * 4 + 1] = dtStart.Value.Hour;
            BellInfo[index * 4 + 2] = dtStart.Value.Minute;
            BellInfo[index * 4 + 3] = cmbWeekday.SelectedIndex;
			DrawBellInfo();
		}

		private void frmBellInfo_FormClosing(object sender, FormClosingEventArgs e)
		{
			Application.OpenForms["frmMain"].Visible = true;
		}
	}
}
