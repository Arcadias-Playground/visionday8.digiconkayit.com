using MailKit;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using Model;
using Newtonsoft.Json;
using QRCoder;
using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Web;

namespace VeritabaniIslemMerkezi
{
    public class MailGonderimIslemleri
    {
        string BaseUrl, PDFGorsel, OnayLinki, RedLinki, MisafirLinki, LogFile;

        SurecBilgiModel SModel;
        SurecVeriModel<MailAyarTablosuModel> SDataMailAyarModel = new MailAyarTablosuIslemler().KayitBilgisi(1);

        MailGonderimTablosuModel MGModel;

        public MailGonderimIslemleri()
        {
            BaseUrl = $"{HttpContext.Current.Request.Url.Scheme}://{$"{HttpContext.Current.Request.Url.Authority}/{HttpContext.Current.Request.ApplicationPath}".Replace("//", "/").Replace("\\\\", "/")}";
            //BaseUrl = "https://localhost:44348/";
            PDFGorsel = $"{BaseUrl}/Dosyalar/MailIcerikGorsel/";
            OnayLinki = $"{BaseUrl}/Katilimci/Onay/email/";
            RedLinki = $"{BaseUrl}/Katilimci/Red/email/";
            MisafirLinki = $"{BaseUrl}/Katilimci/Misafir/";
            LogFile = HttpContext.Current.Server.MapPath($"~/Dosyalar/MailLog/");
        }

