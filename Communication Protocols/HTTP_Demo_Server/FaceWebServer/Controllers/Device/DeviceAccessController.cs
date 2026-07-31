using FaceWebServer.DB.Table;
using FaceWebServer.DTO.Access;
using FaceWebServer.DTO.Cache;
using FaceWebServer.Interface;
using FaceWebServer.Language;
using FaceWebServer.Utility.FilterAttribute;
using FaceWebServer.Utility.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DeviceProtocolServer.Controllers.Device
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class DeviceAccessController : Controller
    {

        private readonly ILogger<DeviceAccessController> _logger;

        private IDeviceAccessService _AccessDB;
        private ICacheService _Cache;

        private LanguageHandler _LanguageHandler;
        public DeviceAccessController(ILogger<DeviceAccessController> logger,
            IDeviceAccessService door,
            ICacheService cache,
            IOptionsSnapshot<LanguageOption> lngopt)
        {
            _logger = logger;
            _AccessDB = door;
            _Cache = cache;
            _LanguageHandler = lngopt.Value.GetCurrentLanguageHandler();


        }


        /// <summary>
        /// 获取人员权限表
        /// </summary>
        /// <param name="par"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("Query")]
        //[TypeFilter(typeof(VerifyActionFilterAttribute))]
        public IActionResult Query([FromBody] DeviceAccessQueryDTO par)
        {
            var devices = _AccessDB.Query(par);

            return new JsonResult(new JsonResultModel(devices));
        }

        /// <summary>
        /// 根据权限ID获取权限详情
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("GetAccessDetail/{AccessID}")]
        public IActionResult GetAccessDetail(int AccessID)
        {
            var detail = _AccessDB.GetAccessDetail(AccessID);
            FaceWebServer.DB.Table.People people = null;
            CacheDeviceDTO device = null;

            if (detail != null)
            {
                var deviceMap = _Cache.GetDeviceDictionary();
                if (deviceMap.ContainsKey(detail.DeviceID))
                {
                    device = deviceMap[detail.DeviceID];
                }

                var peopleMap = _Cache.GetPeopleDictionary();
                if (peopleMap.ContainsKey(detail.PeopleID))
                {
                    people = peopleMap[detail.PeopleID];
                }
            }
            else
            {
                return new JsonResult(new JsonResultModel(1, "no find"));
            }

            if (people == null)
            {
                return new JsonResult(new JsonResultModel(new
                {
                    device.SN,
                    device.DeviceName,
                    device.Protocol,

                    detail.UserID,

                    Access = detail
                }));
            }
            else
            {
                return new JsonResult(new JsonResultModel(new
                {
                    device.SN,
                    device.DeviceName,
                    device.Protocol,

                    people.UserID,
                    people.Name,
                    people.Job,
                    people.Department,
                    people.QRCode,
                    people.PhotoLen,
                    people.Photo,


                    Access = detail
                }));
            }
        }



        [HttpPost]
        [Route("AddAccess")]
        //[TypeFilter(typeof(VerifyActionFilterAttribute))]
        public async Task<IActionResult> AddAccess([FromBody] DeviceAccessAddDTO par)
        {

            if (par.DeviceIDs == null)
            {
                return new JsonResult(new JsonResultModel(101,
                    _LanguageHandler.GetCheckParameterErrorMessage("r25")));//"未选择需要操作的设备"
            }
            if (par.DeviceIDs.Count == 0)
            {
                return new JsonResult(new JsonResultModel(101,
                    _LanguageHandler.GetCheckParameterErrorMessage("r25")));//"未选择需要操作的设备"
            }

            if (par.PeopleIDs == null)
            {
                return new JsonResult(new JsonResultModel(102,
                    _LanguageHandler.GetCheckParameterErrorMessage("r26")));//"未选择需要操作的人员"
            }
            if (par.PeopleIDs.Count == 0)
            {
                return new JsonResult(new JsonResultModel(102,
                    _LanguageHandler.GetCheckParameterErrorMessage("r26")));//"未选择需要操作的人员"
            }

            await _AccessDB.AddAccess(par);
            return new JsonResult(new JsonResultModel());
        }

        [HttpPost]
        [Route("AddAccess_AllPeople")]
        //[TypeFilter(typeof(VerifyActionFilterAttribute))]
        public async Task<IActionResult> AddAccess_AllPeople([FromBody] DeviceAccessAddDTO par)
        {

            if (par.DeviceIDs == null)
            {
                return new JsonResult(new JsonResultModel(101,
                    _LanguageHandler.GetCheckParameterErrorMessage("r25")));//"未选择需要操作的设备"
            }
            if (par.DeviceIDs.Count == 0)
            {
                return new JsonResult(new JsonResultModel(101,
                    _LanguageHandler.GetCheckParameterErrorMessage("r25")));//"未选择需要操作的设备"
            }
            await _AccessDB.AddAccess_ALLPeople(par);
            return new JsonResult(new JsonResultModel());
        }


        [HttpPost]
        [Route("DeleteAccess")]
        public async Task<IActionResult> DeleteAccess([FromBody] DeviceAccessDeleteDTO par)
        {


            if (par.DeviceIDs == null)
            {
                return new JsonResult(new JsonResultModel(101,
                    _LanguageHandler.GetCheckParameterErrorMessage("r25")));//"未选择需要操作的设备"
            }
            if (par.DeviceIDs.Count == 0)
            {
                return new JsonResult(new JsonResultModel(101,
                    _LanguageHandler.GetCheckParameterErrorMessage("r25")));//"未选择需要操作的设备"
            }

            if (par.PeopleIDs == null)
            {
                return new JsonResult(new JsonResultModel(102,
                    _LanguageHandler.GetCheckParameterErrorMessage("r26")));//"未选择需要操作的人员"
            }
            if (par.PeopleIDs.Count == 0)
            {
                return new JsonResult(new JsonResultModel(102,
                    _LanguageHandler.GetCheckParameterErrorMessage("r26")));//"未选择需要操作的人员"
            }

            await _AccessDB.DeleteAccess(par);
            return new JsonResult(new JsonResultModel());
        }
        [HttpPost]
        [Route("DeleteAccess_AllPeople")]
        //[TypeFilter(typeof(VerifyActionFilterAttribute))]
        public async Task<IActionResult> DeleteAccess_AllPeople([FromBody] DeviceAccessDeleteDTO par)
        {

            if (par.DeviceIDs == null)
            {
                return new JsonResult(new JsonResultModel(101,
                    _LanguageHandler.GetCheckParameterErrorMessage("r25")));//"未选择需要操作的设备"
            }
            if (par.DeviceIDs.Count == 0)
            {
                return new JsonResult(new JsonResultModel(101,
                    _LanguageHandler.GetCheckParameterErrorMessage("r25")));//"未选择需要操作的设备"
            }
            await _AccessDB.DeleteAccess_ALLPeople(par);
            return new JsonResult(new JsonResultModel());
        }

        [HttpPost]
        [Route("Delete")]
        public async Task<IActionResult> Delete([FromBody] DeviceAccessRequestIDListDTO par)
        {


            if (par.AccessIDs == null)
            {
                return new JsonResult(new JsonResultModel(101,
                    _LanguageHandler.GetCheckParameterErrorMessage("r25")));//"未选择需要操作的设备"
            }
            if (par.AccessIDs.Count == 0)
            {
                return new JsonResult(new JsonResultModel(101,
                    _LanguageHandler.GetCheckParameterErrorMessage("r25")));//"未选择需要操作的设备"
            }


            await _AccessDB.Delete(par.AccessIDs);
            return new JsonResult(new JsonResultModel());
        }




        /// <summary>
        /// 更新单个开门权限
        /// </summary>
        [HttpPost]
        [Route("UpdateAccess")]
        //[TypeFilter(typeof(VerifyActionFilterAttribute))]
        public IActionResult Update([FromBody] PeopleAccessDetail par)
        {

            _AccessDB.Update(par);
            return new JsonResult(new JsonResultModel());
        }


        /// <summary>
        /// 清空所有开门权限
        /// </summary>
        /// <param name="par"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("ClearAccess")]
        public async Task<IActionResult> ClearAccess()
        {
            //_logger.LogInformation("清空所有开门权限");
            await _AccessDB.ClearAccess();

            return new JsonResult(new JsonResultModel());
        }





        /// <summary>
        /// 使指定的权限重新上传
        /// </summary>
        [HttpPost]
        [Route("Reupload")]
        public async Task<IActionResult> Reupload([FromBody] DeviceAccessRequestIDListDTO dto)
        {
            if (dto.AccessIDs == null)
            {
                return new JsonResult(new JsonResultModel(101,
                    _LanguageHandler.GetCheckParameterErrorMessage("r28")));//"未选择需要重新上传的权限"
            }
            if (dto.AccessIDs.Count == 0)
            {
                return new JsonResult(new JsonResultModel(101,
                    _LanguageHandler.GetCheckParameterErrorMessage("r28")));//"未选择需要重新上传的权限"
            }

            await _AccessDB.Reupload(dto.AccessIDs);
            return new JsonResult(new JsonResultModel());
        }


        /// <summary>
        /// 使全部的权限重新上传
        /// </summary>
        [HttpPost]
        [Route("ReuploadAll")]
        public async Task<IActionResult> ReuploadAll()
        {
            await _AccessDB.ReuploadAll();
            return new JsonResult(new JsonResultModel());
        }

        /// <summary>
        /// 将指定查询条件的记录重新上传
        /// </summary>
        /// <param name="par"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("ReuploadFilterAll")]
        public async Task<IActionResult> ReuploadFilterAllAsync([FromBody] DeviceAccessQueryDTO par)
        {
            await _AccessDB.ReuploadFilterAllAsync(par);
            return new JsonResult(new JsonResultModel());
        }
        

    }
}
