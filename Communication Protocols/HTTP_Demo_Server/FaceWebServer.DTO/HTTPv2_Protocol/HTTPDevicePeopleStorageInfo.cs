using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaceWebServer.DTO.HTTPv2_Protocol
{
    /// <summary>
    /// 人员存储详情
    /// </summary>
    public class HTTPDevicePeopleStorageInfo
    {
        /// <summary>
        /// 人员存储容量
        /// </summary>
        public HTTPDeviceStorageInfo Person { get; set; }

        /// <summary>
        /// 人脸存储容量
        /// </summary>
        public HTTPDeviceStorageInfo Face { get; set; }

        /// <summary>
        /// 卡片存储容量
        /// </summary>
        public HTTPDeviceStorageInfo Card { get; set; }

        /// <summary>
        /// 指纹存储容量
        /// </summary>
        public HTTPDeviceStorageInfo Fingerprint { get; set; }

        /// <summary>
        /// 掌纹存储容量
        /// </summary>
        public HTTPDeviceStorageInfo PalmVein { get; set; }


        /// <summary>
        /// 密码存储容量
        /// </summary>
        public HTTPDeviceStorageInfo Pasword { get; set; }


        /// <summary>
        /// 管理员存储容量
        /// </summary>
        public HTTPDeviceStorageInfo Admin { get; set; }
    }
}
