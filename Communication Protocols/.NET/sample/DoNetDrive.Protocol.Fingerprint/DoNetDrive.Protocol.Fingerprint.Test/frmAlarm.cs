using DoNetDrive.Protocol.Door.Door8800.Data.TimeGroup;
using DoNetDrive.Protocol.Door.Door8800.Utility;
using DoNetDrive.Protocol.Fingerprint.Alarm.AlarmPassword;
using DoNetDrive.Protocol.Fingerprint.Alarm.AntiDisassemblyAlarm;
using DoNetDrive.Protocol.Fingerprint.Alarm.BlacklistAlarm;
using DoNetDrive.Protocol.Fingerprint.Alarm;
using DoNetDrive.Protocol.Fingerprint.Alarm.GateMagneticAlarm;
using DoNetDrive.Protocol.Fingerprint.Alarm.IllegalVerificationAlarm;
using DoNetDrive.Protocol.Fingerprint.Alarm.LegalVerificationCloseAlarm;
using DoNetDrive.Protocol.Fingerprint.Alarm.OpenDoorTimeoutAlarm;
using DoNetDrive.Protocol.Fingerprint.Alarm.SendFireAlarm;
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

namespace DoNetDrive.Protocol.Fingerprint.Test
{
    public partial class frmAlarm : frmNodeForm
    {
        #region 单例模式
        private static object lockobj = new object();
        private static frmAlarm onlyObj;
        public static frmAlarm GetForm(INMain main)
        {
            if (onlyObj == null)
            {
                lock (lockobj)
                {
                    if (onlyObj == null)
                    {
                        onlyObj = new frmAlarm(main);
                        frmMain.AddNodeForms(onlyObj);
                    }
                }
            }
            return onlyObj;
        }
        WeekTimeGroup WeekTimeGroupDoorWorkDto;
        private frmAlarm(INMain main) : base(main)
        {
            InitializeComponent();
            WeekTimeGroupDoorWorkDto = new WeekTimeGroup(8);
            WeekTimeGroupDoorWorkDto.InitTimeGroup();
        }
        #endregion

        string[] modeList;//= new string[] { "不开门，报警输出", "开门，报警输出", "锁定门，报警，只能软件解锁" };
        private void FrmAlarm_Load(object sender, EventArgs e)
        {
            LoadUILanguage();

        }

