using FaceWebServer.DB.Table;
using FaceWebServer.DTO.Device;
using FaceWebServer.DTO.HTTPv1_Protocol;
using FaceWebServer.DTO.HTTPv2_Protocol;
using FaceWebServer.DTO.Remote;
using FaceWebServer.Interface;
using FaceWebServer.Language;
using FaceWebServer.Utility.Extend;
using FaceWebServer.Utility.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace FaceWebServer.Service
{
    /// <summary>
    /// 人脸机设备操作服务
    /// </summary>
    public class FaceDriveService : BaseService, IFaceDriveService
    {
        public ICacheService _Cache { get; set; }
        public ILogger<FaceDriveService> _logger { get; set; }
        public IServiceProvider _ServiceProvider { get; set; }
        private LanguageHandler _LanguageHandler;

        public FaceDriveService(DbContext context,
            IOptionsSnapshot<LanguageOption> lngopt) : base(context)
        {
            _LanguageHandler = lngopt.Value.GetCurrentLanguageHandler();

        }

        public DbSet<DeviceDetail> GetDBSet()
        {
            return this.Context.Set<DeviceDetail>();
        }


        /// <summary>
        /// 分页查询设备信息
        /// </summary>
        /// <returns></returns>
        public PageResult<DeviceQueryResultDTO> Query(DeviceQueryDTO queryDTO)
        {
            List<Expression<Func<DeviceDetail, bool>>> oWheres = new();
            if (queryDTO.ID.HasValue) oWheres.Add(x => x.ID == queryDTO.ID.Value);
            if (!string.IsNullOrWhiteSpace(queryDTO.SN)) oWheres.Add(x => x.SN.Contains(queryDTO.SN));
            if (!string.IsNullOrWhiteSpace(queryDTO.Protocol)) oWheres.Add(x => x.Protocol.Contains(queryDTO.Protocol));
            if (!string.IsNullOrWhiteSpace(queryDTO.DeviceName)) oWheres.Add(x => x.Name.Contains(queryDTO.DeviceName));
            if (queryDTO.IsEntry.HasValue) oWheres.Add(x => x.IsEntry == queryDTO.IsEntry.Value);


            var devices = QueryPage(
            x => new DeviceQueryResultDTO()
            {
                ID = x.ID,
                SN = x.SN,
                Protocol = x.Protocol,
                DeviceName = x.Name,
                DeviceVer = x.DeviceVer,
                IsEntry = x.IsEntry,
            },
            oWheres, queryDTO.PageSize, queryDTO.PageIndex,
            x => x.SN,
            queryDTO.IsAsc);



            if (devices.DataList.Count > 0)
            {   //查询缓存

                var retList = new List<DeviceQueryResultDTO>(devices.DataList.Count);


                foreach (var item in devices.DataList)
                {
                    var cacheDto = _Cache.GetDevice(item.SN);
                    int iKeepaliveTime = 0;

                    if (cacheDto != null)
                    {
                        item.LastKeepaliveTime = cacheDto.LastKeepaliveTime;
                        iKeepaliveTime = (int)(DateTime.Now - item.LastKeepaliveTime).TotalSeconds;
                        item.KeepaliveStatus = cacheDto.KeepaliveStatus;
                    }
                    if (queryDTO.IsOnline.HasValue)
                    {
                        if (queryDTO.IsOnline.Value == 0 && iKeepaliveTime > 60)
                        {
                            retList.Add(item);//离线
                        }
                        if (queryDTO.IsOnline.Value == 1 && iKeepaliveTime < 60)
                        {
                            retList.Add(item);//在线
                        }
                    }
                    else
                    {
                        retList.Add(item);
                    }

                }
                devices.DataList = retList;

            }
            return devices;
        }

        public List<DeviceOnlineStatusQueryResultDTO> GetDeviceOnlineStatus(List<string> SNList)
        {
            List<DeviceOnlineStatusQueryResultDTO> oModel =
                SNList.Select(x => new DeviceOnlineStatusQueryResultDTO() { SN = x }).ToList();

            foreach (var item in oModel)
            {
                var cacheDto = _Cache.GetDevice(item.SN);
                if (cacheDto != null)
                {
                    item.ID = cacheDto.ID;
                    item.LastKeepaliveTime = cacheDto.LastKeepaliveTime;
                    item.KeepaliveStatus = cacheDto.KeepaliveStatus;
                }

            }

            return oModel;
        }

        public DeviceDetail GetDeviceDetail(string SN)
        {
            return Context.Set<DeviceDetail>().FirstOrDefault(a => a.SN == SN);
        }

        public DeviceDetail GetDeviceDetail(int id)
        {
            return Context.Set<DeviceDetail>().FirstOrDefault(a => a.ID == id);
        }

        public async Task<DeviceDetail> Add(DeviceDetail oDevice)
        {
            DeviceDetail oDBModel = GetDeviceDetail(oDevice.SN);
            if (oDBModel == null)
            {
                CurrentUser = new UserDetail() { UserID = 1, UserName = "Auto" };
                AddUserLog(_LanguageHandler.GetUserLog("t3"),// "设备管理",
                    string.Format(_LanguageHandler.GetUserLog("r21"), oDevice.SN),
                    oDevice.SN, string.Empty);//{0} 设备上报参数，自动新增入库。

                oDevice.Name = oDevice.SN;//默认名称
                oDevice.UploadStatus = 1;//更新为已同步

                oDBModel = await InsertAsync<DeviceDetail>(oDevice);

                _Cache.AddDeviceCache(oDBModel);
                return oDBModel;
            }
            else
            {
                //2021年7月22日 增加参数覆盖功能
                oDBModel = Find<DeviceDetail>(oDBModel.ID);

                AddUserLog(_LanguageHandler.GetUserLog("t3"),// "设备管理",
                    string.Format(_LanguageHandler.GetUserLog("r22"),
                    oDBModel.Name, oDBModel.SN),
                    $"{oDBModel.Name}({oDBModel.SN})", string.Empty);//{0}({1}) 设备主动上传参数，覆盖服务器参数。

                oDBModel.Protocol = oDevice.Protocol;
                oDBModel.DeviceVer = oDevice.DeviceVer;
                oDBModel.IsEntry = oDevice.IsEntry;

                oDBModel.Detail = oDevice.Detail;
                oDBModel.LastUpdatetime = DateTime.Now;

                await CommitAsync();

                _Cache.UpdateDeviceCache(oDevice.SN, x =>
                {
                    x.Protocol = oDBModel.Protocol;
                    x.DeviceName = oDBModel.Name;
                });

                return oDBModel;
            }
        }


        public JsonResultModel Update(DeviceDetail oUpdateDto)
        {
            var device = Find<DeviceDetail>(oUpdateDto.ID);
            if (device == null) return new JsonResultModel(200, _LanguageHandler.GetCheckParameterErrorMessage("r72"));//设备不存在

            #region 重复过滤
            List<Expression<Func<DeviceDetail, bool>>> oWheres = [x => x.ID != oUpdateDto.ID];
            Expression<Func<DeviceDetail, bool>> w1 = x => x.SN.Equals(oUpdateDto.SN);

            if (!string.IsNullOrEmpty(oUpdateDto.Name))
            {
                Expression<Func<DeviceDetail, bool>> w2 = x => x.Name.Equals(oUpdateDto.Name);
                oWheres.Add(w1.Or(w2));
            }
            else
            {
                oWheres.Add(w1);
            }


            var devices = QueryPage(
                x => new
                {
                    x.ID,
                    x.SN,
                    x.Name
                },
                oWheres, 100, 1,
                x => x.SN,
                true, true);
            if (devices.TotalCount > 0)
            {
                foreach (var p in devices.DataList)
                {
                    if (p.SN == oUpdateDto.SN)
                    {
                        return new JsonResultModel(201,
                            _LanguageHandler.GetCheckParameterErrorMessage("r127"));//"设备SN重复"
                    }
                    if (p.Name == oUpdateDto.Name)
                    {
                        return new JsonResultModel(202,
                            _LanguageHandler.GetCheckParameterErrorMessage("r128"));//"设备名称重复"
                    }
                }

            }
            #endregion

            //_logger.LogInformation($"更新设备：ID:{device.ID} SN:{device.SN} Name:{device.DeviceName}");
            string SN = device.SN;

            device.Name = oUpdateDto.Name;
            device.IsEntry = oUpdateDto.IsEntry;
            device.Detail = oUpdateDto.Detail;
            device.LastUpdatetime = DateTime.Now;

            device.UploadStatus = 0;

            //更新缓存
            _Cache.UpdateDeviceCache(SN, x =>
            {
                x.Protocol = device.Protocol;
                x.DeviceName = device.Name;
                x.UploadStatus = device.UploadStatus;
            });

            string sLogFormat = _LanguageHandler.GetUserLog("r23");
            AddUserLog(_LanguageHandler.GetUserLog("t3"),// "设备管理",
                string.Format(_LanguageHandler.GetUserLog("r24"), device.Name, device.SN),//"修改设备参数"
                $"{device.Name}({device.SN})", string.Empty);

            //base.Update(device);
            Commit();
            return new JsonResultModel(new { device.ID });
        }

        public async Task<bool> Delete(List<int> IDList)
        {
            var db = Context.Set<DeviceDetail>();
            //查询需要删除的设备
            HashSet<int> devIDLists = new HashSet<int>(IDList);
            HashSet<string> snList = new HashSet<string>(IDList.Count);
            var devices = await db.Where(x => devIDLists.Contains(x.ID)).Select(x => new DeviceDetail
            {
                ID = x.ID,
                Name = x.Name,
                SN = x.SN
            }).ToListAsync();
            if (devices == null) return false;
            if (devices.Count == 0) return false;


            string sLogTitle = _LanguageHandler.GetUserLog("t3");//设备管理
            string sLogFormat = _LanguageHandler.GetUserLog("r25");//删除设备：{0}({1})

            var dbEx = this.Context.Database;
            foreach (var d in devices)
            {
                AddUserLog(sLogTitle, string.Format(sLogFormat, d.Name, d.SN),
                $"{d.Name}({d.SN})", string.Empty);
                snList.Add(d.SN);

                db.Remove(d);
            }

            //删除设备的权限
            await Context.Set<PeopleAccessDetail>()
                .Where(x => devIDLists.Contains(x.DeviceID)).ExecuteDeleteAsync();

            //删除设备的远程操作记录
            await Context.Set<RemoteTaskDetail>()
               .Where(x => snList.Contains(x.SN)).ExecuteDeleteAsync();

            //更新缓存
            _Cache.DeleteDeviceCache(devices.ToList());

            await CommitAsync();

            return true;
        }


        #region 默认值

        public void SaveDefaultValue(SetDefaultValueRequestDTO dto)
        {
            string sKey = "DefaultDevice" + dto.Protocol;
            var sysdb = GetSystemKVDBSet();
            var kv = sysdb.Find(sKey);
            var sJson = dto.DefaultJson;



            AddUserLog(_LanguageHandler.GetUserLog("t3") + " " + dto.Protocol,// "设备管理",
                 _LanguageHandler.GetUserLog("r23"));//更新设备出厂默认参数

            if (kv == null)
            {
                kv = new() { Key = sKey, Value = sJson };
                Insert(kv);
            }
            else
            {
                kv.LastUpdateTime = DateTime.Now;
                kv.Value = sJson;
                Commit();

            }
        }



        public string GetDefaultValue(string sProtocol)
        {
            string sKey = "DefaultDevice" + sProtocol;
            var sysdb = GetSystemKVDBSet();
            var kv = sysdb.Find(sKey);
            string par = string.Empty;
            if (kv == null)
            {
                par = GetDefaultParameter(sProtocol);
            }
            else
            {
                try
                {
                    par = kv.Value;
                }
                catch (Exception)
                {

                    par = GetDefaultParameter(sProtocol);
                }
            }
            return par;
        }



        /// <summary>
        /// 获取默认的出厂参数
        /// </summary>
        /// <returns></returns>
        private string GetDefaultParameter(string sProtocol)
        {
            switch (sProtocol)
            {
                case DeviceDetail.HTTPv1:
                    return GetDefaultParameter_HTTPv1();
                case DeviceDetail.HTTPv2:
                    return GetDefaultParameter_HTTPv2();
                case DeviceDetail.MQTT:
                    return GetDefaultParameter_HTTPv2();
                case DeviceDetail.Websocket:
                //break;
                default:
                    return string.Empty;
            }

        }
        private string GetDefaultParameter_HTTPv1()
        {
            HTTPDeviceParameterCoreV1 target = new();
            target.DeviceID = "FC-8280T00000000";//设备ID

            #region 语音
            target.ParameterVolume = 10;//音量大小(范围0-10)
            target.ParameterVoiceMode = 3;//语音模式0,不播报；1，播放名字;2,播放问候语;3,播放名字和问候语
            target.ParameterGrettings = 0;//问候语0，请通行;1,欢迎光临;2，时间问候语；;3、消费登记成功
            target.ParameterStrangerVoice = 2;//陌生人语音0，不播报;1,播报假体;2，播报陌生人；3，播报假体和陌生人
            #endregion

            #region 补光灯
            target.ParameterLightSwitch = 1;//补光灯开关：0：常闭；  1：常开； 2：自动；
            #endregion

            #region UI界面
            target.ParameterBrightness = 10;//亮度设置1-10
            target.ParameterExposure = 0;// 曝光设置
            target.ParameterIR = 0;//红外图像开关
            target.ParameterLanguageChoose = 0;//语言选择0，中文；1，英文；2，繁体
            target.ParameterCompanyName = "";//主界面设置--公司名称
            target.ParameterLoginPassword = "";//菜单密码
            #endregion

            #region 身份信息
            target.ParameterSerialNum = target.DeviceID;//序列号
            target.ParameterManufacturer = "";//制造商
            target.ParameterWebsite = "";//网址
            target.ParameterProductionDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");//生产日期
            #endregion

            #region 记录存储
            target.ParameterSaveExternalvisitors = 1;//是否存储非雇员识别记录，0:不存储，1:存储
            target.ParameterSavePicture = 1;//保存现场图片 0，不保存；1，保存
            #endregion

            #region 人脸识别
            target.ParameterPreview = 1;//活体检测,1打开，0关闭
            target.ParameterFaceIRThreshold = 6;//活体检测阈值1-10（2021年8月3日 新增）
            target.ParameterDistance = 2;//识别距离 1--远（默认值）、2--中、3--近  
            target.ParameterFaceThreshold = 70;//人脸识别阈值
            target.ParameterMask = 1;//1：打开口罩检测，其他：关闭
            target.ParameterMaskThreshold = 60;//口罩阈值
            #endregion

            #region 体温检测
            target.ParameterTempSwitch = 1;//测温模式开关，0关闭，1打开
            target.ParameterFahrenheitSwitch = 0;//开启华氏温度,1:开，0:关
            target.ParameterCompensate = 0;// -1 -- 1 ,温度补偿值
            target.ParameterTempThresholdMax = 37.6f;//温度比对阈值（最大值）
            #endregion

            #region 网络参数
            target.ParameterCloudserverAddress = "";//服务器地址
            target.ParameterPolling = 20;//主动触发云端轮训时间（以秒为单位）
            target.ParameterServerProtocol = 0;//服务器协议：0--http；1--云筑网；2--websocket；
            #endregion

            #region 网络扩展参数
            target.ParameterEnableWeb = 1;// Web页面管理开关,1:开，0:关
            target.ParameterEnableTelnet = 0;// Telnet协议功能
            target.ParameterEnableUDP = 1;// UDP搜索功能开关
            target.ParameterUDPPort = 8101;// UDP端口号
            target.ParameterHttpPostUseGZIP = 1;//是否使用HTTP协议中的GZIP压缩算法
            #endregion


            #region 门禁参数
            target.ParameterAccessType = 0;//出入类型0,入门；1，出门

            target.ParameterWgFormat = 34;//WG输出格式26/34
            target.ParameterKeepOpenDoor = 3;//开门保持时间1-65535（s）
            target.ParameterLaissezSwitch = 0;//免验证开门1--启用；0--禁用
            target.ParameterRecgInterval = 10;//识别间隔0--禁用;1-65535（s）
            target.ParameterIntervalRecoSwitch = 0;//间隔记录存储设置0,关闭；1，打开
            target.ParameterFireAlarm = 1;// 消防报警参数
            target.ParameterDoorLongOpenAlarmSwitch = 0;//开门超时报警开关，0，关闭；1，开启
            target.ParameterDoorSensorDelay = 60;//开门超时时间，门打开超过这个时间就报警	5-255（s）
            target.ParameterDoorAlarmSwitch = 0;//门磁报警，0，关闭；1，开启
            #endregion

            return JsonConvert.SerializeObject(target);
        }

        private string GetDefaultParameter_HTTPv2()
        {
            HTTPDeviceParameterCoreV2 target = new();

            #region 设备基本信息 SystemInfo
            // 设备SN 
            target.DeviceSN = "FC-8190H00000000";

            // 设备名称
            target.DeviceName = string.Empty;


            // 制造商
            target.Manufacturer = string.Empty;

            // 厂家电话
            target.ManufacturerPhone = string.Empty;

            // 网址
            target.Website = string.Empty;

            // 生产日期
            target.ProductionDate = DateTime.Now.ToString("yyyy-MM-dd");

            // 自定义数据
            target.OEMText = string.Empty;



            // 每天自动重启功能开关
            target.AutoRestart = 1;

            // 每天自动重启的时间，格式  HH:mm
            target.AutoRestartTime = "02:00";
            #endregion

            #region 区域与语言  Language
            // 语言 
            target.Language = 1;

            // 设备时间Unix时间戳（秒）
            target.SystemTime = 0;

            // 启用NTP自动校对时间 1--启用；0--禁用
            target.UseNTP = 1;

            // 设备时区 
            target.UTCTimeZone = "UTC+08:00";

            // 音量大小(范围 0-10)
            target.Volume = 10;

            // 语音播放开关 0,不播报；1,播报
            target.Voice = 1;
            #endregion

            #region 人机交互 UI
            // 屏幕亮度设置 1-10
            target.DisplayBrightness = 8;

            // 菜单密码
            target.MenuPassword = string.Empty;

            // 在设备上显示红外图像 1--启用；0--禁用
            target.ShowIR = 0;

            // 识别后显示人员头像 1--启用；0--禁用
            target.ShowPersonPhoto = 1;

            // 识别后播报人员姓名 1--启用；0--禁用
            target.PlayPersonName = 1;

            // 识别前需要点击识别按钮 1--启用；0--禁用
            target.RecognitionButton = 0;

            // 未注册人员提醒 1--启用；0--禁用
            target.UnregisteredWarn = 1;

            // 识别后是否显示人员姓名  1--启用；0--禁用
            target.ShowPersonName = 1;

            // 补光灯模式：0：常闭；  1：常开； 2：自动；
            target.FillLight = 1;

            // 二维码识别开关   1--启用；0--禁用
            target.UseQRCode = 1;

            target.UseFastRecognition = 0;
            target.UseRequestAuthorization = 0;
            target.UseComplexUserID = 0;
            #endregion

            #region 数据存储 Storage
            // 记录存满循环  1--记录满循环，0--记录满不循环，等待清理
            target.RecordAutoCycle = 1;

            // 保存未注册人员，0,不保存；1,保存   
            target.SaveUnregistered = 0;

            // 保存现场图片 0,不保存；1,保存
            target.SaveRecordPicture = 1;

            #endregion

            #region 人脸识别 Face
            // 活体检测,1 打开,0 关闭
            target.FaceIR = 1;

            // 活体检测阈值 1-99
            target.FaceIRThreshold = 5;

            // 识别距离 1--近距离（0.2-0.5米）；2--中距离（0.2-1.5米）；3--远距离（0.2-1.5米以上）
            target.FaceDistance = 3;

            // 人脸识别阈值1-99 人脸识别阈值 是越大精度越高
            target.FaceThreshold = 58;

            // 指纹比对阈值  取值范围：1-100
            target.FPComparison = 58;

            // 人脸口罩检测,1 打开,0 关闭
            target.FaceMask = 0;

            // 口罩检测阈值 1-100
            target.FaceMaskThreshold = 58;
            #endregion

            #region 体温检测 BodyTemperature 
            // 测温模式开关。0：非测温模式 1：测温模式
            target.UseBodyTemperature = 0;

            // 开启华氏温度显示,1:开,0:关
            target.UseFahrenheitDisplay = 0;

            // 温度补偿值 
            target.TemperatureCompensate = 0;

            // 温度报警阈值
            target.TemperatureAlarmThreshold = 37.5;

            // 是否显示体温 0:不显示 1:显示
            target.TemperatureDisplay = 1;
            #endregion

            #region 服务器参数 NetworkServer
            // 使用 TCPClient 连接服务器
            target.OnecardCloudServerProtocol = 0;

            // 服务器地址  tcp 或 udp 协议服务器地址
            target.ServerAddress = "yun.pc15.net";
            target.ServerIP = string.Empty;
            // 服务器端口号
            target.ServerPort = 9003;

            // 保活包间隔时间 1-65535 秒
            target.KeepaliveTime = 30;
            target.PushOfflineMessage = 0;





            // 是否启用 HTTPClient 协议   1--启用；0--禁用；
            target.UseHTTPClient = 1;

            // http协议服务器地址
            target.HTTPClient_ServerAddr = "http://192.168.1.10/";

            // httpclient 协议的保活包间隔时间
            target.HTTPClient_KeepaliveTime = 20;

            // http 请求时是否使用GZIP压缩 0--不使用；1--使用
            target.HTTPClient_UseGZIP = 1;

            // HTTPClient 的协议类型   100 --- HTTPv1   200 ---HTTPv2
            target.HTTPClient_ProtocolType = 200;





            // 是否启动 MQTTClient 协议  1--启用；0--禁用；
            target.UseMQTTClient = 0;

            // 是否启用MQTT的SSL安全套接字 1--启用；0--禁用；
            target.UseMQTTSSL = 0;

            // MQTT服务器地址   www.abc.com
            target.MQTTServerAddr = string.Empty;

            // MQTT服务器端口号
            target.MQTTPort = 1883;

            // MQTT 协议中 登录用户名
            target.MQTTLoginName = string.Empty;

            // MQTT 协议中 登录密码
            target.MQTTLoginPassword = string.Empty;

            // MQTT 协议中 设备发送数据使用的Topic
            target.MQTTPublishTopic = "iotserver";


            // MQTT 协议中 设备接收数据需要订阅的Topic
            target.MQTTSubscribeTopic = "iot/{sn}";


            // MQTT 协议的保活包间隔时间
            target.MQTT_KeepaliveTime = 20;

            target.MQTT_UseGZIP = 1;




            // 是否启动 WebsocketClient 协议  1--启用；0--禁用；
            target.UseWebsocketClient = 0;

            // Websocket协议 服务器地址  ws://192.168.1.1/websocket   or  wss://192.168.1.1/websocket
            target.WebsocketClient_ServerAddr = "ws://192.168.1.10/websocket";

            // WebsocketClient 协议的保活包间隔时间
            target.WebsocketClient_KeepaliveTime = 0;

            // Websocket 是否使用GZIP压缩 0--不使用；1--使用
            target.WebsocketClient_UseGZIP = 0;

            // Websocket 的协议类型
            target.WebsocketClient_ProtocolType = 0;


            // 是否启动 云筑网 HTTPClient 协议  1--启用；0--禁用；
            target.UseYZW = 0;

            // 云筑网协议 服务器地址  
            target.YZWAddr = "http://192.168.1.10/yzw";

            target.YZW_NotUploadRecord = 0;
            target.YZW_NotUploadUserPhoto = 1;

            #endregion

            #region 机器网络参数 Network

            // Web管理页面开关,1:开,0:关
            target.UseWebPage = 1;

            // Web页面 HTTP端口号, 1-65534
            target.HTTPPort = 80;

            // Web页面 HTTPS端口号, 1-65534
            target.HTTPSPort = 443;

            // 设备Web页面开启SSL``SSL证书使用OpenSSL自签名 1:开,0:关
            target.WebPageUseSSL = 1;



            // 启动UDP
            target.UseUDP = 1;

            // UDP端口号, 1-65534
            target.UDPPort = 8101;

            target.UseTCP = 1;
            target.TCPPort = 8000;
            target.UseTCPSSL = 0;
            target.TCPSSLPort = 8443;
            // 设备通讯密码 8位数字
            target.ConnectPassword = string.Empty;

            target.UseIPV6_UDP = 0;
            target.IPv6_UDPPort = 9101;

            target.UseIPV6_TCP = 0;
            target.IPv6_TCPPort = 9000;

            target.UseIPV6_TCPSSL = 0;
            target.IPv6_TCPSSLPort = 9443;

            target.UseDataEncryption = 0;
            target.EncryptionKey = string.Empty;

            // 启用 Linux Telnet 1:开,0:关
            target.UseTelnet = 0;

            // Telnet端口号, 1-65534
            target.TelnetPort = 23;

            target.UseSSH = 0;
            target.SSHPort = 22;

            target.SSHLoginPassword = "81818181";


            // 启用视频流 1:开,0:关
            target.UseRTSP = 1;

            #endregion

            #region 视频对讲 VideoCall

            //对讲功能  1--启用；0--禁用
            target.VideoCall_Use = 0;


            //本机名称
            target.VideoCall_DeviceName = "";


            //拨号方式  1 --直呼；2--拨号；3 -- 直呼(本楼栋)
            target.VideoCall_DialMode = 1;


            //设备类型  1--楼栋机   2--围墙机
            target.VideoCall_DeviceType = 1;


            //楼栋号
            target.VideoCall_BuildCode = "8888";


            //本机号
            target.VideoCall_LocalCode = "9999";


            //服务中心号码
            target.VideoCall_Help = "8888*8888";


            //安保中心号码
            target.VideoCall_SecurityHelp = "8899*8899";



            //SIP电话功能开关 1--启用；0--禁用
            target.SIP_Use = 0;


            //SIP服务器地址 
            target.SIP_Server = "";


            //SIP服务器端口
            target.SIP_Port = 5060;


            //SIP用户名
            target.SIP_UserName = "";


            //SIP密码
            target.SIP_Password = "";


            //紧急电话
            target.SIP_EmergencyCall = "";



            //STUN 功能开关 1--启用；0--禁用

            target.STUN_Use = 0;


            //STUN服务器地址
            target.STUN_Server = "";


            //STUN服务器端口
            target.STUN_Port = 3478;


            //STUN用户名
            target.STUN_UserName = "";


            //STUN密码
            target.STUN_Password = "";


            //TURN 功能开关 1--启用；0--禁用
            target.TURN_Use = 0;


            //TURN服务器地址
            target.TURN_Server = "";


            //TURN服务器端口

            target.TURN_Port  = 3478;


            //TURN用户名
            target.TURN_UserName = "";


            //TURN密码
            target.TURN_Password = "";
            #endregion

            #region 门禁参数 Door
            // 卡号字节；3、4、8；0--表示禁用读卡
            target.CardBytes = 4;


            // 出入类型 0,入门；1,出门
            target.AccessType = 0;

            // WG输出格式 26 / 34/66
            target.WgFormat = 34;

            // WG输出内容： 1--用户号；2--卡号
            target.WGContent = 1;

            // 开门保持时间 0-65535（s）。0表示0.5秒
            target.ReleaseTime = 3;

            // 延迟开锁时间 0-65535（s）。0表示禁用
            target.DelayOpenDoorTime = 0;

            // 免验证开门 1--启用；0--禁用
            target.FreeOpen = 0;

            // 开门识别间隔 0--禁用 =0; 1-65535（ms）
            target.OpenInterval = 2;

            // 开门识别间隔期间，是否保存记录 0--不保存；1--保存
            target.OpenInterval_SaveRecord = 0;

            // 继电器否支持双稳态``1为支持,0为不支持
            target.Relay = 0;

            // 合法验证后的短消息
            target.ShortMessage = string.Empty;

            // 验证方式   
            target.VerificationType = 1;

            // 权限到期提示 1--启用；0--禁用
            target.OverdueRemind = 0;

            // 权限到期提示天数 1-255
            target.OverdueRemind_Day = 30;

            // 定时常开功能  1--启用；0--禁用
            target.TimingOpen = 0;

            // 定时常开.自动开模式
            target.TimingOpen_mode = 3;

            // 定时常开.常开时段 使用周时段结构
            target.TimingOpen_timegroup = new HTTPDeviceParameterTimegroup();


            // 定时锁定功能  1--启用；0--禁用
            target.TimingLocked = 0;

            // 定时锁定.锁定时段 使用周时段结构
            target.TimingLocked_timegroup = new HTTPDeviceParameterTimegroup();

            // 访客根密码
            target.VisitorRootPassword = string.Empty;

            // 多人组合开门，人数；1-50；
            target.MultiPerson = 0;

            target.DailyLimit = 0;
            #endregion

            #region 电梯功能参数 Elevator
            // 电梯功能开关,1:开,0:关
            target.UseElevator = 0;
            #endregion

            #region 报警参数 Alarm
            // 消防报警,0,关闭；1,开启
            target.FireAlarm = 1;


            //开门超时报警开关,1:开,0:关
            target.DoorLongOpenAlarm = 0;

            // 开门超时时间,门打开超过这个时间就报警	1-65535（s）
            target.DoorLongOpenTime = 0;



            // 门磁报警,0,关闭；1,开启
            target.DoorSensorAlarm = 0;

            // 门磁报警不报警时段,周时段格式
            target.DoorSensorAlarmTimegroup = new HTTPDeviceParameterTimegroup();

            // 黑名单报警,0,关闭；1,开启
            target.BlacklistAlarm = 0;


            // 防拆报警功能开关,0,关闭；1,开启
            target.AntiDisassemblyAlarm = 0;



            // 非法验证报警功能,0,关闭；1,开启
            target.IllegalVerificationAlarm = 0;

            // 非法验证报警功能-非法认证次数,1-255 ，超过此次数报警
            target.IllegalVerificationAlarmLimit = 0;



            // 允许用户验证解除报警  开关,0,关闭；1,开启
            target.UseUserCloseAlarm = 0;



            // 胁迫报警密码功能,0,关闭；1,开启
            target.PasswordAlarm = 0;

            // 胁迫报警密码,输入此密码则发生报警,密码仅支持数字,可以包含0。
            target.PasswordAlarm_Password = "11011011";

            // 胁迫报警报警发生时的工作模式 
            target.PasswordAlarm_Mode = 2;
            #endregion

            return JsonConvert.SerializeObject(target);
        }
        #endregion


        public async Task FormatDevice(int iSN)
        {
            var deviceDetail = Find<DeviceDetail>(iSN);
            deviceDetail.UploadStatus = 0;
            deviceDetail.LastUpdatetime = DateTime.Now;

            string sLogTitle = _LanguageHandler.GetUserLog("t3");//设备管理
            string sLogFormat = _LanguageHandler.GetUserLog("r26");//初始化设备：{0}({1})
            AddUserLog(sLogTitle, string.Format(sLogFormat, deviceDetail.Name, deviceDetail.SN),
                $"{deviceDetail.Name}({deviceDetail.SN})", string.Empty);


            await this.CommitAsync();

            var accessService = _ServiceProvider.GetService<IDeviceAccessService>();
            await accessService.ReuploadByDevice(deviceDetail.ID);
        }

        public async Task UpdateDeviceSoft(string url, string ver, string softMD5, int id)
        {
            var map = _Cache.GetDeviceDictionary();
            if (!map.ContainsKey(id)) return;
            var deviceDetail = map[id];

            string sLogTitle = _LanguageHandler.GetUserLog("t3");//设备管理
            string sLogFormat = _LanguageHandler.GetUserLog("r27");//设置设备更新固件：{0}({1})，新固件版本号{2}
            AddUserLog(sLogTitle, string.Format(sLogFormat, deviceDetail.DeviceName, deviceDetail.SN, ver),
               $"{deviceDetail.DeviceName}({deviceDetail.SN})", string.Empty);

            this.Commit();

            _Cache.UpdateDeviceCache(deviceDetail.SN, x =>
            {
                x.UpdateSoftURL = url;
                x.UpdateSoftVer = ver;
                x.UpdateSoftMD5 = softMD5;
            });

            //添加远程任务
            var remoteService = this._ServiceProvider.GetService<IDeviceRemoteService>();

            //查询是否有相同的任务
            var pushDTO = new PushSoftwareDTO()
            {
                SoftwareURL = url,
                SoftwareVer = ver,
                SoftwareMD5 = softMD5,
            };
            var remoteTask = new RemoteTaskAddDTO()
            {
                DeviceIDs = new List<int>([deviceDetail.ID]),
                TaskType = RemoteTypeEnum.PushSoftware,
                UserID = 0,
                TaskExtension = JsonConvert.SerializeObject(pushDTO),
            };

            await remoteService.Add(remoteTask);

        }







        public void ReplaceDeviceSN(DeviceDetail oDBDevice, string sNewSN)
        {
            var sOldSN = oDBDevice.SN;
            oDBDevice.SN = sNewSN;
            //缓存切换
            var sns = _Cache.GetDevices();
            sns.Remove(sOldSN);
            sns.Add(sNewSN);


            var dto = _Cache.GetDevice(sOldSN);
            Cache.Remove(sOldSN);
            Cache.Set(sNewSN, dto);
            dto.SN = sNewSN;

            Commit();
        }


        public async Task UpdateAllDeviceUploadStatus()
        {
            var db = Context.Set<DeviceDetail>();
            await db.ExecuteUpdateAsync(s => s.SetProperty(f => f.UploadStatus, v => 0));

            var devicequrey = _Cache.GetDevices();

            foreach (var sn in devicequrey)
            {

                _Cache.UpdateDeviceCache(sn, x =>
                {
                    x.UploadStatus = 0;
                });
            }

        }

        /// <summary>
        /// 对设备进行远程抓拍
        /// </summary>
        public async Task<JsonResultModel> RemoteSnapshoot(RemoteSnapshootDTO dto)
        {
            var oDeviceMap = _Cache.GetDeviceDictionary();

            if (!oDeviceMap.TryGetValue(dto.DeviceID, out var device))
                return new JsonResultModel(101,
                    _LanguageHandler.GetCheckParameterErrorMessage("r72"));// "设备不存在"


            var onlineTime = (int)(DateTime.Now - device.LastKeepaliveTime).TotalSeconds;
            if (onlineTime > 60)
            {
                return new JsonResultModel(102,
                    _LanguageHandler.GetCheckParameterErrorMessage("DeviceOffline"));// "设备不在线"
            }
            string sCacheKey = $"Snapshoot_{device.SN}";
            _Cache.Remove(sCacheKey);

            //添加远程任务
            var remoteService = this._ServiceProvider.GetService<IDeviceRemoteService>();

            //查询是否有相同的任务
            var remoteTask = new RemoteTaskAddDTO()
            {
                DeviceIDs = new List<int>([device.ID]),
                TaskType = RemoteTypeEnum.Snapshoot,
                UserID = 0,
                TaskExtension = "",
            };


            //创建任务令牌
            var tcs = new TaskCompletionSource<string>();
            _Cache.Set(sCacheKey, tcs);

            await remoteService.Add(remoteTask);
            Task timeoutTask = Task.Delay(120 * 1000);
            // 等待任务完成或超时
            Task completedTask = await Task.WhenAny(tcs.Task, timeoutTask);
            _Cache.Remove(sCacheKey);
            if (completedTask == timeoutTask)
            {
                tcs.SetCanceled();
                return new JsonResultModel(103,
                    _LanguageHandler.GetRemoteService("TaskWaitTimeout"));// "任务等待超时"
            }
            else
            {
                var photo = tcs.Task.Result;
                _logger.LogInformation($"从设备获取现场照片完成 {photo}");
                photo = $"/PushSnapshoot/{device.SN}.jpg?{DateTimeOffset.UtcNow.ToUnixTimeSeconds()}";
                return new JsonResultModel(photo);
            }



        }

    }
}
