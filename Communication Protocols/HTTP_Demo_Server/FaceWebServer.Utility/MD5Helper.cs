using System;
using System.IO;
using System.Security.Cryptography;

namespace FaceWebServer.Utility
{
    public static class MD5Helper
    {
        public static string GetStreamMD5ByHex(Stream bufStream)
        {
            using MD5 md5 = MD5.Create();

            bufStream.Position = 0;
            int iLen = (int)bufStream.Length;


            //AddLog($"文件长度： {iLen}");
            byte[] md5Value = md5.ComputeHash(bufStream);

            return Convert.ToHexString(md5Value);


        }

        public static string GetStreamMD5ByBase64(Stream bufStream)
        {
            using MD5 md5 = MD5.Create();

            bufStream.Position = 0;
            int iLen = (int)bufStream.Length;


            //AddLog($"文件长度： {iLen}");
            byte[] md5Value = md5.ComputeHash(bufStream);

            return Convert.ToBase64String(md5Value);


        }


        public static string GetByteBufMD5ByBase64(ArraySegment<byte> buf)
        {
            using MD5 md5 = MD5.Create();
            //FileStream  

            using (MemoryStream sRead = new MemoryStream(buf.Array, buf.Offset, buf.Count))
            {
                sRead.Position = 0;
                int iLen = (int)sRead.Length;


                //AddLog($"文件长度： {iLen}");
                byte[] md5Value = md5.ComputeHash(sRead);

                return Convert.ToBase64String(md5Value);
            }

        }

        public static string GetByteBufMD5ByHex(ArraySegment<byte> buf)
        {
            using MD5 md5 = MD5.Create();
            //FileStream  

            using (MemoryStream sRead = new MemoryStream(buf.Array, buf.Offset, buf.Count))
            {
                sRead.Position = 0;
                int iLen = (int)sRead.Length;


                //AddLog($"文件长度： {iLen}");
                byte[] md5Value = md5.ComputeHash(sRead);

                return Convert.ToHexString(md5Value);
            }

        }



        public static string GetFileMD5ByBase64(string sFile)
        {
            using MD5 md5 = MD5.Create();
            //FileStream  

            using (FileStream sRead = new FileStream(sFile, FileMode.Open, FileAccess.Read))
            {
                int iLen = (int)sRead.Length;

                //AddLog($"文件长度： {iLen}");
                byte[] md5Value = md5.ComputeHash(sRead);

                return Convert.ToBase64String(md5Value);
            }

        }

        public static string GetFileMD5ByHex(string sFile)
        {
            using MD5 md5 = MD5.Create();
            //FileStream  

            using (FileStream sRead = new FileStream(sFile, FileMode.Open, FileAccess.Read))
            {
                int iLen = (int)sRead.Length;

                //AddLog($"文件长度： {iLen}");
                byte[] md5Value = md5.ComputeHash(sRead);

                return Convert.ToHexString(md5Value);
            }

        }


        public static string GetStringMD5ByBase64(string sText)
        {
            using MD5 md5 = MD5.Create();
            //FileStream  
            System.Text.Encoding encoding = System.Text.Encoding.UTF8;

            using (MemoryStream sRead = new MemoryStream(encoding.GetBytes(sText)))
            {
                int iLen = (int)sRead.Length;

                //AddLog($"文件长度： {iLen}");
                byte[] md5Value = md5.ComputeHash(sRead);

                return Convert.ToBase64String(md5Value);
            }

        }

        public static string GetMD5ByBase64(byte[] datas)
        {
            var md5 = MD5.Create();
            using var sRead = new MemoryStream(datas);
            byte[] md5Value = md5.ComputeHash(sRead);
            return Convert.ToBase64String(md5Value);
        }

    }

}

