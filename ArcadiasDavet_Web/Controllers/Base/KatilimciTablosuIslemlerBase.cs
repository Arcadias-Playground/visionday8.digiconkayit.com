using Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.OleDb;
using VeritabaniIslemSaglayici.Access;

namespace VeritabaniIslemMerkeziBase
{
	public abstract class KatilimciTablosuIslemlerBase
	{
		public VTOperatorleri VTIslem;

		public List<KatilimciTablosuModel> VeriListe;

		public SurecBilgiModel SModel;
		public SurecVeriModel<KatilimciTablosuModel> SDataModel;
		public SurecVeriModel<IList<KatilimciTablosuModel>> SDataListModel;

		public KatilimciTablosuIslemlerBase()
		{
			VTIslem = new VTOperatorleri();
		}

		public KatilimciTablosuIslemlerBase(OleDbTransaction Transcation)
		{
			VTIslem = new VTOperatorleri(Transcation);
		}

		public virtual SurecBilgiModel YeniKayitEkle(KatilimciTablosuModel YeniKayit)
		{
			VTIslem.SetCommandText("INSERT INTO KatilimciTablosu (KatilimciID, KatilimciTipiID, AdSoyad, ePosta, Telefon, Kurum, Unvan, YoneticiOnay, YoneticiOnayTarihi, KatilimciOnay, KatilimciOnayTarihi, AnaKatilimciID, GuncellenmeTarihi, EklenmeTarihi) VALUES (@KatilimciID, @KatilimciTipiID, @AdSoyad, @ePosta, @Telefon, @Kurum, @Unvan, @YoneticiOnay, @YoneticiOnayTarihi, @KatilimciOnay, @KatilimciOnayTarihi, @AnaKatilimciID, @GuncellenmeTarihi, @EklenmeTarihi)");
			VTIslem.AddWithValue("KatilimciID", YeniKayit.KatilimciID);
			VTIslem.AddWithValue("KatilimciTipiID", YeniKayit.KatilimciTipiID);
			VTIslem.AddWithValue("AdSoyad", YeniKayit.AdSoyad);
			VTIslem.AddWithValue("ePosta", YeniKayit.ePosta);
			VTIslem.AddWithValue("Telefon", YeniKayit.Telefon);
			VTIslem.AddWithValue("Kurum", YeniKayit.Kurum);
			VTIslem.AddWithValue("Unvan", YeniKayit.Unvan);
			VTIslem.AddWithValue("YoneticiOnay", YeniKayit.YoneticiOnay);

			if(YeniKayit.YoneticiOnayTarihi is null)
				VTIslem.AddWithValue("YoneticiOnayTarihi", DBNull.Value);
			else
				VTIslem.AddWithValue("YoneticiOnayTarihi", YeniKayit.YoneticiOnayTarihi);

			VTIslem.AddWithValue("KatilimciOnay", YeniKayit.KatilimciOnay);

			if(YeniKayit.KatilimciOnayTarihi is null)
				VTIslem.AddWithValue("KatilimciOnayTarihi", DBNull.Value);
			else
				VTIslem.AddWithValue("KatilimciOnayTarihi", YeniKayit.KatilimciOnayTarihi);


			if(YeniKayit.AnaKatilimciID is null)
				VTIslem.AddWithValue("AnaKatilimciID", DBNull.Value);
			else
				VTIslem.AddWithValue("AnaKatilimciID", YeniKayit.AnaKatilimciID);

			VTIslem.AddWithValue("GuncellenmeTarihi", YeniKayit.GuncellenmeTarihi);
			VTIslem.AddWithValue("EklenmeTarihi", YeniKayit.EklenmeTarihi);
			return VTIslem.ExecuteNonQuery();
		}

