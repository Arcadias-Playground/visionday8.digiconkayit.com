using Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.OleDb;
using VeritabaniIslemSaglayici.Access;

namespace VeritabaniIslemMerkeziBase
{
	public abstract class KullaniciTipiTablosuIslemlerBase
	{
		public VTOperatorleri VTIslem;

		public List<KullaniciTipiTablosuModel> VeriListe;

		public SurecBilgiModel SModel;
		public SurecVeriModel<KullaniciTipiTablosuModel> SDataModel;
		public SurecVeriModel<IList<KullaniciTipiTablosuModel>> SDataListModel;

		public KullaniciTipiTablosuIslemlerBase()
		{
			VTIslem = new VTOperatorleri();
		}

		public KullaniciTipiTablosuIslemlerBase(OleDbTransaction Transcation)
		{
			VTIslem = new VTOperatorleri(Transcation);
		}

		public virtual SurecBilgiModel YeniKayitEkle(KullaniciTipiTablosuModel YeniKayit)
		{
			VTIslem.SetCommandText("INSERT INTO KullaniciTipiTablosu (KullaniciTipiID, KullaniciTipi, EklenmeTarihi) VALUES (@KullaniciTipiID, @KullaniciTipi, @EklenmeTarihi)");
			VTIslem.AddWithValue("KullaniciTipiID", YeniKayit.KullaniciTipiID);
			VTIslem.AddWithValue("KullaniciTipi", YeniKayit.KullaniciTipi);
			VTIslem.AddWithValue("EklenmeTarihi", YeniKayit.EklenmeTarihi);
			return VTIslem.ExecuteNonQuery();
		}

		public virtual SurecBilgiModel KayitGuncelle(KullaniciTipiTablosuModel GuncelKayit)
		{
			VTIslem.SetCommandText("UPDATE KullaniciTipiTablosu SET KullaniciTipi=@KullaniciTipi, EklenmeTarihi=@EklenmeTarihi WHERE KullaniciTipiID=@KullaniciTipiID");
			VTIslem.AddWithValue("KullaniciTipi", GuncelKayit.KullaniciTipi);
			VTIslem.AddWithValue("EklenmeTarihi", GuncelKayit.EklenmeTarihi);
			VTIslem.AddWithValue("KullaniciTipiID", GuncelKayit.KullaniciTipiID);
			return VTIslem.ExecuteNonQuery();
		}

		public virtual SurecBilgiModel KayitSil(string KullaniciTipiID)
		{
			VTIslem.SetCommandText("DELETE FROM KullaniciTipiTablosu WHERE KullaniciTipiID=@KullaniciTipiID");
			VTIslem.AddWithValue("KullaniciTipiID", KullaniciTipiID);
			return VTIslem.ExecuteNonQuery();
		}

		public virtual SurecVeriModel<KullaniciTipiTablosuModel> KayitBilgisi(string KullaniciTipiID)
		{
			VTIslem.SetCommandText($"SELECT {KullaniciTipiTablosuModel.SQLSutunSorgusu} FROM KullaniciTipiTablosu WHERE KullaniciTipiID = @KullaniciTipiID");
			VTIslem.AddWithValue("KullaniciTipiID", KullaniciTipiID);
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
					SDataModel = new SurecVeriModel<KullaniciTipiTablosuModel>
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
				SDataModel = new SurecVeriModel<KullaniciTipiTablosuModel>
				{
					Sonuc = SModel.Sonuc,
					KullaniciMesaji = SModel.KullaniciMesaji,
					HataBilgi = SModel.HataBilgi
				};
			}
			VTIslem.CloseConnection();
			return SDataModel;
		}

		public virtual SurecVeriModel<IList<KullaniciTipiTablosuModel>> KayitBilgileri()
		{
			VTIslem.SetCommandText($"SELECT {KullaniciTipiTablosuModel.SQLSutunSorgusu} FROM KullaniciTipiTablosu");
			VTIslem.OpenConnection();
			SModel = VTIslem.ExecuteReader(CommandBehavior.Default);
			if (SModel.Sonuc.Equals(Sonuclar.Basarili))
			{
				VeriListe = new List<KullaniciTipiTablosuModel>();
				while (SModel.Reader.Read())
				{
					if (KayitBilgisiAl().Sonuc.Equals(Sonuclar.Basarili))
					{
						VeriListe.Add(SDataModel.Veriler);
					}
					else
					{
						SDataListModel = new SurecVeriModel<IList<KullaniciTipiTablosuModel>>{
							Sonuc = SDataModel.Sonuc,
							KullaniciMesaji = SDataModel.KullaniciMesaji,
							HataBilgi = SDataModel.HataBilgi
						};
						VTIslem.CloseConnection();
						return SDataListModel;
					}
				}
				SDataListModel = new SurecVeriModel<IList<KullaniciTipiTablosuModel>>{
					Sonuc = Sonuclar.Basarili,
					KullaniciMesaji = "Veri listesi başarıyla çekildi",
					Veriler = VeriListe
				};
			}
			else
			{
				SDataListModel = new SurecVeriModel<IList<KullaniciTipiTablosuModel>>{
					Sonuc = SModel.Sonuc,
					KullaniciMesaji = SModel.KullaniciMesaji,
					HataBilgi = SModel.HataBilgi
				};
			}
			VTIslem.CloseConnection();
			return SDataListModel;
		}

		SurecVeriModel<KullaniciTipiTablosuModel> KayitBilgisiAl()
		{
			try
			{
				SDataModel = new SurecVeriModel<KullaniciTipiTablosuModel>{
					Sonuc = Sonuclar.Basarili,
					KullaniciMesaji = "Veri bilgisi başarıyla çekilmiştir.",
					Veriler = new KullaniciTipiTablosuModel
					{
						KullaniciTipiID = SModel.Reader.GetString(0),
						KullaniciTipi = SModel.Reader.GetString(1),
						EklenmeTarihi = SModel.Reader.GetDateTime(2),
					}
				};

			}
			catch (InvalidCastException ex)
			{
				SDataModel = new SurecVeriModel<KullaniciTipiTablosuModel>{
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
				SDataModel = new SurecVeriModel<KullaniciTipiTablosuModel>{
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

		public virtual SurecVeriModel<KullaniciTipiTablosuModel> KayitBilgisiAl(int Baslangic, DbDataReader Reader)
		{
			try
			{
				SDataModel = new SurecVeriModel<KullaniciTipiTablosuModel>{
					Sonuc = Sonuclar.Basarili,
					KullaniciMesaji = "Veri bilgisi başarıyla çekilmiştir.",
					Veriler = new KullaniciTipiTablosuModel{
						KullaniciTipiID = Reader.GetString(Baslangic + 0),
						KullaniciTipi = Reader.GetString(Baslangic + 1),
						EklenmeTarihi = Reader.GetDateTime(Baslangic + 2),
					}
				};
			}
			catch (InvalidCastException ex)
			{
				SDataModel = new SurecVeriModel<KullaniciTipiTablosuModel>{
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
				SDataModel = new SurecVeriModel<KullaniciTipiTablosuModel>{
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