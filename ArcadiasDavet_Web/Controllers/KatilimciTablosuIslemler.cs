using Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using VeritabaniIslemMerkeziBase;

namespace VeritabaniIslemMerkezi
{
    public partial class KatilimciTablosuIslemler : KatilimciTablosuIslemlerBase
    {
        public KatilimciTablosuIslemler() : base() { }

        public KatilimciTablosuIslemler(OleDbTransaction tran) : base(tran) { }

        public string YeniKatilimciID()
        {
            string KatilimciID;

            do
            {
                KatilimciID = Guid.NewGuid().ToString();
                VTIslem.SetCommandText("SELECT COUNT(*) FROM KatilimciTablosu WHERE KatilimciID = @KatilimciID");
                VTIslem.AddWithValue("KatilimciID", KatilimciID);
            } while (!Convert.ToInt32(VTIslem.ExecuteScalar()).Equals(0));

            return KatilimciID;
        }

        public bool MisafirKontrol(string KatilimciID)
        {
            VTIslem.SetCommandText("SELECT KatilimciTipiTablosu.MisafirKontenjan > IIF(MisafirSayisiTablosu.MisafirSayisi IS NULL, 0, MisafirSayisiTablosu.MisafirSayisi) FROM ( KatilimciTablosu INNER JOIN KatilimciTipiTablosu ON KatilimciTablosu.KatilimciTipiID = KatilimciTipiTablosu.KatilimciTipiID ) LEFT JOIN ( SELECT AnaKatilimciID, COUNT(*) AS MisafirSayisi FROM KatilimciTablosu WHERE AnaKatilimciID IS NOT NULL GROUP BY AnaKatilimciID ) AS MisafirSayisiTablosu ON KatilimciTablosu.KatilimciID = MisafirSayisiTablosu.AnaKatilimciID WHERE KatilimciTablosu.KatilimciID = @KatilimciID");
            VTIslem.AddWithValue("KatilimciID", KatilimciID);

            return Convert.ToBoolean(VTIslem.ExecuteScalar());
        }

        public bool KontenjanKontrol(string KatilimciTipiID)
        {
            VTIslem.SetCommandText("SELECT IIF(KatilimciTipiTablosu.Kontenjan = 0, 1, KatilimciTipiTablosu.Kontenjan > IIF(KatilimciSayisiTablosu.KatilimciSayisi IS NULL, 0, KatilimciSayisiTablosu.KatilimciSayisi)) FROM KatilimciTipiTablosu LEFT JOIN ( SELECT KatilimciTipiID, COUNT(*) AS KatilimciSayisi FROM KatilimciTablosu WHERE AnaKatilimciID IS NULL AND (YoneticiOnay <> false OR KatilimciOnay <> false) GROUP BY KatilimciTipiID ) AS KatilimciSayisiTablosu ON KatilimciTipiTablosu.KatilimciTipiID = KatilimciSayisiTablosu.KatilimciTipiID WHERE KatilimciTipiTablosu.KatilimciTipiID = @KatilimciTipiID");
            VTIslem.AddWithValue("KatilimciTipiID", KatilimciTipiID);
            return Convert.ToBoolean(VTIslem.ExecuteScalar());
        }

