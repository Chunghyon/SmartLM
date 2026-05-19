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
    public partial class frmAccessTz : Form
    {
        public frmAccessTz()
        {
            InitializeComponent();
        }

        AxSBXPCLib.AxSBXPC bpc;
        const int DBTIMEZONE_COUNT = 50;
        const int DBTIMESECTION_COUNT = 8;
        const int DBALLCOUNT = DBTIMEZONE_COUNT * DBTIMESECTION_COUNT;

        Byte[] timeZoneInfo;
        private void frmAccessTz_Load(object sender, EventArgs e)
        {
            bpc = (AxSBXPCLib.AxSBXPC) Application.OpenForms["frmMain"].Controls["SBXPC1"];
            timeZoneInfo = new Byte[DBALLCOUNT * 5];
            dtStart.ShowUpDown = true;
            dtEnd.ShowUpDown = true;
            TimeZoneInit();
            DrawTimeZone();
        }

        private void TimeZoneInit()
        {
            for(int i = 0; i < DBTIMEZONE_COUNT; i ++)
            {
                for(int j = 0; j < DBTIMESECTION_COUNT; j ++)
                {
                    timeZoneInfo[(i * DBTIMESECTION_COUNT + j) * 5] = 0;
                    timeZoneInfo[(i * DBTIMESECTION_COUNT + j) * 5 + 1] = 0;
                    timeZoneInfo[(i * DBTIMESECTION_COUNT + j) * 5 + 2] = 23;
                    timeZoneInfo[(i * DBTIMESECTION_COUNT + j) * 5 + 3] = 59;
                    timeZoneInfo[(i * DBTIMESECTION_COUNT + j) * 5 + 4] = 0;
                }

            }
        }

        private void DrawTimeZone()
        {
            string itemString = "";
            lstTimeZone.Items.Clear();
            int startHour, startMinute, endHour, endMinute, verifyMode;
            for (int i = 0; i < DBTIMEZONE_COUNT; i ++ )
            {
                for (int j = 0; j < DBTIMESECTION_COUNT; j ++ )
                {
                    startHour   = timeZoneInfo[(i * DBTIMESECTION_COUNT + j) * 5];
                    startMinute = timeZoneInfo[(i * DBTIMESECTION_COUNT + j) * 5 + 1];
                    endHour     = timeZoneInfo[(i * DBTIMESECTION_COUNT + j) * 5 + 2];
                    endMinute   = timeZoneInfo[(i * DBTIMESECTION_COUNT + j) * 5 + 3];
                    verifyMode = timeZoneInfo[(i * DBTIMESECTION_COUNT + j) * 5 + 4];

                    itemString  = "[Tz.]" + String.Format("{0:D2}-{1:D1} ", i, j);
                    itemString += "[S]" + String.Format("{0:D2}:{1:D2} ", startHour, startMinute);
                    itemString += "[E]" + String.Format("{0:D2}:{1:D2}", endHour, endMinute) + " ";
                    itemString += "[VM]" + cmbVerifyMode.Items[verifyMode];
                    lstTimeZone.Items.Add(itemString);
                }
            }
        }

        private void cmdRead_Click(object sender, EventArgs e)
        {
            Boolean bRet;
            int vErrorCode = 0;
            String strXML = "";

            lblMessage.Text = "Waiting...";
            Application.DoEvents();
            
            if (!bpc.EnableDevice(Program.gMachineNumber, 0))
            {
                lblMessage.Text = util.gstrNoDevice;
                return ;
            }

            GCHandle gh = GCHandle.Alloc(timeZoneInfo, GCHandleType.Pinned);
            IntPtr AddrOftimeZoneInfo = gh.AddrOfPinnedObject();
            int nAddr = AddrOftimeZoneInfo.ToInt32();
            
            util.MakeXMLRequestHeader(bpc, ref strXML, "GetTimezone");
            
            bRet = bpc.GeneralOperationXML(ref strXML);
            bpc.XML_ParseBinaryLong(ref strXML, "TimezoneBinary", ref nAddr, DBALLCOUNT * 5);
            if (bRet)
            {
                DrawTimeZone();
                lblMessage.Text = "Success!";
            }
            else
            {
                bpc.GetLastError(ref vErrorCode);
                lblMessage.Text = util.ErrorPrint(vErrorCode);
            }
            
            bpc.EnableDevice (Program.gMachineNumber, 1);
        }

        private void frmAccessTz_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.OpenForms["frmMain"].Visible = true;
        }

        private void cmdExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void cmdWrite_Click(object sender, EventArgs e)
        {
            Boolean bRet;
            int vErrorCode = 0;
            String strXML = "";

            lblMessage.Text = "Waiting...";
            Application.DoEvents();

            if (!bpc.EnableDevice(Program.gMachineNumber, 0))
            {
                lblMessage.Text = util.gstrNoDevice;
                return;
            }

            GCHandle gh = GCHandle.Alloc(timeZoneInfo, GCHandleType.Pinned);
            IntPtr AddrOftimeZoneInfo = gh.AddrOfPinnedObject();
            int nAddr = AddrOftimeZoneInfo.ToInt32();

            util.MakeXMLRequestHeader(bpc, ref strXML, "SetTimezone");
            bpc.XML_AddBinaryLong(ref strXML, "TimezoneBinary", ref nAddr, DBALLCOUNT * 5);

            bRet = bpc.GeneralOperationXML(ref strXML);
            if (bRet)
                lblMessage.Text = "Success!";
            else
            {
                bpc.GetLastError(ref vErrorCode);
                lblMessage.Text = util.ErrorPrint(vErrorCode);
            }

            bpc.EnableDevice(Program.gMachineNumber, 1); 
        }

        private void lstTimeZone_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstTimeZone.SelectedIndex == -1) 
                return;

            int index = lstTimeZone.SelectedIndex;

            dtStart.Value = new DateTime(   2000, 1, 1,         // Don't care year/month/date
                                            timeZoneInfo[index * 5], 
                                            timeZoneInfo[index * 5 + 1], 
                                            0
                                        );
            dtEnd.Value = new DateTime(     2000, 1, 1,         // Don't care year/month/date
                                            timeZoneInfo[index * 5 + 2], 
                                            timeZoneInfo[index * 5 + 3], 
                                            0
                                      );
            cmbVerifyMode.SelectedIndex = timeZoneInfo[index * 5 + 4];
        }

        private void cmdUpdate_Click(object sender, EventArgs e)
        {
            if (lstTimeZone.SelectedIndex == -1)
                return;

            int index = lstTimeZone.SelectedIndex;

            timeZoneInfo[index * 5] = Convert.ToByte(dtStart.Value.Hour);
            timeZoneInfo[index * 5 + 1] = Convert.ToByte(dtStart.Value.Minute);
            timeZoneInfo[index * 5 + 2] = Convert.ToByte(dtEnd.Value.Hour);
            timeZoneInfo[index * 5 + 3] = Convert.ToByte(dtEnd.Value.Minute);
            timeZoneInfo[index * 5 + 4] = Convert.ToByte(cmbVerifyMode.SelectedIndex);
            DrawTimeZone();
        }
    }
}
