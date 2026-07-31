using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaceWebServer.DTO.Holiday
{
    /// <summary>
    /// 删除节假日请求的参数模型
    /// </summary>
    public class HolidayDeleteRequestDTO
    {
        public List<int> Nums { get; set; }
    }
}
