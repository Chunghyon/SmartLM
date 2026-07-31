using FaceWebServer.DTO.HTTPv2_Protocol;
using FaceWebServer.Utility.VerifyAttribute;
using System.Collections.Generic;

namespace FaceWebServer.DTO.HTTPv1_Protocol
{
    public class HTTPDeviceParameterCoreV1
    {
        /// <summary>
        /// 设备 SN
        /// </summary>
        [VerifyText(iMax: 16, iMin: 16, required: false,
            errCode: 1, sErrorDesc: "设备SN由16个字符组成", lngcode: "r31")]
        public string DeviceID { get; set; }  // 是

        /// <summary>
        /// 同步系统时间（时间戳，比如：1584427174 对应是2020-03-17 14:39:34）
        /// </summary>
        public long ParameterSystemTime { get; set; }

        #region 语音
        /// <summary>
        /// 音量大小(范围 0-10)  
        /// </summary>
        [VerifyNum(iMax: 10, iMin: 0, required: false,
                    errCode: 3, sErrorDesc: "音量超出范围(范围 0-10)", lngcode: "r32")]
        public int? ParameterVolume { get; set; }  // 是


        /// <summary>
        /// 语音模式 0,不播报；1，播放名字; 2,播放问候语;3,播放名字和问候语
        /// </summary>
        [VerifyNumRange(iRange: new int[] { 0, 1, 2, 3 }, required: false,
                       errCode: 4, sErrorDesc: "语音模式，超出范围:0,不播报；1，播放名字; 2,播放问候语;3,播放名字和问候语", lngcode: "r33")]
        public int? ParameterVoiceMode { get; set; }  // 是

        /// <summary>
        /// 问候语 0，请通行;1,欢迎光临;2，时间问候语;3、消费登记成功
        /// </summary>
        [VerifyNumRange(iRange: new int[] { 0, 1, 2, 3 }, required: false,
                       errCode: 5, sErrorDesc: "问候语，超出范围:0，请通行;1,欢迎光临;2，时间问候语;3、消费登记成功", lngcode: "r34")]
        public int? ParameterGrettings { get; set; }  // 是

        /// <summary>
        /// 陌生人语音 0，不播报;1,播报假体;2，播报陌生人；3，播报假体和陌生人
        /// </summary>
        [VerifyNumRange(iRange: new int[] { 0, 1, 2, 3 }, required: false,
                       errCode: 6, sErrorDesc: "陌生人语音，超出范围:0，不播报;1,播报假体;2，播报陌生人；3，播报假体和陌生人", lngcode: "r35")]
        public int? ParameterStrangerVoice { get; set; }  // 是
        #endregion


        #region 补光灯
        /// <summary>
        /// 补光灯开关：0：常闭；  1：常开； 2：自动；
        /// </summary>
        [VerifyNumRange(iRange: new int[] { 0, 1, 2 }, required: false,
                    errCode: 7, sErrorDesc: "补光灯开关取值范围(0：常闭；  1：常开； 2：自动)", lngcode: "r36")]
        public int? ParameterLightSwitch { get; set; }  // 是
        #endregion


        #region UI界面


        /// <summary>
        /// 亮度设置 1-10
        /// </summary>
        [VerifyNum(iMax: 10, iMin: 1, required: false,
                      errCode: 8, sErrorDesc: "亮度设置，超出范围: 1-10", lngcode: "r37")]
        public int? ParameterBrightness { get; set; }  // 是
        /// <summary>
        /// 曝光设置
        /// </summary>
        [VerifyNum(iMax: 3, iMin: -3, required: false,
              errCode: 9, sErrorDesc: "曝光设置，超出范围: -3  -  3 ", lngcode: "r38")]
        public int? ParameterExposure { get; set; }  // 是



        /// <summary>
        /// 红外图像开关 0--关闭；1--开启
        /// </summary>
        [VerifyNumRange(iRange: new int[] { 0, 1 }, required: false,
                       errCode: 10, sErrorDesc: "红外图像开关 0--关闭；1--开启", lngcode: "r39")]
        public int? ParameterIR { get; set; }  // 是

