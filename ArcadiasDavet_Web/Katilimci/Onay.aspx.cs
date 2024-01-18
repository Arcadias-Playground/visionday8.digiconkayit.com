using DocumentFormat.OpenXml.EMMA;
using Microsoft.AspNet.FriendlyUrls;
using Model;
using QRCoder;
using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI;
using VeritabaniIslemMerkezi;

namespace ArcadiasDavet_Web.Katilimci
{
    public partial class Onay : Page
    {
        IList<string> segments;

        StringBuilder Uyarilar = new StringBuilder();

        SurecBilgiModel SModel;
        SurecVeriModel<MailGonderimTablosuModel> SDataMailModel;
        SurecVeriModel<SmsGonderimTablosuModel> SDataSmsModel;

        public const string IletisimMail = "....@.....";
        string style = "<style>html { background: url({0}); }</style>";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                segments = Request.GetFriendlyUrlSegments();

                if (segments.Count.Equals(2))
                {
                    if (segments[0].Equals("email", StringComparison.OrdinalIgnoreCase))
                    {
                        SDataMailModel = new MailGonderimTablosuIslemler().KayitBilgisi(segments[1]);

                        if (SDataMailModel.Sonuc.Equals(Sonuclar.Basarili))
                        {
                            if (SDataMailModel.Veriler.KatilimciBilgisi.YoneticiOnay)
                            {
                                ImgLogo.ImageUrl = $"~/Dosyalar/Logo/{SDataMailModel.Veriler.KatilimciBilgisi.KatilimciTipiID}.png?t={DateTime.Now.Ticks}";
                                div_Icerik.InnerHtml += style.Replace("{0}", ResolveClientUrl($"~/Dosyalar/KarsilamaEkrani/{SDataMailModel.Veriler.KatilimciBilgisi.KatilimciTipiBilgisi.KatilimciTipiID}.jpg?t={DateTime.Now.Ticks}"));

                                if (!SDataMailModel.Veriler.KatilimciBilgisi.KatilimciOnayTarihi.HasValue)
                                {
                                    if (SDataMailModel.Veriler.EklenmeTarihi.AddMinutes(5) <= DateTime.Now)
                                    {
                                        SModel = new KatilimciTablosuIslemler().KatilimciOnay(SDataMailModel.Veriler.KatilimciID, true);
                                        if (SModel.Sonuc.Equals(Sonuclar.Basarisiz))
                                            Uyarilar.Append($"<p>Onay kaydınız sırasında hata meydana geldi. Lütfen {IletisimMail} ile iletişime geçiniz</p><p>Hata mesajı : {SModel.HataBilgi.HataMesaji}</p>");

                                        SModel = new MailGonderimIslemleri().MailGonderim(new KatilimciTablosuIslemler().KayitBilgisi(SDataMailModel.Veriler.KatilimciID, "email", 2).Veriler);
                                        if (SModel.Sonuc.Equals(Sonuclar.Basarisiz))
                                            Uyarilar.Append($"<p>QR mailiniz gönderilirken hata meydana geldi</p><p>Hata mesajı : {SModel.HataBilgi.HataMesaji}</p>");

                                        if (!string.IsNullOrEmpty(Uyarilar.ToString()))
                                            BilgiKontrolMerkezi.UyariEkrani(this, $"UyariBilgilendirme('', '{Uyarilar}', false);", true, true);

                                        QREkraniHazirlama(SDataMailModel.Veriler.KatilimciBilgisi);
                                    }
                                    else
                                    {
                                        BilgiKontrolMerkezi.UyariEkrani(this, $"UyariBilgilendirme('', '<p class=\"text-center\">Sayın {SDataMailModel.Veriler.KatilimciBilgisi.AdSoyad}</p><p class=\"text-center\">Geri bildirim linkiniz \"<u>{SDataMailModel.Veriler.EklenmeTarihi.AddMinutes(6):dd.MM.yyyy HH:mm}</u>\" saatinde aktif olacaktır.</p><p>Tarafınıza gönderilen davetiye mailine tekrar girerek, katılıyorum butonuna basabilirsiniz.</p>', false);", true, true);
                                        return;
                                    }
                                }
                                else
                                {
                                    if (SDataMailModel.Veriler.KatilimciBilgisi.KatilimciOnay)
                                    {
                                        QREkraniHazirlama(SDataMailModel.Veriler.KatilimciBilgisi);
                                    }
                                    else
                                    {
                                        BilgiKontrolMerkezi.UyariEkrani(this, $"UyariBilgilendirme('', '<p class=\"text-center\">Sayın {SDataMailModel.Veriler.KatilimciBilgisi.AdSoyad}</p><p>{SDataMailModel.Veriler.KatilimciBilgisi.KatilimciOnayTarihi:dd.MM.yyyy HH:mm} tarihinde vermiş olduğunuz katılımıyorum cevabınızdan ötürü QR kod oluşturamıyoruz.</p><p>Katılım kararınızın değiştirdiyseniz lütfen {IletisimMail} iletişime geçin</p>', false);", true);
                                    }
                                }
                            }
                            else
                            {
                                BilgiKontrolMerkezi.UyariEkrani(this, $"UyariBilgilendirme('', '<p>Yönetici onayı olmadığından QR oluşturamıyoruz.</p>', false); $('.modal-footer').remove();", true, true);
                            }
                        }
                        else
                        {
                            BilgiKontrolMerkezi.UyariEkrani(this, $"UyariBilgilendirme('', '<p>Katılımcı bilgileriniz alınırken hata meydana geldi</p><p>Hata mesajı : {SDataMailModel.HataBilgi.HataMesaji}</p>', false); $('.modal-footer').remove();", true, true);
                        }
                    }
                    else if(segments[0].Equals("sms", StringComparison.OrdinalIgnoreCase))
                    {
                        SDataSmsModel = new SmsGonderimTablosuIslemler().KayitBilgisi(segments[1]);

                        if (SDataSmsModel.Sonuc.Equals(Sonuclar.Basarili))
                        {
                            if (SDataSmsModel.Veriler.KatilimciBilgisi.YoneticiOnay)
                            {
                                ImgLogo.ImageUrl = $"~/Dosyalar/Logo/{SDataSmsModel.Veriler.KatilimciBilgisi.KatilimciTipiID}.png?t={DateTime.Now.Ticks}";
                                div_Icerik.InnerHtml += style.Replace("{0}", ResolveClientUrl($"~/Dosyalar/KarsilamaEkrani/{SDataSmsModel.Veriler.KatilimciBilgisi.KatilimciTipiBilgisi.KatilimciTipiID}.jpg?t={DateTime.Now.Ticks}"));

                                if (!SDataSmsModel.Veriler.KatilimciBilgisi.KatilimciOnayTarihi.HasValue)
                                {
                                    SModel = new KatilimciTablosuIslemler().KatilimciOnay(SDataSmsModel.Veriler.KatilimciID, true);
                                    if (SModel.Sonuc.Equals(Sonuclar.Basarisiz))
                                        Uyarilar.Append($"<p>Onay kaydınız sırasında hata meydana geldi. Lütfen {IletisimMail} ile iletişime geçiniz</p><p>Hata mesajı : {SModel.HataBilgi.HataMesaji}</p>");

                                    SModel = new SmsGonderimIslemleri().SmsGonderim(new KatilimciTablosuIslemler().KayitBilgisi(SDataSmsModel.Veriler.KatilimciID, "sms", 2).Veriler);
                                    if (SModel.Sonuc.Equals(Sonuclar.Basarisiz))
                                        Uyarilar.Append($"<p>QR mailiniz gönderilirken hata meydana geldi</p><p>Hata mesajı : {SModel.HataBilgi.HataMesaji}</p>");

                                    if (!string.IsNullOrEmpty(Uyarilar.ToString()))
                                        BilgiKontrolMerkezi.UyariEkrani(this, $"UyariBilgilendirme('', '{Uyarilar}', false);", true, true);

                                    QREkraniHazirlama(SDataSmsModel.Veriler.KatilimciBilgisi);
                                }
                                else
                                {
                                    if (SDataSmsModel.Veriler.KatilimciBilgisi.KatilimciOnay)
                                    {
                                        QREkraniHazirlama(SDataSmsModel.Veriler.KatilimciBilgisi);
                                    }
                                    else
                                    {
                                        BilgiKontrolMerkezi.UyariEkrani(this, $"UyariBilgilendirme('', '<p class=\"text-center\">Sayın {SDataSmsModel.Veriler.KatilimciBilgisi.AdSoyad}</p><p>{SDataSmsModel.Veriler.KatilimciBilgisi.KatilimciOnayTarihi:dd.MM.yyyy HH:mm} tarihinde vermiş olduğunuz katılımıyorum cevabınızdan ötürü QR kod oluşturamıyoruz.</p><p>Katılım kararınızın değiştirdiyseniz lütfen {IletisimMail} iletişime geçin</p>', false);", true);
                                    }
                                }
                            }
                            else
                            {
                                BilgiKontrolMerkezi.UyariEkrani(this, $"UyariBilgilendirme('', '<p>Yönetici onayı olmadığından QR oluşturamıyoruz.</p>', false); $('.modal-footer').remove();", true, true);
                            }
                        }
                        else
                        {
                            BilgiKontrolMerkezi.UyariEkrani(this, $"UyariBilgilendirme('', '<p>Katılımcı bilgileriniz alınırken hata meydana geldi</p><p>Hata mesajı : {SDataSmsModel.HataBilgi.HataMesaji}</p>', false); $('.modal-footer').remove();", true, true);
                        }
                    }
                    else
                    {
                        BilgiKontrolMerkezi.UyariEkrani(this, $"UyariBilgilendirme('', '<p class=\"text-center\">Geçersiz link girildi.</p>', false); $('.modal-footer').remove();", true, true);
                    }
                }
                else
                {
                    BilgiKontrolMerkezi.UyariEkrani(this, $"UyariBilgilendirme('', '<p class=\"text-center\">Geçersiz link girildi.</p>', false); $('.modal-footer').remove();", true, true);
                }
            }
        }

        protected void QREkraniHazirlama(KatilimciTablosuModel KModel)
        {
            lblAdSoyad.Text = KModel.AdSoyad;
            using (QRCodeGenerator qRCodeGenerator = new QRCodeGenerator())
            {
                using (QRCodeData qRCodeData = qRCodeGenerator.CreateQrCode(KModel.KatilimciID, QRCodeGenerator.ECCLevel.M))
                {
                    using (Base64QRCode qrCode = new Base64QRCode(qRCodeData))
                    {
                        ImgQR.ImageUrl = $"data:image/png;base64,{qrCode.GetGraphic(20)}";
                        ImgQR.Visible = true;
                    }
                }
            }

           

            PnlMisafirKayit.Visible = !KModel.KatilimciTipiBilgisi.MisafirKontenjan.Equals(0) && string.IsNullOrEmpty(KModel.AnaKatilimciID) && new KatilimciTablosuIslemler().MisafirKontrol(KModel.KatilimciID);
            hyplnkMisafir.NavigateUrl = $"~/Katilimci/Misafir/{KModel.KatilimciID}";

            div_Icerik.InnerHtml += KModel.KatilimciTipiBilgisi.KabulEkranIcerik;
            /*
            ImgLogo.ImageUrl = $"~/Dosyalar/Logo/{KModel.KatilimciTipiID}.png?t={DateTime.Now.Ticks}";
            div_Icerik.InnerHtml += style.Replace("{0}", ResolveClientUrl($"~/Dosyalar/KarsilamaEkrani/{KModel.KatilimciTipiBilgisi.KatilimciTipiID}.jpg?t={DateTime.Now.Ticks}"));
            */

            if (!string.IsNullOrEmpty(KModel.AnaKatilimciID))
            {
                PnlAnaKatilimci.Visible = true;
                lblAnaKatilimci.Text = new KatilimciTablosuIslemler().KayitBilgisi(KModel.AnaKatilimciID).Veriler.AdSoyad;
            }
        }
    }
}