using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaceWebServer.DTO.HTTPv2_Protocol
{
    /// <summary>
    /// 记录存储详情
    /// </summary>
    public class HTTPDeviceRecordStorageInfo
    {
        /// <summary>
        /// 出入记录存储容量
        /// </summary>
        public HTTPDeviceStorageInfo VerifyRecord { get; set; }

        /// <summary>
        /// 门磁记录存储信息
        /// </summary>
        public HTTPDeviceStorageInfo DoorRecord { get; set; }


        /// <summary>
        /// 系统记录存储容量
        /// </summary>
        public HTTPDeviceStorageInfo SystemRecord { get; set; }


        /// <summary>
        /// 现场照片存储容量
        /// </summary>
        public HTTPDeviceStorageInfo RecordPhoto { get; set; }
    }
}
