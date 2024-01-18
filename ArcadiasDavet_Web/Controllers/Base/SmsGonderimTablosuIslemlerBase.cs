using Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.OleDb;
using VeritabaniIslemSaglayici.Access;

namespace VeritabaniIslemMerkeziBase
{
	public abstract class SmsGonderimTablosuIslemlerBase
	{
		public VTOperatorleri VTIslem;

		public List<SmsGonderimTablosuModel> VeriListe;

		public SurecBilgiModel SModel;
		public SurecVeriModel<SmsGonderimTablosuModel> SDataModel;
		public SurecVeriModel<IList<SmsGonderimTablosuModel>> SDataListModel;

		public SmsGonderimTablosuIslemlerBase()
		{
			VTIslem = new VTOperatorleri();
		}

		public SmsGonderimTablosuIslemlerBase(OleDbTransaction Transcation)
		{
			VTIslem = new VTOperatorleri(Transcation);
		}

		public virtual SurecBilgiModel YeniKayitEkle(SmsGonderimTablosuModel YeniKayit)
		{
			VTIslem.SetCommandText("INSERT INTO SmsGonderimTablosu (SmsGonderimID, KatilimciID, SmsIcerikID, Telefon, Durum, EklenmeTarihi) VALUES (@SmsGonderimID, @KatilimciID, @SmsIcerikID, @Telefon, @Durum, @EklenmeTarihi)");
			VTIslem.AddWithValue("SmsGonderimID", YeniKayit.SmsGonderimID);
			VTIslem.AddWithValue("KatilimciID", YeniKayit.KatilimciID);
			VTIslem.AddWithValue("SmsIcerikID", YeniKayit.SmsIcerikID);
			VTIslem.AddWithValue("Telefon", YeniKayit.Telefon);
			VTIslem.AddWithValue("Durum", YeniKayit.Durum);
			VTIslem.AddWithValue("EklenmeTarihi", YeniKayit.EklenmeTarihi);
			return VTIslem.ExecuteNonQuery();
		}

		public virtual SurecBilgiModel KayitGuncelle(SmsGonderimTablosuModel GuncelKayit)
		{
			VTIslem.SetCommandText("UPDATE SmsGonderimTablosu SET KatilimciID=@KatilimciID, SmsIcerikID=@SmsIcerikID, Telefon=@Telefon, Durum=@Durum, EklenmeTarihi=@EklenmeTarihi WHERE SmsGonderimID=@SmsGonderimID");
			VTIslem.AddWithValue("KatilimciID", GuncelKayit.KatilimciID);
			VTIslem.AddWithValue("SmsIcerikID", GuncelKayit.SmsIcerikID);
			VTIslem.AddWithValue("Telefon", GuncelKayit.Telefon);
			VTIslem.AddWithValue("Durum", GuncelKayit.Durum);
			VTIslem.AddWithValue("EklenmeTarihi", GuncelKayit.EklenmeTarihi);
			VTIslem.AddWithValue("SmsGonderimID", GuncelKayit.SmsGonderimID);
			return VTIslem.ExecuteNonQuery();
		}

		public virtual SurecBilgiModel KayitSil(string SmsGonderimID)
		{
			VTIslem.SetCommandText("DELETE FROM SmsGonderimTablosu WHERE SmsGonderimID=@SmsGonderimID");
			VTIslem.AddWithValue("SmsGonderimID", SmsGonderimID);
			return VTIslem.ExecuteNonQuery();
		}

		public virtual SurecVeriModel<SmsGonderimTablosuModel> KayitBilgisi(string SmsGonderimID)
		{
			VTIslem.SetCommandText($"SELECT {SmsGonderimTablosuModel.SQLSutunSorgusu} FROM SmsGonderimTablosu WHERE SmsGonderimID = @SmsGonderimID");
			VTIslem.AddWithValue("SmsGonderimID", SmsGonderimID);
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
					SDataModel = new SurecVeriModel<SmsGonderimTablosuModel>
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
				SDataModel = new SurecVeriModel<SmsGonderimTablosuModel>
				{
					Sonuc = SModel.Sonuc,
					KullaniciMesaji = SModel.KullaniciMesaji,
					HataBilgi = SModel.HataBilgi
				};
			}
			VTIslem.CloseConnection();
			return SDataModel;
		}

