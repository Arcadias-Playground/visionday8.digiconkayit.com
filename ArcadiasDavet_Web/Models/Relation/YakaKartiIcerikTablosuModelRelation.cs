using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using ModelBase;
using Model;

namespace ModelRelation
{
	public abstract class YakaKartiIcerikTablosuModelRelation : YakaKartiIcerikTablosuModelBase
	{
		public virtual YakaKartiCerceveTablosuModel YakaKartiCerceveBilgisi { get; set; }
		public virtual YakaKartiIcerikTipiTablosuModel YakaKartiIcerikTipiBilgisi { get; set; }

		public virtual string RelationJsonModel()
		{
			return JsonConvert.SerializeObject(this);
		}
	}
}