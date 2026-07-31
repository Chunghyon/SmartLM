using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaceWebServer.DTO.HTTPv2_Protocol
{
    /// <summary>
    /// HTTPv2 协议 拉取人员授权响应，/People/DownloadPeopleList
    /// </summary>
    public class HTTPPeopleDownloadPeopleListResponse: HTTPAPIResultV2
    {
        /// <summary>
        /// 本次返回的人员数量
        /// </summary>
        public int PeopleCount { get; set; }

        /// <summary>
        /// 包含人员权限信息结构的数组
        /// </summary>
        public List<HTTPPeopleV2> PeopleList;
    }
}
