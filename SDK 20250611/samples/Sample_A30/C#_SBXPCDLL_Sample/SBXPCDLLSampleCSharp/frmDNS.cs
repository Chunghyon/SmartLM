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
    public partial class frmDNS : Form
    {
        
        public frmDNS()
        {
            InitializeComponent();
        }

        private void btnGetDnsSettings_Click(object sender, EventArgs e)
        {
            string strXML = null;
            string strValue = "";
            sbxpc.SBXPCDLL.XML_AddString(ref strXML, "REQUEST", "GetDnsSettings");
            sbxpc.SBXPCDLL.XML_AddString(ref strXML, "MSGTYPE", "request");
            sbxpc.SBXPCDLL.XML_AddLong(ref strXML, "MachineID", Program.gMachineNumber);

            if (sbxpc.SBXPCDLL.GeneralOperationXML(Program.gMachineNumber, ref strXML))
            {
                chkDnsObtainAuto.Checked = (sbxpc.SBXPCDLL.XML_ParseLong(ref strXML, "DnsMode") != 0) ? true : false;
                sbxpc.SBXPCDLL.XML_ParseString(ref strXML, "DnsServer0IP", out strValue);
                txtDnsServer0.Text = strValue;

                sbxpc.SBXPCDLL.XML_ParseString(ref strXML, "DnsServer1IP", out strValue);
                txtDnsServer1.Text = strValue;

                sbxpc.SBXPCDLL.XML_ParseString(ref strXML, "ManagerPCDomainName", out strValue);
                txtServerDomainName.Text = strValue;

                textBgServerPort.Text = Convert.ToString(sbxpc.SBXPCDLL.XML_ParseLong(ref strXML, "ManagerPCPort"));

                MessageBox.Show("Get DNS Settings OK!");
            }
            else
            {
                MessageBox.Show("Get DNS Settings Failed.");
            }
        }

        private void btnSetDnsSettings_Click(object sender, EventArgs e)
        {
            if (IP_to_String(String_to_IP(txtDnsServer0.Text)) != txtDnsServer0.Text)
            {
                MessageBox.Show("Preferred DNS server address is invalid.");
                return;
            }
            if (IP_to_String(String_to_IP(txtDnsServer1.Text)) != txtDnsServer1.Text)
            {
                MessageBox.Show("Alternate DNS server address is invalid.");
                return;
            }
            if (txtServerDomainName.Text.Length == 0)
            {
                MessageBox.Show("Please input server domain name.");
                return;
            }

            string strXML = null;
            sbxpc.SBXPCDLL.XML_AddString(ref strXML, "REQUEST", "SetDnsSettings");
            sbxpc.SBXPCDLL.XML_AddString(ref strXML, "MSGTYPE", "request");
            sbxpc.SBXPCDLL.XML_AddLong(ref strXML, "MachineID", Program.gMachineNumber);

            sbxpc.SBXPCDLL.XML_AddLong(ref strXML, "DnsMode", chkDnsObtainAuto.Checked ? 1 : 0);
            sbxpc.SBXPCDLL.XML_AddString(ref strXML, "DnsServer0IP", txtDnsServer0.Text);
            sbxpc.SBXPCDLL.XML_AddString(ref strXML, "DnsServer1IP", txtDnsServer1.Text);
            sbxpc.SBXPCDLL.XML_AddString(ref strXML, "ManagerPCDomainName", txtServerDomainName.Text);
            sbxpc.SBXPCDLL.XML_AddLong(ref strXML, "ManagerPCPort", Convert.ToInt32(textBgServerPort.Text));

            if (sbxpc.SBXPCDLL.GeneralOperationXML(Program.gMachineNumber, ref strXML))
            {
                MessageBox.Show("Set DNS Settings OK!");
            }
            else
            {
                string str = "";
                sbxpc.SBXPCDLL.XML_ParseString(ref strXML, "Result", out str);

                MessageBox.Show("Set DNS Settings Failed.\r\nResult:" + str);
            }
        }

        private int String_to_IP(string IP_str)
        {
            string[] t = IP_str.Split('.');

            if (t.Length != 4)
                return 0;

            int ip = Convert.ToInt32(t[0]) << 24 | Convert.ToInt32(t[1]) << 16 | Convert.ToInt32(t[2]) << 8 | Convert.ToInt32(t[3]) << 0;
            return ip;
        }

        private string IP_to_String(int ip)
        {
            return string.Format("{0}.{1}.{2}.{3}", (ip >> 24) & 0xFF, (ip >> 16) & 0xFF, (ip >> 8) & 0xFF, ip & 0xFF);
        }

        private void frmDNS_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.OpenForms["frmMain"].Visible = true;
        }

        private void frmDNS_Load(object sender, EventArgs e)
        {
        }
    }
}
