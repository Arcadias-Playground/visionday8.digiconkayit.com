using Model;
using System.Data;
using System.Data.OleDb;
using VeritabaniIslemMerkeziBase;

namespace VeritabaniIslemMerkezi
{
    public partial class SmsIcerikTablosuIslemler : SmsIcerikTablosuIslemlerBase
    {
        public SmsIcerikTablosuIslemler() : base() { }

        public SmsIcerikTablosuIslemler(OleDbTransaction tran) : base(tran) { }

        public SurecBilgiModel SmsIcerikGuncelle(SmsIcerikTablosuModel GuncelKayit)
        {
            VTIslem.SetCommandText("UPDATE SmsIcerikTablosu SET SmsIcerik=@SmsIcerik, GuncellenmeTarihi=@GuncellenmeTarihi WHERE SmsIcerikID=@SmsIcerikID");
            VTIslem.AddWithValue("SmsIcerik", GuncelKayit.SmsIcerik);
            VTIslem.AddWithValue("GuncellenmeTarihi", GuncelKayit.GuncellenmeTarihi);
            VTIslem.AddWithValue("SmsIcerikID", GuncelKayit.SmsIcerikID);
            return VTIslem.ExecuteNonQuery();
        }

        public virtual SurecVeriModel<SmsIcerikTablosuModel> KayitBilgisi(string KatilimciTipiID, bool AnaKatilimci, int GonderimTipiID)
        {
            VTIslem.SetCommandText("SELECT * FROM SmsIcerikTablosu WHERE KatilimciTipiID = @KatilimciTipiID AND GonderimTipiID = @GonderimTipiID");
            VTIslem.AddWithValue("KatilimciTipiID", KatilimciTipiID);
            VTIslem.AddWithValue("GonderimTipiID", GonderimTipiID);
            VTIslem.OpenConnection();
            SModel = VTIslem.ExecuteReader(CommandBehavior.SingleResult);
            if (SModel.Sonuc.Equals(Sonuclar.Basarili))
            {
                while (SModel.Reader.Read())
                {
                    KayitBilgisiAl(0, SModel.Reader);
                }
                if (SDataModel is null)
                {
                    SDataModel = new SurecVeriModel<SmsIcerikTablosuModel>
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
                SDataModel = new SurecVeriModel<SmsIcerikTablosuModel>
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
