using Microsoft.AspNet.FriendlyUrls;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.UI;

namespace ArcadiasDavet_Web.Admin.MailIslemleri
{
    public partial class MailLog : Page
    {
        IList<string> segment;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                segment = Request.GetFriendlyUrlSegments();

                if (segment.Count.Equals(1) && Guid.TryParse(segment.First(), out Guid MailGonderimID))
                {
                    if (File.Exists(Server.MapPath($"~/Dosyalar/Maillog/{MailGonderimID}.maillog")))
                    {
                        Response.Write(File.ReadAllText(Server.MapPath($"~/Dosyalar/Maillog/{MailGonderimID}.maillog")));
                        Response.Flush();
                        Response.End();
                    }
                }
            }
        }
    }
}