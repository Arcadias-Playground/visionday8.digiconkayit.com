using Model;
using System.Data;
using System.Data.OleDb;
using VeritabaniIslemMerkeziBase;

namespace VeritabaniIslemMerkezi
{
    public partial class YakaKartiIcerikTablosuIslemler : YakaKartiIcerikTablosuIslemlerBase
    {
        public YakaKartiIcerikTablosuIslemler() : base() { }

        public YakaKartiIcerikTablosuIslemler(OleDbTransaction tran) : base(tran) { }

        public override SurecBilgiModel KayitGuncelle(YakaKartiIcerikTablosuModel GuncelKayit)
        {
            VTIslem.SetCommandText("UPDATE YakaKartiIcerikTablosu SET X=@X, Y=@Y, Width=@Width, Height=@Height, GuncellenmeTarihi=@GuncellenmeTarihi WHERE YakaKartiIcerikID=@YakaKartiIcerikID");
            VTIslem.AddWithValue("X", GuncelKayit.X);
            VTIslem.AddWithValue("Y", GuncelKayit.Y);
            VTIslem.AddWithValue("Width", GuncelKayit.Width);
            VTIslem.AddWithValue("Height", GuncelKayit.Height);
            VTIslem.AddWithValue("GuncellenmeTarihi", GuncelKayit.GuncellenmeTarihi);
            VTIslem.AddWithValue("YakaKartiIcerikID", GuncelKayit.YakaKartiIcerikID);
            return VTIslem.ExecuteNonQuery();
        }

        public override SurecVeriModel<YakaKartiIcerikTablosuModel> KayitBilgisi(int YakaKartiIcerikID)
        {
            int
                YakaKartiIcerikIndex = 0,
                YakaKartiIcerikTipiIndex = YakaKartiIcerikIndex + YakaKartiIcerikTablosuModel.OzellikSayisi;

            VTIslem.SetCommandText("SELECT YakaKartiIcerikTablosu.*, YakaKartiIcerikTipiTablosu.* FROM YakaKartiIcerikTablosu INNER JOIN YakaKartiIcerikTipiTablosu ON YakaKartiIcerikTablosu.YakaKartiIcerikTipiID = YakaKartiIcerikTipiTablosu.YakaKartiIcerikTipiID WHERE YakaKartiIcerikID = @YakaKartiIcerikID");
            VTIslem.AddWithValue("YakaKartiIcerikID", YakaKartiIcerikID);
            VTIslem.OpenConnection();
            SModel = VTIslem.ExecuteReader(CommandBehavior.SingleResult);
            if (SModel.Sonuc.Equals(Sonuclar.Basarili))
            {
                while (SModel.Reader.Read())
                {
                    if (KayitBilgisiAl(YakaKartiIcerikIndex, SModel.Reader).Sonuc.Equals(Sonuclar.Basarili))
                        SDataModel.Veriler.YakaKartiIcerikTipiBilgisi = new YakaKartiIcerikTipiTablosuIslemler().KayitBilgisiAl(YakaKartiIcerikTipiIndex, SModel.Reader).Veriler;
                }
                if (SDataModel is null)
                {
                    SDataModel = new SurecVeriModel<YakaKartiIcerikTablosuModel>
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
                SDataModel = new SurecVeriModel<YakaKartiIcerikTablosuModel>
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
