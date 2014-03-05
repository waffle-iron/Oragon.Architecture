// Copyright (c) 2013 Jonathan Magnan (http://zzzportal.com)
// All rights reserved.
// Licensed under MIT License (MIT)
// License can be found here: https://zextensionmethods.codeplex.com/license

using System;
using System.Collections.Generic;

namespace Oragon.Architecture.ExtMethods
{
	public static partial class OragonExtMethods
	{
		public static IEnumerable<DateRange> Reduce(this IEnumerable<DateRange> @this, TimeSpan maxDeltaIntersection)
		{
			return @this;
		}
	}
}