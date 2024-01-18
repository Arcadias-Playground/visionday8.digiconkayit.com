using Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using VeritabaniIslemMerkezi;

namespace ArcadiasDavet_Web
{
    public partial class Default : Page
    {
        BilgiKontrolMerkezi Kontrol = new BilgiKontrolMerkezi();

        SurecBilgiModel SModel;
        SurecVeriModel<KullaniciTablosuModel> SDataModel;

        KullaniciTablosuModel KModel;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (User.Identity.IsAuthenticated)
                {
                    KModel = new KullaniciTablosuIslemler().KayitBilgisi(Context);
                    Response.Redirect($"~/{KModel.KullaniciTipiBilgisi.KullaniciTipi.Replace("ı", "i")}");
                }
            }

        }

        protected void LGGiris_LoggedIn(object sender, EventArgs e)
        {
            SDataModel = new KullaniciTablosuIslemler().KayitBilgisi_ePosta((LVGiris.FindControl("LGGiris").FindControl("UserName") as TextBox).Text);

            SDataModel.Veriler.KullaniciGirisBilgisi = new List<KullaniciGirisTablosuModel>
            {
                new KullaniciGirisTablosuModel
                {
                    KullaniciGirisID = new KullaniciGirisTablosuIslemler().YeniKullaniciGirisID(),
                    KullaniciID = SDataModel.Veriler.KullaniciID,
                    IP = Request.UserHostAddress,
                    Platform = $"{Request.Browser.Browser} / {Request.Browser.Platform}",
                    Mobil = IsMobileOrTabletDevice(Request.ServerVariables["HTTP_USER_AGENT"]),
                    AuthToken = Membership.GeneratePassword(128, 50),
                    EklenmeTarihi = Kontrol.Simdi()
                }
            };

            SModel = new KullaniciGirisTablosuIslemler().YeniKayitEkle(SDataModel.Veriler.KullaniciGirisBilgisi.First());

            if (SModel.Sonuc.Equals(Sonuclar.Basarili))
            {
                Response.Cookies.Add(new HttpCookie(CookiesModel.AuthToken.CookieName)
                {
                    HttpOnly = true,
                    Expires = DateTime.Now.AddDays(1),
                    Value = new SifrelemeIslemleri().Sifrele(SDataModel.Veriler, CookiesModel.AuthToken.Purpose)
                });
                Response.Cookies.Add(new HttpCookie(CookiesModel.UserInfo.CookieName)
                {
                    HttpOnly = true,
                    Expires = DateTime.Now.AddDays(1),
                    Value = new SifrelemeIslemleri().Sifrele(SDataModel.Veriler, CookiesModel.AuthToken.Purpose)
                });
                File.WriteAllText(Server.MapPath($"~/AuthTokens/{SDataModel.Veriler.ePosta}.authtoken"), Response.Cookies[CookiesModel.AuthToken.CookieName].Value);

                Response.Redirect($"~/{SDataModel.Veriler.KullaniciTipiBilgisi.KullaniciTipi.Replace("ı", "i")}");
            }
            else
            {
                FormsAuthentication.SignOut();
                Response.Cookies.Remove(CookiesModel.AuthToken.CookieName);
                Response.Cookies.Remove(CookiesModel.UserInfo.CookieName);
                BilgiKontrolMerkezi.UyariEkrani(this, $"UyariBilgilendirme('Dikkat', '<p>Giriş kaydınız yapılırken hata meydana geldi</p><p>Hata mesajı : {SModel.HataBilgi.HataMesaji}</p>', false);", false);
            }
        }

        protected void LGGiris_LoginError(object sender, EventArgs e)
        {
            BilgiKontrolMerkezi.UyariEkrani(this, "UyariBilgilendirme('Dikkat', '<p>Kullanıcı adınızı ve/veya şifrenizi kontrol ediniz.</p>', false);", false);
        }

        private bool IsMobileOrTabletDevice(string Arg)
        {
            Regex b = new Regex(@"(android|bb\d+|meego).+mobile|avantgo|bada\/|blackberry|blazer|compal|elaine|fennec|hiptop|iemobile|ip(hone|od)|iris|kindle|lge |maemo|midp|mmp|mobile.+firefox|netfront|opera m(ob|in)i|palm( os)?|phone|p(ixi|re)\/|plucker|pocket|psp|series(4|6)0|symbian|treo|up\.(browser|link)|vodafone|wap|windows ce|xda|xiino", RegexOptions.IgnoreCase | RegexOptions.Multiline);
            Regex v = new Regex(@"1207|6310|6590|3gso|4thp|50[1-6]i|770s|802s|a wa|abac|ac(er|oo|s\-)|ai(ko|rn)|al(av|ca|co)|amoi|an(ex|ny|yw)|aptu|ar(ch|go)|as(te|us)|attw|au(di|\-m|r |s )|avan|be(ck|ll|nq)|bi(lb|rd)|bl(ac|az)|br(e|v)w|bumb|bw\-(n|u)|c55\/|capi|ccwa|cdm\-|cell|chtm|cldc|cmd\-|co(mp|nd)|craw|da(it|ll|ng)|dbte|dc\-s|devi|dica|dmob|do(c|p)o|ds(12|\-d)|el(49|ai)|em(l2|ul)|er(ic|k0)|esl8|ez([4-7]0|os|wa|ze)|fetc|fly(\-|_)|g1 u|g560|gene|gf\-5|g\-mo|go(\.w|od)|gr(ad|un)|haie|hcit|hd\-(m|p|t)|hei\-|hi(pt|ta)|hp( i|ip)|hs\-c|ht(c(\-| |_|a|g|p|s|t)|tp)|hu(aw|tc)|i\-(20|go|ma)|i230|iac( |\-|\/)|ibro|idea|ig01|ikom|im1k|inno|ipaq|iris|ja(t|v)a|jbro|jemu|jigs|kddi|keji|kgt( |\/)|klon|kpt |kwc\-|kyo(c|k)|le(no|xi)|lg( g|\/(k|l|u)|50|54|\-[a-w])|libw|lynx|m1\-w|m3ga|m50\/|ma(te|ui|xo)|mc(01|21|ca)|m\-cr|me(rc|ri)|mi(o8|oa|ts)|mmef|mo(01|02|bi|de|do|t(\-| |o|v)|zz)|mt(50|p1|v )|mwbp|mywa|n10[0-2]|n20[2-3]|n30(0|2)|n50(0|2|5)|n7(0(0|1)|10)|ne((c|m)\-|on|tf|wf|wg|wt)|nok(6|i)|nzph|o2im|op(ti|wv)|oran|owg1|p800|pan(a|d|t)|pdxg|pg(13|\-([1-8]|c))|phil|pire|pl(ay|uc)|pn\-2|po(ck|rt|se)|prox|psio|pt\-g|qa\-a|qc(07|12|21|32|60|\-[2-7]|i\-)|qtek|r380|r600|raks|rim9|ro(ve|zo)|s55\/|sa(ge|ma|mm|ms|ny|va)|sc(01|h\-|oo|p\-)|sdk\/|se(c(\-|0|1)|47|mc|nd|ri)|sgh\-|shar|sie(\-|m)|sk\-0|sl(45|id)|sm(al|ar|b3|it|t5)|so(ft|ny)|sp(01|h\-|v\-|v )|sy(01|mb)|t2(18|50)|t6(00|10|18)|ta(gt|lk)|tcl\-|tdg\-|tel(i|m)|tim\-|t\-mo|to(pl|sh)|ts(70|m\-|m3|m5)|tx\-9|up(\.b|g1|si)|utst|v400|v750|veri|vi(rg|te)|vk(40|5[0-3]|\-v)|vm40|voda|vulc|vx(52|53|60|61|70|80|81|83|85|98)|w3c(\-| )|webc|whit|wi(g |nc|nw)|wmlb|wonu|x700|yas\-|your|zeto|zte\-", RegexOptions.IgnoreCase | RegexOptions.Multiline);

            return b.IsMatch(Arg) || v.IsMatch(Arg.Substring(0, 4));
        }
    }
}