        /// <summary>
        /// 语言选择 0，中文；1，英文；2，繁体
        /// </summary>
        [VerifyNumRange(iRange: new int[] { 0, 1, 2, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 }, required: false,
                       errCode: 12, sErrorDesc: "UI界面语言，超出范围:0，中文；1，英文；2，繁体", lngcode: "r41")]
        public int? ParameterLanguageChoose { get; set; }  // 是

        /// <summary>
        /// 主界面设置--公司名称
        /// </summary>
        [VerifyText(iMax: 20, iMin: 0, required: false,
                       errCode: 11, sErrorDesc: "公司名称，长度过长，不可超过20个字符", lngcode: "r40")]
        public string ParameterCompanyName { get; set; }  // 是


        /// <summary>
        /// 菜单密码
        /// </summary>
        [VerifyNumText(iMax: 8, iMin: 4, required: false,
                    errCode: 13, sErrorDesc: "菜单密码由4-8个数字组成", lngcode: "r42")]
        public string ParameterLoginPassword { get; set; }  // 是


        /// <summary>
        /// 设备时区
        /// </summary>
        public int TimeZone { get; set; }
        #endregion



        #region 身份信息
        /// <summary>
        /// 固件版本信息
        /// </summary>
        public string ParameterFirmwareMsg { get; set; }

        /// <summary>
        /// 序列号
        /// </summary>
        [VerifyText(iMax: 16, iMin: 16, required: false,
            errCode: 14, sErrorDesc: "设备新的SN由16个字符组成", lngcode: "r43")]
        public string ParameterSerialNum { get; set; }  // 是

        /// <summary>
        /// 制造商
        /// </summary>
        [VerifyText(iMax: 50, iMin: 0, required: false,
                       errCode: 15, sErrorDesc: "制造商，长度过长，不可超过50个字符", lngcode: "r44")]
        public string ParameterManufacturer { get; set; }  // 是

        /// <summary>
        /// 网址
        /// </summary>
        [VerifyText(iMax: 50, iMin: 0, required: false,
                       errCode: 16, sErrorDesc: "制造商网址，长度过长，不可超过50个字符", lngcode: "r45")]
        public string ParameterWebsite { get; set; }  // 是

        /// <summary>
        /// 生产日期
        /// </summary>
        [VerifyText(iMax: 50, iMin: 0, required: false,
               errCode: 17, sErrorDesc: "生产日期，格式不正确", lngcode: "r46")]
        public string ParameterProductionDate { get; set; }  // 是
        #endregion



        #region 记录存储
        /// <summary>
        /// 是否存储非雇员识别记录，0:不存储，1:存储
        /// </summary>
        [VerifyNumRange(iRange: new int[] { 1, 0 }, required: false,
                            errCode: 18, sErrorDesc: "保存陌生人打卡记录，超出范围:0:关闭，1:打开", lngcode: "r47")]
        public int? ParameterSaveExternalvisitors { get; set; }  // 是

        /// <summary>
        /// 保存现场图片 0，不保存；1，保存
        /// </summary>
        [VerifyNumRange(iRange: new int[] { 1, 0 }, required: false,
                       errCode: 19, sErrorDesc: "保存现场图片，超出范围:0:关闭，1:打开", lngcode: "r48")]
        public int? ParameterSavePicture { get; set; }  // 是
        #endregion


        #region 人脸识别

        /// <summary>
        /// 活体检测,1 打开，0 关闭
        /// </summary>
        [VerifyNumRange(iRange: new int[] { 1, 0 }, required: false,
            errCode: 20, sErrorDesc: "活体检测的取值选项仅为：0 关闭；1 打开。", lngcode: "r49")]
        public int? ParameterPreview { get; set; }  // 是

        /// <summary>
        /// 活体检测阈值1-10（2021年8月3日 新增）
        /// </summary>
        [VerifyNum(iMax: 10, iMin: 1, required: false,
            errCode: 21, sErrorDesc: " 活体检测阈值：1 - 10。", lngcode: "r50")]
        public int? ParameterFaceIRThreshold { get; set; }  // 是


