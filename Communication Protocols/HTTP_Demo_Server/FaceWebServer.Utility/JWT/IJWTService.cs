using FaceWebServer.DB.Table;

namespace FaceWebServer.Utility.JWT
{
    /// <summary>
    /// 简单封装个注入
    /// </summary>
    public interface IJWTService
    {
        /// <summary>
        /// 用户信息
        /// </summary>
        /// <param name="userInfo"></param>
        /// <returns></returns>
        string GetToken(UserDetail userInfo);
    }
}
