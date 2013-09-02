using Oragon.Architecture.AOP.Data.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Oragon.Architecture.AOP.Data.MongoDB
{
	[AttributeUsage(AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
	public sealed class MongoDBContextAttribute : AbstractContextAttribute
	{
		public MongoDBContextAttribute(string contextKey)
		{
			this.ContextKey = contextKey;
		}

		internal MongoDBConnectionString MongoDBConnectionString { get; set; }
	}
}
