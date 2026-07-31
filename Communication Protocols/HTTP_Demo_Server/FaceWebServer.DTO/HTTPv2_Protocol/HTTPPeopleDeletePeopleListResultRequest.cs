using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaceWebServer.DTO.HTTPv2_Protocol
{
    /// <summary>
    /// HTTPv2 协议 反馈删除人员操作结果 /People/DeletePeopleListResult
    /// </summary>
    public class HTTPPeopleDeletePeopleListResultRequest : HTTPPeopleDownloadPeopleListRequest
    {
        /// <summary>
        /// 1 已删除所有人员；0 仅删除指定人员
        /// </summary>
        public int DeleteAll { get; set; }



        /// <summary>
        /// 删除数量
        /// </summary>
        public int DeleteCount { get; set; }

        /// <summary>
        /// 删除的人员编号
        /// </summary>
        public List<long>? DeleteList { get; set; }
    }
}
