using DoNetDrive.Protocol.Door.Door8800.Data;
using DoNetDrive.Protocol.Transaction;
using DoNetDrive.Common.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DoNetDrive.Protocol.Fingerprint.Transaction;
using DoNetDrive.Core.Command;

namespace DoNetDrive.Protocol.Fingerprint.Test
{
    public partial class frmRecord : frmNodeForm
    {
        #region 事件类型初始化
        public static string[] mWatchTypeNameList;
        public static string[] mCardTransactionList, mDoorSensorTransactionList, mSystemTransactionList;
        /// <summary>
        /// 事件代码名称列表
        /// </summary>
        public static List<string[]> mTransactionCodeNameList;

        #endregion

        #region 窗口单例模式
        private static object lockobj = new object();
        private static frmRecord onlyObj;
        public static frmRecord GetForm(INMain main)
        {
            if (onlyObj == null)
            {
                lock (lockobj)
                {
                    if (onlyObj == null)
                    {
                        onlyObj = new frmRecord(main);
                        frmMain.AddNodeForms(onlyObj);
                    }
                }
            }
            return onlyObj;
        }
        #endregion

        private frmRecord(INMain main)
        {
            InitializeComponent();
            mMainForm = main;
        }

        private void frmRecord_Load(object sender, EventArgs e)
        {
            LoadUILanguage();

        }
        public override void LoadUILanguage()
        {
            base.LoadUILanguage();
            Lng(gpTransactionDatabaseDetail);
            Lng(Lbl_Quantity);
            Lng(Lbl_NewRecord);
            Lng(Lbl_WriteIndex);
            Lng(Lbl_ReadIndex);
            Lng(Lbl_CardRecord);
            Lng(Lbl_DoorMagneticRecord);
            Lng(Lbl_SystemRecord);
            Lng(Lbl_BodyTemperature);
            Lng(butTransactionDatabaseDetail);
            Lng(Lbl_MemoryPointerOperation);
            Lng(Lbl_TransactionDatabaseType1);
            Lng(Lbl_WriteIndex1);
            Lng(Lbl_ReadIndex1);
            Lng(butTransactionDatabaseWriteIndex);
            Lng(butTransactionDatabaseReadIndex);
            Lng(gpRecordOperation);
            Lng(Lbl_TransactionDatabaseType2);
            Lng(Lbl_Quantity2);
            Lng(button4);
            Lng(butClearTransactionDatabase);
            Lng(gpRecordOperation2);
            Lng(Lbl_TransactionDatabaseType3);
            Lng(Lbl_Quantity3);
            Lng(Lbl_ReadIndex3);
            Lng(butTransactionDatabaseByIndex);
            Lng(Lbl_Quantity4);
            Lng(chkAutoRead);
            Lng(btnReadTransactionDatabase);
            Lng(Lbl_ImageDire);
            Lng(butSelectDire);
            Lng(btnReadImageTransactionDatabase);
            Lng(butClearAllTransactionDatabase);
            Lng(chkAutoReadImage);
            Lng(chkAutoWriteIndex);
            Lng(gpAutoRead);
            Lng(btnReadOld);
            e_TransactionDatabaseType();

        }
        private static string GetLanguage_St(string sKey, params object[] args)
        {
            string str = ToolLanguage.GetLanguage("frmRecord", sKey);
            return string.Format(str, args);
        }

