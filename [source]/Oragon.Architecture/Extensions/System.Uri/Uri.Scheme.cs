using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oragon.Architecture.Extensions
{
	public static partial class OragonExtensions
	{
		public static Uri Scheme(this Uri @this, string newScheme)
		{
			var builder = new UriBuilder(@this);
			builder.Scheme = newScheme;
			return builder.Uri;
		}
	}
}
