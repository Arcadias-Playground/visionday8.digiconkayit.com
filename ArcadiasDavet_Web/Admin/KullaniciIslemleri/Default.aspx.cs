using Model;
using System;
using System.IO;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using VeritabaniIslemMerkezi;

namespace ArcadiasDavet_Web.Admin.KullaniciIslemleri
{
    public partial class Default : Page
    {
        StringBuilder Uyarilar = new StringBuilder();
        BilgiKontrolMerkezi Kontrol = new BilgiKontrolMerkezi();

        SurecBilgiModel SModel;
        SurecVeriModel<KullaniciTablosuModel> SDataModel;


        KullaniciTablosuModel KModel;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ddlKullaniciTipi.DataBind();
                ddlKullaniciTipi.Items.Insert(0, new ListItem { Value = string.Empty, Text = "Seçiniz" });
            }
        }

        protected void lnkbtnYeniKullaniciEkle_Click(object sender, EventArgs e)
        {
            Kontrol.Temizle(txtKullaniciID);
            Kontrol.Temizle(txtAdSoyad);
            Kontrol.Temizle(txtePosta);
            Kontrol.Temizle(txtSifre);
            Kontrol.Temizle(ddlKullaniciTipi);

            UPnlKullaniciEkleGuncelle.Update();
            BilgiKontrolMerkezi.UyariEkrani(this, $"$({UPnlKullaniciEkleGuncelle.ClientID}).modal('show');", false);
        }

        protected void rptKullaniciListesi_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            SDataModel = new KullaniciTablosuIslemler().KayitBilgisi(e.CommandArgument.ToString());

            switch (SDataModel.Sonuc)
            {
                case Sonuclar.KismiBasarili:
                case Sonuclar.Basarisiz:
                    BilgiKontrolMerkezi.UyariEkrani(this, $"UyariBilgilendirme('', '<p>Kullanıcı bilgisi alınırken hata meydana geldi</p><p>Hata mesajı : {SDataModel.HataBilgi.HataMesaji}</p>', false);", false);
                    break;

                default:
                case Sonuclar.VeriBulunamadi:
                    BilgiKontrolMerkezi.UyariEkrani(this, $"UyariBilgilendirme('', '<p>Kullanıcı bulunamadı</p>', false);", false);
                    rptKullaniciListesi.DataBind();
                    UPnlKullaniciListesi.Update();
                    break;


                case Sonuclar.Basarili:

                    txtKullaniciID.Text = SDataModel.Veriler.KullaniciID;
                    ddlKullaniciTipi.SelectedValue = SDataModel.Veriler.KullaniciTipiID;
                    txtAdSoyad.Text = SDataModel.Veriler.AdSoyad;
                    txtePosta.Text = SDataModel.Veriler.ePosta;
                    txtSifre.Text = SDataModel.Veriler.Sifre;

                    UPnlKullaniciEkleGuncelle.Update();
                    BilgiKontrolMerkezi.UyariEkrani(this, $"$({UPnlKullaniciEkleGuncelle.ClientID}).modal('show');", false);
                    break;


            }

        }

        protected void lnkbtnKullaniciEkleGuncelle_Click(object sender, EventArgs e)
        {
            KModel = new KullaniciTablosuModel
            {
                KullaniciID = string.IsNullOrEmpty(txtKullaniciID.Text) ? new KullaniciTablosuIslemler().YeniKullaniciID() : txtKullaniciID.Text,
                KullaniciTipiID = Kontrol.KelimeKontrol(ddlKullaniciTipi, "Kullanıcı tipini seçiniz", ref Uyarilar),
                AdSoyad = Kontrol.KelimeKontrol(txtAdSoyad, "Ad & Soyad boş bırakılamaz", ref Uyarilar),
                ePosta = Kontrol.ePostaKontrol(txtePosta, "e-Posta boş bırakılamaz", "Geçersiz e-Posta adresi girildi", ref Uyarilar),
                Sifre = Kontrol.KelimeKontrol(txtSifre, "Şifre boş bırakılamaz", ref Uyarilar),
                GunellenmeTarihi = Kontrol.Simdi(),
                EklenmeTarihi = Kontrol.Simdi()
            };


            if (string.IsNullOrEmpty(Uyarilar.ToString()))
            {
                if (string.IsNullOrEmpty(txtKullaniciID.Text))
                {
                    SModel = new KullaniciTablosuIslemler().YeniKayitEkle(KModel);

                    if (SModel.Sonuc.Equals(Sonuclar.Basarili))
                    {
                        Kontrol.Temizle(txtKullaniciID);
                        Kontrol.Temizle(txtAdSoyad);
                        Kontrol.Temizle(txtePosta);
                        Kontrol.Temizle(txtSifre);
                        Kontrol.Temizle(ddlKullaniciTipi);
                        UPnlKullaniciEkleGuncelle.Update();

                        rptKullaniciListesi.DataBind();
                        UPnlKullaniciListesi.Update();
                        BilgiKontrolMerkezi.UyariEkrani(this, $"$({UPnlKullaniciEkleGuncelle.ClientID}).modal('hide'); UyariBilgilendirme('', '<p>Kullanıcı eklendi</p>', true);", false);

                        if (KModel.KullaniciTipiID.Equals("CCD7D302-DAB7-4DD4-8B4A-96D7C0B9EE8D"))
                            File.WriteAllText(Server.MapPath($"~/Dosyalar/YaziciDurum/{KModel.ePosta}.status"), "false");
                    }
                    else
                    {
                        BilgiKontrolMerkezi.UyariEkrani(this, $"AltSayfaUyariBilgilendirme('<p>Kullanıcı eklenirken hata meydana geldi</p><p>Hata mesajı : {SModel.HataBilgi.HataMesaji}</p>', false);", false);
                    }
                }
                else
                {
                    SModel = new KullaniciTablosuIslemler().KayitGuncelle(KModel);

                    if (SModel.Sonuc.Equals(Sonuclar.Basarili))
                    {
                        Kontrol.Temizle(txtKullaniciID);
                        Kontrol.Temizle(txtAdSoyad);
                        Kontrol.Temizle(txtePosta);
                        Kontrol.Temizle(txtSifre);
                        Kontrol.Temizle(ddlKullaniciTipi);
                        UPnlKullaniciEkleGuncelle.Update();

                        rptKullaniciListesi.DataBind();
                        UPnlKullaniciListesi.Update();
                        BilgiKontrolMerkezi.UyariEkrani(this, $"$({UPnlKullaniciEkleGuncelle.ClientID}).modal('hide'); UyariBilgilendirme('', '<p>Kullanıcı bilgileri güncellendi</p>', true);", false);
                    }
                    else
                    {
                        BilgiKontrolMerkezi.UyariEkrani(this, $"AltSayfaUyariBilgilendirme('<p>Kullanıcı bilgileri güncellenirken hata meydana geldi</p><p>Hata mesajı : {SModel.HataBilgi.HataMesaji}</p>', false);", false);
                    }
                }
            }
            else
            {
                BilgiKontrolMerkezi.UyariEkrani(this, $"AltSayfaUyariBilgilendirme('{Uyarilar}', false);", false);
            }
        }
    }
}