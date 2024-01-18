using System;
using System.Web.Security;
using System.Web.UI;

namespace ArcadiasDavet_Web.Admin
{
    public partial class Admin : MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                sml_User.InnerText = Page.User.Identity.Name;

                System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
                System.Diagnostics.FileVersionInfo fvi = System.Diagnostics.FileVersionInfo.GetVersionInfo(assembly.Location);
                h4_VersionNumber.InnerText = $"v{fvi.FileVersion}";
            }
        }

        protected void lnkbtnCikis_Click(object sender, EventArgs e)
        {
            FormsAuthentication.SignOut();
            FormsAuthentication.RedirectToLoginPage();
        }
    }
}