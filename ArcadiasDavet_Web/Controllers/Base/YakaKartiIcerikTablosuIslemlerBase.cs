using Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.OleDb;
using VeritabaniIslemSaglayici.Access;

namespace VeritabaniIslemMerkeziBase
{
	public abstract class YakaKartiIcerikTablosuIslemlerBase
	{
		public VTOperatorleri VTIslem;

		public List<YakaKartiIcerikTablosuModel> VeriListe;

		public SurecBilgiModel SModel;
		public SurecVeriModel<YakaKartiIcerikTablosuModel> SDataModel;
		public SurecVeriModel<IList<YakaKartiIcerikTablosuModel>> SDataListModel;

		public YakaKartiIcerikTablosuIslemlerBase()
		{
			VTIslem = new VTOperatorleri();
		}

		public YakaKartiIcerikTablosuIslemlerBase(OleDbTransaction Transcation)
		{
			VTIslem = new VTOperatorleri(Transcation);
		}

		public virtual SurecBilgiModel YeniKayitEkle(YakaKartiIcerikTablosuModel YeniKayit)
		{
			VTIslem.SetCommandText("INSERT INTO YakaKartiIcerikTablosu (YakaKartiCerceveID, YakaKartiIcerikTipiID, X, Y, Width, Height, GuncellenmeTarihi, EklenmeTarihi) VALUES (@YakaKartiCerceveID, @YakaKartiIcerikTipiID, @X, @Y, @Width, @Height, @GuncellenmeTarihi, @EklenmeTarihi)");
			VTIslem.AddWithValue("YakaKartiCerceveID", YeniKayit.YakaKartiCerceveID);
			VTIslem.AddWithValue("YakaKartiIcerikTipiID", YeniKayit.YakaKartiIcerikTipiID);
			VTIslem.AddWithValue("X", YeniKayit.X);
			VTIslem.AddWithValue("Y", YeniKayit.Y);
			VTIslem.AddWithValue("Width", YeniKayit.Width);
			VTIslem.AddWithValue("Height", YeniKayit.Height);
			VTIslem.AddWithValue("GuncellenmeTarihi", YeniKayit.GuncellenmeTarihi);
			VTIslem.AddWithValue("EklenmeTarihi", YeniKayit.EklenmeTarihi);
			return VTIslem.ExecuteNonQuery();
		}

		public virtual SurecBilgiModel KayitGuncelle(YakaKartiIcerikTablosuModel GuncelKayit)
		{
			VTIslem.SetCommandText("UPDATE YakaKartiIcerikTablosu SET YakaKartiCerceveID=@YakaKartiCerceveID, YakaKartiIcerikTipiID=@YakaKartiIcerikTipiID, X=@X, Y=@Y, Width=@Width, Height=@Height, GuncellenmeTarihi=@GuncellenmeTarihi, EklenmeTarihi=@EklenmeTarihi WHERE YakaKartiIcerikID=@YakaKartiIcerikID");
			VTIslem.AddWithValue("YakaKartiCerceveID", GuncelKayit.YakaKartiCerceveID);
			VTIslem.AddWithValue("YakaKartiIcerikTipiID", GuncelKayit.YakaKartiIcerikTipiID);
			VTIslem.AddWithValue("X", GuncelKayit.X);
			VTIslem.AddWithValue("Y", GuncelKayit.Y);
			VTIslem.AddWithValue("Width", GuncelKayit.Width);
			VTIslem.AddWithValue("Height", GuncelKayit.Height);
			VTIslem.AddWithValue("GuncellenmeTarihi", GuncelKayit.GuncellenmeTarihi);
			VTIslem.AddWithValue("EklenmeTarihi", GuncelKayit.EklenmeTarihi);
			VTIslem.AddWithValue("YakaKartiIcerikID", GuncelKayit.YakaKartiIcerikID);
			return VTIslem.ExecuteNonQuery();
		}

		public virtual SurecBilgiModel KayitSil(int YakaKartiIcerikID)
		{
			VTIslem.SetCommandText("DELETE FROM YakaKartiIcerikTablosu WHERE YakaKartiIcerikID=@YakaKartiIcerikID");
			VTIslem.AddWithValue("YakaKartiIcerikID", YakaKartiIcerikID);
			return VTIslem.ExecuteNonQuery();
		}

