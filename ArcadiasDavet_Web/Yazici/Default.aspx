<%@ Page Title="" Language="C#" MasterPageFile="~/Yazici/Yazici.master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="ArcadiasDavet_Web.Yazici.Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Yazici_head" runat="server">
    <script src="<%=ResolveClientUrl("~/js/jquery.signalR-2.4.3.min.js") %>"></script>
    <script src="<%=ResolveClientUrl("~/ArcadiasDavet/Yazici/Hubs") %>"></script>
    <script src="<%=ResolveClientUrl($"~/js/Yazici.js") %>"></script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="Yazici_Icerik" runat="server">
    <div class="row">
        <div class="col-md-12">
            <div class="card">
                <div class="header">
                    <h2><strong>Yazıcı</strong> İşlemleri</h2>
                </div>
                <div class="body text-center">
                    <p class="p-2 border">
                        <asp:Image ID="ImgArcadiasLogo" runat="server" CssClass="w-50" ImageUrl="~/Gorseller/ArcadiasLogo.png" style="border-radius:5px;"/>
                    </p>
                    <p>
                        Bağlantı Durumu : <span id="span_ConnectionStatus" style="width: 20px; height: 20px; border: 1px solid black; border-radius: 50%;">&nbsp;&nbsp;</span>
                    </p>
                </div>
            </div>
        </div>
    </div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="Yazici_AltSayfa" runat="server">
</asp:Content>