        public override void LoadUILanguage()
        {
            base.LoadUILanguage();
            Lng(gpFireAlarm);
            Lng(btnSendFireAlarm);
            Lng(gpBlacklistAlarmUse);
            Lng(cbBlacklistAlarmUse);
            Lng(btnReadBlacklistAlarm);
            Lng(btnWriteBlacklistAlarm);
            Lng(gpAntiDisassemblyUse);
            Lng(cbAntiDisassemblyUse);
            Lng(btnReadAntiDisassemblyAlarm);
            Lng(btnWriteAntiDisassemblyAlarm);
            Lng(gpIllegalVerificationAlarmUse);
            Lng(cbIllegalVerificationAlarmUse);
            Lng(Lbl_IllegalVerificationTimes1);
            Lng(Lbl_IllegalVerificationTimes2);
            Lng(btnReadIllegalVerification);
            Lng(btnWriteIllegalVerification);
            Lng(gpAlarmPasswordUse);
            Lng(cbAlarmPasswordUse);
            Lng(Lbl_AlarmPasswordMode);
            Lng(Lbl_AlarmPassword);
            Lng(btnReadAlarmPassword);
            Lng(btnWriteAlarmPassword);
            Lng(gpOpenDoorTimeoutAlarmUse);
            Lng(cbOpenDoorTimeoutAlarmUse);
            Lng(Lbl_OpenDoorTimeout);
            Lng(cbRelayOutput);
            Lng(btnReadOpenDoorTimeoutAlarm);
            Lng(btnWriteOpenDoorTimeoutAlarm);
            Lng(gpLegalVerificationCloseAlarmUse);
            Lng(cbLegalVerificationCloseAlarmUse);
            Lng(btnReadLegalVerificationCloseAlarm);
            Lng(btnWriteLegalVerificationCloseAlarm);
            Lng(gpCloseAlarm);
            Lng(checkBox1);
            Lng(checkBox2);
            Lng(checkBox3);
            Lng(checkBox4);
            Lng(checkBox5);
            Lng(checkBox6);
            Lng(checkBox7);
            Lng(btnWriteCloseAlarm);
            Lng(gpDoorWorkSetting);
            Lng(Lbl_DoorWorkSetting);
            Lng(rBtnNoDoorWorkSetting);
            Lng(rBtnDoorWorkSetting);
            Lng(btnReadGateMagneticAlarm);
            Lng(btnWriteGateMagneticAlarm);
            Lng(Lbl_GateMagneticAlarmWeek);
            Lng(Lbl_StartTime1);
            Lng(Lbl_EndTime1);
            Lng(Lbl_StartTime2);
            Lng(Lbl_EndTime2);
            Lng(Lbl_StartTime3);
            Lng(Lbl_EndTime3);
            Lng(Lbl_StartTime4);
            Lng(Lbl_EndTime4);
            Lng(Lbl_StartTime5);
            Lng(Lbl_EndTime5);
            Lng(Lbl_StartTime6);
            Lng(Lbl_EndTime6);
            Lng(Lbl_StartTime7);
            Lng(Lbl_EndTime7);
            Lng(Lbl_StartTime8);
            Lng(Lbl_EndTime8);
            Lng(btnFillNowTime);

            modeList = Lng("modeList").Split(',');
            string[] times = new string[256];
            string[] timeout = new string[257];
            var msg_8 = Lng("Msg_8");
            for (int i = 0; i < 256; i++)
            {
                times[i] = i.ToString();
                timeout[i] = i.ToString() + msg_8;
            }
            timeout[0] = Lng("Msg_9");
            timeout[256] = "65535" + msg_8;
            cmbIllegalVerificationTimes.Items.Clear();
            cmbIllegalVerificationTimes.Items.AddRange(times);
            cmbIllegalVerificationTimes.SelectedIndex = 0;

            cmbAlarmPasswordMode.Items.Clear();
            cmbAlarmPasswordMode.Items.AddRange(modeList);
            cmbAlarmPasswordMode.SelectedIndex = 0;

            cmbOpenDoorTimeout.Items.Clear();
            cmbOpenDoorTimeout.Items.AddRange(timeout);
            cmbOpenDoorTimeout.SelectedIndex = 0;


            cbxWeek.Items.Clear();
            cbxWeek.Items.AddRange(Lng("WeekList").Split(','));
            cbxWeek.SelectedIndex = 0;
            IniFireUse();


        }
        private void FrmAlarm_FormClosed(object sender, FormClosedEventArgs e)
        {
            onlyObj = null;
        }


        #region 消防报警功能开关
        private void IniFireUse()
        {
            //"禁用,启用"
            string[] FireUseList = Lng("cmbFireUse").SplitTrim(",");
            cmdFireUse.Items.Clear();
            cmdFireUse.Items.AddRange(FireUseList);
            cmdFireUse.SelectedIndex = 0;
            Lng(cmdWriteSendFireAlarm);
        }
        private void cmdWriteSendFireAlarm_Click(object sender, EventArgs e)
        {
            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;
            var cmd = new SetFireAlarm(cmdDtl, cmdFireUse.SelectedIndex == 1);
            mMainForm.AddCommand(cmd);
        }
        private void BtnSendFireAlarm_Click(object sender, EventArgs e)
        {
            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;

            WriteSendFireAlarm write = new WriteSendFireAlarm(cmdDtl);
            mMainForm.AddCommand(write);
        }
        #endregion


