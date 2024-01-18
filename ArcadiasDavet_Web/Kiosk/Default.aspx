<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="ArcadiasDavet_Web.Kiosk.Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>

    <link rel="icon" href="<%=ResolveClientUrl("~/Gorseller/favicon.ico") %>" type="image/x-icon" />

    <link rel="stylesheet" href="<%=ResolveClientUrl("~/css/bootstrap.min.css") %>" />
    <link rel="stylesheet" href="<%=ResolveClientUrl("~/css/style.min.css") %>" />
    <link rel="stylesheet" href="<%=ResolveClientUrl("~/css/all.min.css") %>" />
    <link rel="stylesheet" href="<%=ResolveClientUrl($"~/css/AlseinCSS.min.css?t={DateTime.Now.Ticks}") %>" />

    <script type="text/javascript" src='<%=ResolveClientUrl("~/js/libscripts.bundle.js") %>'></script>
    <script type="text/javascript" src='<%=ResolveClientUrl("~/js/vendorscripts.bundle.js") %>'></script>
    <script type="text/javascript" src='<%=ResolveClientUrl("~/js/mainscripts.bundle.js") %>'></script>
    <script type="text/javascript" src='<%=ResolveClientUrl("~/js/bootstrap-notify.min.js") %>'></script>
    <script type="text/javascript" src='<%=ResolveClientUrl($"~/js/AlseinJS.js?t={DateTime.Now.Ticks}") %>'></script>

    <script>
        let Durum = false, keypressQRData = '', timeOutValue;

        window.addEventListener('keypress', (event) => {
            if (!Durum) {
                if (timeOutValue) {
                    clearTimeout(timeOutValue);
                } 

                UyariBilgilendirme('Dikkat', 'İşleminiz yapılıyor. Lütfen Bekleyin');
                Durum = true;
            }
                


            if (event.key === "ENTER" || event.key === "Enter" || event.key.toLowerCase() === "enter") {
                while (keypressQRData.indexOf("=") !== -1 || keypressQRData.indexOf("*") !== -1) {
                    keypressQRData = keypressQRData.replace("=", "-").replace("*", "-");
                }

                $('#hfKatilimciID').val(keypressQRData);
                $('.lnkbtnKatilimciYakaKartiBas')[0].click();
            }
            else {
                keypressQRData += event.key;
            }
        });

        const UyariKapatma = () => {
            if (timeOutValue) {
                clearTimeout(timeOutValue);
            } 

            timeOutValue = setTimeout(() => {
                $('#Uyari').modal('hide');
            }, 5000);
        }
    </script>
</head>
<body style="margin: 0; position: absolute; top: 0; left: 0; width: 100vw; height: 100vh;">
    <asp:Image ID="ImgKiosk" runat="server" ImageUrl="~/Dosyalar/Kiosk/Kiosk.png" Style="position: absolute; top: 0; left: 0; width: 100vw; height: 100vh;" />

    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
        <asp:UpdatePanel ID="UPnl" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <asp:HiddenField ID="hfKatilimciID" runat="server" ClientIDMode="Static" />
                <asp:LinkButton ID="lnkbtnKatilimciYakaKartiBas" runat="server" CssClass="lnkbtnKatilimciYakaKartiBas" OnClick="lnkbtnKatilimciYakaKartiBas_Click"></asp:LinkButton>
            </ContentTemplate>
        </asp:UpdatePanel>
    </form>

    <div id="Uyari" class="modal" tabindex="-1" role="dialog" aria-hidden="true" data-backdrop="static">
        <div class="modal-dialog modal-dialog-centered" role="document">
            <div class="modal-content">
                <div class="modal-header" id="UyariHead">
                    <h5 class="modal-title" id="UyariBaslik"></h5>
                </div>
                <div class="modal-body" id="UyariIcerik"></div>
            </div>
        </div>
    </div>
</body>
</html>
