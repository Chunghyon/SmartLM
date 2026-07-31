using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaceWebServer.Utility.VerifyAttribute
{
    /// <summary>
    /// 对浮点数值范围进行验证
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = true)]
    public class VerifyFloatAttribute : AbstractVerifyAttribute
    {

        /// <summary>
        /// 最大长度
        /// </summary>
        public float Max { get; }
        /// <summary>
        /// 最小长度
        /// </summary>
        public float Min { get; }

        public bool IsRequired { get; }

        /// <summary>
        /// 对数值范围进行验证
        /// </summary>
        /// <param name="iMax">最大值</param>
        /// <param name="iMin">最小值</param>
        /// <param name="errCode">验证不通过时需要返回的代码</param>
        /// <param name="sErrorDesc">验证不通过时需要返回的说明</param>
        public VerifyFloatAttribute(float iMax, float iMin, bool required,
            int errCode, string sErrorDesc, string lngcode) : base(errCode, sErrorDesc, lngcode)
        {
            Max = iMax;
            Min = iMin;
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
                if (float.TryParse(oValue.ToString(), out var iValue))
                {
                    return iValue <= Max && iValue >= Min;
                }
            }
            return !IsRequired;

        }
    }
}
