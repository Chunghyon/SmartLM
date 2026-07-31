using System.Security.Cryptography.X509Certificates;

namespace FaceWebServer.Utility.JWT
{
    public class JWTTokenOptions
    {
        public static X509Certificate2 X509;

        public string Audience
        {
            get;
            set;
        }
        public string SecurityKey
        {
            get;
            set;
        }
        //public SigningCredentials Credentials
        //{
        //    get;
        //    set;
        //}
        public string Issuer
        {
            get;
            set;
        }
    }
}
