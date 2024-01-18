using Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.OleDb;
using VeritabaniIslemSaglayici.Access;

namespace VeritabaniIslemMerkeziBase
{
	public abstract class MailGonderimTablosuIslemlerBase
	{
		public VTOperatorleri VTIslem;

		public List<MailGonderimTablosuModel> VeriListe;

		public SurecBilgiModel SModel;
		public SurecVeriModel<MailGonderimTablosuModel> SDataModel;
		public SurecVeriModel<IList<MailGonderimTablosuModel>> SDataListModel;

		public MailGonderimTablosuIslemlerBase()
		{
			VTIslem = new VTOperatorleri();
		}

		public MailGonderimTablosuIslemlerBase(OleDbTransaction Transcation)
		{
			VTIslem = new VTOperatorleri(Transcation);
		}

		public virtual SurecBilgiModel YeniKayitEkle(MailGonderimTablosuModel YeniKayit)
		{
			VTIslem.SetCommandText("INSERT INTO MailGonderimTablosu (MailGonderimID, KatilimciID, ePosta, MailIcerikID, Durum, GonderimTarihi, EklenmeTarihi) VALUES (@MailGonderimID, @KatilimciID, @ePosta, @MailIcerikID, @Durum, @GonderimTarihi, @EklenmeTarihi)");
			VTIslem.AddWithValue("MailGonderimID", YeniKayit.MailGonderimID);
			VTIslem.AddWithValue("KatilimciID", YeniKayit.KatilimciID);
			VTIslem.AddWithValue("ePosta", YeniKayit.ePosta);
			VTIslem.AddWithValue("MailIcerikID", YeniKayit.MailIcerikID);
			VTIslem.AddWithValue("Durum", YeniKayit.Durum);

			if(YeniKayit.GonderimTarihi is null)
				VTIslem.AddWithValue("GonderimTarihi", DBNull.Value);
			else
				VTIslem.AddWithValue("GonderimTarihi", YeniKayit.GonderimTarihi);

			VTIslem.AddWithValue("EklenmeTarihi", YeniKayit.EklenmeTarihi);
			return VTIslem.ExecuteNonQuery();
		}

		public virtual SurecBilgiModel KayitGuncelle(MailGonderimTablosuModel GuncelKayit)
		{
			VTIslem.SetCommandText("UPDATE MailGonderimTablosu SET KatilimciID=@KatilimciID, ePosta=@ePosta, MailIcerikID=@MailIcerikID, Durum=@Durum, GonderimTarihi=@GonderimTarihi, EklenmeTarihi=@EklenmeTarihi WHERE MailGonderimID=@MailGonderimID");
			VTIslem.AddWithValue("KatilimciID", GuncelKayit.KatilimciID);
			VTIslem.AddWithValue("ePosta", GuncelKayit.ePosta);
			VTIslem.AddWithValue("MailIcerikID", GuncelKayit.MailIcerikID);
			VTIslem.AddWithValue("Durum", GuncelKayit.Durum);

			if(GuncelKayit.GonderimTarihi is null)
				VTIslem.AddWithValue("GonderimTarihi", DBNull.Value);
			else
				VTIslem.AddWithValue("GonderimTarihi", GuncelKayit.GonderimTarihi.Value);

			VTIslem.AddWithValue("EklenmeTarihi", GuncelKayit.EklenmeTarihi);
			VTIslem.AddWithValue("MailGonderimID", GuncelKayit.MailGonderimID);
			return VTIslem.ExecuteNonQuery();
		}

		public virtual SurecBilgiModel KayitSil(string MailGonderimID)
		{
			VTIslem.SetCommandText("DELETE FROM MailGonderimTablosu WHERE MailGonderimID=@MailGonderimID");
			VTIslem.AddWithValue("MailGonderimID", MailGonderimID);
			return VTIslem.ExecuteNonQuery();
		}

