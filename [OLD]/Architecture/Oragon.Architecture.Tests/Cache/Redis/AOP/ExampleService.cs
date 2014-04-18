using Oragon.Architecture.AOP.Data.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Oragon.Architecture.Tests.Cache.Redis.AOP
{
	public class ExampleService : IExampleService
	{
		ExampleModelCacheDataProcess ExampleModelCacheDP { get; set; }

		[RedisContext("Redis1")]
		public void Store(string key, ExampleModel model)
		{
			this.ExampleModelCacheDP.Store(key, model);
		}

		[RedisContext("Redis1")]
		public ExampleModel Retrieve(string key)
		{
			return this.ExampleModelCacheDP.Retrieve(key);
		}
	}
}
