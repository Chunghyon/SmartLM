// SBXPC Multi-Thread Safe C# sample
// © 2004-2012 Beijing Smackbio Technology Co., Ltd.  All rights reserved.
// This sample shows how to use SBXPC's Multi-Thread Safe function 

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.Threading;

using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security.Permissions;

using System.Diagnostics;
using System.IO;
using Microsoft.Win32;

using System.Drawing.Imaging;

using System.Text.RegularExpressions;

namespace SBXPCDLLSampleCSharp
{
    public partial class MainForm : Form
    {
        void MyTrace(String format, params object[] args) //logging
        {
            BeginInvoke(m_delegateAddLog, String.Format(format, args)); //must call BeginInvoke because a window must be accessed from other thread
        }

        static void OCXThreadProc(object param) //sample thread, 'param' includes device connection parameters
        {
            Thread me = Thread.CurrentThread;
            OCXThreadParam p = (OCXThreadParam)param;

            p.mainForm.MyTrace("{0}: Thread started", me.Name);

            IntPtr ppsIpAddr = Marshal.AllocHGlobal(IntPtr.Size); //convert 'string' to 'BSTR*' because '_ConnectTcpip' requires 'BSTR*' type for ip address string
            IntPtr psIpAddr = Marshal.StringToBSTR(p.sIpAddr);
            Marshal.WriteIntPtr(ppsIpAddr, psIpAddr);

            while (true)
            {
                try
                {
                    p.mainForm.MyTrace("{0}: Connecting...", me.Name);
                    int ret = SBXPCDLL._ConnectTcpip(p.nDevID, ppsIpAddr, p.nPortNum, p.nPassword); //connecting
                    // one device cannot be accessed from several threads simultaneously
                    // so several threads cannot call '_ConnectTcpip' with same 'dwMachineNumber' value
                    // that is, '_ConnectTcpip' binds the device ID to the current thread
                    // or, current thread owns the device ID by '_ConnectTcpip'
                    // moreover SBXPCDLL checks duplicated ip address, this means several devices cannot have same ip address

                    p.mainForm.MyTrace("{0}: Connecting result = {1}", me.Name, ret);

                    if (ret != 0) //if connection established
                    {
                        int dwYear = 0, dwMonth = 0, dwDay = 0, dwHour = 0, dwMinute = 0, dwSecond = 0, dwDayOfWeek = 0;
                        ret = SBXPCDLL._GetDeviceTime(p.nDevID, //get device's time
                            ref dwYear, ref dwMonth, ref dwDay, ref dwHour, ref dwMinute, ref dwSecond, ref dwDayOfWeek);
                        // if you do not use the device ID which owned by current thread, any OCX function will be failed
                        // here if you call '_GetDeviceTime' with different device ID other than thread-owned ID, '_GetDeviceTime' will be failed

                        if (ret != 0)
                        {
                            p.mainForm.MyTrace("{0}: The Device's Time = {1}/{2}/{3} {4}:{5}:{6}", me.Name,
                                dwYear, dwMonth, dwDay, dwHour, dwMinute, dwSecond);
                        }
                        Thread.Sleep(1000);
                    }

                    SBXPCDLL._Disconnect(p.nDevID); //disconnecting
                    // '_Disconnect' releases the device ID, the device ID will be free for next other connection
                    // that is, '_Disconnect' un-binds the device ID from the current thread
                    // or the current thread gives up the ownership of the device ID

                    p.mainForm.MyTrace("{0}: Disconnected", me.Name);

                    Thread.Sleep(10);
                }
                catch (ThreadAbortException) //the thread is aborted
				{
					SBXPCDLL._Disconnect(p.nDevID); //disconnecting, to prevent memory leak

					p.mainForm.MyTrace("{0}: Thread stopped", me.Name);
					Marshal.FreeHGlobal(ppsIpAddr);
					Marshal.FreeBSTR(psIpAddr);
				}
            }
        }

        bool m_Working;
        Thread OCXThread1, OCXThread2;
		delegate void delegateAddLog(String sLogLine);
		delegateAddLog m_delegateAddLog;
		void AddLog(String sLogLine) //logging
		{
			rtbLog.Text += sLogLine + "\r\n";
			rtbLog.Select(rtbLog.Text.Length, 0);
		}

        public MainForm()
        {
            InitializeComponent();
            m_Working = false;
			m_delegateAddLog = new delegateAddLog(this.AddLog);
        }

        private void btnStartMulti_Click(object sender, EventArgs e)
        {
	        if (!m_Working)
	        {
		        btnStartMulti.Text = "Stop";
                m_Working = true;

                OCXThread1 = new Thread(new ParameterizedThreadStart(MainForm.OCXThreadProc));
		        OCXThread1.Name = "OCXThread1";
		        OCXThread1.Start(new OCXThreadParam(this, 2, "192.168.1.37", 5005, 0));

                OCXThread2 = new Thread(new ParameterizedThreadStart(MainForm.OCXThreadProc));
                OCXThread2.Name = "OCXThread2";
                OCXThread2.Start(new OCXThreadParam(this, 1, "192.168.1.16", 5005, 0));
            }
	        else
	        {
                OCXThread1.Abort();
                OCXThread1.Join();
                OCXThread2.Abort();
                OCXThread2.Join();

		        btnStartMulti.Text = "Start";
		        m_Working = false;
	        }
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (m_Working)
            {
                e.Cancel = true;
            }
        }

		private void MainForm_Load(object sender, EventArgs e)
		{
			SBXPCDLL._DotNET(); //inform that it is .NET system, and TO PRE-LOAD THE DLL(VERY IMPORTANT, otherwise there will be runtime error!, shw151013)
		}
    }

    public class OCXThreadParam
    {
        public MainForm mainForm;
        public int nDevID;
        public String sIpAddr;
        public int nPortNum;
        public int nPassword;

        public OCXThreadParam(MainForm _mainForm, int _nDevID, String _sIpAddr, int _nPortNum, int _nPassword)
        {
            mainForm = _mainForm;
            nDevID = _nDevID;
            sIpAddr = _sIpAddr;
            nPortNum = _nPortNum;
            nPassword = _nPassword;
        }
    }

    public class SBXPCDLL
    {
		[DllImport("SBXPCDLL.dll", CallingConvention = CallingConvention.Winapi)]
		public static extern void _DotNET();
		
		[DllImport("SBXPCDLL.dll", CallingConvention = CallingConvention.Winapi)]
        public static extern int _ConnectTcpip(int dwMachineNumber, IntPtr lpszIPAddress, int dwPortNumber, int dwPassWord);
        
        [DllImport("SBXPCDLL.dll", CallingConvention = CallingConvention.Winapi)]
        public static extern void _Disconnect(int dwMachineNumber);

        [DllImport("SBXPCDLL.dll", CallingConvention = CallingConvention.Winapi)]
        public static extern int _GetDeviceTime(int dwMachineNumber, ref int dwYear, ref int dwMonth, ref int dwDay, ref int dwHour, ref int dwMinute, ref int dwSecond, ref int dwDayOfWeek);
   }
}
