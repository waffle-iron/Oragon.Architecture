using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Oragon.Architecture.AOP.ExceptionHandling
{
	[Flags]
	public enum ExceptionHandlingStrategy
	{
		BreackOnException,
		ContinueRunning
	}
}
