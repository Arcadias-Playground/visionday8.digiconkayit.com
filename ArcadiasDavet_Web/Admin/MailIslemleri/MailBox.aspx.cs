using HtmlAgilityPack;
using MailKit.Net.Pop3;
using MailKit.Security;
using MimeKit;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using VeritabaniIslemMerkezi;

namespace ArcadiasDavet_Web.Admin.MailIslemleri
{
    public partial class MailBox : Page
    {
        SurecBilgiModel SModel;
        SurecVeriModel<MailAyarTablosuModel> SDataModel;
        SurecVeriModel<WebMailTablosuModel> SDataWebMailModel;
        SurecVeriModel<IList<WebMailTablosuModel>> SDataListModel;

        int NewMessageCount = 0, FailedMessageCount = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                SDataModel = new MailAyarTablosuIslemler().KayitBilgisi(1);

                switch (SDataModel.Sonuc)
                {
                    case Sonuclar.KismiBasarili:
                    case Sonuclar.Basarisiz:
                        BilgiKontrolMerkezi.UyariEkrani(this, $"UyariBilgilendirme('', '<p>Mail ayarları alınırken hata meydana geldi</p><p>Hata mesajı : {SDataModel.HataBilgi.HataMesaji}</p>', false);", true, true);
                        break;

                    default:
                    case Sonuclar.VeriBulunamadi:
                        BilgiKontrolMerkezi.UyariEkrani(this, $"UyariBilgilendirme('', '<p>Mail ayarları bulunamadı</p>', false);", true, true);
                        break;


                    case Sonuclar.Basarili:
                        SDataListModel = new WebMailTablosuIslemler().KayitBilgileri();

                        switch (SDataListModel.Sonuc)
                        {
                            case Sonuclar.KismiBasarili:
                            case Sonuclar.Basarisiz:
                                BilgiKontrolMerkezi.UyariEkrani(this, $"UyariBilgilendirme('', '<p>Web mail bilgileri alınırken hata meydana geldi</p><p>Hata mesajı : {SDataListModel.HataBilgi.HataMesaji}</p>', false);", true, true);
                                break;

                            default:
                            case Sonuclar.VeriBulunamadi:
                                BilgiKontrolMerkezi.UyariEkrani(this, $"UyariBilgilendirme('', '<p>Web mail bulunamadı.</p>', false);", true, true);
                                break;

                            case Sonuclar.Basarili:
                                using (Pop3Client client = new Pop3Client())
                                {
                                    if (SDataModel.Veriler.SSL)
                                        client.CheckCertificateRevocation = false;

                                    try
                                    {
                                        client.Connect(SDataModel.Veriler.GelenMailHost, SDataModel.Veriler.GelenMailPort, SDataModel.Veriler.SSL ? SecureSocketOptions.Auto : SecureSocketOptions.None);
                                        client.Authenticate(SDataModel.Veriler.KullaniciAdi, SDataModel.Veriler.Sifre);

                                        NewMessageCount = client.Count;

                                        if (!client.Count.Equals(0))
                                        {
                                            for (int i = 0; i < client.Count; i++)
                                            {
                                                using (MimeMessage mm = client.GetMessage(i))
                                                {
                                                    if (SDataListModel.Veriler.FirstOrDefault(x => x.WebMailID.Equals(mm.MessageId)) is null)
                                                    {
                                                        HtmlDocument htmlDoc = null;

                                                        string HtmlBody = mm.GetTextBody(MimeKit.Text.TextFormat.Html);

                                                        if (!string.IsNullOrEmpty(HtmlBody))
                                                        {
                                                            htmlDoc = new HtmlDocument();
                                                            htmlDoc.LoadHtml(HtmlBody);

                                                            if (!(htmlDoc.DocumentNode.SelectNodes("//base") is null))
                                                            {
                                                                foreach (HtmlNode Item in htmlDoc.DocumentNode.SelectNodes("//base"))
                                                                {
                                                                    Item.Remove();
                                                                }
                                                            }
                                                        }

                                                        SModel = new WebMailTablosuIslemler().YeniKayitEkle(new WebMailTablosuModel
                                                        {
                                                            WebMailID = mm.MessageId,
                                                            GonderenMail = mm.From.First().ToString(),
                                                            Konu = mm.Subject,
                                                            TextBody = mm.GetTextBody(MimeKit.Text.TextFormat.Text),
                                                            HtmlBody = HtmlBody,
                                                            WebMailTarih = mm.Date.DateTime,
                                                            KullaniciID = null,
                                                            MailGorulmeTarihi = null,
                                                            EklenmeTarihi = new BilgiKontrolMerkezi().Simdi()
                                                        });

                                                        if (SModel.Sonuc.Equals(Sonuclar.Basarili))
                                                        {
                                                            client.DeleteMessage(i);
                                                        }
                                                        else
                                                        {
                                                            FailedMessageCount++;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        client.DeleteMessage(i);
                                                    }
                                                }
                                            }

                                            BilgiKontrolMerkezi.UyariEkrani(this, $"UyariBilgilendirme('', '<table class=\"table table-bordered\"><tr><td style=\"width:80%\">Yeni mesaj sayısı</td><td class=\"text-center\">{NewMessageCount}</td></tr><tr><td>Kayıt sırasında hata alınan mesaj sayısı</td><td class=\"text-center\">{FailedMessageCount}</td></tr></table>', true);", false);
                                        }
                                        else
                                        {
                                            BilgiKontrolMerkezi.UyariEkrani(this, $"UyariBilgilendirme('', '<p>Yeni mail bulunmamaktadır<p>', true);", false);
                                        }




                                    }
                                    catch (Exception ex)
                                    {
                                        BilgiKontrolMerkezi.UyariEkrani(this, $"UyariBilgilendirme('', '<p>Mail içerikleri alınırken hata meydana geldi</p><p>Hata mesajı : {ex.Message.Replace("\r", string.Empty).Replace("\n", string.Empty).Replace("\t", string.Empty).Replace("'", string.Empty)}</p>', false);", false);
                                    }
                                    finally
                                    {
                                        client.Disconnect(true);
                                    }
                                }


                                break;
                        }

                        break;
                }
            }
        }

