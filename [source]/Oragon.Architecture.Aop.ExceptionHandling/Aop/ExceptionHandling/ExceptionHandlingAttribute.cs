using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Oragon.Architecture.Aop.ExceptionHandling
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
