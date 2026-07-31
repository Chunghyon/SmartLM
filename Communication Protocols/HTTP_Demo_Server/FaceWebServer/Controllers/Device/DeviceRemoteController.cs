using FaceWebServer.DB.Table;
using FaceWebServer.DTO.Access;
using FaceWebServer.DTO.Config;
using FaceWebServer.DTO.HTTPv2_Protocol;
using FaceWebServer.DTO.Remote;
using FaceWebServer.Interface;
using FaceWebServer.Language;
using FaceWebServer.Utility;
using FaceWebServer.Utility.FilterAttribute;
using FaceWebServer.Utility.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using NPOI.XWPF.UserModel;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace DeviceProtocolServer.Controllers.Device
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class DeviceRemoteController : Controller
    {

        private readonly ILogger<DeviceRemoteController> _logger;

        private IDeviceRemoteService _RemoteDB;
        private LanguageHandler _LanguageHandler;
        private IDeviceAccessService _AccessDB;
        public DeviceRemoteController(ILogger<DeviceRemoteController> logger,
            IDeviceRemoteService door,
            IDeviceAccessService accessService,
            IOptionsSnapshot<LanguageOption> lngopt)
        {
            _logger = logger;
            _RemoteDB = door;
            _AccessDB = accessService;
            _LanguageHandler = lngopt.Value.GetCurrentLanguageHandler();


        }

        /// <summary>
        /// 获取远程操作任务列表
        /// </summary>
        /// <param name="par"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetList")]

        //[TypeFilter(typeof(VerifyActionFilterAttribute))]
        public IActionResult GetList([FromBody] RemoteTaskQueryDTO par)
        {
            //_logger.LogInformation("获取远程任务列表");
            var result = _RemoteDB.Query(par);

            return new JsonResult(new JsonResultModel(result));
        }

        [HttpPost]
        [Route("Add")]
        public async Task<IActionResult> Add([FromBody] RemoteTaskAddDTO par)
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
            if (par.TaskType == FaceWebServer.DB.Table.RemoteTypeEnum.PushSystemFile)
            {
                List<PushSystemFileDTO> files = new List<PushSystemFileDTO>(){
                    new PushSystemFileDTO("screen_1.jpg", "", 1, 1),
                    new PushSystemFileDTO("screen_2.jpg", "", 1, 2),
                    new PushSystemFileDTO("screen_3.jpg", "", 1, 3),
                    new PushSystemFileDTO("screen_4.jpg", "", 1, 4),
                    new PushSystemFileDTO("screen_5.jpg", "", 1, 5),
                    new PushSystemFileDTO("screen_6.jpg", "", 1, 6),
                    new PushSystemFileDTO("screen_7.jpg", "", 1, 7),
                    new PushSystemFileDTO("screen_8.jpg", "", 1, 8),
                    new PushSystemFileDTO("boot.jpg", "", 2, 1),
                };
                var iOpt = HttpContext.RequestServices.GetService<IOptionsMonitor<HTTPProtocolOption>>();
                var httpOption = iOpt.CurrentValue;

                //计算文件md5
                foreach (var file in files)
                {
                    string sFile = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "SystemFile", file.FileURL);
                    if (System.IO.File.Exists(sFile))
                    {
                        file.FileMD5 = MD5Helper.GetFileMD5ByHex(sFile);
                        file.FileURL = $"{httpOption.PeopleURLPrefix}/SystemFile/{file.FileURL}";
                    }
                    else
                    {
                        file.FileMD5 = string.Empty;
                        file.FileURL = string.Empty;
                        file.IsDelete = 1;
                    }

                }



                par.TaskExtension = JsonConvert.SerializeObject(files);
            }

            await _RemoteDB.Add(par);
            if (par.TaskType == FaceWebServer.DB.Table.RemoteTypeEnum.ClearAllPeople)
            {
                await _AccessDB.ClearAllPeople(new DeviceAccessDeleteDTO
                {
                    DeviceIDs = par.DeviceIDs,
                });
            }
            return new JsonResult(new JsonResultModel());
        }


        [HttpPost]
        [Route("Delete")]
        public async Task<IActionResult> Delete([FromForm] DeviceRemoteDeleteRequestDTO par)
        {

            if (par.TaskIDs == null)
            {
                return new JsonResult(new JsonResultModel(101,
                     _LanguageHandler.GetCheckParameterErrorMessage("r30")));//"未选择需要操作的远程任务"
            }
            if (par.TaskIDs.Count == 0)
            {
                return new JsonResult(new JsonResultModel(101,
                     _LanguageHandler.GetCheckParameterErrorMessage("r30")));//"未选择需要操作的远程任务"
            }

            await _RemoteDB.Delete(par.TaskIDs);
            return new JsonResult(new JsonResultModel());
        }






        /// <summary>
        /// 清空远程操作记录
        /// </summary>
        /// <param name="par"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("ClearRemote")]
        public async Task<IActionResult> ClearRemote()
        {
            //_logger.LogInformation("清空远程操作记录");
            await _RemoteDB.ClearRemote();

            return new JsonResult(new JsonResultModel());
        }
    }
}
