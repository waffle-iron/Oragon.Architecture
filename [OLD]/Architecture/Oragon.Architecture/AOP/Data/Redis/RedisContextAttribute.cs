using Oragon.Architecture.AOP.Data.Abstractions;
using Oragon.Architecture.Data.ConnectionStrings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Oragon.Architecture.AOP.Data.Redis
{
	[AttributeUsage(AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
	public sealed class RedisContextAttribute : AbstractContextAttribute
	{
		public RedisContextAttribute(string contextKey)
		{
			this.ContextKey = contextKey;
		}

		internal RedisConnectionString RedisConnectionString { get; set; }
	}
}
