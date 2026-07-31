using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaceWebServer.DTO.Config
{
    public class HTTPProtocolOption
    {
        public const string UseURL = "URL";
        public const string UseBase64 = "Base64";

        /// <summary>
        /// 上传人员照片的方式，可选值 URL 或者 Base64
        /// </summary>
        public string UploadPeoplePhotoType { get; set; }

        /// <summary>
        /// 上传人员照片时，携带MD5
        /// </summary>
        public bool UploadPeoplePhotoMd5 { get; set; }
        /// <summary>
        /// 上传人员保存结果,不保存会造成无限循环
        /// </summary>
        public bool UploadPeopleSaveResult { get; set; }

        public List<DenySNDetail> DenySN { get; set; }

        public bool TestQRCodeAndOpenDoor { get; set; }

        /// <summary>
        /// 人员照片的URL前缀,例如 http://192.168.1.42:4610
        /// </summary>
        public string PeopleURLPrefix { get; set; }

        /// <summary>
        /// HTTP协议中下发人员的时候是否需要URL前缀，如果为True则会再url前增加 PeopleURLPrefix 变量的值
        /// 如果为False则不会增加
        /// </summary>
        public bool HTTPURLUsePrefix { get; set; }
        /// <summary>
        /// 显示SQL日志
        /// </summary>
        public bool ShowSQLLog { get; set; } = false;

        /// <summary>
        /// 保存 IO日志
        /// </summary>
        public bool SaveIOLog { get; set; } = false;

        /// <summary>
        /// 保存Keepalive日志
        /// </summary>
        public bool SaveKeepaliveLog { get; set; } = false;

        /// <summary>
        /// 指纹特征码下发时使用base64编码，否则使用URL
        /// </summary>
        public bool FeatureCodeUseBase64 { get; set; } = true;

        /// <summary>
        /// MQTT人员照片使用文件方式，否则使用URL或base64
        /// </summary>
        public bool MQTTPeoplePhotoUseBytes { get; set; } = true;

        public DenySNDetail Find(string SN)
        {
            if (DenySN == null) return null;
            foreach (var d in DenySN)
                if (d.SN.Equals(SN))
                    return d;

            return null;
        }

        public Dictionary<string, DenySNDetail> GetDict()
        {
            var dict = new Dictionary<string, DenySNDetail>();

            foreach (var d in DenySN)
                dict[d.SN] = d;

            return dict;
        }

    }
    public class DenySNDetail
    {
        public string SN { get; set; }

        public int Code { get; set; }

        public string Msg { get; set; }
    }
}