        /// <summary>
        /// 识别距离 1--远（默认值）、2--中、3--近  
        /// </summary>
        [VerifyNumRange(iRange: new int[] { 1, 2, 3 }, required: false,
                      errCode: 22, sErrorDesc: "识别距离，超出范围: 1--远（默认值）、2--中、3--近", lngcode: "r51")]
        public int? ParameterDistance { get; set; }  // 是

        /// <summary>
        /// 人脸识别阈值   1-99
        /// </summary>
        [VerifyNum(iMax: 99, iMin: 1, required: false,
                    errCode: 24, sErrorDesc: "人脸识别阈值,超出范围：1-99。", lngcode: "r52")]
        public int? ParameterFaceThreshold { get; set; }  // 是

        /// <summary>
        /// 1：打开口罩检测，其他：关闭
        /// </summary>
        [VerifyNumRange(iRange: new int[] { 1, 0 }, required: false,
                      errCode: 25, sErrorDesc: "口罩检测开关，超出范围:0:关闭，1:打开", lngcode: "r53")]
        public int? ParameterMask { get; set; }  // 是
        /// <summary>
        /// 口罩阈值 1-99
        /// </summary>
        [VerifyNum(iMax: 99, iMin: 1, required: false,
                       errCode: 26, sErrorDesc: "口罩阈值，超出范围: 1-99", lngcode: "r54")]
        public int? ParameterMaskThreshold { get; set; }  // 是
        #endregion

        #region 体温检测

        /// <summary>
        /// 测温模式开关。0：非测温模式 1：测温模式
        /// </summary>
        [VerifyNumRange(iRange: new int[] { 1, 0 }, required: false,
                    errCode: 27, sErrorDesc: "测温模式开关,超出范围:0 关闭；1 打开", lngcode: "r55")]
        public int? ParameterTempSwitch { get; set; }  // 是

        /// <summary>
        /// 开启华氏温度,1:开，0:关
        /// </summary>
        [VerifyNumRange(iRange: new int[] { 1, 0 }, required: false,
                       errCode: 28, sErrorDesc: "华氏温度显示开启开关，超出范围:0:关闭，1:打开", lngcode: "r56")]
        public int? ParameterFahrenheitSwitch { get; set; }  // 是

        /// <summary>
        /// -1~1,温度补偿值
        /// </summary>
        [VerifyFloat(iMax: 1, iMin: -1, required: false,
                    errCode: 29, sErrorDesc: "温度补偿值,超出范围：-1~1。", lngcode: "r57")]
        public float? ParameterCompensate { get; set; }  // 是 

        /// <summary>
        /// 温度比对阈值（最大值）
        /// </summary>
        [VerifyFloat(iMax: 50, iMin: 37, required: false,
                    errCode: 30, sErrorDesc: "温度比对阈值（最大值）,超出范围：0-100。", lngcode: "r58")]
        public float? ParameterTempThresholdMax { get; set; }  // 是
        #endregion

        #region 网络参数
        /// <summary>
        /// 服务器地址
        /// </summary>
        [VerifyText(iMax: 100, iMin: 0, required: false,
                       errCode: 31, sErrorDesc: "服务器地址验证失败，必填，不可超过100个字符", lngcode: "r59")]
        public string ParameterCloudserverAddress { get; set; }  // 是
        /// <summary>
        /// 主动触发云端轮训时间（以秒为单位）  5-7200s
        /// </summary>
        [VerifyNum(iMax: 7200, iMin: 2, required: false,
                    errCode: 32, sErrorDesc: "设备保活包间隔（秒）：2-7200。", lngcode: "r60")]
        public int? ParameterPolling { get; set; }  // 是


        /// <summary>
        /// 服务器协议：0--http；1--云筑网；2--websocket；
        /// </summary>
        [VerifyNum(iMax: 2, iMin: 0, required: false,
                    errCode: 32, sErrorDesc: "服务器协议: 0--http；1--云筑网；2--websocket；", lngcode: "r60")]
        public int? ParameterServerProtocol { get; set; }  // 是
        #region 有线网络