        private void BtnReadBlacklistAlarm_Click(object sender, EventArgs e)
        {
            StringBuilder sb = new StringBuilder();
            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;
            ReadBlacklistAlarm cmd = new ReadBlacklistAlarm(cmdDtl);
            mMainForm.AddCommand(cmd);

            //处理返回值
            cmdDtl.CommandCompleteEvent += (sdr, cmde) =>
            {
                ReadBlacklistAlarm_Result result = cmde.Command.getResult() as ReadBlacklistAlarm_Result;

                sb.Clear();
                sb.AppendLine(Lng("Msg_3"));
                sb.Append(result.IsAlarm ? Lng("Msg_1") : Lng("Msg_2"));

                Invoke(() =>
                {
                    cbBlacklistAlarmUse.Checked = result.IsAlarm;
                });

                mMainForm.AddCmdLog(cmde, sb.ToString());
            };
        }

        private void BtnWriteBlacklistAlarm_Click(object sender, EventArgs e)
        {
            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;

            WriteBlacklistAlarm_Parameter par = new WriteBlacklistAlarm_Parameter(cbBlacklistAlarmUse.Checked);
            WriteBlacklistAlarm write = new WriteBlacklistAlarm(cmdDtl, par);
            mMainForm.AddCommand(write);
        }

        private void BtnReadAntiDisassemblyAlarm_Click(object sender, EventArgs e)
        {
            StringBuilder sb = new StringBuilder();
            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;
            ReadAntiDisassemblyAlarm cmd = new ReadAntiDisassemblyAlarm(cmdDtl);
            mMainForm.AddCommand(cmd);

            //处理返回值
            cmdDtl.CommandCompleteEvent += (sdr, cmde) =>
            {
                ReadAntiDisassemblyAlarm_Result result = cmde.Command.getResult() as ReadAntiDisassemblyAlarm_Result;

                sb.Clear();
                sb.AppendLine(Lng("Msg_4"));
                sb.Append(result.IsUse ? Lng("Msg_1") : Lng("Msg_2"));

                Invoke(() =>
                {
                    cbAntiDisassemblyUse.Checked = result.IsUse;
                });

                mMainForm.AddCmdLog(cmde, sb.ToString());
            };
        }

        private void BtnWriteAntiDisassemblyAlarm_Click(object sender, EventArgs e)
        {
            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;

            WriteAntiDisassemblyAlarm_Parameter par = new WriteAntiDisassemblyAlarm_Parameter(cbAntiDisassemblyUse.Checked);
            WriteAntiDisassemblyAlarm write = new WriteAntiDisassemblyAlarm(cmdDtl, par);
            mMainForm.AddCommand(write);
        }

        private void BtnReadIllegalVerification_Click(object sender, EventArgs e)
        {
            StringBuilder sb = new StringBuilder();
            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;
            ReadIllegalVerificationAlarm cmd = new ReadIllegalVerificationAlarm(cmdDtl);
            mMainForm.AddCommand(cmd);

            //处理返回值
            cmdDtl.CommandCompleteEvent += (sdr, cmde) =>
            {
                ReadIllegalVerificationAlarm_Result result = cmde.Command.getResult() as ReadIllegalVerificationAlarm_Result;

                sb.Clear();
                sb.AppendLine(Lng("Msg_5"));
                sb.AppendLine(result.IsUse ? Lng("Msg_1") : Lng("Msg_2"));
                sb.AppendLine(Lng("Msg_6") + result.Times);
                Invoke(() =>
                {
                    cbIllegalVerificationAlarmUse.Checked = result.IsUse;
                    cmbIllegalVerificationTimes.Text = result.Times.ToString();
                });

                mMainForm.AddCmdLog(cmde, sb.ToString());
            };
        }

        private void BtnWriteIllegalVerification_Click(object sender, EventArgs e)
        {
            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;

            WriteIllegalVerificationAlarm_Parameter par = new WriteIllegalVerificationAlarm_Parameter(cbIllegalVerificationAlarmUse.Checked
                , (byte)(cmbIllegalVerificationTimes.SelectedIndex));
            WriteIllegalVerificationAlarm write = new WriteIllegalVerificationAlarm(cmdDtl, par);
            mMainForm.AddCommand(write);
        }

