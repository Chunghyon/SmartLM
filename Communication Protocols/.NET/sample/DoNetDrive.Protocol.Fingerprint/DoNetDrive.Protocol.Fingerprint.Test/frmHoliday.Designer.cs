namespace DoNetDrive.Protocol.Fingerprint.Test
{
    partial class frmHoliday
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.butReadHolidayDetail = new System.Windows.Forms.Button();
            this.butReadAllHoliday = new System.Windows.Forms.Button();
            this.butClearHoliday = new System.Windows.Forms.Button();
            this.gpAllHoliday = new System.Windows.Forms.GroupBox();
            this.Lbl_HolidayList = new System.Windows.Forms.Label();
            this.checkBoxX1 = new System.Windows.Forms.CheckBox();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.Lbl_Index = new System.Windows.Forms.Label();
            this.cbIndex = new System.Windows.Forms.ComboBox();
            this.btnAddIndex = new System.Windows.Forms.Button();
            this.Lbl_HolidayTime = new System.Windows.Forms.Label();
            this.dtpDay = new System.Windows.Forms.DateTimePicker();
            this.btnAddDay = new System.Windows.Forms.Button();
            this.cbType = new System.Windows.Forms.ComboBox();
            this.cbYear = new System.Windows.Forms.CheckBox();
            this.btnAddList = new System.Windows.Forms.Button();
            this.btnDelList = new System.Windows.Forms.Button();
            this.btnAddDecive = new System.Windows.Forms.Button();
            this.btnDelDevice = new System.Windows.Forms.Button();
            this.btnDelSelect = new System.Windows.Forms.Button();
            this.btnAdd30 = new System.Windows.Forms.Button();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.Selected = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.Index = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Holiday = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.HolidayTypeRender = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.RepeatYear = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.butAddHoliday = new System.Windows.Forms.Button();
            this.gpAllHoliday.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // butReadHolidayDetail
            // 
            this.butReadHolidayDetail.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.butReadHolidayDetail.Location = new System.Drawing.Point(7, 20);
            this.butReadHolidayDetail.Name = "butReadHolidayDetail";
            this.butReadHolidayDetail.Size = new System.Drawing.Size(121, 26);
            this.butReadHolidayDetail.TabIndex = 1;
            this.butReadHolidayDetail.Text = "读取节假日存储详情";
            this.butReadHolidayDetail.Click += new System.EventHandler(this.butReadHolidayDetail_Click);
            // 
            // butReadAllHoliday
            // 
            this.butReadAllHoliday.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.butReadAllHoliday.Location = new System.Drawing.Point(143, 20);
            this.butReadAllHoliday.Name = "butReadAllHoliday";
            this.butReadAllHoliday.Size = new System.Drawing.Size(142, 26);
            this.butReadAllHoliday.TabIndex = 2;
            this.butReadAllHoliday.Text = "从控制板读取所有节假日";
            this.butReadAllHoliday.Click += new System.EventHandler(this.ReadAllHoliday_Click);
            // 
            // butClearHoliday
            // 
            this.butClearHoliday.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.butClearHoliday.Location = new System.Drawing.Point(424, 20);
            this.butClearHoliday.Name = "butClearHoliday";
            this.butClearHoliday.Size = new System.Drawing.Size(109, 26);
            this.butClearHoliday.TabIndex = 3;
            this.butClearHoliday.Text = "删除所有节假日";
            this.butClearHoliday.Click += new System.EventHandler(this.ClearHoliday_Click);
            // 
            // gpAllHoliday
            // 
            this.gpAllHoliday.Controls.Add(this.butAddHoliday);
            this.gpAllHoliday.Controls.Add(this.butReadHolidayDetail);
            this.gpAllHoliday.Controls.Add(this.butClearHoliday);
            this.gpAllHoliday.Controls.Add(this.butReadAllHoliday);
            this.gpAllHoliday.Location = new System.Drawing.Point(12, 12);
            this.gpAllHoliday.Name = "gpAllHoliday";
            this.gpAllHoliday.Size = new System.Drawing.Size(558, 57);
            this.gpAllHoliday.TabIndex = 4;
            this.gpAllHoliday.TabStop = false;
            this.gpAllHoliday.Text = "控制板中的节假日";
            // 
            // Lbl_HolidayList
            // 
            this.Lbl_HolidayList.Location = new System.Drawing.Point(12, 73);
            this.Lbl_HolidayList.Name = "Lbl_HolidayList";
            this.Lbl_HolidayList.Size = new System.Drawing.Size(90, 23);
            this.Lbl_HolidayList.TabIndex = 5;
            this.Lbl_HolidayList.Text = "节假日列表：";
            // 
            // checkBoxX1
            // 
            this.checkBoxX1.Location = new System.Drawing.Point(13, 478);
            this.checkBoxX1.Name = "checkBoxX1";
            this.checkBoxX1.Size = new System.Drawing.Size(65, 23);
            this.checkBoxX1.TabIndex = 6;
            this.checkBoxX1.Text = "反选";
            this.checkBoxX1.CheckedChanged += new System.EventHandler(this.CheckBoxX1_CheckedChanged);
            // 
            // Lbl_Index
            // 
            this.Lbl_Index.Location = new System.Drawing.Point(103, 478);
            this.Lbl_Index.Name = "Lbl_Index";
            this.Lbl_Index.Size = new System.Drawing.Size(55, 23);
            this.Lbl_Index.TabIndex = 7;
            this.Lbl_Index.Text = "序号：";
            // 
            // cbIndex
            // 
            this.cbIndex.FormattingEnabled = true;
            this.cbIndex.Location = new System.Drawing.Point(103, 501);
            this.cbIndex.Name = "cbIndex";
            this.cbIndex.Size = new System.Drawing.Size(36, 20);
            this.cbIndex.TabIndex = 8;
            // 
            // btnAddIndex
            // 
            this.btnAddIndex.Location = new System.Drawing.Point(139, 500);
            this.btnAddIndex.Name = "btnAddIndex";
            this.btnAddIndex.Size = new System.Drawing.Size(19, 22);
            this.btnAddIndex.TabIndex = 9;
            this.btnAddIndex.Text = "+";
            this.btnAddIndex.UseVisualStyleBackColor = true;
            this.btnAddIndex.Click += new System.EventHandler(this.BtnAddIndex_Click);
            // 
            // Lbl_HolidayTime
            // 
            this.Lbl_HolidayTime.AutoSize = true;
            this.Lbl_HolidayTime.Location = new System.Drawing.Point(246, 484);
            this.Lbl_HolidayTime.Name = "Lbl_HolidayTime";
            this.Lbl_HolidayTime.Size = new System.Drawing.Size(77, 12);
            this.Lbl_HolidayTime.TabIndex = 10;
            this.Lbl_HolidayTime.Text = "节假日时间：";
            // 
            // dtpDay
            // 
            this.dtpDay.Location = new System.Drawing.Point(243, 500);
            this.dtpDay.Name = "dtpDay";
            this.dtpDay.Size = new System.Drawing.Size(105, 21);
            this.dtpDay.TabIndex = 11;
            // 
            // btnAddDay
            // 
            this.btnAddDay.Location = new System.Drawing.Point(348, 499);
            this.btnAddDay.Name = "btnAddDay";
            this.btnAddDay.Size = new System.Drawing.Size(19, 23);
            this.btnAddDay.TabIndex = 12;
            this.btnAddDay.Text = "+";
            this.btnAddDay.UseVisualStyleBackColor = true;
            this.btnAddDay.Click += new System.EventHandler(this.BtnAddDay_Click);
            // 
            // cbType
            // 
            this.cbType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbType.FormattingEnabled = true;
            this.cbType.Location = new System.Drawing.Point(410, 501);
            this.cbType.Name = "cbType";
            this.cbType.Size = new System.Drawing.Size(147, 20);
            this.cbType.TabIndex = 13;
            // 
            // cbYear
            // 
            this.cbYear.AutoSize = true;
            this.cbYear.Location = new System.Drawing.Point(410, 478);
            this.cbYear.Name = "cbYear";
            this.cbYear.Size = new System.Drawing.Size(72, 16);
            this.cbYear.TabIndex = 14;
            this.cbYear.Text = "每年循环";
            this.cbYear.UseVisualStyleBackColor = true;
            // 
            // btnAddList
            // 
            this.btnAddList.Location = new System.Drawing.Point(2, 532);
            this.btnAddList.Name = "btnAddList";
            this.btnAddList.Size = new System.Drawing.Size(187, 33);
            this.btnAddList.TabIndex = 15;
            this.btnAddList.Text = "增加至列表";
            this.btnAddList.UseVisualStyleBackColor = true;
            this.btnAddList.Click += new System.EventHandler(this.BtnAddList_Click);
            // 
            // btnDelList
            // 
            this.btnDelList.Location = new System.Drawing.Point(3, 581);
            this.btnDelList.Name = "btnDelList";
            this.btnDelList.Size = new System.Drawing.Size(187, 33);
            this.btnDelList.TabIndex = 16;
            this.btnDelList.Text = "从列表删除";
            this.btnDelList.UseVisualStyleBackColor = true;
            this.btnDelList.Click += new System.EventHandler(this.BtnDelList_Click);
            // 
            // btnAddDecive
            // 
            this.btnAddDecive.Location = new System.Drawing.Point(198, 532);
            this.btnAddDecive.Name = "btnAddDecive";
            this.btnAddDecive.Size = new System.Drawing.Size(187, 33);
            this.btnAddDecive.TabIndex = 17;
            this.btnAddDecive.Text = "增加至设备";
            this.btnAddDecive.UseVisualStyleBackColor = true;
            this.btnAddDecive.Click += new System.EventHandler(this.BtnAddDecive_Click);
            // 
            // btnDelDevice
            // 
            this.btnDelDevice.Location = new System.Drawing.Point(394, 532);
            this.btnDelDevice.Name = "btnDelDevice";
            this.btnDelDevice.Size = new System.Drawing.Size(187, 33);
            this.btnDelDevice.TabIndex = 18;
            this.btnDelDevice.Text = "从设备删除";
            this.btnDelDevice.UseVisualStyleBackColor = true;
            this.btnDelDevice.Click += new System.EventHandler(this.BtnDelDevice_Click);
            // 
            // btnDelSelect
            // 
            this.btnDelSelect.Location = new System.Drawing.Point(198, 581);
            this.btnDelSelect.Name = "btnDelSelect";
            this.btnDelSelect.Size = new System.Drawing.Size(187, 33);
            this.btnDelSelect.TabIndex = 19;
            this.btnDelSelect.Text = "从设备删除删除选中节假日";
            this.btnDelSelect.UseVisualStyleBackColor = true;
            this.btnDelSelect.Click += new System.EventHandler(this.BtnDelSelect_Click);
            // 
            // btnAdd30
            // 
            this.btnAdd30.Location = new System.Drawing.Point(393, 581);
            this.btnAdd30.Name = "btnAdd30";
            this.btnAdd30.Size = new System.Drawing.Size(187, 33);
            this.btnAdd30.TabIndex = 20;
            this.btnAdd30.Text = "生成30个节假日列表";
            this.btnAdd30.UseVisualStyleBackColor = true;
            this.btnAdd30.Click += new System.EventHandler(this.BtnAdd30_Click);
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Selected,
            this.Index,
            this.Holiday,
            this.HolidayTypeRender,
            this.RepeatYear});
            this.dataGridView1.Location = new System.Drawing.Point(13, 102);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.Size = new System.Drawing.Size(557, 361);
            this.dataGridView1.TabIndex = 21;
            this.dataGridView1.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellClick);
            // 
            // Selected
            // 
            this.Selected.DataPropertyName = "Selected";
            this.Selected.HeaderText = "选择";
            this.Selected.Name = "Selected";
            this.Selected.Width = 50;
            // 
            // Index
            // 
            this.Index.DataPropertyName = "Index";
            this.Index.HeaderText = "序号";
            this.Index.Name = "Index";
            this.Index.Width = 60;
            // 
            // Holiday
            // 
            this.Holiday.DataPropertyName = "Holiday";
            this.Holiday.HeaderText = "时间";
            this.Holiday.Name = "Holiday";
            this.Holiday.Width = 110;
            // 
            // HolidayTypeRender
            // 
            this.HolidayTypeRender.DataPropertyName = "HolidayTypeRender";
            this.HolidayTypeRender.HeaderText = "长度";
            this.HolidayTypeRender.Name = "HolidayTypeRender";
            this.HolidayTypeRender.Width = 180;
            // 
            // RepeatYear
            // 
            this.RepeatYear.DataPropertyName = "RepeatYear";
            this.RepeatYear.HeaderText = "每年循环";
            this.RepeatYear.Name = "RepeatYear";
            // 
            // butAddHoliday
            // 
            this.butAddHoliday.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.butAddHoliday.Location = new System.Drawing.Point(300, 20);
            this.butAddHoliday.Name = "butAddHoliday";
            this.butAddHoliday.Size = new System.Drawing.Size(109, 26);
            this.butAddHoliday.TabIndex = 22;
            this.butAddHoliday.Text = "添加列表节假日";
            this.butAddHoliday.Click += new System.EventHandler(this.ButAddHoliday_Click);
            // 
            // frmHoliday
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(582, 621);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.btnAdd30);
            this.Controls.Add(this.btnDelSelect);
            this.Controls.Add(this.btnDelDevice);
            this.Controls.Add(this.btnAddDecive);
            this.Controls.Add(this.btnDelList);
            this.Controls.Add(this.btnAddList);
            this.Controls.Add(this.cbYear);
            this.Controls.Add(this.cbType);
            this.Controls.Add(this.btnAddDay);
            this.Controls.Add(this.dtpDay);
            this.Controls.Add(this.Lbl_HolidayTime);
            this.Controls.Add(this.btnAddIndex);
            this.Controls.Add(this.cbIndex);
            this.Controls.Add(this.Lbl_Index);
            this.Controls.Add(this.checkBoxX1);
            this.Controls.Add(this.Lbl_HolidayList);
            this.Controls.Add(this.gpAllHoliday);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MinimizeBox = false;
            this.Name = "frmHoliday";
            this.Text = "frmHoliday";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FrmHoliday_FormClosed);
            this.Load += new System.EventHandler(this.frmHoliday_Load);
            this.gpAllHoliday.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button butReadHolidayDetail;
        private System.Windows.Forms.Button butReadAllHoliday;
        private System.Windows.Forms.Button butClearHoliday;
        private System.Windows.Forms.GroupBox gpAllHoliday;
        private System.Windows.Forms.Label Lbl_HolidayList;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.Label Lbl_Index;
        private System.Windows.Forms.ComboBox cbIndex;
        private System.Windows.Forms.Button btnAddIndex;
        private System.Windows.Forms.Label Lbl_HolidayTime;
        private System.Windows.Forms.DateTimePicker dtpDay;
        private System.Windows.Forms.Button btnAddDay;
        private System.Windows.Forms.ComboBox cbType;
        private System.Windows.Forms.CheckBox cbYear;
        private System.Windows.Forms.Button btnAddList;
        private System.Windows.Forms.Button btnDelList;
        private System.Windows.Forms.Button btnAddDecive;
        private System.Windows.Forms.Button btnDelDevice;
        private System.Windows.Forms.Button btnDelSelect;
        private System.Windows.Forms.Button btnAdd30;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Button butAddHoliday;
        private System.Windows.Forms.CheckBox checkBoxX1;
        private System.Windows.Forms.DataGridViewCheckBoxColumn Selected;
        private System.Windows.Forms.DataGridViewTextBoxColumn Index;
        private System.Windows.Forms.DataGridViewTextBoxColumn Holiday;
        private System.Windows.Forms.DataGridViewTextBoxColumn HolidayTypeRender;
        private System.Windows.Forms.DataGridViewTextBoxColumn RepeatYear;
    }
}