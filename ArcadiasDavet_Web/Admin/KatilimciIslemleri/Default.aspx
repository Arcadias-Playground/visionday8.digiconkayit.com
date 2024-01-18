<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="ArcadiasDavet_Web.Admin.KatilimciIslemleri.Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Admin_head" runat="server">
    <link rel="stylesheet" href='<%=ResolveClientUrl("~/css/dataTables.bootstrap4.min.css") %>' />

    <script type="text/javascript" src='<%=ResolveClientUrl("~/js/datatablescripts.bundle.js") %>'></script>
    <script type="text/javascript" src='<%=ResolveClientUrl("~/js/jquery.dataTables.i18n.min.js") %>'></script>

    <script type="text/javascript" src='<%=ResolveClientUrl("~/js/jquery.inputmask.min.js") %>'></script>

    <script>
        function pageLoad(sender, args) {
            $('#<%= txtTelefon.ClientID%>').inputmask('(599) 999 99 99', { onincomplete: () => { $('#<%= txtTelefon.ClientID%>').val(''); } })
        }

        $(document).ready(() => {
            DataTableKurulum(tbl_KatilimciListesi, true);
        });
    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="Admin_Icerik" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <div class="row">
        <div class="col-md-12">
            <div class="card">
                <div class="header">
                    <h2><strong>Katılımcı</strong> İşlemleri</h2>
                </div>
                <div class="body">
                    <asp:UpdatePanel ID="UPnlYeniKatilimciEkle" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="false" class="text-center">
                        <ContentTemplate>
                            <p>
                                <asp:LinkButton ID="lnkbtnYeniKatilimciEkle" runat="server" CssClass="btn btn-success" OnClick="lnkbtnYeniKatilimciEkle_Click">
                                   <i class="fa fa-plus"></i>&nbsp;Yeni Katılımcı Ekle
                                </asp:LinkButton>

                                <asp:HyperLink ID="hyplnkExcelSablon" runat="server" CssClass="btn btn-primary" NavigateUrl="~/Dosyalar/Excel/ArcadiasDavetSablon.xlsx">
                                    <i class="fa fa-file-excel"></i>&nbsp;Aktarım Şablonu ( Excel )
                                </asp:HyperLink>

                                <asp:HyperLink ID="hyplnkExcelAktarim" runat="server" CssClass="btn btn-danger" NavigateUrl="~/Dosyalar/Excel/ArcadiasDavetAktarim.zip">
                                    <i class="fa fa-area-chart"></i>&nbsp;Aktarım Programı ( Windows için )
                                </asp:HyperLink>
                            </p>
                        </ContentTemplate>
                    </asp:UpdatePanel>

                    <asp:UpdatePanel ID="UPnlKatilimciListesi" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="false">
                        <ContentTemplate>

                            <div class="table-responsive">
                                <asp:Panel ID="PnlArama" runat="server" DefaultButton="lnkbtnAra">
                                    <table class="AlseinTable mb-3">
                                        <tr>
                                            <td class="d-none">&nbsp;</td>
                                            <td class="d-none">Aranacak Kelime</td>
                                            <td>
                                                <asp:TextBox ID="txtAranacakKelime" runat="server" CssClass="form-control" placeholder="Ad, Soyad yada e-Posta"></asp:TextBox>
                                            </td>
                                            <td style="width: 100px;">
                                                <asp:LinkButton ID="lnkbtnAra" runat="server" CssClass="btn btn-primary" OnClick="lnkbtnAra_Click" OnClientClick="UyariBilgilendirme('', '<p>Listeniz, arama kriterine göre hazırlanıyor. Lütfen bekleyin</p>', false);">
                                           <i class="fa fa-search"></i>&nbsp;Ara
                                                </asp:LinkButton>
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>

                                <table id="tbl_KatilimciListesi" class="table table-bordered table-striped table-hover js-basic-example">
                                    <thead>
                                        <tr>
                                            <th>Katılımcı</th>
                                            <th>Katılımcı Tipi</th>
                                            <th style="width: 130px;" class="text-center">Yönetici Onayı</th>
                                            <th style="width: 130px;" class="text-center">Katılımcı Onayı</th>
                                            <th style="width: 300px;" class="text-center">İşlemler</th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                        <asp:Repeater ID="rptKatilimciListesi" runat="server" ClientIDMode="AutoID" DataSourceID="OleDbKatilimciListesi" OnItemCommand="rptKatilimciListesi_ItemCommand">
                                            <ItemTemplate>
                                                <tr>
                                                    <td>
                                                        <div>
                                                            <%#Eval("AdSoyad") %>
                                                            <asp:Panel ID="PnlePosta" runat="server" Visible='<%#!string.IsNullOrEmpty(Eval("ePosta").ToString()) %>' Font-Size="10px">
                                                                <b>e-Posta</b> : <%#Eval("ePosta").ToString() %>
                                                            </asp:Panel>
                                                            <asp:Panel ID="pnlTelefon" runat="server" Visible='<%#!string.IsNullOrEmpty(Eval("Telefon").ToString()) %>' Font-Size="10px">
                                                                <b>Telefon</b> : <%#Eval("Telefon").ToString() %>
                                                            </asp:Panel>
                                                            <asp:Panel ID="pnlKurum" runat="server" Visible='<%#!string.IsNullOrEmpty(Eval("Kurum").ToString()) %>' Font-Size="10px">
                                                                <b>Kurum</b> : <%#Eval("Kurum").ToString() %>
                                                            </asp:Panel>
                                                            <asp:Panel ID="pnlUnvan" runat="server" Visible='<%#!string.IsNullOrEmpty(Eval("Unvan").ToString()) %>' Font-Size="10px">
                                                                <b>Ünvan</b> : <%#Eval("Unvan").ToString() %>
                                                            </asp:Panel>
                                                        </div>
                                                        <asp:Panel ID="PnlAnaKatilimci" runat="server" Visible='<%#!Eval("AnaKatilimciID").Equals(DBNull.Value) %>'>
                                                            <span style="color: red">Ana Katılımcı</span> : <%#Eval("AnaDavetli") %>
                                                        </asp:Panel>
                                                    </td>
                                                    <td><%#Eval("KatilimciTipi") %></td>
                                                    <td>
                                                        <asp:Panel ID="PnlYoneticiOnayDurum" runat="server" CssClass="text-center" Visible='<%#!Eval("YoneticiOnayTarihi").Equals(DBNull.Value) %>'>
                                                            <p>
                                                                <%#Convert.ToBoolean(Eval("YoneticiOnay")) ? "<i style='color:darkseagreen' class='zmdi zmdi-check'></i>" : "<i style='color:red' class='zmdi zmdi-minus-circle'></i>" %>
                                                            </p>
                                                            <p>
                                                                <%#Eval("YoneticiOnayTarihi") %>
                                                            </p>
                                                        </asp:Panel>
                                                        <div>
                                                            <asp:LinkButton ID="lnkbtnYoneticiOnay" runat="server" CssClass="btn btn-primary" CommandArgument='<%#Eval("KatilimciID") %>' CommandName="YoneticiOnay" Visible='<%#!Convert.ToBoolean(Eval("YoneticiOnay")) || Eval("YoneticiOnayTarihi").Equals(DBNull.Value) %>'>
                                                                    <i class="fa fa-check"></i>&nbsp;Yönetici Olarak Onayla
                                                            </asp:LinkButton>
                                                            <asp:LinkButton ID="lnkbtnYoneticiOnayKaldir" runat="server" CssClass="btn btn-danger" CommandArgument='<%#Eval("KatilimciID") %>' CommandName="YoneticiOnayKaldir" Visible='<%#Convert.ToBoolean(Eval("YoneticiOnay")) %>' OnClientClick="return confirm('Yönentici onayını kaldırmak istediğinize emin misiniz?');">
                                                                    <i class="fa fa-times"></i>&nbsp;Yönetici Onayını Kaldır
                                                            </asp:LinkButton>
                                                        </div>
                                                    </td>
                                                    <td>
                                                        <asp:Panel ID="PnlKatilimciOnayDurum" runat="server" CssClass="text-center" Visible='<%#!Eval("KatilimciOnayTarihi").Equals(DBNull.Value) %>'>
                                                            <p>
                                                                <%#Convert.ToBoolean(Eval("KatilimciOnay")) ? "<i style='color:darkseagreen' class='zmdi zmdi-check'></i>" : "<i style='color:red' class='zmdi zmdi-minus-circle'></i>" %>
                                                            </p>
                                                            <p>
                                                                <%#Eval("KatilimciOnayTarihi") %>
                                                            </p>
                                                        </asp:Panel>
                                                        <div>
                                                            <asp:LinkButton ID="lnkbtnKatilimciOnay" runat="server" CssClass="btn btn-info" CommandArgument='<%#Eval("KatilimciID") %>' CommandName="KatilimciOnay" Visible='<%#!Convert.ToBoolean(Eval("KatilimciOnay")) || Eval("KatilimciOnayTarihi").Equals(DBNull.Value) %>'>
                                                                    <i class="fa fa-check"></i>&nbsp;Katılımcı Olarak Onayla
                                                            </asp:LinkButton>
                                                            <asp:LinkButton ID="lnkbtnKatilimciOnayKaldir" runat="server" CssClass="btn btn-warning" CommandArgument='<%#Eval("KatilimciID") %>' CommandName="KatilimciOnayKaldir" Visible='<%#Convert.ToBoolean(Eval("KatilimciOnay")) %>' OnClientClick="return confirm('Katılımcı onayını kaldırmak istediğinize emin misiniz?');">
                                                                    <i class="fa fa-times"></i>&nbsp;Katılımcı Onayını Kaldır
                                                            </asp:LinkButton>
                                                        </div>
                                                    </td>
                                                    <td class="text-center">
                                                        <div>
                                                            <asp:LinkButton ID="lnkbtnKatilimciGuncelle" runat="server" CssClass="btn btn-success" CommandArgument='<%#Eval("KatilimciID") %>' CommandName="Guncelle">
                                                                <i class="fa fa-sync"></i>&nbsp;Güncelle
                                                            </asp:LinkButton>

                                                            <asp:LinkButton ID="lnkbtnMisafirIslemleri" runat="server" CssClass="btn btn-danger" CommandArgument='<%#Eval("KatilimciID") %>' CommandName="MisafirEkle" Visible='<%#Eval("AnaKatilimciID").Equals(DBNull.Value) && Convert.ToBoolean(Eval("YoneticiOnay")) && Convert.ToBoolean(Eval("KatilimciOnay")) %>'>
                                                                <i class="fa fa-user"></i>&nbsp;Misafir Ekle
                                                            </asp:LinkButton>
                                                        </div>
                                                        <div>
                                                            <asp:LinkButton ID="lnkbtnMailGonderim" runat="server" CssClass="btn btn-primary" CommandArgument='<%#Eval("KatilimciID") %>' Visible='<%#Convert.ToBoolean(Eval("YoneticiOnay")) && !Eval("ePosta").Equals(DBNull.Value) && !string.IsNullOrEmpty(Eval("ePosta").ToString()) %>' OnClientClick="UyariBilgilendirme('', '<p>Katılımcıya mail gönderiliyor. Lütfen bekleyin</p>');" CommandName="MailGonderim">
                                                            <i class="fa fa-envelope"></i>&nbsp;Mail Gönder
                                                            </asp:LinkButton>

                                                            <asp:LinkButton ID="lnkbtnSmsGonderim" runat="server" CssClass="btn btn-info" CommandArgument='<%#Eval("KatilimciID") %>' Visible='<%#Convert.ToBoolean(Eval("YoneticiOnay")) && !Eval("Telefon").Equals(DBNull.Value) && !string.IsNullOrEmpty(Eval("Telefon").ToString()) %>' OnClientClick="UyariBilgilendirme('', '<p>Katılımcıya sms gönderiliyor. Lütfen bekleyin</p>');" CommandName="SmsGonderim">
                                                            <i class="fa fa-sms"></i>&nbsp;Sms Gönder
                                                            </asp:LinkButton>
                                                        </div>
                                                        <div>
                                                            <asp:LinkButton ID="lnkbtnYakaKarti" runat="server" CssClass="btn btn-warning" Visible='<%#Convert.ToBoolean(Eval("YoneticiOnay")) && Convert.ToBoolean(Eval("KatilimciOnay"))%>' CommandArgument='<%#Eval("KatilimciID") %>' CommandName="YakaKarti">
                                                                <i class="fa fa-print"></i>&nbsp;Yaka Kartı Bas
                                                            </asp:LinkButton>

                                                            <asp:LinkButton ID="lnkbtnUzakBaglantiYakaKarti" runat="server" CssClass="btn btn-info" CommandArgument='<%#Eval("KatilimciID") %>' Visible='<%#Convert.ToBoolean(Eval("YoneticiOnay")) && Convert.ToBoolean(Eval("KatilimciOnay")) %>' CommandName="UzakBaglantiYakaKarti">
                                                            <i class="fa fa-print"></i>&nbsp;&nbsp;Ortak Yazıcıdan Yaka Kartı Bas&nbsp;&nbsp;<i class="fa fa-info-circle"></i>
                                                            </asp:LinkButton>

                                                            <asp:LinkButton ID="lnkbtnGiris" runat="server" CssClass="btn btn-default" CommandArgument='<%#Eval("KatilimciID") %>' CommandName="GirisYap" Visible='<%#Convert.ToBoolean(Eval("YoneticiOnay")) && Convert.ToBoolean(Eval("KatilimciOnay"))%>'>
                                                            <i class="fa fa-sign-in-alt"></i>&nbsp;Giriş Yap
                                                            </asp:LinkButton>
                                                        </div>
                                                    </td>
                                                </tr>
                                            </ItemTemplate>
                                        </asp:Repeater>

                                    </tbody>
                                </table>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>

    <asp:SqlDataSource runat="server" ID="OleDbKatilimciListesi" ConnectionString='<%$ ConnectionStrings:ArcadiasDavetConnection %>' ProviderName='<%$ ConnectionStrings:ArcadiasDavetConnection.ProviderName %>' SelectCommand="SELECT KatilimciTablosu.*, KatilimciTipiTablosu.KatilimciTipi, AnaDavetliTablosu.AdSoyad AS [AnaDavetli] FROM ( KatilimciTablosu INNER JOIN KatilimciTipiTablosu ON KatilimciTablosu.KatilimciTipiID = KatilimciTipiTablosu.KatilimciTipiID ) LEFT JOIN ( SELECT * FROM KatilimciTablosu WHERE AnaKatilimciID IS NULL ) AS AnaDavetliTablosu ON KatilimciTablosu.AnaKatilimciID = AnaDavetliTablosu.KatilimciID WHERE KatilimciTablosu.AdSoyad LIKE '%' & ? & '%' OR KatilimciTablosu.ePosta LIKE '%' & ? & '%' OR AnaDavetliTablosu.AdSoyad LIKE '%' & ? & '%' ORDER BY KatilimciTablosu.AdSoyad">
        <SelectParameters>
            <asp:ControlParameter ControlID="txtAranacakKelime" PropertyName="Text" Name="?"></asp:ControlParameter>
            <asp:ControlParameter ControlID="txtAranacakKelime" PropertyName="Text" Name="?"></asp:ControlParameter>
            <asp:ControlParameter ControlID="txtAranacakKelime" PropertyName="Text" Name="?"></asp:ControlParameter>
        </SelectParameters>
    </asp:SqlDataSource>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="Admin_AltSayfa" runat="server">

    <asp:UpdatePanel ID="UPnlKatilimciGuncelleEkle" runat="server" UpdateMode="Conditional" class="modal fade" tabindex="-1" role="dialog" data-backdrop="static">
        <ContentTemplate>
            <div class="modal-dialog modal-lg" role="document">
                <div class="modal-content">
                    <div class="modal-header">
                        <h4 class="title">Katılımcı Ekle / Güncelle</h4>
                    </div>
                    <div class="modal-body">
                        <table class="AlseinTable">
                            <tr id="tr_KatilimciID" runat="server" visible="false">
                                <td>&nbsp;</td>
                                <td>Katılımcı ID</td>
                                <td>
                                    <asp:TextBox ID="txtKatilimciID" runat="server" CssClass="form-control" Enabled="false" ReadOnly="true"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>&nbsp;</td>
                                <td>Katılımcı Tipi</td>
                                <td>
                                    <asp:DropDownList ID="ddlKatilimciTipi" runat="server" CssClass="form-control" DataSourceID="OleDbKatilimciTipiListesi" DataTextField="KatilimciTipi" DataValueField="KatilimciTipiID"></asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td>*</td>
                                <td>Ad & Soyad</td>
                                <td>
                                    <asp:TextBox ID="txtAdSoyad" runat="server" CssClass="form-control"></asp:TextBox>
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
                                <td>Telefon</td>
                                <td>
                                    <asp:TextBox ID="txtTelefon" runat="server" CssClass="form-control"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>&nbsp;</td>
                                <td>Ünvan</td>
                                <td>
                                    <asp:TextBox ID="txtUnvan" runat="server" CssClass="form-control"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>&nbsp;</td>
                                <td>Kurum</td>
                                <td>
                                    <asp:TextBox ID="txtKurum" runat="server" CssClass="form-control"></asp:TextBox>
                                </td>
                            </tr>
                            <tr id="tr_AnaKatilimciID" runat="server" visible="false">
                                <td>&nbsp;</td>
                                <td>Ana Katılımcı ID</td>
                                <td>
                                    <asp:TextBox ID="txtAnaKatilimciID" runat="server" CssClass="form-control" Enabled="false" ReadOnly="true"></asp:TextBox>
                                </td>
                            </tr>
                            <tr id="tr_AnaKatilimciAdSoyad" runat="server" visible="false">
                                <td>&nbsp;</td>
                                <td>Ana Katılımcı</td>
                                <td>
                                    <asp:TextBox ID="txtAnaKatilimci" runat="server" CssClass="form-control" Enabled="false" ReadOnly="true"></asp:TextBox>
                                </td>
                            </tr>
                        </table>
                        <p align="center">
                            <asp:LinkButton ID="lnkbtnKatilimciEkleGuncelle" runat="server" CssClass="btn btn-success" OnClientClick="$(this).css('display', 'none'); $(Yukleniyor).css('display', 'inline-block')" OnClick="lnkbtnKatilimciEkleGuncelle_Click">
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
        </ContentTemplate>
    </asp:UpdatePanel>

    <asp:UpdatePanel ID="UPnlYakaKartiBasimi" runat="server" UpdateMode="Conditional" class="modal fade" tabindex="-1" role="dialog" data-backdrop="static">
        <ContentTemplate>
            <div class="modal-dialog modal-lg modal-dialog-centered" role="document">
                <div class="modal-content">
                    <div class="modal-header">
                        <h4 class="title">Yaka Kartı Basımı</h4>
                    </div>
                    <div class="modal-body">
                        <table class="table table-bordered">
                            <thead>
                                <tr>
                                    <th>Yazıcı</th>
                                    <th>İşlemler</th>
                                </tr>
                            </thead>
                            <tbody>
                                <asp:Repeater ID="rptYaziciListesi" runat="server" ClientIDMode="AutoID" OnItemCommand="rptYaziciListesi_ItemCommand">
                                    <ItemTemplate>
                                        <tr>
                                            <td>
                                                <%#Eval("ePosta") %>
                                            </td>
                                            <td class="text-center">
                                                <asp:Panel ID="PnlOffline" runat="server" Visible='<%#!Convert.ToBoolean(Eval("Durum")) %>'>
                                                    <span style="color:red">
                                                        Offline
                                                    </span>
                                                </asp:Panel>

                                                <asp:Panel ID="PnlOnline" runat="server" Visible='<%#Convert.ToBoolean(Eval("Durum")) %>'>
                                                    <asp:LinkButton ID="lnkbtnYaziciBasim" runat="server" CssClass="btn btn-success" CommandArgument='<%# $"{Eval("ePosta")}" %>'>
                                                        <i class="fa fa-print"></i>&nbsp;Yaka Kartını Bas
                                                    </asp:LinkButton>
                                                </asp:Panel>
                                            </td>
                                        </tr>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </tbody>
                        </table>
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-danger waves-effect" data-dismiss="modal">Kapat</button>
                    </div>
                </div>
            </div>

            <asp:HiddenField ID="hfKatilimciID" runat="server" Visible="false"/>
        </ContentTemplate>
    </asp:UpdatePanel>

    <asp:SqlDataSource runat="server" ID="OleDbKatilimciTipiListesi" ConnectionString='<%$ ConnectionStrings:ArcadiasDavetConnection %>' ProviderName='<%$ ConnectionStrings:ArcadiasDavetConnection.ProviderName %>' SelectCommand="SELECT [KatilimciTipiID], [KatilimciTipi] FROM [KatilimciTipiTablosu]"></asp:SqlDataSource>
</asp:Content>
