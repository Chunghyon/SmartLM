using DoNetDrive.Core;
using DoNetDrive.Core.Command;
using DoNetDrive.Core.Connector;
using DoNetDrive.Core.Connector.TCPServer.Client;
using DoNetDrive.Core.Connector.UDP;
using DoNetDrive.Core.Connector.TCPServer;
using DoNetDrive.Common.Extensions;
using DoNetDrive.Protocol.Door.Door8800.SystemParameter.ConnectPassword;
using DoNetDrive.Protocol.Door.Door8800.SystemParameter.SearchControltor;
using DoNetDrive.Protocol.Door.Door8800.SystemParameter.SN;
using DoNetDrive.Protocol.Door8800;
using DoNetDrive.Protocol.Fingerprint.SystemParameter.Watch;
using DoNetDrive.Protocol.Fingerprint.Test.Model;
using DoNetDrive.Protocol.OnlineAccess;
using DoNetDrive.Protocol.Transaction;
using DotNetty.Buffers;
using log4net;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AutoUpdaterDotNET;
using DoNetDrive.Core.Factory;
using DoNetDrive.Connector.COM;
using DoNetDrive.Protocol.Door.Door8800.SystemParameter.CacheContent;

namespace DoNetDrive.Protocol.Fingerprint.Test
{
    public partial class frmMain : Form, INMain
    {
        ConnectorAllocator mAllocator;
        ConnectorObserverHandler mObserver;
        private static HashSet<frmNodeForm> NodeForms;
        private static string[] TransactionTypeName;
        private static ILog _Log;

        private void Invoke(Action p)
        {
            try
            {
                Invoke((Delegate)p);
            }
            catch (Exception)
            {

                return;
            }

        }

        public void Invoke<T>(Action<T> act, T par)
        {
            try
            {
                this.Invoke((Delegate)act, par);
            }
            catch (Exception)
            {

                return;
            }

        }

        static frmMain()
        {
            NodeForms = new HashSet<frmNodeForm>();
            //初始化log4net
            string sLogXml = Path.Combine(Directory.GetCurrentDirectory(), "log4net.Config");
            log4net.Config.XmlConfigurator.Configure(new FileInfo(sLogXml));
            _Log = LogManager.GetLogger(typeof(frmMain));
            _Log.Debug("App Start");



        }

        public static void AddNodeForms(frmNodeForm frm)
        {
            if (!NodeForms.Contains(frm))
            {
                NodeForms.Add(frm);
            }
        }

        /// <summary>
        /// 准备退出
        /// </summary>
        private bool mStop;

        bool _IsClosed;

        List<string> mSNList;

        public frmMain()
        {
            InitializeComponent();


        }

        /// <summary>
        /// 窗口初始化事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FrmMain_Load(object sender, EventArgs e)
        {
            _IsClosed = false;

            string sLanguage = ConfigurationManager.AppSettings["Languages"];
            cmbToolLanguage.Items.AddRange(sLanguage.SplitTrim(","));
            cmbToolLanguage.SelectedIndex = 0;
            LoadUILanguage();

            tbEvent.SelectedIndex = 1;
            IniConnTypeList();


            AutoUpdater.CheckForUpdateEvent += AutoUpdater_CheckForUpdateEvent;
            AutoUpdater.Start("http://oss2.pc15.net/ToolDownload/Update/FaceDebugToolForNet/update.xml");

            this.Enabled = false;

        }

        private void AutoUpdater_CheckForUpdateEvent(UpdateInfoEventArgs args)
        {

            if (args.Error == null)
            {
                if (args.IsUpdateAvailable)//需要更新
                {
                    try
                    {
                        if (AutoUpdater.DownloadUpdate(args))
                        {
                            Application.Exit();
                            return;
                        }
                    }
                    catch (Exception exception)
                    {//更新下载失败

                    }

                }
            }

            Invoke(new Action(IniForm));
        }


        #region 多语言

        private string GetLanguage(string sKey)
        {
            return ToolLanguage.GetLanguage(Name, sKey);
        }
        private string GetLanguage(string sKey, params object[] args)
        {
            string str = ToolLanguage.GetLanguage(Name, sKey);
            return string.Format(str, args);
        }


        private void GetLanguage(ToolStripItem ctr)
        {
            ctr.Text = ToolLanguage.GetLanguage(Name, ctr.Name);
        }
        private void GetLanguage(Control ctr)
        {
            ctr.Text = ToolLanguage.GetLanguage(Name, ctr.Name);
        }
        private void GetLanguage(DataGridView dg)
        {
            string sCols = GetLanguage($"{dg.Name}_Cols");
            var sArr = sCols.SplitTrim(",");
            var cols = dg.Columns;
            for (int i = 0; i < cols.Count; i++)
            {
                var col = cols[i];
                col.HeaderText = sArr[i];
            }

        }


