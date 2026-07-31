using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaceWebServer.DTO.HTTPv2_Protocol
{
    /// <summary>
    /// 设备功能列表
    /// </summary>
    public class HTTPDeviceFunctionList
    {
        /// <summary>
        /// 体温检测
        /// </summary>
        public bool BodyTemperature { get; set; }


        /// <summary>
        ///  指纹
        /// </summary>
        public bool Fingerprint { get; set; }

        /// <summary>
        ///  掌静脉
        /// </summary>
        public bool Palmvein { get; set; }

        /// <summary>
        ///  人脸识别
        /// </summary>
        public bool Face { get; set; }

        /// <summary>
        /// 二维码
        /// </summary>
        public bool QRCode { get; set; }

        /// <summary>
        /// 门禁功能--定时锁定
        /// </summary>
        public bool TimingLocked { get; set; }

        /// <summary>
        ///  口罩检测
        /// </summary>
        public bool FaceMask { get; set; }

        /// <summary>
        ///  安全帽检测
        /// </summary>
        public bool SafetyHelmet { get; set; }

        /// <summary>
        ///  电梯
        /// </summary>
        public bool Lift { get; set; }

        /// <summary>
        ///  闹铃
        /// </summary>
        public bool AlarmClock { get; set; }

        /// <summary>
        ///  excel导出导入
        /// </summary>
        public bool ExcelFile { get; set; }

        /// <summary>
        ///  zip 导入
        /// </summary>
        public bool ZipFile { get; set; }

        /// <summary>
        /// 时段数量
        /// </summary>
        public int TimeGreoup { get; set; }

        /// <summary>
        /// 无线网络
        /// </summary>
        public bool WIFI { get; set; }

        /// <summary>
        /// HTTPClient v1
        /// </summary>
        public bool HTTPClient_V1 { get; set; }

        /// <summary>
        /// HTTPClient v2
        /// </summary>
        public bool HTTPClient_V2 { get; set; }

        /// <summary>
        /// MQTT
        /// </summary>
        public bool MQTT { get; set; }

        /// <summary>
        /// 云筑网
        /// </summary>
        public bool YZW { get; set; }

        /// <summary>
        /// Websocket V1
        /// </summary>
        public bool Websocket_V1 { get; set; }

        /// <summary>
        /// Websocket V2
        /// </summary>
        public bool Websocket_V2 { get; set; }

        /// <summary>
        /// Websocket 服务器
        /// </summary>
        public bool WebsocketServer { get; set; }

        /// <summary>
        /// SSH 连接调试
        /// </summary>
        public bool SSH { get; set; }
        /// <summary>
        /// Telnet 连接调试
        /// </summary>
        public bool Telnet { get; set; }
        /// <summary>
        /// TCP 服务器
        /// </summary>
        public bool TCPServer { get; set; }
        /// <summary>
        /// RTSP 视频流
        /// </summary>
        public bool RTSP { get; set; }
        /// <summary>
        /// 快速识别，不显示识别人名和照片
        /// </summary>
        public bool FastRecognition { get; set; }
        /// <summary>
        /// 在线鉴权
        /// </summary>
        public bool RequestAuthorization { get; set; }
        /// <summary>
        /// 点击识别
        /// </summary>
        public bool RecognitionButton { get; set; }
        /// <summary>
        /// 每日识别次数限制
        /// </summary>
        public bool DailyLimit { get; set; }

        /// <summary>
        /// 权限到期提示
        /// </summary>
        public bool OverdueRemind { get; set; }

        /// <summary>
        /// 延迟开锁
        /// </summary>
        public bool DelayOpenDoorTime { get; set; }


        /// <summary>
        /// 支持密码
        /// </summary>
        public bool Password { get; set; }

        /// <summary>
        /// 支持刷卡
        /// </summary>
        public bool Card { get; set; }

        /// <summary>
        /// 是否有屏幕
        /// </summary>
        public bool LCD { get; set; }
        /// <summary>
        /// 是否有补光灯
        /// </summary>
        public bool FillLight { get; set; }
        /// <summary>
        /// 是否有语音合成
        /// </summary>
        public bool TTS { get; set; }
        /// <summary>
        /// 是否有语音播报
        /// </summary>
        public bool Voice { get; set; }

        /// <summary>
        /// 多人组合
        /// </summary>
        public bool MultiPerson { get; set; }
        /// <summary>
        /// 组合验证
        /// </summary>
        public bool VerificationType { get; set; }
        /// <summary>
        /// WG输出格式
        /// </summary>
        public bool WGOutput { get; set; }

        /// <summary>
        /// 伦邦二维码一体机平台
        /// </summary>
        public bool LUNBANG_QRCode { get; set; }

        /// <summary>
        /// 客户端证书
        /// </summary>
        public bool ClientCert { get; set; }


        /// <summary>
        /// 视频对讲功能
        /// </summary>
        public bool VideoCall { get; set; }
        /// <summary>
        /// SIP 通话功能
        /// </summary>
        public bool SIP { get; set; }

        /// <summary>
        /// STUN 协议
        /// </summary>
        public bool STUN { get; set; }

        /// <summary>
        /// TURN 协议
        /// </summary>
        public bool TURN { get; set; }

        /// <summary>
        /// 是否支持人员编号
        /// </summary>
        public bool UserCode { get; set; }
        /// <summary>
        /// 是否支持在设备输入复杂编号
        /// </summary>
        public bool ComplexUserID { get; set; }

        /// <summary>
        /// 支持自定义欢迎用语
        /// </summary>
        public bool SuccessRecognitionWelcomeSpeech { get; set; }

        /// <summary>
        /// 支持菜单密码使用增强密码
        /// </summary>
        public bool StrongCipher { get; set; }

        /// <summary>
        /// 是否支持IPv6
        /// </summary>
        public bool IPV6 { get; set; }
    }
}
