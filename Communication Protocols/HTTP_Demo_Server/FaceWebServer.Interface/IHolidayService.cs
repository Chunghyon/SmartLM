using FaceWebServer.DB.Table;
using FaceWebServer.DTO;
using FaceWebServer.Utility.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaceWebServer.Interface
{
    /// <summary>
    /// 节假日服务接口
    /// </summary>
    public interface IHolidayService : IBaseService
    {
        /// <summary>
        /// 获取所有节假日
        /// </summary>
        /// <returns></returns>
        PageResult<Holiday> Query(BasePageParameter pageDto);


        /// <summary>
        /// 获取所有节假日
        /// </summary>
        /// <returns></returns>
        List<Holiday> GetAllList();


        /// <summary>
        /// 获取一个新节假日的编号
        /// </summary>
        /// <returns></returns>
        int GetNewNum();


        /// <summary>
        /// 保存节假日
        /// </summary>
        /// <param name="holiday"></param>
        /// <returns></returns>
        Task<bool> SaveHoliday(Holiday holiday);


        /// <summary>
        /// 删除节假日
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        Task Delete(List<int> numList);

        /// <summary>
        /// 清空所有节假日
        /// </summary>
        Task DeleteAll();
    }
}
