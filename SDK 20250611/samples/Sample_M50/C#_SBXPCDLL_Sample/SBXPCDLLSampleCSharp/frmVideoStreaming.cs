using SBXPCDLLSampleCSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SBXPCSampleCSharp
{
    public partial class frmVideoStreaming : Form
    {
        public frmVideoStreaming()
        {
            InitializeComponent();
        }

         const int CENTER_SCREEN_MSG_LEN = 100;

        const uint CENTER_SCREEN_MSG_DEF_COLOR = 0x54B248;
        const uint CENTER_SCREEN_MSG_DEF_BORDER_COLOR = 0xFFFFFF;

        private uint validate_msg_color(TextBox tb, uint defColor)
        {
            uint color;
            try
            {
                color = Convert.ToUInt32(tb.Text, 16);
            }
            catch
            {
                color = defColor;
            }
            color &= 0xFFFFFF;
            tb.Text = color.ToString("X6");
            return color;
        }

        private void frmVideoStreaming_Load(object sender, EventArgs e)
        {
            chkRtspEnable.Checked = false;
            cmbRtspResolution.SelectedIndex = 0;
            cmbRtspBitrateMbps.SelectedIndex = 0;

            txtTextColor.Text = CENTER_SCREEN_MSG_DEF_COLOR.ToString("X6");
            txtTextBorderColor.Text = CENTER_SCREEN_MSG_DEF_BORDER_COLOR.ToString("X6");
        }

        private void frmVideoStreaming_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.OpenForms["frmMain"].Visible = true;
        }

        private void btnGetRTSPSettings_Click(object sender, EventArgs e)
        {
            string strXML = null;

            sbxpc.SBXPCDLL.XML_AddString(ref strXML, "REQUEST", "GetVideoStreamSetting");
            sbxpc.SBXPCDLL.XML_AddString(ref strXML, "MSGTYPE", "request");
            sbxpc.SBXPCDLL.XML_AddLong(ref strXML, "MachineID", Program.gMachineNumber);

            if (sbxpc.SBXPCDLL.GeneralOperationXML(Program.gMachineNumber, ref strXML))
            {
                chkRtspEnable.Checked = sbxpc.SBXPCDLL.XML_ParseInt(ref strXML, "rtsp_enable") != 0;
                cmbRtspResolution.SelectedIndex = sbxpc.SBXPCDLL.XML_ParseInt(ref strXML, "rtsp_resolution");
                cmbRtspBitrateMbps.SelectedIndex = sbxpc.SBXPCDLL.XML_ParseInt(ref strXML, "rtsp_bitrate_mbps");

                MessageBox.Show("Get RTSP Settings OK!");
            }
            else
            {
                MessageBox.Show("Get RTSP Settings Failed.");
            }

        }

        private void btnSetRTSPSettings_Click(object sender, EventArgs e)
        {
            string strXML = null;
            sbxpc.SBXPCDLL.XML_AddString(ref strXML, "REQUEST", "SetVideoStreamSetting");
            sbxpc.SBXPCDLL.XML_AddString(ref strXML, "MSGTYPE", "request");
            sbxpc.SBXPCDLL.XML_AddLong(ref strXML, "MachineID", Program.gMachineNumber);

            sbxpc.SBXPCDLL.XML_AddLong(ref strXML, "rtsp_enable", chkRtspEnable.Checked ? 1 : 0);
            sbxpc.SBXPCDLL.XML_AddLong(ref strXML, "rtsp_resolution", cmbRtspResolution.SelectedIndex);
            sbxpc.SBXPCDLL.XML_AddLong(ref strXML, "rtsp_bitrate_mbps", cmbRtspBitrateMbps.SelectedIndex);

            if (sbxpc.SBXPCDLL.GeneralOperationXML(Program.gMachineNumber, ref strXML))
            {
                MessageBox.Show("Set RTSP Settings OK!");
            }
            else
            {
                MessageBox.Show("Set RTSP Settings Failed.");
            }
        }

        private void btnGetCenterScreenMsg_Click(object sender, EventArgs e)
        {
            string strXML = null;
            string strValue = "";
            
            sbxpc.SBXPCDLL.XML_AddString(ref strXML, "REQUEST", "GetCenterScreenMessage");
            sbxpc.SBXPCDLL.XML_AddString(ref strXML, "MSGTYPE", "request");
            sbxpc.SBXPCDLL.XML_AddLong(ref strXML, "MachineID", Program.gMachineNumber);

            if (sbxpc.SBXPCDLL.GeneralOperationXML(Program.gMachineNumber, ref strXML))
            {
                sbxpc.SBXPCDLL.XML_ParseBinaryUnicode(ref strXML, "center_screen_message", out strValue, CENTER_SCREEN_MSG_LEN * 2);
                txtCenterScreenMsg.Text = strValue.Replace("\n", "\r\n"); ;

                sbxpc.SBXPCDLL.XML_ParseString(ref strXML, "center_screen_message_color", out strValue);
                txtTextColor.Text = (Convert.ToUInt32(strValue, 16) & 0xFFFFFF).ToString("X6");

                sbxpc.SBXPCDLL.XML_ParseString(ref strXML, "center_screen_message_border_color", out strValue);
                txtTextBorderColor.Text = (Convert.ToUInt32(strValue, 16) & 0xFFFFFF).ToString("X6");

                chkVerifyDisable.Checked = sbxpc.SBXPCDLL.XML_ParseInt(ref strXML, "verify_disable") != 0;
              
                MessageBox.Show("Get Center Screen Message OK!");
            }
            else
            {
                MessageBox.Show("Get Center Screen Message Failed.");
            }

        }

        private void btnSetCenterScreenMsg_Click(object sender, EventArgs e)
        {
            string strXML = null;
            sbxpc.SBXPCDLL.XML_AddString(ref strXML, "REQUEST", "SetCenterScreenMessage");
            sbxpc.SBXPCDLL.XML_AddString(ref strXML, "MSGTYPE", "request");
            sbxpc.SBXPCDLL.XML_AddLong(ref strXML, "MachineID", Program.gMachineNumber);

            sbxpc.SBXPCDLL.XML_AddBinaryUnicode(ref strXML, "center_screen_message", txtCenterScreenMsg.Text.Replace("\r\n", "\n"));

            uint color = validate_msg_color(txtTextColor, CENTER_SCREEN_MSG_DEF_COLOR);
            uint border_color = validate_msg_color(txtTextBorderColor, CENTER_SCREEN_MSG_DEF_BORDER_COLOR);

            sbxpc.SBXPCDLL.XML_AddString(ref strXML, "center_screen_message_color", "FF" + color.ToString("X6"));
            sbxpc.SBXPCDLL.XML_AddString(ref strXML, "center_screen_message_border_color", "FF" + border_color.ToString("X6"));
            
            sbxpc.SBXPCDLL.XML_AddLong(ref strXML, "verify_disable", chkVerifyDisable.Checked ? 1 : 0);

            if (sbxpc.SBXPCDLL.GeneralOperationXML(Program.gMachineNumber, ref strXML))
            {
                MessageBox.Show("Set Center Screen Message OK!");
            }
            else
            {
                MessageBox.Show("Set Center Screen Message Failed.");
            }
        }

        private void btnTextColorPiker_Click(object sender, EventArgs e)
        {
            using (ColorDialog colorDialog = new ColorDialog())
            {
                colorDialog.Color = Color.FromArgb((int)(validate_msg_color(txtTextColor, CENTER_SCREEN_MSG_DEF_COLOR) + 0xFF000000));
                if (colorDialog.ShowDialog() == DialogResult.OK)
                {
                    txtTextColor.Text = (colorDialog.Color.ToArgb() & 0xFFFFFF).ToString("X6");
                }
            }
        }

        private void btnTextBorderColorPiker_Click(object sender, EventArgs e)
        {
            using (ColorDialog colorDialog = new ColorDialog())
            {
                colorDialog.Color = Color.FromArgb((int)(validate_msg_color(txtTextBorderColor, CENTER_SCREEN_MSG_DEF_COLOR) + 0xFF000000));
                if (colorDialog.ShowDialog() == DialogResult.OK)
                {
                    txtTextBorderColor.Text = (colorDialog.Color.ToArgb() & 0xFFFFFF).ToString("X6");
                }
            }
        }
    }
}