        private void LoadComboxItemsLanguage(ComboBox cbx, string sKey)
        {
            int iOldSelectIndex = cbx.SelectedIndex;
            cbx.Items.Clear();
            string sItems = GetLanguage(sKey);
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

        private void LoadUILanguage()
        {
            //指纹机、人脸机 调试程序
            Text = GetLanguage("FormCaption") + "  Ver " + Application.ProductVersion;
            GetLanguage(butSystem);//  系统参数                             
            GetLanguage(butTime);//日期时间                             
            GetLanguage(butDoor);//门参数                             
            GetLanguage(butAlarm);// 报警设置                             
            GetLanguage(butHoliday);//   节假日                             
            GetLanguage(ButTimeGroup);//     开门时段                             
            GetLanguage(butCard);//人事档案                             
            GetLanguage(butRecord);//  记录操作                             
            GetLanguage(butUploadSoftware);//          远程升级                             
            GetLanguage(butAdditionalData);//          人员附加数据   
            GetLanguage(LblConnType);
            LoadComboxItemsLanguage(cmbConnType, "ConnType_Items");
            GetLanguage(lblLocalPort);
            GetLanguage(butTCPServerBind);
            GetLanguage(ChkTLS12);
            GetLanguage(LblTCPClients);
            GetLanguage(gbDriveSN);
            GetLanguage(lblPassword);
            GetLanguage(lblLocalAddress);
            GetLanguage(butStopCommand);
            GetLanguage(butReadSN);
            GetLanguage(butWatch);
            GetLanguage(lblProcess);
            GetLanguage(tpIO);
            GetLanguage(butIOClear);
            GetLanguage(chkShowIO);
            GetLanguage(tpCommand);
            GetLanguage(butClearCommand);
            GetLanguage(chkIsUDPServer);//  服务器模式
            GetLanguage(LblUDPLocalPort);//  本地端口：
            GetLanguage(butUDPBind);//  绑定
            GetLanguage(lblSerialPort);//  串口号：
            GetLanguage(lblBaudrate);//  波特率：
            GetLanguage(butReloadSerialPort);//   刷新
            GetLanguage(gbSerialPort);//   串口
            GetLanguage(lblUDPPort);//   端口：
            GetLanguage(lblUDPAddr);//   IP：
            GetLanguage(LblSN);//   SN：
            GetLanguage(dgvIO);//标签,内容,类型,时间,远程信息,本地信息
            GetLanguage(dgvResult);//类型,内容,身份信息,远程信息,时间,耗时
            GetLanguage(btlToolLift);//电梯模块
            GetLanguage(butSearch);//搜索设备
            GetLanguage(tabDevice);//设备列表
            GetLanguage(dgDevice);//设备列表表格
            IniCommandClassNameList();

            TransactionTypeName = new string[255];
            TransactionTypeName[1] = GetLanguage("TransactionTypeName_1");//认证记录
            TransactionTypeName[2] = GetLanguage("TransactionTypeName_2");//"门操作记录";
            TransactionTypeName[3] = GetLanguage("TransactionTypeName_3");//"系统记录";
            TransactionTypeName[4] = GetLanguage("TransactionTypeName_4");//"体温记录";

            TransactionTypeName[0xA0] = GetLanguage("TransactionTypeName_0xA0");//"连接测试";
            TransactionTypeName[0x22] = GetLanguage("TransactionTypeName_0x22");//"保活包";

            foreach (var frm in NodeForms)
            {
                frm.LoadUILanguage();
            }
        }
        #endregion



        /// <summary>
        /// 窗体初始化
        /// </summary>
        public void IniForm()
        {
            if (_IsClosed) return;
            this.Enabled = true;
            mAllocator = ConnectorAllocator.GetAllocator();


            //导入 串口通讯库
            var defFactory = mAllocator.ConnectorFactory as DefaultConnectorFactory;
            defFactory.ConnectorFactoryDictionary.Add(ConnectorType.SerialPort, DoNetDrive.Connector.COM.SerialPortFactory.GetInstance());




            mObserver = new ConnectorObserverHandler();

            mAllocator.CommandCompleteEvent += mAllocator_CommandCompleteEvent;
            mAllocator.CommandErrorEvent += mAllocator_CommandErrorEvent;
            mAllocator.CommandProcessEvent += mAllocator_CommandProcessEvent;
            mAllocator.CommandTimeout += mAllocator_CommandTimeout;
            mAllocator.AuthenticationErrorEvent += MAllocator_AuthenticationErrorEvent;

            mAllocator.TransactionMessage += MAllocator_TransactionMessage;

            mAllocator.ConnectorConnectedEvent += mAllocator_ConnectorConnectedEvent;
            mAllocator.ConnectorClosedEvent += mAllocator_ConnectorClosedEvent;
            mAllocator.ConnectorErrorEvent += mAllocator_ConnectorErrorEvent;

            mAllocator.ClientOnline += MAllocator_ClientOnline;
            mAllocator.ClientOffline += MAllocator_ClientOffline;

            mObserver.DisposeRequestEvent += MObserver_DisposeRequestEvent;
            mObserver.DisposeResponseEvent += MObserver_DisposeResponseEvent;



            IniLstIO();
            InilstCommand();
            Task.Run((Action)ShowCommandProcesslog);

            butUDPBind_Click(null, null);
        }


        private void LoadSetting()
        {


            string sValue = ConfigurationManager.AppSettings["UDPPort"];
            if (!string.IsNullOrEmpty(sValue)) txtUDPLocalPort.Text = sValue;


            sValue = ConfigurationManager.AppSettings["UDPRemoteIP"];
            if (!string.IsNullOrEmpty(sValue)) txtUDPAddr.Text = sValue;

            sValue = ConfigurationManager.AppSettings["UDPRemotePort"];
            if (!string.IsNullOrEmpty(sValue)) txtUDPPort.Text = sValue;

            txtSN.Text = ConfigurationManager.AppSettings["DriveSN"];
            txtPassword.Text = ConfigurationManager.AppSettings["DrivePassword"];

            sValue = ConfigurationManager.AppSettings["ConnType"];
            if (!string.IsNullOrEmpty(sValue)) cmbConnType.SelectedIndex = int.Parse(sValue);


            sValue = ConfigurationManager.AppSettings["SerialPort"];
            if (!string.IsNullOrEmpty(sValue)) SelectComboxItem(cmbSerialPort, sValue);

            sValue = ConfigurationManager.AppSettings["SerialPortBaudrate"];
            if (!string.IsNullOrEmpty(sValue)) SelectComboxItem(cmbBaudrate, sValue);


            sValue = ConfigurationManager.AppSettings["LocalIP"];
            if (!string.IsNullOrEmpty(sValue)) SelectComboxItem(cmbLocalIP, sValue);

            sValue = ConfigurationManager.AppSettings["TCPServerPort"];
            if (!string.IsNullOrEmpty(sValue)) txtTCPServerPort.Text = sValue;

            sValue = ConfigurationManager.AppSettings["UseSSL"];
            if (!string.IsNullOrEmpty(sValue)) ChkTLS12.Checked = bool.Parse(sValue);

            sValue = ConfigurationManager.AppSettings["ToolLanguage"];
            if (!string.IsNullOrEmpty(sValue)) SelectComboxItem(cmbToolLanguage, sValue);


        }


        private void SaveSetting()
        {
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            var kv = config.AppSettings.Settings;
            SetConfigKey(config, "UDPPort", txtUDPLocalPort.Text);
            SetConfigKey(config, "UDPRemoteIP", txtUDPAddr.Text);
            SetConfigKey(config, "UDPRemotePort", txtUDPPort.Text);
            SetConfigKey(config, "DriveSN", txtSN.Text);
            SetConfigKey(config, "DrivePassword", txtPassword.Text);
            SetConfigKey(config, "ConnType", cmbConnType.SelectedIndex.ToString());
            SetConfigKey(config, "SerialPort", cmbSerialPort.Text);
            SetConfigKey(config, "SerialPortBaudrate", cmbBaudrate.Text);
            SetConfigKey(config, "LocalIP", cmbLocalIP.Text);
            SetConfigKey(config, "TCPServerPort", txtTCPServerPort.Text);
            SetConfigKey(config, "UseSSL", ChkTLS12.Checked.ToString());
            SetConfigKey(config, "ToolLanguage", cmbToolLanguage.Text);

            config.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("appSettings");
        }

        private void SetConfigKey(Configuration config, string key, string sValue)
        {
            var kv = config.AppSettings.Settings;
            if (kv[key] == null)
                kv.Add(key, sValue);
            else
                kv[key].Value = sValue;
        }

        private void SelectComboxItem(ComboBox cmb, string sValue)
        {
            foreach (string sItem in cmb.Items)
            {
                if (sItem == sValue)
                {
                    cmb.SelectedItem = sItem;
                    return;
                }
            }
        }



        #region 通道事件
        /// <summary>
        /// 客户端离线
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MAllocator_ClientOffline(object sender, ServerEventArgs e)
        {
            INConnector inc = sender as INConnector;
            inc.RemoveRequestHandle(typeof(ConnectorObserverHandler));
            inc.RemoveRequestHandle(typeof(Door8800RequestHandle));
            switch (inc.GetConnectorType())
            {
                case ConnectorType.TCPServerClient://tcp客户端断开连接
                    RemoteTCPClient(inc.GetKey());
                    break;
                default:
                    break;
            }
        }


        private class MyRequestHandle : Door8800RequestHandle
        {
            private string ConnectKey;
            private DateTime KeepaliveTime;
            internal INConnectorDetail ConnDetail;

            public MyRequestHandle(IByteBufferAllocator allocator,
                Func<string, byte, byte, AbstractTransaction> factory, INConnectorDetail iDetail)
                : base(allocator, factory)
            {
                ConnectKey = iDetail.GetKey();
                KeepaliveTime = DateTime.Now;
                ConnDetail = (INConnectorDetail)iDetail.Clone();
            }
            //接收
            public override void DisposeRequest(INConnector connector, IByteBuffer msg)
            {
                base.DisposeRequest(connector, msg);
                KeepaliveTime = DateTime.Now;
            }
            //超时
            internal bool CheckTimeout()
            {
                var iSec = (DateTime.Now - KeepaliveTime).TotalSeconds;
                return (iSec > 180);
            }
        }

        /// <summary>
        /// 客户端上线
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MAllocator_ClientOnline(object sender, ServerEventArgs e)
        {
            INConnector inc = sender as INConnector;
            inc.AddRequestHandle(mObserver);
            MyRequestHandle fC8800Request = null;
            switch (inc.GetConnectorType())
            {
                case ConnectorType.TCPServerClient://tcp 客户端已连接
                case ConnectorType.UDPClient://UDP客户端已连接

                    //inc.OpenForciblyConnect();
                    fC8800Request =
                       new MyRequestHandle(DotNetty.Buffers.UnpooledByteBufferAllocator.Default,
                       RequestHandleFactory, inc.GetConnectorDetail());
                    inc.RemoveRequestHandle(typeof(Door8800RequestHandle));//先删除，防止已存在就无法添加。
                    inc.AddRequestHandle(fC8800Request);

                    //AddUDPClient(inc.GetConnectorDetail());
                    break;

                default:
                    break;
            }

            switch (inc.GetConnectorType())
            {
                case ConnectorType.TCPServerClient://tcp 客户端已连接
                    TCPServerClientDetail clientDetail = inc.GetConnectorDetail() as TCPServerClientDetail;
                    AddTCPClient(inc.GetKey(), clientDetail.Remote);


                    mTCPClients.TryGetValue(inc.GetKey(), out var mTcp);
                    mTcp.Handle = fC8800Request;

                    break;

                default:
                    break;
            }
        }

        private void MObserver_DisposeResponseEvent(INConnector connector, string msg)
        {
            if (!mShowIOEvent) return;
            AddIOLog(connector.GetConnectorDetail(), GetLanguage("DisposeResponse"), msg);//"发送数据"
        }


        private void MObserver_DisposeRequestEvent(INConnector connector, string msg)
        {
            if (!mShowIOEvent) return;
            AddIOLog(connector.GetConnectorDetail(), GetLanguage("DisposeRequest"), msg);//"接收数据"
        }

        private void mAllocator_ConnectorErrorEvent(object sender, Core.Connector.INConnectorDetail connector)
        {
            switch (connector.GetTypeName())
            {
                case ConnectorType.UDPServer://UDP服务器
                    Invoke(() => UDPBindOver(false));
                    MsgErr(GetLanguage("UDPBindErr"));//"UDP绑定失败"
                    //                 "UDP绑定", "UDP绑定失败"
                    AddIOLog(connector, GetLanguage("UDPBind"), GetLanguage("UDPBindErr"));
                    break;
                case ConnectorType.TCPServer://UDP服务器
                    Invoke(() => TCPBindOver(false));
                    MsgErr(GetLanguage("TCPBindErr"));//"TCP Server绑定失败"
                    //                 "TCP Server绑定", "TCP Server绑定失败"
                    AddIOLog(connector, GetLanguage("TCPBind"), GetLanguage("TCPBindErr"));
                    break;
                default:
                    AddIOLog(connector, GetLanguage("Error"), GetLanguage("ConnectorError"));//"连接失败"
                    break;
            }
        }

        private void mAllocator_ConnectorClosedEvent(object sender, Core.Connector.INConnectorDetail connector)
        {
            switch (connector.GetTypeName())
            {
                case ConnectorType.UDPServer://UDP服务器
                    Invoke(() => UDPBindOver(false));
                    //                 "UDP绑定", "UDP绑定已关闭"
                    AddIOLog(connector, GetLanguage("UDPBind"), GetLanguage("UDPConnectorClosed"));
                    break;
                case ConnectorType.TCPServer:
                    Invoke(() => TCPBindOver(false));
                    //"TCP Server 绑定已关闭"
                    AddIOLog(connector, GetLanguage("TCPBind"), GetLanguage("TCPConnectorClosed"));
                    break;
                case ConnectorType.TCPServerClient://tcp客户端断开连接
                    RemoteTCPClient(connector.GetKey());
                    break;
                default:
                    // "关闭", "连接通道已关闭"
                    AddIOLog(connector, GetLanguage("Closed"), GetLanguage("ConnectorClosed"));
                    break;
            }
        }

        private void mAllocator_ConnectorConnectedEvent(object sender, Core.Connector.INConnectorDetail connector)
        {
            switch (connector.GetTypeName())
            {
                case ConnectorType.TCPServer://TCP Server 连接绑定成功
                    Invoke(() => TCPBindOver(true));
                    //"TCPServer绑定", "TCPServer绑定成功"
                    AddIOLog(connector, GetLanguage("TCPBind"), GetLanguage("TCPConnectorConnected"));
                    break;
                case ConnectorType.UDPServer://UDP服务器
                    Invoke(() => UDPBindOver(true));
                    //"UDP绑定", "UDP绑定成功"
                    AddIOLog(connector, GetLanguage("UDPBind"), GetLanguage("UDPConnectorConnected"));

                    break;
                default:
                    var AbstractConnector = (sender as AbstractConnector);
                    if (AbstractConnector != null)
                    {
                        AbstractConnector.CommandSendIntervalTimeMS = 30;

                        AbstractConnector.AddRequestHandle(mObserver);

                        //加入消息事件处理器

                        //Door8800RequestHandle fC8800Request =
                        //new Door8800RequestHandle(DotNetty.Buffers.UnpooledByteBufferAllocator.Default, RequestHandleFactory);
                        //AbstractConnector.RemoveRequestHandle(typeof(Door8800RequestHandle));//先删除，防止已存在就无法添加。
                        //AbstractConnector.AddRequestHandle(fC8800Request);

                    }



                    // "成功", "通道连接成功"
                    AddIOLog(connector, GetLanguage("Connected"), GetLanguage("ConnectorConnected"));
                    break;
            }
        }

        #endregion

        #region 命令事件
        private void MAllocator_AuthenticationErrorEvent(object sender, CommandEventArgs e)
        {
            AddCmdLog(e, GetLanguage("AuthenticationError"));//"通讯密码错误"
        }

        private void mAllocator_CommandTimeout(object sender, CommandEventArgs e)
        {
            //if (e.Command.GetType().FullName == typeof(Door8800.SystemParameter.SearchControltor.SearchControltor).FullName)
            //{
            //    AddCmdLog(e, "搜索完毕");
            //    return;
            //}
            AddCmdLog(e, GetLanguage("CommandTimeoutEvent"));//"命令超时");
        }


        private void mAllocator_CommandErrorEvent(object sender, CommandEventArgs e)
        {
            if (e.Command.GetStatus().IsCanceled)
            {
                AddCmdLog(e, GetLanguage("CommandStopEvent"));// "命令已停止");
            }
            else
                AddCmdLog(e, GetLanguage("CommandErrorEvent"));//"命令错误");

        }


        private const string Command_ReadSN = "DoNetDrive.Protocol.Door.Door8800.SystemParameter.SN.ReadSN";
        private const string Command_WriteSN = "DoNetDrive.Protocol.Door.Door8800.SystemParameter.SN.WriteSN";
        private const string Command_WriteSN_Broadcast = "DoNetDrive.Protocol.Door.Door8800.SystemParameter.SN.WriteSN_Broadcast";
        private const string Command_ReadConnectPassword = "DoNetDrive.Protocol.Door.Door8800.SystemParameter.ConnectPassword.ReadConnectPassword";
        private const string Command_WriteConnectPassword = "DoNetDrive.Protocol.Door.Door8800.SystemParameter.ConnectPassword.WriteConnectPassword";
        private const string Command_ResetConnectPassword = "DoNetDrive.Protocol.Door.Door8800.SystemParameter.ConnectPassword.ResetConnectPassword";

        private void mAllocator_CommandCompleteEvent(object sender, CommandEventArgs e)
        {
            /*
            if (e.Command.GetType().FullName == typeof(Door8800.SystemParameter.SearchControltor.SearchControltor).FullName)
            {
                return;
            }
            */
            mAllocator_CommandProcessEvent(sender, e);
            AddCmdLog(e, GetLanguage("CommandCompleteEvent"));//"命令完成");
            string cName = e.Command.GetType().FullName;
            /*   */
            var cmddtl = e.CommandDetail;
            switch (cName)
            {
                case Command_ReadSN://读SN
                    Protocol.Door.Door8800.SystemParameter.SN.SN_Result sn = e.Command.getResult() as Protocol.Door.Door8800.SystemParameter.SN.SN_Result;
                    Invoke(() => txtSN.Text = sn.SNBuf.GetString());
                    if (cmddtl.UserData != null)
                    {
                        string tmpStr = cmddtl.UserData as string;
                        if (tmpStr != null && tmpStr == "AutoReadSN")
                        {
                            DoNetDrive.Protocol.OnlineAccess.OnlineAccessCommandDetail ocd = cmddtl as DoNetDrive.Protocol.OnlineAccess.OnlineAccessCommandDetail;
                            ocd.SN = sn.SNBuf.GetString();
                            ReadConnectPassword cmd = new ReadConnectPassword(cmddtl);
                            AddCommand(cmd);
                        }

                    }
                    break;

                case Command_ReadConnectPassword://读通讯密码
                    Protocol.Door.Door8800.SystemParameter.ConnectPassword.Password_Result pwd = e.Command.getResult() as Protocol.Door.Door8800.SystemParameter.ConnectPassword.Password_Result;
                    Invoke(() => txtPassword.Text = pwd.Password);
                    break;
                case Command_WriteConnectPassword://写通讯密码
                    Protocol.Door.Door8800.SystemParameter.ConnectPassword.Password_Parameter pwdPar = e.Command.Parameter as Protocol.Door.Door8800.SystemParameter.ConnectPassword.Password_Parameter;
                    Invoke(() => txtPassword.Text = pwdPar.Password);
                    break;
                case Command_ResetConnectPassword://复位通讯密码
                    Invoke(() => txtPassword.Text = "FFFFFFFF");
                    break;
                default:
                    break;
            }




        }
        #endregion

        #region 命令进度事件
        /// <summary>
        /// 命令日志
        /// </summary>
        private string mCommandProcessLog;
        /// <summary>
        /// 显示命令进度日志
        /// </summary>
        private void ShowCommandProcesslog()
        {
            do
            {
                if (_IsClosed) break;

                Invoke(() =>
                {
                    if (_IsClosed) return;
                    txtProcess.Text = mCommandProcessLog;

                });
                Sleep(300);
                if (_IsClosed) break;
            } while (!_IsClosed);
            // Console.WriteLine("ShowCommandProcesslog 已退出");
        }


        private void mAllocator_CommandProcessEvent(object sender, CommandEventArgs e)
        {
            var cmd = e.Command;
            string sName = cmd.GetType().FullName;
            if (mCommandClasss.ContainsKey(sName)) sName = mCommandClasss[sName];

            double time = 0;
            var dtl = e.CommandDetail;
            if (dtl.BeginTime != DateTime.MinValue && dtl.EndTime != DateTime.MinValue)
            {
                time = (dtl.EndTime - dtl.BeginTime).TotalMilliseconds;
            }
            else
            {
                if (dtl.BeginTime != DateTime.MinValue)
                    time = (DateTime.Now - e.CommandDetail.BeginTime).TotalMilliseconds;
            }

            //{sName} 已耗时：{time:0},进度：{cmd.getProcessStep()} / {cmd.getProcessMax()}
            string sLog = GetLanguage("CommandProcessEvent", sName, time, cmd.getProcessStep(), cmd.getProcessMax());
            mCommandProcessLog = sLog;
        }



        #endregion

        #region 获取通道描述信息
        private string GetConnectorDetail(INConnector conn)
        {
            return GetConnectorDetail(conn.GetConnectorDetail());
        }
        private string GetConnectorDetail(INConnectorDetail conn)
        {
            string Local, Remote, cType;
            GetConnectorDetail(conn, out cType, out Local, out Remote);
            // $"通道类型：{cType} 本地IP：{Local} ,远端IP：{Remote}";
            string ret = GetLanguage("GetConnectorDetail0", cType, Local, Remote);
            switch (conn.GetTypeName())
            {
                case ConnectorType.UDPServer:
                    // $"通道类型：{cType}  本地绑定IP：{Local}";
                    ret = GetLanguage("GetConnectorDetail1", cType, Local);
                    break;
                case ConnectorType.TCPServer:
                    // $"通道类型：{cType}  本地绑定IP：{Local}";
                    ret = GetLanguage("GetConnectorDetail1", cType, Local);
                    break;
                case ConnectorType.SerialPort:
                    //$"通道类型：{cType} {Local}";
                    ret = GetLanguage("GetConnectorDetail2", cType, Local);

                    break;
                default:
                    //"通道类型：{cType} {Local}";
                    ret = GetLanguage("GetConnectorDetail3", cType, Local);
                    break;
            }

            return $"{ret}:{DateTime.Now.ToTimeffff()}";

        }

        /// <summary>
        /// 获取连接通道详情
        /// </summary>
        /// <param name="conn">连接通道描述符</param>
        /// <param name="Local">返回描述本地信息</param>
        /// <param name="Remote">返回描述远程信息</param>
        /// <returns></returns>
        private void GetConnectorDetail(INConnectorDetail conn, out string cType, out string Local, out string Remote)
        {
            Local = string.Empty;
            Remote = string.Empty;
            cType = string.Empty;

            var oConn = mAllocator.GetConnector(conn);
            if (oConn == null)
            {
                cType = conn.GetTypeName();
                Local = conn.GetKey();
                return;
            }

            IPDetail local = oConn.LocalAddress();
            conn = oConn.GetConnectorDetail();

            switch (conn.GetTypeName())
            {

                case ConnectorType.TCPServerClient:
                    cType = GetLanguage("GetConnectorDetail_TCPServerClient");//TCP客户端节点
                    var tcpclientOnly = conn as TCPServerClientDetail;
                    Local = $"{local.ToString()}";
                    Remote = $"{tcpclientOnly.Remote.ToString()}";
                    break;
                case ConnectorType.UDPClient:
                    cType = GetLanguage("GetConnectorDetail_UDPClient");//UDP客户端";
                    var udpOnly = conn as Core.Connector.TCPClient.TCPClientDetail;
                    Local = $"{local.ToString()}";
                    Remote = $"{udpOnly.Addr}:{udpOnly.Port}";
                    break;
                case ConnectorType.UDPServer:
                    cType = GetLanguage("GetConnectorDetail_UDPServer");//UDP服务器";
                    Local = $"{local.ToString()}";
                    break;
                case ConnectorType.TCPServer:
                    cType = GetLanguage("GetConnectorDetail_TCPServer");//TCP服务器";
                    Local = $"{local.ToString()}";
                    break;
                case ConnectorType.SerialPort:
                    cType = GetLanguage("GetConnectorDetail_SerialPort");//串口";
                    var com = conn as SerialPortDetail;
                    Local = $"COM{local.Port}:{com.Baudrate}";
                    break;
                default:
                    cType = conn.GetTypeName();
                    Local = $"{conn.GetKey()}";
                    break;
            }
        }
        #endregion

        #region 窗体关闭
        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            _IsClosed = true;
            SaveSetting();
            Sleep(500);
            foreach (var frm in NodeForms)
            {
                frm.Dispose();
            }
            NodeForms.Clear();
        }

