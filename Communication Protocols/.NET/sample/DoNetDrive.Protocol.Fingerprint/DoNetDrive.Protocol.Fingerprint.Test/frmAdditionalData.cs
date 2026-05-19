using DoNetDrive.Core.Command;
using DoNetDrive.Common.Extensions;
using DoNetDrive.Common;
using DoNetDrive.Protocol.Fingerprint.AdditionalData;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DoNetDrive.Protocol.Fingerprint.Person;
using System.Diagnostics;
using ReadPersonDetail_Parameter = DoNetDrive.Protocol.Fingerprint.AdditionalData.ReadPersonDetail_Parameter;
using ReadPersonDetail = DoNetDrive.Protocol.Fingerprint.AdditionalData.ReadPersonDetail;
using ReadPersonDetail_Result = DoNetDrive.Protocol.Fingerprint.AdditionalData.ReadPersonDetail_Result;

namespace DoNetDrive.Protocol.Fingerprint.Test
{
    public partial class frmAdditionalData : frmNodeForm
    {
        #region 单例模式

        private string Msg_Have;
        private string Msg_None;
        private string Msg_Photo;
        private string Msg_FP;
        private string Msg_Face;
        private string Msg_Palm;//掌静脉
        private string Msg_6;
        private string Msg_7;
        private string Msg_8;
        private string Msg_9;
        private string Msg_10;
        private string Msg_11;
        private string Msg_12;
        private string Msg_13;
        private string Msg_14;
        private string Msg_15;
        private string Msg_16;
        private string Msg_17;
        private string Msg_18;
        private string Msg_19;
        private string Msg_PersonDetail;
        private static object lockobj = new object();
        private static frmAdditionalData onlyObj;

        public static frmAdditionalData GetForm(INMain main)
        {
            if (onlyObj == null)
            {
                lock (lockobj)
                {
                    if (onlyObj == null)
                    {
                        onlyObj = new frmAdditionalData(main);
                        frmMain.AddNodeForms(onlyObj);
                    }
                }
            }
            return onlyObj;
        }
        #endregion


        private frmAdditionalData(INMain main)
        {
            InitializeComponent();
            mMainForm = main;
            //  InitControl();
        }

        // string[] mUploadTypeList = new string[] { "人员头像照片", "指纹特征码", "红外人脸特征码", "动态人脸特征码" };
        // string[] mDownloadTypeList = new string[] { "人员头像", "指纹特征码", "记录照片", "红外人脸特征码", "动态人脸特征码" };
        /// <summary>
        /// 初始化控件
        /// </summary>
        private void InitControl()
        {
            var mUploadTypeList = Lng("UploadTypeList").Split(',');
            var mDownloadTypeList = Lng("DownloadTypeList").Split(',');
            cmbUploadType.Items.Clear();
            cmbUploadType.Items.AddRange(mUploadTypeList);
            cmbUploadType.SelectedIndex = 0;

            cmbDownloadType.Items.Clear();
            cmbDownloadType.Items.AddRange(mDownloadTypeList);
            cmbDownloadType.SelectedIndex = 0;

            chkFaceSoftware.Text = Lng("FaceSoftware");


            IniEquptType();
        }

        private void BtnGetPerson_Click(object sender, EventArgs e)
        {
            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;
            uint iUsercode = 0;
            if (!uint.TryParse(txtDownloadUserCode.Text, out iUsercode) || iUsercode < 0)
            {
                return;
            }
            ReadPersonDetail_Parameter par = new ReadPersonDetail_Parameter(iUsercode);
            ReadPersonDetail cmd = new ReadPersonDetail(cmdDtl, par);

            mMainForm.AddCommand(cmd);

            cmdDtl.CommandCompleteEvent += (sdr, cmde) =>
            {
                var result = cmd.getResult() as ReadPersonDetail_Result;
                StringBuilder strBuf = new StringBuilder();
                strBuf.Append(string.Format(Msg_PersonDetail + "\r\n", result.UserCode));
                for (int i = 0; i < result.PhotoList.Length; i++)
                {
                    strBuf.Append(Msg_Photo + $"{i + 1}:{(result.PhotoList[i] == 1 ? Msg_Have + "," : Msg_None + ",")}");
                }
                strBuf.Append("\r\n");
                for (int i = 0; i < result.FingerprintList.Length; i++)
                {
                    strBuf.Append(Msg_FP + $"{i + 1}:{(result.FingerprintList[i] == 1 ? Msg_Have + "," : Msg_None + ",")}");
                }
                strBuf.Append("\r\n");
                strBuf.Append(Msg_Face + (result.HasFace ? Msg_Have : Msg_None));
                strBuf.Append("\r\n");
                for (int i = 0; i < result.PalmList.Length; i++)
                {
                    strBuf.Append(Msg_Palm + $"{i + 1}:{(result.PalmList[i] == 1 ? Msg_Have + "," : Msg_None + ",")}");
                }
                mMainForm.AddCmdLog(cmde, strBuf.ToString());
            };
        }

