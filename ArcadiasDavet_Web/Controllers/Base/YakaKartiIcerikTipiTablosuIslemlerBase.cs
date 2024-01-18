using Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.OleDb;
using VeritabaniIslemSaglayici.Access;

namespace VeritabaniIslemMerkeziBase
{
	public abstract class YakaKartiIcerikTipiTablosuIslemlerBase
	{
		public VTOperatorleri VTIslem;

		public List<YakaKartiIcerikTipiTablosuModel> VeriListe;

		public SurecBilgiModel SModel;
		public SurecVeriModel<YakaKartiIcerikTipiTablosuModel> SDataModel;
		public SurecVeriModel<IList<YakaKartiIcerikTipiTablosuModel>> SDataListModel;

		public YakaKartiIcerikTipiTablosuIslemlerBase()
		{
			VTIslem = new VTOperatorleri();
		}

		public YakaKartiIcerikTipiTablosuIslemlerBase(OleDbTransaction Transcation)
		{
			VTIslem = new VTOperatorleri(Transcation);
		}

		public virtual SurecBilgiModel YeniKayitEkle(YakaKartiIcerikTipiTablosuModel YeniKayit)
		{
			VTIslem.SetCommandText("INSERT INTO YakaKartiIcerikTipiTablosu (YakaKartiIcerikTipi, Oran, EklenmeTarihi) VALUES (@YakaKartiIcerikTipi, @Oran, @EklenmeTarihi)");
			VTIslem.AddWithValue("YakaKartiIcerikTipi", YeniKayit.YakaKartiIcerikTipi);
			VTIslem.AddWithValue("Oran", YeniKayit.Oran);
			VTIslem.AddWithValue("EklenmeTarihi", YeniKayit.EklenmeTarihi);
			return VTIslem.ExecuteNonQuery();
		}

		public virtual SurecBilgiModel KayitGuncelle(YakaKartiIcerikTipiTablosuModel GuncelKayit)
		{
			VTIslem.SetCommandText("UPDATE YakaKartiIcerikTipiTablosu SET YakaKartiIcerikTipi=@YakaKartiIcerikTipi, Oran=@Oran, EklenmeTarihi=@EklenmeTarihi WHERE YakaKartiIcerikTipiID=@YakaKartiIcerikTipiID");
			VTIslem.AddWithValue("YakaKartiIcerikTipi", GuncelKayit.YakaKartiIcerikTipi);
			VTIslem.AddWithValue("Oran", GuncelKayit.Oran);
			VTIslem.AddWithValue("EklenmeTarihi", GuncelKayit.EklenmeTarihi);
			VTIslem.AddWithValue("YakaKartiIcerikTipiID", GuncelKayit.YakaKartiIcerikTipiID);
			return VTIslem.ExecuteNonQuery();
		}

		public virtual SurecBilgiModel KayitSil(int YakaKartiIcerikTipiID)
		{
			VTIslem.SetCommandText("DELETE FROM YakaKartiIcerikTipiTablosu WHERE YakaKartiIcerikTipiID=@YakaKartiIcerikTipiID");
			VTIslem.AddWithValue("YakaKartiIcerikTipiID", YakaKartiIcerikTipiID);
			return VTIslem.ExecuteNonQuery();
		}

		public virtual SurecVeriModel<YakaKartiIcerikTipiTablosuModel> KayitBilgisi(int YakaKartiIcerikTipiID)
		{
			VTIslem.SetCommandText($"SELECT {YakaKartiIcerikTipiTablosuModel.SQLSutunSorgusu} FROM YakaKartiIcerikTipiTablosu WHERE YakaKartiIcerikTipiID = @YakaKartiIcerikTipiID");
			VTIslem.AddWithValue("YakaKartiIcerikTipiID", YakaKartiIcerikTipiID);
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
					SDataModel = new SurecVeriModel<YakaKartiIcerikTipiTablosuModel>
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
				SDataModel = new SurecVeriModel<YakaKartiIcerikTipiTablosuModel>
				{
					Sonuc = SModel.Sonuc,
					KullaniciMesaji = SModel.KullaniciMesaji,
					HataBilgi = SModel.HataBilgi
				};
			}
			VTIslem.CloseConnection();
			return SDataModel;
		}