        static frmRecord()
        {
            mWatchTypeNameList = GetLanguage_St("WatchTypeNameList").Split(',');//new string[] { "", "读卡信息", "门磁信息", "系统信息", "连接保活消息", "连接确认信息" };
            mCardTransactionList = new string[256];
            mDoorSensorTransactionList = new string[256];
            mSystemTransactionList = new string[256];

            mTransactionCodeNameList = new List<string[]>(10);
            mTransactionCodeNameList.Add(null);//0是没有的
            mTransactionCodeNameList.Add(mCardTransactionList);
            mTransactionCodeNameList.Add(mDoorSensorTransactionList);
            mTransactionCodeNameList.Add(mSystemTransactionList);

            //mCardTransactionList[1] = "刷卡验证";//
            //mCardTransactionList[2] = "指纹验证";//------------卡号为密码
            //mCardTransactionList[3] = "人脸验证";//
            //mCardTransactionList[4] = "指纹 + 刷卡";//
            //mCardTransactionList[5] = "人脸 + 指纹";//
            //mCardTransactionList[6] = "人脸 + 刷卡";//   ---  常开工作方式中，刷卡进入常开状态
            //mCardTransactionList[7] = "刷卡 + 密码";//  --  多卡验证组合完毕后触发
            //mCardTransactionList[8] = "人脸 + 密码";//
            //mCardTransactionList[9] = "指纹 + 密码";//
            //mCardTransactionList[10] = "手动输入用户号加密码验证";//
            //mCardTransactionList[11] = "指纹+刷卡+密码";//
            //mCardTransactionList[12] = "人脸+刷卡+密码";//
            //mCardTransactionList[13] = "人脸+指纹+密码";//  --  不开门
            //mCardTransactionList[14] = "人脸+指纹+刷卡";//
            //mCardTransactionList[15] = "重复验证";//
            //mCardTransactionList[16] = "有效期过期";//
            //mCardTransactionList[17] = "开门时段过期";//------------卡号为错误密码
            //mCardTransactionList[18] = "节假日时不能开门";//----卡号为卡号。
            //mCardTransactionList[19] = "未注册用户";//
            //mCardTransactionList[20] = "探测锁定";//
            //mCardTransactionList[21] = "有效次数已用尽";//
            //mCardTransactionList[22] = "锁定时验证，禁止开门";//
            //mCardTransactionList[23] = "挂失卡";//
            //mCardTransactionList[24] = "黑名单卡";//
            //mCardTransactionList[25] = "免验证开门 -- 按指纹时用户号为0，刷卡时用户号是卡号";//
            //mCardTransactionList[26] = "禁止刷卡验证  --  【权限认证方式】中禁用刷卡时";//
            //mCardTransactionList[27] = "禁止指纹验证  --  【权限认证方式】中禁用指纹时";//
            //mCardTransactionList[28] = "控制器已过期";//
            //mCardTransactionList[29] = "验证通过—有效期即将过期";//
            //mCardTransactionList[30] = "体温异常，拒绝进入";//
            for (int i = 1; i < 31; i++)
            {
                mCardTransactionList[i] = GetLanguage_St($"CardTransactionList{i}");
            }


            //mDoorSensorTransactionList[1] = "开门";//
            //mDoorSensorTransactionList[2] = "关门";//
            //mDoorSensorTransactionList[3] = "进入门磁报警状态";//
            //mDoorSensorTransactionList[4] = "退出门磁报警状态";//
            //mDoorSensorTransactionList[5] = "门未关好";//
            //mDoorSensorTransactionList[6] = "使用按钮开门";//
            //mDoorSensorTransactionList[7] = "按钮开门时门已锁定";//
            //mDoorSensorTransactionList[8] = "按钮开门时控制器已过期";//
            for (int i = 1; i < 9; i++)
            {
                mDoorSensorTransactionList[i] = GetLanguage_St($"DoorSensorTransactionList{i}");
            }

            //mSystemTransactionList[1] = "软件开门";//
            //mSystemTransactionList[2] = "软件关门";//
            //mSystemTransactionList[3] = "软件常开";//
            //mSystemTransactionList[4] = "控制器自动进入常开";//
            //mSystemTransactionList[5] = "控制器自动关闭门";//
            //mSystemTransactionList[6] = "长按出门按钮常开";//
            //mSystemTransactionList[7] = "长按出门按钮常闭";//
            //mSystemTransactionList[8] = "软件锁定";//
            //mSystemTransactionList[9] = "软件解除锁定";//
            //mSystemTransactionList[10] = "控制器定时锁定--到时间自动锁定";//
            //mSystemTransactionList[11] = "控制器定时锁定--到时间自动解除锁定";//
            //mSystemTransactionList[12] = "报警--锁定";//
            //mSystemTransactionList[13] = "报警--解除锁定";//
            //mSystemTransactionList[14] = "非法认证报警";//
            //mSystemTransactionList[15] = "门磁报警";//
            //mSystemTransactionList[16] = "胁迫报警";//
            //mSystemTransactionList[17] = "开门超时报警";//
            //mSystemTransactionList[18] = "黑名单报警";//
            //mSystemTransactionList[19] = "消防报警";//
            //mSystemTransactionList[20] = "防拆报警";//
            //mSystemTransactionList[21] = "非法认证报警解除";//
            //mSystemTransactionList[22] = "门磁报警解除";//
            //mSystemTransactionList[23] = "胁迫报警解除";//
            //mSystemTransactionList[24] = "开门超时报警解除";//
            //mSystemTransactionList[25] = "黑名单报警解除";//
            //mSystemTransactionList[26] = "消防报警解除";//
            //mSystemTransactionList[27] = "防拆报警解除";//
            //mSystemTransactionList[28] = "系统加电";//
            //mSystemTransactionList[29] = "系统错误复位（看门狗）";//
            //mSystemTransactionList[30] = "设备格式化记录";//
            //mSystemTransactionList[31] = "读卡器接反";//
            //mSystemTransactionList[32] = "读卡器线路未接好";//
            //mSystemTransactionList[33] = "无法识别的读卡器";//
            //mSystemTransactionList[34] = "网线已断开";//
            //mSystemTransactionList[35] = "网线已插入";//
            //mSystemTransactionList[36] = "WIFI 已连接";//
            //mSystemTransactionList[37] = "WIFI 已断开";//

            for (int i = 1; i <= 39; i++)
            {
                mSystemTransactionList[i] = GetLanguage_St($"SystemTransactionList{i}");
            }
        }

