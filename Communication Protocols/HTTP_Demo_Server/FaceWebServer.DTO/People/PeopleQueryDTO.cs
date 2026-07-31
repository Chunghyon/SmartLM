namespace FaceWebServer.DTO.People
{
    /// <summary>
    /// 人员查询接口参数
    /// </summary>
    public class PeopleQueryDTO : BasePageParameter
    {
        /// <summary>
        /// 主键
        /// </summary>
        public int? ID { get; set; }

        /// <summary>
        /// 用户号 （数字 最大值 4294967295 类型 UINT32）
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
        /// 有图片  1--有；0--没有
        /// </summary>
        public int? Photo { get; set; }

        /// <summary>
        /// 有密码  1--有；0--没有
        /// </summary>
        public int? Password { get; set; }


        /// <summary>
        /// IC卡号 纯数字
        /// </summary>
        public ulong? CardNum { get; set; }

        /// <summary>
        /// 二维码   
        /// </summary>
        public string? QRCode { get; set; }

        /// <summary>
        /// 有二维码   1--有；0--没有
        /// </summary>
        public int? UseQRCode { get; set; }

        /// <summary>
        /// 有人脸  1--有；0--没有
        /// </summary>
        public int? Face { get; set; }

        /// <summary>
        /// 有掌静脉   1--有；0--没有
        /// </summary>
        public int? Palmveins { get; set; }


        /// <summary>
        /// 有指纹   1--有；0--没有
        /// </summary>
        public int? Fingerprints { get; set; }
    }
}
