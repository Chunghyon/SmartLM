using FaceWebServer.Utility.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace DeviceProtocolServer.Controllers.Test
{
    [Route("[controller]")]
    [ApiController]
    public class SQLiteTestController : ControllerBase
    {
        private readonly ILogger<SQLiteTestController> _logger;

        public SQLiteTestController(ILogger<SQLiteTestController> logger)
        {
            _logger = logger;

        }


        [Route("CreatePeopleExcel/{count}")]
        [HttpGet]
        public async Task<JsonResult> CreatePeopleExcel(int count)
        {
            if (count < 0 || count > 100_0000)
            {
                return new JsonResult(new JsonResultModel(1, "值范围错误"));
            }
            var sXLSFileName = $"input_{DateTime.Now:yyyyMMdd_HHmmss_ffff}.xlsx";

            var sPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Excel");
            if (!Directory.Exists(sPath))
            {
                Directory.CreateDirectory(sPath);
            }
            var sXLSFile = Path.Combine(sPath, sXLSFileName);
            //System.IO.MemoryStream xlsStream=new MemoryStream();
            await Task.Run(() =>
            {
                IWorkbook book = new XSSFWorkbook();

                var sheet = book.CreateSheet("人员表");

                //创建标题行
                var sColTitle = "人员编号,人员姓名,密码,卡号,职务,联系方式,身份证信息,地址信息,照片URL,人脸识别阈值".Split(",");
                var iCols = sColTitle.Length;
                var row = sheet.CreateRow(0);
                for (int i = 0; i < iCols; i++)
                {
                    row.CreateCell(i).SetCellValue(sColTitle[i]);
                    sheet.SetColumnWidth(i, 4500);
                    if (i == 8)
                    {
                        sheet.SetColumnWidth(i, 12000);
                    }
                }


                //创建内容行
                for (int i = 0; i < count; i++)
                {
                    row = sheet.CreateRow(i + 1);

                    row.CreateCell(0).SetCellValue(i);//人员编号
                    row.CreateCell(1).SetCellValue($"人员{i}");//人员姓名
                    row.CreateCell(2).SetCellValue($"{1000 + i}");//密码
                    row.CreateCell(3).SetCellValue($"{1000 + i}");//卡号
                    row.CreateCell(4).SetCellValue($"职务{i}");//职务
                    row.CreateCell(5).SetCellValue($"联系方式{i}");//联系方式
                    row.CreateCell(6).SetCellValue($"身份证信息{i}");//身份证信息
                    row.CreateCell(7).SetCellValue($"地址信息{i}");//地址信息
                    row.CreateCell(8).SetCellValue($"http://oss2.pc15.net/Test/{i}.jpg");//照片URL
                    row.CreateCell(9).SetCellValue(0);//人脸识别阈值
                }


                using (var FileStreamfile = new FileStream(sXLSFile, FileMode.Create))
                {
                    book.Write(FileStreamfile);
                };
                book.Close();
            });

            return new JsonResult(new JsonResultModel($"/Excel/{sXLSFileName}"));

        }

        [Route("TestReadURL")]
        [HttpPost]
        public async Task<JsonResult> TestReadURL([FromForm] string URL)
        {
            IHttpClientFactory clientFactory = HttpContext.RequestServices.GetService(typeof(IHttpClientFactory)) as IHttpClientFactory;
            HttpClient httpClient = clientFactory.CreateClient();
            Stopwatch wch = new Stopwatch();
            wch.Start();

            var request = new HttpRequestMessage(HttpMethod.Head, URL);
            var response = await httpClient.SendAsync(request);


            wch.Stop();

            return new JsonResult(new JsonResultModel(1, $"耗时：{wch.ElapsedMilliseconds} ms") { Content = response });
        }


    }
}
