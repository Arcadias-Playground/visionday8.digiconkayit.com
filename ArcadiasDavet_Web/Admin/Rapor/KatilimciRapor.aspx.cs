using ClosedXML.Excel;
using Model;
using System;
using System.Data;
using System.Web.UI;
using VeritabaniIslemMerkezi;

namespace ArcadiasDavet_Web.Admin.Rapor
{
    public partial class KatilimciRapor : Page
    {
        SurecVeriModel<DataTable> SDataModel;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                SDataModel = new KatilimciTablosuIslemler().KatilimciRaporu();

                if (SDataModel.Sonuc.Equals(Sonuclar.Basarili))
                {
                    using (XLWorkbook xlWorkbook = new XLWorkbook())
                    {
                        IXLWorksheet xlWorksheet = xlWorkbook.Worksheets.Add(SDataModel.Veriler);
                        xlWorksheet.SheetView.FreezeRows(1);
                        xlWorksheet.SheetView.FreezeColumns(1);

                        xlWorkbook.SaveAs(Response.OutputStream);
                        Response.AddHeader("Content-Disposition", "attachment; filename=Katılımcı Listesi.xlsx");
                        Response.Flush();
                        Response.End();
                    }
                }
            }
        }
    }
}