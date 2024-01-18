using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using ModelBase;
using Model;

namespace ModelRelation
{
	public abstract class KatilimciTipiTablosuModelRelation : KatilimciTipiTablosuModelBase
	{
		public virtual IList<KatilimciTablosuModel> KatilimciBilgisi { get; set; }
		public virtual IList<MailIcerikTablosuModel> MailIcerikBilgisi { get; set; }
		public virtual IList<SmsIcerikTablosuModel> SmsIcerikBilgisi { get; set; }
		public virtual YakaKartiCerceveTablosuModel YakaKartiCerceveBilgisi { get; set; }

		public virtual string RelationJsonModel()
		{
			return JsonConvert.SerializeObject(this);
		}
	}
}