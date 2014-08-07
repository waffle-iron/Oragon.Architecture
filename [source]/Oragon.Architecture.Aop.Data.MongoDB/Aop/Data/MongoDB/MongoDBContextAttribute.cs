using Oragon.Architecture.Aop.Data.Abstractions;
using Oragon.Architecture.Data.ConnectionStrings;
using System;

namespace Oragon.Architecture.Aop.Data.MongoDB
{
	[AttributeUsage(AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
	public sealed class MongoDBContextAttribute : AbstractContextAttribute
	{
		#region Public Constructors

		public MongoDBContextAttribute(string contextKey)
		{
			this.ContextKey = contextKey;
		}

		#endregion Public Constructors

		#region Internal Properties

		internal MongoDBConnectionString MongoDBConnectionString { get; set; }

		#endregion Internal Properties
	}
}