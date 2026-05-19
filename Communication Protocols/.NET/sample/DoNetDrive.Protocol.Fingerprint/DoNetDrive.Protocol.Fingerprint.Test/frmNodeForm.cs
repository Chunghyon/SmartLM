using DoNetDrive.Protocol.Door.Door8800.Data.TimeGroup;
using DoNetDrive.Common.Extensions;
using System;
using System.Windows.Forms;

namespace DoNetDrive.Protocol.Fingerprint.Test
{
    public partial class frmNodeForm : Form
    {

        /// <summary>
        /// 主窗体访问接口
        /// </summary>
        protected INMain mMainForm;

        /// <summary>
        /// 在UI线程上执行一个方法
        /// </summary>
        /// <param name="p"></param>
        protected void Invoke(Action p)
        {
            Invoke((Delegate)p);
        }

        /// <summary>
        /// 线程休眠
        /// </summary>
        /// <param name="time"></param>
        private void Sleep(int time)
        {
            System.Threading.Thread.Sleep(time);
        }

        #region 提示框
        public void MsgTip(string sText)
        {
            MessageBox.Show(sText, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        public void MsgErr(string sText)
        {
            MessageBox.Show(sText, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        #endregion


        public frmNodeForm()
        {
            InitializeComponent();
        }

        public frmNodeForm(INMain main)
        {
            InitializeComponent();
            mMainForm = main;
        }

        private void frmNodeForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            Hide();
        }

        private void frmNodeForm_Load(object sender, EventArgs e)
        {

        }

        public Control FindControl(Control parentControl, string findCtrlName)
        {
            Control _findedControl = null;
            if (!string.IsNullOrEmpty(findCtrlName) && parentControl != null)
            {
                foreach (Control ctrl in parentControl.Controls)
                {
                    if (ctrl.Name.Equals(findCtrlName))
                    {
                        _findedControl = ctrl;
                        break;
                    }
                }
            }
            return _findedControl;
        }

        public void SetAllTimePicker(Control parent,string beginControlName,string endControlName, DayTimeGroup group)
        {
            for (int i = 0; i < 8; i++)
            {
                DateTimePicker beginTimePicker = FindControl(parent, beginControlName + (i + 1).ToString()) as DateTimePicker;
                DateTimePicker endTimePicker = FindControl(parent, endControlName + (i + 1).ToString()) as DateTimePicker;
                if (group != null)
                {
                    TimeSegment segment = group.GetItem(i);
                    Invoke(() => {
                        beginTimePicker.Value = segment.GetBeginTime();
                        endTimePicker.Value = segment.GetEndTime();
                    });
                }
                

            }

        }

        public void SetWeekTimeGroupValue(WeekTimeGroup tg,int weekDayIndex, int timeSegmentIndex, int type, DateTime dateTime)
        {
            if (weekDayIndex == -1)
            {
                weekDayIndex = 0;
            }
            TimeSegment ts = tg.GetItem(weekDayIndex).GetItem(timeSegmentIndex);
            if (type == 1)
            {
                ts.SetBeginTime(dateTime.Hour, dateTime.Minute);
            }
            else
            {
                ts.SetEndTime(dateTime.Hour, dateTime.Minute);
            }
        }



        #region 多语言

        protected string Lng(string sKey)
        {
            return ToolLanguage.GetLanguage(Name, sKey);
        }
        protected string Lng(string sKey, params object[] args)
        {
            string str = ToolLanguage.GetLanguage(Name, sKey);
            return string.Format(str, args);
        }


        protected void Lng(ToolStripItem ctr)
        {
            ctr.Text = ToolLanguage.GetLanguage(Name, ctr.Name);
        }
        protected void Lng(Control ctr)
        {
            ctr.Text = ToolLanguage.GetLanguage(Name, ctr.Name);
        }
        protected void Lng(DataGridView dg)
        {
            string sCols = Lng($"{dg.Name}_Cols");
            var sArr = sCols.SplitTrim(",");
            var cols = dg.Columns;
            for (int i = 0; i < cols.Count; i++)
            {
                var col = cols[i];
                col.HeaderText = sArr[i];
            }

        }


        protected void LoadComboxItemsLanguage(ComboBox cbx, string sKey)
        {
            int iOldSelectIndex = cbx.SelectedIndex;
            cbx.Items.Clear();
            string sItems = Lng(sKey);
            if (string.IsNullOrEmpty(sItems))
                return;

            string[] sArr = sItems.SplitTrim(",");
            cbx.Items.AddRange(sArr);
            if (cbx.Items.Count > iOldSelectIndex)
            {
                cbx.SelectedIndex = iOldSelectIndex;
            }
            else
            {
                cbx.SelectedIndex = 0;
            }
        }

        public virtual void LoadUILanguage()
        {
            //指纹机、人脸机 调试程序
            Text = Lng("FormCaption");
           
        }
        #endregion


    }
}
