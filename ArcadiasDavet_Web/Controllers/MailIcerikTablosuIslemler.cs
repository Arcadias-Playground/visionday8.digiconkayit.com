using Model;
using System.Data;
using System.Data.OleDb;
using VeritabaniIslemMerkeziBase;

namespace VeritabaniIslemMerkezi
{
    public partial class MailIcerikTablosuIslemler : MailIcerikTablosuIslemlerBase
    {
        public MailIcerikTablosuIslemler() : base() { }

        public MailIcerikTablosuIslemler(OleDbTransaction tran) : base(tran) { }

        public override SurecBilgiModel KayitGuncelle(MailIcerikTablosuModel GuncelKayit)
        {
            VTIslem.SetCommandText("UPDATE MailIcerikTablosu SET Konu=@Konu, HtmlIcerik=@HtmlIcerik, GuncellenmeTarihi=@GuncellenmeTarihi WHERE MailIcerikID=@MailIcerikID");
            VTIslem.AddWithValue("Konu", GuncelKayit.Konu);
            VTIslem.AddWithValue("HtmlIcerik", GuncelKayit.HtmlIcerik);
            VTIslem.AddWithValue("GuncellenmeTarihi", GuncelKayit.GuncellenmeTarihi);
            VTIslem.AddWithValue("MailIcerikID", GuncelKayit.MailIcerikID);
            return VTIslem.ExecuteNonQuery();
        }

        public virtual SurecVeriModel<MailIcerikTablosuModel> KayitBilgisi(string KatilimciTipiID, int GonderimTipiID)
        {
            VTIslem.SetCommandText("SELECT * FROM MailIcerikTablosu WHERE KatilimciTipiID = @KatilimciTipiID AND GonderimTipiID = @GonderimTipiID");
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
                    SDataModel = new SurecVeriModel<MailIcerikTablosuModel>
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
                SDataModel = new SurecVeriModel<MailIcerikTablosuModel>
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
