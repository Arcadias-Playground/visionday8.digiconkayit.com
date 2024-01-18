using Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.OleDb;
using VeritabaniIslemSaglayici.Access;

namespace VeritabaniIslemMerkeziBase
{
	public abstract class MailIcerikTablosuIslemlerBase
	{
		public VTOperatorleri VTIslem;

		public List<MailIcerikTablosuModel> VeriListe;

		public SurecBilgiModel SModel;
		public SurecVeriModel<MailIcerikTablosuModel> SDataModel;
		public SurecVeriModel<IList<MailIcerikTablosuModel>> SDataListModel;

		public MailIcerikTablosuIslemlerBase()
		{
			VTIslem = new VTOperatorleri();
		}

		public MailIcerikTablosuIslemlerBase(OleDbTransaction Transcation)
		{
			VTIslem = new VTOperatorleri(Transcation);
		}

		public virtual SurecBilgiModel YeniKayitEkle(MailIcerikTablosuModel YeniKayit)
		{
			VTIslem.SetCommandText("INSERT INTO MailIcerikTablosu (GonderimTipiID, KatilimciTipiID, Konu, HtmlIcerik, AnaKatilimci, GuncellenmeTarihi, EklenmeTarihi) VALUES (@GonderimTipiID, @KatilimciTipiID, @Konu, @HtmlIcerik, @AnaKatilimci, @GuncellenmeTarihi, @EklenmeTarihi)");
			VTIslem.AddWithValue("GonderimTipiID", YeniKayit.GonderimTipiID);
			VTIslem.AddWithValue("KatilimciTipiID", YeniKayit.KatilimciTipiID);
			VTIslem.AddWithValue("Konu", YeniKayit.Konu);
			VTIslem.AddWithValue("HtmlIcerik", YeniKayit.HtmlIcerik);
			VTIslem.AddWithValue("AnaKatilimci", YeniKayit.AnaKatilimci);
			VTIslem.AddWithValue("GuncellenmeTarihi", YeniKayit.GuncellenmeTarihi);
			VTIslem.AddWithValue("EklenmeTarihi", YeniKayit.EklenmeTarihi);
			return VTIslem.ExecuteNonQuery();
		}

		public virtual SurecBilgiModel KayitGuncelle(MailIcerikTablosuModel GuncelKayit)
		{
			VTIslem.SetCommandText("UPDATE MailIcerikTablosu SET GonderimTipiID=@GonderimTipiID, KatilimciTipiID=@KatilimciTipiID, Konu=@Konu, HtmlIcerik=@HtmlIcerik, AnaKatilimci=@AnaKatilimci, GuncellenmeTarihi=@GuncellenmeTarihi, EklenmeTarihi=@EklenmeTarihi WHERE MailIcerikID=@MailIcerikID");
			VTIslem.AddWithValue("GonderimTipiID", GuncelKayit.GonderimTipiID);
			VTIslem.AddWithValue("KatilimciTipiID", GuncelKayit.KatilimciTipiID);
			VTIslem.AddWithValue("Konu", GuncelKayit.Konu);
			VTIslem.AddWithValue("HtmlIcerik", GuncelKayit.HtmlIcerik);
			VTIslem.AddWithValue("AnaKatilimci", GuncelKayit.AnaKatilimci);
			VTIslem.AddWithValue("GuncellenmeTarihi", GuncelKayit.GuncellenmeTarihi);
			VTIslem.AddWithValue("EklenmeTarihi", GuncelKayit.EklenmeTarihi);
			VTIslem.AddWithValue("MailIcerikID", GuncelKayit.MailIcerikID);
			return VTIslem.ExecuteNonQuery();
		}

		public virtual SurecBilgiModel KayitSil(int MailIcerikID)
		{
			VTIslem.SetCommandText("DELETE FROM MailIcerikTablosu WHERE MailIcerikID=@MailIcerikID");
			VTIslem.AddWithValue("MailIcerikID", MailIcerikID);
			return VTIslem.ExecuteNonQuery();
		}