		public virtual SurecVeriModel<IList<YakaKartiIcerikTipiTablosuModel>> KayitBilgileri()
		{
			VTIslem.SetCommandText($"SELECT {YakaKartiIcerikTipiTablosuModel.SQLSutunSorgusu} FROM YakaKartiIcerikTipiTablosu");
			VTIslem.OpenConnection();
			SModel = VTIslem.ExecuteReader(CommandBehavior.Default);
			if (SModel.Sonuc.Equals(Sonuclar.Basarili))
			{
				VeriListe = new List<YakaKartiIcerikTipiTablosuModel>();
				while (SModel.Reader.Read())
				{
					if (KayitBilgisiAl().Sonuc.Equals(Sonuclar.Basarili))
					{
						VeriListe.Add(SDataModel.Veriler);
					}
					else
					{
						SDataListModel = new SurecVeriModel<IList<YakaKartiIcerikTipiTablosuModel>>{
							Sonuc = SDataModel.Sonuc,
							KullaniciMesaji = SDataModel.KullaniciMesaji,
							HataBilgi = SDataModel.HataBilgi
						};
						VTIslem.CloseConnection();
						return SDataListModel;
					}
				}
				SDataListModel = new SurecVeriModel<IList<YakaKartiIcerikTipiTablosuModel>>{
					Sonuc = Sonuclar.Basarili,
					KullaniciMesaji = "Veri listesi başarıyla çekildi",
					Veriler = VeriListe
				};
			}
			else
			{
				SDataListModel = new SurecVeriModel<IList<YakaKartiIcerikTipiTablosuModel>>{
					Sonuc = SModel.Sonuc,
					KullaniciMesaji = SModel.KullaniciMesaji,
					HataBilgi = SModel.HataBilgi
				};
			}
			VTIslem.CloseConnection();
			return SDataListModel;
		}

		SurecVeriModel<YakaKartiIcerikTipiTablosuModel> KayitBilgisiAl()
		{
			try
			{
				SDataModel = new SurecVeriModel<YakaKartiIcerikTipiTablosuModel>{
					Sonuc = Sonuclar.Basarili,
					KullaniciMesaji = "Veri bilgisi başarıyla çekilmiştir.",
					Veriler = new YakaKartiIcerikTipiTablosuModel
					{
						YakaKartiIcerikTipiID = SModel.Reader.GetInt32(0),
						YakaKartiIcerikTipi = SModel.Reader.GetString(1),
						Oran = SModel.Reader.GetDecimal(2),
						EklenmeTarihi = SModel.Reader.GetDateTime(3),
					}
				};

			}
			catch (InvalidCastException ex)
			{
				SDataModel = new SurecVeriModel<YakaKartiIcerikTipiTablosuModel>{
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
				SDataModel = new SurecVeriModel<YakaKartiIcerikTipiTablosuModel>{
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

		public virtual SurecVeriModel<YakaKartiIcerikTipiTablosuModel> KayitBilgisiAl(int Baslangic, DbDataReader Reader)
		{
			try
			{
				SDataModel = new SurecVeriModel<YakaKartiIcerikTipiTablosuModel>{
					Sonuc = Sonuclar.Basarili,
					KullaniciMesaji = "Veri bilgisi başarıyla çekilmiştir.",
					Veriler = new YakaKartiIcerikTipiTablosuModel{
						YakaKartiIcerikTipiID = Reader.GetInt32(Baslangic + 0),
						YakaKartiIcerikTipi = Reader.GetString(Baslangic + 1),
						Oran = Reader.GetDecimal(Baslangic + 2),
						EklenmeTarihi = Reader.GetDateTime(Baslangic + 3),
					}
				};
			}
			catch (InvalidCastException ex)
			{
				SDataModel = new SurecVeriModel<YakaKartiIcerikTipiTablosuModel>{
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
				SDataModel = new SurecVeriModel<YakaKartiIcerikTipiTablosuModel>{
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