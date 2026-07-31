using FaceWebServer.DTO.Record;
using FaceWebServer.Interface;
using FaceWebServer.Utility.FilterAttribute;
using FaceWebServer.Utility.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Threading.Tasks;

namespace DeviceProtocolServer.Controllers.Report
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeviceReportController : ControllerBase
    {

        private readonly ILogger<DeviceReportController> _logger;
        private IRecordService _RecordDB;

        public DeviceReportController(ILogger<DeviceReportController> logger,
            IRecordService door)
        {
            _logger = logger;
            _RecordDB = door;
        }


        /// <summary>
        /// 获取设备出入记录
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("GetIdentifyRecord")]
        [Authorize]
        //[TypeFilter(typeof(VerifyActionFilterAttribute))]
        public IActionResult GetIdentifyRecord([FromBody] IdentifyReportQueryDTO par)
        {
            return new JsonResult(new JsonResultModel(_RecordDB.QueryIdentifyRecord(par)));
        }

        /// <summary>
        /// 获取设备出入记录
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("GetSystemRecord")]
        [Authorize]
        //[TypeFilter(typeof(VerifyActionFilterAttribute))]
        public IActionResult GetSystemRecord([FromBody] SystemReportQueryDTO par)
        {
            return new JsonResult(new JsonResultModel(_RecordDB.QuerySystemRecord(par)));
        }


        /// <summary>
        /// 清空出入记录
        /// </summary>
        /// <param name="par"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("ClearIdentifyRecord")]
        [Authorize]
        public async Task<IActionResult> ClearIdentifyRecord()
        {
            //_logger.LogInformation("清空出入记录");

            await Task.Run(DeleteAllRecordImage);
            await _RecordDB.ClearIdentifyRecord();

            return new JsonResult(new JsonResultModel());
        }

        /// 清空系统记录
        /// </summary>
        /// <param name="par"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("ClearSystemRecord")]
        [Authorize]
        public IActionResult ClearSystemRecord()
        {
            _RecordDB.ClearSystemRecord();

            return new JsonResult(new JsonResultModel());
        }

        /// <summary>
        /// 删除所有记录图片
        /// </summary>
        private void DeleteAllRecordImage()
        {
            try
            {
                string sPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "RecordImage");
                if (!Directory.Exists(sPath))
                {

                    return;
                }
                var sDirs = Directory.GetDirectories(sPath);
                foreach (var sDir in sDirs)
                {
                    Directory.Delete(sDir, true);
                }
            }
            catch (Exception)
            {

            }
        }


    }
}