		public virtual SurecVeriModel<MailIcerikTablosuModel> KayitBilgisi(int MailIcerikID)
		{
			VTIslem.SetCommandText($"SELECT {MailIcerikTablosuModel.SQLSutunSorgusu} FROM MailIcerikTablosu WHERE MailIcerikID = @MailIcerikID");
			VTIslem.AddWithValue("MailIcerikID", MailIcerikID);
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
					SDataModel = new SurecVeriModel<MailIcerikTablosuModel>
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
				SDataModel = new SurecVeriModel<MailIcerikTablosuModel>
				{
					Sonuc = SModel.Sonuc,
					KullaniciMesaji = SModel.KullaniciMesaji,
					HataBilgi = SModel.HataBilgi
				};
			}
			VTIslem.CloseConnection();
			return SDataModel;
		}

		public virtual SurecVeriModel<IList<MailIcerikTablosuModel>> KayitBilgileri()
		{
			VTIslem.SetCommandText($"SELECT {MailIcerikTablosuModel.SQLSutunSorgusu} FROM MailIcerikTablosu");
			VTIslem.OpenConnection();
			SModel = VTIslem.ExecuteReader(CommandBehavior.Default);
			if (SModel.Sonuc.Equals(Sonuclar.Basarili))
			{
				VeriListe = new List<MailIcerikTablosuModel>();
				while (SModel.Reader.Read())
				{
					if (KayitBilgisiAl().Sonuc.Equals(Sonuclar.Basarili))
					{
						VeriListe.Add(SDataModel.Veriler);
					}
					else
					{
						SDataListModel = new SurecVeriModel<IList<MailIcerikTablosuModel>>{
							Sonuc = SDataModel.Sonuc,
							KullaniciMesaji = SDataModel.KullaniciMesaji,
							HataBilgi = SDataModel.HataBilgi
						};
						VTIslem.CloseConnection();
						return SDataListModel;
					}
				}
				SDataListModel = new SurecVeriModel<IList<MailIcerikTablosuModel>>{
					Sonuc = Sonuclar.Basarili,
					KullaniciMesaji = "Veri listesi başarıyla çekildi",
					Veriler = VeriListe
				};
			}
			else
			{
				SDataListModel = new SurecVeriModel<IList<MailIcerikTablosuModel>>{
					Sonuc = SModel.Sonuc,
					KullaniciMesaji = SModel.KullaniciMesaji,
					HataBilgi = SModel.HataBilgi
				};
			}
			VTIslem.CloseConnection();
			return SDataListModel;
		}

