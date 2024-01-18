using Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.OleDb;
using VeritabaniIslemSaglayici.Access;

namespace VeritabaniIslemMerkeziBase
{
	public abstract class MailAyarTablosuIslemlerBase
	{
		public VTOperatorleri VTIslem;

		public List<MailAyarTablosuModel> VeriListe;

		public SurecBilgiModel SModel;
		public SurecVeriModel<MailAyarTablosuModel> SDataModel;
		public SurecVeriModel<IList<MailAyarTablosuModel>> SDataListModel;

		public MailAyarTablosuIslemlerBase()
		{
			VTIslem = new VTOperatorleri();
		}

		public MailAyarTablosuIslemlerBase(OleDbTransaction Transcation)
		{
			VTIslem = new VTOperatorleri(Transcation);
		}

		public virtual SurecBilgiModel YeniKayitEkle(MailAyarTablosuModel YeniKayit)
		{
			VTIslem.SetCommandText("INSERT INTO MailAyarTablosu (GonderenAd, ePosta, KullaniciAdi, Sifre, GidenMailHost, GidenMailPort, GelenMailHost, GelenMailPort, SSL, BCC, ReplyTo, GuncellenmeTarihi, EklemeTarihi) VALUES (@GonderenAd, @ePosta, @KullaniciAdi, @Sifre, @GidenMailHost, @GidenMailPort, @GelenMailHost, @GelenMailPort, @SSL, @BCC, @ReplyTo, @GuncellenmeTarihi, @EklemeTarihi)");
			VTIslem.AddWithValue("GonderenAd", YeniKayit.GonderenAd);
			VTIslem.AddWithValue("ePosta", YeniKayit.ePosta);
			VTIslem.AddWithValue("KullaniciAdi", YeniKayit.KullaniciAdi);
			VTIslem.AddWithValue("Sifre", YeniKayit.Sifre);
			VTIslem.AddWithValue("GidenMailHost", YeniKayit.GidenMailHost);
			VTIslem.AddWithValue("GidenMailPort", YeniKayit.GidenMailPort);
			VTIslem.AddWithValue("GelenMailHost", YeniKayit.GelenMailHost);
			VTIslem.AddWithValue("GelenMailPort", YeniKayit.GelenMailPort);
			VTIslem.AddWithValue("SSL", YeniKayit.SSL);
			VTIslem.AddWithValue("BCC", YeniKayit.BCC);
			VTIslem.AddWithValue("ReplyTo", YeniKayit.ReplyTo);
			VTIslem.AddWithValue("GuncellenmeTarihi", YeniKayit.GuncellenmeTarihi);
			VTIslem.AddWithValue("EklemeTarihi", YeniKayit.EklemeTarihi);
			return VTIslem.ExecuteNonQuery();
		}

		public virtual SurecBilgiModel KayitGuncelle(MailAyarTablosuModel GuncelKayit)
		{
			VTIslem.SetCommandText("UPDATE MailAyarTablosu SET GonderenAd=@GonderenAd, ePosta=@ePosta, KullaniciAdi=@KullaniciAdi, Sifre=@Sifre, GidenMailHost=@GidenMailHost, GidenMailPort=@GidenMailPort, GelenMailHost=@GelenMailHost, GelenMailPort=@GelenMailPort, SSL=@SSL, BCC=@BCC, ReplyTo=@ReplyTo, GuncellenmeTarihi=@GuncellenmeTarihi, EklemeTarihi=@EklemeTarihi WHERE MailAyarID=@MailAyarID");
			VTIslem.AddWithValue("GonderenAd", GuncelKayit.GonderenAd);
			VTIslem.AddWithValue("ePosta", GuncelKayit.ePosta);
			VTIslem.AddWithValue("KullaniciAdi", GuncelKayit.KullaniciAdi);
			VTIslem.AddWithValue("Sifre", GuncelKayit.Sifre);
			VTIslem.AddWithValue("GidenMailHost", GuncelKayit.GidenMailHost);
			VTIslem.AddWithValue("GidenMailPort", GuncelKayit.GidenMailPort);
			VTIslem.AddWithValue("GelenMailHost", GuncelKayit.GelenMailHost);
			VTIslem.AddWithValue("GelenMailPort", GuncelKayit.GelenMailPort);
			VTIslem.AddWithValue("SSL", GuncelKayit.SSL);
			VTIslem.AddWithValue("BCC", GuncelKayit.BCC);
			VTIslem.AddWithValue("ReplyTo", GuncelKayit.ReplyTo);
			VTIslem.AddWithValue("GuncellenmeTarihi", GuncelKayit.GuncellenmeTarihi);
			VTIslem.AddWithValue("EklemeTarihi", GuncelKayit.EklemeTarihi);
			VTIslem.AddWithValue("MailAyarID", GuncelKayit.MailAyarID);
			return VTIslem.ExecuteNonQuery();
		}

		public virtual SurecBilgiModel KayitSil(int MailAyarID)
		{
			VTIslem.SetCommandText("DELETE FROM MailAyarTablosu WHERE MailAyarID=@MailAyarID");
			VTIslem.AddWithValue("MailAyarID", MailAyarID);
			return VTIslem.ExecuteNonQuery();
		}

