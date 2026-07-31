using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using System;
using System.Collections.Generic;

using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeviceProtocolServer.Utilities
{
    public class FaceImageUtil
    {
        /// <summary>
        /// 文件最大尺寸
        /// </summary>
        private const int ImageSizeMax = 300*1024;
        /// <summary>
        /// 进行图片转换，图片像素不能超过 480*640，大小尺寸不能超过50K
        /// </summary>
        /// <param name="strFile"></param>
        /// <returns></returns>
        public static byte[] ConvertImage(byte[] bImage)
        {
            using (Image<Argb32> img = Image.Load<Argb32>(new MemoryStream(bImage)))
            {
                float rate = 1;
                if (img.Width > 480 || img.Height > 640 || bImage.Length > ImageSizeMax)
                {
                    float rate1, rate2;

                    rate1 = 480 / (float)img.Width;
                    rate2 = 640 / (float)img.Height;
                    rate = rate1 > rate2 ? rate2 : rate1;
                    if (rate > 1) rate = 1;

                }
                else
                {
                    img.Dispose();
                    return bImage;
                }

                



                int iWidth = img.Width, iHeight = img.Height;
                iWidth = (int)(iWidth * rate);
                iHeight = (int)(iHeight * rate);
                byte[] newFile = null;

                if(rate<1)
                {
                    // 如果需要，可以按比例缩放图像以适应指定的宽度和高度
                    img.Mutate(x => x
                        .Resize(new ResizeOptions
                        {
                            Size = new Size(iWidth, iHeight),
                            Mode = ResizeMode.Max
                        }));
                }
                


                using (Image<Argb32> copy = new Image<Argb32>(480, 640))
                {

                    copy.Mutate(x => x
                   .DrawImage(img, new Point((480 - iWidth) / 2, (640 - iHeight) / 2), 1));

                    //进行图片大小的测算


                    bool bSave = false;
                    int iQuality = 80;
                    do
                    {
                        JpegEncoder jpegEncoder = new JpegEncoder()
                        {
                            Quality = iQuality
                        };//这里用来设置保存时的图片质量

                        using (MemoryStream ms = new MemoryStream())
                        {
                            copy.Save(ms, jpegEncoder);
                            int iNewLen = (int)ms.Length;
                            if (iNewLen <= ImageSizeMax)
                            {
                                newFile = new byte[iNewLen];
                                ms.Position = 0;
                                ms.Read(newFile, 0, iNewLen);
                                bSave = true;
                            }
                            ms.Close();
                            ms.Dispose();

                            iQuality -= 5;
                        }
                    } while (!bSave);

                }

                return newFile;
            }
        }

    }
}