        public override SurecVeriModel<KatilimciTablosuModel> KayitBilgisi(string KatilimciID)
        {
            int
                KatilimciIndex = 0,
                KatilimciTipiIndex = KatilimciIndex + KatilimciTablosuModel.OzellikSayisi;


            VTIslem.SetCommandText("SELECT KatilimciTablosu.*, KatilimciTipiTablosu.* FROM KatilimciTablosu INNER JOIN KatilimciTipiTablosu ON KatilimciTablosu.KatilimciTipiID = KatilimciTipiTablosu.KatilimciTipiID WHERE KatilimciID = @KatilimciID");
            VTIslem.AddWithValue("KatilimciID", KatilimciID);
            VTIslem.OpenConnection();
            SModel = VTIslem.ExecuteReader(CommandBehavior.SingleResult);
            if (SModel.Sonuc.Equals(Sonuclar.Basarili))
            {
                while (SModel.Reader.Read())
                {
                    if (KayitBilgisiAl(KatilimciIndex, SModel.Reader).Sonuc.Equals(Sonuclar.Basarili))
                        SDataModel.Veriler.KatilimciTipiBilgisi = new KatilimciTipiTablosuIslemler().KayitBilgisiAl(KatilimciTipiIndex, SModel.Reader).Veriler;
                }
                if (SDataModel is null)
                {
                    SDataModel = new SurecVeriModel<KatilimciTablosuModel>
                    {
                        Sonuc = Sonuclar.VeriBulunamadi,
                        KullaniciMesaji = "Belirtilen kayýt bulunamamýþtýr",
                        HataBilgi = new HataBilgileri
                        {
                            HataAlinanKayitID = 0,
                            HataKodu = 0,
                            HataMesaji = "Belirtilen kayýt bulunamamýþtýr"
                        }
                    };
                }
            }
            else
            {
                SDataModel = new SurecVeriModel<KatilimciTablosuModel>
                {
                    Sonuc = SModel.Sonuc,
                    KullaniciMesaji = SModel.KullaniciMesaji,
                    HataBilgi = SModel.HataBilgi
                };
            }
            VTIslem.CloseConnection();
            return SDataModel;
        }

