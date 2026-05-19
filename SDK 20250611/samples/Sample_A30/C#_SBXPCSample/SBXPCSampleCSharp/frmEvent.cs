using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SBXPCSampleCSharp
{
    public partial class frmEvent : Form
    {
        public frmEvent()
        {
            InitializeComponent();
        }

        AxSBXPCLib.AxSBXPC bpc;

        private void frmEvent_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            this.Visible = false;
            Application.OpenForms["frmMain"].Visible = true;
        }

        private void frmEvent_Load(object sender, EventArgs e)
        {
            bpc = (AxSBXPCLib.AxSBXPC) Application.OpenForms["frmMain"].Controls["SBXPC1"];
            cmbComPort.SelectedIndex = 0;
            cmbBaudrate.SelectedIndex = 4;
            SwitchMode(true);
        }

        private void SwitchMode(bool bComport)
        {
            cmbComPort.Enabled = !bComport;
            cmbBaudrate.Enabled = !bComport;
            txtSourceIP.Enabled = bComport;
            txtPortNumber.Enabled = bComport;
        }

        public void ReceiveEvent(string eventXML)
        {
            int year, month, day, hour, minute, second, weekday;
            string strXML, eventItemString;
            string strMachineType = "", strEventType = "";
            int machinId;
            int managerId, userId, result;
            string str1 = "", str2 = "", str3 = "", str4 = "";

            strXML = eventXML;
            year = bpc.XML_ParseInt(ref strXML, "Year");
            month = bpc.XML_ParseInt(ref strXML, "Month");
            day = bpc.XML_ParseInt(ref strXML, "Day");
            hour = bpc.XML_ParseInt(ref strXML, "Hour");
            minute = bpc.XML_ParseInt(ref strXML, "Minute");
            second = bpc.XML_ParseInt(ref strXML, "Second");
            weekday = bpc.XML_ParseInt(ref strXML, "Weekday");

            machinId = bpc.XML_ParseInt(ref strXML, "MachineID");
            bpc.XML_ParseString(ref strXML, "MachineType", ref strMachineType);
            bpc.XML_ParseString(ref strXML, "EventType", ref strEventType);

            eventItemString = String.Format("{0:D2}-", year);
            eventItemString += String.Format("{0:D2}-", month);
            eventItemString += String.Format("{0:D2} ", day);
            eventItemString += String.Format("{0:D2}:", hour);
            eventItemString += String.Format("{0:D2}:", minute);
            eventItemString += String.Format("{0:D2} ", second);

            eventItemString += "[" + strMachineType + ":";
            eventItemString += machinId + "] ";
            eventItemString += strEventType + ", ";

            switch(strEventType)
            {
                case "Management Log":
                    managerId = bpc.XML_ParseInt(ref strXML, "ManagerID");
                    userId = bpc.XML_ParseInt(ref strXML, "UserID");
                    bpc.XML_ParseString(ref strXML, "Action", ref str1);
                    result = bpc.XML_ParseLong(ref strXML, "Result");
                    eventItemString += "Manager ID = " + String.Format("{0:D5}, ", managerId);
                    eventItemString += "User ID = " + String.Format("{0:D5}, ", userId);
                    eventItemString += "Action = " + str1 + ", ";
                    eventItemString += "Result = " + String.Format("{0:D}", result);
                    break;
                case "Time Log":
                    userId = bpc.XML_ParseInt(ref strXML, "UserID");
                    bpc.XML_ParseString(ref strXML, "AttendanceStatus", ref str1);
                    bpc.XML_ParseString(ref strXML, "VerificationMode", ref str2);
                    bpc.XML_ParseString(ref strXML, "AntipassStatus", ref str3);
                    bpc.XML_ParseString(ref strXML, "Photo", ref str4);
                    eventItemString += "User ID = " + String.Format("{0:D5}, ", userId);
                    eventItemString += "AttendanceStatus = " + str1 + ", ";
                    eventItemString += "VerificationMode = " + str2 + ", ";
                    eventItemString += "AntipassStatus = " + str3 + ", ";
                    eventItemString += "Photo = " + str4;
                    break;
                case "Verification Success":
                    userId = bpc.XML_ParseInt(ref strXML, "UserID");
                    bpc.XML_ParseString(ref strXML, "VerificationMode", ref str1);
                    eventItemString += "User ID = " + String.Format("{0:D5}, ", userId);
                    eventItemString += "VerificationMode = " + str1;
                    break;
                case "Verification Failure":
                    userId = bpc.XML_ParseInt(ref strXML, "UserID");
                    bpc.XML_ParseString(ref strXML, "VerificationMode", ref str1);
                    bpc.XML_ParseString(ref strXML, "ReasonOfFailure", ref str2);
                    eventItemString += "User ID = " + String.Format("{0:D5}, ", userId);
                    eventItemString += "VerificationMode = " + str1 + ", ";
                    eventItemString += "ReasonOfFailure = " + str2;
                    break;
                case "Alarm On":
                    userId = bpc.XML_ParseInt(ref strXML, "UserID");
                    bpc.XML_ParseString(ref strXML, "AlarmType", ref str1);
                    eventItemString += "User ID = " + String.Format("{0:D5}, ", userId);
                    eventItemString += "AlarmType = " + str1;
                    break;
                case "Alarm Off":
                    userId = bpc.XML_ParseInt(ref strXML, "UserID");
                    bpc.XML_ParseString(ref strXML, "AlarmType", ref str1);
                    bpc.XML_ParseString(ref strXML, "AlarmOffMethod", ref str2);
                    eventItemString += "User ID = " + String.Format("{0:D5}", userId);
                    eventItemString += "AlarmType = " + str1 + ", ";
                    eventItemString += "AlarmOffMethod = " + str2;
                    break;
                case "DoorBell":
                    bpc.XML_ParseString(ref strXML, "InputType", ref str1);
                    eventItemString += "Input Type = " + str1;
                    break;
            }

            lstEvents.Items.Add(eventItemString);
        }

        private void cmbEndMoniter_Click(object sender, EventArgs e)
        {
            bpc.StopEventCapture();
            cmdStartMoniter.Enabled = true;
            cmdEndMoniter.Enabled = false;
        }

        private void cmdClear_Click(object sender, EventArgs e)
        {
            lstEvents.Items.Clear();
        }

        private void cmdStartMoniter_Click(object sender, EventArgs e)
        {
            bool bRet;
            if(radioNetworkDevice.Checked)
                bRet = bpc.StartEventCapture(0, util.pubIPAddrToLong(txtSourceIP.Text), Convert.ToInt32(txtPortNumber.Text));
            else
                bRet = bpc.StartEventCapture(1, cmbComPort.SelectedIndex + 1, Convert.ToInt32(cmbBaudrate.Text));
            cmdStartMoniter.Enabled = false;
            cmdEndMoniter.Enabled = true;
        }

        private void radioNetwork_Click(object sender, EventArgs e)
        {
            if(radioNetworkDevice.Checked)
            {
                SwitchMode(true);
                radioSerialDevice.Checked = false;
            }
        }

        private void radioSerial_Click(object sender, EventArgs e)
        {
            if(radioSerialDevice.Checked)
            {
                SwitchMode(false);
                radioNetworkDevice.Checked = false;
            }
        }
    }
}
