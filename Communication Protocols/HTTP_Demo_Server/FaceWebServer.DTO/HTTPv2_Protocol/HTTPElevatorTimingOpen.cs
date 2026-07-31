using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaceWebServer.DTO.HTTPv2_Protocol
{
    /// <summary>
    /// 电梯定时常开参数
    /// </summary>
    public class HTTPElevatorTimingOpen
    {
        /// <summary>
        /// 功能开关,0--禁止；1--启用 
        /// </summary>
        public int Use { get; set; }

        /// <summary>
        /// 自动开模式
        /// <para>1、合法认证通过后在指定时段内即可常开                   </para>
        /// <para>2、授权中标记为常开特权的在指定时段内认证通过即可常开   </para>
        /// <para>3、自动开关,到时间自动开关门                            </para>
        /// </summary>
        public int Open { get; set; }

        /// <summary>
        /// 常开时段,使用周时段结构
        /// </summary>
        public HTTPDeviceParameterTimegroup Timegroup { get; set; }
    }
}
