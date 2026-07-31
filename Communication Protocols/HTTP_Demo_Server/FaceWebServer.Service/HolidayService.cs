using FaceWebServer.DB.Table;
using FaceWebServer.DTO;
using FaceWebServer.Interface;
using FaceWebServer.Utility.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaceWebServer.Service
{
    /// <summary>
    /// 节假日服务类
    /// </summary>
    public class HolidayService : BaseService, IHolidayService
    {

        public IFaceDriveService _FaceDriveDB { get; set; }

        public HolidayService(DbContext context) : base(context)
        {
            
        }

        public PageResult<Holiday> Query(BasePageParameter pageDto)
        {
            return QueryPage<Holiday, int>(
                null, pageDto.PageSize, pageDto.PageIndex,
                x => x.Num,
                true); ;
        }


        public List<Holiday> GetAllList()
        {

            var list = Context.Set<Holiday>().ToList();

            return list;
        }


        public int GetNewNum()
        {
            var list = Context.Set<Holiday>().Select(x=>x.Num).ToHashSet();
            for (int i = 1; i <= 32; i++)
            {
                if(!list.Contains(i))
                {
                    return i;
                }
            }
            return 0;
        }

        /// <summary>
        /// 保存节假日
        /// </summary>
        /// <param name="holiday"></param>
        /// <returns></returns>
        public async Task<bool> SaveHoliday(Holiday holiday)
        {
            if(holiday.Num<=0 || holiday.Num >=32) return false;

            //  var model = Context.Find<Holiday>(holiday.Num);
            if (!Context.Set<Holiday>().Any(a => a.Num == holiday.Num))
            {
                await InsertAsync(holiday);
            }
            else
            {
                await UpdateAsync(holiday);
            }
            await _FaceDriveDB.UpdateAllDeviceUploadStatus();
            return true;
        }

        /// <summary>
        /// 删除节假日
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        public async Task Delete(List<int> numList)
        {
            await Context.Set<Holiday>()
                .Where(a => numList.Contains(a.Num))
                .ExecuteDeleteAsync();
            await _FaceDriveDB.UpdateAllDeviceUploadStatus();
        }

        public async Task DeleteAll()
        {
            await Context.Set<Holiday>().ExecuteDeleteAsync();
            await _FaceDriveDB.UpdateAllDeviceUploadStatus();
        }


    }
}
