using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaceWebServer.DTO.MQTT
{
    public class MQTTOptions
    {
        /// <summary>
        /// 启用TCP
        /// </summary>
        public bool UseTCP { get; set; }

        /// <summary>
        /// TCP 端口号
        /// </summary>
        public int TCPPort { get; set; }

        /// <summary>
        /// 启用TLS
        /// </summary>
        public bool UseTLS { get; set; }

        /// <summary>
        /// TLS 端口号
        /// </summary>
        public int TLSPort { get; set; }

        /// <summary>
        /// TLS 证书文件 PFX格式
        /// </summary>
        public string PfxCerfFile { get; set; }


        /// <summary>
        /// TLS PFX证书密码
        /// </summary>
        public string PfxCerfPassword { get; set; }

        /// <summary>
        /// CA证书
        /// </summary>
        public string CaPfxCerf { get; set; }

        /// <summary>
        /// 客户端请求必须携带证书
        /// </summary>
        public bool UseClientCert { get; set; }


    }
}
