using Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.OleDb;
using VeritabaniIslemSaglayici.Access;

namespace VeritabaniIslemMerkeziBase
{
	public abstract class WebMailTablosuIslemlerBase
	{
		public VTOperatorleri VTIslem;

		public List<WebMailTablosuModel> VeriListe;

		public SurecBilgiModel SModel;
		public SurecVeriModel<WebMailTablosuModel> SDataModel;
		public SurecVeriModel<IList<WebMailTablosuModel>> SDataListModel;

		public WebMailTablosuIslemlerBase()
		{
			VTIslem = new VTOperatorleri();
		}

		public WebMailTablosuIslemlerBase(OleDbTransaction Transcation)
		{
			VTIslem = new VTOperatorleri(Transcation);
		}

		public virtual SurecBilgiModel YeniKayitEkle(WebMailTablosuModel YeniKayit)
		{
			VTIslem.SetCommandText("INSERT INTO WebMailTablosu (WebMailID, GonderenMail, Konu, TextBody, HtmlBody, WebMailTarih, KullaniciID, MailGorulmeTarihi, EklenmeTarihi) VALUES (@WebMailID, @GonderenMail, @Konu, @TextBody, @HtmlBody, @WebMailTarih, @KullaniciID, @MailGorulmeTarihi, @EklenmeTarihi)");
			VTIslem.AddWithValue("WebMailID", YeniKayit.WebMailID);
			VTIslem.AddWithValue("GonderenMail", YeniKayit.GonderenMail);
			VTIslem.AddWithValue("Konu", YeniKayit.Konu);

			if(YeniKayit.TextBody is null)
				VTIslem.AddWithValue("TextBody", DBNull.Value);
			else
				VTIslem.AddWithValue("TextBody", YeniKayit.TextBody);


			if(YeniKayit.HtmlBody is null)
				VTIslem.AddWithValue("HtmlBody", DBNull.Value);
			else
				VTIslem.AddWithValue("HtmlBody", YeniKayit.HtmlBody);

			VTIslem.AddWithValue("WebMailTarih", YeniKayit.WebMailTarih);

			if(YeniKayit.KullaniciID is null)
				VTIslem.AddWithValue("KullaniciID", DBNull.Value);
			else
				VTIslem.AddWithValue("KullaniciID", YeniKayit.KullaniciID);


			if(YeniKayit.MailGorulmeTarihi is null)
				VTIslem.AddWithValue("MailGorulmeTarihi", DBNull.Value);
			else
				VTIslem.AddWithValue("MailGorulmeTarihi", YeniKayit.MailGorulmeTarihi);

			VTIslem.AddWithValue("EklenmeTarihi", YeniKayit.EklenmeTarihi);
			return VTIslem.ExecuteNonQuery();
		}

		public virtual SurecBilgiModel KayitGuncelle(WebMailTablosuModel GuncelKayit)
		{
			VTIslem.SetCommandText("UPDATE WebMailTablosu SET GonderenMail=@GonderenMail, Konu=@Konu, TextBody=@TextBody, HtmlBody=@HtmlBody, WebMailTarih=@WebMailTarih, KullaniciID=@KullaniciID, MailGorulmeTarihi=@MailGorulmeTarihi, EklenmeTarihi=@EklenmeTarihi WHERE WebMailID=@WebMailID");
			VTIslem.AddWithValue("GonderenMail", GuncelKayit.GonderenMail);
			VTIslem.AddWithValue("Konu", GuncelKayit.Konu);

			if(GuncelKayit.TextBody is null)
				VTIslem.AddWithValue("TextBody", DBNull.Value);
			else
				VTIslem.AddWithValue("TextBody", GuncelKayit.TextBody);


			if(GuncelKayit.HtmlBody is null)
				VTIslem.AddWithValue("HtmlBody", DBNull.Value);
			else
				VTIslem.AddWithValue("HtmlBody", GuncelKayit.HtmlBody);

			VTIslem.AddWithValue("WebMailTarih", GuncelKayit.WebMailTarih);

			if(GuncelKayit.KullaniciID is null)
				VTIslem.AddWithValue("KullaniciID", DBNull.Value);
			else
				VTIslem.AddWithValue("KullaniciID", GuncelKayit.KullaniciID);


			if(GuncelKayit.MailGorulmeTarihi is null)
				VTIslem.AddWithValue("MailGorulmeTarihi", DBNull.Value);
			else
				VTIslem.AddWithValue("MailGorulmeTarihi", GuncelKayit.MailGorulmeTarihi.Value);

			VTIslem.AddWithValue("EklenmeTarihi", GuncelKayit.EklenmeTarihi);
			VTIslem.AddWithValue("WebMailID", GuncelKayit.WebMailID);
			return VTIslem.ExecuteNonQuery();
		}

		public virtual SurecBilgiModel KayitSil(string WebMailID)
		{
			VTIslem.SetCommandText("DELETE FROM WebMailTablosu WHERE WebMailID=@WebMailID");
			VTIslem.AddWithValue("WebMailID", WebMailID);
			return VTIslem.ExecuteNonQuery();
		}

