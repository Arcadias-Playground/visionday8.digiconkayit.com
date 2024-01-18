using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using ModelBase;
using Model;

namespace ModelRelation
{
	public abstract class KullaniciTipiTablosuModelRelation : KullaniciTipiTablosuModelBase
	{
		public virtual IList<KullaniciTablosuModel> KullaniciBilgisi { get; set; }

		public virtual string RelationJsonModel()
		{
			return JsonConvert.SerializeObject(this);
		}
	}
}