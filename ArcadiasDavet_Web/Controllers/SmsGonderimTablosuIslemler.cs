using Model;
using System;
using System.Data;
using System.Data.OleDb;
using VeritabaniIslemMerkeziBase;

namespace VeritabaniIslemMerkezi
{
    public partial class SmsGonderimTablosuIslemler : SmsGonderimTablosuIslemlerBase
    {
        public SmsGonderimTablosuIslemler() : base() { }

        public SmsGonderimTablosuIslemler(OleDbTransaction tran) : base(tran) { }

        public string YeniSmsGonderimID()
        {
            string SmsGonderimID;

            do
            {
                SmsGonderimID = Guid.NewGuid().ToString();
                VTIslem.SetCommandText("SELECT COUNT(*) FROM SmsGonderimTablosu WHERE SmsGonderimID=@SmsGonderimID");
                VTIslem.AddWithValue("SmsGonderimID", SmsGonderimID);
            } while (!Convert.ToInt32(VTIslem.ExecuteScalar()).Equals(0));

            return SmsGonderimID;
        }

        public override SurecVeriModel<SmsGonderimTablosuModel> KayitBilgisi(string SmsGonderimID)
        {
            int
                SmsGonderimIndex = 0,
                KatilimciIndex = SmsGonderimIndex + SmsGonderimTablosuModel.OzellikSayisi,
                KatilimciTipiIndex = KatilimciIndex + KatilimciTablosuModel.OzellikSayisi;

            VTIslem.SetCommandText("SELECT SmsGonderimTablosu.*, KatilimciTablosu.*, KatilimciTipiTablosu.* FROM ( SmsGonderimTablosu INNER JOIN KatilimciTablosu ON SmsGonderimTablosu.KatilimciID = KatilimciTablosu.KatilimciID ) INNER JOIN KatilimciTipiTablosu ON KatilimciTablosu.KatilimciTipiID = KatilimciTipiTablosu.KatilimciTipiID WHERE SmsGonderimID = @SmsGonderimID");
            VTIslem.AddWithValue("SmsGonderimID", SmsGonderimID);
            VTIslem.OpenConnection();
            SModel = VTIslem.ExecuteReader(CommandBehavior.SingleResult);
            if (SModel.Sonuc.Equals(Sonuclar.Basarili))
            {
                while (SModel.Reader.Read())
                {
                    if (KayitBilgisiAl(SmsGonderimIndex, SModel.Reader).Sonuc.Equals(Sonuclar.Basarili))
                    {
                        SDataModel.Veriler.KatilimciBilgisi = new KatilimciTablosuIslemler().KayitBilgisiAl(KatilimciIndex, SModel.Reader).Veriler;
                        SDataModel.Veriler.KatilimciBilgisi.KatilimciTipiBilgisi = new KatilimciTipiTablosuIslemler().KayitBilgisiAl(KatilimciTipiIndex, SModel.Reader).Veriler;
                    }
                }
                if (SDataModel is null)
                {
                    SDataModel = new SurecVeriModel<SmsGonderimTablosuModel>
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
                SDataModel = new SurecVeriModel<SmsGonderimTablosuModel>
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
