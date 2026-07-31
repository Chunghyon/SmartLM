using FaceWebServer.DTO.HTTPv2_Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaceWebServer.DTO.MQTT_Protocol.Command.Device
{
    /// <summary>
    /// MQTT 设备上传打卡记录 ，由设备发送
    /// </summary>
    public class MQTT_Command_UploadIdentifyRecord : MQTTCommandPacket<MQTT_UploadIdentifyRecord>
    {
        /// <summary>
        /// 记录照片
        /// </summary>
        private ArraySegment<byte> RecordImage = null;


        public MQTT_Command_UploadIdentifyRecord()
        {
            Cmd = MQTT_Command_Define.UploadIdentifyRecord;

        }

        public MQTT_Command_UploadIdentifyRecord(MQTT_UploadIdentifyRecord data)
        {
            Cmd = MQTT_Command_Define.UploadIdentifyRecord;
            Body = data;

        }

        /// <summary>
        /// 设置附加数据到命令中
        /// </summary>
        /// <param name="dataBuf"></param>
        public override void SetDataBuf(ArraySegment<byte> dataBuf)
        {
            //有需要附加数据的命令，需要重写此方法
            RecordImage = dataBuf;
        }

        /// <summary>
        /// 从命令中获取附加数据，如果没有附加数据，返回null
        /// </summary>
        /// <returns></returns>
        public override ArraySegment<byte> GetDataBuf()
        {
            return RecordImage;
        }
    }

    /// <summary>
    /// 设备主动推送的打卡记录
    /// </summary>
    public class MQTT_UploadIdentifyRecord : HTTPRecordUploadIdentifyRecordDetail
    {
    }
}
