using FluentFTP;
using Model;
using System;
using System.IO;
using System.Net;

namespace VeritabaniIslemMerkezi
{
    public class FTPIslemleri
    {
        SurecBilgiModel SModel;

        readonly string Root = "ftp://nas.arkadyas.com";
        readonly string AbstractRoot = $"/Ortak/digiabstract.com";
        readonly NetworkCredential UserInfo = new NetworkCredential("arkadyas", "Arkadyas.0103");

        SurecBilgiModel DosyaYoluKontrolu(string SubDomain, string ApplicationName)
        {
            if (DosyaYoluKontrolu($"{AbstractRoot}/{SubDomain}").Sonuc.Equals(Sonuclar.VeriBulunamadi))
                DosyaOlusturma($"{AbstractRoot}/{SubDomain}");


            if (SModel.Sonuc.Equals(Sonuclar.Basarili))
            {
                if (DosyaYoluKontrolu($"{AbstractRoot}/{SubDomain}/{ApplicationName}").Sonuc.Equals(Sonuclar.VeriBulunamadi))
                    DosyaOlusturma($"{AbstractRoot}/{SubDomain}/{ApplicationName}");
            }

            return SModel;
        }

        SurecBilgiModel DosyaYoluKontrolu(string Dosya)
        {
            try
            {
                using (FtpClient ftpClient = new FtpClient(Root, UserInfo))
                {
                    ftpClient.Connect();

                    if (ftpClient.DirectoryExists(Dosya))
                    {
                        SModel = new SurecBilgiModel
                        {
                            Sonuc = Sonuclar.Basarili,
                            KullaniciMesaji = "FTP'de dosya mevcut"
                        };
                    }
                    else
                    {
                        SModel = new SurecBilgiModel
                        {
                            Sonuc = Sonuclar.VeriBulunamadi,
                            KullaniciMesaji = "FTP'de dosya mevcut değildir"
                        };
                    }

                    ftpClient.Disconnect();
                }
            }
            catch (Exception ex)
            {
                SModel = new SurecBilgiModel
                {
                    Sonuc = Sonuclar.Basarisiz,
                    KullaniciMesaji = "Veri bilgisi çekilirken hatalı atama yapılmaya çalışıldı",
                    HataBilgi = new HataBilgileri
                    {
                        HataMesaji = string.Format(@"{0}", ex.Message.Replace("'", "ʼ")),
                        HataKodu = ex.HResult,
                        HataAlinanKayitID = SModel.Reader.GetValue(0)
                    }
                };
            }

            return SModel;
        }

        SurecBilgiModel DosyaOlusturma(string Dosya)
        {
            try
            {
                using (FtpClient ftpClient = new FtpClient(Root, UserInfo))
                {
                    ftpClient.Connect();

                    if (ftpClient.CreateDirectory(Dosya))
                    {
                        SModel = new SurecBilgiModel
                        {
                            Sonuc = Sonuclar.Basarili,
                            KullaniciMesaji = "Dosya oluşturuldu"
                        };
                    }
                    else
                    {
                        SModel = new SurecBilgiModel
                        {
                            Sonuc = Sonuclar.Basarisiz,
                            KullaniciMesaji = "Dosya oluşturulamadı",
                            HataBilgi = new HataBilgileri
                            {
                                HataKodu = 0,
                                HataAlinanKayitID = 0,
                                HataMesaji = "Dosya oluşturulamadı"
                            }
                        };
                    }

                    ftpClient.Disconnect();
                }
            }
            catch (Exception ex)
            {
                SModel = new SurecBilgiModel
                {
                    Sonuc = Sonuclar.Basarisiz,
                    KullaniciMesaji = "Dosya oluşturulamadı",
                    HataBilgi = new HataBilgileri
                    {
                        HataMesaji = string.Format(@"{0}", ex.Message.Replace("'", "ʼ")),
                        HataKodu = ex.HResult,
                        HataAlinanKayitID = SModel.Reader.GetValue(0)
                    }
                };
            }

            return SModel;
        }

        public SurecBilgiModel DosyaYukleme(string SubDomain, string ApplicationName, string LogFileName, MemoryStream ms)
        {
            if (DosyaYoluKontrolu(SubDomain, ApplicationName).Sonuc.Equals(Sonuclar.Basarili))
            {
                using (FtpClient ftpClient = new FtpClient(Root, UserInfo))
                {
                    ftpClient.Connect();

                    Action<FtpProgress> ftpProgress = (FtpProgress ProgressInfo) =>
                    {
                        File.AppendAllText(LogFileName, $"{DateTime.Now:dd.MM.yyyy HH:mm:ss} ==> FTP aktrımının % {decimal.Round((decimal)ProgressInfo.Progress, 2)} tamamlandı.\r\n");
                    };

                    File.AppendAllText(LogFileName, $"{DateTime.Now:dd.MM.yyyy HH:mm:ss} ==> FTP aktarımına başlanıyor.\r\n\r\n");

                    try
                    {
                        FtpStatus ftpResult = ftpClient.UploadStream(fileStream: ms, remotePath: $"{AbstractRoot}/{SubDomain}/{ApplicationName}/{DateTime.Now:yyyy.MM.dd_HHmmss}.zip", progress: ftpProgress);

                        if (ftpResult.Equals(FtpStatus.Success))
                        {
                            SModel = new SurecBilgiModel
                            {
                                Sonuc = Sonuclar.Basarili,
                                KullaniciMesaji = "Ftp aktarımı tamamlandı"
                            };
                        }
                        else
                        {
                            SModel = new SurecBilgiModel
                            {
                                Sonuc = Sonuclar.Basarisiz,
                                KullaniciMesaji = "Ftp yüklemesinde hata meydana geldi",
                                HataBilgi = new HataBilgileri
                                {
                                    HataAlinanKayitID = 0,
                                    HataKodu = 0,
                                    HataMesaji = "Ftp yüklemesinde hata meydana geldi"
                                }
                            };
                        }
                    }
                    catch (Exception ex)
                    {
                        SModel = new SurecBilgiModel
                        {
                            Sonuc = Sonuclar.Basarisiz,
                            KullaniciMesaji = "Ftp yüklemesinde hata meydana geldi",
                            HataBilgi = new HataBilgileri
                            {
                                HataMesaji = string.Format(@"{0}", ex.Message.Replace("'", "ʼ")),
                                HataKodu = ex.HResult,
                                HataAlinanKayitID = SModel.Reader.GetValue(0)
                            }
                        };
                    }

                    ftpClient.Disconnect();
                }
            }

            return SModel;
        }
    }
}