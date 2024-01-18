using Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.OleDb;
using VeritabaniIslemSaglayici.Access;

namespace VeritabaniIslemMerkeziBase
{
	public abstract class AntetliKagitIcerikTablosuIslemlerBase
	{
		public VTOperatorleri VTIslem;

		public List<AntetliKagitIcerikTablosuModel> VeriListe;

		public SurecBilgiModel SModel;
		public SurecVeriModel<AntetliKagitIcerikTablosuModel> SDataModel;
		public SurecVeriModel<IList<AntetliKagitIcerikTablosuModel>> SDataListModel;

		public AntetliKagitIcerikTablosuIslemlerBase()
		{
			VTIslem = new VTOperatorleri();
		}

		public AntetliKagitIcerikTablosuIslemlerBase(OleDbTransaction Transcation)
		{
			VTIslem = new VTOperatorleri(Transcation);
		}

		public virtual SurecBilgiModel YeniKayitEkle(AntetliKagitIcerikTablosuModel YeniKayit)
		{
			VTIslem.SetCommandText("INSERT INTO AntetliKagitIcerikTablosu (AntetliKagitIcerikTipiID, MailIcerikID, X, Y, Width, Height, GuncellenmeTarihi, EklenmeTarihi) VALUES (@AntetliKagitIcerikTipiID, @MailIcerikID, @X, @Y, @Width, @Height, @GuncellenmeTarihi, @EklenmeTarihi)");
			VTIslem.AddWithValue("AntetliKagitIcerikTipiID", YeniKayit.AntetliKagitIcerikTipiID);
			VTIslem.AddWithValue("MailIcerikID", YeniKayit.MailIcerikID);
			VTIslem.AddWithValue("X", YeniKayit.X);
			VTIslem.AddWithValue("Y", YeniKayit.Y);
			VTIslem.AddWithValue("Width", YeniKayit.Width);
			VTIslem.AddWithValue("Height", YeniKayit.Height);
			VTIslem.AddWithValue("GuncellenmeTarihi", YeniKayit.GuncellenmeTarihi);
			VTIslem.AddWithValue("EklenmeTarihi", YeniKayit.EklenmeTarihi);
			return VTIslem.ExecuteNonQuery();
		}

		public virtual SurecBilgiModel KayitGuncelle(AntetliKagitIcerikTablosuModel GuncelKayit)
		{
			VTIslem.SetCommandText("UPDATE AntetliKagitIcerikTablosu SET AntetliKagitIcerikTipiID=@AntetliKagitIcerikTipiID, MailIcerikID=@MailIcerikID, X=@X, Y=@Y, Width=@Width, Height=@Height, GuncellenmeTarihi=@GuncellenmeTarihi, EklenmeTarihi=@EklenmeTarihi WHERE AntetliKagitIcerikID=@AntetliKagitIcerikID");
			VTIslem.AddWithValue("AntetliKagitIcerikTipiID", GuncelKayit.AntetliKagitIcerikTipiID);
			VTIslem.AddWithValue("MailIcerikID", GuncelKayit.MailIcerikID);
			VTIslem.AddWithValue("X", GuncelKayit.X);
			VTIslem.AddWithValue("Y", GuncelKayit.Y);
			VTIslem.AddWithValue("Width", GuncelKayit.Width);
			VTIslem.AddWithValue("Height", GuncelKayit.Height);
			VTIslem.AddWithValue("GuncellenmeTarihi", GuncelKayit.GuncellenmeTarihi);
			VTIslem.AddWithValue("EklenmeTarihi", GuncelKayit.EklenmeTarihi);
			VTIslem.AddWithValue("AntetliKagitIcerikID", GuncelKayit.AntetliKagitIcerikID);
			return VTIslem.ExecuteNonQuery();
		}

		public virtual SurecBilgiModel KayitSil(int AntetliKagitIcerikID)
		{
			VTIslem.SetCommandText("DELETE FROM AntetliKagitIcerikTablosu WHERE AntetliKagitIcerikID=@AntetliKagitIcerikID");
			VTIslem.AddWithValue("AntetliKagitIcerikID", AntetliKagitIcerikID);
			return VTIslem.ExecuteNonQuery();
		}