		public virtual SurecVeriModel<YakaKartiIcerikTablosuModel> KayitBilgisi(int YakaKartiIcerikID)
		{
			VTIslem.SetCommandText($"SELECT {YakaKartiIcerikTablosuModel.SQLSutunSorgusu} FROM YakaKartiIcerikTablosu WHERE YakaKartiIcerikID = @YakaKartiIcerikID");
			VTIslem.AddWithValue("YakaKartiIcerikID", YakaKartiIcerikID);
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
					SDataModel = new SurecVeriModel<YakaKartiIcerikTablosuModel>
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
				SDataModel = new SurecVeriModel<YakaKartiIcerikTablosuModel>
				{
					Sonuc = SModel.Sonuc,
					KullaniciMesaji = SModel.KullaniciMesaji,
					HataBilgi = SModel.HataBilgi
				};
			}
			VTIslem.CloseConnection();
			return SDataModel;
		}

		public virtual SurecVeriModel<IList<YakaKartiIcerikTablosuModel>> KayitBilgileri()
		{
			VTIslem.SetCommandText($"SELECT {YakaKartiIcerikTablosuModel.SQLSutunSorgusu} FROM YakaKartiIcerikTablosu");
			VTIslem.OpenConnection();
			SModel = VTIslem.ExecuteReader(CommandBehavior.Default);
			if (SModel.Sonuc.Equals(Sonuclar.Basarili))
			{
				VeriListe = new List<YakaKartiIcerikTablosuModel>();
				while (SModel.Reader.Read())
				{
					if (KayitBilgisiAl().Sonuc.Equals(Sonuclar.Basarili))
					{
						VeriListe.Add(SDataModel.Veriler);
					}
					else
					{
						SDataListModel = new SurecVeriModel<IList<YakaKartiIcerikTablosuModel>>{
							Sonuc = SDataModel.Sonuc,
							KullaniciMesaji = SDataModel.KullaniciMesaji,
							HataBilgi = SDataModel.HataBilgi
						};
						VTIslem.CloseConnection();
						return SDataListModel;
					}
				}
				SDataListModel = new SurecVeriModel<IList<YakaKartiIcerikTablosuModel>>{
					Sonuc = Sonuclar.Basarili,
					KullaniciMesaji = "Veri listesi başarıyla çekildi",
					Veriler = VeriListe
				};
			}
			else
			{
				SDataListModel = new SurecVeriModel<IList<YakaKartiIcerikTablosuModel>>{
					Sonuc = SModel.Sonuc,
					KullaniciMesaji = SModel.KullaniciMesaji,
					HataBilgi = SModel.HataBilgi
				};
			}
			VTIslem.CloseConnection();
			return SDataListModel;
		}