        public SurecVeriModel<KatilimciTablosuModel> KayitBilgisi(string KatilimciID, string IletisimTipi, int GonderimTipi = 1)
        {
            int
                KatilimciIndex = 0,
                KatilimciTipiIndex = KatilimciIndex + KatilimciTablosuModel.OzellikSayisi;

            switch (IletisimTipi)
            {
                case "email":
                    int
                        MailIcerikIndex = KatilimciTipiIndex + KatilimciTipiTablosuModel.OzellikSayisi,
                        AntetliKagitIcerikIndex = MailIcerikIndex + MailIcerikTablosuModel.OzellikSayisi;


                    VTIslem.SetCommandText("SELECT KatilimciTablosu.*, KatilimciTipiTablosu.*, MailIcerikTablosu.*, AntetliKagitIcerikTablosu.* FROM ( ( KatilimciTablosu INNER JOIN KatilimciTipiTablosu ON KatilimciTablosu.KatilimciTipiID = KatilimciTipiTablosu.KatilimciTipiID ) INNER JOIN MailIcerikTablosu ON KatilimciTipiTablosu.KatilimciTipiID = MailIcerikTablosu.KatilimciTipiID ) INNER JOIN AntetliKagitIcerikTablosu ON MailIcerikTablosu.MailIcerikID = AntetliKagitIcerikTablosu.MailIcerikID WHERE MailIcerikTablosu.AnaKatilimci = IIF(KatilimciTablosu.AnaKatilimciID IS NULL, true, false) AND KatilimciID = @KatilimciID AND MailIcerikTablosu.GonderimTipiID = @GonderimTipiID");
                    VTIslem.AddWithValue("KatilimciID", KatilimciID);
                    VTIslem.AddWithValue("GonderimTipi", GonderimTipi);
                    VTIslem.OpenConnection();
                    SModel = VTIslem.ExecuteReader(CommandBehavior.Default);
                    if (SModel.Sonuc.Equals(Sonuclar.Basarili))
                    {
                        while (SModel.Reader.Read())
                        {
                            if (SDataModel is null)
                            {
                                if (KayitBilgisiAl(KatilimciIndex, SModel.Reader).Sonuc.Equals(Sonuclar.Basarili))
                                {
                                    SDataModel.Veriler.KatilimciTipiBilgisi = new KatilimciTipiTablosuIslemler().KayitBilgisiAl(KatilimciTipiIndex, SModel.Reader).Veriler;
                                    SDataModel.Veriler.KatilimciTipiBilgisi.MailIcerikBilgisi = new List<MailIcerikTablosuModel> { new MailIcerikTablosuIslemler().KayitBilgisiAl(MailIcerikIndex, SModel.Reader).Veriler };
                                    SDataModel.Veriler.KatilimciTipiBilgisi.MailIcerikBilgisi.First().AntetliKagitIcerikBilgisi = new List<AntetliKagitIcerikTablosuModel>();
                                }
                                else
                                    break;
                            }

                            SDataModel.Veriler.KatilimciTipiBilgisi.MailIcerikBilgisi.First().AntetliKagitIcerikBilgisi.Add(new AntetliKagitIcerikTablosuIslemler().KayitBilgisiAl(AntetliKagitIcerikIndex, SModel.Reader).Veriler);
                        }
                        if (SDataModel is null)
                        {
                            SDataModel = new SurecVeriModel<KatilimciTablosuModel>
                            {
                                Sonuc = Sonuclar.VeriBulunamadi,
                                KullaniciMesaji = "Belirtilen kayýt bulunamamýþtýr",
                                HataBilgi = new HataBilgileri
                                {
                                    HataAlinanKayitID = 0,
                                    HataKodu = 0,
                                    HataMesaji = "Belirtilen kayýt bulunamamýþtýr"
                                }
                            };
                        }
                    }
                    else
                    {
                        SDataModel = new SurecVeriModel<KatilimciTablosuModel>
                        {
                            Sonuc = SModel.Sonuc,
                            KullaniciMesaji = SModel.KullaniciMesaji,
                            HataBilgi = SModel.HataBilgi
                        };
                    }
                    VTIslem.CloseConnection();
                    return SDataModel;


                case "YakaKarti":
                    int
                        YakaKartiCerceveIndex = KatilimciTipiIndex + KatilimciTipiTablosuModel.OzellikSayisi,
                        YakaKartiIcerikIndex = YakaKartiCerceveIndex + YakaKartiCerceveTablosuModel.OzellikSayisi,
                        YakaKartiBasimIndex = YakaKartiIcerikIndex + YakaKartiIcerikTablosuModel.OzellikSayisi;

                    VTIslem.SetCommandText("SELECT KatilimciTablosu.*, KatilimciTipiTablosu.*, YakaKartiCerceveTablosu.*, YakaKartiIcerikTablosu.*, YakaKartiBasimTablosu.* FROM ( ( (  KatilimciTablosu INNER JOIN KatilimciTipiTablosu ON KatilimciTablosu.KatilimciTipiID = KatilimciTipiTablosu.KatilimciTipiID ) INNER JOIN YakaKartiCerceveTablosu ON KatilimciTipiTablosu.KatilimciTipiID = YakaKartiCerceveTablosu.KatilimciTipiID ) INNER JOIN YakaKartiIcerikTablosu ON YakaKartiCerceveTablosu.YakaKartiCerceveID = YakaKartiIcerikTablosu.YakaKartiCerceveID ) LEFT JOIN YakaKartiBasimTablosu ON KatilimciTablosu.KatilimciID = YakaKartiBasimTablosu.KatilimciID WHERE KatilimciTablosu.KatilimciID = @KatilimciID");
                    VTIslem.AddWithValue("KatilimciID", KatilimciID);
                    VTIslem.OpenConnection();
                    SModel = VTIslem.ExecuteReader(CommandBehavior.Default);
                    if (SModel.Sonuc.Equals(Sonuclar.Basarili))
                    {
                        while (SModel.Reader.Read())
                        {
                            if (SDataModel is null)
                            {
                                if (KayitBilgisiAl(KatilimciIndex, SModel.Reader).Sonuc.Equals(Sonuclar.Basarili))
                                {
                                    SDataModel.Veriler.KatilimciTipiBilgisi = new KatilimciTipiTablosuIslemler().KayitBilgisiAl(KatilimciTipiIndex, SModel.Reader).Veriler;
                                    SDataModel.Veriler.KatilimciTipiBilgisi.YakaKartiCerceveBilgisi = new YakaKartiCerceveTablosuIslemler().KayitBilgisiAl(YakaKartiCerceveIndex, SModel.Reader).Veriler;
                                    SDataModel.Veriler.KatilimciTipiBilgisi.YakaKartiCerceveBilgisi.YakaKartiIcerikBilgisi = new List<YakaKartiIcerikTablosuModel>();
                                    SDataModel.Veriler.YakaKartiBasimBilgisi = new List<YakaKartiBasimTablosuModel>();
                                }
                                else
                                    break;
                            }

                            if (SDataModel.Veriler.KatilimciTipiBilgisi.YakaKartiCerceveBilgisi.YakaKartiIcerikBilgisi.Count(x => x.YakaKartiIcerikID.Equals(SModel.Reader.GetInt32(YakaKartiIcerikIndex))).Equals(0))
                                SDataModel.Veriler.KatilimciTipiBilgisi.YakaKartiCerceveBilgisi.YakaKartiIcerikBilgisi.Add(new YakaKartiIcerikTablosuIslemler().KayitBilgisiAl(YakaKartiIcerikIndex, SModel.Reader).Veriler);

                            if (!SModel.Reader.IsDBNull(YakaKartiBasimIndex) && SDataModel.Veriler.YakaKartiBasimBilgisi.Count(x => x.YakaKartiBasimID.Equals(SModel.Reader.GetInt32(YakaKartiBasimIndex))).Equals(0))
                                SDataModel.Veriler.YakaKartiBasimBilgisi.Add(new YakaKartiBasimTablosuIslemler().KayitBilgisiAl(YakaKartiBasimIndex, SModel.Reader).Veriler);
                        }
                        if (SDataModel is null)
                        {
                            SDataModel = new SurecVeriModel<KatilimciTablosuModel>
                            {
                                Sonuc = Sonuclar.VeriBulunamadi,
                                KullaniciMesaji = "Belirtilen kayýt bulunamamýþtýr",
                                HataBilgi = new HataBilgileri
                                {
                                    HataAlinanKayitID = 0,
                                    HataKodu = 0,
                                    HataMesaji = "Belirtilen kayýt bulunamamýþtýr"
                                }
                            };
                        }
                    }
                    else
                    {
                        SDataModel = new SurecVeriModel<KatilimciTablosuModel>
                        {
                            Sonuc = SModel.Sonuc,
                            KullaniciMesaji = SModel.KullaniciMesaji,
                            HataBilgi = SModel.HataBilgi
                        };
                    }
                    VTIslem.CloseConnection();
                    return SDataModel;

                default:
                    int
                        SmsIcerikIndex = KatilimciTipiIndex + KatilimciTipiTablosuModel.OzellikSayisi;


                    VTIslem.SetCommandText("SELECT KatilimciTablosu.*, KatilimciTipiTablosu.*, SmsIcerikTablosu.* FROM ( KatilimciTablosu INNER JOIN KatilimciTipiTablosu ON KatilimciTablosu.KatilimciTipiID = KatilimciTipiTablosu.KatilimciTipiID ) INNER JOIN SmsIcerikTablosu ON KatilimciTipiTablosu.KatilimciTipiID = SmsIcerikTablosu.KatilimciTipiID WHERE SmsIcerikTablosu.AnaKatilimci = KatilimciTablosu.AnaKatilimciID IS NULL AND KatilimciID = @KatilimciID AND SmsIcerikTablosu.GonderimTipiID = @GonderimTipiID");
                    VTIslem.AddWithValue("KatilimciID", KatilimciID);
                    VTIslem.AddWithValue("GonderimTipi", GonderimTipi);
                    VTIslem.OpenConnection();
                    SModel = VTIslem.ExecuteReader(CommandBehavior.SingleResult);
                    if (SModel.Sonuc.Equals(Sonuclar.Basarili))
                    {
                        while (SModel.Reader.Read())
                        {
                            if (KayitBilgisiAl(KatilimciIndex, SModel.Reader).Sonuc.Equals(Sonuclar.Basarili))
                            {
                                SDataModel.Veriler.KatilimciTipiBilgisi = new KatilimciTipiTablosuIslemler().KayitBilgisiAl(KatilimciTipiIndex, SModel.Reader).Veriler;
                                SDataModel.Veriler.KatilimciTipiBilgisi.SmsIcerikBilgisi = new List<SmsIcerikTablosuModel> { new SmsIcerikTablosuIslemler().KayitBilgisiAl(SmsIcerikIndex, SModel.Reader).Veriler };
                            }
                        }
                        if (SDataModel is null)
                        {
                            SDataModel = new SurecVeriModel<KatilimciTablosuModel>
                            {
                                Sonuc = Sonuclar.VeriBulunamadi,
                                KullaniciMesaji = "Belirtilen kayýt bulunamamýþtýr",
                                HataBilgi = new HataBilgileri
                                {
                                    HataAlinanKayitID = 0,
                                    HataKodu = 0,
                                    HataMesaji = "Belirtilen kayýt bulunamamýþtýr"
                                }
                            };
                        }
                    }
                    else
                    {
                        SDataModel = new SurecVeriModel<KatilimciTablosuModel>
                        {
                            Sonuc = SModel.Sonuc,
                            KullaniciMesaji = SModel.KullaniciMesaji,
                            HataBilgi = SModel.HataBilgi
                        };
                    }
                    VTIslem.CloseConnection();
                    return SDataModel;
            }
        }

