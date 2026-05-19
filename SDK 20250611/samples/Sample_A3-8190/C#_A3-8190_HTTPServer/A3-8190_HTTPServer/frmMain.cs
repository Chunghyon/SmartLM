using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using A3_8190_HTTPServer.HttpServer;
using A3_8190_HTTPServer.Models;

namespace A3_8190_HTTPServer
{
    public partial class frmMain : Form
    {
        private readonly A3HttpListener _server = new A3HttpListener();

        // 연결된 디바이스 SN → ListViewItem 캐시
        private readonly Dictionary<string, ListViewItem> _deviceItems =
            new Dictionary<string, ListViewItem>(StringComparer.OrdinalIgnoreCase);

        private frmPersonnel _frmPersonnel;
        private frmRecords   _frmRecords;

        public frmMain()
        {
            InitializeComponent();

            _server.OnLog       += (msg)       => SafeInvoke(() => AppendLog(msg));
            _server.OnKeepalive += (sn, req)   => SafeInvoke(() => UpdateDevice(sn, req));
            _server.OnRecord    += (sn, rec)   => _frmRecords?.AddRecord(sn, rec);
        }

        // ─── Server Start / Stop ─────────────────────────────────────────────
        private void btnStart_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(txtPort.Text.Trim(), out int port) || port < 1 || port > 65535)
            {
                MessageBox.Show("유효한 포트 번호를 입력하세요 (1–65535).",
                                "입력 오류", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                _server.Start(port);
                btnStart.Enabled  = false;
                btnStop.Enabled   = true;
                txtPort.Enabled   = false;
                lblStatus.Text      = "● 실행 중";
                lblStatus.ForeColor = Color.Green;
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"서버 시작 실패:\n{ex.Message}\n\n" +
                    "관리자 권한으로 실행하거나, 다른 포트를 입력하세요.\n" +
                    "(app.manifest에 requireAdministrator 설정 포함)",
                    "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            _server.Stop();
            btnStart.Enabled  = true;
            btnStop.Enabled   = false;
            txtPort.Enabled   = true;
            lblStatus.Text      = "● 중지됨";
            lblStatus.ForeColor = Color.Red;
        }

        // ─── Remote Control ───────────────────────────────────────────────────
        private void btnOpenDoor_Click(object sender, EventArgs e)
        {
            string sn = GetSelectedSN(); if (sn == null) return;
            _server.QueueRemoteCommand(sn, new PendingRemoteCommand { Opendoor = 1 });
        }

        private void btnKeepOpen_Click(object sender, EventArgs e)
        {
            string sn = GetSelectedSN(); if (sn == null) return;
            _server.QueueRemoteCommand(sn, new PendingRemoteCommand { Opendoor = 2 });
        }

        private void btnCloseDoor_Click(object sender, EventArgs e)
        {
            string sn = GetSelectedSN(); if (sn == null) return;
            _server.QueueRemoteCommand(sn, new PendingRemoteCommand { Opendoor = 3 });
        }

        private void btnRestart_Click(object sender, EventArgs e)
        {
            string sn = GetSelectedSN(); if (sn == null) return;
            if (MessageBox.Show($"'{sn}' 디바이스를 원격 재시작하시겠습니까?",
                                "재시작 확인", MessageBoxButtons.YesNo,
                                MessageBoxIcon.Question) == DialogResult.Yes)
                _server.QueueRemoteCommand(sn, new PendingRemoteCommand { Restart = 1 });
        }

        private string GetSelectedSN()
        {
            string sn = cmbDevice.SelectedItem as string;
            if (string.IsNullOrEmpty(sn))
            {
                MessageBox.Show("디바이스를 선택하세요.", "알림",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                return null;
            }
            return sn;
        }

        // ─── Navigation ───────────────────────────────────────────────────────
        private void btnPersonnel_Click(object sender, EventArgs e)
        {
            if (_frmPersonnel == null || _frmPersonnel.IsDisposed)
                _frmPersonnel = new frmPersonnel(_server);
            _frmPersonnel.Show();
            _frmPersonnel.BringToFront();
        }

        private void btnRecords_Click(object sender, EventArgs e)
        {
            if (_frmRecords == null || _frmRecords.IsDisposed)
                _frmRecords = new frmRecords(_server.Records);
            _frmRecords.Show();
            _frmRecords.BringToFront();
        }

        private void btnClearLog_Click(object sender, EventArgs e) => rtbLog.Clear();

        // ─── UI Helpers ───────────────────────────────────────────────────────
        private void AppendLog(string msg)
        {
            rtbLog.AppendText(msg + Environment.NewLine);
            rtbLog.ScrollToCaret();
            // 최대 2000줄 유지
            if (rtbLog.Lines.Length > 2000)
            {
                int nl = rtbLog.Text.IndexOf('\n');
                if (nl >= 0) { rtbLog.Select(0, nl + 1); rtbLog.SelectedText = ""; }
            }
        }

        private void UpdateDevice(string sn, KeepaliveRequest req)
        {
            string relay  = req.RelayStatus      == 1 ? "열림(NO)" : "닫힘(NC)";
            string door   = req.DoorSensorStatus == 1 ? "열림"     : "닫힘";
            string lock_  = req.LockDoorStatus   == 1 ? "잠김"     : "해제";
            string alarm  = string.IsNullOrEmpty(req.AlarmStatus) ? "없음" : req.AlarmStatus;
            string time   = DateTime.Now.ToString("HH:mm:ss");

            if (_deviceItems.TryGetValue(sn, out ListViewItem item))
            {
                item.SubItems[1].Text = time;
                item.SubItems[2].Text = relay;
                item.SubItems[3].Text = door;
                item.SubItems[4].Text = lock_;
                item.SubItems[5].Text = alarm;
            }
            else
            {
                item = new ListViewItem(new[] { sn, time, relay, door, lock_, alarm });
                item.ForeColor = Color.DarkBlue;
                lvDevices.Items.Add(item);
                _deviceItems[sn] = item;

                if (!cmbDevice.Items.Contains(sn))
                {
                    cmbDevice.Items.Add(sn);
                    if (cmbDevice.SelectedIndex < 0) cmbDevice.SelectedIndex = 0;
                }
            }
        }

        private void SafeInvoke(Action action)
        {
            if (InvokeRequired) Invoke(action);
            else                action();
        }

        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            _server.Stop();
        }
    }
}