		public virtual SurecBilgiModel KayitGuncelle(KatilimciTablosuModel GuncelKayit)
		{
			VTIslem.SetCommandText("UPDATE KatilimciTablosu SET KatilimciTipiID=@KatilimciTipiID, AdSoyad=@AdSoyad, ePosta=@ePosta, Telefon=@Telefon, Kurum=@Kurum, Unvan=@Unvan, YoneticiOnay=@YoneticiOnay, YoneticiOnayTarihi=@YoneticiOnayTarihi, KatilimciOnay=@KatilimciOnay, KatilimciOnayTarihi=@KatilimciOnayTarihi, AnaKatilimciID=@AnaKatilimciID, GuncellenmeTarihi=@GuncellenmeTarihi, EklenmeTarihi=@EklenmeTarihi WHERE KatilimciID=@KatilimciID");
			VTIslem.AddWithValue("KatilimciTipiID", GuncelKayit.KatilimciTipiID);
			VTIslem.AddWithValue("AdSoyad", GuncelKayit.AdSoyad);
			VTIslem.AddWithValue("ePosta", GuncelKayit.ePosta);
			VTIslem.AddWithValue("Telefon", GuncelKayit.Telefon);
			VTIslem.AddWithValue("Kurum", GuncelKayit.Kurum);
			VTIslem.AddWithValue("Unvan", GuncelKayit.Unvan);
			VTIslem.AddWithValue("YoneticiOnay", GuncelKayit.YoneticiOnay);

			if(GuncelKayit.YoneticiOnayTarihi is null)
				VTIslem.AddWithValue("YoneticiOnayTarihi", DBNull.Value);
			else
				VTIslem.AddWithValue("YoneticiOnayTarihi", GuncelKayit.YoneticiOnayTarihi.Value);

			VTIslem.AddWithValue("KatilimciOnay", GuncelKayit.KatilimciOnay);

			if(GuncelKayit.KatilimciOnayTarihi is null)
				VTIslem.AddWithValue("KatilimciOnayTarihi", DBNull.Value);
			else
				VTIslem.AddWithValue("KatilimciOnayTarihi", GuncelKayit.KatilimciOnayTarihi.Value);


			if(GuncelKayit.AnaKatilimciID is null)
				VTIslem.AddWithValue("AnaKatilimciID", DBNull.Value);
			else
				VTIslem.AddWithValue("AnaKatilimciID", GuncelKayit.AnaKatilimciID);

			VTIslem.AddWithValue("GuncellenmeTarihi", GuncelKayit.GuncellenmeTarihi);
			VTIslem.AddWithValue("EklenmeTarihi", GuncelKayit.EklenmeTarihi);
			VTIslem.AddWithValue("KatilimciID", GuncelKayit.KatilimciID);
			return VTIslem.ExecuteNonQuery();
		}

		public virtual SurecBilgiModel KayitSil(string KatilimciID)
		{
			VTIslem.SetCommandText("DELETE FROM KatilimciTablosu WHERE KatilimciID=@KatilimciID");
			VTIslem.AddWithValue("KatilimciID", KatilimciID);
			return VTIslem.ExecuteNonQuery();
		}

		public virtual SurecVeriModel<KatilimciTablosuModel> KayitBilgisi(string KatilimciID)
		{
			VTIslem.SetCommandText($"SELECT {KatilimciTablosuModel.SQLSutunSorgusu} FROM KatilimciTablosu WHERE KatilimciID = @KatilimciID");
			VTIslem.AddWithValue("KatilimciID", KatilimciID);
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
					SDataModel = new SurecVeriModel<KatilimciTablosuModel>
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
				SDataModel = new SurecVeriModel<KatilimciTablosuModel>
				{
					Sonuc = SModel.Sonuc,
					KullaniciMesaji = SModel.KullaniciMesaji,
					HataBilgi = SModel.HataBilgi
				};
			}
			VTIslem.CloseConnection();
			return SDataModel;
		}

