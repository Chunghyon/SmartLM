using FaceWebServer.Utility.LZ4;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;

namespace FaceWebServer.Utility.Middleware
{

    /// <summary>
    /// 客户端请求的报文使用压缩算法时，使用此中间件可以解密
    /// </summary>
    public class RequestCompressMiddleware
    {
        private readonly RequestDelegate next;
        private readonly ILogger<RequestCompressMiddleware> _logger;

        public RequestCompressMiddleware(RequestDelegate next, ILogger<RequestCompressMiddleware> log)
        {
            this.next = next;
            _logger = log;
        }

        public async Task Invoke(HttpContext context)
        {
            var request = context.Request;
            if (request.Headers.ContainsKey("Content-Encoding"))
            {
                string clientEncoding = request.Headers["Content-Encoding"].ToString();
                long lCompressLen = request.ContentLength.Value;
                

                if ("br".Equals(clientEncoding) ||
                "gzip".Equals(clientEncoding) ||
                "deflate".Equals(clientEncoding))
                {
                    try
                    {

                        //需要进行解压缩
                        MemoryStream decompressedStream = new MemoryStream();
                        if ("gzip".Equals(clientEncoding))
                        {

                            using (var gzipStream = new GZipStream(request.Body,
                                CompressionMode.Decompress))
                            {
                                await gzipStream.CopyToAsync(decompressedStream);
                            }
                        }
                        else if ("deflate".Equals(clientEncoding))
                        {

                            using (var deflateStream = new DeflateStream(request.Body,
                                CompressionMode.Decompress))
                            {
                                await deflateStream.CopyToAsync(decompressedStream);
                            }
                        }
                        else if ("br".Equals(clientEncoding))
                        {

                            using (var brStream = new BrotliStream(request.Body,
                                CompressionMode.Decompress))
                            {
                                await brStream.CopyToAsync(decompressedStream);
                            }
                        }
                        else if ("lz4".Equals(clientEncoding))
                        {

                            await request.Body.CopyToAsync(decompressedStream);
                            if (decompressedStream.TryGetBuffer(out ArraySegment<byte> bodyBuf))
                            {
                                var bodyNewBuf = LZ4Helper.LZ4_Decode(bodyBuf);
                                decompressedStream.Dispose();
                                decompressedStream = new MemoryStream(bodyNewBuf);
                            }

                        }
                        //Console.WriteLine("客户端对请求进行了压缩，压缩算法为：" + clientEncoding + "，加压缩长度：" + decompressedStream.Length);
                        decompressedStream.Seek(0, SeekOrigin.Begin);

                        request.ContentLength = decompressedStream.Length;
                        request.Body = decompressedStream;
                        request.Headers.Add("Gzip-content-Length", lCompressLen.ToString());

                        //Console.WriteLine($"{request.Path} 请求内容已被压缩，算法：{clientEncoding},解压前：{lCompressLen},解压后：{decompressedStream.Length}");
                    }
                    catch (Exception ex)
                    {
                        context.Response.StatusCode = 500;
                        string sTxt = ex.Message;
                        await context.Response.Body.WriteAsync(System.Text.Encoding.UTF8.GetBytes(sTxt));
                        return;
                    }

                   
                }
            }



            await next(context);
        }
    }
}
