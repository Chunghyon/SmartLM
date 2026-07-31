using FaceWebServer.DB.Table;
using FaceWebServer.Utility.VerifyAttribute;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FaceWebServer.DTO.Remote
{
    /// <summary>
    /// 新增远程任务接口参数
    /// </summary>
    public class RemoteTaskAddDTO
    {
        public List<int> DeviceIDs { get; set; }

        public RemoteTypeEnum TaskType { get; set; }

        public long UserID { get; set; } //是

        /// <summary>
        /// 任务扩展信息，一般使用Json字符串保存任务参数
        /// </summary>
        public string? TaskExtension { get; set; }
    }
}
