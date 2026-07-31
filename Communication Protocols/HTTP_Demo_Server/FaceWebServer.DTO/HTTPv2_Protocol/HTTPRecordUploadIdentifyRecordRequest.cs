using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaceWebServer.DTO.HTTPv2_Protocol
{
    /// <summary>
    /// HTTPv2 协议 设备推送人员打卡记录的请求 /Record/UploadIdentifyRecord
    /// </summary>
    public class HTTPRecordUploadIdentifyRecordDetail: HTTPRecordUploadSystemRecordDetail
    {
        /// <summary>
        /// 用户号 （数字 最大值 4294967295 类型 UINT32）
        /// </summary>
        public long UserID { get; set; }

        /// <summary>
        /// 人员姓名（字符<64位）
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// 职务（字符<64位）
        /// </summary>
        public string? Job { get; set; }

        /// <summary>
        /// 部门（字符<64位）
        /// </summary>
        public string? Department { get; set; }

        /// <summary>
        /// 身份证号码 （字符<64位）
        /// </summary>
        public string? IdentityCard { get; set; }

        /// <summary>
        /// 卡号 （数字，最大值 18446744073709551615  类型 UINT62）
        /// </summary>
        public string? CardNum { get; set; }

        /// <summary>
        /// 人员二维码信息 （字符<128位）
        /// </summary>
        public string? QRCode { get; set; }


        /// <summary>
        /// 打卡照片地址
        /// </summary>
        public string? Photo { get; set; }

        /// <summary>
        /// 照片的长度 最大支持400KB的图片
        /// </summary>
        public int PhotoLen { get; set; }

        /// <summary>
        /// 是否为进入，1表示进入，0表示出门
        /// </summary>
        public int IsEntry { get; set; }

        /// <summary>
        /// 人体测量温度 需要除10
        /// </summary>
        public float BodyTemp { get; set; }
    }
}
