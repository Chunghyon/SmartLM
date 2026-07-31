using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaceWebServer.DTO.MQTT_Protocol.Command.Server
{
    /// <summary>
    /// MQTT  服务器发送远程操作指令  由服务器发送
    /// </summary>
    public class MQTT_Command_RemoteCommand : MQTTCommandPacket<MQTT_RemoteCommand>
    {

        public MQTT_Command_RemoteCommand(MQTT_RemoteCommand data)
        {
            Cmd = MQTT_Command_Define.RemoteCommand;
            Body = data;
            CreateToken();
        }
    }

    /// <summary>
    /// 远程操作指令
    /// </summary>
    public class MQTT_RemoteCommand
    {
        /// <summary>
        /// 远程重启  0:不重启，1:重启
        /// </summary>
        public int? Restart { get; set; }

        /// <summary>
        /// 恢复出厂  0:正常，1：恢复出厂设置
        /// </summary>
        public int? Recover { get; set; }


        /// <summary>
        /// 远程开门命令  
        /// 0：不处理；  1--打开继电器;  2--使门常开；  3--关闭门(解除常开)； 
        /// 4--锁定门;   5--解除门锁定
        /// </summary>
        public int? Opendoor { get; set; }

        /// <summary>
        /// 关闭报警命令
        /// 0:不处理；  1:关闭所有正在发生的报警，并记录
        /// </summary>
        public int? Closealarm { get; set; }

        /// <summary>
        /// 重新上传记录 
        /// 0:不处理；  1:将所有已上传记录重新标记为未上传并重新传输
        /// </summary>
        public int? RepostRecord { get; set; }

        /// <summary>
        /// 要求上传所有已存储的人员名单到服务器
        /// 此时设备调用API [/People/PushPeople] 发送人员名单
        /// 0--不需要处理，1--需要上传所有人员
        /// </summary>
        public int? PushAllPeople { get; set; }

        /// <summary>
        /// 要求上传指定用户号的人员到服务器
        /// 此时设备调用API [/People/PushPeople] 发送人员名单
        /// </summary>
        public HashSet<long>? QueryPeople { get; set; }


        /// <summary>
        /// 删除所有记录； 
        /// 0-- 不处理  1--删除所有记录
        /// </summary>
        public int? ClearRecord { get; set; }


        /// <summary>
        /// 远程驱动一次电梯端口<br />逗号分隔的字符串：1,2,3,4,5
        /// </summary>
        public string? OpenElevatorPort { get; set; }

        /// <summary>
        /// 使电梯端口进入常开状态<br />逗号分隔的字符串：1,2,3,4,5
        /// </summary>
        public string? KeepOpenElevatorPort { get; set; }

        /// <summary>
        /// 使电梯端口退出常开状态<br />逗号分隔的字符串：1,2,3,4,5
        /// </summary>
        public string? CloseElevatorPort { get; set; }

        /// <summary>
        /// 使电梯端口进入锁定状态，锁定时用户不可驱动<br />逗号分隔的字符串：1,2,3,4,5
        /// </summary>
        public string? LockElevatorPort { get; set; }

        /// <summary>
        /// 使电梯端口退出锁定状态<br />逗号分隔的字符串：1,2,3,4,5
        /// </summary>
        public string? UnlockElevatorPort { get; set; }
    }
}
