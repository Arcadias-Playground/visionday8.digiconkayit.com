using Microsoft.AspNet.SignalR;
using Owin;

namespace VeritabaniIslemMerkezi
{
    public class ArcadiasDavet_Web_Owin
    {
        public void Configuration(IAppBuilder app)
        {
            app.Map("/ArcadiasDavet/Yazici", map =>
            {
                var hubConfiguration = new HubConfiguration
                {
                    EnableDetailedErrors = true,
                    EnableJavaScriptProxies = true
                };
                map.RunSignalR(hubConfiguration);
            });
        }
    }
}