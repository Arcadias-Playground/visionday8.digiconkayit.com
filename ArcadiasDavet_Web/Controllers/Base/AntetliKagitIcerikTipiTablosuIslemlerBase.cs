using Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.OleDb;
using VeritabaniIslemSaglayici.Access;

namespace VeritabaniIslemMerkeziBase
{
	public abstract class AntetliKagitIcerikTipiTablosuIslemlerBase
	{
		public VTOperatorleri VTIslem;

		public List<AntetliKagitIcerikTipiTablosuModel> VeriListe;

		public SurecBilgiModel SModel;
		public SurecVeriModel<AntetliKagitIcerikTipiTablosuModel> SDataModel;
		public SurecVeriModel<IList<AntetliKagitIcerikTipiTablosuModel>> SDataListModel;

		public AntetliKagitIcerikTipiTablosuIslemlerBase()
		{
			VTIslem = new VTOperatorleri();
		}

		public AntetliKagitIcerikTipiTablosuIslemlerBase(OleDbTransaction Transcation)
		{
			VTIslem = new VTOperatorleri(Transcation);
		}

		public virtual SurecBilgiModel YeniKayitEkle(AntetliKagitIcerikTipiTablosuModel YeniKayit)
		{
			VTIslem.SetCommandText("INSERT INTO AntetliKagitIcerikTipiTablosu (GonderimTipiID, AntetliKagitIcerikTipi, Oran, GuncellenmeTarihi, EklenmeTarihi) VALUES (@GonderimTipiID, @AntetliKagitIcerikTipi, @Oran, @GuncellenmeTarihi, @EklenmeTarihi)");
			VTIslem.AddWithValue("GonderimTipiID", YeniKayit.GonderimTipiID);
			VTIslem.AddWithValue("AntetliKagitIcerikTipi", YeniKayit.AntetliKagitIcerikTipi);
			VTIslem.AddWithValue("Oran", YeniKayit.Oran);
			VTIslem.AddWithValue("GuncellenmeTarihi", YeniKayit.GuncellenmeTarihi);
			VTIslem.AddWithValue("EklenmeTarihi", YeniKayit.EklenmeTarihi);
			return VTIslem.ExecuteNonQuery();
		}

		public virtual SurecBilgiModel KayitGuncelle(AntetliKagitIcerikTipiTablosuModel GuncelKayit)
		{
			VTIslem.SetCommandText("UPDATE AntetliKagitIcerikTipiTablosu SET GonderimTipiID=@GonderimTipiID, AntetliKagitIcerikTipi=@AntetliKagitIcerikTipi, Oran=@Oran, GuncellenmeTarihi=@GuncellenmeTarihi, EklenmeTarihi=@EklenmeTarihi WHERE AntetliKagitIcerikTipiID=@AntetliKagitIcerikTipiID");
			VTIslem.AddWithValue("GonderimTipiID", GuncelKayit.GonderimTipiID);
			VTIslem.AddWithValue("AntetliKagitIcerikTipi", GuncelKayit.AntetliKagitIcerikTipi);
			VTIslem.AddWithValue("Oran", GuncelKayit.Oran);
			VTIslem.AddWithValue("GuncellenmeTarihi", GuncelKayit.GuncellenmeTarihi);
			VTIslem.AddWithValue("EklenmeTarihi", GuncelKayit.EklenmeTarihi);
			VTIslem.AddWithValue("AntetliKagitIcerikTipiID", GuncelKayit.AntetliKagitIcerikTipiID);
			return VTIslem.ExecuteNonQuery();
		}

		public virtual SurecBilgiModel KayitSil(int AntetliKagitIcerikTipiID)
		{
			VTIslem.SetCommandText("DELETE FROM AntetliKagitIcerikTipiTablosu WHERE AntetliKagitIcerikTipiID=@AntetliKagitIcerikTipiID");
			VTIslem.AddWithValue("AntetliKagitIcerikTipiID", AntetliKagitIcerikTipiID);
			return VTIslem.ExecuteNonQuery();
		}

		public virtual SurecVeriModel<AntetliKagitIcerikTipiTablosuModel> KayitBilgisi(int AntetliKagitIcerikTipiID)
		{
			VTIslem.SetCommandText($"SELECT {AntetliKagitIcerikTipiTablosuModel.SQLSutunSorgusu} FROM AntetliKagitIcerikTipiTablosu WHERE AntetliKagitIcerikTipiID = @AntetliKagitIcerikTipiID");
			VTIslem.AddWithValue("AntetliKagitIcerikTipiID", AntetliKagitIcerikTipiID);
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
					SDataModel = new SurecVeriModel<AntetliKagitIcerikTipiTablosuModel>
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
				SDataModel = new SurecVeriModel<AntetliKagitIcerikTipiTablosuModel>
				{
					Sonuc = SModel.Sonuc,
					KullaniciMesaji = SModel.KullaniciMesaji,
					HataBilgi = SModel.HataBilgi
				};
			}
			VTIslem.CloseConnection();
			return SDataModel;
		}

