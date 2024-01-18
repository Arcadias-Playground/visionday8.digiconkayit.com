using Ionic.Zip;
using Microsoft.Web.Administration;
using Model;
using System;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using VeritabaniIslemMerkezi;
using VeritabaniIslemMerkezi.Access;

namespace ArcadiasDavet_Admin.Yonetim
{
    public partial class Default : Page
    {
        StringBuilder Uyarilar = new StringBuilder();
        BilgiKontrolMerkezi Kontrol = new BilgiKontrolMerkezi();

        SurecBilgiModel SModel;
        SurecVeriModel<KongreTablosuModel> SDataModel;

        KongreTablosuModel KModel;

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void lnkbtnDijitalDavetiyeSistemiEkle_Click(object sender, EventArgs e)
        {
            Kontrol.Temizle(txtKongreID);
            Kontrol.Temizle(txtKongre);
            Kontrol.Temizle(txtWebUrl);
            txtWebUrl.Enabled = true;
            Kontrol.Temizle(txtKapansTarihi);
            chkboxKapanisHatirlatma.Checked = true;

            UPnlDijitalDavetiyeSistemiEkleGuncelle.Update();
            BilgiKontrolMerkezi.UyariEkrani(this, $"$({UPnlDijitalDavetiyeSistemiEkleGuncelle.ClientID}).modal('show');", false);
        }

        protected void rptDijitalDavetiyeSistemiListesi_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            SDataModel = new KongreTablosuIslemler().KayitBilgisi(Convert.ToInt32(e.CommandArgument));

