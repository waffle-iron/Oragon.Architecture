using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oragon.Architecture.Extensions
{
	public static partial class OragonExtensions
	{
		public static Uri Query(this Uri @this, string newQuery)
		{
			var builder = new UriBuilder(@this);
			builder.Path = newQuery;
			return builder.Uri;
		}
	}
}
