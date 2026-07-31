using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaceWebServer.DTO.HTTPv2_Protocol
{
    /// <summary>
    /// HTTPv2协议中的保活包响应包结构
    /// </summary>
    public class HTTPKeepaliveResponseV2 : HTTPAPIResultV2
    {
        /// <summary>
        /// 表示是否有需要设备拉取的人员
        /// >0--表示有需要添加人员到设备<br>设备收到后需要发起 devicePass/selectPassInfo请求 <br>=0或无此字段时表示不需要处理
        /// </summary>
        public int? AddPeople { get; set; }

        /// <summary>
        /// 表示是否有需要设备拉取的待删除人员
        /// </summary>
        public int? DeletePeople { get; set; }

        /// <summary>
        /// 表示是否需要设备拉取工作参数
        /// </summary>

        public int? SyncParameter { get; set; }


        /// <summary>
        /// 表示是否需要设备拉取待执行的远程操控命令
        /// </summary>
        public int? Remote { get; set; }


        /// <summary>
        /// 表示是否需要设备上传参数
        /// </summary>
        public int? UploadWorkParameter { get; set; }
    }
}
