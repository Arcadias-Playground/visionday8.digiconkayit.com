using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using ModelBase;
using Model;

namespace ModelRelation
{
	public abstract class GonderimTipiTablosuModelRelation : GonderimTipiTablosuModelBase
	{
		public virtual IList<AntetliKagitIcerikTipiTablosuModel> AntetliKagitIcerikTipiBilgisi { get; set; }
		public virtual IList<MailIcerikTablosuModel> MailIcerikBilgisi { get; set; }
		public virtual IList<SmsIcerikTablosuModel> SmsIcerikBilgisi { get; set; }

		public virtual string RelationJsonModel()
		{
			return JsonConvert.SerializeObject(this);
		}
	}
}