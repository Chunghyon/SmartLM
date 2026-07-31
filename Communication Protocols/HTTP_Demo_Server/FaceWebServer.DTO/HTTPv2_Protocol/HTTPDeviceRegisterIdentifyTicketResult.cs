using FaceWebServer.DTO.People;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaceWebServer.DTO.HTTPv2_Protocol
{
    /// <summary>
    /// 注册人员识别凭证结果
    /// </summary>
    public class HTTPDeviceRegisterIdentifyTicketResult
    {
        public string SN { get; set; }
        public long UserID { get; set; }
        /// <summary>
        /// 注册凭证的结果
        /// 1、注册成功；
        /// 2、用户取消操作；
        /// 3--注册信息重复
        /// </summary>
        public int Result { get; set; }

        /// <summary>
        /// 当注册的凭证重复时，此处显示重复的用户号
        /// </summary>
        public long? RepetitionUserID { get; set; }

        /// <summary>
        /// 当注册成功后，返回人员的最新详情，如果有人员照片
        /// </summary>
        public HTTPPeopleV2? UserDetail { get; set; }

    }
}
