using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using ModelBase;
using Model;

namespace ModelRelation
{
	public abstract class YakaKartiIcerikTipiTablosuModelRelation : YakaKartiIcerikTipiTablosuModelBase
	{
		public virtual IList<YakaKartiIcerikTablosuModel> YakaKartiIcerikBilgisi { get; set; }

		public virtual string RelationJsonModel()
		{
			return JsonConvert.SerializeObject(this);
		}
	}
}