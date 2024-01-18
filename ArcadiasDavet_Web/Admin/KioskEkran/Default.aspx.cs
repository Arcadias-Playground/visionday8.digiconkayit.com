using System;
using System.IO;
using System.Web.UI;
using VeritabaniIslemMerkezi;

namespace ArcadiasDavet_Web.Admin.KioskEkran
{
    public partial class Default : Page
    {
        BilgiKontrolMerkezi Kontrol = new BilgiKontrolMerkezi();

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void lnkbtnKioskGorselGuncelle_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(hfKioskGorsel.Value))
            {
                try
                {
                    using (MemoryStream ms = new MemoryStream(Convert.FromBase64String(hfKioskGorsel.Value.Substring(hfKioskGorsel.Value.IndexOf(",") + 1))))
                    {
                        using (System.Drawing.Image KioskGorsel = System.Drawing.Image.FromStream(ms))
                        {
                            KioskGorsel.Save(Server.MapPath("~/Dosyalar/Kiosk/Kiosk.png"), System.Drawing.Imaging.ImageFormat.Png);
                        }
                    }

                    BilgiKontrolMerkezi.UyariEkrani(this, $"UyariBilgilendirme('', '<p>Görseliniz güncellenmiştir.</p>', false);", false);
                }
                catch (Exception ex)
                {
                    BilgiKontrolMerkezi.UyariEkrani(this, $"UyariBilgilendirme('', '<p>Görseliniz güncellenirken hata meydana geldi</p><p>Hata mesajı : {ex.Message.Replace("'", string.Empty).Replace("\r", string.Empty).Replace("\n", string.Empty).Replace("\t", string.Empty)}</p>', false);", false);
                }
            }
            else
            {
                BilgiKontrolMerkezi.UyariEkrani(this, $"UyariBilgilendirme('', '<p>Yüklemek istediğiniz görselinizi seçiniz</p>', false);", false);
            }


            Kontrol.Temizle(hfKioskGorsel);
        }
    }
}