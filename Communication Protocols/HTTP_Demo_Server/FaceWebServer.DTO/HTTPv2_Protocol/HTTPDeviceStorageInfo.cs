using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaceWebServer.DTO.HTTPv2_Protocol
{
    /// <summary>
    /// 存储详情
    /// </summary>
    public class HTTPDeviceStorageInfo
    {
        /// <summary>
        /// 最大容量
        /// </summary>
        public int Max { get; set; }

        /// <summary>
        /// 当前存储数量
        /// </summary>
        public int Current { get; set; }
    }
}
