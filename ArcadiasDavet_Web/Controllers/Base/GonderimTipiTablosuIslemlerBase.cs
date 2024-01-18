using Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.OleDb;
using VeritabaniIslemSaglayici.Access;

namespace VeritabaniIslemMerkeziBase
{
	public abstract class GonderimTipiTablosuIslemlerBase
	{
		public VTOperatorleri VTIslem;

		public List<GonderimTipiTablosuModel> VeriListe;

		public SurecBilgiModel SModel;
		public SurecVeriModel<GonderimTipiTablosuModel> SDataModel;
		public SurecVeriModel<IList<GonderimTipiTablosuModel>> SDataListModel;

		public GonderimTipiTablosuIslemlerBase()
		{
			VTIslem = new VTOperatorleri();
		}

		public GonderimTipiTablosuIslemlerBase(OleDbTransaction Transcation)
		{
			VTIslem = new VTOperatorleri(Transcation);
		}

		public virtual SurecBilgiModel YeniKayitEkle(GonderimTipiTablosuModel YeniKayit)
		{
			VTIslem.SetCommandText("INSERT INTO GonderimTipiTablosu (GonderimTipi, EklenmeTarihi) VALUES (@GonderimTipi, @EklenmeTarihi)");
			VTIslem.AddWithValue("GonderimTipi", YeniKayit.GonderimTipi);
			VTIslem.AddWithValue("EklenmeTarihi", YeniKayit.EklenmeTarihi);
			return VTIslem.ExecuteNonQuery();
		}

		public virtual SurecBilgiModel KayitGuncelle(GonderimTipiTablosuModel GuncelKayit)
		{
			VTIslem.SetCommandText("UPDATE GonderimTipiTablosu SET GonderimTipi=@GonderimTipi, EklenmeTarihi=@EklenmeTarihi WHERE GonderimTipiID=@GonderimTipiID");
			VTIslem.AddWithValue("GonderimTipi", GuncelKayit.GonderimTipi);
			VTIslem.AddWithValue("EklenmeTarihi", GuncelKayit.EklenmeTarihi);
			VTIslem.AddWithValue("GonderimTipiID", GuncelKayit.GonderimTipiID);
			return VTIslem.ExecuteNonQuery();
		}

		public virtual SurecBilgiModel KayitSil(int GonderimTipiID)
		{
			VTIslem.SetCommandText("DELETE FROM GonderimTipiTablosu WHERE GonderimTipiID=@GonderimTipiID");
			VTIslem.AddWithValue("GonderimTipiID", GonderimTipiID);
			return VTIslem.ExecuteNonQuery();
		}

		public virtual SurecVeriModel<GonderimTipiTablosuModel> KayitBilgisi(int GonderimTipiID)
		{
			VTIslem.SetCommandText($"SELECT {GonderimTipiTablosuModel.SQLSutunSorgusu} FROM GonderimTipiTablosu WHERE GonderimTipiID = @GonderimTipiID");
			VTIslem.AddWithValue("GonderimTipiID", GonderimTipiID);
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
					SDataModel = new SurecVeriModel<GonderimTipiTablosuModel>
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
				SDataModel = new SurecVeriModel<GonderimTipiTablosuModel>
				{
					Sonuc = SModel.Sonuc,
					KullaniciMesaji = SModel.KullaniciMesaji,
					HataBilgi = SModel.HataBilgi
				};
			}
			VTIslem.CloseConnection();
			return SDataModel;
		}

		public virtual SurecVeriModel<IList<GonderimTipiTablosuModel>> KayitBilgileri()
		{
			VTIslem.SetCommandText($"SELECT {GonderimTipiTablosuModel.SQLSutunSorgusu} FROM GonderimTipiTablosu");
			VTIslem.OpenConnection();
			SModel = VTIslem.ExecuteReader(CommandBehavior.Default);
			if (SModel.Sonuc.Equals(Sonuclar.Basarili))
			{
				VeriListe = new List<GonderimTipiTablosuModel>();
				while (SModel.Reader.Read())
				{
					if (KayitBilgisiAl().Sonuc.Equals(Sonuclar.Basarili))
					{
						VeriListe.Add(SDataModel.Veriler);
					}
					else
					{
						SDataListModel = new SurecVeriModel<IList<GonderimTipiTablosuModel>>{
							Sonuc = SDataModel.Sonuc,
							KullaniciMesaji = SDataModel.KullaniciMesaji,
							HataBilgi = SDataModel.HataBilgi
						};
						VTIslem.CloseConnection();
						return SDataListModel;
					}
				}
				SDataListModel = new SurecVeriModel<IList<GonderimTipiTablosuModel>>{
					Sonuc = Sonuclar.Basarili,
					KullaniciMesaji = "Veri listesi başarıyla çekildi",
					Veriler = VeriListe
				};
			}
			else
			{
				SDataListModel = new SurecVeriModel<IList<GonderimTipiTablosuModel>>{
					Sonuc = SModel.Sonuc,
					KullaniciMesaji = SModel.KullaniciMesaji,
					HataBilgi = SModel.HataBilgi
				};
			}
			VTIslem.CloseConnection();
			return SDataListModel;
		}

		SurecVeriModel<GonderimTipiTablosuModel> KayitBilgisiAl()
		{
			try
			{
				SDataModel = new SurecVeriModel<GonderimTipiTablosuModel>{
					Sonuc = Sonuclar.Basarili,
					KullaniciMesaji = "Veri bilgisi başarıyla çekilmiştir.",
					Veriler = new GonderimTipiTablosuModel
					{
						GonderimTipiID = SModel.Reader.GetInt32(0),
						GonderimTipi = SModel.Reader.GetString(1),
						EklenmeTarihi = SModel.Reader.GetDateTime(2),
					}
				};

			}
			catch (InvalidCastException ex)
			{
				SDataModel = new SurecVeriModel<GonderimTipiTablosuModel>{
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
				SDataModel = new SurecVeriModel<GonderimTipiTablosuModel>{
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

		public virtual SurecVeriModel<GonderimTipiTablosuModel> KayitBilgisiAl(int Baslangic, DbDataReader Reader)
		{
			try
			{
				SDataModel = new SurecVeriModel<GonderimTipiTablosuModel>{
					Sonuc = Sonuclar.Basarili,
					KullaniciMesaji = "Veri bilgisi başarıyla çekilmiştir.",
					Veriler = new GonderimTipiTablosuModel{
						GonderimTipiID = Reader.GetInt32(Baslangic + 0),
						GonderimTipi = Reader.GetString(Baslangic + 1),
						EklenmeTarihi = Reader.GetDateTime(Baslangic + 2),
					}
				};
			}
			catch (InvalidCastException ex)
			{
				SDataModel = new SurecVeriModel<GonderimTipiTablosuModel>{
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
				SDataModel = new SurecVeriModel<GonderimTipiTablosuModel>{
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

	}
}