        public SurecVeriModel<KatilimciTablosuModel> AppGirisKontrol(string KatilimciID)
        {
            int
                KatilimciIndex = 0,
                KatilimciTipiIndex = KatilimciIndex + KatilimciTablosuModel.OzellikSayisi,
                KatilimciGirisIndex = KatilimciTipiIndex + KatilimciTipiTablosuModel.OzellikSayisi;

            VTIslem.SetCommandText("SELECT KatilimciTablosu.*, KatilimciTipiTablosu.*, KatilimciGirisTablosu.* FROM ( KatilimciTablosu INNER JOIN KatilimciTipiTablosu ON KatilimciTablosu.KatilimciTipiID = KatilimciTipiTablosu.KatilimciTipiID ) LEFT JOIN KatilimciGirisTablosu ON KatilimciTablosu.KatilimciID = KatilimciGirisTablosu.KatilimciID WHERE KatilimciTablosu.KatilimciID = @KatilimciID");
            VTIslem.AddWithValue("KatilimciID", KatilimciID);
            VTIslem.OpenConnection();
            SModel = VTIslem.ExecuteReader(CommandBehavior.SingleResult);
            if (SModel.Sonuc.Equals(Sonuclar.Basarili))
            {
                while (SModel.Reader.Read())
                {
                    if (SDataModel is null)
                    {
                        if (KayitBilgisiAl(KatilimciIndex, SModel.Reader).Sonuc.Equals(Sonuclar.Basarili))
                        {
                            SDataModel.Veriler.KatilimciTipiBilgisi = new KatilimciTipiTablosuIslemler().KayitBilgisiAl(KatilimciTipiIndex, SModel.Reader).Veriler;
                            SDataModel.Veriler.KatilimciGirisBilgisi = new List<KatilimciGirisTablosuModel>();
                        }
                        else
                            break;
                    }
                    
                    if (!SModel.Reader.IsDBNull(KatilimciGirisIndex))
                    {
                        SDataModel.Veriler.KatilimciGirisBilgisi.Add(new KatilimciGirisTablosuIslemler().KayitBilgisiAl(KatilimciGirisIndex, SModel.Reader).Veriler);
                    }
                }
                if (SDataModel is null)
                {
                    SDataModel = new SurecVeriModel<KatilimciTablosuModel>
                    {
                        Sonuc = Sonuclar.VeriBulunamadi,
                        KullaniciMesaji = "Belirtilen kayýt bulunamamýþtýr",
                        HataBilgi = new HataBilgileri
                        {
                            HataAlinanKayitID = 0,
                            HataKodu = 0,
                            HataMesaji = "Belirtilen kayýt bulunamamýþtýr"
                        }
                    };
                }
            }
            else
            {
                SDataModel = new SurecVeriModel<KatilimciTablosuModel>
                {
                    Sonuc = SModel.Sonuc,
                    KullaniciMesaji = SModel.KullaniciMesaji,
                    HataBilgi = SModel.HataBilgi
                };
            }
            VTIslem.CloseConnection();
            return SDataModel;
        }

