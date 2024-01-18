using Model;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using VeritabaniIslemMerkezi;

namespace ArcadiasDavet_Web.Controllers.Api
{
    public class KatilimciController : ApiController
    {
        SurecBilgiModel SModel;

        // GET api/<controller>
        public HttpResponseMessage Get()
        {
            return Request.CreateResponse(HttpStatusCode.OK, new KatilimciTipiTablosuIslemler().KayitBilgileri());
        }

        // GET api/<controller>/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<controller>
        public HttpResponseMessage Post([FromBody] KatilimciTablosuModel KModel)
        {
            KModel.KatilimciID = new KatilimciTablosuIslemler().YeniKatilimciID();

            if (new KatilimciTablosuIslemler().KontenjanKontrol(KModel.KatilimciTipiID))
            {
                SModel = new KatilimciTablosuIslemler().YeniKayitEkle(KModel);
                if (SModel.Sonuc.Equals(Sonuclar.Basarili))
                {
                    if (!string.IsNullOrEmpty(KModel.ePosta) && KModel.ePostaGonderimIstek)
                        new MailGonderimIslemleri().MailGonderim(KModel.KatilimciOnayTarihi.HasValue ? new KatilimciTablosuIslemler().KayitBilgisi(KModel.KatilimciID, "email", 2).Veriler : new KatilimciTablosuIslemler().KayitBilgisi(KModel.KatilimciID, "email", 1).Veriler);

                    if (!string.IsNullOrEmpty(KModel.Telefon) && KModel.SmsGonderimIstek)
                        new SmsGonderimIslemleri().SmsGonderim(KModel.KatilimciOnayTarihi.HasValue ? new KatilimciTablosuIslemler().KayitBilgisi(KModel.KatilimciID, "sms", 2).Veriler : new KatilimciTablosuIslemler().KayitBilgisi(KModel.KatilimciID, "sms", 1).Veriler);
                }
            }
            else
            {

                SModel = new SurecBilgiModel
                {
                    Sonuc = Sonuclar.Basarisiz,
                    KullaniciMesaji = "Kontenjan dolu",
                    HataBilgi = new HataBilgileri
                    {
                        HataAlinanKayitID = 0,
                        HataKodu = 0,
                        HataMesaji = "Katılımcı tipine ait kontenjan dolmuştur."
                    }
                };
            }

            return Request.CreateResponse(HttpStatusCode.OK, SModel);
        }

        // PUT api/<controller>/5
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }
    }
}