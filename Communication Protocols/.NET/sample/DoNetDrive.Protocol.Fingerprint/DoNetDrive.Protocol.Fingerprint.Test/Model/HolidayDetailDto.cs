using System;

namespace DoNetDrive.Protocol.Fingerprint.Test.Model
{
    /// <summary>
    /// 节假日 ViewModel
    /// </summary>
    public class HolidayDetailDto
    {
        /// <summary>
        /// 节假日的索引号
        /// </summary>
        public byte Index { get; set; }

        /// <summary>
        /// 节假日日期
        /// </summary>
        public DateTime Holiday { get; set; }

        /// <summary>
        /// 节假日类型：<br/>
        /// 1、上午 (00:00:00  -  11:59:59)<br/>
        /// 2、下午 (12:00:00  -  23:59:59)<br/>
        /// 3、全天 (00:00:00  -  23:59:59)
        /// </summary>
        public byte HolidayType { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string HolidayTypeRender { get; set; }

        /// <summary>
        /// 年：最大0-99，0表示每年重复
        /// </summary>
        public string RepeatYear { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool Selected { get; set; }
    }
}
