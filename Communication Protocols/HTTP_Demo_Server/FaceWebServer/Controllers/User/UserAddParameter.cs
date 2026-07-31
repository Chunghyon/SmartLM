using FaceWebServer.Utility.VerifyAttribute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DeviceProtocolServer.Controllers.User
{
    /// <summary>
    /// 更新用户的参数
    /// </summary>
    public class UserAddParameter
    {
        /// <summary>
        /// 用户名
        /// </summary>
        [VerifyText(iMax: 30, iMin: 3, required: true,
            errCode: 1, sErrorDesc: "用户名不能为空，且必须由3-30个字符组成", lngcode: "r113")]
        public string UserName { get; set; }

        /// <summary>
        /// 手机号
        /// </summary>
        [VerifyText(iMax: 30, iMin: 11, required: true,
            errCode: 2, sErrorDesc: "手机号不能为空，且必须由11-30个字符组成", lngcode: "r114")]
        public string Phone { get; set; }

        /// <summary>
        /// 用户密码
        /// </summary>
        [VerifyText(iMax: 10, iMin: 4, required: true,
            errCode: 3, sErrorDesc: "用户密码不能为空，且必须由4-10个字符组成", lngcode: "r115")]
        public string UserPassword { get; set; }


        /// <summary>
        /// 用户身份
        /// </summary>
        [VerifyTextRange(sRange: new[] { "Admin", "User" },
            errCode: 4, sErrorDesc: "操作员身份不正确", lngcode: "r116")]
        public string Role { get; set; }
    }
}
