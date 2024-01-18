using Model;
using System.Data;
using System.Data.OleDb;
using VeritabaniIslemMerkeziBase;

namespace VeritabaniIslemMerkezi
{
    public partial class YakaKartiCerceveTablosuIslemler : YakaKartiCerceveTablosuIslemlerBase
    {
        public YakaKartiCerceveTablosuIslemler() : base() { }

        public YakaKartiCerceveTablosuIslemler(OleDbTransaction tran) : base(tran) { }

        public override SurecBilgiModel KayitGuncelle(YakaKartiCerceveTablosuModel GuncelKayit)
        {
            VTIslem.SetCommandText("UPDATE YakaKartiCerceveTablosu SET Width=@Width, Height=@Height, YaziciKagitOrtalama=@YaziciKagitOrtalama, GuncellenmeTarihi=@GuncellenmeTarihi WHERE YakaKartiCerceveID=@YakaKartiCerceveID");
            VTIslem.AddWithValue("Width", GuncelKayit.Width);
            VTIslem.AddWithValue("Height", GuncelKayit.Height);
            VTIslem.AddWithValue("YaziciKagitOrtalama", GuncelKayit.YaziciKagitOrtalama);
            VTIslem.AddWithValue("GuncellenmeTarihi", GuncelKayit.GuncellenmeTarihi);
            VTIslem.AddWithValue("YakaKartiCerceveID", GuncelKayit.YakaKartiCerceveID);
            return VTIslem.ExecuteNonQuery();
        }

        public SurecVeriModel<YakaKartiCerceveTablosuModel> KayitBilgisi(string KatilimciTipiID)
        {
            VTIslem.SetCommandText("SELECT * FROM YakaKartiCerceveTablosu WHERE KatilimciTipiID = @KatilimciTipiID");
            VTIslem.AddWithValue("KatilimciTipiID", KatilimciTipiID);
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
                    SDataModel = new SurecVeriModel<YakaKartiCerceveTablosuModel>
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
                SDataModel = new SurecVeriModel<YakaKartiCerceveTablosuModel>
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
