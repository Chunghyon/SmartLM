using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaceWebServer.Utility.VerifyAttribute
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = true)]
    public class VerifyTextAttribute : AbstractVerifyAttribute
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
        /// 自动设置默认值
        /// </summary>
        public bool AutoDef { get; }

        /// <summary>
        /// 自动设置默认值
        /// </summary>
        public string DefValue { get; }

        /// <summary>
        /// 对字符串进行验证
        /// </summary>
        /// <param name="iMax">最大长度</param>
        /// <param name="iMin">最小长度</param>
        /// <param name="required">是否必须的（true 不可为空）</param>
        /// <param name="errCode">验证不通过时需要返回的代码</param>
        /// <param name="sErrorDesc">验证不通过时需要返回的说明</param>
        public VerifyTextAttribute(int iMax, int iMin, bool required, 
            int errCode, string sErrorDesc,string lngcode, bool bAutoDef = true, string defValue ="") 
            : base(errCode, sErrorDesc, lngcode)
        {
            IsRequired = required;
            Max = iMax;
            Min = iMin;
            this.DefValue = defValue;
            this.AutoDef = bAutoDef;
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
                  //  if (AutoDef) oValue = DefValue;
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
