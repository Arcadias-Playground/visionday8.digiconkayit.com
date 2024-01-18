using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using ModelBase;
using Model;

namespace ModelRelation
{
	public abstract class AntetliKagitIcerikTipiTablosuModelRelation : AntetliKagitIcerikTipiTablosuModelBase
	{
		public virtual IList<AntetliKagitIcerikTablosuModel> AntetliKagitIcerikBilgisi { get; set; }
		public virtual GonderimTipiTablosuModel GonderimTipiBilgisi { get; set; }

		public virtual string RelationJsonModel()
		{
			return JsonConvert.SerializeObject(this);
		}
	}
}