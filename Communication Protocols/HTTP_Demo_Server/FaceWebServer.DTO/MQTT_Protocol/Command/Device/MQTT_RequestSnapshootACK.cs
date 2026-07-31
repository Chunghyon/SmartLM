using FaceWebServer.DTO.HTTPv2_Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaceWebServer.DTO.MQTT_Protocol.Command.Device
{
    /// <summary>
    /// 现场快照返回值
    /// </summary>
    public class MQTT_Command_RequestSnapshootResult
    {
        public string Photo { get; set; }
        public long PhotoSize { get; set; }
    }

    /// <summary>
    /// MQTT 设备摄像头快照  由设备发送
    /// </summary>
    public class MQTT_Command_RequestSnapshootACK : MQTTCommandPacket<MQTT_Command_RequestSnapshootResult>
    {
        /// <summary>
        /// 设备摄像头快照
        /// </summary>
        private ArraySegment<byte> SnapshootImage = null;

        public MQTT_Command_RequestSnapshootACK()
        {
            Cmd = MQTT_Command_Define.RequestSnapshootACK;

        }



        /// <summary>
        /// 设置附加数据到命令中
        /// </summary>
        /// <param name="dataBuf"></param>
        public override void SetDataBuf(ArraySegment<byte> dataBuf)
        {
            //有需要附加数据的命令，需要重写此方法
            SnapshootImage = dataBuf;
        }

        /// <summary>
        /// 从命令中获取附加数据，如果没有附加数据，返回null
        /// </summary>
        /// <returns></returns>
        public override ArraySegment<byte> GetDataBuf()
        {
            return SnapshootImage;
        }
    }

}
