using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using ModelBase;
using Model;

namespace ModelRelation
{
	public abstract class KullaniciGirisTablosuModelRelation : KullaniciGirisTablosuModelBase
	{
		public virtual KullaniciTablosuModel KullaniciBilgisi { get; set; }

		public virtual string RelationJsonModel()
		{
			return JsonConvert.SerializeObject(this);
		}
	}
}