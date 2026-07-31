using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaceWebServer.Utility
{
    public static class GZipHelper
    {
        /// <summary>
        /// 解压
        /// </summary>
        /// <param name="formFile"></param>
        /// <returns></returns>
        public static string DecompressFile(IFormFile formFile)
        {
            /**
             * 读取压缩过的数据
             */
            using var compressedStream = formFile.OpenReadStream();
            /**
             * 将压缩过的数据进行gzip解压，之后放到内存流中
             */
            using var gzipStream = new GZipStream(compressedStream, CompressionMode.Decompress);

            /**
             * 将流读取为字符串
             */
            using var decompressedStream = new StreamReader(compressedStream, encoding: System.Text.Encoding.UTF8, leaveOpen: true);
            var json = decompressedStream.ReadToEnd();
            return json;
        }

        /// <summary>
        /// 获取gzip数据
        /// </summary>
        /// <param name="formCollection"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string GetGzipValue(IFormCollection formCollection, string name)
        {
            var result = string.Empty;

            var ifile = formCollection.Files.GetFile(name);
            if (ifile != null && ifile.Headers.ContainsKey("Content-Encoding"))//检查文件是否存在，以及是否存在Content-Encoding
            {
                var encoding = ifile.Headers["Content-Encoding"].ToString();
                if ("gzip".Contains(encoding, StringComparison.OrdinalIgnoreCase))//判断headers 中Content-Encoding是否等于gzip
                {
                    try
                    {
                        result = GZipHelper.DecompressFile(ifile);//解压GZIP
                    }
                    catch { }
                }
            }
            return result;
        }
    }
}