        public SurecBilgiModel MailGonderim(KatilimciTablosuModel KModel)
        {
            MGModel = new MailGonderimTablosuModel
            {
                MailGonderimID = new MailGonderimTablosuIslemler().YeniMailGonderimID(),
                KatilimciID = KModel.KatilimciID,
                ePosta = KModel.ePosta,
                MailIcerikID = KModel.KatilimciTipiBilgisi.MailIcerikBilgisi.First().MailIcerikID,
                Durum = false,
                EklenmeTarihi = new BilgiKontrolMerkezi().Simdi()
            };

            using (MimeMessage mm = new MimeMessage())
            {
                mm.From.Add(new MailboxAddress(SDataMailAyarModel.Veriler.GonderenAd, SDataMailAyarModel.Veriler.ePosta));
                mm.To.Add(new MailboxAddress(KModel.AdSoyad, KModel.ePosta));
                if (!string.IsNullOrEmpty(SDataMailAyarModel.Veriler.ReplyTo))
                {
                    mm.ReplyTo.Clear();
                    mm.ReplyTo.Add(new MailboxAddress(SDataMailAyarModel.Veriler.GonderenAd, SDataMailAyarModel.Veriler.ReplyTo));

                    mm.ResentReplyTo.Clear();
                    mm.ResentReplyTo.Add(new MailboxAddress(SDataMailAyarModel.Veriler.GonderenAd, SDataMailAyarModel.Veriler.ReplyTo));

                    mm.InReplyTo = SDataMailAyarModel.Veriler.ReplyTo;
                }

                if (!string.IsNullOrEmpty(SDataMailAyarModel.Veriler.BCC))
                {
                    mm.Bcc.Clear();
                    mm.Bcc.Add(new MailboxAddress(SDataMailAyarModel.Veriler.GonderenAd, SDataMailAyarModel.Veriler.BCC));
                }

                mm.Subject = KModel.KatilimciTipiBilgisi.MailIcerikBilgisi.First().Konu;


                using (Multipart MailBodyParts = new Multipart("Mixed"))
                {
                    MailBodyParts.Add(new TextPart(MimeKit.Text.TextFormat.Html)
                    {
                        Text =
                            KModel.KatilimciTipiBilgisi.MailIcerikBilgisi.First().HtmlIcerik
                                .Replace("{AdSoyad}", KModel.AdSoyad)
                                .Replace("{Unvan}", KModel.Unvan)
                                .Replace("{ePosta}", KModel.ePosta)
                                .Replace("{Telefon}", KModel.Telefon)
                                .Replace("{Kurum}", KModel.Kurum)

                    });

                    using (HttpClient hc = new HttpClient() { Timeout = new TimeSpan(0, 5, 0), BaseAddress = new Uri("https://pdf.arkadyas.com/Api/") })
                    {
                        StringBuilder PDFRaw = new StringBuilder()
                            .Append($"<!DOCTYPE html><html lang='tr'><head><meta charset='UTF-8'><meta http-equiv='X-UA-Compatible' content='IE=edge'><meta name='viewport' content='width=device-width, initial-scale=1.0'></head><body><div style='position:absolute; top:0; left:0; width:1000px; height:1415px; background: url({PDFGorsel}{MGModel.MailIcerikID}.png); background-size:100%; background-repeat: none;'>");

                        foreach (AntetliKagitIcerikTablosuModel Item in KModel.KatilimciTipiBilgisi.MailIcerikBilgisi.First().AntetliKagitIcerikBilgisi)
                        {
                            switch (Item.AntetliKagitIcerikTipiID)
                            {
                                // [Davetiye] ==> Kabul Ediyorum Butonu
                                case 1:
                                    PDFRaw.Append($"<a href='{OnayLinki}{MGModel.MailGonderimID}' style='position: absolute; top:{Item.Y / 2}px; left: {Item.X / 2}px; width: {Item.Width / 2}px; height: {Item.Height / 2}px;'></a>");
                                    break;


                                // [Davetiye] ==> Red Ediyorum Butonu
                                case 2:
                                    PDFRaw.Append($"<a href='{RedLinki}{MGModel.MailGonderimID}' style='position: absolute; top:{Item.Y / 2}px; left: {Item.X / 2}px; width: {Item.Width / 2}px; height: {Item.Height / 2}px;'></a>");
                                    break;

                                // [QR] ==> Ad & Soyad
                                case 3:
                                    PDFRaw.Append($"<div style='position: absolute; top:{Item.Y / 2}px; left: {Item.X / 2}px; width: {Item.Width / 2}px; height: {Item.Height / 2}px; font-size: {Item.Height / 2}px; line-height: {Item.Height / 2}px; text-align: center;'>{KModel.AdSoyad}</div>");
                                    break;

                                // [QR] ==> QR ( Base64 )
                                case 4:
                                    using (QRCodeGenerator qRCodeGenerator = new QRCodeGenerator())
                                    {
                                        using (QRCodeData qRCodeData = qRCodeGenerator.CreateQrCode(MGModel.KatilimciID, QRCodeGenerator.ECCLevel.Q))
                                        {
                                            using (Base64QRCode qrCode = new Base64QRCode(qRCodeData))
                                            {
                                                PDFRaw.Append($"<div style='position: absolute; top:{Item.Y / 2}px; left: {Item.X / 2}px; width: {Item.Width / 2}px; height: {Item.Height / 2}px; border:1px solid black;'><img src=\"data:image/png;base64,{qrCode.GetGraphic(20)}\" style=\"width:100%;\"/></div>");
                                            }
                                        }
                                    }
                                    break;

                                // [QR] ==> Misafir Kayıt Linki
                                case 5:
                                    if (!KModel.KatilimciTipiBilgisi.MisafirKontenjan.Equals(0))
                                        PDFRaw.Append($"<a href=\"{MisafirLinki}{MGModel.KatilimciID}\" style=\"position: absolute; top:{Item.Y / 2}px; left: {Item.X / 2}px; width: {Item.Width / 2}px; height: {Item.Height / 2}px;\"></a>");

                                    break;

                                default:
                                    break;
                            }
                        }
                        PDFRaw.Append("</div></body></html>");

                        using (HttpResponseMessage ApiRespnse = hc.PostAsync("Html_to_Pdf", new StringContent(JsonConvert.SerializeObject(new { Html = PDFRaw.ToString() }), Encoding.UTF8, "application/json")).GetAwaiter().GetResult())
                        {
                            MailBodyParts.Add(new MimePart("application", "pdf") { ContentDisposition = new ContentDisposition(ContentDisposition.Attachment), ContentTransferEncoding = ContentEncoding.Base64, FileName = $"{(KModel.KatilimciTipiBilgisi.MailIcerikBilgisi.First().GonderimTipiID.Equals(1) ? "Davetiye" : "QR")}.pdf", Content = new MimeContent(new MemoryStream(Convert.FromBase64String(ApiRespnse.Content.ReadAsStringAsync().GetAwaiter().GetResult().Replace("\"", string.Empty)))) });
                        }

                        mm.Body = MailBodyParts;
                    }

                    using (ProtocolLogger logger = new ProtocolLogger($"{LogFile}{MGModel.MailGonderimID}.maillog"))
                    {
                        using (SmtpClient client = new SmtpClient(logger))
                        {
                            try
                            {
                                if (SDataMailAyarModel.Veriler.SSL)
                                    client.CheckCertificateRevocation = false;

                                client.Connect(SDataMailAyarModel.Veriler.GidenMailHost, SDataMailAyarModel.Veriler.GidenMailPort, SDataMailAyarModel.Veriler.SSL ? SecureSocketOptions.Auto : SecureSocketOptions.None);
                                client.Authenticate(SDataMailAyarModel.Veriler.KullaniciAdi, SDataMailAyarModel.Veriler.Sifre);
                                client.Send(mm);

                                MGModel.Durum = true;
                                MGModel.GonderimTarihi = new BilgiKontrolMerkezi().Simdi();

                                new MailGonderimTablosuIslemler().YeniKayitEkle(MGModel);

                                if (client.IsConnected)
                                    client.Disconnect(true);

                                SModel = new SurecBilgiModel
                                {
                                    Sonuc = Sonuclar.Basarili,
                                    KullaniciMesaji = "Mail gönderildi",
                                    YeniKayitID = MGModel.MailGonderimID
                                };
                            }
                            catch (Exception ex)
                            {
                                using (Stream stm = logger.Stream)
                                {
                                    byte[] errorContent = Encoding.UTF8.GetBytes($"\r\nMail gönderiminde hata meydana geldi. Hata mesajı : {ex.Message}");
                                    stm.Write(errorContent, 0, errorContent.Length);
                                }

                                MGModel.Durum = false;
                                MGModel.GonderimTarihi = new BilgiKontrolMerkezi().Simdi();

                                new MailGonderimTablosuIslemler().YeniKayitEkle(MGModel);

                                SModel = new SurecBilgiModel
                                {
                                    Sonuc = Sonuclar.Basarisiz,
                                    KullaniciMesaji = "Mail gönderimi sırasonda hata meydana geldi",
                                    YeniKayitID = MGModel.MailGonderimID,
                                    HataBilgi = new HataBilgileri
                                    {
                                        HataAlinanKayitID = 0,
                                        HataKodu = ex.HResult,
                                        HataMesaji = ex.Message.Replace("'", string.Empty).Replace("\t", string.Empty).Replace("\r", string.Empty).Replace("\n", string.Empty)
                                    }
                                };

                                if (client.IsConnected)
                                    client.Disconnect(false);
                            }
                        }
                    }
                }
            }
            return SModel;
        }
    }
}