using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaceWebServer.DTO.HTTPv2_Protocol
{
    /// <summary>
    /// HTTPv2 协议 拉取人员授权请求，/People/DownloadPeopleList
    /// </summary>
    public class HTTPPeopleDownloadPeopleListRequest : HTTPDeviceRequestV2
    {
        /// <summary>
        /// 每次请求返回的人员数量限制(最大1000)
        /// 设备按自身处理容量来设置此值
        /// </summary>
        public int Limit { get; set; } = 50;
    }
}
