
using DoNetDrive.Protocol.Fingerprint.Test.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using HolidayDetail = DoNetDrive.Protocol.Door.Door8800.Holiday.HolidayDetail;
using DoNetDrive.Protocol.Door.Door8800.Holiday;

namespace DoNetDrive.Protocol.Fingerprint.Test
{
    public partial class frmHoliday : frmNodeForm
    {
        List<HolidayDetailDto> listHoliday = new List<HolidayDetailDto>();
        #region 单例模式

        private static object lockobj = new object();
        private static frmHoliday onlyObj;
        public static frmHoliday GetForm(INMain main)
        {
            if (onlyObj == null)
            {
                lock (lockobj)
                {
                    if (onlyObj == null)
                    {
                        onlyObj = new frmHoliday(main);
                        frmMain.AddNodeForms(onlyObj);
                    }
                }
            }
            return onlyObj;
        }
        #endregion

        private frmHoliday(INMain main)
        {
            InitializeComponent();
            mMainForm = main;
        }

        private void frmHoliday_Load(object sender, EventArgs e)
        {
            LoadUILanguage();
            cbIndex.Items.Clear();
            for (int i = 1; i < 31; i++)
            {
                cbIndex.Items.Add(i.ToString());
            }
            cbIndex.SelectedIndex = 0;
            cbType.SelectedIndex = 0;
            dataGridView1.AutoGenerateColumns = false;
            dataGridView1.DataSource = new BindingList<HolidayDetailDto>(listHoliday);


        }
        public override void LoadUILanguage()
        {
            base.LoadUILanguage();
            Lng(gpAllHoliday);
            Lng(butReadHolidayDetail);
            Lng(butReadAllHoliday);
            Lng(butAddHoliday);
            Lng(butClearHoliday);
            Lng(Lbl_HolidayList);
            Lng(checkBoxX1);
            Lng(Lbl_Index);
            Lng(Lbl_HolidayTime);
            Lng(cbYear);
            Lng(btnAddList);
            Lng(btnDelList);
            Lng(btnAddDecive);
            Lng(btnDelDevice);
            Lng(btnDelSelect);
            Lng(btnAdd30);
            Lng(dataGridView1);
            LoadComboxItemsLanguage(cbType, "cbType");
        }

        private void BindDataGrid()
        {

        }


        #region 从控制板中读取节假日存储详情
        private void butReadHolidayDetail_Click(object sender, EventArgs e)
        {
            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;
            ReadHolidayDetail cmd = new ReadHolidayDetail(cmdDtl);
            mMainForm.AddCommand(cmd);

            //处理返回值
            cmdDtl.CommandCompleteEvent += (sdr, cmde) =>
            {
                ReadHolidayDetail_Result result = cmde.Command.getResult() as ReadHolidayDetail_Result;
                var dtl = result.Detail;
                string log = Lng("Msg_8") + "\r\n" + string.Format(Lng("Msg_4"), dtl.Capacity, dtl.Count);
                mMainForm.AddCmdLog(cmde, log);
            };
        }
        #endregion

        #region 清空节假日
        private void ClearHoliday_Click(object sender, EventArgs e)
        {
            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;
            ClearHoliday cmd = new ClearHoliday(cmdDtl);
            mMainForm.AddCommand(cmd);
        }
        #endregion

        #region 读取控制板中所有的节假日
        private void ReadAllHoliday_Click(object sender, EventArgs e)
        {
            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;
            ReadAllHoliday cmd = new ReadAllHoliday(cmdDtl);
            mMainForm.AddCommand(cmd);

            //处理返回值
            cmdDtl.CommandCompleteEvent += (sdr, cmde) =>
            {
                ReadAllHoliday_Result result = cmde.Command.getResult() as ReadAllHoliday_Result;
                foreach (HolidayDetail item in result.Holidays)
                {
                    HolidayDetailDto dto = new HolidayDetailDto();

                    dto.HolidayTypeRender = ConvertHolidayType(item.HolidayType);
                    dto.RepeatYear = item.Holiday.Year == 2000 ? Lng("Msg_1") : Lng("Msg_2");
                    if (item.Holiday.Year == 2000)
                    {
                        dto.Holiday = new DateTime(DateTime.Now.Year, item.Holiday.Month, item.Holiday.Day);
                    }
                    else
                    {
                        dto.Holiday = item.Holiday;
                    }
                    dto.HolidayType = item.HolidayType;
                    dto.Index = item.Index;
                    dto.Selected = false;
                    listHoliday.Add(dto);
                    //superGridControl1.PrimaryGrid.Rows.Add(new GridRow(new object[] { item.Index, item.Holiday, ConvertHolidayType(item.HolidayType),item.Year }));
                }
                Invoke(() =>
                {
                    dataGridView1.DataSource = new BindingList<HolidayDetailDto>(listHoliday);

                });

                //dataGridView1
                string log = Lng("Msg_3") + result.Count;
                mMainForm.AddCmdLog(cmde, log);
            };


        }
        #endregion

