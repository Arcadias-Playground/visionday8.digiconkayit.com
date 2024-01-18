using ExcelDataReader;
using Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using VeritabaniIslemMerkezi;

namespace ArcadiasDavet_MaillingApp
{
    public partial class Form1 : Form
    {
        StringBuilder Uyarilar;
        BilgiKontrolMerkezi Kontrol;

        SurecBilgiModel SModel;
        SurecVeriModel<IList<KatilimciTipiTablosuModel>> SDataListModel;

        DataSet result;

        List<AktarimTipiModel> AktarimTipiList = new List<AktarimTipiModel>
        {
            new AktarimTipiModel
            {
                Baslik = "Yeni Liste Aktarımı",
                YeniKayitDurum = false,
            },
            new AktarimTipiModel
            {
                Baslik = "Hatırlatma Mail Gönderimi",
                YeniKayitDurum = true
            }
        };

        bool KatilimciOnayDurum;
        string KatilimciTipiID, LogFileName, ApiAdresi, MailGonderimAdresi;

        int BasariliGonderim = 0, BasarisizGonderim = 0, HataliKayit = 0;

        private void cBoxAktarımTipi_SelectedValueChanged(object sender, EventArgs e)
        {
            if ((cBoxAktarımTipi.SelectedItem as AktarimTipiModel).YeniKayitDurum)
            {
                lblKatilimciTipi.Visible = false;
                cBoxKatilimciTipi.Visible = false;
                lblKatilimciOnay.Visible = false;
                chkKatilimciOnay.Visible = false;
            }
            else
            {
                lblKatilimciTipi.Visible = true;
                cBoxKatilimciTipi.Visible = true;
                lblKatilimciOnay.Visible = true;
                chkKatilimciOnay.Visible = true;
            }

            chkePostaGonderimIstek.Checked = false;
            chkSmsGonderimIstek.Checked = false;
        }

        public Form1()
        {
            InitializeComponent();

            System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
            System.Diagnostics.FileVersionInfo fvi = System.Diagnostics.FileVersionInfo.GetVersionInfo(assembly.Location);

            Text = $"Arcadias Davet - v{fvi.FileVersion}";

            cBoxAktarımTipi.DataSource = AktarimTipiList;
            cBoxAktarımTipi.DisplayMember = "Baslik";
            cBoxAktarımTipi.ValueMember = "YeniKayitDurum";
        }

