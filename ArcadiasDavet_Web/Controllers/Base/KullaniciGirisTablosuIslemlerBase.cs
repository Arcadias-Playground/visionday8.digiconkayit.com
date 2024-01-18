using Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.OleDb;
using VeritabaniIslemSaglayici.Access;

namespace VeritabaniIslemMerkeziBase
{
	public abstract class KullaniciGirisTablosuIslemlerBase
	{
		public VTOperatorleri VTIslem;

		public List<KullaniciGirisTablosuModel> VeriListe;

		public SurecBilgiModel SModel;
		public SurecVeriModel<KullaniciGirisTablosuModel> SDataModel;
		public SurecVeriModel<IList<KullaniciGirisTablosuModel>> SDataListModel;

		public KullaniciGirisTablosuIslemlerBase()
		{
			VTIslem = new VTOperatorleri();
		}

		public KullaniciGirisTablosuIslemlerBase(OleDbTransaction Transcation)
		{
			VTIslem = new VTOperatorleri(Transcation);
		}

		public virtual SurecBilgiModel YeniKayitEkle(KullaniciGirisTablosuModel YeniKayit)
		{
			VTIslem.SetCommandText("INSERT INTO KullaniciGirisTablosu (KullaniciGirisID, KullaniciID, IP, Platform, Mobil, AuthToken, EklenmeTarihi) VALUES (@KullaniciGirisID, @KullaniciID, @IP, @Platform, @Mobil, @AuthToken, @EklenmeTarihi)");
			VTIslem.AddWithValue("KullaniciGirisID", YeniKayit.KullaniciGirisID);
			VTIslem.AddWithValue("KullaniciID", YeniKayit.KullaniciID);
			VTIslem.AddWithValue("IP", YeniKayit.IP);
			VTIslem.AddWithValue("Platform", YeniKayit.Platform);
			VTIslem.AddWithValue("Mobil", YeniKayit.Mobil);
			VTIslem.AddWithValue("AuthToken", YeniKayit.AuthToken);
			VTIslem.AddWithValue("EklenmeTarihi", YeniKayit.EklenmeTarihi);
			return VTIslem.ExecuteNonQuery();
		}

		public virtual SurecBilgiModel KayitGuncelle(KullaniciGirisTablosuModel GuncelKayit)
		{
			VTIslem.SetCommandText("UPDATE KullaniciGirisTablosu SET KullaniciID=@KullaniciID, IP=@IP, Platform=@Platform, Mobil=@Mobil, AuthToken=@AuthToken, EklenmeTarihi=@EklenmeTarihi WHERE KullaniciGirisID=@KullaniciGirisID");
			VTIslem.AddWithValue("KullaniciID", GuncelKayit.KullaniciID);
			VTIslem.AddWithValue("IP", GuncelKayit.IP);
			VTIslem.AddWithValue("Platform", GuncelKayit.Platform);
			VTIslem.AddWithValue("Mobil", GuncelKayit.Mobil);
			VTIslem.AddWithValue("AuthToken", GuncelKayit.AuthToken);
			VTIslem.AddWithValue("EklenmeTarihi", GuncelKayit.EklenmeTarihi);
			VTIslem.AddWithValue("KullaniciGirisID", GuncelKayit.KullaniciGirisID);
			return VTIslem.ExecuteNonQuery();
		}

		public virtual SurecBilgiModel KayitSil(string KullaniciGirisID)
		{
			VTIslem.SetCommandText("DELETE FROM KullaniciGirisTablosu WHERE KullaniciGirisID=@KullaniciGirisID");
			VTIslem.AddWithValue("KullaniciGirisID", KullaniciGirisID);
			return VTIslem.ExecuteNonQuery();
		}

		public virtual SurecVeriModel<KullaniciGirisTablosuModel> KayitBilgisi(string KullaniciGirisID)
		{
			VTIslem.SetCommandText($"SELECT {KullaniciGirisTablosuModel.SQLSutunSorgusu} FROM KullaniciGirisTablosu WHERE KullaniciGirisID = @KullaniciGirisID");
			VTIslem.AddWithValue("KullaniciGirisID", KullaniciGirisID);
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
					SDataModel = new SurecVeriModel<KullaniciGirisTablosuModel>
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
				SDataModel = new SurecVeriModel<KullaniciGirisTablosuModel>
				{
					Sonuc = SModel.Sonuc,
					KullaniciMesaji = SModel.KullaniciMesaji,
					HataBilgi = SModel.HataBilgi
				};
			}
			VTIslem.CloseConnection();
			return SDataModel;
		}

