using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaceWebServer.DTO.Access
{
    /// <summary>
    /// 设备权限上传状态更新模型
    /// </summary>
    public class DeviceAccessUploadStatusUpdateDTO
    {
        public int? AccessID { get; set; }


        /// <summary>
        /// 上传结果： 0 --无操作；1--正常；
        /// HTTPv1 返回值为 100-999  HTTPv2 返回值为 1000-1999
        /// </summary>
        public int UploadResult { get; set; }

        /// <summary>
        /// 重复人员编号：如果是上传后，照片重复时，此ID记录跟谁重复
        /// </summary>
        public long RepeatID { get; set; }

        /// <summary>
        /// 上传发生异常时，设备返回的异常描述
        /// </summary>
        public string? UploadResultMsg { get; set; }
    }
}