        private void BtnDownload_Click(object sender, EventArgs e)
        {
            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;
            uint iUsercode = 0;
            if (!uint.TryParse(txtDownloadUserCode.Text, out iUsercode) || iUsercode < 0)
            {
                return;
            }
            if (cmbDownloadSerialNumber.SelectedIndex == -1)
            {
                return;
            }
            int serialNumber = Convert.ToInt32(cmbDownloadSerialNumber.SelectedItem);
            INCommand cmd;


            if (chkByBlock.Checked)
            {
                ReadFile_Parameter par = new ReadFile_Parameter(iUsercode, cmbDownloadType.SelectedIndex + 1, serialNumber);
                cmd = new ReadFile(cmdDtl, par);

            }
            else
            {
                ReadFeatureCode_Parameter par = new ReadFeatureCode_Parameter(iUsercode, cmbDownloadType.SelectedIndex + 1, serialNumber);
                cmd = new ReadFeatureCode(cmdDtl, par);
            }

            mMainForm.AddCommand(cmd);

            cmdDtl.CommandCompleteEvent += (sdr, cmde) =>
            {
                var result = cmd.getResult() as ReadFeatureCode_Result;
                if (result.FileHandle == 0)
                {
                    mMainForm.AddCmdLog(cmde, Msg_6);
                    return;
                }
                if (!result.Result)
                {
                    mMainForm.AddCmdLog(cmde, Msg_7);
                    return;
                }
                Invoke(() =>
                {
                    if (result.FileType == 1 || result.FileType == 3)
                    {
                        cmbUploadType.SelectedIndex = 0;
                        string sNewFile = System.IO.Path.Combine(Application.StartupPath, "Photo");
                        Directory.CreateDirectory(sNewFile);
                        sNewFile = System.IO.Path.Combine(sNewFile, $"tmpPhoto_{result.UserCode}.jpg");
                        File.WriteAllBytes(sNewFile, result.FileDatas);
                        ShowImgForm showImg = new ShowImgForm(Image.FromStream(new System.IO.MemoryStream(result.FileDatas)));
                        showImg.Show();
                        // pictureBox1.Image = Image.FromStream(new System.IO.MemoryStream(result.FileDatas));
                    }
                    else
                    {
                        txtCodeData.Text = Convert.ToBase64String(result.FileDatas);
                        string sNewFile = System.IO.Path.Combine(Application.StartupPath, "Code");
                        Directory.CreateDirectory(sNewFile);
                        sNewFile = System.IO.Path.Combine(sNewFile, $"Code_{result.UserCode}.bin");
                        File.WriteAllBytes(sNewFile, result.FileDatas);
                    }
                    if (result.FileType == 2) cmbUploadType.SelectedIndex = 1;
                    if (result.FileType == 4) cmbUploadType.SelectedIndex = 2;
                    if (result.FileType == 5) cmbUploadType.SelectedIndex = 3;
                    if (result.FileType == 6) cmbUploadType.SelectedIndex = 4;

                });

            };
        }

