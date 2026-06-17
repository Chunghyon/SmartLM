using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace FaceDeviceDesktopClient.Forms;

public class DeviceInstallForm : Form
{
    private TextBox txtDeviceName;
    private TextBox txtTagName;
    private TextBox txtMenuPassword;
    private ComboBox cmbLanguage;
    private Button btnInstall;
    private Button btnCancel;

    public string DeviceName => txtDeviceName.Text.Trim();
    public string TagName => txtTagName.Text.Trim();
    public string MenuPassword => txtMenuPassword.Text.Trim();
    public string Language => cmbLanguage.SelectedItem?.ToString() ?? "English";

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public string DeviceSN { get; set; } = string.Empty;

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public string IpAddress { get; set; } = string.Empty;

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public int HttpPort { get; set; } = 80;

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public string? Model { get; set; }

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public string? FirmwareVersion { get; set; }

    public DeviceInstallForm()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        Text = "Install Device";
        Size = new System.Drawing.Size(450, 320);
        StartPosition = FormStartPosition.CenterParent;
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;

        // Device SN Label
        var lblSN = new Label
        {
            Text = "Device SN:",
            Location = new System.Drawing.Point(20, 20),
            Size = new System.Drawing.Size(120, 25),
            TextAlign = System.Drawing.ContentAlignment.MiddleRight
        };

        var lblSNValue = new Label
        {
            Text = "",
            Location = new System.Drawing.Point(150, 20),
            Size = new System.Drawing.Size(260, 25),
            Font = new System.Drawing.Font(SystemFonts.DefaultFont.FontFamily, 10, System.Drawing.FontStyle.Bold),
            ForeColor = System.Drawing.Color.DarkBlue
        };
        lblSNValue.Name = "lblSNValue";

        // Device Name
        var lblDeviceName = new Label
        {
            Text = "Device Name:",
            Location = new System.Drawing.Point(20, 55),
            Size = new System.Drawing.Size(120, 25),
            TextAlign = System.Drawing.ContentAlignment.MiddleRight
        };

        txtDeviceName = new TextBox
        {
            Location = new System.Drawing.Point(150, 55),
            Size = new System.Drawing.Size(260, 25),
            PlaceholderText = "Enter device name (e.g., Main Entrance)"
        };

        // Tag Name
        var lblTagName = new Label
        {
            Text = "Tag Name:",
            Location = new System.Drawing.Point(20, 90),
            Size = new System.Drawing.Size(120, 25),
            TextAlign = System.Drawing.ContentAlignment.MiddleRight
        };

        txtTagName = new TextBox
        {
            Location = new System.Drawing.Point(150, 90),
            Size = new System.Drawing.Size(260, 25),
            PlaceholderText = "Enter tag name (optional)"
        };

        // Menu Password
        var lblMenuPassword = new Label
        {
            Text = "Menu Password:",
            Location = new System.Drawing.Point(20, 125),
            Size = new System.Drawing.Size(120, 25),
            TextAlign = System.Drawing.ContentAlignment.MiddleRight
        };

        txtMenuPassword = new TextBox
        {
            Location = new System.Drawing.Point(150, 125),
            Size = new System.Drawing.Size(260, 25),
            PlaceholderText = "Enter menu password (default: 888888)",
            Text = "888888",
            UseSystemPasswordChar = true
        };

        // Language
        var lblLanguage = new Label
        {
            Text = "Language:",
            Location = new System.Drawing.Point(20, 160),
            Size = new System.Drawing.Size(120, 25),
            TextAlign = System.Drawing.ContentAlignment.MiddleRight
        };

        cmbLanguage = new ComboBox
        {
            Location = new System.Drawing.Point(150, 160),
            Size = new System.Drawing.Size(260, 25),
            DropDownStyle = ComboBoxStyle.DropDownList
        };
        cmbLanguage.Items.AddRange(new object[] 
        { 
            "English", 
            "Chinese (Simplified)", 
            "Chinese (Traditional)", 
            "Korean", 
            "Japanese",
            "Spanish",
            "French",
            "German",
            "Italian",
            "Portuguese",
            "Russian",
            "Arabic",
            "Thai",
            "Vietnamese"
        });
        cmbLanguage.SelectedIndex = 0;

        // Info Label
        var lblInfo = new Label
        {
            Text = "This will install the device and configure its initial settings.\nThe device will be registered with the server.",
            Location = new System.Drawing.Point(20, 200),
            Size = new System.Drawing.Size(390, 40),
            ForeColor = System.Drawing.Color.Gray,
            Font = new System.Drawing.Font(SystemFonts.DefaultFont.FontFamily, 8)
        };

        // Buttons
        btnInstall = new Button
        {
            Text = "Install",
            DialogResult = DialogResult.OK,
            Location = new System.Drawing.Point(250, 245),
            Size = new System.Drawing.Size(80, 30)
        };

        btnCancel = new Button
        {
            Text = "Cancel",
            DialogResult = DialogResult.Cancel,
            Location = new System.Drawing.Point(340, 245),
            Size = new System.Drawing.Size(80, 30)
        };

        Controls.AddRange(new Control[] 
        { 
            lblSN, lblSNValue,
            lblDeviceName, txtDeviceName, 
            lblTagName, txtTagName,
            lblMenuPassword, txtMenuPassword,
            lblLanguage, cmbLanguage,
            lblInfo,
            btnInstall, btnCancel 
        });

        AcceptButton = btnInstall;
        CancelButton = btnCancel;

        Load += (s, e) =>
        {
            var lblSNValueControl = Controls["lblSNValue"] as Label;
            if (lblSNValueControl != null)
            {
                lblSNValueControl.Text = DeviceSN;
            }
        };
    }
}