		public virtual SurecVeriModel<WebMailTablosuModel> KayitBilgisi(string WebMailID)
		{
			VTIslem.SetCommandText($"SELECT {WebMailTablosuModel.SQLSutunSorgusu} FROM WebMailTablosu WHERE WebMailID = @WebMailID");
			VTIslem.AddWithValue("WebMailID", WebMailID);
			VTIslem.OpenConnection();
			SModel = VTIslem.ExecuteReader(CommandBehavior.SingleResult);
			if (SModel.Sonuc.Equals(Sonuclar.Basarili))
			{
				while (SModel.Reader.Read())
				{
					KayitBilgisiAl();
				}
				if (SDataModel is null)
				{
					SDataModel = new SurecVeriModel<WebMailTablosuModel>
					{
						Sonuc = Sonuclar.VeriBulunamadi,
						KullaniciMesaji = "Belirtilen kayıt bulunamamıştır",
						HataBilgi = new HataBilgileri
						{
							HataAlinanKayitID = 0,
							HataKodu = 0,
							HataMesaji = "Belirtilen kayıt bulunamamıştır"
						}
					};
				}
			}
			else
			{
				SDataModel = new SurecVeriModel<WebMailTablosuModel>
				{
					Sonuc = SModel.Sonuc,
					KullaniciMesaji = SModel.KullaniciMesaji,
					HataBilgi = SModel.HataBilgi
				};
			}
			VTIslem.CloseConnection();
			return SDataModel;
		}

		public virtual SurecVeriModel<IList<WebMailTablosuModel>> KayitBilgileri()
		{
			VTIslem.SetCommandText($"SELECT {WebMailTablosuModel.SQLSutunSorgusu} FROM WebMailTablosu");
			VTIslem.OpenConnection();
			SModel = VTIslem.ExecuteReader(CommandBehavior.Default);
			if (SModel.Sonuc.Equals(Sonuclar.Basarili))
			{
				VeriListe = new List<WebMailTablosuModel>();
				while (SModel.Reader.Read())
				{
					if (KayitBilgisiAl().Sonuc.Equals(Sonuclar.Basarili))
					{
						VeriListe.Add(SDataModel.Veriler);
					}
					else
					{
						SDataListModel = new SurecVeriModel<IList<WebMailTablosuModel>>{
							Sonuc = SDataModel.Sonuc,
							KullaniciMesaji = SDataModel.KullaniciMesaji,
							HataBilgi = SDataModel.HataBilgi
						};
						VTIslem.CloseConnection();
						return SDataListModel;
					}
				}
				SDataListModel = new SurecVeriModel<IList<WebMailTablosuModel>>{
					Sonuc = Sonuclar.Basarili,
					KullaniciMesaji = "Veri listesi başarıyla çekildi",
					Veriler = VeriListe
				};
			}
			else
			{
				SDataListModel = new SurecVeriModel<IList<WebMailTablosuModel>>{
					Sonuc = SModel.Sonuc,
					KullaniciMesaji = SModel.KullaniciMesaji,
					HataBilgi = SModel.HataBilgi
				};
			}
			VTIslem.CloseConnection();
			return SDataListModel;
		}

		SurecVeriModel<WebMailTablosuModel> KayitBilgisiAl()
		{
			try
			{
				SDataModel = new SurecVeriModel<WebMailTablosuModel>{
					Sonuc = Sonuclar.Basarili,
					KullaniciMesaji = "Veri bilgisi başarıyla çekilmiştir.",
					Veriler = new WebMailTablosuModel
					{
						WebMailID = SModel.Reader.GetString(0),
						GonderenMail = SModel.Reader.GetString(1),
						Konu = SModel.Reader.GetString(2),
						TextBody = SModel.Reader.IsDBNull(3) ? string.Empty : SModel.Reader.GetString(3),
						HtmlBody = SModel.Reader.IsDBNull(4) ? string.Empty : SModel.Reader.GetString(4),
						WebMailTarih = SModel.Reader.GetDateTime(5),
						KullaniciID = SModel.Reader.IsDBNull(6) ? string.Empty : SModel.Reader.GetString(6),
						MailGorulmeTarihi = SModel.Reader.IsDBNull(7) ? null : new DateTime?(SModel.Reader.GetDateTime(7)),
						EklenmeTarihi = SModel.Reader.GetDateTime(8),
					}
				};

			}
			catch (InvalidCastException ex)
			{
				SDataModel = new SurecVeriModel<WebMailTablosuModel>{
					Sonuc = Sonuclar.Basarisiz,
					KullaniciMesaji = "Veri bilgisi çekilirken hatalı atama yapılmaya çalışıldı",
					HataBilgi = new HataBilgileri{
						HataMesaji = string.Format(@"{0}", ex.Message.Replace("'", "ʼ")),
						HataKodu = ex.HResult,
						HataAlinanKayitID = SModel.Reader.GetValue(0)
					}
				};
			}
			catch (Exception ex)
			{
				SDataModel = new SurecVeriModel<WebMailTablosuModel>{
					Sonuc = Sonuclar.Basarisiz,
					KullaniciMesaji = "Veri bilgisi çekilirken hatalı atama yapılmaya çalışıldı",
					HataBilgi = new HataBilgileri{
						HataMesaji = string.Format(@"{0}", ex.Message.Replace("'", "ʼ")),
						HataKodu = ex.HResult,
						HataAlinanKayitID = SModel.Reader.GetValue(0)
					}
				};
			}
			return SDataModel;
		}

