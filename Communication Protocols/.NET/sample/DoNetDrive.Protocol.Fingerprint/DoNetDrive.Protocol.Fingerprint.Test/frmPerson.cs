using DoNetDrive.Core.Command;
using DoNetDrive.Protocol.Fingerprint.Test.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DoNetDrive.Common.Extensions;
using DoNetDrive.Protocol.Fingerprint.Person;
using System.IO;

namespace DoNetDrive.Protocol.Fingerprint.Test
{
    public partial class frmPerson : frmNodeForm
    {
        #region 单例模式

        private static object lockobj = new object();
        private static frmPerson onlyObj;
        public static frmPerson GetForm(INMain main)
        {
            if (onlyObj == null)
            {
                lock (lockobj)
                {
                    if (onlyObj == null)
                    {
                        onlyObj = new frmPerson(main);
                        frmMain.AddNodeForms(onlyObj);
                    }
                }
            }
            return onlyObj;
        }
        #endregion

        BindingList<Person_UI> PersonList = null;

        /// <summary>
        /// 卡号字典
        /// </summary>
        HashSet<uint> CardHashTable = null;

        string mPersonImagePath;
        private frmPerson(INMain main)
        {
            InitializeComponent();
            mMainForm = main;
        }
        private void InitControl()
        {
            TimeGroup();
            // CardStatus();
            // EnterStatus();
            // Identity();
            OpenTimes();
        }

        public void OpenTimes()
        {
            cmbOpenTimes.Items.Clear();
            cmbOpenTimes.Items.Add(Lng("cmbOpenTimes0"));
            string str = Lng("cmbOpenTimes");
            string[] time = new string[300];
            for (int i = 1; i <= 300; i++)
            {
                time[i - 1] = i + str;
            }
            cmbOpenTimes.Items.AddRange(time);
            cmbOpenTimes.Items.Add(Lng("cmbOpenTimes65535"));
            cmbOpenTimes.SelectedIndex = cmbOpenTimes.Items.Count - 1;
        }


        public void TimeGroup()
        {
            string[] time = new string[64];
            string str = Lng("cmbTimeGroup");
            for (int i = 0; i < 64; i++)
            {
                time[i] = str + (i + 1);
            }
            cmbTimeGroup.Items.Clear();
            cmbTimeGroup.Items.AddRange(time);
            cmbTimeGroup.SelectedIndex = 0;
        }


        private void FrmPerson_Load(object sender, EventArgs e)
        {

            dgPersonList.AutoGenerateColumns = false;
            PersonList = new BindingList<Person_UI>();
            CardHashTable = new HashSet<uint>();
            dgPersonList.DataSource = PersonList;
            LoadUILanguage();
            // InitControl();
        }

        public override void LoadUILanguage()
        {
            base.LoadUILanguage();
            Lng(Lbl_UserList);
            Lng(chkSelectAll);
            Lng(btnReadDatabaseDetail);
            Lng(btnReadAllPerson);
            Lng(btnClearDataBase);
            Lng(btnWriteAllPerson);
            Lng(btnClearList);
            Lng(tabPage1);
            Lng(gpUserIdentityDetail);
            Lng(Lbl_UserCode);
            Lng(Lbl_Password);
            Lng(Lbl_CardData);
            Lng(Lbl_ValidityTime);
            Lng(Lbl_CardDataHex);
            Lng(Lbl_OpenTimes);
            Lng(Lbl_CardStatus);
            Lng(Lbl_TimeGroup);
            Lng(Lbl_Identity);
            Lng(Lbl_CardType);
            Lng(Lbl_EnterStatus);
            Lng(Lbl_PName);
            Lng(Lbl_PCode);
            Lng(Lbl_Dept);
            Lng(Lbl_Job);
            Lng(Lbl_Holiday);
            Lng(tabPage2);
            Lng(Lbl_Count);
            Lng(butCreateCardNumByRandom);
            Lng(butCreateCardNumByOrder);
            Lng(tabPage3);
            Lng(Lbl_RegUserCode);
            Lng(Lbl_RegUserName);
            Lng(button2);
            Lng(tabPage4);
            Lng(Lbl_UploadCode);
            Lng(Lbl_UploadName);
            Lng(butSelectImage);
            Lng(btnAddPesonAndImage);
            Lng(btnAddList);
            Lng(btnDelList);
            Lng(btnAddDevice);
            Lng(btnDelDevice);
            Lng(btnDelSelect);
            Lng(dgPersonList);
            Lng(button1);
            Lng(btnCheckUserCode);
            Lng(button3);
            Lng(button4);
            LoadComboxItemsLanguage(cmbCardStatus, "CardStatusList");
            cmbCardStatus.SelectedIndex = 0;
            LoadComboxItemsLanguage(cmbEnterStatus, "EnterStatusList");
            cmbEnterStatus.SelectedIndex = 0;
            LoadComboxItemsLanguage(cmbIdentity, "IdentityList");
            cmbIdentity.SelectedIndex = 0;
            LoadComboxItemsLanguage(cmbCardType, "CardTypeList");
            cmbCardType.SelectedIndex = 0;
            InitControl();
        }

