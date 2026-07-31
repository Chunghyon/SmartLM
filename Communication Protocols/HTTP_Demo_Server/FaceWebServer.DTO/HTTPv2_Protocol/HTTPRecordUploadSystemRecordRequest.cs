using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaceWebServer.DTO.HTTPv2_Protocol
{
    /// <summary>
    /// HTTPv2 协议 设备推送设备系统记录的请求 /Record/UploadSystemRecord
    /// </summary>
    public class HTTPRecordUploadSystemRecordRequest : HTTPDeviceRequestV2
    {
        /// <summary>
        /// 记录类型 1-门磁记录;2--系统记录
        /// </summary>
        public int RecordType { get; set; }

        /// <summary>
        /// 记录列表
        /// </summary>
        public List<HTTPRecordUploadSystemRecordDetail> Records { get; set; }

    }

    public class HTTPRecordUploadSystemRecordDetail
    {
        /// <summary>
        /// 记录序号
        /// </summary>
        public int RecordID { get; set; }

        /// <summary>
        /// 事件类型
        /// </summary>
        public int RecordType { get; set; }

        /// <summary>
        /// 打卡时间 unix时间戳
        /// </summary>
        public long RecordDate { get; set; }

    }
}
