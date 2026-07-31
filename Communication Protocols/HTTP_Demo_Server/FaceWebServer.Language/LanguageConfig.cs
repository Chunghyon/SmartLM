using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace FaceWebServer.Language
{
    /// <summary>
    /// 多语言配置
    /// </summary>
    public class LanguageConfig
    {
        /// <summary>
        /// 语言类型
        /// </summary>
        public string Language { get; set; }
        /// <summary>
        /// 语言文件路径
        /// </summary>
        public string JsonFile { get; set; }
    }
}
