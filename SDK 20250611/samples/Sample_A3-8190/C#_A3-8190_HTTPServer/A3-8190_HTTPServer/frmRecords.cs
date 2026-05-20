using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using A3_8190_HTTPServer.Models;

namespace A3_8190_HTTPServer
{
    public partial class frmRecords : Form
    {
        private readonly List<(string SN, RecordDetail Record)> _allRecords;

        public frmRecords(List<(string SN, RecordDetail Record)> records)
        {
            InitializeComponent();
            _allRecords = records;
            RefreshList();
        }

        // ─── 외부에서 기록 추가 (이벤트 스레드에서 호출됨) ───────────────────
        public void AddRecord(string sn, RecordDetail rec)
        {
            if (IsDisposed) return;
            if (lvRecords.InvokeRequired)
                lvRecords.BeginInvoke(new Action(() => AddToListView(sn, rec)));
            else
                AddToListView(sn, rec);
        }

        private void AddToListView(string sn, RecordDetail rec)
        {
            string tempText  = rec.BodyTemp > 0 ? $"{rec.BodyTemp / 10.0:F1}°C" : "-";
            string entryText = rec.IsEntry == 1 ? "입실" : "퇴실";
            string typeText  = RecordDetail.GetRecordTypeText(rec.RecordType);
            string timeText  = rec.GetDateTime().ToString("yyyy-MM-dd HH:mm:ss");

            var item = new ListViewItem(new[] {
                rec.RecordID.ToString(), sn, rec.UserID ?? "",
                rec.Name ?? "", rec.Department ?? "",
                typeText, entryText, timeText, tempText,
                rec.PhotoLen > 0 ? $"있음({rec.PhotoLen}B)" : "없음"
            });
            item.Tag = rec;

            // 색상 구분
            if (rec.RecordType == 19 || rec.RecordType == 24 || rec.RecordType == 22)
                item.ForeColor = Color.Red;          // 경고 (미등록, 블랙리스트)
            else if (rec.IsEntry == 0)
                item.ForeColor = Color.DarkSlateBlue; // 퇴실

            lvRecords.Items.Insert(0, item);  // 최신 기록이 위로
            lblCount.Text = $"총 {lvRecords.Items.Count}건";
        }

        private void RefreshList()
        {
            lvRecords.Items.Clear();
            lock (_allRecords)
            {
                // 최신 순으로 역순 삽입
                for (int i = _allRecords.Count - 1; i >= 0; i--)
                    AddToListView(_allRecords[i].SN, _allRecords[i].Record);
            }
        }

        // ─── 기록 지우기 ──────────────────────────────────────────────────────
        private void btnClear_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("화면 표시 기록을 모두 지우시겠습니까?\n(서버 내부 목록도 함께 삭제됩니다)",
                                "확인", MessageBoxButtons.YesNo,
                                MessageBoxIcon.Warning) != DialogResult.Yes) return;

            lock (_allRecords) { _allRecords.Clear(); }
            lvRecords.Items.Clear();
            lblCount.Text = "총 0건";
        }

        // ─── CSV 내보내기 ─────────────────────────────────────────────────────
        private void btnExport_Click(object sender, EventArgs e)
        {
            using (var dlg = new SaveFileDialog())
            {
                dlg.Filter   = "CSV 파일 (*.csv)|*.csv";
                dlg.FileName = $"A3-8190_Records_{DateTime.Now:yyyyMMdd_HHmmss}.csv";
                if (dlg.ShowDialog() != DialogResult.OK) return;

                using (var sw = new StreamWriter(dlg.FileName, false, new UTF8Encoding(true)))
                {
                    // BOM 포함 UTF-8 (엑셀 한글 호환)
                    sw.WriteLine("기록ID,장치SN,사용자ID,이름,부서,인증방법,입출,시간,체온,이미지");
                    foreach (ListViewItem item in lvRecords.Items)
                    {
                        var cols = new string[item.SubItems.Count];
                        for (int i = 0; i < item.SubItems.Count; i++)
                            cols[i] = $"\"{item.SubItems[i].Text.Replace("\"", "\"\"")}\"";
                        sw.WriteLine(string.Join(",", cols));
                    }
                }
                MessageBox.Show($"저장 완료:\n{dlg.FileName}",
                                "내보내기 완료", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}
