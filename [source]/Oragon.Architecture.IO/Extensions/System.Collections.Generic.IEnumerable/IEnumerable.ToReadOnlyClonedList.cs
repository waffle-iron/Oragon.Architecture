// Copyright (c) 2013 Jonathan Magnan (http://zzzportal.com) All rights reserved. Licensed under MIT License (MIT) License can be found here: https://zextensionmethods.codeplex.com/license

using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

namespace Oragon.Architecture.Extensions
{
	public static partial class OragonExtensions
	{
		#region Public Methods

		/// <summary>
		///     Creates a <see cref="IReadOnlyCollection{T}" /> cloned collection around <paramref name="list" />.
		/// </summary>
		/// <typeparam name="T">The type parameter of the items in the list.</typeparam>
		/// <param name="list">A list object.</param>
		public static IReadOnlyList<T> ToReadOnlyClonedList<T>(this IEnumerable<T> list)
		{
			Contract.Requires(list != null, "collection must not be null");
			Contract.Ensures(Contract.Result<IReadOnlyList<T>>() != null, "returned read-only list is not null");
			Contract.Ensures(Contract.Result<IReadOnlyList<T>>().Count == list.Count(), "returned read-only list has the same number of elements as list");
			return new ListReadOnlyWrapper<T>(list.ToArray());
		}

		#endregion Public Methods
	}
}