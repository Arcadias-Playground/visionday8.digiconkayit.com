using Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.OleDb;
using VeritabaniIslemSaglayici.Access;

namespace VeritabaniIslemMerkeziBase
{
	public abstract class YakaKartiCerceveTablosuIslemlerBase
	{
		public VTOperatorleri VTIslem;

		public List<YakaKartiCerceveTablosuModel> VeriListe;

		public SurecBilgiModel SModel;
		public SurecVeriModel<YakaKartiCerceveTablosuModel> SDataModel;
		public SurecVeriModel<IList<YakaKartiCerceveTablosuModel>> SDataListModel;

		public YakaKartiCerceveTablosuIslemlerBase()
		{
			VTIslem = new VTOperatorleri();
		}

		public YakaKartiCerceveTablosuIslemlerBase(OleDbTransaction Transcation)
		{
			VTIslem = new VTOperatorleri(Transcation);
		}

		public virtual SurecBilgiModel YeniKayitEkle(YakaKartiCerceveTablosuModel YeniKayit)
		{
			VTIslem.SetCommandText("INSERT INTO YakaKartiCerceveTablosu (KatilimciTipiID, Width, Height, YaziciKagitOrtalama, GuncellenmeTarihi, EklenmeTarihi) VALUES (@KatilimciTipiID, @Width, @Height, @YaziciKagitOrtalama, @GuncellenmeTarihi, @EklenmeTarihi)");
			VTIslem.AddWithValue("KatilimciTipiID", YeniKayit.KatilimciTipiID);
			VTIslem.AddWithValue("Width", YeniKayit.Width);
			VTIslem.AddWithValue("Height", YeniKayit.Height);
			VTIslem.AddWithValue("YaziciKagitOrtalama", YeniKayit.YaziciKagitOrtalama);
			VTIslem.AddWithValue("GuncellenmeTarihi", YeniKayit.GuncellenmeTarihi);
			VTIslem.AddWithValue("EklenmeTarihi", YeniKayit.EklenmeTarihi);
			return VTIslem.ExecuteNonQuery();
		}

		public virtual SurecBilgiModel KayitGuncelle(YakaKartiCerceveTablosuModel GuncelKayit)
		{
			VTIslem.SetCommandText("UPDATE YakaKartiCerceveTablosu SET KatilimciTipiID=@KatilimciTipiID, Width=@Width, Height=@Height, YaziciKagitOrtalama=@YaziciKagitOrtalama, GuncellenmeTarihi=@GuncellenmeTarihi, EklenmeTarihi=@EklenmeTarihi WHERE YakaKartiCerceveID=@YakaKartiCerceveID");
			VTIslem.AddWithValue("KatilimciTipiID", GuncelKayit.KatilimciTipiID);
			VTIslem.AddWithValue("Width", GuncelKayit.Width);
			VTIslem.AddWithValue("Height", GuncelKayit.Height);
			VTIslem.AddWithValue("YaziciKagitOrtalama", GuncelKayit.YaziciKagitOrtalama);
			VTIslem.AddWithValue("GuncellenmeTarihi", GuncelKayit.GuncellenmeTarihi);
			VTIslem.AddWithValue("EklenmeTarihi", GuncelKayit.EklenmeTarihi);
			VTIslem.AddWithValue("YakaKartiCerceveID", GuncelKayit.YakaKartiCerceveID);
			return VTIslem.ExecuteNonQuery();
		}

		public virtual SurecBilgiModel KayitSil(int YakaKartiCerceveID)
		{
			VTIslem.SetCommandText("DELETE FROM YakaKartiCerceveTablosu WHERE YakaKartiCerceveID=@YakaKartiCerceveID");
			VTIslem.AddWithValue("YakaKartiCerceveID", YakaKartiCerceveID);
			return VTIslem.ExecuteNonQuery();
		}

		public virtual SurecVeriModel<YakaKartiCerceveTablosuModel> KayitBilgisi(int YakaKartiCerceveID)
		{
			VTIslem.SetCommandText($"SELECT {YakaKartiCerceveTablosuModel.SQLSutunSorgusu} FROM YakaKartiCerceveTablosu WHERE YakaKartiCerceveID = @YakaKartiCerceveID");
			VTIslem.AddWithValue("YakaKartiCerceveID", YakaKartiCerceveID);
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
					SDataModel = new SurecVeriModel<YakaKartiCerceveTablosuModel>
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
				SDataModel = new SurecVeriModel<YakaKartiCerceveTablosuModel>
				{
					Sonuc = SModel.Sonuc,
					KullaniciMesaji = SModel.KullaniciMesaji,
					HataBilgi = SModel.HataBilgi
				};
			}
			VTIslem.CloseConnection();
			return SDataModel;
		}

