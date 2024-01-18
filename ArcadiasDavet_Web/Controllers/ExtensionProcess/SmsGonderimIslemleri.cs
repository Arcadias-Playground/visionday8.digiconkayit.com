using ArcadiasDavet_Web.NetGsmSms;
using Model;
using System;
using System.IO;
using System.Linq;
using System.Web;

namespace VeritabaniIslemMerkezi
{
    public class SmsGonderimIslemleri
    {
        string BaseUrl, PDFGorsel, OnayLinki, RedLinki, LogFile, SmsGonderimSonuc;

        SurecBilgiModel SModel;
        SmsGonderimTablosuModel SMSModel;

        public SmsGonderimIslemleri()
        {
            BaseUrl = $"{HttpContext.Current.Request.Url.Scheme}://{$"{HttpContext.Current.Request.Url.Authority}/{HttpContext.Current.Request.ApplicationPath}".Replace("//", "/").Replace("\\\\", "/")}";
            PDFGorsel = $"{BaseUrl}/Dosyalar/MailIcerikGorsel/";
            OnayLinki = $"{BaseUrl}/Katilimci/Onay/sms/";
            RedLinki = $"{BaseUrl}/Katilimci/Red/sms/";
            LogFile = HttpContext.Current.Server.MapPath($"~/Dosyalar/SmsLog/");
        }

        public SurecBilgiModel SmsGonderim(KatilimciTablosuModel KModel)
        {
            try
            {
                SMSModel = new SmsGonderimTablosuModel
                {
                    SmsGonderimID = new SmsGonderimTablosuIslemler().YeniSmsGonderimID(),
                    KatilimciID = KModel.KatilimciID,
                    Telefon = KModel.Telefon,
                    SmsIcerikID = KModel.KatilimciTipiBilgisi.SmsIcerikBilgisi.First().SmsIcerikID,
                    Durum = false,
                    EklenmeTarihi = new BilgiKontrolMerkezi().Simdi()
                };


                using (smsnnClient SendSms = new smsnnClient())
                {
                    SmsGonderimSonuc = SendSms.smsGonder1NV2(
                        username: "8503082269",
                        password: "93F7C@E",
                        header: "ARKADYAS",
                        msg:
                            KModel.KatilimciTipiBilgisi.SmsIcerikBilgisi.First().SmsIcerik
                                .Replace("{AdSoyad}", KModel.AdSoyad)
                                .Replace("{Unvan}", KModel.Unvan)
                                .Replace("{ePosta}", KModel.ePosta)
                                .Replace("{Telefon}", KModel.Telefon)
                                .Replace("{Kurum}", KModel.Kurum)
                                .Replace("{KabulLinki}", $"{OnayLinki}{SMSModel.SmsGonderimID}")
                                .Replace("{RedLinki}", $"{RedLinki}{SMSModel.SmsGonderimID}"),
                        gsm: new string[] { $"90{KModel.Telefon.Replace("(", string.Empty).Replace(")", string.Empty).Replace(" ", string.Empty)}" },
                        encoding: "TR",
                        startdate: "",
                        stopdate: "",
                        bayikodu: "",
                        filter: 0
                    );

                    File.AppendAllText($"{LogFile}{SMSModel.SmsGonderimID}.smslog", $"{SmsGonderimSonuc}");

                    SMSModel.Durum = SmsGonderimSonuc.Length > 4;

                    if (SMSModel.Durum)
                    {
                        SModel = new SurecBilgiModel
                        {
                            Sonuc = Sonuclar.Basarili,
                            KullaniciMesaji = "SMS gönderildi",
                        };
                    }
                    else
                    {
                        SModel = new SurecBilgiModel
                        {
                            Sonuc = Sonuclar.Basarisiz,
                            KullaniciMesaji = "SMS gönderilken hata meydana geldi",
                            HataBilgi = new HataBilgileri
                            {
                                HataAlinanKayitID = 0,
                                HataKodu = 0,
                                HataMesaji = SmsGonderimSonuc
                            }
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                SModel = new SurecBilgiModel
                {
                    Sonuc = Sonuclar.Basarisiz,
                    KullaniciMesaji = "SMS gönderilirken hata meydana geldi",
                    HataBilgi = new HataBilgileri
                    {
                        HataAlinanKayitID = 0,
                        HataKodu = ex.HResult,
                        HataMesaji = ex.Message.Replace("'", "ʼ").Replace("\r", string.Empty).Replace("\n", string.Empty).Replace("\t", string.Empty)
                    }
                };

                File.AppendAllText($"{LogFile}{SMSModel.SmsGonderimID}.smslog", $"{SmsGonderimSonuc}\r\n\r\n{DateTime.Now:dd.MM.yyyy HH:mm:ss} ==> {SModel.HataBilgi.HataMesaji}");
            }

            new SmsGonderimTablosuIslemler().YeniKayitEkle(SMSModel);

            return SModel;
        }
    }
}