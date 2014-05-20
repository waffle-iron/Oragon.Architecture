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
		///Produces the set of elements, intersection of this <paramref name="hashset"/> and <paramref name="seq"/>. This method is an optimized version of <i>Enumerable.Intersect&lt;T&gt;</i>.
		///</summary>
		///<typeparam name="T">The code element type of the elements of the hashset and the sequence.</typeparam>
		///<param name="hashset">An hashset of elements whose distinct elements that also appear in <paramref name="seq"/> will be returned.</param>
		///<param name="seq">A sequence of elements whose distinct elements that also appear in <paramref name="hashset"/> will be returned.</param>
		///<returns>A sequence that contains the elements that form the set intersection of the hashset and the sequence.</returns>
		///<remarks>This extension method has a <i>O(<paramref name="seq"/>.Count)</i> time complexity.</remarks>
		public static IEnumerable<T> Intersect<T>(this HashSet<T> hashset, IEnumerable<T> seq)
		{
			Contract.Requires(hashset != null, "hashset must not be null");
			Contract.Requires(seq != null, "seq must not be null");
			Contract.Ensures(Contract.Result<IEnumerable<T>>() != null, "returned sequence is not null");
			return IntersectIterator(hashset, seq);
		}


		///<summary>
		///Produces the set of elements, intersection of this <paramref name="hashset"/> and <paramref name="otherHashset"/>. This method is an optimized version of <i>Enumerable.Intersect&lt;T&gt;</i>.
		///</summary>
		///<typeparam name="T">The code element type of the elements of the hashset and the sequence.</typeparam>
		///<param name="hashset">An hashset of elements whose distinct elements that also appear in <paramref name="otherHashset"/> will be returned.</param>
		///<param name="otherHashset">An hashset of elements whose distinct elements that also appear in <paramref name="hashset"/> will be returned.</param>
		///<returns>A sequence that contains the elements that form the set intersection of both hashsets.</returns>
		///<remarks>This extension method has a <i>O(<paramref name="otherHashset"/>.Count)</i> time complexity.</remarks>
		public static IEnumerable<T> Intersect<T>(this HashSet<T> hashset, HashSet<T> otherHashset)
		{
			Contract.Requires(hashset != null, "hashset must not be null");
			Contract.Requires(otherHashset != null, "otherHashset must not be null");
			Contract.Ensures(Contract.Result<IEnumerable<T>>() != null, "returned sequence is not null");
			return IntersectIterator(hashset, otherHashset);
		}



		private static IEnumerable<T> IntersectIterator<T>(HashSet<T> hashset, IEnumerable<T> seq)
		{
			foreach (var t in seq)
			{
				if (!hashset.Contains(t)) { continue; }
				yield return t;
			}
		}

	}
}