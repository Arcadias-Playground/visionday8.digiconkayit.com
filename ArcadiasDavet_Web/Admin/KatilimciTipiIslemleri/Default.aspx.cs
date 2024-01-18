using Model;
using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.IO;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using VeritabaniIslemMerkezi;
using VeritabaniIslemMerkezi.Access;

namespace ArcadiasDavet_Web.Admin.KatilimciTipiIslemleri
{
    public partial class Default : Page
    {
        StringBuilder Uyarilar = new StringBuilder();
        BilgiKontrolMerkezi Kontrol = new BilgiKontrolMerkezi();

        SurecBilgiModel SModel;
        SurecVeriModel<KatilimciTipiTablosuModel> SDataModel;
        SurecVeriModel<MailIcerikTablosuModel> SDataMailIcerikModel;
        SurecVeriModel<SmsIcerikTablosuModel> SDataSmsIcerikModel;
        SurecVeriModel<AntetliKagitIcerikTablosuModel> SDataAntetliKagitIcerikModel;
        SurecVeriModel<YakaKartiCerceveTablosuModel> SDataYakaKartiCerceveModel;
        SurecVeriModel<YakaKartiIcerikTablosuModel> SDataYakaKartiIcerikModel;


        KatilimciTipiTablosuModel KModel;
        SmsIcerikTablosuModel SIModel, SIQModel;
        MailIcerikTablosuModel MIModel, MIQModel;
        AntetliKagitIcerikTablosuModel AIModel;
        YakaKartiCerceveTablosuModel YKCModel;
        YakaKartiIcerikTablosuModel YKIModel;

        const string
            SmsIcerik_Davet = "Sayın {AdSoyad},\r\n\r\nArcadias Tech'in bu seneki yılbaşı kutlamaları için sizi aramızda göremeyi çok istiyoruz.\r\n\r\nDavete katıldığınızı bildirmek için : {KabulLinki}\r\n\r\nDavete katılmayacağınızı bildirmek için {RedLinki}",
            SmsIcerik_QR = "Sayın {AdSoyad},\r\n\r\nArcadias Tech'in bu seneki yılbaşı kutlamalarında sizi aramızda görmek bizleri çok mutlu etti.\r\n\r\nQR kodunuza ulaşmak için : {KabulLinki}",
            MailIcerik_Davet = "<html lang='tr'><head><meta http-equiv='Content-Type' content='text/html; charset=utf-8'></head><body><p>Sayın {AdSoyad}</p><p><b>Bu davetiye iki kişiliktir. Lütfen size eşlik edecek olan kişi için de kayıt yaptırınız.</b>Davetiye kişiye özeldir ve başkasına devredilemez.</p><p>”Katılıyorum” butonuna tıkladığınızda, QR kodunuzu ve refakatçi kaydı için bir bağlantı içeren bir e-posta alacaksınız. Refakatçi kaydını tamamladıktan sonra tarafınıza, refakatçiniz için ikinci bir QR kodu gönderilecektir. Lütfen etkinliğe gelirken QR kodlarınızı ve fotoğraflı kimlik belgelerinizi yanınızda bulundurunuz.</p></body></html>",
            MailIcerik_QR = "<html><head><meta http-equiv='Content-Type' content='text/html; charset=utf-8'></head><body><p>Sayın {AdSoyad}</p><p>Arcadias Tech yılbaşı kutlamaları için QR kodunuz ektedir.</p><p>Etkinlik günü : 31 Aralık</p><ul><li>COVID-19 belirtileri gösteriyorsanız etkinliğe katılmamanızı rica ederiz.</li><li>Araç park yeri bulunmamaktadır.</li></ul></body></html>",
            KabulEkranIcerik = "<p><b>Perşembe, 29. Eylül 2022, 19.00 – 22.30 saatleri arasında</b></p><p>Girişte kimlik kartı ve isminize özel QR kod sorulacaktır.</p><p><b>Adres:</b> Fulya mah. İnci Apt. No:27 D:7 Mecidiyeköy / İstanbul</p><p><b>Etkinlik giriş:</b> 18.00, <b>Etkinliğin başlama saati:</b> 19.00</p> <ul><li>COVID-19 belirtileri gösteriyorsanız etkinliğe katılmamanızı rica ederiz.</li><li>Araç park yeri bulunmamaktadır.</li></ul>",
            RedEkranIcerik = "<p>Geri bildiriminiz için teşekkür ederiz.</p>";

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void lnkbtnYeniKatilimciTipiEkle_Click(object sender, EventArgs e)
        {
            Kontrol.Temizle(txtKatilimciTipiID);
            Kontrol.Temizle(txtKatilimciTipi);
            Kontrol.Temizle(txtKontenjan);
            Kontrol.Temizle(txtMisafirKontenjan);
            Kontrol.Temizle(txtGirisSayisi);
            Kontrol.Temizle(txtYakaKartiBasimSayisi);

            UPnlKatilimciTipiEkleGuncelle.Update();
            BilgiKontrolMerkezi.UyariEkrani(this, $"$({UPnlKatilimciTipiEkleGuncelle.ClientID}).modal('show');", false);
        }

        protected void rptKatilimciTipiListesi_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "Guncelle":
                    SDataModel = new KatilimciTipiTablosuIslemler().KayitBilgisi(e.CommandArgument.ToString());
                    switch (SDataModel.Sonuc)
                    {
                        case Sonuclar.KismiBasarili:
                        case Sonuclar.Basarisiz:
                            BilgiKontrolMerkezi.UyariEkrani(this, $"UyariBilgilendirme('', '<p>Katılımcı tipi bilgisi alınırken hata meydana geldi</p><p>Hata mesajı : {SDataModel.HataBilgi.HataMesaji}</p>', false);", false);
                            break;

                        default:
                        case Sonuclar.VeriBulunamadi:
                            BilgiKontrolMerkezi.UyariEkrani(this, $"UyariBilgilendirme('', '<p>Katılımcı tipi bulunamadı</p>', false);", false);
                            rptKatilimciTipiListesi.DataBind();
                            UPnlKatilimciTipiListesi.Update();
                            break;


                        case Sonuclar.Basarili:
                            txtKatilimciTipiID.Text = SDataModel.Veriler.KatilimciTipiID;
                            txtKatilimciTipi.Text = SDataModel.Veriler.KatilimciTipi;
                            txtKontenjan.Text = SDataModel.Veriler.Kontenjan.ToString();
                            txtMisafirKontenjan.Text = SDataModel.Veriler.MisafirKontenjan.ToString();
                            txtGirisSayisi.Text = SDataModel.Veriler.GirisSayisi.ToString();
                            txtYakaKartiBasimSayisi.Text = SDataModel.Veriler.YakaKartiBasimSayisi.ToString();

                            UPnlKatilimciTipiEkleGuncelle.Update();
                            BilgiKontrolMerkezi.UyariEkrani(this, $"$({UPnlKatilimciTipiEkleGuncelle.ClientID}).modal('show');", false);
                            break;
                    }

                    break;

                case "YakaKartiCerceveGuncelle":

                    SDataYakaKartiCerceveModel = new YakaKartiCerceveTablosuIslemler().KayitBilgisi(e.CommandArgument.ToString());
                    switch (SDataYakaKartiCerceveModel.Sonuc)
                    {
                        case Sonuclar.KismiBasarili:
                        case Sonuclar.Basarisiz:
                            BilgiKontrolMerkezi.UyariEkrani(this, $"UyariBilgilendirme('', '<p>Yaka kartı çerçeve içeriği bilgisi alınırken hata meydana geldi</p><p>Hata mesajı : {SDataYakaKartiCerceveModel.HataBilgi.HataMesaji}</p>', false);", false);
                            break;

                        default:
                        case Sonuclar.VeriBulunamadi:
                            BilgiKontrolMerkezi.UyariEkrani(this, $"UyariBilgilendirme('', '<p>Yaka kartı çerçeve içeriği bulunamadı</p>', false);", false);
                            rptKatilimciTipiListesi.DataBind();
                            UPnlKatilimciTipiListesi.Update();
                            break;


                        case Sonuclar.Basarili:
                            txtYKWidth.Text = SDataYakaKartiCerceveModel.Veriler.Width.ToString();
                            txtYKHeight.Text = SDataYakaKartiCerceveModel.Veriler.Height.ToString();
                            chkYaziciKagitOrtalama.Checked = SDataYakaKartiCerceveModel.Veriler.YaziciKagitOrtalama;
                            lnkbtnYakaKartiGorselGuncelle.CommandArgument = SDataYakaKartiCerceveModel.Veriler.YakaKartiCerceveID.ToString();

                            UPnlYakaKartiGorselGuncelle.Update();
                            BilgiKontrolMerkezi.UyariEkrani(this, $"$({UPnlYakaKartiGorselGuncelle.ClientID}).modal('show');", false);
                            break;
                    }

                    break;

                case "YakaKartiIcerikGuncelle":

                    SDataYakaKartiCerceveModel = new YakaKartiCerceveTablosuIslemler().KayitBilgisi(e.CommandArgument.ToString());
                    switch (SDataYakaKartiCerceveModel.Sonuc)
                    {
                        case Sonuclar.KismiBasarili:
                        case Sonuclar.Basarisiz:
                            BilgiKontrolMerkezi.UyariEkrani(this, $"UyariBilgilendirme('', '<p>Katılımcı tipi bilgisi alınırken hata meydana geldi</p><p>Hata mesajı : {SDataYakaKartiCerceveModel.HataBilgi.HataMesaji}</p>', false);", false);
                            break;

                        default:
                        case Sonuclar.VeriBulunamadi:
                            BilgiKontrolMerkezi.UyariEkrani(this, $"UyariBilgilendirme('', '<p>Katılımcı tipi bulunamadı</p>', false);", false);
                            rptKatilimciTipiListesi.DataBind();
                            UPnlKatilimciTipiListesi.Update();
                            break;


                        case Sonuclar.Basarili:
                            hfYakaKartiCerceveID.Value = SDataYakaKartiCerceveModel.Veriler.YakaKartiCerceveID.ToString();

                            ddlYakaKartiIcerik.DataBind();
                            SDataYakaKartiIcerikModel = new YakaKartiIcerikTablosuIslemler().KayitBilgisi(Convert.ToInt32(ddlYakaKartiIcerik.SelectedValue));
                            switch (SDataYakaKartiIcerikModel.Sonuc)
                            {
                                case Sonuclar.KismiBasarili:
                                case Sonuclar.Basarisiz:
                                    BilgiKontrolMerkezi.UyariEkrani(this, $"UyariBilgilendirme('', '<p>Yaka kartı içeriği bilgisi alınırken hata meydana geldi</p><p>Hata mesajı : {SDataMailIcerikModel.HataBilgi.HataMesaji}</p>', false);", false);
                                    break;

                                default:
                                case Sonuclar.VeriBulunamadi:
                                    BilgiKontrolMerkezi.UyariEkrani(this, $"UyariBilgilendirme('', '<p>Taka krtı içeriği bulunamadı</p>', false);", false);
                                    break;


                                case Sonuclar.Basarili:
                                    ImgYakaKarti.ImageUrl = $"~/Dosyalar/YakaKartiGorsel/{SDataYakaKartiCerceveModel.Veriler.YakaKartiCerceveID}.png?t={DateTime.Now.Ticks}";

                                    ddlYakaKartiIcerikTipi.DataBind();
                                    lnkbtnYakaKartiTipiEkle.Visible = !ddlYakaKartiIcerikTipi.Items.Count.Equals(0);

                                    txtYKIX.Text = SDataYakaKartiIcerikModel.Veriler.X.ToString();
                                    txtYKIY.Text = SDataYakaKartiIcerikModel.Veriler.Y.ToString();
                                    txtYKIWidth.Text = SDataYakaKartiIcerikModel.Veriler.Width.ToString();
                                    txtYKIHeight.Text = SDataYakaKartiIcerikModel.Veriler.Height.ToString();
                                    txtYKIOran.Text = SDataYakaKartiIcerikModel.Veriler.YakaKartiIcerikTipiBilgisi.Oran.ToString();

                                    UPnlYakaKartiIcerikGuncelle.Update();
                                    BilgiKontrolMerkezi.UyariEkrani(this, $"$({UPnlYakaKartiIcerikGuncelle.ClientID}).modal('show'); setTimeout(() => {{ cropperBadgeSetUp(); }}, 250);", false);

                                    break;

                            }
                            break;
                    }

