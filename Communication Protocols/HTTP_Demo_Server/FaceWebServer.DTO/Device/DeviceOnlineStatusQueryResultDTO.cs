using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaceWebServer.DTO.Device
{
    /// <summary>
    /// 查询设备在线状态返回值
    /// </summary>
    public class DeviceOnlineStatusQueryResultDTO
    {
        /// <summary>
        /// 主键
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// 设备 SN
        /// </summary>
        public string SN { get; set; }

        /// <summary>
        /// 最近保活包时间
        /// </summary>
        public DateTime LastKeepaliveTime { get; set; }

        /// <summary>
        /// 保活包状态信息 根据不同协议对象值不同
        /// </summary>
        public object KeepaliveStatus { get; set; }
    }
}
