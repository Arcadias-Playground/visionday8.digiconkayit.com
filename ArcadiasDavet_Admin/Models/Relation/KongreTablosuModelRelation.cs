using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using ModelBase;
using Model;

namespace ModelRelation
{
	public abstract class KongreTablosuModelRelation : KongreTablosuModelBase
	{

		public virtual string JsonModel()
		{
			return JsonConvert.SerializeObject(this);
		}
	}
}