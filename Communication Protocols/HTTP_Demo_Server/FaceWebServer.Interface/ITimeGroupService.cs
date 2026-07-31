using FaceWebServer.DB.Table;
using FaceWebServer.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FaceWebServer.Interface
{
    /// <summary>
    /// 开门时段
    /// </summary>
    public interface ITimeGroupService : IBaseService
    {
        /// <summary>
        /// 获取所有开门时段
        /// </summary>
        /// <returns></returns>
        PageResult<TimeGroupDetail> GetAll(BasePageParameter par);

        /// <summary>
        /// 获取所有开门时段
        /// </summary>
        /// <returns></returns>
        List<TimeGroupDetail> GetAll();

        /// <summary>
        /// 初始化开门时段数据库
        /// </summary>
        Task IniTimeGroupDB();


        /// <summary>
        /// 获取一个可用的时段号
        /// </summary>
        /// <returns></returns>
        int GetNewGroupNum();

        /// <summary>
        /// 添加开门时段
        /// </summary>
        /// <param name="detail"></param>
        Task AddTimeGroup(TimeGroupDetail detail);

        /// <summary>
        /// 更新开门时段
        /// </summary>
        /// <param name="detail"></param>
        Task UpdateTimeGroup(TimeGroupDetail detail);

        /// <summary>
        /// 删除开门时段
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        Task Delete(List<int> numList);
    }
}
