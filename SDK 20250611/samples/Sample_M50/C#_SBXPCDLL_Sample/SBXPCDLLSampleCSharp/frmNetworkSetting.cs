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
    public partial class frmNetworkSetting : Form
    {
        public frmNetworkSetting()
        {
            InitializeComponent();
        }

        private void frmNetworkSetting_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.OpenForms["frmMain"].Visible = true;
        }

        private void btnGetEthernetSetting_Click(object sender, EventArgs e)
        {
            string strXML = null;
            string strValue = "";
            sbxpc.SBXPCDLL.XML_AddString(ref strXML, "REQUEST", "GetEthernetSetting");
            sbxpc.SBXPCDLL.XML_AddString(ref strXML, "MSGTYPE", "request");
            sbxpc.SBXPCDLL.XML_AddLong(ref strXML, "MachineID", Program.gMachineNumber);

            if (sbxpc.SBXPCDLL.GeneralOperationXML(Program.gMachineNumber, ref strXML))
            {
                chkEther_DHCP.Checked = (sbxpc.SBXPCDLL.XML_ParseInt(ref strXML, "DHCP") != 0);
                sbxpc.SBXPCDLL.XML_ParseString(ref strXML, "IP", out strValue);
                txtEther_IP.Text = strValue;
                sbxpc.SBXPCDLL.XML_ParseString(ref strXML, "Subnet", out strValue);
                txtEther_Subnet.Text = strValue;
                sbxpc.SBXPCDLL.XML_ParseString(ref strXML, "DefaultGateway", out strValue);
                txtEther_DefaultGateway.Text = strValue;
                chkEther_ManualDNS.Checked = (sbxpc.SBXPCDLL.XML_ParseInt(ref strXML, "ManualDNS") != 0);
                sbxpc.SBXPCDLL.XML_ParseString(ref strXML, "PrimaryDNSServer", out strValue);
                txtEther_PrimaryDNSServer.Text = strValue;
                sbxpc.SBXPCDLL.XML_ParseString(ref strXML, "SecondaryDNSServer", out strValue);
                txtEther_SecondaryDNSServer.Text = strValue;

                chkEther_DHCP_CheckedChanged(sender, e);
                chkEther_ManualDNS_CheckedChanged(sender, e);

                MessageBox.Show("Get Ethernet Setting OK!");
            }
            else
            {
                MessageBox.Show("Get Ethernet Setting Failed.");
            }
        }

        private void btnSetEthernetSetting_Click(object sender, EventArgs e)
        {
            string strXML = null;
            sbxpc.SBXPCDLL.XML_AddString(ref strXML, "REQUEST", "SetEthernetSetting");
            sbxpc.SBXPCDLL.XML_AddString(ref strXML, "MSGTYPE", "request");
            sbxpc.SBXPCDLL.XML_AddLong(ref strXML, "MachineID", Program.gMachineNumber);

            sbxpc.SBXPCDLL.XML_AddInt(ref strXML, "DHCP", Convert.ToInt32(chkEther_DHCP.Checked ? 1 : 0));
            if (!chkEther_DHCP.Checked)
            {
                sbxpc.SBXPCDLL.XML_AddString(ref strXML, "IP", txtEther_IP.Text);
                sbxpc.SBXPCDLL.XML_AddString(ref strXML, "Subnet", txtEther_Subnet.Text);
                sbxpc.SBXPCDLL.XML_AddString(ref strXML, "DefaultGateway", txtEther_DefaultGateway.Text);
            }
            sbxpc.SBXPCDLL.XML_AddInt(ref strXML, "ManualDNS", Convert.ToInt32(chkEther_ManualDNS.Checked ? 1 : 0));
            if (chkEther_ManualDNS.Checked)
            {
                sbxpc.SBXPCDLL.XML_AddString(ref strXML, "PrimaryDNSServer", txtEther_PrimaryDNSServer.Text);
                sbxpc.SBXPCDLL.XML_AddString(ref strXML, "SecondaryDNSServer", txtEther_SecondaryDNSServer.Text);
            }

            if (sbxpc.SBXPCDLL.GeneralOperationXML(Program.gMachineNumber, ref strXML))
            {
                MessageBox.Show("Set Ethernet Setting OK!");
            }
            else
            {
                string str = "";
                sbxpc.SBXPCDLL.XML_ParseString(ref strXML, "Result", out str);

                MessageBox.Show("Set Ethernet Setting Failed.\r\nResult:" + str);
            }
        }

        private void btnGetWiFiSetting_Click(object sender, EventArgs e)
        {
            string strXML = null;
            string strValue = "";
            sbxpc.SBXPCDLL.XML_AddString(ref strXML, "REQUEST", "GetWiFiSetting");
            sbxpc.SBXPCDLL.XML_AddString(ref strXML, "MSGTYPE", "request");
            sbxpc.SBXPCDLL.XML_AddLong(ref strXML, "MachineID", Program.gMachineNumber);

            if (sbxpc.SBXPCDLL.GeneralOperationXML(Program.gMachineNumber, ref strXML))
            {
                sbxpc.SBXPCDLL.XML_ParseString(ref strXML, "SSID", out strValue);
                txtWiFi_SSID.Text = strValue;
                sbxpc.SBXPCDLL.XML_ParseString(ref strXML, "Key", out strValue);
                txtWiFi_Key.Text = strValue;
                chkWiFi_DHCP.Checked = (sbxpc.SBXPCDLL.XML_ParseInt(ref strXML, "DHCP") != 0);
                sbxpc.SBXPCDLL.XML_ParseString(ref strXML, "IP", out strValue);
                txtWiFi_IP.Text = strValue;
                sbxpc.SBXPCDLL.XML_ParseString(ref strXML, "Subnet", out strValue);
                txtWiFi_Subnet.Text = strValue;
                sbxpc.SBXPCDLL.XML_ParseString(ref strXML, "DefaultGateway", out strValue);
                txtWiFi_DefaultGateway.Text = strValue;
                chkWiFi_ManualDNS.Checked = (sbxpc.SBXPCDLL.XML_ParseInt(ref strXML, "ManualDNS") != 0);
                sbxpc.SBXPCDLL.XML_ParseString(ref strXML, "PrimaryDNSServer", out strValue);
                txtWiFi_PrimaryDNSServer.Text = strValue;
                sbxpc.SBXPCDLL.XML_ParseString(ref strXML, "SecondaryDNSServer", out strValue);
                txtWiFi_SecondaryDNSServer.Text = strValue;

                chkWiFi_DHCP_CheckedChanged(sender, e);
                chkWiFi_ManualDNS_CheckedChanged(sender, e);

                MessageBox.Show("Get WiFi Setting OK!");
            }
            else
            {
                MessageBox.Show("Get WiFi Setting Failed.");
            }
        }

        private void btnSetWiFiSetting_Click(object sender, EventArgs e)
        {
            string strXML = null;
            sbxpc.SBXPCDLL.XML_AddString(ref strXML, "REQUEST", "SetWiFiSetting");
            sbxpc.SBXPCDLL.XML_AddString(ref strXML, "MSGTYPE", "request");
            sbxpc.SBXPCDLL.XML_AddLong(ref strXML, "MachineID", Program.gMachineNumber);

            sbxpc.SBXPCDLL.XML_AddString(ref strXML, "SSID", txtWiFi_SSID.Text);
            sbxpc.SBXPCDLL.XML_AddString(ref strXML, "Key", txtWiFi_Key.Text);
            sbxpc.SBXPCDLL.XML_AddInt(ref strXML, "DHCP", Convert.ToInt32(chkWiFi_DHCP.Checked ? 1 : 0));
            if (!chkWiFi_DHCP.Checked)
            {
                sbxpc.SBXPCDLL.XML_AddString(ref strXML, "IP", txtWiFi_IP.Text);
                sbxpc.SBXPCDLL.XML_AddString(ref strXML, "Subnet", txtWiFi_Subnet.Text);
                sbxpc.SBXPCDLL.XML_AddString(ref strXML, "DefaultGateway", txtWiFi_DefaultGateway.Text);
            }
            sbxpc.SBXPCDLL.XML_AddInt(ref strXML, "ManualDNS", Convert.ToInt32(chkWiFi_ManualDNS.Checked ? 1 : 0));
            if (chkWiFi_ManualDNS.Checked)
            {
                sbxpc.SBXPCDLL.XML_AddString(ref strXML, "PrimaryDNSServer", txtWiFi_PrimaryDNSServer.Text);
                sbxpc.SBXPCDLL.XML_AddString(ref strXML, "SecondaryDNSServer", txtWiFi_SecondaryDNSServer.Text);
            }

            if (sbxpc.SBXPCDLL.GeneralOperationXML(Program.gMachineNumber, ref strXML))
            {
                MessageBox.Show("Set WiFi Setting OK!");
            }
            else
            {
                string str = "";
                sbxpc.SBXPCDLL.XML_ParseString(ref strXML, "Result", out str);

                MessageBox.Show("Set WiFi Setting Failed.\r\nResult:" + str);
            }
        }

        private void btnGetCommSetting_Click(object sender, EventArgs e)
        {
            string strXML = null;
            string strValue = "";
            sbxpc.SBXPCDLL.XML_AddString(ref strXML, "REQUEST", "GetCommSetting");
            sbxpc.SBXPCDLL.XML_AddString(ref strXML, "MSGTYPE", "request");
            sbxpc.SBXPCDLL.XML_AddLong(ref strXML, "MachineID", Program.gMachineNumber);

            if (sbxpc.SBXPCDLL.GeneralOperationXML(Program.gMachineNumber, ref strXML))
            {
                txtDeviceID.Text = Convert.ToString(sbxpc.SBXPCDLL.XML_ParseLong(ref strXML, "DeviceID"));
                txtCommPwd.Text = Convert.ToString(sbxpc.SBXPCDLL.XML_ParseLong(ref strXML, "CommPwd"));
                txtTcpPort.Text = Convert.ToString(sbxpc.SBXPCDLL.XML_ParseLong(ref strXML, "TcpPort"));
                sbxpc.SBXPCDLL.XML_ParseString(ref strXML, "P2PSvr", out strValue);
                txtP2P_Server.Text = strValue;
                txtP2P_Port.Text = Convert.ToString(sbxpc.SBXPCDLL.XML_ParseLong(ref strXML, "P2PPort"));

                MessageBox.Show("Get Communication Setting OK!");
            }
            else
            {
                MessageBox.Show("Get Communication Setting Failed.");
            }
        }

        private void btnSetCommSetting_Click(object sender, EventArgs e)
        {
            string strXML = null;
            sbxpc.SBXPCDLL.XML_AddString(ref strXML, "REQUEST", "SetCommSetting");
            sbxpc.SBXPCDLL.XML_AddString(ref strXML, "MSGTYPE", "request");
            sbxpc.SBXPCDLL.XML_AddLong(ref strXML, "MachineID", Program.gMachineNumber);

            sbxpc.SBXPCDLL.XML_AddLong(ref strXML, "DeviceID", Convert.ToInt32(txtDeviceID.Text));
            sbxpc.SBXPCDLL.XML_AddLong(ref strXML, "CommPwd", Convert.ToInt32(txtCommPwd.Text));
            sbxpc.SBXPCDLL.XML_AddLong(ref strXML, "TcpPort", Convert.ToInt32(txtTcpPort.Text));
            sbxpc.SBXPCDLL.XML_AddString(ref strXML, "P2PSvr", txtP2P_Server.Text);
            sbxpc.SBXPCDLL.XML_AddLong(ref strXML, "P2PPort", Convert.ToInt32(txtP2P_Port.Text));

            if (sbxpc.SBXPCDLL.GeneralOperationXML(Program.gMachineNumber, ref strXML))
            {
                MessageBox.Show("Set Communication Setting OK!");
            }
            else
            {
                string str = "";
                sbxpc.SBXPCDLL.XML_ParseString(ref strXML, "Result", out str);

                MessageBox.Show("Set Communication Setting Failed.\r\nResult:" + str);
            }
        }

        private void chkEther_DHCP_CheckedChanged(object sender, EventArgs e)
        {
            txtEther_IP.Enabled = txtEther_Subnet.Enabled = txtEther_DefaultGateway.Enabled = !chkEther_DHCP.Checked;
        }

        private void chkEther_ManualDNS_CheckedChanged(object sender, EventArgs e)
        {
            txtEther_PrimaryDNSServer.Enabled = txtEther_SecondaryDNSServer.Enabled = chkEther_ManualDNS.Checked;
        }

        private void chkWiFi_DHCP_CheckedChanged(object sender, EventArgs e)
        {
            txtWiFi_IP.Enabled = txtWiFi_Subnet.Enabled = txtWiFi_DefaultGateway.Enabled = !chkWiFi_DHCP.Checked;
        }

        private void chkWiFi_ManualDNS_CheckedChanged(object sender, EventArgs e)
        {
            txtWiFi_PrimaryDNSServer.Enabled = txtWiFi_SecondaryDNSServer.Enabled = chkWiFi_ManualDNS.Checked;
        }

        private void btnApplyCommSetting_Click(object sender, EventArgs e)
        {
            string strXML = null;
            sbxpc.SBXPCDLL.XML_AddString(ref strXML, "REQUEST", "ApplyCommSetting");
            sbxpc.SBXPCDLL.XML_AddString(ref strXML, "MSGTYPE", "request");
            sbxpc.SBXPCDLL.XML_AddLong(ref strXML, "MachineID", Program.gMachineNumber);

            sbxpc.SBXPCDLL.XML_AddInt(ref strXML, "Apply", 1);
            
            if (sbxpc.SBXPCDLL.GeneralOperationXML(Program.gMachineNumber, ref strXML))
            {
                MessageBox.Show("Apply Settings OK!");
            }
            else
            {
                string str = "";
                sbxpc.SBXPCDLL.XML_ParseString(ref strXML, "Result", out str);

                MessageBox.Show("Apply Settings Failed.\r\nResult:" + str);
            }
        }
    }
}
