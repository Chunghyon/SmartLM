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
    public partial class frmDaigong : Form
    {
        public frmDaigong()
        {
            InitializeComponent();
        }

        AxSBXPCLib.AxSBXPC bpc;

        const int PROXY_COUNT = 20;
        string[] proxyNameList;

        private void DrawDepartmentList()
        {
            lstProxy.Items.Clear();
            string itemString;
            for (int i = 0; i < PROXY_COUNT; i++)
            {
                itemString = "[No.]" + String.Format("{0:D2}", i) + " ";
                itemString += "[Name]" + proxyNameList[i];
                lstProxy.Items.Add(itemString);
            }
        }

        private void lstDepartment_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtProxy.Text = proxyNameList[lstProxy.SelectedIndex];
            txtProxy.Focus();
        }

        private void cmdUpdate_Click(object sender, EventArgs e)
        {
            proxyNameList[lstProxy.SelectedIndex] = txtProxy.Text;
            DrawDepartmentList();
        }

        private void frmDaigong_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.OpenForms["frmMain"].Visible = true;
        }

        private void cmdExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void cmdRead_Click(object sender, EventArgs e)
        {
            bool bRet;
            int vErrorCode = 0;

            lblMessage.Text = "Working...";
            Application.DoEvents();

            bRet = bpc.EnableDevice(Program.gMachineNumber, 0);
            if (!bRet)
            {
                lblMessage.Text = util.gstrNoDevice;
                return;
            }

            for (int i = 0; i < PROXY_COUNT; i++)
                bRet = bpc.GetDepartName(Program.gMachineNumber, i + 1, 1, ref proxyNameList[i]) && bRet;
            if (bRet)
                lblMessage.Text = "Success";
            else
            {
                bpc.GetLastError(ref vErrorCode);
                lblMessage.Text = util.ErrorPrint(vErrorCode);
            }

            bRet = bpc.EnableDevice(Program.gMachineNumber, 1);

            DrawDepartmentList();
        }

        private void cmdWrite_Click(object sender, EventArgs e)
        {
            bool bRet;
            int vErrorCode = 0;

            lblMessage.Text = "Working...";
            Application.DoEvents();

            bRet = bpc.EnableDevice(Program.gMachineNumber, 0);
            if (!bRet)
            {
                lblMessage.Text = util.gstrNoDevice;
                return;
            }

            for (int i = 0; i < PROXY_COUNT; i++)
                bRet = bpc.SetDepartName(Program.gMachineNumber, i + 1, 1, ref proxyNameList[i]) && bRet;
            if (bRet)
                lblMessage.Text = "Success";
            else
            {
                bpc.GetLastError(ref vErrorCode);
                lblMessage.Text = util.ErrorPrint(vErrorCode);
            }

            bRet = bpc.EnableDevice(Program.gMachineNumber, 1);

            DrawDepartmentList();
        }

        private void frmDaigong_Load(object sender, EventArgs e)
        {
            bpc = (AxSBXPCLib.AxSBXPC)Application.OpenForms["frmMain"].Controls["SBXPC1"];
            proxyNameList = new string[PROXY_COUNT];
            DrawDepartmentList();
        }
    }
}
