using System.Data.OleDb;
using VeritabaniIslemMerkeziBase;

namespace VeritabaniIslemMerkezi
{
    public partial class YakaKartiIcerikTipiTablosuIslemler : YakaKartiIcerikTipiTablosuIslemlerBase
    {
        public YakaKartiIcerikTipiTablosuIslemler() : base() { }

        public YakaKartiIcerikTipiTablosuIslemler(OleDbTransaction tran) : base(tran) { }
    }
}
