using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaceWebServer.DB.Table
{
    /// <summary>
    /// 远程操作命令枚举
    /// </summary>
    public enum RemoteTypeEnum
    {
        /// <summary>
        /// 远程开门
        /// </summary>
        OpenDoor = 1,

        /// <summary>
        /// 远程关门
        /// </summary>
        CloseDoor = 2,

        /// <summary>
        /// 远程常开
        /// </summary>
        KeepOpen = 3,

        /// <summary>
        /// 锁定
        /// </summary>
        LockDoor = 4,

        /// <summary>
        /// 解除锁定
        /// </summary>
        UnlockDoor = 5,

        /// <summary>
        /// 关闭报警
        /// </summary>
        CloseAlarm = 6,





        /// <summary>
        /// 远程重启
        /// </summary>
        Restart = 10,

        /// <summary>
        /// 恢复出厂设置
        /// </summary>
        Recover = 11,

        /// <summary>
        /// 重新上传所有记录
        /// </summary>
        RepostRecord = 12,
        /// <summary>
        /// 清空所有记录
        /// </summary>
        ClearRecord = 13,
        /// <summary>
        /// 推送软件固件升级包
        /// </summary>
        PushSoftware = 14,
        /// <summary>
        /// 推送系统文件更新
        /// </summary>
        PushSystemFile = 15,
        /// <summary>
        /// 获取设备摄像头快照
        /// </summary>
        Snapshoot = 16,
        /// <summary>
        /// 发送鉴权通知
        /// </summary>
        DeviceAuthorization = 17,


        /// <summary>
        /// 上传所有人员
        /// </summary>
        PushAllPeople = 20,

        /// <summary>
        /// 上传指定用户号的人员
        /// </summary>
        QueryPeople = 21,

        /// <summary>
        /// 在设备上注册凭证类型
        /// </summary>
        RegisterIdentifyTicket = 22,

        /// <summary>
        /// 电梯远程开继电器
        /// </summary>
        Elevator_OpenRelay = 31,

        /// <summary>
        /// 电梯远程关继电器
        /// </summary>
        Elevator_CloseRelay = 32,

        /// <summary>
        /// 电梯远程常开继电器
        /// </summary>
        Elevator_KeepOpenRelay = 33,

        /// <summary>
        /// 电梯继电器锁定
        /// </summary>
        Elevator_LockRelay = 34,

        /// <summary>
        /// 电梯解除继电器锁定
        /// </summary>
        Elevator_UnlockRelay = 35,



        /// <summary>
        /// 清空所有人员
        /// </summary>
        ClearAllPeople = 100,

        /// <summary>
        /// 上传工作参数
        /// </summary>
        UploadWorkSetting = 101,

    }


    /// <summary>
    /// 远程操作命令
    /// </summary>

    [Table("RemoteTask")]
    public class RemoteTaskDetail
    {
        /// <summary>
        /// 远程任务ID
        /// </summary>
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TaskID { get; set; }

        #region 设备信息
        /// <summary>
        /// SN
        /// </summary>
        public string SN { get; set; }
        #endregion


        #region 任务详情
        /// <summary>
        /// 任务类型 ；
        /// 1，远程开门；2、远程关门；3、远程常开；4、锁定；5、解除锁定；6、关闭报警
        /// 10、远程重启；11、恢复出厂设置； 12、重新上传所有记录；13、清空所有记录；
        /// 14、推送软件固件升级包；15、推送系统文件更新；16、获取设备摄像头快照；
        /// 20、上传所有人员；21、上传指定用户号的人员；
        /// 22、在设备上注册凭证类型；
        /// 
        /// 100、清空所有人员；101、上传工作参数;
        /// </summary>
        public RemoteTypeEnum TaskType { get; set; }

        /// <summary>
        /// 需要上传的用户号
        /// </summary>
        public long? UserID { get; set; }

        /// <summary>
        /// 上传状态：0--未执行；1--已执行；
        /// </summary>
        public int TaskStatus { get; set; }

        /// <summary>
        /// 任务扩展信息，一般使用Json字符串保存任务参数
        /// </summary>
        public string? TaskExtension { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 任务执行时间
        /// </summary>
        public DateTime TaskRunTime { get; set; }
        #endregion


        public RemoteTaskDetail()
        {
            TaskStatus = 0;
            CreateTime = DateTime.Now;
            TaskRunTime = DateTime.Now;
            TaskExtension = string.Empty;
        }
    }
}