        private void frmMain_FormClosed(object sender, FormClosedEventArgs e)
        {
            //释放资源
            mAllocator?.Dispose();
            Sleep(500);
            //Sleep(50000);
        }
        #endregion


        #region 公共函数
        /// <summary>
        /// 显示日志
        /// </summary>
        /// <param name="s">需要显示的日志</param>
        public void AddLog(string s)
        {
            if (_IsClosed) return;
            AddCmdLog(null, s);
        }



        /// <summary>
        /// 显示日志
        /// </summary>
        /// <param name="s">需要显示的日志</param>
        public void AddLog(StringBuilder s)
        {
            if (_IsClosed) return;

            AddLog(s.ToString());
        }

        /// <summary>
        /// 将命令加入到分配器开始执行
        /// </summary>
        /// <param name="cmd"></param>
        public void AddCommand(INCommand cmd)
        {
            if (cmd.CommandDetail == null) return;
            try
            {
                mAllocator.AddCommand(cmd);
            }
            catch (Exception)
            {

                AddCmdLog(null, "连接无效");
            }

        }

        public async Task AddCommandAsync(INCommand cmd)
        {
            if (cmd.CommandDetail == null) return;
            try
            {
                await mAllocator.AddCommandAsync(cmd);
            }
            catch (Exception ex)
            {
                var runtimeCmd = cmd as INCommandRuntime;
                AddCmdLog(runtimeCmd.GetEventArgs(), ex.Message);

            }


        }


        /// <summary>
        /// 获取一个命令详情，已经装配好通讯目标的所有信息
        /// </summary>
        /// <returns>命令详情</returns>
        public INCommandDetail GetCommandDetail(bool showErr=true)
        {
            if (_IsClosed) return null;
            var connectType = CommandDetailFactory.ConnectType.TCPClient;
            string addr = string.Empty, sn, password;
            int port = 0;

            sn = txtSN.Text;
            if (string.IsNullOrEmpty(sn))
            {
                sn = "0000000000000000";
            }
            if (sn.GetBytes().Length != 16)
            {
                sn = "0000000000000000";
            }

            password = txtPassword.Text;
            if (!password.IsHex())
            {
                password = Door8800Command.NULLPassword;
            }
            if (password.Length != 8)
            {
                password = Door8800Command.NULLPassword;
            }

            switch (cmbConnType.SelectedIndex)//串口,TCP客户端,UDP,TCP服务器
            {
                case 0://串口
                    if (cmbSerialPort.SelectedIndex == -1)
                    {
                        MsgTip(GetLanguage("GetCommandDetail0"));//"请先选择一个串口号！");
                        return null;
                    }
                    addr = string.Empty;
                    port = cmbSerialPort.Text.Substring(3).ToInt32();
                    int iBaudrate = int.Parse(cmbBaudrate.Text);
                    return GetCommandDetail(new SerialPortDetail((byte)port, iBaudrate)
                        , sn, password);

                case 1://UDP 
                    if (!mUDPIsBind)
                    {
                        MsgErr(GetLanguage("GetCommandDetail1"));//"请先绑定UDP端口");
                        return null;
                    }
                    connectType = CommandDetailFactory.ConnectType.UDPClient;
                    addr = txtUDPAddr.Text;
                    if (!int.TryParse(txtUDPPort.Text, out port))
                    {
                        port = 8000;
                    }
                    break;
                case 2://tcp server 模式，需要寻找客户端
                    if (!mTCPServerBind)
                    {
                        MsgErr(GetLanguage("GetCommandDetail2"));//"请先绑定 TCP Server 端口");
                        return null;
                    }
                    connectType = CommandDetailFactory.ConnectType.TCPServerClient;
                    if (cmbTCPClientList.SelectedIndex == -1)
                    {
                        MsgErr(GetLanguage("GetCommandDetail3"));//"请先选择一个 TCP 客户端!");
                        return null;
                    }
                    var myClient = cmbTCPClientList.SelectedItem as MyTCPServerClientDetail;
                    addr = myClient.GetKey();
                    port = 0;
                    break;
                default:
                    return null;
            }


            if (port > 65535) port = 8000;


            return GetCommandDetail(connectType, sn, password, addr, port);

        }


        /// <summary>
        /// 获取一个命令详情，已经装配好通讯目标的所有信息
        /// </summary>
        /// <returns>命令详情</returns>
        public INCommandDetail GetCommandDetail(CommandDetailFactory.ConnectType connectType,
            string sSN, string sPassword, string sRemoteIP, int iRemotePort)
        {
            if (_IsClosed) return null;
            var protocolType = CommandDetailFactory.ControllerType.A33_Face;


            var cmdDtl = CommandDetailFactory.CreateDetail(connectType, sRemoteIP, iRemotePort,
                protocolType, sSN, sPassword);

            if (connectType == CommandDetailFactory.ConnectType.UDPClient)
            {
                var dtl = cmdDtl.Connector as Core.Connector.TCPClient.TCPClientDetail;
                dtl.LocalAddr = mServerIP;
                dtl.LocalPort = mServerPort;
            }


            cmdDtl.Timeout = 600;
            cmdDtl.RestartCount = 3;
            return cmdDtl;

        }

        public INCommandDetail GetCommandDetail(INConnectorDetail connect,
    string sSN, string sPassword)
        {
            if (_IsClosed) return null;
            var protocolType = CommandDetailFactory.ControllerType.A33_Face;


            var cmdDtl = CommandDetailFactory.CreateDetail(connect,
                protocolType, sSN, sPassword);

            cmdDtl.Timeout = 1000;
            cmdDtl.RestartCount = 3;
            return cmdDtl;

        }





        /// <summary>
        /// 获取UDP连接详情
        /// </summary>
        /// <param name="sSN"></param>
        /// <param name="sPassword"></param>
        /// <param name="sKey"></param>
        /// <returns></returns>
        private INCommandDetail GetUDPCommandDetail(string sSN, string sPassword, string sRemote, int iPort)
        {
            if (_IsClosed) return null;
            var connectType = CommandDetailFactory.ConnectType.UDPClient;
            return GetCommandDetail(connectType, sSN, sPassword, sRemote, iPort);
        }

        /// <summary>
        /// 获取TCP连接详情
        /// </summary>
        /// <param name="sSN"></param>
        /// <param name="sPassword"></param>
        /// <param name="sKey"></param>
        /// <returns></returns>
        private INCommandDetail GetTCPCommandDetail(string sSN, string sPassword, string sKey)
        {
            if (_IsClosed) return null;
            var connectType = CommandDetailFactory.ConnectType.TCPServerClient;
            return GetCommandDetail(connectType, sSN, sPassword, sKey, 0);
        }
        #endregion