        #endregion
        #endregion


        #region 网络扩展参数

        /// <summary>
        ///有线网络功能开关
        /// </summary>
        [VerifyNumObject(iMax: 1, iMin: 0, required: false, bAutoDef: false, defValue: 0,
                    errCode: 54, sErrorDesc: "有线网络功能开关,1:开，0:关。", lngcode: "r137")]
        public int? ParameterNetworkSwitch { get; set; }  // 是

        /// <summary>
        /// 有线网络 DHCP
        /// </summary>
        [VerifyNumObject(iMax: 1, iMin: 0, required: false, bAutoDef: false, defValue: 0,
                     errCode: 54, sErrorDesc: "DHCP功能开关,1:开，0:关。", lngcode: "r137")]
        public int? ParameterNetworkIpActive { get; set; }

        /// <summary>
        /// 有线网络 IP
        /// </summary>
        [VerifyIP(errCode: 1, sErrorDesc: "IP：格式错误", lngcode: "r151")]
        public string ParameterNetworkIpUrl { get; set; }
        /// <summary>
        /// 有线网络 子网掩码
        /// </summary>
        [VerifyIP(errCode: 1, sErrorDesc: "子网掩码：格式错误", lngcode: "r151")]
        public string ParameterNetworkSubnetMask { get; set; }
        /// <summary>
        /// 有线网络 网关
        /// </summary>
        [VerifyIP(errCode: 1, sErrorDesc: "网关：格式错误", lngcode: "r151")]
        public string ParameterNetworkGteway { get; set; }

        /// <summary>
        /// 有线网络 Dns
        /// </summary>
        [VerifyIP(errCode: 1, sErrorDesc: "Dns：格式错误", lngcode: "r151")]
        public string ParameterNetworkDns { get; set; }

        /// <summary>
        /// 无线网开关，1:开，0:关
        /// </summary>
        public int? ParameterWifiSwitch { get; set; }

        /// <summary>
        /// 无线网账号
        /// </summary>
        public string? ParameterWifiName { get; set; }

        /// <summary>
        /// 无线网密码
        /// </summary>
        public string? ParameterWifiPassWord { get; set; }



        /// <summary>
        /// Web页面管理开关,1:开，0:关
        /// </summary>
        [VerifyNumObject(iMax: 1, iMin: 0, required: false, bAutoDef: false, defValue: 0,
                    errCode: 51, sErrorDesc: "Web页面管理开关,1:开，0:关", lngcode: "r135")]
        public int? ParameterEnableWeb { get; set; }  // 是

        /// <summary>
        /// Telnet协议功能
        /// </summary>
        [VerifyNumObject(iMax: 1, iMin: 0, required: false, bAutoDef: false, defValue: 0,
                    errCode: 52, sErrorDesc: "Linux命令行开关 ,1:开，0:关", lngcode: "r136")]
        public int? ParameterEnableTelnet { get; set; }  // 是

        /// <summary>
        /// UDP搜索功能开关
        /// </summary>
        [VerifyNumObject(iMax: 1, iMin: 0, required: false, bAutoDef: false, defValue: 0,
                    errCode: 54, sErrorDesc: "UDP搜索功能开关,1:开，0:关。", lngcode: "r137")]
        public int? ParameterEnableUDP { get; set; }  // 是

        /// <summary>
        /// UDP端口号 （UDP广播搜索发现用）
        /// </summary>
        [VerifyNumObject(iMax: 65534, iMin: 1, required: false, bAutoDef: false, defValue: 8101,
                    errCode: 50, sErrorDesc: "UDP端口号：1-65534。", lngcode: "r134")]
        public int? ParameterUDPPort { get; set; }  // 是

        /// <summary>
        /// 是否使用HTTP协议中的GZIP压缩算法
        /// </summary>
        [VerifyNumObject(iMax: 1, iMin: 0, required: false, bAutoDef: false, defValue: 0,
                    errCode: 53, sErrorDesc: "HTTP协议中的GZIP压缩算法开关 ,1:开，0:关", lngcode: "r138")]
        public int? ParameterHttpPostUseGZIP { get; set; }
        #endregion