		public virtual SurecVeriModel<IList<KullaniciGirisTablosuModel>> KayitBilgileri()
		{
			VTIslem.SetCommandText($"SELECT {KullaniciGirisTablosuModel.SQLSutunSorgusu} FROM KullaniciGirisTablosu");
			VTIslem.OpenConnection();
			SModel = VTIslem.ExecuteReader(CommandBehavior.Default);
			if (SModel.Sonuc.Equals(Sonuclar.Basarili))
			{
				VeriListe = new List<KullaniciGirisTablosuModel>();
				while (SModel.Reader.Read())
				{
					if (KayitBilgisiAl().Sonuc.Equals(Sonuclar.Basarili))
					{
						VeriListe.Add(SDataModel.Veriler);
					}
					else
					{
						SDataListModel = new SurecVeriModel<IList<KullaniciGirisTablosuModel>>{
							Sonuc = SDataModel.Sonuc,
							KullaniciMesaji = SDataModel.KullaniciMesaji,
							HataBilgi = SDataModel.HataBilgi
						};
						VTIslem.CloseConnection();
						return SDataListModel;
					}
				}
				SDataListModel = new SurecVeriModel<IList<KullaniciGirisTablosuModel>>{
					Sonuc = Sonuclar.Basarili,
					KullaniciMesaji = "Veri listesi başarıyla çekildi",
					Veriler = VeriListe
				};
			}
			else
			{
				SDataListModel = new SurecVeriModel<IList<KullaniciGirisTablosuModel>>{
					Sonuc = SModel.Sonuc,
					KullaniciMesaji = SModel.KullaniciMesaji,
					HataBilgi = SModel.HataBilgi
				};
			}
			VTIslem.CloseConnection();
			return SDataListModel;
		}

		SurecVeriModel<KullaniciGirisTablosuModel> KayitBilgisiAl()
		{
			try
			{
				SDataModel = new SurecVeriModel<KullaniciGirisTablosuModel>{
					Sonuc = Sonuclar.Basarili,
					KullaniciMesaji = "Veri bilgisi başarıyla çekilmiştir.",
					Veriler = new KullaniciGirisTablosuModel
					{
						KullaniciGirisID = SModel.Reader.GetString(0),
						KullaniciID = SModel.Reader.GetString(1),
						IP = SModel.Reader.GetString(2),
						Platform = SModel.Reader.GetString(3),
						Mobil = SModel.Reader.GetBoolean(4),
						AuthToken = SModel.Reader.GetString(5),
						EklenmeTarihi = SModel.Reader.GetDateTime(6),
					}
				};

			}
			catch (InvalidCastException ex)
			{
				SDataModel = new SurecVeriModel<KullaniciGirisTablosuModel>{
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
				SDataModel = new SurecVeriModel<KullaniciGirisTablosuModel>{
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

		public virtual SurecVeriModel<KullaniciGirisTablosuModel> KayitBilgisiAl(int Baslangic, DbDataReader Reader)
		{
			try
			{
				SDataModel = new SurecVeriModel<KullaniciGirisTablosuModel>{
					Sonuc = Sonuclar.Basarili,
					KullaniciMesaji = "Veri bilgisi başarıyla çekilmiştir.",
					Veriler = new KullaniciGirisTablosuModel{
						KullaniciGirisID = Reader.GetString(Baslangic + 0),
						KullaniciID = Reader.GetString(Baslangic + 1),
						IP = Reader.GetString(Baslangic + 2),
						Platform = Reader.GetString(Baslangic + 3),
						Mobil = Reader.GetBoolean(Baslangic + 4),
						AuthToken = Reader.GetString(Baslangic + 5),
						EklenmeTarihi = Reader.GetDateTime(Baslangic + 6),
					}
				};
			}
			catch (InvalidCastException ex)
			{
				SDataModel = new SurecVeriModel<KullaniciGirisTablosuModel>{
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
				SDataModel = new SurecVeriModel<KullaniciGirisTablosuModel>{
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

		public virtual SurecVeriModel<IList<KullaniciGirisTablosuModel>> KullaniciBilgileri(string KullaniciID)
		{
			VTIslem.SetCommandText($"SELECT {KullaniciGirisTablosuModel.SQLSutunSorgusu} FROM KullaniciGirisTablosu WHERE KullaniciID=@KullaniciID");
			VTIslem.AddWithValue("KullaniciID", KullaniciID);
			VTIslem.OpenConnection();
			SModel = VTIslem.ExecuteReader(CommandBehavior.Default);
			if (SModel.Sonuc.Equals(Sonuclar.Basarili))
			{
				VeriListe = new List<KullaniciGirisTablosuModel>();
				while (SModel.Reader.Read())
				{
					if (KayitBilgisiAl().Sonuc.Equals(Sonuclar.Basarili))
					{
						VeriListe.Add(SDataModel.Veriler);
					}
					else
					{
						SDataListModel = new SurecVeriModel<IList<KullaniciGirisTablosuModel>>{
							Sonuc = SDataModel.Sonuc,
							KullaniciMesaji = SDataModel.KullaniciMesaji,
							HataBilgi = SDataModel.HataBilgi
						};
						VTIslem.CloseConnection();
						return SDataListModel;
					}
				}
				SDataListModel = new SurecVeriModel<IList<KullaniciGirisTablosuModel>>{
					Sonuc = Sonuclar.Basarili,
					KullaniciMesaji = "Veri listesi başarıyla çekildi",
					Veriler = VeriListe
				};
			}
			else
			{
				SDataListModel = new SurecVeriModel<IList<KullaniciGirisTablosuModel>>{
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