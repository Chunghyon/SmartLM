using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaceWebServer.DTO.People
{
    /// <summary>
    /// 人员特征码
    /// </summary>
    public class PeopleFeatureCode
    {
        /// <summary>
        /// 特征码索引号
        /// </summary>
        public int Num { get; set; }

        /// <summary>
        /// 指纹特征码文件地址或base64 字符串
        /// </summary>
        public string Data { get; set; }

        /// <summary>
        /// 特征码的MD5值 HEX字符串格式
        /// </summary>
        public string MD5 { get; set; }
    }
}
