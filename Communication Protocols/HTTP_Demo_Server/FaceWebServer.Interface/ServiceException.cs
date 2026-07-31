using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaceWebServer.Interface
{
    public class ServiceException: Exception
    {
        public int ErrorCode;
        public ServiceException(int code,string msg):base(msg)
        {
            ErrorCode = code;
        }
    }
}
