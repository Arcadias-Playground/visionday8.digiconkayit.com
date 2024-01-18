using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace ModelBase
{
	[Table("AntetliKagitIcerikTablosu")]
	public abstract class AntetliKagitIcerikTablosuModelBase
	{
		[Key]
		[Required(ErrorMessage = "BosUyari")]
		[Column("AntetliKagitIcerikID", Order = 0)]
		public virtual int AntetliKagitIcerikID { get; set; }

		[Required(ErrorMessage = "BosUyari")]
		[ForeignKey("AntetliKagitIcerikTipiTablosu")]
		[Column("AntetliKagitIcerikTipiID", Order = 1)]
		public virtual int AntetliKagitIcerikTipiID { get; set; }

		[Required(ErrorMessage = "BosUyari")]
		[ForeignKey("MailIcerikTablosu")]
		[Column("MailIcerikID", Order = 2)]
		public virtual int MailIcerikID { get; set; }

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


		public static int OzellikSayisi { get { return typeof(AntetliKagitIcerikTablosuModelBase).GetProperties().Count(x => !x.GetAccessors()[0].IsStatic); }}

		public static string SQLSutunSorgusu { get { return string.Join(", ", typeof(AntetliKagitIcerikTablosuModelBase).GetProperties().Where(x => !x.GetAccessors()[0].IsStatic).OrderBy(x => (x.GetCustomAttributes(typeof(ColumnAttribute), true).First() as ColumnAttribute).Order).Select(x => $"[AntetliKagitIcerikTablosu].[{x.Name}]")); }}

		public virtual string BaseJsonModel()
		{
			return JsonConvert.SerializeObject(this);
		}

	}
}