        private void BtnReadDatabaseDetail_Click(object sender, EventArgs e)
        {
            var cmdDtl = mMainForm.GetCommandDetail();
            var cmd = new ReadPersonDatabaseDetail(cmdDtl);
            mMainForm.AddCommand(cmd);
            cmdDtl.CommandCompleteEvent += (sdr, cmde) =>
            {
                ReadPersonDatabaseDetail_Result result = cmde.Command.getResult() as ReadPersonDatabaseDetail_Result;
                StringBuilder builder = new StringBuilder();
                builder.AppendLine(Lng("btnReadDatabaseDetail"));
                builder.AppendLine(Lng("Msg_1") + result.SortDataBaseSize);
                builder.AppendLine(Lng("Msg_2") + result.SortPersonSize);
                builder.AppendLine(Lng("Msg_3") + result.SortFingerprintDataBaseSize);
                builder.AppendLine(Lng("Msg_4") + result.SortFingerprintSize);
                builder.AppendLine(Lng("Msg_5") + result.SortFaceDataBaseSize);
                builder.AppendLine(Lng("Msg_6") + result.SortFaceSize);
                builder.AppendLine(Lng("PalmStoreMax") + result.PalmDataBaseSize);
                builder.AppendLine(Lng("PalmStoreCount") + result.PalmSize);
                mMainForm.AddCmdLog(cmde, builder.ToString());
            };
        }

        private void BtnReadAllPerson_Click(object sender, EventArgs e)
        {
            var cmdDtl = mMainForm.GetCommandDetail();
            var cmd = new ReadPersonDataBase(cmdDtl);
            cmdDtl.Timeout = 15000;
            mMainForm.AddCommand(cmd);
            cmdDtl.CommandCompleteEvent += (sdr, cmde) =>
            {
                ReadPersonDataBase_Result result = cmde.Command.getResult() as ReadPersonDataBase_Result;
                StringBuilder sLogs = new StringBuilder();
                StringBuilder sLogs2 = new StringBuilder();
                Invoke(() =>
                {
                    if (result.DataBaseSize > 0)
                    {
                        result.PersonList = result.PersonList.OrderBy(o => o.IsFaceFeatureCode).ToList();
                        foreach (Data.Person person in result.PersonList)
                        {
                            AddPersonToList(person);
                            DebugPersonDetail(person, sLogs);
                            if (!person.IsFaceFeatureCode)
                            {
                                sLogs2.Append(person.UserCode + ",");
                            }
                        }
                    }
                    PersonList.RaiseListChangedEvents = true;
                    PersonList.ResetBindings();
                });



                mMainForm.AddCmdLog(cmde, Lng("Msg_7") + result.DataBaseSize);
                if (sLogs2.Length > 0)
                {
                    sLogs2.Length += 1;
                    frmRecord.SaveFile(sLogs2, "没有图片人员编号" + $"{DateTime.Now:yyyyMMddHHmmss}.txt");
                }
                if (sLogs.Length > 0)
                {
                    string sFile = frmRecord.SaveFile(sLogs, Lng("Msg_8") + $"{DateTime.Now:yyyyMMddHHmmss}.txt");
                    mMainForm.AddCmdLog(cmde, Lng("Msg_9") + sFile);
                }
            };
        }

        private void BtnClearDataBase_Click(object sender, EventArgs e)
        {
            var cmdDtl = mMainForm.GetCommandDetail();
            cmdDtl.Timeout = 5000;
            var cmd = new ClearPersonDataBase(cmdDtl);
            mMainForm.AddCommand(cmd);
        }

