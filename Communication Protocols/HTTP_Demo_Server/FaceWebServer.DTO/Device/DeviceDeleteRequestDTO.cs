using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaceWebServer.DTO.Device
{
    /// <summary>
    /// 设备设备请求的参数模型
    /// </summary>
    public class DeviceDeleteRequestDTO
    {
        /// <summary>
        /// 待删除的设备ID列表
        /// </summary>
        public List<int> DeviceIDs { get; set; }
    }
}
