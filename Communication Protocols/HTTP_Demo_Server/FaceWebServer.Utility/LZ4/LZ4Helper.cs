using K4os.Compression.LZ4;
using System;
using System.IO;

namespace FaceWebServer.Utility.LZ4
{
    public static class LZ4Helper
    {
        private static readonly BufferWriterPool<byte> _pool = new BufferWriterPool<byte>();



        /// <summary>
        /// 使用LZ4 进行压缩
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="input"></param>
        /// <returns></returns>
        public static byte[] LZ4_Encode(byte[] input)
        {

            var br2 = _pool.Rent();
            LZ4Pickler.Pickle(new ReadOnlySpan<byte>(input), br2);

            var outBuf = br2.WrittenSpan.ToArray();
            _pool.Return(br2);
            return outBuf;
        }

        /// <summary>
        /// 使用LZ4 进行压缩
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="input"></param>
        /// <returns></returns>
        public static byte[] LZ4_Encode(ReadOnlySpan<byte> input)
        {

            var br2 = _pool.Rent();
            LZ4Pickler.Pickle(input, br2);

            var outBuf = br2.WrittenSpan.ToArray();
            _pool.Return(br2);
            return outBuf;
        }


        /// <summary>
        /// 使用LZ4进行解压缩
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="input"></param>
        /// <returns></returns>
        public static byte[] LZ4_Decode(byte[] input)
        {
            var br = _pool.Rent();
            LZ4Pickler.Unpickle(new ReadOnlySpan<byte>(input), br);
            var outBuf = br.WrittenSpan.ToArray();
            _pool.Return(br);

            return outBuf;
        }

        /// <summary>
        /// 使用LZ4进行解压缩
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="input"></param>
        /// <returns></returns>
        public static byte[] LZ4_Decode(ReadOnlySpan<byte> input)
        {
            var br = _pool.Rent();
            LZ4Pickler.Unpickle(input, br);
            var outBuf = br.WrittenSpan.ToArray();
            _pool.Return(br);

            return outBuf;
        }

    }
}
