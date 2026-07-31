using DeviceProtocolServer.Utilities;
using DoNetDrive.Common.Extensions;
using FaceWebServer.DB.Table;
using FaceWebServer.DTO.Access;
using FaceWebServer.DTO.Cache;
using FaceWebServer.DTO.Config;
using FaceWebServer.DTO.Device;
using FaceWebServer.DTO.HTTPv2_Protocol;
using FaceWebServer.DTO.MQTT;
using FaceWebServer.DTO.MQTT_Protocol;
using FaceWebServer.DTO.MQTT_Protocol.Command.Device;
using FaceWebServer.DTO.MQTT_Protocol.Command.Server;
using FaceWebServer.DTO.People;
using FaceWebServer.DTO.Remote;
using FaceWebServer.Interface;
using FaceWebServer.Language;
using FaceWebServer.Utility;
using FaceWebServer.Utility.Model;
using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using Microsoft.AspNetCore.OutputCaching;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MQTTnet;
using MQTTnet.Server;
using Newtonsoft.Json;
using NPOI.HSSF.Record;
using NPOI.SS.Formula.Functions;
using NPOI.Util;
using NPOI.XWPF.UserModel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;

namespace DeviceProtocolServer.MQTTServer
{
    /// <summary>
    /// MQTT命令处理器
    /// </summary>
    public class MQTTCommandHandler
    {
        private static object lockobject = new object();

        private readonly ILogger<MQTTCommandHandler> _logger;
        private IServiceProvider _ServiceProvider;

        private ICacheService _Cache;
        private LanguageHandler _LanguageHandler;
        private IConnectIOLogService LogDB;
        private HTTPProtocolOption httpOption = null;

        private Dictionary<string, Func<MQTTCommandPacket, Task>> CommandHandlerMap;
        private MqttServer _mqtt;
        private MQTT_Client_Context _MQTT_Context;
        private Dictionary<string, string> APINameMap;
        private static MQTTOptions MQTT_Options;
        private bool IsRun = false;


        public MQTTCommandHandler(ILogger<MQTTCommandHandler> logger,
            ICacheService cache,
            IServiceProvider serviceProvider,
            IOptionsSnapshot<LanguageOption> lngopt,
            IConnectIOLogService connectIOLogService,
            IOptionsMonitor<MQTTOptions> mqttOpt,
            IOptionsMonitor<HTTPProtocolOption> httpConfig)
        {
            LogDB = connectIOLogService;
            _logger = logger;
            _Cache = cache;
            _ServiceProvider = serviceProvider;
            _LanguageHandler = lngopt.Value.GetCurrentLanguageHandler();

            MQTT_Options = mqttOpt.CurrentValue;

            httpOption = httpConfig.CurrentValue;

            APINameMap = new Dictionary<string, string>
            {
                { MQTT_Command_Define.KeepAlive,_LanguageHandler.GetRemoteService("MQTT_Command_KeepAlive") },// "心跳保活包"
                { MQTT_Command_Define.Offline, _LanguageHandler.GetRemoteService("MQTT_Command_Offline") },//"离线通知" },
                { MQTT_Command_Define.UploadWorkSetting,_LanguageHandler.GetRemoteService("MQTT_Command_UploadWorkSetting") },// "上传工作参数" },
                { MQTT_Command_Define.ReadWorkSetting, _LanguageHandler.GetRemoteService("MQTT_Command_ReadWorkSetting") },//"要求设备上传工作参数" },
                { MQTT_Command_Define.PushWorkSetting,_LanguageHandler.GetRemoteService("MQTT_Command_PushWorkSetting") },// "下发工作参数" },
                { MQTT_Command_Define.PushWorkSettingACK, _LanguageHandler.GetRemoteService("MQTT_Command_PushWorkSettingACK") },//"下发工作参数应答" },
                { MQTT_Command_Define.RemoteCommand, _LanguageHandler.GetRemoteService("MQTT_Command_RemoteCommand") },//"发送远程指令" },
                { MQTT_Command_Define.RemoteCommandACK,_LanguageHandler.GetRemoteService("MQTT_Command_RemoteCommandACK") },// "远程指令应答" },
                { MQTT_Command_Define.PushPeople, _LanguageHandler.GetRemoteService("MQTT_Command_PushPeople") },//"推送人员" },
                { MQTT_Command_Define.PushPeopleACK, _LanguageHandler.GetRemoteService("MQTT_Command_PushPeopleACK") },//"推送人员应答" },
                { MQTT_Command_Define.PushDeletePeople, _LanguageHandler.GetRemoteService("MQTT_Command_PushDeletePeople") },//"删除人员" },
                { MQTT_Command_Define.PushDeletePeopleACK, _LanguageHandler.GetRemoteService("MQTT_Command_PushDeletePeopleACK") },//"删除人员应答" },
                { MQTT_Command_Define.UploadPeople,_LanguageHandler.GetRemoteService("MQTT_Command_UploadPeople") },// "上传人员" },
                { MQTT_Command_Define.UploadPeopleACK, _LanguageHandler.GetRemoteService("MQTT_Command_UploadPeopleACK") },//"上传人员应答" },
                { MQTT_Command_Define.UploadIdentifyRecord, _LanguageHandler.GetRemoteService("MQTT_Command_UploadIdentifyRecord") },//"上传打卡记录" },
                { MQTT_Command_Define.UploadIdentifyRecordACK, _LanguageHandler.GetRemoteService("MQTT_Command_UploadIdentifyRecordACK") },//"上传打卡记录应答" },
                { MQTT_Command_Define.UploadSystemRecord,_LanguageHandler.GetRemoteService("MQTT_Command_UploadSystemRecord") },// "上传系统记录" },
                { MQTT_Command_Define.UploadSystemRecordACK, _LanguageHandler.GetRemoteService("MQTT_Command_UploadSystemRecordACK") },//"上传系统记录应答" },
                { MQTT_Command_Define.PushSoftware, _LanguageHandler.GetRemoteService("MQTT_Command_PushSoftware") },//"发送固件升级通知" },
                { MQTT_Command_Define.PushSoftwareACK,_LanguageHandler.GetRemoteService("MQTT_Command_PushSoftwareACK") },// "收到固件升级通知" },
                { MQTT_Command_Define.PushSystemFile, _LanguageHandler.GetRemoteService("MQTT_Command_PushSystemFile") },//"发送系统文件更新通知" },
                { MQTT_Command_Define.PushSystemFileACK, _LanguageHandler.GetRemoteService("MQTT_Command_PushSystemFileACK") },//"收到系统文件更新通知" },
                { MQTT_Command_Define.RegisterIdentifyTicket, _LanguageHandler.GetRemoteService("MQTT_Command_RegisterIdentifyTicket") },//"发送设备注册用户凭证通知" },
                { MQTT_Command_Define.RegisterIdentifyTicketACK, _LanguageHandler.GetRemoteService("MQTT_Command_RegisterIdentifyTicketACK") },//"设备注册用户凭证结果反馈" },
                { MQTT_Command_Define.RequestAuthorization, _LanguageHandler.GetRemoteService("MQTT_Command_RequestAuthorization") },//"设备请求服务器鉴权" },
                { MQTT_Command_Define.RequestAuthorizationACK, _LanguageHandler.GetRemoteService("MQTT_Command_RequestAuthorizationACK") },//"服务器返回鉴权结果" },
                { MQTT_Command_Define.RequestSnapshoot, _LanguageHandler.GetRemoteService("MQTT_Command_RequestSnapshoot") },//"发送获取设备摄像头快照通知" },
                { MQTT_Command_Define.RequestSnapshootACK, _LanguageHandler.GetRemoteService("MQTT_Command_RequestSnapshootACK") },//"设备返回摄像头快照" },
                { MQTT_Command_Define.DeviceAuthentication, _LanguageHandler.GetRemoteService("MQTT_Command_DeviceAuthentication") },//"设备鉴权通知" },
            };
            IniCommandHandlerMap();

        }

        private void IniCommandHandlerMap()
        {
            if (CommandHandlerMap != null) return;
            CommandHandlerMap = new Dictionary<string, Func<MQTTCommandPacket, Task>>();
            CommandHandlerMap.Add(MQTT_Command_Define.KeepAlive, Handler_Keepalive);
            CommandHandlerMap.Add(MQTT_Command_Define.Offline, Handler_Offline);
            CommandHandlerMap.Add(MQTT_Command_Define.UploadWorkSetting, Device_UploadWorkSetting);
            CommandHandlerMap.Add(MQTT_Command_Define.PushWorkSettingACK, PushWorkSettingACK);
            CommandHandlerMap.Add(MQTT_Command_Define.RemoteCommandACK, RemoteCommandACK);
            CommandHandlerMap.Add(MQTT_Command_Define.PushPeopleACK, PushPeopleACK);
            CommandHandlerMap.Add(MQTT_Command_Define.PushDeletePeopleACK, PushDeletePeopleACK);
            CommandHandlerMap.Add(MQTT_Command_Define.UploadPeople, UploadPeople);
            CommandHandlerMap.Add(MQTT_Command_Define.UploadIdentifyRecord, UploadIdentifyRecord);
            CommandHandlerMap.Add(MQTT_Command_Define.UploadSystemRecord, UploadSystemRecord);
            CommandHandlerMap.Add(MQTT_Command_Define.PushSoftwareACK, PushSoftwareACK);
            CommandHandlerMap.Add(MQTT_Command_Define.PushSystemFileACK, PushSystemFileACK);
            CommandHandlerMap.Add(MQTT_Command_Define.RegisterIdentifyTicketACK, RegisterIdentifyTicketACK);
            CommandHandlerMap.Add(MQTT_Command_Define.RequestAuthorization, RequestAuthorization);
            CommandHandlerMap.Add(MQTT_Command_Define.RequestSnapshootACK, RequestSnapshootACK);

        }


        /// <summary>
        /// MQTT 命令处理器
        /// </summary>
        /// <returns></returns>
        private async Task CommandHandler(MQTTCommandPacketParseResult packetDetail)
        {
            var packet = packetDetail.Packet;
            string cmd = packet.Cmd;

            IniCommandHandlerMap();
            await SaveConnectIOLog("Device", packetDetail);

            if (CommandHandlerMap.ContainsKey(cmd))
                await CommandHandlerMap[cmd](packet);



        }


