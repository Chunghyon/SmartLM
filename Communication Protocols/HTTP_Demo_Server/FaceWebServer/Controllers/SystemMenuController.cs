using FaceWebServer.DB.Table;
using FaceWebServer.Interface;
using FaceWebServer.Utility.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace DeviceProtocolServer.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class SystemMenuController : ControllerBase
    {
        private readonly ILogger<SystemMenuController> _logger;

        private ISystemMenuService _SystemMenu;

        public SystemMenuController(ILogger<SystemMenuController> logger, ISystemMenuService record)
        {
            _logger = logger;
            _SystemMenu = record;
        }





        [Route("GetSystemMenu")]
        [HttpGet]
        public JsonResult GetSystemMenu()
        {
            //_logger.LogInformation($"获取系统菜单");

            return new JsonResult(_SystemMenu.GetSystemMenu());
        }

        [Route("GCMemory")]
        [HttpGet]
        public JsonResult GCMemory()
        {
            GC.Collect();

            return new JsonResult(GC.GetGCMemoryInfo());
        }


        [HttpPost]
        [Route("PostFormData")]
        public async Task<IActionResult> PostFromData()
        {
            Console.WriteLine();
            Console.WriteLine($"*********************");
            Console.WriteLine($"*****收到新请求******");
            Console.WriteLine($"*********************");
            Console.WriteLine();
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

            var sectionNum = 1;
            while (section != null)
            {
                Console.WriteLine();
                Console.WriteLine($"*****读取块号：{sectionNum}******");
                Console.WriteLine("section.Headers");
                Console.WriteLine(JsonConvert.SerializeObject(section.Headers, Formatting.Indented));
                //var hasContentDispositionHeader = ContentDispositionHeaderValue.TryParse(section.ContentDisposition,
                //    out var contentDisposition);


                using (var targetStream = new MemoryStream())
                {
                    await section.Body.CopyToAsync(targetStream);
                    targetStream.Position = 0;
                    byte[] buf = new byte[targetStream.Length];
                    targetStream.Read(buf, 0, (int)targetStream.Length);
                    string value = ecr.GetString(buf);
                    //_logger.LogInformation($"上传大文件接口，参数名:{contentDisposition.Name.Value}，值：{value}");

                    Console.WriteLine("section.Body");
                    Console.WriteLine(value);
                }


                section = await reader.ReadNextSectionAsync();
                sectionNum++;

            }



            // If the code runs to this location, it means that no files have been saved
            return new JsonResult(new JsonResultModel());
        }
    }
}