        #region 记录类型
        public void e_TransactionDatabaseType()
        {
            string[] array = Lng("TransactionDatabaseType").Split(','); //new string[] { "读卡记录", "门磁记录", "系统记录", "体温记录" };
            cboe_TransactionDatabaseType1.Items.Clear();
            cboe_TransactionDatabaseType1.Items.AddRange(array);
            cboe_TransactionDatabaseType1.SelectedIndex = 0;

            cboe_TransactionDatabaseType2.Items.Clear();
            cboe_TransactionDatabaseType2.Items.AddRange(array);
            cboe_TransactionDatabaseType2.SelectedIndex = 0;

            cboe_TransactionDatabaseType3.Items.Clear();
            cboe_TransactionDatabaseType3.Items.AddRange(array);
            cboe_TransactionDatabaseType3.SelectedIndex = 0;
        }
        #endregion

        #region 清空所有记录
        private void butClearAllTransactionDatabase_Click(object sender, EventArgs e)
        {

            var cmdDtl = mMainForm.GetCommandDetail();
            var par = new ClearTransactionDatabase_Parameter();
            var cmd = new ClearTransactionDatabase(cmdDtl, par);
            mMainForm.AddCommand(cmd);
        }
        #endregion

        #region 判断记录类型
        private static e_TransactionDatabaseType Get_TransactionDatabaseType(int type)
        {
            type = type + 1;

            if (type == 2)
            {
                return Transaction.e_TransactionDatabaseType.OnDoorSensorTransaction;
            }

            if (type == 3)
            {
                return Transaction.e_TransactionDatabaseType.OnSystemTransaction;
            }
            if (type == 4)
            {
                return Transaction.e_TransactionDatabaseType.OnBodyTemperatureTransaction;
            }
            return Transaction.e_TransactionDatabaseType.OnCardTransaction;
        }
        #endregion

        #region 上传记录尾号
        private void butTransactionDatabaseWriteIndex_Click(object sender, EventArgs e)
        {
            int type = cboe_TransactionDatabaseType1.SelectedIndex;
            int WriteIndex = int.Parse(txtWriteIndex.Text.ToString());
            var cmdDtl = mMainForm.GetCommandDetail();
            var par = new Transaction.WriteTransactionDatabaseWriteIndex_Parameter(Get_TransactionDatabaseType(type), WriteIndex);
            var cmd = new Transaction.WriteTransactionDatabaseWriteIndex(cmdDtl, par);
            mMainForm.AddCommand(cmd);
        }
        #endregion

        #region 更新上传断点
        private void butTransactionDatabaseReadIndex_Click(object sender, EventArgs e)
        {
            int type = cboe_TransactionDatabaseType1.SelectedIndex;
            int ReadIndex = int.Parse(txtReadIndex.Text.ToString());

            var cmdDtl = mMainForm.GetCommandDetail();
            var par = new Transaction.WriteTransactionDatabaseReadIndex_Parameter(
                Get_TransactionDatabaseType(type), ReadIndex);
            var cmd = new Transaction.WriteTransactionDatabaseReadIndex(cmdDtl, par);
            mMainForm.AddCommand(cmd);

        }

