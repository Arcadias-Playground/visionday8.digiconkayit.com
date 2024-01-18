using Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.OleDb;
using VeritabaniIslemSaglayici.Access;

namespace VeritabaniIslemMerkeziBase
{
	public abstract class KongreTablosuIslemlerBase
	{
		public VTOperatorleri VTIslem;

		public List<KongreTablosuModel> VeriListe;

		public SurecBilgiModel SModel;
		public SurecVeriModel<KongreTablosuModel> SDataModel;
		public SurecVeriModel<IList<KongreTablosuModel>> SDataListModel;

		public KongreTablosuIslemlerBase()
		{
			VTIslem = new VTOperatorleri();
		}

		public KongreTablosuIslemlerBase(OleDbTransaction Transcation)
		{
			VTIslem = new VTOperatorleri(Transcation);
		}

		public virtual SurecBilgiModel YeniKayitEkle(KongreTablosuModel YeniKayit)
		{
			VTIslem.SetCommandText("INSERT INTO KongreTablosu (Kongre, WebUrl, KapanisTarihi, KapanisHatirlatma, GuncellenmeTarihi, EklenmeTarihi) VALUES (@Kongre, @WebUrl, @KapanisTarihi, @KapanisHatirlatma, @GuncellenmeTarihi, @EklenmeTarihi)");
			VTIslem.AddWithValue("Kongre", YeniKayit.Kongre);
			VTIslem.AddWithValue("WebUrl", YeniKayit.WebUrl);

			if(YeniKayit.KapanisTarihi is null)
				VTIslem.AddWithValue("KapanisTarihi", DBNull.Value);
			else
				VTIslem.AddWithValue("KapanisTarihi", YeniKayit.KapanisTarihi);

			VTIslem.AddWithValue("KapanisHatirlatma", YeniKayit.KapanisHatirlatma);
			VTIslem.AddWithValue("GuncellenmeTarihi", YeniKayit.GuncellenmeTarihi);
			VTIslem.AddWithValue("EklenmeTarihi", YeniKayit.EklenmeTarihi);
			return VTIslem.ExecuteNonQuery();
		}

		public virtual SurecBilgiModel KayitGuncelle(KongreTablosuModel GuncelKayit)
		{
			VTIslem.SetCommandText("UPDATE KongreTablosu SET Kongre=@Kongre, WebUrl=@WebUrl, KapanisTarihi=@KapanisTarihi, KapanisHatirlatma=@KapanisHatirlatma, GuncellenmeTarihi=@GuncellenmeTarihi, EklenmeTarihi=@EklenmeTarihi WHERE KongreID=@KongreID");
			VTIslem.AddWithValue("Kongre", GuncelKayit.Kongre);
			VTIslem.AddWithValue("WebUrl", GuncelKayit.WebUrl);

			if(GuncelKayit.KapanisTarihi is null)
				VTIslem.AddWithValue("KapanisTarihi", DBNull.Value);
			else
				VTIslem.AddWithValue("KapanisTarihi", GuncelKayit.KapanisTarihi.Value);

			VTIslem.AddWithValue("KapanisHatirlatma", GuncelKayit.KapanisHatirlatma);
			VTIslem.AddWithValue("GuncellenmeTarihi", GuncelKayit.GuncellenmeTarihi);
			VTIslem.AddWithValue("EklenmeTarihi", GuncelKayit.EklenmeTarihi);
			VTIslem.AddWithValue("KongreID", GuncelKayit.KongreID);
			return VTIslem.ExecuteNonQuery();
		}

		public virtual SurecBilgiModel KayitSil(int KongreID)
		{
			VTIslem.SetCommandText("DELETE FROM KongreTablosu WHERE KongreID=@KongreID");
			VTIslem.AddWithValue("KongreID", KongreID);
			return VTIslem.ExecuteNonQuery();
		}

		public virtual SurecVeriModel<KongreTablosuModel> KayitBilgisi(int KongreID)
		{
			VTIslem.SetCommandText("SELECT * FROM KongreTablosu WHERE KongreID = @KongreID");
			VTIslem.AddWithValue("KongreID", KongreID);
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
					SDataModel = new SurecVeriModel<KongreTablosuModel>
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
				SDataModel = new SurecVeriModel<KongreTablosuModel>
				{
					Sonuc = SModel.Sonuc,
					KullaniciMesaji = SModel.KullaniciMesaji,
					HataBilgi = SModel.HataBilgi
				};
			}
			VTIslem.CloseConnection();
			return SDataModel;
		}