        /// <summary>
        /// 检查设备命令队列是否需要执行命令
        /// </summary>
        /// <returns></returns>
        public async Task CheckDeviceCommandQueue(MQTT_Client_Context context, MqttServer mqtt)
        {

            _MQTT_Context = context;
            _mqtt = mqtt;

            //检查消息队列,不为空，优先处理消息队列
            if (!context.ReceivedMessage.IsEmpty)
            {
                //消息队列不为空
                while (!context.ReceivedMessage.IsEmpty)
                {
                    bool bDeq = context.ReceivedMessage.TryDequeue(out var devicePacket);
                    if (bDeq)
                    {
                        try
                        {
                            await CommandHandler(devicePacket);
                        }
                        catch (Exception ex)
                        {

                            _logger.LogWarning($"处理设备消息时发生错误 :{devicePacket.Packet.Cmd}");
                        }

                    }


                }

            }


            //检查是否有命令需要执行
            if (_MQTT_Context.CurrentCommand != null)
            {
                //检查命令是否超时
                if (_MQTT_Context.CurrentCommandTimeOutTime < DateTime.Now)
                {
                    _logger.LogWarning($"命令超时:{_MQTT_Context.CurrentCommand.Cmd}");
                    //超时 丢弃
                    _MQTT_Context.CurrentCommand = null;
                }
                else
                {
                    return; //继续等待
                }
            }


            //只有接收到设备keepalive包后才做检查
            if (!_MQTT_Context.ClientKeepliveActivate) return;

            string sSN = _MQTT_Context.DeviceSN;

            if (string.IsNullOrWhiteSpace(sSN))//设备设备SN为空，则需要发送一次设备工作参数，并在服务器端分配新SN
            {
                //发送新的SN到设备
                await PushWorkSetting();
                return;
            }
            else
            {
                //var deny = httpOption.Find(sSN);
                //if (deny != null)
                //{
                //    //await CloseConnect();
                //    return;
                //}

                if (sSN.Length != 16 || sSN == "0000000000000000")
                {
                    //设备设备不符合规则，则要求设备拉取工作参数，并在服务器端分配新SN
                    await PushWorkSetting();
                    return;
                    //return Task.CompletedTask; //继续等待
                }
                else
                {

                    #region 检查参数是否需要同步
                    var oDevice = _Cache.GetDevice(sSN);
                    #endregion

                    if (oDevice != null)
                    {
                        oDevice.MQTT_ClientID = _MQTT_Context.ClientID;

                        //添加远程操作命令
                        if (oDevice.RemoteTaskTotal > 0)
                        {
                            await RemoteCommand();
                            return;
                        }

                        //检查是否需要上传参数
                        if (oDevice.UploadStatus == 0)
                        {
                            await PushWorkSetting();
                            return;
                        }

                        bool bReadWorkSetting = false;

                        if (oDevice.UploadWorkParameterTaskTotal > 0)
                        {
                            bReadWorkSetting = true;//发送读取工作参数命令
                        }

                        if (oDevice.Protocol != DeviceDetail.MQTT)
                        {
                            bReadWorkSetting = true;//上传参数并注册
                        }

                        if (bReadWorkSetting)
                        {
                            await ReadWorkSetting(); //需要读取工作参数
                            return;
                        }


                        //添加待删除人员同步命令
                        if (oDevice.DeleteAccessTotal > 0 || oDevice.EmptyPeople > 0)
                        {
                            await PushDeletePeople();
                            return;
                        }

                        //添加待添加人员同步命令
                        if (oDevice.NewAccessTotal > 0)
                        {
                            await PushPeople();
                            return;
                        }
                    }
                    else
                    {
                        await ReadWorkSetting();//需要读取工作参数
                        return;
                    }
                }
            }

            //没有命令需要执行
            return;
        }


        /// <summary>
        /// 发送命令，并记录到上下文中
        /// </summary>
        /// <param name="packet"></param>
        /// <param name="timeout"></param>
        /// <returns></returns>
        private Task SendCommandPacket(MQTTCommandPacket packet, int timeout = 10_000)
        {

            if (_MQTT_Context.CurrentCommand == null)
            {
                if (timeout > 0)
                {
                    _MQTT_Context.CurrentCommand = packet;
                    _MQTT_Context.CurrentCommandTimeOutTime = DateTime.Now.AddMilliseconds(timeout);
                    _MQTT_Context.CurrentCommandSendTime = DateTime.Now;
                }



                return SendCommandPacketCore(packet);

            }
            else
            {
                _logger.LogWarning($"当前有命令正在执行，无法发送新命令:{_MQTT_Context.CurrentCommand.Cmd}");
                return Task.CompletedTask;
            }


        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sType"></param>
        /// <param name="packet"></param>
        /// <param name="buf"></param>
        /// <returns></returns>
        private async Task SaveConnectIOLog(string sType, MQTTCommandPacketParseResult packetDetail)
        {
            if (!httpOption.SaveIOLog)
            {
                return;
            }

            var packet = packetDetail.Packet;

            if (packet.Cmd == MQTT_Command_Define.KeepAlive)
            {
                if (!httpOption.SaveKeepaliveLog)
                {
                    return;
                }
            }

            ConnectIOLog ResponseLog = new ConnectIOLog()
            {
                Protocol = DeviceDetail.MQTT,
                APIName = APINameMap[packet.Cmd],
                HttpType = sType,
                IPAddr = _MQTT_Context.RemoteAddr,
                LogTime = DateTime.Now,
                URL = packet.Cmd,
                Method = packet.Cmd,
                ContentLength = packetDetail.PacketBufferSize,
                ContentType = "application/json; charset=utf-8",
                Body = await packet.GetBodyJson(packetDetail),
                SN = _MQTT_Context.DeviceSN,
                RequestID = packet.CmdID
            };

            if (sType == "Device")
            {
                ResponseLog.URL = _MQTT_Context.ClienPublishTopic;
            }
            else
            {
                ResponseLog.URL = _MQTT_Context.ClientSubscribeTopic;
            }
            try
            {
                await LogDB.AddConnectLogAsync(ResponseLog);
            }
            catch (Exception ex)
            {

                _logger.LogError($"保存日志时发生错误，命令:{ResponseLog.APIName} \n {ex.Message}"); ;
            }

        }

        /// <summary>
        /// 发送命令的核心代码
        /// </summary>
        /// <param name="packet"></param>
        /// <returns></returns>
        private async Task SendCommandPacketCore(MQTTCommandPacket packet)
        {
            var buf = await MQTTCommandPacketExtend.ToBuffer(packet, _MQTT_Context.PacketUseGZIP);

            _logger.LogInformation($"发布MQTT消息 {_MQTT_Context.ClientSubscribeTopic} -- cmd:{packet.Cmd}  id:{packet.CmdID}");

            try
            {
                if (httpOption.SaveIOLog)
                {
                    var result = await MQTTCommandPacketExtend.Parse(buf);
                    result.Packet = packet;
                    await SaveConnectIOLog("Server", result);
                }
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, $"发布MQTT消息 发生错误 {_MQTT_Context.ClientSubscribeTopic} -- cmd:{packet.Cmd}  id:{packet.CmdID}");

            }


            var message = new MqttApplicationMessageBuilder()
                        .WithTopic(_MQTT_Context.ClientSubscribeTopic)
                        .WithPayloadSegment(buf)
                        .Build();

            // Now inject the new message at the broker.
            await _mqtt.InjectApplicationMessage(
                new InjectedMqttApplicationMessage(message)
                {
                    SenderClientId = "@@Server",
                });

        }


        /// <summary>
        /// 检查设备是否已注册
        /// </summary>
        /// <param name="SN"></param>
        /// <param name="oDevice"></param>
        /// <returns></returns>
        private bool CheckDeviceReg(string SN, out CacheDeviceDTO oDevice)
        {
            oDevice = null;
            if (string.IsNullOrWhiteSpace(SN))
            {
                return false;
            }

            if (SN.Length < 16 || SN == "0000000000000000")
            {
                return false;
            }

            oDevice = _Cache.GetDevice(SN);
            if (oDevice == null)
            {
                return false;
            }
            return true;
        }

        #region 保活包
        /// <summary>
        /// 心跳保活包
        /// </summary>
        private Task Handler_Keepalive(MQTTCommandPacket packet)
        {
            MQTT_Command_KeepAlive cmdPck = packet as MQTT_Command_KeepAlive;
            if (cmdPck == null)
            {
                return Task.CompletedTask;
            }
            _MQTT_Context.ClientKeepliveActivate = true;

            var pckData = cmdPck.Body;

            #region 检查参数是否需要同步
            var oDevice = _Cache.GetDevice(_MQTT_Context.DeviceSN);
            #endregion

            if (oDevice != null)
            {
                oDevice.LastKeepaliveTime = DateTime.Now;
                oDevice.KeepaliveStatus = pckData;
                oDevice.MQTT_Online = true;
                oDevice.MQTT_ClientID = _MQTT_Context.ClientID;
            }

            if (pckData.RequestAuthentication.HasValue)
            {
                if (pckData.RequestAuthentication.Value == 1)
                {
                    //需要鉴权
                    var deny = httpOption.Find(_MQTT_Context.DeviceSN);
                    if (deny != null)
                    {
                        _logger.LogInformation($"发布拒绝连接的设备授权消息 - SN:{_MQTT_Context.DeviceSN}");
                        return PushDeviceAuthentication(false, deny.Code, deny.Msg);
                    }
                    else
                    {
                        return PushDeviceAuthentication(true, 0, null);
                    }
                }
            }

            return Task.CompletedTask;
        }

        /// <summary>
        /// 发送设备鉴权结果
        /// </summary>
        /// <returns></returns>
        private async Task PushDeviceAuthentication(bool success, int code, string message)
        {
            var data = new MQTT_DeviceAuthentication()
            {
                Authentication = success ? 1 : 0,
                code = code,
                ErrorMessage = message
            };
            var cmdPck = new MQTT_Command_DeviceAuthentication(data);
            await SendCommandPacket(cmdPck, 0);
        }

