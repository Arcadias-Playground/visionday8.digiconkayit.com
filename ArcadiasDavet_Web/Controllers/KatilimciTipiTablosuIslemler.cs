using Model;
using System;
using System.Data.OleDb;
using VeritabaniIslemMerkeziBase;

namespace VeritabaniIslemMerkezi
{
    public partial class KatilimciTipiTablosuIslemler : KatilimciTipiTablosuIslemlerBase
    {
        public KatilimciTipiTablosuIslemler() : base() { }

        public KatilimciTipiTablosuIslemler(OleDbTransaction tran) : base(tran) { }

        public string YeniKatilimciTipiID()
        {
            string KatilimciTipiID;

            do
            {
                KatilimciTipiID = Guid.NewGuid().ToString();
                VTIslem.SetCommandText("SELECT COUNT(*) FROM KatilimciTipiTablosu WHERE KatilimciTipiID = @KatilimciTipiID");
                VTIslem.AddWithValue("KatilimciTipiID", KatilimciTipiID);
            } while (!Convert.ToInt32(VTIslem.ExecuteScalar()).Equals(0));

            return KatilimciTipiID;
        }

        public override SurecBilgiModel KayitGuncelle(KatilimciTipiTablosuModel GuncelKayit)
        {
            VTIslem.SetCommandText("UPDATE KatilimciTipiTablosu SET KatilimciTipi=@KatilimciTipi, Kontenjan=@Kontenjan, MisafirKontenjan=@MisafirKontenjan, GirisSayisi=@GirisSayisi, YakaKartiBasimSayisi=@YakaKartiBasimSayisi, GuncellenmeTarihi=@GuncellenmeTarihi WHERE KatilimciTipiID=@KatilimciTipiID");
            VTIslem.AddWithValue("KatilimciTipi", GuncelKayit.KatilimciTipi);
            VTIslem.AddWithValue("Kontenjan", GuncelKayit.Kontenjan);
            VTIslem.AddWithValue("MisafirKontenjan", GuncelKayit.MisafirKontenjan);
            VTIslem.AddWithValue("GirisSayisi", GuncelKayit.GirisSayisi);
            VTIslem.AddWithValue("YakaKartiBasimSayisi", GuncelKayit.YakaKartiBasimSayisi);
            VTIslem.AddWithValue("GuncellenmeTarihi", GuncelKayit.GuncellenmeTarihi);
            VTIslem.AddWithValue("KatilimciTipiID", GuncelKayit.KatilimciTipiID);
            return VTIslem.ExecuteNonQuery();
        }

        public SurecBilgiModel KabulEkranIcerikGuncelle(string KatilimciTipiID, string Icerik)
        {
            VTIslem.SetCommandText("UPDATE KatilimciTipiTablosu SET KabulEkranIcerik=@KabulEkranIcerik, GuncellenmeTarihi=@GuncellenmeTarihi WHERE KatilimciTipiID=@KatilimciTipiID");
            VTIslem.AddWithValue("KabulEkranIcerik", Icerik);
            VTIslem.AddWithValue("GuncellenmeTarihi", new BilgiKontrolMerkezi().Simdi());
            VTIslem.AddWithValue("KatilimciTipiID", KatilimciTipiID);
            return VTIslem.ExecuteNonQuery();
        }

        public SurecBilgiModel RedEkranIcerikGuncelle(string KatilimciTipiID, string Icerik)
        {
            VTIslem.SetCommandText("UPDATE KatilimciTipiTablosu SET RedEkranIcerik=@RedEkranIcerik, GuncellenmeTarihi=@GuncellenmeTarihi WHERE KatilimciTipiID=@KatilimciTipiID");
            VTIslem.AddWithValue("RedEkranIcerik", Icerik);
            VTIslem.AddWithValue("GuncellenmeTarihi", new BilgiKontrolMerkezi().Simdi());
            VTIslem.AddWithValue("KatilimciTipiID", KatilimciTipiID);
            return VTIslem.ExecuteNonQuery();
        }
    }
}