        private void CmbDownloadType_SelectedIndexChanged(object sender, EventArgs e)
        {//"人员头像,指纹特征码,记录照片,红外人脸特征码,动态人脸特征码,掌静脉特征码" />
            cmbDownloadSerialNumber.Items.Clear();
            if (cmbDownloadType.SelectedIndex == 0)//人员头像
            {
                string[] serialNumberList = new string[5];
                for (int i = 1; i <= 5; i++)
                {
                    serialNumberList[i - 1] = i.ToString();
                }
                cmbDownloadSerialNumber.Items.AddRange(serialNumberList);
                cmbDownloadSerialNumber.SelectedIndex = 0;
            }
            if (cmbDownloadType.SelectedIndex == 1)//指纹特征码
            {
                string[] serialNumberList = new string[10];
                for (int i = 0; i <= 9; i++)
                {
                    serialNumberList[i] = i.ToString();
                }
                cmbDownloadSerialNumber.Items.AddRange(serialNumberList);
                cmbDownloadSerialNumber.SelectedIndex = 0;
            }
            if (cmbDownloadType.SelectedIndex >=2 && cmbDownloadType.SelectedIndex <= 4)//2--记录照片;3--红外人脸特征码;4--动态人脸特征码
            {
                cmbDownloadSerialNumber.Items.Add("1");
                cmbDownloadSerialNumber.SelectedIndex = 0;
            }

            if (cmbDownloadType.SelectedIndex == 5)//5--掌静脉特征码
            {
                cmbDownloadSerialNumber.Items.Add("1");
                cmbDownloadSerialNumber.Items.Add("2");
                cmbDownloadSerialNumber.SelectedIndex = 0;
            }
        }

        private void CmbUploadType_SelectedIndexChanged(object sender, EventArgs e)
        {
            cmbUploadSerialNumber.Items.Clear();
            if (cmbUploadType.SelectedIndex == 2 || cmbUploadType.SelectedIndex == 3)//红外人脸特征码\动态人脸特征码
            {
                cmbUploadSerialNumber.Items.Add("1");
                cmbUploadSerialNumber.SelectedIndex = 0;
                tabControl1.SelectedIndex = 1;
            }
            if (cmbUploadType.SelectedIndex == 4)//掌静脉特征码
            {
                cmbUploadSerialNumber.Items.Add("1");
                cmbUploadSerialNumber.Items.Add("2");
                cmbUploadSerialNumber.SelectedIndex = 0;
                tabControl1.SelectedIndex = 1;
            }
            if (cmbUploadType.SelectedIndex == 0)//人员头像照片
            {
                string[] serialNumberList = new string[5];
                for (int i = 1; i <= 5; i++)
                {
                    serialNumberList[i - 1] = i.ToString();
                }
                cmbUploadSerialNumber.Items.AddRange(serialNumberList);
                cmbUploadSerialNumber.SelectedIndex = 0;
                tabControl1.SelectedIndex = 0;
            }
            if (cmbUploadType.SelectedIndex == 1)//指纹特征码
            {
                string[] serialNumberList = new string[10];
                for (int i = 0; i <= 9; i++)
                {
                    serialNumberList[i] = i.ToString();
                }
                cmbUploadSerialNumber.Items.AddRange(serialNumberList);
                cmbUploadSerialNumber.SelectedIndex = 0;
                tabControl1.SelectedIndex = 1;
            }
        }

        private void BtnDeleteCode_Click(object sender, EventArgs e)
        {
            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;
            uint iUsercode = 0;
            if (!uint.TryParse(txtUploadUserCode.Text, out iUsercode) || iUsercode < 0)
            {
                return;
            }
            byte[] fingerprintList = new byte[10];
            for (int i = 0; i < 10; i++)
            {
                fingerprintList[i] = 0;
            }
            byte[] photoList = new byte[5];
            for (int i = 0; i < 5; i++)
            {
                photoList[i] = 0;
            }

            byte[] palmList = new byte[2];//掌静脉特征码
            for (int i = 0; i < 2; i++)
            {
                palmList[i] = 0;
            }
            bool delFace = false;
            if (cmbUploadType.SelectedIndex == 0)//人员头像照片
            {
                photoList[cmbUploadSerialNumber.SelectedIndex] = 1;
            }
            if (cmbUploadType.SelectedIndex == 1)//指纹特征码
            {
                fingerprintList[cmbUploadSerialNumber.SelectedIndex] = 1;
            }
            if (cmbUploadType.SelectedIndex == 2 || cmbUploadType.SelectedIndex == 3)//红外人脸特征码/动态人脸特征码
            {
                delFace = true;
            }

            if (cmbUploadType.SelectedIndex == 4)//掌静脉特征码
            {
                palmList[cmbUploadSerialNumber.SelectedIndex] = 1;
            }

            DeleteFeatureCode_Parameter par = new DeleteFeatureCode_Parameter(iUsercode, photoList, fingerprintList, delFace, palmList);
            DeleteFeatureCode cmd = new DeleteFeatureCode(cmdDtl, par);

            mMainForm.AddCommand(cmd);
        }

