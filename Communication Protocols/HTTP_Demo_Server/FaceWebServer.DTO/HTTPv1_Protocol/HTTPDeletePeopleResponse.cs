using System.Collections.Generic;

namespace FaceWebServer.DTO.HTTPv1_Protocol
{
    /// <summary>
    /// HTTPv1 协议中拉取待删除的人员列表的响应 /devicePass/selectDeleteInfo
    /// </summary>
    public class HTTPDeletePeopleResponse : HTTPAPIResultV1
    {
        /// <summary>
        /// 1：清空所有雇员信息 0：按指定编号删除
        /// </summary>
        public int Empty;


        /// <summary>
        /// 本次需要删除的人员数量
        /// </summary>
        public int DeleteCount { get; set; }
        /// <summary>
        /// empty=0时，此参数包含需要删除的人员编号
        /// </summary>
        public List<HTTPDeletePeopleDetail> DeleteInfo;
    }

    public class HTTPDeletePeopleDetail
    {
        public long EmployeeID { get; set; }
    }
}
