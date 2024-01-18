using Model;
using System.Data;
using System.Data.OleDb;
using VeritabaniIslemMerkeziBase;

namespace VeritabaniIslemMerkezi
{
    public partial class AntetliKagitIcerikTablosuIslemler : AntetliKagitIcerikTablosuIslemlerBase
    {
        public AntetliKagitIcerikTablosuIslemler() : base() { }

        public AntetliKagitIcerikTablosuIslemler(OleDbTransaction tran) : base(tran) { }

        public SurecBilgiModel OlcuGuncelle(AntetliKagitIcerikTablosuModel GuncelKayit)
        {
            VTIslem.SetCommandText("UPDATE AntetliKagitIcerikTablosu SET X=@X, Y=@Y, Width=@Width, Height=@Height, GuncellenmeTarihi=@GuncellenmeTarihi WHERE AntetliKagitIcerikID=@AntetliKagitIcerikID");
            VTIslem.AddWithValue("X", GuncelKayit.X);
            VTIslem.AddWithValue("Y", GuncelKayit.Y);
            VTIslem.AddWithValue("Width", GuncelKayit.Width);
            VTIslem.AddWithValue("Height", GuncelKayit.Height);
            VTIslem.AddWithValue("GuncellenmeTarihi", GuncelKayit.GuncellenmeTarihi);
            VTIslem.AddWithValue("AntetliKagitIcerikID", GuncelKayit.AntetliKagitIcerikID);
            return VTIslem.ExecuteNonQuery();
        }

        public override SurecVeriModel<AntetliKagitIcerikTablosuModel> KayitBilgisi(int AntetliKagitIcerikID)
        {
            int
                AntetliKagitIcerikIndex = 0,
                AntetliKagitIcerikTipiIndex = AntetliKagitIcerikIndex + AntetliKagitIcerikTablosuModel.OzellikSayisi;

            VTIslem.SetCommandText("SELECT AntetliKagitIcerikTablosu.*, AntetliKagitIcerikTipiTablosu.* FROM AntetliKagitIcerikTablosu INNER JOIN AntetliKagitIcerikTipiTablosu ON AntetliKagitIcerikTablosu.AntetliKagitIcerikTipiID = AntetliKagitIcerikTipiTablosu.AntetliKagitIcerikTipiID WHERE AntetliKagitIcerikID = @AntetliKagitIcerikID");
            VTIslem.AddWithValue("AntetliKagitIcerikID", AntetliKagitIcerikID);
            VTIslem.OpenConnection();
            SModel = VTIslem.ExecuteReader(CommandBehavior.SingleResult);
            if (SModel.Sonuc.Equals(Sonuclar.Basarili))
            {
                while (SModel.Reader.Read())
                {
                    if (KayitBilgisiAl(AntetliKagitIcerikIndex, SModel.Reader).Sonuc.Equals(Sonuclar.Basarili))
                        SDataModel.Veriler.AntetliKagitIcerikTipiBilgisi = new AntetliKagitIcerikTipiTablosuIslemler().KayitBilgisiAl(AntetliKagitIcerikTipiIndex, SModel.Reader).Veriler;
                }
                if (SDataModel is null)
                {
                    SDataModel = new SurecVeriModel<AntetliKagitIcerikTablosuModel>
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
                SDataModel = new SurecVeriModel<AntetliKagitIcerikTablosuModel>
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
