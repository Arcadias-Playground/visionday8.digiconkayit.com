<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.master" AutoEventWireup="true" CodeBehind="Katilimci.aspx.cs" Inherits="ArcadiasDavet_Web.Admin.Rapor.Katilimci" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Admin_head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="Admin_Icerik" runat="server">
    <div class="row">
        <div class="col-md-12">
            <div class="card">
                <div class="header">
                    <h2><strong>Katılımcı</strong> Raporu</h2>
                </div>
            </div>
        </div>
    </div>
    <div class="row clearfix">
        <div class="col-lg-3 col-md-6 col-sm-12">
            <div class="card widget_2">
                <div class="body">
                    <h6>Toplam Katılımcı</h6>
                    <h2><span id="spn_ToplamKatilimci" runat="server"></span></h2>
                    <small>Kayıtlı olan tüm katılımcıların sayısı</small>
                    <div>
                        <asp:HyperLink ID="hyplnkKatilimciListesi" runat="server" NavigateUrl="~/Admin/Rapor/KatilimciRapor" Target="_blank"  CssClass="btn btn-primary">
                            <i class="fa fa-file-excel"></i>&nbsp;Katılımcı Listesi
                        </asp:HyperLink>
                    </div>
                </div>
            </div>
        </div>

        <div class="col-lg-3 col-md-6 col-sm-12">
            <div class="card widget_2">
                <div class="body">
                    <h6>Toplam Katılımcı</h6>
                    <h2><span id="spn_TumOnayliKatilimci" runat="server"></span></h2>
                    <small>Etkinliğe katılacak tüm katılımcıların sayısı</small>
                </div>
            </div>
        </div>
    </div>
    <div class="row clearfix">
        <div class="col-lg-3 col-md-3 col-sm-3">
            <div class="card widget_2">
                <div class="body">
                    <h6>Ana Katılımcı</h6>
                    <h2><span id="spn_AnaKatilimci" runat="server"></span></h2>
                    <small>Kayıtlı olan tüm ana katılımcı sayısı</small>
                </div>
            </div>
        </div>

        <div class="col-lg-3 col-md-3 col-sm-3">
            <div class="card widget_2">
                <div class="body">
                    <h6>Katılacak Ana Katılımcı</h6>
                    <h2><span id="spn_OnayliKatilimci" runat="server">250</span></h2>
                    <small>Etkinliğe katılacağını bildiren ana katılımcı sayısı</small>
                </div>
            </div>
        </div>

        <div class="col-lg-3 col-md-3 col-sm-3">
            <div class="card widget_2">
                <div class="body">
                    <h6>Katılmayacak Ana Katılımcı</h6>
                    <h2><span id="spn_RedKatilimci" runat="server"></span></h2>
                    <small>Etkinliğe katılmayacağını bildiren ana katılımcı sayısı</small>
                </div>
            </div>
        </div>

        <div class="col-lg-3 col-md-3 col-sm-3">
            <div class="card widget_2">
                <div class="body">
                    <h6>Cevap Beklenen Katılımcı</h6>
                    <h2><span id="spn_CevapBekleyenKatilimci" runat="server"></span></h2>
                    <small>Katılım cevabı beklenen ana katılımcı sayısı</small>
                </div>
            </div>
        </div>
    </div>
    <div class="row clearfix">
        <div class="col-lg-3 col-md-6 col-sm-12">
            <div class="card widget_2">
                <div class="body">
                    <h6>Misafir Sayısı</h6>
                    <h2><span id="spn_MisafirKatilimci" runat="server"></span></h2>
                </div>
            </div>
        </div>
    </div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="Admin_AltSayfa" runat="server">
</asp:Content>