        #region 通讯日志
        private bool mShowIOEvent = false;
        private void chkShowIO_CheckedChanged(object sender, EventArgs e)
        {
            mShowIOEvent = chkShowIO.Checked;
        }


        /// <summary>
        /// 初始化通讯日志列表
        /// </summary>
        private void IniLstIO()
        {
            mIOMessageList = new ConcurrentQueue<IOMessage>();
            Task.Run(() =>
            {
                do
                {
                    if (_IsClosed) break;
                    if (!mIOMessageList.IsEmpty)
                    {
                        Invoke(() =>
                        {
                            do
                            {
                                IOMessage oItem;
                                if (mIOMessageList.TryDequeue(out oItem))
                                {
                                    dgvIO.Rows.Insert(0, oItem.Title, oItem.Content, oItem.Type, oItem.Time, oItem.Remote, oItem.Local);
                                }
                            } while (!mIOMessageList.IsEmpty);

                        });

                    }
                    Sleep(1000);
                    if (_IsClosed) break;
                } while (!_IsClosed);

                //Console.WriteLine("IniLstIO 刷新线程 已退出");
            });
        }

        private void Sleep(int time)
        {
            System.Threading.Thread.Sleep(time);
        }

        private ConcurrentQueue<IOMessage> mIOMessageList;

        /// <summary>
        /// 添加一个通讯日志
        /// </summary>
        /// <param name="connDetail"></param>
        /// <param name="sTag">标签</param>
        /// <param name="txt">内容</param>
        public void AddIOLog(INConnectorDetail connDetail, string sTag, string txt)
        {
            if (!mShowIOEvent) return;

            string Local, Remote, cType;
            GetConnectorDetail(connDetail, out cType, out Local, out Remote);
            IOMessage iOMessage = new IOMessage();
            iOMessage.Title = sTag;
            if (txt.Length > 50)
            {
                txt = txt.Substring(0, 50) + "\r\n" + txt.Substring(50);
            }

            iOMessage.Content = txt;
            iOMessage.Type = cType;
            iOMessage.Remote = Remote;
            iOMessage.Local = Local;
            iOMessage.Time = DateTime.Now.ToTimeffff();

            mIOMessageList.Enqueue(iOMessage);
        }


        /// <summary>
        /// 清空所有通讯日志
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void butClear_Click(object sender, EventArgs e)
        {
            dgvIO.Rows.Clear();
            //lstIO.Items.Clear();
        }
        #endregion

        #region 功能菜单

        private void butSystem_Click(object sender, EventArgs e)
        {
            frmSystem frm = frmSystem.GetForm(this);
            frm.Show();
            if (frm.WindowState == FormWindowState.Minimized)
                frm.WindowState = FormWindowState.Normal;
            frm.Activate();
            ShowFrm(frm);
        }

        private void butTime_Click(object sender, EventArgs e)
        {
            frmTime frm = frmTime.GetForm(this);
            frm.Show();
            if (frm.WindowState == FormWindowState.Minimized)
                frm.WindowState = FormWindowState.Normal;
            frm.Activate();
            ShowFrm(frm);
        }

        private void butDoor_Click(object sender, EventArgs e)
        {
            frmDoor frm = frmDoor.GetForm(this);
            frm.Show();
            if (frm.WindowState == FormWindowState.Minimized)
                frm.WindowState = FormWindowState.Normal;
            frm.Activate();
            ShowFrm(frm);
        }

        private void butHoliday_Click(object sender, EventArgs e)
        {

            frmHoliday frm = frmHoliday.GetForm(this);
            frm.Show();
            if (frm.WindowState == FormWindowState.Minimized)
                frm.WindowState = FormWindowState.Normal;
            frm.Activate();
            ShowFrm(frm);
        }

        private void ButPassword_Click(object sender, EventArgs e)
        {
            //frmPassword frm = frmPassword.GetForm(this);
            //frm.Show();
            //if (frm.WindowState == FormWindowState.Minimized)
            //    frm.WindowState = FormWindowState.Normal;
            //frm.Activate();
            //ShowFrm(frm);
        }

        private void ButTimeGroup_Click(object sender, EventArgs e)
        {
            frmTimeGroup frm = frmTimeGroup.GetForm(this);
            frm.Show();
            if (frm.WindowState == FormWindowState.Minimized)
                frm.WindowState = FormWindowState.Normal;
            frm.Activate();
            ShowFrm(frm);
        }

        private void butCard_Click(object sender, EventArgs e)
        {
            frmPerson frm = frmPerson.GetForm(this);
            frm.Show();
            if (frm.WindowState == FormWindowState.Minimized)
                frm.WindowState = FormWindowState.Normal;
            frm.Activate();
            ShowFrm(frm);

        }


        private void ButAlarm_Click(object sender, EventArgs e)
        {
            frmAlarm frm = frmAlarm.GetForm(this);
            frm.Show();
            if (frm.WindowState == FormWindowState.Minimized)
                frm.WindowState = FormWindowState.Normal;
            frm.Activate();
            ShowFrm(frm);
        }
        /// <summary>
        /// 显示窗口在侧边栏
        /// </summary>
        /// <param name="chi"></param>
        private void ShowFrm(Form chi)
        {
            Screen scr = Screen.PrimaryScreen;

            foreach (Screen ss in Screen.AllScreens)
            {
                var rc = ss.Bounds;
                if (rc.Left < this.Left && (rc.Left + rc.Width) > this.Left)
                {
                    if (rc.Top < this.Top && (rc.Top + rc.Bottom) > this.Top)
                    {
                        scr = ss;
                        break;
                    }
                }

            }

            var scrRc = scr.Bounds;
            int iLeft = (scrRc.Width - (Width + chi.Width)) / 2;
            int iTop = (scrRc.Height - (Height > chi.Height ? Height : chi.Height)) / 2;

            this.Left = scrRc.Left + iLeft;
            this.Top = scrRc.Top + iTop;

            chi.Left = scrRc.Left + iLeft + this.Width;
            chi.Top = scrRc.Top + iTop;
            if (this.Left < 0) this.Left = 0;
            if ((chi.Left + chi.Width) > scrRc.Width)
            {
                chi.Left = scrRc.Width - chi.Width;
            }

        }

        private void butRecord_Click(object sender, EventArgs e)
        {
            frmRecord frm = frmRecord.GetForm(this);
            frm.Show();
            if (frm.WindowState == FormWindowState.Minimized)
                frm.WindowState = FormWindowState.Normal;
            frm.Activate();
            ShowFrm(frm);
        }

        private void butUploadSoftware_Click(object sender, EventArgs e)
        {

        }

        private void ButAdditionalData_Click(object sender, EventArgs e)
        {
            frmAdditionalData frm = frmAdditionalData.GetForm(this);
            frm.Show();
            if (frm.WindowState == FormWindowState.Minimized)
                frm.WindowState = FormWindowState.Normal;
            frm.Activate();
            ShowFrm(frm);
        }
        #endregion

        /// <summary>
        /// 保存命令类型的功能名称
        /// </summary>
        private static Dictionary<string, string> mCommandClasss;

