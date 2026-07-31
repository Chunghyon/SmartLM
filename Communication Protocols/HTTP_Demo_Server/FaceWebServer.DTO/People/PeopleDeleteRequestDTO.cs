using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaceWebServer.DTO.People
{
    /// <summary>
    /// 删除人员请求的参数模型
    /// </summary>
    public class PeopleDeleteRequestDTO
    {
        /// <summary>
        /// 待删除人员ID列表
        /// </summary>
        public List<int> PeopleIDs { get; set; }
    }
}
