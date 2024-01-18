using Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.OleDb;
using VeritabaniIslemSaglayici.Access;

namespace VeritabaniIslemMerkeziBase
{
	public abstract class KatilimciTipiTablosuIslemlerBase
	{
		public VTOperatorleri VTIslem;

		public List<KatilimciTipiTablosuModel> VeriListe;

		public SurecBilgiModel SModel;
		public SurecVeriModel<KatilimciTipiTablosuModel> SDataModel;
		public SurecVeriModel<IList<KatilimciTipiTablosuModel>> SDataListModel;

		public KatilimciTipiTablosuIslemlerBase()
		{
			VTIslem = new VTOperatorleri();
		}

		public KatilimciTipiTablosuIslemlerBase(OleDbTransaction Transcation)
		{
			VTIslem = new VTOperatorleri(Transcation);
		}

		public virtual SurecBilgiModel YeniKayitEkle(KatilimciTipiTablosuModel YeniKayit)
		{
			VTIslem.SetCommandText("INSERT INTO KatilimciTipiTablosu (KatilimciTipiID, KatilimciTipi, Kontenjan, MisafirKontenjan, GirisSayisi, YakaKartiBasimSayisi, KabulEkranIcerik, RedEkranIcerik, GuncellenmeTarihi, EklenmeTarihi) VALUES (@KatilimciTipiID, @KatilimciTipi, @Kontenjan, @MisafirKontenjan, @GirisSayisi, @YakaKartiBasimSayisi, @KabulEkranIcerik, @RedEkranIcerik, @GuncellenmeTarihi, @EklenmeTarihi)");
			VTIslem.AddWithValue("KatilimciTipiID", YeniKayit.KatilimciTipiID);
			VTIslem.AddWithValue("KatilimciTipi", YeniKayit.KatilimciTipi);
			VTIslem.AddWithValue("Kontenjan", YeniKayit.Kontenjan);
			VTIslem.AddWithValue("MisafirKontenjan", YeniKayit.MisafirKontenjan);
			VTIslem.AddWithValue("GirisSayisi", YeniKayit.GirisSayisi);
			VTIslem.AddWithValue("YakaKartiBasimSayisi", YeniKayit.YakaKartiBasimSayisi);
			VTIslem.AddWithValue("KabulEkranIcerik", YeniKayit.KabulEkranIcerik);
			VTIslem.AddWithValue("RedEkranIcerik", YeniKayit.RedEkranIcerik);
			VTIslem.AddWithValue("GuncellenmeTarihi", YeniKayit.GuncellenmeTarihi);
			VTIslem.AddWithValue("EklenmeTarihi", YeniKayit.EklenmeTarihi);
			return VTIslem.ExecuteNonQuery();
		}

		public virtual SurecBilgiModel KayitGuncelle(KatilimciTipiTablosuModel GuncelKayit)
		{
			VTIslem.SetCommandText("UPDATE KatilimciTipiTablosu SET KatilimciTipi=@KatilimciTipi, Kontenjan=@Kontenjan, MisafirKontenjan=@MisafirKontenjan, GirisSayisi=@GirisSayisi, YakaKartiBasimSayisi=@YakaKartiBasimSayisi, KabulEkranIcerik=@KabulEkranIcerik, RedEkranIcerik=@RedEkranIcerik, GuncellenmeTarihi=@GuncellenmeTarihi, EklenmeTarihi=@EklenmeTarihi WHERE KatilimciTipiID=@KatilimciTipiID");
			VTIslem.AddWithValue("KatilimciTipi", GuncelKayit.KatilimciTipi);
			VTIslem.AddWithValue("Kontenjan", GuncelKayit.Kontenjan);
			VTIslem.AddWithValue("MisafirKontenjan", GuncelKayit.MisafirKontenjan);
			VTIslem.AddWithValue("GirisSayisi", GuncelKayit.GirisSayisi);
			VTIslem.AddWithValue("YakaKartiBasimSayisi", GuncelKayit.YakaKartiBasimSayisi);
			VTIslem.AddWithValue("KabulEkranIcerik", GuncelKayit.KabulEkranIcerik);
			VTIslem.AddWithValue("RedEkranIcerik", GuncelKayit.RedEkranIcerik);
			VTIslem.AddWithValue("GuncellenmeTarihi", GuncelKayit.GuncellenmeTarihi);
			VTIslem.AddWithValue("EklenmeTarihi", GuncelKayit.EklenmeTarihi);
			VTIslem.AddWithValue("KatilimciTipiID", GuncelKayit.KatilimciTipiID);
			return VTIslem.ExecuteNonQuery();
		}

		public virtual SurecBilgiModel KayitSil(string KatilimciTipiID)
		{
			VTIslem.SetCommandText("DELETE FROM KatilimciTipiTablosu WHERE KatilimciTipiID=@KatilimciTipiID");
			VTIslem.AddWithValue("KatilimciTipiID", KatilimciTipiID);
			return VTIslem.ExecuteNonQuery();
		}

