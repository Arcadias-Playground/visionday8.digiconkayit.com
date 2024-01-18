using Model;
using System;
using System.Text;
using System.Web.UI;
using VeritabaniIslemMerkezi;

namespace ArcadiasDavet_Web.Admin.MailIslemleri
{
    public partial class MailGonderimAyar : Page
    {
        StringBuilder Uyarilar = new StringBuilder();
        BilgiKontrolMerkezi Kontrol = new BilgiKontrolMerkezi();

        SurecBilgiModel SModel;
        SurecVeriModel<MailAyarTablosuModel> SDataModel;

        MailAyarTablosuModel MGModel;


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                SDataModel = new MailAyarTablosuIslemler().KayitBilgisi(1);

                switch (SDataModel.Sonuc)
                {
                    case Sonuclar.KismiBasarili:
                    case Sonuclar.Basarisiz:
                        BilgiKontrolMerkezi.UyariEkrani(this, $"UyariBilgilendirme('', '<p>Mail gönderim ayarları alınırken hata meydana geldi</p><p>Hata mesajı : {SDataModel.HataBilgi.HataMesaji}</p>', false);", true);
                        break;

                    default:
                    case Sonuclar.VeriBulunamadi:
                        BilgiKontrolMerkezi.UyariEkrani(this, $"UyariBilgilendirme('', '<p>Mail gönderim ayarı bulunamadı</p>', false);", true);
                        break;

                    case Sonuclar.Basarili:
                        txtGonderenAd.Text = SDataModel.Veriler.GonderenAd;
                        txtePosta.Text = SDataModel.Veriler.ePosta;
                        txtKullaniciAdi.Text = SDataModel.Veriler.KullaniciAdi;
                        txtSifre.Text = SDataModel.Veriler.Sifre;
                        txtGidenMailHost.Text = SDataModel.Veriler.GidenMailHost;
                        txtGidenMailPort.Text = SDataModel.Veriler.GidenMailPort.ToString();
                        txtGelenMailHost.Text = SDataModel.Veriler.GelenMailHost;
                        txtGelenMailPort.Text = SDataModel.Veriler.GelenMailPort.ToString();
                        chkSSL.Checked = SDataModel.Veriler.SSL;
                        txtBCC.Text = SDataModel.Veriler.BCC;
                        txtReplyTo.Text = SDataModel.Veriler.ReplyTo;

                        lnkbtnMailAyarGuncelle.CommandArgument = SDataModel.Veriler.MailAyarID.ToString();
                        break;


                }
            }
        }

        protected void lnkbtnMailAyarGuncelle_Click(object sender, EventArgs e)
        {
            MGModel = new MailAyarTablosuModel
            {
                MailAyarID = Convert.ToInt32(lnkbtnMailAyarGuncelle.CommandArgument),
                GonderenAd = Kontrol.KelimeKontrol(txtGonderenAd, "Gönderen ad boş bırakılamaz", ref Uyarilar),
                ePosta = Kontrol.ePostaKontrol(txtePosta, "e-Posta adresi boş bırakılamaz", "Geçersiz e-Posta adresi girildi", ref Uyarilar),
                KullaniciAdi = Kontrol.KelimeKontrol(txtKullaniciAdi, "Kullanıcı adı boş bırakılamaz", ref Uyarilar),
                Sifre = Kontrol.KelimeKontrol(txtSifre, "Şifre boş bırakılamaz", ref Uyarilar),
                GidenMailHost = Kontrol.KelimeKontrol(txtGidenMailHost, "Giden mail host adresi boş bırakılamaz", ref Uyarilar),
                GidenMailPort = Kontrol.TamSayiyaKontrol(txtGidenMailPort, "Giden mail port numarası boş bırakılamaz", "Geçersiz giden mail port numarası girildi", ref Uyarilar),
                GelenMailHost = Kontrol.KelimeKontrol(txtGelenMailHost, "Gelen mail host adresi boş bırakılamaz", ref Uyarilar),
                GelenMailPort = Kontrol.TamSayiyaKontrol(txtGelenMailPort, "Gelen mail port nnumarası boş bırakılamaz", "Geçersiz gelen mail port numrası girildi", ref Uyarilar),
                SSL = chkSSL.Checked,
                BCC = string.IsNullOrEmpty(txtBCC.Text) ? string.Empty : Kontrol.ePostaKontrol(txtBCC, "BCC adresi boş bırakılamaz", "Geçersiz BCC adresi girildi", ref Uyarilar),
                ReplyTo = string.IsNullOrEmpty(txtReplyTo.Text) ? string.Empty : Kontrol.ePostaKontrol(txtReplyTo, "Cevaplama adresi boş bırakılamaz", "Geçersiz cevaplama adresi girildi", ref Uyarilar),
                GuncellenmeTarihi = Kontrol.Simdi()
            };


            if (string.IsNullOrEmpty(Uyarilar.ToString()))
            {
                SModel = new MailAyarTablosuIslemler().KayitGuncelle(MGModel);

                if (SModel.Sonuc.Equals(Sonuclar.Basarili))
                    BilgiKontrolMerkezi.UyariEkrani(this, $"UyariBilgilendirme('', '<p>Mail ayarları güncellendi</p>', true);", false);
                else
                    BilgiKontrolMerkezi.UyariEkrani(this, $"UyariBilgilendirme('', '<p>Mail ayarları güncellenirken hata meydana geldi.</p><p>Hata mesajı : {SModel.HataBilgi.HataMesaji}</p>', false);", false);
            }
            else
            {
                BilgiKontrolMerkezi.UyariEkrani(this, $"UyariBilgilendirme('', '{Uyarilar}', false);", false);
            }
        }
    }
}