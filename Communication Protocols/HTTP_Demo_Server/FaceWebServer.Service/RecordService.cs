using DoNetDrive.Common.Extensions;
using FaceWebServer.DTO.Cache;
using FaceWebServer.DTO.Record;
using FaceWebServer.Interface;
using FaceWebServer.Language;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using FaceWebServer.DB.Table;
using FaceWebServer.DTO.Device;
using System.Threading.Tasks;
using FaceWebServer.Utility;
using System.Linq;

namespace FaceWebServer.Service
{
    public class RecordService : BaseService, IRecordService
    {
        private LanguageHandler _LanguageHandler;
        public RecordService(DbContext context, IOptionsSnapshot<LanguageOption> lngopt) : base(context)
        {
            _LanguageHandler = lngopt.Value.GetCurrentLanguageHandler();
        }

        public PageResult<IdentifyRecord> QueryIdentifyRecord(IdentifyReportQueryDTO queryDTO)
        {
            List<Expression<Func<IdentifyRecord, bool>>> oWheres = new();
            oWheres.Add(x => x.RecordDate >= queryDTO.QueryBeginTime && x.RecordDate <= queryDTO.QueryEndTime);


            if (!string.IsNullOrWhiteSpace(queryDTO.SN)) oWheres.Add(x => x.SN.Contains(queryDTO.SN));
            if (queryDTO.RecordType.HasValue) oWheres.Add(x => x.RecordType == queryDTO.RecordType.Value);
            if (queryDTO.RecordTypeList != null) oWheres.Add(x => queryDTO.RecordTypeList.Contains(x.RecordType));


            if (queryDTO.UserID.HasValue) oWheres.Add(x => x.UserID == queryDTO.UserID.Value);
            if (!string.IsNullOrWhiteSpace(queryDTO.Name)) oWheres.Add(x => x.Name.Contains(queryDTO.Name));
            if (!string.IsNullOrWhiteSpace(queryDTO.Job)) oWheres.Add(x => x.Job.Contains(queryDTO.Job));
            if (!string.IsNullOrWhiteSpace(queryDTO.Department)) oWheres.Add(x => x.Department.Contains(queryDTO.Department));
            if (!string.IsNullOrWhiteSpace(queryDTO.IdentityCard)) oWheres.Add(x => x.IdentityCard.Contains(queryDTO.IdentityCard));
            if (queryDTO.CardNum.HasValue) oWheres.Add(x => x.CardNum == queryDTO.CardNum.Value);
            if (!string.IsNullOrWhiteSpace(queryDTO.QRCode)) oWheres.Add(x => x.QRCode.Contains(queryDTO.QRCode));

            if (queryDTO.IsEntry.HasValue) oWheres.Add(x => x.IsEntry == queryDTO.IsEntry.Value);


            var devices = QueryPage(
            oWheres, queryDTO.PageSize, queryDTO.PageIndex,
            x => x.RecordDate,
            queryDTO.IsAsc);

            return devices;
        }

        public PageResult<SystemRecord> QuerySystemRecord(SystemReportQueryDTO queryDTO)
        {
            List<Expression<Func<SystemRecord, bool>>> oWheres = new();
            oWheres.Add(x => x.RecordDate >= queryDTO.QueryBeginTime && x.RecordDate <= queryDTO.QueryEndTime);


            if (!string.IsNullOrWhiteSpace(queryDTO.SN)) oWheres.Add(x => x.SN.Contains(queryDTO.SN));
            if (queryDTO.RecordType.HasValue) oWheres.Add(x => x.RecordType == queryDTO.RecordType.Value);
            if (queryDTO.RecordTypeList != null) oWheres.Add(x => queryDTO.RecordTypeList.Contains(x.RecordType));


            var devices = QueryPage(
            oWheres, queryDTO.PageSize, queryDTO.PageIndex,
            x => x.RecordDate,
            queryDTO.IsAsc);

            return devices;
        }

        private string CreateRecordMD5(string sn, int id, string date, string photo = "")
        {
            string sMD5 = $"{sn}_{id}_{date}_{photo}";
            return MD5Helper.GetStringMD5ByBase64(sMD5);
        }

        public async Task<bool> AddRecord(IdentifyRecord record)
        {

            record.InsertTime = DateTime.Now;
            //计算记录的MD5
            record.RecordMD5 = CreateRecordMD5(record.SN, record.RecordID,
                record.RecordDate.ToDateTimeStr(), $"{record.Photo}_{record.PhotoLen}");

            int iCount = await Query<IdentifyRecord>(x => x.RecordMD5 == record.RecordMD5).CountAsync();
            if (iCount > 0)
            {
                return true;//重复
            }


            await InsertAsync(record);
            return true;
        }

        public async Task<bool> AddRecord(string sSN, List<SystemRecord> recordList)
        {

            foreach (var record in recordList)
            {
                record.SN = sSN;
                record.InsertTime = DateTime.Now;
             //   if(record.RecordDate== "1970/1/21 22:44:39")
                //计算记录的MD5
                record.RecordMD5 = CreateRecordMD5(record.SN, record.RecordID, record.RecordDate.ToDateTimeStr());
            }

            var md5List = recordList.Select(x => x.RecordMD5);

            var dbRecordIDList = (await Query<SystemRecord>(x => md5List.Contains(x.RecordMD5)).Select(x => x.RecordMD5).ToListAsync()).ToHashSet();
            if (dbRecordIDList.Count > 0)
            {
                recordList.RemoveAll(x => dbRecordIDList.Contains(x.RecordMD5));

            }

            if (recordList.Count > 0)
            {
                await AddRangeAsync(recordList);

            }
           
            return true;
        }

        public async Task ClearIdentifyRecord()
        {
            await Context.Set<IdentifyRecord>().ExecuteDeleteAsync();

            AddUserLog(_LanguageHandler.GetUserLog("t4"),//出入记录
                _LanguageHandler.GetUserLog("r28")); //"清空所有出入记录");
            await CommitAsync();
        }

        public async Task ClearSystemRecord()
        {
            await Context.Set<SystemRecord>().ExecuteDeleteAsync();

            AddUserLog(_LanguageHandler.GetUserLog("t4"),//系统记录
                _LanguageHandler.GetUserLog("ClearSystemRecordLog")); //"清空所有系统记录");
            await CommitAsync();
        }
    }
}
