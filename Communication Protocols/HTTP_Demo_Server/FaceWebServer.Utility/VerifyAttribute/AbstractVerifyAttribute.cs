using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaceWebServer.Utility.VerifyAttribute
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = true)]
    public abstract class AbstractVerifyAttribute : Attribute
    {
        /// <summary>
        /// 错误代码
        /// </summary>
        public int ErrorCode { get; }
        /// <summary>
        /// 错误说明
        /// </summary>
        public string ErrorDescription { get; }

        /// <summary>
        /// 多语言代码，根据此代码查表找到多语言文字
        /// </summary>
        public string LanguageCode { get; }

        /// <summary>
        /// 参数验证
        /// </summary>
        /// <param name="errCode">验证不通过时需要返回的代码</param>
        /// <param name="sErrorDesc">验证不通过时需要返回的说明</param>
        public AbstractVerifyAttribute(int errCode, string sErrorDesc, string languageCode)
        {
            ErrorCode = errCode;
            ErrorDescription = sErrorDesc;
            LanguageCode = languageCode;
        }

        public abstract bool Verify(ref object oValue);
    }
}
