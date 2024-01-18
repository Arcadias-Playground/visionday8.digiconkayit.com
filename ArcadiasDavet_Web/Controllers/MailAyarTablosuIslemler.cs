using Model;
using System.Data.OleDb;
using VeritabaniIslemMerkeziBase;

namespace VeritabaniIslemMerkezi
{
    public partial class MailAyarTablosuIslemler : MailAyarTablosuIslemlerBase
    {
        public MailAyarTablosuIslemler() : base() { }

        public MailAyarTablosuIslemler(OleDbTransaction tran) : base(tran) { }

        public override SurecBilgiModel KayitGuncelle(MailAyarTablosuModel GuncelKayit)
        {
            VTIslem.SetCommandText("UPDATE MailAyarTablosu SET GonderenAd=@GonderenAd, ePosta=@ePosta, KullaniciAdi=@KullaniciAdi, Sifre=@Sifre, GidenMailHost=@GidenMailHost, GidenMailPort=@GidenMailPort, GelenMailHost=@GelenMailHost, GelenMailPort=@GelenMailPort, SSL=@SSL, BCC=@BCC, ReplyTo=@ReplyTo, GuncellenmeTarihi=@GuncellenmeTarihi WHERE MailAyarID=@MailAyarID");
            VTIslem.AddWithValue("GonderenAd", GuncelKayit.GonderenAd);
            VTIslem.AddWithValue("ePosta", GuncelKayit.ePosta);
            VTIslem.AddWithValue("KullaniciAdi", GuncelKayit.KullaniciAdi);
            VTIslem.AddWithValue("Sifre", GuncelKayit.Sifre);
            VTIslem.AddWithValue("GidenMailHost", GuncelKayit.GidenMailHost);
            VTIslem.AddWithValue("GidenMailPort", GuncelKayit.GidenMailPort);
            VTIslem.AddWithValue("GelenMailHost", GuncelKayit.GelenMailHost);
            VTIslem.AddWithValue("GelenMailPort", GuncelKayit.GelenMailPort);
            VTIslem.AddWithValue("SSL", GuncelKayit.SSL);
            VTIslem.AddWithValue("BCC", GuncelKayit.BCC);
            VTIslem.AddWithValue("ReplyTo", GuncelKayit.ReplyTo);
            VTIslem.AddWithValue("GuncellenmeTarihi", GuncelKayit.GuncellenmeTarihi);
            VTIslem.AddWithValue("MailAyarID", GuncelKayit.MailAyarID);
            return VTIslem.ExecuteNonQuery();
        }
    }
}
