using FaceWebServer.DB.Table;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace FaceWebServer.Utility.JWT
{

    /// <summary>
    /// 非对称可逆加密  生成TOken
    /// </summary>
    public class JWTRSService : IJWTService
    {
        #region Option注入
        private readonly JWTTokenOptions _JWTTokenOptions;
        public JWTRSService(IOptionsMonitor<JWTTokenOptions> jwtTokenOptions)
        {
            this._JWTTokenOptions = jwtTokenOptions.CurrentValue;
        }
        #endregion



        public string GetToken(UserDetail userModel)
        {
            var claims = new[]
            {
                   new Claim("name", userModel.UserName),
                   new Claim("role", userModel.Role),
                   new Claim("iat",TimestampUtility.ToUnixTimestampBySeconds(userModel.LogTime).ToString()),
                   new Claim("ID", userModel.UserID.ToString())
            };
            /**
 *  Claims (Payload)
    Claims 部分包含了一些跟这个 token 有关的重要信息。 JWT 标准规定了一些字段，下面节选一些字段:

    iss: The issuer of the token，token 是给谁的
    sub: The subject of the token，token 主题
    exp: Expiration Time。 token 过期时间，Unix 时间戳格式
    iat: Issued At。 token 创建时间， Unix 时间戳格式
    jti: JWT ID。针对当前 token 的唯一标识
    除了规定的字段外，可以包含其他任何 JSON 兼容的字段。
 * */
            var credentials = new SigningCredentials(new X509SecurityKey(JWTTokenOptions.X509), SecurityAlgorithms.RsaSha256Signature);



            var token = new JwtSecurityToken(
               issuer: this._JWTTokenOptions.Issuer,
               audience: this._JWTTokenOptions.Audience,
               claims: claims,
               expires: DateTime.Now.AddMinutes(60),//60分钟有效期
               signingCredentials: credentials);

            var handler = new JwtSecurityTokenHandler();
            string tokenString = handler.WriteToken(token);
            return tokenString;
        }
    }
}
