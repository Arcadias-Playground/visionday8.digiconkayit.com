using Model;
using Newtonsoft.Json;
using System;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Text;
using System.Web;
using VeritabaniIslemMerkeziBase;

namespace VeritabaniIslemMerkezi
{
    public partial class KullaniciTablosuIslemler : KullaniciTablosuIslemlerBase
    {
        public KullaniciTablosuIslemler() : base() { }

        public KullaniciTablosuIslemler(OleDbTransaction tran) : base(tran) { }

        public string YeniKullaniciID()
        {
            string KullaniciID;

            do
            {
                KullaniciID = Guid.NewGuid().ToString();
                VTIslem.SetCommandText("SELECT COUNT(*) FROM KullaniciTablosu WHERE KullaniciID = @KullaniciID");
                VTIslem.AddWithValue("KullaniciID", KullaniciID);
            } while (!Convert.ToInt32(VTIslem.ExecuteScalar()).Equals(0));

            return KullaniciID;
        }

        public bool VerifyAuthToken(HttpContext Context)
        {
            if (File.Exists(Context.Server.MapPath($"~/AuthTokens/{Context.User.Identity.Name}.authtoken")))
            {
                if (File.ReadAllText(Context.Server.MapPath($"~/AuthTokens/{Context.User.Identity.Name}.authtoken"), Encoding.UTF8).Equals(Context.Request.Cookies[CookiesModel.AuthToken.CookieName].Value))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
                return false;
        }

        public KullaniciTablosuModel KayitBilgisi(HttpContext Context)
        {
            return JsonConvert.DeserializeObject<KullaniciTablosuModel>(new SifrelemeIslemleri().SifreCoz(Context.Request.Cookies[CookiesModel.UserInfo.CookieName].Value, CookiesModel.AuthToken.Purpose));
        }

        public override SurecBilgiModel KayitGuncelle(KullaniciTablosuModel GuncelKayit)
        {
            VTIslem.SetCommandText("UPDATE KullaniciTablosu SET KullaniciTipiID=@KullaniciTipiID, AdSoyad=@AdSoyad, ePosta=@ePosta, Sifre=@Sifre, GunellenmeTarihi=@GunellenmeTarihi WHERE KullaniciID=@KullaniciID");
            VTIslem.AddWithValue("KullaniciTipiID", GuncelKayit.KullaniciTipiID);
            VTIslem.AddWithValue("AdSoyad", GuncelKayit.AdSoyad);
            VTIslem.AddWithValue("ePosta", GuncelKayit.ePosta);
            VTIslem.AddWithValue("Sifre", GuncelKayit.Sifre);
            VTIslem.AddWithValue("GunellenmeTarihi", GuncelKayit.GunellenmeTarihi);
            VTIslem.AddWithValue("KullaniciID", GuncelKayit.KullaniciID);
            return VTIslem.ExecuteNonQuery();
        }

        public virtual SurecVeriModel<KullaniciTablosuModel> KayitBilgisi_ePosta(string ePosta)
        {
            VTIslem.SetCommandText("SELECT KullaniciTablosu.*, KullaniciTipiTablosu.* FROM KullaniciTablosu INNER JOIN KullaniciTipiTablosu ON KullaniciTablosu.KullaniciTipiID = KullaniciTipiTablosu.KullaniciTipiID WHERE ePosta = @ePosta");
            VTIslem.AddWithValue("ePosta", ePosta);
            VTIslem.OpenConnection();
            SModel = VTIslem.ExecuteReader(CommandBehavior.SingleResult);
            if (SModel.Sonuc.Equals(Sonuclar.Basarili))
            {
                while (SModel.Reader.Read())
                {
                    if (KayitBilgisiAl(0, SModel.Reader).Sonuc.Equals(Sonuclar.Basarili))
                    {
                        SDataModel.Veriler.KullaniciTipiBilgisi = new KullaniciTipiTablosuIslemler().KayitBilgisiAl(KullaniciTablosuModel.OzellikSayisi, SModel.Reader).Veriler;
                    }
                }
                if (SDataModel is null)
                {
                    SDataModel = new SurecVeriModel<KullaniciTablosuModel>
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
                SDataModel = new SurecVeriModel<KullaniciTablosuModel>
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
