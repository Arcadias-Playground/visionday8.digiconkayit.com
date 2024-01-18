using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace ModelBase
{
	[Table("SmsIcerikTablosu")]
	public abstract class SmsIcerikTablosuModelBase
	{
		[Key]
		[Required(ErrorMessage = "BosUyari")]
		[Column("SmsIcerikID", Order = 0)]
		public virtual int SmsIcerikID { get; set; }

		[Required(ErrorMessage = "BosUyari")]
		[MaxLength(36, ErrorMessage = "UzunlukUyari")]
		[ForeignKey("KatilimciTipiTablosu")]
		[Column("KatilimciTipiID", Order = 1)]
		public virtual string KatilimciTipiID { get; set; }

		[Required(ErrorMessage = "BosUyari")]
		[ForeignKey("GonderimTipiTablosu")]
		[Column("GonderimTipiID", Order = 2)]
		public virtual int GonderimTipiID { get; set; }

		[Required(ErrorMessage = "BosUyari")]
		[MaxLength(536870910, ErrorMessage = "UzunlukUyari")]
		[Column("SmsIcerik", Order = 3)]
		public virtual string SmsIcerik { get; set; }

		[Required(ErrorMessage = "BosUyari")]
		[Column("AnaKatilimci", Order = 4)]
		public virtual bool AnaKatilimci { get; set; }

		[Required(ErrorMessage = "BosUyari")]
		[DataType(DataType.DateTime, ErrorMessage = "GecersizUyari")]
		[Column("GuncellenmeTarihi", Order = 5)]
		public virtual DateTime GuncellenmeTarihi { get; set; }

		[Required(ErrorMessage = "BosUyari")]
		[DataType(DataType.DateTime, ErrorMessage = "GecersizUyari")]
		[Column("EklenmeTarihi", Order = 6)]
		public virtual DateTime EklenmeTarihi { get; set; }


		public static int OzellikSayisi { get { return typeof(SmsIcerikTablosuModelBase).GetProperties().Count(x => !x.GetAccessors()[0].IsStatic); }}

		public static string SQLSutunSorgusu { get { return string.Join(", ", typeof(SmsIcerikTablosuModelBase).GetProperties().Where(x => !x.GetAccessors()[0].IsStatic).OrderBy(x => (x.GetCustomAttributes(typeof(ColumnAttribute), true).First() as ColumnAttribute).Order).Select(x => $"[SmsIcerikTablosu].[{x.Name}]")); }}

		public virtual string BaseJsonModel()
		{
			return JsonConvert.SerializeObject(this);
		}

	}
}