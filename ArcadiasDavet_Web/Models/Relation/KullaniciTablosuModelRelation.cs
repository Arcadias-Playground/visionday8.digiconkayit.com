using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using ModelBase;
using Model;

namespace ModelRelation
{
	public abstract class KullaniciTablosuModelRelation : KullaniciTablosuModelBase
	{
		public virtual IList<KullaniciGirisTablosuModel> KullaniciGirisBilgisi { get; set; }
		public virtual IList<WebMailTablosuModel> WebMailBilgisi { get; set; }
		public virtual IList<YakaKartiBasimTablosuModel> YakaKartiBasimBilgisi { get; set; }
		public virtual KullaniciTipiTablosuModel KullaniciTipiBilgisi { get; set; }

		public virtual string RelationJsonModel()
		{
			return JsonConvert.SerializeObject(this);
		}
	}
}