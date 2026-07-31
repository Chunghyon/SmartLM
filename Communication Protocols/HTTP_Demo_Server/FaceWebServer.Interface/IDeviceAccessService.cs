using FaceWebServer.DB.Table;
using FaceWebServer.DTO;
using FaceWebServer.DTO.Access;
using FaceWebServer.DTO.HTTPv1_Protocol;
using FaceWebServer.DTO.HTTPv2_Protocol;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FaceWebServer.Interface
{
    /// <summary>
    /// 设备门禁权限服务
    /// </summary>
    public interface IDeviceAccessService : IBaseService
    {

        /// <summary>
        /// 根据条件查询门禁权限
        /// </summary>
        /// <param name="sn"></param>
        /// <param name="name"></param>
        /// <param name="remote"></param>
        /// <returns></returns>
        PageResult<DeviceAccessQueryResultDTO> Query(DeviceAccessQueryDTO queryPar);

        /// <summary>
        /// 根据权限ID获取权限详情
        /// </summary>
        /// <returns></returns>
        PeopleAccessDetail GetAccessDetail(int iAccessID);


        /// <summary>
        /// 批量授权
        /// </summary>
        /// <returns></returns>
        Task AddAccess(DeviceAccessAddDTO addDto);

        /// <summary>
        /// 对所有人进行权限添加操作的接口
        /// </summary>
        /// <returns></returns>
        Task AddAccess_ALLPeople(DeviceAccessAddDTO addDto);


        /// <summary>
        /// 删除授权
        /// </summary>
        /// <returns></returns>
        Task DeleteAccess(DeviceAccessDeleteDTO dto);


        /// <summary>
        /// 对所有人进行权限删除操作的接口
        /// </summary>
        /// <returns></returns>
        Task DeleteAccess_ALLPeople(DeviceAccessDeleteDTO addDto);

        /// <summary>
        /// 清空当前设备列表有关所有人员权限
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task ClearAllPeople(DeviceAccessDeleteDTO dto);

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="sn"></param>
        /// <returns></returns>
        Task Delete(List<int> iAccessID);

        /// <summary>
        /// 更新单个开门权限
        /// </summary>
        void Update(PeopleAccessDetail detail);

        /// <summary>
        /// 清空所有开门权限
        /// </summary>
        Task ClearAccess();

        /// <summary>
        /// 更新设备权限统计数
        /// </summary>
        /// <param name="deviceIDList"></param>
        /// <returns></returns>
        Task UpdateDeviceAccessTotal(IEnumerable<int> deviceIDList);

        /// <summary>
        /// 使指定的权限重新上传
        /// </summary>
        /// <param name="accessIDs"></param>
        Task Reupload(List<int> accessIDs);

        /// <summary>
        /// 使全部的权限重新上传
        /// </summary>
        Task ReuploadAll();

        /// <summary>
        /// 设备清空人员后调用此接口，将权限设置为未上传，并删除已删除的权限
        /// </summary>
        /// <param name="deviceID"></param>
        Task ReuploadByDevice(int deviceID);



        /// <summary>
        /// 根据门ID获取指定数量的需要同步的权限
        /// </summary>
        /// <returns>返回字典，key表示权限ID</returns>
        Task<List<PeopleAccessDetail>> GetDownloadAccess(int doorID, int iLimit);


        /// <summary>
        /// 更新人员上传状态
        /// </summary>
        Task UpdatePeopleAccessUploadResult(int doorID, List<DeviceAccessUploadStatusUpdateDTO> updateAccessList);


        /// <summary>
        /// 根据门ID获取待删除的权限
        /// </summary>
        /// <returns>返回字典，key表示权限ID   value表示人员用户号</returns>
        Task<Dictionary<int, long>> GetDeleteAccess(int DoorID, int limit);

        /// <summary>
        /// 保存设备删除人员权限的结果
        /// </summary>
        Task SaveDeleteAccessResult(int DoorID, List<int> accessList);


        /// <summary>
        /// 将指定查询条件的记录重新上传
        /// </summary>
        /// <param name="par"></param>
        /// <returns></returns>
        Task ReuploadFilterAllAsync(DeviceAccessQueryDTO par);
    }
}