                    break;

                case "KarsilamaEkraniGorselGuncelle":
                    SDataModel = new KatilimciTipiTablosuIslemler().KayitBilgisi(e.CommandArgument.ToString());
                    switch (SDataModel.Sonuc)
                    {
                        case Sonuclar.KismiBasarili:
                        case Sonuclar.Basarisiz:
                            BilgiKontrolMerkezi.UyariEkrani(this, $"UyariBilgilendirme('', '<p>Katılımcı tipi bilgisi alınırken hata meydana geldi</p><p>Hata mesajı : {SDataModel.HataBilgi.HataMesaji}</p>', false);", false);
                            break;

                        default:
                        case Sonuclar.VeriBulunamadi:
                            BilgiKontrolMerkezi.UyariEkrani(this, $"UyariBilgilendirme('', '<p>Katılımcı tipi bulunamadı</p>', false);", false);
                            rptKatilimciTipiListesi.DataBind();
                            UPnlKatilimciTipiListesi.Update();
                            break;


                        case Sonuclar.Basarili:
                            lnkbtnKarsilamaEkraniGorsel.CommandArgument = SDataModel.Veriler.KatilimciTipiID;

                            UPnlKarsilamaEkraniGorselGuncelle.Update();
                            BilgiKontrolMerkezi.UyariEkrani(this, $"$({UPnlKarsilamaEkraniGorselGuncelle.ClientID}).modal('show');", false);
                            break;
                    }
                    break;

                case "LogoGuncelle":
                    SDataModel = new KatilimciTipiTablosuIslemler().KayitBilgisi(e.CommandArgument.ToString());
                    switch (SDataModel.Sonuc)
                    {
                        case Sonuclar.KismiBasarili:
                        case Sonuclar.Basarisiz:
                            BilgiKontrolMerkezi.UyariEkrani(this, $"UyariBilgilendirme('', '<p>Katılımcı tipi bilgisi alınırken hata meydana geldi</p><p>Hata mesajı : {SDataModel.HataBilgi.HataMesaji}</p>', false);", false);
                            break;

                        default:
                        case Sonuclar.VeriBulunamadi:
                            BilgiKontrolMerkezi.UyariEkrani(this, $"UyariBilgilendirme('', '<p>Katılımcı tipi bulunamadı</p>', false);", false);
                            rptKatilimciTipiListesi.DataBind();
                            UPnlKatilimciTipiListesi.Update();
                            break;


                        case Sonuclar.Basarili:
                            lnkbtnLogoGuncelle.CommandArgument = SDataModel.Veriler.KatilimciTipiID;

                            UPnlLogoGuncelle.Update();
                            BilgiKontrolMerkezi.UyariEkrani(this, $"$({UPnlLogoGuncelle.ClientID}).modal('show');", false);
                            break;
                    }
                    break;

                case "KabulEkranIcerikGuncelle":

                    SDataModel = new KatilimciTipiTablosuIslemler().KayitBilgisi(e.CommandArgument.ToString());
                    switch (SDataModel.Sonuc)
                    {
                        case Sonuclar.KismiBasarili:
                        case Sonuclar.Basarisiz:
                            BilgiKontrolMerkezi.UyariEkrani(this, $"UyariBilgilendirme('', '<p>Katılımcı tipi bilgisi alınırken hata meydana geldi</p><p>Hata mesajı : {SDataModel.HataBilgi.HataMesaji}</p>', false);", false);
                            break;

                        default:
                        case Sonuclar.VeriBulunamadi:
                            BilgiKontrolMerkezi.UyariEkrani(this, $"UyariBilgilendirme('', '<p>Katılımcı tipi bulunamadı</p>', false);", false);
                            rptKatilimciTipiListesi.DataBind();
                            UPnlKatilimciTipiListesi.Update();
                            break;


                        case Sonuclar.Basarili:
                            h4_EkranIcerik.InnerHtml = "Kabul Ekran İçeriği";
                            //td_IcerikBaslik.InnerHtml = "İçerik";
                            td_IcerikBaslik.Attributes["data-durum"] = "Kabul";
                            txtEkranIcerik.Text = SDataModel.Veriler.KabulEkranIcerik;
                            lnkbtnEkranIcerikGuncelle.CommandArgument = SDataModel.Veriler.KatilimciTipiID;

                            UPnlEkranIcerik.Update();
                            BilgiKontrolMerkezi.UyariEkrani(this, $"$({UPnlEkranIcerik.ClientID}).modal('show');", false);
                            break;
                    }
                    break;

                case "RedEkranIcerikGuncelle":
                    SDataModel = new KatilimciTipiTablosuIslemler().KayitBilgisi(e.CommandArgument.ToString());
                    switch (SDataModel.Sonuc)
                    {
                        case Sonuclar.KismiBasarili:
                        case Sonuclar.Basarisiz:
                            BilgiKontrolMerkezi.UyariEkrani(this, $"UyariBilgilendirme('', '<p>Katılımcı tipi bilgisi alınırken hata meydana geldi</p><p>Hata mesajı : {SDataModel.HataBilgi.HataMesaji}</p>', false);", false);
                            break;

                        default:
                        case Sonuclar.VeriBulunamadi:
                            BilgiKontrolMerkezi.UyariEkrani(this, $"UyariBilgilendirme('', '<p>Katılımcı tipi bulunamadı</p>', false);", false);
                            rptKatilimciTipiListesi.DataBind();
                            UPnlKatilimciTipiListesi.Update();
                            break;


                        case Sonuclar.Basarili:
                            h4_EkranIcerik.InnerHtml = "Red Ekran İçeriği";
                            //td_IcerikBaslik.InnerHtml = "İçerik";
                            td_IcerikBaslik.Attributes["data-durum"] = "Red";
                            txtEkranIcerik.Text = SDataModel.Veriler.RedEkranIcerik;
                            lnkbtnEkranIcerikGuncelle.CommandArgument = SDataModel.Veriler.KatilimciTipiID;

                            UPnlEkranIcerik.Update();
                            BilgiKontrolMerkezi.UyariEkrani(this, $"$({UPnlEkranIcerik.ClientID}).modal('show');", false);
                            break;
                    }
                    break;

