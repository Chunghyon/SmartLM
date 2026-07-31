using System.Collections.Generic;

namespace FaceWebServer.DTO.HTTPv1_Protocol
{
    /// <summary>
    /// HTTPv1 协议中删除的人员完毕，反馈删除结果的请求 /devicePass/setDeleteResult
    /// </summary>
    public class HTTPDeletePeopleResultRequest : HTTPDeviceRequestV1
    {
        /// <summary>
        /// 删除数量
        /// </summary>
        public int DeleteCount { get; set; }

        /// <summary>
        /// 1 已删除所有人员；0 仅删除指定人员
        /// </summary>
        public int DeleteAll { get; set; }

        /// <summary>
        /// 删除的人员编号
        /// </summary>
        public List<HTTPDeletePeopleDetail>? DeletList { get; set; }
    }
}
