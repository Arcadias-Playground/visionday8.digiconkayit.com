namespace ArcadiasDavet_MaillingApp
{
    partial class Form1
    {
        /// <summary>
        ///Gerekli tasarımcı değişkeni.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///Kullanılan tüm kaynakları temizleyin.
        /// </summary>
        ///<param name="disposing">yönetilen kaynaklar dispose edilmeliyse doğru; aksi halde yanlış.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer üretilen kod

        /// <summary>
        /// Tasarımcı desteği için gerekli metot - bu metodun 
        ///içeriğini kod düzenleyici ile değiştirmeyin.
        /// </summary>
        private void InitializeComponent()
        {
            this.gBoxAktarimBilgileri = new System.Windows.Forms.GroupBox();
            this.lblHataliKayitEtiket = new System.Windows.Forms.Label();
            this.lblBasarisizGonderimEtiket = new System.Windows.Forms.Label();
            this.lblBasariliGonderimEtiket = new System.Windows.Forms.Label();
            this.lblHataliKayit = new System.Windows.Forms.Label();
            this.lblAktarimYapilanKisiBilgisi = new System.Windows.Forms.Label();
            this.lblBasarisizGonderim = new System.Windows.Forms.Label();
            this.lblBasariliGonderim = new System.Windows.Forms.Label();
            this.lblAktarimSatirSayisi = new System.Windows.Forms.Label();
            this.lblToplamKatilimciSayisi = new System.Windows.Forms.Label();
            this.lblAtarimYapilanKisi = new System.Windows.Forms.Label();
            this.lblAktarimSatir = new System.Windows.Forms.Label();
            this.lblToplamKatilimci = new System.Windows.Forms.Label();
            this.gBoxKatilimciOzellikleri = new System.Windows.Forms.GroupBox();
            this.cBoxAktarımTipi = new System.Windows.Forms.ComboBox();
            this.btnListeAl = new System.Windows.Forms.Button();
            this.chkKatilimciOnay = new System.Windows.Forms.CheckBox();
            this.cBoxKatilimciTipi = new System.Windows.Forms.ComboBox();
            this.lblKatilimciOnay = new System.Windows.Forms.Label();
            this.lblAktarimTipi = new System.Windows.Forms.Label();
            this.lblKatilimciTipi = new System.Windows.Forms.Label();
            this.gBoxApiAdresi = new System.Windows.Forms.GroupBox();
            this.btnApiAdresiOnay = new System.Windows.Forms.Button();
            this.txtApiAdresi = new System.Windows.Forms.TextBox();
            this.lblApiAdresi = new System.Windows.Forms.Label();
            this.oFDExcel = new System.Windows.Forms.OpenFileDialog();
            this.lblePostaGonderim = new System.Windows.Forms.Label();
            this.lblSmsGonderimIstek = new System.Windows.Forms.Label();
            this.chkePostaGonderimIstek = new System.Windows.Forms.CheckBox();
            this.chkSmsGonderimIstek = new System.Windows.Forms.CheckBox();
            this.gBoxAktarimBilgileri.SuspendLayout();
            this.gBoxKatilimciOzellikleri.SuspendLayout();
            this.gBoxApiAdresi.SuspendLayout();
            this.SuspendLayout();
            // 
            // gBoxAktarimBilgileri
            // 
            this.gBoxAktarimBilgileri.Controls.Add(this.lblHataliKayitEtiket);
            this.gBoxAktarimBilgileri.Controls.Add(this.lblBasarisizGonderimEtiket);
            this.gBoxAktarimBilgileri.Controls.Add(this.lblBasariliGonderimEtiket);
            this.gBoxAktarimBilgileri.Controls.Add(this.lblHataliKayit);
            this.gBoxAktarimBilgileri.Controls.Add(this.lblAktarimYapilanKisiBilgisi);
            this.gBoxAktarimBilgileri.Controls.Add(this.lblBasarisizGonderim);
            this.gBoxAktarimBilgileri.Controls.Add(this.lblBasariliGonderim);
            this.gBoxAktarimBilgileri.Controls.Add(this.lblAktarimSatirSayisi);
            this.gBoxAktarimBilgileri.Controls.Add(this.lblToplamKatilimciSayisi);
            this.gBoxAktarimBilgileri.Controls.Add(this.lblAtarimYapilanKisi);
            this.gBoxAktarimBilgileri.Controls.Add(this.lblAktarimSatir);
            this.gBoxAktarimBilgileri.Controls.Add(this.lblToplamKatilimci);
            this.gBoxAktarimBilgileri.Enabled = false;
            this.gBoxAktarimBilgileri.Location = new System.Drawing.Point(12, 353);
            this.gBoxAktarimBilgileri.Name = "gBoxAktarimBilgileri";
            this.gBoxAktarimBilgileri.Size = new System.Drawing.Size(485, 143);
            this.gBoxAktarimBilgileri.TabIndex = 5;
            this.gBoxAktarimBilgileri.TabStop = false;
            this.gBoxAktarimBilgileri.Text = "Aktarım Bilgileri";
            // 
            // lblHataliKayitEtiket
            // 
            this.lblHataliKayitEtiket.AutoSize = true;
            this.lblHataliKayitEtiket.Location = new System.Drawing.Point(360, 92);
            this.lblHataliKayitEtiket.Margin = new System.Windows.Forms.Padding(10);
            this.lblHataliKayitEtiket.Name = "lblHataliKayitEtiket";
            this.lblHataliKayitEtiket.Size = new System.Drawing.Size(69, 13);
            this.lblHataliKayitEtiket.TabIndex = 2;
            this.lblHataliKayitEtiket.Text = "Hatalı Kayıt : ";
            // 
            // lblBasarisizGonderimEtiket
            // 
            this.lblBasarisizGonderimEtiket.AutoSize = true;
            this.lblBasarisizGonderimEtiket.Location = new System.Drawing.Point(324, 59);
            this.lblBasarisizGonderimEtiket.Margin = new System.Windows.Forms.Padding(10);
            this.lblBasarisizGonderimEtiket.Name = "lblBasarisizGonderimEtiket";
            this.lblBasarisizGonderimEtiket.Size = new System.Drawing.Size(105, 13);
            this.lblBasarisizGonderimEtiket.TabIndex = 2;
            this.lblBasarisizGonderimEtiket.Text = "Başarısız Gönderim : ";
            // 
            // lblBasariliGonderimEtiket
            // 
            this.lblBasariliGonderimEtiket.AutoSize = true;
            this.lblBasariliGonderimEtiket.Location = new System.Drawing.Point(332, 26);
            this.lblBasariliGonderimEtiket.Margin = new System.Windows.Forms.Padding(10);
            this.lblBasariliGonderimEtiket.Name = "lblBasariliGonderimEtiket";
            this.lblBasariliGonderimEtiket.Size = new System.Drawing.Size(97, 13);
            this.lblBasariliGonderimEtiket.TabIndex = 2;
            this.lblBasariliGonderimEtiket.Text = "Başarılı Gönderim : ";
            // 
            // lblHataliKayit
            // 
            this.lblHataliKayit.AutoSize = true;
            this.lblHataliKayit.Location = new System.Drawing.Point(442, 92);
            this.lblHataliKayit.Name = "lblHataliKayit";
            this.lblHataliKayit.Size = new System.Drawing.Size(13, 13);
            this.lblHataliKayit.TabIndex = 1;
            this.lblHataliKayit.Text = "0";
            // 
            // lblAktarimYapilanKisiBilgisi
            // 
            this.lblAktarimYapilanKisiBilgisi.AutoSize = true;
            this.lblAktarimYapilanKisiBilgisi.Location = new System.Drawing.Point(30, 115);
            this.lblAktarimYapilanKisiBilgisi.Name = "lblAktarimYapilanKisiBilgisi";
            this.lblAktarimYapilanKisiBilgisi.Size = new System.Drawing.Size(209, 13);
            this.lblAktarimYapilanKisiBilgisi.TabIndex = 1;
            this.lblAktarimYapilanKisiBilgisi.Text = "Altay Serhat İnan ( serhat@arkadyas.com )";
            // 
            // lblBasarisizGonderim
            // 
            this.lblBasarisizGonderim.AutoSize = true;
            this.lblBasarisizGonderim.Location = new System.Drawing.Point(442, 59);
            this.lblBasarisizGonderim.Name = "lblBasarisizGonderim";
            this.lblBasarisizGonderim.Size = new System.Drawing.Size(13, 13);
            this.lblBasarisizGonderim.TabIndex = 1;
            this.lblBasarisizGonderim.Text = "0";
            // 
            // lblBasariliGonderim
            // 
            this.lblBasariliGonderim.AutoSize = true;
            this.lblBasariliGonderim.Location = new System.Drawing.Point(442, 26);
            this.lblBasariliGonderim.Name = "lblBasariliGonderim";
            this.lblBasariliGonderim.Size = new System.Drawing.Size(13, 13);
            this.lblBasariliGonderim.TabIndex = 1;
            this.lblBasariliGonderim.Text = "0";
            // 
            // lblAktarimSatirSayisi
            // 
            this.lblAktarimSatirSayisi.AutoSize = true;
            this.lblAktarimSatirSayisi.Location = new System.Drawing.Point(148, 59);
            this.lblAktarimSatirSayisi.Name = "lblAktarimSatirSayisi";
            this.lblAktarimSatirSayisi.Size = new System.Drawing.Size(13, 13);
            this.lblAktarimSatirSayisi.TabIndex = 1;
            this.lblAktarimSatirSayisi.Text = "0";
            // 
            // lblToplamKatilimciSayisi
            // 
            this.lblToplamKatilimciSayisi.AutoSize = true;
            this.lblToplamKatilimciSayisi.Location = new System.Drawing.Point(148, 26);
            this.lblToplamKatilimciSayisi.Name = "lblToplamKatilimciSayisi";
            this.lblToplamKatilimciSayisi.Size = new System.Drawing.Size(13, 13);
            this.lblToplamKatilimciSayisi.TabIndex = 1;
            this.lblToplamKatilimciSayisi.Text = "0";
            // 
            // lblAtarimYapilanKisi
            // 
            this.lblAtarimYapilanKisi.AutoSize = true;
            this.lblAtarimYapilanKisi.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.lblAtarimYapilanKisi.Location = new System.Drawing.Point(30, 92);
            this.lblAtarimYapilanKisi.Margin = new System.Windows.Forms.Padding(10);
            this.lblAtarimYapilanKisi.Name = "lblAtarimYapilanKisi";
            this.lblAtarimYapilanKisi.Size = new System.Drawing.Size(96, 13);
            this.lblAtarimYapilanKisi.TabIndex = 0;
            this.lblAtarimYapilanKisi.Text = "Aktarım yapılan kişi";
            // 
            // lblAktarimSatir
            // 
            this.lblAktarimSatir.AutoSize = true;
            this.lblAktarimSatir.Location = new System.Drawing.Point(26, 59);
            this.lblAktarimSatir.Margin = new System.Windows.Forms.Padding(10);
            this.lblAktarimSatir.Name = "lblAktarimSatir";
            this.lblAktarimSatir.Size = new System.Drawing.Size(109, 13);
            this.lblAktarimSatir.TabIndex = 0;
            this.lblAktarimSatir.Text = "Aktarım yapılan satır : ";
            // 
            // lblToplamKatilimci
            // 
            this.lblToplamKatilimci.AutoSize = true;
            this.lblToplamKatilimci.Location = new System.Drawing.Point(13, 26);
            this.lblToplamKatilimci.Margin = new System.Windows.Forms.Padding(10);
            this.lblToplamKatilimci.Name = "lblToplamKatilimci";
            this.lblToplamKatilimci.Size = new System.Drawing.Size(122, 13);
            this.lblToplamKatilimci.TabIndex = 0;
            this.lblToplamKatilimci.Text = "Toplam Katılımcı Sayısı : ";
            // 
            // gBoxKatilimciOzellikleri
            // 
            this.gBoxKatilimciOzellikleri.Controls.Add(this.cBoxAktarımTipi);
            this.gBoxKatilimciOzellikleri.Controls.Add(this.btnListeAl);
            this.gBoxKatilimciOzellikleri.Controls.Add(this.chkSmsGonderimIstek);
            this.gBoxKatilimciOzellikleri.Controls.Add(this.chkePostaGonderimIstek);
            this.gBoxKatilimciOzellikleri.Controls.Add(this.chkKatilimciOnay);
            this.gBoxKatilimciOzellikleri.Controls.Add(this.cBoxKatilimciTipi);
            this.gBoxKatilimciOzellikleri.Controls.Add(this.lblSmsGonderimIstek);
            this.gBoxKatilimciOzellikleri.Controls.Add(this.lblePostaGonderim);
            this.gBoxKatilimciOzellikleri.Controls.Add(this.lblKatilimciOnay);
            this.gBoxKatilimciOzellikleri.Controls.Add(this.lblAktarimTipi);
            this.gBoxKatilimciOzellikleri.Controls.Add(this.lblKatilimciTipi);
            this.gBoxKatilimciOzellikleri.Enabled = false;
            this.gBoxKatilimciOzellikleri.Location = new System.Drawing.Point(12, 108);
            this.gBoxKatilimciOzellikleri.Name = "gBoxKatilimciOzellikleri";
            this.gBoxKatilimciOzellikleri.Size = new System.Drawing.Size(485, 239);
            this.gBoxKatilimciOzellikleri.TabIndex = 4;
            this.gBoxKatilimciOzellikleri.TabStop = false;
            this.gBoxKatilimciOzellikleri.Text = "Aktarım Özellikleri";
            // 
            // cBoxAktarımTipi
            // 
            this.cBoxAktarımTipi.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cBoxAktarımTipi.FormattingEnabled = true;
            this.cBoxAktarımTipi.Location = new System.Drawing.Point(100, 23);
            this.cBoxAktarımTipi.Name = "cBoxAktarımTipi";
            this.cBoxAktarımTipi.Size = new System.Drawing.Size(379, 21);
            this.cBoxAktarımTipi.TabIndex = 4;
            this.cBoxAktarımTipi.SelectedValueChanged += new System.EventHandler(this.cBoxAktarımTipi_SelectedValueChanged);
            // 
            // btnListeAl
            // 
            this.btnListeAl.Location = new System.Drawing.Point(16, 184);
            this.btnListeAl.Name = "btnListeAl";
            this.btnListeAl.Size = new System.Drawing.Size(463, 44);
            this.btnListeAl.TabIndex = 3;
            this.btnListeAl.Text = "Excel Listesini Al ve İşleme Başla";
            this.btnListeAl.UseVisualStyleBackColor = true;
            this.btnListeAl.Click += new System.EventHandler(this.btnListeAl_Click);
            // 
            // chkKatilimciOnay
            // 
            this.chkKatilimciOnay.AutoSize = true;
            this.chkKatilimciOnay.Location = new System.Drawing.Point(148, 91);
            this.chkKatilimciOnay.Name = "chkKatilimciOnay";
            this.chkKatilimciOnay.Size = new System.Drawing.Size(156, 17);
            this.chkKatilimciOnay.TabIndex = 2;
            this.chkKatilimciOnay.Text = "Katılımcı onaylı olarak aktar.";
            this.chkKatilimciOnay.UseVisualStyleBackColor = true;
            // 
            // cBoxKatilimciTipi
            // 
            this.cBoxKatilimciTipi.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cBoxKatilimciTipi.FormattingEnabled = true;
            this.cBoxKatilimciTipi.Location = new System.Drawing.Point(100, 56);
            this.cBoxKatilimciTipi.Name = "cBoxKatilimciTipi";
            this.cBoxKatilimciTipi.Size = new System.Drawing.Size(379, 21);
            this.cBoxKatilimciTipi.TabIndex = 1;
            // 
            // lblKatilimciOnay
            // 
            this.lblKatilimciOnay.AutoSize = true;
            this.lblKatilimciOnay.Location = new System.Drawing.Point(13, 92);
            this.lblKatilimciOnay.Margin = new System.Windows.Forms.Padding(10);
            this.lblKatilimciOnay.Name = "lblKatilimciOnay";
            this.lblKatilimciOnay.Size = new System.Drawing.Size(122, 13);
            this.lblKatilimciOnay.TabIndex = 0;
            this.lblKatilimciOnay.Text = "Katılımcı Onay Durumu : ";
            // 
            // lblAktarimTipi
            // 
            this.lblAktarimTipi.AutoSize = true;
            this.lblAktarimTipi.Location = new System.Drawing.Point(16, 26);
            this.lblAktarimTipi.Margin = new System.Windows.Forms.Padding(10);
            this.lblAktarimTipi.Name = "lblAktarimTipi";
            this.lblAktarimTipi.Size = new System.Drawing.Size(71, 13);
            this.lblAktarimTipi.TabIndex = 0;
            this.lblAktarimTipi.Text = "Aktarım Tipi : ";
            // 
            // lblKatilimciTipi
            // 
            this.lblKatilimciTipi.AutoSize = true;
            this.lblKatilimciTipi.Location = new System.Drawing.Point(13, 59);
            this.lblKatilimciTipi.Margin = new System.Windows.Forms.Padding(10);
            this.lblKatilimciTipi.Name = "lblKatilimciTipi";
            this.lblKatilimciTipi.Size = new System.Drawing.Size(74, 13);
            this.lblKatilimciTipi.TabIndex = 0;
            this.lblKatilimciTipi.Text = "Katılımcı Tipi : ";
            // 
            // gBoxApiAdresi
            // 
            this.gBoxApiAdresi.Controls.Add(this.btnApiAdresiOnay);
            this.gBoxApiAdresi.Controls.Add(this.txtApiAdresi);
            this.gBoxApiAdresi.Controls.Add(this.lblApiAdresi);
            this.gBoxApiAdresi.Location = new System.Drawing.Point(12, 12);
            this.gBoxApiAdresi.Name = "gBoxApiAdresi";
            this.gBoxApiAdresi.Size = new System.Drawing.Size(485, 90);
            this.gBoxApiAdresi.TabIndex = 3;
            this.gBoxApiAdresi.TabStop = false;
            this.gBoxApiAdresi.Text = "Api Adresi Bilgileri";
            // 
            // btnApiAdresiOnay
            // 
            this.btnApiAdresiOnay.Location = new System.Drawing.Point(16, 49);
            this.btnApiAdresiOnay.Name = "btnApiAdresiOnay";
            this.btnApiAdresiOnay.Size = new System.Drawing.Size(463, 35);
            this.btnApiAdresiOnay.TabIndex = 2;
            this.btnApiAdresiOnay.Text = "Api Adresini Doğrula";
            this.btnApiAdresiOnay.UseVisualStyleBackColor = true;
            this.btnApiAdresiOnay.Click += new System.EventHandler(this.btnApiAdresiOnay_Click);
            // 
            // txtApiAdresi
            // 
            this.txtApiAdresi.Location = new System.Drawing.Point(89, 23);
            this.txtApiAdresi.Name = "txtApiAdresi";
            this.txtApiAdresi.Size = new System.Drawing.Size(390, 20);
            this.txtApiAdresi.TabIndex = 1;
            this.txtApiAdresi.Text = "https://davet.arcadiastech.com";
            // 
            // lblApiAdresi
            // 
            this.lblApiAdresi.AutoSize = true;
            this.lblApiAdresi.Location = new System.Drawing.Point(13, 26);
            this.lblApiAdresi.Margin = new System.Windows.Forms.Padding(10);
            this.lblApiAdresi.Name = "lblApiAdresi";
            this.lblApiAdresi.Size = new System.Drawing.Size(63, 13);
            this.lblApiAdresi.TabIndex = 0;
            this.lblApiAdresi.Text = "Api Adresi : ";
            // 
            // oFDExcel
            // 
            this.oFDExcel.DefaultExt = "xlsx";
            this.oFDExcel.Filter = "Excel Dosyası 2007 ve üzeri |*.xlsx| Excel Dosyası 2003 - 2007|*.xls";
            // 
            // lblePostaGonderim
            // 
            this.lblePostaGonderim.AutoSize = true;
            this.lblePostaGonderim.Location = new System.Drawing.Point(83, 125);
            this.lblePostaGonderim.Margin = new System.Windows.Forms.Padding(10);
            this.lblePostaGonderim.Name = "lblePostaGonderim";
            this.lblePostaGonderim.Size = new System.Drawing.Size(52, 13);
            this.lblePostaGonderim.TabIndex = 0;
            this.lblePostaGonderim.Text = "e-Posta : ";
            // 
            // lblSmsGonderimIstek
            // 
            this.lblSmsGonderimIstek.AutoSize = true;
            this.lblSmsGonderimIstek.Location = new System.Drawing.Point(99, 158);
            this.lblSmsGonderimIstek.Margin = new System.Windows.Forms.Padding(10);
            this.lblSmsGonderimIstek.Name = "lblSmsGonderimIstek";
            this.lblSmsGonderimIstek.Size = new System.Drawing.Size(36, 13);
            this.lblSmsGonderimIstek.TabIndex = 0;
            this.lblSmsGonderimIstek.Text = "Sms : ";
            // 
            // chkePostaGonderimIstek
            // 
            this.chkePostaGonderimIstek.AutoSize = true;
            this.chkePostaGonderimIstek.Location = new System.Drawing.Point(148, 124);
            this.chkePostaGonderimIstek.Name = "chkePostaGonderimIstek";
            this.chkePostaGonderimIstek.Size = new System.Drawing.Size(100, 17);
            this.chkePostaGonderimIstek.TabIndex = 2;
            this.chkePostaGonderimIstek.Text = "e-Posta Gönder";
            this.chkePostaGonderimIstek.UseVisualStyleBackColor = true;
            // 
            // chkSmsGonderimIstek
            // 
            this.chkSmsGonderimIstek.AutoSize = true;
            this.chkSmsGonderimIstek.Location = new System.Drawing.Point(148, 157);
            this.chkSmsGonderimIstek.Name = "chkSmsGonderimIstek";
            this.chkSmsGonderimIstek.Size = new System.Drawing.Size(84, 17);
            this.chkSmsGonderimIstek.TabIndex = 2;
            this.chkSmsGonderimIstek.Text = "Sms Gönder";
            this.chkSmsGonderimIstek.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(509, 510);
            this.Controls.Add(this.gBoxAktarimBilgileri);
            this.Controls.Add(this.gBoxKatilimciOzellikleri);
            this.Controls.Add(this.gBoxApiAdresi);
            this.Name = "Form1";
            this.Text = "Form1";
            this.gBoxAktarimBilgileri.ResumeLayout(false);
            this.gBoxAktarimBilgileri.PerformLayout();
            this.gBoxKatilimciOzellikleri.ResumeLayout(false);
            this.gBoxKatilimciOzellikleri.PerformLayout();
            this.gBoxApiAdresi.ResumeLayout(false);
            this.gBoxApiAdresi.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gBoxAktarimBilgileri;
        private System.Windows.Forms.Label lblHataliKayitEtiket;
        private System.Windows.Forms.Label lblBasarisizGonderimEtiket;
        private System.Windows.Forms.Label lblBasariliGonderimEtiket;
        private System.Windows.Forms.Label lblHataliKayit;
        private System.Windows.Forms.Label lblAktarimYapilanKisiBilgisi;
        private System.Windows.Forms.Label lblBasarisizGonderim;
        private System.Windows.Forms.Label lblBasariliGonderim;
        private System.Windows.Forms.Label lblAktarimSatirSayisi;
        private System.Windows.Forms.Label lblToplamKatilimciSayisi;
        private System.Windows.Forms.Label lblAtarimYapilanKisi;
        private System.Windows.Forms.Label lblAktarimSatir;
        private System.Windows.Forms.Label lblToplamKatilimci;
        private System.Windows.Forms.GroupBox gBoxKatilimciOzellikleri;
        private System.Windows.Forms.Button btnListeAl;
        private System.Windows.Forms.CheckBox chkKatilimciOnay;
        private System.Windows.Forms.ComboBox cBoxKatilimciTipi;
        private System.Windows.Forms.Label lblKatilimciOnay;
        private System.Windows.Forms.Label lblKatilimciTipi;
        private System.Windows.Forms.GroupBox gBoxApiAdresi;
        private System.Windows.Forms.Button btnApiAdresiOnay;
        private System.Windows.Forms.TextBox txtApiAdresi;
        private System.Windows.Forms.Label lblApiAdresi;
        private System.Windows.Forms.OpenFileDialog oFDExcel;
        private System.Windows.Forms.ComboBox cBoxAktarımTipi;
        private System.Windows.Forms.Label lblAktarimTipi;
        private System.Windows.Forms.Label lblSmsGonderimIstek;
        private System.Windows.Forms.Label lblePostaGonderim;
        private System.Windows.Forms.CheckBox chkePostaGonderimIstek;
        private System.Windows.Forms.CheckBox chkSmsGonderimIstek;
    }
}

