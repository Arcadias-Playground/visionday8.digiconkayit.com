using Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.OleDb;
using VeritabaniIslemSaglayici.Access;

namespace VeritabaniIslemMerkeziBase
{
	public abstract class KullaniciTablosuIslemlerBase
	{
		public VTOperatorleri VTIslem;

		public List<KullaniciTablosuModel> VeriListe;

		public SurecBilgiModel SModel;
		public SurecVeriModel<KullaniciTablosuModel> SDataModel;
		public SurecVeriModel<IList<KullaniciTablosuModel>> SDataListModel;

		public KullaniciTablosuIslemlerBase()
		{
			VTIslem = new VTOperatorleri();
		}

		public KullaniciTablosuIslemlerBase(OleDbTransaction Transcation)
		{
			VTIslem = new VTOperatorleri(Transcation);
		}

		public virtual SurecBilgiModel YeniKayitEkle(KullaniciTablosuModel YeniKayit)
		{
			VTIslem.SetCommandText("INSERT INTO KullaniciTablosu (KullaniciID, KullaniciAdi, Sifre, GuncellenmeTarihi, EklenmeTarihi) VALUES (@KullaniciID, @KullaniciAdi, @Sifre, @GuncellenmeTarihi, @EklenmeTarihi)");
			VTIslem.AddWithValue("KullaniciID", YeniKayit.KullaniciID);
			VTIslem.AddWithValue("KullaniciAdi", YeniKayit.KullaniciAdi);
			VTIslem.AddWithValue("Sifre", YeniKayit.Sifre);
			VTIslem.AddWithValue("GuncellenmeTarihi", YeniKayit.GuncellenmeTarihi);
			VTIslem.AddWithValue("EklenmeTarihi", YeniKayit.EklenmeTarihi);
			return VTIslem.ExecuteNonQuery();
		}

		public virtual SurecBilgiModel KayitGuncelle(KullaniciTablosuModel GuncelKayit)
		{
			VTIslem.SetCommandText("UPDATE KullaniciTablosu SET KullaniciAdi=@KullaniciAdi, Sifre=@Sifre, GuncellenmeTarihi=@GuncellenmeTarihi, EklenmeTarihi=@EklenmeTarihi WHERE KullaniciID=@KullaniciID");
			VTIslem.AddWithValue("KullaniciAdi", GuncelKayit.KullaniciAdi);
			VTIslem.AddWithValue("Sifre", GuncelKayit.Sifre);
			VTIslem.AddWithValue("GuncellenmeTarihi", GuncelKayit.GuncellenmeTarihi);
			VTIslem.AddWithValue("EklenmeTarihi", GuncelKayit.EklenmeTarihi);
			VTIslem.AddWithValue("KullaniciID", GuncelKayit.KullaniciID);
			return VTIslem.ExecuteNonQuery();
		}

		public virtual SurecBilgiModel KayitSil(string KullaniciID)
		{
			VTIslem.SetCommandText("DELETE FROM KullaniciTablosu WHERE KullaniciID=@KullaniciID");
			VTIslem.AddWithValue("KullaniciID", KullaniciID);
			return VTIslem.ExecuteNonQuery();
		}

		public virtual SurecVeriModel<KullaniciTablosuModel> KayitBilgisi(string KullaniciID)
		{
			VTIslem.SetCommandText("SELECT * FROM KullaniciTablosu WHERE KullaniciID = @KullaniciID");
			VTIslem.AddWithValue("KullaniciID", KullaniciID);
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
					SDataModel = new SurecVeriModel<KullaniciTablosuModel>
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
				SDataModel = new SurecVeriModel<KullaniciTablosuModel>
				{
					Sonuc = SModel.Sonuc,
					KullaniciMesaji = SModel.KullaniciMesaji,
					HataBilgi = SModel.HataBilgi
				};
			}
			VTIslem.CloseConnection();
			return SDataModel;
		}

		public virtual SurecVeriModel<IList<KullaniciTablosuModel>> KayitBilgileri()
		{
			VTIslem.SetCommandText("SELECT * FROM KullaniciTablosu");
			VTIslem.OpenConnection();
			SModel = VTIslem.ExecuteReader(CommandBehavior.Default);
			if (SModel.Sonuc.Equals(Sonuclar.Basarili))
			{
				VeriListe = new List<KullaniciTablosuModel>();
				while (SModel.Reader.Read())
				{
					if (KayitBilgisiAl().Sonuc.Equals(Sonuclar.Basarili))
					{
						VeriListe.Add(SDataModel.Veriler);
					}
					else
					{
						SDataListModel = new SurecVeriModel<IList<KullaniciTablosuModel>>{
							Sonuc = SDataModel.Sonuc,
							KullaniciMesaji = SDataModel.KullaniciMesaji,
							HataBilgi = SDataModel.HataBilgi
						};
						VTIslem.CloseConnection();
						return SDataListModel;
					}
				}
				SDataListModel = new SurecVeriModel<IList<KullaniciTablosuModel>>{
					Sonuc = Sonuclar.Basarili,
					KullaniciMesaji = "Veri listesi başarıyla çekildi",
					Veriler = VeriListe
				};
			}
			else
			{
				SDataListModel = new SurecVeriModel<IList<KullaniciTablosuModel>>{
					Sonuc = SModel.Sonuc,
					KullaniciMesaji = SModel.KullaniciMesaji,
					HataBilgi = SModel.HataBilgi
				};
			}
			VTIslem.CloseConnection();
			return SDataListModel;
		}

		SurecVeriModel<KullaniciTablosuModel> KayitBilgisiAl()
		{
			try
			{
				SDataModel = new SurecVeriModel<KullaniciTablosuModel>{
					Sonuc = Sonuclar.Basarili,
					KullaniciMesaji = "Veri bilgisi başarıyla çekilmiştir.",
					Veriler = new KullaniciTablosuModel
					{
						KullaniciID = SModel.Reader.GetString(0),
						KullaniciAdi = SModel.Reader.GetString(1),
						Sifre = SModel.Reader.GetString(2),
						GuncellenmeTarihi = SModel.Reader.GetDateTime(3),
						EklenmeTarihi = SModel.Reader.GetDateTime(4),
					}
				};

			}
			catch (InvalidCastException ex)
			{
				SDataModel = new SurecVeriModel<KullaniciTablosuModel>{
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
				SDataModel = new SurecVeriModel<KullaniciTablosuModel>{
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

		public virtual SurecVeriModel<KullaniciTablosuModel> KayitBilgisiAl(int Baslangic, DbDataReader Reader)
		{
			try
			{
				SDataModel = new SurecVeriModel<KullaniciTablosuModel>{
					Sonuc = Sonuclar.Basarili,
					KullaniciMesaji = "Veri bilgisi başarıyla çekilmiştir.",
					Veriler = new KullaniciTablosuModel{
						KullaniciID = Reader.GetString(Baslangic + 0),
						KullaniciAdi = Reader.GetString(Baslangic + 1),
						Sifre = Reader.GetString(Baslangic + 2),
						GuncellenmeTarihi = Reader.GetDateTime(Baslangic + 3),
						EklenmeTarihi = Reader.GetDateTime(Baslangic + 4),
					}
				};
			}
			catch (InvalidCastException ex)
			{
				SDataModel = new SurecVeriModel<KullaniciTablosuModel>{
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
				SDataModel = new SurecVeriModel<KullaniciTablosuModel>{
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