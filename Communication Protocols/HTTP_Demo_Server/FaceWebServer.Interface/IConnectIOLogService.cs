using FaceWebServer.DB.Table;
using FaceWebServer.DTO.IOLog;
using System.Threading.Tasks;

namespace FaceWebServer.Interface
{
    /// <summary>
    /// 人脸机API调用日志服务
    /// </summary>
    public interface IConnectIOLogService : IBaseService
    {

        /// <summary>
        /// 查询API日志
        /// </summary>
        /// <param name="queryDTO"></param>
        /// <returns></returns>
        PageResult<ConnectIOLog> Query(ConnectIOLogQueryDTO queryDTO);

        /// <summary>
        /// 新增日志
        /// </summary>
        /// <returns></returns>
        bool AddConnectLog(ConnectIOLog log);

        /// <summary>
        /// 新增日志 ，异步
        /// </summary>
        /// <param name="log"></param>
        /// <returns></returns>
        Task<bool> AddConnectLogAsync(ConnectIOLog log);

        /// <summary>
        /// 清空日志
        /// </summary>
        /// <returns></returns>
        bool ClearLog();
    }
}
