using Model;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using VeritabaniIslemMerkezi;

namespace ArcadiasDavet_Web.Controllers.Api
{
    public class MailGonderimController : ApiController
    {
        SurecVeriModel<KatilimciTablosuModel> SDataModel;

        // GET api/<controller>
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<controller>/5
        public HttpResponseMessage Get(string KatilimciID, bool ePostaGonderimIstek, bool SmsGonderimIstek)
        {
            SDataModel = new KatilimciTablosuIslemler().KayitBilgisi(KatilimciID);
            switch (SDataModel.Sonuc)
            {
                case Sonuclar.KismiBasarili:
                case Sonuclar.Basarisiz:
                    return Request.CreateResponse(HttpStatusCode.OK, new SurecBilgiModel() { Sonuc = SDataModel.Sonuc, KullaniciMesaji = SDataModel.KullaniciMesaji, HataBilgi = SDataModel.HataBilgi });

                default:
                case Sonuclar.VeriBulunamadi:
                    return Request.CreateResponse(HttpStatusCode.OK, new SurecBilgiModel() { Sonuc = SDataModel.Sonuc, KullaniciMesaji = SDataModel.KullaniciMesaji, HataBilgi = SDataModel.HataBilgi });


                case Sonuclar.Basarili:

                    if (!string.IsNullOrEmpty(SDataModel.Veriler.ePosta) && ePostaGonderimIstek)
                        new MailGonderimIslemleri().MailGonderim(new KatilimciTablosuIslemler().KayitBilgisi(SDataModel.Veriler.KatilimciID, "email", SDataModel.Veriler.KatilimciOnay ? 2 : 1).Veriler);

                    if (!string.IsNullOrEmpty(SDataModel.Veriler.Telefon) && SmsGonderimIstek)
                        new SmsGonderimIslemleri().SmsGonderim(new KatilimciTablosuIslemler().KayitBilgisi(SDataModel.Veriler.KatilimciID, "sms", SDataModel.Veriler.KatilimciOnay ? 2 : 1).Veriler);

                    return Request.CreateResponse(HttpStatusCode.OK, new SurecBilgiModel { Sonuc = Sonuclar.Basarili, KullaniciMesaji = "Kişiye iletişim kanalları ile hatırlatma içerikleri gönderildi" });
            }
        }

        // POST api/<controller>
        public void Post([FromBody] string value)
        {
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