﻿// Copyright (c) 2013 Jonathan Magnan (http://zzzportal.com) All rights reserved. Licensed under MIT License (MIT) License can be found here: https://zextensionmethods.codeplex.com/license

using System.Collections.Generic;
using FluentAssertions;

namespace Oragon.Architecture.Extensions
{
	public static partial class OragonExtensions
	{
		#region Public Methods

		/// <summary>
		///     Creates a <see cref="IReadOnlyCollection{T}" /> wrapper collection around <paramref name="collection" />.
		/// </summary>
		/// <typeparam name="T">The type parameter of the items in the collection.</typeparam>
		/// <param name="collection">A collection object.</param>
		public static IReadOnlyCollection<T> ToReadOnlyWrappedCollection<T>(this ICollection<T> collection)
		{
			collection.Should().NotBeNull("collection must not be null");
			return new CollectionReadOnlyWrapper<T>(collection);
		}

		#endregion Public Methods
	}
}