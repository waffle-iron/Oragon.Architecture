using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Oragon.Architecture.AOP.ExceptionHandling
{

	[AttributeUsage(AttributeTargets.Method)]
	public class ExceptionHandlingAttribute : Attribute
	{
		public ExceptionHandlingStrategy Strategy { get; set; }

		public ExceptionHandlingAttribute(ExceptionHandlingStrategy strategy)
		{
			this.Strategy = strategy;
		}
	}

	

}