        private async void BtnReadTransactionDatabase_Click(object sender, EventArgs e)
        {
            int type = cboe_TransactionDatabaseType3.SelectedIndex;
            int Quantity = int.Parse(txtReadTransactionDatabaseQuantity.Text.ToString());


            var cmdDtl = mMainForm.GetCommandDetail();
            cmdDtl.Timeout = 1300;
            cmdDtl.RestartCount = 2;

            var par = new ReadTransactionDatabase_Parameter((int)Get_TransactionDatabaseType(type), Quantity);
            par.AutoWriteReadIndex = chkAutoWriteIndex.Checked;

            var cmd = new ReadTransactionDatabase(cmdDtl, par);
            try
            {
                await mMainForm.AddCommandAsync(cmd);
                var result = cmd.getResult() as Protocol.Door.Door8800.Transaction.ReadTransactionDatabase_Result;
                mMainForm.AddCmdLog(cmd.GetEventArgs(), Lng("Msg_1", result.Quantity, result.TransactionList.Count, result.readable));

                if (result.TransactionList.Count > 0)
                {
                    StringBuilder sLogs = new StringBuilder(500 + (result.TransactionList.Count * 140));
                    sLogs.AppendLine(((OnlineAccess.OnlineAccessCommandDetail)cmdDtl).SN);
                    //事件类型:
                    sLogs.AppendLine(Lng("Msg_2") + mWatchTypeNameList[result.TransactionList[0].TransactionType]);
                    //读取计数：{0}；实际数量：{1}；剩余新记录数：{2}
                    sLogs.AppendLine(Lng("Msg_3", result.Quantity, result.TransactionList.Count, result.readable));


                    //按序号排序
                    result.TransactionList.Sort((x, y) => x.SerialNumber.CompareTo(y.SerialNumber));
                    foreach (var t in result.TransactionList)
                    {
                        PrintTransactionList(t, sLogs);
                    }
                    string sFile = SaveFile(sLogs, Lng("Msg_4") + $"_{DateTime.Now:yyyyMMddHHmmss}.txt");
                    mMainForm.AddCmdLog(cmd.GetEventArgs(), Lng("Msg_5") + sFile);
                    if (result.readable > 0) AutoReadTransaction();
                }
            }
            catch (Exception ex)
            {

                mMainForm.AddLog($"{cmd.GetType().Name}：\r\n{ex.Message}");
            }
        }
        private void AutoReadTransaction()
        {
            if (this.InvokeRequired)
            {
                Invoke(AutoReadTransaction);
                return;
            }
            int iReadCount = 0;
            if (!int.TryParse(txtReadTransactionDatabaseQuantity.Text, out iReadCount))
            {
                txtReadTransactionDatabaseQuantity.Text = "0";
            }


            if (chkAutoRead.Checked && chkAutoWriteIndex.Checked && iReadCount > 0)
            {
                BtnReadTransactionDatabase_Click(null, null);
            }
        }

        #endregion

        #region 按序号采集信息
        private void butTransactionDatabaseByIndex_Click(object sender, EventArgs e)
        {
            int type = cboe_TransactionDatabaseType3.SelectedIndex;
            int Quantity = int.Parse(txtQuantity.Text.ToString());
            int ReadIndex = int.Parse(txtReadIndex0.Text.ToString());
            var cmdDtl = mMainForm.GetCommandDetail();
            cmdDtl.Timeout = 2000;
            var par = new ReadTransactionDatabaseByIndex_Parameter((cboe_TransactionDatabaseType3.SelectedIndex + 1), ReadIndex, Quantity);

            var cmd = new ReadTransactionDatabaseByIndex(cmdDtl, par);

            mMainForm.AddCommand(cmd);

            cmdDtl.CommandCompleteEvent += (sdr, cmde) =>
            {

                var result = cmde.Command.getResult() as Protocol.Door.Door8800.Transaction.ReadTransactionDatabaseByIndex_Result;
                mMainForm.AddCmdLog(cmde, Lng("Msg_6", result.Quantity, result.TransactionList.Count));//$"按序号读取成功，读取数量：{result.Quantity},实际解析数量：{result.TransactionList.Count}");

                if (result.Quantity > 0)
                {
                    StringBuilder sLogs = new StringBuilder(500 + (result.TransactionList.Count * 130));
                    sLogs.AppendLine(((OnlineAccess.OnlineAccessCommandDetail)cmdDtl).SN);
                    sLogs.AppendLine(Lng("Msg_2") + mWatchTypeNameList[result.TransactionList[0].TransactionType]);
                    //sLogs.Append("读取计数：").Append(result.Quantity).Append("；实际数量：").Append(result.TransactionList.Count).AppendLine();sLogs
                    sLogs.AppendLine(Lng("Msg_7", result.Quantity, result.TransactionList.Count));
                    foreach (var t in result.TransactionList)
                    {

                        PrintTransactionList(t, sLogs);
                    }
                    string sFile = SaveFile(sLogs, Lng("Msg_8") + $"_{result.TransactionType}_{DateTime.Now:yyyyMMddHHmmss}.txt");

                    mMainForm.AddCmdLog(cmde, Lng("Msg_5") + sFile);

                }
            };
        }

