using FaceWebServer.Utility.Extend;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Buffers;
using FaceWebServer.DTO.MQTT_Protocol.Command.Device;
using System.Security.Cryptography;
using FaceWebServer.DB.Table;
using static System.Runtime.InteropServices.JavaScript.JSType;
using FaceWebServer.Utility;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using FaceWebServer.DTO.Config;
using FaceWebServer.DTO.MQTT;

namespace FaceWebServer.DTO.MQTT_Protocol
{
    /// <summary>
    /// MQTT 数据包解压返回值
    /// </summary>
    public class MQTTCommandPacketDecompressedResult
    {
        /// <summary>
        /// Json字符串
        /// </summary>
        public string JsonString { get; set; }
        /// <summary>
        /// gzip压缩包长度
        /// </summary>
        public int GzipLength { get; set; }

        /// <summary>
        /// Json 字符串长度
        /// </summary>
        public int JsonLength { get; set; }

        /// <summary>
        /// 附加文件大小
        /// </summary>
        public int FileDataSize { get; set; }
        /// <summary>
        /// 附加文件偏移量
        /// </summary>
        public int FileDataOffset { get; set; }
    }

    /// <summary>
    /// MQTT 数据包解析返回值
    /// </summary>
    public class MQTTCommandPacketParseResult
    {
        /// <summary>
        /// 是否使用GZIP压缩包Json包
        /// </summary>
        public bool UseGZIP { get; set; }

        /// <summary>
        /// gzip压缩包长度
        /// </summary>
        public int GzipLength { get; set; }

        /// <summary>
        /// Json 字符串长度
        /// </summary>
        public int JsonLength { get; set; }

        /// <summary>
        /// 附加文件大小
        /// </summary>
        public int FileDataSize { get; set; }

        /// <summary>
        /// 命令包的缓冲区大小
        /// </summary>
        public int PacketBufferSize { get; set; }
        /// <summary>
        /// 解析后的Packet
        /// </summary>
        public MQTTCommandPacket Packet { get; set; }
    }

    public static class MQTTCommandPacketExtend
    {
        private static readonly JsonSerializerSettings _jsonSettings;



