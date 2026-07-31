using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaceWebServer.Utility.VerifyAttribute
{
    /// <summary>
    /// 对数值范围进行验证
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Parameter, AllowMultiple = true)]
    public class VerifyNumAttribute : AbstractVerifyAttribute
    {
        /// <summary>
        /// 是否可为空
        /// </summary>
        public bool IsRequired { get; }
        /// <summary>
        /// 最大长度
        /// </summary>
        public int Max { get; }
        /// <summary>
        /// 最小长度
        /// </summary>
        public int Min { get; }


        /// <summary>
        /// 对数值范围进行验证
        /// </summary>
        /// <param name="iMax">最大值</param>
        /// <param name="iMin">最小值</param>
        /// <param name="errCode">验证不通过时需要返回的代码</param>
        /// <param name="sErrorDesc">验证不通过时需要返回的说明</param>
        public VerifyNumAttribute(int iMax, int iMin, bool required,
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
                if (int.TryParse(oValue.ToString(), out int iValue))
                {
                    return iValue <= Max && iValue >= Min;
                }
            }
            return !IsRequired;
        }
    }
}
