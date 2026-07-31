using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaceWebServer.DTO.HTTPv1_Protocol
{
    /// <summary>
    /// HTTPv1 协议保活包数据结构  /device/updateStateDevice
    /// </summary>
    public class HTTPv1DeviceKeepaliveResponse: HTTPAPIResultV1
    {
        /// <summary>
        /// 1表示需要添加人员到设备
        /// </summary>
        public int AddPeople { get; set; }

        /// <summary>
        /// 1表示需要从设备删除人员
        /// </summary>
        public int DeletePeople { get; set; }

        /// <summary>
        /// 1表示有参数需要设置到设备
        /// </summary>
        public int SyncParameter { get; set; }

        /// <summary>
        /// 1表示有远程操作需要处理
        /// </summary>
        public int Remote { get; set; }

        /// <summary>
        /// 1表示要求设备上传设备工作参数。 此参数可选。
        /// </summary>
        public int UploadWorkParameter { get; set; }
    }
}
