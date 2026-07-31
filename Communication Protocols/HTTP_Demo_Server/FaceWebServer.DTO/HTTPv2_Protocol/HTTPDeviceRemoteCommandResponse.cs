using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaceWebServer.DTO.HTTPv2_Protocol
{
    /// <summary>
    /// HTTPv2 协议中的远程控制命令
    /// </summary>
    public class HTTPDeviceRemoteCommandResponse : HTTPAPIResultV2
    {
        /// <summary>
        /// 远程重启  0:不重启，1:重启
        /// </summary>
        public int Restart { get; set; }

        /// <summary>
        /// 恢复出厂  0:正常，1：恢复出厂设置
        /// </summary>
        public int Recover { get; set; }


        /// <summary>
        /// 远程开门命令  
        /// 0：不处理；  1--打开继电器;  2--使门常开；  3--关闭门(解除常开)； 
        /// 4--锁定门;   5--解除门锁定
        /// </summary>
        public int Opendoor { get; set; }

        /// <summary>
        /// 关闭报警命令
        /// 0:不处理；  1:关闭所有正在发生的报警，并记录
        /// </summary>
        public int Closealarm { get; set; }

        /// <summary>
        /// 重新上传记录 
        /// 0:不处理；  1:将所有已上传记录重新标记为未上传并重新传输
        /// </summary>
        public int RepostRecord { get; set; }

        /// <summary>
        /// 要求上传所有已存储的人员名单到服务器
        /// 此时设备调用API [/People/PushPeople] 发送人员名单
        /// 0--不需要处理，1--需要上传所有人员
        /// </summary>
        public int PushAllPeople { get; set; }

        /// <summary>
        /// 要求上传指定用户号的人员到服务器
        /// 此时设备调用API [/People/PushPeople] 发送人员名单
        /// </summary>
        public HashSet<long> QueryPeople { get; set; }


        /// <summary>
        /// 删除所有记录； 
        /// 0-- 不处理  1--删除所有记录
        /// </summary>
        public int ClearRecord { get; set; }

        /// <summary>
        /// 在设备上注册凭证
        /// </summary>
        public RegisterIdentifyTicketDTO? RegisterIdentifyTicket { get; set; }

        /// <summary>
        /// 推送软件固件升级包的下载url地址。
        /// 设备收到地址后，开始下载升级包，校验成功后就会自动更新固件。
        /// </summary>
        public PushSoftwareDTO? PushSoftware { get; set; }

        /// <summary>
        /// 推送系统文件更新信息
        /// </summary>
        public List<PushSystemFileDTO>? PushSystemFile { get; set; }

        /// <summary>
        /// 获取设备摄像头快照； 0-- 不处理  1--摄像头快照并上传
        /// </summary>
        public int Snapshoot { get; set; }


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

        /// <summary>
        /// 按时间范围上传记录
        /// </summary>
        public RemoteCommand_UploadRecordRangeDTO? UploadRecordRange { get; set; }
    }

    /// <summary>
    /// 系统文件
    /// </summary>
    public class PushSystemFileDTO
    {
        /// <summary>
        /// 文件的URL地址
        /// </summary>
        public string FileURL { get; set; }

        /// <summary>
        /// 文件的MD5 hex编码格式 大写
        /// </summary>
        public string FileMD5 { get; set; }

        /// <summary>
        /// 文件类型
        /// 1--待机图片 可以有8张图  支持索引范围 1-8
        /// 2--开机图片 只有一张图
        /// </summary>
        public int FileType { get; set; }

        /// <summary>
        /// 文件索引
        /// </summary>
        public int FileIndex { get; set; }


        /// <summary>
        /// 是否表示需要删除文件
        /// </summary>
        public int IsDelete { get; set; }

        public PushSystemFileDTO(){}

        /// <summary>
        /// 创建一个系统文件推送对象
        /// </summary>
        /// <param name="sURL"></param>
        /// <param name="sMD5"></param>
        /// <param name="iType"></param>
        /// <param name="iIndex"></param>
        public PushSystemFileDTO(string sURL,string sMD5,int iType,int iIndex)
        {
            FileURL = sURL;
            FileMD5 = sMD5;
            FileType = iType;
            FileIndex = iIndex;
            IsDelete = 0;
        }

        /// <summary>
        /// 创建一个待删除的系统文件推送对象
        /// </summary>
        /// <param name="iType"></param>
        /// <param name="iIndex"></param>
        public PushSystemFileDTO(int iType, int iIndex,bool delete)
        {
            FileURL = string.Empty;
            FileMD5 = string.Empty;
            FileType = iType;
            FileIndex = iIndex;
            IsDelete = delete ? 1 : 0;
        }
    }

    /// <summary>
    /// 推送软件固件升级包。
    /// </summary>
    public class PushSoftwareDTO
    {
        /// <summary>
        /// 文件的URL地址
        /// </summary>
        public string SoftwareURL { get; set; }

        /// <summary>
        /// 文件的MD5 hex编码格式 大写
        /// </summary>
        public string SoftwareMD5 { get; set; }

        /// <summary>
        /// 文件的固件版本号
        /// </summary>
        public string SoftwareVer { get; set; }
    }

    /// <summary>
    /// 在设备上注册凭证
    /// </summary>
    public class RegisterIdentifyTicketDTO
    {
        /// <summary>
        /// 注册的凭证类型
        /// 1--注册卡; 2--注册密码；3--注册指纹; 4--注册人脸; 5--注册掌静脉
        /// </summary>
        public long RegisterType { get; set; }

        /// <summary>
        /// 在设备上注册凭证的用户ID
        /// </summary>
        public long UserID { get; set; }

        /// <summary>
        /// 注册的凭证索引号
        /// 对于指纹的索引号是 0-9;对于掌静脉的索引号是 1-2
        /// </summary>
        public int RegisterIndex { get; set; }
    }


    public class RemoteCommand_UploadRecordRangeDTO
    {
        /// <summary>
        /// 起始时间的时间戳
        /// </summary>
        public long BeginTime { get; set; }
        /// <summary>
        /// 结束时间的时间戳
        /// </summary>
        public long EndTime { get; set; }
    }
}
