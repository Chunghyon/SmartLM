using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaceWebServer.DTO.HTTPv2_Protocol
{
    /// <summary>
    /// HTTPv2 协议中的设备核心参数
    /// </summary>
    public class HTTPDeviceParameterCoreV2
    {
        #region 设备基本信息 SystemInfo
        /// <summary>
        /// 设备SN 
        /// </summary>
        public string? DeviceSN { get; set; }

        /// <summary>
        /// 设备名称
        /// </summary>
        public string? DeviceName { get; set; }


        /// <summary>
        /// 制造商
        /// </summary>
        public string? Manufacturer { get; set; }

        /// <summary>
        /// 厂家电话
        /// </summary>
        public string? ManufacturerPhone { get; set; }

        /// <summary>
        /// 网址
        /// </summary>
        public string? Website { get; set; }

        /// <summary>
        /// 生产日期
        /// </summary>
        public string? ProductionDate { get; set; }

        /// <summary>
        /// 自定义数据
        /// </summary>
        public string? OEMText { get; set; }



        /// <summary>
        /// 每天自动重启功能开关
        /// </summary>
        public int AutoRestart { get; set; }

        /// <summary>
        /// 每天自动重启的时间，格式  HH:mm
        /// </summary>
        public string? AutoRestartTime { get; set; }
        #endregion


        #region 区域与语言  Language
        /// <summary>
        /// 语言 1 - 中文；
        /// <para> 2 - 英文；3 - 繁体;4 - 法语;5 - 俄语</para>
        /// <para> 6 - 葡萄牙语;7 - 西班牙语;8 - 意大利语;9 - 日语</para>
        /// <para> 10 - 韩语;11 - 泰语;12 - 阿拉伯语;13 - 葡萄牙</para>
        /// <para> 14 - 土耳其,15 - 印尼,16 - 乌克兰,17-越南</para>
        /// </summary>
        public int Language { get; set; }

        /// <summary>
        /// 设备时间Unix时间戳（秒）
        /// </summary>
        public long SystemTime { get; set; }

        /// <summary>
        /// 启用NTP自动校对时间 1--启用；0--禁用
        /// </summary>
        public int UseNTP { get; set; }

        /// <summary>
        /// 设备时区 , 格式为 UTC-11:00
        /// </summary>
        public string? UTCTimeZone { get; set; }

        /// <summary>
        /// 音量大小(范围 0-10)
        /// </summary>
        public int Volume { get; set; }

        /// <summary>
        /// 语音播放开关 0,不播报；1,播报
        /// </summary>
        public int Voice { get; set; }
        #endregion

        #region 人机交互 UI
        /// <summary>
        /// 屏幕亮度设置 1-10
        /// </summary>
        public int DisplayBrightness { get; set; }

        /// <summary>
        /// 菜单密码
        /// </summary>
        public string? MenuPassword { get; set; }

        /// <summary>
        /// 强密码开关，<br />
        /// 是指菜单密码必须使用 字符大小写+数字+特殊符号组合，<br />
        /// 且不低于6个字符的密码。
        /// </summary>
        public int StrongCipher { get; set; }

        /// <summary>
        /// 在设备上显示红外图像 1--启用；0--禁用
        /// </summary>
        public int ShowIR { get; set; }

        /// <summary>
        /// 识别后显示人员头像 1--启用；0--禁用
        /// </summary>
        public int ShowPersonPhoto { get; set; }

        /// <summary>
        /// 识别后播报人员姓名 1--启用；0--禁用
        /// </summary>
        public int PlayPersonName { get; set; }

        /// <summary>
        /// 识别前需要点击识别按钮 1--启用；0--禁用
        /// </summary>
        public int RecognitionButton { get; set; }

        /// <summary>
        /// 未注册人员提醒 1--启用；0--禁用
        /// </summary>
        public int UnregisteredWarn { get; set; }

        /// <summary>
        /// 识别后是否显示人员姓名  1--启用；0--禁用
        /// </summary>
        public int ShowPersonName { get; set; }

        /// <summary>
        /// 补光灯模式：0：常闭；  1：常开； 2：自动；
        /// </summary>
        public int FillLight { get; set; }

        /// <summary>
        /// 二维码识别开关   1--启用；0--禁用
        /// </summary>
        public int UseQRCode { get; set; }

        /// <summary>
        /// 快速识别，不显示识别人名和照片  <br /> 
        /// 1--启用；0--禁用
        /// </summary>
        public int UseFastRecognition { get; set; }

        /// <summary>
        /// 在线鉴权<br /> 
        /// 1--启用；0--禁用<br />
        /// 启用后，识别验证后不会开门<br />
        /// 设备需要请求服务器再次验证
        /// </summary>
        public int UseRequestAuthorization { get; set; }


        /// <summary>
        /// 使用复杂编号<br /> 
        /// 1--启用；0--禁用<br />
        /// 启用后，将使用在设备界面上使用人员编号替代用户号
        /// </summary>
        public int UseComplexUserID { get; set; }

        /// <summary>
        /// 识别成功后播报的欢迎语音
        /// </summary>
        public string? SuccessRecognitionWelcomeSpeech { get; set; }
        #endregion

        #region 数据存储 Storage
        /// <summary>
        /// 记录存满循环  1--记录满循环，0--记录满不循环，等待清理
        /// </summary>
        public int RecordAutoCycle { get; set; }

        /// <summary>
        /// 保存未注册人员，0,不保存；1,保存   
        /// 未注册人员是指在人脸识别时，没有在系统中注册的人员，或者刷卡的时候没有在系统中注册的卡号
        /// </summary>
        public int SaveUnregistered { get; set; }

        /// <summary>
        /// 保存现场图片 0,不保存；1,保存
        /// </summary>
        public int SaveRecordPicture { get; set; }

        #endregion

        #region 人脸识别 Face
        /// <summary>
        /// 活体检测,1 打开,0 关闭
        /// </summary>
        public int FaceIR { get; set; }

        /// <summary>
        /// 活体检测阈值 1-99
        /// </summary>
        public int FaceIRThreshold { get; set; }

        /// <summary>
        /// 识别距离 1--近距离（0.2-0.5米）；2--中距离（0.2-1.5米）；3--远距离（0.2-1.5米以上）
        /// </summary>
        public int FaceDistance { get; set; }

        /// <summary>
        /// 人脸识别阈值1-99 人脸识别阈值 是越大精度越高
        /// </summary>
        public int FaceThreshold { get; set; }

        /// <summary>
        /// 指纹比对阈值  取值范围：1-100
        /// </summary>
        public int FPComparison { get; set; }

        /// <summary>
        /// 人脸口罩检测,1 打开,0 关闭
        /// </summary>
        public int FaceMask { get; set; }

        /// <summary>
        /// 口罩检测阈值 1-100
        /// </summary>
        public int FaceMaskThreshold { get; set; }
        #endregion

        #region 体温检测 BodyTemperature 
        /// <summary>
        /// 测温模式开关。0：非测温模式 1：测温模式
        /// </summary>
        public int UseBodyTemperature { get; set; }

        /// <summary>
        /// 开启华氏温度显示,1:开,0:关
        /// </summary>
        public int UseFahrenheitDisplay { get; set; }

        /// <summary>
        /// 温度补偿值 
        /// </summary>
        public double TemperatureCompensate { get; set; }

        /// <summary>
        /// 温度报警阈值
        /// </summary>
        public double TemperatureAlarmThreshold { get; set; }

        /// <summary>
        /// 是否显示体温 0:不显示 1:显示
        /// </summary>
        public int TemperatureDisplay { get; set; }
        #endregion

        #region 服务器参数 NetworkServer
        /// <summary>
        /// 一卡通云协议类型 <br/> 0--禁用；1--UDP；2--TCP；3--SSL
        /// </summary>
        public int OnecardCloudServerProtocol { get; set; }

        /// <summary>
        /// 一卡通协议服务器地址  tcp 或 udp 协议服务器地址
        /// </summary>
        public string? ServerAddress { get; set; }

        /// <summary>
        /// 一卡通协议服务器IP
        /// </summary>
        public string? ServerIP { get; set; }
        /// <summary>
        /// 一卡通协议服务器端口号
        /// </summary>
        public int ServerPort { get; set; }

        /// <summary>
        /// 一卡通协议 保活包间隔时间 1-65535 秒
        /// </summary>
        public int KeepaliveTime { get; set; }

        /// <summary>
        /// 一卡通协议离线消息推送  1--启动；0--禁用；
        /// </summary>
        public int PushOfflineMessage { get; set; }

        






        /// <summary>
        /// 是否启用 HTTPClient 协议   1--启用；0--禁用；
        /// </summary>
        public int UseHTTPClient { get; set; }

        /// <summary>
        /// http协议服务器地址
        /// </summary>
        public string? HTTPClient_ServerAddr { get; set; }

        /// <summary>
        /// httpclient 协议的保活包间隔时间
        /// </summary>
        public int HTTPClient_KeepaliveTime { get; set; }

        /// <summary>
        /// http 请求时是否使用GZIP压缩 0--不使用；1--使用
        /// </summary>
        public int HTTPClient_UseGZIP { get; set; }

        /// <summary>
        /// HTTPClient 的协议类型   100 --- HTTPv1   200 ---HTTPv2
        /// </summary>
        public int HTTPClient_ProtocolType { get; set; }





        /// <summary>
        /// 是否启动 MQTTClient 协议  1--启用；0--禁用；
        /// </summary>
        public int UseMQTTClient { get; set; }

        /// <summary>
        /// 是否启用MQTT的SSL安全套接字 1--启用；0--禁用；
        /// </summary>
        public int UseMQTTSSL { get; set; }

        /// <summary>
        /// MQTT服务器地址   www.abc.com
        /// </summary>
        public string? MQTTServerAddr { get; set; }

        /// <summary>
        /// MQTT服务器端口号
        /// </summary>
        public int MQTTPort { get; set; }

        /// <summary>
        /// MQTT 协议中 客户端ID
        /// </summary>
        public string? MQTTClientID { get; set; }

        /// <summary>
        /// MQTT 协议中 登录用户名
        /// </summary>
        public string? MQTTLoginName { get; set; }

        /// <summary>
        /// MQTT 协议中 登录密码
        /// </summary>
        public string? MQTTLoginPassword { get; set; }

        /// <summary>
        /// MQTT 协议中 设备发送数据使用的Topic
        /// </summary>
        public string? MQTTPublishTopic { get; set; }


        /// <summary>
        /// MQTT 协议中 设备接收数据需要订阅的Topic
        /// </summary>
        public string? MQTTSubscribeTopic { get; set; }


        /// <summary>
        /// MQTT 协议的保活包间隔时间
        /// </summary>
        public int MQTT_KeepaliveTime { get; set; }

        /// <summary>
        /// MQTT 协议是否使用gzip压缩包 <br />
        /// 1--启用；<br />
        /// 0--禁用； 禁用gzip压缩包时，将直接使用json字符串作为传输格式
        /// </summary>
        public int MQTT_UseGZIP { get; set; }




        /// <summary>
        /// 是否启动 WebsocketClient 协议  1--启用；0--禁用；
        /// </summary>
        public int UseWebsocketClient { get; set; }

        /// <summary>
        /// Websocket协议 服务器地址  ws://192.168.1.1/websocket   or  wss://192.168.1.1/websocket
        /// </summary>
        public string? WebsocketClient_ServerAddr { get; set; }

        /// <summary>
        /// WebsocketClient 协议的保活包间隔时间
        /// </summary>
        public int WebsocketClient_KeepaliveTime { get; set; }

        /// <summary>
        /// Websocket 是否使用GZIP压缩 0--不使用；1--使用
        /// </summary>
        public int WebsocketClient_UseGZIP { get; set; }

        /// <summary>
        /// Websocket 的协议类型
        /// </summary>
        public int? WebsocketClient_ProtocolType { get; set; }





        /// <summary>
        /// 是否启动 云筑网 HTTPClient 协议  1--启用；0--禁用；
        /// </summary>
        public int UseYZW { get; set; }

        /// <summary>
        /// 云筑网协议 服务器地址  
        /// </summary>
        public string? YZWAddr { get; set; }

        /// <summary>
        /// 云筑网协议 禁止上传记录   1--禁止；0--允许；
        /// </summary>
        public int YZW_NotUploadRecord { get; set; }

        /// <summary>
        /// 云筑网协议 禁止上传工人近照  1--禁止；0--允许；
        /// </summary>
        public int YZW_NotUploadUserPhoto { get; set; }






        /// <summary>
        /// 客户端证书 pem格式
        /// </summary>
        public string? ClientCert { get; set; }
        /// <summary>
        /// 客户端证书密钥 pem格式
        /// </summary>
        public string? ClientCertKey { get; set; }
        /// <summary>
        /// 服务器的CA证书,用于验证服务器的合法性 pem格式
        /// </summary>
        public string? CACert { get; set; }
        #endregion

        #region 机器网络参数 Network

        /// <summary>
        /// 有线网络开关,1:打开,0:关闭
        /// </summary>
        public int UseWired { get; set; }

        /// <summary>
        /// 有线网络使用DHCP自动 ip,1:开,0:关
        /// </summary>
        public int WiredDHCP { get; set; }

        /// <summary>
        /// 有线网络 Ip 地址  192.168.1.110
        /// </summary>
        public string? WiredIP { get; set; }

        /// <summary>
        /// 有线网络 子网掩码 255.255.255.0
        /// </summary>
        public string? WiredIPMask { get; set; }

        /// <summary>
        /// 有线网络网关 192.168.1.1
        /// </summary>
        public string? WiredGteway { get; set; }

        /// <summary>
        /// 有线网络 DNS  8.8.8.8
        /// </summary>
        public string? WiredDNS { get; set; }

        /// <summary>
        /// 有线网络 MAC地址
        /// </summary>
        public string? WiredMAC { get; set; }


        /// <summary>
        /// 无线网络开关,1:打开,0:关闭
        /// </summary>
        public int UseWIFI { get; set; }

        /// <summary>
        /// 无线网络AP热点名称
        /// </summary>
        public string? WIFIAPName { get; set; }

        /// <summary>
        /// 无线网络AP密码
        /// </summary>
        public string? WIFIAPPassword { get; set; }

        /// <summary>
        /// 无线网络使用DHCP自动 ip,1:开,0:关
        /// </summary>
        public int WIFIDHCP { get; set; }

        /// <summary>
        /// 无线网络 Ip 地址  192.168.1.110
        /// </summary>
        public string? WIFIIP { get; set; }

        /// <summary>
        /// 无线网络 子网掩码 255.255.255.0
        /// </summary>
        public string? WIFIIPMask { get; set; }

        /// <summary>
        /// 无线网络网关 192.168.1.1
        /// </summary>
        public string? WIFIGteway { get; set; }

        /// <summary>
        /// 无线网络 DNS  8.8.8.8
        /// </summary>
        public string? WIFIDNS { get; set; }

        /// <summary>
        /// 无线网络 MAC地址
        /// </summary>
        public string? WIFIMAC { get; set; }



        /// <summary>
        /// Web管理页面开关,1:开,0:关
        /// </summary>
        public int UseWebPage { get; set; }

        /// <summary>
        /// Web页面 HTTP端口号, 1-65534
        /// </summary>
        public int HTTPPort { get; set; }

        /// <summary>
        /// Web页面 HTTPS端口号, 1-65534
        /// </summary>
        public int HTTPSPort { get; set; }

        /// <summary>
        /// 设备Web页面开启SSL``SSL证书使用OpenSSL自签名 1:开,0:关
        /// </summary>
        public int WebPageUseSSL { get; set; }



        /// <summary>
        /// 一卡通协议 UDP端口开关 ,1:开,0:关
        /// </summary>
        public int UseUDP { get; set; }

        /// <summary>
        /// 一卡通协议 UDP端口号 （一卡通协议使用）
        /// </summary>
        public int UDPPort { get; set; }
        /// <summary>
        /// 一卡通协议通讯密码 8位数字
        /// </summary>
        public string? ConnectPassword { get; set; }


        /// <summary>
        /// 一卡通协议 TCP端口开关 ,1:开,0:关
        /// </summary>
        public int UseTCP { get; set; }

        /// <summary>
        /// 一卡通协议 TCP端口号 （一卡通协议使用）
        /// </summary>
        public int TCPPort { get; set; }


        /// <summary>
        /// 一卡通协议 SSL端口开关 ,1:开,0:关
        /// </summary>
        public int UseTCPSSL { get; set; }

        /// <summary>
        /// 一卡通协议 SSL端口号 （一卡通协议使用）
        /// </summary>
        public int TCPSSLPort { get; set; }



        /// <summary>
        /// 一卡通协议 IPV6 UDP端口开关 ,1:开,0:关
        /// </summary>
        public int UseIPV6_UDP { get; set; }

        /// <summary>
        /// 一卡通协议 IPV6 UDP端口号 （一卡通协议使用）
        /// </summary>
        public int IPv6_UDPPort { get; set; }

        /// <summary>
        /// 一卡通协议 IPV6 TCP端口开关 ,1:开,0:关
        /// </summary>
        public int UseIPV6_TCP { get; set; }

        /// <summary>
        /// 一卡通协议 IPV6 TCP端口号 （一卡通协议使用）
        /// </summary>
        public int IPv6_TCPPort { get; set; }

        /// <summary>
        /// 一卡通协议 IPV6 SSL端口开关 ,1:开,0:关
        /// </summary>
        public int UseIPV6_TCPSSL { get; set; }

        /// <summary>
        /// 一卡通协议 IPV6 SSL端口号 （一卡通协议使用）
        /// </summary>
        public int IPv6_TCPSSLPort { get; set; }




        /// <summary>
        ///一卡通协议通讯使用加密数据包 <br />
        ///0--禁用；1--RC4；2--SM4+CBC
        /// </summary>
        public int UseDataEncryption { get; set; }

        /// <summary>
        ///使用HEX编码保存的 32字节加密算法的秘钥; <br />
        ///64个十六进制字符
        /// </summary>
        public string? EncryptionKey { get; set; }



        /// <summary>
        /// 启用 Linux Telnet 1:开,0:关
        /// </summary>
        public int UseTelnet { get; set; }

        /// <summary>
        /// Telnet端口号, 1-65534
        /// </summary>
        public int TelnetPort { get; set; }


        /// <summary>
        /// 启用 Linux SSH 1:开,0:关
        /// </summary>
        public int UseSSH { get; set; }

        /// <summary>
        /// SSH端口号
        /// </summary>
        public int SSHPort { get; set; }

        /// <summary>
        /// SSH或Telnet 登录密码
        /// </summary>
        public string? SSHLoginPassword { get; set; }




        /// <summary>
        /// 启用视频流 1:开,0:关
        /// </summary>
        public int UseRTSP { get; set; }


        #endregion

        #region 视频对讲 VideoCall
        /// <summary>
        /// 对讲功能  1--启用；0--禁用
        /// </summary>
        public int VideoCall_Use { get; set; } = 0;//

        /// <summary>
        /// 本机名称
        /// </summary>
        public string? VideoCall_DeviceName { get; set; }//

        /// <summary>
        /// 拨号方式  1 --直呼；2--拨号；3 -- 直呼(本楼栋)
        /// </summary>
        public int VideoCall_DialMode { get; set; } = 0;//

        /// <summary>
        /// 设备类型  1--楼栋机   2--围墙机
        /// </summary>
        public int VideoCall_DeviceType { get; set; } = 0;//

        /// <summary>
        /// 楼栋号
        /// </summary>
        public string? VideoCall_BuildCode { get; set; }//

        /// <summary>
        /// 本机号
        /// </summary>
        public string? VideoCall_LocalCode { get; set; }//

        /// <summary>
        /// 服务中心号码
        /// </summary>
        public string? VideoCall_Help { get; set; }//

        /// <summary>
        /// 安保中心号码
        /// </summary>
        public string? VideoCall_SecurityHelp { get; set; }//


        /// <summary>
        /// SIP电话功能开关 1--启用；0--禁用
        /// </summary>
        public int SIP_Use { get; set; } = 0;//

        /// <summary>
        /// SIP服务器地址 
        /// </summary>
        public string? SIP_Server { get; set; }//

        /// <summary>
        /// SIP服务器端口
        /// </summary>
        public int SIP_Port { get; set; } = 5060;//

        /// <summary>
        /// SIP用户名
        /// </summary>
        public string? SIP_UserName { get; set; }//

        /// <summary>
        /// SIP密码
        /// </summary>
        public string? SIP_Password { get; set; }//

        /// <summary>
        /// 紧急电话
        /// </summary>
        public string? SIP_EmergencyCall { get; set; }//


        /// <summary>
        /// STUN 功能开关 1--启用；0--禁用
        /// </summary>
        public int STUN_Use { get; set; } = 0;//

        /// <summary>
        /// STUN服务器地址
        /// </summary>
        public string? STUN_Server { get; set; }//

        /// <summary>
        /// STUN服务器端口
        /// </summary>
        public int STUN_Port { get; set; } = 3478;//

        /// <summary>
        /// STUN用户名
        /// </summary>
        public string? STUN_UserName { get; set; }//

        /// <summary>
        /// STUN密码
        /// </summary>
        public string? STUN_Password { get; set; }//

        /// <summary>
        /// TURN 功能开关 1--启用；0--禁用
        /// </summary>
        public int TURN_Use { get; set; } = 0;//

        /// <summary>
        /// TURN服务器地址
        /// </summary>
        public string? TURN_Server { get; set; }//

        /// <summary>
        /// TURN服务器端口
        /// </summary>
        public int TURN_Port { get; set; } = 3478;//

        /// <summary>
        /// TURN用户名
        /// </summary>
        public string? TURN_UserName { get; set; }//

        /// <summary>
        /// TURN密码
        /// </summary>
        public string? TURN_Password { get; set; }//
        #endregion


        #region 门禁参数 Door
        /// <summary>
        /// 卡号字节；3、4、8；0--表示禁用读卡
        /// </summary>
        public int CardBytes { get; set; }


        /// <summary>
        /// 出入类型 0,入门；1,出门
        /// </summary>
        public int AccessType { get; set; }

        /// <summary>
        /// WG输出格式 26 / 34/66  / 0 禁用
        /// </summary>
        public int WgFormat { get; set; }

        /// <summary>
        /// WG字节顺序：1--高位在前低位在后；2--低位在前高位在后
        /// </summary>
        public int WgBytesSort { get; set; }

        /// <summary>
        /// WG输出内容： 1--用户号；2--卡号
        /// </summary>
        public int WGContent { get; set; }

        /// <summary>
        /// 开门保持时间 0-65535（s）。0表示0.5秒
        /// </summary>
        public int ReleaseTime { get; set; }

        /// <summary>
        /// 延迟开锁时间 0-65535（s）。0表示禁用
        /// </summary>
        public int DelayOpenDoorTime { get; set; }

        /// <summary>
        /// 免验证开门 1--启用；0--禁用
        /// </summary>
        public int FreeOpen { get; set; }

        /// <summary>
        /// 开门识别间隔 0--禁用  { get; set; } 1-65535（ms）
        /// </summary>
        public int OpenInterval { get; set; }

        /// <summary>
        /// 开门识别间隔期间，是否保存记录 0--不保存；1--保存
        /// </summary>
        public int OpenInterval_SaveRecord { get; set; }

        /// <summary>
        /// 继电器否支持双稳态``1为支持,0为不支持
        /// </summary>
        public int Relay { get; set; }

        /// <summary>
        /// 合法验证后的短消息
        /// </summary>
        public string? ShortMessage { get; set; }

        /// <summary>
        /// 验证方式   
        /// <para> 1、标准模式； 2、人脸/指纹/掌静脉/刷卡 + 密码           </para>
        /// <para> 2、人脸 / 指纹 / 掌静脉 / 刷卡 + 密码                   </para>
        /// <para> 3、 刷卡 + 人脸 / 指纹 / 掌静脉 / 密码                  </para>
        /// <para> 4、多人考勤    5、人证比对                              </para>
        /// <para> 6、刷卡 + 人脸 / 指纹 / 掌静脉 + 密码                   </para>
        /// <para> 7、刷卡 + 指纹 / 掌静脉 + 人脸                          </para>
        /// <para> 8、指纹 / 掌静脉 + 人脸 + 密码                          </para>
        /// <para> 9、指纹 + 掌静脉 + 人脸                                 </para>
        /// <para> 10、掌静脉 + 人脸；11、指纹 + 人脸                      </para>
        /// <para> 12、只用 掌静脉； 13、只用 指纹； 14、只用 刷卡； 15、只用 密码；</para>
        /// <para> 16、人证比对开门 + 已注册人（身份证 + 人脸 + 已注册）   </para>
        /// </summary>
        public int VerificationType { get; set; }

        /// <summary>
        /// 权限到期提示 1--启用；0--禁用
        /// </summary>
        public int OverdueRemind { get; set; }

        /// <summary>
        /// 权限到期提示天数 1-255
        /// </summary>
        public int OverdueRemind_Day { get; set; }

        /// <summary>
        /// 定时常开功能  1--启用；0--禁用
        /// </summary>
        public int TimingOpen { get; set; }

        /// <summary>
        /// 定时常开.自动开模式
        /// <para> 1、合法认证通过后在指定时段内即可常开                   </para>
        /// <para> 2、授权中标记为常开特权的在指定时段内认证通过即可常开   </para>
        /// <para> 3、自动开关,到时间自动开关门                            </para>
        /// </summary>
        public int TimingOpen_mode { get; set; }

        /// <summary>
        /// 定时常开.常开时段 使用周时段结构
        /// </summary>
        public HTTPDeviceParameterTimegroup? TimingOpen_timegroup { get; set; }


        /// <summary>
        /// 定时锁定功能  1--启用；0--禁用
        /// </summary>
        public int TimingLocked { get; set; }

        /// <summary>
        /// 定时锁定.锁定时段 使用周时段结构
        /// </summary>
        public HTTPDeviceParameterTimegroup? TimingLocked_timegroup { get; set; }

        /// <summary>
        /// 访客根密码
        /// </summary>
        public string? VisitorRootPassword { get; set; }

        /// <summary>
        /// 多人组合开门，人数；1-50；
        /// </summary>
        public int MultiPerson { get; set; }

        /// <summary>
        /// 每日次数限制，0--等禁用此功能；1--启用此功能
        /// </summary>
        public int DailyLimit { get; set; }

        #endregion

        #region 电梯功能参数 Elevator
        /// <summary>
        /// 电梯功能开关,1:开,0:关
        /// </summary>
        public int UseElevator { get; set; }
        public List<HTTPElevatorPort>? ElevatorPorts { get; set; }
        #endregion

        #region 报警参数 Alarm
        /// <summary>
        /// 消防报警,0,关闭；1,开启
        /// </summary>
        public int FireAlarm { get; set; }


        /// <summary>
        ///开门超时报警开关,1:开,0:关
        /// </summary>
        public int DoorLongOpenAlarm { get; set; }

        /// <summary>
		/// 开门超时时间,门打开超过这个时间就报警	1-65535（s）
		/// </summary>
        public int DoorLongOpenTime { get; set; }



        /// <summary>
		/// 门磁报警,0,关闭；1,开启
		/// </summary>
        public int DoorSensorAlarm { get; set; }

        /// <summary>
		/// 门磁报警不报警时段,周时段格式
		/// </summary>
        public HTTPDeviceParameterTimegroup? DoorSensorAlarmTimegroup { get; set; }




        /// <summary>
        /// 黑名单报警,0,关闭；1,开启
        /// </summary>
        public int BlacklistAlarm { get; set; }


        /// <summary>
        /// 防拆报警功能开关,0,关闭；1,开启
        /// </summary>
        public int AntiDisassemblyAlarm { get; set; }



        /// <summary>
        /// 非法验证报警功能,0,关闭；1,开启
        /// </summary>
        public int IllegalVerificationAlarm { get; set; }

        /// <summary>
        /// 非法验证报警功能-非法认证次数,1-255 ，超过此次数报警
        /// </summary>
        public int IllegalVerificationAlarmLimit { get; set; }



        /// <summary>
        /// 允许用户验证解除报警  开关,0,关闭；1,开启
        /// </summary>
        public int UseUserCloseAlarm { get; set; }



        /// <summary>
        /// 胁迫报警密码功能,0,关闭；1,开启
        /// </summary>
        public int PasswordAlarm { get; set; }

        /// <summary>
        /// 胁迫报警密码,输入此密码则发生报警,密码仅支持数字,可以包含0。
        /// </summary>
        public string? PasswordAlarm_Password { get; set; }

        /// <summary>
        /// 胁迫报警报警发生时的工作模式 
        /// <para>1--不开门,报警输出          </para>
        /// <para>2--开门,报警输出            </para>
        /// <para>3--锁定门,报警,只能软件解锁 </para>
        /// </summary>
        public int PasswordAlarm_Mode { get; set; }
        #endregion

        #region 设备开门时段 Timegroup
        /// <summary>
        /// 时段列表列表
        /// </summary>
        public List<HTTPOpenDoorTimegroupV2>? TimeGroups { get; set; }
        #endregion

        #region 设备节假日 Holiday
        /// <summary>
        /// 节假日列表
        /// </summary>
        public List<HTTPDeviceHolidayDay>? Holidays { get; set; }
        #endregion

        #region 闹铃 AlarmClock
        /// <summary>
        /// 闹铃列表 闹铃有24个
        /// </summary>
        public List<HTTPAlarmClockTime>? AlarmClocks { get; set; }
        #endregion

    }
}
