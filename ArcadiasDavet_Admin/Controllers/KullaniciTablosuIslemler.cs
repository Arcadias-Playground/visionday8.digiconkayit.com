using Model;
using System;
using System.Collections.Generic;
using System.Data.OleDb;
using VeritabaniIslemMerkeziBase;

namespace VeritabaniIslemMerkezi
{
	public partial class KullaniciTablosuIslemler : KullaniciTablosuIslemlerBase
	{
		public KullaniciTablosuIslemler() : base() { }

		public KullaniciTablosuIslemler(OleDbTransaction tran) : base (tran) { }
	}
}
