using Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.OleDb;
using VeritabaniIslemSaglayici.Access;

namespace VeritabaniIslemMerkeziBase
{
	public abstract class SmsIcerikTablosuIslemlerBase
	{
		public VTOperatorleri VTIslem;

		public List<SmsIcerikTablosuModel> VeriListe;

		public SurecBilgiModel SModel;
		public SurecVeriModel<SmsIcerikTablosuModel> SDataModel;
		public SurecVeriModel<IList<SmsIcerikTablosuModel>> SDataListModel;

		public SmsIcerikTablosuIslemlerBase()
		{
			VTIslem = new VTOperatorleri();
		}

		public SmsIcerikTablosuIslemlerBase(OleDbTransaction Transcation)
		{
			VTIslem = new VTOperatorleri(Transcation);
		}

		public virtual SurecBilgiModel YeniKayitEkle(SmsIcerikTablosuModel YeniKayit)
		{
			VTIslem.SetCommandText("INSERT INTO SmsIcerikTablosu (KatilimciTipiID, GonderimTipiID, SmsIcerik, AnaKatilimci, GuncellenmeTarihi, EklenmeTarihi) VALUES (@KatilimciTipiID, @GonderimTipiID, @SmsIcerik, @AnaKatilimci, @GuncellenmeTarihi, @EklenmeTarihi)");
			VTIslem.AddWithValue("KatilimciTipiID", YeniKayit.KatilimciTipiID);
			VTIslem.AddWithValue("GonderimTipiID", YeniKayit.GonderimTipiID);
			VTIslem.AddWithValue("SmsIcerik", YeniKayit.SmsIcerik);
			VTIslem.AddWithValue("AnaKatilimci", YeniKayit.AnaKatilimci);
			VTIslem.AddWithValue("GuncellenmeTarihi", YeniKayit.GuncellenmeTarihi);
			VTIslem.AddWithValue("EklenmeTarihi", YeniKayit.EklenmeTarihi);
			return VTIslem.ExecuteNonQuery();
		}

		public virtual SurecBilgiModel KayitGuncelle(SmsIcerikTablosuModel GuncelKayit)
		{
			VTIslem.SetCommandText("UPDATE SmsIcerikTablosu SET KatilimciTipiID=@KatilimciTipiID, GonderimTipiID=@GonderimTipiID, SmsIcerik=@SmsIcerik, AnaKatilimci=@AnaKatilimci, GuncellenmeTarihi=@GuncellenmeTarihi, EklenmeTarihi=@EklenmeTarihi WHERE SmsIcerikID=@SmsIcerikID");
			VTIslem.AddWithValue("KatilimciTipiID", GuncelKayit.KatilimciTipiID);
			VTIslem.AddWithValue("GonderimTipiID", GuncelKayit.GonderimTipiID);
			VTIslem.AddWithValue("SmsIcerik", GuncelKayit.SmsIcerik);
			VTIslem.AddWithValue("AnaKatilimci", GuncelKayit.AnaKatilimci);
			VTIslem.AddWithValue("GuncellenmeTarihi", GuncelKayit.GuncellenmeTarihi);
			VTIslem.AddWithValue("EklenmeTarihi", GuncelKayit.EklenmeTarihi);
			VTIslem.AddWithValue("SmsIcerikID", GuncelKayit.SmsIcerikID);
			return VTIslem.ExecuteNonQuery();
		}

		public virtual SurecBilgiModel KayitSil(int SmsIcerikID)
		{
			VTIslem.SetCommandText("DELETE FROM SmsIcerikTablosu WHERE SmsIcerikID=@SmsIcerikID");
			VTIslem.AddWithValue("SmsIcerikID", SmsIcerikID);
			return VTIslem.ExecuteNonQuery();
		}

