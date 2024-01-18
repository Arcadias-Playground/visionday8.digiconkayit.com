using Microsoft.AspNet.FriendlyUrls;
using Model;
using QRCoder;
using System;
using System.Collections.Generic;
using System.Web.UI;
using VeritabaniIslemMerkezi;


namespace ArcadiasDavet_Web.Yazici
{
    public partial class YakaKarti : Page
    {
        IList<string> segments;

        SurecBilgiModel SModel;
        SurecVeriModel<KatilimciTablosuModel> SDataModel;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                segments = Request.GetFriendlyUrlSegments();

                if (segments.Count.Equals(2))
                {
                    SDataModel = new KatilimciTablosuIslemler().KayitBilgisi(segments[0], "YakaKarti");

                    if (SDataModel.Sonuc.Equals(Sonuclar.Basarili) && SDataModel.Veriler.YoneticiOnay && SDataModel.Veriler.KatilimciOnay)
                    {
                        SModel = new YakaKartiBasimTablosuIslemler().YeniKayitEkle(new YakaKartiBasimTablosuModel
                        {
                            KatilimciID = SDataModel.Veriler.KatilimciID,
                            KullaniciID = segments[1],
                            EklenmeTarihi = new BilgiKontrolMerkezi().Simdi()
                        });


                        
                        div_Cerceve.Attributes.Add("style", $"position:absolute; top:0px; left:{SDataModel.Veriler.KatilimciTipiBilgisi.YakaKartiCerceveBilgisi.PrinterPaperTabDimension}px; width:{SDataModel.Veriler.KatilimciTipiBilgisi.YakaKartiCerceveBilgisi.WebFrameWidth}px; height: {SDataModel.Veriler.KatilimciTipiBilgisi.YakaKartiCerceveBilgisi.WebFrameHeight}px;");


                        foreach (YakaKartiIcerikTablosuModel Item in SDataModel.Veriler.KatilimciTipiBilgisi.YakaKartiCerceveBilgisi.YakaKartiIcerikBilgisi)
                        {

                            switch (Item.YakaKartiIcerikTipiID)
                            {
                                case 1:
                                    using (QRCodeGenerator qRCodeGenerator = new QRCodeGenerator())
                                    {
                                        using (QRCodeData qRCodeData = qRCodeGenerator.CreateQrCode(SDataModel.Veriler.KatilimciID, QRCodeGenerator.ECCLevel.Q))
                                        {
                                            using (Base64QRCode qrCode = new Base64QRCode(qRCodeData))
                                            {
                                                div_Cerceve.InnerHtml += $"<div style='position: absolute; top:{Item.Y}px; left: {Item.X}px; width: {Item.Width}px; height: {Item.Height}px; border:1px solid black;'><img src=\"data:image/png;base64,{qrCode.GetGraphic(20)}\" style=\"width:100%;\"/></div>";
                                            }
                                        }
                                    }
                                    break;

                                case 2:
                                    div_Cerceve.InnerHtml += $"<div style='position: absolute; top:{Item.Y}px; left: {Item.X}px; width: {Item.Width }px; height: {Item.Height}px; font-size: {Item.Height}px; line-height:{Item.Height}px; text-align: center;'>{SDataModel.Veriler.AdSoyad}</div>";
                                    break;

                                case 3:
                                    div_Cerceve.InnerHtml += $"<div style='position: absolute; top:{Item.Y}px; left: {Item.X}px; width: {Item.Width}px; height: {Item.Height}px; font-size: {Item.Height}px; line-height:{Item.Height}px; text-align: center;'>{SDataModel.Veriler.Unvan}</div>";
                                    break;


                                case 4:
                                    div_Cerceve.InnerHtml += $"<div style='position: absolute; top:{Item.Y}px; left: {Item.X}px; width: {Item.Width}px; height: {Item.Height}px; font-size: {Item.Height}px; line-height:{Item.Height}px; text-align: center;'>{SDataModel.Veriler.Kurum}</div>";
                                    break;

                                default:
                                    break;
                            }
                        }

                        BilgiKontrolMerkezi.UyariEkrani(this, $"window.print();", true, true);
                    }
                    else
                    {
                        BilgiKontrolMerkezi.UyariEkrani(this, $"window.close();", true, true);
                    }
                }
                else
                {
                    BilgiKontrolMerkezi.UyariEkrani(this, $"window.close();", true, true);
                }
            }
        }
    }
}