		public virtual SurecVeriModel<MailAyarTablosuModel> KayitBilgisi(int MailAyarID)
		{
			VTIslem.SetCommandText($"SELECT {MailAyarTablosuModel.SQLSutunSorgusu} FROM MailAyarTablosu WHERE MailAyarID = @MailAyarID");
			VTIslem.AddWithValue("MailAyarID", MailAyarID);
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
					SDataModel = new SurecVeriModel<MailAyarTablosuModel>
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
				SDataModel = new SurecVeriModel<MailAyarTablosuModel>
				{
					Sonuc = SModel.Sonuc,
					KullaniciMesaji = SModel.KullaniciMesaji,
					HataBilgi = SModel.HataBilgi
				};
			}
			VTIslem.CloseConnection();
			return SDataModel;
		}

		public virtual SurecVeriModel<IList<MailAyarTablosuModel>> KayitBilgileri()
		{
			VTIslem.SetCommandText($"SELECT {MailAyarTablosuModel.SQLSutunSorgusu} FROM MailAyarTablosu");
			VTIslem.OpenConnection();
			SModel = VTIslem.ExecuteReader(CommandBehavior.Default);
			if (SModel.Sonuc.Equals(Sonuclar.Basarili))
			{
				VeriListe = new List<MailAyarTablosuModel>();
				while (SModel.Reader.Read())
				{
					if (KayitBilgisiAl().Sonuc.Equals(Sonuclar.Basarili))
					{
						VeriListe.Add(SDataModel.Veriler);
					}
					else
					{
						SDataListModel = new SurecVeriModel<IList<MailAyarTablosuModel>>{
							Sonuc = SDataModel.Sonuc,
							KullaniciMesaji = SDataModel.KullaniciMesaji,
							HataBilgi = SDataModel.HataBilgi
						};
						VTIslem.CloseConnection();
						return SDataListModel;
					}
				}
				SDataListModel = new SurecVeriModel<IList<MailAyarTablosuModel>>{
					Sonuc = Sonuclar.Basarili,
					KullaniciMesaji = "Veri listesi başarıyla çekildi",
					Veriler = VeriListe
				};
			}
			else
			{
				SDataListModel = new SurecVeriModel<IList<MailAyarTablosuModel>>{
					Sonuc = SModel.Sonuc,
					KullaniciMesaji = SModel.KullaniciMesaji,
					HataBilgi = SModel.HataBilgi
				};
			}
			VTIslem.CloseConnection();
			return SDataListModel;
		}

		SurecVeriModel<MailAyarTablosuModel> KayitBilgisiAl()
		{
			try
			{
				SDataModel = new SurecVeriModel<MailAyarTablosuModel>{
					Sonuc = Sonuclar.Basarili,
					KullaniciMesaji = "Veri bilgisi başarıyla çekilmiştir.",
					Veriler = new MailAyarTablosuModel
					{
						MailAyarID = SModel.Reader.GetInt32(0),
						GonderenAd = SModel.Reader.GetString(1),
						ePosta = SModel.Reader.GetString(2),
						KullaniciAdi = SModel.Reader.GetString(3),
						Sifre = SModel.Reader.GetString(4),
						GidenMailHost = SModel.Reader.GetString(5),
						GidenMailPort = SModel.Reader.GetInt32(6),
						GelenMailHost = SModel.Reader.GetString(7),
						GelenMailPort = SModel.Reader.GetInt32(8),
						SSL = SModel.Reader.GetBoolean(9),
						BCC = SModel.Reader.GetString(10),
						ReplyTo = SModel.Reader.GetString(11),
						GuncellenmeTarihi = SModel.Reader.GetDateTime(12),
						EklemeTarihi = SModel.Reader.GetDateTime(13),
					}
				};

			}
			catch (InvalidCastException ex)
			{
				SDataModel = new SurecVeriModel<MailAyarTablosuModel>{
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
				SDataModel = new SurecVeriModel<MailAyarTablosuModel>{
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

		public virtual SurecVeriModel<MailAyarTablosuModel> KayitBilgisiAl(int Baslangic, DbDataReader Reader)
		{
			try
			{
				SDataModel = new SurecVeriModel<MailAyarTablosuModel>{
					Sonuc = Sonuclar.Basarili,
					KullaniciMesaji = "Veri bilgisi başarıyla çekilmiştir.",
					Veriler = new MailAyarTablosuModel{
						MailAyarID = Reader.GetInt32(Baslangic + 0),
						GonderenAd = Reader.GetString(Baslangic + 1),
						ePosta = Reader.GetString(Baslangic + 2),
						KullaniciAdi = Reader.GetString(Baslangic + 3),
						Sifre = Reader.GetString(Baslangic + 4),
						GidenMailHost = Reader.GetString(Baslangic + 5),
						GidenMailPort = Reader.GetInt32(Baslangic + 6),
						GelenMailHost = Reader.GetString(Baslangic + 7),
						GelenMailPort = Reader.GetInt32(Baslangic + 8),
						SSL = Reader.GetBoolean(Baslangic + 9),
						BCC = Reader.GetString(Baslangic + 10),
						ReplyTo = Reader.GetString(Baslangic + 11),
						GuncellenmeTarihi = Reader.GetDateTime(Baslangic + 12),
						EklemeTarihi = Reader.GetDateTime(Baslangic + 13),
					}
				};
			}
			catch (InvalidCastException ex)
			{
				SDataModel = new SurecVeriModel<MailAyarTablosuModel>{
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
				SDataModel = new SurecVeriModel<MailAyarTablosuModel>{
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