        private void BtnWriteAllPerson_Click(object sender, EventArgs e)
        {
            var cmdDtl = mMainForm.GetCommandDetail();
            cmdDtl.Timeout = 15000;
            cmdDtl.RestartCount = 0;
            INCommand cmd = null;
            List<Data.Person> persons = new List<Data.Person>();
            foreach (var item in PersonList)
            {
                if (item.Person.CardData < UInt32.MaxValue)
                {
                    persons.Add(item.Person);
                }
            }
            if (persons.Count > 0)
            {
                AddPerson_Parameter par = new AddPerson_Parameter(persons);
                cmd = new AddPerson(cmdDtl, par);
            }

            if (cmd == null) return;
            mMainForm.AddCommand(cmd);

            cmdDtl.CommandCompleteEvent += (sdr, cmde) =>
            {
                var result = cmd.getResult() as WritePerson_Result;
                WritePersonCallBlack(cmde, result);
            };
        }

        private void BtnClearList_Click(object sender, EventArgs e)
        {
            PersonList.Clear();
            CardHashTable.Clear();
        }

        private void DgPersonList_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            int col = e.ColumnIndex, row = e.RowIndex;
            if (row < 0) return;
            var gdRow = dgPersonList.Rows[row];
            var PersonUI = gdRow.DataBoundItem as Person_UI;
            // StringBuilder strBuf = new StringBuilder();

            // DebugPerson(PersonUI.Person, strBuf);
            //txtDebug.Text = strBuf.ToString();
            PersonToControl(PersonUI.Person);
        }

        private void DebugPerson(Data.Person person, StringBuilder sLogs)
        {
            var ui = new Person_UI(person);
            DebugPersonDetail(person, sLogs);
            sLogs.AppendLine();
        }

        /// <summary>
        /// 将卡片输出到buf中
        /// </summary>
        /// <param name="card"></param>
        private StringBuilder DebugPersonDetail(Data.Person person, StringBuilder strBuf)
        {
            Person_UI ui = new Person_UI(person);
            //strBuf.Append("用户号：").Append(ui.UserCode).Append("；姓名：").Append(ui.PName).Append("；部门：").Append(ui.Dept).Append("；职务：").Append(ui.Job).Append("；工号：").Append(ui.PCode);
            //strBuf.Append("卡号：").Append(ui.CardData).Append("；密码：").Append(ui.Password);
            //strBuf.Append("；有效期：").Append(ui.Expiry).Append("；有效次数：").Append(ui.OpenTimes).AppendLine("；");
            //strBuf.Append("；开门时段：").Append(ui.TimeGroup);
            //strBuf.Append("；状态：").Append(ui.CardStatus).Append("；用户身份：").AppendLine(ui.Identity);
            //strBuf.Append("；节假日：").Append(ui.Holiday).Append("(1->32)");
            //strBuf.Append("；出入标志：").Append(ui.EnterStatus);
            //strBuf.Append("；最近读卡时间：").AppendLine(ui.ReadCardDate);
            //strBuf.Append("；是否有人脸：").AppendLine(ui.IsFaceFeature);
            //strBuf.Append("；指纹数：").AppendLine(ui.FingerprintCount.ToString());
            // string.Empty
            strBuf.AppendLine(Lng("Msg_10", ui.UserCode, ui.PName, ui.Dept, ui.Job, ui.PCode));
            strBuf.AppendLine(Lng("Msg_11", ui.CardData, ui.Password));
            strBuf.AppendLine(Lng("Msg_12", ui.Expiry, ui.OpenTimes));
            strBuf.AppendLine(Lng("Msg_13", ui.TimeGroup, ui.CardStatus, ui.Identity));
            strBuf.AppendLine(Lng("Msg_14", ui.Holiday, ui.EnterStatus));
            strBuf.AppendLine(Lng("Msg_15", ui.ReadCardDate, ui.IsFaceFeature, ui.FingerprintCount));
            strBuf.AppendLine("--------------------------------------------------------------------------------------");
            return strBuf;
        }
        //private string IsEmpty(string value)
        //{

        //}

