using System.Data.OleDb;
using VeritabaniIslemMerkeziBase;

namespace VeritabaniIslemMerkezi
{
    public partial class GonderimTipiTablosuIslemler : GonderimTipiTablosuIslemlerBase
    {
        public GonderimTipiTablosuIslemler() : base() { }

        public GonderimTipiTablosuIslemler(OleDbTransaction tran) : base(tran) { }
    }
}
