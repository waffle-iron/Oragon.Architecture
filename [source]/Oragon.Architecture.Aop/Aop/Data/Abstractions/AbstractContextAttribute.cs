using System;

namespace Oragon.Architecture.Aop.Data.Abstractions
{
	[AttributeUsage(AttributeTargets.Method, Inherited = true)]
	public abstract class AbstractContextAttribute : Attribute
	{
		#region Public Properties

		public string ContextKey { get; set; }

		#endregion Public Properties
	}
}