        /// <summary>
        /// 协议类型
        /// </summary>
        CommandDetailFactory.ControllerType[] mProtocolTypeTable = new CommandDetailFactory.ControllerType[3];
        /// <summary>
        /// 初始化命令类型的功能名称
        /// </summary>
        private void IniCommandClassNameList()
        {
            mCommandClasss = new Dictionary<string, string>();

            AddCommandClassNameList(typeof(SystemParameter.WriteClientWorkMode));// "写入客户端网络模式 
            AddCommandClassNameList(typeof(SystemParameter.ReadClientWorkMode));// "读取客户端网络模式 
            AddCommandClassNameList(typeof(SystemParameter.ReadClientStatus));// "获取设备客户端连接状态 
            AddCommandClassNameList(typeof(SystemParameter.RequireConnectServer));// "命令设备立刻重新连接服务器 
            AddCommandClassNameList(typeof(SystemParameter.RequireSendKeepalivePacket));// "要求设备立刻发送一次保活包 
            AddCommandClassNameList(typeof(SystemParameter.WriteNetworkServerDetail));// "写入网络服务器参数 
            AddCommandClassNameList(typeof(SystemParameter.ReadNetworkServerDetail));// "读取网络服务器参数 

            AddCommandClassNameList(typeof(SystemParameter.ReadFaceLEDMode));// "读取补光灯模式 
            AddCommandClassNameList(typeof(SystemParameter.WriteFaceLEDMode));// "写入补光灯模式 
            AddCommandClassNameList(typeof(SystemParameter.ReadFaceMouthmufflePar));// "读取口罩识别开关 
            AddCommandClassNameList(typeof(SystemParameter.WriteFaceMouthmufflePar));// "写入口罩识别开关 

            AddCommandClassNameList(typeof(SystemParameter.ReadFaceBodyTemperaturePar));// "读取体温检测及格式 
            AddCommandClassNameList(typeof(SystemParameter.WriteFaceBodyTemperaturePar));// "写入体温检测及格式 

            AddCommandClassNameList(typeof(SystemParameter.ReadFaceBodyTemperatureAlarmPar));// "读取体温报警阈值 
            AddCommandClassNameList(typeof(SystemParameter.WriteFaceBodyTemperatureAlarmPar));// "写入体温报警阈值 
            AddCommandClassNameList(typeof(Person.RegisterIdentificationData));// "注册识别信息 

            AddCommandClassNameList(typeof(SystemParameter.WriteShortMessage));// "写入合法验证后显示的短消息 
            AddCommandClassNameList(typeof(SystemParameter.ReadShortMessage));// "读取合法验证后显示的短消息 

            AddCommandClassNameList(typeof(SystemParameter.WriteFaceBodyTemperatureShowPar));// "写入人脸机体温数值显示开关 
            AddCommandClassNameList(typeof(SystemParameter.ReadFaceBodyTemperatureShowPar));// "读取人脸机体温数值显示开关 

            AddCommandClassNameList(typeof(Person.AddPeosonAndImage));// "添加人员及照片 
            AddCommandClassNameList(typeof(Transaction.ReadTransactionAndImageDatabase));// "读取记录、体温、照片 
            AddCommandClassNameList(typeof(ReadSN));// "读取SN 
            AddCommandClassNameList(typeof(WriteSN));// "写SN 
            AddCommandClassNameList(typeof(WriteSN_Broadcast));// "广播写SN 

            AddCommandClassNameList(typeof(ReadConnectPassword));// "获取通讯密码 
            AddCommandClassNameList(typeof(WriteConnectPassword));// "设置通讯密码 
            AddCommandClassNameList(typeof(ResetConnectPassword));// "重置通讯密码 

            AddCommandClassNameList(typeof(Protocol.Door.Door8800.SystemParameter.TCPSetting.ReadTCPSetting));// "读取TCP参数 
            AddCommandClassNameList(typeof(Protocol.Door.Door8800.SystemParameter.TCPSetting.WriteTCPSetting));// "写入TCP参数 
            AddCommandClassNameList(typeof(Protocol.Door.Door8800.SystemParameter.KeepAliveInterval.ReadKeepAliveInterval));// "读取保活间隔时间 
            AddCommandClassNameList(typeof(Protocol.Door.Door8800.SystemParameter.KeepAliveInterval.WriteKeepAliveInterval));// "写入保活间隔时间 

            AddCommandClassNameList(typeof(SystemParameter.Version.ReadVersion));// "读取设备版本号 
            AddCommandClassNameList(typeof(SystemParameter.SystemStatus.ReadSystemRunStatus));// "获取设备运行信息 
            AddCommandClassNameList(typeof(SystemParameter.RecordMode.ReadRecordMode));// "获取记录存储方式 
            AddCommandClassNameList(typeof(SystemParameter.RecordMode.WriteRecordMode));// "设置记录存储方式 
            AddCommandClassNameList(typeof(SystemParameter.Watch.BeginWatch));// "开启数据监控 
            AddCommandClassNameList(typeof(SystemParameter.Watch.CloseWatch));// "关闭数据监控 
            AddCommandClassNameList(typeof(SystemParameter.Watch.ReadWatchState));// "读取实时监控状态 
            AddCommandClassNameList(typeof(SystemParameter.SystemStatus.ReadSystemStatus));// "读取设备状态信息 
            AddCommandClassNameList(typeof(DoNetDrive.Protocol.Door.Door8800.SystemParameter.Controller.FormatController));// "初始化设备 
            AddCommandClassNameList(typeof(DoNetDrive.Protocol.Fingerprint.SystemParameter.RequireRestart));// "重启 
            AddCommandClassNameList(typeof(Protocol.Door.Door8800.SystemParameter.SearchControltor.SearchControltor));// "搜索控制器 
            AddCommandClassNameList(typeof(Protocol.Door.Door8800.SystemParameter.SearchControltor.WriteControltorNetCode));// "根据SN设置网络标识 
            AddCommandClassNameList(typeof(SystemParameter.DataEncryptionSwitch.ReadDataEncryptionSwitch));// "读取数据包加密开关 
            AddCommandClassNameList(typeof(SystemParameter.DataEncryptionSwitch.WriteDataEncryptionSwitch));// "设置数据包加密开关 
            AddCommandClassNameList(typeof(SystemParameter.ReadLocalIdentity));// "读取本机身份 
            AddCommandClassNameList(typeof(SystemParameter.WriteLocalIdentity));// "设置本机身份 
            AddCommandClassNameList(typeof(SystemParameter.WiegandOutput.ReadWiegandOutput));// "读取韦根输出 
            AddCommandClassNameList(typeof(SystemParameter.WiegandOutput.WriteWiegandOutput));// "设置韦根输出 
            AddCommandClassNameList(typeof(SystemParameter.ReadComparisonThreshold));// "读取人脸、指纹比对阈值 
            AddCommandClassNameList(typeof(SystemParameter.WriteComparisonThreshold));// "设置人脸、指纹比对阈值 
            AddCommandClassNameList(typeof(SystemParameter.ScreenDisplayContent.ReadScreenDisplayContent));// "读取屏幕显示内容 
            AddCommandClassNameList(typeof(SystemParameter.ScreenDisplayContent.WriteScreenDisplayContent));// "设置屏幕显示内容 
            AddCommandClassNameList(typeof(SystemParameter.ManageMenuPassword.ReadManageMenuPassword));// "读取管理菜单密码 
            AddCommandClassNameList(typeof(SystemParameter.ManageMenuPassword.WriteManageMenuPassword));// "设置管理菜单密码 
            AddCommandClassNameList(typeof(SystemParameter.OEM.ReadOEM));// "读取OEM信息 
            AddCommandClassNameList(typeof(SystemParameter.OEM.WriteOEM));// "设置OEM信息 


            AddCommandClassNameList(typeof(Protocol.Door.Door8800.Time.ReadTime));// "读系统时间 
            AddCommandClassNameList(typeof(Protocol.Door.Door8800.Time.WriteTime));// "写系统时间 
            AddCommandClassNameList(typeof(Protocol.Door.Door8800.Time.WriteCustomTime));// "写系统时间 
            AddCommandClassNameList(typeof(Protocol.Door.Door8800.Time.WriteTimeBroadcast));// "写设备时间_广播命令 

            AddCommandClassNameList(typeof(Protocol.Door.Door8800.Holiday.ReadHolidayDetail));// "从控制板中读取节假日存储详情 
            AddCommandClassNameList(typeof(Protocol.Door.Door8800.Holiday.ClearHoliday));// "清空控制器中的所有节假日 
            AddCommandClassNameList(typeof(Protocol.Door.Door8800.Holiday.ReadAllHoliday));// "读取控制板中已存储的所有节假日 
            AddCommandClassNameList(typeof(Protocol.Door.Door8800.Holiday.AddHoliday));// "添加节假日到控制版 
            AddCommandClassNameList(typeof(Protocol.Door.Door8800.Holiday.DeleteHoliday));// "从控制器删除节假日 


            AddCommandClassNameList(typeof(Protocol.Door.Door8800.TimeGroup.ClearTimeGroup));// "清空所有开门时段 
            AddCommandClassNameList(typeof(Protocol.Door.Door8800.TimeGroup.ReadTimeGroup));// "读取所有开门时段 
            AddCommandClassNameList(typeof(Protocol.Door.Door8800.TimeGroup.AddTimeGroup));// "添加开门时段 

            AddCommandClassNameList(typeof(Transaction.ReadTransactionDatabaseDetail));// "读取控制器中的卡片数据库信息 
            AddCommandClassNameList(typeof(Transaction.ClearTransactionDatabase));// "清空指定类型的记录数据库 
            AddCommandClassNameList(typeof(Transaction.ReadTransactionDatabaseByIndex));// "按指定序号读记录 
            AddCommandClassNameList(typeof(Transaction.ReadTransactionDatabase));// "读取新记录 
            AddCommandClassNameList(typeof(Transaction.WriteTransactionDatabaseReadIndex));// "更新记录指针 
            AddCommandClassNameList(typeof(Transaction.WriteTransactionDatabaseWriteIndex));// "修改指定记录数据库的写索引 

            AddCommandClassNameList(typeof(Alarm.SendFireAlarm.WriteSendFireAlarm));// "通知设备触发消防报警 
            AddCommandClassNameList(typeof(Alarm.BlacklistAlarm.ReadBlacklistAlarm));// "读取黑名单报警 
            AddCommandClassNameList(typeof(Alarm.BlacklistAlarm.WriteBlacklistAlarm));// "设置黑名单报警 
            AddCommandClassNameList(typeof(Alarm.AntiDisassemblyAlarm.ReadAntiDisassemblyAlarm));// "读取防拆报警功能 
            AddCommandClassNameList(typeof(Alarm.AntiDisassemblyAlarm.WriteAntiDisassemblyAlarm));// "设置防拆报警功能 
            AddCommandClassNameList(typeof(Alarm.IllegalVerificationAlarm.ReadIllegalVerificationAlarm));// "读取非法验证报警 
            AddCommandClassNameList(typeof(Alarm.IllegalVerificationAlarm.WriteIllegalVerificationAlarm));// "设置非法验证报警 
            AddCommandClassNameList(typeof(Alarm.AlarmPassword.ReadAlarmPassword));// "读取胁迫报警密码 
            AddCommandClassNameList(typeof(Alarm.AlarmPassword.WriteAlarmPassword));// "设置胁迫报警密码 
            AddCommandClassNameList(typeof(Alarm.OpenDoorTimeoutAlarm.ReadOpenDoorTimeoutAlarm));// "读取开门超时报警参数 
            AddCommandClassNameList(typeof(Alarm.OpenDoorTimeoutAlarm.WriteOpenDoorTimeoutAlarm));// "设置开门超时报警参数 
            AddCommandClassNameList(typeof(Alarm.GateMagneticAlarm.ReadGateMagneticAlarm));// "读取门磁报警参数 
            AddCommandClassNameList(typeof(Alarm.GateMagneticAlarm.WriteGateMagneticAlarm));// "设置门磁报警参数 
            AddCommandClassNameList(typeof(Alarm.LegalVerificationCloseAlarm.ReadLegalVerificationCloseAlarm));// "读取合法验证解除报警开关 
            AddCommandClassNameList(typeof(Alarm.LegalVerificationCloseAlarm.WriteLegalVerificationCloseAlarm));// "设置合法验证解除报警开关 
            AddCommandClassNameList(typeof(Alarm.CloseAlarm));// "解除报警 

            AddCommandClassNameList(typeof(Door.ReaderOption.ReadReaderOption));// "读取读卡器字节数 
            AddCommandClassNameList(typeof(Door.ReaderOption.WriteReaderOption));// "设置读卡器字节数 
            AddCommandClassNameList(typeof(Door.RelayOption.ReadRelayOption));// "读取继电器参数 
            AddCommandClassNameList(typeof(Door.RelayOption.WriteRelayOption));// "设置继电器参数 
            AddCommandClassNameList(typeof(Door.Remote.CloseDoor));// "远程关门 
            AddCommandClassNameList(typeof(Door.Remote.HoldDoor));// "设置门常开 
            AddCommandClassNameList(typeof(Door.Remote.LockDoor));// "锁定门 
            AddCommandClassNameList(typeof(Door.Remote.OpenDoor));// "远程开门 
            AddCommandClassNameList(typeof(Door.Remote.UnlockDoor));// "解除锁定门 
            AddCommandClassNameList(typeof(Door.DoorWorkSetting.ReadDoorWorkSetting));// "读取门定时常开 
            AddCommandClassNameList(typeof(Door.DoorWorkSetting.WriteDoorWorkSetting));// "设置门定时常开 
            AddCommandClassNameList(typeof(Door.RelayReleaseTime.ReadUnlockingTime));// "获取开锁时输出时长 
            AddCommandClassNameList(typeof(Door.RelayReleaseTime.WriteUnlockingTime));// "设置开锁时输出时长 
            AddCommandClassNameList(typeof(Door.ExemptionVerificationOpen.ReadExemptionVerificationOpen));// "读取免验证开门 
            AddCommandClassNameList(typeof(Door.ExemptionVerificationOpen.WriteExemptionVerificationOpen));// "设置免验证开门 
            AddCommandClassNameList(typeof(Door.VoiceBroadcastSetting.ReadVoiceBroadcastSetting));// "读取语音播报功能 
            AddCommandClassNameList(typeof(Door.VoiceBroadcastSetting.WriteVoiceBroadcastSetting));// "设置语音播报功能 
            AddCommandClassNameList(typeof(Door.ReaderIntervalTime.ReadReaderIntervalTime));// "获取重复验证权限间隔 
            AddCommandClassNameList(typeof(Door.ReaderIntervalTime.WriteReaderIntervalTime));// "设置重复验证权限间隔 
            AddCommandClassNameList(typeof(Door.ExpirationPrompt.ReadExpirationPrompt));// "读取权限到期提示参数 
            AddCommandClassNameList(typeof(Door.ExpirationPrompt.WriteExpirationPrompt));// "设置权限到期提示参数 

            AddCommandClassNameList(typeof(Person.ReadPersonDataBase));// "读取人员存储详情 
            AddCommandClassNameList(typeof(Person.ReadPersonDatabaseDetail));// "读取人员存储详情 
            AddCommandClassNameList(typeof(Person.ClearPersonDataBase));// "清空所有人员 
            AddCommandClassNameList(typeof(Person.ReadPersonDetail));// "读取单个人员 
            AddCommandClassNameList(typeof(Person.DeletePerson));// "删除人员 
            AddCommandClassNameList(typeof(Person.AddPerson));// "添加人员 

            AddCommandClassNameList(typeof(AdditionalData.WriteFeatureCode));// "写入特征码 
            AddCommandClassNameList(typeof(AdditionalData.ReadFeatureCode));// "读取特征码 
            AddCommandClassNameList(typeof(AdditionalData.ReadPersonDetail));// "查询人员附加数据详情 
            AddCommandClassNameList(typeof(AdditionalData.DeleteFeatureCode));// "删除特征码 
            AddCommandClassNameList(typeof(AdditionalData.ReadFile));// "读文件 


            AddCommandClassNameList(typeof(Software.UpdateSoftware));// "上传固件 
            AddCommandClassNameList(typeof(Software.UpdateSoftware_FP));// "上传固件 

            AddCommandClassNameList(typeof(Door.ReadDoorOpenCheckMode));// "读取开门验证方式 
            AddCommandClassNameList(typeof(Door.WriteDoorOpenCheckMode));// "设置开门验证方式

            AddCommandClassNameList(typeof(SystemParameter.SendConnectTestResponse));// 连接握手回包



            AddCommandClassNameList(typeof(SystemParameter.ReadAuthenticationMode));//读取人脸机认证模式
            AddCommandClassNameList(typeof(SystemParameter.WriteAuthenticationMode));//设置人脸机认证模式
            AddCommandClassNameList(typeof(SystemParameter.ReadSaveRecordImage));//读取认证记录保存现场照片开关
            AddCommandClassNameList(typeof(SystemParameter.WriteSaveRecordImage));//设置认证记录保存现场照片开关
            AddCommandClassNameList(typeof(SystemParameter.ReadRecordQRCode));//读取识别结果查询二维码生成开关
            AddCommandClassNameList(typeof(SystemParameter.WriteRecordQRCode));//设置识别结果查询二维码生成开关
            AddCommandClassNameList(typeof(SystemParameter.ReadLightPattern));//读取感光模式
            AddCommandClassNameList(typeof(SystemParameter.WriteLightPattern));//设置感光模式


            AddCommandClassNameList(typeof(Alarm.SetFireAlarm));//设置消防报警功能开关
            AddCommandClassNameList(typeof(SystemParameter.ReadYZW_Push));//读取云筑网推送功能
            AddCommandClassNameList(typeof(SystemParameter.WriteYZW_Push));//设置云筑网推送功能
            AddCommandClassNameList(typeof(SystemParameter.ReadThirdpartyAPI));//读取第三方推送功能
            AddCommandClassNameList(typeof(SystemParameter.WriteThirdpartyAPI));//设置第三方推送功能
            AddCommandClassNameList(typeof(SystemParameter.SendReloadYZW_People));//重新拉取云筑网人员

            AddCommandClassNameList(typeof(SystemParameter.ReadFaceBioassaySimilarity));//读取活体检测阈值
            AddCommandClassNameList(typeof(SystemParameter.WriteFaceBioassaySimilarity));//设置活体检测阈值


            AddCommandClassNameList(typeof(SystemParameter.SendCMD_BeginAttendance));//开始点名
            AddCommandClassNameList(typeof(SystemParameter.SendCMD_BroadcastVoice));//播报语音
            AddCommandClassNameList(typeof(SystemParameter.SendCMD_EnterSleep));//进入休眠

            AddCommandClassNameList(typeof(Transaction.ReadTransactionDatabaseByAll));//读取所有历史记录

            AddCommandClassNameList(typeof(SystemParameter.WriteFaceDevice4GModuleStatus));//设置4G网络模块开关
            AddCommandClassNameList(typeof(SystemParameter.ReadFaceDevice4GModuleStatus));//读取4G网络模块开关

            AddCommandClassNameList(typeof(SystemParameter.ReadOfflineRecordPush));//读取离线推送记录开关
            AddCommandClassNameList(typeof(SystemParameter.WriteOfflineRecordPush));//设置离线推送记录开关

            AddCommandClassNameList(typeof(Protocol.Door.Door8800.SystemParameter.CacheContent.ReadCacheContent));//读取自定义字符串
            AddCommandClassNameList(typeof(Protocol.Door.Door8800.SystemParameter.CacheContent.WriteCacheContent));//设置自定义字符串

            AddCommandClassNameList(typeof(SystemParameter.ReadFaceIdentifyRange));//读取识别距离
            AddCommandClassNameList(typeof(SystemParameter.WriteFaceIdentifyRange));//设置识别距离


            AddCommandClassNameList(typeof(DoNetDrive.Protocol.Fingerprint.SystemParameter.Watch.BeginWatch_Broadcast));//广播开启监控
            AddCommandClassNameList(typeof(DoNetDrive.Protocol.Fingerprint.SystemParameter.Watch.CloseWatch_Broadcast));//广播关闭监控

            AddCommandClassNameList(typeof(SystemParameter.ReadFaceBioassay));//读取活体检测开关
            AddCommandClassNameList(typeof(SystemParameter.WriteFaceBioassay));//设置活体检测开关

            AddCommandClassNameList(typeof(SystemParameter.ReadDriveVolume));//读取音量
            AddCommandClassNameList(typeof(SystemParameter.WriteDriveVolume));//设置音量

            AddCommandClassNameList(typeof(SystemParameter.ReadDriveLanguage));//读取语言
            AddCommandClassNameList(typeof(SystemParameter.WriteDriveLanguage));//设置语言


            AddCommandClassNameList(typeof(SystemParameter.ReadHelmetDetection));//读取安全帽检测
            AddCommandClassNameList(typeof(SystemParameter.WriteHelmetDetection));//设置安全帽检测


          //  mCommandClasss = mCommandClasss
               // .Concat(frmElevator.IniCommandClassNameList())
            //    .ToDictionary(kv => kv.Key, kv => kv.Value);
        }


