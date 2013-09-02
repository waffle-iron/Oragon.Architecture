using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace Oragon.Architecture.Extensions
{
	public static class IEnumerableExtensions
	{
		public static IEnumerable<T> ForEach<T>(this IEnumerable<T> collection, Action<T> predicate)
		{
			foreach (var item in collection)
				predicate(item);

			return collection;
		}

		public static IEnumerable ForEach(this IEnumerable collection, Action<object> predicate)
		{
			foreach (object item in collection)
				predicate(item);

			return collection;
		}
	}
}
