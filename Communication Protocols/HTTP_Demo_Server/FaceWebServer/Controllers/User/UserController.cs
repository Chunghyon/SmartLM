using FaceWebServer.Interface;
using FaceWebServer.Utility;
using FaceWebServer.Language;
using FaceWebServer.Utility.FilterAttribute;
using FaceWebServer.Utility.JWT;
using FaceWebServer.Utility.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using FaceWebServer.DB.Table;
using DeviceProtocolServer.Controllers.User.UserLog;

namespace DeviceProtocolServer.Controllers.User
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {

        private readonly ILogger<UserLogController> _logger;
        private IJWTService _iJWTService = null;
        private IUserService _UserDB;
        public IMemoryCache _Cache;
        private LanguageHandler _LanguageHandler;

        public UserController(ILogger<UserLogController> logger, IUserService user,
            IJWTService service, IMemoryCache memory,
            IOptionsSnapshot<LanguageOption> lngopt)
        {
            _logger = logger;
            _UserDB = user;
            _iJWTService = service;
            _Cache = memory;
            _LanguageHandler = lngopt.Value.GetCurrentLanguageHandler();
        }

        /// <summary>
        /// 获取用户身份,并验证此用户是否已失效
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetCurrentUser")]
        [Authorize]
        public IActionResult GetCurrentUser()
        {
            var diclist = new Dictionary<string, string>();
            HashSet<string> sKeys = new(new string[] { "name", "role", "iat", "ID", "exp" });
            var oItems = HttpContext.User.Identities.First().Claims.Where(x => sKeys.Contains(x.Type));
            foreach (var item in oItems)
            {
                diclist.Add($"{item.Type}", $"{item.Value}");
            }

            int iID = GetUserID();
            var user = _UserDB.Find<UserDetail>(iID);
            if (user == null)
            {
                return new JsonResult(new JsonResultModel(401,
                    _LanguageHandler.GetCheckParameterErrorMessage("r118")));// "用户已失效！"
            }
            return new JsonResult(new JsonResultModel(diclist));
        }

        /// <summary>
        /// 登陆验证
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="captcha"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("Login")]
        public IActionResult Login([FromForm] string username, [FromForm] string password)
        {
            //#region 验证验证码
            //string codeID;
            //if (!this.Request.Cookies.ContainsKey("CaptchaID"))
            //{
            //    return new JsonResult(new JsonResultModel(101,
            //        _LanguageHandler.GetCheckParameterErrorMessage("r119")));// "验证码已过期!"
            //}
            //codeID = this.Request.Cookies["CaptchaID"];
            //string code = _Cache.Get<string>(codeID);
            //if (string.IsNullOrEmpty(code))
            //{
            //    return new JsonResult(new JsonResultModel(101,
            //        _LanguageHandler.GetCheckParameterErrorMessage("r119")));// "验证码已过期!"
            //}

            //if (string.IsNullOrWhiteSpace(captcha))
            //{
            //    return new JsonResult(new JsonResultModel(102,
            //        _LanguageHandler.GetCheckParameterErrorMessage("r120")));// "请输入验证码!"
            //}

            //if (!code.ToLower().Equals(captcha.ToLower()))
            //{
            //    return new JsonResult(new JsonResultModel(103,
            //         _LanguageHandler.GetCheckParameterErrorMessage("r121")));//"验证码不正确!"
            //}
            //#endregion

            #region 用户验证
            try
            {
                var oUser = _UserDB.UserLogin(username, password);
                string token = _iJWTService.GetToken(oUser);
                return new JsonResult(new JsonResultModel(token));
            }
            catch (ServiceException sex)
            {

                return new JsonResult(new JsonResultModel(sex.ErrorCode, sex.Message));
            }
            #endregion

        }


        /// <summary>
        /// 刷新用户在线时间，并更新Token
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("Refresh_Login")]
        [Authorize]
        public IActionResult Refresh_Login()
        {
            var iID = GetUserID();
            try
            {
                var user = _UserDB.UpdateUserOnlineTime(iID);
                string token = _iJWTService.GetToken(user);
                return new JsonResult(new JsonResultModel(token));
            }
            catch (Exception)
            {
                return new JsonResult(new JsonResultModel(101,
                    _LanguageHandler.GetCheckParameterErrorMessage("r118")));// "用户已失效！"
            }
        }

        /// <summary>
        /// 获取当前登陆的用户ID
        /// </summary>
        /// <returns></returns>
        protected int GetUserID()
        {
            try
            {
                var ident = HttpContext.User.Identities.First();
                var oItem = ident.Claims.Where(x => x.Type.Equals("ID")).First();
                var sID = oItem.Value;
                return int.Parse(sID);

            }
            catch (Exception)
            {

                return 0;//发生错误时，没有ID
            }

        }


        ///// <summary>
        ///// 获取验证码
        ///// </summary>
        ///// <returns></returns>
        //[HttpGet]
        //[Route("captcha")]
        //public IActionResult CreateCaptchaImage()
        //{
        //    string code = "";
        //    string codeID = Guid.NewGuid().ToString("N");
        //    System.IO.MemoryStream ms = VierificationCodeServices.Create(out code);
        //    Response.Clear();

        //    Response.Cookies.Append("CaptchaID", codeID, new CookieOptions()
        //    {
        //        HttpOnly = true,
        //        Expires = new DateTimeOffset(DateTime.Now.AddMinutes(5))
        //    }); ;

        //    _Cache.Set(codeID, code, TimeSpan.FromMinutes(5));
        //    return File(ms.ToArray(), @"image/png");
        //}


        /// <summary>
        /// 获取用户表格
        /// </summary>
        /// <param name="par"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetUserTable")]
        [Authorize]
        [TypeFilter(typeof(VerifyActionFilterAttribute))]
        public IActionResult GetUserTable([FromBody] UserQueryParameter par)
        {

            List<Expression<Func<UserDetail, bool>>> oWheres = new List<Expression<Func<UserDetail, bool>>>();
            if (!string.IsNullOrWhiteSpace(par.UserName)) oWheres.Add(x => x.UserName.Contains(par.UserName));
            if (!string.IsNullOrWhiteSpace(par.Phone)) oWheres.Add(x => x.Phone.Contains(par.Phone));
            //_logger.LogInformation("获取用户列表");
            var users = _UserDB.QueryPage(
                x => new { x.UserID, x.UserName, x.Phone, x.Role, x.OnlineTime },
                oWheres, par.PageSize, par.PageIndex,
                x => x.UserName,
                par.IsAsc);

            return new JsonResult(new JsonResultModel(users));
        }


        /// <summary>
        /// 更新管理员
        /// </summary>
        /// <param name="par"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("Update")]
        [Authorize]
        [TypeFilter(typeof(VerifyActionFilterAttribute))]
        public IActionResult Update([FromBody] UserUpdateParameter par)
        {
            var User = new UserDetail();
            User.UserID = par.UserID;
            User.Role = par.Role;
            User.Phone = par.Phone;
            User.UserPassword = par.UserPassword;

            if (_UserDB.UpdateUser(User))
            {
                return new JsonResult(new JsonResultModel(new { User.UserID }));
            }
            else
                return new JsonResult(new JsonResultModel(101,
                    _LanguageHandler.GetCheckParameterErrorMessage("r118")));// "用户不存在"


        }

        /// <summary>
        /// 检查用户名是否重复
        /// </summary>
        /// <param name="UserName"></param>
        /// <returns></returns>
        private JsonResultModel CheckUserName(string UserName)
        {
            //_logger.LogInformation("添加用户");

            var users = _UserDB.Query<UserDetail>(x => x.UserName.Equals(UserName));
            if (users.Count() > 0)
            {
                return new JsonResultModel(101,
                    _LanguageHandler.GetCheckParameterErrorMessage("r118"))//"用户已存在"
                { Content = users.Select(x => new UserDetail() { UserID = x.UserID, UserName = x.UserName }).ToList() };
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 添加用户
        /// </summary>
        /// <param name="par"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("Add")]
        [Authorize]
        [TypeFilter(typeof(VerifyActionFilterAttribute))]
        public IActionResult Add([FromBody] UserAddParameter par)
        {
            var chkRst = CheckUserName(par.UserName);
            if (chkRst != null) return new JsonResult(chkRst);


            UserDetail user = new UserDetail()
            {
                UserName = par.UserName,
                UserPassword = par.UserPassword,
                Role = par.Role,
                Phone = par.Phone,
                LogTime = DateTime.MinValue,
                OnlineTime = DateTime.MinValue
            };


            _UserDB.AddUser(user);
            return new JsonResult(new JsonResultModel(new { user.UserID }));
        }


        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="par"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("Delete")]
        [Authorize]
        [TypeFilter(typeof(VerifyActionFilterAttribute))]
        public IActionResult Delete([FromBody] UserDeleteParameter par)
        {

            if (par.UserIDs == null)
            {
                return new JsonResult(new JsonResultModel(101,
                    _LanguageHandler.GetCheckParameterErrorMessage("r122")));//"未选择需要删除的用户"
            }
            if (par.UserIDs.Count == 0)
            {
                return new JsonResult(new JsonResultModel(101,
                    _LanguageHandler.GetCheckParameterErrorMessage("r122")));//"未选择需要删除的用户"
            }
            _UserDB.DeleteUsers(par.UserIDs);
            return new JsonResult(new JsonResultModel(par));
        }

    }
}
