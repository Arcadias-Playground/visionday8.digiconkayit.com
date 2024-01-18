using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace ModelBase
{
	[Table("MailAyarTablosu")]
	public abstract class MailAyarTablosuModelBase
	{
		[Key]
		[Required(ErrorMessage = "BosUyari")]
		[Column("MailAyarID", Order = 0)]
		public virtual int MailAyarID { get; set; }

		[Required(ErrorMessage = "BosUyari")]
		[MaxLength(255, ErrorMessage = "UzunlukUyari")]
		[Column("GonderenAd", Order = 1)]
		public virtual string GonderenAd { get; set; }

		[Required(ErrorMessage = "BosUyari")]
		[EmailAddress(ErrorMessage = "GecersizUyari")]
		[MaxLength(255, ErrorMessage = "UzunlukUyari")]
		[Column("ePosta", Order = 2)]
		public virtual string ePosta { get; set; }

		[Required(ErrorMessage = "BosUyari")]
		[MaxLength(255, ErrorMessage = "UzunlukUyari")]
		[Column("KullaniciAdi", Order = 3)]
		public virtual string KullaniciAdi { get; set; }

		[Required(ErrorMessage = "BosUyari")]
		[MaxLength(255, ErrorMessage = "UzunlukUyari")]
		[Column("Sifre", Order = 4)]
		public virtual string Sifre { get; set; }

		[Required(ErrorMessage = "BosUyari")]
		[MaxLength(255, ErrorMessage = "UzunlukUyari")]
		[Column("GidenMailHost", Order = 5)]
		public virtual string GidenMailHost { get; set; }

		[Required(ErrorMessage = "BosUyari")]
		[Column("GidenMailPort", Order = 6)]
		public virtual int GidenMailPort { get; set; }

		[Required(ErrorMessage = "BosUyari")]
		[MaxLength(255, ErrorMessage = "UzunlukUyari")]
		[Column("GelenMailHost", Order = 7)]
		public virtual string GelenMailHost { get; set; }

		[Required(ErrorMessage = "BosUyari")]
		[Column("GelenMailPort", Order = 8)]
		public virtual int GelenMailPort { get; set; }

		[Required(ErrorMessage = "BosUyari")]
		[Column("SSL", Order = 9)]
		public virtual bool SSL { get; set; }

		[Required(ErrorMessage = "BosUyari")]
		[MaxLength(255, ErrorMessage = "UzunlukUyari")]
		[Column("BCC", Order = 10)]
		public virtual string BCC { get; set; }

		[Required(ErrorMessage = "BosUyari")]
		[MaxLength(255, ErrorMessage = "UzunlukUyari")]
		[Column("ReplyTo", Order = 11)]
		public virtual string ReplyTo { get; set; }

		[Required(ErrorMessage = "BosUyari")]
		[DataType(DataType.DateTime, ErrorMessage = "GecersizUyari")]
		[Column("GuncellenmeTarihi", Order = 12)]
		public virtual DateTime GuncellenmeTarihi { get; set; }

		[Required(ErrorMessage = "BosUyari")]
		[DataType(DataType.DateTime, ErrorMessage = "GecersizUyari")]
		[Column("EklemeTarihi", Order = 13)]
		public virtual DateTime EklemeTarihi { get; set; }


		public static int OzellikSayisi { get { return typeof(MailAyarTablosuModelBase).GetProperties().Count(x => !x.GetAccessors()[0].IsStatic); }}

		public static string SQLSutunSorgusu { get { return string.Join(", ", typeof(MailAyarTablosuModelBase).GetProperties().Where(x => !x.GetAccessors()[0].IsStatic).OrderBy(x => (x.GetCustomAttributes(typeof(ColumnAttribute), true).First() as ColumnAttribute).Order).Select(x => $"[MailAyarTablosu].[{x.Name}]")); }}

		public virtual string BaseJsonModel()
		{
			return JsonConvert.SerializeObject(this);
		}

	}
}