		public virtual SurecVeriModel<MailGonderimTablosuModel> KayitBilgisi(string MailGonderimID)
		{
			VTIslem.SetCommandText($"SELECT {MailGonderimTablosuModel.SQLSutunSorgusu} FROM MailGonderimTablosu WHERE MailGonderimID = @MailGonderimID");
			VTIslem.AddWithValue("MailGonderimID", MailGonderimID);
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
					SDataModel = new SurecVeriModel<MailGonderimTablosuModel>
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
				SDataModel = new SurecVeriModel<MailGonderimTablosuModel>
				{
					Sonuc = SModel.Sonuc,
					KullaniciMesaji = SModel.KullaniciMesaji,
					HataBilgi = SModel.HataBilgi
				};
			}
			VTIslem.CloseConnection();
			return SDataModel;
		}

		public virtual SurecVeriModel<IList<MailGonderimTablosuModel>> KayitBilgileri()
		{
			VTIslem.SetCommandText($"SELECT {MailGonderimTablosuModel.SQLSutunSorgusu} FROM MailGonderimTablosu");
			VTIslem.OpenConnection();
			SModel = VTIslem.ExecuteReader(CommandBehavior.Default);
			if (SModel.Sonuc.Equals(Sonuclar.Basarili))
			{
				VeriListe = new List<MailGonderimTablosuModel>();
				while (SModel.Reader.Read())
				{
					if (KayitBilgisiAl().Sonuc.Equals(Sonuclar.Basarili))
					{
						VeriListe.Add(SDataModel.Veriler);
					}
					else
					{
						SDataListModel = new SurecVeriModel<IList<MailGonderimTablosuModel>>{
							Sonuc = SDataModel.Sonuc,
							KullaniciMesaji = SDataModel.KullaniciMesaji,
							HataBilgi = SDataModel.HataBilgi
						};
						VTIslem.CloseConnection();
						return SDataListModel;
					}
				}
				SDataListModel = new SurecVeriModel<IList<MailGonderimTablosuModel>>{
					Sonuc = Sonuclar.Basarili,
					KullaniciMesaji = "Veri listesi başarıyla çekildi",
					Veriler = VeriListe
				};
			}
			else
			{
				SDataListModel = new SurecVeriModel<IList<MailGonderimTablosuModel>>{
					Sonuc = SModel.Sonuc,
					KullaniciMesaji = SModel.KullaniciMesaji,
					HataBilgi = SModel.HataBilgi
				};
			}
			VTIslem.CloseConnection();
			return SDataListModel;
		}