		public virtual SurecVeriModel<SmsIcerikTablosuModel> KayitBilgisi(int SmsIcerikID)
		{
			VTIslem.SetCommandText($"SELECT {SmsIcerikTablosuModel.SQLSutunSorgusu} FROM SmsIcerikTablosu WHERE SmsIcerikID = @SmsIcerikID");
			VTIslem.AddWithValue("SmsIcerikID", SmsIcerikID);
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
					SDataModel = new SurecVeriModel<SmsIcerikTablosuModel>
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
				SDataModel = new SurecVeriModel<SmsIcerikTablosuModel>
				{
					Sonuc = SModel.Sonuc,
					KullaniciMesaji = SModel.KullaniciMesaji,
					HataBilgi = SModel.HataBilgi
				};
			}
			VTIslem.CloseConnection();
			return SDataModel;
		}

		public virtual SurecVeriModel<IList<SmsIcerikTablosuModel>> KayitBilgileri()
		{
			VTIslem.SetCommandText($"SELECT {SmsIcerikTablosuModel.SQLSutunSorgusu} FROM SmsIcerikTablosu");
			VTIslem.OpenConnection();
			SModel = VTIslem.ExecuteReader(CommandBehavior.Default);
			if (SModel.Sonuc.Equals(Sonuclar.Basarili))
			{
				VeriListe = new List<SmsIcerikTablosuModel>();
				while (SModel.Reader.Read())
				{
					if (KayitBilgisiAl().Sonuc.Equals(Sonuclar.Basarili))
					{
						VeriListe.Add(SDataModel.Veriler);
					}
					else
					{
						SDataListModel = new SurecVeriModel<IList<SmsIcerikTablosuModel>>{
							Sonuc = SDataModel.Sonuc,
							KullaniciMesaji = SDataModel.KullaniciMesaji,
							HataBilgi = SDataModel.HataBilgi
						};
						VTIslem.CloseConnection();
						return SDataListModel;
					}
				}
				SDataListModel = new SurecVeriModel<IList<SmsIcerikTablosuModel>>{
					Sonuc = Sonuclar.Basarili,
					KullaniciMesaji = "Veri listesi başarıyla çekildi",
					Veriler = VeriListe
				};
			}
			else
			{
				SDataListModel = new SurecVeriModel<IList<SmsIcerikTablosuModel>>{
					Sonuc = SModel.Sonuc,
					KullaniciMesaji = SModel.KullaniciMesaji,
					HataBilgi = SModel.HataBilgi
				};
			}
			VTIslem.CloseConnection();
			return SDataListModel;
		}