        /// <summary>
        /// 发送设备鉴权结果
        /// </summary>
        /// <returns></returns>
        private async Task PushDeviceAuthentication(RemoteTaskDetail taskDtl)
        {
            var SN = _MQTT_Context.DeviceSN;
            var _RemoteDB = _ServiceProvider.GetService<IDeviceRemoteService>();

            await _RemoteDB.UpdateTaskRunStatusComplete([taskDtl.TaskID], 0, SN); //更新任务状态


            var deny = httpOption.Find(SN);
            if (deny != null)
            {
                _logger.LogInformation($"发布拒绝连接的设备授权消息 - SN:{SN}");
                await PushDeviceAuthentication(false, deny.Code, deny.Msg);
            }
            else
            {
                _logger.LogInformation($"发布允许连接的设备授权消息 - SN:{SN}");
                await PushDeviceAuthentication(true, 0, null);
            }
        }

        #endregion

        #region 离线通知  

        /// <summary>
        /// 设备离线通知  
        /// </summary>
        /// <param name="packet"></param>
        /// <returns></returns>
        private Task Handler_Offline(MQTTCommandPacket packet)
        {
            MQTT_Command_Offline cmdPck = packet as MQTT_Command_Offline;
            if (cmdPck == null)
            {
                return Task.CompletedTask;
            }
            _logger.LogWarning($"MQTT 离线通知 SN： {_MQTT_Context.DeviceSN}");
            #region 检查参数是否需要同步
            var oDevice = _Cache.GetDevice(_MQTT_Context.DeviceSN);
            #endregion

            if (oDevice != null)
            {
                oDevice.MQTT_Online = false; //设置为离线
            }
            return Task.CompletedTask;
        }
        #endregion


        /// <summary>
        /// 检查命令是否为服务器发送的命令响应
        /// </summary>
        /// <param name="packet"></param>
        /// <returns></returns>
        private bool CheckPacketIsACK(string sCmdName, MQTTCommandPacket packet)
        {
            if (_MQTT_Context.CurrentCommand == null)
            {
                return false;
            }
            if (_MQTT_Context.CurrentCommand.Cmd != sCmdName)
            {
                return false;
            }


            if (_MQTT_Context.CurrentCommand.CmdID != packet.CmdID)
            {
                return false;
            }

            //命令已正确相应，将当前命令清空
            _MQTT_Context.CurrentCommand = null;
            return true;
        }


        #region 设备工作参数

        /// <summary>
        /// 读取设备工作参数
        /// </summary>
        /// <returns></returns>
        private Task ReadWorkSetting()
        {

            var cmdPck = new MQTT_Command_ReadWorkSetting();
            return SendCommandPacket(cmdPck);
        }


        /// <summary>
        /// 设备上传当前所有工作参数
        /// </summary>
        /// <param name="par"></param>
        /// <returns></returns>
        private async Task Device_UploadWorkSetting(MQTTCommandPacket packet)
        {
            MQTT_Command_UploadWorkSetting cmdPck = packet as MQTT_Command_UploadWorkSetting;
            if (cmdPck == null)
            {
                return;
            }

            string SN = _MQTT_Context.DeviceSN;
            bool bIsACK = CheckPacketIsACK(MQTT_Command_Define.ReadWorkSetting, packet);

            if (string.IsNullOrWhiteSpace(SN))
            {
                return;//没有SN不处理
            }

            if (SN.Length < 15 || SN == "0000000000000000")
            {
                return;//没有SN不处理
            }

            var cacheDevice = _Cache.GetDevice(SN);
            var par = cmdPck.Body;

            DeviceDetail dbDevice = new DeviceDetail();
            if (cacheDevice != null)
            {
                dbDevice.ID = cacheDevice.ID;
                dbDevice.Name = cacheDevice.DeviceName;

                if (cacheDevice.UploadStatus == 0 && cacheDevice.UploadWorkParameterTaskTotal == 0)
                {
                    //需要拉取参数，拒绝设备上传参数
                    return;//没有SN不处理
                }
            }
            var _DriveDB = _ServiceProvider.GetService<IFaceDriveService>();
            var _RemoteDB = _ServiceProvider.GetService<IDeviceRemoteService>();

            dbDevice.SN = SN;
            dbDevice.Protocol = DeviceDetail.MQTT;
            dbDevice.DeviceVer = par.FirmwareVerson;
            dbDevice.IsEntry = par.AccessType == 0 ? 1 : 0;
            dbDevice.Detail = JsonConvert.SerializeObject(par);

            var oDevice = await _DriveDB.Add(dbDevice);
            cacheDevice = _Cache.GetDevice(SN);
            if (cacheDevice.UploadWorkParameterTaskTotal > 0)
            {
                cacheDevice.UploadWorkParameterTaskTotal = 0;


                //更新远程操作任务，检查是否有要求设备上传参数的任务
                var remoteList = _RemoteDB.Query(new RemoteTaskQueryDTO()
                {
                    SN = SN,
                    TaskType = RemoteTypeEnum.UploadWorkSetting,
                    TaskStatus = 0
                });
                if (remoteList.TotalCount > 0)
                {
                    await _RemoteDB.UpdateTaskRunStatusComplete(
                        remoteList.DataList.Select(x => x.TaskID).ToList(),
                        oDevice.ID, oDevice.SN);
                }

            }

            await CheckDeviceCommandQueue(_MQTT_Context, _mqtt);
        }


        /// <summary>
        /// 服务器发送存储的工作参数到设备
        /// </summary>
        /// <param name="par"></param>
        /// <returns></returns>
        private async Task PushWorkSetting()
        {
            //_logger.LogInformation($" 设备拉取参数 parameter/selectParameterInfo SN:{par.DeviceID}");
            MQTT_PushWorkSetting detail;
            MQTT_Command_PushWorkSetting cmdPck;
            var _DriveDB = _ServiceProvider.GetService<IFaceDriveService>();
            var sSN = _MQTT_Context.DeviceSN;
            //设置设备工作参数为默认值
            void setDef()
            {
                lock (lockobject)
                {
                    var defValue = _DriveDB.GetDefaultValue(DeviceDetail.MQTT);
                    var defDevice = JsonConvert.DeserializeObject<MQTT_PushWorkSetting>(defValue);
                    //从数据库拉取默认值发过去
                    defDevice.ProductionDate = DateTime.Now.ToDateTimeStr();

                    defDevice.DeviceName = "-";

                    DeviceDetail dbDevice = new DeviceDetail();
                    dbDevice.SN = defDevice.DeviceSN;
                    dbDevice.Protocol = DeviceDetail.MQTT;
                    dbDevice.DeviceVer = string.Empty;
                    dbDevice.IsEntry = defDevice.AccessType == 0 ? 1 : 0;
                    dbDevice.Detail = JsonConvert.SerializeObject(defDevice);

                    //将设备加入到列表中
                    _DriveDB.Add(dbDevice);

                    detail = defDevice.Adapt<MQTT_PushWorkSetting>();
                    string sSNNum = defDevice.DeviceSN.Substring(10, 6);
                    sSNNum = (sSNNum.ToInt32() + 1).ToString("000000");

                    defDevice.DeviceSN = defDevice.DeviceSN.Substring(0, 10) + sSNNum;
                    defValue = JsonConvert.SerializeObject(defDevice);

                    var setPar = new SetDefaultValueRequestDTO()
                    {
                        Protocol = DeviceDetail.MQTT,
                        DefaultJson = defValue
                    };
                    _DriveDB.SaveDefaultValue(setPar);
                }

            }



            if (string.IsNullOrWhiteSpace(sSN))
            {
                setDef();

            }
            else
            {
                if (sSN.Length < 16 || sSN == "0000000000000000")
                {
                    setDef();
                }
                else
                {
                    var chkRet = CheckDeviceReg(sSN, out CacheDeviceDTO oCacheDevice);
                    if (chkRet == null) return;
                    var oDBDevice = _DriveDB.Find<DeviceDetail>(oCacheDevice.ID);

                    detail = JsonConvert.DeserializeObject<MQTT_PushWorkSetting>(oDBDevice.Detail);

                    detail.DeviceName = oDBDevice.Name;
                }
            }

            detail.SystemTime = TimestampUtility.ToUnixTimestampBySeconds(DateTime.Now);


            var _TimeGroupDB = _ServiceProvider.GetService<ITimeGroupService>();
            var _HolidayDB = _ServiceProvider.GetService<IHolidayService>();
            var _AlarmClockDB = _ServiceProvider.GetService<IAlarmClockService>();

            detail.TimeGroups = _TimeGroupDB.GetAll().Adapt<List<HTTPOpenDoorTimegroupV2>>();
            detail.Holidays = _HolidayDB.GetAllList().Adapt<List<HTTPDeviceHolidayDay>>();
            detail.AlarmClocks = _AlarmClockDB.GetAllList().Adapt<List<HTTPAlarmClockTime>>();

            //使用精简的时段定义
            foreach (var tGroup in detail.TimeGroups)
            {
                tGroup.Week1 = SimplifyWeekTimeSection(tGroup.Week1);
                tGroup.Week2 = SimplifyWeekTimeSection(tGroup.Week2);
                tGroup.Week3 = SimplifyWeekTimeSection(tGroup.Week3);
                tGroup.Week4 = SimplifyWeekTimeSection(tGroup.Week4);
                tGroup.Week5 = SimplifyWeekTimeSection(tGroup.Week5);
                tGroup.Week6 = SimplifyWeekTimeSection(tGroup.Week6);
                tGroup.Week7 = SimplifyWeekTimeSection(tGroup.Week7);
            }


            cmdPck = new MQTT_Command_PushWorkSetting(detail);
            await SendCommandPacket(cmdPck, 20_000);
        }

