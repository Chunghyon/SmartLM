using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaceWebServer.DTO.HTTPv1_Protocol
{
    public class HTTPAPIResultV1
    {
        /// <summary>
        /// 指令执行状态
        /// 非 0:失败 ; 0:成功
        /// </summary>
        public int Success { get; set; } = 0;

        /// <summary>
        /// 如果指令执行失败，则需要返回失败的具体原因
        /// </summary>
        public string Msg { get; set; }
    }
}
