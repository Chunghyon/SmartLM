using FaceWebServer.DB.Table;
using FaceWebServer.DTO;
using FaceWebServer.DTO.Cache;
using FaceWebServer.DTO.Remote;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FaceWebServer.Interface
{
    /// <summary>
    /// 设备远程操作服务
    /// </summary>
    public interface IDeviceRemoteService : IBaseService
    {

        /// <summary>
        /// 根据条件查询远程操作
        /// </summary>
        /// <param name="sn"></param>
        /// <param name="name"></param>
        /// <param name="remote"></param>
        /// <returns></returns>
        PageResult<RemoteTaskDetail> Query(RemoteTaskQueryDTO queryDto);

        /// <summary>
        /// 根据设备ID获取待操作的任务
        /// </summary>
        /// <returns></returns>
        List<RemoteTaskDetail> GetRemoteTaskBySN(string sSN);


        /// <summary>
        /// 批量新增远程操作
        /// </summary>
        /// <returns></returns>
        Task Add(RemoteTaskAddDTO parameter);


        /// <summary>
        /// 批量删除远程操作任务
        /// </summary>
        /// <param name="sn"></param>
        /// <returns></returns>
        Task Delete(List<int> taskIDs);

        /// <summary>
        /// 更新指定设备的指定操作类型为已完成
        /// </summary>
        Task UpdateTaskRunStatusComplete(List<int> taskIDs, int deviceID,string sn);

        /// <summary>
        /// 清空所有远程操作记录
        /// </summary>
        Task ClearRemote();

        /// <summary>
        /// 更新指定门ID的远程操作统计
        /// </summary>
        /// <param name="snList"></param>
        /// <returns></returns>
        Task UpdateRemoteTotal(IEnumerable<string> snList);
    }
}
