using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using VeritabaniIslemMerkezi;

namespace ArcadiasDavet_Web.Admin.Rapor
{
    public partial class YakaKarti : System.Web.UI.Page
    {
        SurecVeriModel<IList<YakaKartiBasimTablosuModel>> SDataListModel;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                SDataListModel = new YakaKartiBasimTablosuIslemler().KayitBilgileri();

                spn_ToplamGiris.InnerText = SDataListModel.Veriler.Count.ToString();

                rptKatilimciTipiGirisListesi.DataSource = SDataListModel.Veriler.Where(x => string.IsNullOrEmpty(x.KatilimciBilgisi.AnaKatilimciID)).GroupBy(x => x.KatilimciBilgisi.KatilimciTipiBilgisi.KatilimciTipi).Select(y => new { KatilimciTipi = y.Key, GirisSayisi = y.Count() });
                rptKatilimciTipiGirisListesi.DataBind();

                spn_MisafirGiris.InnerText = SDataListModel.Veriler.Count(x => !string.IsNullOrEmpty(x.KatilimciBilgisi.AnaKatilimciID)).ToString();
            }
        }
    }
}