using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaceWebServer.DTO.AlarmClock
{
    /// <summary>
    /// 删除闹铃请求的参数模型
    /// </summary>
    public class AlarmClockDeleteRequestDTO
    {
        public List<int> Nums { get; set; }
    }
}