                default:
                    break;
            }
        }

        protected void lnkbtnKatilimciTipiEkleGuncelle_Click(object sender, EventArgs e)
        {
            KModel = new KatilimciTipiTablosuModel
            {
                KatilimciTipiID = string.IsNullOrEmpty(txtKatilimciTipiID.Text) ? new KatilimciTipiTablosuIslemler().YeniKatilimciTipiID() : txtKatilimciTipiID.Text,
                KatilimciTipi = Kontrol.KelimeKontrol(txtKatilimciTipi, "Katılımcı tipi başlığı boş bırakılamaz", ref Uyarilar),
                Kontenjan = Kontrol.TamSayiyaKontrol(txtKontenjan, "Kontenjan boş bırakılamaz", "Geçersiz kontenjan girişi yapıldı", ref Uyarilar),
                MisafirKontenjan = Kontrol.TamSayiyaKontrol(txtMisafirKontenjan, "Misafir kontenjan boş bırakılamaz", "Geçerisiz misafir kontenjan girişi yapıldı", ref Uyarilar),
                GirisSayisi = Kontrol.TamSayiyaKontrol(txtGirisSayisi, "İzin verilen giriş sayısı boş bırakılamaz", "Geçersiz giriş sayısı girildi", ref Uyarilar),
                YakaKartiBasimSayisi = Kontrol.TamSayiyaKontrol(txtYakaKartiBasimSayisi, "İzin verilen yaka kartı basım sayısı boş bırakılamaz", "Geçersiz yaka kartı basım sayısı girildi", ref Uyarilar),
                KabulEkranIcerik = KabulEkranIcerik,
                RedEkranIcerik = RedEkranIcerik,
                GuncellenmeTarihi = Kontrol.Simdi(),
                EklenmeTarihi = Kontrol.Simdi()
            };

            if (string.IsNullOrEmpty(Uyarilar.ToString()))
            {
                if (string.IsNullOrEmpty(txtKatilimciTipiID.Text))
                {
                    using (OleDbConnection cnn = ConnectionBuilder.DefaultConnection())
                    {
                        ConnectionBuilder.OpenConnection(cnn);
                        using (OleDbTransaction trn = cnn.BeginTransaction())
                        {
                            SModel = new KatilimciTipiTablosuIslemler(trn).YeniKayitEkle(KModel);
                            if (SModel.Sonuc.Equals(Sonuclar.Basarili))
                            {
                                KModel.SmsIcerikBilgisi = new List<SmsIcerikTablosuModel>();

                                // SMS Icerik Kayıtları
                                for (int i = 1; i <= 2; i++)
                                {
                                    switch (i)
                                    {
                                        // Davet İçeriği
                                        case 1:
                                            SIModel = new SmsIcerikTablosuModel
                                            {
                                                KatilimciTipiID = KModel.KatilimciTipiID,
                                                GonderimTipiID = i,
                                                SmsIcerik = SmsIcerik_Davet,
                                                GuncellenmeTarihi = Kontrol.Simdi(),
                                                EklenmeTarihi = Kontrol.Simdi()
                                            };
                                            SModel = new SmsIcerikTablosuIslemler(trn).YeniKayitEkle(SIModel);

                                            if (SModel.Sonuc.Equals(Sonuclar.Basarili))
                                            {
                                                SIModel.SmsIcerikID = Convert.ToInt32(SModel.YeniKayitID);
                                                KModel.SmsIcerikBilgisi.Add(SIModel);
                                            }

                                            break;

                                        // QR içeriği
                                        case 2:
                                            SIModel = new SmsIcerikTablosuModel
                                            {
                                                KatilimciTipiID = KModel.KatilimciTipiID,
                                                GonderimTipiID = 2,
                                                SmsIcerik = SmsIcerik_QR,
                                                AnaKatilimci = true,
                                                GuncellenmeTarihi = Kontrol.Simdi(),
                                                EklenmeTarihi = Kontrol.Simdi()
                                            };
                                            SModel = new SmsIcerikTablosuIslemler(trn).YeniKayitEkle(SIModel);

                                            if (SModel.Sonuc.Equals(Sonuclar.Basarili))
                                            {
                                                SIModel.SmsIcerikID = Convert.ToInt32(SModel.YeniKayitID);
                                                KModel.SmsIcerikBilgisi.Add(SIModel);

                                                SIQModel = new SmsIcerikTablosuModel
                                                {
                                                    KatilimciTipiID = KModel.KatilimciTipiID,
                                                    GonderimTipiID = 2,
                                                    SmsIcerik = SmsIcerik_QR,
                                                    AnaKatilimci = false,
                                                    GuncellenmeTarihi = Kontrol.Simdi(),
                                                    EklenmeTarihi = Kontrol.Simdi()
                                                };


                                                SModel = new SmsIcerikTablosuIslemler(trn).YeniKayitEkle(SIQModel);
                                                if (SModel.Sonuc.Equals(Sonuclar.Basarili))
                                                {
                                                    SIQModel.SmsIcerikID = Convert.ToInt32(SModel.YeniKayitID);
                                                    KModel.SmsIcerikBilgisi.Add(SIQModel);
                                                }
                                            }

                                            break;
                                    }

                                    if (SModel.Sonuc.Equals(Sonuclar.Basarisiz))
                                        break;
                                }

                                if (SModel.Sonuc.Equals(Sonuclar.Basarili))
                                {
                                    KModel.MailIcerikBilgisi = new List<MailIcerikTablosuModel>();

                                    // Outlook İçeriği
                                    for (int i = 1; i <= 2; i++)
                                    {
                                        switch (i)
                                        {
                                            // Davet İçeriği
                                            case 1:
                                                MIModel = new MailIcerikTablosuModel
                                                {
                                                    GonderimTipiID = i,
                                                    KatilimciTipiID = KModel.KatilimciTipiID,
                                                    Konu = "Arcadias Tech Yılbaşı Etkinliği Daveti",
                                                    HtmlIcerik = MailIcerik_Davet,
                                                    AnaKatilimci = true,
                                                    GuncellenmeTarihi = Kontrol.Simdi(),
                                                    EklenmeTarihi = Kontrol.Simdi()
                                                };
                                                SModel = new MailIcerikTablosuIslemler(trn).YeniKayitEkle(MIModel);

                                                if (SModel.Sonuc.Equals(Sonuclar.Basarili))
                                                {
                                                    MIModel.MailIcerikID = Convert.ToInt32(SModel.YeniKayitID);
                                                    KModel.MailIcerikBilgisi.Add(MIModel);

                                                    try
                                                    {
                                                        File.Copy(Server.MapPath("~/Gorseller/Davetiye.png"), Server.MapPath($"~/Dosyalar/MailIcerikGorsel/{MIModel.MailIcerikID}.png"));
                                                    }
                                                    catch (Exception ex)
                                                    {
                                                        SModel = new SurecBilgiModel
                                                        {
                                                            Sonuc = Sonuclar.Basarisiz,
                                                            KullaniciMesaji = "Davetiye görseli eklenirken hata meydana geldi",
                                                            HataBilgi = new HataBilgileri
                                                            {
                                                                HataAlinanKayitID = MIModel.MailIcerikID,
                                                                HataKodu = ex.HResult,
                                                                HataMesaji = ex.Message.Replace("\r", string.Empty).Replace("\n", string.Empty).Replace("\t", string.Empty).Replace("'", string.Empty)
                                                            }
                                                        };
                                                    }
                                                }

                                                break;

                                            // QR İçeriği
                                            case 2:
                                                MIModel = new MailIcerikTablosuModel
                                                {
                                                    GonderimTipiID = i,
                                                    KatilimciTipiID = KModel.KatilimciTipiID,
                                                    Konu = "Arcadias Tech Yılbaşı Etkinliği Daveti",
                                                    HtmlIcerik = MailIcerik_QR,
                                                    AnaKatilimci = true,
                                                    GuncellenmeTarihi = Kontrol.Simdi(),
                                                    EklenmeTarihi = Kontrol.Simdi()
                                                };
                                                SModel = new MailIcerikTablosuIslemler(trn).YeniKayitEkle(MIModel);

                                                if (SModel.Sonuc.Equals(Sonuclar.Basarili))
                                                {
                                                    MIModel.MailIcerikID = Convert.ToInt32(SModel.YeniKayitID);
                                                    KModel.MailIcerikBilgisi.Add(MIModel);

                                                    try
                                                    {
                                                        File.Copy(Server.MapPath("~/Gorseller/QR.png"), Server.MapPath($"~/Dosyalar/MailIcerikGorsel/{MIModel.MailIcerikID}.png"));

                                                        MIQModel = new MailIcerikTablosuModel
                                                        {
                                                            GonderimTipiID = i,
                                                            KatilimciTipiID = KModel.KatilimciTipiID,
                                                            Konu = "Arcadias Tech Yılbaşı Etkinliği Daveti",
                                                            HtmlIcerik = MailIcerik_QR,
                                                            AnaKatilimci = false,
                                                            GuncellenmeTarihi = Kontrol.Simdi(),
                                                            EklenmeTarihi = Kontrol.Simdi()
                                                        };

                                                        SModel = new MailIcerikTablosuIslemler(trn).YeniKayitEkle(MIQModel);

                                                        if (SModel.Sonuc.Equals(Sonuclar.Basarili))
                                                        {
                                                            MIQModel.MailIcerikID = Convert.ToInt32(SModel.YeniKayitID);
                                                            KModel.MailIcerikBilgisi.Add(MIQModel);

                                                            File.Copy(Server.MapPath("~/Gorseller/QR.png"), Server.MapPath($"~/Dosyalar/MailIcerikGorsel/{MIQModel.MailIcerikID}.png"));
                                                        }
                                                    }
                                                    catch (Exception ex)
                                                    {
                                                        SModel = new SurecBilgiModel
                                                        {
                                                            Sonuc = Sonuclar.Basarisiz,
                                                            KullaniciMesaji = "QR görseli eklenirken hata meydana geldi",
                                                            HataBilgi = new HataBilgileri
                                                            {
                                                                HataAlinanKayitID = MIModel.MailIcerikID,
                                                                HataKodu = ex.HResult,
                                                                HataMesaji = ex.Message.Replace("\r", string.Empty).Replace("\n", string.Empty).Replace("\t", string.Empty).Replace("'", string.Empty)
                                                            }
                                                        };
                                                    }
                                                }
                                                break;
                                        }

                                        if (SModel.Sonuc.Equals(Sonuclar.Basarisiz))
                                            break;
                                    }

                                    if (SModel.Sonuc.Equals(Sonuclar.Basarili))
                                    {
                                        // Antetli Kağıt İçeriği
                                        foreach (MailIcerikTablosuModel Item in KModel.MailIcerikBilgisi)
                                        {
                                            if (Item.GonderimTipiID.Equals(1))
                                            {
                                                // Kabul Ediyorum Butonu
                                                SModel = new AntetliKagitIcerikTablosuIslemler(trn).YeniKayitEkle(new AntetliKagitIcerikTablosuModel
                                                {
                                                    AntetliKagitIcerikTipiID = 1,
                                                    MailIcerikID = Item.MailIcerikID,
                                                    X = 0,
                                                    Y = 0,
                                                    Width = 1,
                                                    Height = 1,
                                                    GuncellenmeTarihi = Kontrol.Simdi(),
                                                    EklenmeTarihi = Kontrol.Simdi()
                                                });

                                                if (SModel.Sonuc.Equals(Sonuclar.Basarisiz))
                                                    break;

                                                // Red Ediyorum Butonu
                                                SModel = new AntetliKagitIcerikTablosuIslemler(trn).YeniKayitEkle(new AntetliKagitIcerikTablosuModel
                                                {
                                                    AntetliKagitIcerikTipiID = 2,
                                                    MailIcerikID = Item.MailIcerikID,
                                                    X = 0,
                                                    Y = 0,
                                                    Width = 1,
                                                    Height = 1,
                                                    GuncellenmeTarihi = Kontrol.Simdi(),
                                                    EklenmeTarihi = Kontrol.Simdi()
                                                });

                                                if (SModel.Sonuc.Equals(Sonuclar.Basarisiz))
                                                    break;
                                            }
                                            else
                                            {
                                                // Ad Soyad
                                                SModel = new AntetliKagitIcerikTablosuIslemler(trn).YeniKayitEkle(new AntetliKagitIcerikTablosuModel
                                                {
                                                    AntetliKagitIcerikTipiID = 3,
                                                    MailIcerikID = Item.MailIcerikID,
                                                    X = 0,
                                                    Y = 0,
                                                    Width = 1,
                                                    Height = 1,
                                                    GuncellenmeTarihi = Kontrol.Simdi(),
                                                    EklenmeTarihi = Kontrol.Simdi()
                                                });

                                                if (SModel.Sonuc.Equals(Sonuclar.Basarisiz))
                                                    break;

                                                // QR
                                                SModel = new AntetliKagitIcerikTablosuIslemler(trn).YeniKayitEkle(new AntetliKagitIcerikTablosuModel
                                                {
                                                    AntetliKagitIcerikTipiID = 4,
                                                    MailIcerikID = Item.MailIcerikID,
                                                    X = 0,
                                                    Y = 0,
                                                    Width = 1,
                                                    Height = 1,
                                                    GuncellenmeTarihi = Kontrol.Simdi(),
                                                    EklenmeTarihi = Kontrol.Simdi()
                                                });

                                                if (SModel.Sonuc.Equals(Sonuclar.Basarisiz))
                                                    break;

                                                // Misafir Kayıt Butonu
                                                if (Item.AnaKatilimci)
                                                {
                                                    
                                                    SModel = new AntetliKagitIcerikTablosuIslemler(trn).YeniKayitEkle(new AntetliKagitIcerikTablosuModel
                                                    {
                                                        AntetliKagitIcerikTipiID = 5,
                                                        MailIcerikID = Item.MailIcerikID,
                                                        X = 0,
                                                        Y = 0,
                                                        Width = 1,
                                                        Height = 1,
                                                        GuncellenmeTarihi = Kontrol.Simdi(),
                                                        EklenmeTarihi = Kontrol.Simdi()
                                                    });
                                                }
                                                

                                                if (SModel.Sonuc.Equals(Sonuclar.Basarisiz))
                                                    break;
                                            }
                                        }

                                        if (SModel.Sonuc.Equals(Sonuclar.Basarili))
                                        {
                                            // Yaka Kartı ve İçerik

                                            KModel.YakaKartiCerceveBilgisi = new YakaKartiCerceveTablosuModel
                                            {
                                                KatilimciTipiID = KModel.KatilimciTipiID,
                                                Width = 105,
                                                Height = 135,
                                                GuncellenmeTarihi = Kontrol.Simdi(),
                                                EklenmeTarihi = Kontrol.Simdi()
                                            };

                                            SModel = new YakaKartiCerceveTablosuIslemler(trn).YeniKayitEkle(KModel.YakaKartiCerceveBilgisi);

                                            if (SModel.Sonuc.Equals(Sonuclar.Basarili))
                                            {
                                                KModel.YakaKartiCerceveBilgisi.YakaKartiCerceveID = Convert.ToInt32(SModel.YeniKayitID);
                                                try
                                                {
                                                    File.Copy(Server.MapPath($"~/Gorseller/YakaKarti.png"), Server.MapPath($"~/Dosyalar/YakaKartiGorsel/{KModel.YakaKartiCerceveBilgisi.YakaKartiCerceveID}.png"));
                                                    SModel = new SurecBilgiModel
                                                    {
                                                        Sonuc = Sonuclar.Basarili,
                                                        KullaniciMesaji = "Yaka kartı görseli yüklendi"
                                                    };
                                                }
                                                catch (Exception ex)
                                                {
                                                    SModel = new SurecBilgiModel
                                                    {
                                                        Sonuc = Sonuclar.Basarisiz,
                                                        KullaniciMesaji = "Yaka kartı görseli eklenirken hata meydana geldi",
                                                        HataBilgi = new HataBilgileri
                                                        {
                                                            HataAlinanKayitID = MIModel.MailIcerikID,
                                                            HataKodu = ex.HResult,
                                                            HataMesaji = ex.Message.Replace("\r", string.Empty).Replace("\n", string.Empty).Replace("\t", string.Empty).Replace("'", string.Empty)
                                                        }
                                                    };
                                                }



                                                if (SModel.Sonuc.Equals(Sonuclar.Basarili))
                                                {
                                                    for (int i = 1; i <= 2; i++)
                                                    {
                                                        SModel = new YakaKartiIcerikTablosuIslemler(trn).YeniKayitEkle(new YakaKartiIcerikTablosuModel
                                                        {
                                                            YakaKartiCerceveID = KModel.YakaKartiCerceveBilgisi.YakaKartiCerceveID,
                                                            YakaKartiIcerikTipiID = i,
                                                            X = 0,
                                                            Y = 0,
                                                            Width = 1,
                                                            Height = 1,
                                                            GuncellenmeTarihi = Kontrol.Simdi(),
                                                            EklenmeTarihi = Kontrol.Simdi()
                                                        });

                                                        if (SModel.Sonuc.Equals(Sonuclar.Basarisiz))
                                                            break;
                                                    }

                                                    if (SModel.Sonuc.Equals(Sonuclar.Basarili))
                                                    {
                                                        try
                                                        {
                                                            File.Copy(Server.MapPath("~/Gorseller/Katilimci_BG.jpg"), Server.MapPath($"~/Dosyalar/KarsilamaEkrani/{KModel.KatilimciTipiID}.jpg"));
                                                            File.Copy(Server.MapPath("~/Gorseller/logo.png"), Server.MapPath($"~/Dosyalar/Logo/{KModel.KatilimciTipiID}.png"));

                                                            trn.Commit();

                                                            Kontrol.Temizle(txtKatilimciTipiID);
                                                            Kontrol.Temizle(txtKatilimciTipi);
                                                            Kontrol.Temizle(txtKontenjan);
                                                            Kontrol.Temizle(txtMisafirKontenjan);
                                                            Kontrol.Temizle(txtGirisSayisi);

                                                            UPnlKatilimciTipiEkleGuncelle.Update();

                                                            rptKatilimciTipiListesi.DataBind();
                                                            UPnlKatilimciTipiListesi.Update();

                                                            BilgiKontrolMerkezi.UyariEkrani(this, $"$({UPnlKatilimciTipiEkleGuncelle.ClientID}).modal('hide'); UyariBilgilendirme('', '<p>Katılımcı tipi eklendi</p>', true);", false);
                                                        }
                                                        catch (Exception ex)
                                                        {
                                                            trn.Rollback();
                                                            foreach (MailIcerikTablosuModel Item in KModel.MailIcerikBilgisi)
                                                            {
                                                                if (File.Exists(Server.MapPath($"~/Dosyalar/MailIcerikGorsel/{Item.MailIcerikID}.png")))
                                                                    File.Delete(Server.MapPath($"~/Dosyalar/MailIcerikGorsel/{Item.MailIcerikID}.png"));
                                                            }

                                                            if (File.Exists(Server.MapPath($"~/Dosyalar/YakaKartiGorsel/{KModel.YakaKartiCerceveBilgisi.YakaKartiCerceveID}.png")))
                                                                File.Delete(Server.MapPath($"~/Dosyalar/YakaKartiGorsel/{KModel.YakaKartiCerceveBilgisi.YakaKartiCerceveID}.png"));

                                                            if (File.Exists(Server.MapPath($"~/Dosyalar/KarsilamaEkrani/{KModel.KatilimciTipiID}.jpg")))
                                                                File.Delete(Server.MapPath($"~/Dosyalar/KarsilamaEkrani/{KModel.KatilimciTipiID}.jpg"));

                                                            if (File.Exists(Server.MapPath($"~/Dosyalar/Logo/{KModel.KatilimciTipiID}.png")))
                                                                File.Delete(Server.MapPath($"~/Dosyalar/Logo/{KModel.KatilimciTipiID}.png"));

                                                            BilgiKontrolMerkezi.UyariEkrani(this, $"AltSayfaUyariBilgilendirme('<p>Karşılama ekranı görselleri eklenirken hata meydana geldi</p><p>Hata mesajı : {ex.Message.Replace("'", string.Empty).Replace("\r", string.Empty).Replace("\n", string.Empty).Replace("\t", string.Empty)}</p>', false);", false);
                                                        }
                                                    }
                                                    else
                                                    {
                                                        trn.Rollback();
                                                        foreach (MailIcerikTablosuModel Item in KModel.MailIcerikBilgisi)
                                                        {
                                                            if (File.Exists(Server.MapPath($"~/Dosyalar/MailIcerikGorsel/{Item.MailIcerikID}.png")))
                                                                File.Delete(Server.MapPath($"~/Dosyalar/MailIcerikGorsel/{Item.MailIcerikID}.png"));
                                                        }

                                                        if (File.Exists(Server.MapPath($"~/Dosyalar/YakaKartiGorsel/{KModel.YakaKartiCerceveBilgisi.YakaKartiCerceveID}.png")))
                                                            File.Delete(Server.MapPath($"~/Dosyalar/YakaKartiGorsel/{KModel.YakaKartiCerceveBilgisi.YakaKartiCerceveID}.png"));

                                                        BilgiKontrolMerkezi.UyariEkrani(this, $"AltSayfaUyariBilgilendirme('<p>Yaka kartı içerik bilgileri eklenirken hata meydana geldi</p><p>Hata mesajı : {SModel.HataBilgi.HataMesaji}</p>', false);", false);
                                                    }
                                                }
                                                else
                                                {
                                                    trn.Rollback();
                                                    foreach (MailIcerikTablosuModel Item in KModel.MailIcerikBilgisi)
                                                    {
                                                        if (File.Exists(Server.MapPath($"~/Dosyalar/MailIcerikGorsel/{Item.MailIcerikID}.png")))
                                                            File.Delete(Server.MapPath($"~/Dosyalar/MailIcerikGorsel/{Item.MailIcerikID}.png"));
                                                    }

                                                    BilgiKontrolMerkezi.UyariEkrani(this, $"AltSayfaUyariBilgilendirme('<p>Yaka kartı görseli eklenirken hata meydana geldi</p><p>Hata mesajı : {SModel.HataBilgi.HataMesaji}</p>', false);", false);
                                                }
                                            }
                                            else
                                            {
                                                trn.Rollback();
                                                foreach (MailIcerikTablosuModel Item in KModel.MailIcerikBilgisi)
                                                {
                                                    if (File.Exists(Server.MapPath($"~/Dosyalar/MailIcerikGorsel/{Item.MailIcerikID}.png")))
                                                        File.Delete(Server.MapPath($"~/Dosyalar/MailIcerikGorsel/{Item.MailIcerikID}.png"));
                                                }

                                                BilgiKontrolMerkezi.UyariEkrani(this, $"AltSayfaUyariBilgilendirme('<p>Yaka kartı çerçeve bilgileri eklenirken hata meydana geldi</p><p>Hata mesajı : {SModel.HataBilgi.HataMesaji}</p>', false);", false);
                                            }
                                        }
                                        else
                                        {
                                            trn.Rollback();
                                            foreach (MailIcerikTablosuModel Item in KModel.MailIcerikBilgisi)
                                            {
                                                if (File.Exists(Server.MapPath($"~/Dosyalar/MailIcerikGorsel/{Item.MailIcerikID}.png")))
                                                    File.Delete(Server.MapPath($"~/Dosyalar/MailIcerikGorsel/{Item.MailIcerikID}.png"));
                                            }

                                            BilgiKontrolMerkezi.UyariEkrani(this, $"AltSayfaUyariBilgilendirme('<p>Davetiye görsel içerikleri eklenirken hata meydana geldi</p><p>Hata mesajı : {SModel.HataBilgi.HataMesaji}</p>', false);", false);
                                        }
                                    }
                                    else
                                    {
                                        trn.Rollback();
                                        foreach (MailIcerikTablosuModel Item in KModel.MailIcerikBilgisi)
                                        {
                                            if (File.Exists(Server.MapPath($"~/Dosyalar/MailIcerikGorsel/{Item.MailIcerikID}.png")))
                                                File.Delete(Server.MapPath($"~/Dosyalar/MailIcerikGorsel/{Item.MailIcerikID}.png"));
                                        }

                                        BilgiKontrolMerkezi.UyariEkrani(this, $"AltSayfaUyariBilgilendirme('<p>Mail içerikleri eklenirken hata meydana geldi</p><p>Hata mesajı : {SModel.HataBilgi.HataMesaji}</p>', false);", false);
                                    }
                                }
                                else
                                {
                                    trn.Rollback();
                                    BilgiKontrolMerkezi.UyariEkrani(this, $"AltSayfaUyariBilgilendirme('<p>SMS içerikleri eklenirken hata meydana geldi</p><p>Hata mesajı : {SModel.HataBilgi.HataMesaji}</p>', false);", false);
                                }
                            }
                            else
                            {
                                trn.Rollback();
                                BilgiKontrolMerkezi.UyariEkrani(this, $"AltSayfaUyariBilgilendirme('<p>Katılımcı tipi eklenirken hata meydana geldi</p><p>Hata mesajı : {SModel.HataBilgi.HataMesaji}</p>', false);", false);
                            }
                        }
                        ConnectionBuilder.CloseConnection(cnn);
                    }
                }
                else
                {
                    SModel = new KatilimciTipiTablosuIslemler().KayitGuncelle(KModel);

                    if (SModel.Sonuc.Equals(Sonuclar.Basarili))
                    {
                        Kontrol.Temizle(txtKatilimciTipiID);
                        Kontrol.Temizle(txtKatilimciTipi);
                        Kontrol.Temizle(txtKontenjan);
                        Kontrol.Temizle(txtMisafirKontenjan);
                        Kontrol.Temizle(txtGirisSayisi);
                        UPnlKatilimciTipiEkleGuncelle.Update();

                        rptKatilimciTipiListesi.DataBind();
                        UPnlKatilimciTipiListesi.Update();

                        BilgiKontrolMerkezi.UyariEkrani(this, $"$({UPnlKatilimciTipiEkleGuncelle.ClientID}).modal('hide'); UyariBilgilendirme('', '<p>Katılımcı tipi bilgileri güncellendi</p>', true);", false);
                    }
                    else
                    {
                        BilgiKontrolMerkezi.UyariEkrani(this, $"AltSayfaUyariBilgilendirme('<p>Katılımcı tipi bilgileri güncellenirken hata meydana geldi</p><p>Hata mesajı : {SModel.HataBilgi.HataMesaji}</p>', false);", false);
                    }
                }
            }
            else
            {
                BilgiKontrolMerkezi.UyariEkrani(this, $"AltSayfaUyariBilgilendirme('{Uyarilar}', false);", false);
            }
        }

        protected void lnkbtnGorselGuncelle_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(hfGorsel.Value))
            {
                try
                {
                    using (MemoryStream ms = new MemoryStream(Convert.FromBase64String(hfGorsel.Value.Substring(hfGorsel.Value.IndexOf(",") + 1))))
                    {
                        using (System.Drawing.Image DavetiyeGorsel = System.Drawing.Image.FromStream(ms))
                        {
                            DavetiyeGorsel.Save(Server.MapPath($"~/Dosyalar/MailIcerikGorsel/{lnkbtnGorselGuncelle.CommandArgument}.png"), System.Drawing.Imaging.ImageFormat.Png);
                        }
                    }

                    BilgiKontrolMerkezi.UyariEkrani(this, $"$('#{UPnlGorselGuncelle.ClientID}').modal('hide'); setTimeout(() => {{ UyariBilgilendirme('', '<p>Davetiye görseliniz güncellenmiştir.</p>', true); }}, 250);", false);
                }
                catch (Exception ex)
                {

                    BilgiKontrolMerkezi.UyariEkrani(this, $"AltSayfaUyariBilgilendirme('<p>Görsel yüklenirken hata meydana geldi</p><p>Hata mesajı : {ex.Message.Replace("'", string.Empty).Replace("\r", string.Empty).Replace("\n", string.Empty).Replace("\t", string.Empty)}</p>', false);", false);
                }
            }
            else
            {
                BilgiKontrolMerkezi.UyariEkrani(this, $"AltSayfaUyariBilgilendirme('<p>Lütfen yüklenecek görselinizi seçiniz</p>', false);", false);
            }

            Kontrol.Temizle(hfGorsel);
        }

        protected void ddlAntetliKagitIcerik_SelectedIndexChanged(object sender, EventArgs e)
        {
            SDataAntetliKagitIcerikModel = new AntetliKagitIcerikTablosuIslemler().KayitBilgisi(Convert.ToInt32(ddlAntetliKagitIcerik.SelectedValue));

            switch (SDataAntetliKagitIcerikModel.Sonuc)
            {
                case Sonuclar.KismiBasarili:
                case Sonuclar.Basarisiz:
                    BilgiKontrolMerkezi.UyariEkrani(this, $"UyariBilgilendirme('', '<p>Görsel içeriği bilgisi alınırken hata meydana geldi</p><p>Hata mesajı : {SDataMailIcerikModel.HataBilgi.HataMesaji}</p>', false);", false);
                    break;

                default:
                case Sonuclar.VeriBulunamadi:
                    BilgiKontrolMerkezi.UyariEkrani(this, $"UyariBilgilendirme('', '<p>Görsel içeriği bulunamadı</p>', false);", false);
                    break;


                case Sonuclar.Basarili:
                    txtX.Text = SDataAntetliKagitIcerikModel.Veriler.X.ToString();
                    txtY.Text = SDataAntetliKagitIcerikModel.Veriler.Y.ToString();
                    txtWidth.Text = SDataAntetliKagitIcerikModel.Veriler.Width.ToString();
                    txtHeight.Text = SDataAntetliKagitIcerikModel.Veriler.Height.ToString();
                    txtOran.Text = SDataAntetliKagitIcerikModel.Veriler.AntetliKagitIcerikTipiBilgisi.Oran.ToString();

                    UPnlAntetliKagitIcerikGuncelle.Update();
                    BilgiKontrolMerkezi.UyariEkrani(this, $"setTimeout(() => {{ cropperSetUp(); }}, 250);", false);
                    break;
            }
        }

        protected void lnkbtnAntetliKagitIcerikGuncelle_Click(object sender, EventArgs e)
        {
            AIModel = new AntetliKagitIcerikTablosuModel
            {
                AntetliKagitIcerikID = Convert.ToInt32(ddlAntetliKagitIcerik.SelectedValue),
                X = Kontrol.TamSayiyaKontrol(txtX, "X değeri boş bırakılamaz", "Geçersiz X değeri girildi", ref Uyarilar),
                Y = Kontrol.TamSayiyaKontrol(txtY, "Y değeri boş bırakılamaz", "Geçersiz Y değeri girildi", ref Uyarilar),
                Width = Kontrol.TamSayiyaKontrol(txtWidth, "Genişlik değeri boş bırakılamaz", "Geçersiz genişlik değeri girildi", ref Uyarilar),
                Height = Kontrol.TamSayiyaKontrol(txtHeight, "Yükseklik değeri boş bırakılamaz", "Geçersiz yükseklik değeri girildi", ref Uyarilar),
                GuncellenmeTarihi = Kontrol.Simdi()
            };

            if (string.IsNullOrEmpty(Uyarilar.ToString()))
            {
                SModel = new AntetliKagitIcerikTablosuIslemler().OlcuGuncelle(AIModel);

                if (SModel.Sonuc.Equals(Sonuclar.Basarili))
                {
                    BilgiKontrolMerkezi.UyariEkrani(this, $"AltSayfaUyariBilgilendirme('<p>İçerik ölçüleri güncellendi</p>', true);", false);
                }
                else
                {
                    BilgiKontrolMerkezi.UyariEkrani(this, $"AltSayfaUyariBilgilendirme('<p>İçerik ölçüleri kaydedilirken hata meydana geldi</p><p>Hata mesajı : {SModel.HataBilgi.HataMesaji}</p>', false);", false);
                }
            }
            else
            {
                BilgiKontrolMerkezi.UyariEkrani(this, $"AltSayfaUyariBilgilendirme('{Uyarilar}',false);", false);
            }
        }

        protected void lnkbtnMailIcerikGuncelle_Click(object sender, EventArgs e)
        {
            MIModel = new MailIcerikTablosuModel
            {
                MailIcerikID = Convert.ToInt32(lnkbtnMailIcerikGuncelle.CommandArgument),
                Konu = Kontrol.KelimeKontrol(txtKonu, "Gönderilecek mail konusu boş bırakılamaz", ref Uyarilar),
                HtmlIcerik = Kontrol.KelimeKontrol(txtMailIcerik, "Mail içeriği boş bırakılamaz", ref Uyarilar),
                GuncellenmeTarihi = Kontrol.Simdi()
            };

            if (string.IsNullOrEmpty(Uyarilar.ToString()))
            {
                SModel = new MailIcerikTablosuIslemler().KayitGuncelle(MIModel);

                if (SModel.Sonuc.Equals(Sonuclar.Basarili))
                {
                    BilgiKontrolMerkezi.UyariEkrani(this, $"$({UPnlMailIcerikGuncelle.ClientID}).modal('hide'); setTimeout(() => {{ UyariBilgilendirme('', '<p>Mail içeriği güncellendi.</p>', true); }}, 250);", false);
                }
                else
                {
                    BilgiKontrolMerkezi.UyariEkrani(this, $"AltSayfaUyariBilgilendirme('<p>Mail içeriği güncellenirken hata meydana geldi</p><p>Hata mesajı : {SModel.HataBilgi.HataMesaji}</p>', false);", false);
                }
            }
            else
            {
                BilgiKontrolMerkezi.UyariEkrani(this, $"AltSayfaUyariBilgilendirme('{Uyarilar}', false);", false);
            }
        }

        protected void lnkbtnSmsIcerikGuncelle_Click(object sender, EventArgs e)
        {
            SIModel = new SmsIcerikTablosuModel
            {
                SmsIcerikID = Convert.ToInt32(lnkbtnSmsIcerikGuncelle.CommandArgument),
                SmsIcerik = Kontrol.KelimeKontrol(txtSmsIcerigi, "SMS içeriği boş bırakılamaz", ref Uyarilar),
                GuncellenmeTarihi = Kontrol.Simdi()
            };

            if (string.IsNullOrEmpty(Uyarilar.ToString()))
            {
                SModel = new SmsIcerikTablosuIslemler().SmsIcerikGuncelle(SIModel);

                if (SModel.Sonuc.Equals(Sonuclar.Basarili))
                {

                    BilgiKontrolMerkezi.UyariEkrani(this, $"$('#{UPnlSmsIcerikGuncelle.ClientID}').modal('hide'); setTimeout(() => {{ UyariBilgilendirme('', '<p>SMS içeriği güncellenmiştir.</p>', true); }}, 250);", false);
                }
                else
                {
                    BilgiKontrolMerkezi.UyariEkrani(this, $"AltSayfaUyariBilgilendirme('<p>SMS içeriği güncellenirken hata meydana geldi</p><p>Hata mesajı : {SModel.HataBilgi.HataMesaji}</p>', false);", false);
                }
            }
            else
            {
                BilgiKontrolMerkezi.UyariEkrani(this, $"AltSayfaUyariBilgilendirme('{Uyarilar}', false);", false);
            }
        }

        protected void lnkbtnYakaKartiGorselGuncelle_Click(object sender, EventArgs e)
        {
            YKCModel = new YakaKartiCerceveTablosuModel
            {
                YakaKartiCerceveID = Convert.ToInt32(lnkbtnYakaKartiGorselGuncelle.CommandArgument),
                Width = Kontrol.TamSayiyaKontrol(txtYKWidth.Text, "Genişlik boş bırakılamaz", "Geçerisiz genişlik girildi", ref Uyarilar),
                Height = Kontrol.TamSayiyaKontrol(txtYKHeight.Text, "Yükseklik boş bırakılamaz", "Geçerisiz yükseklik girildi", ref Uyarilar),
                YaziciKagitOrtalama = chkYaziciKagitOrtalama.Checked,
                GuncellenmeTarihi = Kontrol.Simdi()
            };

            if (YKCModel.Width > 210)
                Uyarilar.Append("<p>Yaka kartı genişliği en fazla 210 mm olabilir.</p>");

            if (YKCModel.Height > 297)
                Uyarilar.Append("<p>Yaka kartı yüksekliği en fazla 297mm olabilir.</p>");

            if (string.IsNullOrEmpty(Uyarilar.ToString()))
            {
                using (OleDbConnection cnn = ConnectionBuilder.DefaultConnection())
                {
                    ConnectionBuilder.OpenConnection(cnn);
                    using (OleDbTransaction trn = cnn.BeginTransaction())
                    {
                        SModel = new YakaKartiCerceveTablosuIslemler(trn).KayitGuncelle(YKCModel);

                        if (SModel.Sonuc.Equals(Sonuclar.Basarili))
                        {
                            try
                            {
                                if (!string.IsNullOrEmpty(hfYakaKartiGorsel.Value))
                                {
                                    using (MemoryStream ms = new MemoryStream(Convert.FromBase64String(hfYakaKartiGorsel.Value.Substring(hfYakaKartiGorsel.Value.IndexOf(",") + 1))))
                                    {
                                        using (System.Drawing.Image YakaKarti = System.Drawing.Image.FromStream(ms))
                                        {
                                            using (System.Drawing.Bitmap CompositeYakaKarti = new System.Drawing.Bitmap(YakaKarti, new System.Drawing.Size(Convert.ToInt32(1000 * YKCModel.Width / 210), Convert.ToInt32(1414 * YKCModel.Height / 297))))
                                            {
                                                CompositeYakaKarti.Save(Server.MapPath($"~/Dosyalar/YakaKartiGorsel/{YKCModel.YakaKartiCerceveID}.png"), System.Drawing.Imaging.ImageFormat.Png);
                                            }
                                        }
                                    }
                                }

                                trn.Commit();
                                BilgiKontrolMerkezi.UyariEkrani(this, $"$({UPnlYakaKartiGorselGuncelle.ClientID}).modal('hide'); setTimeout(() => {{ UyariBilgilendirme('', '<p>Yaka kartı çerçeve ölüçeleri ve görseli güncellendi</p>', true); }}, 250);", false);
                            }
                            catch (Exception ex)
                            {
                                trn.Rollback();
                                BilgiKontrolMerkezi.UyariEkrani(this, $"AltSayfaUyariBilgilendirme('<p>Yaka kartı görseli güncellenirken hata meydana geldi</p><p>Hata mesajı : {ex.Message.Replace("'", string.Empty).Replace("\r", string.Empty).Replace("\n", string.Empty).Replace("\t", string.Empty)}</p>', false);", false);
                            }
                        }
                        else
                        {
                            trn.Rollback();
                            BilgiKontrolMerkezi.UyariEkrani(this, $"AltSayfaUyariBilgilendirme('<p>Yaka kartı çerçeve ölçüleri güncellenirken hata meydana geldi</p><p>Hata mesajı : {SModel.HataBilgi.HataMesaji}</p>', false);", false);
                        }
                    }
                    ConnectionBuilder.CloseConnection(cnn);
                }
            }
            else
            {
                BilgiKontrolMerkezi.UyariEkrani(this, $"AltSayfaUyariBilgilendirme('{Uyarilar}', false);", false);
            }

            Kontrol.Temizle(hfYakaKartiGorsel);
        }

        protected void lnkbtnYakaKartiTipiEkle_Click(object sender, EventArgs e)
        {
            SModel = new YakaKartiIcerikTablosuIslemler().YeniKayitEkle(new YakaKartiIcerikTablosuModel
            {
                YakaKartiCerceveID = Convert.ToInt32(hfYakaKartiCerceveID.Value),
                YakaKartiIcerikTipiID = Convert.ToInt32(ddlYakaKartiIcerikTipi.SelectedValue),
                X = 0,
                Y = 0,
                Width = 1,
                Height = 1,
                GuncellenmeTarihi = Kontrol.Simdi(),
                EklenmeTarihi = Kontrol.Simdi()
            });

            if (SModel.Sonuc.Equals(Sonuclar.Basarili))
            {
                ddlYakaKartiIcerikTipi.DataBind();
                lnkbtnYakaKartiTipiEkle.Visible = !ddlYakaKartiIcerikTipi.Items.Count.Equals(0);


                ddlYakaKartiIcerik.DataBind();
                SDataYakaKartiIcerikModel = new YakaKartiIcerikTablosuIslemler().KayitBilgisi(Convert.ToInt32(ddlYakaKartiIcerik.SelectedValue));

                txtYKIX.Text = SDataYakaKartiIcerikModel.Veriler.X.ToString();
                txtYKIY.Text = SDataYakaKartiIcerikModel.Veriler.Y.ToString();
                txtYKIWidth.Text = SDataYakaKartiIcerikModel.Veriler.Width.ToString();
                txtYKIHeight.Text = SDataYakaKartiIcerikModel.Veriler.Height.ToString();
                txtYKIOran.Text = SDataYakaKartiIcerikModel.Veriler.YakaKartiIcerikTipiBilgisi.Oran.ToString();

                UPnlYakaKartiIcerikGuncelle.Update();
                BilgiKontrolMerkezi.UyariEkrani(this, $"setTimeout(() => {{ cropperBadgeSetUp(); }}, 250);", false);

                BilgiKontrolMerkezi.UyariEkrani(this, $"AltSayfaUyariBilgilendirme('<p>Yaka kartı alanı eklendi</p>', true);", false);
            }
            else
            {
                BilgiKontrolMerkezi.UyariEkrani(this, $"AltSayfaUyariBilgilendirme('<p>Yaka kartı alanı eklenirken hata meydana geldi</p><p>Hata mesajı : {SModel.HataBilgi.HataMesaji}</p>', false);", false);
            }
        }

        protected void ddlYakaKartiIcerik_SelectedIndexChanged(object sender, EventArgs e)
        {
            SDataYakaKartiIcerikModel = new YakaKartiIcerikTablosuIslemler().KayitBilgisi(Convert.ToInt32(ddlYakaKartiIcerik.SelectedValue));
            switch (SDataYakaKartiIcerikModel.Sonuc)
            {
                case Sonuclar.KismiBasarili:
                case Sonuclar.Basarisiz:
                    BilgiKontrolMerkezi.UyariEkrani(this, $"AltSayfaUyariBilgilendirme('<p>Yaka kartı içeriği bilgisi alınırken hata meydana geldi</p><p>Hata mesajı : {SDataMailIcerikModel.HataBilgi.HataMesaji}</p>', false);", false);
                    break;

                default:
                case Sonuclar.VeriBulunamadi:
                    BilgiKontrolMerkezi.UyariEkrani(this, $"AltSayfaUyariBilgilendirme('<p>Taka krtı içeriği bulunamadı</p>', false);", false);
                    break;


                case Sonuclar.Basarili:
                    txtYKIX.Text = SDataYakaKartiIcerikModel.Veriler.X.ToString();
                    txtYKIY.Text = SDataYakaKartiIcerikModel.Veriler.Y.ToString();
                    txtYKIWidth.Text = SDataYakaKartiIcerikModel.Veriler.Width.ToString();
                    txtYKIHeight.Text = SDataYakaKartiIcerikModel.Veriler.Height.ToString();
                    txtYKIOran.Text = SDataYakaKartiIcerikModel.Veriler.YakaKartiIcerikTipiBilgisi.Oran.ToString();

                    UPnlYakaKartiIcerikGuncelle.Update();
                    BilgiKontrolMerkezi.UyariEkrani(this, $"setTimeout(() => {{ cropperBadgeSetUp(); }}, 250);", false);

                    break;

            }
        }

        protected void rptDavetiyeIcerikListesi_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            switch (e.CommandName)
            {
                #region Davetiye İşlemleri

                case "DavetiyeGorselGuncelle":

                    SDataMailIcerikModel = new MailIcerikTablosuIslemler().KayitBilgisi(Convert.ToInt32(e.CommandArgument.ToString()));
                    switch (SDataMailIcerikModel.Sonuc)
                    {
                        case Sonuclar.KismiBasarili:
                        case Sonuclar.Basarisiz:
                            BilgiKontrolMerkezi.UyariEkrani(this, $"UyariBilgilendirme('', '<p>Mail içeriği bilgisi alınırken hata meydana geldi</p><p>Hata mesajı : {SDataMailIcerikModel.HataBilgi.HataMesaji}</p>', false);", false);
                            break;

                        default:
                        case Sonuclar.VeriBulunamadi:
                            BilgiKontrolMerkezi.UyariEkrani(this, $"UyariBilgilendirme('', '<p>Mail içeriği bulunamadı</p>', false);", false);
                            rptKatilimciTipiListesi.DataBind();
                            UPnlKatilimciTipiListesi.Update();
                            break;


                        case Sonuclar.Basarili:
                            h4_GorselGuncelle.InnerHtml = "Davetiye Görseli Güncelleme";
                            lnkbtnGorselGuncelle.CommandArgument = SDataMailIcerikModel.Veriler.MailIcerikID.ToString();

                            UPnlGorselGuncelle.Update();
                            BilgiKontrolMerkezi.UyariEkrani(this, $"$({UPnlGorselGuncelle.ClientID}).modal('show');", false);
                            break;
                    }

                    break;


                case "DavetiyeAntetliKagitIcerikGuncelle":

                    SDataMailIcerikModel = new MailIcerikTablosuIslemler().KayitBilgisi(Convert.ToInt32(e.CommandArgument.ToString()));
                    switch (SDataMailIcerikModel.Sonuc)
                    {
                        case Sonuclar.KismiBasarili:
                        case Sonuclar.Basarisiz:
                            BilgiKontrolMerkezi.UyariEkrani(this, $"UyariBilgilendirme('', '<p>Mail içeriği bilgisi alınırken hata meydana geldi</p><p>Hata mesajı : {SDataMailIcerikModel.HataBilgi.HataMesaji}</p>', false);", false);
                            break;

                        default:
                        case Sonuclar.VeriBulunamadi:
                            BilgiKontrolMerkezi.UyariEkrani(this, $"UyariBilgilendirme('', '<p>Mail içeriği bulunamadı</p>', false);", false);
                            rptKatilimciTipiListesi.DataBind();
                            UPnlKatilimciTipiListesi.Update();
                            break;


                        case Sonuclar.Basarili:

                            hfMailIcerikID.Value = SDataMailIcerikModel.Veriler.MailIcerikID.ToString();
                            ddlAntetliKagitIcerik.DataBind();
                            SDataAntetliKagitIcerikModel = new AntetliKagitIcerikTablosuIslemler().KayitBilgisi(Convert.ToInt32(ddlAntetliKagitIcerik.SelectedValue));
                            switch (SDataAntetliKagitIcerikModel.Sonuc)
                            {
                                case Sonuclar.KismiBasarili:
                                case Sonuclar.Basarisiz:
                                    BilgiKontrolMerkezi.UyariEkrani(this, $"UyariBilgilendirme('', '<p>Davetiye içeriği bilgisi alınırken hata meydana geldi</p><p>Hata mesajı : {SDataMailIcerikModel.HataBilgi.HataMesaji}</p>', false);", false);
                                    break;

                                default:
                                case Sonuclar.VeriBulunamadi:
                                    BilgiKontrolMerkezi.UyariEkrani(this, $"UyariBilgilendirme('', '<p>Davetiye içeriği bulunamadı</p>', false);", false);
                                    break;


                                case Sonuclar.Basarili:
                                    h4_AntetliKagitIcerikGuncelle.InnerHtml = "Davetiye Görsel İçerik Güncelle";
                                    ImgAntetliKagit.ImageUrl = $"~/Dosyalar/MailIcerikGorsel/{SDataMailIcerikModel.Veriler.MailIcerikID}.png?t={DateTime.Now.Ticks}";
                                    txtX.Text = SDataAntetliKagitIcerikModel.Veriler.X.ToString();
                                    txtY.Text = SDataAntetliKagitIcerikModel.Veriler.Y.ToString();
                                    txtWidth.Text = SDataAntetliKagitIcerikModel.Veriler.Width.ToString();
                                    txtHeight.Text = SDataAntetliKagitIcerikModel.Veriler.Height.ToString();
                                    txtOran.Text = SDataAntetliKagitIcerikModel.Veriler.AntetliKagitIcerikTipiBilgisi.Oran.ToString();

                                    UPnlAntetliKagitIcerikGuncelle.Update();
                                    BilgiKontrolMerkezi.UyariEkrani(this, $"$({UPnlAntetliKagitIcerikGuncelle.ClientID}).modal('show'); setTimeout(() => {{ cropperSetUp(); }}, 250);", false);
                                    break;
                            }

                            break;
                    }

                    break;

                case "DavetiyeHtmlIcerikGuncelle":

                    SDataMailIcerikModel = new MailIcerikTablosuIslemler().KayitBilgisi(Convert.ToInt32(e.CommandArgument.ToString()));
                    switch (SDataMailIcerikModel.Sonuc)
                    {
                        case Sonuclar.KismiBasarili:
                        case Sonuclar.Basarisiz:
                            BilgiKontrolMerkezi.UyariEkrani(this, $"UyariBilgilendirme('', '<p>Mail içeriği bilgisi alınırken hata meydana geldi</p><p>Hata mesajı : {SDataMailIcerikModel.HataBilgi.HataMesaji}</p>', false);", false);
                            break;

                        default:
                        case Sonuclar.VeriBulunamadi:
                            BilgiKontrolMerkezi.UyariEkrani(this, $"UyariBilgilendirme('', '<p>Mail içeriği bulunamadı</p>', false);", false);
                            rptKatilimciTipiListesi.DataBind();
                            UPnlKatilimciTipiListesi.Update();
                            break;


                        case Sonuclar.Basarili:
                            h4_MailIcerikGuncelle.InnerHtml = "Davetiye Mail İçeriği Güncelleme";
                            txtKonu.Text = SDataMailIcerikModel.Veriler.Konu;
                            txtMailIcerik.Text = SDataMailIcerikModel.Veriler.HtmlIcerik;
                            lnkbtnMailIcerikGuncelle.CommandArgument = SDataMailIcerikModel.Veriler.MailIcerikID.ToString();

                            UPnlMailIcerikGuncelle.Update();
                            BilgiKontrolMerkezi.UyariEkrani(this, $"$({UPnlMailIcerikGuncelle.ClientID}).modal('show');", false);
                            break;
                    }

                    break;


                case "DavetiyeSmsIcerikGuncelle":
                    string[] args = e.CommandArgument.ToString().Split(',');

                    SDataSmsIcerikModel = new SmsIcerikTablosuIslemler().KayitBilgisi(args[0], Convert.ToBoolean(args[1]), Convert.ToInt32(args[2]));
                    switch (SDataSmsIcerikModel.Sonuc)
                    {
                        case Sonuclar.KismiBasarili:
                        case Sonuclar.Basarisiz:
                            BilgiKontrolMerkezi.UyariEkrani(this, $"UyariBilgilendirme('', '<p>SMS içeriği bilgisi alınırken hata meydana geldi</p><p>Hata mesajı : {SDataSmsIcerikModel.HataBilgi.HataMesaji}</p>', false);", false);
                            break;

                        default:
                        case Sonuclar.VeriBulunamadi:
                            BilgiKontrolMerkezi.UyariEkrani(this, $"UyariBilgilendirme('', '<p>SMS içeriği bulunamadı</p>', false);", false);
                            rptKatilimciTipiListesi.DataBind();
                            UPnlKatilimciTipiListesi.Update();
                            break;


                        case Sonuclar.Basarili:
                            h4_SmsIcerikGuncelle.InnerHtml = "Davetiye Sms Içeriği Güncelleme";
                            txtSmsIcerigi.Text = SDataSmsIcerikModel.Veriler.SmsIcerik;
                            lnkbtnSmsIcerikGuncelle.CommandArgument = SDataSmsIcerikModel.Veriler.SmsIcerikID.ToString();

                            PnlDavetiyeSmsIcerik.Visible = true;
                            PnlQRSmsUcerik.Visible = !PnlDavetiyeSmsIcerik.Visible;

                            UPnlSmsIcerikGuncelle.Update();
                            BilgiKontrolMerkezi.UyariEkrani(this, $"$({UPnlSmsIcerikGuncelle.ClientID}).modal('show');", false);
                            break;
                    }

                    break;

                #endregion
                default:
                    break;
            }
        }

        protected void rptQRIcerikListesi_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            switch (e.CommandName)
            {
                #region QR İşlemleri

                case "QRGorselGuncelle":

                    SDataMailIcerikModel = new MailIcerikTablosuIslemler().KayitBilgisi(Convert.ToInt32(e.CommandArgument.ToString()));
                    switch (SDataMailIcerikModel.Sonuc)
                    {
                        case Sonuclar.KismiBasarili:
                        case Sonuclar.Basarisiz:
                            BilgiKontrolMerkezi.UyariEkrani(this, $"UyariBilgilendirme('', '<p>Mail içeriği bilgisi alınırken hata meydana geldi</p><p>Hata mesajı : {SDataMailIcerikModel.HataBilgi.HataMesaji}</p>', false);", false);
                            break;

                        default:
                        case Sonuclar.VeriBulunamadi:
                            BilgiKontrolMerkezi.UyariEkrani(this, $"UyariBilgilendirme('', '<p>Mail içeriği bulunamadı</p>', false);", false);
                            rptKatilimciTipiListesi.DataBind();
                            UPnlKatilimciTipiListesi.Update();
                            break;


                        case Sonuclar.Basarili:
                            h4_GorselGuncelle.InnerHtml = "QR Görseli Güncelleme";
                            lnkbtnGorselGuncelle.CommandArgument = SDataMailIcerikModel.Veriler.MailIcerikID.ToString();

                            UPnlGorselGuncelle.Update();
                            BilgiKontrolMerkezi.UyariEkrani(this, $"$({UPnlGorselGuncelle.ClientID}).modal('show');", false);
                            break;
                    }

                    break;

                case "QRAntetliKagitIcerikGuncelle":

                    SDataMailIcerikModel = new MailIcerikTablosuIslemler().KayitBilgisi(Convert.ToInt32(e.CommandArgument.ToString()));
                    switch (SDataMailIcerikModel.Sonuc)
                    {
                        case Sonuclar.KismiBasarili:
                        case Sonuclar.Basarisiz:
                            BilgiKontrolMerkezi.UyariEkrani(this, $"UyariBilgilendirme('', '<p>Mail içeriği bilgisi alınırken hata meydana geldi</p><p>Hata mesajı : {SDataMailIcerikModel.HataBilgi.HataMesaji}</p>', false);", false);
                            break;

                        default:
                        case Sonuclar.VeriBulunamadi:
                            BilgiKontrolMerkezi.UyariEkrani(this, $"UyariBilgilendirme('', '<p>Mail içeriği bulunamadı</p>', false);", false);
                            rptKatilimciTipiListesi.DataBind();
                            UPnlKatilimciTipiListesi.Update();
                            break;


                        case Sonuclar.Basarili:
                            hfMailIcerikID.Value = SDataMailIcerikModel.Veriler.MailIcerikID.ToString();

                            ddlAntetliKagitIcerik.DataBind();
                            SDataAntetliKagitIcerikModel = new AntetliKagitIcerikTablosuIslemler().KayitBilgisi(Convert.ToInt32(ddlAntetliKagitIcerik.SelectedValue));

                            switch (SDataAntetliKagitIcerikModel.Sonuc)
                            {
                                case Sonuclar.KismiBasarili:
                                case Sonuclar.Basarisiz:
                                    BilgiKontrolMerkezi.UyariEkrani(this, $"UyariBilgilendirme('', '<p>QR içeriği bilgisi alınırken hata meydana geldi</p><p>Hata mesajı : {SDataMailIcerikModel.HataBilgi.HataMesaji}</p>', false);", false);
                                    break;

                                default:
                                case Sonuclar.VeriBulunamadi:
                                    BilgiKontrolMerkezi.UyariEkrani(this, $"UyariBilgilendirme('', '<p>QR içeriği bulunamadı</p>', false);", false);
                                    break;


                                case Sonuclar.Basarili:
                                    h4_AntetliKagitIcerikGuncelle.InnerHtml = "QR Görsel İçerik Güncelle";
                                    ImgAntetliKagit.ImageUrl = $"~/Dosyalar/MailIcerikGorsel/{SDataMailIcerikModel.Veriler.MailIcerikID}.png?t={DateTime.Now.Ticks}";
                                    txtX.Text = SDataAntetliKagitIcerikModel.Veriler.X.ToString();
                                    txtY.Text = SDataAntetliKagitIcerikModel.Veriler.Y.ToString();
                                    txtWidth.Text = SDataAntetliKagitIcerikModel.Veriler.Width.ToString();
                                    txtHeight.Text = SDataAntetliKagitIcerikModel.Veriler.Height.ToString();
                                    txtOran.Text = SDataAntetliKagitIcerikModel.Veriler.AntetliKagitIcerikTipiBilgisi.Oran.ToString();

                                    UPnlAntetliKagitIcerikGuncelle.Update();
                                    BilgiKontrolMerkezi.UyariEkrani(this, $"$({UPnlAntetliKagitIcerikGuncelle.ClientID}).modal('show'); setTimeout(() => {{ cropperSetUp(); }}, 250);", false);
                                    break;
                            }


                            break;
                    }

                    break;


                case "QRHtmlIcerikGuncelle":

                    SDataMailIcerikModel = new MailIcerikTablosuIslemler().KayitBilgisi(Convert.ToInt32(e.CommandArgument.ToString()));
                    switch (SDataMailIcerikModel.Sonuc)
                    {
                        case Sonuclar.KismiBasarili:
                        case Sonuclar.Basarisiz:
                            BilgiKontrolMerkezi.UyariEkrani(this, $"UyariBilgilendirme('', '<p>Mail içeriği bilgisi alınırken hata meydana geldi</p><p>Hata mesajı : {SDataMailIcerikModel.HataBilgi.HataMesaji}</p>', false);", false);
                            break;

                        default:
                        case Sonuclar.VeriBulunamadi:
                            BilgiKontrolMerkezi.UyariEkrani(this, $"UyariBilgilendirme('', '<p>Mail içeriği bulunamadı</p>', false);", false);
                            rptKatilimciTipiListesi.DataBind();
                            UPnlKatilimciTipiListesi.Update();
                            break;


                        case Sonuclar.Basarili:
                            h4_MailIcerikGuncelle.InnerHtml = "QR Mail İçeriği Güncelleme";
                            txtKonu.Text = SDataMailIcerikModel.Veriler.Konu;
                            txtMailIcerik.Text = SDataMailIcerikModel.Veriler.HtmlIcerik;
                            lnkbtnMailIcerikGuncelle.CommandArgument = SDataMailIcerikModel.Veriler.MailIcerikID.ToString();

                            UPnlMailIcerikGuncelle.Update();
                            BilgiKontrolMerkezi.UyariEkrani(this, $"$({UPnlMailIcerikGuncelle.ClientID}).modal('show');", false);
                            break;
                    }

                    break;

                case "QRSmsIcerikGuncelle":

                    string[] args = e.CommandArgument.ToString().Split(',');
                    SDataSmsIcerikModel = new SmsIcerikTablosuIslemler().KayitBilgisi(args[0], Convert.ToBoolean(args[1]), Convert.ToInt32(args[2]));

                    switch (SDataSmsIcerikModel.Sonuc)
                    {
                        case Sonuclar.KismiBasarili:
                        case Sonuclar.Basarisiz:
                            BilgiKontrolMerkezi.UyariEkrani(this, $"UyariBilgilendirme('', '<p>SMS içeriği bilgisi alınırken hata meydana geldi</p><p>Hata mesajı : {SDataSmsIcerikModel.HataBilgi.HataMesaji}</p>', false);", false);
                            break;

                        default:
                        case Sonuclar.VeriBulunamadi:
                            BilgiKontrolMerkezi.UyariEkrani(this, $"UyariBilgilendirme('', '<p>SMS içeriği bulunamadı</p>', false);", false);
                            rptKatilimciTipiListesi.DataBind();
                            UPnlKatilimciTipiListesi.Update();
                            break;


                        case Sonuclar.Basarili:
                            h4_SmsIcerikGuncelle.InnerHtml = "QR Sms Içeriği Güncelleme";
                            txtSmsIcerigi.Text = SDataSmsIcerikModel.Veriler.SmsIcerik;
                            lnkbtnSmsIcerikGuncelle.CommandArgument = SDataSmsIcerikModel.Veriler.SmsIcerikID.ToString();

                            PnlDavetiyeSmsIcerik.Visible = false;
                            PnlQRSmsUcerik.Visible = !PnlDavetiyeSmsIcerik.Visible;

                            UPnlSmsIcerikGuncelle.Update();
                            BilgiKontrolMerkezi.UyariEkrani(this, $"$({UPnlSmsIcerikGuncelle.ClientID}).modal('show');", false);
                            break;
                    }

                    break;

                #endregion
                default:
                    break;
            }
        }

        protected void lnkbtnYakaKartiIcerikSil_Click(object sender, EventArgs e)
        {
            SModel = new YakaKartiIcerikTablosuIslemler().KayitSil(Convert.ToInt32(ddlYakaKartiIcerik.SelectedValue));

            if (SModel.Sonuc.Equals(Sonuclar.Basarili))
            {
                ddlYakaKartiIcerikTipi.DataBind();
                lnkbtnYakaKartiTipiEkle.Visible = !ddlYakaKartiIcerikTipi.Items.Count.Equals(0);


                ddlYakaKartiIcerik.DataBind();

                SDataYakaKartiIcerikModel = new YakaKartiIcerikTablosuIslemler().KayitBilgisi(Convert.ToInt32(ddlYakaKartiIcerik.SelectedValue));

                txtYKIX.Text = SDataYakaKartiIcerikModel.Veriler.X.ToString();
                txtYKIY.Text = SDataYakaKartiIcerikModel.Veriler.Y.ToString();
                txtYKIWidth.Text = SDataYakaKartiIcerikModel.Veriler.Width.ToString();
                txtYKIHeight.Text = SDataYakaKartiIcerikModel.Veriler.Height.ToString();
                txtYKIOran.Text = SDataYakaKartiIcerikModel.Veriler.YakaKartiIcerikTipiBilgisi.Oran.ToString();

                lnkbtnYakaKartiIcerikSil.Visible = !ddlYakaKartiIcerik.Items.Count.Equals(1);

                UPnlYakaKartiIcerikGuncelle.Update();
                BilgiKontrolMerkezi.UyariEkrani(this, $"setTimeout(() => {{ cropperBadgeSetUp(); }}, 250);", false);

                BilgiKontrolMerkezi.UyariEkrani(this, $"AltSayfaUyariBilgilendirme('<p>Yaka kartı alanı eklendi</p>', true);", false);
            }
            else
            {
                BilgiKontrolMerkezi.UyariEkrani(this, $"AltSayfaUyariBilgilendirme('<p>Yaka kartı içeriği bilgisi silinirken hata meydana geldi</p><p>Hata mesajı : {SModel.HataBilgi.HataMesaji}</p>', false);", false);
            }
        }

        protected void lnkbtnYakaKartiIcerikGuncelle_Click(object sender, EventArgs e)
        {
            YKIModel = new YakaKartiIcerikTablosuModel
            {
                YakaKartiIcerikID = Convert.ToInt32(ddlYakaKartiIcerik.SelectedValue),
                X = Kontrol.TamSayiyaKontrol(txtYKIX, "X değeri boş bırakılamaz", "Geçersiz X değeri girildi", ref Uyarilar),
                Y = Kontrol.TamSayiyaKontrol(txtYKIY, "Y değeri boş bırakılamaz", "Geçersiz Y değeri girildi", ref Uyarilar),
                Width = Kontrol.TamSayiyaKontrol(txtYKIWidth, "Genişlik boş bırakılamaz", "Geçersiz genişlik değeri girildi", ref Uyarilar),
                Height = Kontrol.TamSayiyaKontrol(txtYKIHeight, "Yükseklik değeri boş bırakılamaz", "Geçersiz yükseklik değeri girildi", ref Uyarilar),
                GuncellenmeTarihi = Kontrol.Simdi()
            };

            if (string.IsNullOrEmpty(Uyarilar.ToString()))
            {
                SModel = new YakaKartiIcerikTablosuIslemler().KayitGuncelle(YKIModel);

                if (SModel.Sonuc.Equals(Sonuclar.Basarili))
                {
                    BilgiKontrolMerkezi.UyariEkrani(this, $"AltSayfaUyariBilgilendirme('<p>Yaka kartı içeriği güncellendi</p>', true);", false);
                }
                else
                {
                    BilgiKontrolMerkezi.UyariEkrani(this, $"AltSayfaUyariBilgilendirme('<p>Yaka kartı içeriği güncellenirken hata meydana geldi</p><p>Hata mesajı : {SModel.HataBilgi.HataMesaji}</p>', false);", false);
                }

            }
            else
            {
                BilgiKontrolMerkezi.UyariEkrani(this, $"AltSayfaUyariBilgilendirme('{Uyarilar}', false);", false);
            }
        }

        protected void lnkbtnKarsilamaEkraniGorsel_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(hfKarsilamaEkraniGorsel.Value))
            {
                try
                {
                    using (MemoryStream ms = new MemoryStream(Convert.FromBase64String(hfKarsilamaEkraniGorsel.Value.Substring(hfKarsilamaEkraniGorsel.Value.IndexOf(",") + 1))))
                    {
                        using (System.Drawing.Image Img = System.Drawing.Image.FromStream(ms))
                        {
                            Img.Save(Server.MapPath($"~/Dosyalar/KarsilamaEkrani/{lnkbtnKarsilamaEkraniGorsel.CommandArgument}.jpg"), System.Drawing.Imaging.ImageFormat.Jpeg);
                        }
                    }

                    BilgiKontrolMerkezi.UyariEkrani(this, $"$({UPnlKarsilamaEkraniGorselGuncelle.ClientID}).modal('hide'); setTimeout(() => {{ UyariBilgilendirme('', '<p>karşılama ekran görseliniz  güncellendi</p>', true); }}, 250);", false);
                }
                catch (Exception ex)
                {
                    BilgiKontrolMerkezi.UyariEkrani(this, $"AltSayfaUyariBilgilendirme('<p>Görseliniz kaydedilirken hata meydana geldi</p><p>Hata mesajı : {ex.Message.Replace("'", string.Empty).Replace("\t", string.Empty).Replace("\r", string.Empty).Replace("\n", string.Empty)}</p>', false);", false);
                }
            }
            else
            {
                BilgiKontrolMerkezi.UyariEkrani(this, $"AltSayfaUyariBilgilendirme('<p>Lütfen güncllenecek olan görselinizi seçiniz</p>', false);", false);
            }

            Kontrol.Temizle(hfKarsilamaEkraniGorsel);
        }

        protected void lnkbtnLogoGuncelle_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(hfLogo.Value))
            {
                try
                {
                    using (MemoryStream ms = new MemoryStream(Convert.FromBase64String(hfLogo.Value.Substring(hfLogo.Value.IndexOf(",") + 1))))
                    {
                        using (System.Drawing.Image Img = System.Drawing.Image.FromStream(ms))
                        {
                            Img.Save(Server.MapPath($"~/Dosyalar/Logo/{lnkbtnLogoGuncelle.CommandArgument}.png"), System.Drawing.Imaging.ImageFormat.Png);
                        }
                    }

                    BilgiKontrolMerkezi.UyariEkrani(this, $"$({UPnlLogoGuncelle.ClientID}).modal('hide'); setTimeout(() => {{ UyariBilgilendirme('', '<p>karşılama ekran görseliniz  güncellendi</p>', true); }}, 250);", false);
                }
                catch (Exception ex)
                {
                    BilgiKontrolMerkezi.UyariEkrani(this, $"AltSayfaUyariBilgilendirme('<p>Görseliniz kaydedilirken hata meydana geldi</p><p>Hata mesajı : {ex.Message.Replace("'", string.Empty).Replace("\t", string.Empty).Replace("\r", string.Empty).Replace("\n", string.Empty)}</p>', false);", false);
                }
            }
            else
            {
                BilgiKontrolMerkezi.UyariEkrani(this, $"AltSayfaUyariBilgilendirme('<p>Lütfen güncllenecek olan görselinizi seçiniz</p>', false);", false);
            }

            Kontrol.Temizle(hfKarsilamaEkraniGorsel);
        }

        protected void lnkbtnEkranIcerikGuncelle_Click(object sender, EventArgs e)
        {
            string Icerik = Kontrol.KelimeKontrol(txtEkranIcerik, "İçerik yazısı boş bırakılamaz", ref Uyarilar);

            if (string.IsNullOrEmpty(Uyarilar.ToString()))
            {
                switch (td_IcerikBaslik.Attributes["data-durum"])
                {
                    case "Kabul":
                        SModel = new KatilimciTipiTablosuIslemler().KabulEkranIcerikGuncelle(lnkbtnEkranIcerikGuncelle.CommandArgument, Icerik);
                        break;

                    case "Red":
                        SModel = new KatilimciTipiTablosuIslemler().RedEkranIcerikGuncelle(lnkbtnEkranIcerikGuncelle.CommandArgument, Icerik);
                        break;
                }


                if (SModel.Sonuc.Equals(Sonuclar.Basarili))
                {
                    BilgiKontrolMerkezi.UyariEkrani(this, $"$({UPnlEkranIcerik.ClientID}).modal('hide'); setTimeout(() => {{ UyariBilgilendirme('', '<p>İçerik yazısı güncellendi</p>', true); }}, 250);", false);
                }
                else
                {
                    BilgiKontrolMerkezi.UyariEkrani(this, $"AltSayfaUyariBilgilendirme('<p>İçerik yazısı güncellenirken hata meydana geldi</p><p>Hata mesajı : {SModel.HataBilgi.HataMesaji}</p>', false);", false);
                }
            }
            else
            {
                BilgiKontrolMerkezi.UyariEkrani(this, $"AltSayfaUyariBilgilendirme('{Uyarilar}', false);", false);
            }
        }
    }
}