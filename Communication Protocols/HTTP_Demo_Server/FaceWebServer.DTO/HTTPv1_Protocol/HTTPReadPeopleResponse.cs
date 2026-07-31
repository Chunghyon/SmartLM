using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaceWebServer.DTO.HTTPv1_Protocol
{
    /// <summary>
    /// HTTPv1 设备主动从服务器拉取人员授权信息响应  /devicePass/selectPassInfo
    /// </summary>
    public class HTTPReadPeopleResponse: HTTPAPIResultV1
    {
        public int Count { get; set; }
        /// <summary>
        /// 包含人员权限信息结构的数组
        /// </summary>
        public List<HTTPPeopleV1> PassInfo { get; set; }
    }


}
