using FaceWebServer.DTO;
using FaceWebServer.Utility.VerifyAttribute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FaceWebServer.DTO.Record
{
    /// <summary>
    /// 查询设备的参数
    /// </summary>
    public class IdentifyReportQueryDTO : SystemReportQueryDTO
    {
        /// <summary>
        /// 用户号
        /// </summary>
        public long? UserID { get; set; } //是
        /// <summary>
        /// 人员姓名（字符<32位）
        /// </summary>
        public string? Name { get; set; } //是
        /// <summary>
        /// 职务
        /// </summary>
        public string? Job { get; set; } //否
        /// <summary>
        /// 部门
        /// </summary>
        public string? Department { get; set; }

        /// <summary>
        /// 身份证
        /// </summary>
        public string? IdentityCard { get; set; }

        /// <summary>
        /// IC卡号 纯数字
        /// </summary>
        public ulong? CardNum { get; set; }


        /// <summary>
        /// 二维码
        /// </summary>
        public string? QRCode { get; set; }

        /// <summary>
        /// 是否为进入，1表示进入，0表示出门
        /// </summary>
        public int? IsEntry { get; set; }

        



    }
}
