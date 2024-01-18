<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="ArcadiasDavet_Web.Admin.KioskEkran.Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Admin_head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="Admin_Icerik" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <div class="row">
        <div class="col-md-12">
            <div class="card">
                <div class="header">
                    <h2><strong>Kiosk Ekran</strong> İşlemleri</h2>
                </div>
                <asp:UpdatePanel ID="UPnlGenel" runat="server" class="body">
                    <ContentTemplate>
                        <p class="text-center">
                            Yüklemeniz gereken ekran görüntüsü jpg yada png uzantılı ve en fazla 3mb olmalıdır.
                        </p>
                        <table class="AlseinTable">
                            <tr>
                                <td>*</td>
                                <td>Güncel Kiosk Görseli</td>
                                <td>
                                    <input type="file" class="form-control" accept="image/png, image/jpeg, image/jpg" onchange="KioskGorselKontrol(this);" />
                                    <asp:HiddenField ID="hfKioskGorsel" runat="server" ClientIDMode="Static"/>
                                </td>
                            </tr>
                        </table>
                        <p class="text-center">
                            <asp:LinkButton ID="lnkbtnKioskGorselGuncelle" runat="server" CssClass="btn btn-success" OnClick="lnkbtnKioskGorselGuncelle_Click" OnClientClick="$(this).css('display', 'none'); $(ImgKioskGorselGuncelle).css('display', 'inline-block');">
                                <i class="fa fa-refresh"></i>&nbsp;Güncelle
                            </asp:LinkButton>
                            <asp:Image ID="ImgKioskGorselGuncelle" runat="server" ClientIDMode="Static" Style="display: none; width: 47px"/>
                        </p>

                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
    </div>



</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="Admin_AltSayfa" runat="server">
</asp:Content>
