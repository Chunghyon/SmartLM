using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaceWebServer.Utility.VerifyAttribute
{
    /// <summary>
    /// 对数值选项进行验证
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = true)]
    public class VerifyNumRangeAttribute : AbstractVerifyAttribute
    {

        /// <summary>
        /// 数值可选项
        /// </summary>
        private HashSet<int> _Range;

        /// <summary>
        /// 是否可为空
        /// </summary>
        public bool IsRequired { get; }
        /// <summary>
        /// 对数值范围进行验证
        /// </summary>
        /// <param name="iMax">最大值</param>
        /// <param name="iMin">最小值</param>
        /// <param name="errCode">验证不通过时需要返回的代码</param>
        /// <param name="sErrorDesc">验证不通过时需要返回的说明</param>
        public VerifyNumRangeAttribute(int[] iRange, bool required,
            int errCode, string sErrorDesc, string lngcode) : base(errCode, sErrorDesc, lngcode)
        {
            _Range = new HashSet<int>(iRange);
            IsRequired = required;
        }

        /// <summary>
        /// 进行参数判断
        /// </summary>
        /// <param name="oValue"></param>
        /// <returns></returns>
        public override bool Verify(ref object oValue)
        {
            if (oValue != null)
            {
                if (int.TryParse(oValue.ToString(), out var iValue))
                {
                    return _Range.Contains(iValue);
                }
            }
            return !IsRequired;
        }
    }
}
