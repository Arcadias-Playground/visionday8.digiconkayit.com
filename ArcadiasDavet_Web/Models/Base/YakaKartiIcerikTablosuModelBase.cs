using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace ModelBase
{
	[Table("YakaKartiIcerikTablosu")]
	public abstract class YakaKartiIcerikTablosuModelBase
	{
		[Key]
		[Required(ErrorMessage = "BosUyari")]
		[Column("YakaKartiIcerikID", Order = 0)]
		public virtual int YakaKartiIcerikID { get; set; }

		[Required(ErrorMessage = "BosUyari")]
		[ForeignKey("YakaKartiCerceveTablosu")]
		[Column("YakaKartiCerceveID", Order = 1)]
		public virtual int YakaKartiCerceveID { get; set; }

		[Required(ErrorMessage = "BosUyari")]
		[ForeignKey("YakaKartiIcerikTipiTablosu")]
		[Column("YakaKartiIcerikTipiID", Order = 2)]
		public virtual int YakaKartiIcerikTipiID { get; set; }

		[Required(ErrorMessage = "BosUyari")]
		[Column("X", Order = 3)]
		public virtual int X { get; set; }

		[Required(ErrorMessage = "BosUyari")]
		[Column("Y", Order = 4)]
		public virtual int Y { get; set; }

		[Required(ErrorMessage = "BosUyari")]
		[Column("Width", Order = 5)]
		public virtual int Width { get; set; }

		[Required(ErrorMessage = "BosUyari")]
		[Column("Height", Order = 6)]
		public virtual int Height { get; set; }

		[Required(ErrorMessage = "BosUyari")]
		[DataType(DataType.DateTime, ErrorMessage = "GecersizUyari")]
		[Column("GuncellenmeTarihi", Order = 7)]
		public virtual DateTime GuncellenmeTarihi { get; set; }

		[Required(ErrorMessage = "BosUyari")]
		[DataType(DataType.DateTime, ErrorMessage = "GecersizUyari")]
		[Column("EklenmeTarihi", Order = 8)]
		public virtual DateTime EklenmeTarihi { get; set; }


		public static int OzellikSayisi { get { return typeof(YakaKartiIcerikTablosuModelBase).GetProperties().Count(x => !x.GetAccessors()[0].IsStatic); }}

		public static string SQLSutunSorgusu { get { return string.Join(", ", typeof(YakaKartiIcerikTablosuModelBase).GetProperties().Where(x => !x.GetAccessors()[0].IsStatic).OrderBy(x => (x.GetCustomAttributes(typeof(ColumnAttribute), true).First() as ColumnAttribute).Order).Select(x => $"[YakaKartiIcerikTablosu].[{x.Name}]")); }}

		public virtual string BaseJsonModel()
		{
			return JsonConvert.SerializeObject(this);
		}

	}
}