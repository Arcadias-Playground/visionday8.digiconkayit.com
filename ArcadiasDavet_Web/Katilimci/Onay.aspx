<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Onay.aspx.cs" Inherits="ArcadiasDavet_Web.Katilimci.Onay" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml" style='background-size: 100% 100%; min-height: 100vh;'>
<head runat="server">



    <meta charset="utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=Edge" />
    <meta content="width=device-width, initial-scale=1, maximum-scale=1, user-scalable=no" name="viewport" />
    <meta name="description" content="Arcadias Tech digiAbstract System" />
    <meta name="author" content="Altay Serhat İnan (Arcadias Tech) [serhat@arcadiastech.com / +90 506 953 14 36 / +90 507 574 16 40]" />

    <title></title>

    <link rel="icon" href="<%=ResolveClientUrl("~/Gorseller/favicon.ico") %>" type="image/x-icon" />
    <link rel="stylesheet" href="<%=ResolveClientUrl("~/css/bootstrap.min.css") %>" />
    <link rel="stylesheet" href="<%=ResolveClientUrl("~/css/style.min.css") %>" />

    <script type="text/javascript" src='<%=ResolveClientUrl("~/js/libscripts.bundle.js") %>'></script>
    <script type="text/javascript" src='<%=ResolveClientUrl("~/js/vendorscripts.bundle.js") %>'></script>
    <script type="text/javascript" src='<%=ResolveClientUrl($"~/js/AlseinJS.js?t={DateTime.Now.Ticks}") %>'></script>
</head>
<body style="background-color: transparent;">
    <div class="container">
        <div class="row">
            <div class="col-md-12">
                <div class="text-center">
                    <p>
                        <asp:Image ID="ImgLogo" runat="server" ImageUrl="~/Gorseller/logo.png" Style="width: 70%; max-width: 450px;" />
                    </p>
                    <p style="font-weight: bold">
                        <asp:Label ID="lblAdSoyad" runat="server" Text="...."></asp:Label>
                    </p>
                    <asp:Panel ID="PnlAnaKatilimci" runat="server" Visible="false">
                        <p style="font-size: 10px;">
                            <b style="color: red"><u>Ana Katılımcı</u></b><br />
                            <b>
                                <asp:Label ID="lblAnaKatilimci" runat="server" Style="font-size: 12px" Text="..."></asp:Label></b>
                        </p>
                    </asp:Panel>
                    <p>
                        <asp:Image ID="ImgQR" runat="server" Style="width: 30%;" />
                    </p>
                    <asp:Panel ID="PnlMisafirKayit" runat="server" Visible="false">
                        <p>
                            <asp:HyperLink ID="hyplnkMisafir" runat="server">Misafir kaydını oluşturmak için tıklayınız.</asp:HyperLink>
                        </p>
                    </asp:Panel>
                </div>
                <div id="div_Icerik" runat="server" class="mb-5" style="background: rgba(255,255,255, 0.5); border: 1px solid black; border-radius: 10px; padding: 10px;"></div>
            </div>
        </div>
    </div>

    <form id="form1" runat="server"></form>

    <div id="Uyari" class="modal" tabindex="-1" role="dialog" aria-hidden="true" data-backdrop="static">
        <div class="modal-dialog modal-dialog-centered" role="document">
            <div class="modal-content">
                <div class="modal-header" id="UyariHead">
                    <h5 class="modal-title" id="UyariBaslik"></h5>
                </div>
                <div class="modal-body" id="UyariIcerik">
                </div>
                <div class="modal-footer">
                </div>
            </div>
        </div>
    </div>
</body>
</html>