		public virtual SurecVeriModel<KatilimciTipiTablosuModel> KayitBilgisi(string KatilimciTipiID)
		{
			VTIslem.SetCommandText($"SELECT {KatilimciTipiTablosuModel.SQLSutunSorgusu} FROM KatilimciTipiTablosu WHERE KatilimciTipiID = @KatilimciTipiID");
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
					SDataModel = new SurecVeriModel<KatilimciTipiTablosuModel>
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
				SDataModel = new SurecVeriModel<KatilimciTipiTablosuModel>
				{
					Sonuc = SModel.Sonuc,
					KullaniciMesaji = SModel.KullaniciMesaji,
					HataBilgi = SModel.HataBilgi
				};
			}
			VTIslem.CloseConnection();
			return SDataModel;
		}

		public virtual SurecVeriModel<IList<KatilimciTipiTablosuModel>> KayitBilgileri()
		{
			VTIslem.SetCommandText($"SELECT {KatilimciTipiTablosuModel.SQLSutunSorgusu} FROM KatilimciTipiTablosu");
			VTIslem.OpenConnection();
			SModel = VTIslem.ExecuteReader(CommandBehavior.Default);
			if (SModel.Sonuc.Equals(Sonuclar.Basarili))
			{
				VeriListe = new List<KatilimciTipiTablosuModel>();
				while (SModel.Reader.Read())
				{
					if (KayitBilgisiAl().Sonuc.Equals(Sonuclar.Basarili))
					{
						VeriListe.Add(SDataModel.Veriler);
					}
					else
					{
						SDataListModel = new SurecVeriModel<IList<KatilimciTipiTablosuModel>>{
							Sonuc = SDataModel.Sonuc,
							KullaniciMesaji = SDataModel.KullaniciMesaji,
							HataBilgi = SDataModel.HataBilgi
						};
						VTIslem.CloseConnection();
						return SDataListModel;
					}
				}
				SDataListModel = new SurecVeriModel<IList<KatilimciTipiTablosuModel>>{
					Sonuc = Sonuclar.Basarili,
					KullaniciMesaji = "Veri listesi başarıyla çekildi",
					Veriler = VeriListe
				};
			}
			else
			{
				SDataListModel = new SurecVeriModel<IList<KatilimciTipiTablosuModel>>{
					Sonuc = SModel.Sonuc,
					KullaniciMesaji = SModel.KullaniciMesaji,
					HataBilgi = SModel.HataBilgi
				};
			}
			VTIslem.CloseConnection();
			return SDataListModel;
		}

		SurecVeriModel<KatilimciTipiTablosuModel> KayitBilgisiAl()
		{
			try
			{
				SDataModel = new SurecVeriModel<KatilimciTipiTablosuModel>{
					Sonuc = Sonuclar.Basarili,
					KullaniciMesaji = "Veri bilgisi başarıyla çekilmiştir.",
					Veriler = new KatilimciTipiTablosuModel
					{
						KatilimciTipiID = SModel.Reader.GetString(0),
						KatilimciTipi = SModel.Reader.GetString(1),
						Kontenjan = SModel.Reader.GetInt32(2),
						MisafirKontenjan = SModel.Reader.GetInt32(3),
						GirisSayisi = SModel.Reader.GetInt32(4),
						YakaKartiBasimSayisi = SModel.Reader.GetInt32(5),
						KabulEkranIcerik = SModel.Reader.GetString(6),
						RedEkranIcerik = SModel.Reader.GetString(7),
						GuncellenmeTarihi = SModel.Reader.GetDateTime(8),
						EklenmeTarihi = SModel.Reader.GetDateTime(9),
					}
				};

			}
			catch (InvalidCastException ex)
			{
				SDataModel = new SurecVeriModel<KatilimciTipiTablosuModel>{
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
				SDataModel = new SurecVeriModel<KatilimciTipiTablosuModel>{
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

		public virtual SurecVeriModel<KatilimciTipiTablosuModel> KayitBilgisiAl(int Baslangic, DbDataReader Reader)
		{
			try
			{
				SDataModel = new SurecVeriModel<KatilimciTipiTablosuModel>{
					Sonuc = Sonuclar.Basarili,
					KullaniciMesaji = "Veri bilgisi başarıyla çekilmiştir.",
					Veriler = new KatilimciTipiTablosuModel{
						KatilimciTipiID = Reader.GetString(Baslangic + 0),
						KatilimciTipi = Reader.GetString(Baslangic + 1),
						Kontenjan = Reader.GetInt32(Baslangic + 2),
						MisafirKontenjan = Reader.GetInt32(Baslangic + 3),
						GirisSayisi = Reader.GetInt32(Baslangic + 4),
						YakaKartiBasimSayisi = Reader.GetInt32(Baslangic + 5),
						KabulEkranIcerik = Reader.GetString(Baslangic + 6),
						RedEkranIcerik = Reader.GetString(Baslangic + 7),
						GuncellenmeTarihi = Reader.GetDateTime(Baslangic + 8),
						EklenmeTarihi = Reader.GetDateTime(Baslangic + 9),
					}
				};
			}
			catch (InvalidCastException ex)
			{
				SDataModel = new SurecVeriModel<KatilimciTipiTablosuModel>{
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
				SDataModel = new SurecVeriModel<KatilimciTipiTablosuModel>{
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