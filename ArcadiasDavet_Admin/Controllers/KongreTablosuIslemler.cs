using Model;
using System;
using System.Collections.Generic;
using System.Data.OleDb;
using VeritabaniIslemMerkeziBase;

namespace VeritabaniIslemMerkezi
{
	public partial class KongreTablosuIslemler : KongreTablosuIslemlerBase
	{
		public KongreTablosuIslemler() : base() { }

		public KongreTablosuIslemler(OleDbTransaction tran) : base (tran) { }
	}
}
