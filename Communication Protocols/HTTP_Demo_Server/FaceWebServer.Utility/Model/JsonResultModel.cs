namespace FaceWebServer.Utility.Model
{
    public class JsonResultModel
    {
        /// <summary>
        /// 结果返回值
        /// </summary>
        public bool Result;
        /// <summary>
        /// 正文
        /// </summary>
        public object Content;

        /// <summary>
        /// 错误代码
        /// </summary>
        public int ErrCode;

        /// <summary>
        /// 错误说明
        /// </summary>
        public string Error;


        public JsonResultModel():this(null)
        {
        }

        public JsonResultModel(object oContent)
        {
            Result = true;
            Content = oContent;
            ErrCode = 0;
            Error = null;
        }

        public JsonResultModel(int iErr, string sErrMsg)
        {
            Result = false;
            Content = null;
            ErrCode = iErr;
            Error = sErrMsg;
        }
    }
}