		public virtual SurecVeriModel<AntetliKagitIcerikTablosuModel> KayitBilgisi(int AntetliKagitIcerikID)
		{
			VTIslem.SetCommandText($"SELECT {AntetliKagitIcerikTablosuModel.SQLSutunSorgusu} FROM AntetliKagitIcerikTablosu WHERE AntetliKagitIcerikID = @AntetliKagitIcerikID");
			VTIslem.AddWithValue("AntetliKagitIcerikID", AntetliKagitIcerikID);
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
					SDataModel = new SurecVeriModel<AntetliKagitIcerikTablosuModel>
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
				SDataModel = new SurecVeriModel<AntetliKagitIcerikTablosuModel>
				{
					Sonuc = SModel.Sonuc,
					KullaniciMesaji = SModel.KullaniciMesaji,
					HataBilgi = SModel.HataBilgi
				};
			}
			VTIslem.CloseConnection();
			return SDataModel;
		}

		public virtual SurecVeriModel<IList<AntetliKagitIcerikTablosuModel>> KayitBilgileri()
		{
			VTIslem.SetCommandText($"SELECT {AntetliKagitIcerikTablosuModel.SQLSutunSorgusu} FROM AntetliKagitIcerikTablosu");
			VTIslem.OpenConnection();
			SModel = VTIslem.ExecuteReader(CommandBehavior.Default);
			if (SModel.Sonuc.Equals(Sonuclar.Basarili))
			{
				VeriListe = new List<AntetliKagitIcerikTablosuModel>();
				while (SModel.Reader.Read())
				{
					if (KayitBilgisiAl().Sonuc.Equals(Sonuclar.Basarili))
					{
						VeriListe.Add(SDataModel.Veriler);
					}
					else
					{
						SDataListModel = new SurecVeriModel<IList<AntetliKagitIcerikTablosuModel>>{
							Sonuc = SDataModel.Sonuc,
							KullaniciMesaji = SDataModel.KullaniciMesaji,
							HataBilgi = SDataModel.HataBilgi
						};
						VTIslem.CloseConnection();
						return SDataListModel;
					}
				}
				SDataListModel = new SurecVeriModel<IList<AntetliKagitIcerikTablosuModel>>{
					Sonuc = Sonuclar.Basarili,
					KullaniciMesaji = "Veri listesi başarıyla çekildi",
					Veriler = VeriListe
				};
			}
			else
			{
				SDataListModel = new SurecVeriModel<IList<AntetliKagitIcerikTablosuModel>>{
					Sonuc = SModel.Sonuc,
					KullaniciMesaji = SModel.KullaniciMesaji,
					HataBilgi = SModel.HataBilgi
				};
			}
			VTIslem.CloseConnection();
			return SDataListModel;
		}

