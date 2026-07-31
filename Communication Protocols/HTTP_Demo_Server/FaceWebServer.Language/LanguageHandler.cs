using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaceWebServer.Language
{
    public class LanguageHandler
    {
        private static Dictionary<string, LanguageHandler> LanguageDict = new Dictionary<string, LanguageHandler>();

        /// <summary>
        /// 获取一个语言配置文件
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        public static LanguageHandler GetLanguageConfiguration(LanguageConfig config)
        {
            if(LanguageDict.ContainsKey(config.Language))
            {
                return LanguageDict[config.Language];
            }
            lock (LanguageDict)
            {
                if (LanguageDict.ContainsKey(config.Language))
                {
                    return LanguageDict[config.Language];
                }
                if (!File.Exists(config.JsonFile)) return null;
                ConfigurationBuilder builder = new ConfigurationBuilder();
                builder.SetBasePath(Directory.GetCurrentDirectory());
                builder.AddJsonFile(config.JsonFile,false,true);
                var jsonconfig = builder.Build();

                LanguageDict.Add(config.Language, new LanguageHandler(jsonconfig));
            }
            return LanguageDict[config.Language];
        }

        /// <summary>
        /// 语言配置文件
        /// </summary>
        public IConfiguration LanguageJosnConfig {  get; private set; }

        private LanguageHandler(IConfiguration config)
        {
            LanguageJosnConfig= config;
        }

        /// <summary>
        /// 获取系统菜单的语言信息
        /// </summary>
        /// <param name="sCode"></param>
        /// <returns></returns>
        public string GetSystemMenu(string sCode)
        {
           
            return LanguageJosnConfig[$"SystemMenu:{sCode}"];
        }
        /// <summary>
        /// 获取参数检查的错误返回信息
        /// </summary>
        /// <param name="sCode"></param>
        /// <returns></returns>
        public string GetCheckParameterErrorMessage(string sCode)
        {
            return LanguageJosnConfig[$"CheckParameterErrorMessage:{sCode}"];
        }

        /// <summary>
        /// 获取操作员日志信息
        /// </summary>
        /// <param name="sCode"></param>
        /// <returns></returns>
        public string GetUserLog(string sCode)
        {
            return LanguageJosnConfig[$"UserLog:{sCode}"];
        }
        /// <summary>
        /// 获取设备API返回的文字信息
        /// </summary>
        /// <param name="sCode"></param>
        /// <returns></returns>

        public string GetAPIResultMessage(string sCode)
        {
            return LanguageJosnConfig[$"APIResult:{sCode}"];
        }
        /// <summary>
        /// 获取权限服务的文字信息
        /// </summary>
        /// <param name="sCode"></param>
        /// <returns></returns>
        public string GetAccessService(string sCode)
        {
            return LanguageJosnConfig[$"DeviceAccessService:{sCode}"];
        }

        public string GetRemoteService(string sCode)
        {
            return LanguageJosnConfig[$"RemoteService:{sCode}"];
        }
    }
}