        public static string SaveFile(StringBuilder sLogs, string sFileName)
        {
            string sPath = System.IO.Path.Combine(Application.StartupPath, "Log");
            if (!System.IO.Directory.Exists(sPath))
                System.IO.Directory.CreateDirectory(sPath);

            string sFile = System.IO.Path.Combine(sPath, sFileName);

            System.IO.File.WriteAllText(sFile, sLogs.ToString(), Encoding.UTF8);
            return sFile;
        }

        private void PrintTransactionList(AbstractTransaction tr, StringBuilder sLogs)
        {
            //序号：
            sLogs.Append(Lng("Msg_10")).Append(tr.SerialNumber.ToString());
            if (tr.IsNull())
            {
                sLogs.AppendLine(" --- " + Lng("Msg_11"));
                return;
            }
            if (tr.TransactionType == 4)
            {
                Fingerprint.Data.Transaction.BodyTemperatureTransaction btr = (Data.Transaction.BodyTemperatureTransaction)tr;
                float btmp = (float)btr.BodyTemperature / (float)10;
                sLogs.Append(Lng("Msg_12")).Append(btmp).AppendLine(" ℃");
                return;
            }
            sLogs.Append("，" + Lng("Msg_13")).Append(tr.TransactionDate.ToDateTimeStr());
            string[] codeNameList = mTransactionCodeNameList[tr.TransactionType];

            sLogs.Append("，" + Lng("Msg_14")).Append(tr.TransactionCode);
            sLogs.Append("(").Append(codeNameList[tr.TransactionCode]).Append(")");
            if (tr.TransactionType == 1)//读卡记录
            {
                Data.Transaction.CardTransaction cardTrans = tr as Data.Transaction.CardTransaction;
                sLogs.Append(Lng("Msg_15")).Append(cardTrans.UserCode).Append("，" + Lng("Msg_16"))
                    .Append(cardTrans.Accesstype).Append("，" + Lng("Msg_17")).AppendLine(cardTrans.Photo == 1 ? Lng("Msg_18") : Lng("Msg_19"));
            }

        }

        private void PrintCardAndImageTransactionList(AbstractTransaction tr, StringBuilder sLogs)
        {

            sLogs.Append(Lng("Msg_10")).Append(tr.SerialNumber.ToString());
            if (tr.IsNull())
            {
                sLogs.AppendLine(" --- " + Lng("Msg_11"));
                return;
            }
            Data.Transaction.CardAndImageTransaction cardTrans = (Data.Transaction.CardAndImageTransaction)tr;
            if (cardTrans.BodyTemperature > 0)
            {
                float btmp = (float)cardTrans.BodyTemperature / (float)10;
                sLogs.Append("，" + Lng("Msg_12")).Append(btmp).Append(" ℃");
            }
            sLogs.Append("，" + Lng("Msg_13")).Append(tr.TransactionDate.ToDateTimeStr());
            string[] codeNameList = mTransactionCodeNameList[tr.TransactionType];

            sLogs.Append("，" + Lng("Msg_14")).Append(tr.TransactionCode);
            sLogs.Append("(").Append(codeNameList[tr.TransactionCode]).Append(")");
            sLogs.Append("，" + Lng("Msg_15")).Append(cardTrans.UserCode).Append("，" + Lng("Msg_16"))
                 .Append(cardTrans.Accesstype).Append("，" + Lng("Msg_17"))
                 .AppendLine(cardTrans.Photo == 1 ? Lng("Msg_18") : Lng("Msg_19"));

        }
        #endregion

