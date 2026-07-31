using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaceWebServer.Utility.VerifyAttribute
{
    /// <summary>
    /// 对字符串进行范围检查，仅在指定参数列表中的值才允许通过
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = true)]
    public class VerifyTextRangeAttribute : AbstractVerifyAttribute
    {
        private HashSet<string> _Range;
        /// <summary>
        /// 对字符串进行验证
        /// </summary>
        /// <param name="sRange">最大长度</param>
        /// <param name="errCode">验证不通过时需要返回的代码</param>
        /// <param name="sErrorDesc">验证不通过时需要返回的说明</param>
        public VerifyTextRangeAttribute(string[] sRange,
            int errCode, string sErrorDesc, string lngcode) : base(errCode, sErrorDesc, lngcode)
        {
            _Range = new HashSet<string>(sRange);
        }

        /// <summary>
        /// 进行参数判断
        /// </summary>
        /// <param name="oValue"></param>
        /// <returns></returns>
        public override bool Verify(ref object oValue)
        {
            try
            {
                string sValue = (string)oValue;

                if (string.IsNullOrEmpty(sValue))
                {
                    return false;
                }

                return _Range.Contains(sValue);
            }
            catch (Exception)
            {
                return false;
            }

        }
    }
}