		public virtual SurecVeriModel<IList<SmsGonderimTablosuModel>> KayitBilgileri()
		{
			VTIslem.SetCommandText($"SELECT {SmsGonderimTablosuModel.SQLSutunSorgusu} FROM SmsGonderimTablosu");
			VTIslem.OpenConnection();
			SModel = VTIslem.ExecuteReader(CommandBehavior.Default);
			if (SModel.Sonuc.Equals(Sonuclar.Basarili))
			{
				VeriListe = new List<SmsGonderimTablosuModel>();
				while (SModel.Reader.Read())
				{
					if (KayitBilgisiAl().Sonuc.Equals(Sonuclar.Basarili))
					{
						VeriListe.Add(SDataModel.Veriler);
					}
					else
					{
						SDataListModel = new SurecVeriModel<IList<SmsGonderimTablosuModel>>{
							Sonuc = SDataModel.Sonuc,
							KullaniciMesaji = SDataModel.KullaniciMesaji,
							HataBilgi = SDataModel.HataBilgi
						};
						VTIslem.CloseConnection();
						return SDataListModel;
					}
				}
				SDataListModel = new SurecVeriModel<IList<SmsGonderimTablosuModel>>{
					Sonuc = Sonuclar.Basarili,
					KullaniciMesaji = "Veri listesi başarıyla çekildi",
					Veriler = VeriListe
				};
			}
			else
			{
				SDataListModel = new SurecVeriModel<IList<SmsGonderimTablosuModel>>{
					Sonuc = SModel.Sonuc,
					KullaniciMesaji = SModel.KullaniciMesaji,
					HataBilgi = SModel.HataBilgi
				};
			}
			VTIslem.CloseConnection();
			return SDataListModel;
		}

