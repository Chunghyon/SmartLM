using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaceWebServer.DTO.HTTPv2_Protocol
{
    /// <summary>
    /// 设备闹铃
    /// </summary>
    public class HTTPAlarmClockTime
    {
        /// <summary>
        /// 闹铃序号
        /// </summary>
        public int Num { get; set; }

        /// <summary>
        /// 闹铃时间点 HHmm格式 示例：1230 表示 12点30分
        /// </summary>
        public string Clock { get; set; }

        /// <summary>
        /// 闹铃响铃时长,单位分秒
        /// </summary>
        public int Times { get; set; }
    }
}