        /// <summary>
        /// 将卡片输出到控件中
        /// </summary>
        /// <param name="person"></param>
        private void PersonToControl(Data.Person person)
        {
            Person_UI ui = new Person_UI(person);
            txtCardDataHex.Text = person.CardData.ToString("X");
            txtCardData.Text = person.CardData.ToString();
            txtPassword.Text = ui.Password;
            dtpDate.Value = person.Expiry;
            dtpTime.Value = person.Expiry;
            cmbOpenTimes.Text = ui.OpenTimes;
            if (person.CardStatus < 4)
                cmbCardStatus.SelectedIndex = person.CardStatus;

            cmbTimeGroup.SelectedIndex = person.TimeGroup - 1;
            txtUserCode.Text = person.UserCode.ToString();
            txtPName.Text = person.PName;
            txtPCode.Text = person.PCode;
            txtJob.Text = person.Job;
            txtDept.Text = person.Dept;
            try
            {
                cmbEnterStatus.SelectedIndex = person.EnterStatus;
                cmbIdentity.SelectedIndex = person.Identity;
            }
            catch (Exception)
            {

            }

            //if (person.HolidayUse)
            txtHoliday.Text = ui.Holiday;
            //else
            //    txtHoliday.Text = new string('1', 30);

        }

        private void ButCreateCardNumByRandom_Click(object sender, EventArgs e)
        {
            int iCreateCount = CheckCreateCardCount();
            if (iCreateCount <= 0) return;

            Data.Person person;
            for (int i = 0; i < iCreateCount; i++)
            {
                person = CreateNewPerson(0);
                if (person != null)
                    AddPersonToList(person);
            }
            PersonList.RaiseListChangedEvents = true;
            PersonList.ResetBindings();
        }

        /// <summary>
        /// 将一个人员详情添加到系统缓冲中
        /// </summary>
        /// <param name="card"></param>
        /// <returns></returns>
        private bool AddPersonToList(Data.Person person)
        {

            if (!CardHashTable.Contains(person.UserCode))
            {
                var ui = new Person_UI(person);
                ui.CardIndex = (PersonList.Count + 1).ToString();
                PersonList.Add(ui);
                CardHashTable.Add(person.UserCode);
                return true;
            }
            return false;
        }

        private Data.Person AddPersonDetailToList()
        {
            Data.Person card = ContorlToPerson();
            if (card == null) return null;
            //检查卡片是否已存在
            if (CardHashTable.Contains(card.UserCode))
            {
                int iCount = PersonList.Count;
                for (int i = 0; i < iCount; i++)
                {
                    if (PersonList[i].Person.UserCode == card.UserCode)
                    {
                        PersonList[i].Person = card;
                        PersonList.ResetItem(i);
                        break;
                    }
                }

            }
            else
            {
                CardHashTable.Add(card.UserCode);
                PersonList.Add(new Person_UI(card));

            }
            return card;
        }

        private Random mCardRnd = new Random();
        private int mCardMax = 0x6FFFFFFF;
        private int mCardMin = 0x10000000;

        private Data.Person CreateNewPerson(uint iUserCode)
        {
            Data.Person person;
            uint userCode;
            if (iUserCode == 0)
            {
                // ulong cardNum = (UInt64)(mCardRnd.Next(mCardMax) % (mCardMax - mCardMin + 1) + mCardMin);

                // ulong cardNum2 = (UInt64)(mCardRnd.Next(mCardMax) % (mCardMax - mCardMin + 1) + mCardMin);
                // _ = (cardNum << 32) + cardNum2;

                userCode = (uint)(mCardRnd.Next(mCardMax) % (mCardMax - mCardMin + 1) + mCardMin);
            }
            else
            {
                userCode = iUserCode;
            }
            if (CardHashTable.Contains(userCode))
            {
                if (iUserCode == 0)
                {
                    //有重复
                    return CreateNewPerson(0);
                }
                else
                {
                    return null;
                }

            }
            person = new Data.Person();
            person.UserCode = userCode;
            person.CardData = userCode;
            person.PName = $"人员{userCode}";
            person.EnterStatus = 3;
            person.Expiry = new DateTime(2089, 12, 31);
            person.OpenTimes = 65535;
            person.TimeGroup = 1;
            return person;
        }

        /// <summary>
        /// 检查待创建的卡号数量
        /// </summary>
        /// <returns></returns>
        private int CheckCreateCardCount()
        {
            int max = 20000;
            if (!int.TryParse(txtCount.Text, out int iCreateCount))
            {
                MessageBox.Show("输入的数字不正确，取值范围：0-" + max);
                return 0;
            }
            if (iCreateCount > max)
            {
                MessageBox.Show("输入的数字不正确，取值范围：0-" + max);
                return 0;
            }
            if ((iCreateCount + PersonList.Count) > max)
            {
                iCreateCount = max - PersonList.Count;

            }
            if (iCreateCount <= 0) return 0;

            return iCreateCount;
        }

