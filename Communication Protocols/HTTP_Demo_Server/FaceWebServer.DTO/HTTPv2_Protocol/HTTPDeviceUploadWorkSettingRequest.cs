using FaceWebServer.DTO.HTTPv2_Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaceWebServer.DTO.HTTPv2_Protocol
{
    /// <summary>
    /// HTTPv2 协议中的设备上传工作参数的请求内容 /Device/UploadWorkSetting
    /// </summary>
    public class HTTPDeviceUploadWorkSettingRequest : HTTPDeviceParameterCoreV2
    {
        #region 设备基本信息 SystemInfo

        /// <summary>
        /// 固件版本   "7.81"
        /// </summary>
        public string? FirmwareVerson { get; set; }

        /// <summary>
        /// 指纹算法版本   "7.81"
        /// </summary>
        public string? FingerprintVerson { get; set; }

        /// <summary>
        /// 人脸算法版本   "7.81"
        /// </summary>
        public string? FaceVerson { get; set; }


        /// <summary>
        /// 掌静脉算法
        /// </summary>
        public string? PalmveinVerson { get; set; }
        #endregion

        #region 设备工作状态 Status
        /// <summary>
        /// 系统运行天数
        /// </summary>
        public int RunDays { get; set; }

        /// <summary>
        /// 格式化次数
        /// </summary>
        public int FormatCount { get; set; }

        /// <summary>
        /// 看门狗复位次数
        /// </summary>
        public int WatchDogCount { get; set; }

        /// <summary>
        /// 开机时间 Unix时间戳（秒）
        /// </summary>
        public long BootTime { get; set; }

        /// <summary>
        /// 继电器物状态  0--表示COM和NC常闭  1--表示COM和NO常闭
        /// </summary>
        public int RelayStatus { get; set; }


        /// <summary>
        /// 常开状态  0--表示常闭   1--表示常开
        /// </summary>
        public int KeepOpenStatus { get; set; }

        /// <summary>
        /// 门磁状态  0--表示关  1--表示开
        /// </summary>
        public int DoorSensorStatus { get; set; }

        /// <summary>
        /// 门锁定状态  0--表示未锁定  1--表示已锁定
        /// </summary>
        public int LockDoorStatus { get; set; }

        /// <summary>
        /// 门报警状态  空字符串为无报警，否则会有具体报警名称
        /// <para>fire--消防报警      blacklist--黑名单报警    anti--防拆报警</para>
        /// <para>Illegal--非法验证   password--胁迫报警密码   openTimeout--开门超时报警</para>
        /// <para>doorSensor--门磁报警</para>
        /// <para>有多个报警时，使用逗号分隔 fire,blacklist</para>
        /// </summary>
        public string? AlarmStatus { get; set; }
        #endregion

        #region 数据存储 Storage
        /// <summary>
        /// 人员存储详情
        /// </summary>
        public HTTPDevicePeopleStorageInfo? PeopleStorageInfo { get; set; }

        /// <summary>
        /// 记录存储详情
        /// </summary>
        public HTTPDeviceRecordStorageInfo? RecordStorageInfo { get; set; }
        #endregion


        #region 服务器参数 NetworkServer
        /// <summary>
        /// 一卡通云服务连接状态 <br />
        /// 0--已禁用;1 -- UDP 无连接；2--TCP 未连接；3--TCP 已连接；
        /// </summary>
        public int OnecardCloudStatus { get; set; }


        /// <summary>
        /// 一卡通云服务连接状态 <br />
        ///HTTPClient连接状态 <br />
        ///0--已禁用;1--已连接；2--连接失败； 3--服务器响应错误; 4--服务器拒绝连接
        /// </summary>
        public int HTTPClientStatus { get; set; }

        /// <summary>
        /// 一卡通云服务连接状态 <br />
        ///MQTTClient连接状态，  <br />
        ///0--已禁用;1--已连接；2--连接失败； 3--MQTT CONNECT 握手失败 ; 4 -- Subscribe 订阅主题失败
        /// </summary>
        public int MQTTClientStatus { get; set; }


        /// <summary>
        /// WebsocketClient连接状态 <br />
        /// 0--已禁用;1--TCP 未连接；2--TCP 已连接；3--服务器拒绝连接
        /// </summary>
        public int WebsocketClientStatus { get; set; }


        /// <summary>
        /// 云筑网协议连接状态，  <br /> 
        /// 0--已禁用;1--TCP 已连接； 2--连接失败；3--服务器响应错误；4--SN未登记
        /// </summary>
        public int YZWClientStatus { get; set; }
        #endregion


        #region 设备功能列表 FunctionList
        /// <summary>
        /// 设备功能列表
        /// </summary>
        public HTTPDeviceFunctionList? FunctionList { get; set; }
        #endregion
    }
}
