using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaceWebServer.DTO.Device
{
    /// <summary>
    /// 更新设备固件的请求模型
    /// </summary>
    public class UpdateDeviceSoftRequestDTO
    {
        public string SoftName { get; set; }
        public int DeviceID { get; set; }
    }
}
