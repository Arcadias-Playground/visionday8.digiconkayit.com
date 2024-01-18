using Model;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using VeritabaniIslemMerkeziBase;

namespace VeritabaniIslemMerkezi
{
    public partial class YakaKartiBasimTablosuIslemler : YakaKartiBasimTablosuIslemlerBase
    {
        public YakaKartiBasimTablosuIslemler() : base() { }

        public YakaKartiBasimTablosuIslemler(OleDbTransaction tran) : base(tran) { }

        public override SurecVeriModel<IList<YakaKartiBasimTablosuModel>> KayitBilgileri()
        {
            int
               YakaKartiBasimIndex = 0,
               KatilimciIndex = YakaKartiBasimIndex + YakaKartiBasimTablosuModel.OzellikSayisi,
               KatilimciTipiIndex = KatilimciIndex + KatilimciTablosuModel.OzellikSayisi;

            VTIslem.SetCommandText("SELECT YakaKartiBasimTablosu.*, KatilimciTablosu.*, KatilimciTipiTablosu.* FROM ( YakaKartiBasimTablosu INNER JOIN KatilimciTablosu ON YakaKartiBasimTablosu.KatilimciID = KatilimciTablosu.KatilimciID ) INNER JOIN KatilimciTipiTablosu ON KatilimciTablosu.KatilimciTipiID = KatilimciTipiTablosu.KatilimciTipiID");
            VTIslem.OpenConnection();
            SModel = VTIslem.ExecuteReader(CommandBehavior.Default);
            if (SModel.Sonuc.Equals(Sonuclar.Basarili))
            {
                VeriListe = new List<YakaKartiBasimTablosuModel>();
                while (SModel.Reader.Read())
                {
                    if (KayitBilgisiAl(0, SModel.Reader).Sonuc.Equals(Sonuclar.Basarili))
                    {
                        SDataModel.Veriler.KatilimciBilgisi = new KatilimciTablosuIslemler().KayitBilgisiAl(KatilimciIndex, SModel.Reader).Veriler;
                        SDataModel.Veriler.KatilimciBilgisi.KatilimciTipiBilgisi = new KatilimciTipiTablosuIslemler().KayitBilgisiAl(KatilimciTipiIndex, SModel.Reader).Veriler;
                        VeriListe.Add(SDataModel.Veriler);
                    }
                    else
                    {
                        SDataListModel = new SurecVeriModel<IList<YakaKartiBasimTablosuModel>>
                        {
                            Sonuc = SDataModel.Sonuc,
                            KullaniciMesaji = SDataModel.KullaniciMesaji,
                            HataBilgi = SDataModel.HataBilgi
                        };
                        VTIslem.CloseConnection();
                        return SDataListModel;
                    }
                }
                SDataListModel = new SurecVeriModel<IList<YakaKartiBasimTablosuModel>>
                {
                    Sonuc = Sonuclar.Basarili,
                    KullaniciMesaji = "Veri listesi baþarýyla çekildi",
                    Veriler = VeriListe
                };
            }
            else
            {
                SDataListModel = new SurecVeriModel<IList<YakaKartiBasimTablosuModel>>
                {
                    Sonuc = SModel.Sonuc,
                    KullaniciMesaji = SModel.KullaniciMesaji,
                    HataBilgi = SModel.HataBilgi
                };
            }
            VTIslem.CloseConnection();
            return SDataListModel;
        }

        public SurecVeriModel<DataTable> YakaKartiBasimRaporu()
        {
            VTIslem.SetCommandText("SELECT KatilimciTablosu.AdSoyad AS [Katýlýmcý], KatilimciTipiTablosu.KatilimciTipi AS [Katýlýmcý Tipi], KatilimciTablosu.ePosta AS [e - Posta], AnaKatilimciTablosu.AdSoyad AS [Ana Katýlýmcý], YakaKartiBasimTablosu.EklenmeTarihi AS [Basým Tarihi], KullaniciTablosu.ePosta AS [Basým Yapan Hesap] FROM ( ( ( YakaKartiBasimTablosu INNER JOIN KullaniciTablosu ON YakaKartiBasimTablosu.KullaniciID = KullaniciTablosu.KullaniciID ) INNER JOIN KatilimciTablosu ON YakaKartiBasimTablosu.KatilimciID = KatilimciTablosu.KatilimciID ) INNER JOIN KatilimciTipiTablosu ON KatilimciTablosu.KatilimciTipiID = KatilimciTipiTablosu.KatilimciTipiID ) LEFT JOIN ( SELECT * FROM KatilimciTablosu WHERE AnaKatilimciID IS NULL ) AS AnaKatilimciTablosu ON KatilimciTablosu.AnaKatilimciID = AnaKatilimciTablosu.KatilimciID ORDER BY YakaKartiBasimTablosu.EklenmeTarihi DESC");

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
