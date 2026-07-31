using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaceWebServer.Utility.Extend
{
    public static class ArraySegmentByteExtend
    {
        public static UInt32 ReadInt32_BigEndian(this ArraySegment<byte> dataBuf, ref int offset)
        {
            UInt32 value = BitConverter.ToUInt32(dataBuf.Array, dataBuf.Offset + offset);
            offset += 4;
            return value;
        }
        public static UInt32 ReadInt32(this ArraySegment<byte> dataBuf, ref int offset)
        {
            UInt32 value = dataBuf.Array[dataBuf.Offset + offset];
            value <<= 8;
            value |= dataBuf.Array[dataBuf.Offset + offset + 1];
            value <<= 8;
            value |= dataBuf.Array[dataBuf.Offset + offset + 2];
            value <<= 8;
            value |= dataBuf.Array[dataBuf.Offset + offset + 3];
            offset += 4;
            return value;
        }

        public static ArraySegment<byte> WriteInt32(this ArraySegment<byte> dataBuf, uint value, ref int offset)
        {
            dataBuf.Array[dataBuf.Offset + offset + 3] = (byte)(value % 256);
            value >>= 8;
            dataBuf.Array[dataBuf.Offset + offset + 2] = (byte)(value % 256);
            value >>= 8;
            dataBuf.Array[dataBuf.Offset + offset + 1] = (byte)(value % 256);
            value >>= 8;
            dataBuf.Array[dataBuf.Offset + offset + 0] = (byte)(value % 256);

            offset += 4;
            return dataBuf;
        }
    }
}
