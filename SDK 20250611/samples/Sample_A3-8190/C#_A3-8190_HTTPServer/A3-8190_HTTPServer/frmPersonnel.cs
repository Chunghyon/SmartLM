using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using A3_8190_HTTPServer.HttpServer;
using A3_8190_HTTPServer.Models;

namespace A3_8190_HTTPServer
{
    public partial class frmPersonnel : Form
    {
        private readonly A3HttpListener _server;

        public frmPersonnel(A3HttpListener server)
        {
            InitializeComponent();
            _server = server;
            RefreshList();
        }

        // ─── 목록 새로 고침 ───────────────────────────────────────────────────
        private void RefreshList()
        {
            lvPersonnel.Items.Clear();
            lock (_server.PersonnelList)
            {
                foreach (var p in _server.PersonnelList)
                {
                    string access = p.AccessType == 1 ? "관리자" : p.AccessType == 2 ? "블랙리스트" : "일반";
                    var item = new ListViewItem(new[] {
                        p.UserID ?? "", p.Name ?? "", p.Department ?? "",
                        p.Job ?? "", p.CardNum ?? "", access
                    });
                    item.Tag = p;
                    lvPersonnel.Items.Add(item);
                }
            }
            lblCount.Text = $"총 {lvPersonnel.Items.Count}명";
        }

        // ─── 인원 추가 ────────────────────────────────────────────────────────
        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtUserID.Text))
            {
                MessageBox.Show("사용자 ID를 입력하세요.", "입력 오류",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtUserID.Focus();
                return;
            }

            string uid = txtUserID.Text.Trim();

            lock (_server.PersonnelList)
            {
                if (_server.PersonnelList.Find(p => p.UserID == uid) != null)
                {
                    MessageBox.Show($"UserID '{uid}' 이(가) 이미 목록에 존재합니다.",
                                    "중복 오류", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var person = new PersonInfo
                {
                    UserID      = uid,
                    Name        = txtName.Text.Trim(),
                    Department  = txtDept.Text.Trim(),
                    Job         = txtJob.Text.Trim(),
                    CardNum     = txtCard.Text.Trim(),
                    Password    = txtPassword.Text.Trim(),
                    AccessType  = rdoAdmin.Checked ? 1 : rdoBlacklist.Checked ? 2 : 0,
                    OpenTimes   = 65535  // 무제한
                };
                _server.PersonnelList.Add(person);
            }

            RefreshList();
            ClearInputs();
        }

        // ─── 인원 삭제 (로컬 목록) ────────────────────────────────────────────
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (lvPersonnel.SelectedItems.Count == 0)
            {
                MessageBox.Show("삭제할 인원을 선택하세요.", "알림");
                return;
            }

            var item   = lvPersonnel.SelectedItems[0];
            var person = item.Tag as PersonInfo;
            if (person == null) return;

            if (MessageBox.Show($"'{person.Name}' (ID: {person.UserID}) 를 목록에서 삭제합니까?\n\n" +
                                "삭제 후 '디바이스 삭제 전송' 버튼을 눌러 디바이스에도 반영하세요.",
                                "삭제 확인", MessageBoxButtons.YesNo,
                                MessageBoxIcon.Question) != DialogResult.Yes) return;

            lock (_server.PersonnelList) { _server.PersonnelList.Remove(person); }
            lock (_server.DeleteList)    { _server.DeleteList.Add(person.UserID); }

            RefreshList();
        }

        // ─── 디바이스에 인원 추가 전송 ────────────────────────────────────────
        private void btnPushAdd_Click(object sender, EventArgs e)
        {
            string sn = txtPushSN.Text.Trim();
            if (string.IsNullOrEmpty(sn))
            {
                MessageBox.Show("디바이스 SN을 입력하세요.", "입력 오류",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPushSN.Focus(); return;
            }

            int count;
            lock (_server.PersonnelList) { count = _server.PersonnelList.Count; }
            if (count == 0)
            {
                MessageBox.Show("전송할 인원이 없습니다.", "알림"); return;
            }

            _server.QueueAddPeople(sn, count);
            MessageBox.Show($"[{sn}] 에 {count}명 추가 전송이 예약되었습니다.\n" +
                            "다음 Keepalive 수신 시 디바이스가 인원 목록을 요청합니다.",
                            "전송 예약", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        // ─── 디바이스에 인원 삭제 전송 ────────────────────────────────────────
        private void btnPushDelete_Click(object sender, EventArgs e)
        {
            string sn = txtPushSN.Text.Trim();
            if (string.IsNullOrEmpty(sn))
            {
                MessageBox.Show("디바이스 SN을 입력하세요.", "입력 오류",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPushSN.Focus(); return;
            }

            int delCount;
            lock (_server.DeleteList) { delCount = _server.DeleteList.Count; }
            if (delCount == 0)
            {
                MessageBox.Show("삭제 대기 목록이 비어있습니다.\n먼저 목록에서 인원을 삭제하세요.", "알림");
                return;
            }

            _server.QueueDeletePeople(sn);
            MessageBox.Show($"[{sn}] 에 삭제 전송이 예약되었습니다 ({delCount}건).\n" +
                            "다음 Keepalive 수신 시 디바이스가 삭제 목록을 요청합니다.",
                            "삭제 예약", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        // ─── Helpers ─────────────────────────────────────────────────────────
        private void ClearInputs()
        {
            txtUserID.Clear(); txtName.Clear(); txtDept.Clear();
            txtJob.Clear();    txtCard.Clear(); txtPassword.Clear();
            rdoNormal.Checked = true;
        }

        private void btnClearAll_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("전체 인원 목록을 초기화하시겠습니까?",
                                "초기화 확인", MessageBoxButtons.YesNo,
                                MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                lock (_server.PersonnelList) { _server.PersonnelList.Clear(); }
                RefreshList();
            }
        }
    }
}
