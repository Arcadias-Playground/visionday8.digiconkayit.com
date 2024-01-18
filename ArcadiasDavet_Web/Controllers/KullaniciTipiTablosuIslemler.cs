using System.Data.OleDb;
using VeritabaniIslemMerkeziBase;

namespace VeritabaniIslemMerkezi
{
    public partial class KullaniciTipiTablosuIslemler : KullaniciTipiTablosuIslemlerBase
    {
        public KullaniciTipiTablosuIslemler() : base() { }

        public KullaniciTipiTablosuIslemler(OleDbTransaction tran) : base(tran) { }
    }
}
