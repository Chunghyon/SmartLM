using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace FaceWebServer.DTO.HTTPv2_Protocol
{
    /// <summary>
    /// HTTPv2 协议版本中的人员信息
    /// </summary>
    public class HTTPPeopleV2
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
        /// 其他人员信息,自定义格式字符串 （字符<200位）
        /// </summary>
        public string? Attachment { get; set; }


        /// <summary>
        /// 照片信息，http或https开头表示使用url地址，否则表示使用base64
        /// </summary>
        /// <summary>
        ///  斜杠开头说明使用人脸机本地文件
        /// </summary>
        public string? Photo { get; set; }

        /// <summary>
        /// 照片的MD5  HEX字符串格式 （字符=32位）
        /// </summary>
        public string? PhotoMD5 { get; set; }

        /// <summary>
        /// 照片的长度 最大支持400KB的图片
        /// </summary>
        public int PhotoLen { get; set; }

        /// <summary>
        /// 密码,纯数字,长度：（0 / 4-8） 
        /// </summary>
        public string? Password { get; set; }

        /// <summary>
        /// 卡号 （数字，最大值 18446744073709551615  类型 UINT62）
        /// </summary>
        public ulong CardNum { get; set; }

        /// <summary>
        /// 人员二维码信息 （字符<128位）
        /// </summary>
        public string? QRCode { get; set; }




        /// <summary>
        /// 指纹对象
        /// </summary>
        public List<HTTPPeopleFeatureCode>? Fingerprints { get; set; }

        /// <summary>
        /// 掌纹对象
        /// </summary>
        public List<HTTPPeopleFeatureCode>? Palmveins { get; set; }


        /// <summary>
        /// 人脸特征码
        /// </summary>
        public string? FaceFeature { get; set; }

        /// <summary>
        /// 人脸特征码的MD5值 HEX字符串格式
        /// </summary>
        public string? FaceFeatureMD5 { get; set; }






        /// <summary>
        /// 角色 0--普通人员；1--管理员； 2--黑名单
        /// </summary>
        public int AccessType { get; set; }

        /// <summary>
        /// unix 时间戳 秒级   0 表示无期限
        /// </summary>
        public long ExpirationDate { get; set; }

        /// <summary>
        /// 开门次数 0-65535；  65535--表示无限制，0--表示禁止通行
        /// </summary>
        public int OpenTimes { get; set; }

        /// <summary>
        /// 是否为常开卡，1--是；0--否
        /// </summary>
        public int KeepOpen { get; set; }

        /// <summary>
        /// 开门时段组 1-64 0--表示受限制
        /// </summary>
        public int Timegroup { get; set; }

        /// <summary>
        /// 节假日限制 逗号分隔：1,2,3,4,5
        /// </summary>
        public string? Holidays { get; set; }

        /// <summary>
        /// 电梯端口权限数组 逗号分隔：1,2,3,4,5
        /// </summary>
        public string? Elevators { get; set; }
    }
}
