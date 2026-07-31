using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaceWebServer.DTO.Access
{
    /// <summary>
    /// 查询人员权限列表的返回值
    /// </summary>
    public class DeviceAccessQueryResultDTO
    {
        public int AccessID { get; set; }

        public int PeopleID { get; set; }

        public int DeviceID { get; set; }

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
        public string? Name { get; set; } //是

        /// <summary>
        /// 照片
        /// </summary>
        public string? Photo { get; set; }

        /// <summary>
        /// 图片文件大小 字节
        /// </summary>
        public int? PhotoLen { get; set; }

        /// <summary>
        /// 密码，纯数字,长度：（0 / 4-8）
        /// </summary>
        public string Password { get; set; }


        /// <summary>
        /// IC卡号 纯数字
        /// </summary>
        public ulong? CardNum { get; set; }


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




        /// <summary>
        /// 人员角色 0,普通人员；1，管理员;2 黑名单
        /// </summary>
        public int AccessType { get; set; }

        //截止日期  unix 时间戳 秒级  
        public DateTime ExpirationDate { get; set; }

        /// <summary>
        /// 开门次数 0-65535；  65535--表示无限制，0--表示禁止通行
        /// </summary>
        public int OpenTimes { get; set; }

        /// <summary>
        /// 开门时段组号
        /// </summary>
        public int Timegroup { get; set; }


        /// <summary>
        /// 上传时间
        /// </summary>
        public DateTime UploadTime { get; set; }

        /// <summary>
        /// 上传状态：0--未上传；1--已上传；2--待删除
        /// </summary>
        public int UploadStatus { get; set; }

        /// <summary>
        /// 上传结果： 0 --无操作；1--正常；
        /// HTTPv1 返回值为 100-999  HTTPv2 返回值为 1000-1999
        /// </summary>
        public int UploadResult { get; set; }

        /// <summary>
        /// 重复人员编号：如果是上传后，照片重复时，此ID记录跟谁重复
        /// </summary>
        public long RepeatID { get; set; }

        /// <summary>
        /// 上传发生异常时，设备返回的异常描述
        /// </summary>
        public string? UploadResultMsg { get; set; }
    }


    public class DeviceAccessQueryAccessIDResultDTO
    {
        public int AccessID { get; set; }
        public int DeviceID { get; set; }
        public long UserID { get; set; } //是
    }
}
