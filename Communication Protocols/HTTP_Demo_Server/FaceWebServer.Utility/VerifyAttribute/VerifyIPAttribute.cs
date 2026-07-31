using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaceWebServer.Utility.VerifyAttribute
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = true)]
    public class VerifyIP : AbstractVerifyAttribute
    {

        /// <summary>
        /// 对IPv4格式进行验证
        /// </summary>
        /// <param name="iMax">最大长度</param>
        /// <param name="iMin">最小长度</param>
        /// <param name="required">是否必须的（true 不可为空）</param>
        /// <param name="errCode">验证不通过时需要返回的代码</param>
        /// <param name="sErrorDesc">验证不通过时需要返回的说明</param>
        public VerifyIP(int errCode, string sErrorDesc, string lngcode) : base(errCode, sErrorDesc, lngcode) { }

        /// <summary>
        /// 进行参数判断
        /// </summary>
        /// <param name="oValue"></param>
        /// <returns></returns>
        public override bool Verify(ref object oValue)
        {
            try
            {
                if (oValue == null)
                {
                    return true;
                }
                string sValue = (string)oValue;
                string[] sArr = sValue.Split('.');

                if (sArr.Length != 4) return false;
                for (int i = 0; i < 4; i++)
                {
                    int iValue = 0;
                    sValue = sArr[i];
                    if (sValue.Length > 3 || sValue.Length == 0) return false;

                    if (int.TryParse(sArr[i], out iValue))
                    {
                        if (iValue < 0 || iValue > 255)
                        {
                            return false;
                        }
                    }
                    else
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
