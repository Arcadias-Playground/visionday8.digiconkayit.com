using System.Configuration;

namespace Model
{
    public sealed class CookiesModel
    {
        public sealed class UserInfo
        {
            public static string[] Purpose = new string[] { "UserInfo" };
            public static string CookieName = ConfigurationManager.AppSettings["UserInfo"];
        }

        public sealed class AuthToken
        {
            public static string[] Purpose = new string[] { "AuthToken" };
            public static string CookieName = ConfigurationManager.AppSettings["AuthToken"];
        }
    }
}