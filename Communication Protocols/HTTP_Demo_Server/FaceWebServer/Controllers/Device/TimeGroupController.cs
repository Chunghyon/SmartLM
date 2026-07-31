using FaceWebServer.DB.Table;
using FaceWebServer.DTO;
using FaceWebServer.DTO.TimeGroup;
using FaceWebServer.Interface;
using FaceWebServer.Language;
using FaceWebServer.Utility.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace DeviceProtocolServer.Controllers.Device
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TimeGroupController : ControllerBase
    {

        private readonly ILogger<TimeGroupController> _logger;
        private ITimeGroupService _TimeGroupDB;
        private LanguageHandler _LanguageHandler;
        public TimeGroupController(ILogger<TimeGroupController> logger,
            ITimeGroupService door,
            IOptionsSnapshot<LanguageOption> lngopt)
        {
            _logger = logger;
            _TimeGroupDB = door;
            _LanguageHandler = lngopt.Value.GetCurrentLanguageHandler();
        }


        /// <summary>
        /// 获取开门时段列表
        /// </summary>
        /// <param name="par"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetList")]
        public IActionResult GetList([FromBody] BasePageParameter par)
        {
            //_logger.LogInformation("获取开门时段列表");
            var oTimeGroups = _TimeGroupDB.GetAll(par);

            return new JsonResult(new JsonResultModel(oTimeGroups));
        }


        /// <summary>
        /// 获取一个可用的时段号
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("GetNewGroupNum")]
        public IActionResult GetNewGroupNum()
        {
            int iNum = _TimeGroupDB.GetNewGroupNum();
            if (iNum == 0)
            {
                return new JsonResult(new JsonResultModel(101,
                    _LanguageHandler.GetCheckParameterErrorMessage("TimeGroupMaxErr")));// "时段已满，不可新增"
            }

            return new JsonResult(new JsonResultModel(iNum));
        }


        /// <summary>
        /// 更新开门时段
        /// </summary>
        /// <param name="par"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("Add")]
        public IActionResult Add([FromBody] TimeGroupDetail par)
        {
            if (par.GroupNum == 1)
            {
                return new JsonResult(new JsonResultModel(101,
                    _LanguageHandler.GetCheckParameterErrorMessage("r105")));// "时段1不可更改"
            }
            if (par.GroupNum > 64)
            {
                return new JsonResult(new JsonResultModel(102,
                    _LanguageHandler.GetCheckParameterErrorMessage("TimeGroupMaxErr")));// "时段已满，不可新增"
            }

            _TimeGroupDB.AddTimeGroup(par);

            return new JsonResult(new JsonResultModel());
        }


        /// <summary>
        /// 更新开门时段
        /// </summary>
        /// <param name="par"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("Update")]
        public IActionResult Update([FromBody] TimeGroupDetail par)
        {
            if (par.GroupNum == 1)
            {
                return new JsonResult(new JsonResultModel(101,
                    _LanguageHandler.GetCheckParameterErrorMessage("r105")));// "时段1不可更改"
            }
            _TimeGroupDB.UpdateTimeGroup(par);

            return new JsonResult(new JsonResultModel());
        }


        /// <summary>
        /// 更新开门时段
        /// </summary>
        /// <param name="par"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("Delete")]
        public async Task<IActionResult> Delete([FromBody] TimeGroupDeleteRequestDTO par)
        {
            if (par.GroupNums == null) return new JsonResult(new JsonResultModel());
            par.GroupNums.RemoveAll(x => x == 1);

            await _TimeGroupDB.Delete(par.GroupNums);

            return new JsonResult(new JsonResultModel());
        }




        /// <summary>
        /// 恢复默认值
        /// </summary>
        /// <param name="par"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("SetDefault")]
        public async Task<IActionResult> SetDefault()
        {
            await _TimeGroupDB.IniTimeGroupDB();

            return new JsonResult(new JsonResultModel());
        }

        /// <summary>
        /// 添加测试数据
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("AddTestData")]
        public async Task<IActionResult> AddTestData()
        {
            await _TimeGroupDB.IniTimeGroupDB();



            for (int i = 2; i <= 64; i++)
            {
                var detail = new TimeGroupDetail(i);
                detail.Week1 = "00:00-08:59/09:00-12:59/13:00-18:59/19:00-23:59";
                detail.Week2 = detail.Week1;
                detail.Week3 = detail.Week1;
                detail.Week4 = detail.Week1;
                detail.Week5 = detail.Week1;
                detail.Week6 = detail.Week1;
                detail.Week7 = detail.Week1;
                await _TimeGroupDB.InsertAsync(detail);
            }





            return new JsonResult(new JsonResultModel());
        }

    }
}