        public override SurecBilgiModel KayitGuncelle(KatilimciTablosuModel GuncelKayit)
        {
            VTIslem.SetCommandText("UPDATE KatilimciTablosu SET KatilimciTipiID=@KatilimciTipiID, AdSoyad=@AdSoyad, ePosta=@ePosta, Telefon=@Telefon, Unvan=@Unvan, Kurum=@Kurum, GuncellenmeTarihi=@GuncellenmeTarihi WHERE KatilimciID=@KatilimciID");
            VTIslem.AddWithValue("KatilimciTipiID", GuncelKayit.KatilimciTipiID);
            VTIslem.AddWithValue("AdSoyad", GuncelKayit.AdSoyad);
            VTIslem.AddWithValue("ePosta", GuncelKayit.ePosta);
            VTIslem.AddWithValue("Telefon", GuncelKayit.Telefon);
            VTIslem.AddWithValue("Unvan", GuncelKayit.Unvan);
            VTIslem.AddWithValue("Kurum", GuncelKayit.Kurum);
            VTIslem.AddWithValue("GuncellenmeTarihi", GuncelKayit.GuncellenmeTarihi);
            VTIslem.AddWithValue("KatilimciID", GuncelKayit.KatilimciID);
            return VTIslem.ExecuteNonQuery();
        }

