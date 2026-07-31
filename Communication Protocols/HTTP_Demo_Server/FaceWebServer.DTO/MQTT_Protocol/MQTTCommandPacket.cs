using FaceWebServer.Utility.Extend;
using Newtonsoft.Json.Linq;
using System;
using System.Diagnostics.Metrics;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace FaceWebServer.DTO.MQTT_Protocol
{
    public class MQTTCommandPacket
    {
        /// <summary>
        /// 命令类型
        /// </summary>
        public string Cmd { get; set; }

        /// <summary>
        /// unix时间戳，表示命令发出的时间
        /// </summary>
        public UInt32 CmdTime { get; set; }

        /// <summary>
        /// 命令ID，由发生方生成，一般使用自增id或uuid
        /// </summary>
        public string? CmdID { get; set; }

        private static UInt64 MessageID = 0;

        public void CreateToken()
        {
            CmdTime = (UInt32)DateTimeOffset.Now.ToUnixTimeSeconds();
            var id = Interlocked.Increment(ref MessageID);
            CmdID = id.ToString();
        }

        public void SetToken(string id)
        {
            CmdTime = (UInt32)DateTimeOffset.Now.ToUnixTimeSeconds();
            CmdID = id;
        }

        /// <summary>
        /// 设置附加数据到命令中
        /// </summary>
        /// <param name="dataBuf"></param>
        public virtual void SetDataBuf(ArraySegment<byte> dataBuf)
        {
            //有需要附加数据的命令，需要重写此方法
            //DataBuf = dataBuf;
        }

        /// <summary>
        /// 从命令中获取附加数据，如果没有附加数据，返回null
        /// </summary>
        /// <returns></returns>
        public virtual ArraySegment<byte> GetDataBuf()
        {
            return null;
        }

        public virtual async Task<string> GetBodyJson(MQTTCommandPacketParseResult packetDetail)
        {

            var sJson = Newtonsoft.Json.JsonConvert.SerializeObject(packetDetail.Packet, Newtonsoft.Json.Formatting.Indented); ;
            return Newtonsoft.Json.JsonConvert.SerializeObject(new
            {
                packetDetail.GzipLength,
                packetDetail.JsonLength,
                Body = sJson
            });
        }
    }

    public class MQTTCommandPacket<T> : MQTTCommandPacket
        where T : class
    {
        /// <summary>
        /// 命令的主体参数，具体由命令定义
        /// </summary>
        public T Body { get; set; }

        public override async Task<string> GetBodyJson(MQTTCommandPacketParseResult packetDetail)
        {
            if (Body == null)
            {
                return string.Empty;
            }


            var sJson = Newtonsoft.Json.JsonConvert.SerializeObject(packetDetail.Packet, Newtonsoft.Json.Formatting.Indented); 
            return Newtonsoft.Json.JsonConvert.SerializeObject(new
            {
                packetDetail.GzipLength,
                packetDetail.JsonLength,
                packetDetail.FileDataSize,
                Body = sJson
            });

        }
    }
}
