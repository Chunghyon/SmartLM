using FaceWebServer.DB.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaceWebServer.DTO.Access
{
    /// <summary>
    /// 人员权限详情
    /// </summary>
    public class DeviceAccessDetailDTO : PeopleAccessDetail
    {
        /// <summary>
        /// 设备 SN
        /// </summary>
        public string SN { get; set; }

        /// <summary>
        /// 协议类型  HTTPv1   HTTPv2  MQTT  Websocket
        /// </summary>
        public string Protocol { get; set; }

        /// <summary>
        /// 设备名称
        /// </summary>
        public string DeviceName { get; set; }


        /// <summary>
        /// 用户号 （数字 最大值 4294967295 类型 UINT32）
        /// </summary>
        public long UserID { get; set; } //是
        /// <summary>
        /// 人员姓名（字符<32位）
        /// </summary>
        public string Name { get; set; } //是
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
        /// 照片
        /// </summary>
        public string? Photo { get; set; }

        /// <summary>
        /// 图片文件大小 字节
        /// </summary>
        public int PhotoLen { get; set; }

        /// <summary>
        /// 密码，纯数字,长度：（0 / 4-8）
        /// </summary>
        public string Password { get; set; }


        /// <summary>
        /// IC卡号 纯数字
        /// </summary>
        public ulong CardNum { get; set; }


        /// <summary>
        /// 二维码
        /// </summary>
        public string? QRCode { get; set; }

        /// <summary>
        /// 人脸数量
        /// </summary>
        public int FaceNum { get; set; }

        /// <summary>
        /// 掌静脉数量
        /// </summary>
        public int PalmveinsNum { get; set; }


        /// <summary>
        /// 指纹数量
        /// </summary>
        public int FingerprintsNum { get; set; }
    }
}
