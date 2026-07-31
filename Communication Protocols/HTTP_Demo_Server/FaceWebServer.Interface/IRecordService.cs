using FaceWebServer.DB.Table;
using FaceWebServer.DTO.Record;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaceWebServer.Interface
{
    /// <summary>
    /// 人脸机记录服务
    /// </summary>
    public interface IRecordService : IBaseService
    {
        /// <summary>
        /// 查询记录
        /// </summary>
        /// <param name="par"></param>
        /// <returns></returns>
        PageResult<IdentifyRecord> QueryIdentifyRecord(IdentifyReportQueryDTO par);

        /// <summary>
        /// 查询系统记录
        /// </summary>
        /// <param name="par"></param>
        /// <returns></returns>
        PageResult<SystemRecord> QuerySystemRecord(SystemReportQueryDTO par);

        /// <summary>
        /// 新增一条打卡记录
        /// </summary>
        /// <param name="record"></param>
        /// <returns></returns>
        Task<bool> AddRecord(IdentifyRecord record);

        /// <summary>
        /// 新增系统记录
        /// </summary>
        Task<bool> AddRecord(string sSN, List<SystemRecord> records);

        /// <summary>
        /// 清空打卡记录
        /// </summary>
        /// <returns></returns>
        Task ClearIdentifyRecord();

        /// <summary>
        /// 清空出入记录
        /// </summary>
        /// <returns></returns>
        Task ClearSystemRecord();

       
    }
}