        private void butTransactionDatabaseDetail_Click(object sender, EventArgs e)
        {
            var cmdDtl = mMainForm.GetCommandDetail();
            var cmd = new Transaction.ReadTransactionDatabaseDetail(cmdDtl);
            mMainForm.AddCommand(cmd);
            cmdDtl.CommandCompleteEvent += (sdr, cmde) =>
            {
                var result = cmd.getResult() as Transaction.ReadTransactionDatabaseDetail_Result;
                for (int i = 0; i < 4; i++)
                {
                    TextBox txtQuantity = FindControl(gpTransactionDatabaseDetail, "txtQuantity" + (i + 1).ToString()) as TextBox;
                    TextBox txtNewRecord = FindControl(gpTransactionDatabaseDetail, "txtNewRecord" + (i + 1).ToString()) as TextBox;
                    TextBox txtWriteIndex = FindControl(gpTransactionDatabaseDetail, "txtWriteIndex" + (i + 1).ToString()) as TextBox;
                    TextBox txtReadIndex = FindControl(gpTransactionDatabaseDetail, "txtReadIndex" + (i + 1).ToString()) as TextBox;
                    TextBox txtIsCircle = FindControl(gpTransactionDatabaseDetail, "txtIsCircle" + (i + 1).ToString()) as TextBox;
                    Invoke(() =>
                    {
                        txtQuantity.Text = result.DatabaseDetail.ListTransaction[i].DataBaseMaxSize.ToString();
                        txtWriteIndex.Text = result.DatabaseDetail.ListTransaction[i].WriteIndex.ToString();
                        txtNewRecord.Text = result.DatabaseDetail.ListTransaction[i].readable().ToString();
                        txtReadIndex.Text = result.DatabaseDetail.ListTransaction[i].ReadIndex.ToString();
                        // txtIsCircle.Text = result.DatabaseDetail.ListTransaction[i].IsCircle ? "【1、循环】" : "【0、未循环】";
                    });
                }
            };
        }
        Random ran = new Random();
        private void btnReadImageTransactionDatabase_Click(object sender, EventArgs e)
        {
            int type = cboe_TransactionDatabaseType3.SelectedIndex;
            int Quantity = 0;
            int.TryParse(txtReadTransactionDatabaseQuantity.Text.ToString(), out Quantity);
            if (Quantity <= 0 || Quantity > 500)
            {
                txtReadTransactionDatabaseQuantity.Text = "500";
                Quantity = 500;
            }

            var cmdDtl = mMainForm.GetCommandDetail();
            cmdDtl.Timeout = 400;
            cmdDtl.RestartCount = 5;
            string sDir = txtImageDire.Text;
            if (string.IsNullOrWhiteSpace(sDir))
            {
                return;
            }
            if (!System.IO.Directory.Exists(sDir))
            {
                try
                {
                    System.IO.Directory.CreateDirectory(sDir);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, Lng("Msg_20"));
                    return;
                }
            }

            var par = new ReadTransactionAndImageDatabase_Parameter(Quantity, true, sDir);
            par.AutoWriteReadIndex = chkAutoWriteIndex.Checked;
            par.AutoDownloadImage = chkAutoReadImage.Checked;
            par.ImageDownloadCheckCallblack = (imgSerialNumber, record) =>
            {
                /*int RandKey = ran.Next(1, 100);
                if (RandKey > 60)
                {
                    Console.WriteLine($"跳过照片，序号：{imgSerialNumber}");
                    return false;
                }*/
                return true;
            };

            var cmd = new ReadTransactionAndImageDatabase(cmdDtl, par);
            mMainForm.AddCommand(cmd);

