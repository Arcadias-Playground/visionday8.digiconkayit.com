using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace ModelBase
{
	[Table("MailIcerikTablosu")]
	public abstract class MailIcerikTablosuModelBase
	{
		[Key]
		[Required(ErrorMessage = "BosUyari")]
		[Column("MailIcerikID", Order = 0)]
		public virtual int MailIcerikID { get; set; }

		[Required(ErrorMessage = "BosUyari")]
		[ForeignKey("GonderimTipiTablosu")]
		[Column("GonderimTipiID", Order = 1)]
		public virtual int GonderimTipiID { get; set; }

		[Required(ErrorMessage = "BosUyari")]
		[MaxLength(36, ErrorMessage = "UzunlukUyari")]
		[ForeignKey("KatilimciTipiTablosu")]
		[Column("KatilimciTipiID", Order = 2)]
		public virtual string KatilimciTipiID { get; set; }

		[Required(ErrorMessage = "BosUyari")]
		[MaxLength(255, ErrorMessage = "UzunlukUyari")]
		[Column("Konu", Order = 3)]
		public virtual string Konu { get; set; }

		[Required(ErrorMessage = "BosUyari")]
		[MaxLength(536870910, ErrorMessage = "UzunlukUyari")]
		[Column("HtmlIcerik", Order = 4)]
		public virtual string HtmlIcerik { get; set; }

		[Required(ErrorMessage = "BosUyari")]
		[Column("AnaKatilimci", Order = 5)]
		public virtual bool AnaKatilimci { get; set; }

		[Required(ErrorMessage = "BosUyari")]
		[DataType(DataType.DateTime, ErrorMessage = "GecersizUyari")]
		[Column("GuncellenmeTarihi", Order = 6)]
		public virtual DateTime GuncellenmeTarihi { get; set; }

		[Required(ErrorMessage = "BosUyari")]
		[DataType(DataType.DateTime, ErrorMessage = "GecersizUyari")]
		[Column("EklenmeTarihi", Order = 7)]
		public virtual DateTime EklenmeTarihi { get; set; }


		public static int OzellikSayisi { get { return typeof(MailIcerikTablosuModelBase).GetProperties().Count(x => !x.GetAccessors()[0].IsStatic); }}

		public static string SQLSutunSorgusu { get { return string.Join(", ", typeof(MailIcerikTablosuModelBase).GetProperties().Where(x => !x.GetAccessors()[0].IsStatic).OrderBy(x => (x.GetCustomAttributes(typeof(ColumnAttribute), true).First() as ColumnAttribute).Order).Select(x => $"[MailIcerikTablosu].[{x.Name}]")); }}

		public virtual string BaseJsonModel()
		{
			return JsonConvert.SerializeObject(this);
		}

	}
}