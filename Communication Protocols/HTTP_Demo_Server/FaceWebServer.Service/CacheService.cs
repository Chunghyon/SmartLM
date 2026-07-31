using FaceWebServer.DB.Table;
using FaceWebServer.DTO.Cache;
using FaceWebServer.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FaceWebServer.Service
{
    public class CacheService : BaseService, ICacheService
    {
        public IServiceProvider _ServiceProvider { get; set; }
        public CacheService(DbContext context) : base(context)
        {
        }

        public void Set<T>(string skey, T value,TimeSpan time)
        {
            Cache.Set<T>(skey, value, time);
        }

        public void Set<T>(string skey, T value)
        {
            Cache.Set<T>(skey, value);
        }
        public T Get<T>(string skey)
        {
            return Cache.Get<T>(skey);
        }
        public void Remove(string skey)
        {
            Cache.Remove(skey);
        }
        

        public bool IniSystemCache()
        {
            var sKey = string.Empty;


            //加载所有设备列表
            var devices = Set<DeviceDetail>().AsNoTracking()
            .Select(x => new CacheDeviceDTO()
            {
                ID = x.ID,
                SN = x.SN,
                Protocol = x.Protocol,
                DeviceName = x.Name,
                UploadStatus = x.UploadStatus
            }).ToDictionary(x => x.SN);


            foreach (var item in devices)
            {
                Cache.Set(item.Key, item.Value);
            }
            //缓存设备列表
            Cache.Set(ICacheService.DevicesCacheKey, devices.Keys.ToHashSet());

            Cache.Set(ICacheService.DeviceDictionaryCacheKey, devices.Values.ToDictionary(x => x.ID));


            #region 人员
            //加载所有的人员--为了给记录使用
            var peoples = Set<People>().AsNoTracking();
            foreach (var item in peoples)
            {
                Cache.Set(item.UserID, item);
            }
            Cache.Set(ICacheService.PeopleUserIDsCacheKey, peoples.Select(x => x.UserID).ToHashSet());

            Cache.Set(ICacheService.PeopleDictionaryCacheKey, peoples.ToDictionary(x => x.ID));
            peoples = null;
            #endregion

            #region 权限统计
            //更新所有权限信息
            var accessService = _ServiceProvider.GetService<IDeviceAccessService>();
            accessService.UpdateDeviceAccessTotal(devices.Values.Select(t => t.ID));

            //加载所有任务

            var remoteService = _ServiceProvider.GetService<IDeviceRemoteService>();
            remoteService.UpdateRemoteTotal(devices.Values.Select(t => t.SN));
            #endregion


            #region 开门时段
            //加载所有开门时段
            ITimeGroupService timeGroupService = _ServiceProvider.GetService<ITimeGroupService>();

            var TimeGroupDetails = timeGroupService.GetAll();
            if (TimeGroupDetails.Count == 0)
            {
                //初始化开门时段
                timeGroupService.IniTimeGroupDB();
            }
            #endregion

            return true;
        }

        #region 设备缓存

        public HashSet<string> GetDevices()
        {
            return Cache.Get<HashSet<string>>(ICacheService.DevicesCacheKey);
        }

        public Dictionary<int, CacheDeviceDTO> GetDeviceDictionary()
        {
            return Cache.Get<Dictionary<int, CacheDeviceDTO>>(ICacheService.DeviceDictionaryCacheKey);
        }
        public CacheDeviceDTO AddDeviceCache(DeviceDetail oDevice)
        {

            var dto = new CacheDeviceDTO()
            {
                ID = oDevice.ID,
                SN = oDevice.SN,
                Protocol = oDevice.Protocol,
                DeviceName = oDevice.Name,
                UploadStatus = oDevice.UploadStatus
            };
            Cache.Set(dto.SN, dto);
            var snList = GetDevices();
            snList.Add(oDevice.SN);

            var deviceDic = GetDeviceDictionary();
            deviceDic.Add(oDevice.ID, dto);
            return dto;
        }

        public bool UpdateDeviceCache(string SN, Action<CacheDeviceDTO> updateAction)
        {
            var dto = Cache.Get<CacheDeviceDTO>(SN);
            updateAction(dto);
            return true;
        }

        public CacheDeviceDTO GetDevice(string sn)
        {
            if (string.IsNullOrEmpty(sn)) return null;
            return Cache.Get<CacheDeviceDTO>(sn);
        }

        public bool DeleteDeviceCache(List<DeviceDetail> oDevices)
        {
            var snList = GetDevices();
            var deviceDic = GetDeviceDictionary();
            CacheDeviceDTO dto;
            foreach (var item in oDevices)
            {
                dto = Cache.Get<CacheDeviceDTO>(item.SN);

                Cache.Remove(item.SN);
                snList.Remove(item.SN);
                if (dto != null)
                {
                    deviceDic.Remove(dto.ID);
                }
            }
            return true;
        }

        public bool DeleteDeviceCache(params string[] snList)
        {
            var snMap = GetDevices();
            var deviceDic = GetDeviceDictionary();
            CacheDeviceDTO dto;
            foreach (var sn in snList)
            {
                dto = Cache.Get<CacheDeviceDTO>(sn);

                Cache.Remove(sn);
                snMap.Remove(sn);
                if (dto != null)
                {
                    deviceDic.Remove(dto.ID);
                }
            }
            return true;
        }

        #endregion

        #region 人员缓存

        public HashSet<long> GetPeopleUserIDs()
        {
            return Cache.Get<HashSet<long>>(ICacheService.PeopleUserIDsCacheKey);
        }

        public Dictionary<int, People> GetPeopleDictionary()
        {
            return Cache.Get<Dictionary<int, People>>(ICacheService.PeopleDictionaryCacheKey);
        }


        public bool AddPeopleCache(People people)
        {

            Cache.Set(people.UserID, people);
            var userIDs = GetPeopleUserIDs();
            userIDs.Add(people.UserID);
            var dic = GetPeopleDictionary();
            dic.Add(people.ID, people);

            return true;
        }

        public bool UpdatePeopleCache(People people)
        {
            var dic = GetPeopleDictionary();
            dic[people.ID] = people;

            Cache.Remove(people.UserID);
            Cache.Set(people.UserID, people);
            return true;
        }

        public People GetPeopleCache(long sCode)
        {
            return Cache.Get<People>(sCode);

        }

        public bool DeletePeopleCache(List<long> sUserIDList)
        {
            var peoples = GetPeopleUserIDs();
            var dic = GetPeopleDictionary();
            People dto;

            foreach (var item in sUserIDList)
            {
                dto = Cache.Get<People>(item);
                peoples.Remove(item);
                Cache.Remove(item);
                if (dto != null)
                {
                    dic.Remove(dto.ID);
                }
            }
            return true;
        }
        #endregion

        #region 设备拉取权限缓存
        public void SavePeopleAccessList(string sSN, Dictionary<long, int> userAccessMap)
        {
            string sKey = $"{ICacheService.AccessCachePrefix}{sSN}";
            Cache.Set(sKey, userAccessMap);
        }


        public Dictionary<long, int> GetPeopleAccessList(string sSN)
        {
            string sKey = $"{ICacheService.AccessCachePrefix}{sSN}";
            return Cache.Get<Dictionary<long, int>>(sKey);
        }

        public void DeletePeopleAccessList(string sSN)
        {
            string sKey = $"{ICacheService.AccessCachePrefix}{sSN}";
            Cache.Remove(sKey);
        }



        public void SaveDeletePeopleAccessList(string sSN, Dictionary<long, int> userAccessMap)
        {
            string sKey = $"{ICacheService.AccessDeleteCachePrefix}{sSN}";
            Cache.Set(sKey, userAccessMap);
        }

        public Dictionary<long, int> GetDeletePeopleAccessList(string sSN)
        {
            string sKey = $"{ICacheService.AccessDeleteCachePrefix}{sSN}";
            return Cache.Get<Dictionary<long, int>>(sKey);
        }

        public void DeleteDeletePeopleAccessList(string sSN)
        {
            string sKey = $"{ICacheService.AccessDeleteCachePrefix}{sSN}";
            Cache.Remove(sKey);
        }
        #endregion

    }
}