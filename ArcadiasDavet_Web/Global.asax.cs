using Model;
using System;
using System.Web;
using System.Web.Http;
using System.Web.Routing;
using System.Web.Security;
using VeritabaniIslemMerkezi;

namespace ArcadiasDavet_Web
{
    public class WebApiApplication : HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            if (HttpContext.Current.Request.HttpMethod == "OPTIONS")
            {
                HttpContext.Current.Response.AddHeader("Cache-Control", "no-cache");
                HttpContext.Current.Response.AddHeader("Access-Control-Allow-Methods", "GET, POST, PUT, DELETE");
                HttpContext.Current.Response.AddHeader("Access-Control-Allow-Headers", "Content-Type, Accept, Pragma, Cache-Control, Authorization ");
                HttpContext.Current.Response.AddHeader("Access-Control-Max-Age", "1728000");
                HttpContext.Current.Response.End();
            }
        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {
            if (!(User is null))
            {
                if (User.Identity.IsAuthenticated)
                {
                    if (User.Identity.AuthenticationType.Equals("Forms"))
                    {
                        if (!new KullaniciTablosuIslemler().VerifyAuthToken(HttpContext.Current))
                        {
                            FormsAuthentication.SignOut();
                            Response.Cookies[CookiesModel.AuthToken.CookieName].Expires = DateTime.Now.AddSeconds(-1);
                            Response.Cookies[CookiesModel.UserInfo.CookieName].Expires = DateTime.Now.AddSeconds(-1);
                            Response.Redirect("~/", true);
                        }
                    }
                }

            }
        }

    }
}