		SurecVeriModel<AntetliKagitIcerikTablosuModel> KayitBilgisiAl()
		{
			try
			{
				SDataModel = new SurecVeriModel<AntetliKagitIcerikTablosuModel>{
					Sonuc = Sonuclar.Basarili,
					KullaniciMesaji = "Veri bilgisi başarıyla çekilmiştir.",
					Veriler = new AntetliKagitIcerikTablosuModel
					{
						AntetliKagitIcerikID = SModel.Reader.GetInt32(0),
						AntetliKagitIcerikTipiID = SModel.Reader.GetInt32(1),
						MailIcerikID = SModel.Reader.GetInt32(2),
						X = SModel.Reader.GetInt32(3),
						Y = SModel.Reader.GetInt32(4),
						Width = SModel.Reader.GetInt32(5),
						Height = SModel.Reader.GetInt32(6),
						GuncellenmeTarihi = SModel.Reader.GetDateTime(7),
						EklenmeTarihi = SModel.Reader.GetDateTime(8),
					}
				};

			}
			catch (InvalidCastException ex)
			{
				SDataModel = new SurecVeriModel<AntetliKagitIcerikTablosuModel>{
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
				SDataModel = new SurecVeriModel<AntetliKagitIcerikTablosuModel>{
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

		public virtual SurecVeriModel<AntetliKagitIcerikTablosuModel> KayitBilgisiAl(int Baslangic, DbDataReader Reader)
		{
			try
			{
				SDataModel = new SurecVeriModel<AntetliKagitIcerikTablosuModel>{
					Sonuc = Sonuclar.Basarili,
					KullaniciMesaji = "Veri bilgisi başarıyla çekilmiştir.",
					Veriler = new AntetliKagitIcerikTablosuModel{
						AntetliKagitIcerikID = Reader.GetInt32(Baslangic + 0),
						AntetliKagitIcerikTipiID = Reader.GetInt32(Baslangic + 1),
						MailIcerikID = Reader.GetInt32(Baslangic + 2),
						X = Reader.GetInt32(Baslangic + 3),
						Y = Reader.GetInt32(Baslangic + 4),
						Width = Reader.GetInt32(Baslangic + 5),
						Height = Reader.GetInt32(Baslangic + 6),
						GuncellenmeTarihi = Reader.GetDateTime(Baslangic + 7),
						EklenmeTarihi = Reader.GetDateTime(Baslangic + 8),
					}
				};
			}
			catch (InvalidCastException ex)
			{
				SDataModel = new SurecVeriModel<AntetliKagitIcerikTablosuModel>{
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
				SDataModel = new SurecVeriModel<AntetliKagitIcerikTablosuModel>{
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

		public virtual SurecVeriModel<IList<AntetliKagitIcerikTablosuModel>> AntetliKagitIcerikTipiBilgileri(int AntetliKagitIcerikTipiID)
		{
			VTIslem.SetCommandText($"SELECT {AntetliKagitIcerikTablosuModel.SQLSutunSorgusu} FROM AntetliKagitIcerikTablosu WHERE AntetliKagitIcerikTipiID=@AntetliKagitIcerikTipiID");
			VTIslem.AddWithValue("AntetliKagitIcerikTipiID", AntetliKagitIcerikTipiID);
			VTIslem.OpenConnection();
			SModel = VTIslem.ExecuteReader(CommandBehavior.Default);
			if (SModel.Sonuc.Equals(Sonuclar.Basarili))
			{
				VeriListe = new List<AntetliKagitIcerikTablosuModel>();
				while (SModel.Reader.Read())
				{
					if (KayitBilgisiAl().Sonuc.Equals(Sonuclar.Basarili))
					{
						VeriListe.Add(SDataModel.Veriler);
					}
					else
					{
						SDataListModel = new SurecVeriModel<IList<AntetliKagitIcerikTablosuModel>>{
							Sonuc = SDataModel.Sonuc,
							KullaniciMesaji = SDataModel.KullaniciMesaji,
							HataBilgi = SDataModel.HataBilgi
						};
						VTIslem.CloseConnection();
						return SDataListModel;
					}
				}
				SDataListModel = new SurecVeriModel<IList<AntetliKagitIcerikTablosuModel>>{
					Sonuc = Sonuclar.Basarili,
					KullaniciMesaji = "Veri listesi başarıyla çekildi",
					Veriler = VeriListe
				};
			}
			else
			{
				SDataListModel = new SurecVeriModel<IList<AntetliKagitIcerikTablosuModel>>{
					Sonuc = SModel.Sonuc,
					KullaniciMesaji = SModel.KullaniciMesaji,
					HataBilgi = SModel.HataBilgi
				};
			}
			VTIslem.CloseConnection();
			return SDataListModel;
		}

		public virtual SurecVeriModel<IList<AntetliKagitIcerikTablosuModel>> MailIcerikBilgileri(int MailIcerikID)
		{
			VTIslem.SetCommandText($"SELECT {AntetliKagitIcerikTablosuModel.SQLSutunSorgusu} FROM AntetliKagitIcerikTablosu WHERE MailIcerikID=@MailIcerikID");
			VTIslem.AddWithValue("MailIcerikID", MailIcerikID);
			VTIslem.OpenConnection();
			SModel = VTIslem.ExecuteReader(CommandBehavior.Default);
			if (SModel.Sonuc.Equals(Sonuclar.Basarili))
			{
				VeriListe = new List<AntetliKagitIcerikTablosuModel>();
				while (SModel.Reader.Read())
				{
					if (KayitBilgisiAl().Sonuc.Equals(Sonuclar.Basarili))
					{
						VeriListe.Add(SDataModel.Veriler);
					}
					else
					{
						SDataListModel = new SurecVeriModel<IList<AntetliKagitIcerikTablosuModel>>{
							Sonuc = SDataModel.Sonuc,
							KullaniciMesaji = SDataModel.KullaniciMesaji,
							HataBilgi = SDataModel.HataBilgi
						};
						VTIslem.CloseConnection();
						return SDataListModel;
					}
				}
				SDataListModel = new SurecVeriModel<IList<AntetliKagitIcerikTablosuModel>>{
					Sonuc = Sonuclar.Basarili,
					KullaniciMesaji = "Veri listesi başarıyla çekildi",
					Veriler = VeriListe
				};
			}
			else
			{
				SDataListModel = new SurecVeriModel<IList<AntetliKagitIcerikTablosuModel>>{
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