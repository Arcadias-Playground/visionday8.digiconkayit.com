<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="ArcadiasDavet_Admin.Default" %>

<!DOCTYPE html>

<html class="no-js " lang="en">
<head>
    <meta charset="utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=Edge" />
    <meta content="width=device-width, initial-scale=1, maximum-scale=1, user-scalable=no" name="viewport" />
    <meta name="description" content="Arcadias Tech digiAbstract System" />
    <meta name="author" content="Altay Serhat İnan (Arcadias Tech) [serhat@arcadias.com / +90 506 953 14 36 / +90 507 574 16 40]" />

    <title>Arcadias Tech digiAbstract Admin Panel</title>

    <link rel="icon" href='<%=ResolveClientUrl("~/Gorseller/favicon.ico") %>' type="image/x-icon"/>
    <link rel="stylesheet" href='<%=ResolveClientUrl("~/css/bootstrap.min.css") %>' />
    <link rel="stylesheet" href='<%=ResolveClientUrl("~/css/style.min.css") %>' />

    <script type="text/javascript" src='<%=ResolveClientUrl("~/js/libscripts.bundle.js") %>'></script>
    <script type="text/javascript" src='<%=ResolveClientUrl("~/js/vendorscripts.bundle.js") %>'></script>
    <script type="text/javascript" src='<%=ResolveClientUrl("~/js/AlseinJS.js") %>'></script>
</head>

<body class="theme-blush">
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
        <div class="authentication">
            <div class="container">
                <div class="row">
                    <div class="col-lg-4 col-sm-12">
                        <div class="card auth_form">
                            <div class="header">
                                <asp:Image ID="ImgLogo" runat="server" ImageUrl="~/Gorseller/logo.svg"/>
                                <h5>Giriş</h5>
                            </div>
                            <div class="body">
                                <asp:LoginView ID="LVGiris" runat="server">
                                    <AnonymousTemplate>
                                        <asp:Login ID="LGGiris" runat="server" Style="width: 100%" OnLoggedIn="LGGiris_LoggedIn" OnLoginError="LGGiris_LoginError">
                                            <LayoutTemplate>
                                                <div class="input-group mb-3">
                                                    <asp:TextBox ID="UserName" CssClass="form-control" runat="server" placeholder="Kullanıcı Adını"></asp:TextBox>
                                                    <div class="input-group-append">
                                                        <span class="input-group-text"><i class="zmdi zmdi-account-circle"></i></span>
                                                    </div>
                                                </div>
                                                <div class="input-group mb-3">
                                                    <asp:TextBox ID="Password" TextMode="Password" CssClass="form-control" runat="server" placeholder="Şifreniz"></asp:TextBox>
                                                    <div class="input-group-append">
                                                        <span class="input-group-text"><i class="zmdi zmdi-lock"></i></span>
                                                    </div>
                                                </div>
                                                <asp:UpdatePanel ID="UPnlGiris" runat="server" class="text-center">
                                                    <ContentTemplate>
                                                        <asp:LinkButton ID="lnkbtnGiris" CommandName="Login" runat="server" CssClass="btn btn-primary btn-block waves-effect waves-light" OnClientClick="$(this).css('display', 'none'); $(Yukleniyor).css('display', 'inline-block');">
                                                    <i class="fa fa-check"></i>&nbsp;Giriş Yap
                                                    </asp:LinkButton>
                                                    <asp:Image ID="Yukleniyor" runat="server" Style="display: none; width: 47px;" ClientIDMode="Static" ImageUrl="~/Gorseller/loadspinner.gif" />
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </LayoutTemplate>
                                        </asp:Login>
                                    </AnonymousTemplate>
                                </asp:LoginView>
                            </div>
                        </div>
                        <div class="copyright text-center">
                            &copy;
                            <span id="span_Year"></span>
                            <script>$('#span_Year').html(new Date().getFullYear())</script>, <span><a href="http://www.arcadiastech.com/" target="_blank">Arcadias Tech</a></span>
                        </div>
                    </div>
                    <div class="col-lg-8 col-sm-12">
                        <div class="card">
                            <asp:Image ID="ImgSignIn" runat="server" ImageUrl="~/Gorseller/signin.svg"/>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <div id="Uyari" class="modal" tabindex="-1" role="dialog" aria-hidden="true" data-backdrop="static">
            <div class="modal-dialog" role="document">
                <div class="modal-content">
                    <div class="modal-header" id="UyariHead">
                        <h5 class="modal-title" id="UyariBaslik"></h5>
                    </div>
                    <div class="modal-body" id="UyariIcerik">
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-danger" data-dismiss="modal"><i class="zmdi zmdi-close"></i>&nbsp;Kapat</button>
                    </div>
                </div>
            </div>
        </div>
    </form>

</body>
</html>