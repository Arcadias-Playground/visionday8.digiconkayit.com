<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.master" AutoEventWireup="true" CodeBehind="SmsGonderimListesi.aspx.cs" Inherits="ArcadiasDavet_Web.Admin.MailIslemleri.SmsGonderimListesi" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Admin_head" runat="server">
    <link rel="stylesheet" href='<%=ResolveClientUrl("~/css/dataTables.bootstrap4.min.css") %>' />

    <script type="text/javascript" src='<%=ResolveClientUrl("~/js/datatablescripts.bundle.js") %>'></script>
    <script type="text/javascript" src='<%=ResolveClientUrl("~/js/jquery.dataTables.i18n.min.js") %>'></script>

    <script>
        $(document).ready(() => {
            DataTableKurulum(tbl_SmsGonderimListesi, true);
        });
    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="Admin_Icerik" runat="server">
    <div class="row">
        <div class="col-md-12">
            <div class="card">
                <div class="header">
                    <h2><strong>Mail Gönderim</strong> Detayları</h2>
                </div>
                <div class="body">
                    <table id="tbl_SmsGonderimListesi" class="table table-bordered table-striped table-hover js-basic-example">
                        <thead>
                            <tr>
                                <th>Katılımcı</th>
                                <th>Katılımcı Tipi</th>
                                <th>Telefon</th>
                                <th>Sms Tipi</th>
                                <th>Durum</th>
                                <th>Gönderim Tarihi</th>
                            </tr>
                        </thead>
                        <tbody>
                            <asp:Repeater ID="rptSmsGonderimListesi" runat="server" DataSourceID="OleDbSmsGonderimListesi">
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
                                            <%#Eval("Telefon") %>
                                        </td>
                                        <td>
                                           <%#Eval("GonderimTipi") %>
                                        </td>
                                        <td class="text-center">
                                            
                                            <asp:HyperLink ID="hyplnkSmsLog_Basarili" runat="server" CssClass="btn btn-success" Visible='<%#Convert.ToBoolean(Eval("Durum")) %>' NavigateUrl='<%#Eval("SmsGonderimID", "~/Admin/MailIslemleri/SmsLog/{0}") %>' Target="_blank"><i class="fa fa-check"></i></asp:HyperLink>
      
                                            <asp:HyperLink ID="hyplnkSmsLog_Basarisiz" runat="server" CssClass="btn btn-danger" Visible='<%#!Convert.ToBoolean(Eval("Durum")) %>' NavigateUrl='<%#Eval("SmsGonderimID", "~/Admin/MailIslemleri/SmsLog/{0}") %>' Target="_blank"><i class="fa fa-times"></i></asp:HyperLink>
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

    <asp:SqlDataSource runat="server" ID="OleDbSmsGonderimListesi" ConnectionString='<%$ ConnectionStrings:ArcadiasDavetConnection %>' ProviderName='<%$ ConnectionStrings:ArcadiasDavetConnection.ProviderName %>' SelectCommand="SELECT SmsGonderimTablosu.*, KatilimciTablosu.AdSoyad, KatilimciTablosu.AnaKatilimciID, KatilimciTipiTablosu.KatilimciTipi, AnaDavetliTablosu.AdSoyad AS [AnaDavetli], GonderimTipiTablosu.GonderimTipi FROM ( ( ( ( KatilimciTablosu INNER JOIN KatilimciTipiTablosu ON KatilimciTablosu.KatilimciTipiID = KatilimciTipiTablosu.KatilimciTipiID ) LEFT JOIN ( SELECT * FROM KatilimciTablosu WHERE AnaKatilimciID IS NULL ) AS AnaDavetliTablosu ON KatilimciTablosu.AnaKatilimciID = AnaDavetliTablosu.KatilimciID ) INNER JOIN SmsGonderimTablosu ON KatilimciTablosu.KatilimciID = SmsGonderimTablosu.KatilimciID ) INNER JOIN SmsIcerikTablosu ON SmsIcerikTablosu.SmsIcerikID = SmsGonderimTablosu.SmsIcerikID ) INNER JOIN GonderimTipiTablosu ON GonderimTipiTablosu.GonderimTipiID = SmsIcerikTablosu.GonderimTipiID ORDER BY SmsGonderimTablosu.EklenmeTarihi DESC"></asp:SqlDataSource>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="Admin_AltSayfa" runat="server">
</asp:Content>