        private void btnApiAdresiOnay_Click(object sender, EventArgs e)
        {
            Uyarilar = new StringBuilder();
            Kontrol = new BilgiKontrolMerkezi();

            Kontrol.URLKontrol(txtApiAdresi.Text, "Api adresi boş bırakılamaz", "Geçersiz api adresi girildi", ref Uyarilar, "- ", "\r\n");

            if (string.IsNullOrEmpty(Uyarilar.ToString()))
            {
                try
                {
                    ApiAdresi = $"{txtApiAdresi.Text}/Api/Katilimci";
                    MailGonderimAdresi = $"{txtApiAdresi.Text}/Api/MailGonderim?KatilimciID={{0}}&ePostaGonderimIstek={{1}}&SmsGonderimIstek={{2}}";
                    using (HttpClient client = new HttpClient())
                    {
                        using (HttpResponseMessage response = client.GetAsync(ApiAdresi).GetAwaiter().GetResult())
                        {
                            if (response.StatusCode.Equals(HttpStatusCode.OK))
                            {
                                SDataListModel = JsonConvert.DeserializeObject<SurecVeriModel<IList<KatilimciTipiTablosuModel>>>(response.Content.ReadAsStringAsync().GetAwaiter().GetResult());

                                if (SDataListModel.Sonuc.Equals(Sonuclar.Basarili))
                                {
                                    cBoxKatilimciTipi.DataSource = SDataListModel.Veriler;
                                    cBoxKatilimciTipi.ValueMember = "KatilimciTipiID";
                                    cBoxKatilimciTipi.DisplayMember = "KatilimciTipi";

                                    chkKatilimciOnay.Checked = false;
                                    chkePostaGonderimIstek.Checked = false;
                                    chkSmsGonderimIstek.Checked = false;

                                    gBoxApiAdresi.Enabled = false;
                                    gBoxKatilimciOzellikleri.Enabled = true;

                                    MessageBox.Show("Api adresi doğrulandı. Katılımcı tipleriniz listelendi.");
                                }
                                else
                                {
                                    MessageBox.Show($"Api bağlantısından geçersiz işlem yanıtı geldi.\r\n\\nHata mesajı : {SDataListModel.HataBilgi.HataMesaji}");
                                }
                            }
                            else
                            {
                                MessageBox.Show($"Api bağlantısı sırasında hata meydana gedi.\r\n\r\nDurumKodu : {response.StatusCode}");
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Api bağlantısı sırasında hata meydana gedi.\r\n\r\nHata mesajı  : {ex.Message}");
                }
            }
            else
            {
                MessageBox.Show(Uyarilar.ToString());
            }
        }

        private async void btnListeAl_Click(object sender, EventArgs e)
        {
            if (oFDExcel.ShowDialog().Equals(DialogResult.OK))
            {
                try
                {
                    using (var stream = File.Open(oFDExcel.FileName, FileMode.Open, FileAccess.Read))
                    {
                        using (var reader = ExcelReaderFactory.CreateReader(stream))
                        {
                            result = reader.AsDataSet(new ExcelDataSetConfiguration()
                            {
                                ConfigureDataTable = (tableReader) => new ExcelDataTableConfiguration
                                {
                                    UseHeaderRow = true
                                }
                            });
                        }
                    }

                    KatilimciTipiID = cBoxKatilimciTipi.SelectedValue.ToString();
                    KatilimciOnayDurum = chkKatilimciOnay.Checked;

                    lblToplamKatilimciSayisi.Text = result.Tables[0].Rows.Count.ToString();

                    BasariliGonderim = 0;
                    BasarisizGonderim = 0;
                    HataliKayit = 0;

                    lblBasariliGonderim.Text = BasariliGonderim.ToString();
                    lblBasarisizGonderim.Text = BasarisizGonderim.ToString();
                    lblHataliKayit.Text = HataliKayit.ToString();

                    gBoxKatilimciOzellikleri.Enabled = false;
                    gBoxAktarimBilgileri.Enabled = true;

                    LogFileName = $"{DateTime.Now:yyyyMMdd_HHmmss}.log";

                    if ((cBoxAktarımTipi.SelectedItem as AktarimTipiModel).YeniKayitDurum)
                        await Task.Run(new Action(Davetiye_QR_GonderimBasla));
                    else
                        await Task.Run(new Action(MaillingBasla));


                    gBoxKatilimciOzellikleri.Enabled = true;
                    gBoxAktarimBilgileri.Enabled = false;

                    lblToplamKatilimciSayisi.Text = 0.ToString();
                    lblAktarimSatirSayisi.Text = 0.ToString();
                    lblAktarimYapilanKisiBilgisi.Text = "Altay Serhat İnan ( serhat@arkadyas.com )";


                    MessageBox.Show($"{result.Tables[0].Rows.Count} kadar kişi sayısı aktarımı tamamlandı. Detaylı bilgi için log'a bakınız.\r\n\r\nLog dosya yolu : \r\n\r\n{Path.Combine(Application.StartupPath, "Log", LogFileName)}");
                }
                catch (Exception ex)
                {
                    gBoxKatilimciOzellikleri.Enabled = true;
                    gBoxAktarimBilgileri.Enabled = false;
                    MessageBox.Show($"Excel dosyanız, excel tarafından açık olabilir. Lütfen Excel programını kapatıp bir daha deneyiniz.\r\n\r\nDetaylı hata mesajı : {ex.Message}");
                }
            }
            else
            {
                MessageBox.Show("Dosya seçimi iptal edildi.");
            }
        }

        void MaillingBasla()
        {
            for (int i = 0; i < result.Tables[0].Rows.Count; i++)
            {
                lblAktarimSatirSayisi.Invoke((MethodInvoker)delegate
                {
                    lblAktarimSatirSayisi.Text = (i + 1).ToString();
                });

                lblAktarimYapilanKisiBilgisi.Invoke((MethodInvoker)delegate
                {
                    lblAktarimYapilanKisiBilgisi.Text = $"{result.Tables[0].Rows[i][0]} ( {result.Tables[0].Rows[i][1]} )";
                });

                File.AppendAllText(Path.Combine(Application.StartupPath, "Log", LogFileName), $"{i + 1}. satır ==> {result.Tables[0].Rows[i][0]} ( {result.Tables[0].Rows[i][1]} )\r\n");
                File.AppendAllText(Path.Combine(Application.StartupPath, "Log", LogFileName), $"{DateTime.Now:dd.MM.yyyy HH:mm:ss} ==> Bilgi kontrolüne başlanıyor.\r\n");

                Uyarilar = new StringBuilder();

                KatilimciTablosuModel KModel = new KatilimciTablosuModel
                {
                    KatilimciID = null,
                    KatilimciTipiID = KatilimciTipiID,
                    AdSoyad = Kontrol.KelimeKontrol(result.Tables[0].Rows[i][0].ToString(), "Ad & Soyad boş bırakılamaz", ref Uyarilar, $"{DateTime.Now:dd.MM.yyyy HH:mm:ss} ==> ", "\r\n"),
                    ePosta = string.IsNullOrEmpty(result.Tables[0].Rows[i][1].ToString()) ? string.Empty : Kontrol.ePostaKontrol(result.Tables[0].Rows[i][1].ToString(), "e-Posta boş bırakılamaz", "Geçersiz e-Posta adresi girildi", ref Uyarilar, $"{DateTime.Now:dd.MM.yyyy HH:mm:ss} ==> ", "\r\n"),
                    Telefon = string.IsNullOrEmpty(result.Tables[0].Rows[i][2].ToString()) ? string.Empty : Kontrol.TelefonKontrol(result.Tables[0].Rows[i][2].ToString(), "{DateTime.Now:dd.MM.yyyy HH:mm:ss} ==> Telefon boş bırakılamaz", "Geçersiz telefon girildi\r\n", ref Uyarilar),
                    Unvan = result.Tables[0].Rows[i][3].ToString(),
                    Kurum = result.Tables[0].Rows[i][4].ToString(),
                    YoneticiOnay = true,
                    YoneticiOnayTarihi = Kontrol.Simdi(),
                    KatilimciOnay = KatilimciOnayDurum,
                    KatilimciOnayTarihi = KatilimciOnayDurum ? new DateTime?(Kontrol.Simdi()) : null,
                    GuncellenmeTarihi = Kontrol.Simdi(),
                    EklenmeTarihi = Kontrol.Simdi(),

                    ePostaGonderimIstek = chkePostaGonderimIstek.Checked,
                    SmsGonderimIstek = chkSmsGonderimIstek.Checked
                };

                if (string.IsNullOrEmpty(Uyarilar.ToString()))
                {
                    File.AppendAllText(Path.Combine(Application.StartupPath, "Log", LogFileName), $"{DateTime.Now:dd.MM.yyyy HH:mm:ss} ==> Katılımcı aktarılıyor. ==> {KModel.AdSoyad} ( {KModel.ePosta} )\r\n");

                    try
                    {
                        using (HttpClient hc = new HttpClient())
                        {
                            using (HttpResponseMessage response = hc.PostAsync(ApiAdresi, new StringContent(JsonConvert.SerializeObject(KModel), Encoding.UTF8, "application/json")).GetAwaiter().GetResult())
                            {
                                if (response.StatusCode.Equals(HttpStatusCode.OK))
                                {
                                    SModel = JsonConvert.DeserializeObject<SurecBilgiModel>(response.Content.ReadAsStringAsync().GetAwaiter().GetResult());

                                    if (SModel.Sonuc.Equals(Sonuclar.Basarili))
                                    {
                                        File.AppendAllText(Path.Combine(Application.StartupPath, "Log", LogFileName), $"{DateTime.Now:dd.MM.yyyy HH:mm:ss} ==> Katılımcı kaydı tamamlandı\r\n");

                                        lblBasariliGonderim.Invoke((MethodInvoker)delegate
                                        {
                                            BasariliGonderim++;
                                            lblBasariliGonderim.Text = BasariliGonderim.ToString();
                                        });
                                    }
                                    else
                                    {
                                        File.AppendAllText(Path.Combine(Application.StartupPath, "Log", LogFileName), $"{DateTime.Now:dd.MM.yyyy HH:mm:ss} ==> Katılımcı kaydı sırasında hata meydana geldi. Hata mesajı : {SModel.HataBilgi.HataMesaji}\r\n");

                                        lblBasarisizGonderim.Invoke((MethodInvoker)delegate
                                        {
                                            BasarisizGonderim++;
                                            lblBasarisizGonderim.Text = BasarisizGonderim.ToString();
                                        });
                                    }
                                }
                                else
                                {
                                    File.AppendAllText(Path.Combine(Application.StartupPath, "Log", LogFileName), $"{DateTime.Now:dd.MM.yyyy HH:mm:ss} ==> Aktarım sırasında api hatası meydana geldi.\r\n");
                                    File.AppendAllText(Path.Combine(Application.StartupPath, "Log", LogFileName), $"{DateTime.Now:dd.MM.yyyy HH:mm:ss} ==> Durum Kodu : {response.StatusCode}\r\n");
                                    File.AppendAllText(Path.Combine(Application.StartupPath, "Log", LogFileName), $"{DateTime.Now:dd.MM.yyyy HH:mm:ss} ==> Hata mesajı : {response.ReasonPhrase}\r\n");
                                    lblBasarisizGonderim.Invoke((MethodInvoker)delegate
                                    {
                                        BasarisizGonderim++;
                                        lblBasarisizGonderim.Text = BasarisizGonderim.ToString();
                                    });
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        File.AppendAllText(Path.Combine(Application.StartupPath, "Log", LogFileName), $"{DateTime.Now:dd.MM.yyyy HH:mm:ss} ==> Aktarım sırasında api hatası meydana geldi.\r\n");
                        File.AppendAllText(Path.Combine(Application.StartupPath, "Log", LogFileName), $"{DateTime.Now:dd.MM.yyyy HH:mm:ss} ==> Hata mesajı : {ex.Message}\r\n");
                        lblBasarisizGonderim.Invoke((MethodInvoker)delegate
                        {
                            BasarisizGonderim++;
                            lblBasarisizGonderim.Text = BasarisizGonderim.ToString();
                        });
                    }
                }
                else
                {
                    HataliKayit++;
                    lblHataliKayit.Invoke((MethodInvoker)delegate
                    {
                        lblHataliKayit.Text = HataliKayit.ToString();
                    });
                    File.AppendAllText(Path.Combine(Application.StartupPath, "Log", LogFileName), $"Hatalı alanlar mevcut.\r\n{Uyarilar}");
                }

                File.AppendAllText(Path.Combine(Application.StartupPath, "Log", LogFileName), "\r\n\r\n\r\n");

                System.Threading.Thread.Sleep(new Random().Next(12000, 25000));
            }
        }

        void Davetiye_QR_GonderimBasla()
        {

            for (int i = 0; i < result.Tables[0].Rows.Count; i++)
            {
                string KatilimciID = result.Tables[0].Rows[i][0].ToString();

                lblAktarimSatirSayisi.Invoke((MethodInvoker)delegate
                {
                    lblAktarimSatirSayisi.Text = (i + 1).ToString();
                });

                lblAktarimYapilanKisiBilgisi.Invoke((MethodInvoker)delegate
                {
                    lblAktarimYapilanKisiBilgisi.Text = $"{KatilimciID}";
                });

                File.AppendAllText(Path.Combine(Application.StartupPath, "Log", LogFileName), $"{i + 1}. satır ==> {KatilimciID}\r\n");
                File.AppendAllText(Path.Combine(Application.StartupPath, "Log", LogFileName), $"{DateTime.Now:dd.MM.yyyy HH:mm:ss} ==> Bilgi kontrolüne başlanıyor.\r\n");

                Uyarilar = new StringBuilder();

                File.AppendAllText(Path.Combine(Application.StartupPath, "Log", LogFileName), $"{DateTime.Now:dd.MM.yyyy HH:mm:ss} ==> Mail gönderiliyor. ==> {KatilimciID}\r\n");
                try
                {
                    using (HttpClient hc = new HttpClient())
                    {
                        using (HttpResponseMessage response = hc.GetAsync(MailGonderimAdresi.Replace("{0}", KatilimciID).Replace("{1}", chkePostaGonderimIstek.Checked.ToString().ToLower()).Replace("{2}", chkSmsGonderimIstek.Checked.ToString().ToLower())).GetAwaiter().GetResult())
                        {
                            if (response.StatusCode.Equals(HttpStatusCode.OK))
                            {
                                SModel = JsonConvert.DeserializeObject<SurecBilgiModel>(response.Content.ReadAsStringAsync().GetAwaiter().GetResult());

                                if (SModel.Sonuc.Equals(Sonuclar.Basarili))
                                {
                                    File.AppendAllText(Path.Combine(Application.StartupPath, "Log", LogFileName), $"{DateTime.Now:dd.MM.yyyy HH:mm:ss} ==> Mail gönderimi tamamlandı\r\n");

                                    lblBasariliGonderim.Invoke((MethodInvoker)delegate
                                    {
                                        BasariliGonderim++;
                                        lblBasariliGonderim.Text = BasariliGonderim.ToString();
                                    });
                                }
                                else
                                {
                                    File.AppendAllText(Path.Combine(Application.StartupPath, "Log", LogFileName), $"{DateTime.Now:dd.MM.yyyy HH:mm:ss} ==> Mail gönderimi sırasında hata meydana geldi. Hata mesajı : {SModel.HataBilgi.HataMesaji}\r\n");

                                    lblBasarisizGonderim.Invoke((MethodInvoker)delegate
                                    {
                                        BasarisizGonderim++;
                                        lblBasarisizGonderim.Text = BasarisizGonderim.ToString();
                                    });
                                }
                            }
                            else
                            {
                                File.AppendAllText(Path.Combine(Application.StartupPath, "Log", LogFileName), $"{DateTime.Now:dd.MM.yyyy HH:mm:ss} ==>  Mail gönderimi sırasında api hatası meydana geldi.\r\n");
                                File.AppendAllText(Path.Combine(Application.StartupPath, "Log", LogFileName), $"{DateTime.Now:dd.MM.yyyy HH:mm:ss} ==> Durum Kodu : {response.StatusCode}\r\n");
                                File.AppendAllText(Path.Combine(Application.StartupPath, "Log", LogFileName), $"{DateTime.Now:dd.MM.yyyy HH:mm:ss} ==> Hata mesajı : {response.ReasonPhrase}\r\n");
                                lblBasarisizGonderim.Invoke((MethodInvoker)delegate
                                {
                                    BasarisizGonderim++;
                                    lblBasarisizGonderim.Text = BasarisizGonderim.ToString();
                                });
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    File.AppendAllText(Path.Combine(Application.StartupPath, "Log", LogFileName), $"{DateTime.Now:dd.MM.yyyy HH:mm:ss} ==>  Mail gönderimi sırasında api hatası meydana geldi.\r\n");
                    File.AppendAllText(Path.Combine(Application.StartupPath, "Log", LogFileName), $"{DateTime.Now:dd.MM.yyyy HH:mm:ss} ==> Hata mesajı : {ex.Message}\r\n");
                    lblBasarisizGonderim.Invoke((MethodInvoker)delegate
                    {
                        BasarisizGonderim++;
                        lblBasarisizGonderim.Text = BasarisizGonderim.ToString();
                    });
                }


                File.AppendAllText(Path.Combine(Application.StartupPath, "Log", LogFileName), "\r\n\r\n\r\n");

                System.Threading.Thread.Sleep(new Random().Next(12000, 25000));
            }
        }
    }
}
