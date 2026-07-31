using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaceWebServer.DTO.HTTPv1_Protocol
{
    /// <summary>
    /// HTTPv1 设备拉取人员名单处理后，反馈的处理结果  /devicePass/setPassResult
    /// </summary>
    public class HTTPReadPeopleResultRequest: HTTPDeviceRequestV1
    {
        /// <summary>
        /// 导入成功的数量
        /// </summary>
        public int SuccessNumber { get; set; }

        /// <summary>
        /// 导入失败的数量
        /// </summary>
        public int FailNumber { get; set; }

        /// <summary>
        /// 导入失败原因
        /// </summary>
        public List<HTTPReadPeopleResultPeopleDetail> FailEmployeeId { get; set; }
    }


    /// <summary>
    ///  HTTPv1 设备拉取人员，保存失败的人员信息
    /// </summary>
    public class HTTPReadPeopleResultPeopleDetail 
    {
        /// <summary>
        /// 人员编号（数字<20位）
        /// </summary>
        public long EmployeeID { get; set; }

        /// <summary>
        /// 错误代码
        /// </summary>
        public int ErrorCode { get; set; }

        /// <summary>
        /// 重复人员编号
        /// </summary>
        public long RepeatID { get; set; }

        /// <summary>
        /// 人员错误信息描述
        /// </summary>
        public string? ErrMsg { get; set; }
    }
}
