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
        Text = "단말기 등록";
        Size = new System.Drawing.Size(450, 320);
        StartPosition = FormStartPosition.CenterParent;
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;

        // Device SN Label
        var lblSN = new Label
        {
            Text = "시리얼넘버:",
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
            Text = "단말기명:",
            Location = new System.Drawing.Point(20, 55),
            Size = new System.Drawing.Size(120, 25),
            TextAlign = System.Drawing.ContentAlignment.MiddleRight
        };

        txtDeviceName = new TextBox
        {
            Location = new System.Drawing.Point(150, 55),
            Size = new System.Drawing.Size(260, 25),
            PlaceholderText = "단말기 이름 입력 (예: 정문)"
        };

        // Tag Name
        var lblTagName = new Label
        {
            Text = "위치:",
            Location = new System.Drawing.Point(20, 90),
            Size = new System.Drawing.Size(120, 25),
            TextAlign = System.Drawing.ContentAlignment.MiddleRight
        };

        txtTagName = new TextBox
        {
            Location = new System.Drawing.Point(150, 90),
            Size = new System.Drawing.Size(260, 25),
            PlaceholderText = "위치 정보 입력 (선택사항)"
        };

        // Menu Password
        var lblMenuPassword = new Label
        {
            Text = "관리패스워드:",
            Location = new System.Drawing.Point(20, 125),
            Size = new System.Drawing.Size(120, 25),
            TextAlign = System.Drawing.ContentAlignment.MiddleRight
        };

        txtMenuPassword = new TextBox
        {
            Location = new System.Drawing.Point(150, 125),
            Size = new System.Drawing.Size(260, 25),
            PlaceholderText = "관리 비밀번호 입력 (기본값: 888888)",
            Text = "888888",
            UseSystemPasswordChar = true
        };

        // Language
        var lblLanguage = new Label
        {
            Text = "언어:",
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
            Text = "단말기를 등록하고 초기 설정을 구성합니다.\n단말기가 서버에 등록됩니다.",
            Location = new System.Drawing.Point(20, 200),
            Size = new System.Drawing.Size(390, 40),
            ForeColor = System.Drawing.Color.Gray,
            Font = new System.Drawing.Font(SystemFonts.DefaultFont.FontFamily, 8)
        };

        // Buttons
        btnInstall = new Button
        {
            Text = "등록",
            DialogResult = DialogResult.OK,
            Location = new System.Drawing.Point(250, 245),
            Size = new System.Drawing.Size(80, 30)
        };

        btnCancel = new Button
        {
            Text = "취소",
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
