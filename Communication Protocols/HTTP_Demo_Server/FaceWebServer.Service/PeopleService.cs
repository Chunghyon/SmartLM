using FaceWebServer.DB.Table;
using FaceWebServer.DTO.Access;
using FaceWebServer.DTO.Device;
using FaceWebServer.DTO.HTTPv2_Protocol;
using FaceWebServer.DTO.People;
using FaceWebServer.DTO.Remote;
using FaceWebServer.Interface;
using FaceWebServer.Language;
using FaceWebServer.Utility;
using FaceWebServer.Utility.Extend;
using FaceWebServer.Utility.Model;
using Mapster;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace FaceWebServer.Service
{
    public class PeopleService : BaseService, IPeopleService
    {
        public IDeviceAccessService _AccessDB { get; set; }
        public ICacheService _Cache { get; set; }
        private readonly ILogger<PeopleService> _logger;
        private LanguageHandler _LanguageHandler;
        private IServiceProvider serviceProvider;
        public PeopleService(DbContext context,
            IOptionsSnapshot<LanguageOption> lngopt,
            IServiceProvider serviceProvider,
            ILogger<PeopleService> log) : base(context)
        {
            _LanguageHandler = lngopt.Value.GetCurrentLanguageHandler();
            this.serviceProvider = serviceProvider;
            _logger = log;
        }

        public PageResult<PeopleQueryResultDTO> Query(PeopleQueryDTO queryDTO)
        {
            List<Expression<Func<People, bool>>> oWheres = new();
            #region 拼接条件
            if (queryDTO.ID.HasValue) oWheres.Add(x => x.ID == queryDTO.ID.Value);
            if (queryDTO.UserID.HasValue) oWheres.Add(x => x.UserID == queryDTO.UserID.Value);

            if (!string.IsNullOrWhiteSpace(queryDTO.Name)) oWheres.Add(x => x.Name.Contains(queryDTO.Name));
            if (!string.IsNullOrWhiteSpace(queryDTO.Job)) oWheres.Add(x => x.Job.Contains(queryDTO.Job));
            if (!string.IsNullOrWhiteSpace(queryDTO.Department)) oWheres.Add(x => x.Department.Contains(queryDTO.Department));
            if (!string.IsNullOrWhiteSpace(queryDTO.IdentityCard)) oWheres.Add(x => x.IdentityCard.Contains(queryDTO.IdentityCard));
            if (queryDTO.Photo.HasValue)
            {
                if (queryDTO.Photo.Value == 1)
                {
                    oWheres.Add(x => x.PhotoLen > 0);
                }
                else
                {
                    oWheres.Add(x => x.PhotoLen == 0);
                }
            }

            if (queryDTO.Password.HasValue)
            {
                if (queryDTO.Password.Value == 1)
                {
                    oWheres.Add(x => x.Password != string.Empty);
                }
                else
                {
                    oWheres.Add(x => x.Password == string.Empty);
                }
            }
            if (queryDTO.CardNum.HasValue) oWheres.Add(x => x.CardNum == queryDTO.CardNum.Value);
            if (!string.IsNullOrWhiteSpace(queryDTO.QRCode)) oWheres.Add(x => x.QRCode.Contains(queryDTO.QRCode));
            if (queryDTO.UseQRCode.HasValue)
            {
                if (queryDTO.UseQRCode.Value == 1)
                {
                    oWheres.Add(x => x.QRCode != string.Empty);
                }
                else
                {
                    oWheres.Add(x => x.QRCode == string.Empty);
                }
            }


            if (queryDTO.Face.HasValue)
            {
                if (queryDTO.Face.Value == 1)
                {
                    oWheres.Add(x => x.FaceNum > 0);
                }
                else
                {
                    oWheres.Add(x => x.FaceNum == 0);
                }
            }

            if (queryDTO.Palmveins.HasValue)
            {
                if (queryDTO.Palmveins.Value == 1)
                {
                    oWheres.Add(x => x.PalmveinsNum > 0);
                }
                else
                {
                    oWheres.Add(x => x.PalmveinsNum == 0);
                }
            }

            if (queryDTO.Fingerprints.HasValue)
            {
                if (queryDTO.Fingerprints.Value == 1)
                {
                    oWheres.Add(x => x.FingerprintsNum > 0);
                }
                else
                {
                    oWheres.Add(x => x.FingerprintsNum == 0);
                }
            }
            #endregion


            var devices = QueryPage(
            x => new PeopleQueryResultDTO()
            {
                ID = x.ID,
                UserID = x.UserID,
                Name = x.Name,
                Job = x.Job,
                Department = x.Department,
                IdentityCard = x.IdentityCard,
                Photo = x.Photo,
                PhotoLen = x.PhotoLen,
                Password = x.Password,
                CardNum = x.CardNum,
                QRCode = x.QRCode,
                FaceNum = x.FaceNum,
                PalmveinsNum = x.PalmveinsNum,
                FingerprintsNum = x.FingerprintsNum

            },
            oWheres, queryDTO.PageSize, queryDTO.PageIndex,
            x => x.UserID,
            queryDTO.IsAsc);

            return devices;
        }


        public long GetNewAutoUserID()
        {
            long iAutoUserID = 10000;

            iAutoUserID = Cache.Get<long>("AutoUserID");
            if (iAutoUserID == 0)
            {
                iAutoUserID = 10000;
                //需要自动分配一个ID；
                var sysdb = GetSystemKVDBSet();
                var kv = sysdb.Find("AutoUserID");
                if (kv == null)
                {

                    kv = new() { Key = "AutoUserID", Value = "10000" };
                    Insert<SystemKV>(kv);
                }
                else
                {

                    iAutoUserID = long.Parse(kv.Value);
                }
            }

            long iNewUserID = iAutoUserID;

            var EmplIDs = _Cache.GetPeopleUserIDs();
            do
            {
                if (!EmplIDs.Contains(iNewUserID))
                {
                    break;
                }
                iNewUserID++;
            } while (true);

            if (iNewUserID != iAutoUserID)
                Cache.Set("AutoUserID", iNewUserID);
            //kv.LastUpdateTime = DateTime.Now;
            //kv.Value = iAutoUserID.ToString();
            //Update(kv);
            return iNewUserID;

        }

        public void UpdateAutoUserID(long iNewUserID)
        {
            var sysdb = GetSystemKVDBSet();
            var kv = sysdb.Find("AutoUserID");
            if (kv == null)
            {

                kv = new() { Key = "AutoUserID", Value = iNewUserID.ToString() };
                Insert<SystemKV>(kv);
            }
            else
            {

                kv.Value = iNewUserID.ToString();
            }
            Commit();
            Cache.Set("AutoUserID", iNewUserID);
        }

        public async Task InputPeople(List<People> peoples)
        {
            var peopleUserIDMap = _Cache.GetPeopleDictionary().ToDictionary(x => x.Value.UserID, x => x.Value);
            var peopleUpdateList = new List<People>();
            var peopleNewList = new List<People>();

            foreach (var people in peoples)
            {
                if (people.UserID == 0)
                {
                    people.UserID = GetNewAutoUserID();
                }

                if (peopleUserIDMap.ContainsKey(people.UserID))
                {
                    //需要修改
                    people.ID = peopleUserIDMap[people.UserID].ID;
                    peopleUpdateList.Add(people);
                }
                else
                {
                    people.CreateTime = DateTime.Now;
                    peopleNewList.Add(people);
                }
                people.LastUpdatetime = DateTime.Now;
            }

            if (peopleUpdateList.Count > 0)
            {
                await UpdateListAsync(peopleUpdateList);
            }

            if (peopleNewList.Count > 0)
            {
                await AddRangeAsync(peopleNewList);
            }


            //重建缓存
            _Cache.IniSystemCache();
        }

        public async Task<JsonResultModel> AddNew(People people, Func<People, JsonResultModel> imageCallblack)
        {
            people.CreateTime = DateTime.Now;
            people.LastUpdatetime = DateTime.Now;
            string sLog;
            #region 重复过滤
            Expression<Func<People, bool>> oWheres = null;
            if (people.UserID > 0) oWheres = x => x.UserID.Equals(people.UserID);
            if (people.CardNum.HasValue) if (people.CardNum.Value > 0) oWheres = oWheres.Or(x => x.CardNum.Equals(people.CardNum));
            if (!string.IsNullOrEmpty(people.QRCode)) oWheres = oWheres.Or(x => x.QRCode.Equals(people.QRCode));
            if (!string.IsNullOrEmpty(people.IdentityCard)) oWheres = oWheres.Or(x => x.IdentityCard.Equals(people.IdentityCard));

            var peopleQuery = Query(oWheres).AsNoTracking();
            var peoples = await peopleQuery.ToListAsync();


            if (peoples.Count() > 0)
            {
                foreach (var p in peoples)
                {
                    if (people.UserID > 0 && p.UserID == people.UserID)
                    {
                        return new JsonResultModel(200,
                            string.Format(_LanguageHandler.GetCheckParameterErrorMessage("r129"), p.Name));//$"人员编号重复，重复人员名称：{p.Name}");
                    }
                    if (people.CardNum > 0 && p.CardNum == people.CardNum)
                    {
                        //人员卡号重复，重复人员名称：{0}({1})
                        sLog = _LanguageHandler.GetCheckParameterErrorMessage("r130");//人员卡号重复，重复人员名称：{0}({1})
                        return new JsonResultModel(201,
                           string.Format(sLog, p.Name, p.UserID));
                    }

                    if (!string.IsNullOrEmpty(people.QRCode) && p.QRCode == people.QRCode)
                    {
                        //人员二维码重复，重复人员名称：{0}({1})
                        sLog = _LanguageHandler.GetCheckParameterErrorMessage("r173");
                        return new JsonResultModel(203,
                           string.Format(sLog, p.Name, p.UserID));
                    }

                    if (!string.IsNullOrEmpty(people.IdentityCard) && p.IdentityCard == people.IdentityCard)
                    {
                        //人员身份证重复，重复人员名称：{0}({1})
                        sLog = _LanguageHandler.GetCheckParameterErrorMessage("r174");
                        return new JsonResultModel(204,
                           string.Format(sLog, p.Name, p.UserID));
                    }
                }

            }
            #endregion


            if (people.UserID == 0)
            {
                people.UserID = GetNewAutoUserID();
            }

            if (imageCallblack != null)
            {
                var result = imageCallblack(people);
                if (result.Result != true)
                {
                    return result;
                }
            }

            //添加人员：{0}({1}),职务：{2},卡号：{3},照片地址：{4}
            sLog = _LanguageHandler.GetUserLog("r31");
            AddUserLog(_LanguageHandler.GetUserLog("t6"),// "人员管理",
                string.Format(sLog, people.Name, people.UserID, people.Job, people.CardNum, people.Photo),
                string.Empty, $"{people.Name}({people.UserID})");

            if (people.UserID >= GetNewAutoUserID())
            {
                UpdateAutoUserID(people.UserID + 1);
            }

            await InsertAsync(people);
            _Cache.AddPeopleCache(people);



            return new JsonResultModel();
        }


        public async Task<JsonResultModel> UpdatePeople(People newPeople, Func<People, JsonResultModel> imageCallblack, bool bUpdateAccess = true)
        {
            string sLog;

            //查询旧的人员信息
            var peopleMap = _Cache.GetPeopleDictionary();
            if (!peopleMap.ContainsKey(newPeople.ID))
            {
                return new JsonResultModel(205,
                _LanguageHandler.GetCheckParameterErrorMessage("r96"));//"人员不存在"
            }
            var old = peopleMap[newPeople.ID];

            newPeople.CreateTime = old.CreateTime;
            if (newPeople.UserID != old.UserID)
            {
                return new JsonResultModel(206,
                _LanguageHandler.GetCheckParameterErrorMessage("r175"));//"人员不存在"
            }


            #region 重复过滤

            if (newPeople.CardNum.HasValue || !string.IsNullOrEmpty(newPeople.QRCode) || !string.IsNullOrEmpty(newPeople.IdentityCard))
            {
                Expression<Func<People, bool>> oWheres = x => 1 == 1;
                if (newPeople.CardNum.HasValue) if (newPeople.CardNum > 0) oWheres = oWheres.Or(x => x.CardNum.Equals(newPeople.CardNum));
                if (!string.IsNullOrEmpty(newPeople.QRCode)) oWheres = oWheres.Or(x => x.QRCode.Equals(newPeople.QRCode));
                if (!string.IsNullOrEmpty(newPeople.IdentityCard)) oWheres = oWheres.Or(x => x.IdentityCard.Equals(newPeople.IdentityCard));

                var peopleQuery = Query<People>(oWheres).AsNoTracking();
                var peoples = await peopleQuery.ToListAsync();

                if (peoples.Count() > 0)
                {
                    foreach (var p in peoples)
                    {
                        if (newPeople.UserID > 0 && p.UserID == newPeople.UserID && newPeople.ID != p.ID)
                        {
                            return new JsonResultModel(200,
                                string.Format(_LanguageHandler.GetCheckParameterErrorMessage("r129"), p.Name));//$"人员编号重复，重复人员名称：{p.Name}");
                        }
                        if (newPeople.CardNum > 0 && p.CardNum == newPeople.CardNum && newPeople.ID != p.ID)
                        {
                            //人员卡号重复，重复人员名称：{0}({1})
                            sLog = _LanguageHandler.GetCheckParameterErrorMessage("r130");//人员卡号重复，重复人员名称：{0}({1})
                            return new JsonResultModel(201,
                               string.Format(sLog, p.Name, p.UserID));
                        }

                        if (!string.IsNullOrEmpty(newPeople.QRCode) && p.QRCode == newPeople.QRCode && newPeople.ID != p.ID)
                        {
                            //人员二维码重复，重复人员名称：{0}({1})
                            sLog = _LanguageHandler.GetCheckParameterErrorMessage("r173");
                            return new JsonResultModel(203,
                               string.Format(sLog, p.Name, p.UserID));
                        }

                        if (!string.IsNullOrEmpty(newPeople.IdentityCard) && p.IdentityCard == newPeople.IdentityCard && newPeople.ID != p.ID)
                        {
                            //人员身份证重复，重复人员名称：{0}({1})
                            sLog = _LanguageHandler.GetCheckParameterErrorMessage("r174");
                            return new JsonResultModel(204,
                               string.Format(sLog, p.Name, p.UserID));
                        }
                    }

                }
            }


            #endregion

            bool bCheckImage = true;
            if (old.PhotoLen > 0)
            {
                if (old.PhotoLen == newPeople.PhotoLen && old.PhotoMD5 == newPeople.PhotoMD5 && old.Photo == newPeople.Photo)
                {
                    bCheckImage = false;

                }

            }

            if (bCheckImage)
            {
                if (imageCallblack != null)
                {
                    var result = imageCallblack(newPeople);
                    if (result.Result != true)
                    {
                        return result;
                    }
                }
            }


            newPeople.LastUpdatetime = DateTime.Now;
            _Cache.UpdatePeopleCache(newPeople);

            await UpdateAsync(newPeople);

            //修改人员：{0}({1}),职务：{2},卡号：{3},照片地址：{4}
            sLog = _LanguageHandler.GetUserLog("r31");
            AddUserLog(_LanguageHandler.GetUserLog("t6"),// "人员管理",
                string.Format(sLog, newPeople.Name, newPeople.UserID, newPeople.Job, newPeople.CardNum, newPeople.Photo),
                string.Empty, $"{newPeople.Name}({newPeople.UserID})");

            await CommitAsync();
            if(bUpdateAccess)
            {
                var db = Context.Set<PeopleAccessDetail>();
                db.Where(p => p.PeopleID == newPeople.ID && p.UploadStatus == 1).ExecuteUpdate(s =>
                s.SetProperty(f => f.UploadResult, v => 0)
                .SetProperty(f => f.UploadStatus, v => 0)
                .SetProperty(f => f.UploadStatus, v => 0)
                .SetProperty(f => f.LastUpdatetime, v => DateTime.Now));

                await CommitAsync();

                //更新所有门的权限统计信息
                var doors = Context.Set<DeviceDetail>().Select(d => d.ID);
                await _AccessDB.UpdateDeviceAccessTotal(doors);
            }
           
            return new JsonResultModel();
        }



        public void ClearPeople()
        {

            //删除所有照片
            string sPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "People");
            if (Directory.Exists(sPath))
            {
                try
                {
                    var sfiles = Directory.GetFiles(sPath);

                    foreach (var f in sfiles)
                    {
                        File.Delete(f);
                    }
                }
                catch (Exception)
                {

                    //throw;
                }
            }
            Context.Set<PeopleAccessDetail>().ExecuteDelete();
            Context.Set<People>().ExecuteDelete();

            AddUserLog(_LanguageHandler.GetUserLog("t6"),// "人员管理",
               _LanguageHandler.GetUserLog("r34"));//删除所有人员

            UpdateAutoUserID(10000);

            var useridList = _Cache.GetPeopleUserIDs();
            foreach (var userid in useridList)
            {
                Cache.Remove(userid);
            }
            useridList.Clear();

            _Cache.GetPeopleDictionary().Clear();

            var sns = _Cache.GetDevices();
            foreach (var sn in sns)
            {
                _Cache.UpdateDeviceCache(sn, x =>
                {
                    x.AccessTotal = 0;
                    x.NewAccessTotal = 0;
                    x.DeleteAccessTotal = 0;
                    x.EmptyPeople = 0;
                });
            }
        }

        public async Task DeletePeople(HashSet<int> peopleIDLists)
        {
            var db = this.Context.Set<People>();
            var peoplesDB = db.Where(x => peopleIDLists.Contains(x.ID))
                .Select(x => new People
                {
                    ID = x.ID,
                    Name = x.Name,
                    UserID = x.UserID,
                    Job = x.Job,
                    CardNum = x.CardNum,
                    Photo = x.Photo
                }).ToList();
            if (peoplesDB.Count == 0) return;

            await Task.Run(async () =>
            {
                string sPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "People");
                if (!Directory.Exists(sPath))
                {
                    Directory.CreateDirectory(sPath);
                }
                string sLogTitle = _LanguageHandler.GetUserLog("t6");//人员管理
                string sLogFormat = _LanguageHandler.GetUserLog("r35");//删除人员: {0}({1}),职务：{2},卡号：{3},照片地址：{4}

                foreach (var p in peoplesDB)
                {
                    AddUserLog(sLogTitle,//"人员管理",
                   string.Format(sLogFormat, p.Name, p.UserID, p.Job, p.CardNum, p.Photo),
                    string.Empty, $"{p.Name}({p.UserID})");


                    if (!string.IsNullOrWhiteSpace(p.Photo))
                    {
                        var sFileName = GetImageFileName(p.Photo);
                        if (System.IO.File.Exists(sFileName))
                        {
                            try
                            {
                                System.IO.File.Delete(sFileName);
                            }
                            catch (Exception ex)
                            {

                                //_logger.LogError($" Delete 删除文件时发生错误：{sFileName}  {ex.Message} ");
                            }

                        }
                    }

                }


                //更新缓存
                var delUserIDList = peoplesDB.Select(p => p.UserID).ToList();
                _Cache.DeletePeopleCache(delUserIDList);

                //删除人员
                await db.Where(p => peopleIDLists.Contains(p.ID)).ExecuteDeleteAsync();
                //更新权限
                await Context.Set<PeopleAccessDetail>()
                .Where(a => peopleIDLists.Contains(a.PeopleID))
                .ExecuteUpdateAsync(s => s.SetProperty(f => f.UploadResult, v => 0)
                .SetProperty(f => f.UploadStatus, v => 2)
                .SetProperty(f => f.LastUpdatetime, v => DateTime.Now));

                await CommitAsync();

                var doors = Query<DeviceDetail>(null).AsNoTracking().Select(d => d.ID);
                await _AccessDB.UpdateDeviceAccessTotal(doors);
            });


        }

        /// <summary>
        /// 从URL 连接中获取文件名，去掉末尾的?crc
        /// </summary>
        /// <param name="sPath"></param>
        /// <param name="sFileName"></param>
        /// <returns></returns>
        private string GetImageFileName(string sURL)
        {
            string sPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");


            var iIndex = sURL.LastIndexOf("?");
            if (iIndex > 0)
            {
                sURL = sURL.Substring(0, iIndex);
            }
            string[] sURLSegments = sURL.Split("/");
            string sFile = sPath;
            foreach (var sSegment in sURLSegments)
            {
                sFile = Path.Combine(sFile, sSegment);
            }
            return sFile;
        }


        ///// <summary>
        ///// 添加特征数据
        ///// </summary>
        ///// <param name="addFeature"></param>
        ///// <returns></returns>
        //public JsonResultModel AddFeature(FeatureParameter addFeature)
        //{
        //    var people = Context.Find<People>(addFeature.PeopleId);
        //    if (people == null) return new JsonResultModel(101,
        //        _LanguageHandler.GetCheckParameterErrorMessage("r96"));//"人员不存在"
        //    var featureStr = addFeature.FeatureType == 1 ? people.Fingerprint : people.Palmvein; //判断是指纹还是掌静脉
        //    var featureList = JsonConvert.DeserializeObject<List<DetailInfo>>(featureStr); //将特征json信息序列化为对象
        //    var featureData = addFeature.FeatureData.FromBase64();//获取base64格式特征数据
        //    if (featureData == null)
        //    {
        //        return new JsonResultModel(101, "不是有效的base64格式");
        //    }
        //    var oldData = string.Empty;
        //    var feature = featureList.FirstOrDefault(a => a.Num == addFeature.Num); //判断索引号之前是否已经存在特征数据
        //    if (feature == null)
        //    {
        //        feature = new DetailInfo();//没有特征数据重新创建
        //        featureList.Add(feature);
        //    }
        //    else
        //    {
        //        oldData = feature.Data;//临时保存旧特征
        //    }

        //    feature.Data = SaveFeatureFile(addFeature.FeatureType, featureData);//保存新特征数据
        //    if (string.IsNullOrWhiteSpace(feature.Data))
        //    {
        //        return new JsonResultModel(101, "特征码数据保存失败");
        //    }
        //    feature.Num = addFeature.Num;
        //    feature.FileSize = featureData.Length;
        //    feature.DataMD5 = MD5Helper.GetMD5ByBase64(featureData);
        //    //     featureList.Add(feature);
        //    var data = JsonConvert.SerializeObject(featureList);
        //    if (addFeature.FeatureType == 1)
        //    {
        //        people.Fingerprint = data;
        //    }
        //    else
        //    {
        //        people.Palmvein = data;
        //    }
        //    Context.Update(people); //更新到数据库
        //    DeleteFeatureFile(oldData);//删除旧特征数据
        //    Commit();
        //    return new JsonResultModel();
        //}
        ///// <summary>
        ///// 保存特征文件
        ///// </summary>
        ///// <param name="type"></param>
        ///// <param name="data"></param>
        ///// <returns></returns>
        //private static string SaveFeatureFile(int type, byte[] data)
        //{
        //    try
        //    {
        //        var basePath = "feature";//特征根目录
        //        string targetPath = type == 1 ? "fingerprint" : "palmvein"; //指纹或掌静脉目录
        //        var fileName = Guid.NewGuid().ToString("N") + (type == 1 ? ".fp" : ".pm"); //文件名称
        //        string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", basePath, targetPath);//组合路径
        //        if (!Directory.Exists(path))//判断文件夹是否存在
        //        {
        //            Directory.CreateDirectory(path);//不存在则新建文件夹
        //        }
        //        var filePath = Path.Combine(path, fileName); //组合完整路径
        //        System.IO.File.WriteAllBytes(filePath, data);//写入文件
        //        return $"/{basePath}/{targetPath}/{fileName}";//返回URL路径地址
        //    }
        //    catch
        //    {
        //        return string.Empty;
        //    }
        //}
        ///// <summary>
        ///// 删除特征文件
        ///// </summary>
        ///// <param name="filePath"></param>
        //private static void DeleteFeatureFile(string filePath)
        //{
        //    try
        //    {
        //        if (!string.IsNullOrWhiteSpace(filePath))
        //        {
        //            string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");//组合路径
        //            path += filePath;
        //            if (System.IO.File.Exists(path))
        //            {
        //                System.IO.File.Delete(path);
        //            }
        //        }
        //    }
        //    catch
        //    {
        //    }
        //}
        ///// <summary>
        ///// 删除特征码
        ///// </summary>
        ///// <param name="fPar"></param>
        ///// <returns></returns>
        //public JsonResultModel DeleteFeature(FeatureParameter fPar)
        //{
        //    var people = Context.Find<People>(fPar.PeopleId);
        //    if (people == null) return new JsonResultModel(101,
        //        _LanguageHandler.GetCheckParameterErrorMessage("r96"));//"人员不存在"
        //    var featureStr = fPar.FeatureType == 1 ? people.Fingerprint : people.Palmvein; //判断是指纹还是掌静脉
        //    var featureList = JsonConvert.DeserializeObject<List<DetailInfo>>(featureStr); //将特征json信息序列化为对象
        //    var feature = featureList.FirstOrDefault(a => a.Num == fPar.Num);
        //    if (feature != null)
        //    {
        //        DeleteFeatureFile(feature.Data);
        //        featureList.Remove(feature);
        //    }
        //    var data = JsonConvert.SerializeObject(featureList);
        //    if (fPar.FeatureType == 1)
        //    {
        //        people.Fingerprint = data;
        //    }
        //    else
        //    {
        //        people.Palmvein = data;
        //    }
        //    Context.Update(people); //更新到数据库
        //    Commit();
        //    return new JsonResultModel();
        //}


        public People GetPeopleByUserID(long UserID)
        {
            return _Cache.GetPeopleCache(UserID);
        }




        /// <summary>
        /// 加载特征码
        /// </summary>
        /// <param name="people"></param>
        /// <returns></returns>
        public async Task LoadFeatureCode(PeopleDTO people)
        {
            //人脸特征码
            await LoadFeatureCode(people.FaceFeature, people.UserID, "Face");

            if (people.Fingerprints != null)
            {
                foreach (var item in people.Fingerprints)
                {
                    //指纹
                    await LoadFeatureCode(item, people.UserID, "FP");

                }
            }
            if (people.Palmveins != null)
            {

                foreach (var item in people.Palmveins)
                {
                    //掌静脉
                    await LoadFeatureCode(item, people.UserID, "Palmveins");

                }
            }
        }

        public async Task LoadFeatureCode(PeopleFeatureCode code, long userID, string sType)
        {
            if (code == null)
            {
                return;
            }

            if (string.IsNullOrEmpty(code.Data))
            {
                return;
            }

            if (code.Data.Length > 100)
                return;


            // 获取 IWebHostEnvironment 服务
            var env = this.serviceProvider.GetService<IWebHostEnvironment>();

            // 获取 wwwroot 目录路径
            var wwwrootPath = env.WebRootPath;

            string sFileName = code.Data;
            sFileName = sFileName.Substring(sFileName.LastIndexOf("/") + 1);
            string sFile = Path.Combine(wwwrootPath, "FeatureCode", sType, sFileName);

            if (System.IO.File.Exists(sFile))
            {
                var bBuf = await System.IO.File.ReadAllBytesAsync(sFile);
                code.Data = Convert.ToBase64String(bBuf);
                var buf = Convert.FromBase64String(code.Data);
                var md5 = MD5Helper.GetByteBufMD5ByHex(buf);
                if (code.MD5 != md5)
                {
                    code.Data = string.Empty;//校验不通过
                    _logger.LogError("特征码MD5校验失败！");
                }
            }
            else
            {
                code.Data = string.Empty;//文件未找到
                code.MD5 = string.Empty;
                _logger.LogError($"特征码文件丢失！{code.Data}");
            }

        }


        /// <summary>
        /// 删除特征码文件
        /// </summary>
        /// <param name="people"></param>
        /// <returns></returns>
        public async Task DeleteFeatureCode(PeopleDTO people)
        {
            //人脸特征码
            await DeleteFeatureCode(people.UserID, "Face");


            //指纹
            await DeleteFeatureCode(people.UserID, "FP");


            //掌静脉
            await DeleteFeatureCode(people.UserID, "Palmveins");

        }

        /// <summary>
        /// 删除特征码文件
        /// </summary>
        /// <param name="people"></param>
        /// <returns></returns>
        public Task DeleteFeatureCode(long userID, string sType)
        {
            // 获取 IWebHostEnvironment 服务
            var env = this.serviceProvider.GetService<IWebHostEnvironment>();

            // 获取 wwwroot 目录路径
            var wwwrootPath = env.WebRootPath;

            string sFileName = $"{userID}_{sType}*";
            string sFile = Path.Combine(wwwrootPath, "FeatureCode", sType);
            Directory.CreateDirectory(sFile);

            // 获取指定目录下以关键字开头的文件
            string[] files = Directory.GetFiles(sFile, sFileName);

            // 输出文件列表
            foreach (var file in files)
            {
                Console.WriteLine(file);
                if (System.IO.File.Exists(file))
                {
                    System.IO.File.Delete(file);
                }
            }

            return Task.CompletedTask;
        }


        /// <summary>
        /// 保存人员特征码到本地文件
        /// </summary>
        /// <param name="people"></param>
        /// <returns></returns>
        public async Task SaveFeatureCode(PeopleDTO people)
        {
            //人脸特征码
            await SaveFeatureCode(people.FaceFeature, people.UserID, "Face");

            if (people.Fingerprints != null)
            {
                foreach (var item in people.Fingerprints)
                {
                    //指纹
                    await SaveFeatureCode(item, people.UserID, "FP");

                }
            }
            if (people.Palmveins != null)
            {

                foreach (var item in people.Palmveins)
                {
                    //掌静脉
                    await SaveFeatureCode(item, people.UserID, "Palmveins");

                }
            }
        }





        /// <summary>
        /// 保存人员特征码到本地文件
        /// </summary>
        /// <param name="code"></param>
        /// <param name="userID"></param>
        /// <param name="sType"></param>
        /// <returns></returns>
        public async Task SaveFeatureCode(PeopleFeatureCode code, long userID, string sType)
        {
            if (code == null)
            {
                return;
            }

            if (!string.IsNullOrEmpty(code.Data))
            {
                var buf = Convert.FromBase64String(code.Data);
                var md5 = MD5Helper.GetByteBufMD5ByHex(buf);
                if (!string.IsNullOrEmpty(code.MD5))
                {
                    if (code.MD5 != md5)
                    {
                        code.Data = string.Empty;//校验不通过
                        _logger.LogError("特征码MD5校验失败！");
                    }
                }
                else
                {
                    code.MD5 = md5;
                }

            }


            // 获取 IWebHostEnvironment 服务
            var env = this.serviceProvider.GetService<IWebHostEnvironment>();

            // 获取 wwwroot 目录路径
            var wwwrootPath = env.WebRootPath;

            string sFileName = $"{userID}_{sType}_{code.Num}.bin";
            string sFile = Path.Combine(wwwrootPath, "FeatureCode", sType);
            Directory.CreateDirectory(sFile);
            sFile = Path.Combine(sFile, sFileName);
            if (string.IsNullOrEmpty(code.Data))
            {
                if (System.IO.File.Exists(sFile))
                {
                    System.IO.File.Delete(sFile);
                }
            }
            else
            {
                //base64转换为字节
                var buf = Convert.FromBase64String(code.Data);
                await System.IO.File.WriteAllBytesAsync(sFile, buf);

                code.Data = $"/FeatureCode/{sType}/{sFileName}";//替换为文件名

            }
        }

        public async Task<JsonResultModel> EnrollUserMediaData(EnrollUserMediaDataDTO enrollMediaData)
        {
            JsonResultModel ret = new JsonResultModel();
            //检查设备是否存在
            var deviceMap = _Cache.GetDeviceDictionary();
            if (!deviceMap.ContainsKey(enrollMediaData.DeviceID))
            {
                ret = new JsonResultModel(1, "设备不存在");
                return ret;
            }
            var device = deviceMap[enrollMediaData.DeviceID];

            //检查人员是否存在
            var cachePeople = _Cache.GetPeopleCache(enrollMediaData.UserID);
            if (cachePeople == null)
            {
                ret = new JsonResultModel(2, "人员不存在");
                return ret;
            }
            _logger.LogInformation($"开始在设备上注册凭证 {device.SN} UserId={cachePeople.UserID}");
            //对人员进行授权,并等待设备拉取
            var accessService = this.serviceProvider.GetService<IDeviceAccessService>();
            Stopwatch stopwatch = Stopwatch.StartNew();
            do
            {
                var accessPage = accessService.Query(new DeviceAccessQueryDTO()
                {
                    DeviceID = enrollMediaData.DeviceID,
                    PeopleID = cachePeople.ID,
                });
                if (accessPage.TotalCount == 0)
                {
                    _logger.LogInformation($"在设备上注册凭证 用户没有权限，增加权限");
                    await accessService.AddAccess(new DeviceAccessAddDTO()
                    {
                        DeviceIDs = new List<int>([enrollMediaData.DeviceID]),
                        PeopleIDs = new List<int>([cachePeople.ID]),
                        AccessType = 0,
                        ExpirationDate = DateTime.Now.AddYears(1),
                        OpenTimes = 65535,
                        KeepOpen = 0,
                        Timegroup = 1,
                        Elevators = "",
                        Holidays = ""
                    });
                }
                else
                {
                    int iAccessCount = accessPage.DataList.Where(page => page.UploadStatus == 1).Count();
                    if (iAccessCount == 1)
                    {
                        var access = accessPage.DataList.First();

                        if (access.UploadResult != 1)
                        {
                            ret = new JsonResultModel(10, "人员下发时发生问题！");
                            return ret;
                        }

                        _logger.LogInformation($"在设备上注册凭证 设备已拉取权限");
                        break;
                    }
                }

                if (stopwatch.ElapsedMilliseconds > 120_000)
                {
                    ret = new JsonResultModel(11, "设备操作超时");
                    return ret;
                }

                await Task.Delay(1000);
            } while (true);

            //添加远程任务
            var remoteService = this.serviceProvider.GetService<IDeviceRemoteService>();

            //查询是否有相同的任务
            var page = remoteService.Query(new RemoteTaskQueryDTO
            {
                SN = device.SN,
                TaskType = RemoteTypeEnum.RegisterIdentifyTicket,
                TaskStatus = 0,
            });

            if (page.TotalCount > 0)
            {
                _logger.LogInformation($"在设备上注册凭证 删除已存在远程任务");
                //删除旧的任务
                await remoteService.Delete(page.DataList.Select(x => x.TaskID).ToList());
            }

            var enrollData = JsonConvert.SerializeObject(enrollMediaData);
            var remoteTask = new RemoteTaskAddDTO()
            {
                DeviceIDs = new List<int>([enrollMediaData.DeviceID]),
                TaskType = RemoteTypeEnum.RegisterIdentifyTicket,
                UserID = enrollMediaData.UserID,
                TaskExtension = enrollData,
            };
            string sCacheKey = $"RegisterIdentifyTicket_{device.SN}";
            Cache.Remove(sCacheKey);

            await remoteService.Add(remoteTask);
            //获取到任务ID
            page = remoteService.Query(new RemoteTaskQueryDTO
            {
                SN = device.SN,
                TaskType = RemoteTypeEnum.RegisterIdentifyTicket,
                TaskStatus = 0,
            });
            var TaskID = page.DataList.First().TaskID;
            _logger.LogInformation($"在设备上注册凭证 添加任务完成，任务ID:{TaskID}");


            //等待任务完成
            do
            {
                page = remoteService.Query(new RemoteTaskQueryDTO
                {
                    TaskID = TaskID,
                    TaskStatus = 1

                });

                if (page.TotalCount == 1)
                {


                    //设备已拉取任务，等待操作完成
                    do
                    {
                        HTTPDeviceRegisterIdentifyTicketResult RegisterResult = null;
                        //检查缓存结果中是否有设备反馈的注册凭证信息
                        if (Cache.TryGetValue(sCacheKey, out RegisterResult))
                        {
                            _logger.LogInformation($"在设备上注册凭证 收到设备反馈 Result:{RegisterResult.Result}");

                            if (RegisterResult.Result == 1)
                            {
                                var hPeople = RegisterResult.UserDetail;
                                //更新人员
                                PeopleDTO dto = new PeopleDTO()
                                {
                                    UserID = hPeople.UserID
                                };
                                await DeleteFeatureCode(dto); //清空人员的特征码
                                //更新人员特征码
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
                                await SaveFeatureCode(dto);//保存特征码到文件

                                if (!string.IsNullOrEmpty(hPeople.FaceFeature))
                                {
                                    hPeople.FaceFeature = dto.FaceFeature.Data;
                                    hPeople.FaceFeatureMD5 = dto.FaceFeature.MD5;
                                }

                                //检查是否包含人员照片
                                if (hPeople.PhotoLen > 0)
                                {
                                    //转移照片路径
                                    string sPhotoFile = hPeople.Photo;
                                    hPeople.Photo =  $"/People/{cachePeople.UserID}.jpg?md5={hPeople.PhotoMD5}";
                                    //移动人员照片
                                    string sNewFile = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "People", $"{cachePeople.UserID}.jpg");
                                    try
                                    {
                                        if (System.IO.File.Exists(sNewFile))
                                            System.IO.File.Delete(sNewFile);

                                        if (System.IO.File.Exists(sPhotoFile))
                                            System.IO.File.Move(sPhotoFile, sNewFile);
                                    }
                                    catch (Exception ex)
                                    {
                                        _logger.LogError("在设备上注册凭证，保存人员照片时发生错误！" + ex.Message);
                                    }
                                }

                                var upPeople = hPeople.Adapt<People>();
                                upPeople.ID = cachePeople.ID;
                                upPeople.CreateTime = cachePeople.CreateTime;
                                await UpdatePeople(upPeople, null);//更新数据库
                                dto = upPeople.Adapt<PeopleDTO>();
                                await LoadFeatureCode(dto);//重新加载特征码
                                ret = new JsonResultModel(dto);//返回到前端
                            }
                            if (RegisterResult.Result == 2)
                            {
                                ret = new JsonResultModel(4, "用户取消操作");
                            }
                            if (RegisterResult.Result == 3)
                            {
                                ret = new JsonResultModel(5, "重复注册，重复用户号：" + RegisterResult.RepetitionUserID.ToString());
                                ret.Content = new { RegisterResult.RepetitionUserID };
                            }
                            if (RegisterResult.Result == 4)
                            {
                                ret = new JsonResultModel(6, "不支持指纹");
                            }
                            if (RegisterResult.Result == 5)
                            {
                                ret = new JsonResultModel(7, "不支持掌静脉");
                            }
                            if (RegisterResult.Result == 6)
                            {
                                ret = new JsonResultModel(8, "设备正在忙");
                            }
                            break;
                        }
                        if (stopwatch.ElapsedMilliseconds > 120_000)
                        {
                            ret = new JsonResultModel(3, "设备操作超时");
                            break;
                        }
                        await Task.Delay(1000);
                    } while (true);
                    break;
                }
                else
                {
                    if (stopwatch.ElapsedMilliseconds > 120_000)
                    {
                        ret = new JsonResultModel(3, "设备操作超时");
                        break;
                    }
                }

                await Task.Delay(1000);

            } while (true);

            return ret;

        }

    }
}
