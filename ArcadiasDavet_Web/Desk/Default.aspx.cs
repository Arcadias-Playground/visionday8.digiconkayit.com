using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using VeritabaniIslemMerkezi;


namespace ArcadiasDavet_Web.Desk
{
    public partial class Default : Page
    {
        const string DataTableID = "tbl_KatilimciListesi";

        StringBuilder Uyarilar = new StringBuilder();
        BilgiKontrolMerkezi Kontrol = new BilgiKontrolMerkezi();

        SurecBilgiModel SModel;
        SurecVeriModel<KatilimciTablosuModel> SDataModel;

        KatilimciTablosuModel KModel;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ddlKatilimciTipi.DataBind();
                ddlKatilimciTipi.Items.Insert(0, new ListItem { Text = "Seçiniz", Value = string.Empty });
            }
        }

        protected void lnkbtnAra_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtAranacakKelime.Text))
            {
                BilgiKontrolMerkezi.UyariEkrani(this, $"UyariBilgilendirme('Dikkat', '<p>Aranacak kelime boş bırakılamaz</p>', false);", false);
            }
            else
            {
                rptKatilimciListesi.DataBind();
                UPnlKatilimciListesi.Update();

                BilgiKontrolMerkezi.UyariEkrani(this, $"UyariBilgilendirme('', '<p>Arama kriterinize göre katılımcılar listelendi</p>', true); DataTableKurulum({DataTableID}, true);", false);
            }
        }

        protected void rptKatilimciListesi_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "Guncelle":
                    SDataModel = new KatilimciTablosuIslemler().KayitBilgisi(e.CommandArgument.ToString());
                    switch (SDataModel.Sonuc)
                    {
                        case Sonuclar.KismiBasarili:
                        case Sonuclar.Basarisiz:
                            BilgiKontrolMerkezi.UyariEkrani(this, $"UyariBilgilendirme('', '<p>Katılımcı bilgisi alınırken hata meydana geldi</p><p>Hata mesajı : {SDataModel.HataBilgi.HataMesaji}</p>', false);", false);
                            break;

                        default:
                        case Sonuclar.VeriBulunamadi:
                            rptKatilimciListesi.DataBind();
                            UPnlKatilimciListesi.Update();
                            BilgiKontrolMerkezi.UyariEkrani(this, $"UyariBilgilendirme('', '<p>Katılımcı bulunamadı</p>', false); DataTableKurulum({DataTableID}, false);", false);
                            break;


                        case Sonuclar.Basarili:

                            txtKatilimciID.Text = SDataModel.Veriler.KatilimciID;
                            ddlKatilimciTipi.SelectedValue = SDataModel.Veriler.KatilimciTipiID;
                            txtAdSoyad.Text = SDataModel.Veriler.AdSoyad;
                            txtePosta.Text = SDataModel.Veriler.ePosta;
                            txtTelefon.Text = SDataModel.Veriler.Telefon;
                            txtUnvan.Text = SDataModel.Veriler.Unvan;
                            txtKurum.Text = SDataModel.Veriler.Kurum;

                            UPnlKatilimciGuncelleEkle.Update();
                            BilgiKontrolMerkezi.UyariEkrani(this, $"$({UPnlKatilimciGuncelleEkle.ClientID}).modal('show');", false);

                            break;
                    }
                    break;

                case "YoneticiOnay":
                    SModel = new KatilimciTablosuIslemler().YoneticiOnay(e.CommandArgument.ToString(), true);
                    if (SModel.Sonuc.Equals(Sonuclar.Basarili))
                    {
                        rptKatilimciListesi.DataBind();
                        UPnlKatilimciListesi.Update();

                        BilgiKontrolMerkezi.UyariEkrani(this, $"UyariBilgilendirme('', '<p>Yönetici onayı verildi</p>', true); DataTableKurulum({DataTableID}, false);", false);
                    }
                    else
                    {
                        BilgiKontrolMerkezi.UyariEkrani(this, $"UyariBilgilendirme('', '<p>Yönetici onayı sırasında hata meydana geldi</p><p>Hata mesajı : {SModel.HataBilgi.HataMesaji}</p>', false);", false);
                    }
                    break;

                case "YoneticiOnayKaldir":
                    SModel = new KatilimciTablosuIslemler().YoneticiOnay(e.CommandArgument.ToString(), false);
                    if (SModel.Sonuc.Equals(Sonuclar.Basarili))
                    {
                        rptKatilimciListesi.DataBind();
                        UPnlKatilimciListesi.Update();

                        BilgiKontrolMerkezi.UyariEkrani(this, $"UyariBilgilendirme('', '<p>Yönetici onayı kaldırıldı</p>', true); DataTableKurulum({DataTableID}, false);", false);
                    }
                    else
                    {
                        BilgiKontrolMerkezi.UyariEkrani(this, $"UyariBilgilendirme('', '<p>Yönetici onayı kaldırılırken hata meydana geldi</p><p>Hata mesajı : {SModel.HataBilgi.HataMesaji}</p>', false);", false);
                    }
                    break;

                case "KatilimciOnay":
                    SModel = new KatilimciTablosuIslemler().KatilimciOnay(e.CommandArgument.ToString(), true);
                    if (SModel.Sonuc.Equals(Sonuclar.Basarili))
                    {
                        rptKatilimciListesi.DataBind();
                        UPnlKatilimciListesi.Update();

                        BilgiKontrolMerkezi.UyariEkrani(this, $"UyariBilgilendirme('', '<p>Katılımcı onayı verildi</p>', true); DataTableKurulum({DataTableID}, false);", false);
                    }
                    else
                    {
                        BilgiKontrolMerkezi.UyariEkrani(this, $"UyariBilgilendirme('', '<p>Katılımcı onayı sırasında hata meydana geldi</p><p>Hata mesajı : {SModel.HataBilgi.HataMesaji}</p>', false);", false);
                    }
                    break;

                case "KatilimciOnayKaldir":
                    SModel = new KatilimciTablosuIslemler().KatilimciOnay(e.CommandArgument.ToString(), false);
                    if (SModel.Sonuc.Equals(Sonuclar.Basarili))
                    {
                        rptKatilimciListesi.DataBind();
                        UPnlKatilimciListesi.Update();

                        BilgiKontrolMerkezi.UyariEkrani(this, $"UyariBilgilendirme('', '<p>Katılımcı onayı kaldırıldı</p>', true); DataTableKurulum({DataTableID}, false);", false);
                    }
                    else
                    {
                        BilgiKontrolMerkezi.UyariEkrani(this, $"UyariBilgilendirme('', '<p>Katılımcı onayı kaldırılırken hata meydana geldi</p><p>Hata mesajı : {SModel.HataBilgi.HataMesaji}</p>', false);", false);
                    }
                    break;

                case "MisafirEkle":
                    SDataModel = new KatilimciTablosuIslemler().KayitBilgisi(e.CommandArgument.ToString());
                    switch (SDataModel.Sonuc)
                    {
                        case Sonuclar.KismiBasarili:
                        case Sonuclar.Basarisiz:
                            BilgiKontrolMerkezi.UyariEkrani(this, $"UyariBilgilendirme('', '<p>Katılımcı bilgisi alınırken hata meydana geldi</p><p>Hata mesajı : {SDataModel.HataBilgi.HataMesaji}</p>', false);", false);
                            break;

                        default:
                        case Sonuclar.VeriBulunamadi:
                            rptKatilimciListesi.DataBind();
                            UPnlKatilimciListesi.Update();
                            BilgiKontrolMerkezi.UyariEkrani(this, $"UyariBilgilendirme('', '<p>Katılımcı bulunamadı</p>', false); DataTableKurulum({DataTableID}, false);", false);
                            break;


                        case Sonuclar.Basarili:
                            if (new KatilimciTablosuIslemler().MisafirKontrol(SDataModel.Veriler.KatilimciID))
                            {
                                Kontrol.Temizle(txtKatilimciID);
                                ddlKatilimciTipi.SelectedValue = SDataModel.Veriler.KatilimciTipiID;
                                Kontrol.Temizle(txtAdSoyad);
                                txtePosta.Text = SDataModel.Veriler.ePosta;
                                txtTelefon.Text = SDataModel.Veriler.Telefon;
                                txtUnvan.Text = SDataModel.Veriler.Unvan;
                                txtKurum.Text = SDataModel.Veriler.Kurum;
                                txtAnaKatilimciID.Text = SDataModel.Veriler.KatilimciID;
                                txtAnaKatilimci.Text = SDataModel.Veriler.AdSoyad;

                                tr_AnaKatilimciAdSoyad.Visible = true;

                                UPnlKatilimciGuncelleEkle.Update();
                                BilgiKontrolMerkezi.UyariEkrani(this, $"$({UPnlKatilimciGuncelleEkle.ClientID}).modal('show');", false);
                            }
                            else
                            {
                                BilgiKontrolMerkezi.UyariEkrani(this, $"UyariBilgilendirme('', '<p>Katılımcının misafir hakkı dolmuştur.</p>', false);", false);
                            }
                            break;
                    }
                    break;

                case "GirisYap":
                    SModel = new KatilimciGirisTablosuIslemler().YeniKayitEkle(new KatilimciGirisTablosuModel
                    {
                        KatilimciID = e.CommandArgument.ToString(),
                        KullaniciID = User.Identity.Name,
                        EklenmeTarihi = Kontrol.Simdi()
                    });
                    if (SModel.Sonuc.Equals(Sonuclar.Basarili))
                    {
                        BilgiKontrolMerkezi.UyariEkrani(this, $"UyariBilgilendirme('', '<p>Katılımcı girişi kaydedildi</p>', true);", false);
                    }
                    else
                    {
                        BilgiKontrolMerkezi.UyariEkrani(this, $"UyariBilgilendirme('', '<p>Katılımcı girişi kaydedilirken hata meydan geldi</p><p>Hata mesajı : {SModel.HataBilgi.HataMesaji}</p>', false);", false);
                    }
                    break;

                case "MailGonderim":

                    SDataModel = new KatilimciTablosuIslemler().KayitBilgisi(e.CommandArgument.ToString());
                    switch (SDataModel.Sonuc)
                    {
                        case Sonuclar.KismiBasarili:
                        case Sonuclar.Basarisiz:
                            BilgiKontrolMerkezi.UyariEkrani(this, $"UyariBilgilendirme('', '<p>Katılımcı bilgisi alınırken hata meydana geldi</p><p>Hata mesajı : {SDataModel.HataBilgi.HataMesaji}</p>', false);", false);
                            break;

                        default:
                        case Sonuclar.VeriBulunamadi:
                            rptKatilimciListesi.DataBind();
                            UPnlKatilimciListesi.Update();
                            BilgiKontrolMerkezi.UyariEkrani(this, $"UyariBilgilendirme('', '<p>Katılımcı bulunamadı</p>', false); DataTableKurulum({DataTableID}, false);", false);
                            break;

                        case Sonuclar.Basarili:

                            if (SDataModel.Veriler.KatilimciOnay)
                            {
                                SModel = new MailGonderimIslemleri().MailGonderim(new KatilimciTablosuIslemler().KayitBilgisi(e.CommandArgument.ToString(), "email", 2).Veriler);
                                if (SModel.Sonuc.Equals(Sonuclar.Basarili))
                                {
                                    BilgiKontrolMerkezi.UyariEkrani(this, $"UyariBilgilendirme('', '<p>Katılımcıya QR maili gönderildi</p>', true);", false);
                                }
                                else
                                {
                                    BilgiKontrolMerkezi.UyariEkrani(this, $"UyariBilgilendirme('', '<p>Katılımcıya QR maili gönderilirken hata meydana geldi</p><p>Hata mesajı : {SModel.HataBilgi.HataMesaji}</p>', false);", false);
                                }
                            }
                            else
                            {
                                SModel = new MailGonderimIslemleri().MailGonderim(new KatilimciTablosuIslemler().KayitBilgisi(e.CommandArgument.ToString(), "email", 1).Veriler);
                                if (SModel.Sonuc.Equals(Sonuclar.Basarili))
                                {
                                    BilgiKontrolMerkezi.UyariEkrani(this, $"UyariBilgilendirme('', '<p>Katılımcıya davetiye maili gönderildi</p>', true);", false);
                                }
                                else
                                {
                                    BilgiKontrolMerkezi.UyariEkrani(this, $"UyariBilgilendirme('', '<p>Katılımcıya davetiye maili gönderilirken hata meydana geldi</p><p>Hata mesajı : {SModel.HataBilgi.HataMesaji}</p>', false);", false);
                                }
                            }

                            break;
                    }

                    break;

                case "SmsGonderim":
                    SDataModel = new KatilimciTablosuIslemler().KayitBilgisi(e.CommandArgument.ToString());
                    switch (SDataModel.Sonuc)
                    {
                        case Sonuclar.KismiBasarili:
                        case Sonuclar.Basarisiz:
                            BilgiKontrolMerkezi.UyariEkrani(this, $"UyariBilgilendirme('', '<p>Katılımcı bilgisi alınırken hata meydana geldi</p><p>Hata mesajı : {SDataModel.HataBilgi.HataMesaji}</p>', false);", false);
                            break;

                        default:
                        case Sonuclar.VeriBulunamadi:
                            rptKatilimciListesi.DataBind();
                            UPnlKatilimciListesi.Update();
                            BilgiKontrolMerkezi.UyariEkrani(this, $"UyariBilgilendirme('', '<p>Katılımcı bulunamadı</p>', false); DataTableKurulum({DataTableID}, false);", false);
                            break;

                        case Sonuclar.Basarili:

                            if (SDataModel.Veriler.KatilimciOnay)
                            {
                                SModel = new SmsGonderimIslemleri().SmsGonderim(new KatilimciTablosuIslemler().KayitBilgisi(e.CommandArgument.ToString(), "sms", 2).Veriler);
                                if (SModel.Sonuc.Equals(Sonuclar.Basarili))
                                {
                                    BilgiKontrolMerkezi.UyariEkrani(this, $"UyariBilgilendirme('', '<p>Katılımcıya QR mesajı gönderildi</p>', true);", false);
                                }
                                else
                                {
                                    BilgiKontrolMerkezi.UyariEkrani(this, $"UyariBilgilendirme('', '<p>Katılımcıya QR mesajı gönderilirken hata meydana geldi</p><p>Hata mesajı : {SModel.HataBilgi.HataMesaji}</p>', false);", false);
                                }
                            }
                            else
                            {
                                SModel = new SmsGonderimIslemleri().SmsGonderim(new KatilimciTablosuIslemler().KayitBilgisi(e.CommandArgument.ToString(), "sms", 1).Veriler);
                                if (SModel.Sonuc.Equals(Sonuclar.Basarili))
                                {
                                    BilgiKontrolMerkezi.UyariEkrani(this, $"UyariBilgilendirme('', '<p>Katılımcıya davetie mesajı gönderildi</p>', true);", false);
                                }
                                else
                                {
                                    BilgiKontrolMerkezi.UyariEkrani(this, $"UyariBilgilendirme('', '<p>Katılımcıya davetie mesajı gönderilirken hata meydana geldi</p><p>Hata mesajı : {SModel.HataBilgi.HataMesaji}</p>', false);", false);
                                }
                            }

                            break;
                    }
                    break;

                case "YakaKarti":
                    SDataModel = new KatilimciTablosuIslemler().KayitBilgisi(e.CommandArgument.ToString());
                    switch (SDataModel.Sonuc)
                    {
                        case Sonuclar.KismiBasarili:
                        case Sonuclar.Basarisiz:
                            BilgiKontrolMerkezi.UyariEkrani(this, $"UyariBilgilendirme('', '<p>Katılımcı bilgisi alınırken hata meydana geldi</p><p>Hata mesajı : {SDataModel.HataBilgi.HataMesaji}</p>', false);", false);
                            break;

                        default:
                        case Sonuclar.VeriBulunamadi:
                            rptKatilimciListesi.DataBind();
                            UPnlKatilimciListesi.Update();
                            BilgiKontrolMerkezi.UyariEkrani(this, $"UyariBilgilendirme('', '<p>Katılımcı bulunamadı</p>', false); DataTableKurulum({DataTableID}, false);", false);
                            break;

                        case Sonuclar.Basarili:

                            if (SDataModel.Veriler.YoneticiOnay)
                            {
                                if (SDataModel.Veriler.KatilimciOnay)
                                {
                                    BilgiKontrolMerkezi.UyariEkrani(this, $"UyariBilgilendirme('', '<p><b>{SDataModel.Veriler.AdSoyad}</b>, yaka kartı basılıyor.</p>', true); window.open('{ResolveClientUrl($"~/Yazici/YakaKarti/{SDataModel.Veriler.KatilimciID}/{new KullaniciTablosuIslemler().KayitBilgisi(Context).KullaniciID}")}', '_blank', 'width = 10, height = 10, top = 2000, left = 2000');", false);
                                }
                                else
                                {
                                    BilgiKontrolMerkezi.UyariEkrani(this, $"UyariBilgilendirme('', '<p>Katılımcı onayı olmadığından yaka kartı basılamaz.</p>', false); DataTableKurulum({DataTableID}, false);", false);
                                }
                            }
                            else
                            {
                                BilgiKontrolMerkezi.UyariEkrani(this, $"UyariBilgilendirme('', '<p>Yönetici onayı olmadığından yaka kartı basılamaz.</p>', false); DataTableKurulum({DataTableID}, false);", false);
                            }

                            break;
                    }

                    break;

                case "UzakBaglantiYakaKarti":
                    List<YaziciDurumModel> YaziciListesi = new List<YaziciDurumModel>();

                    foreach (string Item in System.IO.Directory.GetFiles(Server.MapPath("~/Dosyalar/YaziciDurum"), "*.status").ToList())
                    {
                        YaziciListesi.Add(new YaziciDurumModel
                        {
                            ePosta = Item.Substring(Item.LastIndexOf("\\") + 1).Replace(".status", string.Empty),
                            Durum = Convert.ToBoolean(System.IO.File.ReadAllText(Item))
                        });
                    }


                    rptYaziciListesi.DataSource = YaziciListesi;
                    rptYaziciListesi.DataBind();
                    hfKatilimciID.Value = e.CommandArgument.ToString();

                    UPnlYakaKartiBasimi.Update();
                    BilgiKontrolMerkezi.UyariEkrani(this, $"$({UPnlYakaKartiBasimi.ClientID}).modal('show');", false);

                    break;

                default:
                    break;
            }
        }

        protected void lnkbtnYeniKatilimciEkle_Click(object sender, EventArgs e)
        {
            Kontrol.Temizle(txtKatilimciID);
            Kontrol.Temizle(ddlKatilimciTipi);
            Kontrol.Temizle(txtAdSoyad);
            Kontrol.Temizle(txtePosta);
            Kontrol.Temizle(txtTelefon);
            Kontrol.Temizle(txtKurum);
            Kontrol.Temizle(txtUnvan);
            Kontrol.Temizle(txtAnaKatilimciID);
            Kontrol.Temizle(txtAnaKatilimci);

            tr_AnaKatilimciAdSoyad.Visible = false;

            UPnlKatilimciGuncelleEkle.Update();
            BilgiKontrolMerkezi.UyariEkrani(this, $"$({UPnlKatilimciGuncelleEkle.ClientID}).modal('show');", false);
        }

        protected void lnkbtnKatilimciEkleGuncelle_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtKatilimciID.Text))
            {
                KModel = new KatilimciTablosuModel
                {
                    KatilimciID = new KatilimciTablosuIslemler().YeniKatilimciID(),
                    KatilimciTipiID = Kontrol.KelimeKontrol(ddlKatilimciTipi, "Katılımcı tipini seçiniz", ref Uyarilar),
                    AdSoyad = Kontrol.KelimeKontrol(txtAdSoyad, "Ad & Soyad boş bırakılamaz", ref Uyarilar),
                    ePosta = string.IsNullOrEmpty(txtePosta.Text) ? string.Empty : Kontrol.ePostaKontrol(txtePosta, "e-Posta boş bırakılamaz", "Geçersiz e-Posta adresi girildi", ref Uyarilar),
                    Telefon = txtTelefon.Text,
                    Unvan = txtUnvan.Text,
                    Kurum = txtKurum.Text,
                    YoneticiOnay = true,
                    YoneticiOnayTarihi = Kontrol.Simdi(),
                    KatilimciOnay = !string.IsNullOrEmpty(txtAnaKatilimci.Text),
                    KatilimciOnayTarihi = string.IsNullOrEmpty(txtAnaKatilimci.Text) ? null : new DateTime?(Kontrol.Simdi()),
                    AnaKatilimciID = string.IsNullOrEmpty(txtAnaKatilimci.Text) ? null : txtAnaKatilimciID.Text,
                    GuncellenmeTarihi = Kontrol.Simdi(),
                    EklenmeTarihi = Kontrol.Simdi()
                };

                if (string.IsNullOrEmpty(Uyarilar.ToString()))
                {
                    if (new KatilimciTablosuIslemler().KontenjanKontrol(KModel.KatilimciTipiID))
                    {
                        SModel = new KatilimciTablosuIslemler().YeniKayitEkle(KModel);

                        if (SModel.Sonuc.Equals(Sonuclar.Basarili))
                        {
                            txtAranacakKelime.Text = KModel.AdSoyad;
                            rptKatilimciListesi.DataBind();
                            UPnlKatilimciListesi.Update();

                            Kontrol.Temizle(txtKatilimciID);
                            Kontrol.Temizle(ddlKatilimciTipi);
                            Kontrol.Temizle(txtAdSoyad);
                            Kontrol.Temizle(txtePosta);
                            Kontrol.Temizle(txtTelefon);
                            Kontrol.Temizle(txtKurum);
                            Kontrol.Temizle(txtUnvan);
                            Kontrol.Temizle(txtAnaKatilimciID);
                            Kontrol.Temizle(txtAnaKatilimci);

                            tr_AnaKatilimciAdSoyad.Visible = false;

                            UPnlKatilimciGuncelleEkle.Update();
                            BilgiKontrolMerkezi.UyariEkrani(this, $"$({UPnlKatilimciGuncelleEkle.ClientID}).modal('hide'); UyariBilgilendirme('', '<p>Katılımcı eklendi</p>', true); DataTableKurulum({DataTableID}, true);", false);
                        }
                        else
                        {
                            BilgiKontrolMerkezi.UyariEkrani(this, $"AltSayfaUyariBilgilendirme('<p>Katılımcı eklenirken hata meydana geldi</p><p>Hata mesajı : {SModel.HataBilgi.HataMesaji}</p>', false);", false);
                        }
                    }
                    else
                    {
                        BilgiKontrolMerkezi.UyariEkrani(this, $"AltSayfaUyariBilgilendirme('<p>Katılımcı tipi kontenjanı dolmuştur</p>', false);", false);
                    }
                }
                else
                {
                    BilgiKontrolMerkezi.UyariEkrani(this, $"AltSayfaUyariBilgilendirme('{Uyarilar}', false);", false);
                }
            }
            else
            {
                KModel = new KatilimciTablosuModel
                {
                    KatilimciID = txtKatilimciID.Text,
                    KatilimciTipiID = Kontrol.KelimeKontrol(ddlKatilimciTipi, "Katılımcı tipini seçiniz", ref Uyarilar),
                    AdSoyad = Kontrol.KelimeKontrol(txtAdSoyad, "Ad & Soyad boş bırakılamaz", ref Uyarilar),
                    ePosta = string.IsNullOrEmpty(txtePosta.Text) ? string.Empty : Kontrol.ePostaKontrol(txtePosta, "e-Posta boş bırakılamaz", "Geçersiz e-Posta adresi girildi", ref Uyarilar),
                    Telefon = txtTelefon.Text,
                    Unvan = txtUnvan.Text,
                    Kurum = txtKurum.Text,
                    YoneticiOnay = false,
                    YoneticiOnayTarihi = null,
                    KatilimciOnay = false,
                    KatilimciOnayTarihi = null,
                    AnaKatilimciID = null,
                    GuncellenmeTarihi = Kontrol.Simdi(),
                    EklenmeTarihi = Kontrol.Simdi()
                };

                if (string.IsNullOrEmpty(Uyarilar.ToString()))
                {
                    SModel = new KatilimciTablosuIslemler().KayitGuncelle(KModel);

                    if (SModel.Sonuc.Equals(Sonuclar.Basarili))
                    {
                        //Kontrol.Temizle(txtAranacakKelime);
                        rptKatilimciListesi.DataBind();
                        UPnlKatilimciListesi.Update();

                        Kontrol.Temizle(txtKatilimciID);
                        Kontrol.Temizle(ddlKatilimciTipi);
                        Kontrol.Temizle(txtAdSoyad);
                        Kontrol.Temizle(txtePosta);
                        Kontrol.Temizle(txtAnaKatilimciID);
                        Kontrol.Temizle(txtAnaKatilimci);

                        tr_AnaKatilimciAdSoyad.Visible = false;

                        UPnlKatilimciGuncelleEkle.Update();
                        BilgiKontrolMerkezi.UyariEkrani(this, $"$({UPnlKatilimciGuncelleEkle.ClientID}).modal('hide'); UyariBilgilendirme('', '<p>Katılımcı bilgileri güncellendi</p>', true); DataTableKurulum({DataTableID}, true);", false);
                    }
                    else
                    {
                        BilgiKontrolMerkezi.UyariEkrani(this, $"AltSayfaUyariBilgilendirme('<p>Katılımcı bilgileri güncellenirken hata meydana geldi</p><p>Hata mesajı : {SModel.HataBilgi.HataMesaji}</p>', false);", false);
                    }
                }
                else
                {
                    BilgiKontrolMerkezi.UyariEkrani(this, $"AltSayfaUyariBilgilendirme('{Uyarilar}', false);", false);
                }
            }
        }
    }
}