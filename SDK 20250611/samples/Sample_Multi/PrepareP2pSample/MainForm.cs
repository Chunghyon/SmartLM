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

namespace PrepareP2pSample
{
    public partial class MainForm : Form
    {
        const string APP_TITLE = "PrepareP2pSample";
        const string REG_KEY = "Software\\Smackbio\\PrepareP2pSample";

        enum p2p_erros
        {
	        P2P_ERR_UNKNOWN,
	        P2P_ERR_CANNOT_CONNECT_SERVER,
	        P2P_ERR_NOT_FOUND,
	        P2P_ERR_PWD_MISMATCHED,
	        P2P_ERR_RELAYED,
	        P2P_ERR_DIRECT_LOCAL,
        };

        List<DeviceEntry> devices = new List<DeviceEntry>();
        string svrIp;
        int svrPort;

        public MainForm()
        {
            InitializeComponent();
        }

		void RefreshDeviceStatusThread(Object state)
		{
            string[] errs = 
            {
	            "UNKNOWN",
	            "CANNOT_CONNECT_SERVER",
	            "OFFLINE", //"NOT_FOUND",
	            "PWD_MISMATCHED",
	            "RELAYED",
	            "DIRECT_LOCAL",
            };
            List<DeviceEntry> devices_tmp = new List<DeviceEntry>();
            string svrIp_tmp;
            int svrPort_tmp;

            while (true)
            {
                devices_tmp.Clear();
                lock (devices)
                {
                    foreach (DeviceEntry entry in devices)
                    {
                        devices_tmp.Add(new DeviceEntry(entry.id));
                    }
                }
                lock(svrIp)
                {
                    svrIp_tmp = String.Copy(svrIp);
                    svrPort_tmp = svrPort;
                }

                foreach (DeviceEntry entry in devices_tmp)
                {
                    string devid = Convert.ToString((Int64)entry.id, 16);
                    int dwYearStart=0, dwMonthStart=0, dwDayStart=0, dwYearEnd=0, dwMonthEnd=0, dwDayEnd=0, nError=0;
                    bool ret = axSBXPC1.PrepareP2p(ref devid, ref svrIp_tmp, svrPort_tmp,
                        ref dwYearStart, ref dwMonthStart, ref dwDayStart, ref dwYearEnd, ref dwMonthEnd, ref dwDayEnd, ref nError);

                    if (!ret)
                    {
                        entry.status = errs[nError];
                        if ((p2p_erros)nError == p2p_erros.P2P_ERR_NOT_FOUND)
                        {
                            try
                            {
                                entry.startDate = new DateTime(dwYearStart, dwMonthStart, dwDayStart);
                                entry.endDate = new DateTime(dwYearEnd, dwMonthEnd, dwDayEnd);
                            }
                            catch (System.Exception ex)
                            {//if all (dwYearStart, dwMonthStart, dwDayStart, dwYearEnd, dwMonthEnd, dwDayEnd) return 0
                                //never registered in the server, the server has no info for the device.
                                entry.status = "OFFLINE (NOT REGISTERED)";
                            }
                        }
                    }
                    else
                    {
                        entry.status = "ONLINE";
                        entry.startDate = new DateTime(dwYearStart, dwMonthStart, dwDayStart);
                        entry.endDate = new DateTime(dwYearEnd, dwMonthEnd, dwDayEnd);
                    }
                }

                lock (devices)
                {
                    List<DeviceEntry>.Enumerator e = devices.GetEnumerator();
                    foreach (DeviceEntry entry in devices_tmp)
                    {
                        e.MoveNext();
                        e.Current.status = String.Copy(entry.status);
                        e.Current.startDate = entry.startDate;
                        e.Current.endDate = entry.endDate;
                    }
                }
                Thread.Sleep(1000);
            }
		}

        private void btnAddDevice_Click(object sender, EventArgs e)
        {
            UInt64 id = Convert.ToUInt64(txtDeviceId.Text, 16);
            lock (devices)
            {
                foreach (DeviceEntry entry in devices)
                {
                    if (entry.id == id)
                        return;
                }
            }
            ListViewItem item = new ListViewItem(Convert.ToString((Int64)id, 16));
            item.SubItems.Add("pending");
            item.SubItems.Add("pending");
            item.SubItems.Add("pending");
            lstDevices.Items.Add(item);
            lock(devices)
            {
                devices.Add(new DeviceEntry(id));
            }
        }

        private void txtServerIp_TextChanged(object sender, EventArgs e)
        {
            try
            {
                RegistryKey appKey = Registry.CurrentUser.CreateSubKey(REG_KEY);
                appKey.SetValue("ServerIp", txtServerIp.Text);
                lock (svrIp)
                {
                    svrIp = txtServerIp.Text;
                }
            }
            catch (System.Exception ex)
            {
            }
        }

        private void txtServerPort_TextChanged(object sender, EventArgs e)
        {
            try
            {
                RegistryKey appKey = Registry.CurrentUser.CreateSubKey(REG_KEY);
                appKey.SetValue("ServerPort", txtServerPort.Text);
                lock (svrIp)
                {
                    svrPort = Convert.ToInt32(txtServerPort.Text);
                }
            }
            catch (System.Exception ex)
            {
            }
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            try
            {
                RegistryKey appKey = Registry.CurrentUser.CreateSubKey(REG_KEY);
                txtServerIp.Text = (string)appKey.GetValue("ServerIp", "192.168.1.200");
                txtServerPort.Text = (string)appKey.GetValue("ServerPort", "4000");

                svrIp = txtServerIp.Text;
                svrPort = Convert.ToInt32(txtServerPort.Text);
            }
            catch (System.Exception ex)
            {
            }

            ThreadPool.QueueUserWorkItem(new WaitCallback(RefreshDeviceStatusThread));
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            lock (devices)
            {
                int no = 0;
                foreach (DeviceEntry entry in devices)
                {
                    lstDevices.Items[no].SubItems[1].Text = entry.status;
                    if (entry.status == "ONLINE" || entry.status == "OFFLINE")
                    {
                        lstDevices.Items[no].SubItems[2].Text = entry.startDate.ToString();
                        lstDevices.Items[no].SubItems[3].Text = entry.endDate.ToString();
                    }
                    else
                    {
                        lstDevices.Items[no].SubItems[2].Text = "unknown";
                        lstDevices.Items[no].SubItems[3].Text = "unknown";
                    }
                    no++;
                }
            }
        }
    }

    public class DeviceEntry
    {
        public UInt64 id;
        public string status;
        public DateTime startDate;
        public DateTime endDate;

        public DeviceEntry(UInt64 _id)
        {
            id = _id;
            status = "unknown";
        }
    }
}
