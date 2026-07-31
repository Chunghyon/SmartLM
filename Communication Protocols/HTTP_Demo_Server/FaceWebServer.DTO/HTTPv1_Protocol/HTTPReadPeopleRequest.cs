using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaceWebServer.DTO.HTTPv1_Protocol
{
    /// <summary>
    /// HTTPv1 设备主动从服务器拉取人员授权信息请求  /devicePass/selectPassInfo
    /// </summary>
    public class HTTPReadPeopleRequest: HTTPDeviceRequestV1
    {
        /// <summary>
        /// 当前导入雇员最末位 id 值（后台按需求使用）
        /// </summary>
        public string? Last_Employee_Number { get; set; }

        /// <summary>
        /// 每次请求返回的人员数量限制(不携带则默认50，最大1000)
        /// </summary>
        public int Limit { get; set; }
    }
}
