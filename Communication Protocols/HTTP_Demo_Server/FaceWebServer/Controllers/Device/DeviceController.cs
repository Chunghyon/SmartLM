using DoNetDrive.Common.Extensions;
using FaceWebServer.DB.Table;
using FaceWebServer.DTO.Device;
using FaceWebServer.Interface;
using FaceWebServer.Language;
using FaceWebServer.Utility;
using FaceWebServer.Utility.FilterAttribute;
using FaceWebServer.Utility.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;


namespace DeviceProtocolServer.Controllers.Device
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class DeviceController : ControllerBase
    {

        private readonly ILogger<DeviceController> _logger;
        private IFaceDriveService _DeviceDB;
        private readonly string[] _permittedExtensions = { ".pkg" };
        private LanguageHandler _LanguageHandler;

        public DeviceController(ILogger<DeviceController> logger,
            IFaceDriveService door,
            IOptionsSnapshot<LanguageOption> lngopt)
        {
            _logger = logger;
            _DeviceDB = door;
            _LanguageHandler = lngopt.Value.GetCurrentLanguageHandler();
        }


        /// <summary>
        /// 获取已安装的设备表格
        /// </summary>
        /// <param name="par"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("Query")]
        //[TypeFilter(typeof(VerifyActionFilterAttribute))]
        public IActionResult GetDeviceTable([FromBody] DeviceQueryDTO par)
        {
            return new JsonResult(new JsonResultModel(_DeviceDB.Query(par)));
        }

        /// <summary>
        /// 获取设备在线状态
        /// </summary>
        /// <param name="par"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetDeviceOnlineStatus")]
        //[TypeFilter(typeof(VerifyActionFilterAttribute))]
        public IActionResult GetDeviceOnlineStatus([FromBody] GetDeviceOnlineStatusRequestDTO queryDto)
        {
            return new JsonResult(new JsonResultModel(_DeviceDB.GetDeviceOnlineStatus(queryDto.SNList)));
        }


        /// <summary>
        /// 获取设备详情
        /// </summary>
        /// <param name="par"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetDeviceDetail")]

        public IActionResult GetDeviceDetail(GetDeviceDetailRequestDTO parameter)
        {
            var device = _DeviceDB.GetDeviceDetail(parameter.DeviceID);
            if (device == null) return new JsonResult(new JsonResultModel(101,
                    _LanguageHandler.GetCheckParameterErrorMessage("r72")));// "设备不存在"

            return new JsonResult(new JsonResultModel(device));
        }



        /// <summary>
        /// 更新设备信息
        /// </summary>
        /// <param name="par"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("Update")]
        //[TypeFilter(typeof(VerifyActionFilterAttribute))]
        public IActionResult Update([FromBody] DeviceDetail par)
        {
            JsonResultModel ret = _DeviceDB.Update(par);
            return new JsonResult(ret);
        }

        /// <summary>
        /// 删除设备
        /// </summary>
        /// <param name="par"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("Delete")]
        //[TypeFilter(typeof(VerifyActionFilterAttribute))]
        public async Task<IActionResult> Delete([FromBody] DeviceDeleteRequestDTO par)
        {

            if (par.DeviceIDs == null)
            {
                return new JsonResult(new JsonResultModel(101,
                    _LanguageHandler.GetCheckParameterErrorMessage("r71")));//"未选择需要删除的设备"
            }
            if (par.DeviceIDs.Count == 0)
            {
                return new JsonResult(new JsonResultModel(101,
                    _LanguageHandler.GetCheckParameterErrorMessage("r71")));//"未选择需要删除的设备"
            }

            await _DeviceDB.Delete(par.DeviceIDs);
            return new JsonResult(new JsonResultModel(par));
        }



        /// <summary>
        /// 获取设备出厂默认值
        /// </summary>
        /// <param name="par"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetDefaultValue/{Protocol}")]
        //[Authorize]
        public IActionResult GetDefaultValue(string Protocol)
        {
            //Protocol = DeviceDetail.HTTPv1
            //Protocol = DeviceDetail.HTTPv2
            var deviceJson = _DeviceDB.GetDefaultValue(Protocol);
            return new JsonResult(new JsonResultModel(deviceJson));
        }


        /// <summary>
        /// 设置设备出厂默认值
        /// </summary>
        /// <param name="par"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("SetDefaultValue")]
        public IActionResult SetDefaultValue([FromBody] SetDefaultValueRequestDTO par)
        {
            _DeviceDB.SaveDefaultValue(par);

            return new JsonResult(new JsonResultModel());
        }


        /// <summary>
        /// 获取设备固件列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetDeviceSoftList")]
        [Authorize]
        public IActionResult GetDeviceSoftList()
        {

            return new JsonResult(LoadDeviceSoftList());
        }

        private softDetailList LoadDeviceSoftList()
        {
            var settingFile = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "softpkg", "softsetting.json");
            string settingJson = string.Empty;
            softDetailList oDetailList;

            if (System.IO.File.Exists(settingFile))
            {
                settingJson = System.IO.File.ReadAllText(settingFile);
                oDetailList = JsonConvert.DeserializeObject<softDetailList>(settingJson);
            }
            else
            {
                oDetailList = new softDetailList() { list = new List<softDetail>() };
            }
            return oDetailList;
        }

        /// <summary>
        /// 设备固件升级
        /// </summary>
        /// <param name="par"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("UpdateDeviceSoft")]
        [Authorize]
        //[TypeFilter(typeof(VerifyActionFilterAttribute))]
        public IActionResult UpdateDeviceSoft(
            [FromBody] UpdateDeviceSoftRequestDTO dto)
        {
            var oDevice = _DeviceDB.Find<DeviceDetail>(dto.DeviceID);
            if (oDevice == null)
                return new JsonResult(new JsonResultModel(101,
                    _LanguageHandler.GetCheckParameterErrorMessage("r72")));// "设备不存在"
            string sURL = dto.SoftName;


            var softList = LoadDeviceSoftList();
            var oSoft = softList.list.Find((x) => x.name == dto.SoftName);
            if (oSoft == null)
            {
                return new JsonResult(new JsonResultModel(102,
                _LanguageHandler.GetCheckParameterErrorMessage("r73")));//"固件文件不存在"
            }

            var sPath = System.IO.Directory.GetCurrentDirectory();
            sPath = Path.Combine(sPath, "wwwroot", "softpkg");
            var sFile = Path.Combine(sPath, oSoft.url);
            if (!System.IO.File.Exists(sFile))
            {
                return new JsonResult(new JsonResultModel(102,
                _LanguageHandler.GetCheckParameterErrorMessage("r73")));//"固件文件不存在"
            }

            sURL = $"/softpkg/{oSoft.url}";


            _DeviceDB.UpdateDeviceSoft(sURL, oSoft.ver, oSoft.md5, dto.DeviceID);

            return new JsonResult(new JsonResultModel());
        }


        /// <summary>
        /// 上传固件文件
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("UploadDeviceSoftFile")]
        public async Task<IActionResult> UploadDeviceSoftFile()
        {
            var request = HttpContext.Request;
            var ecr = System.Text.Encoding.UTF8;

            // validation of Content-Type
            // 1. first, it must be a form-data request
            // 2. a boundary should be found in the Content-Type
            if (!request.HasFormContentType ||
                !MediaTypeHeaderValue.TryParse(request.ContentType, out var mediaTypeHeader) ||
                string.IsNullOrEmpty(mediaTypeHeader.Boundary.Value))
            {
                return new UnsupportedMediaTypeResult();
            }

            var reader = new MultipartReader(mediaTypeHeader.Boundary.Value, request.Body);
            var section = await reader.ReadNextSectionAsync();
            var dicValue = new Dictionary<string, string>();
            // This sample try to get the first file from request and save it
            // Make changes according to your needs in actual use
            while (section != null)
            {
                var hasContentDispositionHeader = ContentDispositionHeaderValue.TryParse(section.ContentDisposition,
                    out var contentDisposition);

                if (hasContentDispositionHeader && contentDisposition.DispositionType.Equals("form-data") &&
                    !string.IsNullOrEmpty(contentDisposition.FileName.Value))
                {
                    // Don't trust any file name, file extension, and file data from the request unless you trust them completely
                    // Otherwise, it is very likely to cause problems such as virus uploading, disk filling, etc
                    // In short, it is necessary to restrict and verify the upload
                    // Here, we just use the temporary folder and a random file name

                    // Get the temporary folder, and combine a random file name with it
                    var sfilename = Path.GetFileName(contentDisposition.FileName.Value);

                    if (dicValue.ContainsKey("fileName"))
                    {
                        return new JsonResult(new JsonResultModel(101,
                    _LanguageHandler.GetCheckParameterErrorMessage("r76")));// "一次仅允许上传一个固件文件"
                    }
                    dicValue.Add("fileName", sfilename);
                    if (!sfilename.EndsWith(".pkg"))
                    {
                        return new JsonResult(new JsonResultModel(102,
                    _LanguageHandler.GetCheckParameterErrorMessage("r77")));// "固件文件后缀不正确"
                    }
                    var _targetFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "softpkg", sfilename);

                    using (var targetStream = System.IO.File.Create(_targetFilePath))
                    {
                        await section.Body.CopyToAsync(targetStream);
                    }
                    var fileinfo = new FileInfo(_targetFilePath);
                    dicValue.Add("fileSize", fileinfo.Length.ToString());
                    dicValue.Add("filePath", _targetFilePath);
                    if (fileinfo.Length < 300 * 1024 * 1024)
                    {
                        System.IO.File.Delete(_targetFilePath);
                        return new JsonResult(new JsonResultModel(103,
                        _LanguageHandler.GetCheckParameterErrorMessage("r78")));// "固件文件太小"));
                    }
                    //_logger.LogInformation($"上传大文件接口，文件名:{contentDisposition.FileName.Value}，文件大小：{fileinfo.Length}");
                    //验证文件合法性
                    string sHex;
                    using (var stm = System.IO.File.OpenRead(_targetFilePath))
                    {
                        byte[] buf = new byte[10];
                        stm.Read(buf, 0, 10);
                        sHex = buf.ToHex();

                    }
                    if (sHex != "20504B20000000001100")
                    {
                        System.IO.File.Delete(_targetFilePath);
                        return new JsonResult(new JsonResultModel(104,
                         _LanguageHandler.GetCheckParameterErrorMessage("r79")));// "固件文件特征不正确"
                    }
                }
                else
                {
                    using (var targetStream = new MemoryStream())
                    {
                        await section.Body.CopyToAsync(targetStream);
                        targetStream.Position = 0;
                        byte[] buf = new byte[targetStream.Length];
                        targetStream.Read(buf, 0, (int)targetStream.Length);
                        string value = ecr.GetString(buf);
                        //_logger.LogInformation($"上传大文件接口，参数名:{contentDisposition.Name.Value}，值：{value}");

                        dicValue.Add(contentDisposition.Name.Value, value);
                    }
                }

                section = await reader.ReadNextSectionAsync();
            }

            if (!dicValue.ContainsKey("fileName"))
            {
                return new JsonResult(new JsonResultModel(105,
                         _LanguageHandler.GetCheckParameterErrorMessage("r80")));//  "上传固件时参数应包含 fileName 字段"
            }

            if (!dicValue.ContainsKey("softName") || string.IsNullOrWhiteSpace(dicValue["softName"]))
            {
                return new JsonResult(new JsonResultModel(106,
                     _LanguageHandler.GetCheckParameterErrorMessage("r81")));// "固件名称不能为空"
            }
            if (dicValue["softName"].Length < 5)
            {
                return new JsonResult(new JsonResultModel(107,
                    _LanguageHandler.GetCheckParameterErrorMessage("r82")));// "固件名称太短"
            }


            if (!dicValue.ContainsKey("softVer") || string.IsNullOrWhiteSpace(dicValue["softVer"]))
            {
                return new JsonResult(new JsonResultModel(108,
                    _LanguageHandler.GetCheckParameterErrorMessage("r83")));// "固件版本不能为空"
            }


            var settingFile = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "softpkg", "softsetting.json");
            string settingJson = string.Empty;
            softDetailList oDetailList;

            if (System.IO.File.Exists(settingFile))
            {
                settingJson = System.IO.File.ReadAllText(settingFile);
                oDetailList = JsonConvert.DeserializeObject<softDetailList>(settingJson);
            }
            else
            {
                oDetailList = new softDetailList() { list = new List<softDetail>() };
            }


            var sFileName = dicValue["fileName"];
            var hash = new Dictionary<string, softDetail>();

            foreach (var item in oDetailList.list)
            {
                hash.Add(item.url, item);
            }
            softDetail detail = null;
            if (hash.ContainsKey(sFileName))
            {
                //已存在
                detail = hash[sFileName];
            }
            else
            {
                detail = new softDetail()
                {
                    url = sFileName
                };
                hash.Add(sFileName, detail);
                oDetailList.list.Insert(0, detail);
            }
            detail.md5 = MD5Helper.GetFileMD5ByHex(dicValue["filePath"]);
            detail.name = dicValue["softName"];
            detail.ver = dicValue["softVer"];
            settingJson = JsonConvert.SerializeObject(oDetailList, Formatting.Indented);
            System.IO.File.WriteAllText(settingFile, settingJson, ecr);


            // If the code runs to this location, it means that no files have been saved
            return new JsonResult(new JsonResultModel());
        }



        [HttpPost]
        [Route("RemoteSnapshoot")]
        public async Task<IActionResult> RemoteSnapshoot([FromBody] RemoteSnapshootDTO dto)
        {
  
            var ret = await _DeviceDB.RemoteSnapshoot(dto);
             

            
            return new JsonResult(ret);
        }
    }
}