		SurecVeriModel<YakaKartiIcerikTablosuModel> KayitBilgisiAl()
		{
			try
			{
				SDataModel = new SurecVeriModel<YakaKartiIcerikTablosuModel>{
					Sonuc = Sonuclar.Basarili,
					KullaniciMesaji = "Veri bilgisi başarıyla çekilmiştir.",
					Veriler = new YakaKartiIcerikTablosuModel
					{
						YakaKartiIcerikID = SModel.Reader.GetInt32(0),
						YakaKartiCerceveID = SModel.Reader.GetInt32(1),
						YakaKartiIcerikTipiID = SModel.Reader.GetInt32(2),
						X = SModel.Reader.GetInt32(3),
						Y = SModel.Reader.GetInt32(4),
						Width = SModel.Reader.GetInt32(5),
						Height = SModel.Reader.GetInt32(6),
						GuncellenmeTarihi = SModel.Reader.GetDateTime(7),
						EklenmeTarihi = SModel.Reader.GetDateTime(8),
					}
				};

			}
			catch (InvalidCastException ex)
			{
				SDataModel = new SurecVeriModel<YakaKartiIcerikTablosuModel>{
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
				SDataModel = new SurecVeriModel<YakaKartiIcerikTablosuModel>{
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

		public virtual SurecVeriModel<YakaKartiIcerikTablosuModel> KayitBilgisiAl(int Baslangic, DbDataReader Reader)
		{
			try
			{
				SDataModel = new SurecVeriModel<YakaKartiIcerikTablosuModel>{
					Sonuc = Sonuclar.Basarili,
					KullaniciMesaji = "Veri bilgisi başarıyla çekilmiştir.",
					Veriler = new YakaKartiIcerikTablosuModel{
						YakaKartiIcerikID = Reader.GetInt32(Baslangic + 0),
						YakaKartiCerceveID = Reader.GetInt32(Baslangic + 1),
						YakaKartiIcerikTipiID = Reader.GetInt32(Baslangic + 2),
						X = Reader.GetInt32(Baslangic + 3),
						Y = Reader.GetInt32(Baslangic + 4),
						Width = Reader.GetInt32(Baslangic + 5),
						Height = Reader.GetInt32(Baslangic + 6),
						GuncellenmeTarihi = Reader.GetDateTime(Baslangic + 7),
						EklenmeTarihi = Reader.GetDateTime(Baslangic + 8),
					}
				};
			}
			catch (InvalidCastException ex)
			{
				SDataModel = new SurecVeriModel<YakaKartiIcerikTablosuModel>{
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
				SDataModel = new SurecVeriModel<YakaKartiIcerikTablosuModel>{
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

		public virtual SurecVeriModel<IList<YakaKartiIcerikTablosuModel>> YakaKartiCerceveBilgileri(int YakaKartiCerceveID)
		{
			VTIslem.SetCommandText($"SELECT {YakaKartiIcerikTablosuModel.SQLSutunSorgusu} FROM YakaKartiIcerikTablosu WHERE YakaKartiCerceveID=@YakaKartiCerceveID");
			VTIslem.AddWithValue("YakaKartiCerceveID", YakaKartiCerceveID);
			VTIslem.OpenConnection();
			SModel = VTIslem.ExecuteReader(CommandBehavior.Default);
			if (SModel.Sonuc.Equals(Sonuclar.Basarili))
			{
				VeriListe = new List<YakaKartiIcerikTablosuModel>();
				while (SModel.Reader.Read())
				{
					if (KayitBilgisiAl().Sonuc.Equals(Sonuclar.Basarili))
					{
						VeriListe.Add(SDataModel.Veriler);
					}
					else
					{
						SDataListModel = new SurecVeriModel<IList<YakaKartiIcerikTablosuModel>>{
							Sonuc = SDataModel.Sonuc,
							KullaniciMesaji = SDataModel.KullaniciMesaji,
							HataBilgi = SDataModel.HataBilgi
						};
						VTIslem.CloseConnection();
						return SDataListModel;
					}
				}
				SDataListModel = new SurecVeriModel<IList<YakaKartiIcerikTablosuModel>>{
					Sonuc = Sonuclar.Basarili,
					KullaniciMesaji = "Veri listesi başarıyla çekildi",
					Veriler = VeriListe
				};
			}
			else
			{
				SDataListModel = new SurecVeriModel<IList<YakaKartiIcerikTablosuModel>>{
					Sonuc = SModel.Sonuc,
					KullaniciMesaji = SModel.KullaniciMesaji,
					HataBilgi = SModel.HataBilgi
				};
			}
			VTIslem.CloseConnection();
			return SDataListModel;
		}

		public virtual SurecVeriModel<IList<YakaKartiIcerikTablosuModel>> YakaKartiIcerikTipiBilgileri(int YakaKartiIcerikTipiID)
		{
			VTIslem.SetCommandText($"SELECT {YakaKartiIcerikTablosuModel.SQLSutunSorgusu} FROM YakaKartiIcerikTablosu WHERE YakaKartiIcerikTipiID=@YakaKartiIcerikTipiID");
			VTIslem.AddWithValue("YakaKartiIcerikTipiID", YakaKartiIcerikTipiID);
			VTIslem.OpenConnection();
			SModel = VTIslem.ExecuteReader(CommandBehavior.Default);
			if (SModel.Sonuc.Equals(Sonuclar.Basarili))
			{
				VeriListe = new List<YakaKartiIcerikTablosuModel>();
				while (SModel.Reader.Read())
				{
					if (KayitBilgisiAl().Sonuc.Equals(Sonuclar.Basarili))
					{
						VeriListe.Add(SDataModel.Veriler);
					}
					else
					{
						SDataListModel = new SurecVeriModel<IList<YakaKartiIcerikTablosuModel>>{
							Sonuc = SDataModel.Sonuc,
							KullaniciMesaji = SDataModel.KullaniciMesaji,
							HataBilgi = SDataModel.HataBilgi
						};
						VTIslem.CloseConnection();
						return SDataListModel;
					}
				}
				SDataListModel = new SurecVeriModel<IList<YakaKartiIcerikTablosuModel>>{
					Sonuc = Sonuclar.Basarili,
					KullaniciMesaji = "Veri listesi başarıyla çekildi",
					Veriler = VeriListe
				};
			}
			else
			{
				SDataListModel = new SurecVeriModel<IList<YakaKartiIcerikTablosuModel>>{
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