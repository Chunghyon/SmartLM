using FaceWebServer.DB.Table;
using FaceWebServer.DTO.IOLog;
using FaceWebServer.Interface;
using FaceWebServer.Utility;
using FaceWebServer.Utility.FilterAttribute;
using FaceWebServer.Utility.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace DeviceProtocolServer.Controllers.Report
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConnectIOLogController : ControllerBase
    {

        private readonly ILogger<ConnectIOLogController> _logger;
        private IConnectIOLogService _LogDB;

        public ConnectIOLogController(ILogger<ConnectIOLogController> logger,
            IConnectIOLogService db)
        {
            _logger = logger;
            _LogDB = db;
        }


        /// <summary>
        /// 获取日志表
        /// </summary>
        /// <param name="par"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetLogs")]
        [Authorize]
        public IActionResult GetReport([FromBody] ConnectIOLogQueryDTO par)
        {


            //_logger.LogInformation("获取通讯日志列表");
            var reports = _LogDB.Query(par);

            return new JsonResult(new JsonResultModel(reports));
        }


        /// <summary>
        /// 删除日志
        /// </summary>
        /// <param name="par"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("DeleteLogs")]
        [Authorize]
        public IActionResult DeleteLogs()
        {
            //_logger.LogInformation("清空通讯日志列表");
            var reports = _LogDB.ClearLog();

            return new JsonResult(new JsonResultModel());
        }
    }
}
