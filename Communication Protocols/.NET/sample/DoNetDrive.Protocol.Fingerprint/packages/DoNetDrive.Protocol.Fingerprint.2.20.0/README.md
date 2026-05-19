# DoNetDrive.Protocol.Fingerprint

## 介绍

用于定义 符合 人脸机/指纹机 UDP/TCP协议文档的设备命令，含命令协议文档中的所有章节


## 软件架构
基于 netstandard2.0 ；



## 使用说明

~~~ c#
var mAllocator = ConnectorAllocator.GetAllocator();



var cmdDtl = CommandDetailFactory.CreateDetail(CommandDetailFactory.ConnectType.UDPClient, "192.168.1.56", 8101,
                CommandDetailFactory.ControllerType.Door88, "0000000000000000", "FFFFFFFF");
ReadSN cmd = new ReadSN(cmdDtl);
try
{
    await mAllocator.AddCommandAsync(cmd);
    var snResult = cmd.getResult() as SN_Result;
    Console.WriteLine(snResult.SNBuf);

}
catch (Exception ex)
{
    Console.WriteLine(ex.Message);
}

//释放
mAllocator.Dispose();
~~~



## 版本记录



### Ver 2.02
增加命令：人脸机消防开关，云筑网开关及重新拉取，点名机相关命令，人脸活体识别阈值

### Ver 2.03
新增加 ReadTransactionDatabaseByAll 命令，可读取历史记录。



### ver 2.05.0

修改命令ReadTransactionAndImageDatabase，修改此命令的参数 ImageDownloadCheckCallblack，回调时增加当前记录详情，另外修改读取记录照片的检测逻辑，增加检测命令详情中的photo字段。此字段为0则不读取照片。


### ver 2.06.0

修改命令 ReadTransactionAndImageDatabase，减少写索引的次数，防止读索引由于某些原因被覆盖为旧索引。



### ver 2.07.0

增加命令 人脸机4G模块状态设置 WriteFaceDevice4GModuleStatus/ReadFaceDevice4GModuleStatus
用于配置人脸机是否启用4G模块。


### ver 2.08.0

修复命令 WriteFaceDevice4GModuleStatus 执行会报错的bug



### ver 2.09.0

修复命令参数 WriteLocalIdentity_Parameter 使用了错误的字符集导致报错的bug

### ver 2.10.0

修复WriteOEM命令将设备生产日期错误设置为12H制的问题

### ver 2.11.0

> 更新依赖关系  
- DoNetDrive.Protocol.Door Version="2.7.0"
- DoNetDrive.Core Version="2.9.0"

### ver 2.12.0

> 为解决<6.0的人脸机老版本固件升级丢包重发后会导致固件升级失败问题，
UpdateSoftware_Parameter 增加了 SkipTimeoutPacket 可以设置命令不重发数据包。

### ver 2.13.0

> 增加人脸机第三方平台对接参数配置命令 ReadThirdpartyAPI 和 WriteThirdpartyAPI。


### ver 2.14.0

> 修复人脸机第三方平台对接参数的解析函数对于冒号的错误处理导致解析不了url的bug。

### ver 2.15.0
> WriteThirdpartyAPI_Parameter 增加注释
> ReadTransactionDatabaseDetail 修正注释
> WriteAuthenticationMode_Parameter 修正注释

### ver 2.16.0
> 增加掌静脉特征码相关操作，影响的类：
> WriteFeatureCode_Parameter、ReadPersonDetail_Result、ReadFeatureCode_Parameter、DeleteFeatureCode_Parameter
> ReadPersonDatabaseDetail_Result、Person、ReadVersion_Result
> 增加访客根密码设置 ReadGuestPassword、WriteGuestPassword


### ver 2.17.0
> 修改特征码读取时参数检验规则 ReadFeatureCode_Parameter
> 修改部分接口掌静脉的命名

### ver 2.18.0-2.20.0
> 特征码操作相关类的 usercode 类型由 int改为uint


