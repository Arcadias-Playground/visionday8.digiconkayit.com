using Microsoft.AspNet.FriendlyUrls;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using VeritabaniIslemMerkezi;

namespace ArcadiasDavet_Web.Katilimci
{
    public partial class Misafir : Page
    {
        IList<string> segments;

        StringBuilder Uyarilar = new StringBuilder();
        BilgiKontrolMerkezi Kontrol = new BilgiKontrolMerkezi();

        SurecBilgiModel SModel, SePostaModel, SSmsModel;
        SurecVeriModel<KatilimciTablosuModel> SDataModel;

        KatilimciTablosuModel KModel;

        public string style = "<style>html { background: url({0}); }</style>";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                segments = Request.GetFriendlyUrlSegments();

                if (segments.Count.Equals(1))
                {
                    SDataModel = new KatilimciTablosuIslemler().KayitBilgisi(segments.First());

                    if (SDataModel.Sonuc.Equals(Sonuclar.Basarili) && SDataModel.Veriler.YoneticiOnay && SDataModel.Veriler.KatilimciOnay && string.IsNullOrEmpty(SDataModel.Veriler.AnaKatilimciID))
                    {
                        if (new KatilimciTablosuIslemler().MisafirKontrol(SDataModel.Veriler.KatilimciID))
                        {
                            lblAdSoyad.Text = SDataModel.Veriler.AdSoyad;
                            txtKatilimciID.Text = SDataModel.Veriler.KatilimciID;
                            ddlKatilimciTipi.DataBind();
                            ddlKatilimciTipi.SelectedValue = SDataModel.Veriler.KatilimciTipiID;
                            txtePosta.Text = SDataModel.Veriler.ePosta;
                            txtTelefon.Text = SDataModel.Veriler.Telefon;

                            txtePosta.Enabled = !string.IsNullOrEmpty(SDataModel.Veriler.ePosta);
                            txtTelefon.Enabled = !string.IsNullOrEmpty(SDataModel.Veriler.Telefon);

                            ImgLogo.ImageUrl = $"~/Dosyalar/Logo/{SDataModel.Veriler.KatilimciTipiID}.png?t={DateTime.Now.Ticks}";
                            style = style.Replace("{0}", ResolveClientUrl($"~/Dosyalar/KarsilamaEkrani/{SDataModel.Veriler.KatilimciTipiBilgisi.KatilimciTipiID}.jpg?t={DateTime.Now.Ticks}"));
                        }
                        else
                        {
                            BilgiKontrolMerkezi.UyariEkrani(this, $"UyariBilgilendirme('', '<p>Misafir hakkınız dolmuştur. Lütfen pencereyi kapatınız.</p>', false);", true);
                        }
                    }
                    else
                    {
                        BilgiKontrolMerkezi.UyariEkrani(this, $"UyariBilgilendirme('', '<p>Katılımcı bulunamadı</p>', false);", true);
                    }
                }
                else
                {
                    BilgiKontrolMerkezi.UyariEkrani(this, $"UyariBilgilendirme('', '<p>Katılımcı bulunamadı</p>', false);", true);
                }
            }
        }

        protected void lnkbtnMisafirEkle_Click(object sender, EventArgs e)
        {
            KModel = new KatilimciTablosuModel
            {
                KatilimciID = new KatilimciTablosuIslemler().YeniKatilimciID(),
                KatilimciTipiID = ddlKatilimciTipi.SelectedValue,
                AdSoyad = Kontrol.KelimeKontrol(txtAdSoyad, "Ad & Soyad boş bırakılamaz", ref Uyarilar),
                ePosta = txtePosta.Enabled ? Kontrol.ePostaKontrol(txtePosta, "e-Posta boş bırakılamaz", "Geçerisiz e-Posta adresi girildi", ref Uyarilar) : string.Empty,
                Telefon = txtTelefon.Enabled ? Kontrol.TelefonKontrol(txtTelefon.Text, "Telefon boş bırakılamaz", "Geçerisiz telefon girildi", ref Uyarilar) : string.Empty,
                Unvan = txtUnvan.Text,
                Kurum = txtKurum.Text,
                YoneticiOnay = true,
                YoneticiOnayTarihi = Kontrol.Simdi(),
                KatilimciOnay = true,
                KatilimciOnayTarihi = Kontrol.Simdi(),
                AnaKatilimciID = txtKatilimciID.Text,
                GuncellenmeTarihi = Kontrol.Simdi(),
                EklenmeTarihi = Kontrol.Simdi()
            };

            if (string.IsNullOrEmpty(Uyarilar.ToString()))
            {
                SModel = new KatilimciTablosuIslemler().YeniKayitEkle(KModel);
                if (SModel.Sonuc.Equals(Sonuclar.Basarili))
                {
                    if (txtePosta.Enabled)
                        SePostaModel = new MailGonderimIslemleri().MailGonderim(new KatilimciTablosuIslemler().KayitBilgisi(KModel.KatilimciID, "email", 2).Veriler);


                    if (txtTelefon.Enabled)
                        SSmsModel = new SmsGonderimIslemleri().SmsGonderim(new KatilimciTablosuIslemler().KayitBilgisi(KModel.KatilimciID, "sms", 2).Veriler);


                    if (txtePosta.Enabled && !(SePostaModel is null))
                        Response.Redirect($"~/Katilimci/Onay/email/{SePostaModel.YeniKayitID}", true);
                    else if (txtTelefon.Enabled && !(SSmsModel is null))
                        Response.Redirect($"~/Katilimci/Onay/sms/{SSmsModel.YeniKayitID}", true);
                    else
                        BilgiKontrolMerkezi.UyariEkrani(this, $"UyariBilgilendirme('', '<p>Sayın {KModel.AdSoyad}</p><p>Misafir olarak katılımcı kaydınız başarıyla tamamlanmıştır.</p><p>e-Posta yada Cep Telefonu girmediğiniz için giriş kartınızı tarafınıza iletemedik.</p><p>Bu sebeple lütfen etkinlik sekreteryası ile iletişime geçiniz.</p>');", false);
                }
                else
                {
                    BilgiKontrolMerkezi.UyariEkrani(this, $"UyariBilgilendirme('', '<p>Misafir kaydı sırasında hata meydana geldi</p><p>Hata mesajı : {SModel.HataBilgi.HataMesaji}</p>', false);", false);
                }
            }
            else
            {
                BilgiKontrolMerkezi.UyariEkrani(this, $"UyariBilgilendirme('', '{Uyarilar}', false);", false);
            }
        }
    }
}