        /// <summary>
        /// 设备确认收到工作参数
        /// </summary>
        /// <returns></returns>
        private Task PushWorkSettingACK(MQTTCommandPacket packet)
        {
            string SN = _MQTT_Context.DeviceSN;
            bool bIsACK = CheckPacketIsACK(MQTT_Command_Define.PushWorkSetting, packet);

            if (bIsACK)
            {
                var _DriveDB = _ServiceProvider.GetService<IFaceDriveService>();

                var chkRet = CheckDeviceReg(SN, out CacheDeviceDTO oCacheDevice);
                if (chkRet == null) return Task.CompletedTask;
                var oDBDevice = _DriveDB.Find<DeviceDetail>(oCacheDevice.ID);

                var detail = JsonConvert.DeserializeObject<MQTT_PushWorkSetting>(oDBDevice.Detail);


                oCacheDevice.UploadStatus = 1;
                oDBDevice.UploadStatus = 1; //更新为已同步
                oDBDevice.UploadStatusTime = DateTime.Now;
                _DriveDB.Commit();
                //替换设备
                if (oDBDevice.SN != detail.DeviceSN)
                {
                    if (detail.DeviceSN == "0000000000000000")
                    {
                        //删除
                        _DriveDB.Delete(new List<int>() { oCacheDevice.ID });

                    }
                    else
                    {
                        //更新SN
                        _DriveDB.ReplaceDeviceSN(oDBDevice, detail.DeviceSN);

                    }
                }
                else
                {
                    detail.DeviceName = oDBDevice.Name;
                }

            }

            return CheckDeviceCommandQueue(_MQTT_Context, _mqtt);
        }

        /// <summary>
        /// 精简每周的时段定义  01:00-01:59/02:00-02:59/01:00-01:59/02:00-02:59
        /// </summary>
        /// <param name="weekTimeSection"></param>
        /// <returns></returns>
        private string SimplifyWeekTimeSection(string weekTimeSection)
        {
            if (string.IsNullOrEmpty(weekTimeSection))
                return null;
            string sDefaultTimeSection = "00:00-00:00";
            string[] times = weekTimeSection.SplitTrim("/");
            Queue<string> useTimes = new Queue<string>(8);
            foreach (var t in times)
            {
                if (!sDefaultTimeSection.Equals(t))
                {
                    useTimes.Enqueue(t);
                }
            }
            if (useTimes.Count > 0)
                return string.Join("/", useTimes.ToArray());
            else
                return null;

        }

        #endregion

        #region 远程操作指令
        /// <summary>
        /// 服务器检测到有需要下发的远程操作时，发送此命令
        /// </summary>
        /// <returns></returns>
        private Task RemoteCommand()
        {
            var SN = _MQTT_Context.DeviceSN;

            var chkRet = CheckDeviceReg(SN, out CacheDeviceDTO oDevice);
            if (!chkRet) return Task.CompletedTask;

            var _RemoteDB = _ServiceProvider.GetService<IDeviceRemoteService>();

            var oRemoteList = _RemoteDB.GetRemoteTaskBySN(SN);

            var result = new MQTT_RemoteCommand();
            int lWait = 3000;
            foreach (var item in oRemoteList)
            {
                /// 任务类型 ；
                /// 1，远程开门；2、远程关门；3、远程常开；4、锁定；5、解除锁定；6、关闭报警
                /// 10、远程重启；11、恢复出厂设置； 12、重新上传所有记录；13、清空所有记录；
                /// 20、上传所有人员；21、上传指定用户号的人员；
                /// 
                /// 100、清空所有人员；101、上传工作参数;
                /// 
                switch (item.TaskType)
                {
                    /// 1，远程开门；2、远程关门；3、远程常开；4、锁定；5、解除锁定；6、关闭报警
                    case RemoteTypeEnum.OpenDoor:
                        result.Opendoor = 1;//1--打开继电器
                        break;
                    case RemoteTypeEnum.CloseDoor:
                        result.Opendoor = 3;//3--关闭门(解除常开)
                        break;
                    case RemoteTypeEnum.KeepOpen:
                        result.Opendoor = 2;//2--使门常开
                        break;
                    case RemoteTypeEnum.LockDoor:
                        result.Opendoor = 4;//4--锁定门
                        break;
                    case RemoteTypeEnum.UnlockDoor:
                        result.Opendoor = 5;// 5--解除门锁定
                        break;
                    case RemoteTypeEnum.CloseAlarm:
                        result.Closealarm = 1;
                        break;

                    /// 10、远程重启；11、恢复出厂设置；12、清空所有人员；13、上传工作参数; 14、重新上传所有记录；15、清空所有记录；
                    case RemoteTypeEnum.Restart://10、远程重启
                        result.Restart = 1;
                        break;
                    case RemoteTypeEnum.Recover://11、恢复出厂设置
                        result.Recover = 1;
                        lWait = 120000;
                        break;
                    case RemoteTypeEnum.RepostRecord://12、重新上传所有记录
                        result.RepostRecord = 1;
                        break;
                    case RemoteTypeEnum.ClearRecord://13、清空所有记录
                        result.ClearRecord = 1;
                        lWait = 120000;
                        break;

                    //
                    case RemoteTypeEnum.PushSoftware:
                        return PushSoftware(item);//固件升级
                    case RemoteTypeEnum.PushSystemFile:
                        return PushSystemFile(item);//推送系统文件
                    case RemoteTypeEnum.Snapshoot:
                        return RequestSnapshoot(item);//获取摄像头快照
                    case RemoteTypeEnum.DeviceAuthorization://设备授权消息
                        return PushDeviceAuthentication(item);//获取摄像头快照

                    /// 20、上传所有人员；21、上传指定用户号的人员；
                    case RemoteTypeEnum.PushAllPeople://20、上传所有人员
                        result.PushAllPeople = 1;
                        break;
                    case RemoteTypeEnum.QueryPeople://21、上传指定用户号的人员
                        if (item.UserID.HasValue)
                        {
                            if (result.QueryPeople == null)
                                result.QueryPeople = new HashSet<long>();

                            result.QueryPeople.Add(item.UserID.Value);
                        }

                        break;
                    case RemoteTypeEnum.RegisterIdentifyTicket:
                        return RegisterIdentifyTicket(item);//注册识别凭证


                    /// 1，远程开门；2、远程关门；3、远程常开；4、锁定；5、解除锁定；6、关闭报警
                    case RemoteTypeEnum.Elevator_OpenRelay:
                        result.OpenElevatorPort = item.TaskExtension;//1--打开继电器
                        break;
                    case RemoteTypeEnum.Elevator_CloseRelay:
                        result.CloseElevatorPort = item.TaskExtension;//3--关闭门(解除常开)
                        break;
                    case RemoteTypeEnum.Elevator_KeepOpenRelay:
                        result.KeepOpenElevatorPort = item.TaskExtension;//2--使门常开
                        break;
                    case RemoteTypeEnum.Elevator_LockRelay:
                        result.LockElevatorPort = item.TaskExtension;//4--锁定门
                        break;
                    case RemoteTypeEnum.Elevator_UnlockRelay:
                        result.UnlockElevatorPort = item.TaskExtension;// 5--解除门锁定
                        break;


                    default:
                        break;
                }

            }

            _Cache.Set($"{SN}_MQTT_RemoteCommand", oRemoteList.Select(x => x.TaskID).ToList());

            return SendCommandPacket(new MQTT_Command_RemoteCommand(result));
        }

        /// <summary>
        /// 设备确认收到远程指令
        /// </summary>
        /// <returns></returns>
        private async Task RemoteCommandACK(MQTTCommandPacket packet)
        {
            string SN = _MQTT_Context.DeviceSN;
            bool bIsACK = CheckPacketIsACK(MQTT_Command_Define.RemoteCommand, packet);

            var chkRet = CheckDeviceReg(SN, out CacheDeviceDTO oDevice);
            if (!chkRet) return;

            if (bIsACK)
            {
                var _RemoteDB = _ServiceProvider.GetService<IDeviceRemoteService>();

                var taskIDList = _Cache.Get<List<int>>($"{SN}_MQTT_RemoteCommand");
                await _RemoteDB.UpdateTaskRunStatusComplete(taskIDList,
                oDevice.ID, oDevice.SN); //更新任务状态
            }
        }
        #endregion


