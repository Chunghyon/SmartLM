using FaceWebServer.Utility.VerifyAttribute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaceWebServer.DTO.IOLog
{
    public class ConnectIOLogQueryDTO: BasePageParameter
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
        /// 协议类型  HTTPv1   HTTPv2  MQTT  Websocket 
        /// </summary>
        public string? Protocol { get; set; }

        /// <summary>
        /// API名称
        /// </summary>
        public string? APIName { get; set; }

        /// <summary>
        /// 不显示API名称
        /// </summary>
        public string? NotAPIName { get; set; }

        /// <summary>
        /// 请求的设备SN
        /// </summary>
        public string? SN { get; set; }

        /// <summary>
        /// 请求类型 Request or Response
        /// </summary>
        public string? HttpType { get; set; }


    }
}
