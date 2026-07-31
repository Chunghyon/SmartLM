using FaceWebServer.Interface;
using FaceWebServer.Utility.VerifyAttribute;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using FaceWebServer.DTO;

namespace DeviceProtocolServer.Controllers.User.UserLog
{
    /// <summary>
    /// 用户日志查询参数
    /// </summary>
    public class UserLogQueryParameter : BasePageParameter
    {
        [VerifyText(iMax: 30, iMin: 0, required: false,
            errCode: 1, sErrorDesc: "用户名由1-30个字符组成", lngcode: "r106")]
        public string? UserName { get; set; }

        [VerifyText(iMax: 30, iMin: 0, required: false,
            errCode: 2, sErrorDesc: "类别由1-30个字符组成", lngcode: "r107")]
        public string? LogType { get; set; }

        [VerifyText(iMax: 60, iMin: 0, required: false,
            errCode: 3, sErrorDesc: "设备信息由1-60个字符组成", lngcode: "r108")]
        public string? LogDrive { get; set; }

        [VerifyText(iMax: 60, iMin: 0, required: false,
            errCode: 4, sErrorDesc: "人员信息由1-60个字符组成", lngcode: "r109")]
        public string? LogPeople { get; set; }

        [VerifyText(iMax: 60, iMin: 0, required: false,
            errCode: 5, sErrorDesc: "日志详情由1-60个字符组成", lngcode: "r110")]
        public string? LogDetail { get; set; }

        [VerifyDateTime(required: true,
            errCode: 6, sErrorDesc: "日志起始时间必须输入", lngcode: "r111")]
        public DateTime LogTimeBegin { get; set; }

        [VerifyDateTime(required: true,
            errCode: 7, sErrorDesc: "日志结束时间必须输入", lngcode: "r112")]
        public DateTime LogTimeEnd { get; set; }
    }
}