        #region 拉取人员
        /// <summary>
        ///  设备拉取人员授权信息
        /// </summary>
        private async Task PushPeople()
        {
            Stopwatch swTime = new Stopwatch();
            swTime.Start();

            //_logger.LogInformation($" 设备拉取人事信息 ReadPeople SN:{par.DeviceID}");
            var SN = _MQTT_Context.DeviceSN;

            List<MQTT_PushPeople> PeopleList = new();
            CacheDeviceDTO oDevice = null;
            var chkRet = CheckDeviceReg(SN, out oDevice);
            if (!chkRet) return;
            int Limit = 1;

            //客户端不使用压缩时，下发的人员中，人员照片强子不使用字节流方式传输
            bool MQTTPeoplePhotoUseBytes = httpOption.MQTTPeoplePhotoUseBytes;
            if (_MQTT_Context.PacketUseGZIP == false)
            {
                MQTTPeoplePhotoUseBytes = false;

            }

            if (!MQTTPeoplePhotoUseBytes)
            {
                if (httpOption.UploadPeoplePhotoType == HTTPProtocolOption.UseURL)
                {
                    Limit = 100;
                }

            }

            //检查有没有需要添加的人员
            var TotalDetail = oDevice.NewAccessTotal;

            if (TotalDetail == 0)
            {
                return;//没有需要上传的人员
            }

            int lWatiTime = 1000 + 1500 * Limit;

            var _AccessDB = _ServiceProvider.GetService<IDeviceAccessService>();

            //从数据库中获取需要导入的人员
            var accessList = await _AccessDB.GetDownloadAccess(oDevice.ID, Limit);

            if (accessList != null)
            {
                if (accessList.Count > 0)
                    PeopleList = new List<MQTT_PushPeople>(accessList.Count);

                var peopleMap = _Cache.GetPeopleDictionary();
                IMapper mapper = new Mapper();
                foreach (var access in accessList)
                {
                    MQTT_PushPeople pushPeople = new MQTT_PushPeople();

                    HTTPPeopleV2 httpPeople = pushPeople;
                    var dbPeople = peopleMap[access.PeopleID];
                    mapper.Map(access, httpPeople);
                    mapper.Map(dbPeople, httpPeople);

                    PeopleList.Add(pushPeople);
                }
            }

            ArraySegment<byte> imageBuf = null;


            if (MQTTPeoplePhotoUseBytes) //使用字节流传输
            {
                if (PeopleList.Count > 0)
                {
                    foreach (var people in PeopleList)
                    {
                        if (people.PhotoLen > 0)
                        {
                            //读取文件，转为Base64
                            var sFile = FileHelpers.GetPeopleImagePath(people.Photo);

                            //检查文件是否存在
                            if (!string.IsNullOrEmpty(sFile) && System.IO.File.Exists(sFile))
                            {
                                imageBuf = System.IO.File.ReadAllBytes(sFile);

                                people.PhotoLen = imageBuf.Count;
                            }
                            else
                            {
                                people.PhotoLen = 0;
                                people.Photo = string.Empty;
                                people.PhotoMD5 = string.Empty;
                            }
                        }
                    }
                }
            }
            else
            {

                if (httpOption.UploadPeoplePhotoType == HTTPProtocolOption.UseURL)
                {//使用URL下发，需要增加文件的url前缀
                    if (PeopleList.Count > 0)
                    {
                        foreach (var people in PeopleList)
                        {
                            if (people.PhotoLen > 0)
                            {
                                people.Photo = $"{httpOption.PeopleURLPrefix}{people.Photo}";
                            }
                        }
                    }
                }
                else //使用base64传输
                {
                    if (PeopleList.Count > 0)
                    {
                        foreach (var people in PeopleList)
                        {
                            if (people.PhotoLen > 0)
                            {
                                //读取文件，转为Base64
                                var sFile = FileHelpers.GetPeopleImagePath(people.Photo);

                                //检查文件是否存在
                                if (!string.IsNullOrEmpty(sFile) && System.IO.File.Exists(sFile))
                                {
                                    var fileBuf = System.IO.File.ReadAllBytes(sFile);
                                    people.Photo = Convert.ToBase64String(fileBuf);
                                    people.PhotoLen = fileBuf.Length;
                                }
                                else
                                {
                                    people.PhotoLen = 0;
                                    people.Photo = string.Empty;
                                    people.PhotoMD5 = string.Empty;
                                }
                            }
                        }
                    }

                }
            }


            if (httpOption.UploadPeoplePhotoMd5 == false)
            {
                //消除md5
                foreach (var people in PeopleList)
                {
                    if (people.PhotoLen > 0)
                    {
                        people.PhotoMD5 = string.Empty;

                    }
                }
            }


            //使用URL传输特征码
            if (httpOption.FeatureCodeUseBase64 == false)
            {
                if (PeopleList.Count > 0)
                {
                    foreach (var people in PeopleList)
                    {

                        if (!string.IsNullOrEmpty(people.FaceFeature))
                        {
                            people.FaceFeature = $"{httpOption.PeopleURLPrefix}{people.FaceFeature}";
                        }
                        if (people.Fingerprints != null && people.Fingerprints.Count > 0)
                        {
                            foreach (var item in people.Fingerprints)
                            {
                                if (!string.IsNullOrEmpty(item.Data))
                                {
                                    item.Data = $"{httpOption.PeopleURLPrefix}{item.Data}";
                                }
                            }
                        }
                        if (people.Palmveins != null && people.Palmveins.Count > 0)
                        {
                            foreach (var item in people.Palmveins)
                            {
                                if (!string.IsNullOrEmpty(item.Data))
                                {
                                    item.Data = $"{httpOption.PeopleURLPrefix}{item.Data}";
                                }
                            }
                        }
                    }
                }
            }
            else //使用 base64 传输
            {
                if (PeopleList.Count > 0)
                {
                    var peopleService = _ServiceProvider.GetService<IPeopleService>();
                    foreach (var people in PeopleList)
                    {

                        await LoadFeatureCode(people, peopleService);

                    }
                }
            }

            var cmdPck = new MQTT_Command_PushPeople(PeopleList, imageBuf);

            await SendCommandPacket(cmdPck, lWatiTime);
            swTime.Stop();
            if (PeopleList.Count == 1)
            {
                _logger.LogInformation($"服务器推送人员信息耗时:{swTime.ElapsedMilliseconds}ms, 用户号：{PeopleList.First().UserID}");
            }
            else if (PeopleList.Count > 1)
            {
                _logger.LogInformation($"服务器推送人员信息耗时:{swTime.ElapsedMilliseconds}ms,共{PeopleList.Count}人");
            }


        }

        private async Task LoadFeatureCode(HTTPPeopleV2 hPeople, IPeopleService service)
        {
            PeopleDTO dto = new PeopleDTO()
            {
                UserID = hPeople.UserID
            };
            if (!string.IsNullOrEmpty(hPeople.FaceFeature))
            {
                dto.FaceFeature = new PeopleFeatureCode()
                {
                    Data = hPeople.FaceFeature,
                    MD5 = hPeople.FaceFeatureMD5
                };
            }
            if (hPeople.Fingerprints != null)
            {
                dto.Fingerprints = new List<PeopleFeatureCode>(hPeople.Fingerprints);
            }
            if (hPeople.Palmveins != null)
            {
                dto.Palmveins = new List<PeopleFeatureCode>(hPeople.Palmveins);
            }
            await service.LoadFeatureCode(dto);

            if (!string.IsNullOrEmpty(hPeople.FaceFeature))
            {
                hPeople.FaceFeature = dto.FaceFeature.Data;
                hPeople.FaceFeatureMD5 = dto.FaceFeature.MD5;
            }
        }


        /// <summary>
        /// 设备拉取人员后反馈人员保存结果
        /// </summary>
        private async Task PushPeopleACK(MQTTCommandPacket packet)
        {
            Stopwatch swTime = new Stopwatch();
            swTime.Start();

            MQTT_Command_PushPeopleACK cmdPck = packet as MQTT_Command_PushPeopleACK;
            if (cmdPck == null)
            {
                return;
            }

            string SN = _MQTT_Context.DeviceSN;
            var par = cmdPck.Body;
            par.SN = SN;
            bool bIsACK = CheckPacketIsACK(MQTT_Command_Define.PushPeople, packet);
            if (!bIsACK)
            {
                if (_MQTT_Context.CurrentCommand != null)
                {
                    _logger.LogError($"PushPeopleACK  CmdID和等待的CmdID不匹配 wait:{_MQTT_Context.CurrentCommand.CmdID}  read:{packet.CmdID}");
                }

                return; //不是当前命令相应，退出
            }


            if (httpOption.UploadPeopleSaveResult == false)
            {
                //不保存人员存储结果
                await PushPeople();//继续发送
                return;
            }


            //获取设备拉取的人员列表
            var downloadAccessList = _Cache.GetPeopleAccessList(par.SN);
            if (downloadAccessList == null) return;//没有找到上传的人员列表
            if (downloadAccessList.Count == 0) return;

            var chkRet = CheckDeviceReg(SN, out CacheDeviceDTO oDevice);
            if (!chkRet) return;

            //key是userid
            Dictionary<long, DeviceAccessUploadStatusUpdateDTO> peopleInfos =
                downloadAccessList.ToDictionary(x => x.Key,
                x => new DeviceAccessUploadStatusUpdateDTO()
                {
                    AccessID = x.Value,
                    UploadResult = 1,
                    RepeatID = 0,
                    UploadResultMsg = string.Empty,
                });
            if (par.FailCount > 0)
            {
                foreach (var item in par.FailList)
                {
                    if (peopleInfos.ContainsKey(item.UserID))
                    {
                        var p = peopleInfos[item.UserID];
                        p.UploadResult = 1000 + item.ErrorCode;
                        p.UploadResultMsg = item.ErrMsg;
                        p.RepeatID = item.RepeatID;
                    }

                }
            }

            var _AccessDB = _ServiceProvider.GetService<IDeviceAccessService>();
            await _AccessDB.UpdatePeopleAccessUploadResult(oDevice.ID, peopleInfos.Values.ToList());

            swTime.Stop();

            _logger.LogInformation($"保存推送人员结果耗时:{swTime.ElapsedMilliseconds}ms");

            await PushPeople();//继续发送
        }
        #endregion

        #region 删除人员

        /// <summary>
        /// 服务器下发待删除人员名单
        /// </summary>
        private async Task PushDeletePeople()
        {
            //_logger.LogInformation($" 设备拉取人事信息 ReadPeople SN:{par.DeviceID}");
            var SN = _MQTT_Context.DeviceSN;

            List<MQTT_PushPeople> PeopleList = new();

            var chkRet = CheckDeviceReg(SN, out CacheDeviceDTO oDevice);
            if (!chkRet) return;


            MQTT_PushDeletePeople rst = new();

            //检查有没有需要删除的人员

            if (oDevice.DeleteAccessTotal == 0 && oDevice.EmptyPeople == 0)
            {
                return;//"没有需要删除的人员!";
            }

            var _AccessDB = _ServiceProvider.GetService<IDeviceAccessService>();
            int lWatiTime = 1000;
            if (oDevice.EmptyPeople > 0)
            {
                //需要删除所有人员
                rst.DeleteAll = 1;
                lWatiTime = 120000;
            }
            else
            {
                int Limit = 50;
                lWatiTime = 1000 + 300 * Limit;

                //从数据库中获取需要删除的人员
                var oDeleteMap = await _AccessDB.GetDeleteAccess(oDevice.ID, Limit);
                rst.DeleteCount = oDeleteMap.Count;
                if (oDeleteMap.Count > 0)
                {
                    rst.DeleteList = oDeleteMap.Values.ToList();
                }


            }
            await SendCommandPacket(new MQTT_Command_PushDeletePeople(rst), lWatiTime);



        }