        private void BtnReadAlarmPassword_Click(object sender, EventArgs e)
        {
            StringBuilder sb = new StringBuilder();
            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;
            ReadAlarmPassword cmd = new ReadAlarmPassword(cmdDtl);
            mMainForm.AddCommand(cmd);

            //处理返回值
            cmdDtl.CommandCompleteEvent += (sdr, cmde) =>
            {
                ReadAlarmPassword_Result result = cmde.Command.getResult() as ReadAlarmPassword_Result;

                sb.Clear();
                sb.AppendLine(Lng("Msg_7"));
                sb.AppendLine(result.Use ? Lng("Msg_1") : Lng("Msg_2"));
                sb.AppendLine(Lng("Msg_10") + modeList[result.AlarmOption - 1]);
                sb.AppendLine(Lng("Msg_11")+result.Password);
                Invoke(() =>
                {
                    cbAlarmPasswordUse.Checked = result.Use;
                    cmbAlarmPasswordMode.SelectedIndex = result.AlarmOption - 1;
                    txtAlarmPassword.Text = result.Password;
                });

                mMainForm.AddCmdLog(cmde, sb.ToString());
            };
        }

        private void BtnWriteAlarmPassword_Click(object sender, EventArgs e)
        {
            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;

            WriteAlarmPassword_Parameter par = new WriteAlarmPassword_Parameter(cbAlarmPasswordUse.Checked, txtAlarmPassword.Text
                , Convert.ToByte(cmbAlarmPasswordMode.SelectedIndex + 1));
            WriteAlarmPassword write = new WriteAlarmPassword(cmdDtl, par);
            mMainForm.AddCommand(write);
        }

        private void BtnReadOpenDoorTimeoutAlarm_Click(object sender, EventArgs e)
        {
            StringBuilder sb = new StringBuilder();
            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;
            ReadOpenDoorTimeoutAlarm cmd = new ReadOpenDoorTimeoutAlarm(cmdDtl);
            mMainForm.AddCommand(cmd);

            //处理返回值
            cmdDtl.CommandCompleteEvent += (sdr, cmde) =>
            {
                ReadOpenDoorTimeoutAlarm_Result result = cmde.Command.getResult() as ReadOpenDoorTimeoutAlarm_Result;

                sb.Clear();
                sb.AppendLine(Lng("gpOpenDoorTimeoutAlarmUse"));
                sb.AppendLine(result.IsUse ? Lng("Msg_1") : Lng("Msg_2"));
                string str = Lng("rBtnNoDoorWorkSetting");
                if (result.AllowTime != 0)
                {
                    str = result.AllowTime + Lng("Msg_8");
                }
                sb.AppendLine(Lng("Lbl_OpenDoorTimeout") + str);
                sb.AppendLine(Lng("cbRelayOutput") + result.RelayOutput);
                Invoke(() =>
                {
                    cbOpenDoorTimeoutAlarmUse.Checked = result.IsUse;
                    if (result.AllowTime > 255)
                    {
                        cmbOpenDoorTimeout.SelectedIndex = cmbOpenDoorTimeout.Items.Count - 1;
                    }
                    else
                    {
                        cmbOpenDoorTimeout.SelectedIndex = result.AllowTime;
                    }
                    cbRelayOutput.Checked = cbRelayOutput.Checked;
                });

                mMainForm.AddCmdLog(cmde, sb.ToString());
            };
        }

        private void BtnWriteOpenDoorTimeoutAlarm_Click(object sender, EventArgs e)
        {
            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;
            ushort timeout = (ushort)cmbOpenDoorTimeout.SelectedIndex;
            if (cmbOpenDoorTimeout.SelectedIndex == cmbOpenDoorTimeout.Items.Count)
            {
                timeout = 65535;
            }
            WriteOpenDoorTimeoutAlarm_Parameter par = new WriteOpenDoorTimeoutAlarm_Parameter(cbOpenDoorTimeoutAlarmUse.Checked, timeout, cbRelayOutput.Checked);
            WriteOpenDoorTimeoutAlarm write = new WriteOpenDoorTimeoutAlarm(cmdDtl, par);
            mMainForm.AddCommand(write);
        }