		public virtual SurecVeriModel<WebMailTablosuModel> KayitBilgisiAl(int Baslangic, DbDataReader Reader)
		{
			try
			{
				SDataModel = new SurecVeriModel<WebMailTablosuModel>{
					Sonuc = Sonuclar.Basarili,
					KullaniciMesaji = "Veri bilgisi başarıyla çekilmiştir.",
					Veriler = new WebMailTablosuModel{
						WebMailID = Reader.GetString(Baslangic + 0),
						GonderenMail = Reader.GetString(Baslangic + 1),
						Konu = Reader.GetString(Baslangic + 2),
						TextBody = Reader.IsDBNull(Baslangic + 3) ? string.Empty : Reader.GetString(Baslangic + 3),
						HtmlBody = Reader.IsDBNull(Baslangic + 4) ? string.Empty : Reader.GetString(Baslangic + 4),
						WebMailTarih = Reader.GetDateTime(Baslangic + 5),
						KullaniciID = Reader.IsDBNull(Baslangic + 6) ? string.Empty : Reader.GetString(Baslangic + 6),
						MailGorulmeTarihi = Reader.IsDBNull(Baslangic + 7) ? null : new DateTime?(Reader.GetDateTime(Baslangic + 7)),
						EklenmeTarihi = Reader.GetDateTime(Baslangic + 8),
					}
				};
			}
			catch (InvalidCastException ex)
			{
				SDataModel = new SurecVeriModel<WebMailTablosuModel>{
					Sonuc = Sonuclar.Basarisiz,
					KullaniciMesaji = "Veri bilgisi çekilirken hatalı atama yapılmaya çalışıldı",
					HataBilgi = new HataBilgileri{
						HataMesaji = string.Format(@"{0}", ex.Message.Replace("'", "ʼ")),
						HataKodu = ex.HResult,
						HataAlinanKayitID = Reader.GetValue(0)
					}
				};
			}
			catch (Exception ex)
			{
				SDataModel = new SurecVeriModel<WebMailTablosuModel>{
					Sonuc = Sonuclar.Basarisiz,
					KullaniciMesaji = "Veri bilgisi çekilirken hatalı atama yapılmaya çalışıldı",
						HataBilgi = new HataBilgileri{
						HataMesaji = string.Format(@"{0}", ex.Message.Replace("'", "ʼ")),
						HataKodu = ex.HResult,
						HataAlinanKayitID = Reader.GetValue(0)
					}
				};
			}
			return SDataModel;
		}

		public virtual SurecVeriModel<IList<WebMailTablosuModel>> KullaniciBilgileri(string KullaniciID)
		{
			VTIslem.SetCommandText($"SELECT {WebMailTablosuModel.SQLSutunSorgusu} FROM WebMailTablosu WHERE KullaniciID=@KullaniciID");
			VTIslem.AddWithValue("KullaniciID", KullaniciID);
			VTIslem.OpenConnection();
			SModel = VTIslem.ExecuteReader(CommandBehavior.Default);
			if (SModel.Sonuc.Equals(Sonuclar.Basarili))
			{
				VeriListe = new List<WebMailTablosuModel>();
				while (SModel.Reader.Read())
				{
					if (KayitBilgisiAl().Sonuc.Equals(Sonuclar.Basarili))
					{
						VeriListe.Add(SDataModel.Veriler);
					}
					else
					{
						SDataListModel = new SurecVeriModel<IList<WebMailTablosuModel>>{
							Sonuc = SDataModel.Sonuc,
							KullaniciMesaji = SDataModel.KullaniciMesaji,
							HataBilgi = SDataModel.HataBilgi
						};
						VTIslem.CloseConnection();
						return SDataListModel;
					}
				}
				SDataListModel = new SurecVeriModel<IList<WebMailTablosuModel>>{
					Sonuc = Sonuclar.Basarili,
					KullaniciMesaji = "Veri listesi başarıyla çekildi",
					Veriler = VeriListe
				};
			}
			else
			{
				SDataListModel = new SurecVeriModel<IList<WebMailTablosuModel>>{
					Sonuc = SModel.Sonuc,
					KullaniciMesaji = SModel.KullaniciMesaji,
					HataBilgi = SModel.HataBilgi
				};
			}
			VTIslem.CloseConnection();
			return SDataListModel;
		}

	}
}