        /// <summary>
        /// 删除人员反馈   设备从服务器拉取的待删除的人员操作后的结果反馈
        /// </summary>
        private async Task PushDeletePeopleACK(MQTTCommandPacket packet)
        {
            //_logger.LogInformation($" 设备推送删除人员操作结果 ReadDeletePeople SN:{par.DeviceID}");

            MQTT_Command_PushDeletePeopleACK cmdPck = packet as MQTT_Command_PushDeletePeopleACK;
            if (cmdPck == null)
            {
                return;
            }

            string SN = _MQTT_Context.DeviceSN;
            var par = cmdPck.Body;
            bool bIsACK = CheckPacketIsACK(MQTT_Command_Define.PushDeletePeople, packet);
            if (!bIsACK) return; //不是当前命令相应，退出

            var chkRet = CheckDeviceReg(SN, out CacheDeviceDTO oDevice);
            if (!chkRet) return;


            var _AccessDB = _ServiceProvider.GetService<IDeviceAccessService>();
            var _RemoteDB = _ServiceProvider.GetService<IDeviceRemoteService>();

            if (par.DeleteAll == 1)
            {
                //删除已所有人员

                //更新远程操作任务，检查是否有要求设备上传参数的任务
                var remoteList = _RemoteDB.Query(new RemoteTaskQueryDTO()
                {
                    SN = SN,
                    TaskType = RemoteTypeEnum.ClearAllPeople,
                    TaskStatus = 0
                });
                if (remoteList.TotalCount > 0)
                {
                    await _RemoteDB.UpdateTaskRunStatusComplete(
                        remoteList.DataList.Select(x => x.TaskID).ToList(),
                        oDevice.ID, oDevice.SN);
                }

                //更新设备权限
                await _AccessDB.ReuploadByDevice(oDevice.ID);
                oDevice.EmptyPeople = 0;

                await CheckDeviceCommandQueue(_MQTT_Context, _mqtt);//继续发送
                return;
            }


            //获取上次拉取的待删除人员列表
            var cacheDeleteMap = _Cache.GetDeletePeopleAccessList(SN);
            if (cacheDeleteMap == null) return;
            if (cacheDeleteMap.Count == 0) return;

            await _AccessDB.SaveDeleteAccessResult(oDevice.ID, cacheDeleteMap.Values.ToList());

            await PushDeletePeople();//继续发送
            return;
        }
        #endregion