        protected void rptMailBoxListesi_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            SDataWebMailModel = new WebMailTablosuIslemler().KayitBilgisi(e.CommandArgument.ToString());

            switch (SDataWebMailModel.Sonuc)
            {
                case Sonuclar.KismiBasarili:
                case Sonuclar.Basarisiz:
                    BilgiKontrolMerkezi.UyariEkrani(this, $"UyariBilgilendirme('', '<p>Mail içeriği alınırken hata meydana geldi</p><p>Hata mesajı : {SModel.HataBilgi.HataMesaji}</p>', false);", false);
                    break;

                default:
                case Sonuclar.VeriBulunamadi:
                    BilgiKontrolMerkezi.UyariEkrani(this, $"UyariBilgilendirme('', '<p>Mail içeriği bulunamadı</p>', false);", false);

                    rptMailBoxListesi.DataBind();
                    UPnlMailBoxListesi.Update();

                    break;


                case Sonuclar.Basarili:
                    if (new WebMailTablosuIslemler().MailOkumaBilgisiGuncelle(SDataWebMailModel.Veriler.WebMailID, new KullaniciTablosuIslemler().KayitBilgisi(Context).KullaniciID).Sonuc.Equals(Sonuclar.Basarili))
                    {
                        rptMailBoxListesi.DataBind();
                        UPnlMailBoxListesi.Update();
                    }

                    h4_GonderenMail.InnerHtml = SDataWebMailModel.Veriler.GonderenMail;
                    lblKonu.Text = SDataWebMailModel.Veriler.Konu;
                    div_WebMailIcerik.InnerHtml = string.IsNullOrEmpty(SDataWebMailModel.Veriler.HtmlBody) ? SDataWebMailModel.Veriler.TextBody : SDataWebMailModel.Veriler.HtmlBody;

                    UPnlWebMailDetay.Update();
                    BilgiKontrolMerkezi.UyariEkrani(this, $"$({UPnlWebMailDetay.ClientID}).modal('show');", false);

                    break;

            }
        }
    }
}