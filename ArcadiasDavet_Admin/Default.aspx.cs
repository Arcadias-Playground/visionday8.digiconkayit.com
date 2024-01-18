using System;
using System.Web.UI;
using VeritabaniIslemMerkezi;

namespace ArcadiasDavet_Admin
{
    public partial class Default : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (User.Identity.IsAuthenticated)
            {
                Response.Redirect("~/Yonetim");
            }
        }

        protected void LGGiris_LoggedIn(object sender, EventArgs e)
        {
            Response.Redirect("~/Yonetim");
        }

        protected void LGGiris_LoginError(object sender, EventArgs e)
        {
            BilgiKontrolMerkezi.UyariEkrani(this, "UyariBilgilendirme('Dikkat', '<p>Kullanıcı adınızı ve/veya şifrenizi kontrol ediniz.</p>', false);", false);
        }
    }
}