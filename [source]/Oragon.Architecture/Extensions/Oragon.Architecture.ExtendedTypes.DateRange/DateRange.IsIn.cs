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
		public static bool IsIn(this DateTime date, DateRange interval)
		{
			bool returnValue = true;

			if (interval.StartDate.HasValue)
				returnValue &= (date >= interval.StartDate);

			if (interval.EndDate.HasValue)
				returnValue &= (date <= interval.EndDate);

			return returnValue;
		}
	}
}