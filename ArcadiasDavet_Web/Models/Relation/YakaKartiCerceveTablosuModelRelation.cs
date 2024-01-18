using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using ModelBase;
using Model;

namespace ModelRelation
{
	public abstract class YakaKartiCerceveTablosuModelRelation : YakaKartiCerceveTablosuModelBase
	{
		public virtual IList<YakaKartiIcerikTablosuModel> YakaKartiIcerikBilgisi { get; set; }
		public virtual KatilimciTipiTablosuModel KatilimciTipiBilgisi { get; set; }

		public virtual string RelationJsonModel()
		{
			return JsonConvert.SerializeObject(this);
		}
	}
}