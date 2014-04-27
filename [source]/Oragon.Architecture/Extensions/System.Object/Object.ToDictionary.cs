// Copyright (c) 2013 Jonathan Magnan (http://zzzportal.com)
// All rights reserved.
// Licensed under MIT License (MIT)
// License can be found here: https://zextensionmethods.codeplex.com/license

using System;
using System.Collections;
using System.Collections.Generic;

namespace Oragon.Architecture.Extensions
{
	public static partial class OragonExtensions
	{
		/// <id>9BCDD187-0372-4B95-819A-8B911FFEF8BC</id>
		/// <summary>
		///     An object extension method that converts the @this to a boolean.
		/// </summary>
		/// <param name="this">The @this to act on.</param>
		/// <returns>@this as a bool.</returns>
		public static IDictionary<string, object> ToDictionary(this object @this)
		{
			Dictionary<string, object> returnValue = new Dictionary<string, object>();

			if (@this is IDictionary<string, object>)
				return (IDictionary<string, object>)@this;

			if (@this is IEnumerable)
			{
				string key = null;
				bool keyIsRead = false;
				foreach (var item in (IEnumerable)@this)
				{
					if (keyIsRead == false)
						key = (string)item;
					else
						returnValue.Add(key, item);

					keyIsRead = !keyIsRead;
				}
				return returnValue;
			}

			var attr = System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance;
			foreach (var property in @this.GetType().GetProperties(attr))
			{
				if (property.CanRead)
				{
					returnValue.Add(property.Name, property.GetValue(@this, null));
				}
			}
			return returnValue;
		}
	}
}