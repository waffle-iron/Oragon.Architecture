using System;

namespace Oragon.Architecture.Aop.ExceptionHandling
{
	[Flags]
	public enum ExceptionHandlingStrategy
	{
		BreackOnException,
		ContinueRunning
	}
}