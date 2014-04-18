using Oragon.Architecture.AOP.Data.Abstractions;
using Oragon.Architecture.Cache.Redis;
using ServiceStack.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Oragon.Architecture.AOP.Data.Redis
{
	public class RedisContext : AbstractContext<RedisContextAttribute>
	{

		public RedisContext(RedisContextAttribute contextAttribute, Stack<AbstractContext<RedisContextAttribute>> contextStack)
			: base(contextAttribute, contextStack)
		{

			BasicRedisClientManager manager = new BasicRedisClientManager(contextAttribute.RedisConnectionString.ConnectionString);
			var nativeClient = manager.GetClient();
			this.Client = new RedisClientForSpring(nativeClient, contextAttribute.RedisConnectionString.IsolationKey);
		}

		public RedisClientForSpring Client { get; private set; }


		protected override void DisposeContext()
		{
			base.DisposeContext();
		}

		protected override void DisposeFields()
		{
			this.Client.Dispose();
			this.Client = null;
			base.DisposeFields();
		}

	}
}
