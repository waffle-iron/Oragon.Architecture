// Copyright (c) 2013 Jonathan Magnan (http://zzzportal.com)
// All rights reserved.
// Licensed under MIT License (MIT)
// License can be found here: https://zextensionmethods.codeplex.com/license

using Oragon.Architecture.ExtendedTypes;
using System;
using System.Collections.Generic;

namespace Oragon.Architecture.Extensions
{
	public static partial class OragonExtensions
	{
		public static bool IsIn<T>(this T value, Range<T> interval)
			 where T : struct, IComparable<T>, IEquatable<T>
		{
			bool returnValue = true;

			if (interval.Start.HasValue)
				returnValue &= (value.CompareTo(interval.Start.Value) >= 0);

			if (interval.End.HasValue)
				returnValue &= (value.CompareTo(interval.End.Value) <= 0);

			return returnValue;
		}
	}
}