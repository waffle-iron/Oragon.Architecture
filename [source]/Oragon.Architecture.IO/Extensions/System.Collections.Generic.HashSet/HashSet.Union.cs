// Copyright (c) 2013 Jonathan Magnan (http://zzzportal.com)
// All rights reserved.
// Licensed under MIT License (MIT)
// License can be found here: https://zextensionmethods.codeplex.com/license

using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

namespace Oragon.Architecture.Extensions
{
	public static partial class OragonExtensions
	{

		///<summary>
		///Produces the set of elements, union of this <paramref name="hashset"/> and <paramref name="seq"/>. This method is an optimized version of <i>Enumerable.Union&lt;T&gt;</i>.
		///</summary>
		///<typeparam name="T">The code element type of the elements of the hashset and the sequence.</typeparam>
		///<param name="hashset">An hashset of elements whose distinct elements form the first set for the union.</param>
		///<param name="seq">A sequence of elements whose distinct elements form the second set for the union.</param>
		///<returns>A sequence that contains the elements that form the set union of the hashset and the sequence, excluding duplicates.</returns>
		///<remarks>This extension method has a <i>O(<paramref name="seq"/>.Count)</i> time complexity.</remarks>
		public static IEnumerable<T> Union<T>(this HashSet<T> hashset, IEnumerable<T> seq)
		{
			Contract.Requires(hashset != null, "hashset must not be null");
			Contract.Requires(seq != null, "seq must not be null");
			Contract.Ensures(Contract.Result<IEnumerable<T>>() != null, "returned sequence is not null");
			return UnionIterator(hashset, seq);
		}


		///<summary>
		///Produces the set of elements, union of this <paramref name="thisHashset"/> and <paramref name="otherHashset"/>. This method is an optimized version of <i>Enumerable.Union&lt;T&gt;</i>.
		///</summary>
		///<typeparam name="T">The code element type of the elements of the hashset and the sequence.</typeparam>
		///<param name="thisHashset">A sequence of elements whose distinct elements form the first set for the union.</param>
		///<param name="otherHashset">An hashset of elements whose distinct elements form the second set for the union.</param>
		///<returns>A sequence that contains the elements that form the set union of both hashsets, excluding duplicates.</returns>
		///<remarks>This extension method has a <i>O(<paramref name="otherHashset"/>.Count)</i> time complexity.</remarks>
		public static IEnumerable<T> Union<T>(this HashSet<T> thisHashset, HashSet<T> otherHashset)
		{
			Contract.Requires(thisHashset != null, "hashset must not be null");
			Contract.Requires(otherHashset != null, "otherHashset must not be null");
			Contract.Ensures(Contract.Result<IEnumerable<T>>() != null, "returned sequence is not null");
			return UnionIterator(thisHashset, otherHashset);
		}

	}
}