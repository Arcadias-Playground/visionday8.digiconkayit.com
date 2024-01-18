using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using ModelBase;
using Model;

namespace ModelRelation
{
	public abstract class SmsIcerikTablosuModelRelation : SmsIcerikTablosuModelBase
	{
		public virtual IList<SmsGonderimTablosuModel> SmsGonderimBilgisi { get; set; }
		public virtual KatilimciTipiTablosuModel KatilimciTipiBilgisi { get; set; }
		public virtual GonderimTipiTablosuModel GonderimTipiBilgisi { get; set; }

		public virtual string RelationJsonModel()
		{
			return JsonConvert.SerializeObject(this);
		}
	}
}