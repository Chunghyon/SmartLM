using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaceWebServer.DTO.HTTPv2_Protocol
{
    /// <summary>
    /// HTTPv1协议中的设备开门周时段
    /// </summary>
    public class HTTPOpenDoorTimegroupV2
    {
        /// <summary>
        /// 周时段序号 1-64
        /// </summary>
        public int Num { get; set; }

        /// <summary>
        /// 星期一
        /// </summary>
        public string? Week1 { get; set; }

        /// <summary>
        /// 星期二
        /// </summary>
        public string? Week2 { get; set; }

        /// <summary>
        /// 星期三
        /// </summary>
        public string? Week3 { get; set; }

        /// <summary>
        /// 星期四
        /// </summary>
        public string? Week4 { get; set; }

        /// <summary>
        /// 星期五
        /// </summary>
        public string? Week5 { get; set; }

        /// <summary>
        /// 星期六
        /// </summary>
        public string? Week6 { get; set; }

        /// <summary>
        /// 星期日
        /// </summary>
        public string? Week7 { get; set; }
    }
}