		public virtual SurecVeriModel<IList<YakaKartiCerceveTablosuModel>> KayitBilgileri()
		{
			VTIslem.SetCommandText($"SELECT {YakaKartiCerceveTablosuModel.SQLSutunSorgusu} FROM YakaKartiCerceveTablosu");
			VTIslem.OpenConnection();
			SModel = VTIslem.ExecuteReader(CommandBehavior.Default);
			if (SModel.Sonuc.Equals(Sonuclar.Basarili))
			{
				VeriListe = new List<YakaKartiCerceveTablosuModel>();
				while (SModel.Reader.Read())
				{
					if (KayitBilgisiAl().Sonuc.Equals(Sonuclar.Basarili))
					{
						VeriListe.Add(SDataModel.Veriler);
					}
					else
					{
						SDataListModel = new SurecVeriModel<IList<YakaKartiCerceveTablosuModel>>{
							Sonuc = SDataModel.Sonuc,
							KullaniciMesaji = SDataModel.KullaniciMesaji,
							HataBilgi = SDataModel.HataBilgi
						};
						VTIslem.CloseConnection();
						return SDataListModel;
					}
				}
				SDataListModel = new SurecVeriModel<IList<YakaKartiCerceveTablosuModel>>{
					Sonuc = Sonuclar.Basarili,
					KullaniciMesaji = "Veri listesi başarıyla çekildi",
					Veriler = VeriListe
				};
			}
			else
			{
				SDataListModel = new SurecVeriModel<IList<YakaKartiCerceveTablosuModel>>{
					Sonuc = SModel.Sonuc,
					KullaniciMesaji = SModel.KullaniciMesaji,
					HataBilgi = SModel.HataBilgi
				};
			}
			VTIslem.CloseConnection();
			return SDataListModel;
		}

		SurecVeriModel<YakaKartiCerceveTablosuModel> KayitBilgisiAl()
		{
			try
			{
				SDataModel = new SurecVeriModel<YakaKartiCerceveTablosuModel>{
					Sonuc = Sonuclar.Basarili,
					KullaniciMesaji = "Veri bilgisi başarıyla çekilmiştir.",
					Veriler = new YakaKartiCerceveTablosuModel
					{
						YakaKartiCerceveID = SModel.Reader.GetInt32(0),
						KatilimciTipiID = SModel.Reader.GetString(1),
						Width = SModel.Reader.GetInt32(2),
						Height = SModel.Reader.GetInt32(3),
						YaziciKagitOrtalama = SModel.Reader.GetBoolean(4),
						GuncellenmeTarihi = SModel.Reader.GetDateTime(5),
						EklenmeTarihi = SModel.Reader.GetDateTime(6),
					}
				};

			}
			catch (InvalidCastException ex)
			{
				SDataModel = new SurecVeriModel<YakaKartiCerceveTablosuModel>{
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
				SDataModel = new SurecVeriModel<YakaKartiCerceveTablosuModel>{
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

		public virtual SurecVeriModel<YakaKartiCerceveTablosuModel> KayitBilgisiAl(int Baslangic, DbDataReader Reader)
		{
			try
			{
				SDataModel = new SurecVeriModel<YakaKartiCerceveTablosuModel>{
					Sonuc = Sonuclar.Basarili,
					KullaniciMesaji = "Veri bilgisi başarıyla çekilmiştir.",
					Veriler = new YakaKartiCerceveTablosuModel{
						YakaKartiCerceveID = Reader.GetInt32(Baslangic + 0),
						KatilimciTipiID = Reader.GetString(Baslangic + 1),
						Width = Reader.GetInt32(Baslangic + 2),
						Height = Reader.GetInt32(Baslangic + 3),
						YaziciKagitOrtalama = Reader.GetBoolean(Baslangic + 4),
						GuncellenmeTarihi = Reader.GetDateTime(Baslangic + 5),
						EklenmeTarihi = Reader.GetDateTime(Baslangic + 6),
					}
				};
			}
			catch (InvalidCastException ex)
			{
				SDataModel = new SurecVeriModel<YakaKartiCerceveTablosuModel>{
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
				SDataModel = new SurecVeriModel<YakaKartiCerceveTablosuModel>{
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

		public virtual SurecVeriModel<YakaKartiCerceveTablosuModel> KatilimciTipiBilgisi(string KatilimciTipiID)
		{
			VTIslem.SetCommandText($"SELECT {YakaKartiCerceveTablosuModel.SQLSutunSorgusu}FROM YakaKartiCerceveTablosu WHERE KatilimciTipiID=@KatilimciTipiID");
			VTIslem.AddWithValue("KatilimciTipiID", KatilimciTipiID);
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
					SDataModel = new SurecVeriModel<YakaKartiCerceveTablosuModel>
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
				SDataModel = new SurecVeriModel<YakaKartiCerceveTablosuModel>
				{
					Sonuc = SModel.Sonuc,
					KullaniciMesaji = SModel.KullaniciMesaji,
					HataBilgi = SModel.HataBilgi
				};
			}
			VTIslem.CloseConnection();
			return SDataModel;
		}

	}
}