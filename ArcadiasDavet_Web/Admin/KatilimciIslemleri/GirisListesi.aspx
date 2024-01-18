<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.master" AutoEventWireup="true" CodeBehind="GirisListesi.aspx.cs" Inherits="ArcadiasDavet_Web.Admin.KatilimciIslemleri.GirisListesi" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Admin_head" runat="server">
    <link rel="stylesheet" href='<%=ResolveClientUrl("~/css/dataTables.bootstrap4.min.css") %>' />

    <script type="text/javascript" src='<%=ResolveClientUrl("~/js/datatablescripts.bundle.js") %>'></script>
    <script type="text/javascript" src='<%=ResolveClientUrl("~/js/jquery.dataTables.i18n.min.js") %>'></script>

    <script>
        $(document).ready(() => {
            DataTableKurulum(tbl_KatilimciGirisListesi, true);
        });
    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="Admin_Icerik" runat="server">
    <div class="row">
        <div class="col-md-12">
            <div class="card">
                <div class="header">
                    <h2><strong>QR Okutarak</strong> Giriş Yapanlar Listesi</h2>
                </div>
                <div class="body">
                    <table id="tbl_KatilimciGirisListesi" class="table table-bordered table-striped table-hover js-basic-example">
                        <thead>
                            <tr>
                                <th>Katılımcı</th>
                                <th>Katılımcı Tipi</th>
                                <th>Girişi Onaylayan</th>
                                <th>Giriş Tarihi</th>
                            </tr>
                        </thead>
                        <tbody>
                            <asp:Repeater ID="rptMailGonderimListesi" runat="server" DataSourceID="OleDbMailGonderimListesi">
                                <ItemTemplate>
                                    <tr>
                                        <td>
                                            <div>
                                                <%#Eval("AdSoyad") %>
                                            </div>
                                            <asp:Panel ID="PnlAnaKatilimci" runat="server" Visible='<%#!Eval("AnaKatilimciID").Equals(DBNull.Value) %>'>
                                                <span style="color: red">Ana Katılımcı</span> : <%#Eval("AnaDavetli") %>
                                            </asp:Panel>
                                        </td>
                                        <td>
                                            <%#Eval("KatilimciTipi") %>
                                        </td>
                                        <td>
                                            <%#Eval("KullaniciID") %>
                                        </td>
                                        <td>
                                            <%#Eval("EklenmeTarihi") %>
                                        </td>
                                    </tr>
                                </ItemTemplate>
                            </asp:Repeater>
                        </tbody>
                    </table>
                </div>
            </div>
        </div>
    </div>


    <asp:SqlDataSource runat="server" ID="OleDbMailGonderimListesi" ConnectionString='<%$ ConnectionStrings:ArcadiasDavetConnection %>' ProviderName='<%$ ConnectionStrings:ArcadiasDavetConnection.ProviderName %>' SelectCommand="SELECT KatilimciTablosu.AdSoyad, KatilimciTablosu.ePosta, KatilimciTablosu.AnaKatilimciID, KatilimciTipiTablosu.KatilimciTipi, AnaKatilimciTablosu.AdSoyad AS [AnaDavetli], KatilimciGirisTablosu.KullaniciID, KatilimciGirisTablosu.EklenmeTarihi FROM ( ( KatilimciTablosu INNER JOIN KatilimciGirisTablosu ON KatilimciTablosu.KatilimciID = KatilimciGirisTablosu.KatilimciID ) INNER JOIN KatilimciTipiTablosu ON KatilimciTablosu.KatilimciTipiID = KatilimciTipiTablosu.KatilimciTipiID ) LEFT JOIN ( SELECT * FROM KatilimciTablosu WHERE AnaKatilimciID IS NULL ) AS AnaKatilimciTablosu ON KatilimciTablosu.AnaKatilimciID = AnaKatilimciTablosu.KatilimciID ORDER BY KatilimciGirisTablosu.EklenmeTarihi DESC"></asp:SqlDataSource>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="Admin_AltSayfa" runat="server">
</asp:Content>