        private void RBtnNoDoorWorkSetting_CheckedChanged(object sender, EventArgs e)
        {
            DoorOpenTimePanel.Visible = false;
            if (cbxWeek.SelectedIndex == -1)
            {
                cbxWeek.SelectedIndex = 0;
            }
        }

        private void RBtnDoorWorkSetting_CheckedChanged(object sender, EventArgs e)
        {
            DoorOpenTimePanel.Visible = true;
            if (cbxWeek.SelectedIndex == -1)
            {
                cbxWeek.SelectedIndex = 0;
            }
        }
        
        private void BtnReadGateMagneticAlarm_Click(object sender, EventArgs e)
        {
            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;
            ReadGateMagneticAlarm cmd = new ReadGateMagneticAlarm(cmdDtl);
            mMainForm.AddCommand(cmd);

            //处理返回值
            cmdDtl.CommandCompleteEvent += (sdr, cmde) =>
            {
                ReadGateMagneticAlarm_Result result = cmde.Command.getResult() as ReadGateMagneticAlarm_Result;
                StringBuilder sb = new StringBuilder();
                string OpenDoorWayStr = string.Empty;
                string DoorTriggerModeStr = string.Empty;
                if (!result.IsUse)
                {
                    sb.AppendLine(Lng("Msg_12"));
                    mMainForm.AddCmdLog(cmde, sb.ToString());
                }
                else
                {
                    for (int i = 0; i < 7; i++)
                    {
                        sb.AppendLine();
                        sb.AppendLine(ToolLanguage.GetWeekStr(i));
                        for (int j = 0; j < 8; j++)
                        {
                            sb.AppendLine(Lng("Msg_13") + (j + 1) + "：" + StringUtility.TimeHourAndMinuteStr(result.WeekTimeGroup.GetItem(i).GetItem(j).GetBeginTime(), result.WeekTimeGroup.GetItem(i).GetItem(j).GetEndTime()));
                        }
                        // mMainForm.AddCmdLog(null, sb.ToString());
                    }
                    mMainForm.AddCmdLog(cmde, sb.ToString());
                }
                Invoke(() =>
                {
                    if (!result.IsUse)
                    {
                        rBtnNoDoorWorkSetting.Checked = true;
                    }
                    else
                    {
                        rBtnDoorWorkSetting.Checked = true;

                        WeekTimeGroupDoorWorkDto = result.WeekTimeGroup;
                        SetAllTimePicker(DoorOpenTimePanel, "beginTimePicker", "endTimePicker", WeekTimeGroupDoorWorkDto.GetItem(0));
                    }
                });
            };
        }

        private void BtnWriteGateMagneticAlarm_Click(object sender, EventArgs e)
        {
            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;

            WriteGateMagneticAlarm_Parameter par = new WriteGateMagneticAlarm_Parameter(rBtnDoorWorkSetting.Checked, WeekTimeGroupDoorWorkDto);
            WriteGateMagneticAlarm write = new WriteGateMagneticAlarm(cmdDtl, par);
            mMainForm.AddCommand(write);
        }

        private void CbxWeek_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (WeekTimeGroupDoorWorkDto != null)
            {
                var day = WeekTimeGroupDoorWorkDto.GetItem(cbxWeek.SelectedIndex);
                SetAllTimePicker(DoorOpenTimePanel, "beginTimePicker", "endTimePicker", day);
            }
        }

        private void BtnWriteCloseAlarm_Click(object sender, EventArgs e)
        {
            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;

            byte[] list = new byte[7];
            list[0] = Convert.ToByte(checkBox1.Checked ? 1 : 0);
            list[1] = Convert.ToByte(checkBox2.Checked ? 1 : 0);
            list[2] = Convert.ToByte(checkBox3.Checked ? 1 : 0);
            list[3] = Convert.ToByte(checkBox4.Checked ? 1 : 0);
            list[4] = Convert.ToByte(checkBox5.Checked ? 1 : 0);
            list[5] = Convert.ToByte(checkBox6.Checked ? 1 : 0);
            list[6] = Convert.ToByte(checkBox7.Checked ? 1 : 0);

            CloseAlarm_Parameter par = new CloseAlarm_Parameter(list);
            CloseAlarm write = new CloseAlarm(cmdDtl, par);
            mMainForm.AddCommand(write);
        }

