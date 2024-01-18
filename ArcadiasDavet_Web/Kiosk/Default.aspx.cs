using Model;
using System;
using System.Linq;
using System.Web.UI;
using VeritabaniIslemMerkezi;

namespace ArcadiasDavet_Web.Kiosk
{
    public partial class Default : Page
    {
        BilgiKontrolMerkezi Kontrol = new BilgiKontrolMerkezi();
        SurecVeriModel<KatilimciTablosuModel> SDataModel;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ImgKiosk.ImageUrl = $"~/Dosyalar/Kiosk/Kiosk.png?t={DateTime.Now.Ticks}";
            }
        }

        protected void lnkbtnKatilimciYakaKartiBas_Click(object sender, EventArgs e)
        {
            SDataModel = new KatilimciTablosuIslemler().KayitBilgisi(hfKatilimciID.Value, "YakaKarti");

            switch (SDataModel.Sonuc)
            {
                case Sonuclar.KismiBasarili:
                case Sonuclar.Basarisiz:
                    BilgiKontrolMerkezi.UyariEkrani(this, $"UyariBilgilendirme('', '<p>Katılımcı bilgileriniz alınırken hata meydana geldi</p><p>Hata mesajı : {SDataModel.HataBilgi.HataMesaji}</p>', false); Durum = false; keypressQRData = '';  UyariKapatma();", false);
                    break;
                default:

                case Sonuclar.VeriBulunamadi:
                    BilgiKontrolMerkezi.UyariEkrani(this, $"UyariBilgilendirme('', '<p>Katılımcı bilginiz bulunamadı. Lütfen kayıt masasından yardım alınız.</p>', false); Durum = false; keypressQRData = '';  UyariKapatma();", false);
                    break;

                case Sonuclar.Basarili:

                    if (SDataModel.Veriler.KatilimciTipiBilgisi.YakaKartiBasimSayisi > SDataModel.Veriler.YakaKartiBasimBilgisi.Count)
                    {
                        BilgiKontrolMerkezi.UyariEkrani(this, $"UyariBilgilendirme('', '<p>Sayın {SDataModel.Veriler.AdSoyad}</p><p>Yaka kartınızı basılmaya başlanıyor. Lütfen aşağıdaki bölmeden yaka kartınızı almayı unutmayınız.</p>', true); Durum = false; keypressQRData = ''; window.open('{ResolveClientUrl($"~/Yazici/YakaKarti/{SDataModel.Veriler.KatilimciID}/{new KullaniciTablosuIslemler().KayitBilgisi(Context).KullaniciID}")}', '_blank', 'width = 10, height = 10, top = 2000, left = 2000'); UyariKapatma();", false);
                    }
                    else
                    {
                        BilgiKontrolMerkezi.UyariEkrani(this, $"UyariBilgilendirme('', '<p>Sayın {SDataModel.Veriler.AdSoyad}</p><p>Yaka kartınızı {SDataModel.Veriler.YakaKartiBasimBilgisi.Last().EklenmeTarihi:dd.MM.yyyy HH:mm} tarihinde aldınız.</p><p>Yeni yaka kartı almak için lütfen kayıt masasına gidiniz</p>', false); Durum = false; keypressQRData = '';  UyariKapatma();", false);
                    }

                    break;
            }

            Kontrol.Temizle(hfKatilimciID);
        }
    }
}