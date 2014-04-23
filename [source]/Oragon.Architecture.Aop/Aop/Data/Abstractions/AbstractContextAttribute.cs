using System;

namespace Oragon.Architecture.Aop.Data.Abstractions
{
	[AttributeUsage(AttributeTargets.Method, Inherited = true)]
	public abstract class AbstractContextAttribute : Attribute
	{
		public string ContextKey { get; set; }
	}
}