        /// <summary>
        /// 节假日类型 转换
        /// </summary>
        /// <param name="b"></param>
        /// <returns></returns>
        private string ConvertHolidayType(byte b)
        {
            string result = "";
            switch (b)
            {
                case 1:
                    result = Lng("Msg_5");
                    break;
                case 2:
                    result = Lng("Msg_6");
                    break;
                case 3:
                    result = Lng("Msg_7");
                    break;
                default:
                    break;
            }
            return result;
        }

        /// <summary>
        /// 关闭窗口
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FrmHoliday_FormClosed(object sender, FormClosedEventArgs e)
        {
            onlyObj = null;
        }

        /// <summary>
        /// 增加序号 事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnAddIndex_Click(object sender, EventArgs e)
        {
            if (cbIndex.SelectedIndex != cbIndex.Items.Count - 1)
            {
                cbIndex.SelectedIndex++;
            }
            else
            {
                cbIndex.SelectedIndex = 0;
            }
        }

        /// <summary>
        /// 增加日期 事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnAddDay_Click(object sender, EventArgs e)
        {
            dtpDay.Value = dtpDay.Value.AddDays(1);
        }

        /// <summary>
        /// 增加至列表 事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnAddList_Click(object sender, EventArgs e)
        {
            byte bIndex = Convert.ToByte(cbIndex.SelectedIndex + 1);
            HolidayDetailDto holidayDto = listHoliday.FirstOrDefault(t => t.Index == bIndex);
            bool bExist = true;
            if (holidayDto == null)
            {
                holidayDto = new HolidayDetailDto() { };
                bExist = false;
            }
            holidayDto.Selected = false;
            holidayDto.Index = bIndex;
            holidayDto.HolidayType = (byte)(cbType.SelectedIndex + 1);
            holidayDto.Holiday = dtpDay.Value;
            holidayDto.HolidayTypeRender = ConvertHolidayType(Convert.ToByte(cbType.SelectedIndex + 1));
            holidayDto.RepeatYear = cbYear.Checked ? Lng("Msg_1") : Lng("Msg_2");
            if (!bExist)
            {
                listHoliday.Add(holidayDto);
            }
            dataGridView1.DataSource = new BindingList<HolidayDetailDto>(listHoliday);
        }

        /// <summary>
        /// 从列表删除 事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnDelList_Click(object sender, EventArgs e)
        {
            int count = dataGridView1.Rows.Count;
            for (int i = count - 1; i >= 0; i--)
            {
                DataGridViewCheckBoxCell checkCell = (DataGridViewCheckBoxCell)dataGridView1.Rows[i].Cells[0];
                bool bSelected = Convert.ToBoolean(checkCell.Value);
                if (bSelected)
                {
                    DataGridViewTextBoxCell text = (DataGridViewTextBoxCell)dataGridView1.Rows[i].Cells[1];
                    listHoliday.RemoveAt(listHoliday.FindIndex(t => t.Index == Convert.ToByte(text.Value)));
                }
            }
            dataGridView1.DataSource = new BindingList<HolidayDetailDto>(listHoliday);
        }

        /// <summary>
        /// 添加设备节假日
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnAddDecive_Click(object sender, EventArgs e)
        {
            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;
            HolidayDetail holiday = new HolidayDetail() { HolidayType = Convert.ToByte(cbType.SelectedIndex + 1), Index = Convert.ToByte(cbIndex.SelectedIndex + 1) };
            int year = dtpDay.Value.Year;
            if (cbYear.Checked)
            {
                year = 0;
            }
            holiday.Holiday = new DateTime(year, dtpDay.Value.Month, dtpDay.Value.Day);
            List<HolidayDetail> _list = new List<HolidayDetail>() { holiday };
            AddHoliday_Parameter par = new AddHoliday_Parameter(_list);
            AddHoliday cmd = new AddHoliday(cmdDtl, par);
            mMainForm.AddCommand(cmd);
            BtnAddList_Click(null, null);
        }

