using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaceWebServer.DTO.HTTPv1_Protocol
{
    /// <summary>
    /// HTTPv1 协议中的远程控制命令的响应  /device/selectRestart
    /// </summary>
    public class HTTPRemoteCommandResponse: HTTPAPIResultV1
    {
        /// <summary>
        ///重启  0:不重启，1:重启
        /// </summary>
        public int Restart { get; set; }

        /// <summary>
        /// 恢复出厂设置  0:不做操作，1：恢复出厂设置
        /// </summary>
        public int Recover { get; set; }


        /// <summary>
        /// 远程开门 0：不处理，1：打开继电器
        /// </summary>
        public int Opendoor { get; set; }

        /// <summary>
        /// 关闭报警 0:不处理，1:关闭所有正在发生的报警，并记录
        /// </summary>
        public int Closealarm { get; set; }

        /// <summary>
        /// 重新上传记录  0:不处理，1:将所有已上传记录重新标记为未上传并重新传输
        /// </summary>
        public int RepostRecord { get; set; }

    }
}
