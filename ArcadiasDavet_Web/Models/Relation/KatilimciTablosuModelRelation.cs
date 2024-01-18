using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using ModelBase;
using Model;

namespace ModelRelation
{
	public abstract class KatilimciTablosuModelRelation : KatilimciTablosuModelBase
	{
		public virtual IList<KatilimciGirisTablosuModel> KatilimciGirisBilgisi { get; set; }
		public virtual IList<KatilimciTablosuModel> KatilimciBilgisi { get; set; }
		public virtual IList<MailGonderimTablosuModel> MailGonderimBilgisi { get; set; }
		public virtual IList<SmsGonderimTablosuModel> SmsGonderimBilgisi { get; set; }
		public virtual IList<YakaKartiBasimTablosuModel> YakaKartiBasimBilgisi { get; set; }
		public virtual KatilimciTipiTablosuModel KatilimciTipiBilgisi { get; set; }

		public virtual string RelationJsonModel()
		{
			return JsonConvert.SerializeObject(this);
		}
	}
}