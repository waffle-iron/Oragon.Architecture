using Oragon.Architecture.Aop.Data.Abstractions;
using Oragon.Architecture.Data.ConnectionStrings;
using System;

namespace Oragon.Architecture.Aop.Data.Redis
{
	[AttributeUsage(AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
	public sealed class RedisContextAttribute : AbstractContextAttribute
	{
		#region Public Constructors

		public RedisContextAttribute(string contextKey)
		{
			this.ContextKey = contextKey;
		}

		#endregion Public Constructors

		#region Internal Properties

		internal RedisConnectionString RedisConnectionString { get; set; }

		#endregion Internal Properties
	}
}