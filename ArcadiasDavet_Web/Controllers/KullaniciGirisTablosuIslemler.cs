using System;
using System.Data.OleDb;
using VeritabaniIslemMerkeziBase;

namespace VeritabaniIslemMerkezi
{
    public partial class KullaniciGirisTablosuIslemler : KullaniciGirisTablosuIslemlerBase
    {
        public KullaniciGirisTablosuIslemler() : base() { }

        public KullaniciGirisTablosuIslemler(OleDbTransaction tran) : base(tran) { }

        public string YeniKullaniciGirisID()
        {
            string KullaniciGirisID;

            do
            {
                KullaniciGirisID = Guid.NewGuid().ToString();
                VTIslem.SetCommandText("SELECT COUNT(*) FROM KullaniciGirisTablosu WHERE KullaniciGirisID = @KullaniciGirisID");
                VTIslem.AddWithValue("KullaniciGirisID", KullaniciGirisID);
            } while (!Convert.ToInt32(VTIslem.ExecuteScalar()).Equals(0));

            return KullaniciGirisID;
        }
    }
}
