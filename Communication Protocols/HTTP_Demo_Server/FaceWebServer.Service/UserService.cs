using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FaceWebServer.DB.Table;
using FaceWebServer.Interface;
using FaceWebServer.Language;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace FaceWebServer.Service
{
    /// <summary>
    /// 网址管理员操作服务
    /// </summary>
    public class UserService : BaseService, IUserService
    {


        private LanguageHandler _LanguageHandler;
        public UserService(DbContext context, IOptionsSnapshot<LanguageOption> lngopt) : base(context)
        {
            _LanguageHandler = lngopt.Value.GetCurrentLanguageHandler();
        }

        public UserDetail GetCurrentUser()
        {
            return CurrentUser;
        }

        public DbSet<UserDetail> GetUserDBSet()
        {
            return this.Context.Set<UserDetail>();
        }



        public UserDetail UserLogin(string username, string password)
        {
            var user = Query<UserDetail>(x => x.UserName == username).AsTracking().FirstOrDefault();
            if (user == null)
            {
                throw new ServiceException(201,
                    _LanguageHandler.GetCheckParameterErrorMessage("r131"));// "没有找到此用户！
            }
            if (user.UserName.Equals(username) && user.UserPassword.Equals(password))//应该数据库
            {
                user.LogTime = DateTime.Now;
                user.OnlineTime = DateTime.Now;

                CurrentUser = user;
                AddUserLog(_LanguageHandler.GetUserLog("t7"),//"登陆",
                     _LanguageHandler.GetUserLog("r36"));//"通过UI界面登陆"
                Commit();//更新到数据库
                return user;
            }
            else
            {
                throw new ServiceException(202,
                    _LanguageHandler.GetCheckParameterErrorMessage("r132"));//"用户名或密码错误！
            }
        }


        /// <summary>
        /// 更新用户在线时间
        /// </summary>
        /// <returns></returns>
        public UserDetail UpdateUserOnlineTime(int id)
        {
            var user = Find<UserDetail>(id);

            if (user != null)
            {
                user.OnlineTime = DateTime.Now;
                this.Commit();
                return user;
            }
            else
            {
                throw new ServiceException(201,
                    _LanguageHandler.GetCheckParameterErrorMessage("r133"));//"用户已失效！"
            }
        }

        public void ClearLogs()
        {
            Context.Set<UserLogModel>().ExecuteDelete();

            //AddUserLog("操作员管理", "清空操作员日志");
            AddUserLog(_LanguageHandler.GetUserLog("t7"),//"登陆",
                    _LanguageHandler.GetUserLog("r37"));//清空操作员日志
            Commit();
        }



        public void AddUser(UserDetail user)
        {
            //添加管理员：{user.UserName},身份：{user.Role},电话：{user.Phone}
            string sLog = _LanguageHandler.GetUserLog("r38");
            AddUserLog(_LanguageHandler.GetUserLog("t7"),//"登陆",
                    string.Format(sLog, user.UserName, user.Role, user.Phone));
            Insert(user);
        }


        public bool UpdateUser(UserDetail newuser)
        {
            var db = this.Context.Set<UserDetail>();
            UserDetail User = db.Find(newuser.UserID);
            if (User == null) return false;

            User.Role = newuser.Role;
            User.Phone = newuser.Phone;
            if (!string.IsNullOrWhiteSpace(newuser.UserPassword))
                User.UserPassword = newuser.UserPassword;

            //修改管理员：{user.UserName},身份：{user.Role},电话：{user.Phone}
            string sLog = _LanguageHandler.GetUserLog("r38");
            AddUserLog(_LanguageHandler.GetUserLog("t7"),
                    string.Format(sLog, User.UserName, User.Role, User.Phone));
            Commit();
            return true;
        }


        public void DeleteUsers(List<int> userids)
        {
            var db = this.Context.Set<UserDetail>();
            var users = from x in db
                        where userids.Contains(x.UserID)
                        select new UserDetail()
                        {
                            UserID = x.UserID,
                            UserName = x.UserName,
                            Role = x.Role,
                            Phone = x.Phone
                        };
            users = users.AsTracking();

            string sLogTitle = _LanguageHandler.GetUserLog("t7");//操作员管理
            string sLog = _LanguageHandler.GetUserLog("r40");//删除操作员：{0},身份：{1},电话：{2}
            foreach (var user in users)
            {
                AddUserLog(sLogTitle,
                    string.Format(sLog, user.UserName, user.Role, user.Phone));

            }
            db.RemoveRange(users);
            //var Delusers = par.UserIDs.Select(p => new UserDetail() { UserID = p });

            //Delete(Delusers);
            Commit();
        }

    }
}
