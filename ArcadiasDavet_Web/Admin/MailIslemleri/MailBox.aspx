<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.master" AutoEventWireup="true" CodeBehind="MailBox.aspx.cs" Inherits="ArcadiasDavet_Web.Admin.MailIslemleri.MailBox" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Admin_head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="Admin_Icerik" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <div class="row">
        <asp:UpdatePanel ID="UPnlMailBoxListesi" runat="server" class="col-md-12" ChildrenAsTriggers="false" UpdateMode="Conditional">
            <ContentTemplate>
                <table id="tbl_MailBox" class="table table-bordered table-striped table-hover js-basic-example">
                <thead>
                    <tr>
                        <th class="text-center" style="width:50px;">Durum</th>
                        <th>Gönderen</th>
                        <th>Konu</th>
                        <th class="text-center">Son Okuma Bilgileri</th>
                        <th class="text-center" style="width:50px;">İşlemler</th>
                    </tr>
                </thead>
                <tbody>
                    <asp:Repeater ID="rptMailBoxListesi" runat="server" ClientIDMode="AutoID" DataSourceID="OleDbWebMailListesi" OnItemCommand="rptMailBoxListesi_ItemCommand">
                        <ItemTemplate>
                            <tr>
                                <td style="vertical-align:middle; text-align:center;"><%#Eval("MailGorulmeTarihi").Equals(DBNull.Value) ? "<i class='fa fa-circle' style='color:cornflowerblue'></i>" : "<i class='fa fa-check' style='color:darkseagreen'></i>" %></td>
                                <td><%#Eval("GonderenMail") %></td>
                                <td><p><%#Eval("Konu") %></p><div><%#Eval("WebMailTarih") %></div></td>
                                <td class="text-center">
                                    <div>
                                        <%#Eval("AdSoyad") %>
                                    </div>
                                    <div>
                                        <%#Eval("MailGorulmeTarihi") %>
                                    </div>
                                </td>
                                <td style="vertical-align:middle; text-align:center;">
                                    <asp:LinkButton ID="lnkbtnMailOku" runat="server" CssClass="btn btn-success" CommandArgument='<%#Eval("WebMailID") %>' CommandName="WebMailOku">
                                        <i class="fa fa-envelope-circle-check"></i>
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



     <asp:SqlDataSource runat="server" ID="OleDbWebMailListesi" ConnectionString='<%$ ConnectionStrings:ArcadiasDavetConnection %>' ProviderName='<%$ ConnectionStrings:ArcadiasDavetConnection.ProviderName %>' SelectCommand="SELECT WebMailTablosu.*, KullaniciTablosu.AdSoyad FROM WebMailTablosu LEFT JOIN KullaniciTablosu ON WebMailTablosu.KullaniciID = KullaniciTablosu.KullaniciID ORDER BY WebMailTarih DESC"></asp:SqlDataSource>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="Admin_AltSayfa" runat="server">
    <asp:UpdatePanel ID="UPnlWebMailDetay" runat="server" UpdateMode="Conditional" class="modal fade" tabindex="-1" role="dialog" data-backdrop="static">
        <ContentTemplate>
            <div class="modal-dialog modal-lg" role="document">
                <div class="modal-content">
                    <div class="modal-header">
                        <h4 class="title" id="h4_GonderenMail" runat="server"></h4>
                    </div>
                    <div class="modal-body">
                        <table class="AlseinTable">
                            <tr id="tr_KatilimciID" runat="server" visible="false">
                                <td>&nbsp;</td>
                                <td>Konu</td>
                                <td>
                                    <asp:Label ID="lblKonu" runat="server" Text=""></asp:Label>
                                </td>
                            </tr>
                        </table>
                        <div id="div_WebMailIcerik" runat="server" clientid="static"></div>
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-danger waves-effect" data-dismiss="modal" onclick="$('#div_WebMailIcerik').html('')">Kapat</button>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>