		SurecVeriModel<MailGonderimTablosuModel> KayitBilgisiAl()
		{
			try
			{
				SDataModel = new SurecVeriModel<MailGonderimTablosuModel>{
					Sonuc = Sonuclar.Basarili,
					KullaniciMesaji = "Veri bilgisi başarıyla çekilmiştir.",
					Veriler = new MailGonderimTablosuModel
					{
						MailGonderimID = SModel.Reader.GetString(0),
						KatilimciID = SModel.Reader.GetString(1),
						ePosta = SModel.Reader.GetString(2),
						MailIcerikID = SModel.Reader.GetInt32(3),
						Durum = SModel.Reader.GetBoolean(4),
						GonderimTarihi = SModel.Reader.IsDBNull(5) ? null : new DateTime?(SModel.Reader.GetDateTime(5)),
						EklenmeTarihi = SModel.Reader.GetDateTime(6),
					}
				};

			}
			catch (InvalidCastException ex)
			{
				SDataModel = new SurecVeriModel<MailGonderimTablosuModel>{
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
				SDataModel = new SurecVeriModel<MailGonderimTablosuModel>{
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

		public virtual SurecVeriModel<MailGonderimTablosuModel> KayitBilgisiAl(int Baslangic, DbDataReader Reader)
		{
			try
			{
				SDataModel = new SurecVeriModel<MailGonderimTablosuModel>{
					Sonuc = Sonuclar.Basarili,
					KullaniciMesaji = "Veri bilgisi başarıyla çekilmiştir.",
					Veriler = new MailGonderimTablosuModel{
						MailGonderimID = Reader.GetString(Baslangic + 0),
						KatilimciID = Reader.GetString(Baslangic + 1),
						ePosta = Reader.GetString(Baslangic + 2),
						MailIcerikID = Reader.GetInt32(Baslangic + 3),
						Durum = Reader.GetBoolean(Baslangic + 4),
						GonderimTarihi = Reader.IsDBNull(Baslangic + 5) ? null : new DateTime?(Reader.GetDateTime(Baslangic + 5)),
						EklenmeTarihi = Reader.GetDateTime(Baslangic + 6),
					}
				};
			}
			catch (InvalidCastException ex)
			{
				SDataModel = new SurecVeriModel<MailGonderimTablosuModel>{
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
				SDataModel = new SurecVeriModel<MailGonderimTablosuModel>{
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

		public virtual SurecVeriModel<IList<MailGonderimTablosuModel>> KatilimciBilgileri(string KatilimciID)
		{
			VTIslem.SetCommandText($"SELECT {MailGonderimTablosuModel.SQLSutunSorgusu} FROM MailGonderimTablosu WHERE KatilimciID=@KatilimciID");
			VTIslem.AddWithValue("KatilimciID", KatilimciID);
			VTIslem.OpenConnection();
			SModel = VTIslem.ExecuteReader(CommandBehavior.Default);
			if (SModel.Sonuc.Equals(Sonuclar.Basarili))
			{
				VeriListe = new List<MailGonderimTablosuModel>();
				while (SModel.Reader.Read())
				{
					if (KayitBilgisiAl().Sonuc.Equals(Sonuclar.Basarili))
					{
						VeriListe.Add(SDataModel.Veriler);
					}
					else
					{
						SDataListModel = new SurecVeriModel<IList<MailGonderimTablosuModel>>{
							Sonuc = SDataModel.Sonuc,
							KullaniciMesaji = SDataModel.KullaniciMesaji,
							HataBilgi = SDataModel.HataBilgi
						};
						VTIslem.CloseConnection();
						return SDataListModel;
					}
				}
				SDataListModel = new SurecVeriModel<IList<MailGonderimTablosuModel>>{
					Sonuc = Sonuclar.Basarili,
					KullaniciMesaji = "Veri listesi başarıyla çekildi",
					Veriler = VeriListe
				};
			}
			else
			{
				SDataListModel = new SurecVeriModel<IList<MailGonderimTablosuModel>>{
					Sonuc = SModel.Sonuc,
					KullaniciMesaji = SModel.KullaniciMesaji,
					HataBilgi = SModel.HataBilgi
				};
			}
			VTIslem.CloseConnection();
			return SDataListModel;
		}

		public virtual SurecVeriModel<IList<MailGonderimTablosuModel>> MailIcerikBilgileri(int MailIcerikID)
		{
			VTIslem.SetCommandText($"SELECT {MailGonderimTablosuModel.SQLSutunSorgusu} FROM MailGonderimTablosu WHERE MailIcerikID=@MailIcerikID");
			VTIslem.AddWithValue("MailIcerikID", MailIcerikID);
			VTIslem.OpenConnection();
			SModel = VTIslem.ExecuteReader(CommandBehavior.Default);
			if (SModel.Sonuc.Equals(Sonuclar.Basarili))
			{
				VeriListe = new List<MailGonderimTablosuModel>();
				while (SModel.Reader.Read())
				{
					if (KayitBilgisiAl().Sonuc.Equals(Sonuclar.Basarili))
					{
						VeriListe.Add(SDataModel.Veriler);
					}
					else
					{
						SDataListModel = new SurecVeriModel<IList<MailGonderimTablosuModel>>{
							Sonuc = SDataModel.Sonuc,
							KullaniciMesaji = SDataModel.KullaniciMesaji,
							HataBilgi = SDataModel.HataBilgi
						};
						VTIslem.CloseConnection();
						return SDataListModel;
					}
				}
				SDataListModel = new SurecVeriModel<IList<MailGonderimTablosuModel>>{
					Sonuc = Sonuclar.Basarili,
					KullaniciMesaji = "Veri listesi başarıyla çekildi",
					Veriler = VeriListe
				};
			}
			else
			{
				SDataListModel = new SurecVeriModel<IList<MailGonderimTablosuModel>>{
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