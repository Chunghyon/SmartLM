using FaceWebServer.DB.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaceWebServer.DTO.Remote
{
    /// <summary>
    /// 远程任务查询模型
    /// </summary>
    public class RemoteTaskQueryDTO: BasePageParameter 
    {
        public int? TaskID { get; set; }

        /// <summary>
        /// SN
        /// </summary>
        public string? SN { get; set; }

        /// <summary>
        /// 任务类型 ；
        /// 1，远程开门；2、远程关门；3、远程常开；4、锁定；5、解除锁定；6、关闭报警
        /// 10、远程重启；11、恢复出厂设置； 12、重新上传所有记录；13、清空所有记录；
        /// 20、上传所有人员；21、上传指定用户号的人员；
        /// 
        /// 100、清空所有人员；101、上传工作参数;
        /// </summary>
        public RemoteTypeEnum? TaskType { get; set; }


        /// <summary>
        /// 上传状态：0--未执行；1--已执行；
        /// </summary>
        public int? TaskStatus { get; set; }
    }
}
