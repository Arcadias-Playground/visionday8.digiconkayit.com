using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace ModelBase
{
	[Table("KatilimciTipiTablosu")]
	public abstract class KatilimciTipiTablosuModelBase
	{
		[Key]
		[Required(ErrorMessage = "BosUyari")]
		[MaxLength(36, ErrorMessage = "UzunlukUyari")]
		[Column("KatilimciTipiID", Order = 0)]
		public virtual string KatilimciTipiID { get; set; }

		[Required(ErrorMessage = "BosUyari")]
		[MaxLength(255, ErrorMessage = "UzunlukUyari")]
		[Column("KatilimciTipi", Order = 1)]
		public virtual string KatilimciTipi { get; set; }

		[Required(ErrorMessage = "BosUyari")]
		[Column("Kontenjan", Order = 2)]
		public virtual int Kontenjan { get; set; }

		[Required(ErrorMessage = "BosUyari")]
		[Column("MisafirKontenjan", Order = 3)]
		public virtual int MisafirKontenjan { get; set; }

		[Required(ErrorMessage = "BosUyari")]
		[Column("GirisSayisi", Order = 4)]
		public virtual int GirisSayisi { get; set; }

		[Required(ErrorMessage = "BosUyari")]
		[Column("YakaKartiBasimSayisi", Order = 5)]
		public virtual int YakaKartiBasimSayisi { get; set; }

		[Required(ErrorMessage = "BosUyari")]
		[MaxLength(536870910, ErrorMessage = "UzunlukUyari")]
		[Column("KabulEkranIcerik", Order = 6)]
		public virtual string KabulEkranIcerik { get; set; }

		[Required(ErrorMessage = "BosUyari")]
		[MaxLength(536870910, ErrorMessage = "UzunlukUyari")]
		[Column("RedEkranIcerik", Order = 7)]
		public virtual string RedEkranIcerik { get; set; }

		[Required(ErrorMessage = "BosUyari")]
		[DataType(DataType.DateTime, ErrorMessage = "GecersizUyari")]
		[Column("GuncellenmeTarihi", Order = 8)]
		public virtual DateTime GuncellenmeTarihi { get; set; }

		[Required(ErrorMessage = "BosUyari")]
		[DataType(DataType.DateTime, ErrorMessage = "GecersizUyari")]
		[Column("EklenmeTarihi", Order = 9)]
		public virtual DateTime EklenmeTarihi { get; set; }


		public static int OzellikSayisi { get { return typeof(KatilimciTipiTablosuModelBase).GetProperties().Count(x => !x.GetAccessors()[0].IsStatic); }}

		public static string SQLSutunSorgusu { get { return string.Join(", ", typeof(KatilimciTipiTablosuModelBase).GetProperties().Where(x => !x.GetAccessors()[0].IsStatic).OrderBy(x => (x.GetCustomAttributes(typeof(ColumnAttribute), true).First() as ColumnAttribute).Order).Select(x => $"[KatilimciTipiTablosu].[{x.Name}]")); }}

		public virtual string BaseJsonModel()
		{
			return JsonConvert.SerializeObject(this);
		}

	}
}