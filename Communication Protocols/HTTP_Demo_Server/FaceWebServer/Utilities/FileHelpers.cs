using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;
using FaceWebServer.DB.Table;
using FaceWebServer.Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;
using NPOI.SS.Formula.Functions;

namespace DeviceProtocolServer.Utilities
{
    public static class FileHelpers
    {
        // If you require a check on specific characters in the IsValidFileExtensionAndSignature
        // method, supply the characters in the _allowedChars field.
        private static readonly byte[] _allowedChars = { };
        // For more file signatures, see the File Signatures Database (https://www.filesignatures.net/)
        // and the official specifications for the file types you wish to add.
        private static readonly Dictionary<string, List<byte[]>> _fileSignature = new Dictionary<string, List<byte[]>>
        {
            { ".gif", new List<byte[]> { new byte[] { 0x47, 0x49, 0x46, 0x38 } } },
            { ".png", new List<byte[]> { new byte[] { 0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A } } },
            { ".jpeg", new List<byte[]>
                {
                    new byte[] { 0xFF, 0xD8, 0xFF, 0xE0 },
                    new byte[] { 0xFF, 0xD8, 0xFF, 0xE2 },
                    new byte[] { 0xFF, 0xD8, 0xFF, 0xE3 },
                }
            },
            { ".jpg", new List<byte[]>
                {
                    new byte[] { 0xFF, 0xD8, 0xFF, 0xE0 },
                    new byte[] { 0xFF, 0xD8, 0xFF, 0xE1 },
                    new byte[] { 0xFF, 0xD8, 0xFF, 0xE8 },
                }
            },
            { ".zip", new List<byte[]>
                {
                    new byte[] { 0x50, 0x4B, 0x03, 0x04 },
                    new byte[] { 0x50, 0x4B, 0x4C, 0x49, 0x54, 0x45 },
                    new byte[] { 0x50, 0x4B, 0x53, 0x70, 0x58 },
                    new byte[] { 0x50, 0x4B, 0x05, 0x06 },
                    new byte[] { 0x50, 0x4B, 0x07, 0x08 },
                    new byte[] { 0x57, 0x69, 0x6E, 0x5A, 0x69, 0x70 },
                }
            },
        };

        // **WARNING!**
        // In the following file processing methods, the file's content isn't scanned.
        // In most production scenarios, an anti-virus/anti-malware scanner API is
        // used on the file before making the file available to users or other
        // systems. For more information, see the topic that accompanies this sample
        // app.

        public static async Task<byte[]> ProcessFormFile<T>(IFormFile formFile,
            ModelStateDictionary modelState, string[] permittedExtensions,
            long sizeLimit)
        {
            var fieldDisplayName = string.Empty;

            // Use reflection to obtain the display name for the model
            // property associated with this IFormFile. If a display
            // name isn't found, error messages simply won't show
            // a display name.
            MemberInfo property =
                typeof(T).GetProperty(
                    formFile.Name.Substring(formFile.Name.IndexOf(".",
                    StringComparison.Ordinal) + 1));

            if (property != null)
            {
                if (property.GetCustomAttribute(typeof(DisplayAttribute)) is
                    DisplayAttribute displayAttribute)
                {
                    fieldDisplayName = $"{displayAttribute.Name} ";
                }
            }

            // Don't trust the file name sent by the client. To display
            // the file name, HTML-encode the value.
            var trustedFileNameForDisplay = WebUtility.HtmlEncode(
                formFile.FileName);

            // Check the file length. This check doesn't catch files that only have 
            // a BOM as their content.
            if (formFile.Length == 0)
            {
                modelState.AddModelError(formFile.Name,
                    $"{fieldDisplayName}({trustedFileNameForDisplay}) is empty.");

                return Array.Empty<byte>();
            }

            if (formFile.Length > sizeLimit)
            {
                var megabyteSizeLimit = sizeLimit / 1048576;
                modelState.AddModelError(formFile.Name,
                    $"{fieldDisplayName}({trustedFileNameForDisplay}) exceeds " +
                    $"{megabyteSizeLimit:N1} MB.");

                return Array.Empty<byte>();
            }

            try
            {
                using (var memoryStream = new MemoryStream())
                {
                    await formFile.CopyToAsync(memoryStream);

                    // Check the content length in case the file's only
                    // content was a BOM and the content is actually
                    // empty after removing the BOM.
                    if (memoryStream.Length == 0)
                    {
                        modelState.AddModelError(formFile.Name,
                            $"{fieldDisplayName}({trustedFileNameForDisplay}) is empty.");
                    }

                    if (!IsValidFileExtensionAndSignature(
                        formFile.FileName, memoryStream, permittedExtensions))
                    {
                        modelState.AddModelError(formFile.Name,
                            $"{fieldDisplayName}({trustedFileNameForDisplay}) file " +
                            "type isn't permitted or the file's signature " +
                            "doesn't match the file's extension.");
                    }
                    else
                    {
                        return memoryStream.ToArray();
                    }
                }
            }
            catch (Exception ex)
            {
                modelState.AddModelError(formFile.Name,
                    $"{fieldDisplayName}({trustedFileNameForDisplay}) upload failed. " +
                    $"Please contact the Help Desk for support. Error: {ex.HResult}");
                // Log the exception
            }

            return Array.Empty<byte>();
        }