            switch (e.CommandName)
            {
                case "Guncelle":

                    switch (SDataModel.Sonuc)
                    {
                        case Sonuclar.KismiBasarili:
                        case Sonuclar.Basarisiz:
                            BilgiKontrolMerkezi.UyariEkrani(this, $"UyariBilgilendirme('Dikkat', '<p>Bildiri sistemi bilgisi alınırken hata meydana geldi</p><p>Hata mesajı : {SDataModel.HataBilgi.HataMesaji}</p>', false);", false);
                            break;

                        case Sonuclar.VeriBulunamadi:
                            BilgiKontrolMerkezi.UyariEkrani(this, $"UyariBilgilendirme('Dikkat', '<p>Bildiri sistemi bulunamadı</p>', false);", false);
                            break;

                        case Sonuclar.Basarili:
                            txtKongreID.Text = SDataModel.Veriler.KongreID.ToString();
                            txtKongre.Text = SDataModel.Veriler.Kongre;
                            txtWebUrl.Text = SDataModel.Veriler.WebUrl;
                            txtWebUrl.Enabled = false;
                            txtKapansTarihi.Text = SDataModel.Veriler.KapanisTarihi.HasValue ? SDataModel.Veriler.KapanisTarihi.Value.ToShortDateString() : string.Empty;
                            chkboxKapanisHatirlatma.Checked = SDataModel.Veriler.KapanisHatirlatma;

                            UPnlDijitalDavetiyeSistemiEkleGuncelle.Update();
                            BilgiKontrolMerkezi.UyariEkrani(this, $"$({UPnlDijitalDavetiyeSistemiEkleGuncelle.ClientID}).modal('show');", false);

                            break;
                        default:
                            break;
                    }

                    break;


                case "Sil":


                    switch (SDataModel.Sonuc)
                    {
                        case Sonuclar.KismiBasarili:
                        case Sonuclar.Basarisiz:
                            BilgiKontrolMerkezi.UyariEkrani(this, $"UyariBilgilendirme('Dikkat', '<p>Bildiri sistemi bilgisi alınırken hata meydana geldi</p><p>Hata mesajı : {SDataModel.HataBilgi.HataMesaji}</p>', false);", false);
                            break;

                        case Sonuclar.VeriBulunamadi:
                            BilgiKontrolMerkezi.UyariEkrani(this, $"UyariBilgilendirme('Dikkat', '<p>Bildiri sistemi bulunamadı</p>', false);", false);
                            break;

                        case Sonuclar.Basarili:
                            try
                            {
                                using (ServerManager sm = new ServerManager())
                                {
                                    Site s = sm.Sites.FirstOrDefault(x => x.Name.Equals(Request.Url.Authority));
                                    if (!(s is null))
                                    {
                                        Application app = s.Applications.FirstOrDefault(x => x.Path.Equals($"/{SDataModel.Veriler.WebUrl}"));
                                        if (!(app is null))
                                        {
                                            s.Applications.Remove(app);
                                            sm.CommitChanges();
                                        }
                                    }
                                }

                                try
                                {
                                    if (Directory.Exists(Server.MapPath($"~/{SDataModel.Veriler.WebUrl}")))
                                    {
                                        using (ZipFile ZFile = new ZipFile())
                                        {
                                            ZFile.AddDirectoryByName(SDataModel.Veriler.WebUrl);
                                            ZFile.AddSelectedFiles("*.*", Server.MapPath($"~/{SDataModel.Veriler.WebUrl}"), SDataModel.Veriler.WebUrl, true);

                                            using (MemoryStream ms = new MemoryStream())
                                            {
                                                ZFile.Save(ms);

                                                if (new FTPIslemleri().DosyaYukleme(Request.Url.Authority.Split('.').First(), SDataModel.Veriler.WebUrl, Server.MapPath($"~/Dosyalar/Arsiv/{SDataModel.Veriler.WebUrl}_{DateTime.Now:yyyyMMdd_HHmmss}.log"), ms).Sonuc.Equals(Sonuclar.Basarisiz))
                                                {
                                                    ZFile.Save(Server.MapPath($"~/Dosyalar/Arsiv/{SDataModel.Veriler.WebUrl}_{DateTime.Now:yyyyMMdd_HHmmss}.zip"));
                                                }
                                            }
                                        }
                                        Directory.Delete(Server.MapPath($"~/{SDataModel.Veriler.WebUrl}"), true);
                                    }

                                    SModel = new KongreTablosuIslemler().KayitSil(SDataModel.Veriler.KongreID);

                                    if (SModel.Sonuc.Equals(Sonuclar.Basarili))
                                    {
                                        rptDijitalDavetiyeSistemiListesi.DataBind();
                                        UPnlDijitalDavetiyeSistemiListesi.Update();
                                        BilgiKontrolMerkezi.UyariEkrani(this, $"UyariBilgilendirme('Başarılı İşlem', '<p>Bildiri sistemi silindi</p>', true);  DataTableKurulum(tbl_BildiriSistemiListesi, false); ", false);
                                    }
                                    else
                                    {
                                        BilgiKontrolMerkezi.UyariEkrani(this, $"UyariBilgilendirme('Dikkat', '<p>Bildiri sistemi veritabanından silinirken hata meydana geldi</p><p>Hata mesajı : {SModel.HataBilgi.HataMesaji}</p>', false);", false);
                                    }
                                }
                                catch (Exception ex)
                                {
                                    BilgiKontrolMerkezi.UyariEkrani(this, $"UyariBilgilendirme('Dikkat', '<p>Dosyalar silinirken hata meydana geldi</p><p>Hata mesajı : {ex.Message.Replace("\r", string.Empty).Replace("\n", string.Empty).Replace("\t", string.Empty).Replace("'", string.Empty)}</p>', false);", false);
                                }
                            }
                            catch (Exception ex)
                            {
                                BilgiKontrolMerkezi.UyariEkrani(this, $"UyariBilgilendirme('Dikkat', '<p>IIS yapılandırması sırasında hata meydana geldi</p><p>Hata mesajı : {ex.Message.Replace("\r", string.Empty).Replace("\n", string.Empty).Replace("\t", string.Empty).Replace("'", string.Empty)}</p>', false);", false);
                            }

                            break;
                        default:
                            break;
                    }

                    break;

                default:
                    break;
            }
        }

