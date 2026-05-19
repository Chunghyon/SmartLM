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
using System.Xml.Linq;

namespace SBXPCDLLSampleCSharp
{
	public partial class frmEnrollCustom1 : Form
	{

		public frmEnrollCustom1()
		{
			InitializeComponent();
		}
		private void frmEnrollCustom1_Load(object sender, EventArgs e)
		{

		}

		private void btnGetUserMessage_Click(object sender, EventArgs e)
		{
			Boolean bRet;
			int vErrorCode = 0;
			int lUserID;
			String strXML = "";

			lblMessage.Text = "Working...";
			Application.DoEvents();

			bRet = sbxpc.SBXPCDLL.EnableDevice(Program.gMachineNumber, 0);
			if (!bRet)
			{
				lblMessage.Text = util.gstrNoDevice;
				return;
			}

			lUserID = Convert.ToInt32(txtEnrollNumber.Text);

			sbxpc.SBXPCDLL.XML_AddString(ref strXML, "REQUEST", "GetUserMessage");
			sbxpc.SBXPCDLL.XML_AddString(ref strXML, "MSGTYPE", "request");
			sbxpc.SBXPCDLL.XML_AddInt(ref strXML, "MachineID", Program.gMachineNumber);
			sbxpc.SBXPCDLL.XML_AddInt(ref strXML, "UserID", lUserID);

			bRet = sbxpc.SBXPCDLL.GeneralOperationXML(Program.gMachineNumber, ref strXML);

			if (bRet)
			{
				string base64_name;
				if (!sbxpc.SBXPCDLL.XML_ParseString(ref strXML, "Message", out base64_name))
				{
					lblMessage.Text = "Failed to parse 'Message' string.";
				}
				else
				{
					if (base64_name != null)
					{
						try
						{
							byte[] name_binary = Convert.FromBase64String(base64_name);
							int index = 0;
							for (int i = 0; i < name_binary.Length - 1; i += 2)
							{
								if (name_binary[i] == 0 && name_binary[i + 1] == 0)
								{
									index = i;
									break;
								}
							}

							txtUserMessage.Text = Encoding.Unicode.GetString(name_binary, 0, index);
						}
						catch (Exception)
						{
						}
					}
					lblMessage.Text = "Success!";
				}
			}
			else
			{
				sbxpc.SBXPCDLL.GetLastError(Program.gMachineNumber, out vErrorCode);
				lblMessage.Text = util.ErrorPrint(vErrorCode);
			}
			sbxpc.SBXPCDLL.EnableDevice(Program.gMachineNumber, 1);
		}

		private void btnSetUserMessage_Click(object sender, EventArgs e)
		{
			Boolean bRet;
			int vErrorCode = 0;
			int lUserID;
			String strXML = "";

			lblMessage.Text = "Waiting...";
			Application.DoEvents();

			bRet = sbxpc.SBXPCDLL.EnableDevice(Program.gMachineNumber, 0);
			if (!bRet)
			{
				lblMessage.Text = util.gstrNoDevice;
				return;
			}

			lUserID = Convert.ToInt32(txtEnrollNumber.Text);

			sbxpc.SBXPCDLL.XML_AddString(ref strXML, "REQUEST", "SetUserMessage");
			sbxpc.SBXPCDLL.XML_AddString(ref strXML, "MSGTYPE", "request");
			sbxpc.SBXPCDLL.XML_AddInt(ref strXML, "MachineID", Program.gMachineNumber);
			sbxpc.SBXPCDLL.XML_AddInt(ref strXML, "UserID", lUserID);

			{
				byte[] name_binary = Encoding.Unicode.GetBytes(txtUserMessage.Text);
				sbxpc.SBXPCDLL.XML_AddString(ref strXML, "Message", Convert.ToBase64String(name_binary));
			}

			bRet = sbxpc.SBXPCDLL.GeneralOperationXML(Program.gMachineNumber, ref strXML);

			if (bRet)
			{
				lblMessage.Text = "Success!";
			}
			else
			{
				sbxpc.SBXPCDLL.GetLastError(Program.gMachineNumber, out vErrorCode);
				lblMessage.Text = util.ErrorPrint(vErrorCode);
			}
			sbxpc.SBXPCDLL.EnableDevice(Program.gMachineNumber, 1);
		}