		SurecVeriModel<SmsGonderimTablosuModel> KayitBilgisiAl()
		{
			try
			{
				SDataModel = new SurecVeriModel<SmsGonderimTablosuModel>{
					Sonuc = Sonuclar.Basarili,
					KullaniciMesaji = "Veri bilgisi başarıyla çekilmiştir.",
					Veriler = new SmsGonderimTablosuModel
					{
						SmsGonderimID = SModel.Reader.GetString(0),
						KatilimciID = SModel.Reader.GetString(1),
						SmsIcerikID = SModel.Reader.GetInt32(2),
						Telefon = SModel.Reader.GetString(3),
						Durum = SModel.Reader.GetBoolean(4),
						EklenmeTarihi = SModel.Reader.GetDateTime(5),
					}
				};

			}
			catch (InvalidCastException ex)
			{
				SDataModel = new SurecVeriModel<SmsGonderimTablosuModel>{
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
				SDataModel = new SurecVeriModel<SmsGonderimTablosuModel>{
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

		public virtual SurecVeriModel<SmsGonderimTablosuModel> KayitBilgisiAl(int Baslangic, DbDataReader Reader)
		{
			try
			{
				SDataModel = new SurecVeriModel<SmsGonderimTablosuModel>{
					Sonuc = Sonuclar.Basarili,
					KullaniciMesaji = "Veri bilgisi başarıyla çekilmiştir.",
					Veriler = new SmsGonderimTablosuModel{
						SmsGonderimID = Reader.GetString(Baslangic + 0),
						KatilimciID = Reader.GetString(Baslangic + 1),
						SmsIcerikID = Reader.GetInt32(Baslangic + 2),
						Telefon = Reader.GetString(Baslangic + 3),
						Durum = Reader.GetBoolean(Baslangic + 4),
						EklenmeTarihi = Reader.GetDateTime(Baslangic + 5),
					}
				};
			}
			catch (InvalidCastException ex)
			{
				SDataModel = new SurecVeriModel<SmsGonderimTablosuModel>{
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
				SDataModel = new SurecVeriModel<SmsGonderimTablosuModel>{
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

		public virtual SurecVeriModel<IList<SmsGonderimTablosuModel>> KatilimciBilgileri(string KatilimciID)
		{
			VTIslem.SetCommandText($"SELECT {SmsGonderimTablosuModel.SQLSutunSorgusu} FROM SmsGonderimTablosu WHERE KatilimciID=@KatilimciID");
			VTIslem.AddWithValue("KatilimciID", KatilimciID);
			VTIslem.OpenConnection();
			SModel = VTIslem.ExecuteReader(CommandBehavior.Default);
			if (SModel.Sonuc.Equals(Sonuclar.Basarili))
			{
				VeriListe = new List<SmsGonderimTablosuModel>();
				while (SModel.Reader.Read())
				{
					if (KayitBilgisiAl().Sonuc.Equals(Sonuclar.Basarili))
					{
						VeriListe.Add(SDataModel.Veriler);
					}
					else
					{
						SDataListModel = new SurecVeriModel<IList<SmsGonderimTablosuModel>>{
							Sonuc = SDataModel.Sonuc,
							KullaniciMesaji = SDataModel.KullaniciMesaji,
							HataBilgi = SDataModel.HataBilgi
						};
						VTIslem.CloseConnection();
						return SDataListModel;
					}
				}
				SDataListModel = new SurecVeriModel<IList<SmsGonderimTablosuModel>>{
					Sonuc = Sonuclar.Basarili,
					KullaniciMesaji = "Veri listesi başarıyla çekildi",
					Veriler = VeriListe
				};
			}
			else
			{
				SDataListModel = new SurecVeriModel<IList<SmsGonderimTablosuModel>>{
					Sonuc = SModel.Sonuc,
					KullaniciMesaji = SModel.KullaniciMesaji,
					HataBilgi = SModel.HataBilgi
				};
			}
			VTIslem.CloseConnection();
			return SDataListModel;
		}

		public virtual SurecVeriModel<IList<SmsGonderimTablosuModel>> SmsIcerikBilgileri(int SmsIcerikID)
		{
			VTIslem.SetCommandText($"SELECT {SmsGonderimTablosuModel.SQLSutunSorgusu} FROM SmsGonderimTablosu WHERE SmsIcerikID=@SmsIcerikID");
			VTIslem.AddWithValue("SmsIcerikID", SmsIcerikID);
			VTIslem.OpenConnection();
			SModel = VTIslem.ExecuteReader(CommandBehavior.Default);
			if (SModel.Sonuc.Equals(Sonuclar.Basarili))
			{
				VeriListe = new List<SmsGonderimTablosuModel>();
				while (SModel.Reader.Read())
				{
					if (KayitBilgisiAl().Sonuc.Equals(Sonuclar.Basarili))
					{
						VeriListe.Add(SDataModel.Veriler);
					}
					else
					{
						SDataListModel = new SurecVeriModel<IList<SmsGonderimTablosuModel>>{
							Sonuc = SDataModel.Sonuc,
							KullaniciMesaji = SDataModel.KullaniciMesaji,
							HataBilgi = SDataModel.HataBilgi
						};
						VTIslem.CloseConnection();
						return SDataListModel;
					}
				}
				SDataListModel = new SurecVeriModel<IList<SmsGonderimTablosuModel>>{
					Sonuc = Sonuclar.Basarili,
					KullaniciMesaji = "Veri listesi başarıyla çekildi",
					Veriler = VeriListe
				};
			}
			else
			{
				SDataListModel = new SurecVeriModel<IList<SmsGonderimTablosuModel>>{
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