        protected void lnkbtnDijitalDavetiyeSistemiEkleGuncelle_Click(object sender, EventArgs e)
        {
            KModel = new KongreTablosuModel
            {
                KongreID = string.IsNullOrEmpty(txtKongreID.Text) ? 0 : Convert.ToInt32(txtKongreID.Text),
                Kongre = Kontrol.KelimeKontrol(txtKongre, "Bildiri sistemi adı boş olamaz", ref Uyarilar),
                WebUrl = Kontrol.KelimeKontrol(txtWebUrl, "Bildiri URL boş olamaz", ref Uyarilar),
                KapanisTarihi = string.IsNullOrEmpty(txtKapansTarihi.Text) ? null : Kontrol.BosTariheKontrol(txtKapansTarihi, "Kapanma tarihi boş bırakılamaz", "Geçersiz kapanma tarihi girildi", ref Uyarilar),
                KapanisHatirlatma = chkboxKapanisHatirlatma.Checked,
                GuncellenmeTarihi = Kontrol.Simdi(),
                EklenmeTarihi = Kontrol.Simdi()
            };

            if (string.IsNullOrEmpty(Uyarilar.ToString()))
            {
                if (KModel.KongreID.Equals(0))
                {
                    // Yeni sistem

                    using (OleDbConnection cnn = new OleDbConnection(ConnectionBuilder.DefaultConnectionString))
                    {
                        ConnectionBuilder.OpenConnection(cnn);
                        using (OleDbTransaction trn = cnn.BeginTransaction())
                        {
                            SModel = new KongreTablosuIslemler(trn).YeniKayitEkle(KModel);
                            if (SModel.Sonuc.Equals(Sonuclar.Basarili))
                            {
                                try
                                {
                                    if (!Directory.Exists(Server.MapPath($"~/{KModel.WebUrl}")))
                                    {
                                        foreach (string dirPath in Directory.GetDirectories(Server.MapPath($"~/Yonetim/Web"), "*", SearchOption.AllDirectories))
                                            Directory.CreateDirectory(dirPath.Replace(Server.MapPath($"~/Yonetim/Web"), Server.MapPath($"~/{KModel.WebUrl}")));

                                        foreach (string newPath in Directory.GetFiles(Server.MapPath($"~/Yonetim/Web"), "*.*", SearchOption.AllDirectories))
                                            File.Copy(newPath, newPath.Replace(Server.MapPath($"~/Yonetim/Web"), Server.MapPath($"~/{KModel.WebUrl}")), true);

                                        string WebConfigContent = File.ReadAllText(Server.MapPath($"~/{KModel.WebUrl}/Web.config"));
                                        string UpdatedWebConfigContent = WebConfigContent
                                            .Replace(".ArcadiasDavetAT", $".{Guid.NewGuid().ToString().Substring(0, 8)}")
                                            .Replace(".ArcadiasDavetUI", $".{Guid.NewGuid().ToString().Substring(0, 8)}")
                                            .Replace(".ArcadiasDavetAuth", $".{Guid.NewGuid().ToString().Substring(0, 8)}")
                                            .Replace(".ArcadiasDavetRole", $".{Guid.NewGuid().ToString().Substring(0, 8)}")
                                            .Replace("A464D8C9BCDA0F1D591966E3898DA48898575E1A37C08A8983C73E4E5ACDE314D86722F465F50A9590CEDD18A37B753D4DE0FB2F593C7E8927B0D50A733839F4", GetRandomKey(64))
                                            .Replace("EE5DA5D59724C00BC3037AB248010C67AB24A7786E898941E1BB174393257CC6", GetRandomKey(32));

                                        File.WriteAllText(Server.MapPath($"~/{KModel.WebUrl}/Web.config"), UpdatedWebConfigContent);
                                    }

                                    try
                                    {
                                        using (ServerManager sm = new ServerManager())
                                        {
                                            string Authority = Request.Url.Authority;

                                            Site site = sm.Sites.First(x => x.Name.Equals(Authority));
                                            Application app = site.Applications.Add($"/{KModel.WebUrl}", Server.MapPath($"~/{KModel.WebUrl}"));

                                            app.ApplicationPoolName = Authority;

                                            string UserName = string.Empty;
                                            if (Authority.Length <= 20)
                                                UserName = Authority.Last().Equals('.') ? Authority.Remove(Authority.Length - 1) : Authority;
                                            else
                                                UserName = Authority.Substring(0, 20).Last().Equals('.') ? Authority.Substring(0, 19) : Authority.Substring(0, 20);

                                            string Password = UserName;
                                            while (int.TryParse(Password.First().ToString(), out int Sayi))
                                                Password = Password.Remove(0, 1);

                                            app.VirtualDirectories[0].UserName = UserName;
                                            app.VirtualDirectories[0].Password = $"{Password.First()}!123";
                                            sm.CommitChanges();
                                        }


                                        trn.Commit();
                                        rptDijitalDavetiyeSistemiListesi.DataBind();
                                        UPnlDijitalDavetiyeSistemiListesi.Update();
                                        BilgiKontrolMerkezi.UyariEkrani(this, $"$({UPnlDijitalDavetiyeSistemiEkleGuncelle.ClientID}).modal('hide'); UyariBilgilendirme('Başarılı İşlem', '<p>Dijital davetiye sistemi açıldı</p>', true);  DataTableKurulum(tbl_DijitalDavetiyeSistemiListesi, false); ", false);

                                    }
                                    catch (Exception ex)
                                    {

                                        trn.Rollback();

                                        if (Directory.Exists(Server.MapPath($"~/{KModel.WebUrl}")))
                                            Directory.Delete(Server.MapPath($"~/{KModel.WebUrl}"), true);

                                        BilgiKontrolMerkezi.UyariEkrani(this, $"AltSayfaUyariBilgilendirme('<p>IIS yapılandırması sırasında hata meydana geldi</p><p>Hata mesajı : {ex.Message.Replace("\r", string.Empty).Replace("\n", string.Empty).Replace("\t", string.Empty).Replace("'", string.Empty)}</p>', false);", false);
                                    }
                                }
                                catch (Exception ex)
                                {
                                    trn.Rollback();

                                    if (Directory.Exists(Server.MapPath($"~/{KModel.WebUrl}")))
                                        Directory.Delete(Server.MapPath($"~/{KModel.WebUrl}"), true);

                                    BilgiKontrolMerkezi.UyariEkrani(this, $"AltSayfaUyariBilgilendirme('<p>Dijital davetiye sistemi dosyaları kopyalanırken hata meydana geldi</p><p>Hata mesajı : {ex.Message.Replace("\r", string.Empty).Replace("\n", string.Empty).Replace("\t", string.Empty).Replace("'", string.Empty)}</p>', false);", false);
                                }
                            }
                            else
                            {
                                trn.Rollback();
                                BilgiKontrolMerkezi.UyariEkrani(this, $"AltSayfaUyariBilgilendirme('<p>Dijital davetiye sistemi bilgileri güncellenirken hata meydana geldi</p><p>Hata mesajı : {SModel.HataBilgi.HataMesaji}</p>', false);", false);
                            }
                        }
                        ConnectionBuilder.CloseConnection(cnn);
                    }
                }
                else
                {
                    // Güncelleme
                    SModel = new KongreTablosuIslemler().KayitGuncelle(KModel);

                    if (SModel.Sonuc.Equals(Sonuclar.Basarili))
                    {
                        rptDijitalDavetiyeSistemiListesi.DataBind();
                        UPnlDijitalDavetiyeSistemiListesi.Update();

                        BilgiKontrolMerkezi.UyariEkrani(this, $"$({UPnlDijitalDavetiyeSistemiEkleGuncelle.ClientID}).modal('hide'); UyariBilgilendirme('Başarılı İşlem', '<p>Dijital davetiye sistemi bilgileri güncellendi</p>', true);  DataTableKurulum(tbl_DijitalDavetiyeSistemiListesi, false); ", false);
                    }
                    else
                    {
                        BilgiKontrolMerkezi.UyariEkrani(this, $"AltSayfaUyariBilgilendirme('<p>Dijital davetiye sistemi bilgileri güncellenirken hata meydana geldi</p><p>Hata mesajı : {SModel.HataBilgi.HataMesaji}</p>', false);", false);
                    }
                }
            }
            else
            {
                BilgiKontrolMerkezi.UyariEkrani(this, $"UyariBilgilendirme('Dikkat', '{Uyarilar}', false);", false);
            }
        }

        string GetRandomKey(int bytelength)
        {

            byte[] buff = new byte[bytelength];
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            rng.GetBytes(buff);
            StringBuilder sb = new StringBuilder(bytelength * 2);
            for (int i = 0; i < buff.Length; i++)
                sb.Append(string.Format("{0:X2}", buff[i]));
            return sb.ToString();
        }
    }
}