        private void BtnDelList_Click(object sender, EventArgs e)
        {
            var lst = PersonList.Where(t => t.Selected == false).ToArray();

            if (lst.Length == 0)
                return;

            PersonList.RaiseListChangedEvents = false;
            PersonList.Clear();
            CardHashTable.Clear();
            foreach (var c in lst)
            {
                PersonList.Add(c);
                CardHashTable.Add(c.Person.UserCode);
            }
            PersonList.RaiseListChangedEvents = true;
            PersonList.ResetBindings();
        }

        private void BtnDelDevice_Click(object sender, EventArgs e)
        {
            var cmdDtl = mMainForm.GetCommandDetail();
            cmdDtl.Timeout = 15000;
            cmdDtl.RestartCount = 0;
            INCommand cmd = null;
            Data.Person person = ContorlToPerson();
            if (person == null) return;
            List<Data.Person> persons = new List<Data.Person>();
            persons.Add(person);
            var par = new DeletePerson_Parameter(persons);
            cmd = new DeletePerson(cmdDtl, par);

            mMainForm.AddCommand(cmd);
        }

        /// <summary>
        /// 将控件中的值转换为卡详情
        /// </summary>
        /// <returns></returns>
        private Data.Person ContorlToPerson()
        {
            Data.Person person = new Data.Person();
            string CardStr = txtCardData.Text;
            person.CardData = CardStr.ToUInt64();
            if (person.CardData < 0)
            {
                MsgErr("卡号输入不正确！");
                return null;
            }
            string sPwd = txtPassword.Text;
            if (!string.IsNullOrEmpty(sPwd))
            {
                if (!sPwd.IsNum())
                {
                    MsgErr("个人密码必须输入数字！");
                    return null;
                }
                if (sPwd.Length < 4)
                {
                    MsgErr("个人密码请输入 4-8个数字！");
                    return null;
                }
                person.Password = sPwd.FillString(8, 'F');
            }
            //有效期
            var dtpD = dtpDate.Value; var dtpT = dtpTime.Value;
            person.Expiry = new DateTime(dtpD.Year, dtpD.Month, dtpD.Day, dtpT.Hour, dtpT.Minute, 59);
            person.UserCode = uint.Parse(txtUserCode.Text);
            person.PName = txtPName.Text;
            person.PCode = txtPCode.Text;
            person.Dept = txtDept.Text;
            person.Job = txtJob.Text;
            person.TimeGroup = cmbTimeGroup.SelectedIndex + 1;
            person.CardStatus = cmbCardStatus.SelectedIndex;
            person.CardType = cmbCardType.SelectedIndex;
            person.Identity = cmbIdentity.SelectedIndex;
            if (cmbOpenTimes.Text == Person_UI.OpenTimes_Invalid)
                person.OpenTimes = 0;
            else
            {
                if (cmbOpenTimes.Text == Person_UI.OpenTimes_Off)
                    person.OpenTimes = 65535;
                else
                {
                    string sTimes = cmbOpenTimes.Text.Replace("次", string.Empty).Trim();
                    if (sTimes.IsNum())
                    {
                        person.OpenTimes = Convert.ToUInt16(sTimes);
                    }
                }
            }

            string sHol = txtHoliday.Text.Trim();
            sHol.FillString(32, '0');
            var chars = sHol.ToCharArray();
            for (int i = 0; i < 32; i++)
            {
                person.SetHolidayValue(i + 1, chars[i] == '1');
            }
            return person;

        }

        private void BtnAddDevice_Click(object sender, EventArgs e)
        {
            Data.Person person = AddPersonDetailToList();
            if (person == null) return;

            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;
            //cmdDtl.Timeout = 10000;
            INCommand cmd;

            List<Data.Person> persons = new List<Data.Person>();
            persons.Add(person);
            var par = new AddPerson_Parameter(persons);
            cmd = new AddPerson(cmdDtl, par);


            mMainForm.AddCommand(cmd);

            cmdDtl.CommandCompleteEvent += (sdr, cmde) =>
            {
                var result = cmd.getResult() as WritePerson_Result;
                WritePersonCallBlack(cmde, result);
            };
        }

