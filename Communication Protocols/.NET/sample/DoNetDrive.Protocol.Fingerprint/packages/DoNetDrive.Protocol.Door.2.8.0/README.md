# DoNetDrive.Protocol.Door

## 介绍

用于定义门禁控制器 FC89\FC89H\MC58 型号设备的命令，包含命令协议文档中的所有章节


## 软件架构
基于 netstandard2.0 ；



## 使用说明

~~~ c#
var mAllocator = ConnectorAllocator.GetAllocator();



var cmdDtl = CommandDetailFactory.CreateDetail(CommandDetailFactory.ConnectType.TCPClient, "192.168.1.56", 8000,
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




### ver 2.02.0

更新引用的 DoNetDrive.Core 版本到 2.04

### ver 2.03.0

更新引用的 DoNetDrive.Protocol版本到 2.02 解决由于版本引用问题导致的错误


###  ver 2.04.0

增加适用于 FC89H、MC-59T的门禁控制板的在线升级函数。

###  ver 2.05.0
修改节假日API，修复命令错误的bug，适用于 门禁控制器

###  ver 2.06.0
修改设备SN检查函数，增加支持SN中带有小写字母


###  ver 2.07.0
更新 DoNetDrive.Core 的依赖版本到 v2.09

###  ver 2.08.0
更新 DoNetDrive.Protocol 的依赖版本到 v2.04
ReadSN 命令增加参数 UDPBroadcast 用于指示在UDP广播时让设备使用广播回复
CacheContent 增加参数字段 ContentBytes ，以便将任意二进制数据写入到设备