        static MQTTCommandPacketExtend()
        {
            // 配置全局序列化参数
            _jsonSettings = new JsonSerializerSettings
            {
                ContractResolver = new DefaultContractResolver(), // 不改变字母大小写，以字段定义的格式为准
                NullValueHandling = NullValueHandling.Ignore, // 空字段忽略
                Formatting = Formatting.Indented, // 格式化 JSON 输出
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore, // 忽略循环引用
                DefaultValueHandling = DefaultValueHandling.Include // 包含默认值
            };

            // 添加日期时间格式转换器
            _jsonSettings.Converters.Add(new IsoDateTimeConverter { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
        }

        /// <summary>
        /// json 压缩块的最大长度
        /// </summary>
        public static int MaxJsonLength = 3072_000;//3000kb

        /// <summary>
        /// 将MQTT接收到的数据包解析为对应的数据包结构
        /// </summary>
        /// <param name="dataBuf"></param>
        /// <returns></returns>
        public static async Task<MQTTCommandPacketParseResult> Parse(ArraySegment<byte> dataBuf)
        {

            //进行gzip解压缩
            var decResult = await DecompressedGzipJson(dataBuf);
            if (decResult == null)
            {
                return null;
            }
            if (decResult.JsonLength == 0)
            {
                return null;
            }
            ArraySegment<byte> fileDataBuf = null;
            JObject jsonObj = null;
            string cmd = null;

            jsonObj = JObject.Parse(decResult.JsonString);
            var cmdlinq = jsonObj.Properties().Where(p => p.Name.ToLower() == "cmd");
            if (!cmdlinq.Any())
            {
                return null;//没有找到命令字段
            }
            cmd = cmdlinq.First().Value.Value<string>();




            if (decResult.FileDataSize > 0)
            {
                fileDataBuf = dataBuf.Slice(decResult.FileDataOffset, decResult.FileDataSize);
                //Console.WriteLine("文件大小：" + iFileSize);

            }



            try
            {
                //根据命令创建对应的数据包结构
                var packet = MQTTCommandFactory.CreateCommand(cmd, jsonObj, fileDataBuf); 
               return new MQTTCommandPacketParseResult()
                {
                    Packet = packet,
                    GzipLength = decResult.GzipLength,
                    JsonLength = decResult.JsonLength,
                    FileDataSize = decResult.FileDataSize,
                    PacketBufferSize = dataBuf.Count,
                    UseGZIP = decResult.GzipLength > 0,
                };

            }
            catch (Exception ex)
            {
                Console.WriteLine($"MQTT解包，创建命令时发生错误：{ex} \n {decResult.JsonString}");
                return null;
            }
        }

        //将Gzip压缩的数据包解压缩
        public static async Task<MQTTCommandPacketDecompressedResult> DecompressedGzipJson(ArraySegment<byte> dataBuf)
        {
            MQTTCommandPacketDecompressedResult result = new MQTTCommandPacketDecompressedResult();
            if (dataBuf[0] == 123) //123=={
            {
                //如果直接使用字符串传输
                result.JsonLength = dataBuf.Count;
                result.JsonString = Encoding.UTF8.GetString(dataBuf.Array, dataBuf.Offset, dataBuf.Count);

            }
            else
            {
                //使用 gzip 压缩传输
                if (dataBuf[0] != 00)
                {
                    return result;
                }

                int ioffset = 0;
                int iGzipLength = (int)dataBuf.ReadInt32(ref ioffset);
                int iJsonLength = (int)dataBuf.ReadInt32(ref ioffset);
                if (iJsonLength > MaxJsonLength)
                {
                    return result;
                }
                result.GzipLength = iGzipLength;
                result.JsonLength = iJsonLength;


                //进行gzip解压缩
                //需要进行解压缩
                // 从池中租用一个字节数组
                byte[] decompressedBuffer = ArrayPool<byte>.Shared.Rent(iJsonLength);
                string sJson = string.Empty;


                try
                {
                    using MemoryStream decompressedStream = new MemoryStream(decompressedBuffer);
                    using MemoryStream sourceStream = new MemoryStream(dataBuf.Array, dataBuf.Offset + ioffset, iGzipLength, false);

                    using (var gzipStream = new GZipStream(sourceStream, CompressionMode.Decompress))
                    {
                        await gzipStream.CopyToAsync(decompressedStream);
                    }
                    // 重置解压缩流的位置
                    int lJsonSize = (int)decompressedStream.Position;
                    if (lJsonSize != iJsonLength)
                    {
                        return new MQTTCommandPacketDecompressedResult(); //解压缩后的数据长度不对
                    }
                    decompressedStream.Position = 0;

                    sJson = Encoding.UTF8.GetString(decompressedBuffer, 0, lJsonSize);

                }
                finally
                {
                    // 归还数组到池中
                    ArrayPool<byte>.Shared.Return(decompressedBuffer);
                }

                //返回解压后的Json字符串
                result.JsonString = sJson;


                ioffset += iGzipLength;

                if (ioffset < dataBuf.Count)
                {
                    result.FileDataSize = dataBuf.Count - ioffset;
                    result.FileDataOffset = ioffset;
                }
            }



            return result;


        }


        /// <summary>
        /// 将命令数据包转换为二进制数据包,以便发送到MQTT服务器
        /// </summary>
        /// <param name="packet"></param>
        /// <returns></returns>
        public static async Task<ArraySegment<byte>> ToBuffer(MQTTCommandPacket packet, bool useGzip = true)
        {

            int iJsonLength = 0; //json数据长度
            string sJson = Newtonsoft.Json.JsonConvert.SerializeObject(packet, _jsonSettings);
            //将字符串使用UTF8编码
            iJsonLength = Encoding.UTF8.GetByteCount(sJson);

            if (useGzip)
            {

                int iGzipLength = 0; //json数据压缩后长度
                int iPacketLength = 0; //数据包总长度


                // 从池中租用一个字节数组
                byte[] jsonBuf = ArrayPool<byte>.Shared.Rent(iJsonLength);
                iJsonLength = Encoding.UTF8.GetBytes(sJson, jsonBuf);

                iPacketLength = iJsonLength + 8 + 128;//8字节表示2个长度指示   128 是压缩预留的空间，字符串有时压缩后会变大一点

                //检测是否有附加数据包
                ArraySegment<byte> fileDataBuf = packet.GetDataBuf();
                if (fileDataBuf != null)
                {
                    iPacketLength += fileDataBuf.Count;
                }

                // 创建一个数据包缓冲器，用于保存压缩后的数据
                byte[] packetBuf = new byte[iPacketLength];
                //在进行gzip压缩
                using MemoryStream compressedStream = new MemoryStream(packetBuf);
                compressedStream.Position = 8;//跳过8字节的json数据长度

                using MemoryStream sourceStream = new MemoryStream(jsonBuf, 0, iJsonLength, false);
                using (var gzipStream = new GZipStream(compressedStream, CompressionMode.Compress, true))
                {
                    await sourceStream.CopyToAsync(gzipStream);
                }

                // 归还数组到池中
                ArrayPool<byte>.Shared.Return(jsonBuf);//jsonbuf已用完
                jsonBuf = null;

                //获取压缩后的数据长度
                iGzipLength = (int)compressedStream.Position - 8;


                if (fileDataBuf != null)
                {
                    //将附加数据写入到流中
                    compressedStream.Write(fileDataBuf.Array, fileDataBuf.Offset, fileDataBuf.Count);
                }

                var outBuf = new ArraySegment<byte>(packetBuf, 0, (int)compressedStream.Position);

                int ioffset = 0;
                outBuf.WriteInt32((uint)iGzipLength, ref ioffset);//写入压缩后json数据长度
                outBuf.WriteInt32((uint)iJsonLength, ref ioffset);//写入压缩前json数据长度
                return outBuf;
            }
            else
            {
                byte[] jsonBuf = new byte[iJsonLength + 1];
                iJsonLength = Encoding.UTF8.GetBytes(sJson, jsonBuf);
                var outBuf = new ArraySegment<byte>(jsonBuf);

                return outBuf;
            }

        }


        public static void TestPacket()
        {
            MQTT_UploadPeople p = new MQTT_UploadPeople();
            p.PushType = 1;
            p.UserID = 1;
            p.Detail = new HTTPv2_Protocol.HTTPPeopleV2()
            {
                UserID = 1,
                Name = "测试人员",
                Job = "开发工程师",
                Department = "研发部",
                IdentityCard = "1234567890",
                Attachment = "测试人员",
                Photo = "1.jpg",
                PhotoMD5 = "1234567890",
                PhotoLen = 1024,
                Password = "1234567890",
                CardNum = 123456789123456789,
                QRCode = "卡号 （数字，最大值 18446744073709551615  类型 UINT62）卡号 （数字，最大值 18446744073709551615  类型 UINT62）卡号 （数字，最大值 18446744073709551615  类型 UINT62）",
                /*
                Fingerprints = new List<HTTPv2_Protocol.HTTPPeopleFeatureCode>([
                    new HTTPv2_Protocol.HTTPPeopleFeatureCode() {
                        Num = 1,
                        Data = "gH8JBPk1HlReXoRskq7gAwEAAggFDAYSDg8ZAwsJDRwHEBMVgdUBj/CzXLO5TxcbGBwXIRcdLCoM2f6E4jd6TvqtBAgaAgwJAAYFEQIHCgEFDBoZDQ8DBAEZEhQWAAMPEQILExgbGhkIBAsCCg4DEQAFExcJFh0ECAERBgMKFQcNDRMFEQMLBlAwPmYlZBB1Fz44cjO9RfnpCZZhhImolIqnhm+NcXPAt7h9zEiYUqlRuWJJyJ+srr5bAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAIBedJZVkKShSEuqdVGft5kxrEy9Q78dI6pXWS4xFQAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAACIAHCAgGiosICIsOn42MDhEeERYZC5EJji+nqC+p0AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA//+rr8D/////g4oK4P///x+gIAD4////AAACAPj//z8ABAAA////AwAAAPD//z8AAAAA////AwAAAPD//z8AAAAA/P//AwAAAMD//z8AAAAA/P//AwAAAMD//z8AAAAA/P//AgAAAMD//y8AAAAA/P//AgAAAID//z8AAAAA8P//DwAAAAD///8DAAAA8P//DwAAAFT///8AAABA9f//HwAAAFD///8CAAAA8P//PwAAAKD///8LAAAA+P///686APD//////w//////////////////////////////////rsQYGAABAPGq1OLmuTTtl9sAAAr8+fNB6s7sPAP6Ez4qF9YhcBFpAAAAAAAHAADgHwAA4P9/d+D8f3cA4H93AAAAAAAAAAAOAAAADgAAAA4AADj/H/48/x/+PP8f/hwfAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA=",
                        MD5 = "asdfasdfasdfasdfadf"
                    },
                    new HTTPv2_Protocol.HTTPPeopleFeatureCode() {
                        Num = 2,
                        Data = "gH8JBPk1HlReXoRskq7gAwEAAggFDAYSDg8ZAwsJDRwHEBMVgdUBj/CzXLO5TxcbGBwXIRcdLCoM2f6E4jd6TvqtBAgaAgwJAAYFEQIHCgEFDBoZDQ8DBAEZEhQWAAMPEQILExgbGhkIBAsCCg4DEQAFExcJFh0ECAERBgMKFQcNDRMFEQMLBlAwPmYlZBB1Fz44cjO9RfnpCZZhhImolIqnhm+NcXPAt7h9zEiYUqlRuWJJyJ+srr5bAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAIBedJZVkKShSEuqdVGft5kxrEy9Q78dI6pXWS4xFQAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAACIAHCAgGiosICIsOn42MDhEeERYZC5EJji+nqC+p0AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA//+rr8D/////g4oK4P///x+gIAD4////AAACAPj//z8ABAAA////AwAAAPD//z8AAAAA////AwAAAPD//z8AAAAA/P//AwAAAMD//z8AAAAA/P//AwAAAMD//z8AAAAA/P//AgAAAMD//y8AAAAA/P//AgAAAID//z8AAAAA8P//DwAAAAD///8DAAAA8P//DwAAAFT///8AAABA9f//HwAAAFD///8CAAAA8P//PwAAAKD///8LAAAA+P///686APD//////w//////////////////////////////////rsQYGAABAPGq1OLmuTTtl9sAAAr8+fNB6s7sPAP6Ez4qF9YhcBFpAAAAAAAHAADgHwAA4P9/d+D8f3cA4H93AAAAAAAAAAAOAAAADgAAAA4AADj/H/48/x/+PP8f/hwfAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA=",
                        MD5 = "asdfasdfasdfasdfadf"
                    },
                    new HTTPv2_Protocol.HTTPPeopleFeatureCode() {
                        Num = 3,
                        Data = "gH8JBPk1HlReXoRskq7gAwEAAggFDAYSDg8ZAwsJDRwHEBMVgdUBj/CzXLO5TxcbGBwXIRcdLCoM2f6E4jd6TvqtBAgaAgwJAAYFEQIHCgEFDBoZDQ8DBAEZEhQWAAMPEQILExgbGhkIBAsCCg4DEQAFExcJFh0ECAERBgMKFQcNDRMFEQMLBlAwPmYlZBB1Fz44cjO9RfnpCZZhhImolIqnhm+NcXPAt7h9zEiYUqlRuWJJyJ+srr5bAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAIBedJZVkKShSEuqdVGft5kxrEy9Q78dI6pXWS4xFQAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAACIAHCAgGiosICIsOn42MDhEeERYZC5EJji+nqC+p0AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA//+rr8D/////g4oK4P///x+gIAD4////AAACAPj//z8ABAAA////AwAAAPD//z8AAAAA////AwAAAPD//z8AAAAA/P//AwAAAMD//z8AAAAA/P//AwAAAMD//z8AAAAA/P//AgAAAMD//y8AAAAA/P//AgAAAID//z8AAAAA8P//DwAAAAD///8DAAAA8P//DwAAAFT///8AAABA9f//HwAAAFD///8CAAAA8P//PwAAAKD///8LAAAA+P///686APD//////w//////////////////////////////////rsQYGAABAPGq1OLmuTTtl9sAAAr8+fNB6s7sPAP6Ez4qF9YhcBFpAAAAAAAHAADgHwAA4P9/d+D8f3cA4H93AAAAAAAAAAAOAAAADgAAAA4AADj/H/48/x/+PP8f/hwfAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA=",
                        MD5 = "asdfasdfasdfasdfadf"
                    },
                    ]),
                Palmveins = new List<HTTPv2_Protocol.HTTPPeopleFeatureCode>([
                    new HTTPv2_Protocol.HTTPPeopleFeatureCode() {
                        Num = 1,
                        Data = "zAAAADEBAACpNsQ5jKbIMYhmyDmJNsQ5mTPG6UG5huVVuc7kZTHIvFGqTjFpO0E86TFRtEkxVqzsvMU5jK7BMeymmXXks8hxSbHOaWGxnOxFsYjkaTGINJVmzjHJp8U16DfBtOkxRKTMPsExzKpBYWyrGGXku4h1WbnOZFG5jmxZMY5kYTGIdJF2zjHIZskx6GZZNeg5WbSJMU5kQbleZGG5WmSVrs4xmK7MMbimzDlpsZ50wbWKdME1zqiJdkO5rHZLOegxWjSRvkbhYbpM6VG+TGmZZkxhmSbMMZk3xDlJt8Y8RTGKJOExSqiFdke4hDZcuel2XLiBudbsUTtW7Bk5RuyRPkZpkW7EYZkjxnlVtsg4YTGabEk1GqyFNUqsGTVGrFE5RqTlMZWsaTmVbIExlmzlMZHkzOOBYUUzy2lhu8os4TvqZEW5E2zMuVvkWaZKpVGxXuTEYVW4yGsRrOExWmzhuRrkbKuR5FWmyWlVqsmlzOqRteyzEXzIsVpsGbFe5Fm5XuQRJl647KdZtOUxiazBM5fk4bmR5EUuwTxRbsk8zHaBNcwzkTRJsZpsmLFOZBnjzjVRsUaoiTVe7OkxxLTBOU7kYbkZ5ExqG7RRJsk8yKfLPMxnETRIORpkiDtOZZzmWrURt8a4iTdGqBkzxrwRMc5kQTGaZEwjGTxE5pk8WabJPMzmyTXMo4E1iDXKZYxmGDVBscNsiTVGbJE3jmyBN850WTFabGmxGWxErlk8EbbKPMm2yzTM5pk15DWZNIRmiThKnFiQd6GtmH9mUl2IbqhjjKD9pPIPK3lVaRR1dPpvFsqY5d2DsiH6si8UcmajX9ni/EjmofyPjlQqRi4sBh/GFrvEIMhMDeHawnxboVCWsyYY90AOz2ilfHghxJ/1pcI9jjfCQzoBKQjy+j0G4vzZtNCGVS6sgk6MeO1g/1Nxhx4sKw6C8EXLXMXTo1miB95o+9TPA8iOXvY+bYr4wsR/NjwGdJLCCHtDyxzCtC8BeO8n2pVrbaLTGdCVL1IJHLWaRq5s2kVYyJhvvr+NbGgfS5c3dj5f2Uzh/tjzfQgrpJ5Yvpm8jPwNDCI/NLdAIR5ENeS2fmNVPysuKvX386xtMxj+FPV0mrShKmMqpg5Tf07CwsbVfwsrcoS8cEpcXH32LkR0EQNiQFTNAefUi0SUCGiht3B6f4WQRlyX9mvyASkZX4eO1ya55Da1kes568MeOpzRNwHy4/rQlI4Xgj4/e4gDbCsx/efKmcukXdYtlKUPz0/xw7agw2PWsVutf7VpWpVPvzN79jQuNlQ+Ll6CU59QR/IdLPIdMPEt7j/KTPes2NThyLHCjIPeyBQBFjMm73lTUSg/ChPlqH4rtG3aNZrcj/JW7BG4qKHr9X8JE11WRe0R/OU0oFQEn1gtPbrarejq2L5LE8OjVicw9AwvVmoiKwMr91soD9vJ6ZbbjMznh9w7Oe7N/PSO195NSROcrbE4bYF/g/vVArSqjHUTf1sNdctNQXUw+wHdj1g97N3RKZGED9zDFrkZveGGsB0wFGmztOa0XpAaTox0aucdElTUPnPoRTwgVYTFFhQWswYuDQEgDjT6AvT9AAXa7/v/+CDfGOwLDAMRAAoa//z3AgYND/n1BAAH9fAE7Aby/vz5CwX6DvgJ8wcJ9AP94/r4Aff8CgMFAAv2+f0N9Pj3AgYe3foL/xDm/QsQ3yMECv0Y//n08BAZAv8D+/Hz9gYI9Qn6/Q//5Az15Q3qBe31/fYPAgnt+fsW8vMLAA/gC9v8/Rb27/TsBxL1/AAEDvoM/gsH5Qv+6iTw9QUdCfT9+uUH/fEQ9vD5D/UNFOH3Af0EEPQIJPYFEPYAIgEGAP72+P8HGv0H9Qr8Ae7oAh0I9QUSIPsGGfcN8QoCB+EE7vb0DOkb2x32Cv/1DgT9C/UPCwUE7/X36wwC5Pr04RUAMQEAAME8yrWJNM45iTZOOZk2RjGZO0bpQTnO5MU5zqRlsUm8kb5GaGk7QbxJOROsQTFbrIkmxTGMdsg5jGZIOYh2TDmZM8bpSTHE5EGxhuRpsYjkEX7GqUFuxbhlOUG8wTFbrMyuwTHMrsU56KqZdMyjiHVJscZsQbGG5GExjORpOYhkEWbOOcFmhbnFNsG87DFRtMyuQTFMulExbLsYZEWriHRlqch0UbnGbEExjmRJMZpkUTWObMF3yrnMZ0mx7DlZtEmxzmRBuV5kQTlebFWuzDGZrswxkTbEOek3xTzpsRp0YTFarOk1QqjEZlk47DFZtJm2zDFpu0zlaTlMacE2TGmYbswxiTdEOcinRTjpOZ5sSTlarMk1RrgVtke4xbdOuZGxxmTJOcbsUbtG7IE5RuycblfpiaZXaVmqRzhhs0l8STHKbEmxR6xZtka8WTFOpGWxxK7oMZWsYbGUbMGzFuzMoxHsTOvRaW2Zw2roo8E16blTdEixW2xZO0q8Wbte5GWxVKzMc1W8ZLMZbKEzHuRpuRHkTOZR5VWmyeHMrhE1zKsRNEixEWzJsV5kQbHOpFGqTLkVpkq8bKdZtIkxXuRJOZPkTKORpFk2wbTIfsE0yK5TNWixE2yBsUZswTvONXm5RjyRsU5o6LVZPOmRWjRJsRpsTLMZpExmkbxJJsk8yDbBNOwxkTSJM8ZkyXrGtRGmRzmRs0Y4mbNGPIGzTmTBsR7kbLEZ5EzmG7Rprsk8WT7DPMx6iTXoOUY1yGZFuYLbctgGqL2of1VBVvkmjSOQdk++FR4rZd8/7mNW0Uz7vQjp6XuFCeGF/M5P6IA9uP3rVNCD2ltIEOGmzdjdqpkzkmvzjSO7hIBVGerlmlN8Nu3IUNRdHj/09rtg8FbINmxn0FTTwY+3dXJdOXtbhJ0qkhCx/gz0v7/cib3Vocy0YGFtZVNr7wQpm+dFEwq0MmmSpFNfuvyWRHFcsQmcrH9RYvVMo468hKvNNeXDMvOUsvfUjkcyQBBc3bw+NvxmgnwqY1qiGiziUV6viGhLP/IGZfkMyDhnfs3hkvcrpG1aXFV+FJZES+Ya8wj5v9rGlpqhr7Wyp6pZG53Fja+DW04HWq95xYpNRz0XENuhRA2ahDwzKSDtmgshZ6M5GeOaBzjaYtqksrzhKGJ8isjChspNnyDCs4d/eEs631+0NAQNNTRolaXNLeX9M8+Tv9cDc2halcEY0I2lNHWNTWIidUGloPVJ1uObojg6mYT+CzaBqPGeCauKYDA4Ny28XmGXsxHAOOy6x4N/6ZuZzR3FYEvMSN/Fk5p4fTWFhXmdgn7xjChQc2shMjT0uD4JETiEEs4TezXq2u+Tu345FHMuz4vxExvZfIg8VYDD0/Xxfw0KKTt+q8M6S1HIYBCrUhQwULGwyL9oErQ4raFLEFJvaQ3v8pQMT6XjrSX+1buoYGoFkpInorBR7baP5WySi0pUPnQrOVh0cWK6baYudEhRKVHur0kZMhoLn6F5j4c2hycS5/Ag0zCTzaSNZ+LcUS9XlKKugSXMqySBsAoTC4QmW7yi85e2vMIJ2jowrwkdFxa2DS8IABoGLvUC6wMLA9ni9wD8J+AW7gkHBQsAFhny+fMO/QAM9PwP/A/+8gjzAO8E8PUQCOwL8gbzCQr1DAHrAfAO8wAQAQn+EwP2+wz79QYNCRja/A4GD+b9AADlHPoAAgvzAPDqCBcJBPr06fH0BwcACPIAGxTpEfTzCusF7vf48AcIFQDw/xr9/AL5Ee0E3fvyFPvy9PsBC/XuAwAJAwT9C/3s9vzmHPj0/hMM8v7z2Qf39RP75PMW9hAM2vH98wMM8wge+AsP7/EmAgj8Afj7BwMb/P77CvUI7fL8GgX8CBAh+QcU8ArvDQQM3gLn9fwE5w7fH+8NBAEQAwgO9A4KCgT38PjsDArvAPftEQAxAQAAiTTOOYw2zjmINsY5iDZGMYk6RmlhOYY0RbnOtGQxSLgZNk65eblOrEgxF6xJMRes7DbFMYzmgTHMZsg5jD5MOVk2znlhMYxsRbHO5GmxzDiRNsa5VTvEuWwzwbRpMVekzK7FMYyugXHsoZh07LGKfEmxjmxFMYpkQTuKZGk5yGSRds65SXbEuUw3zLXuMdm06bHHZOy4yWVsudh1RKvIdVm6zjFhOc52STGedEkxiuSRNM7pgXaLsUR2yLHsOVi0iblG5GGZSmZpOsxxjT7MMZk+zDEZts55aLXYPOUxmKRlMciopXZBueU0WDjsMRg0mbpG4VG7TulZvkxpjDrMMZk2zjGZp0W4aLPFvEUxmqxFMVisxTdOqIW2XrjlNli5wbmX7FG7luxZuUbsxDNR6YmnTnmJo0N4TKNROOWxGqxIMRqs5TFKpJQ2xqVZOcao6TmVrGkxlWzhsZbsxKOR5cXqwWhB58t4aavBOMm5V2RIuVNsbDFbpFg6S6VZuV7kxXtFvEg7kTyBsx7k6blR5EzqUaRVqsl4VeqBOMzukzRsuZF8SLGTZMizziVZu57lxb5EPMw3GTTJMV7kyTETZOyxEWxVpsE8UabBPMzuETTsO5E0SDGaZMizznVRO17lwbFOaOg1mDzpMUo0yTEabMa7EWxE7pE8Ue5JPMyuWTXMp5E0SLGedIgzjnQRZ8o1yTFGaMkxVmyhO040wbleZEw5GexM6pO0Ve7BPFyuwzRM5hk1SLEZNIl5RnSIbkkxnndFuHCGqLh/RVZ5kka7iTnGNJHt/yNswGE4jGztnQJTgKliHkrt2Z4K46Qj2Fu5i9nzbEWtUGMmBG0MCEcFUXZtRqZL9a+4maRTMTE8pYN4jTbIgVEPTCgvB4cj57tsk9SCA6/AlOGagrZ2nXGsdIEMrUWI+HpbkCJ/C1/WzRiHBInszohq534y3idFHwaFHB6X39k93FhtX8e2AZqkf1hk/0Y1oJJbKS4cvsYx2YKwYO6otQlb9QfDIxzy35eAhRx4NI8rQLNIWpJRRzg+4AJlARHgDCNnh4dSxRg6MFZSHmYV7yI1ThvqNQo4C8ij9kHVEShUpqK2XIpJqZ9YPkVatmCZyHWllObBjzqH7G+olWxfOsuHnWWUiReKtEntYMiqWKM602Q1epPUK7Q2aJNhzMf5WX+oBTYxn2yXlSzr2DJWeMAX5wQeCzeBhG5hru89ge3P38z85wECcJjKiwISnxbR3aq3mVWFsIWIeyJEiGUDlvxY+zs0N2jGd+YMok3s37LCdYFMSr/iIsjI2IEHmJR9qGiRW51XY5J3CoYG0emzerxmf+yVdTRpfj8p9+J6o15yhUYTMet72a1pSQm6SCwaTuE8sLN+7uF/HRCETEzEHdkPM38kzIpADD5isOrAfDTlEaJjRg/NVoCL7bAQrRVaK/rkw6CGpJRIZgV9iwdpkmyBVejAD4x2PF4OkB8PYjPbtE7KkrCeWDomLQWsgNP/i4VhlEfuTmYrVGIRInSuPRLz6R8RiHmFKB4Z6CmS6woHc7f1VIUNEJr9L6pxv7W1YFTzk0Y1WRIZFbIRJgf8Gwss/P3qAAcM1d4D//Ig4Q/vFxn8Dv0JGfcA8wv1AwXzAAIA//jl/fMA7Pz9/A4K9A/0CfQFD/oC/d4A7gfx+xYEDP8S+fn6Dff7CwANF+H9FQUL5fQDDu0i/f/7Dvb2+ucPDQkEAPnw7vUGC/YF7v8ZC+oT8t8N6QTv9gTsBgUI9vwGIPf7AP4M6Ajc+f4S9P309AAM9vUEAxD9Cv4Q/OgK+Ocb+fQEEAn7+/LkA/jvE/vp9hLxBQrf/AD6ABH4CBj0CxXw7CX7CgH3+fgA/R/9CP8J9Avr7AUSA/EMFRz9DhXzE/AU+QzjAuz7//rlE9sl9AgB/A0MBAv7FQ0FAfL3BekOA/IC9ekSADEBAACBNMqhiTTOOY02xjmINsQ5iT5GsUk5RuhBOcbkVbHHpGgzSLFJO0o46DFRvOgxW6QZNsSxbDbBOezmgTGIZ0g5gTpEOVkzxulRscbsgTHOpOkxzrWRfs65yWdBtOkzSbTMLsExjKbBuYymgTXsoZh15TGOaEmxjmhRsYZo6TGI5OF+yDWRZk6xSWZYocl2SrXsrkExbLxBsey4WWXku5h1xDuMceGqzDHhuc5uSTGeZME1imQRNFqhSWZaqelzSrUZtM6hGbVO4UmxXmVRuc5lgSLMMZmmzDGJpsw56LfIPeUxGHzlMVqshXZJuOU1yjiZZkyxmbZE6Vm6TGkZsk5p4TNMOYk2xDGJZsQxSDbEOWU5ynjpNUqshTfHvIU2xjgRMc6swTGW7EkxFuwZOcbs5TFV6IEyxSnBOk45ZbZIOcmxTnRItUt87bFDNJE6SjxlNcysqTGXrGk5lWyBM5bsxKOR7MRjkWnlEcp5YbmKdMEzxiRMuZNsTLFLZFi3SrXkd8y5hXfFuEVrkWzls+hl4bmaZOSrGeXFOsgxxTrINcxukbRssZFsyLGabImzznRhM065hWdKucTrWDTl61h14TEaZOwxGWTMI5k0zG7JNMx+wTVoO4E0SbGabIkzynQZsU6sEbFO6MmxXmzp5lg16TNKdOSxmXRE4xu0XO7ZPFzuwzXIbpE1aDGaZIhzxmWRp8S5kbfGuUkxxmyBOU7kwblOZEGxmmRF6pu0VO4RNBjuQTTIblk1yDYZNYk1XqRwLpcB/sbSo39bLDj+/mjS1EFp9DkuRYeAONs0GcUYtcTiRdJYmhb4n+CzWupdAX9Ya3ggjghtKBXhrLWdoo5wy8phIJ0814x6TQvpz7Gtr2K/UD4LcS5J9OvEQOJsHMyrli6N3YGMvm9QMCNjgmJewdUhzdHmraxpDarGeZ+pjllVDzw71+zmkXWUTh3/pvahtp90cJE6y6a5XwE83kZ/wR7E/aBGWJUebXIW91QGfLi7t1/HCfGvl0YvRXACwIZ2/EIRcrrah6zJBnuMZ1MBGnfkGoHgStI+vNGNcMOna2jhlj7C4iMJOyQDId8B6sXez8QptPppPBIjC+S5oGZNwrqSb0l8CgxWZFHy+VBA9JxycCs46jtgnNufOy7hq9MLl1GivJOht4KF2TDMKHtDfNEg4KuKf3RTHN8tg70tK3dhmqjc3xXo7x1/j6nhXqCDuLjpJtaPjVxyWkEobKDQwLYDbvTloZJIJus+3fS8y+IbokiwW1smEBVfreCDCdAX127mxMSGcH2KhOiUc4eu2JDBun+LWUTvg3BfW3kpQlo7QxOr9BRO71kBBT/YSkIkFFA3/+LAwIOXF+NXCALaebcIu9SmnqM0EOIID3/w9xnvy1xgDazBCYsx10YOEyeflRDFPHXycRDgbxNmcTT6I+VKtQqGNFhLB+7duGyX+o90yPtqwEgJqt6puJ5iVw2sZhUafqvdd3xSRIdzSC9n5ZNgZL5KLRQR05qGNykfIUKlC2X/X9HiwoO7nFdBgeHzgqtuv16nq71GIRPVrm5PWMmmN7sLGQv8h8WqFx4NuAssBAkUBjIBBev3Dv7Y4vLz+B7pB+wACvwJ9hMZ+ATpBvcEDgTwCAQE+/3//P0CA/X8CwHrDPkO8AwE/wgA8gDtA+4AC/sT9goB7vsRBP78EBAn5QUODg3l7wAD5xkL/wIb9Pjr/xwRB/f2Bu349gsCAg739RkL5gf17Q/xBez48vkGAQgB8wIq7PgDAgvtFdv6/Qz18vr9Axf78AD0DwEDAA//7wf75hnt+QITDfoA9eIL9e4X/ev2EPUCEuL4+vwCDfoQJPgFFf35HwAAA/r5+f3+G/kN8//w/u/wBBQI8w0bJQMJGfsH6gsEDNcB6/38/O8h2RzyCAL7CfwJCv0PDxAD9fv25QsE6wDp6BQAMQEAAOk0zjmMdsgxjGbMOYg+RDGZOsZpQTnG5EW5zqRlMci4GTZOuWgzRbzpMVesSTFWrMS2wTmEpsExjOeIdcQ6zHFJts5xZbGMZUW5iGRpeYh0kXbOuck2x7noM1G0yTFXvMS+wTnEPsE17DOIdeS7mHVZuc5kQTmOZFk5jmRhMYpkkXbK6YhmSDHIakE16DlVNMmxxmTBsUrk4bNIcYRqzDEZrswxaLLMMUmxjnTFMZpskTGe6Jxmw2HsO8k16DFapJmzTuFpuU7lGbNOYZgnzDGZvs4xmbbEOemzxj1lORh8ZHFYrJTnxbmkt8axaDNYpVGzTmRJsVbsGbNG7JkyTGmZNs5xiaNMOUm6TDlJOZ48zDGLrJU1x6UZNcalWTpGpWUxlexhOZVsgbEWbOQxEWzEo1Fo5ZFKeeWzyDXpM+o0bDGZrFkxSqRZMU7kWble5MxzVbzMe5F8oTMWbOk5EeTEa1nlRaPKocWqibXMqoM0TLmbbEgxHmRJs07kSTle5MxuSjXMOVk07TlbZMkxk2TkM5HkxTbJPEx+yDXMPkk0bLkZfMixHmTJu15l6TmOpYG1TjnoNVq0qDdKNckxk2zEu5E0TGoRPFzuWTzIrlk06DqRNMG5nnSBesp1xXbKsYG3xjmJNc7sgTle5MG5XmRkuRlsTO4TtHzuWbwRrlo0yCoZNeE5lqSBdk6xhGaJsYkzxniBN86ogbbOZYGxTmRBMZpsxKORPOXm0TyZukY8yC5ZNcg2W7WlN065hHdZueSoP8aJlKOTf2xca3U4wFVNGDt5AQEif4NnHoV+9G8aRH24vA+UFAW8F/pmTsR9AnT4GcJ47nqHSSD5MQ9RJPjcf4T9oUIE7c6weVtGunke9My0N++0YoloRh+reAGYr+FM+IX+FtAc3MXTlOak5foouDjQx+F147dbyEqLMJ+gMytGGTouNcAmFujDrOkHJp4uHMHghHNStjFQc/G1tX9FSQ2MsM5IU4gvCZuvKwJ9sT7FpIB5q6n4mjcBLvnsnJZNsnPnSICoilCYpIFeWgw9vzBZKXP9rL7KmexH/x6Qg3HCdnGWgiLd+hs5gTMG7p8kFKnzZyz3Ae7119nTm3k2VxzMw6C6XtkcTtuHckcdmL2quJ+QdYyaoW19nZAzwwZ8JtNH6vMtYAisxqY08n7XQMO+Omd/mMdAOMAngKd88Pg6bnW/JMsIW+cf1I+hlOA2b7Ej+evZ+vTQv5Tj69IjXNhYECft08KRu/KTkeBRY+2sSNhqrp+QgSi7VghYfzW/bjnhEfzTLSYfCV0LGkQSfMMVx/Ol4J644YSKxq8fomCmaqj2bbp0XAPdWQY6DRaOb0kh/TAWK0MFxhT+jfsPc51f0TWk2pyOe+vufxEThXFhJVrzJQF7LviMSTNMyufdvq75hgHbm4E4/i3RByjqZNsjH+06F/vltejKcYNt0NKo+mn8r6nR7zam04OD8Eo8p0GpWmRB8LYGqolhXTKGJrKXAQPtJwi2v8Nia09mOaXsZUzXwXjNYeiefHsiFD3OgKKguj3xpO01rTaCFkLr7izD8q5i3uYtqz0aDhC7DSsM+hQPNPj57QAFAtLx+Ab7HuIc7wYT/g8IChsA//D/Ag8N+vsMAQP86Azy/vP8+v0SB/QP+fr4BA75AP3mAPQH8/4LARP5HPr5+w799/8IECTa+xIKEvD1BQrhJfkJABH5/fTyDxUMAQH58/XzDAL0DvoBFA7jBefkF/AA8/z89BQKBvv1ABf58AoBE94E2fj7EPT29fQAEPf4APwSAAH+DgHnDPzrH/XvBw8U+QH45A/67BD/6voO9xcc6PgAAAEO7g8e+g4L8vgh/wMD/fL1/AEU+A/6BfYG8/AEGgj3DhMg+Qsa/RDpCAME2P/lAPYA4hTeIfgJAgALCgEJ/BINCfz39vrtCgXqAO7vCQAxAQAAwTXKscg2zjmMNsY5iDZGMYm6RjFJOc5kRTnGpGW5SbxZPk44xbpHOMk5V+zpOVOs6TbOMcg2zDnsNsk5iHZMOZk6xjFZOcTlQbnO5EGxyqRROc64lTZGOek3QbzpMUakzK7BMYy2wTnMp4F05KOIfEG7zmlhuYZsQTmO5Gk5iGQRds45lXfOuUk2x7noMcW0zK5BMYS+wTnlo1h05bGYdGG5znxROc5mSTmOZEkxjmRRMY5slXfKqcxnybHsO1m1ibXO5RmxTuyJMU5kqbPOcRguzDGZPsQxWTbEPOixHDxlMViopXdBuIQ3y7nkMZo0mTdO4Rm7RulZOkxpyDJMOYg+xDGJPsQxyKdEOOU5Vj7FMVqo6TFDrIU2TriFNk65wbEW7GmxFOxZuVbs6TtG4emzVKGJpkZxSadMMc2zSDFpMUqs6bFDpJg2zrWJMU6lwbVWrMkxlmxJMZbs5bGW5MSjkejF6sF5QbnKfOGxyzXpOVPk5DlbZFg6yrVJMU7lxHdBuMhzlbxks5lsgbFe5Oi5EeTFqlFhRarIMeWqkTTEq5G0RbmZbFmxjmRBs45lxXZKucWjQTRMoxg1wbleZWmxGeTEulk0yb5BNcmuwTVFrlM0SbmabIGxynSR6841wbFOOOGxTjxopRg0ybHOdUmxmmzEsxk8TKoRNEzqETVM7hs0aDqZNIExinSRZso1kbzGMYGxTijJsU6kgbnOZUmxGmxEuxm8XO5RtFzuQTRIrkM1yG4ZNck5zjSJZsgxxMpp5cqluqp/U0RZMxmWNHhBT7sKGjl5lkf3cTjEV9F6A7Wp/XPv4JT1mmkWoEAMLhYzqmXUT/0H37283M3OeHsjBeNYBbN4ZT8T7e7nrJIQyjEUyzoEMtzpuYXyf5pOqUiaM6SCbexgP0MgfNSHGuZF8IhTHReSnviDwQ3Dq69SMv8lEunE09OmsqjwNvRqaptLJUP5C6JriwbBEsGTf2I87Tr5c5UKW3E85dk9BHimle2C2A8dNoDKJiIu4pF5fyFyHW4aIVx/fNx3XUg55xZe/wfc+w2iBNqE8CyTWkxX/FsBr8sKyAkIAwia1MNcgiYwIfVnf9rOr5yIipFaP3ODkIdamV/F2PMipPVR8YKGYDIjNipaL81dz1FJXXUPUMRs66Xou3dbYb5gcPlc5d6K88+nf3+AVy8LdBwZ1V0jM2V+sMEdyfkV0NmTGwbJ0Xd8tfrKhJIxh4kBUzfEr01AvzvMxYp7TjQ2BjjIXFCi0XUKnRxEDAz/HA8q535ayKv8tqGiaVY+VpQf33mnSJQ/o45aiV1bDzK/rjShYNlZ6dbBjq1babRFG5pW/2EG7h2r9xvFGUqr4u4j3If3tAYYyrz5e0dkzNO69v9/AQlDKjKbuYmoSMBY/7NHDw2jqsah949BaAejlD7oZ3Lh9+Lvbwkq6Y8l+967vpNNYflwhHZQeoTIt0bdYIx7Skb/fhMWSApgBIVhqOhgWsIbV+CbhrnbfNGMaGlnffkuNaV5szwjIl9vg1wglYZHHT7sVFZtii6FfNfGaW89YQfalX4BngW85UzhyyOs/yEbCbcOIAn/EhEx+ffz/A753OL79wEj6An3AhD7DvgQE/QB+AP6AAkA/QkACgD2EAD+9QTq+xEQ6g32APIIDfkCAun19Abw/BEADv4HBez8Bvv2AAkRIt0IDgoR5u0ABOkg/Pn7GPYA9vMWEwgB9P3y++8IBAgL/PsPCeEH8vAa5wHz9vXxCgAE+fYCFPr7/wAW6wvT8QAO9e33APsM9+8C+RMBBfsR/OYK/Okb9fUGGw/3+vPaCf7tGwDf9Rr1BRXb9fcAAAn1Cib3CBX39SD5Cv79Avz8BBr+DfgM7wr09QAgB/YNFSH/ByL5DPIL+gXdBO4A8P3wENwh8gkA9QsDAAz5FgoLBu/y9OwLDPr/9OoRADEBAACMpkExjKZZOYg2RTnJO0ZpybHWYcGxhuXpMcFkWLZKMWimWTXosVu0aLFavMkxXqSc7lsxzKZZccyrUXnIu1ZoybFXbMk5luxpeZSsiTZKMcimQzHIpkG16DlRtMy5VjRcKlixzDNZ4cyzWWVpsU5lWbnGbMkxlmzhNZd8gaZHOchmSzHofkk16DlRteSxVjRsq0WhZKpJ6Yw2QeGYp8yhwafEOMmxlnzltRs85TZbvIl2SziJMUq06LFTpImxV6QZukbpGbNO6Zk2TGmZN8ThiTfFOcQ6lTjlORk47HlZPIU2QzyFNUa8wblHpOk5V6TJMV5sQTFebIEzTmyBpsNphabHacWqWTnpOcp8aDWLPIU+w7xVOEauSbhepeGxUaTJc1ms4TkYbOQzEWTEY5FlxbPDYcWqSbVJumqlXDlabMU5WnxZuEN0Wbhb5OU4GaTFNVi8wTEWbOS5EWTE45G0xabBPMXugTRcPsOlxDGRZIUxmmTJM060abhbNERumbzBMU6sgTGOZMkxG2TMMxG0zG6RNEVuWTxV7lGk5TmR5ME5mmyBds6l4ebLsURumbiBM0a4gTNGbMG7HmTluRukxO4bPETu2TxB7ss8xGqRNckxluSJZk6x5OZZscRuGTiJs0ZoibNGZIEx1mTBsZtsROaZPGTumTyZ7sM8zGabNcimS7WIp0axzOeVtcxokTjJsVZsybGWZMGxHmRBsxrsRbpJvJXuwTyUrsE8jG5RNBxsW7lsoVG5zOdROcihVzyjHdy04nCWh394aJAcSyDi0QypMBfH8G5+ZDlyy2TsgeUXg2g+dQX7tFJ/l7c54IiCZtK4W81+qF0Tunk/M5NNURgeAm4iEA302b+5v7PoxXfImj3Jl1eATcw+CEJ+NDP7XYuTiG5kRkUGNKNMIQk8C8KzNYT2yFc/YhusEL1rEHTf3KX0idGZ1IiwK6l6NLcTmD5la/3mqmW3DSi3nb1/P2JH1/NFnOLIEKBCfP78WURjzunG6Wbht1VZgATm5U+fgwPiT9YfXHmgeHVmSkoQXvlB6/8dtZYoe497ShEkZ4ycEhoV8QR1TLrfXXRKHFt2vcMS7PIbB+2ANkppaGp4fqchxPYVuUabza6azQIJU8QWHz2ArAJdMPhC+xPM++rO1op+xgZP2FqMQHMMKU7qUuvNjBBef6Hwcy8UrhVi3Y2P0jdXuSnRC9AuKigU6yCBsC2EJ/7ZrYyMY2kt3RZrsB+2QQppVEccCGuZYd5Y7RIEVxwC8eDGyKurud3I2GnXylMKx8yz1I6VNWFohNBSE+UeWWj8194klnGizp+rPv35KDlW06TNIbSKUOI7mUI1X13Dwkz1He7e1KMKyB1CgKYQouJNaks7NUOS2H8mbL3KxLSWbEZ9DdedbENXtOYyF+gx3tqfc0w5AeRm5SuO5vgGuR4H78Whu5pi4oq2461SQ7BEa0AOkIxbiypaQXDjO4fzoHbZL4HQqMpOzehALFTQXcTtO32GLNGKfXpzRtSNEExs29MkajCVaGgnYaz+qAcogPWAUeoUFtdo53aQNXd14xViOXPkPsHYIBkFtBgbG/cMDDP//QEGHgDQ9PP9ACHlB+wALQAWEhYVG/gCAQcLAAYDCwL8CvEM+P3/AOsADA3wGO8D8gkG6fj7AAD2BgH7DAEe9xIAAfgF8vwJBxYr8AEFBwv19Q0F4hj4B+cHC/fq/hITBvb59fX2+w4AAf/3+vwC5RL59Qn3+/cA7/wCBhH59v0K//P+AhDsC975AA3y4fTy/QL59AT9DvwI8vgF3QD46hzv9QAZF/j27ucE9OkO+/f8FfEIFd/+A+/5C/AGMfcWEvv4FQMDBvnv+fD/DwUC/ALvBf3qARgF7A8bHwANF/QJ7AYI+uUD8/TuBfIR2xroBwX0EAALB/4SDAD7+/H28gsG8O/r5AYAMQEAAJk0zjmMNs45mDbGMZk+RqFJOVboZTnErEW5zrR1Pk65EbpG6Gk5VuxJOZasaDlUPOymxTmM5ogxHKZccZm+TGFZM8bpQbGG6EmxzqRpNcy8ETZGuOgzRbzJMVa0yTFWrMyuwTGspph1bKGYdFmxnmVZsc5hYTmKaHk5yGRpN8w1kXbGuck2xrjpNUe8yTFGvOm5Q+XlsVhl5KMYdGWqzDVRmM51YTmOZEgximTJNYpkgXfKqYlmUznpblG16DlVtBG5TuxBuU5k4bFOZJGuzDGZrswxWD7MMWyxmDTJNZrsoTVKqInmQ6nkZVk86DFbpBG6RukZvkxpwTpOaYk6xDGJPsQxSD5EOWWxnj7lMVio4TdJlZXmQ6mEZ1m5aHdZvEE5l2xJORbk4TnG5MkzRWmJvlZ5SbpMOWE7zjlBMc6o4THHpJWmx6mFPkapWTpEqWw5kWxhMZ7kxKOR5cTjEWzF6kF5YbnOaGm6zjxJOU5sYblbrFE6RqVROE6pWbVO5clryTTls8rl5bOZ5GSrEaTFolmhWarKoUyqiTVlO5E8ZTmTLEE5zqRZs87kWTFO7MmmyTXl44k17DkRZEw7kaTMp5k0SaNDPMzuWTVEqlk0QbmbfME5zmRRO86lWXEerMk1hjzpMYM2yTmebEw7mTzMa5M0TOqBPMXuwTVMroM1QbiDZME5inSR6s414XdKtckxRuyJMUbkWTle5EG5GuRMK5G0RO6RvFnuwTRM7oE1bjqRNMl5RjSIZko1rGeYMVwP8btqaZ2lf1hil5VEDPDffu53vOEiZJJqEriZJK1CCDgrKb0509iYGu3NaO6Y+JFlNkjvjj51Iwmj8hrdPvb4imV+EMSoqp2YcUFPK3nP4GTdgYgxzDguNPTJPgYuN+SeL97SxmDs1XPnW9NhMJbrvGQFTYsvUnY8cQ6uEt1LQdfF0NSEnHRqKYxpca8oKj1nxOH76GUWAS1wV+WTb3+UbBmpjtL9b8vk4aKrF+1qt7TQz9Vbrcrkftrf4baCZ4dLmGednW9X3hdNQUomKq/si/c1QkoM5OYwGqLpwe39PvBB9wo+RucUs8u3+Pqde+qzjzCGBgE6YstZ3Qs+RDX0F/61mhfUxXLHkygMJ8Kj0IutCGAsQ2YVfNYT596XxsQ/3ZUoD3yYA6zaZt51eSXY84Od3Ut/tSBhfAwl2mDFSkngHlqPHPUdXSiH/vTXCDp99FXRvY/NHS6WrLy5+/aQz0n+t9iox8CAo5isygVhlHUlzIvjXR04StPe8w0602UWD8x/tIiMYor3+XCwAZEk1c9pfio81AznsUkyoXlO+3rVk4OvDTMWsZ+YNmxXNarM2q+adG8tcWONlzQg11NJ6VGU4JJll4l2OdHbfyMuyIl1aJvDVesb539iLCpact9KGbTlx3xADQvclleBqgjCcj6oFs6Oh3hH3ZVzJEwobeRSt1vW6V1Lb7yfaitDBAfhWu+Ac5u/pk2VgSlUNGci2dA5NQsl9THg3jZ3UVkQecegj8U3J1Yqtfp/LRugAf1hZ7rMRrtqZPUgqeLGuFPgpBQFbyAYJPNE5gIcEA60EyUN/wcWNQAG5wQFAdfo/P33IegN6wceAA0MCRj5CPgR/AURAv7////47wz69/f+/P8QEPUN9gH4AAfxAPrw/vMG8vsX+Af0DwD+9xL++gv+DB/fBQsDBu37AgviKf/8ARn3AAD4EAgLAQT+/PfyA/0ECPT6GArhEvTtEeb65fv+/BENDv/v/iL29vv8Dt0J5Pv5Efj0APUIEPn4BPgF9goCDAHrDPTiF/LxARoR9Pzz4gD88Rv/7fUR/gUW7P/3+v4M7g0p9gkS8fMn//0E/fn1/PYVAQb5BvX69ewJFAD4Dh4nAAYb+Q7rEvsD5QDs8/P64xXZIPcSAvoTBAIN8hUVAwjsAO/xBBTv+/HuBAAuAQAIAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAADxpLwA7azIAOmowADtpLwA8ai8AP2ovADxmLgA8bDEANQDK/wIA2f8MADMAvP81AKf/xf/v/ywAyv/X/xsA2v8LAM7/QADM/zUAMABZADQAif8lABoA8//7/+7//v8OAC0A/v/u/wIA4f8JAA0A5//A/xEAFgDL/+X/IwDe/yIA4P8dABUAFwDi/9T/vv8KAN3/+f+p/ygA1//R/8n/PgDx/9L/0P/v/wAA1/89AAsAIAD4/9P/yv8TAAYAYQAKAO7/5P8NABgAdAAOAOj/BwDW/w8A3f/q/9z/8P9JABMA7f8FAAcAf//0/+X/xv9fABEA6/8AACYAIwA/AOr/wv/T/w0A9P/9/97/6//l/x4ArP/5/+j/zP8hAHcA1v8SAGMA7f8dAAAA2/+3/+z/EwDg//b/AwAWAPf/5f/Q/zoA4f8uAMz/0v/y/+//yf++/30AXwDl/xIAAQC0/8T/jf8QAB8AOQD9/7T/NgDt/7X/0P/k/0YA+P/7/0AA8P8SAEwAZQCv//b/EQDm/2wAxP/g/+3/xf+8/y0A/v8nAPb/GgDV/ykA9P/0/xUARwD//8T/RACw/8T/KQDj//D/RQDh/8D/DQC0/yoApv/j/+n/uv8NANj/RAADAPT/4v81ACMA6f8rABkASwAFAJv/PwAXAEgAEgDG/7z/LQCU/zkAt//q/x4ABADx/wwAAwDa/////P/F/xwA5P/C/6P/AQAoALP/7/8NAPn/PQDK/w0Al//B/9//MQDi/9j/DADb/wsA1/9WAKv/KwBFAFIAIQCN/zEAFgALAAMAzP/4/xkAUwANAN//CQDQ/wUAAADp/63/KwD7/8H/+f8XAAYA/f/S/zcA9/8bANL/6v+i////0f/Y/4b/KQD0/7f/0P8VANn/0f/J//r/9//V/ysA9v8TAPD/sP/O/zAAFABwABcA4P8DAP//BgBgAP//9v8IAM3/LQDa/////v/w/10ADQABACoA+f9k/83/5P+5/20AEADv//j/SwD7/1QA9P/I/8P//f/i/xYAyP8VAAgAKwCm/yYAAwDX/zQAhAD5/ygAWADt/yoACQDa/8z/2f8LANz/6f/X//v/DADO/+D/JwDE/w0An//n//j/EADu/7z/fwBYAOb/6/8dAL7/yf+I/xwANAAsABEArv8yAPL/xP/k//n/IQARANv/OQDN/xAAaABnAL3/9/8JAMf/ZwC6/9b/AwDW/7r/QgATADkA+P8xAML/PQDb/9b/EQBtABYAvf82AK//of8mANH/FwBNANf/z/8NAL7/HQCO//z/3/+J/wUA4f8xAMP/BwDH/zQARQDP/xMAEQA+AAAAmv8/ADQAUAAbAOH/tv9HAJP/WQDC//b/IgDQ//b/DgAmAOf/6f8bAMT/GADm/8j/rP8TACYAs//e/xIAHAAgALL/DwCb/7L/+v8vAN//3f8DAOr/HQDZ/00AxP8bACQAUwA9AIT/GAAmABUACQDJ/w0AHAA5ABsABQAIANz/GAAAANv/tP8cAAQAw/8YADIA//8TAOL/RwDY/yIAvf/R/8P//v/T//n/fv8lAPL/yv/K/xoA7P/P/8T/4//k/8r/IgDz/w4A6//E/8r/NAAzAGoAEgDU/+L/9v8NAHkAGwAGAPH/1f8mAN3/9v/2/wMATQALAPD/HQD5/2b/yv/r/8P/TgAbANb/9P9SAA4ASgDk/+j/0P8NAPj/8v/Z/+//4v8UALf/EwD9/8j/BgB3AN//FABlAAwAOwD//+L/0f/X/xEA3//U/9//7P8KANv/yP88AND/MQC3/9j/6f8EAMT/w/91AEgA6P/s/ycAtv/N/5b/EAAcAE0ACADB/yUA9v+n/+v/+f8kABgA6f9HAPf/BgB3AFoAr/8OABUAwP9rAMD/yv/7/8j/4/9FABcARAADADQA4/8oAPz/7P8CAFwAEwDB/04Awf+2/x0A2v8kAEsAwf/B/xIAuv8JAKL/9v/8/6P/CwDT/zEAy/8HANT/KAAwANr/MQAgAD8A+f+k/zgANgBRAAIA1P/B/ygAmv8xAL3/+/8kAOr/DAALABIA0v/m/xgAqv8YAO//z/+o/xsAEQCv//v/9P8dACsAx/8uAJr/uv///wAA3f/b/wAA6/8dAOT/YAC+/x4AJgBOADoAe/8XABwA4P/u/9L//f8yAC4ADADz/wEA1v8QAOz/2//G/xEADQDQ//T/JQD5/xQA2/8zAAoAKgDf/9j/uv/9/8z///97/xYAFQDR/7r/GwABANH/zf8DAAkAyv85AAMAFgAAAM//2/8YAEcAcAAGANP/3f///+z/hgAUAPz/FgCw/x8Axf/u//v/AQBAABgA//8RAO7/b//E/+L/s/9fAP//3/8bADMAAwBMAPL/0f+q//7/7f8NANz/8P/7/yMAy/8YAP//vf8WAHAA//8TAF4A+P9LAAAAyP+2//j/+//U/+P/7f/z/wYA9P/B/zcAz/8UAKr/zv/x//f/xf/J/4EAVQDZ/wcAEwCl/8v/g/8HADIALgALALf/AQD+/7T/1v/5/y4AEQDk/z0A3/8PAFwAZQCw//v/EADJ/2oAz//2//j/yv+//ysACgA2APb/OADN/xYA+f/P/w8ATgDm/8//KQC7/8v/KQDP/woAUwDh/8D/CwDA/w8Aof8BAPL/sf/2/8//UQDV/wYAyf8mABoA3f8jABkASwDp/7r/MgAqAEsA/f/O/9X/RACd/0cAv/8dABYA0P////r/BgDW/+v/KQDY/ycA6//F/7D/CAAkAJ//4P8JAAoAFwDL/zcApv/I//r/KQDa/9n/EwDy//n/2f9uAMv/EAAXAEwALwCE/xwAGwACAPr/1//5/ygAKgD9//z/DQDJ/wgA7v/a/8D/CgD//9D/AgAtAAoA+P/U/x8A6v8ZAMj/0v+0/wcAvf8BAJb/EgAHAK//0f8QAPr/6v/R//b/8f+9/zwADQAhAAYA4f/J/ycAKgB0AC0Azv/p/wIA9f96AB8A5/8BAMT/HQDY/+3/8/8YAD4AFgDz/yoA+/90/9H/6v/R/1sAEQDY/wUARwAWAE4A7//i/8//CQD7//r/5v8CAOv/JgC5/xMACgDD/x4AhQDx/w0AXQAPACIAAQDI/63/9v/0/+//0//9//T/+//X/8//OADH/w0Axv/F/+//7//D/73/mABaAM3/BAAMAJz/y/+P/x4AGgArAPX/pv8cAPv/v//T/wAAGwAPAO//MAD7/wUAbQBgAKn/AQD9/8D/cwCx/+b/4f+9/9r/HwAKADUA/f83ALX/MQAIAOf/CgBMAP3/xv84AKz/v/8fAM3/FgBTAOf/2f8UAML/HACW/+7/6f+w/w4A4f86APT/AADL/zYAMwDb////IABWAOL/tf8tAD4ARgD7/9f/y/9JAKX/QwDZ//r/EQDx/x8A+/8ZAMj/+v8dAMD/LwDm/8f/sv8PAFIArf/g/9z/BAAeAMH/OgDg/9z/7P80ANv/wv8DAPz/EgC8/0YAzv88ADkATQAZAKj/IwAZAP3/6P/c/wMABwA1APf/7P8WAPf/EwAAAM7/t/8BACMA1P8CACsA4/8GAPj/LgAJACgA6f/K/7///f8LABEAif8nAN//z//g/xUA8f/J/8z/7/8PANr/LAAfABwADAD8/+n/JAAQAF4AKQD7/+n/BAAIAI8A//8DAPH/7P/0//b//f/f//z/ZwD6/93/IAD6/2H/9P/m/9j/WQD//7P//P9KAA4AOQD0//D/3v8PAOL/HQDG/wEA7P8VAL//GgD4/8X/JgBgANL/JgBZAPr/JgDw/+b/yf/2//7/8v/3/93/GgDv/+//0v8rAMj/GAC//+f/FwDr/+n/q/9rAE0AAwAeAN7/vv+7/6T/IgAgACkACADI/yEA5/+9/9j/4P8JAAgA2v8iAP3/4/9MAGYAs/8RACUA1P9nAN3/BwD1/8D/yv8FAN7/HQAQAAgAzP8bAOv/4f/9/04AGQDF/0MAuv/Y/x8A4P/2/zcA6//a//7/xP8NAJX/8v/i/57/+P/v/yAAHQAPAMf/PAAAAPL/DgAlAFsA/v/G/z0ANwBtABUA1P/P/yYAkP87AMv/2P8YAPz/BgAaAPL/6f/U//3/0v8VANj/2v+5/w8APgC9/+//2/8tAD4Aov9DALn/tv/6/0cAxP/c/zcA9f8rAMv/WwDX/wQAMQBEAFQAkP8WACMADAAGANP/GgAaAFUA6P/s/xUA6v/3//P/6v+//x0APAC1//L/DADi/w8A2v87AA8AGwDx/+b/kf8DANr/3f+M/z4A2P+q/6v/KwARAOT/5v/e/xMAy/9bABMALwDp/8v/5f9eABwAcgDt//T/uf/Z/x4AhwBAAPj/BgDE/xsA3//a/woA5f9EAPf/7P8EAAwAov/a/+f/vf9AACkADwD0/1YALgBMAPz/vP+0/wgABgASANX/2//L/wgAvP/v/+z/zP8OAHQAw/8aAFAACAAcAOH/2v+e//7/HQDB/wQACgDB/wMA/P+S/0wAz/9RAMH/1v/r//r/vv+b/3IAiADu/woACAC+/9f/bv8ZACYANgDs/6j/MgAKAID/1P/z/xAA8v/d/1EA6f8JAGQATgCa/wEADgDY/z4Anv/h/wwA1f+7/zUAGgAaAOr/KgAJACAABAD4/ykARgDj/+j/YQCo/7r/JQDR/xEARwDi/7P/+v+r//3/yv/D/+H/0P8lAOD/TQAZAN7/BgAkAGAA6v8vABkAWgD2/5L/OAAPADkABgDh/57/IgCo/z8Ay//z/wIACwDo/+//IwDc//z/AACi/wwA2/+y/6X/IAAPAKP/BgDy/w4ANQCx/x0Awv/M/+T/NQDI/9P/EwDv/xgA5P9fALH/GwBBAGkAPgB0/xQAKAAXAP//w//2/yoASAD8/+T/FAC1/wMADwD7/8P/IQD8/9L/4//8/xAA5f/d/0wA/f8IAMj/1v+1/xEA5v/g/2D/KwANALL/4f8XAOr/8P+2/wMAHADP/z0A//8eAAIAuv/N/xUAMQB3ACwA6/8IAAkA9f+DABcA/P/8/97/GwDj/xIAAwDy/18AEADt/yEA7P9i/7b/8f/G/3MAEQDn//D/QwAAAHMACgDq/8H/9P/Y/xUA5/8cAA4AFwC+/x0A7v+3/yEAigD0/zMAbQDl/z8A9f/n/6r/4f8JAM7/AgDq//D//f/Z/8j/MQDH/xIAtf/N//H/GgDm/9L/cQBUANX/2/8GAMH/5P+S/w8ADgA7AAMAsv82AAMAqv/x/wkAGwAHAOH/MADS/yIATABfAMX/FwAZAMb/eAC5/9H/BQDa/9r/OwAEAB8A7v8eAKz/NADq/93/FABdAAIAw/8lAKr/qf8aAMP/9P88AM7/wP8JANH/AwCR/woAsv+k/+//6v8+AOT/5v/T/z4AGQDh/w4AIABTANn/qf86ABUAOgAeAO3/qf9FAIH/RQDn/+z/FQDC/wsABQAZAN7/x/8VAKb/HgDx/8j/s/8GAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA=",
                        MD5 = "asdfasdfasdfasdfadf"
                    },
                    new HTTPv2_Protocol.HTTPPeopleFeatureCode() {
                        Num = 2,
                        Data = "zAAAADEBAACpNsQ5jKbIMYhmyDmJNsQ5mTPG6UG5huVVuc7kZTHIvFGqTjFpO0E86TFRtEkxVqzsvMU5jK7BMeymmXXks8hxSbHOaWGxnOxFsYjkaTGINJVmzjHJp8U16DfBtOkxRKTMPsExzKpBYWyrGGXku4h1WbnOZFG5jmxZMY5kYTGIdJF2zjHIZskx6GZZNeg5WbSJMU5kQbleZGG5WmSVrs4xmK7MMbimzDlpsZ50wbWKdME1zqiJdkO5rHZLOegxWjSRvkbhYbpM6VG+TGmZZkxhmSbMMZk3xDlJt8Y8RTGKJOExSqiFdke4hDZcuel2XLiBudbsUTtW7Bk5RuyRPkZpkW7EYZkjxnlVtsg4YTGabEk1GqyFNUqsGTVGrFE5RqTlMZWsaTmVbIExlmzlMZHkzOOBYUUzy2lhu8os4TvqZEW5E2zMuVvkWaZKpVGxXuTEYVW4yGsRrOExWmzhuRrkbKuR5FWmyWlVqsmlzOqRteyzEXzIsVpsGbFe5Fm5XuQRJl647KdZtOUxiazBM5fk4bmR5EUuwTxRbsk8zHaBNcwzkTRJsZpsmLFOZBnjzjVRsUaoiTVe7OkxxLTBOU7kYbkZ5ExqG7RRJsk8yKfLPMxnETRIORpkiDtOZZzmWrURt8a4iTdGqBkzxrwRMc5kQTGaZEwjGTxE5pk8WabJPMzmyTXMo4E1iDXKZYxmGDVBscNsiTVGbJE3jmyBN850WTFabGmxGWxErlk8EbbKPMm2yzTM5pk15DWZNIRmiThKnFiQd6GtmH9mUl2IbqhjjKD9pPIPK3lVaRR1dPpvFsqY5d2DsiH6si8UcmajX9ni/EjmofyPjlQqRi4sBh/GFrvEIMhMDeHawnxboVCWsyYY90AOz2ilfHghxJ/1pcI9jjfCQzoBKQjy+j0G4vzZtNCGVS6sgk6MeO1g/1Nxhx4sKw6C8EXLXMXTo1miB95o+9TPA8iOXvY+bYr4wsR/NjwGdJLCCHtDyxzCtC8BeO8n2pVrbaLTGdCVL1IJHLWaRq5s2kVYyJhvvr+NbGgfS5c3dj5f2Uzh/tjzfQgrpJ5Yvpm8jPwNDCI/NLdAIR5ENeS2fmNVPysuKvX386xtMxj+FPV0mrShKmMqpg5Tf07CwsbVfwsrcoS8cEpcXH32LkR0EQNiQFTNAefUi0SUCGiht3B6f4WQRlyX9mvyASkZX4eO1ya55Da1kes568MeOpzRNwHy4/rQlI4Xgj4/e4gDbCsx/efKmcukXdYtlKUPz0/xw7agw2PWsVutf7VpWpVPvzN79jQuNlQ+Ll6CU59QR/IdLPIdMPEt7j/KTPes2NThyLHCjIPeyBQBFjMm73lTUSg/ChPlqH4rtG3aNZrcj/JW7BG4qKHr9X8JE11WRe0R/OU0oFQEn1gtPbrarejq2L5LE8OjVicw9AwvVmoiKwMr91soD9vJ6ZbbjMznh9w7Oe7N/PSO195NSROcrbE4bYF/g/vVArSqjHUTf1sNdctNQXUw+wHdj1g97N3RKZGED9zDFrkZveGGsB0wFGmztOa0XpAaTox0aucdElTUPnPoRTwgVYTFFhQWswYuDQEgDjT6AvT9AAXa7/v/+CDfGOwLDAMRAAoa//z3AgYND/n1BAAH9fAE7Aby/vz5CwX6DvgJ8wcJ9AP94/r4Aff8CgMFAAv2+f0N9Pj3AgYe3foL/xDm/QsQ3yMECv0Y//n08BAZAv8D+/Hz9gYI9Qn6/Q//5Az15Q3qBe31/fYPAgnt+fsW8vMLAA/gC9v8/Rb27/TsBxL1/AAEDvoM/gsH5Qv+6iTw9QUdCfT9+uUH/fEQ9vD5D/UNFOH3Af0EEPQIJPYFEPYAIgEGAP72+P8HGv0H9Qr8Ae7oAh0I9QUSIPsGGfcN8QoCB+EE7vb0DOkb2x32Cv/1DgT9C/UPCwUE7/X36wwC5Pr04RUAMQEAAME8yrWJNM45iTZOOZk2RjGZO0bpQTnO5MU5zqRlsUm8kb5GaGk7QbxJOROsQTFbrIkmxTGMdsg5jGZIOYh2TDmZM8bpSTHE5EGxhuRpsYjkEX7GqUFuxbhlOUG8wTFbrMyuwTHMrsU56KqZdMyjiHVJscZsQbGG5GExjORpOYhkEWbOOcFmhbnFNsG87DFRtMyuQTFMulExbLsYZEWriHRlqch0UbnGbEExjmRJMZpkUTWObMF3yrnMZ0mx7DlZtEmxzmRBuV5kQTlebFWuzDGZrswxkTbEOek3xTzpsRp0YTFarOk1QqjEZlk47DFZtJm2zDFpu0zlaTlMacE2TGmYbswxiTdEOcinRTjpOZ5sSTlarMk1RrgVtke4xbdOuZGxxmTJOcbsUbtG7IE5RuycblfpiaZXaVmqRzhhs0l8STHKbEmxR6xZtka8WTFOpGWxxK7oMZWsYbGUbMGzFuzMoxHsTOvRaW2Zw2roo8E16blTdEixW2xZO0q8Wbte5GWxVKzMc1W8ZLMZbKEzHuRpuRHkTOZR5VWmyeHMrhE1zKsRNEixEWzJsV5kQbHOpFGqTLkVpkq8bKdZtIkxXuRJOZPkTKORpFk2wbTIfsE0yK5TNWixE2yBsUZswTvONXm5RjyRsU5o6LVZPOmRWjRJsRpsTLMZpExmkbxJJsk8yDbBNOwxkTSJM8ZkyXrGtRGmRzmRs0Y4mbNGPIGzTmTBsR7kbLEZ5EzmG7Rprsk8WT7DPMx6iTXoOUY1yGZFuYLbctgGqL2of1VBVvkmjSOQdk++FR4rZd8/7mNW0Uz7vQjp6XuFCeGF/M5P6IA9uP3rVNCD2ltIEOGmzdjdqpkzkmvzjSO7hIBVGerlmlN8Nu3IUNRdHj/09rtg8FbINmxn0FTTwY+3dXJdOXtbhJ0qkhCx/gz0v7/cib3Vocy0YGFtZVNr7wQpm+dFEwq0MmmSpFNfuvyWRHFcsQmcrH9RYvVMo468hKvNNeXDMvOUsvfUjkcyQBBc3bw+NvxmgnwqY1qiGiziUV6viGhLP/IGZfkMyDhnfs3hkvcrpG1aXFV+FJZES+Ya8wj5v9rGlpqhr7Wyp6pZG53Fja+DW04HWq95xYpNRz0XENuhRA2ahDwzKSDtmgshZ6M5GeOaBzjaYtqksrzhKGJ8isjChspNnyDCs4d/eEs631+0NAQNNTRolaXNLeX9M8+Tv9cDc2halcEY0I2lNHWNTWIidUGloPVJ1uObojg6mYT+CzaBqPGeCauKYDA4Ny28XmGXsxHAOOy6x4N/6ZuZzR3FYEvMSN/Fk5p4fTWFhXmdgn7xjChQc2shMjT0uD4JETiEEs4TezXq2u+Tu345FHMuz4vxExvZfIg8VYDD0/Xxfw0KKTt+q8M6S1HIYBCrUhQwULGwyL9oErQ4raFLEFJvaQ3v8pQMT6XjrSX+1buoYGoFkpInorBR7baP5WySi0pUPnQrOVh0cWK6baYudEhRKVHur0kZMhoLn6F5j4c2hycS5/Ag0zCTzaSNZ+LcUS9XlKKugSXMqySBsAoTC4QmW7yi85e2vMIJ2jowrwkdFxa2DS8IABoGLvUC6wMLA9ni9wD8J+AW7gkHBQsAFhny+fMO/QAM9PwP/A/+8gjzAO8E8PUQCOwL8gbzCQr1DAHrAfAO8wAQAQn+EwP2+wz79QYNCRja/A4GD+b9AADlHPoAAgvzAPDqCBcJBPr06fH0BwcACPIAGxTpEfTzCusF7vf48AcIFQDw/xr9/AL5Ee0E3fvyFPvy9PsBC/XuAwAJAwT9C/3s9vzmHPj0/hMM8v7z2Qf39RP75PMW9hAM2vH98wMM8wge+AsP7/EmAgj8Afj7BwMb/P77CvUI7fL8GgX8CBAh+QcU8ArvDQQM3gLn9fwE5w7fH+8NBAEQAwgO9A4KCgT38PjsDArvAPftEQAxAQAAiTTOOYw2zjmINsY5iDZGMYk6RmlhOYY0RbnOtGQxSLgZNk65eblOrEgxF6xJMRes7DbFMYzmgTHMZsg5jD5MOVk2znlhMYxsRbHO5GmxzDiRNsa5VTvEuWwzwbRpMVekzK7FMYyugXHsoZh07LGKfEmxjmxFMYpkQTuKZGk5yGSRds65SXbEuUw3zLXuMdm06bHHZOy4yWVsudh1RKvIdVm6zjFhOc52STGedEkxiuSRNM7pgXaLsUR2yLHsOVi0iblG5GGZSmZpOsxxjT7MMZk+zDEZts55aLXYPOUxmKRlMciopXZBueU0WDjsMRg0mbpG4VG7TulZvkxpjDrMMZk2zjGZp0W4aLPFvEUxmqxFMVisxTdOqIW2XrjlNli5wbmX7FG7luxZuUbsxDNR6YmnTnmJo0N4TKNROOWxGqxIMRqs5TFKpJQ2xqVZOcao6TmVrGkxlWzhsZbsxKOR5cXqwWhB58t4aavBOMm5V2RIuVNsbDFbpFg6S6VZuV7kxXtFvEg7kTyBsx7k6blR5EzqUaRVqsl4VeqBOMzukzRsuZF8SLGTZMizziVZu57lxb5EPMw3GTTJMV7kyTETZOyxEWxVpsE8UabBPMzuETTsO5E0SDGaZMizznVRO17lwbFOaOg1mDzpMUo0yTEabMa7EWxE7pE8Ue5JPMyuWTXMp5E0SLGedIgzjnQRZ8o1yTFGaMkxVmyhO040wbleZEw5GexM6pO0Ve7BPFyuwzRM5hk1SLEZNIl5RnSIbkkxnndFuHCGqLh/RVZ5kka7iTnGNJHt/yNswGE4jGztnQJTgKliHkrt2Z4K46Qj2Fu5i9nzbEWtUGMmBG0MCEcFUXZtRqZL9a+4maRTMTE8pYN4jTbIgVEPTCgvB4cj57tsk9SCA6/AlOGagrZ2nXGsdIEMrUWI+HpbkCJ/C1/WzRiHBInszohq534y3idFHwaFHB6X39k93FhtX8e2AZqkf1hk/0Y1oJJbKS4cvsYx2YKwYO6otQlb9QfDIxzy35eAhRx4NI8rQLNIWpJRRzg+4AJlARHgDCNnh4dSxRg6MFZSHmYV7yI1ThvqNQo4C8ij9kHVEShUpqK2XIpJqZ9YPkVatmCZyHWllObBjzqH7G+olWxfOsuHnWWUiReKtEntYMiqWKM602Q1epPUK7Q2aJNhzMf5WX+oBTYxn2yXlSzr2DJWeMAX5wQeCzeBhG5hru89ge3P38z85wECcJjKiwISnxbR3aq3mVWFsIWIeyJEiGUDlvxY+zs0N2jGd+YMok3s37LCdYFMSr/iIsjI2IEHmJR9qGiRW51XY5J3CoYG0emzerxmf+yVdTRpfj8p9+J6o15yhUYTMet72a1pSQm6SCwaTuE8sLN+7uF/HRCETEzEHdkPM38kzIpADD5isOrAfDTlEaJjRg/NVoCL7bAQrRVaK/rkw6CGpJRIZgV9iwdpkmyBVejAD4x2PF4OkB8PYjPbtE7KkrCeWDomLQWsgNP/i4VhlEfuTmYrVGIRInSuPRLz6R8RiHmFKB4Z6CmS6woHc7f1VIUNEJr9L6pxv7W1YFTzk0Y1WRIZFbIRJgf8Gwss/P3qAAcM1d4D//Ig4Q/vFxn8Dv0JGfcA8wv1AwXzAAIA//jl/fMA7Pz9/A4K9A/0CfQFD/oC/d4A7gfx+xYEDP8S+fn6Dff7CwANF+H9FQUL5fQDDu0i/f/7Dvb2+ucPDQkEAPnw7vUGC/YF7v8ZC+oT8t8N6QTv9gTsBgUI9vwGIPf7AP4M6Ajc+f4S9P309AAM9vUEAxD9Cv4Q/OgK+Ocb+fQEEAn7+/LkA/jvE/vp9hLxBQrf/AD6ABH4CBj0CxXw7CX7CgH3+fgA/R/9CP8J9Avr7AUSA/EMFRz9DhXzE/AU+QzjAuz7//rlE9sl9AgB/A0MBAv7FQ0FAfL3BekOA/IC9ekSADEBAACBNMqhiTTOOY02xjmINsQ5iT5GsUk5RuhBOcbkVbHHpGgzSLFJO0o46DFRvOgxW6QZNsSxbDbBOezmgTGIZ0g5gTpEOVkzxulRscbsgTHOpOkxzrWRfs65yWdBtOkzSbTMLsExjKbBuYymgTXsoZh15TGOaEmxjmhRsYZo6TGI5OF+yDWRZk6xSWZYocl2SrXsrkExbLxBsey4WWXku5h1xDuMceGqzDHhuc5uSTGeZME1imQRNFqhSWZaqelzSrUZtM6hGbVO4UmxXmVRuc5lgSLMMZmmzDGJpsw56LfIPeUxGHzlMVqshXZJuOU1yjiZZkyxmbZE6Vm6TGkZsk5p4TNMOYk2xDGJZsQxSDbEOWU5ynjpNUqshTfHvIU2xjgRMc6swTGW7EkxFuwZOcbs5TFV6IEyxSnBOk45ZbZIOcmxTnRItUt87bFDNJE6SjxlNcysqTGXrGk5lWyBM5bsxKOR7MRjkWnlEcp5YbmKdMEzxiRMuZNsTLFLZFi3SrXkd8y5hXfFuEVrkWzls+hl4bmaZOSrGeXFOsgxxTrINcxukbRssZFsyLGabImzznRhM065hWdKucTrWDTl61h14TEaZOwxGWTMI5k0zG7JNMx+wTVoO4E0SbGabIkzynQZsU6sEbFO6MmxXmzp5lg16TNKdOSxmXRE4xu0XO7ZPFzuwzXIbpE1aDGaZIhzxmWRp8S5kbfGuUkxxmyBOU7kwblOZEGxmmRF6pu0VO4RNBjuQTTIblk1yDYZNYk1XqRwLpcB/sbSo39bLDj+/mjS1EFp9DkuRYeAONs0GcUYtcTiRdJYmhb4n+CzWupdAX9Ya3ggjghtKBXhrLWdoo5wy8phIJ0814x6TQvpz7Gtr2K/UD4LcS5J9OvEQOJsHMyrli6N3YGMvm9QMCNjgmJewdUhzdHmraxpDarGeZ+pjllVDzw71+zmkXWUTh3/pvahtp90cJE6y6a5XwE83kZ/wR7E/aBGWJUebXIW91QGfLi7t1/HCfGvl0YvRXACwIZ2/EIRcrrah6zJBnuMZ1MBGnfkGoHgStI+vNGNcMOna2jhlj7C4iMJOyQDId8B6sXez8QptPppPBIjC+S5oGZNwrqSb0l8CgxWZFHy+VBA9JxycCs46jtgnNufOy7hq9MLl1GivJOht4KF2TDMKHtDfNEg4KuKf3RTHN8tg70tK3dhmqjc3xXo7x1/j6nhXqCDuLjpJtaPjVxyWkEobKDQwLYDbvTloZJIJus+3fS8y+IbokiwW1smEBVfreCDCdAX127mxMSGcH2KhOiUc4eu2JDBun+LWUTvg3BfW3kpQlo7QxOr9BRO71kBBT/YSkIkFFA3/+LAwIOXF+NXCALaebcIu9SmnqM0EOIID3/w9xnvy1xgDazBCYsx10YOEyeflRDFPHXycRDgbxNmcTT6I+VKtQqGNFhLB+7duGyX+o90yPtqwEgJqt6puJ5iVw2sZhUafqvdd3xSRIdzSC9n5ZNgZL5KLRQR05qGNykfIUKlC2X/X9HiwoO7nFdBgeHzgqtuv16nq71GIRPVrm5PWMmmN7sLGQv8h8WqFx4NuAssBAkUBjIBBev3Dv7Y4vLz+B7pB+wACvwJ9hMZ+ATpBvcEDgTwCAQE+/3//P0CA/X8CwHrDPkO8AwE/wgA8gDtA+4AC/sT9goB7vsRBP78EBAn5QUODg3l7wAD5xkL/wIb9Pjr/xwRB/f2Bu349gsCAg739RkL5gf17Q/xBez48vkGAQgB8wIq7PgDAgvtFdv6/Qz18vr9Axf78AD0DwEDAA//7wf75hnt+QITDfoA9eIL9e4X/ev2EPUCEuL4+vwCDfoQJPgFFf35HwAAA/r5+f3+G/kN8//w/u/wBBQI8w0bJQMJGfsH6gsEDNcB6/38/O8h2RzyCAL7CfwJCv0PDxAD9fv25QsE6wDp6BQAMQEAAOk0zjmMdsgxjGbMOYg+RDGZOsZpQTnG5EW5zqRlMci4GTZOuWgzRbzpMVesSTFWrMS2wTmEpsExjOeIdcQ6zHFJts5xZbGMZUW5iGRpeYh0kXbOuck2x7noM1G0yTFXvMS+wTnEPsE17DOIdeS7mHVZuc5kQTmOZFk5jmRhMYpkkXbK6YhmSDHIakE16DlVNMmxxmTBsUrk4bNIcYRqzDEZrswxaLLMMUmxjnTFMZpskTGe6Jxmw2HsO8k16DFapJmzTuFpuU7lGbNOYZgnzDGZvs4xmbbEOemzxj1lORh8ZHFYrJTnxbmkt8axaDNYpVGzTmRJsVbsGbNG7JkyTGmZNs5xiaNMOUm6TDlJOZ48zDGLrJU1x6UZNcalWTpGpWUxlexhOZVsgbEWbOQxEWzEo1Fo5ZFKeeWzyDXpM+o0bDGZrFkxSqRZMU7kWble5MxzVbzMe5F8oTMWbOk5EeTEa1nlRaPKocWqibXMqoM0TLmbbEgxHmRJs07kSTle5MxuSjXMOVk07TlbZMkxk2TkM5HkxTbJPEx+yDXMPkk0bLkZfMixHmTJu15l6TmOpYG1TjnoNVq0qDdKNckxk2zEu5E0TGoRPFzuWTzIrlk06DqRNMG5nnSBesp1xXbKsYG3xjmJNc7sgTle5MG5XmRkuRlsTO4TtHzuWbwRrlo0yCoZNeE5lqSBdk6xhGaJsYkzxniBN86ogbbOZYGxTmRBMZpsxKORPOXm0TyZukY8yC5ZNcg2W7WlN065hHdZueSoP8aJlKOTf2xca3U4wFVNGDt5AQEif4NnHoV+9G8aRH24vA+UFAW8F/pmTsR9AnT4GcJ47nqHSSD5MQ9RJPjcf4T9oUIE7c6weVtGunke9My0N++0YoloRh+reAGYr+FM+IX+FtAc3MXTlOak5foouDjQx+F147dbyEqLMJ+gMytGGTouNcAmFujDrOkHJp4uHMHghHNStjFQc/G1tX9FSQ2MsM5IU4gvCZuvKwJ9sT7FpIB5q6n4mjcBLvnsnJZNsnPnSICoilCYpIFeWgw9vzBZKXP9rL7KmexH/x6Qg3HCdnGWgiLd+hs5gTMG7p8kFKnzZyz3Ae7119nTm3k2VxzMw6C6XtkcTtuHckcdmL2quJ+QdYyaoW19nZAzwwZ8JtNH6vMtYAisxqY08n7XQMO+Omd/mMdAOMAngKd88Pg6bnW/JMsIW+cf1I+hlOA2b7Ej+evZ+vTQv5Tj69IjXNhYECft08KRu/KTkeBRY+2sSNhqrp+QgSi7VghYfzW/bjnhEfzTLSYfCV0LGkQSfMMVx/Ol4J644YSKxq8fomCmaqj2bbp0XAPdWQY6DRaOb0kh/TAWK0MFxhT+jfsPc51f0TWk2pyOe+vufxEThXFhJVrzJQF7LviMSTNMyufdvq75hgHbm4E4/i3RByjqZNsjH+06F/vltejKcYNt0NKo+mn8r6nR7zam04OD8Eo8p0GpWmRB8LYGqolhXTKGJrKXAQPtJwi2v8Nia09mOaXsZUzXwXjNYeiefHsiFD3OgKKguj3xpO01rTaCFkLr7izD8q5i3uYtqz0aDhC7DSsM+hQPNPj57QAFAtLx+Ab7HuIc7wYT/g8IChsA//D/Ag8N+vsMAQP86Azy/vP8+v0SB/QP+fr4BA75AP3mAPQH8/4LARP5HPr5+w799/8IECTa+xIKEvD1BQrhJfkJABH5/fTyDxUMAQH58/XzDAL0DvoBFA7jBefkF/AA8/z89BQKBvv1ABf58AoBE94E2fj7EPT29fQAEPf4APwSAAH+DgHnDPzrH/XvBw8U+QH45A/67BD/6voO9xcc6PgAAAEO7g8e+g4L8vgh/wMD/fL1/AEU+A/6BfYG8/AEGgj3DhMg+Qsa/RDpCAME2P/lAPYA4hTeIfgJAgALCgEJ/BINCfz39vrtCgXqAO7vCQAxAQAAwTXKscg2zjmMNsY5iDZGMYm6RjFJOc5kRTnGpGW5SbxZPk44xbpHOMk5V+zpOVOs6TbOMcg2zDnsNsk5iHZMOZk6xjFZOcTlQbnO5EGxyqRROc64lTZGOek3QbzpMUakzK7BMYy2wTnMp4F05KOIfEG7zmlhuYZsQTmO5Gk5iGQRds45lXfOuUk2x7noMcW0zK5BMYS+wTnlo1h05bGYdGG5znxROc5mSTmOZEkxjmRRMY5slXfKqcxnybHsO1m1ibXO5RmxTuyJMU5kqbPOcRguzDGZPsQxWTbEPOixHDxlMViopXdBuIQ3y7nkMZo0mTdO4Rm7RulZOkxpyDJMOYg+xDGJPsQxyKdEOOU5Vj7FMVqo6TFDrIU2TriFNk65wbEW7GmxFOxZuVbs6TtG4emzVKGJpkZxSadMMc2zSDFpMUqs6bFDpJg2zrWJMU6lwbVWrMkxlmxJMZbs5bGW5MSjkejF6sF5QbnKfOGxyzXpOVPk5DlbZFg6yrVJMU7lxHdBuMhzlbxks5lsgbFe5Oi5EeTFqlFhRarIMeWqkTTEq5G0RbmZbFmxjmRBs45lxXZKucWjQTRMoxg1wbleZWmxGeTEulk0yb5BNcmuwTVFrlM0SbmabIGxynSR6841wbFOOOGxTjxopRg0ybHOdUmxmmzEsxk8TKoRNEzqETVM7hs0aDqZNIExinSRZso1kbzGMYGxTijJsU6kgbnOZUmxGmxEuxm8XO5RtFzuQTRIrkM1yG4ZNck5zjSJZsgxxMpp5cqluqp/U0RZMxmWNHhBT7sKGjl5lkf3cTjEV9F6A7Wp/XPv4JT1mmkWoEAMLhYzqmXUT/0H37283M3OeHsjBeNYBbN4ZT8T7e7nrJIQyjEUyzoEMtzpuYXyf5pOqUiaM6SCbexgP0MgfNSHGuZF8IhTHReSnviDwQ3Dq69SMv8lEunE09OmsqjwNvRqaptLJUP5C6JriwbBEsGTf2I87Tr5c5UKW3E85dk9BHimle2C2A8dNoDKJiIu4pF5fyFyHW4aIVx/fNx3XUg55xZe/wfc+w2iBNqE8CyTWkxX/FsBr8sKyAkIAwia1MNcgiYwIfVnf9rOr5yIipFaP3ODkIdamV/F2PMipPVR8YKGYDIjNipaL81dz1FJXXUPUMRs66Xou3dbYb5gcPlc5d6K88+nf3+AVy8LdBwZ1V0jM2V+sMEdyfkV0NmTGwbJ0Xd8tfrKhJIxh4kBUzfEr01AvzvMxYp7TjQ2BjjIXFCi0XUKnRxEDAz/HA8q535ayKv8tqGiaVY+VpQf33mnSJQ/o45aiV1bDzK/rjShYNlZ6dbBjq1babRFG5pW/2EG7h2r9xvFGUqr4u4j3If3tAYYyrz5e0dkzNO69v9/AQlDKjKbuYmoSMBY/7NHDw2jqsah949BaAejlD7oZ3Lh9+Lvbwkq6Y8l+967vpNNYflwhHZQeoTIt0bdYIx7Skb/fhMWSApgBIVhqOhgWsIbV+CbhrnbfNGMaGlnffkuNaV5szwjIl9vg1wglYZHHT7sVFZtii6FfNfGaW89YQfalX4BngW85UzhyyOs/yEbCbcOIAn/EhEx+ffz/A753OL79wEj6An3AhD7DvgQE/QB+AP6AAkA/QkACgD2EAD+9QTq+xEQ6g32APIIDfkCAun19Abw/BEADv4HBez8Bvv2AAkRIt0IDgoR5u0ABOkg/Pn7GPYA9vMWEwgB9P3y++8IBAgL/PsPCeEH8vAa5wHz9vXxCgAE+fYCFPr7/wAW6wvT8QAO9e33APsM9+8C+RMBBfsR/OYK/Okb9fUGGw/3+vPaCf7tGwDf9Rr1BRXb9fcAAAn1Cib3CBX39SD5Cv79Avz8BBr+DfgM7wr09QAgB/YNFSH/ByL5DPIL+gXdBO4A8P3wENwh8gkA9QsDAAz5FgoLBu/y9OwLDPr/9OoRADEBAACMpkExjKZZOYg2RTnJO0ZpybHWYcGxhuXpMcFkWLZKMWimWTXosVu0aLFavMkxXqSc7lsxzKZZccyrUXnIu1ZoybFXbMk5luxpeZSsiTZKMcimQzHIpkG16DlRtMy5VjRcKlixzDNZ4cyzWWVpsU5lWbnGbMkxlmzhNZd8gaZHOchmSzHofkk16DlRteSxVjRsq0WhZKpJ6Yw2QeGYp8yhwafEOMmxlnzltRs85TZbvIl2SziJMUq06LFTpImxV6QZukbpGbNO6Zk2TGmZN8ThiTfFOcQ6lTjlORk47HlZPIU2QzyFNUa8wblHpOk5V6TJMV5sQTFebIEzTmyBpsNphabHacWqWTnpOcp8aDWLPIU+w7xVOEauSbhepeGxUaTJc1ms4TkYbOQzEWTEY5FlxbPDYcWqSbVJumqlXDlabMU5WnxZuEN0Wbhb5OU4GaTFNVi8wTEWbOS5EWTE45G0xabBPMXugTRcPsOlxDGRZIUxmmTJM060abhbNERumbzBMU6sgTGOZMkxG2TMMxG0zG6RNEVuWTxV7lGk5TmR5ME5mmyBds6l4ebLsURumbiBM0a4gTNGbMG7HmTluRukxO4bPETu2TxB7ss8xGqRNckxluSJZk6x5OZZscRuGTiJs0ZoibNGZIEx1mTBsZtsROaZPGTumTyZ7sM8zGabNcimS7WIp0axzOeVtcxokTjJsVZsybGWZMGxHmRBsxrsRbpJvJXuwTyUrsE8jG5RNBxsW7lsoVG5zOdROcihVzyjHdy04nCWh394aJAcSyDi0QypMBfH8G5+ZDlyy2TsgeUXg2g+dQX7tFJ/l7c54IiCZtK4W81+qF0Tunk/M5NNURgeAm4iEA302b+5v7PoxXfImj3Jl1eATcw+CEJ+NDP7XYuTiG5kRkUGNKNMIQk8C8KzNYT2yFc/YhusEL1rEHTf3KX0idGZ1IiwK6l6NLcTmD5la/3mqmW3DSi3nb1/P2JH1/NFnOLIEKBCfP78WURjzunG6Wbht1VZgATm5U+fgwPiT9YfXHmgeHVmSkoQXvlB6/8dtZYoe497ShEkZ4ycEhoV8QR1TLrfXXRKHFt2vcMS7PIbB+2ANkppaGp4fqchxPYVuUabza6azQIJU8QWHz2ArAJdMPhC+xPM++rO1op+xgZP2FqMQHMMKU7qUuvNjBBef6Hwcy8UrhVi3Y2P0jdXuSnRC9AuKigU6yCBsC2EJ/7ZrYyMY2kt3RZrsB+2QQppVEccCGuZYd5Y7RIEVxwC8eDGyKurud3I2GnXylMKx8yz1I6VNWFohNBSE+UeWWj8194klnGizp+rPv35KDlW06TNIbSKUOI7mUI1X13Dwkz1He7e1KMKyB1CgKYQouJNaks7NUOS2H8mbL3KxLSWbEZ9DdedbENXtOYyF+gx3tqfc0w5AeRm5SuO5vgGuR4H78Whu5pi4oq2461SQ7BEa0AOkIxbiypaQXDjO4fzoHbZL4HQqMpOzehALFTQXcTtO32GLNGKfXpzRtSNEExs29MkajCVaGgnYaz+qAcogPWAUeoUFtdo53aQNXd14xViOXPkPsHYIBkFtBgbG/cMDDP//QEGHgDQ9PP9ACHlB+wALQAWEhYVG/gCAQcLAAYDCwL8CvEM+P3/AOsADA3wGO8D8gkG6fj7AAD2BgH7DAEe9xIAAfgF8vwJBxYr8AEFBwv19Q0F4hj4B+cHC/fq/hITBvb59fX2+w4AAf/3+vwC5RL59Qn3+/cA7/wCBhH59v0K//P+AhDsC975AA3y4fTy/QL59AT9DvwI8vgF3QD46hzv9QAZF/j27ucE9OkO+/f8FfEIFd/+A+/5C/AGMfcWEvv4FQMDBvnv+fD/DwUC/ALvBf3qARgF7A8bHwANF/QJ7AYI+uUD8/TuBfIR2xroBwX0EAALB/4SDAD7+/H28gsG8O/r5AYAMQEAAJk0zjmMNs45mDbGMZk+RqFJOVboZTnErEW5zrR1Pk65EbpG6Gk5VuxJOZasaDlUPOymxTmM5ogxHKZccZm+TGFZM8bpQbGG6EmxzqRpNcy8ETZGuOgzRbzJMVa0yTFWrMyuwTGspph1bKGYdFmxnmVZsc5hYTmKaHk5yGRpN8w1kXbGuck2xrjpNUe8yTFGvOm5Q+XlsVhl5KMYdGWqzDVRmM51YTmOZEgximTJNYpkgXfKqYlmUznpblG16DlVtBG5TuxBuU5k4bFOZJGuzDGZrswxWD7MMWyxmDTJNZrsoTVKqInmQ6nkZVk86DFbpBG6RukZvkxpwTpOaYk6xDGJPsQxSD5EOWWxnj7lMVio4TdJlZXmQ6mEZ1m5aHdZvEE5l2xJORbk4TnG5MkzRWmJvlZ5SbpMOWE7zjlBMc6o4THHpJWmx6mFPkapWTpEqWw5kWxhMZ7kxKOR5cTjEWzF6kF5YbnOaGm6zjxJOU5sYblbrFE6RqVROE6pWbVO5clryTTls8rl5bOZ5GSrEaTFolmhWarKoUyqiTVlO5E8ZTmTLEE5zqRZs87kWTFO7MmmyTXl44k17DkRZEw7kaTMp5k0SaNDPMzuWTVEqlk0QbmbfME5zmRRO86lWXEerMk1hjzpMYM2yTmebEw7mTzMa5M0TOqBPMXuwTVMroM1QbiDZME5inSR6s414XdKtckxRuyJMUbkWTle5EG5GuRMK5G0RO6RvFnuwTRM7oE1bjqRNMl5RjSIZko1rGeYMVwP8btqaZ2lf1hil5VEDPDffu53vOEiZJJqEriZJK1CCDgrKb0509iYGu3NaO6Y+JFlNkjvjj51Iwmj8hrdPvb4imV+EMSoqp2YcUFPK3nP4GTdgYgxzDguNPTJPgYuN+SeL97SxmDs1XPnW9NhMJbrvGQFTYsvUnY8cQ6uEt1LQdfF0NSEnHRqKYxpca8oKj1nxOH76GUWAS1wV+WTb3+UbBmpjtL9b8vk4aKrF+1qt7TQz9Vbrcrkftrf4baCZ4dLmGednW9X3hdNQUomKq/si/c1QkoM5OYwGqLpwe39PvBB9wo+RucUs8u3+Pqde+qzjzCGBgE6YstZ3Qs+RDX0F/61mhfUxXLHkygMJ8Kj0IutCGAsQ2YVfNYT596XxsQ/3ZUoD3yYA6zaZt51eSXY84Od3Ut/tSBhfAwl2mDFSkngHlqPHPUdXSiH/vTXCDp99FXRvY/NHS6WrLy5+/aQz0n+t9iox8CAo5isygVhlHUlzIvjXR04StPe8w0602UWD8x/tIiMYor3+XCwAZEk1c9pfio81AznsUkyoXlO+3rVk4OvDTMWsZ+YNmxXNarM2q+adG8tcWONlzQg11NJ6VGU4JJll4l2OdHbfyMuyIl1aJvDVesb539iLCpact9KGbTlx3xADQvclleBqgjCcj6oFs6Oh3hH3ZVzJEwobeRSt1vW6V1Lb7yfaitDBAfhWu+Ac5u/pk2VgSlUNGci2dA5NQsl9THg3jZ3UVkQecegj8U3J1Yqtfp/LRugAf1hZ7rMRrtqZPUgqeLGuFPgpBQFbyAYJPNE5gIcEA60EyUN/wcWNQAG5wQFAdfo/P33IegN6wceAA0MCRj5CPgR/AURAv7////47wz69/f+/P8QEPUN9gH4AAfxAPrw/vMG8vsX+Af0DwD+9xL++gv+DB/fBQsDBu37AgviKf/8ARn3AAD4EAgLAQT+/PfyA/0ECPT6GArhEvTtEeb65fv+/BENDv/v/iL29vv8Dt0J5Pv5Efj0APUIEPn4BPgF9goCDAHrDPTiF/LxARoR9Pzz4gD88Rv/7fUR/gUW7P/3+v4M7g0p9gkS8fMn//0E/fn1/PYVAQb5BvX69ewJFAD4Dh4nAAYb+Q7rEvsD5QDs8/P64xXZIPcSAvoTBAIN8hUVAwjsAO/xBBTv+/HuBAAuAQAIAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAADxpLwA7azIAOmowADtpLwA8ai8AP2ovADxmLgA8bDEANQDK/wIA2f8MADMAvP81AKf/xf/v/ywAyv/X/xsA2v8LAM7/QADM/zUAMABZADQAif8lABoA8//7/+7//v8OAC0A/v/u/wIA4f8JAA0A5//A/xEAFgDL/+X/IwDe/yIA4P8dABUAFwDi/9T/vv8KAN3/+f+p/ygA1//R/8n/PgDx/9L/0P/v/wAA1/89AAsAIAD4/9P/yv8TAAYAYQAKAO7/5P8NABgAdAAOAOj/BwDW/w8A3f/q/9z/8P9JABMA7f8FAAcAf//0/+X/xv9fABEA6/8AACYAIwA/AOr/wv/T/w0A9P/9/97/6//l/x4ArP/5/+j/zP8hAHcA1v8SAGMA7f8dAAAA2/+3/+z/EwDg//b/AwAWAPf/5f/Q/zoA4f8uAMz/0v/y/+//yf++/30AXwDl/xIAAQC0/8T/jf8QAB8AOQD9/7T/NgDt/7X/0P/k/0YA+P/7/0AA8P8SAEwAZQCv//b/EQDm/2wAxP/g/+3/xf+8/y0A/v8nAPb/GgDV/ykA9P/0/xUARwD//8T/RACw/8T/KQDj//D/RQDh/8D/DQC0/yoApv/j/+n/uv8NANj/RAADAPT/4v81ACMA6f8rABkASwAFAJv/PwAXAEgAEgDG/7z/LQCU/zkAt//q/x4ABADx/wwAAwDa/////P/F/xwA5P/C/6P/AQAoALP/7/8NAPn/PQDK/w0Al//B/9//MQDi/9j/DADb/wsA1/9WAKv/KwBFAFIAIQCN/zEAFgALAAMAzP/4/xkAUwANAN//CQDQ/wUAAADp/63/KwD7/8H/+f8XAAYA/f/S/zcA9/8bANL/6v+i////0f/Y/4b/KQD0/7f/0P8VANn/0f/J//r/9//V/ysA9v8TAPD/sP/O/zAAFABwABcA4P8DAP//BgBgAP//9v8IAM3/LQDa/////v/w/10ADQABACoA+f9k/83/5P+5/20AEADv//j/SwD7/1QA9P/I/8P//f/i/xYAyP8VAAgAKwCm/yYAAwDX/zQAhAD5/ygAWADt/yoACQDa/8z/2f8LANz/6f/X//v/DADO/+D/JwDE/w0An//n//j/EADu/7z/fwBYAOb/6/8dAL7/yf+I/xwANAAsABEArv8yAPL/xP/k//n/IQARANv/OQDN/xAAaABnAL3/9/8JAMf/ZwC6/9b/AwDW/7r/QgATADkA+P8xAML/PQDb/9b/EQBtABYAvf82AK//of8mANH/FwBNANf/z/8NAL7/HQCO//z/3/+J/wUA4f8xAMP/BwDH/zQARQDP/xMAEQA+AAAAmv8/ADQAUAAbAOH/tv9HAJP/WQDC//b/IgDQ//b/DgAmAOf/6f8bAMT/GADm/8j/rP8TACYAs//e/xIAHAAgALL/DwCb/7L/+v8vAN//3f8DAOr/HQDZ/00AxP8bACQAUwA9AIT/GAAmABUACQDJ/w0AHAA5ABsABQAIANz/GAAAANv/tP8cAAQAw/8YADIA//8TAOL/RwDY/yIAvf/R/8P//v/T//n/fv8lAPL/yv/K/xoA7P/P/8T/4//k/8r/IgDz/w4A6//E/8r/NAAzAGoAEgDU/+L/9v8NAHkAGwAGAPH/1f8mAN3/9v/2/wMATQALAPD/HQD5/2b/yv/r/8P/TgAbANb/9P9SAA4ASgDk/+j/0P8NAPj/8v/Z/+//4v8UALf/EwD9/8j/BgB3AN//FABlAAwAOwD//+L/0f/X/xEA3//U/9//7P8KANv/yP88AND/MQC3/9j/6f8EAMT/w/91AEgA6P/s/ycAtv/N/5b/EAAcAE0ACADB/yUA9v+n/+v/+f8kABgA6f9HAPf/BgB3AFoAr/8OABUAwP9rAMD/yv/7/8j/4/9FABcARAADADQA4/8oAPz/7P8CAFwAEwDB/04Awf+2/x0A2v8kAEsAwf/B/xIAuv8JAKL/9v/8/6P/CwDT/zEAy/8HANT/KAAwANr/MQAgAD8A+f+k/zgANgBRAAIA1P/B/ygAmv8xAL3/+/8kAOr/DAALABIA0v/m/xgAqv8YAO//z/+o/xsAEQCv//v/9P8dACsAx/8uAJr/uv///wAA3f/b/wAA6/8dAOT/YAC+/x4AJgBOADoAe/8XABwA4P/u/9L//f8yAC4ADADz/wEA1v8QAOz/2//G/xEADQDQ//T/JQD5/xQA2/8zAAoAKgDf/9j/uv/9/8z///97/xYAFQDR/7r/GwABANH/zf8DAAkAyv85AAMAFgAAAM//2/8YAEcAcAAGANP/3f///+z/hgAUAPz/FgCw/x8Axf/u//v/AQBAABgA//8RAO7/b//E/+L/s/9fAP//3/8bADMAAwBMAPL/0f+q//7/7f8NANz/8P/7/yMAy/8YAP//vf8WAHAA//8TAF4A+P9LAAAAyP+2//j/+//U/+P/7f/z/wYA9P/B/zcAz/8UAKr/zv/x//f/xf/J/4EAVQDZ/wcAEwCl/8v/g/8HADIALgALALf/AQD+/7T/1v/5/y4AEQDk/z0A3/8PAFwAZQCw//v/EADJ/2oAz//2//j/yv+//ysACgA2APb/OADN/xYA+f/P/w8ATgDm/8//KQC7/8v/KQDP/woAUwDh/8D/CwDA/w8Aof8BAPL/sf/2/8//UQDV/wYAyf8mABoA3f8jABkASwDp/7r/MgAqAEsA/f/O/9X/RACd/0cAv/8dABYA0P////r/BgDW/+v/KQDY/ycA6//F/7D/CAAkAJ//4P8JAAoAFwDL/zcApv/I//r/KQDa/9n/EwDy//n/2f9uAMv/EAAXAEwALwCE/xwAGwACAPr/1//5/ygAKgD9//z/DQDJ/wgA7v/a/8D/CgD//9D/AgAtAAoA+P/U/x8A6v8ZAMj/0v+0/wcAvf8BAJb/EgAHAK//0f8QAPr/6v/R//b/8f+9/zwADQAhAAYA4f/J/ycAKgB0AC0Azv/p/wIA9f96AB8A5/8BAMT/HQDY/+3/8/8YAD4AFgDz/yoA+/90/9H/6v/R/1sAEQDY/wUARwAWAE4A7//i/8//CQD7//r/5v8CAOv/JgC5/xMACgDD/x4AhQDx/w0AXQAPACIAAQDI/63/9v/0/+//0//9//T/+//X/8//OADH/w0Axv/F/+//7//D/73/mABaAM3/BAAMAJz/y/+P/x4AGgArAPX/pv8cAPv/v//T/wAAGwAPAO//MAD7/wUAbQBgAKn/AQD9/8D/cwCx/+b/4f+9/9r/HwAKADUA/f83ALX/MQAIAOf/CgBMAP3/xv84AKz/v/8fAM3/FgBTAOf/2f8UAML/HACW/+7/6f+w/w4A4f86APT/AADL/zYAMwDb////IABWAOL/tf8tAD4ARgD7/9f/y/9JAKX/QwDZ//r/EQDx/x8A+/8ZAMj/+v8dAMD/LwDm/8f/sv8PAFIArf/g/9z/BAAeAMH/OgDg/9z/7P80ANv/wv8DAPz/EgC8/0YAzv88ADkATQAZAKj/IwAZAP3/6P/c/wMABwA1APf/7P8WAPf/EwAAAM7/t/8BACMA1P8CACsA4/8GAPj/LgAJACgA6f/K/7///f8LABEAif8nAN//z//g/xUA8f/J/8z/7/8PANr/LAAfABwADAD8/+n/JAAQAF4AKQD7/+n/BAAIAI8A//8DAPH/7P/0//b//f/f//z/ZwD6/93/IAD6/2H/9P/m/9j/WQD//7P//P9KAA4AOQD0//D/3v8PAOL/HQDG/wEA7P8VAL//GgD4/8X/JgBgANL/JgBZAPr/JgDw/+b/yf/2//7/8v/3/93/GgDv/+//0v8rAMj/GAC//+f/FwDr/+n/q/9rAE0AAwAeAN7/vv+7/6T/IgAgACkACADI/yEA5/+9/9j/4P8JAAgA2v8iAP3/4/9MAGYAs/8RACUA1P9nAN3/BwD1/8D/yv8FAN7/HQAQAAgAzP8bAOv/4f/9/04AGQDF/0MAuv/Y/x8A4P/2/zcA6//a//7/xP8NAJX/8v/i/57/+P/v/yAAHQAPAMf/PAAAAPL/DgAlAFsA/v/G/z0ANwBtABUA1P/P/yYAkP87AMv/2P8YAPz/BgAaAPL/6f/U//3/0v8VANj/2v+5/w8APgC9/+//2/8tAD4Aov9DALn/tv/6/0cAxP/c/zcA9f8rAMv/WwDX/wQAMQBEAFQAkP8WACMADAAGANP/GgAaAFUA6P/s/xUA6v/3//P/6v+//x0APAC1//L/DADi/w8A2v87AA8AGwDx/+b/kf8DANr/3f+M/z4A2P+q/6v/KwARAOT/5v/e/xMAy/9bABMALwDp/8v/5f9eABwAcgDt//T/uf/Z/x4AhwBAAPj/BgDE/xsA3//a/woA5f9EAPf/7P8EAAwAov/a/+f/vf9AACkADwD0/1YALgBMAPz/vP+0/wgABgASANX/2//L/wgAvP/v/+z/zP8OAHQAw/8aAFAACAAcAOH/2v+e//7/HQDB/wQACgDB/wMA/P+S/0wAz/9RAMH/1v/r//r/vv+b/3IAiADu/woACAC+/9f/bv8ZACYANgDs/6j/MgAKAID/1P/z/xAA8v/d/1EA6f8JAGQATgCa/wEADgDY/z4Anv/h/wwA1f+7/zUAGgAaAOr/KgAJACAABAD4/ykARgDj/+j/YQCo/7r/JQDR/xEARwDi/7P/+v+r//3/yv/D/+H/0P8lAOD/TQAZAN7/BgAkAGAA6v8vABkAWgD2/5L/OAAPADkABgDh/57/IgCo/z8Ay//z/wIACwDo/+//IwDc//z/AACi/wwA2/+y/6X/IAAPAKP/BgDy/w4ANQCx/x0Awv/M/+T/NQDI/9P/EwDv/xgA5P9fALH/GwBBAGkAPgB0/xQAKAAXAP//w//2/yoASAD8/+T/FAC1/wMADwD7/8P/IQD8/9L/4//8/xAA5f/d/0wA/f8IAMj/1v+1/xEA5v/g/2D/KwANALL/4f8XAOr/8P+2/wMAHADP/z0A//8eAAIAuv/N/xUAMQB3ACwA6/8IAAkA9f+DABcA/P/8/97/GwDj/xIAAwDy/18AEADt/yEA7P9i/7b/8f/G/3MAEQDn//D/QwAAAHMACgDq/8H/9P/Y/xUA5/8cAA4AFwC+/x0A7v+3/yEAigD0/zMAbQDl/z8A9f/n/6r/4f8JAM7/AgDq//D//f/Z/8j/MQDH/xIAtf/N//H/GgDm/9L/cQBUANX/2/8GAMH/5P+S/w8ADgA7AAMAsv82AAMAqv/x/wkAGwAHAOH/MADS/yIATABfAMX/FwAZAMb/eAC5/9H/BQDa/9r/OwAEAB8A7v8eAKz/NADq/93/FABdAAIAw/8lAKr/qf8aAMP/9P88AM7/wP8JANH/AwCR/woAsv+k/+//6v8+AOT/5v/T/z4AGQDh/w4AIABTANn/qf86ABUAOgAeAO3/qf9FAIH/RQDn/+z/FQDC/wsABQAZAN7/x/8VAKb/HgDx/8j/s/8GAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA=",
                        MD5 = "asdfasdfasdfasdfadf"
                    }
                    ]),
                FaceFeature = "gH8JBPk1HlReXoRskq7gAwEAAggFDAYSDg8ZAwsJDRwHEBMVgdUBj/CzXLO5TxcbGBwXIRcdLCoM2f6E4jd6TvqtBAgaAgwJAAYFEQIHCgEFDBoZDQ8DBAEZEhQWAAMPEQILExgbGhkIBAsCCg4DEQAFExcJFh0ECAERBgMKFQcNDRMFEQMLBlAwPmYlZBB1Fz44cjO9RfnpCZZhhImolIqnhm+NcXPAt7h9zEiYUqlRuWJJyJ+srr5bAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAIBedJZVkKShSEuqdVGft5kxrEy9Q78dI6pXWS4xFQAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAACIAHCAgGiosICIsOn42MDhEeERYZC5EJji+nqC+p0AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA//+rr8D/////g4oK4P///x+gIAD4////AAACAPj//z8ABAAA////AwAAAPD//z8AAAAA////AwAAAPD//z8AAAAA/P//AwAAAMD//z8AAAAA/P//AwAAAMD//z8AAAAA/P//AgAAAMD//y8AAAAA/P//AgAAAID//z8AAAAA8P//DwAAAAD///8DAAAA8P//DwAAAFT///8AAABA9f//HwAAAFD///8CAAAA8P//PwAAAKD///8LAAAA+P///686APD//////w//////////////////////////////////rsQYGAABAPGq1OLmuTTtl9sAAAr8+fNB6s7sPAP6Ez4qF9YhcBFpAAAAAAAHAADgHwAA4P9/d+D8f3cA4H93AAAAAAAAAAAOAAAADgAAAA4AADj/H/48/x/+PP8f/hwfAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA=",
                FaceFeatureMD5 = "abcasdfasdfadfdgfdgfdg",
                */

                AccessType = 0,
                ExpirationDate = 0,
                OpenTimes = 65535,
                KeepOpen = 1,
                Timegroup = 1,
                Holidays = "1,2,3,4,5,6,7,8,9,10,1,2,3,4,5,6,7,8,9,10,1,2,3,4,5,6,7,8,9,10",
                Elevators = "1,2,3,4,5,6,7,8,9,10,1,2,3,4,5,6,7,8,9,10,1,2,3,4,5,6,7,8,9,10,1,2,3,4,5,6,7,8,9,10,1,2,3,4,5,6,7,8,9,10,1,2,3,4,5,6,7,8,9,10,1,2,3,4,5,6,7,8,9,10,1,2,3,4,5,6,7,8,9,10,1,2,3,4,5,6,7,8,9,10,1,2,3,4,5,6,7,8,9,10,1,2,3,4,5,6,7,8,9,10,1,2,3,4,5,6,7,8,9,10,1,2,3,4,5,6,7,8,9,10,1,2,3,4,5,6,7,8,9,10,1,2,3,4,5,6,7,8,9,10",
            };
            MQTT_Command_UploadPeople cmdPeople = new MQTT_Command_UploadPeople(p);

            string sImage = "G:\\头像\\222.jpg";
            if (File.Exists(sImage))
            {
                byte[] image = File.ReadAllBytes(sImage);


                cmdPeople.SetDataBuf(image);
                p.Detail.PhotoMD5 = MD5Helper.GetByteBufMD5ByHex(image);
            }

            var buf = ToBuffer(cmdPeople, false).Result;

            var parResult = Parse(buf).Result;
            if(parResult == null)
            {
                Console.WriteLine("TestPacket Fail");
                return;
            }
            var packet = parResult.Packet;
            if (packet.Cmd == cmdPeople.Cmd && packet.CmdID == cmdPeople.CmdID && packet.CmdTime == cmdPeople.CmdTime)
            {
                string sSaveFile = "G:\\1.jpg";
                var saveBuf = packet.GetDataBuf();
                if (saveBuf != null)
                {
                    // 创建文件流
                    using (FileStream fileStream = new FileStream(sSaveFile, FileMode.Create, FileAccess.Write, FileShare.None))
                    {
                        // 写入数据
                        fileStream.Write(saveBuf.Array, saveBuf.Offset, saveBuf.Count);
                    }
                    string sPhotoMD5 = MD5Helper.GetByteBufMD5ByHex(saveBuf);

                    if (sPhotoMD5 == p.Detail.PhotoMD5)
                    {
                        Console.WriteLine("TestPacket OK");
                    }
                    else
                        Console.WriteLine("TestPacket iamge error");
                }

            }
            else
            {
                Console.WriteLine("TestPacket Fail");
            }
        }
    }
}
