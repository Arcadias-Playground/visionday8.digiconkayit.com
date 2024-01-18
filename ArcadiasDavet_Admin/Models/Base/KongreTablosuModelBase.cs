using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace ModelBase
{
	[Table("KongreTablosu")]
	public abstract class KongreTablosuModelBase
	{
		[Key]
		[Required(ErrorMessage = "BosUyari")]
		[Column("KongreID", Order = 0)]
		public virtual int KongreID { get; set; }

		[Required(ErrorMessage = "BosUyari")]
		[MaxLength(255, ErrorMessage = "UzunlukUyari")]
		[Column("Kongre", Order = 1)]
		public virtual string Kongre { get; set; }

		[Required(ErrorMessage = "BosUyari")]
		[MaxLength(255, ErrorMessage = "UzunlukUyari")]
		[Column("WebUrl", Order = 2)]
		public virtual string WebUrl { get; set; }

		[DataType(DataType.DateTime, ErrorMessage = "GecersizUyari")]
		[Column("KapanisTarihi", Order = 3)]
		public virtual DateTime? KapanisTarihi { get; set; }

		[Required(ErrorMessage = "BosUyari")]
		[Column("KapanisHatirlatma", Order = 4)]
		public virtual bool KapanisHatirlatma { get; set; }

		[Required(ErrorMessage = "BosUyari")]
		[DataType(DataType.DateTime, ErrorMessage = "GecersizUyari")]
		[Column("GuncellenmeTarihi", Order = 5)]
		public virtual DateTime GuncellenmeTarihi { get; set; }

		[Required(ErrorMessage = "BosUyari")]
		[DataType(DataType.DateTime, ErrorMessage = "GecersizUyari")]
		[Column("EklenmeTarihi", Order = 6)]
		public virtual DateTime EklenmeTarihi { get; set; }


		public static int OzellikSayisi { get{ return typeof(KongreTablosuModelBase).GetProperties().Count(x => !x.GetAccessors()[0].IsStatic);}}

		public virtual string BaseJsonModel()
		{
			return JsonConvert.SerializeObject(this);
		}

	}
}