        private void BtnReadLegalVerificationCloseAlarm_Click(object sender, EventArgs e)
        {
            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;
            ReadLegalVerificationCloseAlarm cmd = new ReadLegalVerificationCloseAlarm(cmdDtl);
            mMainForm.AddCommand(cmd);

            //处理返回值
            cmdDtl.CommandCompleteEvent += (sdr, cmde) =>
            {
                StringBuilder sb = new StringBuilder();
                ReadLegalVerificationCloseAlarm_Result result = cmde.Command.getResult() as ReadLegalVerificationCloseAlarm_Result;

                sb.Clear();
                sb.AppendLine(Lng("gpLegalVerificationCloseAlarmUse"));
                sb.AppendLine(result.IsUse ? Lng("Msg_1") : Lng("Msg_2"));

                Invoke(() =>
                {
                    cbLegalVerificationCloseAlarmUse.Checked = result.IsUse;
                });

                mMainForm.AddCmdLog(cmde, sb.ToString());
            };
        }

        private void BtnWriteLegalVerificationCloseAlarm_Click(object sender, EventArgs e)
        {
            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;

            WriteLegalVerificationCloseAlarm_Parameter par = new WriteLegalVerificationCloseAlarm_Parameter(cbLegalVerificationCloseAlarmUse.Checked);
            WriteLegalVerificationCloseAlarm write = new WriteLegalVerificationCloseAlarm(cmdDtl, par);
            mMainForm.AddCommand(write);
        }

        private void BtnFillNowTime_Click(object sender, EventArgs e)
        {
            //开门时段

            WeekTimeGroup weekTimeGroup = new WeekTimeGroup(8);
            //weekTimeGroup.mDay
            //星期一 至 星期日
            for (int y = 0; y < 7; y++)
            {
                DayTimeGroup dayTimeGroup = weekTimeGroup.GetItem(y);
                //每天时段
                for (int i = 0; i < 8; i++)
                {
                    DateTime dtBegin = DateTime.Now;
                    DateTime dtEnd = DateTime.Now;
                    //dt = dt.AddMinutes(-1);
                    TimeSegment segment = dayTimeGroup.GetItem(i);

                    dtBegin = dtBegin.AddMinutes(i + 1);
                    segment.SetBeginTime(dtBegin.Hour, dtBegin.Minute);
                    dtEnd = dtBegin.AddMinutes(1);
                    segment.SetEndTime(dtEnd.Hour, dtEnd.Minute);
                    DateTimePicker beginTimePicker = FindControl(DoorOpenTimePanel, "beginTimePicker" + (i + 1).ToString()) as DateTimePicker;
                    DateTimePicker endTimePicker = FindControl(DoorOpenTimePanel, "endTimePicker" + (i + 1).ToString()) as DateTimePicker;
                    beginTimePicker.Value = dtBegin;// segment.GetBeginTime();
                    endTimePicker.Value = dtEnd;// segment.GetEndTime();
                }
            }
        }

        private void BeginTP_ValueChanged(object sender, EventArgs e)
        {
            DateTimePicker dtp = sender as DateTimePicker;
            SetWeekTimeGroupValue(WeekTimeGroupDoorWorkDto, cbxWeek.SelectedIndex, int.Parse(dtp.Name.Substring(15)) - 1, 1, dtp.Value);
        }

        private void EndTP_ValueChanged(object sender, EventArgs e)
        {
            DateTimePicker dtp = sender as DateTimePicker;
            SetWeekTimeGroupValue(WeekTimeGroupDoorWorkDto, cbxWeek.SelectedIndex, int.Parse(dtp.Name.Substring(13)) - 1, 2, dtp.Value);
        }
    }
}
