using Oragon.Architecture.AOP.Data.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Oragon.Architecture.Tests.Cache.Redis.AOP
{
	public class ExampleModelCacheDataProcess : RedisDataProcess
	{
		internal void Store(string key, ExampleModel model)
		{
			this.ObjectContext.Client.Insert(key, model);
		}

		internal ExampleModel Retrieve(string key)
		{
			return this.ObjectContext.Client.Get(key) as ExampleModel;
		}
	}
}
