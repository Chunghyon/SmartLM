using FaceWebServer.DB.Table;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaceWebServer.Interface
{
    /// <summary>
    /// 网址用户服务器
    /// </summary>
    public interface IUserService : IBaseService
    {
        /// <summary>
        /// 获取当前已登录用户
        /// </summary>
        /// <returns></returns>
        UserDetail GetCurrentUser();

        DbSet<UserDetail> GetUserDBSet();
        DbSet<UserLogModel> GetUserLogDBSet();

        /// <summary>
        /// 用户登录
        /// </summary>
        /// <returns>登录凭证</returns>
        UserDetail UserLogin(string username,string password);

        /// <summary>
        /// 更新用户在线时间
        /// </summary>
        /// <returns></returns>
        UserDetail UpdateUserOnlineTime(int id);

        /// <summary>
        /// 清空操作员日志
        /// </summary>
        void ClearLogs();

        /// <summary>
        /// 添加新操作员
        /// </summary>
        void AddUser(UserDetail user);

        /// <summary>
        /// 更新操作员
        /// </summary>
        bool UpdateUser(UserDetail user);

        /// <summary>
        /// 删除操作员
        /// </summary>
        void DeleteUsers(List<int> users);
    }
}
