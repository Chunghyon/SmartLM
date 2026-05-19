using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SBXPCDLLSampleCSharp
{
    public partial class frmNtpServerSetting : Form
    {
        public frmNtpServerSetting()
        {
            InitializeComponent();
        }

        private void btnGetDnsSettings_Click(object sender, EventArgs e)
        {
            string strXML = null;
            string strValue = "";
            sbxpc.SBXPCDLL.XML_AddString(ref strXML, "REQUEST", "GetDeviceInfoExt");
            sbxpc.SBXPCDLL.XML_AddString(ref strXML, "MSGTYPE", "request");
            sbxpc.SBXPCDLL.XML_AddLong(ref strXML, "MachineID", Program.gMachineNumber);
            sbxpc.SBXPCDLL.XML_AddString(ref strXML, "ParamName", "NTPServer");

            if (sbxpc.SBXPCDLL.GeneralOperationXML(Program.gMachineNumber, ref strXML))
            {
                sbxpc.SBXPCDLL.XML_ParseString(ref strXML, "Value1", out strValue);
                txtServerAddress.Text = strValue;

                sbxpc.SBXPCDLL.XML_ParseString(ref strXML, "Value2", out strValue);
                txtTimezone.Text = strValue;

                sbxpc.SBXPCDLL.XML_ParseString(ref strXML, "Value3", out strValue);
                txtInterval.Text = strValue;

                MessageBox.Show("Get NTP Server Settings OK!");
            }
            else
            {
                MessageBox.Show("Get NTP Server Settings Failed.");
            }
        }

        private void btnSetDnsSettings_Click(object sender, EventArgs e)
        {

            string strXML = null;
            sbxpc.SBXPCDLL.XML_AddString(ref strXML, "REQUEST", "SetDeviceInfoExt");
            sbxpc.SBXPCDLL.XML_AddString(ref strXML, "MSGTYPE", "request");
            sbxpc.SBXPCDLL.XML_AddLong(ref strXML, "MachineID", Program.gMachineNumber);
            sbxpc.SBXPCDLL.XML_AddString(ref strXML, "ParamName", "NTPServer");

            sbxpc.SBXPCDLL.XML_AddString(ref strXML, "Value1", txtServerAddress.Text);
            sbxpc.SBXPCDLL.XML_AddInt(ref strXML, "Value2", Convert.ToInt32(txtTimezone.Text));
            sbxpc.SBXPCDLL.XML_AddInt(ref strXML, "Value3", Convert.ToInt32(txtInterval.Text));
            
            if (sbxpc.SBXPCDLL.GeneralOperationXML(Program.gMachineNumber, ref strXML))
            {
                MessageBox.Show("Set NTP Server Settings OK!");
            }
            else
            {
                string str = "";
                sbxpc.SBXPCDLL.XML_ParseString(ref strXML, "Result", out str);

                MessageBox.Show("Set NTP Server Settings Failed.\r\nResult:" + str);
            }
        }

        private void frmNtpServerSetting_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.OpenForms["frmMain"].Visible = true;
        }
    }
}
