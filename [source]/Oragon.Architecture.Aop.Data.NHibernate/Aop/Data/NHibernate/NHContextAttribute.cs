using Oragon.Architecture.Aop.Data.Abstractions;
using Oragon.Architecture.Data;
using System;

namespace Oragon.Architecture.Aop.Data.NHibernate
{
	[AttributeUsage(AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
	public sealed class NHContextAttribute : AbstractContextAttribute
	{
		public NHContextAttribute(string contextKey, bool isTransactional)
		{
			this.ContextKey = contextKey;
			this.IsTransactional = isTransactional;
		}

		public bool? IsTransactional { get; set; }


		internal ISessionFactoryBuilder SessionFactoryBuilder { get; set; }


	}
}