        public static async Task<byte[]> ProcessStreamedFile(
            MultipartSection section, ContentDispositionHeaderValue contentDisposition,
            ModelStateDictionary modelState, string[] permittedExtensions, long sizeLimit)
        {
            try
            {
                using (var memoryStream = new MemoryStream())
                {
                    await section.Body.CopyToAsync(memoryStream);

                    // Check if the file is empty or exceeds the size limit.
                    if (memoryStream.Length == 0)
                    {
                        modelState.AddModelError("File", "The file is empty.");
                    }
                    else if (memoryStream.Length > sizeLimit)
                    {
                        var megabyteSizeLimit = sizeLimit / 1048576;
                        modelState.AddModelError("File",
                        $"The file exceeds {megabyteSizeLimit:N1} MB.");
                    }
                    //else if (!IsValidFileExtensionAndSignature(
                    //    contentDisposition.FileName.Value, memoryStream, 
                    //    permittedExtensions))
                    //{
                    //    modelState.AddModelError("File",
                    //        "The file type isn't permitted or the file's " +
                    //        "signature doesn't match the file's extension.");
                    //}
                    else
                    {
                        return memoryStream.ToArray();
                    }
                }
            }
            catch (Exception ex)
            {
                modelState.AddModelError("File",
                    "The upload failed. Please contact the Help Desk " +
                    $" for support. Error: {ex.HResult}");
                // Log the exception
            }

            return Array.Empty<byte>();
        }

        private static bool IsValidFileExtensionAndSignature(string fileName, Stream data, string[] permittedExtensions)
        {
            if (string.IsNullOrEmpty(fileName) || data == null || data.Length == 0)
            {
                return false;
            }

            var ext = Path.GetExtension(fileName).ToLowerInvariant();

            if (string.IsNullOrEmpty(ext) || !permittedExtensions.Contains(ext))
            {
                return false;
            }

            data.Position = 0;

            using (var reader = new BinaryReader(data))
            {
                if (ext.Equals(".txt") || ext.Equals(".csv") || ext.Equals(".prn"))
                {
                    if (_allowedChars.Length == 0)
                    {
                        // Limits characters to ASCII encoding.
                        for (var i = 0; i < data.Length; i++)
                        {
                            if (reader.ReadByte() > sbyte.MaxValue)
                            {
                                return false;
                            }
                        }
                    }
                    else
                    {
                        // Limits characters to ASCII encoding and
                        // values of the _allowedChars array.
                        for (var i = 0; i < data.Length; i++)
                        {
                            var b = reader.ReadByte();
                            if (b > sbyte.MaxValue ||
                                !_allowedChars.Contains(b))
                            {
                                return false;
                            }
                        }
                    }

                    return true;
                }

                // Uncomment the following code block if you must permit
                // files whose signature isn't provided in the _fileSignature
                // dictionary. We recommend that you add file signatures
                // for files (when possible) for all file types you intend
                // to allow on the system and perform the file signature
                // check.
                /*
                if (!_fileSignature.ContainsKey(ext))
                {
                    return true;
                }
                */

                // File signature check
                // --------------------
                // With the file signatures provided in the _fileSignature
                // dictionary, the following code tests the input content's
                // file signature.
                var signatures = _fileSignature[ext];
                var headerBytes = reader.ReadBytes(signatures.Max(m => m.Length));

                return signatures.Any(signature =>
                    headerBytes.Take(signature.Length).SequenceEqual(signature));
            }
        }



