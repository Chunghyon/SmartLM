using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaceWebServer.DTO.TimeGroup
{
    /// <summary>
    /// 开门时段删除请求参数模型
    /// </summary>
    public class TimeGroupDeleteRequestDTO
    {
        public List<int> GroupNums { get; set; }
    }
}
