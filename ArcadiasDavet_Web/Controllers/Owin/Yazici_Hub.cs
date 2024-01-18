using Microsoft.AspNet.SignalR;
using System.IO;
using System.Threading.Tasks;

namespace ArcadiasDavet_Web.Controllers.Owin
{
    [Authorize]
    public class Yazici_Hub : Hub
    {

        public void YakaKartiBasmaTalebi(string ePosta, string KatilimciID, string KullaniciID)
        {
            var Info = new { KatilimciID = KatilimciID, KullaniciID = KullaniciID };
            Clients.User(ePosta).yakaKartiBasmaTalebi(Info);
        }

        public override Task OnConnected()
        {
            if (Context.User.IsInRole("Yazıcı") && File.Exists(Path.Combine(System.Web.Hosting.HostingEnvironment.MapPath("~/"), "Dosyalar", "YaziciDurum", $"{Context.User.Identity.Name}.status")))
                File.WriteAllText(Path.Combine(System.Web.Hosting.HostingEnvironment.MapPath("~/"), "Dosyalar", "YaziciDurum", $"{Context.User.Identity.Name}.status"), "true");


            return base.OnConnected();
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            if (Context.User.IsInRole("Yazıcı") && File.Exists(Path.Combine(System.Web.Hosting.HostingEnvironment.MapPath("~/"), "Dosyalar", "YaziciDurum", $"{Context.User.Identity.Name}.status")))
                File.WriteAllText(Path.Combine(System.Web.Hosting.HostingEnvironment.MapPath("~/"), "Dosyalar", "YaziciDurum", $"{Context.User.Identity.Name}.status"), "false");

            return base.OnDisconnected(stopCalled);
        }

        public override Task OnReconnected()
        {
            if (Context.User.IsInRole("Yazıcı") && File.Exists(Path.Combine(System.Web.Hosting.HostingEnvironment.MapPath("~/"), "Dosyalar", "YaziciDurum", $"{Context.User.Identity.Name}.status")))
                File.WriteAllText(Path.Combine(System.Web.Hosting.HostingEnvironment.MapPath("~/"), "Dosyalar", "YaziciDurum", $"{Context.User.Identity.Name}.status"), "true");

            return base.OnReconnected();
        }
    }
}