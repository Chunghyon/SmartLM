using FaceWebServer.Utility.VerifyAttribute;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DeviceProtocolServer.Controllers.User
{
    /// <summary>
    /// 更新用户的参数
    /// </summary>
    public class UserDeleteParameter
    {
        /// <summary>
        /// 包含需要删除的用户ID
        /// </summary>
        [Required]
        public List<int> UserIDs { get; set; }
    }
}
