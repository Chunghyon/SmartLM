using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaceWebServer.DTO.Device
{
    /// <summary>
    /// 获取设备在线状态请求参数模型
    /// </summary>
    public class GetDeviceOnlineStatusRequestDTO
    {
        public List<string> SNList { get; set;}
    }
}
