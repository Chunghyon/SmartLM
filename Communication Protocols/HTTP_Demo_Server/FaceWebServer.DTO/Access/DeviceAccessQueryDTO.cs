using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaceWebServer.DTO.Access
{
    /// <summary>
    /// 查询人员权限列表的查询条件
    /// </summary>
    public class DeviceAccessQueryDTO: BasePageParameter
    {
        public int? AccessID { get; set; }

        public int? PeopleID { get; set; }

        public int? DeviceID { get; set; }

        /// <summary>
        /// 设备 SN
        /// </summary>
        public string? SN { get; set; }

        /// <summary>
        /// 协议类型  HTTPv1   HTTPv2  MQTT  Websocket
        /// </summary>
        public string? Protocol { get; set; }

        /// <summary>
        /// 设备名称
        /// </summary>
        public string? DeviceName { get; set; }


        /// <summary>
        /// 用户号 （数字 最大值 4294967295 类型 UINT32）
        /// </summary>
        public long? UserID { get; set; } 

        /// <summary>
        /// 人员姓名（字符<32位）
        /// </summary>
        public string? Name { get; set; }


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
        /// 有二维码   1--有；0--没有
        /// </summary>
        public int? QRCode { get; set; }

        /// <summary>
        /// 人脸数量
        /// </summary>
        public int? Face { get; set; }

        /// <summary>
        /// 掌静脉数量
        /// </summary>
        public int? Palmveins { get; set; }


        /// <summary>
        /// 指纹数量
        /// </summary>
        public int? Fingerprints { get; set; }




        /// <summary>
        /// 人员角色 0,普通人员；1，管理员;2 黑名单
        /// </summary>
        public int? AccessType { get; set; }


        /// <summary>
        /// 上传状态：0--未上传；1--已上传；2--待删除
        /// </summary>
        public int? UploadStatus { get; set; }


        /// <summary>
        /// 上传结果：1--正常； 2--有异常
        /// </summary>
        public int? UploadResult { get; set; }
    }
}
