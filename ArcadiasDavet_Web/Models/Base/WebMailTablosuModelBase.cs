using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace ModelBase
{
	[Table("WebMailTablosu")]
	public abstract class WebMailTablosuModelBase
	{
		[Key]
		[Required(ErrorMessage = "BosUyari")]
		[MaxLength(255, ErrorMessage = "UzunlukUyari")]
		[Column("WebMailID", Order = 0)]
		public virtual string WebMailID { get; set; }

		[Required(ErrorMessage = "BosUyari")]
		[MaxLength(255, ErrorMessage = "UzunlukUyari")]
		[Column("GonderenMail", Order = 1)]
		public virtual string GonderenMail { get; set; }

		[Required(ErrorMessage = "BosUyari")]
		[MaxLength(255, ErrorMessage = "UzunlukUyari")]
		[Column("Konu", Order = 2)]
		public virtual string Konu { get; set; }

		[MaxLength(536870910, ErrorMessage = "UzunlukUyari")]
		[Column("TextBody", Order = 3)]
		public virtual string TextBody { get; set; }

		[MaxLength(536870910, ErrorMessage = "UzunlukUyari")]
		[Column("HtmlBody", Order = 4)]
		public virtual string HtmlBody { get; set; }

		[Required(ErrorMessage = "BosUyari")]
		[DataType(DataType.DateTime, ErrorMessage = "GecersizUyari")]
		[Column("WebMailTarih", Order = 5)]
		public virtual DateTime WebMailTarih { get; set; }

		[MaxLength(36, ErrorMessage = "UzunlukUyari")]
		[ForeignKey("KullaniciTablosu")]
		[Column("KullaniciID", Order = 6)]
		public virtual string KullaniciID { get; set; }

		[DataType(DataType.DateTime, ErrorMessage = "GecersizUyari")]
		[Column("MailGorulmeTarihi", Order = 7)]
		public virtual DateTime? MailGorulmeTarihi { get; set; }

		[Required(ErrorMessage = "BosUyari")]
		[DataType(DataType.DateTime, ErrorMessage = "GecersizUyari")]
		[Column("EklenmeTarihi", Order = 8)]
		public virtual DateTime EklenmeTarihi { get; set; }


		public static int OzellikSayisi { get { return typeof(WebMailTablosuModelBase).GetProperties().Count(x => !x.GetAccessors()[0].IsStatic); }}

		public static string SQLSutunSorgusu { get { return string.Join(", ", typeof(WebMailTablosuModelBase).GetProperties().Where(x => !x.GetAccessors()[0].IsStatic).OrderBy(x => (x.GetCustomAttributes(typeof(ColumnAttribute), true).First() as ColumnAttribute).Order).Select(x => $"[WebMailTablosu].[{x.Name}]")); }}

		public virtual string BaseJsonModel()
		{
			return JsonConvert.SerializeObject(this);
		}

	}
}