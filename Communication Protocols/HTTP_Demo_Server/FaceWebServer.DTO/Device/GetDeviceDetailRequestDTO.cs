using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaceWebServer.DTO.Device
{
    /// <summary>
    /// 获取设备详情的接口
    /// </summary>
    public class GetDeviceDetailRequestDTO
    {
        /// <summary>
        /// 设备ID
        /// </summary>
        public int DeviceID { get; set; }
    }
}
