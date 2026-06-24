namespace FaceDeviceDesktopClient;

public class DebugLogForm : Form
{
    private static DebugLogForm? _instance;
    private readonly RichTextBox _logTextBox;

    public static DebugLogForm Instance
    {
        get
        {
            if (_instance == null || _instance.IsDisposed)
            {
                _instance = new DebugLogForm();
            }
            return _instance;
        }
    }

    private DebugLogForm()
    {
        Text = "디버그 로그";
        Size = new Size(900, 600);
        StartPosition = FormStartPosition.CenterScreen;

        _logTextBox = new RichTextBox
        {
            Dock = DockStyle.Fill,
            Font = new Font("Consolas", 9F),
            ReadOnly = true,
            BackColor = Color.Black,
            ForeColor = Color.LightGreen,
            WordWrap = false
        };

        var panel = new Panel
        {
            Dock = DockStyle.Bottom,
            Height = 40
        };

        var btnClear = new Button
        {
            Text = "로그 지우기",
            Location = new Point(10, 8),
            Size = new Size(100, 25)
        };
        btnClear.Click += (s, e) => Clear();

        panel.Controls.Add(btnClear);

        Controls.Add(_logTextBox);
        Controls.Add(panel);

        FormClosing += (s, e) =>
        {
            e.Cancel = true;
            Hide();
        };
    }

    public void Log(string message)
    {
        if (InvokeRequired)
        {
            Invoke(() => Log(message));
            return;
        }

        _logTextBox.AppendText($"[{DateTime.Now:HH:mm:ss.fff}] {message}\n");
        _logTextBox.ScrollToCaret();
    }

    public void Clear()
    {
        if (InvokeRequired)
        {
            Invoke(Clear);
            return;
        }

        _logTextBox.Clear();
    }

    public new void Show()
    {
        if (InvokeRequired)
        {
            Invoke(Show);
            return;
        }

        if (!Visible)
        {
            base.Show();
        }
        BringToFront();
    }
}
