using System;

namespace FaceWebServer.DTO.HTTPv1_Protocol
{
    /// <summary>
    /// HTTPv1 协议版本中的人员信息
    /// </summary>
    public class HTTPPeopleV1
    {
        /// <summary>
        /// 人员编号（数字<20位）
        /// </summary>
        public long EmployeeID { get; set; } //是
        /// <summary>
        /// 人员姓名（字符<32位）
        /// </summary>
        public string EmployeeName { get; set; } //是
        /// <summary>
        /// 职务
        /// </summary>
        public string EmployeeJob { get; set; } //否

        /// <summary>
        /// 身份证信息
        /// </summary>
        public string EmployeeIdentity { get; set; } //否



        /// <summary>
        /// 密码，纯数字,长度：（0 / 4-8）
        /// </summary>
        public string EmployeePassword { get; set; } //否
        /// <summary>
        /// IC卡号 纯数字
        /// </summary>
        public ulong EmployeeIc { get; set; } //否
        /// <summary>
        /// 照片格式”base”:bse64 编码字符，”path”:下载路径
        /// </summary>
        public string EmployeePhotoWay { get; set; } //是
        /// <summary>
        /// 照片
        /// </summary>
        public string EmployeePhoto { get; set; } //是
        /// <summary>
        /// 照片的MD5
        /// </summary>
        public string PhotoMD5 { get; set; } //是

        /// <summary>
        /// 角色 0,普通人员；1，管理员
        /// </summary>
        public int EmployeeRoot { get; set; }

        /// <summary>
        /// 开门时段组 1-64
        /// </summary>
        public int TimegroupID { get; set; }

        /// <summary>
        /// 人脸识别阈值，0:未设置，其他：阈值大小
        /// </summary>
        public float EmployeeShold { get; set; } //否


        /// <summary>
        /// 权限结构 人员权限信息
        /// </summary>
        public HTTPPeopleV1_AccessDetail DevicePassBean { get; set; }

    }

    /// <summary>
    /// HTTPv1 协议版本中的人员信息 的权限信息
    /// </summary>
    public class HTTPPeopleV1_AccessDetail
    {
        public const string DateTimeFormat = "yyyy-MM-dd HH:mm:ss";
        /// <summary>
        /// 通行开始时间（年-月-日 时：分：秒，例 如：”2020-03-06 16:45:20”）
        /// </summary>
        public DateTime DevicePassStart { get; set; }
        /// <summary>
        /// 通行结束时间（年-月-日 时：分：秒，例 如：”2020-03-06 16:45:20”）
        /// </summary>
        public DateTime DevicePassEnd { get; set; }

        /// <summary>
        /// 0:通行时间不限制 1:根据通行时间限制
        /// </summary>
        public int DevicePassTimeOver { get; set; }

        /// <summary>
        /// 0无限通行次数，其他：通行次数；-1表示次数已用尽
        /// </summary>
        public int DevicePassNumber { get; set; }

        public void SetAccessDeadline(DateTime dateTime)
        {
            DevicePassStart = DateTime.Now.AddYears(-1);
            DevicePassEnd = dateTime;
        }
    }

}
