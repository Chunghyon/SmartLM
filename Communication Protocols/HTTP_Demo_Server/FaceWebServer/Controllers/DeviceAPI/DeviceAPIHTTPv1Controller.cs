using DeviceProtocolServer.Utilities;
using DoNetDrive.Common.Extensions;
using FaceWebServer.DB.Table;
using FaceWebServer.DTO.Access;
using FaceWebServer.DTO.Cache;
using FaceWebServer.DTO.Config;
using FaceWebServer.DTO.Device;
using FaceWebServer.DTO.HTTPv1_Protocol;
using FaceWebServer.DTO.Remote;
using FaceWebServer.Interface;
using FaceWebServer.Language;
using FaceWebServer.Utility;
using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using NPOI.SS.Formula.Functions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace DeviceProtocolServer.Controllers.DeviceAPI
{
    [ApiController]
    public class DeviceAPIHTTPv1Controller : ControllerBase
    {


        private static object lockobject = new object();

        private readonly ILogger<DeviceAPIHTTPv1Controller> _logger;
        private IServiceProvider _ServiceProvider;

        private ICacheService _Cache;
        private LanguageHandler _LanguageHandler;

        private HTTPProtocolOption httpOption = null;

        static DeviceAPIHTTPv1Controller()
        {
            SystemRecordConvertMap = new Dictionary<int, int>();
            CreateRecordConvertMap();
        }

        public DeviceAPIHTTPv1Controller(ILogger<DeviceAPIHTTPv1Controller> logger,
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
        private JsonResult? CheckDeviceReg(string SN, out CacheDeviceDTO oDevice)
        {
            HTTPAPIResultV1 rst = new();
            oDevice = null;
            if (string.IsNullOrWhiteSpace(SN))
            {
                rst.Success = 1;
                rst.Msg = _LanguageHandler.GetAPIResultMessage("r1");// "设备未注册！";
                return new JsonResult(rst);
            }

            if (SN.Length < 16 || SN == "0000000000000000")
            {
                rst.Success = 1;
                rst.Msg = _LanguageHandler.GetAPIResultMessage("r1");// "设备未注册！";
                return new JsonResult(rst);
            }

            oDevice = _Cache.GetDevice(SN);
            if (oDevice == null)
            {
                rst.Success = 1;
                rst.Msg = _LanguageHandler.GetAPIResultMessage("r1");// "设备未注册！";
                return new JsonResult(rst);
            }
            return null;
        }

        #region 保活包
        /// <summary>
        /// 心跳保活包
        /// </summary>
        [Route("/device/updateStateDevice")]
        [HttpPost]
        public IActionResult DeviceKeepalive([FromBody] HTTPv1DeviceKeepaliveRequest par)
        {
            var result = new HTTPv1DeviceKeepaliveResponse();

            //_logger.LogInformation($" 设备心跳包 device/updateStateDevice SN:{par.DeviceID}");

            if (string.IsNullOrWhiteSpace(par.DeviceID))
            {
                result.SyncParameter = 1;
            }
            else
            {
                if (par.DeviceID.Length < 16 || par.DeviceID == "0000000000000000")
                {
                    result.SyncParameter = 1;
                }
                else
                {

                    #region 检查参数是否需要同步
                    var oDevice = _Cache.GetDevice(par.DeviceID);
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

                        if (oDevice.Protocol != DeviceDetail.HTTPv1)
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


        #region 属性配置
        /// <summary>
        ///  上传参数 设备将机器现有配置上传服务器
        /// </summary>
        /// <param name="par"></param>
        /// <returns></returns>
        [Route("/parameter/inertParameter")]
        [HttpPost]
        public async Task<IActionResult> UploadWorkParameter([FromBody] HTTPDeviceUploadParameterRequestV1 par)
        {
            //_logger.LogInformation($" 设备上传参数 parameter/inertParameter SN:{par.DeviceID}");
            string SN = par.DeviceID;
            if (string.IsNullOrWhiteSpace(SN))
            {
                return new JsonResult(new HTTPAPIResultV1());//没有SN不处理
            }

            if (SN.Length < 16 || SN == "0000000000000000")
            {
                return new JsonResult(new HTTPAPIResultV1());//没有SN不处理
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
                    return new JsonResult(new HTTPAPIResultV1());//没有SN不处理
                }
            }

            var _DriveDB = _ServiceProvider.GetService<IFaceDriveService>();
            var _RemoteDB = _ServiceProvider.GetService<IDeviceRemoteService>();


            if(!par.ParameterServerProtocol.HasValue)
            {
                par.ParameterServerProtocol = 0;
            }

            dbDevice.SN = SN;
            dbDevice.Protocol = DeviceDetail.HTTPv1;
            dbDevice.DeviceVer = par.ParameterFirmwareMsg.Replace("Ver ", string.Empty);
            dbDevice.IsEntry = par.ParameterAccessType == 0 ? 1 : 0;
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

            return new JsonResult(new HTTPAPIResultV1());
        }


        /// <summary>
        ///  拉取参数 设备请求同步参数，将服务器存储内容发送到设备
        /// </summary>
        /// <param name="par"></param>
        /// <returns></returns>
        [Route("/parameter/selectParameterInfo")]
        [HttpPost]
        public IActionResult GetWorkParameter([FromBody] HTTPDeviceRequestV1 par)
        {
            //_logger.LogInformation($" 设备拉取参数 parameter/selectParameterInfo SN:{par.DeviceID}");

            var detail = new HTTPDeviceDownloadParameterResponseV1();
            var _DriveDB = _ServiceProvider.GetService<IFaceDriveService>();

            //设置设备工作参数为默认值
            void setDef()
            {
                lock (lockobject)
                {
                    var defValue = _DriveDB.GetDefaultValue(DeviceDetail.HTTPv1);
                    var defDevice = JsonConvert.DeserializeObject<HTTPDeviceParameterCoreV1>(defValue);

                    //从数据库拉取默认值发过去
                    defDevice.ParameterProductionDate = DateTime.Now.ToDateTimeStr();

                    defDevice.ParameterSerialNum = defDevice.DeviceID;


                    DeviceDetail dbDevice = new DeviceDetail();
                    dbDevice.SN = defDevice.DeviceID;
                    dbDevice.Protocol = DeviceDetail.HTTPv1;
                    dbDevice.DeviceVer = string.Empty;
                    dbDevice.IsEntry = defDevice.ParameterAccessType == 0 ? 1 : 0;
                    dbDevice.Detail = JsonConvert.SerializeObject(defDevice);

                    //将设备加入到列表中
                    _DriveDB.Add(dbDevice);
                    detail = defDevice.Adapt<HTTPDeviceDownloadParameterResponseV1>();
                    string sSNNum = defDevice.DeviceID.Substring(10, 6);
                    sSNNum = (sSNNum.ToInt32() + 1).ToString("000000");

                    defDevice.DeviceID = defDevice.DeviceID.Substring(0, 10) + sSNNum;
                    defValue = JsonConvert.SerializeObject(defDevice);

                    var setPar = new SetDefaultValueRequestDTO()
                    {
                        Protocol = DeviceDetail.HTTPv1,
                        DefaultJson = defValue
                    };
                    _DriveDB.SaveDefaultValue(setPar);

                    
                }

            }

            if (string.IsNullOrWhiteSpace(par.DeviceID))
            {
                setDef();

            }
            else
            {
                if (par.DeviceID.Length < 16 || par.DeviceID == "0000000000000000")
                {
                    setDef();
                }
                else
                {
                    var chkRet = CheckDeviceReg(par.DeviceID, out CacheDeviceDTO oCacheDevice);
                    if (chkRet != null) return chkRet;
                    var oDBDevice = _DriveDB.Find<DeviceDetail>(oCacheDevice.ID);

                    detail = JsonConvert.DeserializeObject<HTTPDeviceDownloadParameterResponseV1>(oDBDevice.Detail);

                    //要求设备固件升级
                    if (!string.IsNullOrEmpty(oCacheDevice.UpdateSoftMD5))
                    {
                        detail.parameterFirmwareMd5 = oCacheDevice.UpdateSoftMD5;
                        detail.parameterFirmwarePath = oCacheDevice.UpdateSoftURL;
                        detail.ParameterFirmwareMsg = oCacheDevice.UpdateSoftVer;
                        oCacheDevice.UpdateSoftMD5 = string.Empty;
                        oCacheDevice.UpdateSoftURL = string.Empty;
                        oCacheDevice.UpdateSoftVer = string.Empty;
                    }

                    oCacheDevice.UploadStatus = 1;
                    oDBDevice.UploadStatus = 1; //更新为已同步
                    oDBDevice.UploadStatusTime = DateTime.Now;
                    _DriveDB.Commit();

                    //替换设备
                    if (oDBDevice.SN != detail.DeviceID)
                    {
                        if (detail.DeviceID == "0000000000000000")
                        {
                            //删除
                            _DriveDB.Delete(new List<int>() { oCacheDevice.ID });

                        }
                        else
                        {
                            //更新SN
                            _DriveDB.ReplaceDeviceSN(oDBDevice, detail.DeviceID);
                        }
                    }

                    detail.ParameterSerialNum = detail.DeviceID;
                }
            }
            detail.ParameterSystemTime = TimestampUtility.ToUnixTimestampBySeconds(DateTime.Now);
            detail.Timegroups = GetTimeGroups();

            return new JsonResult(detail);
        }

        private List<HTTPOpenDoorTimegroupV1> GetTimeGroups()
        {
            var _TimeGroupDB = _ServiceProvider.GetService<ITimeGroupService>();

            var list = _TimeGroupDB.GetAll().Adapt<List<HTTPOpenDoorTimegroupV1>>();
            //使用精简的时段定义
            foreach (var tGroup in list)
            {
                tGroup.week1 = SimplifyWeekTimeSection(tGroup.week1);
                tGroup.week2 = SimplifyWeekTimeSection(tGroup.week2);
                tGroup.week3 = SimplifyWeekTimeSection(tGroup.week3);
                tGroup.week4 = SimplifyWeekTimeSection(tGroup.week4);
                tGroup.week5 = SimplifyWeekTimeSection(tGroup.week5);
                tGroup.week6 = SimplifyWeekTimeSection(tGroup.week6);
                tGroup.week7 = SimplifyWeekTimeSection(tGroup.week7);
            }
            return list;
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
            {
                var sTime = string.Join("/", useTimes.ToArray());

                return sTime;

            }
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
        [Route("/device/selectRestart")]
        [HttpPost]
        public async Task<IActionResult> DeviceRemote([FromBody] HTTPDeviceRequestV1 par)
        {
            var chkRet = CheckDeviceReg(par.DeviceID, out CacheDeviceDTO oDevice);
            if (chkRet != null) return chkRet;

            var _RemoteDB = _ServiceProvider.GetService<IDeviceRemoteService>();

            var oRemoteList = _RemoteDB.GetRemoteTaskBySN(par.DeviceID);


            var result = new HTTPRemoteCommandResponse();

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

                    case RemoteTypeEnum.CloseAlarm://关闭报警
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
                    default:
                        break;

                }

            }
            if (oRemoteList.Count > 0)
            {
                await _RemoteDB.UpdateTaskRunStatusComplete(oRemoteList.Select(x => x.TaskID).ToList(),
                oDevice.ID, oDevice.SN);
            }


            if (httpOption.TestQRCodeAndOpenDoor)
            {
                result.Opendoor = 1;//1--打开继电器
                oDevice.RemoteTaskTotal = 0;
            }

            return new JsonResult(result);
        }
        #endregion


        #region 人员操作
        /// <summary>
        ///  添加人员 设备主动触发云端导入雇员授权信息
        /// </summary>
        [Route("/devicePass/selectPassInfo")]
        [HttpPost]
        public async Task<IActionResult> ReadPeople([FromBody] HTTPReadPeopleRequest par)
        {
            //_logger.LogInformation($" 设备拉取人事信息 ReadPeople SN:{par.DeviceID}");

            HTTPReadPeopleResponse rst = new();
            CacheDeviceDTO oDevice = null;
            var chkRet = CheckDeviceReg(par.DeviceID, out oDevice);
            if (chkRet != null) return chkRet;

            if (par.Limit == 0) par.Limit = 50;
            if (par.Limit > 2000) par.Limit = 2000;
            if (httpOption.UploadPeoplePhotoType == HTTPProtocolOption.UseBase64)
            {
                par.Limit = 1;
            }

            //检查有没有需要添加的人员
            var TotalDetail = oDevice.NewAccessTotal;

            if (TotalDetail == 0)
            {
                //没有需要上传的人员
                rst.Success = 2;
                rst.Msg = _LanguageHandler.GetAPIResultMessage("r2");//  "没有需要同步的人员!";
                return new JsonResult(rst);
            }
            var _AccessDB = _ServiceProvider.GetService<IDeviceAccessService>();

            //从数据库中获取需要导入的人员
            var accessList = await _AccessDB.GetDownloadAccess(oDevice.ID, par.Limit);


            if (accessList != null)
            {
                rst.Count = accessList.Count;
                if (rst.Count > 0)
                    rst.PassInfo = new List<HTTPPeopleV1>(rst.Count);

                var peopleMap = _Cache.GetPeopleDictionary();
                IMapper mapper = new Mapper();
                foreach (var access in accessList)
                {
                    HTTPPeopleV1 httpPeople = new HTTPPeopleV1();
                    var dbPeople = peopleMap[access.PeopleID];

                    mapper.Map(access, httpPeople);
                    mapper.Map(dbPeople, httpPeople);

                    if (access.OpenTimes == 0)
                    {
                        httpPeople.DevicePassBean.DevicePassNumber = -1;

                    }

                    if (dbPeople.PhotoLen == 0)
                    {
                        httpPeople.EmployeePhotoWay = string.Empty;
                        httpPeople.PhotoMD5 = string.Empty;
                        httpPeople.EmployeePhoto = string.Empty;
                    }
                    else
                    {
                        if (httpOption.UploadPeoplePhotoType == HTTPProtocolOption.UseBase64)
                        {
                            //读取文件，转为Base64
                            var sFile = FileHelpers.GetPeopleImagePath(dbPeople.Photo);

                            //检查文件是否存在
                            if (!string.IsNullOrEmpty(sFile) && System.IO.File.Exists(sFile))
                            {
                                var bBuf = System.IO.File.ReadAllBytes(sFile);

                                httpPeople.EmployeePhotoWay = "base";
                                httpPeople.EmployeePhoto = bBuf.ToBase64();
                            }
                            else
                            {
                                httpPeople.EmployeePhotoWay = string.Empty;
                                httpPeople.PhotoMD5 = string.Empty;
                                httpPeople.EmployeePhoto = string.Empty;
                            }

                        }
                    }

                    rst.PassInfo.Add(httpPeople);
                }
            }
            if (accessList != null)
            {
                if (httpOption.UploadPeoplePhotoMd5 == false)
                {
                    //消除md5
                    foreach (var access in rst.PassInfo)
                    {
                        if (!string.IsNullOrEmpty(access.PhotoMD5))
                        {
                            access.PhotoMD5 = string.Empty;
                        }
                    }
                }
                else
                {
                    //md5 编码转换

                    foreach (var access in rst.PassInfo)
                    {
                        if (!string.IsNullOrEmpty(access.PhotoMD5))
                        {
                            //将hex md5 转为 base64 md5
                            var bBuf = access.PhotoMD5.HexToByte();
                            access.PhotoMD5 = bBuf.ToBase64();

                        }
                    }

                }
            }


            return new JsonResult(rst);
        }

        /// <summary>
        /// 添加人员反馈  拉取人员导入人脸机后的结果反馈
        /// </summary>
        [Route("/devicePass/setPassResult")]
        [HttpPost]
        public async Task<IActionResult> ReadPeopleResult([FromBody] HTTPReadPeopleResultRequest par)
        {
            //_logger.LogInformation($" 设备推送人事保存结果 ReadPeopleResult SN:{par.DeviceID}");
            CacheDeviceDTO oDevice = null;
            var chkRet = CheckDeviceReg(par.DeviceID, out oDevice);
            if (chkRet != null) return chkRet;
            if (httpOption.UploadPeopleSaveResult == false)
            {
                return new JsonResult(new HTTPAPIResultV1());
            }
            //获取设备拉取的人员列表
            var downloadAccessList = _Cache.GetPeopleAccessList(par.DeviceID);
            if (downloadAccessList == null) return new JsonResult(new HTTPAPIResultV1());
            if (downloadAccessList.Count == 0) return new JsonResult(new HTTPAPIResultV1());

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
            if (par.FailNumber > 0)
            {
                foreach (var item in par.FailEmployeeId)
                {
                    if (peopleInfos.ContainsKey(item.EmployeeID))
                    {
                        var p = peopleInfos[item.EmployeeID];
                        p.UploadResult = 100 + item.ErrorCode;
                        p.UploadResultMsg = item.ErrMsg;
                        p.RepeatID = item.RepeatID;
                    }

                }
            }
            var _AccessDB = _ServiceProvider.GetService<IDeviceAccessService>();
            await _AccessDB.UpdatePeopleAccessUploadResult(oDevice.ID, peopleInfos.Values.ToList());


            return new JsonResult(new HTTPAPIResultV1());
        }



        /// <summary>
        /// 删除人员   拉取待删除的人员
        /// </summary>
        [Route("/devicePass/selectDeleteInfo")]
        [HttpPost]
        public async Task<IActionResult> DeletePeople([FromBody] HTTPDeletePeopleRequest par)
        {
            //_logger.LogInformation($" 设备拉取待删除人员 DeletePeople SN:{par.DeviceID}");

            HTTPDeletePeopleResponse rst = new();
            CacheDeviceDTO oDevice = null;
            var chkRet = CheckDeviceReg(par.DeviceID, out oDevice);
            if (chkRet != null) return chkRet;




            //检查有没有需要删除的人员

            if (oDevice.DeleteAccessTotal == 0 && oDevice.EmptyPeople == 0)
            {
                //没有需要上传的人员
                rst.Success = 2;
                rst.Msg = _LanguageHandler.GetAPIResultMessage("r3");//"没有需要删除的人员!";
                return new JsonResult(rst);
            }
            var _AccessDB = _ServiceProvider.GetService<IDeviceAccessService>();

            if (oDevice.AccessTotal == 0 || oDevice.EmptyPeople > 0)
            {
                //需要删除所有人员
                rst.Empty = 1;
                rst.Success = 0;
                rst.Msg = _LanguageHandler.GetAPIResultMessage("r4");// "删除所有人员";
                _AccessDB.AddUserLog(_LanguageHandler.GetUserLog("t1"),// "权限管理",
                                     string.Format(_LanguageHandler.GetUserLog("r1"), par.DeviceID)); //$"设备SN:{par.DeviceID} ，请求清空人员命令");
                return new JsonResult(rst);
            }

            if (par.Limit == 0) par.Limit = 50;
            if (par.Limit > 1000) par.Limit = 1000;

            //从数据库中获取需要删除的人员
            var oDeleteMap = await _AccessDB.GetDeleteAccess(oDevice.ID, par.Limit);
            rst.DeleteCount = oDeleteMap.Count;
            if (oDeleteMap.Count > 0)
            {
                rst.DeleteInfo = oDeleteMap.Values.Select(x => new HTTPDeletePeopleDetail() { EmployeeID = x }).ToList();
            }

            return new JsonResult(rst);
        }

        /// <summary>
        /// 删除人员反馈   设备从服务器拉取的待删除的人员操作后的结果反馈
        /// </summary>
        [Route("/devicePass/setDeleteResult")]
        [HttpPost]
        public async Task<IActionResult> ReadDeletePeople([FromBody] HTTPDeletePeopleResultRequest par)
        {
            //_logger.LogInformation($" 设备推送删除人员操作结果 ReadDeletePeople SN:{par.DeviceID}");

            var chkRet = CheckDeviceReg(par.DeviceID, out CacheDeviceDTO oDevice);
            if (chkRet != null) return chkRet;
            var _AccessDB = _ServiceProvider.GetService<IDeviceAccessService>();
            var _RemoteDB = _ServiceProvider.GetService<IDeviceRemoteService>();


            if (par.DeleteAll == 1)
            {
                //删除已所有人员
                if (oDevice.EmptyPeople == 1)
                {
                    //更新远程操作任务，检查是否有要求设备上传参数的任务
                    var remoteList = _RemoteDB.Query(new RemoteTaskQueryDTO()
                    {
                        SN = par.DeviceID,
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
                }
                return new JsonResult(new HTTPAPIResultV1());
            }


            //获取上次拉取的待删除人员列表
            var cacheDeleteMap = _Cache.GetDeletePeopleAccessList(par.DeviceID);
            if (cacheDeleteMap == null) return new JsonResult(new HTTPAPIResultV1());
            if (cacheDeleteMap.Count == 0) return new JsonResult(new HTTPAPIResultV1());

            await _AccessDB.SaveDeleteAccessResult(oDevice.ID, cacheDeleteMap.Values.ToList());

            return new JsonResult(new HTTPAPIResultV1());
        }
        #endregion


        #region 打卡记录

        /// <summary>
        /// 系统记录和HTTPv1上传的记录代码转换字典
        /// key 是httpv1设备记录代码
        /// value 是系统记录代码 <1000是系统记录 >=1000是门磁记录
        /// </summary>
        /// <returns></returns>
        private static Dictionary<int, int> SystemRecordConvertMap;
        /// <summary>
        /// 创建记录转换字典
        /// </summary>
        private static void CreateRecordConvertMap()
        {
            SystemRecordConvertMap.Add(118, 1);//软件开门
            SystemRecordConvertMap.Add(119, 2);//软件关门
            SystemRecordConvertMap.Add(120, 3);//软件常开
            SystemRecordConvertMap.Add(121, 4);//控制器自动进入常开
            SystemRecordConvertMap.Add(122, 5);//控制器自动关闭门
            SystemRecordConvertMap.Add(123, 8);//软件锁定
            SystemRecordConvertMap.Add(124, 9);//软件解除锁定
            SystemRecordConvertMap.Add(125, 14);//非法认证报警
            SystemRecordConvertMap.Add(101, 15);//门磁报警
            SystemRecordConvertMap.Add(126, 16);//胁迫报警
            SystemRecordConvertMap.Add(100, 17);//开门超时报警
            SystemRecordConvertMap.Add(112, 18);//黑名单报警
            SystemRecordConvertMap.Add(108, 19);//消防报警
            SystemRecordConvertMap.Add(127, 20);//防拆报警
            SystemRecordConvertMap.Add(128, 21);//非法认证报警解除
            SystemRecordConvertMap.Add(110, 22);//门磁报警解除
            SystemRecordConvertMap.Add(129, 23);//胁迫报警解除
            SystemRecordConvertMap.Add(109, 24);//开门超时报警解除
            SystemRecordConvertMap.Add(113, 25);//黑名单报警解除
            SystemRecordConvertMap.Add(111, 26);//消防报警解除
            SystemRecordConvertMap.Add(130, 27);//防拆报警解除
            SystemRecordConvertMap.Add(104, 28);//系统加电
            SystemRecordConvertMap.Add(131, 29);//系统错误复位（看门狗）
            SystemRecordConvertMap.Add(102, 30);//设备格式化记录
            SystemRecordConvertMap.Add(132, 34);//网线已断开
            SystemRecordConvertMap.Add(133, 35);//网线已插入
            SystemRecordConvertMap.Add(106, 36);//WIFI 已连接
            SystemRecordConvertMap.Add(107, 37);//WIFI 已断开
            SystemRecordConvertMap.Add(134, 38);//蓝牙开门
            //SystemRecordConvertMap.Add(103, 40);//在设备菜单中清空所有人员
            SystemRecordConvertMap.Add(135, 41);//在设备菜单中备份人员到U盘
            SystemRecordConvertMap.Add(136, 42);//在设备菜单中从U盘导入人员
            SystemRecordConvertMap.Add(137, 43);//室内机远程开门
            SystemRecordConvertMap.Add(116, 44);//删除所有记录
            SystemRecordConvertMap.Add(103, 45);//删除所有人员


            SystemRecordConvertMap.Add(114, 1001);//门磁开
            SystemRecordConvertMap.Add(115, 1002);//门磁-关门
            SystemRecordConvertMap.Add(117, 1005);//门未关好
            SystemRecordConvertMap.Add(8, 1006);//使用按钮开门
            SystemRecordConvertMap.Add(11, 1007);//按钮开门时门已锁定
        }

        /// <summary>
        /// 设备主动往云端实时上传打卡记录
        /// </summary>
        /// <param name="record"></param>
        /// <returns></returns>
        [Route("/note/insertNoteFace")]
        [HttpPost]
        public async Task<JsonResult> UploadRecord([FromForm] string? recordJson, [FromForm] IFormCollection files)
        {
            if (string.IsNullOrEmpty(recordJson))
            {
                //没有找到 recordJson，可能是gzip压缩了
                recordJson = await FileHelpers.FindGzipJsonString(_logger, files, "recordJson");
            }
            if (string.IsNullOrEmpty(recordJson))
            {
                Console.WriteLine("URL:{0} RemoteIP:{1}:{2} 打卡记录没有找到 recordJson 字段", HttpContext.Request.Path,
                   HttpContext.Connection.RemoteIpAddress,
                   HttpContext.Connection.RemotePort);
                return new JsonResult(new HTTPAPIResultV1() { Success = 400, Msg = "The recordJson field was not found" });
            }


            HTTPPushRecordDetail record = JsonConvert.DeserializeObject<HTTPPushRecordDetail>(recordJson);

            var SN = record.DeviceID;
            var deny = httpOption.Find(SN);
            if (deny != null)
            {
                return new JsonResult(new HTTPAPIResultV1() { Success = deny.Code, Msg = deny.Msg });
            }
            var oRecordDate = record.NoteTime;
            //保存照片
            string sPath = Path.Combine(Directory.GetCurrentDirectory(),
                   "wwwroot", "RecordImage", SN, oRecordDate.ToString("yyyyMM"),
            oRecordDate.ToString("dd"));
            string sFileName = $"{oRecordDate.ToString("yyyy-MM-dd_HH_mm_ss")}.jpg";
            var httpFile = await FileHelpers.SaveHTTPPOSTFile(_logger, files, "pic", sPath,
                sFileName);

            if (httpFile != null)
            {//有照片
                record.Pic_Len = httpFile.FileLength;
                record.ImgURL = $"/RecordImage/{SN}/{oRecordDate:yyyyMM}/{oRecordDate:dd}/{sFileName}";
            }
            else
            {
                //没有照片
                record.Pic_Len = 0;
                record.ImgURL = string.Empty;
            }

            //准备保存记录


            if (record.EventType >= 100 ||
                ((record.EventType == 8 || record.EventType == 11) && record.NoteWay == 10))
            {
                //系统事件
                await SaveSystemRecord(record);
            }
            else
            {
                //打卡事件
                await SaveIdentifyRecord(record);
                //2024年7月22日 添加代码 测试远程开门

                if (httpOption.TestQRCodeAndOpenDoor)
                {
                    var oDevice = _Cache.GetDevice(record.DeviceID);
                    if (oDevice != null)
                    {
                        oDevice.RemoteTaskTotal = 1;
                    }
                }


            }




            return new JsonResult(new HTTPAPIResultV1());
        }

        private async Task SaveSystemRecord(HTTPPushRecordDetail record)
        {
            if (SystemRecordConvertMap.ContainsKey(record.EventType))
            {
                var _RecordDB = _ServiceProvider.GetService<IRecordService>();
                //系统事件
                var dbRecord = record.Adapt<SystemRecord>();

                dbRecord.RecordType = SystemRecordConvertMap[record.EventType];

                await _RecordDB.AddRecord(record.DeviceID, [dbRecord]);
            }
        }

        private async Task SaveIdentifyRecord(HTTPPushRecordDetail record)
        {

            var dbRecord = record.Adapt<IdentifyRecord>();


            if (record.EventType == 1)
            {
                switch (record.NoteWay)
                {
                    case 0://人脸验证
                        dbRecord.RecordType = 3;
                        break;
                    case 1://刷卡验证
                        dbRecord.RecordType = 1;
                        break;
                    case 2://密码验证   用户号加密码
                        dbRecord.RecordType = 10;
                        break;
                    case 3://指纹验证
                        dbRecord.RecordType = 2;
                        break;
                    case 4://掌静脉识别
                        dbRecord.RecordType = 36;
                        break;
                    case 5://访客密码开门
                        dbRecord.RecordType = 31;
                        break;
                    case 6://动态二维码扫码开门
                        dbRecord.RecordType = 32;
                        break;
                    default:
                        return;
                }
            }

            if (record.EventType == 2)
            {
                switch (record.NoteWay)
                {
                    case 0://未注册用户
                        dbRecord.RecordType = 19;
                        break;
                    case 1://未注册卡
                        dbRecord.RecordType = 48;
                        break;
                    case 6://未注册二维码
                        dbRecord.RecordType = 49;
                        break;
                    default:
                        return;
                }
            }

            if (record.EventType == 3)
            {
                switch (record.NoteWay)
                {
                    case 0://有效期过期
                        dbRecord.RecordType = 16;
                        break;
                    case 3://控制器已过期
                        dbRecord.RecordType = 28;
                        break;
                    default:
                        return;
                }
            }

            if (record.EventType == 4)
            {
                dbRecord.RecordType = 15;//重复验证
            }


            if (record.EventType == 9)
            {
                dbRecord.RecordType = 25;//免验证开门
            }

            if (record.EventType == 10)
            {
                switch (record.NoteWay)
                {
                    case 0://黑名单卡
                        dbRecord.RecordType = 24;
                        break;
                    case 1://挂失卡
                        dbRecord.RecordType = 23;
                        break;
                    default:
                        return;
                }
            }


            if (record.EventType == 11)
            {
                switch (record.NoteWay)
                {
                    case 0://锁定时验证，禁止开门
                        dbRecord.RecordType = 22;
                        break;
                    case 1://禁止刷卡验证  --  【权限认证方式】中禁用刷卡时
                        dbRecord.RecordType = 26;
                        break;
                    case 3://禁止指纹验证  --  【权限认证方式】中禁用指纹时
                        dbRecord.RecordType = 27;
                        break;
                    default:
                        return;
                }
            }
            if (record.EventType == 12)
            {
                dbRecord.RecordType = 30;//体温异常，拒绝进入
            }

            if (record.EventType == 13)
            {
                switch (record.NotePass)
                {
                    case 0://组合验证--等待下一个人员
                        dbRecord.RecordType = 44;
                        break;
                    case 1:////组合验证完成
                        dbRecord.RecordType = 46;
                        break;
                    default:
                        return;
                }
            }

            if (record.EventType == 14)
            {
                dbRecord.RecordType = 45;//组合验证失败
            }

            var _RecordDB = _ServiceProvider.GetService<IRecordService>();
            await _RecordDB.AddRecord(dbRecord);
        }
        #endregion
    }



}
