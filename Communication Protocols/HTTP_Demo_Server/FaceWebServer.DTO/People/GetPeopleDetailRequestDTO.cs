using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaceWebServer.DTO.People
{
    /// <summary>
    /// 获取人员详情的请求参数模型
    /// </summary>
    public class GetPeopleDetailRequestDTO
    {
        public long UserID { get; set; }
    }
}
