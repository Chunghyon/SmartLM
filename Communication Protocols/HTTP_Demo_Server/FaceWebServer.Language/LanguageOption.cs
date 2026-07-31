using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace FaceWebServer.Language
{
    /// <summary>
    /// 多语言选项
    /// </summary>
    public class LanguageOption
    {
        /// <summary>
        /// 所支持的所有语言
        /// </summary>
        public List<LanguageConfig> Languages { get; set; }
        /// <summary>
        /// 默认的语言
        /// </summary>
        public string DefaultLanguage { get; set; }

        /// <summary>
        /// 当前使用的语言
        /// </summary>
        public string CurrentLanguage { get; set; }


        /// <summary>
        /// 获取当前使用的语言的处理器
        /// </summary>
        /// <returns></returns>
        public LanguageHandler GetCurrentLanguageHandler()
        {
            if (string.IsNullOrEmpty(CurrentLanguage))
                CurrentLanguage = DefaultLanguage;
            return LanguageHandler.GetLanguageConfiguration(Languages.Find(x => x.Language == CurrentLanguage));
        }
    }

}
