using Autofac.Core;
using DeviceProtocolServer.Utilities;
using DoNetDrive.Common.Extensions;
using FaceWebServer.DB.Table;
using FaceWebServer.DTO.Access;
using FaceWebServer.DTO.Cache;
using FaceWebServer.DTO.Config;
using FaceWebServer.DTO.Device;
using FaceWebServer.DTO.HTTPv2_Protocol;
using FaceWebServer.DTO.People;
using FaceWebServer.DTO.Remote;
using FaceWebServer.Interface;
using FaceWebServer.Language;
using FaceWebServer.Service;
using FaceWebServer.Utility;
using FaceWebServer.Utility.Model;
using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using NPOI.SS.Formula.Functions;
using Org.BouncyCastle.Utilities.Encoders;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace DeviceProtocolServer.Controllers.DeviceAPI
{


    [ApiController]
    public class DeviceAPIHTTPv2Controller : ControllerBase
    {
        private static object lockobject = new object();

        private readonly ILogger<DeviceAPIHTTPv2Controller> _logger;
        private IServiceProvider _ServiceProvider;

        private ICacheService _Cache;
        private LanguageHandler _LanguageHandler;

        private HTTPProtocolOption httpOption = null;

        public DeviceAPIHTTPv2Controller(ILogger<DeviceAPIHTTPv2Controller> logger,
            ICacheService cache,
            IServiceProvider serviceProvider,
            IOptionsSnapshot<LanguageOption> lngopt,
            IOptionsMonitor<HTTPProtocolOption> DenySNconfig)
        {
            _logger = logger;
            _Cache = cache;
            _ServiceProvider = serviceProvider;
            _LanguageHandler = lngopt.Value.GetCurrentLanguageHandler();


            httpOption = DenySNconfig.CurrentValue;
        }



        /// <summary>
        /// 检查设备是否已注册
        /// </summary>
        /// <param name="SN"></param>
        /// <param name="oDevice"></param>
        /// <returns></returns>
        private JsonResult CheckDeviceReg(string SN, out CacheDeviceDTO oDevice)
        {
            HTTPAPIResultV2 rst = new();
            oDevice = null;
            if (string.IsNullOrWhiteSpace(SN))
            {
                rst.Success = 400;
                rst.Message = _LanguageHandler.GetAPIResultMessage("r1");// "设备未注册！";
                return new JsonResult(rst);
            }

            if (SN.Length < 16 || SN == "0000000000000000")
            {
                rst.Success = 400;
                rst.Message = _LanguageHandler.GetAPIResultMessage("r1");// "设备未注册！";
                return new JsonResult(rst);
            }

            oDevice = _Cache.GetDevice(SN);
            if (oDevice == null)
            {
                rst.Success = 401;
                rst.Message = _LanguageHandler.GetAPIResultMessage("r1");// "设备未注册！";
                return new JsonResult(rst);
            }
            return null;
        }

        #region 保活包
        /// <summary>
        /// 心跳保活包
        /// </summary>
        [Route("/Device/Keepalive")]
        [HttpPost]
        public IActionResult DeviceKeepalive([FromBody] HTTPKeepaliveRequestV2 par)
        {


            var result = new HTTPKeepaliveResponseV2();
            if (string.IsNullOrWhiteSpace(par.SN))//设备设备SN为空，则要求设备拉取工作参数，并在服务器端分配新SN
            {
                result.SyncParameter = 1;
            }
            else
            {
                var deny = httpOption.Find(par.SN);
                if (deny != null)
                {
                    return new JsonResult(new HTTPAPIResultV2() { Success = deny.Code, Message = deny.Msg });
                }

                if (par.SN.Length < 16 || par.SN == "0000000000000000")
                {
                    //设备设备不符合规则，则要求设备拉取工作参数，并在服务器端分配新SN
                    result.SyncParameter = 1;
                }
                else
                {

                    #region 检查参数是否需要同步
                    var oDevice = _Cache.GetDevice(par.SN);
                    #endregion

                    if (oDevice != null)
                    {
                        #region 检查权限是否需要同步

                        result.DeletePeople = oDevice.DeleteAccessTotal;
                        if (oDevice.DeleteAccessTotal == 0 && oDevice.EmptyPeople != 0)
                        {
                            result.DeletePeople = 1;
                        }
                        result.AddPeople = oDevice.NewAccessTotal;
                        result.Remote = oDevice.RemoteTaskTotal;

                        if (result.AddPeople.Value == 0)
                            result.AddPeople = null;
                        if (result.DeletePeople.Value == 0)
                            result.DeletePeople = null;
                        if (result.Remote.Value == 0)
                            result.Remote = null;

                        #endregion

                        //oDevice = _DriveDB.Find<FaceDeviceDetail>(oDevice.ID);
                        oDevice.LastKeepaliveTime = DateTime.Now;
                        oDevice.KeepaliveStatus = par;
                        if (oDevice.UploadStatus == 0)
                        {
                            result.SyncParameter = 1;
                        }
                        if (oDevice.UploadWorkParameterTaskTotal > 0)
                        {
                            result.UploadWorkParameter = 1;//上传参数
                        }

                        if (oDevice.Protocol != DeviceDetail.HTTPv2)
                        {
                            result.UploadWorkParameter = 1;//上传参数并注册
                        }
                    }
                    else
                    {
                        result.UploadWorkParameter = 1;//上传参数并注册
                    }
                }


            }

            return new JsonResult(result);
        }


        #endregion

        #region 设备工作参数
        /// <summary>
        /// 设备上传当前所有工作参数
        /// </summary>
        /// <param name="par"></param>
        /// <returns></returns>
        [Route("/Device/UploadWorkSetting")]
        [HttpPost]
        public async Task<IActionResult> DeviceUploadWorkSetting([FromBody] HTTPDeviceUploadWorkSettingRequest par)
        {
            //_logger.LogInformation($" 设备上传参数 parameter/inertParameter SN:{SN}");
            string SN = par.DeviceSN;
            if (string.IsNullOrWhiteSpace(SN))
            {
                return new JsonResult(new HTTPAPIResultV2());//没有SN不处理
            }

            if (SN.Length < 15 || SN == "0000000000000000")
            {
                return new JsonResult(new HTTPAPIResultV2());//没有SN不处理
            }

            var cacheDevice = _Cache.GetDevice(SN);



            DeviceDetail dbDevice = new DeviceDetail();
            if (cacheDevice != null)
            {
                dbDevice.ID = cacheDevice.ID;
                dbDevice.Name = cacheDevice.DeviceName;

                if (cacheDevice.UploadStatus == 0)
                {
                    //需要拉取参数，拒绝设备上传参数
                    return new JsonResult(new HTTPAPIResultV2());//没有SN不处理
                }
            }
            var _DriveDB = _ServiceProvider.GetService<IFaceDriveService>();
            var _RemoteDB = _ServiceProvider.GetService<IDeviceRemoteService>();

            dbDevice.SN = SN;
            dbDevice.Protocol = DeviceDetail.HTTPv2;
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

            return new JsonResult(new HTTPAPIResultV2());
        }



        /// <summary>
        ///  拉取参数 设备请求同步参数，将服务器存储内容发送到设备
        /// </summary>
        /// <param name="par"></param>
        /// <returns></returns>
        [Route("/Device/DownloadWorkSetting")]
        [HttpPost]
        public IActionResult DeviceDownloadWorkSetting([FromBody] HTTPDeviceRequestV2 par)
        {
            //_logger.LogInformation($" 设备拉取参数 parameter/selectParameterInfo SN:{par.DeviceID}");

            HTTPDeviceDownloadWorkSettingResponse detail;
            var _DriveDB = _ServiceProvider.GetService<IFaceDriveService>();

            //设置设备工作参数为默认值
            void setDef()
            {
                lock (lockobject)
                {
                    var defValue = _DriveDB.GetDefaultValue(DeviceDetail.HTTPv2);
                    var defDevice = JsonConvert.DeserializeObject<HTTPDeviceParameterCoreV2>(defValue);
                    //从数据库拉取默认值发过去
                    defDevice.ProductionDate = DateTime.Now.ToDateTimeStr();
                    if (defDevice.IllegalVerificationAlarmLimit <= 0)
                        defDevice.IllegalVerificationAlarmLimit = 3;
                    if (defDevice.MultiPerson <= 0)
                        defDevice.MultiPerson = 1;
                    if (defDevice.WebsocketClient_KeepaliveTime <= 0)
                        defDevice.WebsocketClient_KeepaliveTime = 60;
                    defDevice.WebsocketClient_ProtocolType = 100;

                    defDevice.DeviceName = "-";

                    DeviceDetail dbDevice = new DeviceDetail();
                    dbDevice.SN = defDevice.DeviceSN;
                    dbDevice.Protocol = DeviceDetail.HTTPv2;
                    dbDevice.DeviceVer = string.Empty;
                    dbDevice.IsEntry = defDevice.AccessType == 0 ? 1 : 0;
                    dbDevice.Detail = JsonConvert.SerializeObject(defDevice);

                    //将设备加入到列表中
                    _DriveDB.Add(dbDevice);

                    detail = defDevice.Adapt<HTTPDeviceDownloadWorkSettingResponse>();
                    string sSNNum = defDevice.DeviceSN.Substring(10, 6);
                    sSNNum = (sSNNum.ToInt32() + 1).ToString("000000");

                    defDevice.DeviceSN = defDevice.DeviceSN.Substring(0, 10) + sSNNum;
                    defValue = JsonConvert.SerializeObject(defDevice);

                    var setPar = new SetDefaultValueRequestDTO()
                    {
                        Protocol = DeviceDetail.HTTPv2,
                        DefaultJson = defValue
                    };
                    _DriveDB.SaveDefaultValue(setPar);
                }

            }


            if (string.IsNullOrWhiteSpace(par.SN))
            {
                setDef();

            }
            else
            {
                if (par.SN.Length < 16 || par.SN == "0000000000000000")
                {
                    setDef();
                }
                else
                {
                    var chkRet = CheckDeviceReg(par.SN, out CacheDeviceDTO oCacheDevice);
                    if (chkRet != null) return chkRet;
                    var oDBDevice = _DriveDB.Find<DeviceDetail>(oCacheDevice.ID);

                    detail = JsonConvert.DeserializeObject<HTTPDeviceDownloadWorkSettingResponse>(oDBDevice.Detail);


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
            }

            detail.SystemTime = TimestampUtility.ToUnixTimestampBySeconds(DateTime.Now.AddHours(-1));
            //detail.SystemTime = detail.SystemTime - (60 * 10);

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

            return new JsonResult(detail);
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
        /// 远程操作 设备请求远程操作任务
        /// </summary>
        /// <param name="par"></param>
        /// <returns></returns>
        [Route("/Device/RemoteCommand")]
        [HttpPost]
        public async Task<IActionResult> Device_RemoteCommand([FromBody] HTTPDeviceRequestV2 par)
        {
            var chkRet = CheckDeviceReg(par.SN, out CacheDeviceDTO oDevice);
            if (chkRet != null) return await Task.FromResult(chkRet);
            var _RemoteDB = _ServiceProvider.GetService<IDeviceRemoteService>();

            var oRemoteList = _RemoteDB.GetRemoteTaskBySN(par.SN);

            var result = new HTTPDeviceRemoteCommandResponse();

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
                        break;
                    case RemoteTypeEnum.RepostRecord://12、重新上传所有记录
                        result.RepostRecord = 1;
                        break;
                    case RemoteTypeEnum.ClearRecord://13、清空所有记录
                        result.ClearRecord = 1;
                        break;
                    case RemoteTypeEnum.PushSoftware://14、推送固件
                        result.PushSoftware = JsonConvert.DeserializeObject<PushSoftwareDTO>(item.TaskExtension);
                        break;
                    case RemoteTypeEnum.PushSystemFile://15、推送系统文件
                        //result.PushSoftware = 1;
                        result.PushSystemFile = JsonConvert.DeserializeObject<List<PushSystemFileDTO>>(item.TaskExtension);
                        break;
                    case RemoteTypeEnum.Snapshoot://16、获取设备摄像头快照
                        result.Snapshoot = 1;
                        break;

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
                    case RemoteTypeEnum.RegisterIdentifyTicket://22 上传人员注册凭证
                        result.RegisterIdentifyTicket = new RegisterIdentifyTicketDTO();
                        result.RegisterIdentifyTicket.UserID = item.UserID.Value;
                        var Regdto = JsonConvert.DeserializeObject<EnrollUserMediaDataDTO>(item.TaskExtension);
                        result.RegisterIdentifyTicket.RegisterType = Regdto.EnrollTypeToInt();
                        result.RegisterIdentifyTicket.RegisterIndex = Regdto.EnrollIndex;
                        break;




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
            await _RemoteDB.UpdateTaskRunStatusComplete(oRemoteList.Select(x => x.TaskID).ToList(),
                oDevice.ID, oDevice.SN);

            //result.UploadRecordRange = new RemoteCommand_UploadRecordRangeDTO
            //{
            //    BeginTime = 1743145069,
            //    EndTime = 1743145160
            //};

            return new JsonResult(result);
        }


        /// <summary>
        /// 反馈人员注册凭证的结果
        /// </summary>
        /// <param name="ResultJson">人员注册凭证的结果</param>
        /// <param name="files"></param>
        /// <returns></returns>
        [Route("/Device/RegisterIdentifyTicketResult")]
        [HttpPost]
        public async Task<IActionResult> Device_RegisterIdentifyTicketResult([FromForm] string? ResultJson,
            [FromForm] IFormCollection files)
        {
            if (string.IsNullOrEmpty(ResultJson))
            {
                //没有找到 Detail，可能是gzip压缩了
                ResultJson = await FileHelpers.FindGzipJsonString(_logger, files, "ResultJson");
            }
            if (string.IsNullOrEmpty(ResultJson))
            {
                //设备中不存在指定的用户号
                _logger.LogInformation($"反馈人员注册凭证的结果 参数错误，缺少字段: ResultJson");
                return new JsonResult(new HTTPAPIResultV2());
            }
            var RegisterResult = JsonConvert.DeserializeObject<HTTPDeviceRegisterIdentifyTicketResult>(ResultJson);

            var people = RegisterResult.UserDetail;
            if (people != null)
            {
                string sSavePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "PushPeople");
                string sImageFileName = $"{RegisterResult.UserID}.jpg";
                var httpFile = await FileHelpers.SaveHTTPPOSTFile(_logger, files, "Photo", sSavePath, sImageFileName);
                if (httpFile != null)
                {
                    people.PhotoLen = httpFile.FileLength;

                    people.PhotoMD5 = httpFile.FileMD5;

                    people.Photo = httpFile.FileName;
                }
                else
                {
                    people.PhotoLen = 0;
                    people.PhotoMD5 = string.Empty;
                    people.Photo = string.Empty;
                }
            }
            string sCacheKey = $"RegisterIdentifyTicket_{RegisterResult.SN}";

            _Cache.Set(sCacheKey, RegisterResult, TimeSpan.FromMinutes(10));
            _logger.LogInformation($"反馈人员注册凭证的结果 {RegisterResult.SN} 用户号：{RegisterResult.UserID} 结果：{RegisterResult.Result}");
            return new JsonResult(new HTTPAPIResultV2());
        }

        /// <summary>
        /// 上传设备摄像头快照
        /// </summary>
        /// <param name="SN"></param>
        /// <param name="files"></param>
        /// <returns></returns>
        [Route("/Device/UploadSnapshoot")]
        [HttpPost]
        public async Task<IActionResult> Device_UploadSnapshoot([FromForm] string? SN,
            [FromForm] IFormCollection files)
        {

            if (string.IsNullOrEmpty(SN))
            {
                //上传设备摄像头快照
                _logger.LogInformation($"上传设备摄像头快照 参数错误，缺少字段:SN");
                return new JsonResult(new HTTPAPIResultV2());
            }
            string sSavePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "PushSnapshoot");
            string sImageFileName = $"{SN}.jpg";
            var httpFile = await FileHelpers.SaveHTTPPOSTFile(_logger, files, "Photo", sSavePath, sImageFileName);
            if (httpFile == null)
            {
                _logger.LogInformation($"上传设备摄像头快照 参数错误，缺少字段:Photo");
                return new JsonResult(new HTTPAPIResultV2());
            }

            string sCacheKey = $"Snapshoot_{SN}";

            var tsk = _Cache.Get<TaskCompletionSource<string>>(sCacheKey);
            if (tsk != null)
            {
                tsk.SetResult(httpFile.FileName);
            }

            _logger.LogInformation($"上传设备摄像头快照 {SN}");
            return new JsonResult(new HTTPAPIResultV2());
        }

        #endregion


        #region 拉取人员
        /// <summary>
        ///  设备拉取人员授权信息
        /// </summary>
        [Route("/People/DownloadPeopleList")]
        [HttpPost]
        public async Task<IActionResult> PeopleDownloadPeopleList(HTTPPeopleDownloadPeopleListRequest par)
        {
            //_logger.LogInformation($" 设备拉取人事信息 ReadPeople SN:{par.DeviceID}");

            HTTPPeopleDownloadPeopleListResponse rst = new();
            CacheDeviceDTO oDevice = null;
            var chkRet = CheckDeviceReg(par.SN, out oDevice);
            if (chkRet != null) return chkRet;

            if (par.Limit == 0) par.Limit = 50;
            if (par.Limit > 500) par.Limit = 500;

            if (httpOption.UploadPeoplePhotoType == HTTPProtocolOption.UseBase64)
            {
                par.Limit = 1;
            }


            //检查有没有需要添加的人员
            var TotalDetail = oDevice.NewAccessTotal;

            if (TotalDetail == 0)
            {
                //没有需要上传的人员
                rst.Success = 0;
                rst.Message = _LanguageHandler.GetAPIResultMessage("r2");//  "没有需要同步的人员!";
                return new JsonResult(rst);
            }

            var _AccessDB = _ServiceProvider.GetService<IDeviceAccessService>();

            //从数据库中获取需要导入的人员
            var accessList = await _AccessDB.GetDownloadAccess(oDevice.ID, par.Limit);

            if (accessList != null)
            {
                rst.PeopleCount = accessList.Count;
                if (rst.PeopleCount > 0)
                    rst.PeopleList = new List<HTTPPeopleV2>(rst.PeopleCount);

                var peopleMap = _Cache.GetPeopleDictionary();
                IMapper mapper = new Mapper();
                foreach (var access in accessList)
                {
                    HTTPPeopleV2 httpPeople = new HTTPPeopleV2();
                    var dbPeople = peopleMap[access.PeopleID];
                    mapper.Map(access, httpPeople);
                    mapper.Map(dbPeople, httpPeople);

                    rst.PeopleList.Add(httpPeople);
                }
            }
            if (httpOption.UploadPeoplePhotoType == HTTPProtocolOption.UseBase64)
            {
                if (rst.PeopleList.Count > 0)
                {
                    foreach (var people in rst.PeopleList)
                    {
                        if (people.PhotoLen > 0)
                        {
                            //读取文件，转为Base64
                            var sFile = FileHelpers.GetPeopleImagePath(people.Photo);

                            //检查文件是否存在
                            if (!string.IsNullOrEmpty(sFile) && System.IO.File.Exists(sFile))
                            {
                                var bBuf = System.IO.File.ReadAllBytes(sFile);

                                people.PhotoLen = bBuf.Length;
                                people.Photo = bBuf.ToBase64();
                                if (!String.IsNullOrEmpty(people.PhotoMD5))
                                {
                                    people.PhotoMD5 = people.PhotoMD5.ToLower();
                                }
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

            if (httpOption.UploadPeoplePhotoMd5 == false)
            {
                //消除md5.
                foreach (var people in rst.PeopleList)
                {
                    if (people.PhotoLen > 0)
                    {
                        people.PhotoMD5 = string.Empty;

                    }
                }
            }

            var peopleService = _ServiceProvider.GetService<IPeopleService>();

            if (httpOption.FeatureCodeUseBase64 == true)
            {
                // 특징 코드를 Base64로 변환하여 전달
                foreach (var people in rst.PeopleList)
                {
                    await LoadFeatureCode(people, peopleService);
                }
            }
            else
            {
                // URL 방식: 특징 코드 파일 경로를 단말기에 전달
                foreach (var people in rst.PeopleList)
                {
                    await EnsureFeatureCodePaths(people, peopleService);
                }
            }

            if (httpOption.HTTPURLUsePrefix)
            {
                foreach (var people in rst.PeopleList)
                {
                    if (!string.IsNullOrEmpty(people.Photo) && httpOption.UploadPeoplePhotoType == HTTPProtocolOption.UseURL)
                    {
                        people.Photo = httpOption.PeopleURLPrefix + people.Photo;
                    }

                    if (!string.IsNullOrEmpty(people.FaceFeature) && !people.FaceFeature.StartsWith("http"))
                    {
                        people.FaceFeature = httpOption.PeopleURLPrefix + people.FaceFeature;
                    }

                    if (people.Fingerprints != null)
                    {
                        foreach (var fp in people.Fingerprints)
                        {
                            if (!string.IsNullOrEmpty(fp.Data) && !fp.Data.StartsWith("http"))
                                fp.Data = httpOption.PeopleURLPrefix + fp.Data;
                        }
                    }

                    if (people.Palmveins != null)
                    {
                        foreach (var pv in people.Palmveins)
                        {
                            if (!string.IsNullOrEmpty(pv.Data) && !pv.Data.StartsWith("http"))
                                pv.Data = httpOption.PeopleURLPrefix + pv.Data;
                        }
                    }
                }
            }
            return new JsonResult(rst);
        }

        /// <summary>
        /// URL 방식에서 특징 코드 파일 경로의 유효성을 확인하고, 파일이 없으면 경로를 지움
        /// </summary>
        private Task EnsureFeatureCodePaths(HTTPPeopleV2 hPeople, IPeopleService service)
        {
            string wwwroot = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");

            void validatePath(ref string data)
            {
                if (string.IsNullOrEmpty(data)) return;
                if (data.Length > 200) { data = string.Empty; return; } // already base64
                string localPath = data.TrimStart('/').Replace('/', Path.DirectorySeparatorChar);
                string fullPath = Path.Combine(wwwroot, localPath);
                if (!System.IO.File.Exists(fullPath))
                    data = string.Empty;
            }

            if (!string.IsNullOrEmpty(hPeople.FaceFeature))
            {
                string tmp = hPeople.FaceFeature;
                validatePath(ref tmp);
                hPeople.FaceFeature = tmp;
                if (string.IsNullOrEmpty(hPeople.FaceFeature))
                    hPeople.FaceFeatureMD5 = string.Empty;
            }

            if (hPeople.Fingerprints != null)
            {
                foreach (var fp in hPeople.Fingerprints)
                {
                    string tmp = fp.Data;
                    validatePath(ref tmp);
                    fp.Data = tmp;
                }
            }

            if (hPeople.Palmveins != null)
            {
                foreach (var pv in hPeople.Palmveins)
                {
                    string tmp = pv.Data;
                    validatePath(ref tmp);
                    pv.Data = tmp;
                }
            }

            return Task.CompletedTask;
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
                hPeople.FaceFeature = dto.FaceFeature?.Data;
                hPeople.FaceFeatureMD5 = dto.FaceFeature?.MD5;
            }

            if (hPeople.Fingerprints != null && dto.Fingerprints != null)
            {
                for (int i = 0; i < hPeople.Fingerprints.Count && i < dto.Fingerprints.Count; i++)
                {
                    hPeople.Fingerprints[i].Data = dto.Fingerprints[i].Data;
                    hPeople.Fingerprints[i].MD5 = dto.Fingerprints[i].MD5;
                }
            }

            if (hPeople.Palmveins != null && dto.Palmveins != null)
            {
                for (int i = 0; i < hPeople.Palmveins.Count && i < dto.Palmveins.Count; i++)
                {
                    hPeople.Palmveins[i].Data = dto.Palmveins[i].Data;
                    hPeople.Palmveins[i].MD5 = dto.Palmveins[i].MD5;
                }
            }
        }

        /// <summary>
        /// 设备拉取人员后反馈人员保存结果
        /// </summary>
        [Route("/People/DownloadPeopleListResult")]
        [HttpPost]
        public async Task<IActionResult> PeopleDownloadPeopleListResult([FromBody] HTTPPeopleDownloadPeopleListResultRequest par)
        {
            //_logger.LogInformation($" 设备推送人事保存结果 ReadPeopleResult SN:{par.DeviceID}");
            CacheDeviceDTO oDevice = null;
            var chkRet = CheckDeviceReg(par.SN, out oDevice);
            if (chkRet != null) return chkRet;
            if (httpOption.UploadPeopleSaveResult == false)
            {
                //不保存人员存储结果
                return new JsonResult(new HTTPAPIResultV2());
            }
            //获取设备拉取的人员列表
            var downloadAccessList = _Cache.GetPeopleAccessList(par.SN);
            if (downloadAccessList == null) return new JsonResult(new HTTPAPIResultV2());
            if (downloadAccessList.Count == 0) return new JsonResult(new HTTPAPIResultV2());


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


            return new JsonResult(new HTTPAPIResultV2());
        }
        #endregion

        #region 删除人员

        /// <summary>
        /// 设备拉取待删除人员名单
        /// </summary>
        [Route("/People/DeletePeopleList")]
        [HttpPost]
        public async Task<IActionResult> PeopleDeletePeopleList([FromBody] HTTPPeopleDeletePeopleListRequest par)
        {
            //_logger.LogInformation($" 设备拉取待删除人员 DeletePeople SN:{par.DeviceID}");

            HTTPPeopleDeletePeopleListResponse rst = new();
            CacheDeviceDTO oDevice = null;
            var chkRet = CheckDeviceReg(par.SN, out oDevice);
            if (chkRet != null) return chkRet;


            //检查有没有需要删除的人员

            if (oDevice.DeleteAccessTotal == 0 && oDevice.EmptyPeople == 0)
            {
                //没有需要上传的人员
                rst.Success = 0;
                rst.Message = _LanguageHandler.GetAPIResultMessage("r3");//"没有需要删除的人员!";
                return new JsonResult(rst);
            }
            var _AccessDB = _ServiceProvider.GetService<IDeviceAccessService>();

            if (oDevice.EmptyPeople > 0)
            {
                //需要删除所有人员
                rst.DeleteAll = 1;
                rst.Success = 1;
                rst.Message = _LanguageHandler.GetAPIResultMessage("r4");// "删除所有人员";
                _AccessDB.AddUserLog(_LanguageHandler.GetUserLog("t1"),// "权限管理",
                                     string.Format(_LanguageHandler.GetUserLog("r1"), par.SN)); //$"设备SN:{par.DeviceID} ，请求清空人员命令");
                return new JsonResult(rst);
            }

            if (par.Limit == 0) par.Limit = 50;
            if (par.Limit > 1000) par.Limit = 1000;

            //从数据库中获取需要删除的人员
            var oDeleteMap = await _AccessDB.GetDeleteAccess(oDevice.ID, par.Limit);
            rst.DeleteCount = oDeleteMap.Count;
            if (oDeleteMap.Count > 0)
            {
                rst.DeleteList = oDeleteMap.Values.ToList();
            }

            return new JsonResult(rst);
        }

        /// <summary>
        /// 删除人员反馈   设备从服务器拉取的待删除的人员操作后的结果反馈
        /// </summary>
        [Route("/People/DeletePeopleListResult")]
        [HttpPost]
        public async Task<IActionResult> PeopleDeletePeopleListResult([FromBody] HTTPPeopleDeletePeopleListResultRequest par)
        {
            //_logger.LogInformation($" 设备推送删除人员操作结果 ReadDeletePeople SN:{par.DeviceID}");

            var chkRet = CheckDeviceReg(par.SN, out CacheDeviceDTO oDevice);
            if (chkRet != null) return chkRet;
            var _AccessDB = _ServiceProvider.GetService<IDeviceAccessService>();
            var _RemoteDB = _ServiceProvider.GetService<IDeviceRemoteService>();

            if (par.DeleteAll == 1)
            {
                //删除已所有人员

                //更新远程操作任务，检查是否有要求设备上传参数的任务
                var remoteList = _RemoteDB.Query(new RemoteTaskQueryDTO()
                {
                    SN = par.SN,
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

                return new JsonResult(new HTTPAPIResultV2());
            }


            //获取上次拉取的待删除人员列表
            var cacheDeleteMap = _Cache.GetDeletePeopleAccessList(par.SN);
            if (cacheDeleteMap == null) return new JsonResult(new HTTPAPIResultV2());
            if (cacheDeleteMap.Count == 0) return new JsonResult(new HTTPAPIResultV2());

            await _AccessDB.SaveDeleteAccessResult(oDevice.ID, cacheDeleteMap.Values.ToList());

            return new JsonResult(new HTTPAPIResultV2());
        }
        #endregion




        #region 推送人员
        /// <summary>
        /// 设备推送人员到服务器
        /// </summary>
        [Route("/People/PushPeople")]
        [HttpPost]
        public async Task<IActionResult> PeoplePushPeople([FromForm] string SN, [FromForm] int PushType,
            [FromForm] string? Detail,
            [FromForm] long UserID,
            [FromForm] IFormCollection files)
        {

            if (string.IsNullOrEmpty(Detail))
            {
                //没有找到 Detail，可能是gzip压缩了
                Detail = await FileHelpers.FindGzipJsonString(_logger, files, "Detail");
            }

            //if (string.IsNullOrEmpty(Detail))
            //{
            //    Console.WriteLine("URL:{0} RemoteIP:{1}:{2} 推送人员接口没有 Detail 字段", HttpContext.Request.Path,
            //       HttpContext.Connection.RemoteIpAddress,
            //       HttpContext.Connection.RemotePort);
            //    return new JsonResult(new HTTPAPIResultV2() { Success = 400, Message = "The Detail field was not found" });
            //}

            HTTPPeoplePushPeopleRequest par = new HTTPPeoplePushPeopleRequest()
            {
                SN = SN,
                PushType = PushType,
                UserID = UserID,

            };
            if (!string.IsNullOrEmpty(Detail))
                par.Detail = JsonConvert.DeserializeObject<HTTPPeopleV2>(Detail);
            else
            {
                //设备中不存在指定的用户号
                _logger.LogInformation($"设备推送人员 设备中查询不到指定的用户号:{UserID}");
                return new JsonResult(new HTTPAPIResultV2());
            }

            string sSavePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "PushPeople");
            string sImageFileName = $"{par.Detail.UserID}.jpg";
            var httpFile = await FileHelpers.SaveHTTPPOSTFile(_logger, files, "Photo", sSavePath, sImageFileName);
            if (httpFile != null)
            {
                par.Detail.PhotoLen = httpFile.FileLength;
                if (string.IsNullOrEmpty(par.Detail.PhotoMD5))
                {
                    par.Detail.PhotoMD5 = httpFile.FileMD5;
                }
                par.Detail.Photo = $"/People/{sImageFileName}?md5={par.Detail.PhotoMD5}";
                par.Photo = httpFile.FileName;
            }
            else
            {
                par.Detail.PhotoLen = 0;
                par.Detail.PhotoMD5 = string.Empty;
                par.Detail.Photo = string.Empty;
                par.Photo = string.Empty;
            }


            var _PeopleDB = _ServiceProvider.GetService<IPeopleService>();
            //检查用户号是否存在
            var cachePeople = _Cache.GetPeopleCache(par.Detail.UserID);
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
                    _ = Directory.CreateDirectory(Path.GetDirectoryName(sNewFile));
                    if (System.IO.File.Exists(sNewFile))
                        System.IO.File.Delete(sNewFile);

                    if (System.IO.File.Exists(par.Photo))
                        System.IO.File.Move(par.Photo, sNewFile);
                }
                catch (Exception ex)
                {
                    _logger.LogError("设备推送人员，保存人员入库时发生错误！" + ex.Message);
                }

            }
            else
            {
                return new JsonResult(new HTTPAPIResultV2() { Success = 0, Message = addRet.Error });
            }

            return new JsonResult(new HTTPAPIResultV2());
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
        [Route("/Record/UploadIdentifyRecord")]
        [HttpPost]
        public async Task<JsonResult> RecordUploadIdentifyRecord([FromForm] string SN, [FromForm] string? RecordDetail, [FromForm] IFormCollection files)
        {
            var deny = httpOption.Find(SN);
            if (deny != null)
            {
                return new JsonResult(new HTTPAPIResultV2() { Success = deny.Code, Message = deny.Msg });
            }

            if (string.IsNullOrEmpty(RecordDetail))
            {
                //没有找到 RecordDetail，可能是gzip压缩了
                RecordDetail = await FileHelpers.FindGzipJsonString(_logger, files, "RecordDetail");
            }
            if (string.IsNullOrEmpty(RecordDetail))
            {
                Console.WriteLine("URL:{0} RemoteIP:{1}:{2} 打卡记录没有找到 RecordDetail 字段", HttpContext.Request.Path,
                   HttpContext.Connection.RemoteIpAddress,
                   HttpContext.Connection.RemotePort);
                return new JsonResult(new HTTPAPIResultV2() { Success = 400, Message = "The RecordDetail field was not found" });
            }

            HTTPRecordUploadIdentifyRecordDetail record = null;
            try
            {
                record = JsonConvert.DeserializeObject<HTTPRecordUploadIdentifyRecordDetail>(RecordDetail);
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, $"Json解码时发生错误：{RecordDetail}");
            }

            if (record == null)
            {
                return new JsonResult(new HTTPAPIResultV2() { Success = 500, Message = "Json string error" });
            }

            //_logger.LogInformation($"设备推送打卡记录 {RecordDetail}");


            var oRecordDate = TimestampUtility.ToLocalTimeDateBySeconds(record.RecordDate);

            string sPath = Path.Combine(Directory.GetCurrentDirectory(),
                   "wwwroot", "RecordImage", SN, oRecordDate.ToString("yyyyMM"),
            oRecordDate.ToString("dd"));
            string sFileName = $"{oRecordDate.ToString("yyyy-MM-dd_HH_mm_ss")}.jpg";
            var httpFile = await FileHelpers.SaveHTTPPOSTFile(_logger, files, "Photo", sPath,
                sFileName);


            if (httpFile != null)
            {
                record.PhotoLen = httpFile.FileLength;
                record.Photo = $"/RecordImage/{SN}/{oRecordDate:yyyyMM}/{oRecordDate:dd}/{sFileName}";
                _logger.LogInformation($"设备推送打卡记录 {RecordDetail} \n 记录有照片，长度:{record.PhotoLen} 保存地址：{record.Photo}");
            }
            else
            {
                record.PhotoLen = 0;
                record.Photo = string.Empty;
                _logger.LogInformation($"设备推送打卡记录 {RecordDetail} \n 记录无照片");
            }
            var _RecordDB = _ServiceProvider.GetService<IRecordService>();


            if (record.BodyTemp > 0)
                record.BodyTemp = record.BodyTemp / 10;
            if (string.IsNullOrEmpty(record.CardNum)) record.CardNum = "0";

            var dbRecord = record.Adapt<IdentifyRecord>();
            dbRecord.SN = SN;
            await _RecordDB.AddRecord(dbRecord);

            return new JsonResult(new HTTPAPIResultV2());
        }


        #endregion



        #region 推送系统记录
        private const long MinxTime = 1577808000;
        /// <summary>
        /// 设备推送系统记录
        /// </summary>
        /// <returns></returns>
        [Route("/Record/UploadSystemRecord")]
        [HttpPost]
        public async Task<JsonResult> RecordUploadSystemRecord([FromBody] HTTPRecordUploadSystemRecordRequest recordDto)
        {
            _logger.LogInformation("设备推送系统记录");

            recordDto.Records.RemoveAll(x =>
            {
                var result = x.RecordDate < MinxTime;
                if (result)
                {
                    _logger.LogInformation($"设备推送系统记录时间错误，sn:{recordDto.SN},记录序号:{x.RecordID}");
                }
                return result;
            });
            if (recordDto.Records.Count > 0)
            {
                var recordList = recordDto.Records.Adapt<List<SystemRecord>>();

                if (recordDto.RecordType == 1)
                {
                    foreach (var item in recordList)
                    {
                        item.RecordType += 1000;
                    }
                }


                var _RecordDB = _ServiceProvider.GetService<IRecordService>();
                if (_RecordDB != null)
                    await _RecordDB.AddRecord(recordDto.SN, recordList);
            }
            return new JsonResult(new HTTPAPIResultV2());
        }

        #endregion


        #endregion


        #region 在线请求鉴权
        static int RequestAuthorizationCount = 0;
        /// <summary>
        /// 设备在线请求鉴权
        /// </summary>
        /// <returns></returns>
        [Route("/Device/RequestAuthorization")]
        [HttpPost]
        public async Task<JsonResult> Device_RequestAuthorization([FromForm] string SN, [FromForm] string? RecordDetail, [FromForm] IFormCollection files)
        {

            if (string.IsNullOrEmpty(RecordDetail))
            {
                //没有找到 RecordDetail，可能是gzip压缩了
                RecordDetail = await FileHelpers.FindGzipJsonString(_logger, files, "RecordDetail");
            }
            if (string.IsNullOrEmpty(RecordDetail))
            {
                _logger.LogInformation($"URL:{HttpContext.Request.Path} RemoteIP:{HttpContext.Connection.RemoteIpAddress}:{HttpContext.Connection.RemotePort} 设备在线请求鉴权没有找到 RecordDetail 字段");
                return new JsonResult(new HTTPAPIResultV2() { Success = 400, Message = "The RecordDetail field was not found" });
            }


            var record = JsonConvert.DeserializeObject<HTTPRecordUploadIdentifyRecordDetail>(RecordDetail);


            _logger.LogInformation($"设备请求鉴权  RecordDetail {RecordDetail}");


            var oRecordDate = TimestampUtility.ToLocalTimeDateBySeconds(record.RecordDate);

            string sPath = Path.Combine(Directory.GetCurrentDirectory(),
                   "wwwroot", "RequestAuthorization");
            string sFileName = $"{SN}.jpg";
            var httpFile = await FileHelpers.SaveHTTPPOSTFile(_logger, files, "Photo", sPath,
                sFileName);


            if (httpFile != null)
            {
                record.PhotoLen = httpFile.FileLength;
                record.Photo = $"/RequestAuthorization/{sFileName}";
                _logger.LogInformation($"设备请求鉴权  设备上传照片 文件大小：{record.PhotoLen}");
            }
            else
            {
                _logger.LogInformation($"设备请求鉴权  设备没有上传照片");
                record.PhotoLen = 0;
                record.Photo = string.Empty;
            }



            if (record.BodyTemp > 0)
                record.BodyTemp = record.BodyTemp / 10;

            if (string.IsNullOrEmpty(record.CardNum)) record.CardNum = "0";
            var dbRecord = record.Adapt<IdentifyRecord>();
            dbRecord.SN = SN;
            _Cache.Set($"RequestAuthorization_{SN}", dbRecord, TimeSpan.FromMinutes(10));


            int iResult = Random.Shared.Next(0, 2);
            //int iWaitTime = 1 * 1000;//Random.Shared.Next(2000, 5000);
            //if (RequestAuthorizationCount % 2 == 0)
            //{
            //    iWaitTime = 1 * 1000;
            //}
            iResult = 1;
            RequestAuthorizationCount++;

            string sMessage = "HTTP 验证成功";
            if (iResult == 0)
                sMessage = "HTTP 拒绝开门！";
            //_logger.LogInformation($"请求验证 结果：{iResult} ,延迟：{iWaitTime} ms");
            //if (iWaitTime > 0)
            //    await Task.Delay(iWaitTime);
            _logger.LogInformation($"请求验证 返回结果：{iResult}");
            return new JsonResult(new HTTPAPIResultV2()
            {
                Success = iResult,
                Message = sMessage
            });
        }


        #endregion

    }
}
