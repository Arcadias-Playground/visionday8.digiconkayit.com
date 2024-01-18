using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using VeritabaniIslemMerkezi;

namespace ArcadiasDavet_Web.Admin.Rapor
{
    public partial class Katilimci : Page
    {
        SurecVeriModel<IList<KatilimciTablosuModel>> SDataListModel;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                SDataListModel = new KatilimciTablosuIslemler().KayitBilgileri();

                if (SDataListModel.Sonuc.Equals(Sonuclar.Basarili))
                {

                    spn_ToplamKatilimci.InnerText = SDataListModel.Veriler.Count.ToString();
                    spn_TumOnayliKatilimci.InnerText = SDataListModel.Veriler.Count(x => x.YoneticiOnay && x.YoneticiOnayTarihi.HasValue && x.KatilimciOnay && x.KatilimciOnayTarihi.HasValue).ToString();

                    spn_AnaKatilimci.InnerText = SDataListModel.Veriler.Count(x => string.IsNullOrEmpty(x.AnaKatilimciID)).ToString();
                    spn_OnayliKatilimci.InnerText = SDataListModel.Veriler.Count(x => string.IsNullOrEmpty(x.AnaKatilimciID) && x.YoneticiOnay && x.YoneticiOnayTarihi.HasValue && x.KatilimciOnayTarihi.HasValue && x.KatilimciOnay).ToString();
                    spn_RedKatilimci.InnerText = SDataListModel.Veriler.Count(x => string.IsNullOrEmpty(x.AnaKatilimciID) && x.KatilimciOnayTarihi.HasValue && !x.KatilimciOnay).ToString();
                    spn_CevapBekleyenKatilimci.InnerText = SDataListModel.Veriler.Count(x => string.IsNullOrEmpty(x.AnaKatilimciID) && x.YoneticiOnay && x.YoneticiOnayTarihi.HasValue && !x.KatilimciOnayTarihi.HasValue).ToString();
                    spn_MisafirKatilimci.InnerText = SDataListModel.Veriler.Count(x => !string.IsNullOrEmpty(x.AnaKatilimciID) && x.YoneticiOnay && x.YoneticiOnayTarihi.HasValue && x.KatilimciOnay && x.KatilimciOnayTarihi.HasValue).ToString();
                }
                else
                {
                    spn_ToplamKatilimci.InnerText = 0.ToString();
                    spn_AnaKatilimci.InnerText = spn_ToplamKatilimci.InnerText;
                    spn_OnayliKatilimci.InnerText = spn_ToplamKatilimci.InnerText;
                    spn_RedKatilimci.InnerText = spn_ToplamKatilimci.InnerText;
                    spn_CevapBekleyenKatilimci.InnerText = spn_ToplamKatilimci.InnerText;
                    spn_MisafirKatilimci.InnerText = spn_ToplamKatilimci.InnerText;

                    BilgiKontrolMerkezi.UyariEkrani(this, $"UyariBilgilendirme('', '<p>Katılımcı listesi alınırken hata meydana geldi</p><p>Hata mesajı : {SDataListModel.HataBilgi.HataMesaji}</p>', false);", true, true);
                }
            }
        }
    }
}