		public virtual SurecVeriModel<IList<KatilimciTablosuModel>> KayitBilgileri()
		{
			VTIslem.SetCommandText($"SELECT {KatilimciTablosuModel.SQLSutunSorgusu} FROM KatilimciTablosu");
			VTIslem.OpenConnection();
			SModel = VTIslem.ExecuteReader(CommandBehavior.Default);
			if (SModel.Sonuc.Equals(Sonuclar.Basarili))
			{
				VeriListe = new List<KatilimciTablosuModel>();
				while (SModel.Reader.Read())
				{
					if (KayitBilgisiAl().Sonuc.Equals(Sonuclar.Basarili))
					{
						VeriListe.Add(SDataModel.Veriler);
					}
					else
					{
						SDataListModel = new SurecVeriModel<IList<KatilimciTablosuModel>>{
							Sonuc = SDataModel.Sonuc,
							KullaniciMesaji = SDataModel.KullaniciMesaji,
							HataBilgi = SDataModel.HataBilgi
						};
						VTIslem.CloseConnection();
						return SDataListModel;
					}
				}
				SDataListModel = new SurecVeriModel<IList<KatilimciTablosuModel>>{
					Sonuc = Sonuclar.Basarili,
					KullaniciMesaji = "Veri listesi başarıyla çekildi",
					Veriler = VeriListe
				};
			}
			else
			{
				SDataListModel = new SurecVeriModel<IList<KatilimciTablosuModel>>{
					Sonuc = SModel.Sonuc,
					KullaniciMesaji = SModel.KullaniciMesaji,
					HataBilgi = SModel.HataBilgi
				};
			}
			VTIslem.CloseConnection();
			return SDataListModel;
		}