        #region 设备推送人员
        /// <summary>
        /// 设备推送人员到服务器
        /// </summary>
        private async Task UploadPeople(MQTTCommandPacket packet)
        {
            MQTT_Command_UploadPeople cmdPck = packet as MQTT_Command_UploadPeople;
            if (cmdPck == null)
            {
                return;
            }

            string SN = _MQTT_Context.DeviceSN;
            var par = cmdPck.Body;


            var chkRet = CheckDeviceReg(SN, out CacheDeviceDTO oDevice);
            if (!chkRet) return;


            if (par.PushType == 4 && par.Detail == null)//在设备中查询人员，但是人员存在
            {
                //设备中不存在指定的用户号
                _logger.LogInformation($"设备推送人员 设备中查询不到指定的用户号:{par.UserID}");

                //发送响应,不需要等待响应
                await SendCommandPacketCore(new MQTT_Command_UploadPeopleACK(packet.CmdID));

                return;
            }
            if (par.PushType == 3)//在设备中删除人员消息
            {
                //设备中不存在指定的用户号
                _logger.LogInformation($"设备推送人员 删除人员消息:{par.UserID}");

                //发送响应,不需要等待响应
                await SendCommandPacketCore(new MQTT_Command_UploadPeopleACK(packet.CmdID));

                return;
            }

            string sSavePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "PushPeople");
            if (par.Detail != null)
            {


                string sImageFileName = $"{par.Detail.UserID}.jpg";
                string sTmpFile = string.Empty;
                if (Directory.Exists(sSavePath) == false)
                {
                    Directory.CreateDirectory(sSavePath);
                }

                sTmpFile = Path.Combine(sSavePath, sImageFileName);


                var imageBuf = packet.GetDataBuf();
                if (imageBuf != null)
                {

                    _logger.LogInformation($"MQTT 设备推送人员 用户号： {par.Detail.UserID} 有照片 ，照片长度：{imageBuf.Count}");
                }
                else
                {
                    //设备可能使用base64编码照片
                    if (par.Detail.Photo.Length > 1000)
                    {
                        //照片采用的是base64编码
                        try
                        {
                            imageBuf = Convert.FromBase64String(par.Detail.Photo);
                            _logger.LogInformation($"MQTT 设备推送人员 用户号： {par.Detail.UserID} 有照片,照片使用base64编码 ，照片长度：{imageBuf.Count}");
                        }
                        catch (Exception)
                        {
                            _logger.LogError($"MQTT 设备推送人员 用户号： {par.Detail.UserID} 有照片,照片使用base64编码 ，照片解码失败");
                            imageBuf = null;
                        }

                    }
                }

                if (imageBuf != null)
                {
                    //保存图片
                    await FileHelpers.WriteArraySegmentToFileAsync(imageBuf, sTmpFile);

                    par.Detail.PhotoLen = imageBuf.Count;
                    if (string.IsNullOrEmpty(par.Detail.PhotoMD5))
                    {
                        par.Detail.PhotoMD5 = MD5Helper.GetByteBufMD5ByHex(imageBuf);
                    }
                    par.Detail.Photo = $"/People/{sImageFileName}?md5={par.Detail.PhotoMD5}";
                }
                else
                {
                    par.Detail.PhotoLen = 0;
                    par.Detail.PhotoMD5 = string.Empty;
                    par.Detail.Photo = string.Empty;
                    sTmpFile = string.Empty;
                }

                var _PeopleDB = _ServiceProvider.GetService<IPeopleService>();
                //检查用户号是否存在
                var cachePeople = _Cache.GetPeopleCache(par.UserID);
                JsonResultModel addRet = null;
                if (cachePeople == null)
                {
                    await SaveFeatureCode(par.Detail, _PeopleDB);
                    var newPeople = par.Detail.Adapt<FaceWebServer.DB.Table.People>();

                    //新增用户
                    addRet = await _PeopleDB.AddNew(newPeople, null);
                }
                else
                {
                    await DeleteFeatureCode(par.Detail, _PeopleDB);
                    await SaveFeatureCode(par.Detail, _PeopleDB);

                    //更新人员
                    var upPeople = par.Detail.Adapt<FaceWebServer.DB.Table.People>();
                    upPeople.ID = cachePeople.ID;
                    upPeople.CreateTime = cachePeople.CreateTime;
                    addRet = await _PeopleDB.UpdatePeople(upPeople, null, false);



                }

                if (addRet.Result == true)
                {
                    //移动人员照片
                    string sNewFile = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "People", sImageFileName);
                    try
                    {
                        if (System.IO.File.Exists(sNewFile))
                            System.IO.File.Delete(sNewFile);


                        if (System.IO.File.Exists(sTmpFile))
                            System.IO.File.Move(sTmpFile, sNewFile);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError("设备推送人员，保存人员入库时发生错误！" + ex.Message);
                    }

                    //发送响应,不需要等待响应
                    await SendCommandPacketCore(new MQTT_Command_UploadPeopleACK(packet.CmdID));

                }
                else
                {
                    _logger.LogError("设备推送人员，保存人员入库时发生错误！" + addRet.Error);
                    //发送响应,不需要等待响应
                    await SendCommandPacketCore(new MQTT_Command_UploadPeopleACK(packet.CmdID));
                }
            }
            else
            {
                //设备中不存在指定的用户号
                _logger.LogInformation($"设备推送人员 设备中查询不到指定的用户号:{par.UserID}   类型：{par.PushType}");

                //发送响应,不需要等待响应
                await SendCommandPacketCore(new MQTT_Command_UploadPeopleACK(packet.CmdID));
            }
        }

        private async Task SaveFeatureCode(HTTPPeopleV2 hPeople, IPeopleService service)
        {
            PeopleDTO dto = new PeopleDTO()
            {
                UserID = hPeople.UserID
            };
            if (!string.IsNullOrEmpty(hPeople.FaceFeature))
            {
                dto.FaceFeature = new PeopleFeatureCode()
                {
                    Data = hPeople.FaceFeature,
                    MD5 = hPeople.FaceFeatureMD5
                };
            }
            if (hPeople.Fingerprints != null)
            {
                dto.Fingerprints = new List<PeopleFeatureCode>(hPeople.Fingerprints);
            }
            if (hPeople.Palmveins != null)
            {
                dto.Palmveins = new List<PeopleFeatureCode>(hPeople.Palmveins);
            }
            await service.SaveFeatureCode(dto);

            if (!string.IsNullOrEmpty(hPeople.FaceFeature))
            {
                hPeople.FaceFeature = dto.FaceFeature.Data;
                hPeople.FaceFeatureMD5 = dto.FaceFeature.MD5;
            }
        }

        private Task DeleteFeatureCode(HTTPPeopleV2 hPeople, IPeopleService service)
        {
            PeopleDTO dto = new PeopleDTO()
            {
                UserID = hPeople.UserID
            };
            return service.DeleteFeatureCode(dto);
        }

        #endregion


        #region 推送记录

        #region 推送打卡记录
        /// <summary>
        /// 设备推送打卡记录
        /// </summary>
        /// <returns></returns>
        private async Task UploadIdentifyRecord(MQTTCommandPacket packet)
        {

            MQTT_Command_UploadIdentifyRecord cmdPck = packet as MQTT_Command_UploadIdentifyRecord;
            if (cmdPck == null)
            {
                return;
            }



            string SN = _MQTT_Context.DeviceSN;
            var record = cmdPck.Body;
            var deny = httpOption.Find(SN);
            if (deny != null)
            {
                _logger.LogInformation($"MQTT 设备上传打卡记录 记录ID：{record.RecordID} 命令ID:{cmdPck.CmdID} 被拒绝!");
                return;
            }
           
            var imgBuf = cmdPck.GetDataBuf();

            if (imgBuf == null)
            {
                if (record.Photo.Length > 1000)
                {
                    try
                    {
                        imgBuf = new ArraySegment<byte>(Convert.FromBase64String(record.Photo));
                        _logger.LogInformation($"MQTT 设备上传打卡记录 {cmdPck.CmdID} 有现场照片 使用base64编码照片，照片长度：{imgBuf.Count}");
                    }
                    catch (Exception)
                    {

                        _logger.LogError($"MQTT 设备上传打卡记录 {cmdPck.CmdID} 有现场照片 使用base64编码照片，照片解码失败");
                        imgBuf = null;
                    }

                }
                else
                {
                    _logger.LogInformation($"MQTT 设备上传打卡记录 {cmdPck.CmdID} 无现场照片");
                }


            }
            else
            {
                _logger.LogInformation($"MQTT 设备上传打卡记录 {cmdPck.CmdID} 有现场照片，照片长度：{imgBuf.Count}");
            }



            var oRecordDate = TimestampUtility.ToLocalTimeDateBySeconds(record.RecordDate);

            if (imgBuf != null)
            {
                string sPath = Path.Combine(Directory.GetCurrentDirectory(),
                  "wwwroot", "RecordImage", SN, oRecordDate.ToString("yyyyMM"),
                    oRecordDate.ToString("dd"));

                if (Directory.Exists(sPath) == false)
                    Directory.CreateDirectory(sPath);

                string sFileName = $"{oRecordDate.ToString("yyyy_MM_dd_HH_mm_ss")}.jpg";

                string sSaveFile = Path.Combine(sPath, sFileName);

                //保存图片
                await FileHelpers.WriteArraySegmentToFileAsync(imgBuf, sSaveFile);

                if (imgBuf != null)
                {
                    record.PhotoLen = imgBuf.Count;
                    record.Photo = $"/RecordImage/{SN}/{oRecordDate:yyyyMM}/{oRecordDate:dd}/{sFileName}";
                }
                else
                {
                    record.PhotoLen = 0;
                    record.Photo = string.Empty;
                }
            }

            var _RecordDB = _ServiceProvider.GetService<IRecordService>();


            if (record.BodyTemp > 0)
                record.BodyTemp = record.BodyTemp / 10;
            if (string.IsNullOrEmpty(record.CardNum))
            {
                record.CardNum = "0";
            }

            try
            {
                var dbRecord = record.Adapt<IdentifyRecord>();
                dbRecord.SN = SN;
                await _RecordDB.AddRecord(dbRecord);
                _logger.LogInformation($"设备上传打卡记录 \n {JsonConvert.SerializeObject(record, Formatting.Indented)}");
                await SendCommandPacketCore(new MQTT_Command_UploadIdentifyRecordACK(cmdPck.CmdID));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"保存打卡记录异常 \n {JsonConvert.SerializeObject(record, Formatting.Indented)}");


            }



        }


        #endregion



        #region 推送系统记录
        /// <summary>
        /// 设备推送系统记录
        /// </summary>
        /// <returns></returns>
        private async Task UploadSystemRecord(MQTTCommandPacket packet)
        {
            MQTT_Command_UploadSystemRecord cmdPck = packet as MQTT_Command_UploadSystemRecord;
            if (cmdPck == null)
            {
                return;
            }

            string SN = _MQTT_Context.DeviceSN;

            var chkRet = CheckDeviceReg(SN, out CacheDeviceDTO oDevice);
            if (!chkRet) return;

            var deny = httpOption.Find(SN);
            if (deny != null)
            {
                return;
            }
            var recordDto = cmdPck.Body;
            recordDto.SN = SN;
            var recordList = recordDto.Records.Adapt<List<SystemRecord>>();

            if (recordDto.RecordType == 1)
            {
                foreach (var item in recordList)
                {
                    item.RecordType += 1000;
                }
            }



            var _RecordDB = _ServiceProvider.GetService<IRecordService>();
            await _RecordDB.AddRecord(recordDto.SN, recordList);


            await SendCommandPacketCore(new MQTT_Command_UploadSystemRecordACK(cmdPck.CmdID));
        }

        #endregion


        #endregion

        #region 固件升级



        private async Task PushSoftware(RemoteTaskDetail taskDtl)
        {
            //_logger.LogInformation($" 固件升级通知 PushSoftware SN:{par.DeviceID}");
            var SN = _MQTT_Context.DeviceSN;
            if (string.IsNullOrEmpty(taskDtl.TaskExtension))
                return;


            var iOpt = _ServiceProvider.GetService<IOptionsMonitor<HTTPProtocolOption>>();
            var httpOption = iOpt.CurrentValue;


            var body = JsonConvert.DeserializeObject<PushSoftwareDTO>(taskDtl.TaskExtension);

            body.SoftwareURL = $"{httpOption.PeopleURLPrefix}{body.SoftwareURL}";

            var sKey = $"MQTT_PushSoftware_{SN}";
            _Cache.Set(sKey, taskDtl.TaskID);
            await SendCommandPacket(new MQTT_Command_PushSoftware(body));
        }

        /// <summary>
        /// 固件升级响应
        /// </summary>
        /// <param name="packet"></param>
        /// <returns></returns>
        private async Task PushSoftwareACK(MQTTCommandPacket packet)
        {
            //_logger.LogInformation($" 固件升级响应 PushSoftwareACK");

            //检查参数类型是否正确
            MQTT_Command_PushSoftwareACK cmdPck = packet as MQTT_Command_PushSoftwareACK;
            if (cmdPck == null)
            {
                return;
            }



            bool bIsACK = CheckPacketIsACK(MQTT_Command_Define.PushSoftware, packet);
            if (!bIsACK) return; //不是当前命令相应，退出
            var SN = _MQTT_Context.DeviceSN;
            var sKey = $"MQTT_PushSoftware_{SN}";

            var chkRet = CheckDeviceReg(SN, out CacheDeviceDTO oDevice);
            if (!chkRet) return;

            oDevice.UpdateSoftURL = string.Empty;
            oDevice.UpdateSoftMD5 = string.Empty;
            oDevice.UpdateSoftVer = string.Empty;

            int taskID = _Cache.Get<int>(sKey);
            _Cache.Remove(sKey);

            var _RemoteDB = _ServiceProvider.GetService<IDeviceRemoteService>();

            await _RemoteDB.UpdateTaskRunStatusComplete([taskID],
            oDevice.ID, oDevice.SN); //更新任务状态


        }
        #endregion

        #region 系统文件推送


        /// <summary>
        /// 系统文件推送
        /// </summary>
        /// <returns></returns>
        private async Task PushSystemFile(RemoteTaskDetail taskDtl)
        {
            //_logger.LogInformation($" 系统文件推送 PushSystemFile SN:{SN}");
            var SN = _MQTT_Context.DeviceSN;

            var body = JsonConvert.DeserializeObject<List<PushSystemFileDTO>>(taskDtl.TaskExtension);

            var sKey = $"MQTT_PushSystemFile_{SN}";
            _Cache.Set(sKey, taskDtl.TaskID);
            await SendCommandPacket(new MQTT_Command_PushSystemFile(body));

        }

        /// <summary>
        /// 系统文件推送响应
        /// </summary>
        /// <param name="packet"></param>
        /// <returns></returns>
        private async Task PushSystemFileACK(MQTTCommandPacket packet)
        {
            //检查参数类型是否正确
            var cmdPck = packet as MQTT_Command_PushSystemFileACK;
            if (cmdPck == null)
            {
                return;
            }



            bool bIsACK = CheckPacketIsACK(MQTT_Command_Define.PushSystemFile, packet);
            if (!bIsACK) return; //不是当前命令相应，退出
            var SN = _MQTT_Context.DeviceSN;
            var sKey = $"MQTT_PushSystemFile_{SN}";

            var chkRet = CheckDeviceReg(SN, out CacheDeviceDTO oDevice);
            if (!chkRet) return;

            int taskID = _Cache.Get<int>(sKey);
            _Cache.Remove(sKey);

            var _RemoteDB = _ServiceProvider.GetService<IDeviceRemoteService>();

            await _RemoteDB.UpdateTaskRunStatusComplete([taskID],
            oDevice.ID, oDevice.SN); //更新任务状态
        }
        #endregion


        #region 在设备上注册用户凭证
        /// <summary>
        /// 服务器发送通知设备注册用户凭证
        /// </summary>
        /// <returns></returns>
        private async Task RegisterIdentifyTicket(RemoteTaskDetail taskDtl)
        {
            //_logger.LogInformation($" 服务器发送通知设备注册用户凭证 RegisterIdentifyTicket SN:{par.DeviceID}");
            var SN = _MQTT_Context.DeviceSN;

            var body = new RegisterIdentifyTicketDTO();
            body.UserID = taskDtl.UserID.Value;
            var Regdto = JsonConvert.DeserializeObject<EnrollUserMediaDataDTO>(taskDtl.TaskExtension);
            body.RegisterType = Regdto.EnrollTypeToInt();
            body.RegisterIndex = Regdto.EnrollIndex;

            var sKey = $"MQTT_RegisterIdentifyTicket_{SN}";
            _Cache.Set(sKey, taskDtl.TaskID);
            await SendCommandPacket(new MQTT_Command_RegisterIdentifyTicket(body), 90 * 1000);

        }

        /// <summary>
        /// 设备反馈注册用户凭证结果
        /// </summary>
        /// <param name="packet"></param>
        /// <returns></returns>
        private async Task RegisterIdentifyTicketACK(MQTTCommandPacket packet)
        {
            //_logger.LogInformation($" 设备反馈注册用户凭证结果 RegisterIdentifyTicketACK");
            //检查参数类型是否正确
            var cmdPck = packet as MQTT_Command_RegisterIdentifyTicketACK;
            if (cmdPck == null)
            {
                return;
            }

            bool bIsACK = CheckPacketIsACK(MQTT_Command_Define.RegisterIdentifyTicket, packet);
            if (!bIsACK) return; //不是当前命令相应，退出
            var SN = _MQTT_Context.DeviceSN;
            var sKey = $"MQTT_RegisterIdentifyTicket_{SN}";

            var chkRet = CheckDeviceReg(SN, out CacheDeviceDTO oDevice);
            if (!chkRet) return;

            int taskID = _Cache.Get<int>(sKey);
            _Cache.Remove(sKey);



            var _RemoteDB = _ServiceProvider.GetService<IDeviceRemoteService>();

            await _RemoteDB.UpdateTaskRunStatusComplete([taskID],
            oDevice.ID, oDevice.SN); //更新任务状态
            var people = cmdPck.Body.UserDetail;
            if (cmdPck.Body.Result == 1 && people.PhotoLen > 0)
            {

                //保存照片
                string sSavePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "PushPeople");
                string sImageFileName = $"{people.UserID}.jpg";
                string sTmpFile = string.Empty;
                if (Directory.Exists(sSavePath) == false)
                    Directory.CreateDirectory(sSavePath);

                sTmpFile = Path.Combine(sSavePath, sImageFileName);

                var imageBuf = cmdPck.GetDataBuf();



                //设备可能使用base64编码照片
                if (imageBuf == null && people.Photo.Length > 1000)
                {
                    //照片采用的是base64编码
                    try
                    {
                        imageBuf = Convert.FromBase64String(people.Photo);
                        _logger.LogInformation($"反馈人员注册凭证的结果 {SN} 用户号： {people.UserID} 有照片,照片使用base64编码 ，照片长度：{imageBuf.Count}");
                    }
                    catch (Exception)
                    {
                        _logger.LogError($"反馈人员注册凭证的结果 {SN} 用户号： {people.UserID} 有照片,照片使用base64编码 ，照片解码失败");
                        imageBuf = null;
                    }

                }
                else
                {
                    _logger.LogInformation($"反馈人员注册凭证的结果 {SN} 用户号：{people.UserID} 用户照片长度：{imageBuf.Count}");
                }


                if (imageBuf != null)
                {

                    //保存图片
                    await FileHelpers.WriteArraySegmentToFileAsync(imageBuf, sTmpFile);

                    people.PhotoLen = imageBuf.Count;
                    if (string.IsNullOrEmpty(people.PhotoMD5))
                    {
                        people.PhotoMD5 = MD5Helper.GetByteBufMD5ByHex(imageBuf);
                    }
                    people.Photo = sTmpFile;
                }
                else
                {
                    people.PhotoLen = 0;
                    people.PhotoMD5 = string.Empty;
                    people.Photo = string.Empty;
                    sTmpFile = string.Empty;
                }
            }


            //将人员注册反馈保存到缓存中
            string sCacheKey = $"RegisterIdentifyTicket_{SN}";
            _Cache.Set(sCacheKey, cmdPck.Body, TimeSpan.FromMinutes(10));
            _logger.LogInformation($"反馈人员注册凭证的结果 {SN} 用户号：{cmdPck.Body.UserID} 结果：{cmdPck.Body.Result}");

        }
        #endregion


        #region 请求服务器鉴权
        /// <summary>
        /// 服务器反馈设备鉴权结果
        /// </summary>
        /// <returns></returns>
        private async Task RequestAuthorizationACK(RequestAuthorizationResultDTO body, string cmdid)
        {

            var SN = _MQTT_Context.DeviceSN;
            _logger.LogInformation($" 服务器反馈设备鉴权结果 RequestAuthorizationACK SN:{SN}");

            await SendCommandPacketCore(new MQTT_Command_RequestAuthorizationACK(body, cmdid));

        }
        static int RequestAuthorizationCount = 0;
        /// <summary>
        /// 设备发送请求服务器鉴权
        /// </summary>
        /// <param name="packet"></param>
        /// <returns></returns>
        private async Task RequestAuthorization(MQTTCommandPacket packet)
        {

            //检查参数类型是否正确
            var cmdPck = packet as MQTT_Command_RequestAuthorization;
            if (cmdPck == null)
            {
                return;
            }
            var SN = _MQTT_Context.DeviceSN;
            string sSavePath = Path.Combine(Directory.GetCurrentDirectory(),
                   "wwwroot", "RequestAuthorization");
            string sImageFileName = $"{SN}.jpg";
            //保存图片到请求鉴权目录
            if (Directory.Exists(sSavePath) == false)
                Directory.CreateDirectory(sSavePath);

            string sTmpFile = Path.Combine(sSavePath, sImageFileName);
            var imageBuf = packet.GetDataBuf();
            int iImgSize = 0;

            var record = cmdPck.Body;
            //设备可能使用base64编码照片
            if (imageBuf == null && record.Photo.Length > 1000)
            {
                //照片采用的是base64编码
                try
                {
                    imageBuf = Convert.FromBase64String(record.Photo);
                    record.Photo = "base64";
                    _logger.LogInformation($"设备发送请求服务器鉴权 {SN} 用户号： {record.UserID} 有照片,照片使用base64编码 ，照片长度：{imageBuf.Count}");
                }
                catch (Exception)
                {
                    _logger.LogError($"设备发送请求服务器鉴权 {SN} 用户号： {record.UserID} 有照片,照片使用base64编码 ，照片解码失败");
                    imageBuf = null;
                }

            }
            else
            {
                _logger.LogInformation($"设备发送请求服务器鉴权 {SN} 用户号： {record.UserID} 有照片 ，照片长度：{imageBuf.Count}");
            }


            if (imageBuf != null)
            {

                //保存图片
                await FileHelpers.WriteArraySegmentToFileAsync(imageBuf, sTmpFile);
                iImgSize = imageBuf.Count;

            }

            {
                _logger.LogInformation($" 设备发送请求服务器鉴权 RequestAuthorization \n {JsonConvert.SerializeObject(cmdPck.Body, Formatting.Indented)} \n图片大小:{iImgSize}");
            }


            var cmdid = cmdPck.CmdID;
            var recordID = cmdPck.Body.RecordID;


            int iWaitTime = 15 * 1000;
            if (RequestAuthorizationCount % 2 == 0)
            {
                iWaitTime = 1 * 1000;
            }
            RequestAuthorizationCount++;

            await Task.Delay(iWaitTime);
            var result = new RequestAuthorizationResultDTO()
            {
                RecordID = recordID,
                VerifyResult = 1,
                VerifyMessage = "MQTT 验证通过！"
            };
            await RequestAuthorizationACK(result, cmdid);

        }
        #endregion


        #region 获取设备摄像头快照
        /// <summary>
        /// 服务器发送获取设备摄像头快照请求
        /// </summary>
        /// <returns></returns>
        private async Task RequestSnapshoot(RemoteTaskDetail taskDtl)
        {
            //_logger.LogInformation($" 获取设备摄像头快照 RequestSnapshoot SN:{_MQTT_Context.DeviceSN}");
            var SN = _MQTT_Context.DeviceSN;
            var sKey = $"MQTT_RequestSnapshoot_{SN}";
            _Cache.Set(sKey, taskDtl.TaskID);
            await SendCommandPacket(new MQTT_Command_RequestSnapshoot());
        }

        /// <summary>
        /// 设备返回摄像头快照
        /// </summary>
        /// <param name="packet"></param>
        /// <returns></returns>
        private async Task RequestSnapshootACK(MQTTCommandPacket packet)
        {
            //_logger.LogInformation($" 设备返回摄像头快照 RequestSnapshootACK");
            //检查参数类型是否正确
            var cmdPck = packet as MQTT_Command_RequestSnapshootACK;
            if (cmdPck == null)
            {
                return;
            }

            bool bIsACK = CheckPacketIsACK(MQTT_Command_Define.RequestSnapshoot, packet);
            if (!bIsACK) return; //不是当前命令相应，退出
            var SN = _MQTT_Context.DeviceSN;
            var sKey = $"MQTT_RequestSnapshoot_{SN}";

            var chkRet = CheckDeviceReg(SN, out CacheDeviceDTO oDevice);
            if (!chkRet) return;

            int taskID = _Cache.Get<int>(sKey);
            _Cache.Remove(sKey);

            var _RemoteDB = _ServiceProvider.GetService<IDeviceRemoteService>();

            await _RemoteDB.UpdateTaskRunStatusComplete([taskID],
            oDevice.ID, oDevice.SN); //更新任务状态


            //保存照片到快照目录
            string sSavePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "PushSnapshoot");
            string sImageFileName = $"{SN}.jpg";
            string sTmpFile = "";
            if (Directory.Exists(sSavePath) == false)
                Directory.CreateDirectory(sSavePath);

            sTmpFile = Path.Combine(sSavePath, sImageFileName);

            System.IO.File.Delete(sTmpFile);
            if (cmdPck.Body == null)
            {
                cmdPck.Body = new MQTT_Command_RequestSnapshootResult();
            }

            var imageBuf = packet.GetDataBuf();
            if (imageBuf != null)
            {
                _logger.LogInformation($"上传设备摄像头快照 {SN} 二进制上传 照片大小：{imageBuf.Count}");
                //保存图片
                await FileHelpers.WriteArraySegmentToFileAsync(imageBuf, sTmpFile);
            }
            else if (cmdPck.Body.Photo.Length > 1000)
            {
                _logger.LogInformation($"上传设备摄像头快照 {SN} Base64字符串上传 照片大小：{cmdPck.Body.PhotoSize} 字符串长度：{cmdPck.Body.Photo.Length}");

                //保存图片
                await FileHelpers.Base64StringConverBinToFileAsync(cmdPck.Body.Photo, sTmpFile);
            }
            else
            {
                _logger.LogInformation($"上传设备摄像头快照 {SN} 没有照片");
            }


            string sCacheKey = $"Snapshoot_{SN}";

            do
            {
                var tsk = _Cache.Get<TaskCompletionSource<string>>(sCacheKey);
                if (tsk != null)
                {
                    tsk.SetResult(sTmpFile);
                    break;
                }
                await Task.Delay(100);
            } while (true);


            _logger.LogInformation($"上传设备摄像头快照 {SN} 处理完毕");
        }
        #endregion

        /// <summary>
        /// 关闭客户端的MQTT连接    
        /// </summary>
        /// <returns></returns>
        private Task CloseConnect()
        {
            return _mqtt.DisconnectClientAsync(_MQTT_Context.ClientID);
        }
    }
}
