﻿// Copyright (c) 2013 Jonathan Magnan (http://zzzportal.com)
// All rights reserved.
// Licensed under MIT License (MIT)
// License can be found here: https://zextensionmethods.codeplex.com/license

using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

namespace Oragon.Architecture.Extensions
{
	public static partial class OragonExtensions
	{
		/// <summary>
		/// Creates a <see cref="IReadOnlyCollection{T}"/> cloned collection around <paramref name="collection"/>.
		/// </summary>
		/// <typeparam name="T">The type parameter of the items in the collection.</typeparam>
		/// <param name="collection">A collection object.</param>
		public static IReadOnlyCollection<T> ToReadOnlyClonedCollection<T>(this IEnumerable<T> collection)
		{
			Contract.Requires(collection != null, "collection must not be null");
			Contract.Ensures(Contract.Result<IReadOnlyCollection<T>>() != null, "returned read-only collection is not null");
			Contract.Ensures(Contract.Result<IReadOnlyCollection<T>>().Count == collection.Count(), "returned read-only collection has the same number of elements as collection");
			return new CollectionReadOnlyWrapper<T>(collection.ToArray());
		}
	}
}