        private void BtnUploadCode_Click(object sender, EventArgs e)
        {
            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;
            uint iUsercode = 0;
            if (cmbUploadType.SelectedIndex == 0) return;
            if (!uint.TryParse(txtUploadUserCode.Text, out iUsercode) || iUsercode < 0)
            {
                return;
            }
            if (string.IsNullOrWhiteSpace(txtCodeData.Text))
            {
                return;
            }
            byte[] datas = Convert.FromBase64String(txtCodeData.Text);
            WriteFeatureCode_Parameter par = new WriteFeatureCode_Parameter(iUsercode, cmbUploadType.SelectedIndex + 1, Convert.ToInt32(cmbUploadSerialNumber.SelectedItem), datas);
            WriteFeatureCode cmd = new WriteFeatureCode(cmdDtl, par);

            mMainForm.AddCommand(cmd);

            cmdDtl.CommandCompleteEvent += (sdr, cmde) =>
            {
                var result = cmde.Command.getResult() as WriteFeatureCode_Result;
                if (result.Result == 1)
                {
                    mMainForm.AddCmdLog(cmde, Msg_8);
                }
                else
                {
                    mMainForm.AddCmdLog(cmde, Msg_9 + $"：code={result.Result}");
                }
            };
        }

        private void BtnCompute_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtCodeData.Text))
            {
                return;
            }
            byte[] datas = Convert.FromBase64String(txtCodeData.Text);
            uint CRC32 = DoNetDrive.Common.Cryptography.CRC32_C.CalculateDigest(datas, 0, (uint)datas.Length);
            MessageBox.Show(Msg_10 + "：" + CRC32.ToString("x"));
        }



        /// <summary>
        /// 文件最大尺寸
        /// </summary>
        private const int ImageSizeMax = 100 * 1024;
        /// <summary>
        /// 进行图片转换,图片像素不能超过 480*640,大小尺寸不能超过50K
        /// </summary>
        /// <param name="strFile"></param>
        /// <returns></returns>
        private byte[] ConvertImage(byte[] bImage, out Bitmap newImage)
        {
            Image img = Image.FromStream(new System.IO.MemoryStream(bImage));
            float rate = 1;
            if ((img.Width != 480 && img.Height != 640) || bImage.Length > ImageSizeMax)
            {
                float rate1, rate2;

                rate1 = (float)480 / (float)img.Width;
                rate2 = (float)640 / (float)img.Height;
                rate = rate1 > rate2 ? rate2 : rate1;
                //if (rate > 1) rate = 1;

            }
            int iWidth = img.Width, iHeight = img.Height;
            iWidth = (int)(iWidth * rate);
            iHeight = (int)(iHeight * rate);
            byte[] newFile = null;
            ImageCodecInfo jgpEncoder = GetEncoder(ImageFormat.Jpeg);

            System.Drawing.Imaging.Encoder myEncoder = System.Drawing.Imaging.Encoder.Quality;
            // 创建一个EncoderParameters对象.
            // 一个EncoderParameters对象有一个EncoderParameter数组对象
            EncoderParameters myEncoderParameters = new EncoderParameters(1);



            using (Bitmap bimg = new Bitmap(480, 640, PixelFormat.Format32bppArgb))
            {
                using (Graphics graphics = Graphics.FromImage(bimg))
                {
                    graphics.PageUnit = GraphicsUnit.Pixel;
                    graphics.CompositingQuality = CompositingQuality.HighQuality;
                    graphics.SmoothingMode = SmoothingMode.HighQuality;
                    graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    graphics.Clear(Color.White);
                    graphics.DrawImage(img, new Rectangle((480 - iWidth) / 2, (640 - iHeight) / 2, iWidth, iHeight));
                    graphics.Dispose();
                }
                newImage = new Bitmap(bimg);

                //进行图片大小的测算
                long iQuality = 80;
                bool bSave = false;
                do
                {
                    EncoderParameter myEncoderParameter = new EncoderParameter(myEncoder, iQuality);//这里用来设置保存时的图片质量
                    myEncoderParameters.Param[0] = myEncoderParameter;
                    using (MemoryStream ms = new MemoryStream())
                    {
                        bimg.Save(ms, jgpEncoder, myEncoderParameters);
                        myEncoderParameter.Dispose();
                        if (ms.Length <= ImageSizeMax)
                        {
                            newFile = ms.GetBuffer();
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

        private ImageCodecInfo GetEncoder(ImageFormat format)
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



        private void ButUploadImage_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = Msg_11 + "|*.jpg;*.jpeg;*.bmp;*.png";
            ofd.Multiselect = false;
            if (ofd.ShowDialog() != DialogResult.OK) return;



            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;
            uint iUsercode = 0;
            if (!uint.TryParse(txtUploadUserCode.Text, out iUsercode) || iUsercode < 0)
            {
                return;
            }

            byte[] datas = System.IO.File.ReadAllBytes(ofd.FileName);
            Bitmap newImg;
            datas = ConvertImage(datas, out newImg);
            //pictureBox1.Image = newImg;
            ShowImgForm showImg = new ShowImgForm(newImg);
            showImg.Show();
            string sNewFile = System.IO.Path.Combine(Application.StartupPath, "tmpImage.jpg");
            File.WriteAllBytes(sNewFile, datas);

            WriteFeatureCode_Parameter par = new WriteFeatureCode_Parameter(iUsercode, 1, 1, datas);
            par.WaitRepeatMessage = true;
            WriteFeatureCode cmd = new WriteFeatureCode(cmdDtl, par);
            cmdDtl.Timeout = 5000;
            mMainForm.AddCommand(cmd);

            cmdDtl.CommandCompleteEvent += (sdr, cmde) =>
            {
                var result = cmde.Command.getResult() as WriteFeatureCode_Result;
                if (result.Result == 1)
                {
                    mMainForm.AddCmdLog(cmde, Msg_12);
                }
                else
                {
                    if (result.Result == 4)
                    {
                        mMainForm.AddCmdLog(cmde, Msg_13 + result.RepeatUser);
                    }
                    else
                        mMainForm.AddCmdLog(cmde, Msg_14 + $"：code={result.Result}");
                }
            };
        }
        #region 上传固件

        private class EquptAESKey
        {
            public string Name;
            /// <summary>
            /// AES 密码
            /// </summary>
            public string Key;
            /// <summary>
            /// 
            /// </summary>
            public int PacketByteLen;

            public EquptAESKey(string sName, string sKey, int iPLen)
            {
                Name = sName;
                Key = sKey;
                PacketByteLen = iPLen;
            }
        }
        private Dictionary<String, EquptAESKey> mAesKey;

        private void IniEquptType()
        {
            mAesKey = new Dictionary<string, EquptAESKey>();

            var typeFile = Lng("EquptTypeList");
            typeFile = Path.Combine(Directory.GetCurrentDirectory(), typeFile);
            if (!File.Exists(typeFile))
            {
                return;
            }
            var sTypes = File.ReadAllText(typeFile, Encoding.UTF8).Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);

            foreach (var sTypeLine in sTypes)
            {
                var sTypeDetails = sTypeLine.Split(',');
                var oKey = new EquptAESKey(sTypeDetails[0], sTypeDetails[1], sTypeDetails[2].ToInt32());
                mAesKey.Add(oKey.Name, oKey);

            }




            cmbEquptType.Items.Clear();
            cmbEquptType.Items.AddRange(mAesKey.Keys.ToArray());
            cmbEquptType.SelectedIndex = 0;

        }

        private void ButUploadSoftware_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(cmbEquptType.Text)) return;

            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = Msg_15 + "|*.RCBin";
            ofd.Multiselect = false;
            if (ofd.ShowDialog() != DialogResult.OK) return;

            string sFile = ofd.FileName;
            if (!File.Exists(sFile)) return;

            var oItem = mAesKey[cmbEquptType.Text];
            int iRCPacketByteLen = oItem.PacketByteLen;
            var bSurFile = File.ReadAllBytes(sFile);
            int iFileLen = bSurFile.Length;

            var sKey = Encoding.ASCII.GetString(bSurFile, 0, 16);
            if (!sKey.Equals(oItem.Key))
            {
                MsgErr(Msg_16);
                return;
            }
            string sVer = $"{bSurFile[16]}.{bSurFile[17]}";

            byte[] iCRCBuf = bSurFile.Copy(18, 4);
            uint iSoftwareCRC32 = iCRCBuf.ToInt32();
            int iSoftwareSize = iFileLen - 26;
            uint iFileCRC32 = bSurFile.Copy(iFileLen - 4, 4).ToInt32();
            byte[] bSoftWareData = bSurFile.Copy(22, iSoftwareSize);
            uint itmpCRC32 = DoNetDrive.Common.Cryptography.CRC32_C.CalculateDigest(bSoftWareData, 0, (uint)iSoftwareSize);
            if (itmpCRC32 != iFileCRC32)
            {
                MsgErr(Msg_17);
                return;
            }


            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;
            var par = new Software.UpdateSoftware_Parameter(bSoftWareData, iSoftwareCRC32);
            INCommand cmd = null;
            if (chkFaceSoftware.Checked)
            {
                cmd = new Software.UpdateSoftware(cmdDtl, par);
            }
            else
            {
                cmd = new Software.UpdateSoftware_FP(cmdDtl, par);
            }

            cmdDtl.Timeout = 500;
            cmdDtl.RestartCount = 10;
            par.SkipTimeoutPacket = chkSkipTimeoutPacket.Checked;

            mMainForm.AddCommand(cmd);

            cmdDtl.CommandCompleteEvent += (sdr, cmde) =>
            {
                var result = cmde.Command.getResult() as Software.UpdateSoftware_Result;
                if (result.Success == 1)
                {
                    mMainForm.AddCmdLog(cmde, Msg_18);
                    if (result.SkipPacketCount > 0)
                    {
                        mMainForm.AddCmdLog(cmde, $"SkipPacketCount={result.SkipPacketCount}");
                    }
                }
                else
                {
                    mMainForm.AddCmdLog(cmde, Msg_19 + $"：code={result.Success}");
                }
            };

        }

        private void CreateCRC32()
        {

        }


        #endregion

        private void frmAdditionalData_Load(object sender, EventArgs e)
        {
            LoadUILanguage();

        }

        public override void LoadUILanguage()
        {
            base.LoadUILanguage();
            Lng(gpUpload);
            Lng(Lbl_UploadUserCode);
            Lng(Lbl_UploadType);
            Lng(Lbl_UploadSerialNumber);
            Lng(tabPage2);
            Lng(tabPage1);
            Lng(butUploadImage);
            Lng(Lbl_CodeData);
            Lng(btnDeleteCode);
            Lng(btnCompute);
            Lng(btnUploadCode);
            Lng(gpDownload);
            Lng(Lbl_DownloadUserCode);
            Lng(Lbl_DownloadType);
            Lng(Lbl_DownloadSerialNumber);
            Lng(btnGetPerson);
            Lng(chkByBlock);
            Lng(btnDownload);
            Lng(gpUpdateSoftware);
            Lng(Lbl_EquptType);
            Lng(butUpdateSoftware);
            Lng(butUploadFolder);
            Msg_Have = Lng("Msg_1");
            Msg_None = Lng("Msg_2");
            Msg_Photo = Lng("Msg_3");
            Msg_FP = Lng("Msg_4");
            Msg_Face = Lng("Msg_5");
            Msg_Palm = Lng("Msg_Palm");
            Msg_6 = Lng("Msg_6");
            Msg_7 = Lng("Msg_7");
            Msg_8 = Lng("Msg_8");
            Msg_9 = Lng("Msg_9");
            Msg_10 = Lng("Msg_10");
            Msg_11 = Lng("Msg_11");
            Msg_12 = Lng("Msg_12");
            Msg_13 = Lng("Msg_13");
            Msg_14 = Lng("Msg_14");
            Msg_15 = Lng("Msg_15");
            Msg_16 = Lng("Msg_16");
            Msg_17 = Lng("Msg_17");
            Msg_18 = Lng("Msg_18");
            Msg_19 = Lng("Msg_19");
            Msg_PersonDetail = Lng("Msg_PersonDetail");

            Lng(btnSaveBase64ToBin);
            InitControl();
        }

        private void Lbl_DownloadType_Click(object sender, EventArgs e)
        {

        }

        #region 上传文件夹
        private List<string> mFiles;
        private int mFileCount;
        private int mUploadFolderIndex;
        private uint mUploadFolderUserCode;
        private INCommandDetail mUploadCommandDtl;
        private bool mUploading;
        private bool mUploadIsRun;

        private void butUploadFolder_Click(object sender, EventArgs e)
        {
            if (mUploading)
            {
                mUploadIsRun = false;
            }
            else
            {
                OpenFileDialog sfd = new OpenFileDialog();

                sfd.Filter = Lng("UF_Msg_19");//"选择人员照片文件夹(*.JPG)|*.JPG";
                sfd.AddExtension = true;
                sfd.AutoUpgradeEnabled = true;
                sfd.Title = Lng("UF_Msg_18");// "选择人员照片";
                sfd.FilterIndex = 0;
                sfd.RestoreDirectory = true;

                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    var sDir = Path.GetDirectoryName(sfd.FileName);
                    var oFiles = Directory.EnumerateFiles(sDir, "*.jpg");
                    var oFileList = new List<string>(oFiles);
                    if (oFileList.Count == 0)
                    {
                        //MessageBox.Show("没有找到图片", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        MessageBox.Show(Lng("UF_Msg_16"), Lng("UF_Msg_17"),
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    //bool bClear = MessageBox.Show("是否清空人员资料", "提示",
                    //    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes;
                    bool bClear = MessageBox.Show(Lng("UF_Msg_15"), Lng("UF_Msg_14"),
                        MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes;
                    UploadFolder(oFileList, bClear);

                }
            }

        }
        private void SetRowTip(string sLog, Color color)
        {
            mMainForm.AddLog(sLog);
        }
        public void UploadFolder(List<string> sFiles, bool bClearPeople)
        {

            mFileCount = sFiles.Count;
            if (mFileCount == 0) return;
            mUploadIsRun = true;
            mUploading = true;

            mUploadFolderIndex = -1;
            mUploadFolderUserCode = 800000 - 1;
            mFiles = new List<string>(sFiles);
            mUploadCommandDtl = mMainForm.GetCommandDetail();
            if (mUploadCommandDtl == null)
            {
                mUploadIsRun = false;
                mUploading = false;
                return;
            }
                
            mUploadCommandDtl.CommandTimeout += MUploadCommandDtl_CommandTimeout;
            mUploadCommandDtl.CommandErrorEvent += MUploadCommandDtl_CommandErrorEvent;
            if (bClearPeople)
            {

                mUploadCommandDtl.Timeout = 15000;
                var uploadcmd = new ClearPersonDataBase(mUploadCommandDtl);
                mUploadCommandDtl.CommandCompleteEvent += UploadFolder_ClearPerson_CommandCompleteEvent;
                mMainForm.AddCommand(uploadcmd);
                //SetRowTip("正在清空人事资料", Color.Magenta);//正在清空人事资料
                SetRowTip(Lng("UF_Msg_13"), Color.Magenta);

            }
            else
            {
                //SetRowTip("开始批量上传", Color.Magenta);//正在清空人事资料
                SetRowTip(Lng("UF_Msg_12"), Color.Magenta);
                UploadNext();
            }

        }

        private void MUploadCommandDtl_CommandErrorEvent(object sender, CommandEventArgs e)
        {
            MUploadCommandDtl_CommandTimeout(sender, e);
        }

        private void MUploadCommandDtl_CommandTimeout(object sender, CommandEventArgs e)
        {
            //SetRowTip("上传失败", Color.Red);
            SetRowTip(Lng("UF_Msg_11"), Color.Red);
            //mUploadIsRun = false;
            //mUploading = false;

            Task.Run(() =>
            {
                do
                {
                    Task.Delay(10000);

                    
                    this.Invoke(() =>
                    {
                        mUploadCommandDtl = mMainForm.GetCommandDetail(false);

                    });
                    if (mUploadCommandDtl != null)
                    {
                        mUploadCommandDtl.CommandTimeout += MUploadCommandDtl_CommandTimeout;
                        mUploadCommandDtl.CommandErrorEvent += MUploadCommandDtl_CommandErrorEvent;
                        mUploadFolderIndex--;
                        UploadNext();
                        break;
                    }
                } while (true);


            });
        }

        private void UploadFolder_ClearPerson_CommandCompleteEvent(object sender, CommandEventArgs e)
        {
            //SetRowTip("清空人事资料完毕", Color.Blue);
            SetRowTip(Lng("UF_Msg_10"), Color.Blue);

            UploadNext();
        }

        private void UploadNext()
        {
            mUploadFolderIndex++;
            if (mUploadFolderIndex >= mFileCount)
            {
                //SetRowTip("上传完毕", Color.Blue);
                SetRowTip(Lng("UF_Msg_9"), Color.Blue);
                mUploadIsRun = false;
                mUploading = false;

                return;
            }

            if (!mUploadIsRun)
            {
                //SetRowTip("上传已被中止完毕", Color.Blue);
                SetRowTip(Lng("UF_Msg_8"), Color.Red);
                mUploadIsRun = false;
                mUploading = false;

                return;
            }
            string sTip = Lng("UF_Msg_7");//"正在上传人员 {0}/{1}";
            sTip = string.Format(sTip, mUploadFolderIndex + 1, mFileCount);
            SetRowTip(sTip, Color.Magenta);
            int iOptStep = 1;
            try
            {
                //上传人员
                var cmdDtl = mUploadCommandDtl;
                cmdDtl.CommandCompleteEvent -= UploadFolder_CommandCompleteEvent;
                cmdDtl.CommandCompleteEvent -= UploadFolder_ClearPerson_CommandCompleteEvent;
                cmdDtl.CommandCompleteEvent += UploadFolder_CommandCompleteEvent;

                cmdDtl.Timeout = 600;
                mUploadFolderUserCode++;

                #region 生成人员资料
                string sImageFile = mFiles[mUploadFolderIndex];
                string sName = System.IO.Path.GetFileNameWithoutExtension(sImageFile);
                var oUser = new DoNetDrive.Protocol.Fingerprint.Data.Person(mUploadFolderUserCode, sName);
                byte[] datas = System.IO.File.ReadAllBytes(sImageFile);
                int iMaxFileLen = 122880;
                datas = ImageTool.ConvertImage(datas, 480, 640, iMaxFileLen);
                iOptStep = 2;
                var oIdt = new IdentificationData(1, datas);
                #endregion

                iOptStep = 3;
                var PersonPar = new AddPersonAndImage_Parameter(oUser, oIdt);
                PersonPar.WaitVerifyTime = 3000;
                PersonPar.WaitRepeatMessage = true;
                var uploadcmd = new AddPeosonAndImage(cmdDtl, PersonPar);

                iOptStep = 4;
                mMainForm.AddCommand(uploadcmd);
                iOptStep = 5;
            }
            catch (Exception ex)
            {

                //Trace.WriteLine($"上传文件夹时发生错误,错误步骤：{iOptStep} 错误信息：{ex.Message}");//进度报告;
                UploadNext();
            }


        }

        /// <summary>
        /// 上传一个人员完毕
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UploadFolder_CommandCompleteEvent(object sender, CommandEventArgs e)
        {
            string sRegResult, sImgResult = string.Empty;
            AddPersonAndImage_Result Result = e.Result as AddPersonAndImage_Result;
            //sRegResult = Result.UserUploadStatus ? "成功" : "失败";
            sRegResult = Result.UserUploadStatus ? Lng("UF_Msg_5") : Lng("UF_Msg_6");
            int sStatus = Result.IdDataUploadStatus[0];
            if (Result.UserUploadStatus)
            {
                switch (sStatus)
                {
                    case 1:
                        sImgResult = Lng("UF_Msg_4");//"成功";
                        break;
                    case 3:
                        sImgResult = Lng("UF_Msg_3");//"照片不可识别";
                        break;
                    case 4:
                        sImgResult = Lng("UF_Msg_2");// "照片和编号：{0} 重复";
                        sImgResult = string.Format(sImgResult, Result.IdDataRepeatUser[0]);
                        break;
                    default:
                        break;
                }
            }
            string sTip = Lng("UF_Msg_1");//"用户号：{0},人员注册结果：{1}；照片注册结果：{2}；";
            sTip = string.Format(sTip, mUploadFolderUserCode, sRegResult, sImgResult);
            SetRowTip(sTip, Color.Magenta);
            UploadNext();
        }
        #endregion

        private void btnSaveBase64ToBin_Click(object sender, EventArgs e)
        {
            string sBase64 = txtCodeData.Text;
            if (string.IsNullOrEmpty(sBase64))
                return;

            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Title = Lng("btnSaveBase64ToBin");
            saveFileDialog.Filter = "(*.bin)|*.bin";
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                var sFileName = saveFileDialog.FileName;
                if (File.Exists(sFileName))
                {
                    File.Delete(sFileName);
                }

                try
                {
                    File.WriteAllBytes(sFileName, Convert.FromBase64String(sBase64));
                }
                catch (Exception ex)
                {

                    MsgErr(ex.Message);
                }

            }
        }
    }
}
