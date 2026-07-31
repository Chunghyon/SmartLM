using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaceWebServer.DTO.HTTPv2_Protocol
{
    /// <summary>
    /// 电梯端口对象
    /// </summary>
    public class HTTPElevatorPort
    {
        /// <summary>
        /// 电梯端口号
        /// </summary>
        public int Num { get; set; }

        /// <summary>
        /// 电梯继电器
        /// <para>1、COM和NC常闭（默认值）；2、COM和NO常闭</para>
        /// </summary>
        public int RelayType { get; set; }

        /// <summary>
        /// 开锁时输出时长  最大65535秒。0表示0.5秒
        /// </summary>
        public int ReleaseTime { get; set; }

        /// <summary>
        /// 定时常功能结构
        /// </summary>
        public HTTPElevatorTimingOpen TimingOpen { get; set; }
    }
}
