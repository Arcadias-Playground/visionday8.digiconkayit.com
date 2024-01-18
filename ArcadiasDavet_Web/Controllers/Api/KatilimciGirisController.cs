using Model;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using VeritabaniIslemMerkezi;

namespace ArcadiasDavet_Web.Controllers.Api
{
    public class KatilimciGirisController : ApiController
    {
        SurecBilgiModel SModel;

        SurecVeriModel<KatilimciTablosuModel> SDataModel;
        SurecVeriModel<IList<KatilimciGirisTablosuModel>> SDataListModel;

        // GET api/<controller>
        public HttpResponseMessage Get()
        {
            return Request.CreateResponse(HttpStatusCode.Unused);
        }

        // GET api/<controller>/5
        public HttpResponseMessage Get(string UUID)
        {
            SDataListModel = new KatilimciGirisTablosuIslemler().KayitBilgileri(UUID);
            return Request.CreateResponse(HttpStatusCode.OK, SDataListModel);
        }

        // POST api/<controller>
        public HttpResponseMessage Post([FromBody] KatilimciGirisTablosuModel KGModel)
        {
            SDataModel = new KatilimciTablosuIslemler().AppGirisKontrol(KGModel.KatilimciID);
            switch (SDataModel.Sonuc)
            {
                case Sonuclar.KismiBasarili:
                case Sonuclar.Basarisiz:
                    return Request.CreateResponse(HttpStatusCode.OK, SDataModel);

                default:
                case Sonuclar.VeriBulunamadi:
                    SDataModel.HataBilgi.HataMesaji = "Katılımcı kaydı bulunamadı";
                    return Request.CreateResponse(HttpStatusCode.OK, SDataModel);


                case Sonuclar.Basarili:
                    if (SDataModel.Veriler.YoneticiOnay)
                    {
                        if (SDataModel.Veriler.KatilimciOnay)
                        {
                            if (SDataModel.Veriler.KatilimciGirisBilgisi is null || SDataModel.Veriler.KatilimciGirisBilgisi.Count < SDataModel.Veriler.KatilimciTipiBilgisi.GirisSayisi)
                            {
                                KGModel.EklenmeTarihi = new BilgiKontrolMerkezi().Simdi();

                                SModel = new KatilimciGirisTablosuIslemler().YeniKayitEkle(KGModel);

                                if (SModel.Sonuc.Equals(Sonuclar.Basarili))
                                {
                                    KGModel.KatilimciGirisID = Convert.ToInt32(SModel.YeniKayitID);
                                    KGModel.KatilimciBilgisi = SDataModel.Veriler;

                                    return Request.CreateResponse(HttpStatusCode.OK, new SurecVeriModel<KatilimciGirisTablosuModel> { Sonuc = Sonuclar.Basarili, KullaniciMesaji = "Katılımcı girişi kaydedildi", Veriler = KGModel, HataBilgi = null });
                                }
                                else
                                {
                                    return Request.CreateResponse(HttpStatusCode.OK, SModel);
                                }
                            }
                            else
                            {
                                SDataModel.Sonuc = Sonuclar.Basarisiz;
                                SDataModel.HataBilgi = new HataBilgileri
                                {
                                    HataAlinanKayitID = SDataModel.Veriler.KatilimciID,
                                    HataKodu = 0,
                                    HataMesaji = "Katılımcı giriş kaydı mevcut olduğundan yeniden giriş yapamaz. Detaylı bilgi için katılımcıyı kayıt masasına yönlendirin"
                                };
                                return Request.CreateResponse(HttpStatusCode.OK, SDataModel);
                            }
                        }
                        else
                        {
                            SDataModel.Sonuc = Sonuclar.Basarisiz;
                            SDataModel.HataBilgi = new HataBilgileri
                            {
                                HataAlinanKayitID = SDataModel.Veriler.KatilimciID,
                                HataKodu = 0,
                                HataMesaji = "Katılımcı onayı olmadığından giriş kaydı yapılmadı. Detaylı bilgi için katılımcıyı kayıt masasına yönlendirin."
                            };
                            return Request.CreateResponse(HttpStatusCode.OK, SDataModel);
                        }
                    }
                    else
                    {
                        SDataModel.Sonuc = Sonuclar.Basarisiz;
                        SDataModel.HataBilgi = new HataBilgileri
                        {
                            HataAlinanKayitID = SDataModel.Veriler.KatilimciID,
                            HataKodu = 0,
                            HataMesaji = "Yönetici onayı olmadığından giriş kaydı yapılmadı. Detaylı bilgi için katılımcıyı kayıt masasına yönlendirin.d"
                        };
                        return Request.CreateResponse(HttpStatusCode.OK, SDataModel);
                    }
            }
        }

        // PUT api/<controller>/5
        public HttpResponseMessage Put(int id, [FromBody] string value)
        {
            return Request.CreateResponse(HttpStatusCode.Unused);
        }

        // DELETE api/<controller>/5
        public HttpResponseMessage Delete(int id)
        {
            return Request.CreateResponse(HttpStatusCode.Unused);
        }
    }
}