		private void btnGetUserBalanceTime_Click(object sender, EventArgs e)
		{
			Boolean bRet;
			int vErrorCode = 0;
			int lUserID, lBalanceTimeInMinues;
			int hh, mm;
			String strXML = "";

			lblMessage.Text = "Working...";
			Application.DoEvents();

			bRet = sbxpc.SBXPCDLL.EnableDevice(Program.gMachineNumber, 0);
			if (!bRet)
			{
				lblMessage.Text = util.gstrNoDevice;
				return;
			}

			lUserID = Convert.ToInt32(txtEnrollNumber.Text);

			sbxpc.SBXPCDLL.XML_AddString(ref strXML, "REQUEST", "GetUserBalanceTime");
			sbxpc.SBXPCDLL.XML_AddString(ref strXML, "MSGTYPE", "request");
			sbxpc.SBXPCDLL.XML_AddInt(ref strXML, "MachineID", Program.gMachineNumber);
			sbxpc.SBXPCDLL.XML_AddInt(ref strXML, "UserID", lUserID);

			bRet = sbxpc.SBXPCDLL.GeneralOperationXML(Program.gMachineNumber, ref strXML);

			if (bRet)
			{
				lBalanceTimeInMinues = sbxpc.SBXPCDLL.XML_ParseInt(ref strXML, "BalanceTimeInMinues");
				hh = lBalanceTimeInMinues / 60;
				mm = lBalanceTimeInMinues % 60;
				dtBalanceTime.Value = new DateTime(2000, 1, 1, hh, mm, 0);
				
				lblMessage.Text = "Success!";
			}
			else
			{
				sbxpc.SBXPCDLL.GetLastError(Program.gMachineNumber, out vErrorCode);
				lblMessage.Text = util.ErrorPrint(vErrorCode);
			}
			sbxpc.SBXPCDLL.EnableDevice(Program.gMachineNumber, 1);
		}

		private void btnSetUserBalanceTime_Click(object sender, EventArgs e)
		{
			Boolean bRet;
			int vErrorCode = 0;
			int lUserID;
			int lBalanceTimeInMinues;
			String strXML = "";

			lblMessage.Text = "Waiting...";
			Application.DoEvents();

			bRet = sbxpc.SBXPCDLL.EnableDevice(Program.gMachineNumber, 0);
			if (!bRet)
			{
				lblMessage.Text = util.gstrNoDevice;
				return;
			}

			lUserID = Convert.ToInt32(txtEnrollNumber.Text);

			sbxpc.SBXPCDLL.XML_AddString(ref strXML, "REQUEST", "SetUserBalanceTime");
			sbxpc.SBXPCDLL.XML_AddString(ref strXML, "MSGTYPE", "request");
			sbxpc.SBXPCDLL.XML_AddInt(ref strXML, "MachineID", Program.gMachineNumber);
			sbxpc.SBXPCDLL.XML_AddInt(ref strXML, "UserID", lUserID);

			lBalanceTimeInMinues = dtBalanceTime.Value.Hour * 60 + dtBalanceTime.Value.Minute;
			
			sbxpc.SBXPCDLL.XML_AddInt(ref strXML, "BalanceTimeInMinues", lBalanceTimeInMinues);

			bRet = sbxpc.SBXPCDLL.GeneralOperationXML(Program.gMachineNumber, ref strXML);

			if (bRet)
			{
				lblMessage.Text = "Success!";
			}
			else
			{
				sbxpc.SBXPCDLL.GetLastError(Program.gMachineNumber, out vErrorCode);
				lblMessage.Text = util.ErrorPrint(vErrorCode);
			}
			sbxpc.SBXPCDLL.EnableDevice(Program.gMachineNumber, 1);
		}