        /// <summary>
        /// 删除设备节假日
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnDelDevice_Click(object sender, EventArgs e)
        {
            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;
            List<HolidayDetail> _list = new List<HolidayDetail>();
            _list.Add(new HolidayDetail() { Index = Convert.ToByte(cbIndex.SelectedIndex + 1) });
            DeleteHoliday_Parameter par = new DeleteHoliday_Parameter(_list);
            DeleteHoliday cmd = new DeleteHoliday(cmdDtl, par);
            mMainForm.AddCommand(cmd);

        }

        private void BtnAdd30_Click(object sender, EventArgs e)
        {
            listHoliday.Clear();
            for (int i = 0; i < 30; i++)
            {
                HolidayDetailDto holiday = new HolidayDetailDto() { Index = Convert.ToByte(i + 1), Holiday = dtpDay.Value.AddDays(i + 1) };
                holiday.RepeatYear = cbYear.Checked ? Lng("Msg_1") : Lng("Msg_2");
                holiday.HolidayType = Convert.ToByte(cbType.SelectedIndex + 1);
                holiday.HolidayTypeRender = ConvertHolidayType(holiday.HolidayType);
                listHoliday.Add(holiday);
            }
            dtpDay.Value = dtpDay.Value.AddDays(30);
            dataGridView1.DataSource = new BindingList<HolidayDetailDto>(listHoliday);

            //dataGridView1.DataSource = listHoliday;
        }

        private void BtnDelSelect_Click(object sender, EventArgs e)
        {
            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;
            List<HolidayDetail> _list = new List<HolidayDetail>();
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                DataGridViewCheckBoxCell checkCell = (DataGridViewCheckBoxCell)dataGridView1.Rows[i].Cells[0];
                bool bSelected = Convert.ToBoolean(checkCell.Value);
                if (bSelected)
                {
                    DataGridViewTextBoxCell text = (DataGridViewTextBoxCell)dataGridView1.Rows[i].Cells[1];
                    byte bIndex = Convert.ToByte(text.Value);
                    _list.Add(new HolidayDetail() { Index = bIndex });
                    
                }
            }
            foreach (var item in _list)
            {
                listHoliday.RemoveAt(listHoliday.FindIndex(t => t.Index == item.Index));
            }
            dataGridView1.DataSource = new BindingList<HolidayDetailDto>(listHoliday);

            DeleteHoliday_Parameter par = new DeleteHoliday_Parameter(_list);
            DeleteHoliday cmd = new DeleteHoliday(cmdDtl, par);
            mMainForm.AddCommand(cmd);
        }


        /// <summary>
        /// 添加列表节假日
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButAddHoliday_Click(object sender, EventArgs e)
        {
            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;
            List<HolidayDetail> _list = new List<HolidayDetail>();
            for (int i = 0; i < listHoliday.Count; i++)
            {
                HolidayDetail holiday = new HolidayDetail() { Index = listHoliday[i].Index, HolidayType = (byte)(listHoliday[i].HolidayType) };
                holiday.Holiday = listHoliday[i].Holiday;
                if (cbYear.Checked)
                {
                    holiday.Holiday = new DateTime(2000, holiday.Holiday.Month, holiday.Holiday.Day);
                }

                _list.Add(holiday);
            }
            AddHoliday_Parameter par = new AddHoliday_Parameter(_list);
            AddHoliday cmd = new AddHoliday(cmdDtl, par);
            mMainForm.AddCommand(cmd);
        }

        private void DataGridView1_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex is -1) return;
            if (e.ColumnIndex == 0)
            {
                DataGridViewCheckBoxCell cell = (DataGridViewCheckBoxCell)dataGridView1.Rows[e.RowIndex].Cells[0];
                if ((bool)cell.FormattedValue)
                {
                    cell.Value = false;
                  //  cell.EditingCellFormattedValue = false;
                }
                else
                {
                    cell.Value = true;
                  //  cell.EditingCellFormattedValue = true;
                }
            }
        }

        private void CheckBoxX1_CheckedChanged(object sender, EventArgs e)
        {
            //checkBoxX1.Checked;
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                DataGridViewCheckBoxCell cell = (DataGridViewCheckBoxCell)dataGridView1.Rows[i].Cells[0];
                if ((bool)cell.FormattedValue)
                {
                    cell.Value = false;
                }
                else
                {
                    cell.Value = true;
                }
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex is -1) return;
            DataGridViewCheckBoxCell cell = (DataGridViewCheckBoxCell)dataGridView1.Rows[e.RowIndex].Cells[0];
            cell.Value = !Convert.ToBoolean(cell.Value);
        }
    }
}