        private void AddCommandClassNameList(System.Type tType)
        {
            string sKey = tType.FullName;
            sKey = sKey.Replace('.', '_');
            string sValue = GetLanguage(sKey);
            if (sValue == "NULL") sValue = sKey;
            mCommandClasss.Add(tType.FullName, sValue);
        }



        #region 串口管理


        /// <summary>
        /// 重新加载串口列表
        /// </summary>
        private void IniSerialPortList()
        {
            cmbSerialPort.Items.Clear();
            var Ports = System.IO.Ports.SerialPort.GetPortNames();
            if (Ports.Length > 0)
            {
                cmbSerialPort.Items.AddRange(Ports);
                cmbSerialPort.SelectedIndex = 0;
            }

            cmbBaudrate.Items.Clear();
            cmbBaudrate.Items.AddRange("9600,19200,57600,115200,256000".Split(','));
            cmbBaudrate.SelectedIndex = 1;

        }

        private void butReloadSerialPort_Click(object sender, EventArgs e)
        {
            IniSerialPortList();
        }

        #endregion

        #region UDP
        /// <summary>
        /// UDP是否已绑定
        /// </summary>
        private bool mUDPIsBind = false;

        private string mServerIP;
        private int mServerPort;

        private void butUDPBind_Click(object sender, EventArgs e)
        {
            if (!txtUDPLocalPort.Text.IsNum())
            {
                MsgErr(GetLanguage("butUDPBind_Click0"));// "端口号不正确！
                return;
            }
            int port = txtUDPLocalPort.Text.ToInt32();
            string sLocalIP = cmbLocalIP.SelectedItem?.ToString();
            if (string.IsNullOrEmpty(sLocalIP))
            {
                MsgErr(GetLanguage("butUDPBind_Click1"));//"没有绑定本地IP！");
                return;
            }


            UDPServerDetail detail = new UDPServerDetail(sLocalIP, port);
            if (mUDPIsBind)
            {
                //关闭UDP服务器
                mAllocator.CloseConnector(detail);
                butUDPBind.Text = GetLanguage("butUDPBind_Click2");// "开启服务";
                mUDPIsBind = false;
                txtUDPLocalPort.Enabled = true;
                cmbLocalIP.Enabled = true;
            }
            else
            {
                butUDPBind.Enabled = false;
                mUDPIsBind = true;
                txtUDPLocalPort.Enabled = false;
                cmbLocalIP.Enabled = false;
                //打开UDP服务器
                mAllocator.OpenForciblyConnect(detail);
                mServerIP = sLocalIP;
                mServerPort = port;
                //等待后续事件，事件触发 mAllocator_ConnectorConnectedEvent 表示绑定成功
                //事件触发 mAllocator_ConnectorClosedEvent 表示绑定失败


            }
        }
        /// <summary>
        /// UDP绑定完毕
        /// </summary>
        /// <param name="bind">true 表示绑定成功</param>
        private void UDPBindOver(bool bind)
        {
            if (bind)
            {
                butUDPBind.Text = GetLanguage("UDPBindOver0");// "关闭服务";
            }
            else
            {
                mUDPIsBind = false;
                cmbLocalIP.Enabled = true;
                txtUDPLocalPort.Enabled = true;
            }

            butUDPBind.Enabled = true;
        }
        #endregion

        #region 初始化通讯类型
        /// <summary>
        /// 初始化通讯类型列表
        /// </summary>
        private void IniConnTypeList()
        {
            cmbConnType.SelectedIndex = 1;
            ShowConnTypePanel();


            _IsClosed = false;

            int iTop = gbSerialPort.Top, iLeft = gbSerialPort.Left;
            gbUDP.Top = iTop; gbUDP.Left = iLeft;
            gpTCPServer.Top = iTop; gpTCPServer.Left = iLeft;

            IniSerialPortList();

            IniLoadLocalIP();
            LoadSetting();
        }

        /// <summary>
        /// 初始化本机IP
        /// </summary>
        private void IniLoadLocalIP()
        {
            cmbLocalIP.Items.Clear();

            IPHostEntry localentry = Dns.GetHostEntry(Dns.GetHostName());
            foreach (IPAddress oItem in localentry.AddressList)
            {
                IPAddress ip = oItem;

                if (ip.IsIPv4MappedToIPv6)
                {
                    ip = ip.MapToIPv4();
                }
                if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                {
                    cmbLocalIP.Items.Add(ip.ToString());
                }
            }

            if (cmbLocalIP.Items.Count > 0)
            {
                cmbLocalIP.SelectedIndex = cmbLocalIP.Items.Count - 1;
            }
        }

        private void cmdConnType_SelectedIndexChanged(object sender, EventArgs e)
        {

            ShowConnTypePanel();
        }
        /// <summary>
        /// 显示通讯方式面板
        /// </summary>
        private void ShowConnTypePanel()
        {
            bool[] pnlShow = new bool[3];
            if (cmbConnType.SelectedIndex == -1) cmbConnType.SelectedIndex = 0;
            pnlShow[cmbConnType.SelectedIndex] = true;

            GroupBox[] pnls = new GroupBox[] { gbSerialPort, gbUDP, gpTCPServer };

            for (int i = 0; i < 3; i++)
            {
                pnls[i].Visible = pnlShow[i];
            }
        }

        #endregion

