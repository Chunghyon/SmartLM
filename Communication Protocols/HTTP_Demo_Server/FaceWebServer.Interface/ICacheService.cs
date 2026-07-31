using FaceWebServer.DB.Table;
using FaceWebServer.DTO.Cache;
using System;
using System.Collections.Generic;

namespace FaceWebServer.Interface
{
    /// <summary>
    /// 缓存预热服务
    /// </summary>
    public interface ICacheService : IBaseService
    {
        /// <summary>
        /// 设备列表缓存键
        /// </summary>
        public const string DevicesCacheKey = "Devices";

        /// <summary>
        /// 设备ID为key的字典键
        /// </summary>
        public const string DeviceDictionaryCacheKey = "DeviceDictionary";

        /// <summary>
        /// 人员列表缓存键
        /// </summary>
        public const string PeopleUserIDsCacheKey = "Peoples";

        /// <summary>
        /// 人员ID为key的字典键
        /// </summary>
        public const string PeopleDictionaryCacheKey = "PeopleDictionary";

        /// <summary>
        /// 设备拉取权限缓存键前缀
        /// </summary>
        public const string AccessCachePrefix = "Access:";

        /// <summary>
        /// 设备拉取待删除权限缓存键前缀
        /// </summary>
        public const string AccessDeleteCachePrefix = "DeleteAccess:";





        /// <summary>
        /// 初始化系统缓存
        /// </summary>
        /// <returns></returns>
        bool IniSystemCache();

        #region 设备缓存

        /// <summary>
        /// 获取所有设备SN列表
        /// </summary>
        /// <returns></returns>
        HashSet<string> GetDevices();


        /// <summary>
        /// 获取所有设备列表，门ID为Key
        /// </summary>
        /// <returns></returns>
        Dictionary<int, CacheDeviceDTO> GetDeviceDictionary();

        /// <summary>
        /// 添加设备缓存--新增设备时使用
        /// </summary>
        /// <param name="oDevice"></param>
        /// <returns></returns>
        CacheDeviceDTO AddDeviceCache(DeviceDetail oDevice);



        /// <summary>
        /// 更新设备缓存
        /// </summary>
        /// <param name="SN"></param>
        /// <param name="updateAction"></param>
        /// <returns></returns>
        bool UpdateDeviceCache(string SN, Action<CacheDeviceDTO> updateAction);


        /// <summary>
        /// 根据SN获取设备信息
        /// </summary>
        /// <param name="sn"></param>
        /// <returns></returns>
        CacheDeviceDTO GetDevice(string sn);

        /// <summary>
        /// 删除设备缓存
        /// </summary>
        /// <param name="oDevices"></param>
        /// <returns></returns>
        bool DeleteDeviceCache(List<DeviceDetail> oDevices);

        /// <summary>
        /// 删除设备缓存
        /// </summary>
        /// <param name="deviceIds"></param>
        /// <returns></returns>
        bool DeleteDeviceCache(params string[] deviceIds);
        #endregion

        #region 人员缓存
        /// <summary>
        /// 获取所有用户号列表
        /// </summary>
        /// <returns></returns>
        HashSet<long> GetPeopleUserIDs();

        /// <summary>
        /// 获取以人员ID为key的人员字典
        /// </summary>
        /// <returns></returns>
        Dictionary<int, People> GetPeopleDictionary();

        /// <summary>
        /// 添加人员缓存
        /// </summary>
        /// <param name="people"></param>
        /// <returns></returns>
        bool AddPeopleCache(People people);

        /// <summary>
        /// 更新人员缓存
        /// </summary>
        /// <param name="people"></param>
        /// <returns></returns>
        bool UpdatePeopleCache(People people);

        /// <summary>
        /// 通过用户号 获取人员缓存
        /// </summary>
        /// <param name="sUserID"></param>
        /// <returns></returns>
        People GetPeopleCache(long sUserID);

        /// <summary>
        /// 删除人员缓存
        /// </summary>
        /// <param name="sUserIDList"></param>
        /// <returns></returns>
        bool DeletePeopleCache(List<long> sUserIDList);
        #endregion

        #region 设备拉取权限缓存

        /// <summary>
        /// 保存设备拉取的用户号列表
        /// </summary>
        /// <param name="sSN"></param>
        /// <param name="userAccessMap">人员权限映射表  key是用户号，value是权限ID</param>
        void SavePeopleAccessList(string sSN, Dictionary<long, int> userAccessMap);

        /// <summary>
        /// 获取设备拉取的用户号列表
        /// </summary>
        /// <param name="sSN"></param>
        /// <returns>人员权限映射表  key是用户号，value是权限ID</returns>
        Dictionary<long, int> GetPeopleAccessList(string sSN);

        /// <summary>
        /// 从缓存中删除设备拉取的用户号列表
        /// </summary>
        /// <param name="sSN"></param>
        void DeletePeopleAccessList(string sSN);

        /// <summary>
        /// 保存设备拉取的待删除用户号列表 key是用户号，value是权限ID
        /// </summary>
        /// <param name="sSN"></param>
        /// <param name="userAccessMap">人员权限映射表  key是用户号，value是权限ID</param>
        void SaveDeletePeopleAccessList(string sSN, Dictionary<long, int> userAccessMap);

        /// <summary>
        /// 获取设备拉取的待删除用户号列表
        /// </summary>
        /// <param name="sSN"></param>
        /// <returns>人员权限映射表  key是用户号，value是权限ID</returns>
        Dictionary<long, int> GetDeletePeopleAccessList(string sSN);

        /// <summary>
        /// 从缓存中删除设备拉取的待删除用户号列表
        /// </summary>
        /// <param name="sSN"></param>
        void DeleteDeletePeopleAccessList(string sSN);
        #endregion

        void Set<T>(string skey, T value, TimeSpan time);
        void Set<T>(string skey, T value);
        T Get<T>(string skey);

        void Remove(string skey);
    }
}
