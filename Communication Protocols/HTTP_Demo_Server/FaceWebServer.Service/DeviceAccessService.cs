using DoNetDrive.Common.Extensions;
using FaceWebServer.DB.Table;
using FaceWebServer.DTO.Access;
using FaceWebServer.DTO.Cache;
using FaceWebServer.Interface;
using FaceWebServer.Language;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace FaceWebServer.Service
{
    /// <summary>
    /// 设备门禁权限服务
    /// </summary>
    public class DeviceAccessService : BaseService, IDeviceAccessService
    {
        public ICacheService _Cache { get; set; }
        private LanguageHandler _LanguageHandler;

        //private static Dictionary<int, string> PeopleAddErrCodeDict;
        private static HashSet<int> RepeatErrCodes;

        static DeviceAccessService()
        {
            RepeatErrCodes = new HashSet<int>();
            RepeatErrCodes.Add(12);
            RepeatErrCodes.Add(23);
            //RepeatErrCodes.Add(16); //2021年8月4日 反馈说已废弃这个代码
            //RepeatErrCodes.Add(17); //2021年8月4日 反馈说已废弃这个代码

            //PeopleAddErrCodeDict = new Dictionary<int, string>();
            //PeopleAddErrCodeDict.Add(0, "导入成功              ");
            //PeopleAddErrCodeDict.Add(1, "人脸被拒绝            ");
            //PeopleAddErrCodeDict.Add(2, "人员信息数据错误      ");
            //PeopleAddErrCodeDict.Add(3, "无法检测脸部          ");
            //PeopleAddErrCodeDict.Add(4, "人脸特征值提取失败    ");
            //PeopleAddErrCodeDict.Add(5, "检测到多张脸          ");
            //PeopleAddErrCodeDict.Add(6, "图像尺寸不符合要求    ");
            //PeopleAddErrCodeDict.Add(7, "图像解码失败          ");
            //PeopleAddErrCodeDict.Add(8, "图像拷贝出错          ");
            //PeopleAddErrCodeDict.Add(9, "没有图像              ");
            //PeopleAddErrCodeDict.Add(10, "人员信息写入数据库失败");
            //PeopleAddErrCodeDict.Add(11, "图像画质过低          ");
            //PeopleAddErrCodeDict.Add(12, "人脸相似度太高        ");
            //PeopleAddErrCodeDict.Add(13, "超过一个注册限制      ");
            //PeopleAddErrCodeDict.Add(14, "图像格式不符合要求    ");
            //PeopleAddErrCodeDict.Add(15, "应用程序内部错误      ");
            ////PeopleAddErrCodeDict.Add(16, "重复注册错误          ");//2021年8月4日 反馈说已废弃这个代码
            ////PeopleAddErrCodeDict.Add(17, "人脸被人为是别人      ");//2021年8月4日 反馈说已废弃这个代码
            ////PeopleAddErrCodeDict.Add(18, "人脸注册校验成功      ");//2021年8月4日 反馈说已废弃这个代码
            ////PeopleAddErrCodeDict.Add(19, "人脸注册校验失败      ");//2021年8月4日 反馈说已废弃这个代码
            //PeopleAddErrCodeDict.Add(20, "Json 数据解析出错     ");
        }


        public DeviceAccessService(DbContext context,
            IOptionsSnapshot<LanguageOption> lngopt) : base(context)
        {
            _LanguageHandler = lngopt.Value.GetCurrentLanguageHandler();
        }

        /// <summary>
        /// 拼装查询条件
        /// </summary>
        /// <param name="queryDTO"></param>
        /// <param name="dbPeople"></param>
        /// <param name="dbDevice"></param>
        /// <param name="dbAccess"></param>
        private void CreateQueryDBContext(DeviceAccessQueryDTO queryDTO,
            out IQueryable<People> dbPeople,
            out IQueryable<DeviceDetail> dbDevice,
            out IQueryable<PeopleAccessDetail> dbAccess)
        {
            List<Expression<Func<People, bool>>> oPeopleWheres = new();
            List<Expression<Func<DeviceDetail, bool>>> oDeviceWheres = new();
            List<Expression<Func<PeopleAccessDetail, bool>>> oAccessWheres = new();
            #region 拼接条件
            if (queryDTO.AccessID.HasValue) oAccessWheres.Add(x => x.AccessID == queryDTO.AccessID.Value);
            if (queryDTO.PeopleID.HasValue) oAccessWheres.Add(x => x.PeopleID == queryDTO.PeopleID.Value);
            if (queryDTO.DeviceID.HasValue) oAccessWheres.Add(x => x.DeviceID == queryDTO.DeviceID.Value);
            if (queryDTO.UserID.HasValue) oAccessWheres.Add(x => x.UserID == queryDTO.UserID.Value);
            if (queryDTO.AccessType.HasValue) oAccessWheres.Add(x => x.AccessType == queryDTO.AccessType.Value);
            if (queryDTO.UploadStatus.HasValue) oAccessWheres.Add(x => x.UploadStatus == queryDTO.UploadStatus.Value);
            if (queryDTO.UploadResult.HasValue)
            {
                if (queryDTO.UploadResult == 0)
                {
                    oAccessWheres.Add(x => x.UploadResult == 0);
                }

                if (queryDTO.UploadResult == 1)
                {
                    oAccessWheres.Add(x => x.UploadResult == 1);
                }

                if (queryDTO.UploadResult == 2)
                {
                    oAccessWheres.Add(x => x.UploadResult > 1);
                }
            }


            if (!string.IsNullOrWhiteSpace(queryDTO.SN)) oDeviceWheres.Add(x => x.SN.Equals(queryDTO.SN));
            if (!string.IsNullOrWhiteSpace(queryDTO.Protocol)) oDeviceWheres.Add(x => x.Protocol.Contains(queryDTO.Protocol));
            if (!string.IsNullOrWhiteSpace(queryDTO.DeviceName)) oDeviceWheres.Add(x => x.Name.Contains(queryDTO.DeviceName));
            if (!string.IsNullOrWhiteSpace(queryDTO.DeviceName)) oDeviceWheres.Add(x => x.Name.Contains(queryDTO.DeviceName));


            if (!string.IsNullOrWhiteSpace(queryDTO.Name)) oPeopleWheres.Add(x => x.Name.Contains(queryDTO.Name));
            if (queryDTO.Photo.HasValue)
            {
                if (queryDTO.Photo == 1)
                {
                    oPeopleWheres.Add(x => x.PhotoLen > 0);
                }
                else
                {
                    oPeopleWheres.Add(x => x.PhotoLen == 0);
                }
            }
            if (queryDTO.Password.HasValue)
            {
                if (queryDTO.Password == 1)
                {
                    oPeopleWheres.Add(x => x.Password == string.Empty);
                }
                else
                {
                    oPeopleWheres.Add(x => x.Password != string.Empty);
                }
            }
            if (queryDTO.CardNum.HasValue) oPeopleWheres.Add(x => x.CardNum == queryDTO.CardNum);
            if (queryDTO.QRCode.HasValue)
            {
                if (queryDTO.QRCode == 1)
                {
                    oPeopleWheres.Add(x => x.QRCode == string.Empty);
                }
                else
                {
                    oPeopleWheres.Add(x => x.QRCode != string.Empty);
                }
            }

            if (queryDTO.Face.HasValue)
            {
                if (queryDTO.Face == 1)
                {
                    oPeopleWheres.Add(x => x.FaceNum > 0);
                }
                else
                {
                    oPeopleWheres.Add(x => x.FaceNum == 0);
                }
            }

            if (queryDTO.Palmveins.HasValue)
            {
                if (queryDTO.Palmveins == 1)
                {
                    oPeopleWheres.Add(x => x.PalmveinsNum > 0);
                }
                else
                {
                    oPeopleWheres.Add(x => x.PalmveinsNum == 0);
                }
            }

            if (queryDTO.Fingerprints.HasValue)
            {
                if (queryDTO.Fingerprints == 1)
                {
                    oPeopleWheres.Add(x => x.FingerprintsNum > 0);
                }
                else
                {
                    oPeopleWheres.Add(x => x.FingerprintsNum == 0);
                }
            }
            #endregion

            dbPeople = MergeExpression(Context.Set<People>(), oPeopleWheres);
            dbDevice = MergeExpression(Context.Set<DeviceDetail>(), oDeviceWheres);
            dbAccess = MergeExpression(Context.Set<PeopleAccessDetail>(), oAccessWheres);
        }

        public PageResult<DeviceAccessQueryResultDTO> Query(DeviceAccessQueryDTO queryDTO)
        {
            CreateQueryDBContext(queryDTO, out var dbPeople, out var dbDevice, out var dbAccess);


            var query = from access in dbAccess
                        join device in dbDevice on access.DeviceID equals device.ID
                        join people in dbPeople on access.PeopleID equals people.ID

                        select new DeviceAccessQueryResultDTO()
                        {
                            AccessID = access.AccessID,
                            PeopleID = access.PeopleID,
                            DeviceID = access.DeviceID,

                            SN = device.SN,
                            Protocol = device.Protocol,
                            DeviceName = device.Name,


                            UserID = access.UserID,
                            Name = people.Name,
                            Photo = people.Photo,
                            PhotoLen = people.PhotoLen,
                            CardNum = people.CardNum,
                            QRCode = people.QRCode,
                            FaceNum = people.FaceNum,
                            PalmveinsNum = people.PalmveinsNum,
                            FingerprintsNum = people.FingerprintsNum,


                            AccessType = access.AccessType,
                            OpenTimes = access.OpenTimes,
                            ExpirationDate = access.ExpirationDate,
                            Timegroup = access.Timegroup,

                            UploadTime = access.UploadTime,
                            UploadStatus = access.UploadStatus,
                            UploadResult = access.UploadResult,
                            RepeatID = access.RepeatID,
                            UploadResultMsg = access.UploadResultMsg,
                        };

            if (queryDTO.IsAsc)
            {
                query = query.OrderBy(x => x.AccessID);
            }
            else
            {
                query = query.OrderByDescending(x => x.AccessID);
            }
            var result = new PageResult<DeviceAccessQueryResultDTO>()
            {
                DataList = query.Skip((queryDTO.PageIndex - 1) * queryDTO.PageSize).Take(queryDTO.PageSize).ToList(),
                PageIndex = queryDTO.PageIndex,
                PageSize = queryDTO.PageSize,
                TotalCount = query.Count()
            };
            return result;
        }

        public PeopleAccessDetail GetAccessDetail(int iAccessID)
        {
            var Accessdb = Context.Set<PeopleAccessDetail>().Where(x => x.AccessID == iAccessID);
            return Accessdb.FirstOrDefault();
        }


        public async Task AddAccess(DeviceAccessAddDTO addDto)
        {
            var db = Context.Set<PeopleAccessDetail>();
            //查询人员信息
            var peopleMap = _Cache.GetPeopleDictionary();
            var peoples = new List<People>(peopleMap.Count);
            foreach (var pid in addDto.PeopleIDs)
            {
                if (peopleMap.ContainsKey(pid))
                {
                    peoples.Add(peopleMap[pid]);
                }
            }

            //查询门信息
            var deviceMap = _Cache.GetDeviceDictionary();
            var devices = new List<CacheDeviceDTO>(deviceMap.Count);
            foreach (var did in addDto.DeviceIDs)
            {
                if (deviceMap.ContainsKey(did))
                {
                    devices.Add(deviceMap[did]);
                }
            }
            var now = DateTime.Now;
            //创建新的集合表
            var peopleAccessTableQuery = from device in devices
                                         from people in peoples
                                         select new PeopleAccessDetail()
                                         {
                                             DeviceID = device.ID,

                                             PeopleID = people.ID,
                                             UserID = people.UserID,

                                             AccessType = addDto.AccessType,
                                             ExpirationDate = addDto.ExpirationDate,
                                             OpenTimes = addDto.OpenTimes,
                                             KeepOpen = addDto.KeepOpen,
                                             Timegroup = addDto.Timegroup,
                                             Holidays = addDto.Holidays,
                                             Elevators = addDto.Elevators,

                                             LastUpdatetime = now,
                                             UploadTime = now,
                                             UploadStatus = 0,
                                             UploadResult = 0,
                                             RepeatID = 0,
                                             UploadResultMsg = string.Empty,
                                         };
            var peopleAccessMap = peopleAccessTableQuery.ToDictionary(k => $"p{k.PeopleID}_d{k.DeviceID}");

            //从数据库中查询已存在的记录
            var dbAccessList = await db
                .Where(a => addDto.PeopleIDs.Contains(a.PeopleID) && addDto.DeviceIDs.Contains(a.DeviceID))
                .Select(a => new
                {
                    a.AccessID,
                    a.CreateTime,
                    a.UploadStatus,
                    a.PeopleID,
                    a.DeviceID
                }).ToListAsync();
            foreach (var item in dbAccessList)
            {
                string sKey = $"p{item.PeopleID}_d{item.DeviceID}";
                if (peopleAccessMap.ContainsKey(sKey))
                {
                    var accessItem = peopleAccessMap[sKey];
                    accessItem.AccessID = item.AccessID;
                    accessItem.CreateTime = item.CreateTime;
                }
            }
            //更新数据库
            var updateIds = peopleAccessMap.Where(x => x.Value.AccessID > 0).Select(x => x.Value.AccessID).ToHashSet();
            if (updateIds.Count > 0)
            {
                await db.Where(x => updateIds.Contains(x.AccessID)).ExecuteUpdateAsync(s => s
                .SetProperty(f => f.AccessType, v => addDto.AccessType)
                .SetProperty(f => f.ExpirationDate, v => addDto.ExpirationDate)
                .SetProperty(f => f.OpenTimes, v => addDto.OpenTimes)
                .SetProperty(f => f.KeepOpen, v => addDto.KeepOpen)
                .SetProperty(f => f.Timegroup, v => addDto.Timegroup)
                .SetProperty(f => f.Holidays, v => addDto.Holidays)
                .SetProperty(f => f.Elevators, v => addDto.Elevators)
                .SetProperty(f => f.LastUpdatetime, v => now)
                .SetProperty(f => f.UploadStatus, v => 0)
                .SetProperty(f => f.UploadResult, v => 0)
            );
            }


            //筛选出待更新的，并添加到数据库
            var iInserList = peopleAccessMap.Where(x => x.Value.AccessID == 0).Select(x => x.Value);
            if (iInserList.Count() > 0)
            {
                await AddRangeAsync(iInserList);
            }



            //保存日志
            {


                var sbuf = new StringBuilder(devices.Count * 50);
                foreach (var device in devices)
                {
                    sbuf.AppendFormat("{0}({1})", device.DeviceName, device.SN).Append(",");
                }
                sbuf.Length -= 1;
                var sDoorInfo = sbuf.ToString();
                sbuf.Clear();


                string sLogTitle = _LanguageHandler.GetUserLog("t1");//"权限管理",
                string sAddLogFromat = _LanguageHandler.GetUserLog("r2");//批量修改开门权限，人员：{0}({1})，有效期：{2},有效次数：{3}，门:{4}
                string sDeleteLogFromat = _LanguageHandler.GetUserLog("r5");//批量删除开门权限，人员：{0}({1})，门:{2}
                string strdevicePassEnd = addDto.ExpirationDate.ToDateTimeStr();


                foreach (var people in peoples)
                {
                    AddUserLog(sLogTitle, //"权限管理",
                            string.Format(sAddLogFromat, people.Name, people.UserID,
                            strdevicePassEnd, addDto.ExpirationDate, sDoorInfo),
                            sDoorInfo, $"{people.Name}({people.UserID})");// "修改人员开门权限"

                }
                await CommitAsync();
            }

            await UpdateDeviceAccessTotal(addDto.DeviceIDs);
        }


        public Task AddAccess_ALLPeople(DeviceAccessAddDTO addDto)
        {
            addDto.PeopleIDs = _Cache.GetPeopleDictionary().Keys.ToList();
            return AddAccess(addDto);
        }

        public Task DeleteAccess_ALLPeople(DeviceAccessDeleteDTO addDto)
        {
            addDto.PeopleIDs = _Cache.GetPeopleDictionary().Keys.ToList();
            return DeleteAccess(addDto);
        }
        /// <summary>
        /// 清空当前设备列表有关所有人员权限
        /// </summary>
        /// <param name="addDto"></param>
        /// <returns></returns>
        public async Task ClearAllPeople(DeviceAccessDeleteDTO dto)
        {
            var db = Context.Set<PeopleAccessDetail>();

            await db.Where(x => dto.DeviceIDs.Contains(x.DeviceID)).ExecuteDeleteAsync(); //直接删除

            Commit();
        }

        public async Task DeleteAccess(DeviceAccessDeleteDTO dto)
        {
            var db = Context.Set<PeopleAccessDetail>();
            //查询人员信息
            var peopleMap = _Cache.GetPeopleDictionary();
            var peoples = new List<People>(peopleMap.Count);
            foreach (var pid in dto.PeopleIDs)
            {
                if (peopleMap.ContainsKey(pid))
                {
                    peoples.Add(peopleMap[pid]);
                }
            }

            //查询门信息
            var deviceMap = _Cache.GetDeviceDictionary();
            var devices = new List<CacheDeviceDTO>(deviceMap.Count);
            foreach (var did in dto.DeviceIDs)
            {
                if (deviceMap.ContainsKey(did))
                {
                    devices.Add(deviceMap[did]);
                }
            }
            var now = DateTime.Now;


            //从数据库中查询已存在的记录
            var dbAccessList = await db
                .Where(a => dto.PeopleIDs.Contains(a.PeopleID) && dto.DeviceIDs.Contains(a.DeviceID) && a.UploadStatus != 2)
                .Select(a => new
                {
                    a.AccessID,
                    a.UploadStatus
                }).ToListAsync();

            var updateList = dbAccessList.Where(x => x.UploadStatus == 1).Select(x => x.AccessID).ToHashSet();

            //更新数据库
            if (updateList.Count > 0)
            {
                await db.Where(x => updateList.Contains(x.AccessID)).ExecuteUpdateAsync(s => s
                    .SetProperty(f => f.LastUpdatetime, v => now)
                    .SetProperty(f => f.UploadStatus, v => 2)
                    .SetProperty(f => f.UploadResult, v => 0)
                );
            }


            //筛选出需要删除的，从数据库删除
            var iDeleteList = dbAccessList.Where(x => x.UploadStatus == 0).Select(x => x.AccessID).ToHashSet();
            if (iDeleteList.Count > 0)
            {
                await db.Where(x => updateList.Contains(x.AccessID)).ExecuteDeleteAsync();
            }



            //保存日志
            {


                var sbuf = new StringBuilder(devices.Count * 50);
                foreach (var device in devices)
                {
                    sbuf.AppendFormat("{0}({1})", device.DeviceName, device.SN).Append(",");
                }
                sbuf.Length -= 1;
                var sDoorInfo = sbuf.ToString();
                sbuf.Clear();


                string sLogTitle = _LanguageHandler.GetUserLog("t1");//"权限管理",
                string sDeleteLogFromat = _LanguageHandler.GetUserLog("r5");//批量删除开门权限，人员：{0}({1})，门:{2}

                foreach (var people in peoples)
                {
                    AddUserLog(sLogTitle, //"权限管理",
                            string.Format(sDeleteLogFromat, people.Name, people.UserID, sDoorInfo),
                            sDoorInfo, $"{people.Name}({people.UserID})");// "修改人员开门权限"

                }
                Commit();
            }


            //更新缓存
            //查询出所有权限

            await UpdateDeviceAccessTotal(dto.DeviceIDs);

        }


        public async Task Delete(List<int> iAccessIDs)
        {
            HashSet<int> hsAccessID = new HashSet<int>(iAccessIDs);
            var peoples = _Cache.GetPeopleDictionary();
            var devices = _Cache.GetDeviceDictionary();

            CacheDeviceDTO device;
            People people;

            var hsDeviceID = new HashSet<int>(iAccessIDs.Count);
            var Accessdb = Set<PeopleAccessDetail>();
            var oldAccess = Accessdb.Where(x => hsAccessID.Contains(x.AccessID) && x.UploadStatus != 2).Select(x => new PeopleAccessDetail()
            {
                AccessID = x.AccessID,
                DeviceID = x.DeviceID,
                PeopleID = x.PeopleID,
                UploadStatus = x.UploadStatus
            });
            if (oldAccess == null)
                return;

            var bUpdate = false;
            var iUpdateCount = 0;
            var sLogTitle = _LanguageHandler.GetUserLog("t1"); //"权限管理", 
            var sLogFormat = _LanguageHandler.GetUserLog("r4");//删除开门权限，人员：{0}({1})，门:{2}{3}
            string sDoorName, sSN, sPName;
            long lUserID;
            var hsDeleteAccessID = new HashSet<int>(iAccessIDs.Count);
            var hsUpdateAccessID = new HashSet<int>(iAccessIDs.Count);

            foreach (var item in oldAccess)
            {
                hsDeviceID.Add(item.DeviceID);
                if (devices.ContainsKey(item.DeviceID))
                {
                    device = devices[item.DeviceID];
                    sDoorName = device.DeviceName;
                    sSN = device.SN;
                }
                else
                {
                    sDoorName = string.Empty;
                    sSN = string.Empty;
                }

                if (peoples.ContainsKey(item.PeopleID))
                {
                    people = peoples[item.PeopleID];
                    sPName = people.Name;
                    lUserID = people.UserID;
                }
                else
                {
                    sPName = string.Empty;
                    lUserID = 0;
                }
                //bUpdate = false;

                //if (item.UploadStatus == 0)
                //{
                //    bUpdate = true;
                //    hsDeleteAccessID.Add(item.AccessID);
                //}

                //if (item.UploadStatus == 1)
                //{
                bUpdate = true;
                hsUpdateAccessID.Add(item.AccessID);
                //}

                if (bUpdate)
                {   //更新
                    iUpdateCount++;

                    AddUserLog(sLogTitle, //"权限管理", 
                        String.Format(sLogFormat, sPName, lUserID, sDoorName, sSN),
                        $"{sDoorName}({sSN})", $"{sPName}({lUserID})");//"删除开门权限，人员：{0}({1})，门:{2}{3}
                }
            }
            var now = DateTime.Now;

            //先更新数据库
            if (hsUpdateAccessID.Count > 0)
            {
                await Accessdb.Where(x => hsUpdateAccessID.Contains(x.AccessID)).ExecuteUpdateAsync(s => s
                .SetProperty(f => f.UploadStatus, v => 2)
                .SetProperty(f => f.UploadTime, v => now)
                );
            }
            //if (hsDeleteAccessID.Count > 0)
            //{
            //    await Accessdb.Where(x => hsDeleteAccessID.Contains(x.AccessID)).ExecuteDeleteAsync();
            //}


            await CommitAsync();
            if (iUpdateCount > 0)
            {
                //更新缓存
                //查询出所有权限

                await UpdateDeviceAccessTotal(hsDeviceID);
            }
            await CommitAsync();
        }


        public async void Update(PeopleAccessDetail detail)
        {
            var Accessdb = Set<PeopleAccessDetail>();
            var oldAccess = Accessdb.Where(x => x.AccessID == detail.AccessID).FirstOrDefault();
            if (oldAccess == null)
                return;
            var peoples = _Cache.GetPeopleDictionary();
            var devices = _Cache.GetDeviceDictionary();

            CacheDeviceDTO device;
            People people;

            if (!devices.ContainsKey(oldAccess.DeviceID))
                return;
            if (!peoples.ContainsKey(oldAccess.PeopleID))
                return;
            device = devices[oldAccess.DeviceID];
            people = peoples[oldAccess.PeopleID];

            //更新
            oldAccess.AccessType = detail.AccessType;
            oldAccess.ExpirationDate = detail.ExpirationDate;
            oldAccess.OpenTimes = detail.OpenTimes;
            oldAccess.KeepOpen = detail.KeepOpen;
            oldAccess.Timegroup = detail.Timegroup;
            oldAccess.Holidays = detail.Holidays;
            oldAccess.Elevators = detail.Elevators;
            oldAccess.LastUpdatetime = DateTime.Now;
            oldAccess.UploadStatus = 0;


            string sLog = _LanguageHandler.GetUserLog("r6");//修改开门权限,人员：{0}({1})，门:{2}{3}，有效期：{4},有效次数：{5}
            AddUserLog(_LanguageHandler.GetUserLog("t1"), //"权限管理", 
                string.Format(sLog, people.Name, people.UserID,
                device.DeviceName, device.SN,
                detail.ExpirationDate.ToDateTimeStr(), detail.OpenTimes),
                $"{device.DeviceName}({device.SN})", $"{people.Name}({people.UserID})");//修改员开门权限,人员：{0}({1})，门:{2}{3}


            await CommitAsync();


            //更新缓存
            //查询出所有权限

            var hsDeviceID = new HashSet<int>();
            hsDeviceID.Add(oldAccess.DeviceID);

            await UpdateDeviceAccessTotal(hsDeviceID);

        }


        public async Task ClearAccess()
        {
            var Accessdb = Set<PeopleAccessDetail>();
            await Accessdb.ExecuteDeleteAsync();
            AddUserLog(_LanguageHandler.GetUserLog("t1"), //"权限管理", 
                _LanguageHandler.GetUserLog("r3"));//"清空所有开门权限"
            await CommitAsync();

            var query = _Cache.GetDevices();
            foreach (var sn in query)
            {
                _Cache.UpdateDeviceCache(sn, x =>
                {
                    x.NewAccessTotal = 0;
                    x.DeleteAccessTotal = 0;
                    x.AccessTotal = 0;
                    x.EmptyPeople = 1;
                });
            }

        }


        public async Task UpdateDeviceAccessTotal(IEnumerable<int> hsDoorID)
        {
            var AccessTotalQuery = await (from dAccess in Set<PeopleAccessDetail>()
                                          where hsDoorID.Contains(dAccess.DeviceID)
                                          orderby dAccess.DeviceID
                                          group dAccess by dAccess.DeviceID into groupedItems
                                          select new
                                          {
                                              DoorID = groupedItems.Key, //0--未上传；1--已上传；2--待删除
                                              NewTotal = groupedItems.Where(t => t.UploadStatus == 0).Count(),
                                              UploadTotal = groupedItems.Where(t => t.UploadStatus == 1).Count(),
                                              DelTotal = groupedItems.Where(t => t.UploadStatus == 2).Count()
                                          }
            ).ToDictionaryAsync(r => r.DoorID);




            //获取缓存的所有门
            var allDevice = _Cache.GetDeviceDictionary();

            foreach (int doorID in hsDoorID)
            {
                var door = allDevice[doorID];


                //0--未上传；1--已上传；2--待删除
                if (AccessTotalQuery.ContainsKey(doorID))
                {
                    var Status = AccessTotalQuery[doorID];

                    door.DeleteAccessTotal = Status.DelTotal;
                    door.NewAccessTotal = Status.NewTotal;
                    door.AccessTotal = Status.NewTotal + Status.UploadTotal;
                }
                else
                {
                    door.DeleteAccessTotal = 0;
                    door.NewAccessTotal = 0;
                    door.AccessTotal = 0;
                }

            }
            allDevice = null;
            AccessTotalQuery = null;

        }




        public async Task Reupload(List<int> accessIDs)
        {
            var now = DateTime.Now;
            var Accessdb = Set<PeopleAccessDetail>();
            await Accessdb.Where(x => accessIDs.Contains(x.AccessID)).ExecuteUpdateAsync(s => s
                .SetProperty(f => f.UploadStatus, v => 0)
                .SetProperty(f => f.UploadResult, v => 0)
                .SetProperty(f => f.RepeatID, v => 0)
                .SetProperty(f => f.UploadResultMsg, v => string.Empty)
                .SetProperty(f => f.LastUpdatetime, v => now)
            );


            var doors = Accessdb.Where(x => accessIDs.Contains(x.AccessID))
                .Select(d => new
                {
                    d.DeviceID,
                    d.UserID
                });
            var deviceDic = _Cache.GetDeviceDictionary();
            string sLogTitle = _LanguageHandler.GetUserLog("t1");//"权限管理",
            string sLogFormat = _LanguageHandler.GetUserLog("r15"); //批量重新上传人员开门权限， 人员：{0}({1})，门:{2}{3}
            foreach (var item in doors)
            {
                var device = deviceDic[item.DeviceID];

                var people = _Cache.GetPeopleCache(item.UserID);
                if (people == null)
                {
                    AddUserLog(sLogTitle, //"权限管理",
                  string.Format(sLogFormat, string.Empty, item.UserID,
                  device.DeviceName, device.SN),
                  $"{device.DeviceName}({device.SN})", $"{item.UserID}");// "批量重新上传人员开门权限"
                }
                else
                {
                    AddUserLog(sLogTitle, //"权限管理",
                  string.Format(sLogFormat, people.Name, people.UserID,
                  device.DeviceName, device.SN),
                  $"{device.DeviceName}({device.SN})", $"{people.Name}({people.UserID})");// "批量重新上传人员开门权限"
                }
            }

            await CommitAsync();
            await UpdateDeviceAccessTotal(doors.Select(x => x.DeviceID).Distinct());
        }

        public async Task ReuploadAll()
        {
            var now = DateTime.Now;
            var Accessdb = Set<PeopleAccessDetail>();
            await Accessdb.ExecuteUpdateAsync(s => s
                .SetProperty(f => f.UploadStatus, v => 0)
                .SetProperty(f => f.UploadResult, v => 0)
                .SetProperty(f => f.RepeatID, v => 0)
                .SetProperty(f => f.UploadResultMsg, v => string.Empty)
                .SetProperty(f => f.LastUpdatetime, v => now)
            );

            //更新缓存
            var doors = _Cache.GetDeviceDictionary().Keys;

            AddUserLog(_LanguageHandler.GetUserLog("t1"), //"权限管理",
                     _LanguageHandler.GetUserLog("r16"));// "重新上传所有开门权限"
            await CommitAsync();

            await UpdateDeviceAccessTotal(doors);
        }

        public async Task ReuploadByDevice(int deviceID)
        {
            var accessDB = Set<PeopleAccessDetail>();

            accessDB.Where(x => x.DeviceID == deviceID && x.UploadStatus == 1)
                .ExecuteUpdate(u =>
                    u.SetProperty(f => f.UploadStatus, v => 0)
                    .SetProperty(f => f.LastUpdatetime, v => DateTime.Now)
                );
            accessDB.Where(x => x.DeviceID == deviceID && x.UploadStatus == 2)
                .ExecuteDelete();

            //更新到缓存
            await UpdateDeviceAccessTotal([deviceID]);
        }


        public async Task<List<PeopleAccessDetail>> GetDownloadAccess(int doorID, int iLimit)
        {
            var deviceMap = _Cache.GetDeviceDictionary();
            if (!deviceMap.ContainsKey(doorID)) return null;
            var device = _Cache.GetDeviceDictionary()[doorID];
            var SN = device.SN;

            var Accessdb = Context.Set<PeopleAccessDetail>();

            var lst = Accessdb.Where(x => x.DeviceID == doorID && x.UploadStatus == 0);
            lst = lst.AsNoTracking().OrderBy(x => x.UserID).Take(iLimit);




            var accessList = await lst.ToListAsync();
            var peopleMap = _Cache.GetPeopleDictionary();



            if (accessList.Count > 0)
            {
                var sLogFormat = _LanguageHandler.GetUserLog("r8");//设备:{0}({1}) 拉取人员:{2}({3})
                var sLogTitle = _LanguageHandler.GetUserLog("t1"); //"权限管理", 


                foreach (var item in accessList)
                {
                    People p = null;
                    if (peopleMap.ContainsKey(item.PeopleID))
                    {
                        p = peopleMap[item.PeopleID];
                        AddUserLog(sLogTitle, //"权限管理", 
                          string.Format(sLogFormat, device.DeviceName, SN, p.Name, item.UserID),
                          $"{device.DeviceName}({SN})", $"{p.Name}({item.UserID})");
                    }
                    else
                    {
                        AddUserLog(sLogTitle, //"权限管理", 
                          string.Format(sLogFormat, device.DeviceName, SN, string.Empty, item.UserID),
                          $"{device.DeviceName}({SN})", $"{item.UserID}");
                    }
                }
                await CommitAsync();
                _Cache.SavePeopleAccessList(SN, accessList.ToDictionary(x => x.UserID, v => v.AccessID));
            }
            else
            {
                //没有权限
                if (device.NewAccessTotal > 0)
                {
                    //缓存有人，但是实际查不到，需要刷新缓存
                    await UpdateDeviceAccessTotal(new int[] { device.ID });
                }

            }


            return accessList;
        }

        public async Task UpdatePeopleAccessUploadResult(int doorID, List<DeviceAccessUploadStatusUpdateDTO> updateAccessList)
        {
            var deviceMap = _Cache.GetDeviceDictionary();
            if (!deviceMap.ContainsKey(doorID)) return;
            var device = _Cache.GetDeviceDictionary()[doorID];
            var SN = device.SN;

            var UpdateIDList = updateAccessList.Where(x => x.UploadResult == 1).Select(x => x.AccessID);
            var Accessdb = Context.Set<PeopleAccessDetail>();
            var now = DateTime.Now;

            //更新人员权限信息
            await Accessdb.Where(x => UpdateIDList.Contains(x.AccessID)).ExecuteUpdateAsync(s => s
                .SetProperty(f => f.UploadStatus, v => 1)
                .SetProperty(f => f.UploadTime, v => now)
                .SetProperty(f => f.UploadResult, v => 1)
                .SetProperty(f => f.RepeatID, v => 0)
                .SetProperty(f => f.UploadResultMsg, v => string.Empty)
            );

            var saveErrList = updateAccessList.Where(x => x.UploadResult > 1);
            foreach (var item in saveErrList)
            {
                await Accessdb.Where(x => x.AccessID == item.AccessID).ExecuteUpdateAsync(s => s
                    .SetProperty(f => f.UploadStatus, v => 1)
                    .SetProperty(f => f.UploadTime, v => now)
                    .SetProperty(f => f.UploadResult, v => item.UploadResult)
                    .SetProperty(f => f.RepeatID, v => item.RepeatID)
                    .SetProperty(f => f.UploadResultMsg, v => item.UploadResultMsg)
                );
            }

            //更新全新统计数
            await UpdateDeviceAccessTotal(new int[] { doorID });

            //清空已拉取人员缓存
            _Cache.DeletePeopleAccessList(SN);
        }


        public async Task<Dictionary<int, long>> GetDeleteAccess(int doorID, int limit)
        {
            var deviceMap = _Cache.GetDeviceDictionary();
            if (!deviceMap.ContainsKey(doorID)) return null;
            var device = _Cache.GetDeviceDictionary()[doorID];
            var SN = device.SN;


            var Accessdb = Context.Set<PeopleAccessDetail>();

            var lst = Accessdb.Where(x => x.DeviceID == doorID && x.UploadStatus == 2)
                        .Select(x => new
                        {
                            x.AccessID,
                            x.UserID
                        });

            lst = lst.OrderBy(x => x.UserID).Take(limit);

            var peoples = await lst.ToListAsync();

            People peopleDTO;
            string Name;
            var sLogFormat = _LanguageHandler.GetUserLog("r7");//设备:{0}({1}) 拉取待删除人员:{2}({3})
            var sLogTitle = _LanguageHandler.GetUserLog("t1"); //"权限管理", 
            foreach (var item in peoples)
            {
                Name = string.Empty;
                peopleDTO = _Cache.GetPeopleCache(item.UserID);
                if (peopleDTO != null) Name = peopleDTO.Name;
                AddUserLog(sLogTitle, //"权限管理", 
                   string.Format(sLogFormat, device.DeviceName, SN, Name, item.UserID),
                   $"{device.DeviceName}({SN})", $"{Name}({item.UserID})");
            }
            await CommitAsync();

            _Cache.SaveDeletePeopleAccessList(SN, peoples.ToDictionary(x => x.UserID, x => x.AccessID));


            return peoples.ToDictionary(x => x.AccessID, x => x.UserID);
        }

        public async Task SaveDeleteAccessResult(int doorID, List<int> accessList)
        {
            var deviceMap = _Cache.GetDeviceDictionary();
            if (!deviceMap.ContainsKey(doorID)) return;
            var device = _Cache.GetDeviceDictionary()[doorID];
            var SN = device.SN;

            var Accessdb = Context.Set<PeopleAccessDetail>();
            await Accessdb.Where(x => accessList.Contains(x.AccessID) && x.UploadStatus == 2).ExecuteDeleteAsync();
            _Cache.DeleteDeletePeopleAccessList(SN);

            //更新全新统计数
            await UpdateDeviceAccessTotal(new int[] { doorID });

        }

        /// <summary>
        /// 将指定查询条件的记录重新上传
        /// </summary>
        /// <param name="par"></param>
        /// <returns></returns>
        public async Task ReuploadFilterAllAsync(DeviceAccessQueryDTO queryDTO)
        {
            var now = DateTime.Now;

            CreateQueryDBContext(queryDTO, out var dbPeople, out var dbDevice, out var dbAccess);
            var query = from access in dbAccess
                        join device in dbDevice on access.DeviceID equals device.ID
                        join people in dbPeople on access.PeopleID equals people.ID
                        select new DeviceAccessQueryAccessIDResultDTO()
                        {
                            AccessID = access.AccessID,
                            DeviceID = access.DeviceID,
                            UserID = access.UserID
                        };
            var accesss = await query.ToListAsync();


            var Accessdb = Set<PeopleAccessDetail>();

            var batchSize = 1000;
            var batches = accesss.Chunk(batchSize);
            foreach (var batch in batches)
            {
                var accessIds = accesss.Select(x => x.AccessID);
                await Accessdb.Where(x => accessIds.Contains(x.AccessID) && x.UploadStatus != 2).ExecuteUpdateAsync(s => s
                    .SetProperty(f => f.UploadStatus, v => 0)
                    .SetProperty(f => f.UploadResult, v => 0)
                    .SetProperty(f => f.RepeatID, v => 0)
                    .SetProperty(f => f.UploadResultMsg, v => string.Empty)
                    .SetProperty(f => f.LastUpdatetime, v => now)
                );
            }


            var doors = accesss.Select(x => x.DeviceID).Distinct();

            await CommitAsync();
            await UpdateDeviceAccessTotal(doors);
        }
    }
}