        #region 门禁参数
        /// <summary>
        /// 出入类型 0,入门；1，出门
        /// </summary>
        [VerifyNumRange(iRange: new int[] { 1, 0 }, required: false,
                       errCode: 33, sErrorDesc: "出入类型，超出范围: 0,入门；1，出门", lngcode: "r61")]
        public int? ParameterAccessType { get; set; }  // 是

        /// <summary>
        /// WG输出格式 26 / 34
        /// </summary>
        [VerifyNumRange(iRange: new int[] { 26, 34, 66 }, required: false,
                       errCode: 34, sErrorDesc: "WG输出格式，超出范围: 26 或 34 或 66", lngcode: "r62")]
        public int? ParameterWgFormat { get; set; }  // 是

        /// <summary>
        /// 开门保持时间 1-65535（s）
        /// </summary>
        [VerifyNum(iMax: 65535, iMin: 0, required: false,
                       errCode: 35, sErrorDesc: "开门保持时间，超出范围: 0-65535（s）", lngcode: "r63")]
        public int? ParameterKeepOpenDoor { get; set; }  // 是

        /// <summary>
        /// 免验证开门 1--启用；0--禁用
        /// </summary>
        [VerifyNumRange(iRange: new int[] { 0, 1 }, required: false,
                       errCode: 36, sErrorDesc: "免验证开门，超出范围:1--启用；0--禁用", lngcode: "r64")]
        public int? ParameterLaissezSwitch { get; set; }  // 是

        /// <summary>
        /// 识别间隔 0--禁用 ; 1-65535（s）
        /// </summary>
        [VerifyNum(iMax: 65535, iMin: 0, required: false,
                       errCode: 37, sErrorDesc: "识别间隔，超出范围: 0--禁用 ; 1-65535（s）", lngcode: "r65")]
        public int? ParameterRecgInterval { get; set; }  // 是

        /// <summary>
        /// 间隔记录存储设置 0,关闭；1，打开
        /// </summary>
        [VerifyNumRange(iRange: new int[] { 0, 1 }, required: false,
                       errCode: 38, sErrorDesc: "验证间隔，重复记录存储设置，超出范围:0,关闭；1，打开", lngcode: "r66")]
        public int? ParameterIntervalRecoSwitch { get; set; }  // 是


        #region 报警参数
        /// <summary>
        /// 消防报警参数
        /// </summary>
        [VerifyNum(iMax: 1, iMin: 0, required: false,
                      errCode: 54, sErrorDesc: "消防报警，超出范围:0，关闭；1，开启", lngcode: "r143")]
        public int? ParameterFireAlarm { get; set; }  // 

        /// <summary>
        /// 开门超时报警开关，0，关闭；1，开启
        /// </summary>

        [VerifyNumRange(iRange: new int[] { 0, 1 }, required: false,
                      errCode: 39, sErrorDesc: "开门超时报警，超出范围:0，关闭；1，开启", lngcode: "r67")]
        public int? ParameterDoorLongOpenAlarmSwitch { get; set; }  // 是

        /// <summary>
        /// 开门超时时间，门打开超过这个时间就报警	5-255（s）
        /// </summary>
        [VerifyNum(iMax: 255, iMin: 5, required: false,
                       errCode: 40, sErrorDesc: "开门超时时间，超出范围: 5-255（s）", lngcode: "r68")]
        public int? ParameterDoorSensorDelay { get; set; }  // 是

        /// <summary>
        /// 门磁报警开关，0，关闭；1，开启
        /// </summary>
        [VerifyNumRange(iRange: new int[] { 0, 1 }, required: false,
                      errCode: 41, sErrorDesc: "门磁报警，超出范围:0，关闭；1，开启", lngcode: "r69")]
        public int? ParameterDoorAlarmSwitch { get; set; }  // 是
        #endregion

        #endregion

        #region 开门时段

        /// <summary>
        /// 开门时段
        /// </summary>
        public List<HTTPOpenDoorTimegroupV1>? Timegroups { get; set; }  // 是
        #endregion

    }
}
