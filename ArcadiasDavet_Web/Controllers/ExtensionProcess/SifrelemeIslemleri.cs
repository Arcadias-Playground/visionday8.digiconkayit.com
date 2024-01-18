using Newtonsoft.Json;
using System;
using System.Text;
using System.Web.Security;

namespace VeritabaniIslemMerkezi
{
    public class SifrelemeIslemleri
    {
        Encoding Encode = Encoding.UTF8;

        public string Sifrele(string Veri, string[] Anahtar)
        {
            return Convert.ToBase64String(MachineKey.Protect(Encode.GetBytes(Veri), Anahtar));
        }

        public string Sifrele<T>(T Veri, string[] Anahtar)
        {
            return Convert.ToBase64String(MachineKey.Protect(Encode.GetBytes(JsonConvert.SerializeObject(Veri)), Anahtar));
        }

        /// <summary>
        /// Gönderilen şifreyi string olarak döndürür;
        /// </summary>
        /// <param name="Veri">Şifrelenmiş veri</param>
        /// <param name="Anahtar">Şifreyi çözecek anahtar</param>
        /// <returns>string</returns>
        public string SifreCoz(string Veri, string[] Anahtar)
        {
            return Encode.GetString(MachineKey.Unprotect(Convert.FromBase64String(Veri), Anahtar));
        }


        /// <summary>
        /// Gönderilen şifreyi belirlenen sınıfa döndürür.
        /// </summary>
        /// <typeparam name="T">Döndürelcek sınıf/model</typeparam>
        /// <param name="Veri">Şifrelenmiş veri</param>
        /// <param name="Anahtar">Şifreyi çözecek anahtar</param>
        /// <returns>Belirtilen sınıf/model</returns>
        public T SifreCoz<T>(string Veri, string[] Anahtar)
        {
            return JsonConvert.DeserializeObject<T>(Encode.GetString(MachineKey.Unprotect(Convert.FromBase64String(Veri), Anahtar)));
        }
    }
}