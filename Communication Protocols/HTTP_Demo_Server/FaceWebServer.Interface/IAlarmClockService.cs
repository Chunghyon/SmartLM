using FaceWebServer.DB.Table;
using FaceWebServer.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaceWebServer.Interface
{
    /// <summary>
    /// 闹铃服务接口
    /// </summary>
    public interface IAlarmClockService : IBaseService
    {
        /// <summary>
        /// 获取所有闹铃
        /// </summary>
        /// <returns></returns>
        PageResult<AlarmClock> Query(BasePageParameter pageDto);

        /// <summary>
        /// 获取所有闹铃
        /// </summary>
        /// <returns></returns>
        List<AlarmClock> GetAllList();

        /// <summary>
        /// 获取一个新闹铃的编号
        /// </summary>
        /// <returns></returns>
        int GetNewNum();


        /// <summary>
        /// 保存闹铃
        /// </summary>
        /// <param name="alarmClock"></param>
        /// <returns></returns>
        Task<bool> SaveAlarmClock(AlarmClock alarmClock);


        /// <summary>
        /// 删除闹铃
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        Task Delete(List<int> numList);

        /// <summary>
        /// 清空所有闹铃
        /// </summary>
        Task DeleteAll();
    }
}
