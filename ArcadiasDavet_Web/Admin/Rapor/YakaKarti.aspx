<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.master" AutoEventWireup="true" CodeBehind="YakaKarti.aspx.cs" Inherits="ArcadiasDavet_Web.Admin.Rapor.YakaKarti" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Admin_head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="Admin_Icerik" runat="server">
    <div class="row">
        <div class="col-md-12">
            <div class="card">
                <div class="header">
                    <h2><strong>Yaka Kartı</strong> Raporu</h2>
                </div>
            </div>
        </div>
    </div>

    <div class="row clearfix">
        <div class="col-lg-3 col-md-6 col-sm-12">
            <div class="card widget_2">
                <div class="body">
                    <h6>Toplam Basılan Yaka Kartı Sayısı</h6>
                    <h2><span id="spn_ToplamGiris" runat="server"></span></h2>
                    <small>Tüm giriş kaydı sayısı</small>
                    <div>
                        <asp:HyperLink ID="hyplnkKatilimciListesi" runat="server" NavigateUrl="~/Admin/Rapor/YakaKartiRapor" Target="_blank"  CssClass="btn btn-primary">
                            <i class="fa fa-file-excel"></i>&nbsp;Yaka Kartı Basım Listesi
                        </asp:HyperLink>
                    </div>
                </div>
            </div>
        </div>
    </div>


    <div class="row clearfix">
        <asp:Repeater ID="rptKatilimciTipiGirisListesi" runat="server">
            <ItemTemplate>
                <div class="col-lg-3 col-md-3 col-sm-3">
                    <div class="card widget_2">
                        <div class="body">
                            <h6><%#Eval("KatilimciTipi") %></h6>
                            <h2><%#Eval("GirisSayisi") %></h2>
                        </div>
                    </div>
                </div>
            </ItemTemplate>
        </asp:Repeater>
    </div>

    <div class="row clearfix">
        <div class="col-lg-3 col-md-6 col-sm-12">
            <div class="card widget_2">
                <div class="body">
                    <h6>Misafir Sayısı</h6>
                    <h2><span id="spn_MisafirGiris" runat="server"></span></h2>
                </div>
            </div>
        </div>
    </div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="Admin_AltSayfa" runat="server">
</asp:Content>