		SurecVeriModel<KatilimciTablosuModel> KayitBilgisiAl()
		{
			try
			{
				SDataModel = new SurecVeriModel<KatilimciTablosuModel>{
					Sonuc = Sonuclar.Basarili,
					KullaniciMesaji = "Veri bilgisi başarıyla çekilmiştir.",
					Veriler = new KatilimciTablosuModel
					{
						KatilimciID = SModel.Reader.GetString(0),
						KatilimciTipiID = SModel.Reader.GetString(1),
						AdSoyad = SModel.Reader.GetString(2),
						ePosta = SModel.Reader.GetString(3),
						Telefon = SModel.Reader.GetString(4),
						Kurum = SModel.Reader.GetString(5),
						Unvan = SModel.Reader.GetString(6),
						YoneticiOnay = SModel.Reader.GetBoolean(7),
						YoneticiOnayTarihi = SModel.Reader.IsDBNull(8) ? null : new DateTime?(SModel.Reader.GetDateTime(8)),
						KatilimciOnay = SModel.Reader.GetBoolean(9),
						KatilimciOnayTarihi = SModel.Reader.IsDBNull(10) ? null : new DateTime?(SModel.Reader.GetDateTime(10)),
						AnaKatilimciID = SModel.Reader.IsDBNull(11) ? string.Empty : SModel.Reader.GetString(11),
						GuncellenmeTarihi = SModel.Reader.GetDateTime(12),
						EklenmeTarihi = SModel.Reader.GetDateTime(13),
					}
				};

			}
			catch (InvalidCastException ex)
			{
				SDataModel = new SurecVeriModel<KatilimciTablosuModel>{
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
				SDataModel = new SurecVeriModel<KatilimciTablosuModel>{
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

		public virtual SurecVeriModel<KatilimciTablosuModel> KayitBilgisiAl(int Baslangic, DbDataReader Reader)
		{
			try
			{
				SDataModel = new SurecVeriModel<KatilimciTablosuModel>{
					Sonuc = Sonuclar.Basarili,
					KullaniciMesaji = "Veri bilgisi başarıyla çekilmiştir.",
					Veriler = new KatilimciTablosuModel{
						KatilimciID = Reader.GetString(Baslangic + 0),
						KatilimciTipiID = Reader.GetString(Baslangic + 1),
						AdSoyad = Reader.GetString(Baslangic + 2),
						ePosta = Reader.GetString(Baslangic + 3),
						Telefon = Reader.GetString(Baslangic + 4),
						Kurum = Reader.GetString(Baslangic + 5),
						Unvan = Reader.GetString(Baslangic + 6),
						YoneticiOnay = Reader.GetBoolean(Baslangic + 7),
						YoneticiOnayTarihi = Reader.IsDBNull(Baslangic + 8) ? null : new DateTime?(Reader.GetDateTime(Baslangic + 8)),
						KatilimciOnay = Reader.GetBoolean(Baslangic + 9),
						KatilimciOnayTarihi = Reader.IsDBNull(Baslangic + 10) ? null : new DateTime?(Reader.GetDateTime(Baslangic + 10)),
						AnaKatilimciID = Reader.IsDBNull(Baslangic + 11) ? string.Empty : Reader.GetString(Baslangic + 11),
						GuncellenmeTarihi = Reader.GetDateTime(Baslangic + 12),
						EklenmeTarihi = Reader.GetDateTime(Baslangic + 13),
					}
				};
			}
			catch (InvalidCastException ex)
			{
				SDataModel = new SurecVeriModel<KatilimciTablosuModel>{
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
				SDataModel = new SurecVeriModel<KatilimciTablosuModel>{
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

		public virtual SurecVeriModel<IList<KatilimciTablosuModel>> KatilimciTipiBilgileri(string KatilimciTipiID)
		{
			VTIslem.SetCommandText($"SELECT {KatilimciTablosuModel.SQLSutunSorgusu} FROM KatilimciTablosu WHERE KatilimciTipiID=@KatilimciTipiID");
			VTIslem.AddWithValue("KatilimciTipiID", KatilimciTipiID);
			VTIslem.OpenConnection();
			SModel = VTIslem.ExecuteReader(CommandBehavior.Default);
			if (SModel.Sonuc.Equals(Sonuclar.Basarili))
			{
				VeriListe = new List<KatilimciTablosuModel>();
				while (SModel.Reader.Read())
				{
					if (KayitBilgisiAl().Sonuc.Equals(Sonuclar.Basarili))
					{
						VeriListe.Add(SDataModel.Veriler);
					}
					else
					{
						SDataListModel = new SurecVeriModel<IList<KatilimciTablosuModel>>{
							Sonuc = SDataModel.Sonuc,
							KullaniciMesaji = SDataModel.KullaniciMesaji,
							HataBilgi = SDataModel.HataBilgi
						};
						VTIslem.CloseConnection();
						return SDataListModel;
					}
				}
				SDataListModel = new SurecVeriModel<IList<KatilimciTablosuModel>>{
					Sonuc = Sonuclar.Basarili,
					KullaniciMesaji = "Veri listesi başarıyla çekildi",
					Veriler = VeriListe
				};
			}
			else
			{
				SDataListModel = new SurecVeriModel<IList<KatilimciTablosuModel>>{
					Sonuc = SModel.Sonuc,
					KullaniciMesaji = SModel.KullaniciMesaji,
					HataBilgi = SModel.HataBilgi
				};
			}
			VTIslem.CloseConnection();
			return SDataListModel;
		}

		public virtual SurecVeriModel<IList<KatilimciTablosuModel>> KatilimciBilgileri(string KatilimciID)
		{
			VTIslem.SetCommandText($"SELECT {KatilimciTablosuModel.SQLSutunSorgusu} FROM KatilimciTablosu WHERE KatilimciID=@KatilimciID");
			VTIslem.AddWithValue("KatilimciID", KatilimciID);
			VTIslem.OpenConnection();
			SModel = VTIslem.ExecuteReader(CommandBehavior.Default);
			if (SModel.Sonuc.Equals(Sonuclar.Basarili))
			{
				VeriListe = new List<KatilimciTablosuModel>();
				while (SModel.Reader.Read())
				{
					if (KayitBilgisiAl().Sonuc.Equals(Sonuclar.Basarili))
					{
						VeriListe.Add(SDataModel.Veriler);
					}
					else
					{
						SDataListModel = new SurecVeriModel<IList<KatilimciTablosuModel>>{
							Sonuc = SDataModel.Sonuc,
							KullaniciMesaji = SDataModel.KullaniciMesaji,
							HataBilgi = SDataModel.HataBilgi
						};
						VTIslem.CloseConnection();
						return SDataListModel;
					}
				}
				SDataListModel = new SurecVeriModel<IList<KatilimciTablosuModel>>{
					Sonuc = Sonuclar.Basarili,
					KullaniciMesaji = "Veri listesi başarıyla çekildi",
					Veriler = VeriListe
				};
			}
			else
			{
				SDataListModel = new SurecVeriModel<IList<KatilimciTablosuModel>>{
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