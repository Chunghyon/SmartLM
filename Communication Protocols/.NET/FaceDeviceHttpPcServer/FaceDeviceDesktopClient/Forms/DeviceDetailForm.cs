using System.Net.Http.Json;

namespace FaceDeviceDesktopClient.Forms;

public partial class DeviceDetailForm : Form
{
    private readonly DeviceInfo _device;
    private readonly HttpClient _httpClient;

    // UI Controls
    private TabControl tabControl = null!;
    private TabPage tabBasicInfo = null!;
    private TabPage tabSetting = null!;
    private TabPage tabCommunication = null!;

    // Basic Info controls
    private TextBox txtDeviceName = null!;
    private TextBox txtTagName = null!;
    private TextBox txtSN = null!;
    private TextBox txtModel = null!;
    private TextBox txtFirmwareVersion = null!;
    private TextBox txtIpAddress = null!;
    private NumericUpDown nudHttpPort = null!;
    private NumericUpDown nudUnitNo = null!;

    // Buttons
    private Button btnSave = null!;
    private Button btnCancel = null!;

    public DeviceDetailForm(DeviceInfo device, HttpClient httpClient)
    {
        _device = device;
        _httpClient = httpClient;

        InitializeComponent();
        LoadDeviceData();
    }

    private void InitializeComponent()
    {
        this.Text = $"Device Details - {_device.DeviceName}";
        this.Size = new Size(600, 500);
        this.StartPosition = FormStartPosition.CenterParent;
        this.FormBorderStyle = FormBorderStyle.FixedDialog;
        this.MaximizeBox = false;
        this.MinimizeBox = false;

        // Tab Control
        tabControl = new TabControl
        {
            Dock = DockStyle.Fill,
            Padding = new Point(10, 10)
        };

        // Create tabs
        CreateBasicInfoTab();
        CreateSettingTab();
        CreateCommunicationTab();

        tabControl.TabPages.Add(tabBasicInfo);
        tabControl.TabPages.Add(tabSetting);
        tabControl.TabPages.Add(tabCommunication);

        // Buttons
        var panelButtons = new Panel
        {
            Dock = DockStyle.Bottom,
            Height = 50,
            Padding = new Padding(10)
        };

        btnSave = new Button
        {
            Text = "Save",
            Size = new Size(100, 30),
            Location = new Point(370, 10),
            DialogResult = DialogResult.OK
        };
        btnSave.Click += BtnSave_Click;

        btnCancel = new Button
        {
            Text = "Cancel",
            Size = new Size(100, 30),
            Location = new Point(480, 10),
            DialogResult = DialogResult.Cancel
        };

        panelButtons.Controls.Add(btnSave);
        panelButtons.Controls.Add(btnCancel);

        this.Controls.Add(tabControl);
        this.Controls.Add(panelButtons);
        this.AcceptButton = btnSave;
        this.CancelButton = btnCancel;
    }

    private void CreateBasicInfoTab()
    {
        tabBasicInfo = new TabPage("Basic Info");
        tabBasicInfo.Padding = new Padding(10);

        int y = 20;
        int labelWidth = 120;
        int controlWidth = 400;

        // Device Name
        tabBasicInfo.Controls.Add(new Label 
        { 
            Text = "Device Name:", 
            Location = new Point(20, y), 
            Width = labelWidth 
        });
        txtDeviceName = new TextBox 
        { 
            Location = new Point(150, y), 
            Width = controlWidth 
        };
        tabBasicInfo.Controls.Add(txtDeviceName);
        y += 35;

        // Tag Name
        tabBasicInfo.Controls.Add(new Label 
        { 
            Text = "Tag Name:", 
            Location = new Point(20, y), 
            Width = labelWidth 
        });
        txtTagName = new TextBox 
        { 
            Location = new Point(150, y), 
            Width = controlWidth 
        };
        tabBasicInfo.Controls.Add(txtTagName);
        y += 35;

        // SN (Read-only)
        tabBasicInfo.Controls.Add(new Label 
        { 
            Text = "Serial Number:", 
            Location = new Point(20, y), 
            Width = labelWidth 
        });
        txtSN = new TextBox 
        { 
            Location = new Point(150, y), 
            Width = controlWidth,
            ReadOnly = true,
            BackColor = SystemColors.Control
        };
        tabBasicInfo.Controls.Add(txtSN);
        y += 35;

        // Model (Read-only)
        tabBasicInfo.Controls.Add(new Label 
        { 
            Text = "Model:", 
            Location = new Point(20, y), 
            Width = labelWidth 
        });
        txtModel = new TextBox 
        { 
            Location = new Point(150, y), 
            Width = controlWidth,
            ReadOnly = true,
            BackColor = SystemColors.Control
        };
        tabBasicInfo.Controls.Add(txtModel);
        y += 35;

        // Firmware Version (Read-only)
        tabBasicInfo.Controls.Add(new Label 
        { 
            Text = "Firmware Version:", 
            Location = new Point(20, y), 
            Width = labelWidth 
        });
        txtFirmwareVersion = new TextBox 
        { 
            Location = new Point(150, y), 
            Width = controlWidth,
            ReadOnly = true,
            BackColor = SystemColors.Control
        };
        tabBasicInfo.Controls.Add(txtFirmwareVersion);
        y += 35;

        // Unit No
        tabBasicInfo.Controls.Add(new Label 
        { 
            Text = "Unit No.:", 
            Location = new Point(20, y), 
            Width = labelWidth 
        });
        nudUnitNo = new NumericUpDown 
        { 
            Location = new Point(150, y), 
            Width = 100,
            Minimum = 0,
            Maximum = 999
        };
        tabBasicInfo.Controls.Add(nudUnitNo);
    }

