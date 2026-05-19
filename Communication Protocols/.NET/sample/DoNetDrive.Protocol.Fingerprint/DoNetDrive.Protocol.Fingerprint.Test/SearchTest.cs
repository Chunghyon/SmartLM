using DoNetDrive.Core;
using DoNetDrive.Core.Connector.UDP;
using DoNetDrive.Protocol.Door.Door8800.SystemParameter.SearchControltor;
using DoNetDrive.Protocol.OnlineAccess;
using System;
using System.Collections.Generic;

namespace DoNetDrive.Protocol.Fingerprint.Test
{
    public class SearchTest
    {
        ConnectorAllocator _connectorAllocator;
        string _serverIP;
        int _serverPort;
        /// <summary>
        /// 自动搜索的随机数
        /// </summary>
        private int mSearchID;
        /// <summary>
        /// 搜索次数
        /// </summary>
        private int mSearchCount = 0;
        /// <summary>
        /// 随机数产生器
        /// </summary>
        public static Random CodeRandom = new Random();
        /// <summary>
        /// 设备列表
        /// </summary>
        List<string> mSNList = new List<string>();
        /// <summary>
        /// 设备端口
        /// </summary>
        int mDrivePort = 8101;
        /// <summary>
        /// 获得一个随机数
        /// </summary>
        /// <returns></returns>
        public static int GetRandomNum() => CodeRandom.Next(1, 65530);
        public SearchTest()
        {
            //获取连接通道对象（单例）
            _connectorAllocator = ConnectorAllocator.GetAllocator();
            //本机IP地址
            _serverIP = "192.168.1.120";
            //监听端口
            _serverPort = 9000;

            //开启UDP监听
            UDPServerDetail serverDetail = new UDPServerDetail(_serverIP, _serverPort);
            _connectorAllocator.OpenForciblyConnect(serverDetail);
        }
        /// <summary>
        /// 搜索命令详情
        /// </summary>
        /// <returns></returns>
        private OnlineAccessCommandDetail SearchCommandDetail()
        {
            string sDestIP = "255.255.255.255";
            string sSearchSN = "0000000000000000", sPassword = "FFFFFFFF";
            var oUDPDtl = new UDPClientDetail(sDestIP, mDrivePort,
                _serverIP, _serverPort);
            var dtl = new OnlineAccessCommandDetail(oUDPDtl, sSearchSN, sPassword)
            {
                Timeout = 2000,
                RestartCount = 3,
                UserData = null
            };
            return dtl;
        }
        /// <summary>
        /// 搜索方法
        /// </summary>
        public void BeginSearch()
        {
            mSearchID = GetRandomNum();
            mSearchCount++;
            if (mSearchCount == 5)
            {
                //搜索次数大于5次退出搜索
                return;
            }
            var cmdDtl = SearchCommandDetail();
            var searchPar = new SearchControltor_Parameter((ushort)mSearchID);
            searchPar.UDPBroadcast = true;//广播搜索
            var searchCmd = new SearchControltor(cmdDtl, searchPar);
            _connectorAllocator.AddCommand(searchCmd);

            cmdDtl.CommandCompleteEvent += Search_CommandCompleteEvent;
            cmdDtl.CommandTimeout += Search_CommandTimeout;
            cmdDtl.CommandErrorEvent += Search_CommandErrorEvent;
        }
        /// <summary>
        /// 命令错误再次重新搜索
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Search_CommandErrorEvent(object sender, Core.Command.CommandEventArgs e)
        {
            ushort iSearchID = (ushort)(int)e.CommandDetail.UserData;
            if (iSearchID != mSearchID) return;
            BeginSearch();
        }
        /// <summary>
        /// 命令超时再次重新搜索
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Search_CommandTimeout(object sender, Core.Command.CommandEventArgs e)
        {
            ushort iSearchID = (ushort)(int)e.CommandDetail.UserData;
            if (iSearchID != mSearchID) return;
            BeginSearch();
        }
        /// <summary>
        /// 搜索成功返回设备信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Search_CommandCompleteEvent(object sender, Core.Command.CommandEventArgs e)
        {
            ushort iSearchID = (ushort)(int)e.CommandDetail.UserData;
            if (iSearchID != mSearchID) return;
            SearchControltor_Result rst = e.Result as SearchControltor_Result;
            if (rst != null)
            {
                if (!mSNList.Contains(rst.SN))
                {
                    mSNList.Add(rst.SN);
                }
            }
        }
    }
}
