using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaceWebServer.DTO.People
{
    /// <summary>
    /// 注册人员识别凭证接口参数
    /// </summary>
    public class EnrollUserMediaDataDTO
    {

        public const int ENROLL_CARD = 1;
        public const int ENROLL_PWD = 2;
        public const int ENROLL_FP = 3;
        public const int ENROLL_FACE = 4;
        public const int ENROLL_PALM = 5;


        public long UserID { get; set; }
        public string EnrollType { get; set; }

        public int EnrollIndex { get; set; }

        public int DeviceID { get; set; }

        public int EnrollTypeToInt()
        {
            if (EnrollType == "Face")
            {
                return ENROLL_FACE;
            }
            if (EnrollType == "Card")
            {
                return ENROLL_CARD;
            }
            if (EnrollType == "PIN")
            {
                return ENROLL_PWD;
            }

            if (EnrollType == "FP")
            {
                return ENROLL_FP;
            }

            if (EnrollType == "Palm")
            {
                return ENROLL_PALM;
            }

            return ENROLL_FACE;
        }
    }



}