        public SurecBilgiModel YoneticiOnay(string KatilimciID, bool Durum)
        {
            VTIslem.SetCommandText("UPDATE KatilimciTablosu SET YoneticiOnay = @YoneticiOnay, YoneticiOnayTarihi = Now(), GuncellenmeTarihi = Now() WHERE KatilimciID = @KatilimciID");
            VTIslem.AddWithValue("YoneticiOnay", Durum);
            VTIslem.AddWithValue("KatilimciID", KatilimciID);
            return VTIslem.ExecuteNonQuery();
        }

        public SurecBilgiModel KatilimciOnay(string KatilimciID, bool Durum)
        {
            VTIslem.SetCommandText("UPDATE KatilimciTablosu SET KatilimciOnay = @KatilimciOnay, KatilimciOnayTarihi = Now(), GuncellenmeTarihi = Now() WHERE KatilimciID = @KatilimciID");
            VTIslem.AddWithValue("KatilimciOnay", Durum);
            VTIslem.AddWithValue("KatilimciID", KatilimciID);
            return VTIslem.ExecuteNonQuery();
        }

        public SurecVeriModel<DataTable> KatilimciRaporu()
        {
            DataTable dt = new DataTable();

            VTIslem.SetCommandText("SELECT KatilimciTablosu.KatilimciID AS [Katýlýmcý No], KatilimciTablosu.AdSoyad AS [Katýlýmcý], KatilimciTipiTablosu.KatilimciTipi AS [Katýlýmcý Tipi], KatilimciTablosu.ePosta AS [e-Posta], IIF(KatilimciTablosu.YoneticiOnayTarihi IS NULL, '', IIF(KatilimciTablosu.YoneticiOnay, 'Onaylý', 'Onaysiz')) AS [Yönetici Onay Durumu], IIF(KatilimciTablosu.YoneticiOnayTarihi IS NULL, '', KatilimciTablosu.YoneticiOnayTarihi) AS [Yönetici Onay Tarihi], IIF(KatilimciTablosu.KatilimciOnayTarihi IS NULL, '', IIF(KatilimciTablosu.KatilimciOnay, 'Onaylý', 'Onaysiz')) AS [Katýlýmcý Onay Durumu], IIF(KatilimciTablosu.KatilimciOnayTarihi IS NULL, '', KatilimciTablosu.KatilimciOnayTarihi) AS [Katýlýmcý Onay Tarihi], AnaKatilimciTablosu.AdSoyad AS [Ana Katýlýmcý], KatilimciTablosu.EklenmeTarihi AS [Kayýt Tarihi] FROM ( KatilimciTablosu INNER JOIN KatilimciTipiTablosu ON KatilimciTablosu.KatilimciTipiID = KatilimciTipiTablosu.KatilimciTipiID ) LEFT JOIN ( SELECT * FROM KatilimciTablosu WHERE AnaKatilimciID IS NULL ) AS AnaKatilimciTablosu ON KatilimciTablosu.AnaKatilimciID = AnaKatilimciTablosu.KatilimciID");

            SModel = VTIslem.ExecuteDataAdapter();

            SurecVeriModel<DataTable> SDataRaporModel = new SurecVeriModel<DataTable>
            {
                Sonuc = SModel.Sonuc,
                KullaniciMesaji = SModel.KullaniciMesaji,
                HataBilgi = SModel.HataBilgi,
                Veriler = new DataTable("Katýlýmcýlar")
            };

            if (SModel.Sonuc.Equals(Sonuclar.Basarili))
                SModel.Adapter.Fill(SDataRaporModel.Veriler);

            return SDataRaporModel;
        }
    }
}
