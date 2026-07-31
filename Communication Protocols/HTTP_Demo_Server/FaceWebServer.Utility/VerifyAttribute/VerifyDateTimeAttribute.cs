using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaceWebServer.Utility.VerifyAttribute
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = true)]
    public class VerifyDateTimeAttribute : AbstractVerifyAttribute
    {
        /// <summary>
        /// 是否可为空
        /// </summary>
        public bool IsRequired { get; }


        /// <summary>
        /// 对字符串进行验证
        /// </summary>
        /// <param name="required">是否必须的（true 不可为空）</param>
        /// <param name="errCode">验证不通过时需要返回的代码</param>
        /// <param name="sErrorDesc">验证不通过时需要返回的说明</param>
        public VerifyDateTimeAttribute(bool required, 
            int errCode, string sErrorDesc,string lngcode) : base(errCode, sErrorDesc,lngcode)
        {
            IsRequired = required;
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
                DateTime sValue = (DateTime)oValue;
                if (IsRequired)//需要进行非空判断
                {
                    if (sValue == DateTime.MinValue)
                    {
                        return false;
                    }
                }


                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }
    }
}
