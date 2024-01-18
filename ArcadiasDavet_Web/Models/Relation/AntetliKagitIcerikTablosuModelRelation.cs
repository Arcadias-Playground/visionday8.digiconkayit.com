using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using ModelBase;
using Model;

namespace ModelRelation
{
	public abstract class AntetliKagitIcerikTablosuModelRelation : AntetliKagitIcerikTablosuModelBase
	{
		public virtual AntetliKagitIcerikTipiTablosuModel AntetliKagitIcerikTipiBilgisi { get; set; }
		public virtual MailIcerikTablosuModel MailIcerikBilgisi { get; set; }

		public virtual string RelationJsonModel()
		{
			return JsonConvert.SerializeObject(this);
		}
	}
}