		public virtual SurecVeriModel<IList<KongreTablosuModel>> KayitBilgileri()
		{
			VTIslem.SetCommandText("SELECT * FROM KongreTablosu");
			VTIslem.OpenConnection();
			SModel = VTIslem.ExecuteReader(CommandBehavior.Default);
			if (SModel.Sonuc.Equals(Sonuclar.Basarili))
			{
				VeriListe = new List<KongreTablosuModel>();
				while (SModel.Reader.Read())
				{
					if (KayitBilgisiAl().Sonuc.Equals(Sonuclar.Basarili))
					{
						VeriListe.Add(SDataModel.Veriler);
					}
					else
					{
						SDataListModel = new SurecVeriModel<IList<KongreTablosuModel>>{
							Sonuc = SDataModel.Sonuc,
							KullaniciMesaji = SDataModel.KullaniciMesaji,
							HataBilgi = SDataModel.HataBilgi
						};
						VTIslem.CloseConnection();
						return SDataListModel;
					}
				}
				SDataListModel = new SurecVeriModel<IList<KongreTablosuModel>>{
					Sonuc = Sonuclar.Basarili,
					KullaniciMesaji = "Veri listesi başarıyla çekildi",
					Veriler = VeriListe
				};
			}
			else
			{
				SDataListModel = new SurecVeriModel<IList<KongreTablosuModel>>{
					Sonuc = SModel.Sonuc,
					KullaniciMesaji = SModel.KullaniciMesaji,
					HataBilgi = SModel.HataBilgi
				};
			}
			VTIslem.CloseConnection();
			return SDataListModel;
		}

		SurecVeriModel<KongreTablosuModel> KayitBilgisiAl()
		{
			try
			{
				SDataModel = new SurecVeriModel<KongreTablosuModel>{
					Sonuc = Sonuclar.Basarili,
					KullaniciMesaji = "Veri bilgisi başarıyla çekilmiştir.",
					Veriler = new KongreTablosuModel
					{
						KongreID = SModel.Reader.GetInt32(0),
						Kongre = SModel.Reader.GetString(1),
						WebUrl = SModel.Reader.GetString(2),
						KapanisTarihi = SModel.Reader.IsDBNull(3) ? null : new DateTime?(SModel.Reader.GetDateTime(3)),
						KapanisHatirlatma = SModel.Reader.GetBoolean(4),
						GuncellenmeTarihi = SModel.Reader.GetDateTime(5),
						EklenmeTarihi = SModel.Reader.GetDateTime(6),
					}
				};

			}
			catch (InvalidCastException ex)
			{
				SDataModel = new SurecVeriModel<KongreTablosuModel>{
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
				SDataModel = new SurecVeriModel<KongreTablosuModel>{
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

		public virtual SurecVeriModel<KongreTablosuModel> KayitBilgisiAl(int Baslangic, DbDataReader Reader)
		{
			try
			{
				SDataModel = new SurecVeriModel<KongreTablosuModel>{
					Sonuc = Sonuclar.Basarili,
					KullaniciMesaji = "Veri bilgisi başarıyla çekilmiştir.",
					Veriler = new KongreTablosuModel{
						KongreID = Reader.GetInt32(Baslangic + 0),
						Kongre = Reader.GetString(Baslangic + 1),
						WebUrl = Reader.GetString(Baslangic + 2),
						KapanisTarihi = Reader.IsDBNull(Baslangic + 3) ? null : new DateTime?(Reader.GetDateTime(Baslangic + 3)),
						KapanisHatirlatma = Reader.GetBoolean(Baslangic + 4),
						GuncellenmeTarihi = Reader.GetDateTime(Baslangic + 5),
						EklenmeTarihi = Reader.GetDateTime(Baslangic + 6),
					}
				};
			}
			catch (InvalidCastException ex)
			{
				SDataModel = new SurecVeriModel<KongreTablosuModel>{
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
				SDataModel = new SurecVeriModel<KongreTablosuModel>{
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