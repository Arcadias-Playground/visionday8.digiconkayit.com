using System;
using System.Web.UI;
namespace ArcadiasDavet_Admin
{
    public partial class Temel : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
                System.Diagnostics.FileVersionInfo fvi = System.Diagnostics.FileVersionInfo.GetVersionInfo(assembly.Location);
                h4_VersionNumber.InnerHtml = Page.User.Identity.Name;

                sml_User.InnerHtml = $"v{fvi.FileVersion}";
            }
        }
    }
}