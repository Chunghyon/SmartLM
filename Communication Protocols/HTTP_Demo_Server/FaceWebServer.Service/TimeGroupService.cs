using FaceWebServer.DB.Table;
using FaceWebServer.DTO;
using FaceWebServer.Interface;
using FaceWebServer.Language;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FaceWebServer.Service
{
    /// <summary>
    /// 开门时段
    /// </summary>
    public class TimeGroupService : BaseService, ITimeGroupService
    {
        public ICacheService _CacheService { get; set; }
        public IFaceDriveService _FaceDriveDB { get; set; }

        private LanguageHandler _LanguageHandler;
        public TimeGroupService(DbContext context, IOptionsSnapshot<LanguageOption> lngopt) : base(context)
        {
            _LanguageHandler = lngopt.Value.GetCurrentLanguageHandler();
        }

        public PageResult<TimeGroupDetail> GetAll(BasePageParameter par)
        {
            return QueryPage<TimeGroupDetail, int>(
                null, par.PageSize, par.PageIndex,
                x => x.GroupNum,
                true); ;
        }

        public List<TimeGroupDetail> GetAll()
        {
            return Query<TimeGroupDetail>(null).ToList();
        }
        /// <summary>
        /// 获取一个可用的时段号
        /// </summary>
        /// <returns></returns>
        public int GetNewGroupNum()
        {
            var numList = Query<TimeGroupDetail>(null).Select(x => x.GroupNum).ToHashSet();
            for (int i = 1; i <= 64; i++)
            {
                if(!numList.Contains(i))
                {
                    return i;
                }
            }
            return 0;
        }

        public async Task IniTimeGroupDB()
        {
            var db = Context.Set<TimeGroupDetail>();
            await db.ExecuteDeleteAsync();

            AddUserLog(_LanguageHandler.GetUserLog("t5"),//开门时段
                _LanguageHandler.GetUserLog("r29"));//初始化开门时段

            var detail = new TimeGroupDetail(1);
            detail.Week1 = "00:00-23:59";
            detail.Week2 = detail.Week1;
            detail.Week3 = detail.Week1;
            detail.Week4 = detail.Week1;
            detail.Week5 = detail.Week1;
            detail.Week6 = detail.Week1;
            detail.Week7 = detail.Week1;


            await InsertAsync(detail);

            await _FaceDriveDB.UpdateAllDeviceUploadStatus();

        }



        /// <summary>
        /// 添加开门时段
        /// </summary>
        /// <param name="detail"></param>
        public async Task AddTimeGroup(TimeGroupDetail detail)
        {
            if (detail.GroupNum < 0 || detail.GroupNum > 64)
                return;

            //检查编号是否存在
            int iCount = await Query<TimeGroupDetail>(x => x.GroupNum == detail.GroupNum).CountAsync();
            if (iCount > 0)
                return;

            AddUserLog(_LanguageHandler.GetUserLog("t5"),//开门时段
                string.Format(_LanguageHandler.GetUserLog("r30"), detail.GroupNum));//修改开门时段，时段号：{1}

            await InsertAsync(detail);
            await _FaceDriveDB.UpdateAllDeviceUploadStatus();
        }

        public async Task UpdateTimeGroup(TimeGroupDetail detail)
        {
            AddUserLog(_LanguageHandler.GetUserLog("t5"),//开门时段
                string.Format(_LanguageHandler.GetUserLog("r30"), detail.GroupNum));//修改开门时段，时段号：{1}

            await UpdateAsync(detail);
            await _FaceDriveDB.UpdateAllDeviceUploadStatus();
        }


        public async Task Delete(List<int> numList)
        {
            var db = Context.Set<TimeGroupDetail>();
            await db.Where(x => numList.Contains(x.GroupNum)).ExecuteDeleteAsync();
            await _FaceDriveDB.UpdateAllDeviceUploadStatus();
        }
    }
}
