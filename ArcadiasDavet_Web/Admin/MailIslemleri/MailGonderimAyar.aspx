<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.master" AutoEventWireup="true" CodeBehind="MailGonderimAyar.aspx.cs" Inherits="ArcadiasDavet_Web.Admin.MailIslemleri.MailGonderimAyar" ValidateRequest="false"%>

<asp:Content ID="Content1" ContentPlaceHolderID="Admin_head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="Admin_Icerik" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <div class="row">
        <div class="col-md-12">
            <div class="card">
                <div class="header">
                    <h2>
                        <strong>Mail Gönderim</strong> Ayarları
                    </h2>
                </div>

                <asp:UpdatePanel ID="UPnlMailAyar" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="false" class="body">
                    <ContentTemplate>

                        <table class="AlseinTable">
                            <tbody>
                                <tr>
                                    <td>*</td>
                                    <td>Gönderen Ad</td>
                                    <td>
                                        <asp:TextBox ID="txtGonderenAd" runat="server" CssClass="form-control"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>*</td>
                                    <td>e-Posta</td>
                                    <td>
                                        <asp:TextBox ID="txtePosta" runat="server" CssClass="form-control" TextMode="Email"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>*</td>
                                    <td>Kullanıcı Adı</td>
                                    <td>
                                        <asp:TextBox ID="txtKullaniciAdi" runat="server" CssClass="form-control"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>*</td>
                                    <td>Şifre</td>
                                    <td>
                                        <asp:TextBox ID="txtSifre" runat="server" CssClass="form-control"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>*</td>
                                    <td>Giden Mail Host</td>
                                    <td>
                                        <asp:TextBox ID="txtGidenMailHost" runat="server" CssClass="form-control"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>*</td>
                                    <td>Giden Mail Port</td>
                                    <td>
                                        <asp:TextBox ID="txtGidenMailPort" runat="server" CssClass="form-control" TextMode="Number"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>*</td>
                                    <td>Gelen Mail Host</td>
                                    <td>
                                        <asp:TextBox ID="txtGelenMailHost" runat="server" CssClass="form-control"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>*</td>
                                    <td>Gelen Mail Port</td>
                                    <td>
                                        <asp:TextBox ID="txtGelenMailPort" runat="server" CssClass="form-control" TextMode="Number"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>*</td>
                                    <td>SSL</td>
                                    <td>
                                        <asp:CheckBox ID="chkSSL" runat="server" Text="SSL Kullanılıyor" CssClass="form-control"/>
                                    </td>
                                </tr>
                                <tr>
                                    <td>*</td>
                                    <td>BCC</td>
                                    <td>
                                        <asp:TextBox ID="txtBCC" runat="server" CssClass="form-control"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>*</td>
                                    <td>Cevaplama Adresi</td>
                                    <td>
                                        <asp:TextBox ID="txtReplyTo" runat="server" CssClass="form-control"></asp:TextBox>
                                    </td>
                                </tr>
                            </tbody>
                        </table>

                        <p class="text-center">
                            <asp:LinkButton ID="lnkbtnMailAyarGuncelle" runat="server" CssClass="btn btn-success" OnClick="lnkbtnMailAyarGuncelle_Click" OnClientClick="">
                                <i class="fa fa-refresh"></i>&nbsp;Güncelle
                            </asp:LinkButton>
                        </p>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
    </div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="Admin_AltSayfa" runat="server">
</asp:Content>