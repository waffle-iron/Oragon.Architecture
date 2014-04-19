using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AopAlliance.Intercept;

namespace Oragon.Architecture.Extensions
{
	public static partial class OragonExtensions
	{
		public static IEnumerable<T> GetAttibutes<T>(this IMethodInvocation invocation, Func<T, bool> predicate = null)
			where T : Attribute
		{
			if (predicate == null)
				predicate = (it => true);

			//Recupera os atributos do método
			IEnumerable<T> returnValue = invocation.Method.GetCustomAttributes(typeof(T), true)
			.Cast<T>()
			.Where(predicate);
			return returnValue;
		}

	}
}
