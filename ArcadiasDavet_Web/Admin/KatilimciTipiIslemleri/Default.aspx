<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="ArcadiasDavet_Web.Admin.KatilimciTipiIslemleri.Default" %>

<%@ Register Assembly="CKEditor.NET" Namespace="CKEditor.NET" TagPrefix="CKEditor" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Admin_head" runat="server">
    <link rel="stylesheet" href='<%=ResolveClientUrl($"~/css/cropper.min.css") %>' />
    <script type="text/javascript" src='<%=ResolveClientUrl("~/js/cropper.min.js") %>'></script>

    <style>
        .table td {
            vertical-align: middle;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="Admin_Icerik" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <div class="row">
        <div class="col-md-12">
            <div class="card">
                <div class="header">
                    <h2><strong>Katılımcı Tipi</strong> İşlemleri</h2>
                </div>
                <asp:UpdatePanel ID="UPnlKatilimciTipiListesi" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="false" class="body">
                    <ContentTemplate>
                        <p class="text-center">
                            <asp:LinkButton ID="lnkbtnYeniKatilimciTipiEkle" runat="server" CssClass="btn btn-success" OnClick="lnkbtnYeniKatilimciTipiEkle_Click">
                                <i class="fa fa-plus"></i>&nbsp;Yeni Katılımcı Tipi Ekle
                            </asp:LinkButton>
                        </p>

                        <div class="table-responsive border">
                            <table class="table table-striped table-bordered">
                                <thead>
                                    <tr class="text-center" role="row">
                                        <th class="align-center" rowspan="2">İşlemler</th>
                                        <th class="align-center" rowspan="2" style="min-width: 150px;">Katılımcı Tipi</th>
                                        <th class="align-center" rowspan="2">Kontenjan</th>
                                        <th class="align-center" rowspan="2">Misafir Kontenjan</th>
                                        <th class="align-center" rowspan="2" style="min-width: 350px;">İzin Verilen Giriş & Yaka Kartı Basım Sayısı</th>
                                        <th class="align-center" rowspan="2" style="min-width: 230px;">Davetiye İçerik İşlemleri</th>
                                        <th class="align-center" rowspan="2" style="min-width: 450px;">QR İçerik işlemleri</th>
                                        <th colspan="2">Yaka Kartı İşlemleri</th>
                                        <th colspan="4">Karşılama  Ekranı İşlemleri</th>
                                    </tr>
                                    <tr class="text-center">

                                        <th>Görsel ve Ölçü</th>
                                        <th>Görsel İçerik Düzeni</th>
                                        <th>Arka Plan Görseli</th>
                                        <th>Logo</th>
                                        <th>Kabul Ekranı Açıklama İçerik</th>
                                        <th>Red Ekranı Açıklama İçerik</th>
                                    </tr>
                                </thead>
                                <tbody>
                                    <asp:Repeater ID="rptKatilimciTipiListesi" runat="server" ClientIDMode="AutoID" DataSourceID="OleDbKatilimciTipiListesi" OnItemCommand="rptKatilimciTipiListesi_ItemCommand">
                                        <ItemTemplate>
                                            <tr class="text-center">
                                                <td>
                                                    <asp:LinkButton ID="lnkbtnKatilimciTipiGuncelle" runat="server" CssClass="btn btn-success" CommandArgument='<%#Eval("KatilimciTipiID") %>' CommandName="Guncelle">
                                                        <i class="fa fa-sync"></i>
                                                    </asp:LinkButton>
                                                    <asp:HiddenField ID="hfKatilimciTipiID" runat="server" Value='<%#Eval("KatilimciTipiID") %>' Visible="false" />
                                                </td>
                                                <td><%#Eval("KatilimciTipi") %></td>
                                                <td><%#Eval("Kontenjan") %></td>
                                                <td><%#Eval("MisafirKontenjan") %></td>
                                                <td>
                                                    <p>
                                                        Giriş Sayısı : <%#Eval("GirisSayisi") %>
                                                    </p>
                                                    <p>
                                                        Yaka Kartı Basım Sayısı : <%#Eval("YakaKartiBasimSayisi") %>
                                                    </p>
                                                </td>
                                                <td class="text-center ">
                                                    <div class="row">
                                                        <asp:Repeater ID="rptDavetiyeIcerikListesi" runat="server" ClientIDMode="AutoID" DataSourceID="OleDbDavetiyeIslemleriListesi" OnItemCommand="rptDavetiyeIcerikListesi_ItemCommand">
                                                            <ItemTemplate>
                                                                <div class="col-md-11 p-3 mx-auto" style="border:1px solid black; border-radius:5px;">
                                                                    <h6 class="text-center" style="border-bottom: 1px solid #efefef">
                                                                        <%# Convert.ToBoolean(Eval("AnaKatilimci")) ? "Ana Katılımcı" : "Misafir" %>
                                                                    </h6>
                                                                    <div>
                                                                        <asp:LinkButton ID="lnkbtnDavetiyeGorselGuncelle" runat="server" CssClass="btn btn-primary" CommandArgument='<%#Eval("MailIcerikID") %>' CommandName="DavetiyeGorselGuncelle">
                                                        <i class="fa fa-image mr-2"></i>Görsel Güncelle
                                                                        </asp:LinkButton>
                                                                    </div>
                                                                    <div>
                                                                        <asp:LinkButton ID="lnkbtnDavetiyeAntetliKagitIcerikGuncelle" runat="server" CssClass="btn btn-warning" CommandArgument='<%#Eval("MailIcerikID") %>' CommandName="DavetiyeAntetliKagitIcerikGuncelle">
                                                        <i class="fa fa-file-pdf mr-2"></i>Görsel İçeriklerini Düzenle
                                                                        </asp:LinkButton>
                                                                    </div>
                                                                    <div>
                                                                        <asp:LinkButton ID="lnkbtnDavetiyeHtmIcerikGuncelle" runat="server" CssClass="btn btn-success" CommandArgument='<%#Eval("MailIcerikID") %>' CommandName="DavetiyeHtmlIcerikGuncelle">
                                                        <i class="fa fa-envelope mr-2"></i>Mail İçeriğini Güncelle
                                                                        </asp:LinkButton>
                                                                    </div>
                                                                    <div>
                                                                        <asp:LinkButton ID="lnkbtDavetiyeSmsIcerikGuncelle" runat="server" CssClass="btn btn-danger" CommandArgument='<%# $"{Eval("KatilimciTipiID")},{Eval("AnaKatilimci")},1" %>' CommandName="DavetiyeSmsIcerikGuncelle">
                                                        <i class="fa fa-sms mr-2"></i>Sms İçeriğini Güncelle
                                                                        </asp:LinkButton>
                                                                    </div>
                                                                </div>
                                                            </ItemTemplate>
                                                        </asp:Repeater>
                                                        <asp:SqlDataSource runat="server" ID="OleDbDavetiyeIslemleriListesi" ConnectionString='<%$ ConnectionStrings:ArcadiasDavetConnection %>' ProviderName='<%$ ConnectionStrings:ArcadiasDavetConnection.ProviderName %>' SelectCommand="SELECT * FROM [MailIcerikTablosu] WHERE ([KatilimciTipiID] = ?) AND (GonderimTipiID = 1) AND (AnaKatilimci=true)">
                                                            <SelectParameters>
                                                                <asp:ControlParameter ControlID="hfKatilimciTipiID" PropertyName="Value" DefaultValue="0" Name="KatilimciTipiID" Type="String"></asp:ControlParameter>
                                                            </SelectParameters>
                                                        </asp:SqlDataSource>
                                                    </div>
                                                </td>
                                                <td>
                                                    <div class="row">
                                                        <asp:Repeater ID="rptQRIcerikListesi" runat="server" ClientIDMode="AutoID" DataSourceID="OleDbQRIslemleriListesi" OnItemCommand="rptQRIcerikListesi_ItemCommand">
                                                            <ItemTemplate>
                                                                <div class="col-md-5 p-3 mx-auto" style="border:1px solid black; border-radius:5px;">
                                                                    <h6 class="text-center" style="border-bottom: 1px solid #efefef">
                                                                        <%# Convert.ToBoolean(Eval("AnaKatilimci")) ? "Ana Katılımcı" : "Misafir" %>
                                                                    </h6>
                                                                    <div>
                                                                        <asp:LinkButton ID="lnkbtnDavetiyeGorselGuncelle" runat="server" CssClass="btn btn-primary" CommandArgument='<%#Eval("MailIcerikID") %>' CommandName="QRGorselGuncelle">
                                                        <i class="fa fa-image mr-2"></i>Görsel Güncelle
                                                                        </asp:LinkButton>
                                                                    </div>
                                                                    <div>
                                                                        <asp:LinkButton ID="lnkbtnDavetiyeAntetliKagitIcerikGuncelle" runat="server" CssClass="btn btn-warning" CommandArgument='<%#Eval("MailIcerikID") %>' CommandName="QRAntetliKagitIcerikGuncelle">
                                                        <i class="fa fa-file-pdf mr-2"></i>Görsel İçeriklerini Düzenle
                                                                        </asp:LinkButton>
                                                                    </div>
                                                                    <div>
                                                                        <asp:LinkButton ID="lnkbtnDavetiyeHtmIcerikGuncelle" runat="server" CssClass="btn btn-success" CommandArgument='<%#Eval("MailIcerikID") %>' CommandName="QRHtmlIcerikGuncelle">
                                                        <i class="fa fa-envelope mr-2"></i>Mail İçeriğini Güncelle
                                                                        </asp:LinkButton>
                                                                    </div>
                                                                    <div>
                                                                        <asp:LinkButton ID="lnkbtDavetiyeSmsIcerikGuncelle" runat="server" CssClass="btn btn-danger" CommandArgument='<%#$"{Eval("KatilimciTipiID")},{Eval("AnaKatilimci")},2" %>' CommandName="QRSmsIcerikGuncelle">
                                                        <i class="fa fa-sms mr-2"></i>Sms İçeriğini Güncelle
                                                                        </asp:LinkButton>
                                                                    </div>
                                                                </div>
                                                            </ItemTemplate>
                                                        </asp:Repeater>
                                                        <asp:SqlDataSource runat="server" ID="OleDbQRIslemleriListesi" ConnectionString='<%$ ConnectionStrings:ArcadiasDavetConnection %>' ProviderName='<%$ ConnectionStrings:ArcadiasDavetConnection.ProviderName %>' SelectCommand="SELECT * FROM [MailIcerikTablosu] WHERE ([KatilimciTipiID] = ?) AND (GonderimTipiID = 2)">
                                                            <SelectParameters>
                                                                <asp:ControlParameter ControlID="hfKatilimciTipiID" PropertyName="Value" DefaultValue="0" Name="KatilimciTipiID" Type="String"></asp:ControlParameter>
                                                            </SelectParameters>
                                                        </asp:SqlDataSource>
                                                    </div>
                                                </td>
                                                <td>
                                                    <asp:LinkButton ID="lnkbtnYakaKartiCerceveGuncelle" runat="server" CssClass="btn btn-success" CommandArgument='<%#Eval("KatilimciTipiID") %>' CommandName="YakaKartiCerceveGuncelle">
                                                        <i class="fa fa-image"></i>
                                                    </asp:LinkButton>
                                                </td>
                                                <td>
                                                    <asp:LinkButton ID="lnkbtnYakaKartiIcerikGuncelle" runat="server" CssClass="btn btn-danger" CommandArgument='<%#Eval("KatilimciTipiID") %>' CommandName="YakaKartiIcerikGuncelle">
                                                        <i class="fa fa-arrow-down-up-across-line"></i>
                                                    </asp:LinkButton>
                                                </td>
                                                <td>
                                                    <asp:LinkButton ID="lnkbtnKarsilamaEkraniGorselGuncelle" runat="server" CssClass="btn btn-info" CommandArgument='<%#Eval("KatilimciTipiID") %>' CommandName="KarsilamaEkraniGorselGuncelle">
                                                        <i class="fa fa-image"></i>
                                                    </asp:LinkButton>
                                                </td>
                                                <td>
                                                    <asp:LinkButton ID="lnkbtnLogoGuncelle" runat="server" CssClass="btn btn-info" CommandArgument='<%#Eval("KatilimciTipiID") %>' CommandName="LogoGuncelle">
                                                        <i class="fa fa-location"></i>
                                                    </asp:LinkButton>
                                                </td>
                                                <td>
                                                    <asp:LinkButton ID="lnkbtnKabulEkranIcerikGuncelle" runat="server" CssClass="btn btn-success" CommandArgument='<%#Eval("KatilimciTipiID") %>' CommandName="KabulEkranIcerikGuncelle">
                                                        <i class="fa fa-check"></i>
                                                    </asp:LinkButton>
                                                </td>
                                                <td>
                                                    <asp:LinkButton ID="lnkbtnRedEkranIcerikGuncelle" runat="server" CssClass="btn btn-danger" CommandArgument='<%#Eval("KatilimciTipiID") %>' CommandName="RedEkranIcerikGuncelle">
                                                        <i class="fa fa-times"></i>
                                                    </asp:LinkButton>
                                                </td>
                                            </tr>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                </tbody>
                            </table>
                        </div>

                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
    </div>

    <asp:SqlDataSource runat="server" ID="OleDbKatilimciTipiListesi" ConnectionString='<%$ ConnectionStrings:ArcadiasDavetConnection %>' ProviderName='<%$ ConnectionStrings:ArcadiasDavetConnection.ProviderName %>' SelectCommand="SELECT * FROM [KatilimciTipiTablosu] ORDER BY KatilimciTipi"></asp:SqlDataSource>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="Admin_AltSayfa" runat="server">

    <asp:UpdatePanel ID="UPnlKatilimciTipiEkleGuncelle" runat="server" UpdateMode="Conditional" class="modal fade" tabindex="-1" role="dialog" data-backdrop="static">
        <ContentTemplate>
            <div class="modal-dialog modal-dialog-centered modal-lg" role="document">
                <div class="modal-content">
                    <div class="modal-header">
                        <h4 class="title">Katılımcı Tipi Ekle / Güncelle</h4>
                    </div>
                    <div class="modal-body">
                        <table class="AlseinTable">
                            <tr id="tr_KatilimciTipiID" runat="server" visible="false">
                                <td>&nbsp;</td>
                                <td>Katılımcı Tipi ID</td>
                                <td>
                                    <asp:TextBox ID="txtKatilimciTipiID" runat="server" CssClass="form-control" Enabled="false" ReadOnly="true"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>&nbsp;</td>
                                <td style="width: 190px;">Katılımcı Tipi</td>
                                <td>
                                    <asp:TextBox ID="txtKatilimciTipi" runat="server" CssClass="form-control" onchange="toUpper(this);" onkeyup="toUpper(this);"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>&nbsp;</td>
                                <td>Kontenjan</td>
                                <td>
                                    <asp:TextBox ID="txtKontenjan" runat="server" CssClass="form-control"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3" class="text-center">Kontenjan sınırı koymak istemiyorsanız, kontenjanı "0" olarak giriniz.</td>
                            </tr>
                            <tr>
                                <td>&nbsp;</td>
                                <td>Misafir Kontenjan</td>
                                <td>
                                    <asp:TextBox ID="txtMisafirKontenjan" runat="server" CssClass="form-control"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>&nbsp;</td>
                                <td>İzin Verilen Giriş Sayısı</td>
                                <td>
                                    <asp:TextBox ID="txtGirisSayisi" runat="server" CssClass="form-control"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>&nbsp;</td>
                                <td>İzin Verilen Yaka Kartı Basım Sayısı</td>
                                <td>
                                    <asp:TextBox ID="txtYakaKartiBasimSayisi" runat="server" CssClass="form-control"></asp:TextBox>
                                </td>
                            </tr>
                        </table>
                        <p align="center">
                            <asp:LinkButton ID="lnkbtnKatilimciTipiEkleGuncelle" runat="server" CssClass="btn btn-success" OnClientClick="$(this).css('display', 'none'); $(Yukleniyor).css('display', 'inline-block')" OnClick="lnkbtnKatilimciTipiEkleGuncelle_Click">
                                <i class="zmdi zmdi-plus"></i>&nbsp;Ekle / Güncelle&nbsp;<i class="zmdi zmdi-refresh"></i>
                            </asp:LinkButton>
                            <asp:Image ID="Yukleniyor" runat="server" ImageUrl="~/Gorseller/loadspinner.gif" ClientIDMode="Static" Style="display: none; width: 47px" />
                        </p>
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-danger waves-effect" data-dismiss="modal">Kapat</button>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>

    <asp:UpdatePanel ID="UPnlGorselGuncelle" runat="server" UpdateMode="Conditional" class="modal fade" tabindex="-1" role="dialog" data-backdrop="static">
        <ContentTemplate>
            <div class="modal-dialog modal-dialog-centered modal-lg" role="document">
                <div class="modal-content">
                    <div class="modal-header">
                        <h4 class="title" runat="server" id="h4_GorselGuncelle"></h4>
                    </div>
                    <div class="modal-body">
                        <table class="AlseinTable">
                            <tr>
                                <td>&nbsp;</td>
                                <td>Güncel Görsel</td>
                                <td>
                                    <input id="fuGorsel" type="file" class="form-control" accept="image/png, image/jpeg" onchange="GorselKontrol(this);" />
                                    <asp:HiddenField ID="hfGorsel" runat="server" ClientIDMode="Static" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3" class="text-center">Görseliniz png yada jpg uzantılı olmalı, en fazla 3mb ve 2000px x 2828px ölçülerinde olmalıdır.</td>
                            </tr>
                        </table>
                        <p align="center">
                            <asp:LinkButton ID="lnkbtnGorselGuncelle" runat="server" CssClass="btn btn-success" OnClientClick="$(this).css('display', 'none'); $(ImgGorselGuncelle).css('display', 'inline-block')" OnClick="lnkbtnGorselGuncelle_Click">
                                <i class="zmdi zmdi-refresh"></i>&nbsp;Güncelle
                            </asp:LinkButton>
                            <asp:Image ID="ImgGorselGuncelle" runat="server" ImageUrl="~/Gorseller/loadspinner.gif" ClientIDMode="Static" Style="display: none; width: 47px" />
                        </p>
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-danger waves-effect" data-dismiss="modal" onclick="$('#hfGorsel').val('');">Kapat</button>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>

    <asp:UpdatePanel ID="UPnlAntetliKagitIcerikGuncelle" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="false" class="modal fade" tabindex="-1" role="dialog" data-backdrop="static">
        <ContentTemplate>
            <div class="modal-dialog modal-dialog-centered modal-lg" role="document">
                <div class="modal-content">
                    <div class="modal-header">
                        <h4 class="title" runat="server" id="h4_AntetliKagitIcerikGuncelle"></h4>
                    </div>
                    <div class="modal-body">
                        <div class="row m-0 p-3">
                            <div class="col-md-6">
                                <table class="AlseinTable">
                                    <tr>
                                        <td>&nbsp;</td>
                                        <td style="width: 50px;">İçerik</td>
                                        <td>
                                            <asp:DropDownList ID="ddlAntetliKagitIcerik" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlAntetliKagitIcerik_SelectedIndexChanged" onchange="$('.btn-AntetliKagitIcerik').css('display', 'none'); $(ImgAntetliKagitIcerikGuncelle).css('display', 'inline-block')" DataSourceID="OleDbAntetliKagitIcerik" DataTextField="AntetliKagitIcerikTipi" DataValueField="AntetliKagitIcerikID"></asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>*</td>
                                        <td style="width: 50px;">X</td>
                                        <td>
                                            <asp:TextBox ID="txtX" runat="server" CssClass="form-control txt-x"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>*</td>
                                        <td style="width: 50px;">Y</td>
                                        <td>
                                            <asp:TextBox ID="txtY" runat="server" CssClass="form-control txt-y"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>*</td>
                                        <td style="width: 50px;"><i class="fa fa-arrow-up"></i></td>
                                        <td>
                                            <asp:TextBox ID="txtWidth" runat="server" CssClass="form-control txt-width"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>*</td>
                                        <td style="width: 50px;"><i class="fa fa-arrow-right"></i></td>
                                        <td>
                                            <asp:TextBox ID="txtHeight" runat="server" CssClass="form-control txt-height"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>*</td>
                                        <td style="width: 50px;"><i class="fa fa-arrow-right-arrow-left"></i></td>
                                        <td>
                                            <asp:TextBox ID="txtOran" runat="server" CssClass="form-control txt-oran" Enabled="false"></asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <div class="col-md-6">
                                <asp:Image ID="ImgAntetliKagit" runat="server" Style="width: 100%; visibility: hidden" ClientIDMode="Static" />
                            </div>
                        </div>
                        <asp:UpdatePanel ID="UPnlAntetliKagitIcerikButon" runat="server">
                            <ContentTemplate>
                                <p align="center">
                                    <asp:LinkButton ID="lnkbtnAntetliKagitIcerikGuncelle" runat="server" CssClass="btn btn-success btn-AntetliKagitIcerik" OnClientClick="$(this).css('display', 'none'); $(ImgAntetliKagitIcerikGuncelle).css('display', 'inline-block');" OnClick="lnkbtnAntetliKagitIcerikGuncelle_Click">
                                        <i class="zmdi zmdi-refresh"></i>&nbsp;Güncelle
                                    </asp:LinkButton>
                                    <asp:Image ID="ImgAntetliKagitIcerikGuncelle" runat="server" ImageUrl="~/Gorseller/loadspinner.gif" ClientIDMode="Static" Style="display: none; width: 47px" />
                                </p>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-danger waves-effect" data-dismiss="modal">Kapat</button>
                    </div>
                </div>
            </div>

            <asp:HiddenField ID="hfMailIcerikID" runat="server" Visible="false" />
            <asp:SqlDataSource runat="server" ID="OleDbAntetliKagitIcerik" ConnectionString='<%$ ConnectionStrings:ArcadiasDavetConnection %>' ProviderName='<%$ ConnectionStrings:ArcadiasDavetConnection.ProviderName %>' SelectCommand="SELECT [AntetliKagitIcerikTablosu].[AntetliKagitIcerikID], [AntetliKagitIcerikTipiTablosu].[AntetliKagitIcerikTipi]  FROM [AntetliKagitIcerikTablosu] INNER JOIN [AntetliKagitIcerikTipiTablosu] ON [AntetliKagitIcerikTablosu].[AntetliKagitIcerikTipiID]  = [AntetliKagitIcerikTipiTablosu].[AntetliKagitIcerikTipiID] WHERE [AntetliKagitIcerikTablosu].[MailIcerikID] = ?">
                <SelectParameters>
                    <asp:ControlParameter ControlID="hfMailIcerikID" PropertyName="Value" Name="?"></asp:ControlParameter>
                </SelectParameters>
            </asp:SqlDataSource>
        </ContentTemplate>
    </asp:UpdatePanel>

    <asp:UpdatePanel ID="UPnlMailIcerikGuncelle" runat="server" UpdateMode="Conditional" class="modal fade" tabindex="-1" role="dialog" data-backdrop="static">
        <ContentTemplate>
            <div class="modal-dialog modal-dialog-centered modal-lg" role="document">
                <div class="modal-content">
                    <div class="modal-header">
                        <h4 class="title" runat="server" id="h4_MailIcerikGuncelle"></h4>
                    </div>
                    <div class="modal-body">
                        <div class="text-center">
                            <span class="btn btn-primary" data-content="{AdSoyad}" onclick="InsertContentToEditor(this, txtMailIcerik);">Ad & Soyad<br />
                                <span style="font-size: 8px;">{AdSoyad}</span>
                            </span>

                            <span class="btn btn-primary" data-content="{Unvan}" onclick="InsertContentToEditor(this, txtMailIcerik);">Üvan<br />
                                <span style="font-size: 8px;">{Unvan}</span>
                            </span>

                            <span class="btn btn-primary" data-content="{ePosta}" onclick="InsertContentToEditor(this, txtMailIcerik);">e-Posta<br />
                                <span style="font-size: 8px;">{ePosta}</span>
                            </span>

                            <span class="btn btn-primary" data-content="{Telefon}" onclick="InsertContentToEditor(this, txtMailIcerik);">Telefon<br />
                                <span style="font-size: 8px;">{Telefon}</span>
                            </span>

                            <span class="btn btn-primary" data-content="{Kurum}" onclick="InsertContentToEditor(this, txtMailIcerik);">Kurum<br />
                                <span style="font-size: 8px;">{Kurum}</span>
                            </span>

                            <span class="btn btn-success" data-content="{KabulLinki}" onclick="InsertContentToEditor(this, txtMailIcerik);">QR Linki<br />
                                <span style="font-size: 8px;">{KabulLinki}</span>
                            </span>
                        </div>
                        <table class="AlseinTable">
                            <tr>
                                <td>*</td>
                                <td>Konu</td>
                                <td>
                                    <asp:TextBox ID="txtKonu" runat="server" CssClass="form-control"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>*</td>
                                <td>Mail İçeriği</td>
                                <td>
                                    <CKEditor:CKEditorControl ID="txtMailIcerik" runat="server" BasePath="https://cdn.arkadyas.com/js/ckeditor" Toolbar="Bold|Italic|Underline|Subscript|Superscript|-|FontSize|TextColor|-|JustifyBlock|-|Link|NumberedList|BulletedList-|Source" ResizeEnabled="false" Language="tr" ClientIDMode="Static"></CKEditor:CKEditorControl>
                                </td>
                            </tr>
                        </table>
                        <p align="center">
                            <asp:LinkButton ID="lnkbtnMailIcerikGuncelle" runat="server" CssClass="btn btn-success" OnClientClick="$(this).css('display', 'none'); $(ImgMailIcerikGuncelle).css('display', 'inline-block')" OnClick="lnkbtnMailIcerikGuncelle_Click">
                                <i class="zmdi zmdi-refresh"></i>&nbsp;Güncelle
                            </asp:LinkButton>
                            <asp:Image ID="ImgMailIcerikGuncelle" runat="server" ImageUrl="~/Gorseller/loadspinner.gif" ClientIDMode="Static" Style="display: none; width: 47px" />
                        </p>
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-danger waves-effect" data-dismiss="modal">Kapat</button>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>

    <asp:UpdatePanel ID="UPnlSmsIcerikGuncelle" runat="server" UpdateMode="Conditional" class="modal fade" tabindex="-1" role="dialog" data-backdrop="static">
        <ContentTemplate>
            <div class="modal-dialog modal-dialog-centered modal-lg" role="document">
                <div class="modal-content">
                    <div class="modal-header">
                        <h4 class="title" runat="server" id="h4_SmsIcerikGuncelle"></h4>
                    </div>
                    <div class="modal-body">
                        <div class="text-center">
                            <span class="btn btn-primary" data-content="{AdSoyad}" onclick="InsertContentToTextArea(this, txtSmsIcerigi);">Ad & Soyad<br />
                                <span style="font-size: 8px;">{AdSoyad}</span>
                            </span>

                            <span class="btn btn-primary" data-content="{Unvan}" onclick="InsertContentToTextArea(this, txtSmsIcerigi);">Ünvan<br />
                                <span style="font-size: 8px;">{Unvan}</span>
                            </span>

                            <span class="btn btn-primary" data-content="{ePosta}" onclick="InsertContentToTextArea(this, txtSmsIcerigi);">e-Posta<br />
                                <span style="font-size: 8px;">{ePosta}</span>
                            </span>

                            <span class="btn btn-primary" data-content="{Telefon}" onclick="InsertContentToTextArea(this, txtSmsIcerigi);">Telefon<br />
                                <span style="font-size: 8px;">{Telefon}</span>
                            </span>

                            <span class="btn btn-primary" data-content="{Kurum}" onclick="InsertContentToTextArea(this, txtSmsIcerigi);">Kurum<br />
                                <span style="font-size: 8px;">{Kurum}</span>
                            </span>

                            <asp:Panel ID="PnlDavetiyeSmsIcerik" runat="server" Style="display: inline-block" Visible="false">
                                <span class="btn btn-success" data-content="{KabulLinki}" onclick="InsertContentToTextArea(this, txtSmsIcerigi);">Kabul Linki<br />
                                    <span style="font-size: 8px;">{KabulLinki}</span>
                                </span>

                                <span class="btn btn-danger" data-content="{RedLinki}" onclick="InsertContentToTextArea(this, txtSmsIcerigi);">Red Linki<br />
                                    <span style="font-size: 8px;">{RedLinki}</span>
                                </span>
                            </asp:Panel>

                            <asp:Panel ID="PnlQRSmsUcerik" runat="server" Style="display: inline-block;" Visible="false">
                                <span class="btn btn-success" data-content="{KabulLinki}" onclick="InsertContentToTextArea(this, txtSmsIcerigi);">QR Linki<br />
                                    <span style="font-size: 8px;">{KabulLinki}</span>
                                </span>
                            </asp:Panel>


                        </div>
                        <table class="AlseinTable">
                            <tr>
                                <td>&nbsp;</td>
                                <td>Sms İçeriği</td>
                                <td>
                                    <asp:TextBox ID="txtSmsIcerigi" runat="server" ClientIDMode="Static" CssClass="form-control" TextMode="MultiLine" Style="resize: none; height: 250px;"></asp:TextBox>
                                </td>
                            </tr>
                        </table>
                        <p align="center">
                            <asp:LinkButton ID="lnkbtnSmsIcerikGuncelle" runat="server" CssClass="btn btn-success" OnClientClick="$(this).css('display', 'none'); $(ImgSmsIcerikGuncelle).css('display', 'inline-block')" OnClick="lnkbtnSmsIcerikGuncelle_Click">
                                <i class="zmdi zmdi-refresh"></i>&nbsp;Güncelle
                            </asp:LinkButton>
                            <asp:Image ID="ImgSmsIcerikGuncelle" runat="server" ImageUrl="~/Gorseller/loadspinner.gif" ClientIDMode="Static" Style="display: none; width: 47px" />
                        </p>
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-danger waves-effect" data-dismiss="modal">Kapat</button>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>

    <asp:UpdatePanel ID="UPnlYakaKartiGorselGuncelle" runat="server" UpdateMode="Conditional" class="modal fade" tabindex="-1" role="dialog" data-backdrop="static">
        <ContentTemplate>
            <div class="modal-dialog modal-dialog-centered modal-lg" role="document">
                <div class="modal-content">
                    <div class="modal-header">
                        <h4 class="title">Yaka Kartı Görseli & Ölçüsü Güncelle</h4>
                    </div>
                    <div class="modal-body">
                        <table class="AlseinTable">
                            <tr>
                                <td>*</td>
                                <td>Genişlik ( mm )</td>
                                <td>
                                    <asp:TextBox ID="txtYKWidth" runat="server" CssClass="form-control"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>*</td>
                                <td>Yükseklik ( mm )</td>
                                <td>
                                    <asp:TextBox ID="txtYKHeight" runat="server" CssClass="form-control"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>*</td>
                                <td>Yazıcı Kağıt Ortalama</td>
                                <td>
                                    <asp:CheckBox ID="chkYaziciKagitOrtalama" runat="server" CssClass="form-control" Text="Yazıcım, yaka kartını, kağıt tepsisini ortalayarak basıyor." />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3" class="text-center font-bold">
                                    <p>Laser Jet türünde yazıcılar, genel olarak kağıt tepsisini ortalayarak basım yaparlar. Eğer kullandığınız yazıcı bu tip ise, yukarıdaki çentiği işaretleyiniz.</p>
                                </td>
                            </tr>
                            <tr>
                                <td>&nbsp;</td>
                                <td>Güncel Görsel</td>
                                <td>
                                    <input id="fuYakaKartiGorsel" type="file" class="form-control" accept="image/png, image/jpeg" onchange="YakaKartiGorselKontrol(this);" />
                                    <asp:HiddenField ID="hfYakaKartiGorsel" runat="server" ClientIDMode="Static" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3" class="text-center">Görseliniz png yada jpg uzantılı olmalı, en fazla 2mb olmalıdır.</td>
                            </tr>
                        </table>
                        <p align="center">
                            <asp:LinkButton ID="lnkbtnYakaKartiGorselGuncelle" runat="server" CssClass="btn btn-success" OnClientClick="$(this).css('display', 'none'); $(ImgYakaKartiGorselGuncelle).css('display', 'inline-block')" OnClick="lnkbtnYakaKartiGorselGuncelle_Click">
                                <i class="zmdi zmdi-refresh"></i>&nbsp;Güncelle
                            </asp:LinkButton>
                            <asp:Image ID="ImgYakaKartiGorselGuncelle" runat="server" ImageUrl="~/Gorseller/loadspinner.gif" ClientIDMode="Static" Style="display: none; width: 47px" />
                        </p>
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-danger waves-effect" data-dismiss="modal" onclick="$('#hfYakaKartiGorsel').val('');">Kapat</button>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>

    <asp:UpdatePanel ID="UPnlYakaKartiIcerikGuncelle" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="false" class="modal fade" tabindex="-1" role="dialog" data-backdrop="static">
        <ContentTemplate>
            <div class="modal-dialog modal-dialog-centered modal-lg" role="document">
                <div class="modal-content">
                    <div class="modal-header">
                        <h4 class="title" runat="server">Yaka Kartı İçerik Güncelle</h4>
                    </div>
                    <div class="modal-body">
                        <div class="row m-0 p-3">
                            <div class="col-md-12">
                                <h6 class="text-center">Eklenebilir Alanlar</h6>
                                <table class="AlseinTable">
                                    <tr>
                                        <td>&nbsp;</td>
                                        <td>Alan</td>
                                        <td>
                                            <asp:DropDownList ID="ddlYakaKartiIcerikTipi" runat="server" CssClass="form-control" DataSourceID="OleDbYakaKartiIcerikTipiListesi" DataTextField="YakaKartiIcerikTipi" DataValueField="YakaKartiIcerikTipiID"></asp:DropDownList>
                                        </td>
                                    </tr>
                                </table>
                                <asp:UpdatePanel ID="UPnlYakaKartiIcerikTipiEkleButton" runat="server">
                                    <ContentTemplate>
                                        <p class="text-center">
                                            <asp:LinkButton ID="lnkbtnYakaKartiTipiEkle" runat="server" CssClass="btn btn-success btn-YakaKartiIcerik" OnClick="lnkbtnYakaKartiTipiEkle_Click" OnClientClick="$(this).css('display', 'none'); $(ImgYakaKartiIcerikGuncelle).css('display', 'inline-block');">
                                                <i class="fa fa-plus"></i>&nbsp;Yaka Kartı Alanı Ekle
                                            </asp:LinkButton>
                                        </p>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                            <div class="col-md-6">
                                <table class="AlseinTable">
                                    <tr>
                                        <td>&nbsp;</td>
                                        <td style="width: 50px;">İçerik</td>
                                        <td>
                                            <asp:DropDownList ID="ddlYakaKartiIcerik" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlYakaKartiIcerik_SelectedIndexChanged" onchange="$('.btn-YakaKartiIcerik').css('display', 'none'); $(ImgYakaKartiIcerikGuncelle).css('display', 'inline-block')" DataSourceID="OleDbYakaKartiIcerik" DataTextField="YakaKartiIcerikTipi" DataValueField="YakaKartiIcerikID"></asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>*</td>
                                        <td style="width: 50px;">X</td>
                                        <td>
                                            <asp:TextBox ID="txtYKIX" runat="server" CssClass="form-control txt-ykix"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>*</td>
                                        <td style="width: 50px;">Y</td>
                                        <td>
                                            <asp:TextBox ID="txtYKIY" runat="server" CssClass="form-control txt-ykiy"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>*</td>
                                        <td style="width: 50px;"><i class="fa fa-arrow-up"></i></td>
                                        <td>
                                            <asp:TextBox ID="txtYKIWidth" runat="server" CssClass="form-control txt-ykiwidth"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>*</td>
                                        <td style="width: 50px;"><i class="fa fa-arrow-right"></i></td>
                                        <td>
                                            <asp:TextBox ID="txtYKIHeight" runat="server" CssClass="form-control txt-ykiheight"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>*</td>
                                        <td style="width: 50px;"><i class="fa fa-arrow-right-arrow-left"></i></td>
                                        <td>
                                            <asp:TextBox ID="txtYKIOran" runat="server" CssClass="form-control txt-ykioran" Enabled="false"></asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                                <p class="text-center">
                                    <asp:LinkButton ID="lnkbtnYakaKartiIcerikSil" runat="server" CssClass="btn btn-danger btn-YakaKartiIcerik" OnClick="lnkbtnYakaKartiIcerikSil_Click" OnClientClick="$(this).css('display', 'none'); $(ImgYakaKartiIcerikGuncelle).css('display', 'inline-block');">
                                        <i class="fa fa-trash"></i>&nbsp;Alanı Sil
                                    </asp:LinkButton>
                                </p>
                            </div>
                            <div class="col-md-6">
                                <asp:Image ID="ImgYakaKarti" runat="server" Style="width: 100%; visibility: hidden" ClientIDMode="Static" />
                            </div>
                        </div>
                        <asp:UpdatePanel ID="UPnlYKIButon" runat="server">
                            <ContentTemplate>
                                <p align="center">
                                    <asp:LinkButton ID="lnkbtnYakaKartiIcerikGuncelle" runat="server" CssClass="btn btn-success btn-YakaKartiIcerik" OnClientClick="$(this).css('display', 'none'); $(ImgYakaKartiIcerikGuncelle).css('display', 'inline-block');" OnClick="lnkbtnYakaKartiIcerikGuncelle_Click">
                                        <i class="zmdi zmdi-refresh"></i>&nbsp;Güncelle
                                    </asp:LinkButton>
                                    <asp:Image ID="ImgYakaKartiIcerikGuncelle" runat="server" ImageUrl="~/Gorseller/loadspinner.gif" ClientIDMode="Static" Style="display: none; width: 47px" />
                                </p>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-danger waves-effect" data-dismiss="modal">Kapat</button>
                    </div>
                </div>
            </div>

            <asp:HiddenField ID="hfYakaKartiCerceveID" runat="server" Visible="false" />
            <asp:SqlDataSource runat="server" ID="OleDbYakaKartiIcerik" ConnectionString='<%$ ConnectionStrings:ArcadiasDavetConnection %>' ProviderName='<%$ ConnectionStrings:ArcadiasDavetConnection.ProviderName %>' SelectCommand="SELECT [YakaKartiIcerikTablosu].[YakaKartiIcerikID], [YakaKartiIcerikTipiTablosu].[YakaKartiIcerikTipi] FROM ( [YakaKartiIcerikTablosu] INNER JOIN [YakaKartiIcerikTipiTablosu] ON [YakaKartiIcerikTablosu].[YakaKartiIcerikTipiID] = [YakaKartiIcerikTipiTablosu].[YakaKartiIcerikTipiID] ) INNER JOIN [YakaKartiCerceveTablosu] ON [YakaKartiIcerikTablosu].[YakaKartiCerceveID] = [YakaKartiCerceveTablosu].[YakaKartiCerceveID] WHERE [YakaKartiCerceveTablosu].[YakaKartiCerceveID] = ?">
                <SelectParameters>
                    <asp:ControlParameter ControlID="hfYakaKartiCerceveID" PropertyName="Value" Name="?"></asp:ControlParameter>
                </SelectParameters>
            </asp:SqlDataSource>

            <asp:SqlDataSource runat="server" ID="OleDbYakaKartiIcerikTipiListesi" ConnectionString='<%$ ConnectionStrings:ArcadiasDavetConnection %>' ProviderName='<%$ ConnectionStrings:ArcadiasDavetConnection.ProviderName %>' SelectCommand="SELECT [YakaKartiIcerikTipiTablosu].* FROM [YakaKartiIcerikTipiTablosu] LEFT JOIN ( SELECT [YakaKartiIcerikTablosu].[YakaKartiIcerikID], [YakaKartiIcerikTablosu].[YakaKartiIcerikTipiID] FROM [YakaKartiIcerikTablosu] INNER JOIN [YakaKartiCerceveTablosu] ON [YakaKartiIcerikTablosu].[YakaKartiCerceveID] = [YakaKartiCerceveTablosu].[YakaKartiCerceveID] WHERE [YakaKartiCerceveTablosu].[YakaKartiCerceveID] = ? ) AS [YakaKartiIcerikTablosu] ON [YakaKartiIcerikTipiTablosu].[YakaKartiIcerikTipiID] = [YakaKartiIcerikTablosu].[YakaKartiIcerikTipiID] WHERE [YakaKartiIcerikTablosu].[YakaKartiIcerikID] IS NULL">
                <SelectParameters>
                    <asp:ControlParameter ControlID="hfYakaKartiCerceveID" PropertyName="Value" DefaultValue="0" Name="?"></asp:ControlParameter>
                </SelectParameters>
            </asp:SqlDataSource>
        </ContentTemplate>
    </asp:UpdatePanel>

    <asp:UpdatePanel ID="UPnlKarsilamaEkraniGorselGuncelle" runat="server" UpdateMode="Conditional" class="modal fade" tabindex="-1" role="dialog" data-backdrop="static">
        <ContentTemplate>
            <div class="modal-dialog modal-dialog-centered modal-lg" role="document">
                <div class="modal-content">
                    <div class="modal-header">
                        <h4 class="title">Karşılama Ekranı Güncelle</h4>
                    </div>
                    <div class="modal-body">
                        <table class="AlseinTable">
                            <tr>
                                <td>&nbsp;</td>
                                <td>Güncel Görsel</td>
                                <td>
                                    <input id="fuKarsilamaEkraniGorsel" type="file" class="form-control" accept="image/png, image/jpeg" onchange="KarsilamaEkraniGorselKontrol(this);" />
                                    <asp:HiddenField ID="hfKarsilamaEkraniGorsel" runat="server" ClientIDMode="Static" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3" class="text-center">Görseliniz png yada jpg uzantılı olmalı, en fazla 2mb olmalıdır.</td>
                            </tr>
                        </table>
                        <p align="center">
                            <asp:LinkButton ID="lnkbtnKarsilamaEkraniGorsel" runat="server" CssClass="btn btn-success" OnClientClick="$(this).css('display', 'none'); $(ImgKarsilamaEkraniGorsel).css('display', 'inline-block')" OnClick="lnkbtnKarsilamaEkraniGorsel_Click">
                                <i class="zmdi zmdi-refresh"></i>&nbsp;Güncelle
                            </asp:LinkButton>
                            <asp:Image ID="ImgKarsilamaEkraniGorsel" runat="server" ImageUrl="~/Gorseller/loadspinner.gif" ClientIDMode="Static" Style="display: none; width: 47px" />
                        </p>
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-danger waves-effect" data-dismiss="modal" onclick="$('#hfKarsilamaEkraniGorsel').val('');">Kapat</button>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>

    <asp:UpdatePanel ID="UPnlLogoGuncelle" runat="server" UpdateMode="Conditional" class="modal fade" tabindex="-1" role="dialog" data-backdrop="static">
        <ContentTemplate>
            <div class="modal-dialog modal-dialog-centered modal-lg" role="document">
                <div class="modal-content">
                    <div class="modal-header">
                        <h4 class="title">Logo Güncelleme</h4>
                    </div>
                    <div class="modal-body">
                        <table class="AlseinTable">
                            <tr>
                                <td>&nbsp;</td>
                                <td>Logo</td>
                                <td>
                                    <input id="fuLogo" type="file" class="form-control" accept="image/png, image/jpeg" onchange="LogoKontrol(this);" />
                                    <asp:HiddenField ID="hfLogo" runat="server" ClientIDMode="Static" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3" class="text-center">Görseliniz png yada jpg uzantılı olmalı, en fazla 1mb olmalıdır.</td>
                            </tr>
                        </table>
                        <p align="center">
                            <asp:LinkButton ID="lnkbtnLogoGuncelle" runat="server" CssClass="btn btn-success" OnClientClick="$(this).css('display', 'none'); $(ImgLogoGuncelle).css('display', 'inline-block')" OnClick="lnkbtnLogoGuncelle_Click">
                                <i class="zmdi zmdi-refresh"></i>&nbsp;Güncelle
                            </asp:LinkButton>
                            <asp:Image ID="ImgLogoGuncelle" runat="server" ImageUrl="~/Gorseller/loadspinner.gif" ClientIDMode="Static" Style="display: none; width: 47px" />
                        </p>
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-danger waves-effect" data-dismiss="modal" onclick="$('#hfLogo').val('');">Kapat</button>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>

    <asp:UpdatePanel ID="UPnlEkranIcerik" runat="server" UpdateMode="Conditional" class="modal fade" tabindex="-1" role="dialog" data-backdrop="static">
        <ContentTemplate>
            <div class="modal-dialog modal-dialog-centered modal-lg" role="document">
                <div class="modal-content">
                    <div class="modal-header">
                        <h4 class="title" runat="server" id="h4_EkranIcerik"></h4>
                    </div>
                    <div class="modal-body">
                        <table class="AlseinTable">
                            <tr>
                                <td>*</td>
                                <td id="td_IcerikBaslik" runat="server" data-durum="">İçerik</td>
                                <td>
                                    <CKEditor:CKEditorControl ID="txtEkranIcerik" runat="server" BasePath="https://cdn.arkadyas.com/js/ckeditor" Toolbar="Bold|Italic|Underline|Subscript|Superscript|-|FontSize|TextColor|-|JustifyBlock|-|Link|NumberedList|BulletedList-|Source" ResizeEnabled="false" Language="tr" ClientIDMode="Static"></CKEditor:CKEditorControl>
                                </td>
                            </tr>
                        </table>
                        <p align="center">
                            <asp:LinkButton ID="lnkbtnEkranIcerikGuncelle" runat="server" CssClass="btn btn-success" OnClientClick="$(this).css('display', 'none'); $(ImgEkranIcerikGuncelle).css('display', 'inline-block')" OnClick="lnkbtnEkranIcerikGuncelle_Click">
                                <i class="zmdi zmdi-refresh"></i>&nbsp;Güncelle
                            </asp:LinkButton>
                            <asp:Image ID="ImgEkranIcerikGuncelle" runat="server" ImageUrl="~/Gorseller/loadspinner.gif" ClientIDMode="Static" Style="display: none; width: 47px" />
                        </p>
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-danger waves-effect" data-dismiss="modal">Kapat</button>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>

</asp:Content>
