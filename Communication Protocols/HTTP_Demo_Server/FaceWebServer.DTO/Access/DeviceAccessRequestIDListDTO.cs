using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaceWebServer.DTO.Access
{
    /// <summary>
    /// 根据权限ID的执行操作的请求参数模型
    /// </summary>
    public class DeviceAccessRequestIDListDTO
    {
        public List<int> AccessIDs { get; set; }
    }
}
