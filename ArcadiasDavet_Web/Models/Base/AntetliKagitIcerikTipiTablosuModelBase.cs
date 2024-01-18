using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace ModelBase
{
	[Table("AntetliKagitIcerikTipiTablosu")]
	public abstract class AntetliKagitIcerikTipiTablosuModelBase
	{
		[Key]
		[Required(ErrorMessage = "BosUyari")]
		[Column("AntetliKagitIcerikTipiID", Order = 0)]
		public virtual int AntetliKagitIcerikTipiID { get; set; }

		[Required(ErrorMessage = "BosUyari")]
		[ForeignKey("GonderimTipiTablosu")]
		[Column("GonderimTipiID", Order = 1)]
		public virtual int GonderimTipiID { get; set; }

		[Required(ErrorMessage = "BosUyari")]
		[MaxLength(255, ErrorMessage = "UzunlukUyari")]
		[Column("AntetliKagitIcerikTipi", Order = 2)]
		public virtual string AntetliKagitIcerikTipi { get; set; }

		[Required(ErrorMessage = "BosUyari")]
		[Column("Oran", Order = 3)]
		public virtual decimal Oran { get; set; }

		[Required(ErrorMessage = "BosUyari")]
		[DataType(DataType.DateTime, ErrorMessage = "GecersizUyari")]
		[Column("GuncellenmeTarihi", Order = 4)]
		public virtual DateTime GuncellenmeTarihi { get; set; }

		[Required(ErrorMessage = "BosUyari")]
		[DataType(DataType.DateTime, ErrorMessage = "GecersizUyari")]
		[Column("EklenmeTarihi", Order = 5)]
		public virtual DateTime EklenmeTarihi { get; set; }


		public static int OzellikSayisi { get { return typeof(AntetliKagitIcerikTipiTablosuModelBase).GetProperties().Count(x => !x.GetAccessors()[0].IsStatic); }}

		public static string SQLSutunSorgusu { get { return string.Join(", ", typeof(AntetliKagitIcerikTipiTablosuModelBase).GetProperties().Where(x => !x.GetAccessors()[0].IsStatic).OrderBy(x => (x.GetCustomAttributes(typeof(ColumnAttribute), true).First() as ColumnAttribute).Order).Select(x => $"[AntetliKagitIcerikTipiTablosu].[{x.Name}]")); }}

		public virtual string BaseJsonModel()
		{
			return JsonConvert.SerializeObject(this);
		}

	}
}