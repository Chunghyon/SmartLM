using FaceWebServer.DB.Table;
using FaceWebServer.Interface;
using FaceWebServer.Utility;
using FaceWebServer.Utility.FilterAttribute;
using FaceWebServer.Utility.JWT;
using FaceWebServer.Utility.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace DeviceProtocolServer.Controllers.User.UserLog
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserLogController : ControllerBase
    {

        private readonly ILogger<UserLogController> _logger;
        private IJWTService _iJWTService = null;
        private IUserService _UserDB;

        public UserLogController(ILogger<UserLogController> logger, IUserService user, IJWTService service)
        {
            _logger = logger;
            _UserDB = user;
            _iJWTService = service;
        }


        /// <summary>
        /// 获取用户表格
        /// </summary>
        /// <param name="par"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetLogs")]
        [TypeFilter(typeof(VerifyActionFilterAttribute))]
        public IActionResult GetLogs([FromBody] UserLogQueryParameter par)
        {

            List<Expression<Func<UserLogModel, bool>>> oWheres = new List<Expression<Func<UserLogModel, bool>>>();
            oWheres.Add(x => x.LogTime >= par.LogTimeBegin && x.LogTime <= par.LogTimeEnd);

            if (!string.IsNullOrWhiteSpace(par.UserName)) oWheres.Add(x => x.UserName.Contains(par.UserName));
            if (!string.IsNullOrWhiteSpace(par.LogType)) oWheres.Add(x => x.LogType.Contains(par.LogType));
            if (!string.IsNullOrWhiteSpace(par.LogPeople)) oWheres.Add(x => x.LogPeople.Contains(par.LogPeople));
            if (!string.IsNullOrWhiteSpace(par.LogDrive)) oWheres.Add(x => x.LogDrive.Contains(par.LogDrive));
            if (!string.IsNullOrWhiteSpace(par.LogDetail)) oWheres.Add(x => x.LogDetail.Contains(par.LogDetail));
            //_logger.LogInformation("获取用户日志列表");
            var logs = _UserDB.QueryPage(
                x => new { x.UserName, x.LogTime, x.LogType, x.LogDrive, x.LogPeople, x.LogDetail },
                oWheres, par.PageSize, par.PageIndex,
                x => x.LogTime,
                par.IsAsc);

            return new JsonResult(new JsonResultModel(logs));
        }

        /// <summary>
        /// 清空出入记录
        /// </summary>
        /// <param name="par"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("ClearLogs")]
        [Authorize]
        public IActionResult ClearLogs()
        {


            //_logger.LogInformation("清空用户操作记录");
            _UserDB.ClearLogs();

            return new JsonResult(new JsonResultModel());
        }

    }
}
