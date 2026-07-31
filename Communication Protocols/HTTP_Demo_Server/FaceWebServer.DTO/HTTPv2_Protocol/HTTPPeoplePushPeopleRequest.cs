using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaceWebServer.DTO.HTTPv2_Protocol
{
    /// <summary>
    /// HTTPv2 协议 设备推送人员的请求 /People/PushPeople
    /// </summary>
    public class HTTPPeoplePushPeopleRequest : HTTPDeviceRequestV2
    {
        /// <summary>
        /// 人员在设备中的改变类型：  1--新增；2--更新；3--删除；4--查询；
        /// </summary>
        public int PushType { get; set; }
        /// <summary>
        /// 推送的人员用户号
        /// </summary>
        public long UserID { get; set; }

        /// <summary>
        /// 人员详情
        /// </summary>
        public HTTPPeopleV2 Detail { get; set; }

        /// <summary>
        /// 人员照片路径
        /// </summary>
        public string Photo { get; set; }
    }
}
