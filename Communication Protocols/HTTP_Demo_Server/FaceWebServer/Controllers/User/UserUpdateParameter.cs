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
    public class UserUpdateParameter
    {

        /// <summary>
        /// 手机号
        /// </summary>
        [VerifyText(iMax: 30, iMin: 11, required: true,
            errCode: 1, sErrorDesc: "手机号不能为空，且必须由11-30个字符组成", lngcode: "r114")]
        public string Phone { get; set; }

        /// <summary>
        /// 用户密码
        /// </summary>
        [VerifyText(iMax: 10, iMin: 4, required: false,
            errCode: 2, sErrorDesc: "用户密码不能为空，且必须由4-10个字符组成", lngcode: "r115")]
        public string UserPassword { get; set; }

        /// <summary>
        /// 用户ID
        /// </summary>
        //[VerifyNum(iMax: Int32.MaxValue, iMin: 0, errCode: 5, sErrorDesc: "用户ID不正确")]
        public int UserID { get; set; }


        /// <summary>
        /// 用户身份
        /// </summary>
        [VerifyTextRange(sRange: new[] { "Admin", "User" },
            errCode: 3, sErrorDesc: "操作员身份不正确", lngcode: "r116")]
        public string Role { get; set; }
    }
}
