using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace DoNetDrive.Protocol.Fingerprint.Test
{
    /// <summary>
    /// 图片处理
    /// </summary>
    public class ImageTool
    {

        /// <summary>
        /// 进行图片转换，图片像素不能超过 480*640，大小尺寸不能超过50K
        /// </summary>
        /// <param name="bImage">图片文件缓冲区</param>
        /// <param name="iMaxWidth">最大像素宽</param>
        /// <param name="iMaxHeight">最大像素高</param>
        /// <param name="iMaxSize">最大文件长度</param>
        /// <param name="bPadding">是否允许填充空白</param>
        /// <returns></returns>
        public static byte[] ConvertImage(byte[] bImage, int iMaxWidth, int iMaxHeight, int iMaxSize, bool bPadding = true)
        {

            using (Image img = ReadImageByBuf(bImage))
            {
                var bBuf = ConvertImage(img, bImage.Length, iMaxWidth, iMaxHeight, iMaxSize, bPadding);
                if (bBuf == null)
                {
                    return bImage;
                }
                return bBuf;
            }
        }
        /// <summary>
        /// 转换图片格式，并限定最大值
        /// </summary>
        /// <param name="img">需要转换的图片</param>
        /// <param name="srcLen">源文件长度</param>
        /// <param name="iMaxWidth">最大像素宽</param>
        /// <param name="iMaxHeight">最大像素高</param>
        /// <param name="iMaxSize">最大文件长度</param>
        /// <param name="bPadding">是否允许填充空白</param>
        /// <returns></returns>
        public static byte[] ConvertImage(Image img, int srcLen, int iMaxWidth, int iMaxHeight, int iMaxSize, bool bPadding = false)
        {
            float rate = 1;
            if (img.Width > iMaxWidth || img.Height > iMaxHeight || srcLen > iMaxSize)
            {
                float rate1, rate2;

                rate1 = (float)iMaxWidth / (float)img.Width;
                rate2 = (float)iMaxHeight / (float)img.Height;
                rate = rate1 > rate2 ? rate2 : rate1;
                //if (rate > 1) rate = 1;

            }
            else
            {
                return null;
            }
            int iWidth = img.Width, iHeight = img.Height;
            iWidth = (int)(iWidth * rate);
            iHeight = (int)(iHeight * rate);
            byte[] newFile = null;
            if (iWidth != iMaxWidth || iHeight != iMaxHeight)
            {
                if (!bPadding)
                {
                    iMaxWidth = iWidth;
                    iMaxHeight = iHeight;
                }
            }
            ImageCodecInfo jgpEncoder = GetEncoder(ImageFormat.Jpeg);

            System.Drawing.Imaging.Encoder myEncoder = System.Drawing.Imaging.Encoder.Quality;
            // 创建一个EncoderParameters对象.
            // 一个EncoderParameters对象有一个EncoderParameter数组对象
            EncoderParameters myEncoderParameters = new EncoderParameters(1);



            using (Bitmap bimg = new Bitmap(iMaxWidth, iMaxHeight, PixelFormat.Format32bppArgb))
            {
                using (Graphics graphics = Graphics.FromImage(bimg))
                {
                    graphics.PageUnit = GraphicsUnit.Pixel;
                    graphics.CompositingQuality = CompositingQuality.HighQuality;
                    graphics.SmoothingMode = SmoothingMode.HighQuality;
                    graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    graphics.Clear(Color.White);
                    graphics.DrawImage(img, new Rectangle((iMaxWidth - iWidth) / 2, (iMaxHeight - iHeight) / 2, iWidth, iHeight));
                    graphics.Dispose();
                }

                //进行图片大小的测算
                long iQuality = 100;
                bool bSave = false;
                do
                {
                    EncoderParameter myEncoderParameter = new EncoderParameter(myEncoder, iQuality);//这里用来设置保存时的图片质量
                    myEncoderParameters.Param[0] = myEncoderParameter;
                    using (MemoryStream ms = new MemoryStream())
                    {
                        bimg.Save(ms, jgpEncoder, myEncoderParameters);
                        myEncoderParameter.Dispose();
                        int iNewLen = (int)ms.Length;
                        if (iNewLen <= iMaxSize)
                        {
                            newFile = new byte[iNewLen];
                            ms.Position = 0;
                            ms.Read(newFile, 0, iNewLen);
                            bSave = true;
                        }
                        ms.Close();
                        ms.Dispose();

                        iQuality -= 2;
                    }
                } while (!bSave);

            }

            return newFile;

        }

        public static ImageCodecInfo GetEncoder(ImageFormat format)
        {

            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageDecoders();

            foreach (ImageCodecInfo codec in codecs)
            {
                if (codec.FormatID == format.Guid)
                {
                    return codec;
                }
            }
            return null;
        }

        public static Bitmap ReadImageByFile(string sFile)
        {
            var bImage = File.ReadAllBytes(sFile);

            return ReadImageByBuf(bImage);
        }
        public static Bitmap ReadImageByBuf(byte[] sbuf)
        {
            Bitmap oImage;
            using (var imgms = new MemoryStream(sbuf))
            {
                oImage = (Bitmap)Bitmap.FromStream(imgms);
            }
            return oImage;
        }



    }//end class
}
