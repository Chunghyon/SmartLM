using FaceWebServer.DB.Table;
using FaceWebServer.DTO.IOLog;
using FaceWebServer.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace FaceWebServer.Service
{
    /// <summary>
    /// 人脸机API调用日志服务
    /// </summary>
    public class ConnectIOLogService : BaseService, IConnectIOLogService
    {


        public ConnectIOLogService(DbContext context) : base(context)
        {

        }

        public PageResult<ConnectIOLog> Query(ConnectIOLogQueryDTO queryDTO)
        {
            List<Expression<Func<ConnectIOLog, bool>>> oWheres = new List<Expression<Func<ConnectIOLog, bool>>>();
            oWheres.Add(x => x.LogTime >= queryDTO.QueryBeginTime && x.LogTime <= queryDTO.QueryEndTime);
            if (!string.IsNullOrWhiteSpace(queryDTO.Protocol)) oWheres.Add(x => x.Protocol.Equals(queryDTO.Protocol));
            if (!string.IsNullOrWhiteSpace(queryDTO.APIName)) oWheres.Add(x => x.APIName.Equals(queryDTO.APIName));
            if (!string.IsNullOrWhiteSpace(queryDTO.NotAPIName)) oWheres.Add(x => !x.APIName.Equals(queryDTO.NotAPIName));

            if (!string.IsNullOrWhiteSpace(queryDTO.SN)) oWheres.Add(x => x.SN.Contains(queryDTO.SN));
            if (!string.IsNullOrWhiteSpace(queryDTO.HttpType)) oWheres.Add(x => x.HttpType.Equals(queryDTO.HttpType));
            

            return QueryPage(
               oWheres, queryDTO.PageSize, queryDTO.PageIndex,
               x => x.LogTime,
               queryDTO.IsAsc);

        }

        public bool AddConnectLog(ConnectIOLog log)
        {
            log.LogTime = System.DateTime.Now;
            Insert(log);
            return true;
        }

        public async Task<bool> AddConnectLogAsync(ConnectIOLog log)
        {
            log.LogTime = System.DateTime.Now;
            await InsertAsync(log);
            return true;
        }

        public bool ClearLog()
        {
            Excute("Delete from ConnectIOLog");

            //try
            //{
            //    ExcuteNoTransaction("VACUUM");
            //}
            //catch (System.Exception ex) 
            //{
            //    throw ex;
            //}
            
            AddUserLog("通讯日志","清空通讯日志");
            Commit();
            return true;
        }

        
    }
}
