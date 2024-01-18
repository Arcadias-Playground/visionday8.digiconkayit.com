using System.Web;
using System.Web.Http;
using System.Web.Routing;

namespace ArcadiasDavet_Admin
{
    public class WebApiApplication : HttpApplication
    {
        protected void Application_Start()
        {
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            GlobalConfiguration.Configure(WebApiConfig.Register);
        }
    }
}
