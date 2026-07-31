using FaceWebServer.Interface;
using FaceWebServer.Utility.VerifyAttribute;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using FaceWebServer.DTO;

namespace DeviceProtocolServer.Controllers.User
{
    /// <summary>
    /// 用户查询参数
    /// </summary>
    public class UserQueryParameter : BasePageParameter
    {
        [VerifyText(iMax: 30, iMin: 0, required: false,
            errCode: 1, sErrorDesc: "用户名由1-30个字符组成", lngcode: "r106")]
        public string? UserName { get; set; }

        [VerifyText(iMax: 30, iMin: 0, required: false,
            errCode: 2, sErrorDesc: "手机号由1-30个字符组成", lngcode: "117")]
        public string? Phone { get; set; }
    }
}
