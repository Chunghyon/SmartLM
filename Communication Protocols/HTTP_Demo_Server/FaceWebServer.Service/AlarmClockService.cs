using FaceWebServer.DB.Table;
using FaceWebServer.DTO;
using FaceWebServer.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaceWebServer.Service
{
    /// <summary>
    /// 闹铃服务
    /// </summary>
    public class AlarmClockService : BaseService, IAlarmClockService
    {
        public IFaceDriveService _FaceDriveDB { get; set; }

        public AlarmClockService(DbContext context) : base(context)
        {

        }

        public PageResult<AlarmClock> Query(BasePageParameter pageDto)
        {
            return QueryPage<AlarmClock, int>(
                null, pageDto.PageSize, pageDto.PageIndex,
                x => x.Num,
                true); ;
        }


        public List<AlarmClock> GetAllList()
        {
            var list = Context.Set<AlarmClock>().ToList();

            return list;
        }

        public int GetNewNum()
        {
            var list = Context.Set<AlarmClock>().Select(x => x.Num).ToHashSet();
            for (int i = 1; i <= 24; i++)
            {
                if (!list.Contains(i))
                {
                    return i;
                }
            }
            return 0;
        }


        public async Task<bool> SaveAlarmClock(AlarmClock alarmClock)
        {

            if (!Context.Set<AlarmClock>().Any(a => a.Num == alarmClock.Num))
            {
                await InsertAsync(alarmClock);
            }
            else
            {
                await UpdateAsync(alarmClock);
            }
            await _FaceDriveDB.UpdateAllDeviceUploadStatus();
            return true;
        }


        public async Task Delete(List<int> numList)
        {
            await Context.Set<AlarmClock>()
                .Where(a => numList.Contains(a.Num))
                .ExecuteDeleteAsync();
            await _FaceDriveDB.UpdateAllDeviceUploadStatus();
        }

        public async Task DeleteAll()
        {
            await Context.Set<AlarmClock>()
                .ExecuteDeleteAsync();
            await _FaceDriveDB.UpdateAllDeviceUploadStatus();
        }
    }
}