		private void btnGetUserHolidays_Click(object sender, EventArgs e)
		{
			Boolean bRet;
			int vErrorCode = 0;
			int lUserID, lHolidaysInDays10;
			String strXML = "";

			lblMessage.Text = "Working...";
			Application.DoEvents();

			bRet = sbxpc.SBXPCDLL.EnableDevice(Program.gMachineNumber, 0);
			if (!bRet)
			{
				lblMessage.Text = util.gstrNoDevice;
				return;
			}

			lUserID = Convert.ToInt32(txtEnrollNumber.Text);

			sbxpc.SBXPCDLL.XML_AddString(ref strXML, "REQUEST", "GetUserHolidays");
			sbxpc.SBXPCDLL.XML_AddString(ref strXML, "MSGTYPE", "request");
			sbxpc.SBXPCDLL.XML_AddInt(ref strXML, "MachineID", Program.gMachineNumber);
			sbxpc.SBXPCDLL.XML_AddInt(ref strXML, "UserID", lUserID);

			bRet = sbxpc.SBXPCDLL.GeneralOperationXML(Program.gMachineNumber, ref strXML);

			if (bRet)
			{
				lHolidaysInDays10 = sbxpc.SBXPCDLL.XML_ParseInt(ref strXML, "HolidaysInDays10");
				txtHolidays.Text = (lHolidaysInDays10 / 10).ToString() + "." + (lHolidaysInDays10 % 10).ToString();

				lblMessage.Text = "Success!";
			}
			else
			{
				sbxpc.SBXPCDLL.GetLastError(Program.gMachineNumber, out vErrorCode);
				lblMessage.Text = util.ErrorPrint(vErrorCode);
			}
			sbxpc.SBXPCDLL.EnableDevice(Program.gMachineNumber, 1);
		}

		private void btnSetUserHolidays_Click(object sender, EventArgs e)
		{
			Boolean bRet;
			int vErrorCode = 0;
			int lUserID;
			int lHolidaysInDays10;
			String strXML = "";

			lblMessage.Text = "Waiting...";
			Application.DoEvents();

			bRet = sbxpc.SBXPCDLL.EnableDevice(Program.gMachineNumber, 0);
			if (!bRet)
			{
				lblMessage.Text = util.gstrNoDevice;
				return;
			}

			lUserID = Convert.ToInt32(txtEnrollNumber.Text);

			sbxpc.SBXPCDLL.XML_AddString(ref strXML, "REQUEST", "SetUserHolidays");
			sbxpc.SBXPCDLL.XML_AddString(ref strXML, "MSGTYPE", "request");
			sbxpc.SBXPCDLL.XML_AddInt(ref strXML, "MachineID", Program.gMachineNumber);
			sbxpc.SBXPCDLL.XML_AddInt(ref strXML, "UserID", lUserID);

			lHolidaysInDays10 = Convert.ToInt32(Convert.ToDouble(txtHolidays.Text) * 10);
			txtHolidays.Text = (lHolidaysInDays10 / 10).ToString() + "." + (lHolidaysInDays10 % 10).ToString();

			sbxpc.SBXPCDLL.XML_AddInt(ref strXML, "HolidaysInDays10", lHolidaysInDays10);

			bRet = sbxpc.SBXPCDLL.GeneralOperationXML(Program.gMachineNumber, ref strXML);

			if (bRet)
			{
				lblMessage.Text = "Success!";
			}
			else
			{
				sbxpc.SBXPCDLL.GetLastError(Program.gMachineNumber, out vErrorCode);
				lblMessage.Text = util.ErrorPrint(vErrorCode);
			}
			sbxpc.SBXPCDLL.EnableDevice(Program.gMachineNumber, 1);
		}

		private void frmEnrollCustom1_FormClosed(object sender, FormClosedEventArgs e)
		{
			Application.OpenForms["frmEnroll"].Visible = true;
		}
    }
}