		SurecVeriModel<MailIcerikTablosuModel> KayitBilgisiAl()
		{
			try
			{
				SDataModel = new SurecVeriModel<MailIcerikTablosuModel>{
					Sonuc = Sonuclar.Basarili,
					KullaniciMesaji = "Veri bilgisi başarıyla çekilmiştir.",
					Veriler = new MailIcerikTablosuModel
					{
						MailIcerikID = SModel.Reader.GetInt32(0),
						GonderimTipiID = SModel.Reader.GetInt32(1),
						KatilimciTipiID = SModel.Reader.GetString(2),
						Konu = SModel.Reader.GetString(3),
						HtmlIcerik = SModel.Reader.GetString(4),
						AnaKatilimci = SModel.Reader.GetBoolean(5),
						GuncellenmeTarihi = SModel.Reader.GetDateTime(6),
						EklenmeTarihi = SModel.Reader.GetDateTime(7),
					}
				};

			}
			catch (InvalidCastException ex)
			{
				SDataModel = new SurecVeriModel<MailIcerikTablosuModel>{
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
				SDataModel = new SurecVeriModel<MailIcerikTablosuModel>{
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

		public virtual SurecVeriModel<MailIcerikTablosuModel> KayitBilgisiAl(int Baslangic, DbDataReader Reader)
		{
			try
			{
				SDataModel = new SurecVeriModel<MailIcerikTablosuModel>{
					Sonuc = Sonuclar.Basarili,
					KullaniciMesaji = "Veri bilgisi başarıyla çekilmiştir.",
					Veriler = new MailIcerikTablosuModel{
						MailIcerikID = Reader.GetInt32(Baslangic + 0),
						GonderimTipiID = Reader.GetInt32(Baslangic + 1),
						KatilimciTipiID = Reader.GetString(Baslangic + 2),
						Konu = Reader.GetString(Baslangic + 3),
						HtmlIcerik = Reader.GetString(Baslangic + 4),
						AnaKatilimci = Reader.GetBoolean(Baslangic + 5),
						GuncellenmeTarihi = Reader.GetDateTime(Baslangic + 6),
						EklenmeTarihi = Reader.GetDateTime(Baslangic + 7),
					}
				};
			}
			catch (InvalidCastException ex)
			{
				SDataModel = new SurecVeriModel<MailIcerikTablosuModel>{
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
				SDataModel = new SurecVeriModel<MailIcerikTablosuModel>{
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

		public virtual SurecVeriModel<IList<MailIcerikTablosuModel>> GonderimTipiBilgileri(int GonderimTipiID)
		{
			VTIslem.SetCommandText($"SELECT {MailIcerikTablosuModel.SQLSutunSorgusu} FROM MailIcerikTablosu WHERE GonderimTipiID=@GonderimTipiID");
			VTIslem.AddWithValue("GonderimTipiID", GonderimTipiID);
			VTIslem.OpenConnection();
			SModel = VTIslem.ExecuteReader(CommandBehavior.Default);
			if (SModel.Sonuc.Equals(Sonuclar.Basarili))
			{
				VeriListe = new List<MailIcerikTablosuModel>();
				while (SModel.Reader.Read())
				{
					if (KayitBilgisiAl().Sonuc.Equals(Sonuclar.Basarili))
					{
						VeriListe.Add(SDataModel.Veriler);
					}
					else
					{
						SDataListModel = new SurecVeriModel<IList<MailIcerikTablosuModel>>{
							Sonuc = SDataModel.Sonuc,
							KullaniciMesaji = SDataModel.KullaniciMesaji,
							HataBilgi = SDataModel.HataBilgi
						};
						VTIslem.CloseConnection();
						return SDataListModel;
					}
				}
				SDataListModel = new SurecVeriModel<IList<MailIcerikTablosuModel>>{
					Sonuc = Sonuclar.Basarili,
					KullaniciMesaji = "Veri listesi başarıyla çekildi",
					Veriler = VeriListe
				};
			}
			else
			{
				SDataListModel = new SurecVeriModel<IList<MailIcerikTablosuModel>>{
					Sonuc = SModel.Sonuc,
					KullaniciMesaji = SModel.KullaniciMesaji,
					HataBilgi = SModel.HataBilgi
				};
			}
			VTIslem.CloseConnection();
			return SDataListModel;
		}

		public virtual SurecVeriModel<IList<MailIcerikTablosuModel>> KatilimciTipiBilgileri(string KatilimciTipiID)
		{
			VTIslem.SetCommandText($"SELECT {MailIcerikTablosuModel.SQLSutunSorgusu} FROM MailIcerikTablosu WHERE KatilimciTipiID=@KatilimciTipiID");
			VTIslem.AddWithValue("KatilimciTipiID", KatilimciTipiID);
			VTIslem.OpenConnection();
			SModel = VTIslem.ExecuteReader(CommandBehavior.Default);
			if (SModel.Sonuc.Equals(Sonuclar.Basarili))
			{
				VeriListe = new List<MailIcerikTablosuModel>();
				while (SModel.Reader.Read())
				{
					if (KayitBilgisiAl().Sonuc.Equals(Sonuclar.Basarili))
					{
						VeriListe.Add(SDataModel.Veriler);
					}
					else
					{
						SDataListModel = new SurecVeriModel<IList<MailIcerikTablosuModel>>{
							Sonuc = SDataModel.Sonuc,
							KullaniciMesaji = SDataModel.KullaniciMesaji,
							HataBilgi = SDataModel.HataBilgi
						};
						VTIslem.CloseConnection();
						return SDataListModel;
					}
				}
				SDataListModel = new SurecVeriModel<IList<MailIcerikTablosuModel>>{
					Sonuc = Sonuclar.Basarili,
					KullaniciMesaji = "Veri listesi başarıyla çekildi",
					Veriler = VeriListe
				};
			}
			else
			{
				SDataListModel = new SurecVeriModel<IList<MailIcerikTablosuModel>>{
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