    private void CreateSettingTab()
    {
        tabSetting = new TabPage("Setting");
        tabSetting.Padding = new Padding(10);

        var lblInfo = new Label
        {
            Text = "Device settings can be configured here.\nThis feature will be implemented in a future update.",
            Location = new Point(20, 20),
            AutoSize = true
        };
        tabSetting.Controls.Add(lblInfo);
    }

    private void CreateCommunicationTab()
    {
        tabCommunication = new TabPage("Communication");
        tabCommunication.Padding = new Padding(10);

        int y = 20;
        int labelWidth = 120;
        int controlWidth = 400;

        // IP Address
        tabCommunication.Controls.Add(new Label 
        { 
            Text = "IP Address:", 
            Location = new Point(20, y), 
            Width = labelWidth 
        });
        txtIpAddress = new TextBox 
        { 
            Location = new Point(150, y), 
            Width = controlWidth 
        };
        tabCommunication.Controls.Add(txtIpAddress);
        y += 35;

        // HTTP Port
        tabCommunication.Controls.Add(new Label 
        { 
            Text = "HTTP Port:", 
            Location = new Point(20, y), 
            Width = labelWidth 
        });
        nudHttpPort = new NumericUpDown 
        { 
            Location = new Point(150, y), 
            Width = 100,
            Minimum = 1,
            Maximum = 65535,
            Value = 80
        };
        tabCommunication.Controls.Add(nudHttpPort);
    }

    private void LoadDeviceData()
    {
        txtDeviceName.Text = _device.DeviceName ?? "";
        txtTagName.Text = _device.TagName ?? "";
        txtSN.Text = _device.SN;
        txtModel.Text = _device.Model ?? "Unknown";
        txtFirmwareVersion.Text = _device.FirmwareVersion ?? "Unknown";
        txtIpAddress.Text = _device.IpAddress ?? "";
        nudHttpPort.Value = _device.HttpPort;
        nudUnitNo.Value = _device.UnitNo;
    }

    private async void BtnSave_Click(object? sender, EventArgs e)
    {
        try
        {
            btnSave.Enabled = false;
            btnSave.Text = "Saving...";

            // Prepare update payload
            var updatePayload = new
            {
                DeviceSN = _device.SN,
                IpAddress = txtIpAddress.Text.Trim(),
                HttpPort = (int)nudHttpPort.Value,
                DeviceName = txtDeviceName.Text.Trim(),
                TagName = txtTagName.Text.Trim(),
                Model = _device.Model,
                FirmwareVersion = _device.FirmwareVersion,
                UnitNo = (int)nudUnitNo.Value
            };

            // Send update request
            var response = await _httpClient.PostAsJsonAsync("/api/Device/Connect", updatePayload);

            if (response.IsSuccessStatusCode)
            {
                MessageBox.Show("Device updated successfully!", "Success", 
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                MessageBox.Show($"Failed to update device: HTTP {response.StatusCode}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                btnSave.Enabled = true;
                btnSave.Text = "Save";
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Failed to update device: {ex.Message}", "Error", 
                MessageBoxButtons.OK, MessageBoxIcon.Error);
            btnSave.Enabled = true;
            btnSave.Text = "Save";
        }
    }
}
