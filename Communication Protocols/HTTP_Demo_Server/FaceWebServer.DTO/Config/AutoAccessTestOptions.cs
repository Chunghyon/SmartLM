using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaceWebServer.DTO.Config
{
    /// <summary>
    /// 自动权限测试选项
    /// </summary>
    public class AutoAccessTestOptions
    {
        public HashSet<string> SNList { get; set; }
        public List<long> PCodes { get; set; }
    }
}
