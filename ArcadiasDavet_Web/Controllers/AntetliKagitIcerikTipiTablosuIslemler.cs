using System.Data.OleDb;
using VeritabaniIslemMerkeziBase;

namespace VeritabaniIslemMerkezi
{
    public partial class AntetliKagitIcerikTipiTablosuIslemler : AntetliKagitIcerikTipiTablosuIslemlerBase
    {
        public AntetliKagitIcerikTipiTablosuIslemler() : base() { }

        public AntetliKagitIcerikTipiTablosuIslemler(OleDbTransaction tran) : base(tran) { }
    }
}
