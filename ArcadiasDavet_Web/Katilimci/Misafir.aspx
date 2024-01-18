<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Misafir.aspx.cs" Inherits="ArcadiasDavet_Web.Katilimci.Misafir" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml" style='background-size:100% 100%; min-height:100vh;'>
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
    <link rel="stylesheet" href="<%=ResolveClientUrl("~/css/all.min.css") %>" />
    <link rel="stylesheet" href="<%=ResolveClientUrl($"~/css/AlseinCSS.min.css?t={DateTime.Now.Ticks}") %>" />

    <script type="text/javascript" src='<%=ResolveClientUrl("~/js/libscripts.bundle.js") %>'></script>
    <script type="text/javascript" src='<%=ResolveClientUrl("~/js/vendorscripts.bundle.js") %>'></script>
    <script type="text/javascript" src='<%=ResolveClientUrl($"~/js/AlseinJS.js?t={DateTime.Now.Ticks}") %>'></script>
</head>
<body style="background: none !important;">
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
        <div class="container">
            <div class="row">
                <asp:UpdatePanel ID="UPnlGenel" runat="server" UpdateMode="Conditional" class="col-md-12">
                    <ContentTemplate>
                        <div class="text-center">
                            <p>
                                <asp:Image ID="ImgLogo" runat="server" ImageUrl="~/Gorseller/logo.png" Style="width: 50%; max-width: 450px;" />
                            </p>
                            <p style="font-weight: bold">
                                Sayın<br />
                                <asp:Label ID="lblAdSoyad" runat="server"></asp:Label>
                            </p>
                        </div>
                        <div class="mb-5" style="background: rgba(255,255,255, 0.5); border: 1px solid black; border-radius: 10px; padding: 10px; text-align: justify; font-weight: bold">
                            <h5 class="text-center">Misafir Bilgileri</h5>
                            <table class="AlseinTable">
                                <tr id="tr_KatilimciID" runat="server" visible="false">
                                    <td>*</td>
                                    <td>Katılımcı ID</td>
                                    <td>
                                        <asp:TextBox ID="txtKatilimciID" runat="server" CssClass="form-control" Enabled="false" ReadOnly="true"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr id="tr_KatilimciTipiID" runat="server" visible="false">
                                    <td>*</td>
                                    <td>Katılımcı Tipi</td>
                                    <td>
                                        <asp:DropDownList ID="ddlKatilimciTipi" runat="server" CssClass="form-control" DataSourceID="OleDbKatilimciTipiListesi" DataTextField="KatilimciTipi" DataValueField="KatilimciTipiID"></asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>*</td>
                                    <td style="width:300px;">Misafir Ad & Soyad</td>
                                    <td>
                                        <asp:TextBox ID="txtAdSoyad" runat="server" CssClass="form-control" style="width:fit-content; min-width:100%"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>&nbsp;</td>
                                    <td>e-Mail</td>
                                    <td>
                                        <asp:TextBox ID="txtePosta" runat="server" CssClass="form-control" style="width:fit-content; min-width:100%"></asp:TextBox>
                                    </td>
                                </tr>
                                 <tr>
                                    <td>&nbsp;</td>
                                    <td>Telefon</td>
                                    <td>
                                        <asp:TextBox ID="txtTelefon" runat="server" CssClass="form-control" style="width:fit-content; min-width:100%"></asp:TextBox>
                                    </td>
                                </tr>
                                 <tr>
                                    <td>&nbsp;</td>
                                    <td>Unvan</td>
                                    <td>
                                        <asp:TextBox ID="txtUnvan" runat="server" CssClass="form-control" style="width:fit-content; min-width:100%"></asp:TextBox>
                                    </td>
                                </tr>
                                 <tr>
                                    <td>&nbsp;</td>
                                    <td>Kurum</td>
                                    <td>
                                        <asp:TextBox ID="txtKurum" runat="server" CssClass="form-control" style="width:fit-content; min-width:100%"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                            <p class="text-center">
                                <asp:LinkButton ID="lnkbtnMisafirEkle" runat="server" CssClass="btn btn-success" OnClick="lnkbtnMisafirEkle_Click" OnClientClick="$(this).css('display', 'none'); $(Yukleniyor).css('display', 'inline-block')">
                                        Misafirimi Kaydet<i class="fa fa-check ml-2"></i>
                                </asp:LinkButton>
                                <asp:Image ID="Yukleniyor" runat="server" ImageUrl="~/Gorseller/loadspinner.gif" ClientIDMode="Static" Style="display: none; width: 47px" />
                            </p>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>

        <asp:SqlDataSource runat="server" ID="OleDbKatilimciTipiListesi" ConnectionString='<%$ ConnectionStrings:ArcadiasDavetConnection %>' ProviderName='<%$ ConnectionStrings:ArcadiasDavetConnection.ProviderName %>' SelectCommand="SELECT [KatilimciTipiID], [KatilimciTipi] FROM [KatilimciTipiTablosu]"></asp:SqlDataSource>
    </form>

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

    <%=style %>
</body>
</html>