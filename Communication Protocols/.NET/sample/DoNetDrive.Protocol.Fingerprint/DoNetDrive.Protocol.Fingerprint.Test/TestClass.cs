
using DoNetDrive.Core;
using DoNetDrive.Core.Command;
using DoNetDrive.Core.Connector.UDP;
using DoNetDrive.Protocol.Door.Door8800.SystemParameter.SN;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace DoNetDrive.Protocol.Fingerprint.Test
{
    internal class TestClass
    {
        ConnectorAllocator _connectorAllocator;
        string _serverIP;
        int _serverPort;
        public TestClass()
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
        /// 获取命令详情信息
        /// </summary>
        /// <returns></returns>
        public INCommandDetail GetCommandDetail()
        {
            /**
             * CommandDetailFactory.ConnectType.UDPClient 通讯方式
             * 192.168.1.150 设备局域网地址
             * 8101 设备的UDP端口
             * CommandDetailFactory.ControllerType.A33_Face 设备类型
             * 0000000000000000 16位设备SN
             * FFFFFFFF 设备通讯密码
             */
            var cmdDtl = CommandDetailFactory.CreateDetail(CommandDetailFactory.ConnectType.UDPClient, "192.168.1.150", 8101,
                CommandDetailFactory.ControllerType.A33_Face, "0000000000000000", "FFFFFFFF");

            //UDP方式下需要添加本地IP和本地监听端口，TCP方式不需要
            var dtl = cmdDtl.Connector as UDPClientDetail;
            dtl.LocalAddr = _serverIP;
            dtl.LocalPort = _serverPort;
            //超时时间（需要消耗时间的可以设置长一点）
            cmdDtl.Timeout = 600;
            //超时重试次数
            cmdDtl.RestartCount = 3;
            return cmdDtl;
        }
        /// <summary>
        ///  async  await 方式调用
        /// </summary>
        /// <returns></returns>
        public async Task GetDeviceSnUdpAsync()
        {
            var cmdDetail = GetCommandDetail();
            //创建读取SN的命令对象
            var cmd = new ReadSN(cmdDetail);
           
            await _connectorAllocator.AddCommandAsync(cmd);
            var result = cmd.getResult() as SN_Result;
            var sn = Encoding.ASCII.GetString(result.SNBuf);
        }
        /// <summary>
        /// 事件回调方式
        /// </summary>
        public void GetDeviceSn()
        {
            var cmdDetail = GetCommandDetail();
            //创建读取SN的命令对象
            INCommand cmd = new ReadSN(cmdDetail);
            //回调方式调用
            cmdDetail.CommandCompleteEvent += ReadSN_CommandCompleteEvent;
            _connectorAllocator.AddCommand(cmd);
        }
        /// <summary>
        /// 读取SN回调
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ReadSN_CommandCompleteEvent(object sender, CommandEventArgs e)
        {
            var result = e.Result as SN_Result;
            var sn = Encoding.ASCII.GetString(result.SNBuf);
        }
    }
}
