using Microsoft.AspNet.FriendlyUrls;
using Model;
using System;
using System.Collections.Generic;
using System.Web.UI;
using VeritabaniIslemMerkezi;

namespace ArcadiasDavet_Web.Katilimci
{
    public partial class Red : Page
    {
        IList<string> segments;

        SurecBilgiModel SModel;
        SurecVeriModel<MailGonderimTablosuModel> SDataMailModel;
        SurecVeriModel<SmsGonderimTablosuModel> SDataSmsModel;


        string style = "<style>html { background: url({0}); }</style>";
        protected void Page_Load(object sender, EventArgs e)
        {
            segments = Request.GetFriendlyUrlSegments();

            if (segments.Count.Equals(2))
            {
                if (segments[0].Equals("email", StringComparison.OrdinalIgnoreCase))
                {
                    SDataMailModel = new MailGonderimTablosuIslemler().KayitBilgisi(segments[1]);

                    if (SDataMailModel.Sonuc.Equals(Sonuclar.Basarili))
                    {
                        if (!SDataMailModel.Veriler.KatilimciBilgisi.KatilimciOnayTarihi.HasValue)
                        {
                            if (SDataMailModel.Veriler.EklenmeTarihi.AddMinutes(5) <= DateTime.Now)
                            {
                                SModel = new KatilimciTablosuIslemler().KatilimciOnay(SDataMailModel.Veriler.KatilimciID, false);

                                if (!SModel.Sonuc.Equals(Sonuclar.Basarili))
                                    BilgiKontrolMerkezi.UyariEkrani(this, $"UyariBilgilendirme('', '<p>Geri bildiriminiz kaydedilirken hata meydana geldi</p><p>Hata mesajı : {SModel.HataBilgi.HataMesaji}</p>', false);", true, true);

                                lblAdSoyad.Text = SDataMailModel.Veriler.KatilimciBilgisi.AdSoyad;

                                ImgLogo.ImageUrl = $"~/Dosyalar/Logo/{SDataMailModel.Veriler.KatilimciBilgisi.KatilimciTipiID}.png?t={DateTime.Now.Ticks}";
                                div_Icerik.InnerHtml = SDataMailModel.Veriler.KatilimciBilgisi.KatilimciTipiBilgisi.KabulEkranIcerik;
                                div_Icerik.InnerHtml += style.Replace("{0}", ResolveClientUrl($"~/Dosyalar/KarsilamaEkrani/{SDataMailModel.Veriler.KatilimciBilgisi.KatilimciTipiBilgisi.KatilimciTipiID}.jpg"));
                            }
                            else
                            {
                                BilgiKontrolMerkezi.UyariEkrani(this, $"UyariBilgilendirme('', '<p class=\"text-center\">Sayın {SDataMailModel.Veriler.KatilimciBilgisi.AdSoyad}</p><p class=\"text-center\">Geri bildirim linkiniz \"{SDataMailModel.Veriler.EklenmeTarihi.AddMinutes(6):dd.MM.yyyy HH:mm}\" tarihinde aktif olacaktır.</p><p>Tarafınıza gönderilen davetiye mailine tekrar girerek, katılıyorum butonuna basabilirsiniz.</p>', false);", true, true);
                            }
                        }
                        else
                        {
                            if (!SDataMailModel.Veriler.KatilimciBilgisi.KatilimciOnay)
                            {
                                lblAdSoyad.Text = SDataSmsModel.Veriler.KatilimciBilgisi.AdSoyad;

                                ImgLogo.ImageUrl = $"~/Dosyalar/Logo/{SDataMailModel.Veriler.KatilimciBilgisi.KatilimciTipiID}.png?t={DateTime.Now.Ticks}";
                                div_Icerik.InnerHtml = SDataMailModel.Veriler.KatilimciBilgisi.KatilimciTipiBilgisi.KabulEkranIcerik;
                                div_Icerik.InnerHtml += style.Replace("{0}", ResolveClientUrl($"~/Dosyalar/KarsilamaEkrani/{SDataMailModel.Veriler.KatilimciBilgisi.KatilimciTipiBilgisi.KatilimciTipiID}.jpg"));
                            }
                            else
                                BilgiKontrolMerkezi.UyariEkrani(this, $"UyariBilgilendirme('', '<p class=\"text-center\">Sayın {SDataMailModel.Veriler.KatilimciBilgisi.AdSoyad}</p><p class=\"text-center\"> {SDataMailModel.Veriler.EklenmeTarihi.AddMinutes(6):dd.MM.yyyy HH:mm} tarihinde vermiş olduğunuzu etkinliğe katılım kararınızda değişiklik yapmak istiyorsanız lütfen {Onay.IletisimMail} iletişime geçiniz.</p>', false);", true, true);
                        }
                    }
                    else
                    {
                        BilgiKontrolMerkezi.UyariEkrani(this, $"UyariBilgilendirme('', '<p class=\"text-center\">Katılımcı bulunamadı</p>', false); $('.modal-footer').remove();", true, true);
                    }
                }
                else
                {
                    SDataSmsModel = new SmsGonderimTablosuIslemler().KayitBilgisi(segments[1]);

                    if (SDataSmsModel.Sonuc.Equals(Sonuclar.Basarili))
                    {
                        if (!SDataSmsModel.Veriler.KatilimciBilgisi.KatilimciOnayTarihi.HasValue)
                        {
                            SModel = new KatilimciTablosuIslemler().KatilimciOnay(SDataSmsModel.Veriler.KatilimciID, false);

                            if (!SModel.Sonuc.Equals(Sonuclar.Basarili))
                                BilgiKontrolMerkezi.UyariEkrani(this, $"UyariBilgilendirme('', '<p>Geri bildiriminiz kaydedilirken hata meydana geldi</p><p>Hata mesajı : {SModel.HataBilgi.HataMesaji}</p>', false);", true, true);

                            lblAdSoyad.Text = SDataSmsModel.Veriler.KatilimciBilgisi.AdSoyad;

                            ImgLogo.ImageUrl = $"~/Dosyalar/Logo/{SDataMailModel.Veriler.KatilimciBilgisi.KatilimciTipiID}.png?t={DateTime.Now.Ticks}";
                            div_Icerik.InnerHtml = SDataMailModel.Veriler.KatilimciBilgisi.KatilimciTipiBilgisi.KabulEkranIcerik;
                            div_Icerik.InnerHtml += style.Replace("{0}", ResolveClientUrl($"~/Dosyalar/KarsilamaEkrani/{SDataMailModel.Veriler.KatilimciBilgisi.KatilimciTipiBilgisi.KatilimciTipiID}.jpg"));
                        }
                        else
                        {
                            if (!SDataSmsModel.Veriler.KatilimciBilgisi.KatilimciOnay)
                            {
                                lblAdSoyad.Text = SDataSmsModel.Veriler.KatilimciBilgisi.AdSoyad;

                                ImgLogo.ImageUrl = $"~/Dosyalar/Logo/{SDataMailModel.Veriler.KatilimciBilgisi.KatilimciTipiID}.png?t={DateTime.Now.Ticks}";
                                div_Icerik.InnerHtml = SDataMailModel.Veriler.KatilimciBilgisi.KatilimciTipiBilgisi.KabulEkranIcerik;
                                div_Icerik.InnerHtml += style.Replace("{0}", ResolveClientUrl($"~/Dosyalar/KarsilamaEkrani/{SDataMailModel.Veriler.KatilimciBilgisi.KatilimciTipiBilgisi.KatilimciTipiID}.jpg"));
                            }

                            else
                                BilgiKontrolMerkezi.UyariEkrani(this, $"UyariBilgilendirme('', '<p class=\"text-center\">Sayın {SDataSmsModel.Veriler.KatilimciBilgisi.AdSoyad}</p><p class=\"text-center\"> {SDataSmsModel.Veriler.EklenmeTarihi.AddMinutes(6):dd.MM.yyyy HH:mm} tarihinde vermiş olduğunuzu etkinliğe katılım kararınızda değişiklik yapmak istiyorsanız lütfen {Onay.IletisimMail} iletişime geçiniz.</p>', false);", true, true);
                        }
                    }
                    else
                    {
                        BilgiKontrolMerkezi.UyariEkrani(this, $"UyariBilgilendirme('', '<p class=\"text-center\">Katılımcı bulunamadı</p>', false); $('.modal-footer').remove();", true, true);
                    }
                }
            }
            else
            {
                BilgiKontrolMerkezi.UyariEkrani(this, $"UyariBilgilendirme('', '<p class=\"text-center\">Geçersiz link girildi.</p>', false); $('.modal-footer').remove();", true, true);
            }
        }
    }
}