        private void WritePersonCallBlack(CommandEventArgs cmde, WritePerson_Result result)
        {
            if (result != null)
            {
                mMainForm.AddCmdLog(cmde, Lng("Msg_16") + result.FailTotal);
                if (result.FailTotal > 0)
                {
                    StringBuilder strBuf = new StringBuilder();
                    foreach (var item in result.PersonList)
                    {
                        strBuf.Append(item.ToString("00000000000000000000")).Append("(0x").Append(item.ToString("X18")).Append(")");
                    }
                    //txtDebug.Text = strBuf.ToString();
                    System.IO.File.WriteAllText(System.IO.Path.Combine(Application.StartupPath, Lng("Msg_17") + ".txt"), strBuf.ToString(), Encoding.UTF8);
                }
            }
        }

        private void ChkSelectAll_CheckedChanged(object sender, EventArgs e)
        {
            bool bSelect = chkSelectAll.Checked;
            foreach (var item in PersonList)
            {
                item.Selected = bSelect;
            }
        }

        private void BtnAddList_Click(object sender, EventArgs e)
        {
            AddPersonDetailToList();
        }

        private void BtnCheckUserCode_Click(object sender, EventArgs e)
        {
            var cmdDtl = mMainForm.GetCommandDetail();
            uint usercode = 0;
            if (!uint.TryParse(txtUserCode.Text, out usercode))
            {
                MessageBox.Show(Lng("Msg_18"));
                return;
            }
            var par = new ReadPersonDetail_Parameter(usercode);
            var cmd = new ReadPersonDetail(cmdDtl, par);
            mMainForm.AddCommand(cmd);

            cmdDtl.CommandCompleteEvent += (sdr, cmde) =>
            {
                var result = cmd.getResult() as ReadPersonDetail_Result;

                if (!result.IsReady)
                {
                    mMainForm.AddCmdLog(cmde, Lng("Msg_19"));
                }
                else
                {
                    Invoke(() => {
                        PersonToControl(result.Person);
                        mMainForm.AddCmdLog(cmde, Lng("Msg_20"));
                    });
                    
                }
            };
        }

        private void butSelectImage_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = Lng("Msg_21") + "|*.jpg";
            ofd.Multiselect = false;
            if (ofd.ShowDialog() != DialogResult.OK) return;
            picUpload.Image = null;
            mPersonImagePath = string.Empty;

            var buf = System.IO.File.ReadAllBytes(ofd.FileName);
            buf = ImageTool.ConvertImage(buf, 480, 640, 122880);