        /// <summary>
        /// 在文件集合中查找指定字段，并进行Gzip解压缩
        /// </summary>
        /// <param name="files"></param>
        /// <param name="sFileName"></param>
        /// <returns></returns>
        public static async Task<string> FindGzipJsonString(ILogger _logger, IFormCollection files, string sFileName)
        {
            //没有找到recordJson，可能是gzip压缩了
            var ifile = files.Files.GetFile(sFileName);

            if (ifile != null)
            {
                if (ifile.Headers.ContainsKey("Content-Encoding"))
                {
                    var encoding = ifile.Headers["Content-Encoding"].ToString();
                    //Console.WriteLine($"/note/insertNoteFace 请求内容已被压缩，算法：{encoding}");

                    if ("gzip".Equals(encoding))
                    {
                        try
                        {
                            using MemoryStream compressedStream = new MemoryStream();
                            using MemoryStream decompressedStream = new MemoryStream();
                            await ifile.CopyToAsync(compressedStream);
                            compressedStream.Position = 0;
                            using var gzipStream = new GZipStream(compressedStream, CompressionMode.Decompress);

                            await gzipStream.CopyToAsync(decompressedStream);
                            decompressedStream.Position = 0;
                            using var requestReader = new StreamReader(decompressedStream, encoding: System.Text.Encoding.UTF8, leaveOpen: true);
                            return await requestReader.ReadToEndAsync();
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError("寻找压缩字符串时发生错误 " + ex.Message);
                        }
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// 文件信息
        /// </summary>
        public class HTTPFileDetail
        {
            /// <summary>
            /// 文件名
            /// </summary>
            public string FileName { get; set; }

            /// <summary>
            /// 文件长度
            /// </summary>
            public int FileLength { get; set; }

            /// <summary>
            /// 文件MD5
            /// </summary>
            public string FileMD5 { get; set; }

        }


        /// <summary>
        /// 保存HTTP post 发送来的文件
        /// </summary>
        /// <param name="files">http post 发送来的文件集合</param>
        /// <param name="sFileName">需要保存的文件字段名 例如：Photo</param>
        /// <param name="sSavePath">需要将文件保存的目录名  例如：c:\wwwroot\People</param>
        /// <param name="sSaveFileName">需要保存的文件名 例如： 1.jpg</param>
        /// <returns></returns>
        public static async Task<HTTPFileDetail> SaveHTTPPOSTFile(ILogger _logger, IFormCollection files, string sFileName,
            string sSavePath, string sSaveFileName,bool bAutoZoom = false)
        {
            try
            {
                var photoFile = files.Files.GetFile(sFileName);
                if (photoFile != null)
                {
                    if (photoFile.Length > 1024000)//1024kb
                    {
                        _logger.LogError("照片太大，请先压缩照片！");// "照片太大，请先压缩照片！"
                        return null;
                    }

                    
                    if (!Directory.Exists(sSavePath))
                    {
                        Directory.CreateDirectory(sSavePath);
                    }

                    string sFile = Path.Combine(sSavePath, sSaveFileName);

                    if (System.IO.File.Exists(sFile)) System.IO.File.Delete(sFile);
                    HTTPFileDetail fileDtl = new HTTPFileDetail();
                    fileDtl.FileName = sFile;
                    fileDtl.FileLength = (int)photoFile.Length;


                    using (var stream = new MemoryStream(fileDtl.FileLength))
                    {
                        photoFile.CopyTo(stream);
                        byte[] bImg = stream.ToArray();

                        if(bAutoZoom) bImg = FaceImageUtil.ConvertImage(bImg); //对图片进行缩放
                        fileDtl.FileMD5 = MD5Helper.GetByteBufMD5ByHex(new ArraySegment<byte>(bImg));


                        System.IO.File.WriteAllBytes(sFile, bImg);
                    }

                    return fileDtl;
                }

            }
            catch (Exception ex)
            {

                _logger.LogError("从上传的文件列表中查询文件时发生错误 " + ex.Message);
            }

            return null;
        }


        /// <summary>
        /// 根据人员照片URL返回人员的照片的存储路径
        /// /People/1000004.jpg?md5=9338FFBC16B393C816B855D1339E5D7C
        /// </summary>
        /// <param name="sPeopleURL"></param>
        /// <returns></returns>
        public static string GetPeopleImagePath(string sPeopleURL)
        {
            if (string.IsNullOrEmpty(sPeopleURL))
                return string.Empty;

            string sRootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
            //读取文件，转为Base64
            var photo = sPeopleURL;
            if (photo.Contains("?"))
            {
                photo = photo.Substring(1, photo.IndexOf("?") - 1);
            }

            var photoArr = photo.Split("/");
            string sFile = sRootPath;
            foreach (var item in photoArr)
            {
                sFile = Path.Combine(sFile, item);
            }

            return sFile;
        }


        /// <summary>
        /// 将 ArraySegment 数组写到文件中
        /// </summary>
        /// <param name="data"></param>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static async Task WriteArraySegmentToFileAsync(ArraySegment<byte> data, string filePath)
        {
            // 创建文件流
            using (FileStream fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                // 写入数据
                await fileStream.WriteAsync(data.Array, data.Offset, data.Count);
            }
        }

        /// <summary>
        /// 将 Base64字符串转换写到文件中
        /// </summary>
        /// <param name="sBase64Str">需要转换的base64字符串</param>
        /// <param name="filePath">需要写入到的文件</param>
        /// <returns>文件长度</returns>
        public static async Task<int> Base64StringConverBinToFileAsync(string sBase64Str, string filePath)
        {
            try
            {
                var buf = Convert.FromBase64String(sBase64Str);
                // 创建文件流
                using (FileStream fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None))
                {
                    // 写入数据
                    await fileStream.WriteAsync(buf, 0, buf.Length);
                }
                return buf.Length;
            }
            catch (Exception)
            {

                return 0;
            }
            
        }
    }
}
