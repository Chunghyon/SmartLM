using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaceWebServer.DTO.HTTPv2_Protocol
{
    /// <summary>
    /// HTTPv2 协议中 /Device/DownloadWorkSetting 设备拉取参数时返回的格式
    /// </summary>
    public class HTTPDeviceDownloadWorkSettingResponse : HTTPDeviceParameterCoreV2
    {
        /// <summary>
        /// 指令执行状态
        /// 1--表示成功，非1--表示失败
        /// </summary>
        public int Success { get; set; } = 1;

        /// <summary>
        /// 如果指令执行失败，则需要返回失败的具体原因
        /// </summary>
        public string Message { get; set; }
    }
}
