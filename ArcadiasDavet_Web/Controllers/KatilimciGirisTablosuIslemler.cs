using Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using VeritabaniIslemMerkeziBase;

namespace VeritabaniIslemMerkezi
{
    public partial class KatilimciGirisTablosuIslemler : KatilimciGirisTablosuIslemlerBase
    {
        public KatilimciGirisTablosuIslemler() : base() { }

        public KatilimciGirisTablosuIslemler(OleDbTransaction tran) : base(tran) { }

        public bool GirisKontrol(string KatilimciID)
        {
            VTIslem.SetCommandText("SELECT COUNT(*) FROM KatilimciGirisTablosu WHERE KatilimciID = @KatilimciID");
            VTIslem.AddWithValue("KatilimciID", KatilimciID);
            return Convert.ToInt32(VTIslem.ExecuteScalar()).Equals(0);
        }

        public override SurecVeriModel<IList<KatilimciGirisTablosuModel>> KayitBilgileri()
        {
            int
                KatilimciGirisIndex = 0,
                KatilimciIndex = KatilimciGirisIndex + KatilimciGirisTablosuModel.OzellikSayisi,
                KatilimciTipiIndex = KatilimciIndex + KatilimciTablosuModel.OzellikSayisi;

            VTIslem.SetCommandText("SELECT KatilimciGirisTablosu.*, KatilimciTablosu.*, KatilimciTipiTablosu.*  FROM (KatilimciGirisTablosu INNER JOIN KatilimciTablosu ON KatilimciGirisTablosu.KatilimciID = KatilimciTablosu.KatilimciID) INNER JOIN KatilimciTipiTablosu ON KatilimciTablosu.KatilimciTipiID = KatilimciTipiTablosu.KatilimciTipiID");
            VTIslem.OpenConnection();
            SModel = VTIslem.ExecuteReader(CommandBehavior.Default);
            if (SModel.Sonuc.Equals(Sonuclar.Basarili))
            {
                VeriListe = new List<KatilimciGirisTablosuModel>();
                while (SModel.Reader.Read())
                {
                    if (KayitBilgisiAl(KatilimciGirisIndex, SModel.Reader).Sonuc.Equals(Sonuclar.Basarili))
                    {
                        SDataModel.Veriler.KatilimciBilgisi = new KatilimciTablosuIslemler().KayitBilgisiAl(KatilimciIndex, SModel.Reader).Veriler;
                        SDataModel.Veriler.KatilimciBilgisi.KatilimciTipiBilgisi = new KatilimciTipiTablosuIslemler().KayitBilgisiAl(KatilimciTipiIndex, SModel.Reader).Veriler;
                        VeriListe.Add(SDataModel.Veriler);
                    }
                    else
                    {
                        SDataListModel = new SurecVeriModel<IList<KatilimciGirisTablosuModel>>
                        {
                            Sonuc = SDataModel.Sonuc,
                            KullaniciMesaji = SDataModel.KullaniciMesaji,
                            HataBilgi = SDataModel.HataBilgi
                        };
                        VTIslem.CloseConnection();
                        return SDataListModel;
                    }
                }
                SDataListModel = new SurecVeriModel<IList<KatilimciGirisTablosuModel>>
                {
                    Sonuc = Sonuclar.Basarili,
                    KullaniciMesaji = "Veri listesi baþarýyla çekildi",
                    Veriler = VeriListe
                };
            }
            else
            {
                SDataListModel = new SurecVeriModel<IList<KatilimciGirisTablosuModel>>
                {
                    Sonuc = SModel.Sonuc,
                    KullaniciMesaji = SModel.KullaniciMesaji,
                    HataBilgi = SModel.HataBilgi
                };
            }
            VTIslem.CloseConnection();
            return SDataListModel;
        }

