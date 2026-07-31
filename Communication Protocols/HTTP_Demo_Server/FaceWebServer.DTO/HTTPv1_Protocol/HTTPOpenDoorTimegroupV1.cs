using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaceWebServer.DTO.HTTPv1_Protocol
{
    /// <summary>
    /// HTTPv1协议中的设备开门周时段
    /// </summary>
    public class HTTPOpenDoorTimegroupV1
    {
        /// <summary>
        /// 周时段序号 1-64
        /// </summary>
        public int num { get; set; }

        /// <summary>
        /// 星期一
        /// </summary>
        public string? week1 { get; set; }

        /// <summary>
        /// 星期二
        /// </summary>
        public string? week2 { get; set; }

        /// <summary>
        /// 星期三
        /// </summary>
        public string? week3 { get; set; }

        /// <summary>
        /// 星期四
        /// </summary>
        public string? week4 { get; set; }

        /// <summary>
        /// 星期五
        /// </summary>
        public string? week5 { get; set; }

        /// <summary>
        /// 星期六
        /// </summary>
        public string? week6 { get; set; }

        /// <summary>
        /// 星期日
        /// </summary>
        public string? week7 { get; set; }
    }
}
