using FaceWebServer.DB.Table;
using FaceWebServer.Interface;
using FaceWebServer.Utility.JWT;
using FaceWebServer.Utility.RSA;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace DeviceProtocolServer.Controllers.Test
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        #region MyRegion
        private ILogger<AuthenticationController> _logger = null;
        private IJWTService _iJWTService = null;
        private IUserService _user;
        public AuthenticationController(ILoggerFactory factory,
            ILogger<AuthenticationController> logger,
            IUserService user, IJWTService service)
        {
            _logger = logger;
            _iJWTService = service;
            _user = user;
        }
        #endregion

        [Route("GetKey")]
        [HttpGet]
        public string GetKey()
        {
            X509Certificate2 clientCertificate = this.HttpContext.Connection.ClientCertificate;
            if (clientCertificate != null)
            {
                Console.WriteLine(clientCertificate.Subject);
            }
            string keyDir = Directory.GetCurrentDirectory();
            if (RSAHelper.TryGetKeyParameters(keyDir, false, out RSAParameters keyParams) == false)
            {
                keyParams = RSAHelper.GenerateAndSaveKey(keyDir, false);
            }

            return JsonConvert.SerializeObject(keyParams);
        }


        [Route("Wait")]
        [HttpGet]
        public async Task<IActionResult> Wait()
        {
            await Task.Delay(20000);

            return new JsonResult(new
            {
                Result = true
            });
        }


        [Route("Login")]
        [HttpPost]
        public string Login(string name, string password)
        {

            {
                //大家自己在开发的时候这里肯定是需要去数据库中验证
            }
            var users = _user.Query<UserDetail>(x => x.UserName == name).ToList();
            if (users.Count != 1)
            {
                return JsonConvert.SerializeObject(new
                {
                    result = false,
                    error = "User not find!"
                });

            }
            var user = users.First();
            if (user.UserName.Equals(name) && user.UserPassword.Equals(password))//应该数据库
            {
                string token = _iJWTService.GetToken(user);
                return JsonConvert.SerializeObject(new
                {
                    result = true,
                    token
                });
            }
            else
            {
                return JsonConvert.SerializeObject(new
                {
                    result = false,
                    token = "User password error!"
                });
            }
        }
    }
}