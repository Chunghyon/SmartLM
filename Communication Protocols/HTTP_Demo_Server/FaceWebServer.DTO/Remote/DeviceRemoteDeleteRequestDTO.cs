using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaceWebServer.DTO.Remote
{
    /// <summary>
    /// 删除远程操作命令的请求参数模型
    /// </summary>
    public class DeviceRemoteDeleteRequestDTO
    {
        public List<int> TaskIDs { get; set; }
    }
}
