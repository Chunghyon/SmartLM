using DoNetDrive.Common.Extensions;
using FaceWebServer.DB.Table;
using FaceWebServer.DTO;
using FaceWebServer.DTO.Device;
using FaceWebServer.DTO.Holiday;
using FaceWebServer.Interface;
using FaceWebServer.Language;
using FaceWebServer.Utility.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Org.BouncyCastle.Crypto.Modes;
using System;
using System.Threading.Tasks;

namespace DeviceProtocolServer.Controllers.Device
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class HolidayController : ControllerBase
    {
        IHolidayService _HolidayService;
        private LanguageHandler _LanguageHandler;
        public HolidayController(IHolidayService holidayService,
             IOptionsSnapshot<LanguageOption> lngopt)
        {
            _HolidayService = holidayService;
            _LanguageHandler = lngopt.Value.GetCurrentLanguageHandler();
        }


        [HttpPost("Query")]
        public IActionResult Query([FromBody] BasePageParameter pageDto)
        {
            return new JsonResult(new JsonResultModel(_HolidayService.Query(pageDto)));
        }

        [HttpPost("GetNewNum")]
        public IActionResult GetNewNum()
        {
            int num = _HolidayService.GetNewNum();
            if (num == 0)
            {
                return new JsonResult(new JsonResultModel(101,
                    _LanguageHandler.GetCheckParameterErrorMessage("HolidayMaxErr")));//节假日已满
            }


            return new JsonResult(new JsonResultModel(num));

        }

        [HttpPost("Add")]
        public async Task<IActionResult> AddHoliday([FromBody] Holiday holiday)
        {
            bool ret = await _HolidayService.SaveHoliday(holiday);
            return new JsonResult(new JsonResultModel(ret));
        }


        [HttpPost("Delete")]
        public async Task<IActionResult> Delete([FromBody] HolidayDeleteRequestDTO deleteDto)
        {
            if (deleteDto != null)
            {
                await _HolidayService.Delete(deleteDto.Nums);
            }
            return new JsonResult(new JsonResultModel());
        }

        [HttpPost("DeleteAll")]
        public async Task<IActionResult> DeleteAll()
        {

            await _HolidayService.DeleteAll();

            return new JsonResult(new JsonResultModel());
        }


        [HttpPost("AddTestData")]
        public async Task<IActionResult> AddTestData()
        {

            await _HolidayService.DeleteAll();


            var date = DateTime.Now;
            for (int i = 1; i <= 32; i++)
            {
                var h = new Holiday()
                {
                    Num = i,
                    Date = date.ToDateStr(),
                    HolidayType = 1,
                    Cycle = 1
                };
                await _HolidayService.InsertAsync(h);
                date= date.AddDays(1);


            }


            return new JsonResult(new JsonResultModel());
        }

    }
}
