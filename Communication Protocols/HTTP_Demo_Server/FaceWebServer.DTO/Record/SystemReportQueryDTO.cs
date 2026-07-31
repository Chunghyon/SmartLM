using FaceWebServer.Utility.VerifyAttribute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaceWebServer.DTO.Record
{
    public class SystemReportQueryDTO : BasePageParameter
    {
        /// <summary>
        /// 查询时间范围起始时间
        /// </summary>
        public DateTime QueryBeginTime { get; set; }

        /// <summary>
        /// 查询时间范围结束时间
        /// </summary>
        public DateTime QueryEndTime { get; set; }

        /// <summary>
        /// 设备SN
        /// </summary>
        public string? SN { get; set; }

        /// <summary>
        /// 事件类型
        /// </summary>
        public int? RecordType { get; set; }

        /// <summary>
        /// 事件类型
        /// </summary>
        public List<int>? RecordTypeList { get; set; }
    }
}