		SurecVeriModel<SmsIcerikTablosuModel> KayitBilgisiAl()
		{
			try
			{
				SDataModel = new SurecVeriModel<SmsIcerikTablosuModel>{
					Sonuc = Sonuclar.Basarili,
					KullaniciMesaji = "Veri bilgisi başarıyla çekilmiştir.",
					Veriler = new SmsIcerikTablosuModel
					{
						SmsIcerikID = SModel.Reader.GetInt32(0),
						KatilimciTipiID = SModel.Reader.GetString(1),
						GonderimTipiID = SModel.Reader.GetInt32(2),
						SmsIcerik = SModel.Reader.GetString(3),
						AnaKatilimci = SModel.Reader.GetBoolean(4),
						GuncellenmeTarihi = SModel.Reader.GetDateTime(5),
						EklenmeTarihi = SModel.Reader.GetDateTime(6),
					}
				};

			}
			catch (InvalidCastException ex)
			{
				SDataModel = new SurecVeriModel<SmsIcerikTablosuModel>{
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
				SDataModel = new SurecVeriModel<SmsIcerikTablosuModel>{
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

		public virtual SurecVeriModel<SmsIcerikTablosuModel> KayitBilgisiAl(int Baslangic, DbDataReader Reader)
		{
			try
			{
				SDataModel = new SurecVeriModel<SmsIcerikTablosuModel>{
					Sonuc = Sonuclar.Basarili,
					KullaniciMesaji = "Veri bilgisi başarıyla çekilmiştir.",
					Veriler = new SmsIcerikTablosuModel{
						SmsIcerikID = Reader.GetInt32(Baslangic + 0),
						KatilimciTipiID = Reader.GetString(Baslangic + 1),
						GonderimTipiID = Reader.GetInt32(Baslangic + 2),
						SmsIcerik = Reader.GetString(Baslangic + 3),
						AnaKatilimci = Reader.GetBoolean(Baslangic + 4),
						GuncellenmeTarihi = Reader.GetDateTime(Baslangic + 5),
						EklenmeTarihi = Reader.GetDateTime(Baslangic + 6),
					}
				};
			}
			catch (InvalidCastException ex)
			{
				SDataModel = new SurecVeriModel<SmsIcerikTablosuModel>{
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
				SDataModel = new SurecVeriModel<SmsIcerikTablosuModel>{
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

		public virtual SurecVeriModel<IList<SmsIcerikTablosuModel>> KatilimciTipiBilgileri(string KatilimciTipiID)
		{
			VTIslem.SetCommandText($"SELECT {SmsIcerikTablosuModel.SQLSutunSorgusu} FROM SmsIcerikTablosu WHERE KatilimciTipiID=@KatilimciTipiID");
			VTIslem.AddWithValue("KatilimciTipiID", KatilimciTipiID);
			VTIslem.OpenConnection();
			SModel = VTIslem.ExecuteReader(CommandBehavior.Default);
			if (SModel.Sonuc.Equals(Sonuclar.Basarili))
			{
				VeriListe = new List<SmsIcerikTablosuModel>();
				while (SModel.Reader.Read())
				{
					if (KayitBilgisiAl().Sonuc.Equals(Sonuclar.Basarili))
					{
						VeriListe.Add(SDataModel.Veriler);
					}
					else
					{
						SDataListModel = new SurecVeriModel<IList<SmsIcerikTablosuModel>>{
							Sonuc = SDataModel.Sonuc,
							KullaniciMesaji = SDataModel.KullaniciMesaji,
							HataBilgi = SDataModel.HataBilgi
						};
						VTIslem.CloseConnection();
						return SDataListModel;
					}
				}
				SDataListModel = new SurecVeriModel<IList<SmsIcerikTablosuModel>>{
					Sonuc = Sonuclar.Basarili,
					KullaniciMesaji = "Veri listesi başarıyla çekildi",
					Veriler = VeriListe
				};
			}
			else
			{
				SDataListModel = new SurecVeriModel<IList<SmsIcerikTablosuModel>>{
					Sonuc = SModel.Sonuc,
					KullaniciMesaji = SModel.KullaniciMesaji,
					HataBilgi = SModel.HataBilgi
				};
			}
			VTIslem.CloseConnection();
			return SDataListModel;
		}

		public virtual SurecVeriModel<IList<SmsIcerikTablosuModel>> GonderimTipiBilgileri(int GonderimTipiID)
		{
			VTIslem.SetCommandText($"SELECT {SmsIcerikTablosuModel.SQLSutunSorgusu} FROM SmsIcerikTablosu WHERE GonderimTipiID=@GonderimTipiID");
			VTIslem.AddWithValue("GonderimTipiID", GonderimTipiID);
			VTIslem.OpenConnection();
			SModel = VTIslem.ExecuteReader(CommandBehavior.Default);
			if (SModel.Sonuc.Equals(Sonuclar.Basarili))
			{
				VeriListe = new List<SmsIcerikTablosuModel>();
				while (SModel.Reader.Read())
				{
					if (KayitBilgisiAl().Sonuc.Equals(Sonuclar.Basarili))
					{
						VeriListe.Add(SDataModel.Veriler);
					}
					else
					{
						SDataListModel = new SurecVeriModel<IList<SmsIcerikTablosuModel>>{
							Sonuc = SDataModel.Sonuc,
							KullaniciMesaji = SDataModel.KullaniciMesaji,
							HataBilgi = SDataModel.HataBilgi
						};
						VTIslem.CloseConnection();
						return SDataListModel;
					}
				}
				SDataListModel = new SurecVeriModel<IList<SmsIcerikTablosuModel>>{
					Sonuc = Sonuclar.Basarili,
					KullaniciMesaji = "Veri listesi başarıyla çekildi",
					Veriler = VeriListe
				};
			}
			else
			{
				SDataListModel = new SurecVeriModel<IList<SmsIcerikTablosuModel>>{
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