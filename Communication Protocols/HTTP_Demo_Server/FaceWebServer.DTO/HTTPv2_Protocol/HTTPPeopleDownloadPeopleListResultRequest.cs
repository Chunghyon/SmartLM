using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaceWebServer.DTO.HTTPv2_Protocol
{
    /// <summary>
    /// HTTPv2 协议中设备拉取人员并存储完毕后反馈存储结果的请求 /People/DownloadPeopleListResult
    /// </summary>
    public class HTTPPeopleDownloadPeopleListResultRequest: HTTPDeviceRequestV2
    {
        /// <summary>
        /// 导入成功的数量
        /// </summary>
        public int SuccessCount { get; set; }

        /// <summary>
        /// 导入失败的数量
        /// </summary>
        public int FailCount { get; set; }

        /// <summary>
        /// 导入失败的数量
        /// </summary>
        public List<HTTPPeopleDownloadPeopleListResultDetail>? FailList { get; set; }
    }

    /// <summary>
    /// HTTPv2 协议中设备人员存储失败结果详情
    /// </summary>
    public class HTTPPeopleDownloadPeopleListResultDetail
    {
        /// <summary>
        /// 用户号 （数字 最大值 4294967295 类型 UINT32）
        /// </summary>
        public long UserID { get; set; }

        /// <summary>
        /// 错误代码
        /// </summary>
        public int ErrorCode { get; set; }

        /// <summary>
        /// 重复的用户号，此字段只是出这个人员跟谁重复
        /// </summary>
        public long RepeatID { get; set; }

        /// <summary>
        /// 错误描述
        /// </summary>
        public string? ErrMsg { get; set; }
    }
}
