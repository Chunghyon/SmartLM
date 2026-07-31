using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaceWebServer.DTO.MQTT_Protocol
{
    /// <summary>
    /// 定义MQTT命令名称
    /// </summary>
    public static class MQTT_Command_Define
    {
        /// <summary>
        /// MQTT 设备心跳保活包  由设备发送
        /// </summary>
        public const string KeepAlive = "KeepAlive";

        /// <summary>
        /// MQTT 设备离线的遗嘱  由设备发送
        /// </summary>
        public const string Offline = "Will_DeviceOffline";

        /// <summary>
        /// MQTT 设备主动上传工作参数 由设备发送
        /// </summary>
        public const string UploadWorkSetting = "UploadWorkSetting";

        /// <summary>
        /// MQTT 服务器推送工作参数 由服务器发送
        /// </summary>
        public const string PushWorkSetting = "PushWorkSetting";


        /// <summary>
        ///  MQTT 服务器要求设备上传工作参数  由服务器发送
        /// </summary>
        public const string ReadWorkSetting = "ReadWorkSetting";



        /// <summary>
        /// MQTT 设备确认收到工作参数  由设备发送
        /// </summary>
        public const string PushWorkSettingACK = "PushWorkSettingACK";


        /// <summary>
        /// MQTT 服务器发送远程操作指令  由服务器发送
        /// </summary>
        public const string RemoteCommand = "RemoteCommand";


        /// <summary>
        /// MQTT 设备确认服务器发送的远程操作指令  由设备发送
        /// </summary>
        public const string RemoteCommandACK = "RemoteCommandACK";


        /// <summary>
        /// MQTT 服务器推送人员  由服务器发送
        /// </summary>
        public const string PushPeople = "PushPeople";


        /// <summary>
        /// MQTT 设备反馈人员存储结果  由设备发送
        /// </summary>
        public const string PushPeopleACK = "PushPeopleACK";

        /// <summary>
        /// MQTT 服务器发送删除人员消息  由服务器发送
        /// </summary>
        public const string PushDeletePeople = "PushDeletePeople";


        /// <summary>
        /// MQTT 设备反馈删除人员结果  由设备发送
        /// </summary>
        public const string PushDeletePeopleACK = "PushDeletePeopleACK";


        /// <summary>
        /// MQTT 设备推送人员信息  由设备发送
        /// </summary>
        public const string UploadPeople = "UploadPeople";


        /// <summary>
        /// MQTT 服务器反馈接收到设备推送的人员  由服务器发送
        /// </summary>
        public const string UploadPeopleACK = "UploadPeopleACK";


        /// <summary>
        /// MQTT 设备上传打卡记录  由设备发送
        /// </summary>
        public const string UploadIdentifyRecord = "UploadIdentifyRecord";


        /// <summary>
        /// MQTT 服务器反馈接收到设备推送的打卡记录  由服务器发送
        /// </summary>
        public const string UploadIdentifyRecordACK = "UploadIdentifyRecordACK";


        /// <summary>
        /// MQTT 设备上传系统记录  由设备发送
        /// </summary>
        public const string UploadSystemRecord = "UploadSystemRecord";


        /// <summary>
        /// MQTT 服务器反馈接收到设备推送的系统记录  由服务器发送
        /// </summary>
        public const string UploadSystemRecordACK = "UploadSystemRecordACK";


        /// <summary>
        /// MQTT  服务器发送固件升级通知  由服务器发送
        /// </summary>
        public const string PushSoftware = "PushSoftware";


        /// <summary>
        /// MQTT 设备反馈已接收固件升级通知  由设备发送
        /// </summary>
        public const string PushSoftwareACK = "PushSoftwareACK";


        /// <summary>
        /// MQTT  服务器发送系统文件更新通知  由服务器发送
        /// </summary>
        public const string PushSystemFile = "PushSystemFile";


        /// <summary>
        /// MQTT 设备反馈已收到系统文件更新通知  由设备发送
        /// </summary>
        public const string PushSystemFileACK = "PushSystemFileACK";


        /// <summary>
        /// MQTT  服务器发送通知设备注册用户凭证  由服务器发送
        /// </summary>
        public const string RegisterIdentifyTicket = "RegisterIdentifyTicket";

        /// <summary>
        /// MQTT 设备反馈注册用户凭证结果  由设备发送
        /// </summary>
        public const string RegisterIdentifyTicketACK = "RegisterIdentifyTicketACK";

        /// <summary>
        /// MQTT  设备发送请求服务器鉴权  由设备发送
        /// </summary>
        public const string RequestAuthorization = "RequestAuthorization";

        /// <summary>
        /// MQTT  服务器反馈设备鉴权结果  由服务器发送
        /// </summary>
        public const string RequestAuthorizationACK = "RequestAuthorizationACK";


        /// <summary>
        /// MQTT  服务器发送获取设备摄像头快照请求  由服务器发送
        /// </summary>
        public const string RequestSnapshoot = "RequestSnapshoot";

        /// <summary>
        /// MQTT 设备摄像头快照  由设备发送
        /// </summary>
        public const string RequestSnapshootACK = "RequestSnapshootACK";

        /// <summary>
        /// 当设备权限发生变化时，由服务器发送到设备
        /// </summary>
        public const string DeviceAuthentication = "DeviceAuthentication";


    }
}
