using Oragon.Architecture.Aop.Data.Abstractions;
using Oragon.Architecture.Caching.Redis;
using ServiceStack.Redis;
using System;
using System.Collections.Generic;

namespace Oragon.Architecture.Aop.Data.Redis
{
	[CLSCompliant(false)]
	public class RedisContext : AbstractContext<RedisContextAttribute>
	{
		#region Public Constructors

		public RedisContext(RedisContextAttribute contextAttribute, Stack<AbstractContext<RedisContextAttribute>> contextStack)
			: base(contextAttribute, contextStack)
		{
			BasicRedisClientManager manager = new BasicRedisClientManager(contextAttribute.RedisConnectionString.ConnectionString);
			var nativeClient = manager.GetClient();
			this.Client = new RedisClientForSpring(nativeClient, contextAttribute.RedisConnectionString.IsolationKey);
		}

		#endregion Public Constructors

		#region Public Properties

		public RedisClientForSpring Client { get; private set; }

		#endregion Public Properties

		#region Protected Methods

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

		#endregion Protected Methods
	}
}