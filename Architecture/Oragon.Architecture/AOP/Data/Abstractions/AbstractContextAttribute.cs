using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Oragon.Architecture.AOP.Data.Abstractions
{
	[AttributeUsage(AttributeTargets.Method, Inherited = true)]
	public abstract class AbstractContextAttribute : Attribute
	{
		public string ContextKey { get; set; }


	}
}