		public virtual SurecVeriModel<IList<AntetliKagitIcerikTipiTablosuModel>> KayitBilgileri()
		{
			VTIslem.SetCommandText($"SELECT {AntetliKagitIcerikTipiTablosuModel.SQLSutunSorgusu} FROM AntetliKagitIcerikTipiTablosu");
			VTIslem.OpenConnection();
			SModel = VTIslem.ExecuteReader(CommandBehavior.Default);
			if (SModel.Sonuc.Equals(Sonuclar.Basarili))
			{
				VeriListe = new List<AntetliKagitIcerikTipiTablosuModel>();
				while (SModel.Reader.Read())
				{
					if (KayitBilgisiAl().Sonuc.Equals(Sonuclar.Basarili))
					{
						VeriListe.Add(SDataModel.Veriler);
					}
					else
					{
						SDataListModel = new SurecVeriModel<IList<AntetliKagitIcerikTipiTablosuModel>>{
							Sonuc = SDataModel.Sonuc,
							KullaniciMesaji = SDataModel.KullaniciMesaji,
							HataBilgi = SDataModel.HataBilgi
						};
						VTIslem.CloseConnection();
						return SDataListModel;
					}
				}
				SDataListModel = new SurecVeriModel<IList<AntetliKagitIcerikTipiTablosuModel>>{
					Sonuc = Sonuclar.Basarili,
					KullaniciMesaji = "Veri listesi başarıyla çekildi",
					Veriler = VeriListe
				};
			}
			else
			{
				SDataListModel = new SurecVeriModel<IList<AntetliKagitIcerikTipiTablosuModel>>{
					Sonuc = SModel.Sonuc,
					KullaniciMesaji = SModel.KullaniciMesaji,
					HataBilgi = SModel.HataBilgi
				};
			}
			VTIslem.CloseConnection();
			return SDataListModel;
		}

		SurecVeriModel<AntetliKagitIcerikTipiTablosuModel> KayitBilgisiAl()
		{
			try
			{
				SDataModel = new SurecVeriModel<AntetliKagitIcerikTipiTablosuModel>{
					Sonuc = Sonuclar.Basarili,
					KullaniciMesaji = "Veri bilgisi başarıyla çekilmiştir.",
					Veriler = new AntetliKagitIcerikTipiTablosuModel
					{
						AntetliKagitIcerikTipiID = SModel.Reader.GetInt32(0),
						GonderimTipiID = SModel.Reader.GetInt32(1),
						AntetliKagitIcerikTipi = SModel.Reader.GetString(2),
						Oran = SModel.Reader.GetDecimal(3),
						GuncellenmeTarihi = SModel.Reader.GetDateTime(4),
						EklenmeTarihi = SModel.Reader.GetDateTime(5),
					}
				};

			}
			catch (InvalidCastException ex)
			{
				SDataModel = new SurecVeriModel<AntetliKagitIcerikTipiTablosuModel>{
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
				SDataModel = new SurecVeriModel<AntetliKagitIcerikTipiTablosuModel>{
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

		public virtual SurecVeriModel<AntetliKagitIcerikTipiTablosuModel> KayitBilgisiAl(int Baslangic, DbDataReader Reader)
		{
			try
			{
				SDataModel = new SurecVeriModel<AntetliKagitIcerikTipiTablosuModel>{
					Sonuc = Sonuclar.Basarili,
					KullaniciMesaji = "Veri bilgisi başarıyla çekilmiştir.",
					Veriler = new AntetliKagitIcerikTipiTablosuModel{
						AntetliKagitIcerikTipiID = Reader.GetInt32(Baslangic + 0),
						GonderimTipiID = Reader.GetInt32(Baslangic + 1),
						AntetliKagitIcerikTipi = Reader.GetString(Baslangic + 2),
						Oran = Reader.GetDecimal(Baslangic + 3),
						GuncellenmeTarihi = Reader.GetDateTime(Baslangic + 4),
						EklenmeTarihi = Reader.GetDateTime(Baslangic + 5),
					}
				};
			}
			catch (InvalidCastException ex)
			{
				SDataModel = new SurecVeriModel<AntetliKagitIcerikTipiTablosuModel>{
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
				SDataModel = new SurecVeriModel<AntetliKagitIcerikTipiTablosuModel>{
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

		public virtual SurecVeriModel<IList<AntetliKagitIcerikTipiTablosuModel>> GonderimTipiBilgileri(int GonderimTipiID)
		{
			VTIslem.SetCommandText($"SELECT {AntetliKagitIcerikTipiTablosuModel.SQLSutunSorgusu} FROM AntetliKagitIcerikTipiTablosu WHERE GonderimTipiID=@GonderimTipiID");
			VTIslem.AddWithValue("GonderimTipiID", GonderimTipiID);
			VTIslem.OpenConnection();
			SModel = VTIslem.ExecuteReader(CommandBehavior.Default);
			if (SModel.Sonuc.Equals(Sonuclar.Basarili))
			{
				VeriListe = new List<AntetliKagitIcerikTipiTablosuModel>();
				while (SModel.Reader.Read())
				{
					if (KayitBilgisiAl().Sonuc.Equals(Sonuclar.Basarili))
					{
						VeriListe.Add(SDataModel.Veriler);
					}
					else
					{
						SDataListModel = new SurecVeriModel<IList<AntetliKagitIcerikTipiTablosuModel>>{
							Sonuc = SDataModel.Sonuc,
							KullaniciMesaji = SDataModel.KullaniciMesaji,
							HataBilgi = SDataModel.HataBilgi
						};
						VTIslem.CloseConnection();
						return SDataListModel;
					}
				}
				SDataListModel = new SurecVeriModel<IList<AntetliKagitIcerikTipiTablosuModel>>{
					Sonuc = Sonuclar.Basarili,
					KullaniciMesaji = "Veri listesi başarıyla çekildi",
					Veriler = VeriListe
				};
			}
			else
			{
				SDataListModel = new SurecVeriModel<IList<AntetliKagitIcerikTipiTablosuModel>>{
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