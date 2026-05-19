
using DoNetDrive.Common.Extensions;
using DoNetDrive.Core;
using DoNetDrive.Core.Command;
using DoNetDrive.Core.Connector.UDP;
using DoNetDrive.Protocol.Door.Door8800.SystemParameter.SN;
using DoNetDrive.Protocol.Door8800;
using DoNetDrive.Protocol.Fingerprint.Data.Transaction;
using DoNetDrive.Protocol.Fingerprint.Test.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace DoNetDrive.Protocol.Fingerprint.Test
{
    internal class ConnectorAllocatorTestClass
    {
        ConnectorAllocator _connectorAllocator;
        public ConnectorAllocatorTestClass()
        {
            //获取连接通道对象（单例）
            _connectorAllocator = ConnectorAllocator.GetAllocator();

            _connectorAllocator.CommandCompleteEvent += CommandCompleteEvent;
            _connectorAllocator.CommandErrorEvent += CommandErrorEvent;
            _connectorAllocator.CommandProcessEvent += CommandProcessEvent;
            _connectorAllocator.CommandTimeout += CommandTimeout;
            _connectorAllocator.AuthenticationErrorEvent += AuthenticationErrorEvent;
            _connectorAllocator.TransactionMessage += TransactionMessage;
            _connectorAllocator.ConnectorConnectedEvent += ConnectorConnectedEvent;
            _connectorAllocator.ConnectorClosedEvent += ConnectorClosedEvent;
            _connectorAllocator.ConnectorErrorEvent += ConnectorErrorEvent;
            _connectorAllocator.ClientOnline += ClientOnline;
            _connectorAllocator.ClientOffline += ClientOffline;
        }

        private void ClientOffline(object sender, Core.Connector.ServerEventArgs e)
        {
            Console.WriteLine("设备离线");
        }

        private void ClientOnline(object sender, Core.Connector.ServerEventArgs e)
        {
            Console.WriteLine("设备上线");
        }
        /// <summary>
        /// 连接错误
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="connector"></param>
        private void ConnectorErrorEvent(object sender, Core.Connector.INConnectorDetail connector)
        {
            Console.WriteLine("连接错误");
        }
        /// <summary>
        /// 连接关闭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="connector"></param>
        private void ConnectorClosedEvent(object sender, Core.Connector.INConnectorDetail connector)
        {
            Console.WriteLine("连接关闭");
        }
        /// <summary>
        /// 连接成功
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="connector"></param>
        private void ConnectorConnectedEvent(object sender, Core.Connector.INConnectorDetail connector)
        {
            Console.WriteLine("连接成功");
        }

        /// <summary>
        /// 数据接收事件
        /// </summary>
        /// <param name="connector"></param>
        /// <param name="EventData"></param>
        /// <exception cref="NotImplementedException"></exception>
        private void TransactionMessage(Core.Connector.INConnectorDetail connector, Core.Data.INData EventData)
        {
            Door8800Transaction transaction = EventData as Door8800Transaction;

            switch (transaction.CmdIndex)
            {
                case 1:
                    //人脸识别记录
                    var cardTransaction = EventData as CardTransaction;
                    break;
                case 2:
                    //门磁记录
                    var doorSensor = EventData as DoorSensorTransaction;
                    break;
                case 3:
                    //系统记录
                    var systemTransaction= EventData as SystemTransaction;
                    break;
                case 4:
                    //体温记录
                    var bodyTemperature = EventData as BodyTemperatureTransaction;
                    break;
                    case 0xA0:
                    //连接测试消息 需要回复SendConnectTestResponse命令
                    break;
                case 0x22:
                    //心跳消息
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// 身份鉴权验证失败
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <exception cref="NotImplementedException"></exception>
        private void AuthenticationErrorEvent(object sender, CommandEventArgs e)
        {
            var commandName = e.Command.GetType().FullName;
            Console.WriteLine($"{commandName}身份鉴权验证失败");
        }

        /// <summary>
        /// 命令超时
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <exception cref="NotImplementedException"></exception>
        private void CommandTimeout(object sender, CommandEventArgs e)
        {
            var commandName = e.Command.GetType().FullName;
            Console.WriteLine($"{commandName}命令超时");
        }

        /// <summary>
        /// 命令进度
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <exception cref="NotImplementedException"></exception>
        private void CommandProcessEvent(object sender, CommandEventArgs e)
        {
            var commandName = e.Command.GetType().FullName;
            Console.WriteLine($"{commandName}命令当前进度{e.Command.getProcessMax()}/{e.Command.getProcessStep()}");
        }

        /// <summary>
        /// 命令失败
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <exception cref="NotImplementedException"></exception>
        private void CommandErrorEvent(object sender, CommandEventArgs e)
        {
            OnlineAccess.OnlineAccessCommandDetail fcDtl = e.CommandDetail as OnlineAccess.OnlineAccessCommandDetail;
            var commandName = e.Command.GetType().FullName;
            Console.WriteLine($"{fcDtl.SN}设备执行{commandName} 命令失败");
        }
        /// <summary>
        /// 命令完成
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <exception cref="NotImplementedException"></exception>
        private void CommandCompleteEvent(object sender, CommandEventArgs e)
        {
            var result = e.Result;//返回结果
        }
    }
}
