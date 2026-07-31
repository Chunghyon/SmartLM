using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaceWebServer.DTO.HTTPv2_Protocol
{
    /// <summary>
    /// HTTPv2 协议 设备拉取待删除人员名单的返回值  /People/DeletePeopleList
    /// </summary>
    public class HTTPPeopleDeletePeopleListResponse: HTTPAPIResultV2
    {
        /// <summary>
        /// 1：清空所有人员信息  0：按指定用户号删除
        /// </summary>
        public int DeleteAll { get; set; }



        /// <summary>
        /// 待删除人员数量
        /// </summary>
        public int DeleteCount { get; set; }

        /// <summary>
        /// 需要删除的用户号列表
        /// </summary>
        public List<long>? DeleteList { get; set; }
    }
}
