using Model;
using System.Data.OleDb;
using VeritabaniIslemMerkeziBase;

namespace VeritabaniIslemMerkezi
{
    public partial class WebMailTablosuIslemler : WebMailTablosuIslemlerBase
    {
        public WebMailTablosuIslemler() : base() { }

        public WebMailTablosuIslemler(OleDbTransaction tran) : base(tran) { }

        public SurecBilgiModel MailOkumaBilgisiGuncelle(string WebMailID, string KullaniciID)
        {
            VTIslem.SetCommandText("UPDATE WebMailTablosu SET KullaniciID = @KullaniciID, MailGorulmeTarihi = @MailGorulmeTarihi WHERE WebMailID = @WebMailID");
            VTIslem.AddWithValue("KullaniciID", KullaniciID);
            VTIslem.AddWithValue("MailGorulmeTarihi", new BilgiKontrolMerkezi().Simdi());
            VTIslem.AddWithValue("WebMailID", WebMailID);
            return VTIslem.ExecuteNonQuery();
        }
    }
}
