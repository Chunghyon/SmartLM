using FaceWebServer.DB.Table;
using FaceWebServer.DTO.AlarmClock;
using FaceWebServer.DTO;
using FaceWebServer.Interface;
using FaceWebServer.Language;
using FaceWebServer.Utility.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using System;
using Microsoft.AspNetCore.Authorization;

namespace DeviceProtocolServer.Controllers.Device
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AlarmClockController : ControllerBase
    {
        IAlarmClockService _AlarmClockService;
        private LanguageHandler _LanguageHandler;
        public AlarmClockController(IAlarmClockService AlarmClockService,
             IOptionsSnapshot<LanguageOption> lngopt)
        {
            _AlarmClockService = AlarmClockService;
            _LanguageHandler = lngopt.Value.GetCurrentLanguageHandler();
        }


        [HttpPost("Query")]
        public IActionResult Query([FromBody] BasePageParameter pageDto)
        {
            return new JsonResult(new JsonResultModel(_AlarmClockService.Query(pageDto)));
        }

        [HttpPost("GetNewNum")]
        public IActionResult GetNewNum()
        {
            int num = _AlarmClockService.GetNewNum();
            if (num == 0)
            {
                return new JsonResult(new JsonResultModel(101,
                    _LanguageHandler.GetCheckParameterErrorMessage("AlarmClockMaxErr")));//闹铃已满
            }


            return new JsonResult(new JsonResultModel(num));

        }

        [HttpPost("Add")]
        public async Task<IActionResult> AddAlarmClock([FromBody] AlarmClock AlarmClock)
        {
            bool ret = await _AlarmClockService.SaveAlarmClock(AlarmClock);
            return new JsonResult(new JsonResultModel(ret));
        }


        [HttpPost("Delete")]
        public async Task<IActionResult> Delete([FromBody] AlarmClockDeleteRequestDTO deleteDto)
        {
            if (deleteDto != null)
            {
                await _AlarmClockService.Delete(deleteDto.Nums);
            }
            return new JsonResult(new JsonResultModel());
        }

        [HttpPost("DeleteAll")]
        public async Task<IActionResult> DeleteAll()
        {

            await _AlarmClockService.DeleteAll();

            return new JsonResult(new JsonResultModel());
        }


        [HttpPost("AddTestData")]
        public async Task<IActionResult> AddTestData()
        {

            await _AlarmClockService.DeleteAll();


            var date = DateTime.Now.Date;
            for (int i = 1; i <= 24; i++)
            {
                var h = new AlarmClock()
                {
                    Num = i,
                    Date = date,
                    Times = 10
                };
                await _AlarmClockService.InsertAsync(h);
                date = date.AddHours(1);


            }


            return new JsonResult(new JsonResultModel());
        }
    }
}
