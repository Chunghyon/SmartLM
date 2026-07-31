using DeviceProtocolServer.Utilities;
using FaceWebServer.DB.Table;
using FaceWebServer.DTO.People;
using FaceWebServer.Interface;
using FaceWebServer.Language;
using FaceWebServer.Utility;
using FaceWebServer.Utility.Model;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using NPOI.SS.Formula.Functions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Web;
using static Microsoft.Extensions.Logging.EventSource.LoggingEventSource;

namespace DeviceProtocolServer.Controllers.People
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PeopleController : ControllerBase
    {

        private readonly ILogger<PeopleController> _logger;

        private IPeopleService _PeopleDB;
        private ICacheService _Cache;
        private LanguageHandler _LanguageHandler;

        public PeopleController(ILogger<PeopleController> logger,
            IPeopleService peopledb,
            ICacheService cache,
            IOptionsSnapshot<LanguageOption> lngopt)
        {
            _logger = logger;
            _PeopleDB = peopledb;
            _Cache = cache;
            _LanguageHandler = lngopt.Value.GetCurrentLanguageHandler();
        }


        /// <summary>
        /// 获取已添加的人员表格
        /// </summary>
        /// <param name="par"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetPeopleTable")]
        //[TypeFilter(typeof(VerifyActionFilterAttribute))]
        public IActionResult GetPeopleTable([FromBody] PeopleQueryDTO par)
        {

            par.IsAsc = true;
            var peoples = _PeopleDB.Query(par);

            return new JsonResult(new JsonResultModel(peoples));
        }

        /// <summary>
        /// 获取新的用户ID
        /// </summary>
        [HttpPost]
        [Route("GetNewID")]
        public IActionResult GetNewID()
        {
            var newID = _PeopleDB.GetNewAutoUserID();

            return new JsonResult(new JsonResultModel(new
            {
                NewUserID = newID
            }));
        }

        /// <summary>
        /// 获取人员详情
        /// </summary>
        [HttpPost]
        [Route("GetDetail")]
        public async Task<IActionResult> GetPeopleDetail([FromBody] GetPeopleDetailRequestDTO questDto)
        {
            var people = _PeopleDB.GetPeopleByUserID(questDto.UserID);
            if (people == null) return new JsonResult(new JsonResultModel(101,
                _LanguageHandler.GetCheckParameterErrorMessage("r96")));//"人员不存在"

            var retPeople = people.Adapt<PeopleDTO>();
            await _PeopleDB.LoadFeatureCode(retPeople);
            return new JsonResult(new JsonResultModel(retPeople));
        }

        /// <summary>
        /// 添加人员
        /// </summary>
        [HttpPost]
        [Route("New")]
        public async Task<IActionResult> AddNew([FromForm] string PeopleJson, [FromForm] IFormCollection files)
        {
            var oDto = JsonConvert.DeserializeObject<PeopleDTO>(PeopleJson);
            if (oDto.UserID == 0)
            {
                oDto.UserID = _PeopleDB.GetNewAutoUserID();
            }

            //将特征码保存到本地文件
            await _PeopleDB.SaveFeatureCode(oDto);



            var newPeople = oDto.Adapt<FaceWebServer.DB.Table.People>();

            var result = await _PeopleDB.AddNew(newPeople, p => CheckPeopleImage(p, files));
            if (result.Result)
            {
                result.Content = newPeople.ID;
                //_logger.LogInformation($"添加人员 ID:{people.ID} 编号：{people.UserID} 姓名：{people.Name}");
            }
            return new JsonResult(result);
        }


        [HttpPost]
        [Route("EnrollUserMediaData")]
        public async Task<IActionResult> EnrollUserMediaData([FromBody] EnrollUserMediaDataDTO enrollMediaData)
        {

            var result = await _PeopleDB.EnrollUserMediaData(enrollMediaData);

            return new JsonResult(result);
        }


        /// <summary>
        /// 检查照片
        /// </summary>
        /// <param name="p"></param>
        /// <param name="files"></param>
        /// <returns></returns>
        private JsonResultModel CheckPeopleImage(FaceWebServer.DB.Table.People people, IFormCollection files)
        {
            string sPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "People");
            if (!Directory.Exists(sPath))
            {
                Directory.CreateDirectory(sPath);
            }

            string sFileName = $"{people.UserID}.jpg";
            string sFile = Path.Combine(sPath, sFileName);




            if (files.Files.Count == 1)
            {
                try
                {
                    var uploadFile = files.Files[0];
                    if (uploadFile.Length > 0)
                    {
                        if (uploadFile.Length > 20971520)//20*1024KB=20M
                        {
                            return new JsonResultModel(202,
                            _LanguageHandler.GetCheckParameterErrorMessage("r93"));// "人员照片太大，请先压缩照片！"
                        }
                        if (System.IO.File.Exists(sFile)) System.IO.File.Delete(sFile);

                        people.PhotoLen = (int)uploadFile.Length;


                        using (var stream = new MemoryStream(people.PhotoLen))
                        {
                            uploadFile.CopyTo(stream);
                            byte[] bImg = stream.ToArray();

                            var convertResult = JpegConvertHelper.Convert(bImg);
                            if (!string.IsNullOrEmpty(convertResult.MD5))
                            {
                                people.PhotoMD5 = convertResult.MD5;
                            }

                            if (convertResult.OutputData != null)
                                System.IO.File.WriteAllBytes(sFile, convertResult.OutputData!);
                        }


                        people.Photo = $"/People/{sFileName}?md5={HttpUtility.UrlEncode(people.PhotoMD5)}";
                    }
                }
                catch (Exception)
                {

                    throw;
                }

            }
            else
            {
                if (System.IO.File.Exists(sFile)) System.IO.File.Delete(sFile);
                //没有照片
                people.PhotoMD5 = string.Empty;
                people.Photo = string.Empty;
                people.PhotoLen = 0;
            }


            return new JsonResultModel();
        }

        /// <summary>
        /// 更新人员
        /// </summary>
        [HttpPost]
        [Route("Update")]
        public async Task<IActionResult> UpdatePeople([FromForm] string PeopleJson, [FromForm] IFormCollection files)
        {

            var oDto = JsonConvert.DeserializeObject<PeopleDTO>(PeopleJson);
            if (oDto.ID == 0)
                return new JsonResult(new JsonResultModel(101,
                 _LanguageHandler.GetCheckParameterErrorMessage("r96")));//"人员不存在"

            await _PeopleDB.DeleteFeatureCode(oDto);
            await _PeopleDB.SaveFeatureCode(oDto);
            var newPeople = oDto.Adapt<FaceWebServer.DB.Table.People>();



            var result = await _PeopleDB.UpdatePeople(newPeople, p => CheckPeopleImage(p, files));
            if (result.Result)
            {
                result.Content = newPeople.ID;
            }
            return new JsonResult(result);
        }



        /// <summary>
        /// 删除人员
        /// </summary>
        /// <param name="par"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("Delete")]
        //[TypeFilter(typeof(VerifyActionFilterAttribute))]
        public async Task<IActionResult> Delete([FromBody] PeopleDeleteRequestDTO par)
        {


            if (par.PeopleIDs == null)
            {
                return new JsonResult(new JsonResultModel(101,
                    _LanguageHandler.GetCheckParameterErrorMessage("r97")));//"未选择需要删除的人员"
            }
            if (par.PeopleIDs.Count == 0)
            {
                return new JsonResult(new JsonResultModel(101,
                    _LanguageHandler.GetCheckParameterErrorMessage("r97")));//"未选择需要删除的人员"
            }
            HashSet<int> peopleIDLists = new HashSet<int>(par.PeopleIDs);

            await _PeopleDB.DeletePeople(peopleIDLists);

            return new JsonResult(new JsonResultModel(par));
        }

        /// <summary>
        /// 清空所有人员
        /// </summary>
        /// <param name="par"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("ClearPeople")]
        public IActionResult ClearPeople()
        {
            _PeopleDB.ClearPeople();
            return new JsonResult(new JsonResultModel());
        }





        #region 文件夹导入人员
        /// <summary>
        /// 根据文件夹导入人员
        /// </summary>
        /// <param name="par"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("InputPeopleByPath")]
        public async Task<IActionResult> InputPeopleByPath([FromBody] InputPeopleByPathRequestDTO intputDto)
        {
            if (string.IsNullOrEmpty(intputDto.PhotoPath))
            {
                return new JsonResult(new JsonResultModel(101,
                    _LanguageHandler.GetCheckParameterErrorMessage("r139")));//"导入文件夹的参数过多"
            }

            if (!Directory.Exists(intputDto.PhotoPath))
            {
                return new JsonResult(new JsonResultModel(101,
                    _LanguageHandler.GetCheckParameterErrorMessage("r141")));//"文件夹不存在！"
            }

            var fileList = Directory.GetFiles(intputDto.PhotoPath, "*.jpg");
            if (fileList.Length == 0)
            {
                return new JsonResult(new JsonResultModel(101,
                    _LanguageHandler.GetCheckParameterErrorMessage("r142")));//"文件夹中不包含照片！"
            }
            _logger.LogInformation($"开始从文件夹导入照片，文件夹中照片数量：{fileList.Length}");
            await InputPeopleByFileList(fileList);

            return new JsonResult(new JsonResultModel(intputDto.PhotoPath));
        }

        private async Task InputPeopleByFileList(string[] files)
        {
            List<FaceWebServer.DB.Table.People> peoples = new List<FaceWebServer.DB.Table.People>(files.Length);
            string photoPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "People");
            if (!Directory.Exists(photoPath))
            {
                Directory.CreateDirectory(photoPath);
            }

            long lNewUserID = 100_0000;
            //查找系统中最大的用户号
            var EmplIDs = _Cache.GetPeopleUserIDs();
            do
            {
                if (!EmplIDs.Contains(lNewUserID))
                {
                    break;
                }
                lNewUserID++;
            } while (true);

            var lFileCount = files.Length;
            var lSaveCount = 0;

            foreach (var file in files)
            {
                FileInfo fileInfo = new FileInfo(file);

                FaceWebServer.DB.Table.People people = new();
                string sFileName = System.IO.Path.GetFileNameWithoutExtension(file);
                people.UserID = lNewUserID;
                lNewUserID++;
                people.Name = sFileName;//姓名
                people.Job = string.Empty;//职务
                people.Department = string.Empty;//部门
                people.IdentityCard = string.Empty;//身份证
                people.Attachment = string.Empty;//其他信息




                var imagefile = file;
                var peopleImage = Path.Combine(photoPath, $"{people.UserID}.jpg");
                if (System.IO.File.Exists(peopleImage))
                {
                    System.IO.File.Delete(peopleImage);
                }
                
                //System.IO.File.Copy(imagefile, peopleImage);
               
                var bImageBuf = System.IO.File.ReadAllBytes(imagefile);

                var convertResult = JpegConvertHelper.Convert(bImageBuf);
                var md5 = convertResult.MD5;
                bImageBuf = convertResult.OutputData!;
                System.IO.File.WriteAllBytes(peopleImage, bImageBuf);

                people.PhotoLen = bImageBuf.Length;
                people.Photo = $"/People/{people.UserID}.jpg?md5={md5}";
                people.PhotoMD5 = md5;

                people.Password = string.Empty;//密码
                people.CardNum = 0;//卡号
                people.QRCode = string.Empty;//二维码

                people.FaceFeature = string.Empty;//人脸特征码
                people.FaceNum = 0;

                people.Fingerprints = string.Empty; //指纹特征码
                people.FingerprintsNum = 0;

                people.Palmveins = string.Empty;//掌静脉
                people.PalmveinsNum = 0;

                peoples.Add(people);

                if (peoples.Count > 1000)
                {
                    //开始导入到数据库
                    await _PeopleDB.InputPeople(peoples);
                    lSaveCount += peoples.Count;
                    _logger.LogInformation($"正在从文件夹导入照片，导入数量：{lSaveCount}/{lFileCount}");
                    peoples.Clear();
                }
            }



            if (peoples.Count > 0)
            {
                //开始导入到数据库
                await _PeopleDB.InputPeople(peoples);
                lSaveCount += peoples.Count;
                peoples.Clear();
            }
            _logger.LogInformation($"从文件夹导入照片完毕，已导入数量：{lSaveCount}");
        }
        #endregion

        //#region excel 导入人员

        ///// <summary>
        ///// 根据Excel导入人员
        ///// </summary>
        ///// <param name="par"></param>
        ///// <returns></returns>
        //[HttpPost]
        //[Route("InputPeopleByExcel")]
        //[RequestSizeLimit(500 * 1024 * 1024)]
        //[RequestFormLimits(MultipartBodyLengthLimit = 500 * 1024 * 1024)]
        //public async Task<IActionResult> InputPeopleByExcel([FromForm] IFormCollection files)
        //{
        //    if (files.Files.Count != 1)
        //    {
        //        return new JsonResult(new JsonResultModel(101,
        //            _LanguageHandler.GetCheckParameterErrorMessage("r98")));//"导入人员时仅支持上传一个文件"
        //    }
        //    var xlsFile = files.Files[0];
        //    var sFileName = xlsFile.FileName;
        //    var fileExt = Path.GetExtension(sFileName).ToLower();
        //    List<FaceWebServer.DB.Table.People> peoples = new List<FaceWebServer.DB.Table.People>();
        //    using (var xlsStream = xlsFile.OpenReadStream())
        //    {
        //        JsonResult ReadRst = null;
        //        try
        //        {
        //            switch (fileExt)
        //            {
        //                case ".xlsx":
        //                    ReadRst = await ReadExcelPeopleByFile(new XSSFWorkbook(xlsStream), peoples);
        //                    break;
        //                case ".xls":
        //                    ReadRst = await ReadExcelPeopleByFile(new HSSFWorkbook(xlsStream), peoples);
        //                    break;
        //                case ".zip":
        //                    ReadRst = UnZipFileAndOpenExcelByUSB(xlsStream, peoples);
        //                    break;
        //                default:
        //                    return new JsonResult(new JsonResultModel(102,
        //                        _LanguageHandler.GetCheckParameterErrorMessage("r99")));//"不支持此文件类型"
        //            }
        //            if (ReadRst != null)
        //            {
        //                return ReadRst;
        //            }

        //        }
        //        catch (Exception ex)
        //        {

        //            return new JsonResult(new JsonResultModel(104,
        //                _LanguageHandler.GetCheckParameterErrorMessage("r101") + Environment.NewLine + ex.Message));//"Excel 格式解析失败，请重新编辑文件! " + Environment.NewLine + ex.Message));
        //        }
        //    }

        //    if (peoples.Count > 0)
        //    {

        //        //开始导入到数据库
        //        _PeopleDB.InputPeople(peoples);
        //    }



        //    return new JsonResult(new JsonResultModel());
        //}

        //#region 联网验证图片
        //private async Task MultiWrokProgress<T>(List<T> sources, Func<string, IEnumerable<T>, Task> WrokProgress)
        //{
        //    List<Task> URLCheckTasks = new List<Task>();
        //    int iListCount = sources.Count;
        //    if (iListCount < 100)
        //    {
        //        URLCheckTasks.Add(WrokProgress("Task0", sources));
        //    }
        //    else
        //    {
        //        int iTaskMax = 10;
        //        int iBegin = 0;
        //        int iChunkCount = iListCount / iTaskMax;
        //        SubList<T> sub;
        //        for (int i = 0; i < iTaskMax; i++)
        //        {
        //            if (i == iTaskMax - 1)
        //            {
        //                iChunkCount = iListCount - iBegin;
        //            }
        //            sub = new SubList<T>(sources, iBegin, iChunkCount);

        //            string sTaskName = $"Task{i}";
        //            _logger.LogInformation($"{sTaskName} 分组范围：{iBegin} - {iBegin + iChunkCount - 1} 总数： {iChunkCount}");
        //            iBegin += iChunkCount;

        //            URLCheckTasks.Add(WrokProgress($"{sTaskName}", sub));
        //        }

        //    }
        //    await Task.WhenAll(URLCheckTasks.ToArray());
        //}

        //private async Task CheckPeopleURLProgress(string sTaskName, IEnumerable<FaceWebServer.DB.Table.People> peoples)
        //{
        //    IHttpClientFactory clientFactory = this.HttpContext.RequestServices.GetService(typeof(IHttpClientFactory)) as IHttpClientFactory;
        //    HttpClient httpClient = clientFactory.CreateClient();
        //    Stopwatch wch = new Stopwatch();
        //    wch.Start();
        //    string name = $"{sTaskName} Thread-{Thread.CurrentThread.ManagedThreadId} ";
        //    int lMax = peoples.Count();
        //    int lCount = 0;
        //    int iStep = 0;

        //    string sLocalPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "People");
        //    if (Directory.Exists(sLocalPath))
        //    {

        //    }

        //    _logger.LogInformation($"{name}  准备处理 {lMax} 个 URL地址请求");
        //    foreach (var people in peoples)
        //    {
        //        //检查这个URL地址
        //        try
        //        {
        //            string sPhotoURL = people.Photo;



        //            try
        //            {
        //                do
        //                {
        //                    if (sPhotoURL.StartsWith("/People") && people.PhotoLen > 0)
        //                    {
        //                        _logger.LogInformation($"{sTaskName}  {people.Name} 重复处理！");
        //                        break;
        //                    }
        //                    people.PhotoWay = string.Empty;
        //                    people.Photo = string.Empty;
        //                    people.PhotoLen = 0;
        //                    people.PhotoMD5 = string.Empty;

        //                    if (string.IsNullOrWhiteSpace(sPhotoURL))
        //                    {
        //                        break;

        //                    }

        //                    //_logger.LogInformation($"{sTaskName}  {people.Name} {sPhotoURL} 开始处理！");

        //                    if (sPhotoURL.StartsWith("file"))//本地文件请求
        //                    {
        //                        await CheckLocalPhotoFile(sPhotoURL, people, sLocalPath, name);

        //                    }
        //                    else//网络请求
        //                    {
        //                        await CheckWebPhotoFile(sPhotoURL, people, httpClient, name);
        //                    }
        //                    //_logger.LogInformation($"{sTaskName}  {people.Name} {sPhotoURL} 处理完毕！");
        //                } while (false);




        //            }
        //            catch (Exception ex)
        //            {
        //                _logger.LogInformation($"{name}  {people.Name} {sPhotoURL} 发生未知错误\r\n{ex.Message}");
        //                //throw;
        //            }



        //            lCount++;
        //            int iTmpStep = (int)(((Single)lCount / (Single)lMax) * 100);
        //            if (iTmpStep != iStep)
        //            {
        //                iStep = iTmpStep;
        //                _logger.LogInformation($"{name} 进度：{iStep}%  {lMax}/{lCount}");
        //            }

        //        }
        //        catch (Exception)
        //        {

        //            throw;
        //        }
        //    }
        //    wch.Stop();

        //    _logger.LogInformation($"{name} 处理 {peoples.Count()} 个 URL地址请求耗时:{wch.ElapsedMilliseconds} ms");

        //}

        //private async Task CheckWebPhotoFile(string sPhotoURL, FaceWebServer.DB.Table.People people,
        //    HttpClient httpClient,
        //    string sTaskName)
        //{
        //    var request = new HttpRequestMessage(HttpMethod.Head, sPhotoURL);
        //    var response = await httpClient.SendAsync(request);
        //    if (response.IsSuccessStatusCode)
        //    {
        //        var headers = response.Content.Headers;
        //        if ("image/jpeg".Equals(headers.ContentType.MediaType))
        //        {
        //            string md5 = string.Empty;
        //            if (headers.Contains("Content-MD5"))
        //            {
        //                md5 = Convert.ToBase64String(headers.ContentMD5);

        //            }
        //            else
        //            {
        //                request = new HttpRequestMessage(HttpMethod.Get, sPhotoURL);
        //                response = await httpClient.SendAsync(request);
        //                if (response.IsSuccessStatusCode)
        //                {
        //                    response.EnsureSuccessStatusCode();
        //                    var imageStream = await response.Content.ReadAsStreamAsync();
        //                    md5 = MD5Helper.GetStreamMD5ByBase64(imageStream);
        //                }
        //            }


        //            people.PhotoWay = "path";
        //            people.PhotoLen = (int)(headers.ContentLength ?? 0);
        //            people.Photo = people.Photo + "?md5=" + HttpUtility.UrlEncode(md5);
        //            people.PhotoMD5 = md5;



        //        }
        //        else
        //        {//不支持的图片格式
        //            _logger.LogError($"{sTaskName} {sPhotoURL} 图片格式不支持：{headers.ContentType.MediaType}");
        //        }

        //    }
        //    else
        //    {
        //        _logger.LogError($"{sTaskName} {sPhotoURL} URL地址请求错误 {response.StatusCode} {response.ReasonPhrase}");
        //    }
        //}

        //private async Task<bool> CheckLocalPhotoFile(string sPhotoURL, FaceWebServer.DB.Table.People people,
        //    string sLocalPath,
        //    string sTaskName)
        //{
        //    Uri uri = new Uri(sPhotoURL);
        //    string sFilePath = uri.LocalPath;
        //    if (System.IO.File.Exists(sFilePath))
        //    {
        //        FileInfo fileInfo = new FileInfo(sFilePath);
        //        var iFileLength = (int)fileInfo.Length;
        //        string sLocalFileName = $"{people.UserID}.jpg";


        //        if (iFileLength > 20971520)//20*1024KB=20M
        //        {
        //            _logger.LogInformation($"{sTaskName}  {sPhotoURL} 人员照片太大，请先压缩照片！");
        //            return false;
        //        }
        //        if (!".jpg".Equals(fileInfo.Extension))
        //        {
        //            _logger.LogInformation($"{sTaskName}  {sPhotoURL} 不支持的文件类型！");
        //            return false;
        //        }


        //        string sLocalFile = Path.Combine(sLocalPath, sLocalFileName);
        //        try
        //        {
        //            if (System.IO.File.Exists(sLocalFile)) System.IO.File.Delete(sLocalFile);

        //            string md5 = string.Empty;
        //            using var localFileStream = new FileStream(sLocalFile, FileMode.Create);
        //            using (var stream = fileInfo.OpenRead())
        //            {
        //                //计算MD5
        //                md5 = MD5Helper.GetStreamMD5ByBase64(stream);
        //                stream.Position = 0;
        //                await stream.CopyToAsync(localFileStream);

        //            }

        //            people.PhotoMD5 = md5;
        //            people.Photo = $"/People/{sLocalFileName}?md5={HttpUtility.UrlEncode(md5)}";
        //            people.PhotoWay = "path";
        //            people.PhotoLen = iFileLength;

        //        }
        //        catch (Exception ex)
        //        {
        //            _logger.LogInformation($"{sTaskName}  {sPhotoURL} 文件操作失败\r\n{ex.Message}");
        //            return false;
        //        }

        //        return true;

        //    }
        //    else
        //    {
        //        _logger.LogInformation($"{sTaskName}  {sPhotoURL} 不存在");
        //        return false;

        //    }
        //}


        //#endregion

        ///// <summary>
        ///// 从自定义的excel中读取人员资料
        ///// </summary>
        ///// <param name="book"></param>
        ///// <param name="peoples"></param>
        ///// <returns></returns>
        //private async Task<JsonResult> ReadExcelPeopleByFile(IWorkbook book, List<FaceWebServer.DB.Table.People> peoples)
        //{
        //    if (book.NumberOfSheets == 0)
        //    {
        //        return new JsonResult(new JsonResultModel(103,
        //            _LanguageHandler.GetCheckParameterErrorMessage("r100")));//"Excel中没有表格"
        //    }

        //    //Excel 格式检查
        //    try
        //    {
        //        #region 标题检查
        //        var sColTitle = "人员编号,人员姓名,密码,卡号,职务,联系方式,身份证信息,地址信息,照片URL,人脸识别阈值".Split(",");
        //        sColTitle = _LanguageHandler.GetCheckParameterErrorMessage("r102").Split(",");
        //        var sheet = book.GetSheetAt(0);
        //        var oTitelRow = sheet.GetRow(sheet.FirstRowNum);
        //        if (oTitelRow == null)
        //        {
        //            return new JsonResult(new JsonResultModel(105,
        //                _LanguageHandler.GetCheckParameterErrorMessage("r103")));//"Excel 未包含标题 "
        //        }
        //        var minCol = oTitelRow.FirstCellNum;
        //        for (int i = 0; i < sColTitle.Length; i++)
        //        {
        //            var sTitle = GetExcelCellValue(oTitelRow.GetCell(i));
        //            if (sTitle != sColTitle[i])
        //            {
        //                return new JsonResult(new JsonResultModel(106,
        //                     _LanguageHandler.GetCheckParameterErrorMessage("r104")));//"Excel 标题检查不通过! "
        //            }
        //        }

        //        for (int i = 0; i < sColTitle.Length; i++)
        //        {
        //            var sTitle = GetExcelCellValue(oTitelRow.GetCell(i));
        //            if (sTitle != sColTitle[i])
        //            {
        //                return new JsonResult(new JsonResultModel(106,
        //                     _LanguageHandler.GetCheckParameterErrorMessage("r104")));//"Excel 标题检查不通过! "
        //            }
        //        }
        //        #endregion


        //        var iMaxRow = sheet.LastRowNum;
        //        var iRow = sheet.FirstRowNum + 1;

        //        do
        //        {
        //            if (iRow > iMaxRow)
        //            {
        //                break;
        //            }
        //            var oDataRow = sheet.GetRow(iRow);
        //            iRow++;

        //            if (oDataRow == null)
        //                continue;


        //            string sValue = GetExcelCellValue(oDataRow.GetCell(0));//人员编号
        //            if (string.IsNullOrWhiteSpace(sValue))
        //                continue;
        //            long lValue;
        //            if (!long.TryParse(sValue, out lValue))
        //            {
        //                continue;
        //            }
        //            FaceWebServer.DB.Table.People people = new FaceWebServer.Model.People.People();
        //            people.UserID = lValue;
        //            people.Name = GetExcelCellValue(oDataRow.GetCell(1));//人员姓名
        //            people.Password = GetExcelCellValue(oDataRow.GetCell(2));//密码

        //            //卡号
        //            sValue = GetExcelCellValue(oDataRow.GetCell(3));
        //            if (long.TryParse(sValue, out lValue))
        //            {
        //                people.CardNum = lValue;
        //            }
        //            people.Job = GetExcelCellValue(oDataRow.GetCell(4));//职务
        //            people.EmployeePhone = GetExcelCellValue(oDataRow.GetCell(5));//联系方式
        //            people.UserIDentity = GetExcelCellValue(oDataRow.GetCell(6));//身份证信息
        //            people.EmployeeAddress = GetExcelCellValue(oDataRow.GetCell(7));//地址信息
        //            people.PhotoWay = "path";
        //            people.Photo = GetExcelCellValue(oDataRow.GetCell(8));//照片URL
        //            if (!string.IsNullOrEmpty(people.Photo))
        //            {
        //                people.PhotoLen = people.Photo.Length;
        //            }



        //            //人脸识别阈值
        //            sValue = GetExcelCellValue(oDataRow.GetCell(9));
        //            if (long.TryParse(sValue, out lValue))
        //            {
        //                people.EmployeeShold = lValue;
        //            }

        //            peoples.Add(people);

        //        } while (true);
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }


        //    #region 验证照片合法性

        //    string sLocalPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "People");
        //    if (!Directory.Exists(sLocalPath))
        //    {
        //        Directory.CreateDirectory(sLocalPath);
        //    }


        //    List<Task> URLCheckTasks = new List<Task>();
        //    int iPeoples = peoples.Count;
        //    if (iPeoples < 300)
        //    {
        //        URLCheckTasks.Add(CheckPeopleURLProgress("Task0", peoples));
        //    }
        //    else
        //    {
        //        int iTaskMax = 3;
        //        int iBegin = 0;
        //        int iChunkCount = iPeoples / iTaskMax;

        //        for (int i = 0; i < iTaskMax; i++)
        //        {
        //            if (i == iTaskMax - 1)
        //            {
        //                iChunkCount = iPeoples - iBegin;
        //            }
        //            {
        //                var sub = new SubList<FaceWebServer.DB.Table.People>(peoples, iBegin, iChunkCount);

        //                string sTaskName = $"Task{i}";
        //                _logger.LogInformation($"{sTaskName} 分组范围：{iBegin} - {iBegin + iChunkCount - 1} 总数： {iChunkCount}");
        //                iBegin += iChunkCount;

        //                URLCheckTasks.Add(Task.Run(() => CheckPeopleURLProgress($"{sTaskName}", sub)));
        //            }

        //        }

        //    }
        //    await Task.WhenAll(URLCheckTasks.ToArray());

        //    #endregion

        //    return null;

        //}

        //private static string GetExcelCellValue(ICell cell)
        //{
        //    if (cell == null)
        //        return string.Empty;
        //    switch (cell.CellType)
        //    {
        //        case CellType.Unknown:
        //            return cell.StringCellValue;
        //        case CellType.Numeric:
        //            if (cell.ToString().Length > 15)
        //            {
        //                return cell.ToString();
        //            }
        //            return cell.NumericCellValue.ToString();
        //        case CellType.String:
        //            return cell.StringCellValue;
        //        case CellType.Formula:
        //            return "=" + cell.CellFormula;
        //        case CellType.Blank:
        //            return string.Empty;
        //        case CellType.Boolean:
        //            return cell.BooleanCellValue.ToString();
        //        case CellType.Error:
        //            return cell.ErrorCellValue.ToString();
        //        default:
        //            return cell.StringCellValue;
        //    }
        //}

        ///// <summary>
        ///// 从设备导出的excel中读取人员资料
        ///// </summary>
        ///// <param name="zipStream"></param>
        ///// <param name="peoples"></param>
        ///// <returns></returns>
        ///// <exception cref="Exception"></exception>
        //private JsonResult UnZipFileAndOpenExcelByUSB(Stream zipStream, List<FaceWebServer.DB.Table.People> peoples)
        //{
        //    //创建临时目录
        //    string extractPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "People", "zip");
        //    if (Directory.Exists(extractPath))
        //    {
        //        Directory.Delete(extractPath, true);
        //    }
        //    Directory.CreateDirectory(extractPath);
        //    extractPath = Path.GetFullPath(extractPath);
        //    if (!extractPath.EndsWith(Path.DirectorySeparatorChar.ToString(), StringComparison.Ordinal))
        //        extractPath += Path.DirectorySeparatorChar;

        //    ZipArchive zip = new ZipArchive(zipStream, ZipArchiveMode.Read, false,
        //        System.Text.Encoding.GetEncoding("GB2312"));
        //    string xlsFile = string.Empty;
        //    Dictionary<string, string> imagefiles = new Dictionary<string, string>();
        //    foreach (ZipArchiveEntry entry in zip.Entries)
        //    {
        //        bool bWrite = false;
        //        bool isXls = false;

        //        if (entry.Length > 0)
        //        {
        //            if (entry.FullName.EndsWith(".jpg", StringComparison.OrdinalIgnoreCase))
        //            {
        //                bWrite = true;
        //            }

        //            if (entry.FullName.EndsWith(".xls", StringComparison.OrdinalIgnoreCase))
        //            {
        //                bWrite = true;
        //                isXls = true;
        //            }

        //            if (entry.FullName.EndsWith(".xlsx", StringComparison.OrdinalIgnoreCase))
        //            {
        //                bWrite = true;
        //                isXls = true;
        //            }
        //        }
        //        else
        //        {
        //            bWrite = true;
        //        }

        //        if (bWrite)
        //        {
        //            // 获取完整路径，以确保删除了相关段。
        //            string destinationPath = Path.GetFullPath(Path.Combine(extractPath, entry.FullName));
        //            //序号匹配是最安全的，区分大小写的卷可以装入
        //            //是不区分大小写的。
        //            if (destinationPath.StartsWith(extractPath, StringComparison.Ordinal))
        //            {
        //                if (entry.Length == 0 && entry.FullName.EndsWith("/"))
        //                {
        //                    //这是个目录
        //                    if (!Directory.Exists(destinationPath))
        //                        Directory.CreateDirectory(destinationPath);
        //                }
        //                else
        //                {
        //                    entry.ExtractToFile(destinationPath);
        //                    if (isXls)
        //                    {
        //                        xlsFile = destinationPath;
        //                    }
        //                    else
        //                    {
        //                        //照片
        //                        imagefiles.Add(entry.FullName, destinationPath);
        //                        imagefiles.Add(Path.GetFileName(destinationPath), destinationPath);
        //                    }
        //                }

        //            }
        //        }



        //    }
        //    zip.Dispose();
        //    zip = null;

        //    if (string.IsNullOrEmpty(xlsFile))
        //    {
        //        return null;
        //    }
        //    var fileExt = Path.GetExtension(xlsFile).ToLower();
        //    IWorkbook book = null;
        //    switch (fileExt)
        //    {
        //        case ".xlsx":
        //            book = new XSSFWorkbook(xlsFile);
        //            break;
        //        case ".xls":
        //            book = new HSSFWorkbook(new FileStream(xlsFile, FileMode.Open));
        //            break;

        //        default:
        //            throw new Exception(
        //                _LanguageHandler.GetCheckParameterErrorMessage("r99"));//"不支持此文件类型"
        //    }
        //    if (book.NumberOfSheets == 0)
        //    {
        //        return new JsonResult(new JsonResultModel(104,
        //            _LanguageHandler.GetCheckParameterErrorMessage("r100")));//"Excel中没有表格"
        //    }


        //    #region 标题检查
        //    var sColTitle = "人员编号,姓名,人员角色,密码,身份证号码,电话号码,职务,门禁卡 卡号,登记照片名".Split(",");

        //    var sheet = book.GetSheetAt(0);
        //    var oTitelRow = sheet.GetRow(sheet.FirstRowNum);
        //    if (oTitelRow == null)
        //    {
        //        return new JsonResult(new JsonResultModel(104,
        //            _LanguageHandler.GetCheckParameterErrorMessage("r103")));//"Excel 未包含标题 "
        //    }
        //    var minCol = oTitelRow.FirstCellNum;
        //    for (int i = 0; i < sColTitle.Length; i++)
        //    {
        //        var sTitle = GetExcelCellValue(oTitelRow.GetCell(i));
        //        if (sTitle != sColTitle[i])
        //        {
        //            throw new Exception(
        //                 _LanguageHandler.GetCheckParameterErrorMessage("r104"));//"Excel 标题检查不通过! "
        //        }
        //    }

        //    for (int i = 0; i < sColTitle.Length; i++)
        //    {
        //        var sTitle = GetExcelCellValue(oTitelRow.GetCell(i));
        //        if (sTitle != sColTitle[i])
        //        {
        //            throw new Exception(
        //                 _LanguageHandler.GetCheckParameterErrorMessage("r104"));//"Excel 标题检查不通过! "
        //        }
        //    }
        //    #endregion




        //    var iMaxRow = sheet.LastRowNum;
        //    var iRow = sheet.FirstRowNum + 1;
        //    string photoPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "People");
        //    do
        //    {
        //        if (iRow > iMaxRow)
        //        {
        //            break;
        //        }
        //        var oDataRow = sheet.GetRow(iRow);
        //        iRow++;

        //        if (oDataRow == null)
        //            continue;


        //        string sValue = GetExcelCellValue(oDataRow.GetCell(0));//人员编号
        //        if (string.IsNullOrWhiteSpace(sValue))
        //            continue;
        //        long lValue;
        //        if (!long.TryParse(sValue, out lValue))
        //        {
        //            continue;
        //        }
        //        FaceWebServer.DB.Table.People people = new FaceWebServer.Model.People.People();
        //        people.UserID = lValue;
        //        people.Name = GetExcelCellValue(oDataRow.GetCell(1));//姓名
        //        people.Password = GetExcelCellValue(oDataRow.GetCell(3));//密码

        //        //卡号
        //        sValue = GetExcelCellValue(oDataRow.GetCell(7));
        //        if (long.TryParse(sValue, out lValue))
        //        {
        //            people.CardNum = lValue;
        //        }
        //        people.Job = GetExcelCellValue(oDataRow.GetCell(6));//职务
        //        people.EmployeePhone = GetExcelCellValue(oDataRow.GetCell(5));//联系方式
        //        people.UserIDentity = GetExcelCellValue(oDataRow.GetCell(4));//身份证信息
        //        people.EmployeeAddress = String.Empty;

        //        people.PhotoWay = "path";
        //        people.Photo = GetExcelCellValue(oDataRow.GetCell(8));//照片URL
        //        if (!imagefiles.ContainsKey(people.Photo))
        //        {
        //            people.Photo = String.Empty;
        //            people.PhotoLen = 0;
        //            people.PhotoMD5 = String.Empty;
        //        }
        //        else
        //        {
        //            var imagefile = imagefiles[people.Photo];
        //            var peopleImage = Path.Combine(photoPath, $"{people.UserID}.jpg");
        //            if (System.IO.File.Exists(peopleImage))
        //            {
        //                System.IO.File.Delete(peopleImage);
        //            }
        //            var md5 = MD5Helper.GetFileMD5ByBase64(imagefile);
        //            System.IO.File.Copy(imagefile, peopleImage);
        //            people.PhotoLen = (int)(new FileInfo(peopleImage)).Length;
        //            people.Photo = $"/People/{people.UserID}.jpg?md5={HttpUtility.UrlEncode(md5)}";
        //            people.PhotoMD5 = md5;
        //        }
        //        people.EmployeeShold = 0;

        //        peoples.Add(people);

        //    } while (true);

        //    book.Close();



        //    return null;
        //}

        //#endregion




    }



}


