<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="ArcadiasDavet_Web.Admin.KullaniciIslemleri.Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Admin_head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="Admin_Icerik" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <div class="row">
        <div class="col-md-12">
            <div class="card">
                <div class="header">
                    <h2>
                        <strong>Kullanıcı</strong> İşlemleri
                    </h2>
                </div>


                <asp:UpdatePanel ID="UPnlKullaniciListesi" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="false" class="body">
                    <ContentTemplate>
                        <p class="text-center">
                            <asp:LinkButton ID="lnkbtnYeniKullaniciEkle" runat="server" CssClass="btn btn-success" OnClick="lnkbtnYeniKullaniciEkle_Click">
                               <i class="fa fa-plus"></i>&nbsp;Yeni Kullanıcı Ekle
                            </asp:LinkButton>
                        </p>

                        <table class="table table-striped">
                            <thead>
                                <tr>
                                    <th>Kullanıcı Tipi</th>
                                    <th>Kullanıcı</th>
                                    <th>e-Posta</th>
                                    <th>Şifre</th>
                                    <th>İşlemler</th>
                                </tr>
                            </thead>
                            <tbody>
                                <asp:Repeater ID="rptKullaniciListesi" runat="server" ClientIDMode="AutoID" OnItemCommand="rptKullaniciListesi_ItemCommand" DataSourceID="OleDbKullaniciListesi">
                                    <ItemTemplate>
                                        <tr>
                                            <td><%#Eval("KullaniciTipi") %></td>
                                            <td><%#Eval("AdSoyad") %></td>
                                            <td><%#Eval("ePosta") %></td>
                                            <td><%#Eval("Sifre") %></td>
                                            <td>
                                                <asp:LinkButton ID="lnkbtnKullaniciGuncelle" runat="server" CssClass="btn btn-success" CommandArgument='<%#Eval("KullaniciID") %>' CommandName="Guncelle">
                                                    <i class="fa fa-sync"></i>&nbsp;Güncelle
                                                </asp:LinkButton>
                                            </td>
                                        </tr>
                                    </ItemTemplate>
                                </asp:Repeater>

                            </tbody>
                        </table>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
    </div>

    <asp:SqlDataSource runat="server" ID="OleDbKullaniciListesi" ConnectionString='<%$ ConnectionStrings:ArcadiasDavetConnection %>' ProviderName='<%$ ConnectionStrings:ArcadiasDavetConnection.ProviderName %>' SelectCommand="SELECT KullaniciTablosu.*, KullaniciTipiTablosu.KullaniciTipi FROM KullaniciTablosu INNER JOIN KullaniciTipiTablosu ON KullaniciTablosu.KullaniciTipiID = KullaniciTipiTablosu.KullaniciTipiID WHERE KullaniciTablosu.KullaniciID <> '00000000-0000-0000-0000-000000000000' ORDER BY KullaniciTablosu.AdSoyad"></asp:SqlDataSource>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="Admin_AltSayfa" runat="server">
    <asp:UpdatePanel ID="UPnlKullaniciEkleGuncelle" runat="server" UpdateMode="Conditional" class="modal fade" tabindex="-1" role="dialog" data-backdrop="static">
        <ContentTemplate>
            <div class="modal-dialog modal-lg" role="document">
                <div class="modal-content">
                    <div class="modal-header">
                        <h4 class="title">Kullanıcı Ekle / Güncelle</h4>
                    </div>
                    <div class="modal-body">
                        <table class="AlseinTable">
                            <tr id="tr_KullaniciID" runat="server" visible="false">
                                <td>&nbsp;</td>
                                <td>Kullanıcı ID</td>
                                <td>
                                    <asp:TextBox ID="txtKullaniciID" runat="server" CssClass="form-control" Enabled="false" ReadOnly="true"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>&nbsp;</td>
                                <td>Kullanıcı Tipi</td>
                                <td>
                                    <asp:DropDownList ID="ddlKullaniciTipi" runat="server" CssClass="form-control" DataSourceID="OleDbKullaniciTipiListesi" DataTextField="KullaniciTipi" DataValueField="KullaniciTipiID"></asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td>&nbsp;</td>
                                <td>Ad & Soyad</td>
                                <td>
                                    <asp:TextBox ID="txtAdSoyad" runat="server" CssClass="form-control" onchange="toUpper(this);" onkeyup="toUpper(this);"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>&nbsp;</td>
                                <td>e-Posta</td>
                                <td>
                                    <asp:TextBox ID="txtePosta" runat="server" CssClass="form-control"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>&nbsp;</td>
                                <td>Şifre</td>
                                <td>
                                    <asp:TextBox ID="txtSifre" runat="server" CssClass="form-control"></asp:TextBox>
                                </td>
                            </tr>
                        </table>
                        <p align="center">
                            <asp:LinkButton ID="lnkbtnKullaniciEkleGuncelle" runat="server" CssClass="btn btn-success" OnClientClick="$(this).css('display', 'none'); $(Yukleniyor).css('display', 'inline-block')" OnClick="lnkbtnKullaniciEkleGuncelle_Click">
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

            <asp:SqlDataSource runat="server" ID="OleDbKullaniciTipiListesi" ConnectionString='<%$ ConnectionStrings:ArcadiasDavetConnection %>' ProviderName='<%$ ConnectionStrings:ArcadiasDavetConnection.ProviderName %>' SelectCommand="SELECT [KullaniciTipiID], [KullaniciTipi] FROM [KullaniciTipiTablosu]"></asp:SqlDataSource>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
