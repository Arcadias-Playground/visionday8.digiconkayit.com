<%@ Page Title="" Language="C#" MasterPageFile="~/Temel.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="ArcadiasDavet_Admin.Yonetim.Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Temel_head" runat="server">
    <link rel="stylesheet" href='<%=ResolveClientUrl("~/css/dataTables.bootstrap4.min.css") %>' />
    <link rel="stylesheet" href='<%=ResolveClientUrl("~/css/bootstrap-material-datetimepicker.css") %>' />

    <script type="text/javascript" src='<%=ResolveClientUrl("~/js/datatablescripts.bundle.js") %>'></script>
    <script type="text/javascript" src='<%=ResolveClientUrl("~/js/jquery.dataTables.i18n.min.js") %>'></script>
    <script type="text/javascript" src='<%=ResolveClientUrl("~/js/moment-with-locales.min.js") %>'></script>
    <script type="text/javascript" src='<%=ResolveClientUrl("~/js/bootstrap-material-datetimepicker.js") %>' charset="windows-1254"></script>

    <script>
        let countDownInterval;


        function pageLoad(sender, args) {
            if (!args._isPartialLoad) {
                DataTableKurulum(tbl_BildiriSistemiListesi, true);
            }

            var txtKapanmaTarihi = $(<%= txtKapansTarihi.ClientID%>);
            txtKapanmaTarihi.prop('readonly', 'true');
            txtKapanmaTarihi.bootstrapMaterialDatePicker({
                lang: 'tr',
                format: "DD.MM.YYYY",
                clearButton: true,
                weekStart: 1,
                time: false,
            });

            if (countDownInterval) {
                clearInterval(countDownInterval);
                countDownInterval = undefined;
            }
        }



        const countDown = () => {
            const startTimestamp = moment().startOf("day");
            countDownInterval = setInterval(function () {
                startTimestamp.add(1, 'second');
                document.getElementById('div_Sayac').innerHTML =
                    startTimestamp.format('HH:mm:ss');
            }, 1000);
        }


        const BildiriSilOnay = () => {
            if (confirm('DİKKAT !!! SİLME İŞLEMİ GERİ ALINAMAZ. Silme işlemine devam etmek istiyor musunuz?')) {
                UyariBilgilendirme('Bildiri Sistemi Silme', '<p class="text-center">Dijital davetiye sistemi siliniyor. Lütfen bekleyin</p><div class="text-center"><div id="div_Sayac"></div><div><img src="<%= ResolveClientUrl("~/Gorseller/loadspinner.gif")%>" style="width:47px;"/></div></div>');
                countDown();
                return true;
            }
            else {
                return false;
            }
        }
    </script>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="Temel_Icerik" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" AsyncPostBackTimeout="600"></asp:ScriptManager>

    <div class="row clearfix">
        <div class="col-md-12">
            <div class="card">
                <div class="header">
                    <h2><strong>Dijital Davetiye</strong> Sistemleri Listesi</h2>
                </div>
                <div class="body">
                    <asp:UpdatePanel ID="UPnlDijitalDavetiyeSistemiListesi" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="false">
                        <ContentTemplate>
                            <p class="text-center">
                                <asp:LinkButton ID="lnkbtnDijitalDavetiyeSistemiEkle" runat="server" CssClass="btn btn-success" OnClick="lnkbtnDijitalDavetiyeSistemiEkle_Click">
                                    <i class="zmdi zmdi-plus"></i>&nbsp;Yeni Dijital Davetiye Sistemi Ekle
                                </asp:LinkButton>
                            </p>


                            <table id="tbl_DijitalDavetiyeSistemiListesi" class="table table-bordered table-striped table-hover js-basic-example dataTable">
                                <thead>
                                    <tr>
                                        <th>Sistem Adı</th>
                                        <th>Kapanma Tarihi</th>
                                        <th>Açılma Tarihi</th>
                                        <th>İşlemler</th>
                                    </tr>
                                </thead>
                                <tbody>
                                    <asp:Repeater ID="rptDijitalDavetiyeSistemiListesi" runat="server" ClientIDMode="AutoID" OnItemCommand="rptDijitalDavetiyeSistemiListesi_ItemCommand" DataSourceID="OleDbDijitalDavetiyeSistemiListesi">
                                        <ItemTemplate>
                                            <tr>
                                                <td><%#Eval("Kongre") %></td>
                                                <td><%#Eval("KapanisTarihi").Equals(DBNull.Value) ? string.Empty :  Convert.ToDateTime(Eval("KapanisTarihi")).ToShortDateString() %></td>
                                                <td><%#Eval("EklenmeTarihi") %></td>
                                                <td>
                                                    <asp:LinkButton ID="lnkbtnGuncelle" runat="server" CssClass="btn btn-success" CommandName="Guncelle" CommandArgument='<%#Eval("KongreID") %>'>
                                                            <i class="zmdi zmdi-refresh-sync"></i>&nbsp;Güncelle
                                                    </asp:LinkButton>

                                                    <asp:LinkButton ID="lnkbtnSil" runat="server" CssClass="btn btn-danger" CommandName="Sil" CommandArgument='<%#Eval("KongreID") %>' OnClientClick="return BildiriSilOnay();">
                                                         <i class="zmdi zmdi-delete"></i>&nbsp;Sil
                                                    </asp:LinkButton>

                                                    <asp:HyperLink ID="hyplnkDijitalDavetiyeSistemiYonlendirme" runat="server" CssClass="btn btn-primary" NavigateUrl='<%#Eval("WebUrl", "~/{0}") %>' Target="_blank">
                                                        <i class="zmdi zmdi-view-web"></i>&nbsp;Siteye Git
                                                    </asp:HyperLink>
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
    </div>

    <asp:SqlDataSource runat="server" ID="OleDbDijitalDavetiyeSistemiListesi" ConnectionString='<%$ ConnectionStrings:digitalDavetiyeAdmin %>' ProviderName='<%$ ConnectionStrings:digitalDavetiyeAdmin.ProviderName %>' SelectCommand="SELECT * FROM KongreTablosu"></asp:SqlDataSource>
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="Temel_AltSayfa" runat="server">
    <asp:UpdatePanel ID="UPnlDijitalDavetiyeSistemiEkleGuncelle" runat="server" UpdateMode="Conditional" class="modal fade" tabindex="-1" role="dialog">
        <ContentTemplate>
            <div class="modal-dialog modal-lg" role="document">
                <div class="modal-content">
                    <div class="modal-header">
                        <h4 class="title">Dijital Davetiye Sistemi Ekle / Güncelle</h4>
                    </div>
                    <div class="modal-body">
                        <table class="AlseinTable">
                            <tr id="tr_KongreID" runat="server" visible="false">
                                <td>&nbsp;</td>
                                <td>Kongre ID</td>
                                <td>
                                    <asp:TextBox ID="txtKongreID" runat="server" CssClass="form-control" Enabled="false" ReadOnly="true"></asp:TextBox>
                                </td>
                            </tr>
                             <tr>
                                <td>&nbsp;</td>
                                <td>Dijital Davetiye Sistemi Adı</td>
                                <td>
                                    <asp:TextBox ID="txtKongre" runat="server" CssClass="form-control"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>&nbsp;</td>
                                <td>Web URL</td>
                                <td>
                                    <div class="input-group">
                                        <div class="input-group-prepend">
                                            <span class="input-group-text"><%=$"{Request.Url.Scheme}://{Request.Url.Authority}"%></span>
                                        </div>
                                        <asp:TextBox ID="txtWebUrl" runat="server" CssClass="form-control"></asp:TextBox>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td>&nbsp;</td>
                                <td>Kapanma Tarihi</td>
                                <td>
                                    <asp:TextBox ID="txtKapansTarihi" runat="server" CssClass="form-control" placeholder="Tarih seçmek için tıklayınız"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>&nbsp;</td>
                                <td>Kapanış Hatırlatma</td>
                                <td>
                                    <asp:CheckBox ID="chkboxKapanisHatirlatma" runat="server" CssClass="form-control" Text="Kapanış tarihini mail ile hatırlat" />
                                </td>
                            </tr>
                        </table>
                        <p align="center">
                            <asp:LinkButton ID="lnkbtnDijitalDavetiyeSistemiEkleGuncelle" runat="server" CssClass="btn btn-success" OnClientClick="$(this).css('display', 'none'); $(Yukleniyor).css('display', 'inline-block')" OnClick="lnkbtnDijitalDavetiyeSistemiEkleGuncelle_Click">
                                <i class="zmdi zmdi-plus"></i>&nbsp;Ekle / Güncelle&nbsp;<i class="zmdi zmdi-refresh"></i>
                            </asp:LinkButton>
                            <asp:Image ID="Yukleniyor" runat="server" ImageUrl="~/Gorseller/loadspinner.gif" ClientIDMode="Static" Style="display: none; width: 47px" />
                        </p>
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-danger waves-effect" data-dismiss="modal" id="UyariKapatButon">Kapat</button>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>