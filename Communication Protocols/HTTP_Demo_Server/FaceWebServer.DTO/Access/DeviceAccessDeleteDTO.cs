using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaceWebServer.DTO.Access
{
    public class DeviceAccessDeleteDTO
    {
        /// <summary>
        /// 需要删除授权的设备ID列表
        /// </summary>
        public List<int> DeviceIDs { get; set; }

        /// <summary>
        /// 需要删除授权的人员ID列表
        /// </summary>
        public List<int>? PeopleIDs { get; set; }
    }
}