        #region 提示框
        public void MsgTip(string sText)
        {
            //提示
            MessageBox.Show(sText, GetLanguage("MsgTip"), MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        public void MsgErr(string sText)
        {
            //错误
            MessageBox.Show(sText, GetLanguage("MsgErr"), MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        #endregion


        #region 命令结果日志

        private void InilstCommand()
        {

        }

        /// <summary>
        /// 添加命令日志
        /// </summary>
        /// <param name="e">命令描述符</param>
        /// <param name="txt">命令需要输出的内容</param>
        public void AddCmdLog(CommandEventArgs e, string txt)
        {
            CommandResult commandResult = new CommandResult();
            INCommandDetail cmdDtl = e?.CommandDetail; string sType = e?.Command.GetType().FullName;
            if (_IsClosed) return;
            if (e != null)
            {
                double Timemill = 0;
                if (cmdDtl.EndTime == DateTime.MinValue || cmdDtl.BeginTime == DateTime.MinValue)
                {
                    Timemill = 0;
                }
                else
                {
                    Timemill = (cmdDtl.EndTime - cmdDtl.BeginTime).TotalMilliseconds;//命令耗时毫秒数
                }

                if (mCommandClasss.ContainsKey(sType))
                {
                    commandResult.Title = mCommandClasss[sType];
                }
                else
                {
                    commandResult.Title = sType;
                }
                commandResult.Content = txt;
                string Local, Remote, cType;
                GetConnectorDetail(cmdDtl.Connector, out cType, out Local, out Remote);
                OnlineAccess.OnlineAccessCommandDetail fcDtl = cmdDtl as OnlineAccess.OnlineAccessCommandDetail;
                commandResult.SN = fcDtl.SN;
                commandResult.Remote = Remote;
                commandResult.Time = DateTime.Now.ToTimeffff();
                commandResult.Timemill = Timemill.ToString("0");
            }
            else
            {
                commandResult.Title = "-";
                commandResult.Content = txt;
            }
            AddCmdItem(commandResult);
        }

        private void AddCmdItem(CommandResult commandResult)
        {
            if (dgvResult.InvokeRequired)
            {
                Invoke(() => AddCmdItem(commandResult));
                return;
            }
            dgvResult.Rows.Insert(0, commandResult.Title, commandResult.Content, commandResult.SN, commandResult.Remote, commandResult.Time, commandResult.Timemill);
            //int index = this.dgvResult.Rows.Add();
            //dgvResult.Rows[index].Cells[0].Value = commandResult.Title;
            //dgvResult.Rows[index].Cells[1].Value = commandResult.Content;
            //dgvResult.Rows[index].Cells[2].Value = commandResult.SN;
            //dgvResult.Rows[index].Cells[3].Value = commandResult.Remote;
            //dgvResult.Rows[index].Cells[4].Value = commandResult.Time;
            //dgvResult.Rows[index].Cells[5].Value = commandResult.Timemill;
        }

        private void butClearCommand_Click(object sender, EventArgs e)
        {
            dgvResult.Rows.Clear();
        }
        #endregion



        #region 数据监控


        private void buWatch_Click(object sender, EventArgs e)
        {
            var cmdDtl = GetCommandDetail();
            if (cmdDtl == null) return;

            INConnector cnt = mAllocator.GetConnector(cmdDtl.Connector);
            if (cnt == null)
            {
                //未开启监控
                mAllocator.OpenForciblyConnect(cmdDtl.Connector);
                cnt = mAllocator.GetConnector(cmdDtl.Connector);

            }
            BeginWatch cmd = new BeginWatch(cmdDtl);
            AddCommand(cmd);
            //处理返回值
            cmdDtl.CommandCompleteEvent += (sdr, cmde) =>
            {
                AddCmdLog(cmde, GetLanguage("buWatch_Click0"));// "已开启监控
            };

            //使通道保持连接不关闭
            cnt.OpenForciblyConnect();
            Door8800RequestHandle fC8800Request =
                new Door8800RequestHandle(DotNetty.Buffers.UnpooledByteBufferAllocator.Default, RequestHandleFactory);
            cnt.RemoveRequestHandle(typeof(Door8800RequestHandle));//先删除，防止已存在就无法添加。
            cnt.AddRequestHandle(fC8800Request);



        }

        /// <summary>
        /// 用于根据SN，命令参数、命令索引生产用于处理对应消息的处理类工厂函数
        /// </summary>
        /// <param name="sn"></param>
        /// <param name="cmdIndex"></param>
        /// <param name="cmdPar"></param>
        /// <returns></returns>
        private AbstractTransaction RequestHandleFactory(string sn, byte cmdIndex, byte cmdPar)
        {

            //在这里需要根据SN进行类型判定，也可以根据SN来进行查表
            if (cmdIndex >= 1 && cmdIndex <= 4)
            {
                return Transaction.ReadTransactionDatabaseByIndex.NewTransactionTable[cmdIndex]();
            }
            if (cmdIndex == 0x22)
            {
                return new DoNetDrive.Protocol.Door.Door8800.Data.Transaction.KeepaliveTransaction();
            }

            if (cmdIndex == 0xA0)
            {
                return new DoNetDrive.Protocol.Door.Door8800.Data.Transaction.ConnectMessageTransaction();
            }
            return null;
        }

        /// <summary>
        /// 监控消息
        /// </summary>
        /// <param name="connectorDTL"></param>
        /// <param name="EventData"></param>
        private void MAllocator_TransactionMessage(INConnectorDetail connectorDTL, Core.Data.INData EventData)
        {
            CommandResult commandResult = new CommandResult();
            var conn = mAllocator.GetConnector(connectorDTL);
            if (conn != null)
            {
                if (!conn.IsForciblyConnect())
                {
                    conn.OpenForciblyConnect();
                }
            }
            if (_IsClosed) return;

            Door8800Transaction fcTrn = EventData as Door8800Transaction;
            StringBuilder strbuf = new StringBuilder();
            var evn = fcTrn.EventData;
            string sRemoteIP = String.Empty; int iRemotePort = 0;
            bool bIsUDP = false;
            bool bIsCOM = false;
            if (connectorDTL is TCPServerClientDetail)
            {
                var tcp = connectorDTL as TCPServerClientDetail;
                sRemoteIP = tcp.Remote.Addr;
                iRemotePort = tcp.Remote.Port;
            }
            else if (connectorDTL is Core.Connector.TCPClient.TCPClientDetail)
            {
                bIsUDP = true;
                var UDP = connectorDTL as Core.Connector.TCPClient.TCPClientDetail;
                sRemoteIP = UDP.Addr;
                iRemotePort = UDP.Port;
            }

            else if (connectorDTL is SerialPortDetail)
            {
                //bIsUDP = true;
                //var comDTL = connectorDTL as SerialPortDetail;
                bIsCOM = true;
                sRemoteIP = String.Empty;
                iRemotePort = 0;
            }

            //消息类型
            commandResult.Title = TransactionTypeName[fcTrn.CmdIndex];
            commandResult.SN = fcTrn.SN;

            //客户端信息
            string Local, Remote, cType;
            GetConnectorDetail(connectorDTL, out cType, out Local, out Remote);

            commandResult.Remote = Remote;

            commandResult.Time = fcTrn.EventData.TransactionDate.ToDateTimeStr();
            commandResult.Timemill = "-";


            if (fcTrn.CmdIndex <= 4)
            {
                //"序号："
                strbuf.Append(GetLanguage("TransactionMessage0")).Append(evn.SerialNumber).Append("；");

                if (evn.TransactionType < 4)
                {
                    string[] codeNameList = frmRecord.mTransactionCodeNameList[evn.TransactionType];
                    //"事件："
                    strbuf.Append(GetLanguage("TransactionMessage1")).Append(evn.TransactionCode).Append("(").Append(codeNameList[evn.TransactionCode]).Append(")");
                    //"；时间："
                    strbuf.Append(GetLanguage("TransactionMessage2")).Append(evn.TransactionDate.ToString("yyyy-MM-dd HH:mm:ss"));
                }

                strbuf.AppendLine();
                if (fcTrn.CmdIndex == 1)
                {
                    Data.Transaction.CardTransaction cardtrn = evn as Data.Transaction.CardTransaction;
                    //"用户号："
                    strbuf.Append(GetLanguage("TransactionMessage3")).Append(cardtrn.UserCode.ToString());
                    //；出/入：
                    strbuf.Append(GetLanguage("TransactionMessage4")).Append(cardtrn.Accesstype.ToString());

                    string sPhotoUse, PhotoNULL;
                    sPhotoUse = GetLanguage("TransactionMessage5_1");//有
                    PhotoNULL = GetLanguage("TransactionMessage5_0");//无
                    //"，照片："
                    strbuf.Append(GetLanguage("TransactionMessage5")).Append(cardtrn.Photo == 1 ? sPhotoUse : PhotoNULL);
                }
                if (fcTrn.CmdIndex == 4)
                {
                    Data.Transaction.BodyTemperatureTransaction btr = evn as Data.Transaction.BodyTemperatureTransaction;
                    //体温：
                    strbuf.Append(GetLanguage("TransactionMessage7")).Append((float)btr.BodyTemperature / (float)10);
                }


            }
            if (fcTrn.CmdIndex == 0x22)
            {
                //"保活包消息，远端信息：
                strbuf.Append(fcTrn.SN).Append(GetLanguage("TransactionMessage8")).Append(Remote);
                //发送一个回包
                if (!bIsCOM)
                    SendCallblack(bIsUDP, fcTrn, sRemoteIP, iRemotePort, connectorDTL.GetKey());
            }

            if (fcTrn.CmdIndex == 0xA0)
            {
                if (!bIsCOM)
                    SendCallblack(bIsUDP, fcTrn, sRemoteIP, iRemotePort, connectorDTL.GetKey());

                //"连接测试消息，远端信息："
                strbuf.Append(fcTrn.SN).Append(GetLanguage("TransactionMessage9")).Append(Remote);
            }

            if ((fcTrn.CmdIndex == 0x22 || fcTrn.CmdIndex == 0xA0) && bIsUDP)
            {
                Invoke(() =>
                {
                    if (chkIsUDPServer.Checked)
                    {
                        txtUDPAddr.Text = sRemoteIP;
                        txtUDPPort.Text = iRemotePort.ToString();
                        txtSN.Text = fcTrn.SN;
                    }

                });
            }
            var log = strbuf.ToString();
            _Log.Debug(log.Replace("\r\n", " "));
            commandResult.Content = log;

            AddCmdItem(commandResult);
        }


        private void SendCallblack(bool bIsUDP, Door8800Transaction fcTrn, string sRemoteIP, int iRemotePort, string ConnKey)
        {
            //连接测试消息
            INCommandDetail cmdDtl;
            if (bIsUDP)
                cmdDtl = GetUDPCommandDetail(fcTrn.SN, "FFFFFFFF", sRemoteIP, iRemotePort);
            else
                cmdDtl = GetTCPCommandDetail(fcTrn.SN, "FFFFFFFF", ConnKey);


            var sndConntmsg = new SystemParameter.SendConnectTestResponse(cmdDtl);
            AddCommand(sndConntmsg);
        }


        #endregion

        private void DgvIO_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewTextBoxColumn textbox = dgvIO.Columns[e.ColumnIndex] as DataGridViewTextBoxColumn;
            if (textbox != null) //如果该列是TextBox列
            {
                dgvIO.BeginEdit(true); //开始编辑状态
                dgvIO.ReadOnly = false;
            }
        }

        private void ButReadSN_Click(object sender, EventArgs e)
        {
            var cmdDtl = GetCommandDetail();
            if (cmdDtl == null) return;

            cmdDtl.UserData = "AutoReadSN";
            ReadSN cmd = new ReadSN(cmdDtl);
            AddCommand(cmd);

            cmdDtl.CommandTimeout += CmdDtl_CommandTimeout;
            cmdDtl.CommandCompleteEvent += CmdDtl_CommandCompleteEvent;
            cmdDtl.CommandErrorEvent += CmdDtl_CommandErrorEvent;
        }

        private void CmdDtl_CommandErrorEvent(object sender, CommandEventArgs e)
        {
        }

        private void CmdDtl_CommandCompleteEvent(object sender, CommandEventArgs e)
        {
        }

        private void CmdDtl_CommandTimeout(object sender, CommandEventArgs e)
        {
        }

        private void ButStopCommand_Click(object sender, EventArgs e)
        {
            mStop = true;
            var cmdDtl = GetCommandDetail();
            if (cmdDtl == null) return;
            mAllocator.StopCommand(cmdDtl.Connector);
        }

        #region  TCP Server 服务绑定
        private bool mTCPServerBind = false;
        private string mTCPServer_IP;
        private int mTCPServer_Port;


        private class MyTCPServerClientDetail : TCPServerClientDetail
        {
            public String SN;
            internal MyRequestHandle Handle;

            public MyTCPServerClientDetail(string sKey, IPDetail remoteIP, int iClientID) :
                base(sKey, remoteIP, new IPDetail("any", 0), iClientID)
            {

            }

            public override string ToString()
            {
                if (string.IsNullOrEmpty(SN))
                {
                    return Remote.ToString();
                }
                return $"{SN} {Remote.ToString()}";
            }

        }
        /// <summary>
        /// 客户端连接对象集合
        /// </summary>
        private ConcurrentDictionary<string, MyTCPServerClientDetail> mTCPClients;

        private void butTCPServerBind_Click(object sender, EventArgs e)
        {
            if (!mTCPServerBind)
            {
                //开始绑定
                BeginTCPServer();
            }
            else
            {
                //解除绑定
                StopTCPServer();
            }

        }
        /// <summary>
        /// 解除绑定
        /// </summary>
        private void StopTCPServer()
        {

            if (!mTCPServerBind) return;
            var tcp = new TCPServerDetail(mTCPServer_IP, mTCPServer_Port);

            mAllocator.CloseConnector(tcp);

            var sKeys = mTCPClients.Keys.ToArray();
            foreach (var key in sKeys)
            {
                TCPServerClientDetail cdt = new TCPServerClientDetail(key);
                mAllocator.CloseConnector(cdt);
            }
            mTCPClients.Clear();
            cmbTCPClientList.Items.Clear();

        }

        /// <summary>
        /// 开始绑定
        /// </summary>
        private void BeginTCPServer()
        {
            if (mTCPClients == null) mTCPClients = new ConcurrentDictionary<string, MyTCPServerClientDetail>();
            try
            {   //生成连接通道消息
                mTCPServer_IP = cmbLocalIP.Text;
                mTCPServer_Port = int.Parse(txtTCPServerPort.Text);
                if (ChkTLS12.Checked)
                {
                    string sSSLFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "SSLX509.pfx");
                    if (File.Exists(sSSLFile))
                    {
                        byte[] x509Data = File.ReadAllBytes(sSSLFile);
                        var x509 = new X509Certificate2(x509Data, "BRUsqOWH");
                        var tcp = new TCPServerDetail(mTCPServer_IP, mTCPServer_Port, true, x509);

                        mAllocator.OpenConnector(tcp);
                    }
                    else
                    {
                        MessageBox.Show("没有找到SSL证书");
                    }

                }
                else
                {
                    var tcp = new TCPServerDetail(mTCPServer_IP, mTCPServer_Port);

                    mAllocator.OpenConnector(tcp);
                }

                //等待后续事件，事件触发 mAllocator_ConnectorConnectedEvent 表示绑定成功
                //事件触发 mAllocator_ConnectorClosedEvent 表示绑定失败
                butTCPServerBind.Enabled = false;
                ChkTLS12.Enabled = false;
            }
            catch (Exception ex)
            {
                MsgErr(ex.Message);
                return;
            }

        }

        private void TCPBindOver(bool bResult)
        {
            butTCPServerBind.Enabled = true;
            mTCPServerBind = bResult;
            txtTCPServerPort.Enabled = !bResult;
            cmbLocalIP.Enabled = !bResult;
            ChkTLS12.Enabled = !bResult;
            if (bResult)
            {
                //绑定成功
                butTCPServerBind.Text = GetLanguage("butTCPServerBind_Close");//"关闭"
                Task.Run(CheckTCPClient);
            }
            else
            {
                butTCPServerBind.Text = GetLanguage("butTCPServerBind");// "绑定";
            }

        }

        /// <summary>
        /// Add TCP Client 到列表
        /// </summary>
        private void AddTCPClient(string sKey, IPDetail remoteIP)
        {
            MyTCPServerClientDetail myTCP = new MyTCPServerClientDetail(
                sKey, remoteIP, 0);
            if (!mTCPClients.TryAdd(sKey, myTCP))
            {
                //重复添加客户端，Key:{sKey}
                AddLog(GetLanguage("AddTCPClient0", sKey));
                return;
            }
            else
            {
                Invoke(new Action(() =>
                {
                    cmbTCPClientList.BeginUpdate();
                    cmbTCPClientList.Items.Add(myTCP);
                    if (cmbTCPClientList.Items.Count == 1)
                    {
                        cmbTCPClientList.SelectedIndex = 0;
                    }
                    else
                    {
                        cmbTCPClientList.SelectedIndex = cmbTCPClientList.Items.Count - 1;
                    }
                    cmbTCPClientList.EndUpdate();
                }));

            }
        }




        /// <summary>
        /// 检查TCP客户端
        /// </summary>
        private void CheckTCPClient()
        {
            do
            {
                var keys = mTCPClients.Keys.ToArray();
                MyTCPServerClientDetail mTCP;
                foreach (var key in keys)
                {
                    if (mTCPClients.TryGetValue(key, out mTCP))
                    {
                        if (mTCP.Handle != null)
                        {
                            if (mTCP.Handle.CheckTimeout())
                            {
                                RemoteTCPClient(key);
                                AddLog($"TCP 连接超时，自动关闭连接通道:{key}");
                                mAllocator.CloseConnector(mTCP.Handle.ConnDetail);
                            }
                        }

                    }
                }
                System.Threading.Thread.Sleep(100);

            } while (mTCPServerBind);


        }

        /// <summary>
        /// 删除客户端
        /// </summary>
        /// <param name="sKey"></param>
        private void RemoteTCPClient(string sKey)
        {
            MyTCPServerClientDetail myTCP = null;
            if (mTCPClients.TryRemove(sKey, out myTCP))
            {
                Invoke(new Action(() =>
                {
                    cmbTCPClientList.BeginUpdate();
                    cmbTCPClientList.Items.Remove(myTCP);
                    cmbTCPClientList.EndUpdate();
                }));
            }
            else
            {


            }
        }
        #endregion

        #region 调试器多语言
        private void cmbToolLanguage_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadLanguage();
        }

        private void LoadLanguage()
        {
            var sFile = cmbToolLanguage.Text;
            sFile = ConfigurationManager.AppSettings[$"Language_{sFile}"];
            sFile = Path.Combine(Application.StartupPath, sFile);
            if (File.Exists(sFile))
                ToolLanguage.LoadLanguage(sFile);
            LoadUILanguage();
        }
        #endregion

        #region 产生随机数
        /// <summary>
        /// 随机数产生器
        /// </summary>
        public static Random CodeRandom = new Random();

        /// <summary>
        /// 获得一个随机数
        /// </summary>
        /// <returns></returns>
        public static int GetRandomNum() => CodeRandom.Next(1, 65530);

        #endregion

        #region 设备搜索
        private int mSerarchUDPNetPort = 8101;
        private int mSerarchCOMPort = 1;
        private int mSerarchCOMBaudrate = 115200;
        private string mSerarchTCPClientKey = string.Empty;
        private int mSerarchConnectType = 1;//1 udp;2--tcp;0--com

        /// <summary>
        /// 自动搜索的随机数
        /// </summary>
        private int mSearchID;

        private int mSearchCount;
        private void butSearch_Click(object sender, EventArgs e)
        {
            mSearchID = GetRandomNum();
            mStop = false;
            mSNList = new List<string>();
            dgDevice.Rows.Clear();
            mSearchCount = 0;
            BeginSearchDrive();
        }

        private void BeginSearchDrive()
        {
            if (this.InvokeRequired)
            {
                Invoke(BeginSearchDrive);
                return;
            }
            mSerarchConnectType = cmbConnType.SelectedIndex;//通讯模式


            switch (mSerarchConnectType)//串口,TCP客户端,UDP,TCP服务器
            {
                case 0://串口
                    if (cmbSerialPort.SelectedIndex == -1)
                    {
                        MsgTip(GetLanguage("GetCommandDetail0"));//"请先选择一个串口号！");
                        return;
                    }
                    mSerarchCOMPort = cmbSerialPort.Text.Substring(3).ToInt32();
                    mSerarchCOMBaudrate = int.Parse(cmbBaudrate.Text);
                    break;
                case 1://UDP 
                    if (!mUDPIsBind)
                    {
                        MsgErr(GetLanguage("GetCommandDetail1"));//"请先绑定UDP端口");
                        return;
                    }
                    if (!int.TryParse(txtUDPPort.Text, out mSerarchUDPNetPort))
                    {
                        mSerarchUDPNetPort = 8000;
                    }
                    break;
                case 2://tcp server 模式，需要寻找客户端
                    if (!mTCPServerBind)
                    {
                        MsgErr(GetLanguage("GetCommandDetail2"));//"请先绑定 TCP Server 端口");
                        return;
                    }
                    if (cmbTCPClientList.SelectedIndex == -1)
                    {
                        MsgErr(GetLanguage("GetCommandDetail3"));//"请先选择一个 TCP 客户端!");
                        return;
                    }
                    var myClient = cmbTCPClientList.SelectedItem as MyTCPServerClientDetail;
                    mSerarchTCPClientKey = myClient.GetKey();
                    break;
                default:
                    return;
            }

            mSearchCount++;
            if (mSearchCount > 10)
                return;
            var cmdDtl = SearchCommandDetail();
            cmdDtl.Timeout = 2000;
            cmdDtl.RestartCount = 0;
            cmdDtl.UserData = mSearchID;
            var searchPar = new SearchControltor_Parameter((ushort)mSearchID);
            searchPar.UDPBroadcast = true;

            var searchCmd = new SearchControltor(cmdDtl, searchPar);
            mAllocator.AddCommand(searchCmd);
            cmdDtl.CommandCompleteEvent += search_CommandCompleteEvent;
            cmdDtl.CommandTimeout += search_CommandTimeout;
            cmdDtl.CommandErrorEvent += search_CommandErrorEvent;
        }


        private OnlineAccessCommandDetail SearchCommandDetail()
        {
            string sSearchSN = "0000000000000000", sPassword = "FFFFFFFF";
            
            var connectType = CommandDetailFactory.ConnectType.UDPClient;
            INCommandDetail returnDtl = null;
            switch (mSerarchConnectType)//串口,TCP客户端,UDP,TCP服务器
            {
                case 0://串口

                    returnDtl= GetCommandDetail(new SerialPortDetail((byte)mSerarchCOMPort, mSerarchCOMBaudrate)
                        , sSearchSN, sPassword);
                    break;
                case 1://UDP 

                    connectType = CommandDetailFactory.ConnectType.UDPClient;
                    string sDestIP = "255.255.255.255";
                    returnDtl = GetCommandDetail(connectType, sSearchSN, sPassword, sDestIP, mSerarchUDPNetPort);
                    break;
                case 2://tcp server 模式，需要寻找客户端

                    connectType = CommandDetailFactory.ConnectType.TCPServerClient;
                    returnDtl = GetCommandDetail(connectType, sSearchSN, sPassword, mSerarchTCPClientKey, mSerarchUDPNetPort);
                    break;
                default:
                    return null;
            }

            returnDtl.Timeout = 2000;
            returnDtl.RestartCount = 3;
            returnDtl.UserData = null;

            return returnDtl as OnlineAccessCommandDetail;

        }


        private void search_CommandErrorEvent(object sender, CommandEventArgs e)
        {
            //MessageBox.Show("命令错误");
            ushort iSearchID = (ushort)(int)e.CommandDetail.UserData;
            if (iSearchID != mSearchID) return;
            if (mStop) return;
            Task.Run(BeginSearchDrive);
        }

        private void search_CommandTimeout(object sender, CommandEventArgs e)
        {
            ushort iSearchID = (ushort)(int)e.CommandDetail.UserData;
            if (iSearchID != mSearchID) return;
            //搜索结束;
            if (mStop) return;
            Task.Run(BeginSearchDrive);
        }



        private void search_CommandCompleteEvent(object sender, CommandEventArgs e)
        {
            ushort iSearchID = (ushort)(int)e.CommandDetail.UserData;
            if (iSearchID != mSearchID) return;
            if (mStop) return;

            SearchControltor_Result rst = e.Result as SearchControltor_Result;

            Task.Run(() =>
            {
                Invoke(AddSearchDrive, rst);
            });
        }

        private void AddSearchDrive(SearchControltor_Result oDriveResult)
        {
            if (oDriveResult == null)
                return;
            if (this.InvokeRequired)
            {
                Invoke(AddSearchDrive, oDriveResult);
                return;
            }
            if (oDriveResult == null)
                return;
            var oTCP = oDriveResult.TCP;
            string sSearchSN = oDriveResult.SN;
            if (sSearchSN.Length != 16) return;

            string sSNFormat = sSearchSN.Substring(0, 8);


            #region 将网络代码写入到设备
            var cmdDtl = SearchCommandDetail();
            var connDTL = cmdDtl.Connector as UDPClientDetail;
            cmdDtl.SN = sSearchSN;
            connDTL.Port = oTCP.mUDPPort;
            var searchPar = new SearchControltor_Parameter((ushort)mSearchID);
            searchPar.UDPBroadcast = true;

            var searchCmd = new WriteControltorNetCode(cmdDtl, searchPar);
            mAllocator.AddCommand(searchCmd);
            #endregion

            if (!mSNList.Contains(sSearchSN))
            {
                mSNList.Add(sSearchSN);
                DeviceInfo deviceInfo = new DeviceInfo(sSearchSN, $"{oTCP.mIP}:{oTCP.mUDPPort}", oTCP.mMAC);

                AddDeviceItem(deviceInfo);
            }

        }

        private void AddDeviceItem(DeviceInfo deviceInfo)
        {
            if (dgDevice.InvokeRequired)
            {
                Invoke(() => AddDeviceItem(deviceInfo));
                return;
            }

            dgDevice.Rows.Insert(0, dgDevice.Rows.Count + 1, deviceInfo.SN, deviceInfo.MAC, deviceInfo.IP);
        }

        private void dgDevice_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            int col = e.ColumnIndex, row = e.RowIndex;
            if (row < 0) return;
            var gdRow = dgDevice.Rows[row];
            txtSN.Text = gdRow.Cells[1].Value.ToString();
        }
        #endregion

        private void btlToolLift_Click(object sender, EventArgs e)
        {
           // frmElevator frm = frmElevator.GetForm(this);
            //frm.Show();
            //if (frm.WindowState == FormWindowState.Minimized)
            //    frm.WindowState = FormWindowState.Normal;
            //frm.Activate();
            //ShowFrm(frm);
        }
    }
}