        public SurecVeriModel<IList<KatilimciGirisTablosuModel>> KayitBilgileri(string KullaniciID)
        {
            int
                KatilimciGirisIndex = 0,
                KatilimciIndex = KatilimciGirisIndex + KatilimciGirisTablosuModel.OzellikSayisi,
                KatilimciTipiIndex = KatilimciIndex + KatilimciTablosuModel.OzellikSayisi;

            VTIslem.SetCommandText("SELECT TOP 5 KatilimciGirisTablosu.*, KatilimciTablosu.*, KatilimciTipiTablosu.*  FROM (KatilimciGirisTablosu INNER JOIN KatilimciTablosu ON KatilimciGirisTablosu.KatilimciID = KatilimciTablosu.KatilimciID) INNER JOIN KatilimciTipiTablosu ON KatilimciTablosu.KatilimciTipiID = KatilimciTipiTablosu.KatilimciTipiID WHERE KullaniciID = @KullaniciID ORDER BY KatilimciGirisTablosu.EklenmeTarihi DESC");
            VTIslem.AddWithValue("KullaniciID", KullaniciID);
            VTIslem.OpenConnection();
            SModel = VTIslem.ExecuteReader(CommandBehavior.Default);
            if (SModel.Sonuc.Equals(Sonuclar.Basarili))
            {
                VeriListe = new List<KatilimciGirisTablosuModel>();
                while (SModel.Reader.Read())
                {
                    if (KayitBilgisiAl(KatilimciGirisIndex, SModel.Reader).Sonuc.Equals(Sonuclar.Basarili))
                    {
                        SDataModel.Veriler.KatilimciBilgisi = new KatilimciTablosuIslemler().KayitBilgisiAl(KatilimciIndex, SModel.Reader).Veriler;
                        SDataModel.Veriler.KatilimciBilgisi.KatilimciTipiBilgisi = new KatilimciTipiTablosuIslemler().KayitBilgisiAl(KatilimciTipiIndex, SModel.Reader).Veriler;
                        VeriListe.Add(SDataModel.Veriler);
                    }
                    else
                    {
                        SDataListModel = new SurecVeriModel<IList<KatilimciGirisTablosuModel>>
                        {
                            Sonuc = SDataModel.Sonuc,
                            KullaniciMesaji = SDataModel.KullaniciMesaji,
                            HataBilgi = SDataModel.HataBilgi
                        };
                        VTIslem.CloseConnection();
                        return SDataListModel;
                    }
                }
                SDataListModel = new SurecVeriModel<IList<KatilimciGirisTablosuModel>>
                {
                    Sonuc = Sonuclar.Basarili,
                    KullaniciMesaji = "Veri listesi baþarýyla çekildi",
                    Veriler = VeriListe
                };
            }
            else
            {
                SDataListModel = new SurecVeriModel<IList<KatilimciGirisTablosuModel>>
                {
                    Sonuc = SModel.Sonuc,
                    KullaniciMesaji = SModel.KullaniciMesaji,
                    HataBilgi = SModel.HataBilgi
                };
            }
            VTIslem.CloseConnection();
            return SDataListModel;
        }

        public SurecVeriModel<DataTable> KatilimciGirisRaporu()
        {
            VTIslem.SetCommandText("SELECT KatilimciTablosu.AdSoyad AS [Katýlýmcý], KatilimciTipiTablosu.KatilimciTipi AS [Katýlýmcý Tipi], KatilimciTablosu.ePosta AS [e - Posta], AnaKatilimciTablosu.AdSoyad AS [Ana Katýlýmcý], KatilimciGirisTablosu.EklenmeTarihi AS [Giriþ Tarihi] FROM ( ( KatilimciGirisTablosu INNER JOIN KatilimciTablosu ON KatilimciGirisTablosu.KatilimciID = KatilimciTablosu.KatilimciID ) INNER JOIN KatilimciTipiTablosu ON KatilimciTablosu.KatilimciTipiID = KatilimciTipiTablosu.KatilimciTipiID ) LEFT JOIN ( SELECT * FROM KatilimciTablosu WHERE AnaKatilimciID IS NULL ) AS AnaKatilimciTablosu ON KatilimciTablosu.AnaKatilimciID = AnaKatilimciTablosu.KatilimciID ORDER BY KatilimciGirisTablosu.EklenmeTarihi DESC");

            SModel = VTIslem.ExecuteDataAdapter();

            SurecVeriModel<DataTable> SDataRaporModel = new SurecVeriModel<DataTable>
            {
                Sonuc = SModel.Sonuc,
                KullaniciMesaji = SModel.KullaniciMesaji,
                HataBilgi = SModel.HataBilgi,
                Veriler = new DataTable("Giriþ Raporu")
            };

            if (SModel.Sonuc.Equals(Sonuclar.Basarili))
                SModel.Adapter.Fill(SDataRaporModel.Veriler);

            return SDataRaporModel;
        }

        public SurecBilgiModel AdminGirisSil()
        {

            List<string> KatilimciID = new List<string>
            {
                "'bd8ed122-8f3e-494e-87bd-a2d227dda238'",
                "'3c0ee74f-85dc-4d65-958e-87205937c0aa'",
                "'8a0f8f15-b4ac-4bd9-b2f1-e60124ef9013'"
            };


            VTIslem.SetCommandText($"DELETE FROM KatilimciGirisTablosu WHERE KatilimciID IN ({string.Join(",", KatilimciID)})");
            return VTIslem.ExecuteNonQuery();
        }
    }
}
