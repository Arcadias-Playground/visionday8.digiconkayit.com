using Model;
using System;
using System.Data;
using System.Data.OleDb;
using VeritabaniIslemMerkeziBase;

namespace VeritabaniIslemMerkezi
{
    public partial class MailGonderimTablosuIslemler : MailGonderimTablosuIslemlerBase
    {
        public MailGonderimTablosuIslemler() : base() { }

        public MailGonderimTablosuIslemler(OleDbTransaction tran) : base(tran) { }

        public string YeniMailGonderimID()
        {
            string MailGonderimID;

            do
            {
                MailGonderimID = Guid.NewGuid().ToString();
                VTIslem.SetCommandText("SELECT COUNT(*) FROM MailGonderimTablosu WHERE MailGonderimID = @MailGonderimID");
                VTIslem.AddWithValue("MailGonderimID", MailGonderimID);
            } while (!Convert.ToInt32(VTIslem.ExecuteScalar()).Equals(0));

            return MailGonderimID;
        }

        public override SurecVeriModel<MailGonderimTablosuModel> KayitBilgisi(string MailGonderimID)
        {
            int
                MailGonderimIndex = 0,
                KatilimciIndex = MailGonderimIndex + MailGonderimTablosuModel.OzellikSayisi,
                KatilimciTipiIndex = KatilimciIndex + KatilimciTablosuModel.OzellikSayisi;

            VTIslem.SetCommandText("SELECT MailGonderimTablosu.*, KatilimciTablosu.*, KatilimciTipiTablosu.* FROM (MailGonderimTablosu INNER JOIN KatilimciTablosu ON MailGonderimTablosu.KatilimciID = KatilimciTablosu.KatilimciID) INNER JOIN KatilimciTipiTablosu ON KatilimciTablosu.KatilimciTipiID = KatilimciTipiTablosu.KatilimciTipiID WHERE MailGonderimID = @MailGonderimID");

            VTIslem.AddWithValue("MailGonderimID", MailGonderimID);
            VTIslem.OpenConnection();
            SModel = VTIslem.ExecuteReader(CommandBehavior.SingleResult);
            if (SModel.Sonuc.Equals(Sonuclar.Basarili))
            {
                while (SModel.Reader.Read())
                {
                    KayitBilgisiAl(MailGonderimIndex, SModel.Reader);

                    if (SDataModel.Sonuc.Equals(Sonuclar.Basarili))
                    {
                        SDataModel.Veriler.KatilimciBilgisi = new KatilimciTablosuIslemler().KayitBilgisiAl(KatilimciIndex, SModel.Reader).Veriler;
                        SDataModel.Veriler.KatilimciBilgisi.KatilimciTipiBilgisi = new KatilimciTipiTablosuIslemler().KayitBilgisiAl(KatilimciTipiIndex, SModel.Reader).Veriler;
                    }

                }
                if (SDataModel is null)
                {
                    SDataModel = new SurecVeriModel<MailGonderimTablosuModel>
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
                SDataModel = new SurecVeriModel<MailGonderimTablosuModel>
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
}