            cmdDtl.CommandCompleteEvent += (sdr, cmde) =>
            {

                var result = cmde.Command.getResult() as ReadTransactionAndImageDatabase_Result;

                mMainForm.AddCmdLog(cmde, Lng("Msg_1", result.Quantity, result.TransactionList.Count, result.readable));
                Console.WriteLine("BodyTemperatureReadIndex =" + result.BodyTemperatureReadIndex);
                if (result.TransactionList.Count > 0)
                {
                    StringBuilder sLogs = new StringBuilder(500 + (result.TransactionList.Count * 140));
                    sLogs.AppendLine(((OnlineAccess.OnlineAccessCommandDetail)cmdDtl).SN);

                    sLogs.AppendLine(Lng("Msg_2") + mWatchTypeNameList[result.TransactionList[0].TransactionType]);
                    //sLogs.Append("读取计数：").Append(result.Quantity).Append("；实际数量：").Append(result.TransactionList.Count).Append("；剩余新记录数：").Append(result.readable).AppendLine();
                    sLogs.AppendLine(Lng("Msg_3", result.Quantity, result.TransactionList.Count, result.readable));
                    //按序号排序
                    result.TransactionList.Sort((x, y) => x.SerialNumber.CompareTo(y.SerialNumber));

                    foreach (var t in result.TransactionList)
                    {
                        PrintCardAndImageTransactionList(t, sLogs);
                    }
                    string sFile = SaveFile(sLogs, Lng("Msg_4") + $"_{DateTime.Now:yyyyMMddHHmmss}.txt");
                    mMainForm.AddCmdLog(cmde, Lng("Msg_5") + sFile);
                    if (result.readable > 0) AutoReadTransactionAndImage();
                }
            };
        }
        private void AutoReadTransactionAndImage()
        {
            if (this.InvokeRequired)
            {
                Invoke(AutoReadTransactionAndImage);
                return;
            }
            if (chkAutoRead.Checked)
            {
                btnReadImageTransactionDatabase_Click(null, null);
            }
        }

        string SelectedPath = "";

        private void butSelectDire_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.FolderBrowserDialog dialog = new System.Windows.Forms.FolderBrowserDialog();
            dialog.Description = Lng("Msg_21");
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                if (string.IsNullOrEmpty(dialog.SelectedPath))
                {
                    //System.Windows.MessageBox.Show(this, "文件夹路径不能为空", "提示");
                    MessageBox.Show(Lng("Msg_22"));
                    return;
                }
                txtImageDire.Text = dialog.SelectedPath;
            }
        }

        private async void btnReadOld_Click(object sender, EventArgs e)
        {
            int type = cboe_TransactionDatabaseType3.SelectedIndex;

            var cmdDtl = mMainForm.GetCommandDetail();
            cmdDtl.Timeout = 1300;
            cmdDtl.RestartCount = 2;

            var par = new ReadTransactionDatabase_Parameter((int)Transaction.e_TransactionDatabaseType.OnCardTransaction, 0);

            var cmd = new ReadTransactionDatabaseByAll(cmdDtl, par);
            try
            {
                await mMainForm.AddCommandAsync(cmd);
                var result = cmd.getResult() as Protocol.Door.Door8800.Transaction.ReadTransactionDatabase_Result;
                mMainForm.AddCmdLog(cmd.GetEventArgs(), Lng("Msg_1", result.Quantity, result.TransactionList.Count, result.readable));

                if (result.TransactionList.Count > 0)
                {
                    StringBuilder sLogs = new StringBuilder(500 + (result.TransactionList.Count * 140));
                    sLogs.AppendLine(((OnlineAccess.OnlineAccessCommandDetail)cmdDtl).SN);
                    //事件类型:
                    sLogs.AppendLine(Lng("Msg_2") + mWatchTypeNameList[result.TransactionList[0].TransactionType]);
                    //读取计数：{0}；实际数量：{1}；剩余新记录数：{2}
                    sLogs.AppendLine(Lng("Msg_3", result.Quantity, result.TransactionList.Count, result.readable));


                    //按序号排序
                    result.TransactionList.Sort((x, y) => x.SerialNumber.CompareTo(y.SerialNumber));
                    foreach (var t in result.TransactionList)
                    {
                        PrintTransactionList(t, sLogs);
                    }
                    string sFile = SaveFile(sLogs, Lng("Msg_4") + $"_{DateTime.Now:yyyyMMddHHmmss}.txt");
                    mMainForm.AddCmdLog(cmd.GetEventArgs(), Lng("Msg_5") + sFile);
                    if (result.readable > 0) AutoReadTransaction();
                }
            }
            catch (Exception ex)
            {

                mMainForm.AddLog($"{cmd.GetType().Name}：\r\n{ex.Message}");
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {

        }

        public new Control FindControl(Control parentControl, string findCtrlName)
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

        private void ButClearTransactionDatabase_Click(object sender, EventArgs e)
        {
            int type = cboe_TransactionDatabaseType2.SelectedIndex;
            var cmdDtl = mMainForm.GetCommandDetail();
            var par = new Transaction.ClearTransactionDatabase_Parameter(Get_TransactionDatabaseType(type));
            var cmd = new Transaction.ClearTransactionDatabase(cmdDtl, par);
            mMainForm.AddCommand(cmd);
        }
    }
}
