using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaceWebServer.Utility.VerifyAttribute
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = true)]
    public class VerifyNumTextAttribute : VerifyTextAttribute
    {


        /// <summary>
        /// 对数字字符串进行验证
        /// </summary>
        /// <param name="iMax">最大长度</param>
        /// <param name="iMin">最小长度</param>
        /// <param name="required">是否必须的（true 不可为空）</param>
        /// <param name="errCode">验证不通过时需要返回的代码</param>
        /// <param name="sErrorDesc">验证不通过时需要返回的说明</param>
        public VerifyNumTextAttribute(int iMax, int iMin, bool required, 
            int errCode, string sErrorDesc,string lngcode, 
            bool bAutoDef = true, string defValue = "")
            : base(iMax, iMin, required, 
                  errCode, sErrorDesc, lngcode,
                  bAutoDef, defValue)
        {
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
                if (IsRequired)//需要进行非空判断
                {
                    if (string.IsNullOrEmpty(sValue))
                    {
                        return false;
                    }
                }

                if (!string.IsNullOrEmpty(sValue))
                {
                    int iValue = 0;
                    if (!int.TryParse(sValue, out iValue))
                    {
                        return false;//不能转换为数字
                    }

                    if (Max > 0)//需要进行最大长度判断
                    {
                        if (sValue.Length > Max) return false;
                    }
                    if (Min > 0)//需要进行最大长度判断
                    {
                        if (sValue.Length < Min) return false;
                    }
                }
                else
                {
               //     if (AutoDef) oValue = DefValue;
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