            picUpload.Image = ImageTool.ReadImageByBuf(buf);
            mPersonImagePath = ofd.FileName;
        }

        private void btnAddPesonAndImage_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(mPersonImagePath))
            {
                MsgTip(Lng("Msg_22"));
                return;
            }
            uint sCode = 0;
            string sName = string.Empty;
            try
            {
                sCode = uint.Parse(txtUploadCode.Text);
                sName = txtUploadName.Text;
            }
            catch (Exception)
            {
                MsgErr(Lng("Msg_23"));
                return;
            }


            Data.Person person = new Data.Person(sCode, sName);
            if (person == null) return;

            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;
            //cmdDtl.Timeout = 10000;
            INCommand cmd;

            byte[] datas = System.IO.File.ReadAllBytes(mPersonImagePath);
            datas = ImageTool.ConvertImage(datas, 480, 640, 122880);
            IdentificationData id = new IdentificationData(1, datas);

            var par = new AddPersonAndImage_Parameter(person, id);
            par.WaitRepeatMessage = true;//固件版本v4.28以上才能用
            cmd = new AddPeosonAndImage(cmdDtl, par);


            mMainForm.AddCommand(cmd);

            cmdDtl.CommandCompleteEvent += (sdr, cmde) =>
            {

                var result = cmd.getResult() as AddPersonAndImage_Result;
                AddPersonAndImageCallBlack(cmde, result);
            };
        }

        private void AddPersonAndImageCallBlack(CommandEventArgs cmde, AddPersonAndImage_Result result)
        {
            if (result != null)
            {
                var ids = result.IdDataUploadStatus;

                mMainForm.AddCmdLog(cmde, Lng("Msg_24", result.UserUploadStatus, ids[0]));
                if (ids[0] == 4)
                {
                    mMainForm.AddCmdLog(cmde, Lng("Msg_25") + result.IdDataRepeatUser[0]);
                }
            }
        }

        Dictionary<int, string> sStatusTipDic = new Dictionary<int, string>();

        private void Button2_Click(object sender, EventArgs e)
        {
            uint sCode = uint.Parse(txtRegUserCode.Text);
            string sName = txtRegUserName.Text;

            Data.Person person = new Data.Person(sCode, sName);

            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;
            //cmdDtl.Timeout = 10000;
            INCommand cmd;

            var par = new RegisterIdentificationData_Parameter(person, 3);
            cmd = new RegisterIdentificationData(cmdDtl, par);
            picReg.Image = null;

            if (sStatusTipDic.Count == 0)
            {
                //1、已开始注册；2、用户号不存在；3、类型错误或不支持；4、序号已超出范围。5、设备存储空间已满 101、注册成功；102、用户取消操作
                sStatusTipDic.Add(1, Lng("Register_Status1"));//已开始注册
                sStatusTipDic.Add(2, Lng("Register_Status2"));//用户号不存在
                sStatusTipDic.Add(3, Lng("Register_Status3"));//类型错误或不支持
                sStatusTipDic.Add(4, Lng("Register_Status4"));//序号已超出范围
                sStatusTipDic.Add(5, Lng("Register_Status5"));//设备存储空间已满
                sStatusTipDic.Add(102, Lng("Register_Status102"));//用户取消操作
                sStatusTipDic.Add(103, Lng("Register_Status103"));//注册信息重复
            }

            mMainForm.AddCommand(cmd);

            cmdDtl.CommandCompleteEvent += (sdr, cmde) =>
            {
                var result = cmde.Result as RegisterIdentificationData_Result;
                if (result.Status == 101)
                {

                    if (result.ResultData != null)
                    {
                        string sFile = System.IO.Path.Combine(Application.StartupPath, $"Photo_{sCode}.jpg");
                        var buf = result.ResultData.DataBuf;
                        System.IO.File.WriteAllBytes(sFile, buf);
                        using (var ms = new System.IO.MemoryStream(buf))
                        {
                            Image img = Image.FromStream(ms);
                            picReg.Image = img;
                        }

                        mMainForm.AddLog(Lng("Msg_26"));
                    }
                    else
                    {
                        mMainForm.AddLog(Lng("Msg_29"));//注册成功，但是读取文件时发生错误。

                    }

                }
                else
                {
                    string sTip = sStatusTipDic[result.Status];

                    if (result.Status == 103)
                    {
                        sTip = string.Format(sTip, result.UserID);
                    }
                    mMainForm.AddLog(sTip);
                }
            };
        }


        private void btnDelSelect_Click(object sender, EventArgs e)
        {

        }

        private void tabPage4_Click(object sender, EventArgs e)
        {

        }

        private void gpUserIdentityDetail_Enter(object sender, EventArgs e)
        {

        }

        private void butCreateCardNumByOrder_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            var outFileDialog = new SaveFileDialog();
            outFileDialog.Filter = "Excel|*.csv;";
            if (outFileDialog.ShowDialog() != DialogResult.OK)
            {
                return;
            }
            var filePath = outFileDialog.FileName;
            StringBuilder sBuf = new StringBuilder(1024);
            string sTitleCol = Lng("Export_TitleCol");
            sTitleCol = sTitleCol.Replace("\\t", "\t");
            sBuf.AppendLine(sTitleCol);//用户号\t,人员编号\t,人员姓名\t,人员部门\t,人员职务\t,卡号\t,密码\t,人脸\t,指纹
            foreach (var item in PersonList)
            {
                sBuf.Append(item.UserCode).Append("\t,");
                sBuf.Append(item.PCode).Append("\t,");
                sBuf.Append(item.PName).Append("\t,");
                sBuf.Append(item.Dept).Append("\t,");
                sBuf.Append(item.Job).Append("\t,");
                sBuf.Append(item.CardData).Append("\t,");
                sBuf.Append(item.Password).Append("\t,");
                sBuf.Append(item.IsFaceFeature).Append("\t,");
                sBuf.Append(item.FingerprintCount).Append("\t");
                sBuf.AppendLine();

                //sBuf.AppendLine(item.UserCode + "," + item.PCode + "," + item.PName + ","
                //    + item.Dept + "," + item.Job + "," + item.CardData + ","
                //    + item.Password + "," + item.IsFaceFeature + "," + item.FingerprintCount);
                
            }
            try
            {
                File.WriteAllText(filePath, sBuf.ToString());
            }
            catch (Exception ex)
            {

                MsgErr(ex.Message);
                return;
            }
            
        }

        private void button4_Click(object sender, EventArgs e)
        {

        }
    }
}
