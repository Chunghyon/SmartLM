using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaceWebServer.DTO.Cache
{
    /// <summary>
    /// 设备缓存信息
    /// </summary>
    public class CacheDeviceDTO
    {
        /// <summary>
        /// 主键
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// 设备 SN
        /// </summary>
        public string SN { get; set; }

        /// <summary>
        /// 协议类型  HTTPv1   HTTPv2  MQTT  Websocket
        /// </summary>
        public string Protocol { get; set; }

        /// <summary>
        /// 设备名称
        /// </summary>
        public string DeviceName { get; set; }

        /// <summary>
        /// 同步状态
        /// 0--未同步；1--已同步
        /// </summary>
        public int UploadStatus { get; set; }

        /// <summary>
        /// 最近保活包时间
        /// </summary>
        public DateTime LastKeepaliveTime { get; set; }

        /// <summary>
        /// MQTT在线状态
        /// </summary>
        public bool MQTT_Online { get; set; }

        /// <summary>
        /// 关联的MQTT客户端ID
        /// </summary>
        public string MQTT_ClientID { get; set; }

        /// <summary>
        /// 保活包状态信息 根据不同协议对象值不同
        /// </summary>
        public object KeepaliveStatus { get; set; }

        /// <summary>
        /// 权限统计数
        /// </summary>
        public int AccessTotal { get; set; }
        /// <summary>
        /// 新权限数
        /// </summary>
        public int NewAccessTotal { get; set; }
        /// <summary>
        /// 待删除权限数
        /// </summary>
        public int DeleteAccessTotal { get; set; }

        /// <summary>
        /// 待执行的远程任务数量
        /// </summary>
        public int RemoteTaskTotal { get; set; }

        /// <summary>
        /// 是否需要执行清空人员任务
        /// </summary>
        public int EmptyPeople { get; set; }

        /// <summary>
        /// 是否需要执行通知设备上传参数任务
        /// </summary>
        public int UploadWorkParameterTaskTotal { get; set; }


        /// <summary>
        /// 上传设备固件的URL
        /// </summary>
        public string UpdateSoftURL { get; set; }
        /// <summary>
        /// 固件版本号
        /// </summary>
        public string UpdateSoftVer { get; set; }
        /// <summary>
        /// 固件MD5
        /// </summary>
        public string UpdateSoftMD5 { get; set; }


    }
}
