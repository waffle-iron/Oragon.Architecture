using Oragon.Architecture.Aop.Data.Abstractions;
using Oragon.Architecture.Data.ConnectionStrings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oragon.Architecture.Aop.Data.Redis
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
