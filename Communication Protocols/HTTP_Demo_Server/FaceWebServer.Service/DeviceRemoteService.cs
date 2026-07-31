using FaceWebServer.DB.Table;
using FaceWebServer.DTO;
using FaceWebServer.DTO.Cache;
using FaceWebServer.DTO.Remote;
using FaceWebServer.Interface;
using FaceWebServer.Language;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace FaceWebServer.Service
{
    /// <summary>
    /// 设备远程操作服务
    /// </summary>
    public class DeviceRemoteService : BaseService, IDeviceRemoteService
    {

        public ICacheService _Cache { get; set; }
        public IFaceDriveService _DriveDB { get; set; }



        private LanguageHandler _LanguageHandler;

        public DeviceRemoteService(DbContext context, IOptionsSnapshot<LanguageOption> lngopt) : base(context)
        {
            _LanguageHandler = lngopt.Value.GetCurrentLanguageHandler();
        }

        public PageResult<RemoteTaskDetail> Query(RemoteTaskQueryDTO queryDto)
        {

            //组装查询条件
            List<Expression<Func<RemoteTaskDetail, bool>>> oWheres = new();
            if (queryDto.TaskID.HasValue) oWheres.Add(x => x.TaskID == queryDto.TaskID.Value);
            if (!string.IsNullOrWhiteSpace(queryDto.SN)) oWheres.Add(x => x.SN.Contains(queryDto.SN));
            if (queryDto.TaskType.HasValue) oWheres.Add(x => x.TaskType == queryDto.TaskType.Value);
            if (queryDto.TaskStatus.HasValue) oWheres.Add(x => x.TaskStatus == queryDto.TaskStatus.Value);

            var queryResult = QueryPage(oWheres, queryDto.PageSize, queryDto.PageIndex,
                x => x.CreateTime, queryDto.IsAsc);

            return queryResult;

        }

        public List<RemoteTaskDetail> GetRemoteTaskBySN(string sSN)
        {
            return Query<RemoteTaskDetail>(x => x.SN == sSN && x.TaskStatus == 0
            && x.TaskType < RemoteTypeEnum.ClearAllPeople).ToList();
        }


        public async Task Add(RemoteTaskAddDTO parameter)
        {
            if (parameter.TaskType == RemoteTypeEnum.QueryPeople && (parameter.UserID <= 0 || parameter.UserID > uint.MaxValue))
                return;

            //需要刷新缓存的设备列表
            HashSet<string> snList = new();

            var DeviceDict = _Cache.GetDeviceDictionary();


            var db = Context.Set<RemoteTaskDetail>();

            string sRemoteTitle = _LanguageHandler.GetRemoteService($"remote{parameter.TaskType}");
            //新增设备远程操作任务： 详情：门：{0}({1})，操作类型：{2}
            string sLogFormat = _LanguageHandler.GetUserLog("r17");

            CacheDeviceDTO door;
            foreach (var iDeviceID in parameter.DeviceIDs)
            {
                if (DeviceDict.ContainsKey(iDeviceID))
                {

                    door = DeviceDict[iDeviceID];
                    snList.Add(door.SN);
                    var dtl = new RemoteTaskDetail()
                    {
                        SN = door.SN,
                        TaskType = parameter.TaskType,
                        UserID = parameter.UserID,
                        TaskExtension = parameter.TaskExtension,
                        TaskStatus = 0,
                        CreateTime = DateTime.Now,
                    };


                    //AddUserLog("设备远程操作", $"新增设备远程操作任务： 详情：{dtl.ToJSON()}");

                    AddUserLog(_LanguageHandler.GetUserLog("t2"), //"设备远程操作",
                        string.Format(sLogFormat, door.DeviceName, door.SN, sRemoteTitle),// "新增设备远程操作任务： 详情：{0}"
                        door.SN, string.Empty);

                    db.Add(dtl);
                }

            }
 
            await this.CommitAsync();



            await UpdateRemoteTotal(snList);

        }


        public async Task Delete(List<int> taskIDs)
        {
            var db = Context.Set<RemoteTaskDetail>();
            //查询需要删除的设备
            HashSet<int> taskIDLists = new HashSet<int>(taskIDs);
            //需要刷新缓存的设备列表
            HashSet<string> snList = new HashSet<string>();

            var tasks = db.Where(x => taskIDLists.Contains(x.TaskID));

            //删除设备远程操作任务： 详情：门：{0}，操作类型：{1}
            string sLogFormat = _LanguageHandler.GetUserLog("r19");
            string sDeviceName, sSN;
            foreach (var item in tasks)
            {
                string sRemoteTitle = _LanguageHandler.GetRemoteService($"remote{item.TaskType}");

                var device = _Cache.GetDevice(item.SN);

                if (device != null)
                {
                    sDeviceName = device.DeviceName;
                    sSN = device.SN;
                    snList.Add(device.SN);
                }
                else
                {
                    sDeviceName = string.Empty;
                    sSN = string.Empty;
                }

                //AddUserLog("设备远程操作", $"删除设备远程操作任务： 详情：{item.ToJSON()}");
                AddUserLog(_LanguageHandler.GetUserLog("t2"), //"设备远程操作",
                        string.Format(sLogFormat, sDeviceName, sSN, sRemoteTitle, item.TaskStatus),// "新增设备远程操作任务： 详情：{0}"
                        $"{sDeviceName}({sSN})", string.Empty);

            }
            await CommitAsync();

            await tasks.ExecuteDeleteAsync();


            await UpdateRemoteTotal(snList);
        }


        public async Task UpdateTaskRunStatusComplete(List<int> taskIDs, int deviceID, string sn)
        {
            var db = Context.Set<RemoteTaskDetail>();

            var query = db.Where(x => taskIDs.Contains(x.TaskID));
            foreach (var detail in query)
            {
                var sRemoteTitle = _LanguageHandler.GetRemoteService($"remote{detail.TaskType}");

                //执行远程操作任务： 详情：门：{0}({1})，操作类型：{2}
                AddUserLog(_LanguageHandler.GetUserLog("t2"),
                    string.Format(_LanguageHandler.GetUserLog("r20"), string.Empty, detail.SN, sRemoteTitle),
                     detail.SN, string.Empty);

                if (detail.TaskType == RemoteTypeEnum.Recover)//设备恢复出厂设备，重置所有权限和门禁参数
                {
                    await _DriveDB.FormatDevice(deviceID);
                }
                //Update<RemoteTaskDetail>(detail);
            }

            db.Where(x => taskIDs.Contains(x.TaskID))
                .ExecuteUpdate(u =>
                u.SetProperty(f => f.TaskStatus, v => 1)
                .SetProperty(f => f.TaskRunTime, v => DateTime.Now));

            await this.CommitAsync();


            await UpdateRemoteTotal(new string[] { sn });


        }

        public async Task ClearRemote()
        {
            var db = Context.Set<RemoteTaskDetail>();
            await db.ExecuteDeleteAsync();
            //AddUserLog("设备远程操作", "清空所有设备远程操作记录");
            AddUserLog(_LanguageHandler.GetUserLog("t2"), //"设备远程操作",
                       _LanguageHandler.GetUserLog("r18"));

            await CommitAsync();

            var query = _Cache.GetDevices();

            foreach (var sn in query)
            {
                _Cache.UpdateDeviceCache(sn, x =>
                {
                    x.RemoteTaskTotal = 0;
                    x.EmptyPeople = 0;
                    x.UploadWorkParameterTaskTotal = 0;
                });
            }
        }

        /// <summary>
        /// 需要更新的设备ID
        /// </summary>
        /// <param name="DoorIDs"></param>
        public async Task UpdateRemoteTotal(IEnumerable<string> snList)
        {


            //加载所有待上传任务，并分组统计
            var query = await (from dTask in Set<RemoteTaskDetail>()
                               where snList.Contains(dTask.SN) && dTask.TaskStatus == 0
                               group dTask by new { dTask.SN, dTask.TaskType } into groupedItems
                               select new { groupedItems.Key.SN, groupedItems.Key.TaskType, Count = groupedItems.Count() })
                         .ToListAsync();

            foreach (var sn in snList)
            {

                var Door = _Cache.GetDevice(sn);

                Door.RemoteTaskTotal = 0;
                Door.EmptyPeople = 0;
                Door.UploadWorkParameterTaskTotal = 0;

                var doorQuery = query.Where(d => d.SN == sn).ToList();
                if (doorQuery.Count > 0)
                {
                    var EmptyPeopleTask = doorQuery.Where(d => d.TaskType == RemoteTypeEnum.ClearAllPeople).FirstOrDefault();

                    if (EmptyPeopleTask != null)
                        Door.EmptyPeople = EmptyPeopleTask.Count;


                    var UploadWorkParameterTaskTotal = doorQuery.Where(d => d.TaskType == RemoteTypeEnum.UploadWorkSetting).FirstOrDefault();
                    if (UploadWorkParameterTaskTotal != null)
                        Door.UploadWorkParameterTaskTotal = UploadWorkParameterTaskTotal.Count;

                    Door.RemoteTaskTotal = query.Where(d => d.TaskType < RemoteTypeEnum.ClearAllPeople).Sum(d => d.Count);
                }
            }

            query = null;

        }







    }
}
